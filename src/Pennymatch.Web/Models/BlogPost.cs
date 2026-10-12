namespace Pennymatch.Web.Models;

public sealed class BlogPost
{
    public required string Slug { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Author { get; init; }
    public DateOnly PublishedAt { get; init; }
    public required string Category { get; init; }
    public IReadOnlyList<string> Tags { get; init; } = [];
    public int ReadingMinutes { get; init; }
    public required string Markdown { get; init; }
    public string Html { get; init; } = string.Empty;
}
