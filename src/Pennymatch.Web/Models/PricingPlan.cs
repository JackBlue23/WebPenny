namespace Pennymatch.Web.Models;

public sealed class PricingPlan
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string PriceLabel { get; init; }
    public required string Cadence { get; init; }
    public required string Summary { get; init; }
    public IReadOnlyList<string> Features { get; init; } = [];
    public bool Highlighted { get; init; }
    public required string CtaLabel { get; init; }
}
