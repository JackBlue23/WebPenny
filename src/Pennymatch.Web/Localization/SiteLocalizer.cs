using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace Pennymatch.Web.Localization;

/// <summary>
/// Resolves UI copy from injectable static dictionaries keyed by <see cref="UiCulture"/>.
/// Culture is pinned once per DI scope from the RequestLocalization feature / culture cookie
/// so Interactive Server circuits keep ESP/ENG after prerender (CurrentUICulture alone is not enough).
/// </summary>
public sealed class SiteLocalizer : ISiteLocalizer
{
    private readonly UiCulture _culture;

    public SiteLocalizer(IHttpContextAccessor httpContextAccessor)
    {
        _culture = Resolve(httpContextAccessor.HttpContext);
        var info = _culture.ToCultureInfo();
        CultureInfo.CurrentCulture = info;
        CultureInfo.CurrentUICulture = info;
    }

    public UiCulture Culture => _culture;

    public string this[string key] => SiteCatalog.Get(_culture, key);

    public string T(string key) => this[key];

    private static UiCulture Resolve(HttpContext? http)
    {
        if (http is not null)
        {
            var feature = http.Features.Get<IRequestCultureFeature>();
            if (feature?.RequestCulture.UICulture is { } uiFromFeature)
            {
                return UiCulture.From(uiFromFeature);
            }

            if (http.Request.Cookies.TryGetValue(CookieRequestCultureProvider.DefaultCookieName, out var raw)
                && !string.IsNullOrEmpty(raw)
                && CookieRequestCultureProvider.ParseCookieValue(raw) is { UICultures.Count: > 0 } parsed)
            {
                return UiCulture.ParseOrDefault(parsed.UICultures[0].Value);
            }
        }

        return UiCulture.From(CultureInfo.CurrentUICulture);
    }
}
