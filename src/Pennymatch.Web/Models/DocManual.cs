namespace Pennymatch.Web.Models;

/// <summary>Integration / ops manual card for the docs hub.</summary>
public sealed class DocManual
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string Summary { get; init; }
    public required string Audience { get; init; }
    public required string Category { get; init; }
    public required string Href { get; init; }
    public IReadOnlyList<string> Tags { get; init; } = [];
}
