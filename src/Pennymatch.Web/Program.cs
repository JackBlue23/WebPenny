using System.Globalization;
using System.IO.Compression;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Localization;
using Microsoft.Net.Http.Headers;
using Pennymatch.Web.Components;
using Pennymatch.Web.Localization;
using Pennymatch.Web.Models;
using Pennymatch.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppOptions>(builder.Configuration.GetSection(AppOptions.SectionName));
builder.Services.AddSingleton<AuthLinkService>();
builder.Services.AddSingleton<BlogContentService>();
builder.Services.AddSingleton<DocsCatalogService>();
builder.Services.AddSingleton<PublicUrlService>();
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ISiteLocalizer, SiteLocalizer>();
builder.Services.AddLocalization();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
    [
        "text/html",
        "text/css",
        "application/javascript",
        "application/json"
    ]);
});
builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Optimal;
});

var supportedCultures = UiCulture.SupportedCodes
    .Select(c => new CultureInfo(c))
    .ToList();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(UiCulture.Es.Code);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.ApplyCurrentCultureToResponseHeaders = true;

    // Cookie first so /, /docs, /blog keep language without flicker after a switch.
    options.RequestCultureProviders =
    [
        new CookieRequestCultureProvider(),
        new AcceptLanguageHeaderRequestCultureProvider()
    ];
});

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

// Order: ResponseCompression → StaticFiles → Routing → Localization → Antiforgery → Endpoints
app.UseResponseCompression();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = static ctx =>
    {
        var name = ctx.File.Name;
        // site.js drives culture switching — never pin a year-long cache on the unfingerprinted path.
        // Fingerprinted Assets["js/site.js"] URLs remain immutable via MapStaticAssets.
        if (name.Equals("site.js", StringComparison.OrdinalIgnoreCase))
        {
            ctx.Context.Response.Headers.CacheControl = "no-cache, must-revalidate";
            ctx.Context.Response.Headers[HeaderNames.Vary] = HeaderNames.AcceptEncoding;
            return;
        }

        var ext = Path.GetExtension(name);
        if (IsImmutableStaticExtension(ext))
        {
            ctx.Context.Response.Headers.CacheControl = "public,max-age=31536000";
            ctx.Context.Response.Headers[HeaderNames.Vary] = HeaderNames.AcceptEncoding;
        }
    }
});

app.UseRouting();
app.UseRequestLocalization();
app.UseAntiforgery();

// Persist UI culture via the standard ASP.NET Core culture cookie, then redirect.
// Explicitly accepts es|en (UiCulture.ParseOrDefault). Clears prior culture cookies first
// so a stale Path/Secure variant cannot keep the UI locked on ENG.
app.MapGet("/culture/set", (string? culture, string? returnUrl, HttpContext http) =>
{
    var ui = UiCulture.ParseOrDefault(culture);
    var cookieName = CookieRequestCultureProvider.DefaultCookieName;
    var secure = http.Request.IsHttps;

    // Drop conflicting cookies (any prior Path/Secure combination browsers may still send).
    http.Response.Cookies.Delete(cookieName);
    http.Response.Cookies.Delete(cookieName, new CookieOptions { Path = "/", Secure = secure });
    http.Response.Cookies.Delete(cookieName, new CookieOptions { Path = "/", Secure = !secure });

    var cookieValue = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(ui.Code, ui.Code));
    http.Response.Cookies.Append(
        cookieName,
        cookieValue,
        new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddYears(1),
            IsEssential = true,
            HttpOnly = false,
            Secure = secure,
            SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax,
            Path = "/"
        });

    // Never let intermediaries / enhanced-nav caches serve a stale-language document.
    http.Response.Headers.CacheControl = "no-store, no-cache, must-revalidate";
    http.Response.Headers.Pragma = "no-cache";

    var target = SanitizeReturnUrl(returnUrl);
    return Results.LocalRedirect(target);
});

app.MapGet("/robots.txt", (PublicUrlService urls) =>
{
    var body =
        $"User-agent: *{Environment.NewLine}" +
        $"Allow: /{Environment.NewLine}" +
        $"Sitemap: {urls.Absolute("/sitemap.xml")}{Environment.NewLine}";
    return Results.Text(body, "text/plain; charset=utf-8");
});

app.MapGet("/sitemap.xml", async (DocsCatalogService docs, BlogContentService blog, PublicUrlService urls) =>
{
    var paths = new List<string>
    {
        "/",
        "/docs",
        "/blog",
        "/legal-notice",
        "/privacy",
        "/terms",
        "/cookies",
        "/subprocessors"
    };

    foreach (var manual in await docs.GetAllAsync().ConfigureAwait(false))
    {
        paths.Add(manual.Href);
    }

    foreach (var post in blog.GetAll())
    {
        paths.Add($"/blog/{post.Slug}");
    }

    // Deduplicate while preserving order (aliases like /manuales stay off the sitemap).
    var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    var sb = new System.Text.StringBuilder();
    sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
    sb.Append("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");
    foreach (var path in paths)
    {
        if (!seen.Add(path))
        {
            continue;
        }

        sb.Append("<url><loc>")
            .Append(System.Security.SecurityElement.Escape(urls.Absolute(path)))
            .Append("</loc></url>");
    }

    sb.Append("</urlset>");
    return Results.Text(sb.ToString(), "application/xml; charset=utf-8");
});

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Warm docs catalog + formatted bodies for both UI cultures (O(1) after first paint elsewhere).
DocsCatalogService.WarmCache(app.Services);

app.Run();

static bool IsImmutableStaticExtension(string extension) =>
    extension.Equals(".css", StringComparison.OrdinalIgnoreCase)
    || extension.Equals(".js", StringComparison.OrdinalIgnoreCase)
    || extension.Equals(".mjs", StringComparison.OrdinalIgnoreCase)
    || extension.Equals(".png", StringComparison.OrdinalIgnoreCase)
    || extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase)
    || extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase)
    || extension.Equals(".gif", StringComparison.OrdinalIgnoreCase)
    || extension.Equals(".webp", StringComparison.OrdinalIgnoreCase)
    || extension.Equals(".svg", StringComparison.OrdinalIgnoreCase)
    || extension.Equals(".ico", StringComparison.OrdinalIgnoreCase)
    || extension.Equals(".woff", StringComparison.OrdinalIgnoreCase)
    || extension.Equals(".woff2", StringComparison.OrdinalIgnoreCase);

static string SanitizeReturnUrl(string? returnUrl)
{
    if (string.IsNullOrWhiteSpace(returnUrl))
    {
        return "/";
    }

    // Relative local paths only (path + optional query + fragment). Reject open redirects and culture loops.
    if (!returnUrl.StartsWith('/') || returnUrl.StartsWith("//", StringComparison.Ordinal))
    {
        return "/";
    }

    var withoutFragment = returnUrl.Split('#', 2)[0];
    var path = withoutFragment.Split('?', 2)[0];
    if (path.Equals("/culture/set", StringComparison.OrdinalIgnoreCase))
    {
        return "/";
    }

    return returnUrl;
}
