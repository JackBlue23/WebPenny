using System.Globalization;
using Markdig;
using Pennymatch.Web.Models;

namespace Pennymatch.Web.Services;

public sealed class BlogContentService
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<BlogContentService> _logger;
    private readonly MarkdownPipeline _pipeline;
    private readonly object _gate = new();
    private IReadOnlyList<BlogPost>? _cache;

    public BlogContentService(IWebHostEnvironment env, ILogger<BlogContentService> logger)
    {
        _env = env;
        _logger = logger;
        _pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UseAutoIdentifiers()
            .Build();
    }

    public IReadOnlyList<BlogPost> GetAll()
    {
        EnsureLoaded();
        return _cache!;
    }

    public BlogPost? GetBySlug(string slug)
    {
        EnsureLoaded();
        return _cache!.FirstOrDefault(p => p.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
    }

    public IReadOnlyList<BlogPost> GetRecommended(string? excludeSlug, int take = 3)
    {
        return GetAll()
            .Where(p => excludeSlug is null || !p.Slug.Equals(excludeSlug, StringComparison.OrdinalIgnoreCase))
            .Take(take)
            .ToList();
    }

    private void EnsureLoaded()
    {
        if (_cache is not null)
        {
            return;
        }

        lock (_gate)
        {
            if (_cache is not null)
            {
                return;
            }

            var dir = Path.Combine(_env.ContentRootPath, "Content", "blog");
            if (!Directory.Exists(dir))
            {
                _logger.LogWarning("Blog content directory missing: {Dir}", dir);
                _cache = [];
                return;
            }

            var posts = new List<BlogPost>();
            foreach (var file in Directory.EnumerateFiles(dir, "*.md").OrderBy(f => f))
            {
                try
                {
                    posts.Add(ParseFile(file));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to parse blog post {File}", file);
                }
            }

            _cache = posts
                .OrderByDescending(p => p.PublishedAt)
                .ThenBy(p => p.Title, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }

    private BlogPost ParseFile(string path)
    {
        var raw = File.ReadAllText(path);
        var (frontMatter, body) = SplitFrontMatter(raw);
        var meta = ParseFrontMatter(frontMatter);
        var slug = meta.Slug ?? Path.GetFileNameWithoutExtension(path);
        var html = Markdown.ToHtml(body.Trim(), _pipeline);

        return new BlogPost
        {
            Slug = slug,
            Title = meta.Title ?? slug,
            Description = meta.Description ?? string.Empty,
            Author = meta.Author ?? "Equipo Pennymatch",
            PublishedAt = meta.PublishedAt ?? DateOnly.FromDateTime(File.GetLastWriteTimeUtc(path)),
            Category = meta.Category ?? "Producto",
            Tags = meta.Tags ?? [],
            ReadingMinutes = EstimateReadingMinutes(body),
            Markdown = body.Trim(),
            Html = html
        };
    }

    private static (string FrontMatter, string Body) SplitFrontMatter(string raw)
    {
        if (!raw.StartsWith("---", StringComparison.Ordinal))
        {
            return (string.Empty, raw);
        }

        var end = raw.IndexOf("---", 3, StringComparison.Ordinal);
        if (end < 0)
        {
            return (string.Empty, raw);
        }

        var fm = raw[3..end].Trim();
        var body = raw[(end + 3)..];
        return (fm, body);
    }

    private static FrontMatterDto ParseFrontMatter(string yaml)
    {
        if (string.IsNullOrWhiteSpace(yaml))
        {
            return new FrontMatterDto();
        }

        // Lightweight YAML parse without an extra dependency for simple key: value lists.
        var dto = new FrontMatterDto();
        string? listKey = null;
        var listBuffer = new List<string>();

        foreach (var line in yaml.Split('\n'))
        {
            var trimmed = line.TrimEnd();
            if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith('#'))
            {
                continue;
            }

            if (trimmed.StartsWith("- ") && listKey is not null)
            {
                listBuffer.Add(trimmed[2..].Trim().Trim('"', '\''));
                continue;
            }

            FlushList();

            var idx = trimmed.IndexOf(':');
            if (idx <= 0)
            {
                continue;
            }

            var key = trimmed[..idx].Trim();
            var value = trimmed[(idx + 1)..].Trim().Trim('"', '\'');

            switch (key.ToLowerInvariant())
            {
                case "title":
                    dto.Title = value;
                    break;
                case "description":
                    dto.Description = value;
                    break;
                case "author":
                    dto.Author = value;
                    break;
                case "slug":
                    dto.Slug = value;
                    break;
                case "category":
                    dto.Category = value;
                    break;
                case "published":
                case "publishedat":
                case "date":
                    if (DateOnly.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var d))
                    {
                        dto.PublishedAt = d;
                    }
                    break;
                case "tags":
                    if (string.IsNullOrEmpty(value))
                    {
                        listKey = "tags";
                        listBuffer.Clear();
                    }
                    else if (value.StartsWith('['))
                    {
                        dto.Tags = value.Trim('[', ']')
                            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                            .Select(t => t.Trim('"', '\''))
                            .ToList();
                    }
                    break;
            }
        }

        FlushList();
        return dto;

        void FlushList()
        {
            if (listKey == "tags" && listBuffer.Count > 0)
            {
                dto.Tags = listBuffer.ToList();
            }

            listKey = null;
            listBuffer.Clear();
        }
    }

    private static int EstimateReadingMinutes(string markdown)
    {
        var words = markdown.Split([' ', '\n', '\r', '\t'], StringSplitOptions.RemoveEmptyEntries).Length;
        return Math.Max(1, (int)Math.Ceiling(words / 200.0));
    }

    private sealed class FrontMatterDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }
        public string? Slug { get; set; }
        public string? Category { get; set; }
        public DateOnly? PublishedAt { get; set; }
        public List<string>? Tags { get; set; }
    }
}
