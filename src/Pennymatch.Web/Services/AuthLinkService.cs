using Microsoft.Extensions.Options;
using Pennymatch.Web.Models;

namespace Pennymatch.Web.Services;

public sealed class AuthLinkService
{
    private readonly AppOptions _options;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthLinkService(
        IOptions<AppOptions> options,
        IHttpContextAccessor httpContextAccessor)
    {
        _options = options.Value;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>Pennymatch.Api — not used for Login/Register CTAs.</summary>
    public string ApiBaseUrl => NormalizeBase(_options.ApiBaseUrl);

    /// <summary>Pennymatch.Client UI origin — Login/Register target.</summary>
    public string ProductBaseUrl => _options.ProductBaseUrl.TrimEnd('/');

    public string? SessionCookieName => string.IsNullOrWhiteSpace(_options.SessionCookieName)
        ? null
        : _options.SessionCookieName;

    public string LoginUrl => BuildLoginUrl(includeReturnUrl: true);

    /// <summary>
    /// Primary product CTA. Default: http://localhost:5288/login?returnUrl=dashboard
    /// (avoids blank Client /auth/register until that page ships).
    /// </summary>
    public string RegisterUrl => Combine(ProductBaseUrl, string.IsNullOrWhiteSpace(_options.RegisterPath)
        ? "/login?returnUrl=dashboard"
        : _options.RegisterPath);

    public string DemoUrl => ResolveDemo(_options.DemoPath);

    /// <summary>True when DemoUrl leaves the marketing origin (http/mailto).</summary>
    public bool DemoOpensExternally =>
        DemoUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase)
        || DemoUrl.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase);

    public string BuildLoginUrl(bool includeReturnUrl = true, string? returnUrl = null)
    {
        var path = _options.LoginPath;
        if (string.IsNullOrWhiteSpace(path))
        {
            path = "/login?returnUrl=dashboard";
        }

        // Support "/login?returnUrl=" stub and "/login?returnUrl=dashboard" concrete values.
        if (path.Contains("returnUrl=", StringComparison.OrdinalIgnoreCase))
        {
            var baseLogin = Combine(ProductBaseUrl, path);
            if (!includeReturnUrl || HasConcreteReturnUrl(path))
            {
                return baseLogin;
            }

            var resolved = returnUrl ?? ResolveMarketingReturnUrl();
            return string.IsNullOrEmpty(resolved)
                ? baseLogin
                : baseLogin + Uri.EscapeDataString(resolved);
        }

        var url = Combine(ProductBaseUrl, path);
        if (!includeReturnUrl)
        {
            return url;
        }

        var ret = returnUrl ?? "dashboard";
        if (string.IsNullOrEmpty(ret))
        {
            return url;
        }

        var separator = url.Contains('?', StringComparison.Ordinal) ? "&" : "?";
        return $"{url}{separator}returnUrl={Uri.EscapeDataString(ret)}";
    }

    /// <summary>True when path already supplies a non-empty returnUrl value (not a trailing stub).</summary>
    private static bool HasConcreteReturnUrl(string path)
    {
        var marker = "returnUrl=";
        var idx = path.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
        if (idx < 0)
        {
            return false;
        }

        var value = path[(idx + marker.Length)..];
        var amp = value.IndexOf('&');
        if (amp >= 0)
        {
            value = value[..amp];
        }

        return !string.IsNullOrWhiteSpace(value);
    }

    private string? ResolveMarketingReturnUrl()
    {
        var ctx = _httpContextAccessor.HttpContext;
        if (ctx is null)
        {
            return null;
        }

        var req = ctx.Request;
        return $"{req.Scheme}://{req.Host}/";
    }

    private string ResolveDemo(string path)
    {
        if (string.IsNullOrWhiteSpace(path) || path == "#")
        {
            return "/docs";
        }

        // Site-relative manuals / anchors stay on the marketing origin.
        if (path.StartsWith('#')
            || path.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase)
            || path.StartsWith("http", StringComparison.OrdinalIgnoreCase)
            || path.StartsWith('/'))
        {
            return path;
        }

        return Combine(ProductBaseUrl, path);
    }

    private static string NormalizeBase(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return "http://localhost:5199/";
        }

        return url.EndsWith('/') ? url : url + "/";
    }

    private static string Combine(string baseUrl, string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return baseUrl;
        }

        if (path.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        {
            return path;
        }

        // Preserve query string on path (e.g. /login?returnUrl=)
        return $"{baseUrl}/{path.TrimStart('/')}";
    }
}
