namespace Pennymatch.Web.Models;

/// <summary>
/// Product auth, API, and demo deep-links for marketing CTAs.
/// Bind from configuration section <c>App</c>.
/// </summary>
public sealed class AppOptions
{
    public const string SectionName = "App";

    /// <summary>
    /// Pennymatch.Api base URL (from Client appsettings ApiBaseUrl).
    /// Not used for Login/Register CTAs.
    /// </summary>
    public string ApiBaseUrl { get; set; } = "http://localhost:5199/";

    /// <summary>
    /// Origin of Pennymatch.Client (Blazor UI). Confirmed local: http://localhost:5288
    /// </summary>
    public string ProductBaseUrl { get; set; } = "http://localhost:5288";

    /// <summary>Client login path. May include query stub like /login?returnUrl=</summary>
    public string LoginPath { get; set; } = "/login?returnUrl=";

    public string RegisterPath { get; set; } = "/register";

    /// <summary>
    /// Demo CTA target. Absolute URL, path under ProductBaseUrl, page anchor (#...), or mailto:.
    /// </summary>
    public string DemoPath { get; set; } = "#demo";

    /// <summary>
    /// Optional cookie name shared with the product app when both are same-site.
    /// Cross-origin cookies cannot be read; leave empty to always show login/register.
    /// </summary>
    public string? SessionCookieName { get; set; }
}
