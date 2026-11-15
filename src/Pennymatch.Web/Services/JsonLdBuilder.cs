using System.Text.Json;
using Pennymatch.Web.Models;

namespace Pennymatch.Web.Services;

/// <summary>Minimal JSON-LD builders for SoftwareApplication, TechArticle, and FAQPage.</summary>
public static class JsonLdBuilder
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = null,
        WriteIndented = false,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public static string SoftwareApplication(string canonicalHome)
    {
        var payload = new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org",
            ["@type"] = "SoftwareApplication",
            ["name"] = "Pennymatch",
            ["applicationCategory"] = "BusinessApplication",
            ["operatingSystem"] = "Web",
            ["url"] = canonicalHome,
            ["description"] =
                "Technical middleware: Stripe, Shopify and Wise events to immutable Spanish PGC journals and CSV for Holded, Xero and QuickBooks.",
            ["offers"] = new Dictionary<string, object?>
            {
                ["@type"] = "Offer",
                ["price"] = "0",
                ["priceCurrency"] = "EUR",
                ["description"] = "Free trial"
            },
            ["publisher"] = new Dictionary<string, object?>
            {
                ["@type"] = "Organization",
                ["name"] = "PENNYMATCH, S.L.U.",
                ["url"] = canonicalHome
            }
        };
        return JsonSerializer.Serialize(payload, Options);
    }

    public static string TechArticle(
        string headline,
        string description,
        string url,
        string? datePublished = null,
        string? author = null)
    {
        var payload = new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org",
            ["@type"] = "TechArticle",
            ["headline"] = headline,
            ["description"] = description,
            ["url"] = url,
            ["mainEntityOfPage"] = url,
            ["author"] = new Dictionary<string, object?>
            {
                ["@type"] = "Organization",
                ["name"] = string.IsNullOrWhiteSpace(author) ? "Pennymatch" : author!
            },
            ["publisher"] = new Dictionary<string, object?>
            {
                ["@type"] = "Organization",
                ["name"] = "PENNYMATCH, S.L.U."
            }
        };
        if (!string.IsNullOrWhiteSpace(datePublished))
        {
            payload["datePublished"] = datePublished;
        }

        return JsonSerializer.Serialize(payload, Options);
    }

    public static string Article(
        BlogPost post,
        string url)
    {
        var payload = new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org",
            ["@type"] = "Article",
            ["headline"] = post.Title,
            ["description"] = post.Description,
            ["url"] = url,
            ["datePublished"] = post.PublishedAt.ToString("yyyy-MM-dd"),
            ["author"] = new Dictionary<string, object?>
            {
                ["@type"] = "Person",
                ["name"] = post.Author
            },
            ["publisher"] = new Dictionary<string, object?>
            {
                ["@type"] = "Organization",
                ["name"] = "PENNYMATCH, S.L.U."
            }
        };
        return JsonSerializer.Serialize(payload, Options);
    }
}
