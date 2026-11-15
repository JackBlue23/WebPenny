using Microsoft.Extensions.Options;
using Pennymatch.Web.Models;

namespace Pennymatch.Web.Services;

/// <summary>
/// Absolute public URLs for canonical, hreflang, Open Graph and sitemap.
/// Never emits localhost — uses <see cref="AppOptions.PublicBaseUrl"/>.
/// </summary>
public sealed class PublicUrlService
{
    private readonly string _origin;

    public PublicUrlService(IOptions<AppOptions> options)
    {
        _origin = NormalizeOrigin(options.Value.PublicBaseUrl);
    }

    public string Origin => _origin;

    public string Absolute(string pathAndQuery)
    {
        if (string.IsNullOrWhiteSpace(pathAndQuery))
        {
            return _origin + "/";
        }

        if (pathAndQuery.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
            || pathAndQuery.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            var uri = new Uri(pathAndQuery, UriKind.Absolute);
            if (IsLoopback(uri.Host))
            {
                return Absolute(uri.PathAndQuery);
            }

            return pathAndQuery;
        }

        var path = pathAndQuery.StartsWith('/') ? pathAndQuery : "/" + pathAndQuery;
        return _origin + path;
    }

    public string Canonical(string path) => Absolute(path);

    private static string NormalizeOrigin(string? configured)
    {
        var raw = string.IsNullOrWhiteSpace(configured)
            ? "https://pennymatch.com"
            : configured.Trim().TrimEnd('/');

        if (!Uri.TryCreate(raw, UriKind.Absolute, out var uri)
            || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            || IsLoopback(uri.Host))
        {
            return "https://pennymatch.com";
        }

        return uri.GetLeftPart(UriPartial.Authority).TrimEnd('/');
    }

    private static bool IsLoopback(string host) =>
        string.Equals(host, "localhost", StringComparison.OrdinalIgnoreCase)
        || host is "127.0.0.1" or "::1"
        || host.EndsWith(".local", StringComparison.OrdinalIgnoreCase);
}
