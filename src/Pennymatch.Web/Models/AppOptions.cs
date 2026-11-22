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

    /// <summary>
    /// Public production origin for canonical, hreflang, Open Graph and sitemap.
    /// Must never be localhost — SEO tags fall back to https://pennymatch.com.
    /// </summary>
    public string PublicBaseUrl { get; set; } = "https://pennymatch.com";

    /// <summary>Client login path. Prefer a concrete returnUrl (e.g. /login?returnUrl=dashboard).</summary>
    public string LoginPath { get; set; } = "/login?returnUrl=dashboard";

    /// <summary>
    /// Primary product CTA path. Points at Client login with dashboard return
    /// (blank /auth/register is avoided until Client register ships).
    /// </summary>
    public string RegisterPath { get; set; } = "/login?returnUrl=dashboard";

    /// <summary>
    /// Pricing-card checkout path under <see cref="ProductBaseUrl"/>.
    /// Query params <c>plan</c>, <c>interval</c>, and <c>currency</c> are appended by
    /// <c>AuthLinkService.BuildPricingRegisterUrl</c>. Override in QA/Prod if Client
    /// still lacks <c>/register</c> (e.g. temporary login fallback).
    /// </summary>
    public string PricingRegisterPath { get; set; } = "/register";

    /// <summary>
    /// Secondary CTA target. Absolute URL, path under ProductBaseUrl, site-relative path (/docs),
    /// page anchor (#...), or mailto:. Default: integration manuals hub.
    /// </summary>
    public string DemoPath { get; set; } = "/docs";

    /// <summary>
    /// Optional cookie name shared with the product app when both are same-site.
    /// Cross-origin cookies cannot be read; leave empty to always show login/register.
    /// </summary>
    public string? SessionCookieName { get; set; }
}
