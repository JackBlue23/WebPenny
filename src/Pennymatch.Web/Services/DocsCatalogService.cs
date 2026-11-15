using System.Globalization;
using Microsoft.Extensions.Caching.Memory;
using Pennymatch.Web.Localization;
using Pennymatch.Web.Models;

namespace Pennymatch.Web.Services;

/// <summary>
/// Static catalog of integration manuals and ops guides (no reflection).
/// Formatted manuals + HTML bodies are cached in <see cref="IMemoryCache"/> after first resolve;
/// subsequent reads are O(1). Optional disk markdown under Content/docs is loaded via
/// <see cref="File.ReadAllTextAsync"/> only on cache miss.
/// </summary>
public sealed class DocsCatalogService
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(12);

    private readonly IMemoryCache _cache;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<DocsCatalogService> _logger;

    private static readonly IReadOnlyList<DocManualDef> Manuals =
    [
        new(
            Id: "quickstart",
            Href: "/docs/quickstart",
            Tags: ["quickstart", "PGC", "CSV"],
            Es: new DocCopy(
                Title: "Quickstart: primer payout asentado en PGC",
                Summary:
                "De la prueba gratuita al primer asiento Ready: conectar Stripe/Shopify/Wise, mapear cuentas PGC y exportar CSV a Holded, Xero o QuickBooks.",
                Audience: "Admin · Contable",
                Category: "Inicio"),
            En: new DocCopy(
                Title: "Quickstart: first payout posted to PGC",
                Summary:
                "From free trial to the first Ready journal: connect Stripe/Shopify/Wise, map PGC accounts, and export CSV to Holded, Xero, or QuickBooks.",
                Audience: "Admin · Bookkeeper",
                Category: "Getting started")),
        new(
            Id: "webhooks-hmac",
            Href: "/docs/webhooks-hmac",
            Tags: ["webhooks", "HMAC", "Stripe", "Shopify", "Wise"],
            Es: new DocCopy(
                Title: "Ingesta firmada HMAC: Stripe, Shopify y Wise",
                Summary:
                "Cómo llegan los eventos crudos al libro: verificación HMAC, circuit breaker tras descuadres, idempotencia SHA-256 y buzón de ingesta.",
                Audience: "CTO · Ingeniería",
                Category: "Integración"),
            En: new DocCopy(
                Title: "HMAC-signed ingest: Stripe, Shopify, and Wise",
                Summary:
                "How raw events reach the ledger: HMAC verification, circuit breaker after imbalances, SHA-256 idempotency, and the ingest inbox.",
                Audience: "CTO · Engineering",
                Category: "Integration")),
        new(
            Id: "pgc-engine",
            Href: "/docs/pgc-engine",
            Tags: ["PGC", "668", "768", "129", "477"],
            Es: new DocCopy(
                Title: "Motor PGC: 668, 768, 129 y 477",
                Summary:
                "Partida doble PGC inmutable: diferencias de cambio, cierre trimestral, impuesto de origen en 477 y tolerancia de 0,01 €.",
                Audience: "Contable · Auditor",
                Category: "Contabilidad"),
            En: new DocCopy(
                Title: "PGC engine: 668, 768, 129, and 477",
                Summary:
                "Immutable PGC double-entry: FX differences, quarterly close, source tax on 477, and 0.01 tolerance.",
                Audience: "Bookkeeper · Auditor",
                Category: "Accounting")),
        new(
            Id: "fx-bce",
            Href: "/docs/fx-bce",
            Tags: ["BCE", "ECB", "FX", "multi-divisa"],
            Es: new DocCopy(
                Title: "FX BCE en tres capas",
                Summary:
                "Consulta analítica (sin escribir Posted), asiento del día y cierre trimestral 668/768/129. Contingencia de cotización y fusible 0,01 €.",
                Audience: "CFO · Contable",
                Category: "Divisas"),
            En: new DocCopy(
                Title: "Three-layer ECB FX",
                Summary:
                "Analytics query (never writes Posted), day journal, and quarterly 668/768/129 close. Rate contingency and the 0.01 fuse.",
                Audience: "CFO · Bookkeeper",
                Category: "FX")),
        new(
            Id: "erp-csv",
            Href: "/docs/erp-csv",
            Tags: ["Holded", "Xero", "QuickBooks", "CSV"],
            Es: new DocCopy(
                Title: "Export CSV e importación en Holded, Xero y QuickBooks",
                Summary:
                "Pasos para descargar pennymatch_report / xero / quickbooks journals e importarlos en tu ERP. OAuth QBO/Xero opcional; Holded vive del CSV.",
                Audience: "Admin · Contable",
                Category: "ERP"),
            En: new DocCopy(
                Title: "CSV export and import into Holded, Xero, and QuickBooks",
                Summary:
                "Steps to download pennymatch_report / xero / quickbooks journals and import them into your ERP. Optional QBO/Xero OAuth; Holded runs on CSV.",
                Audience: "Admin · Bookkeeper",
                Category: "ERP")),
        new(
            Id: "scope-limits",
            Href: "/docs/scope-limits",
            Tags: ["transparencia", "alcance", "ERP", "CSV"],
            Es: new DocCopy(
                Title: "Qué es y qué no es Pennymatch",
                Summary:
                "Qué NO incluye: no es ERP ni software de facturación; no emite facturas ni presenta modelos fiscales. Sí middleware PGC + CSV a Holded/Xero/QB. Planes con techo finito.",
                Audience: "CEO · CFO · Gestor",
                Category: "Producto"),
            En: new DocCopy(
                Title: "What Pennymatch is and is not",
                Summary:
                "Not an ERP, not invoicing software, does not issue invoices or file tax returns. Yes: PGC double-entry middleware + CSV to Xero/QB/Holded. Finite plan ceilings.",
                Audience: "CEO · CFO · Advisor",
                Category: "Product")),
        new(
            Id: "security-billing",
            Href: "/docs/security-billing",
            Tags: ["seguridad", "SHA-256", "Lemon Squeezy", "DeepSeek"],
            Es: new DocCopy(
                Title: "Seguridad, privacidad y facturación MoR",
                Summary:
                "Aislamiento multi-tenant, idempotencia, Native AOT, cifrado, auditoría SHA-256, DeepSeek/Penny (sin OpenAI) y Lemon Squeezy como MoR SaaS.",
                Audience: "CTO · Legal · Admin",
                Category: "Seguridad"),
            En: new DocCopy(
                Title: "Security, privacy, and MoR billing",
                Summary:
                "Multi-tenant isolation, idempotency, Native AOT, encryption, SHA-256 audit, DeepSeek/Penny (no OpenAI), and Lemon Squeezy as SaaS MoR.",
                Audience: "CTO · Legal · Admin",
                Category: "Security")),
        // Legacy ids redirected in bodies for old links
        new(
            Id: "idempotency",
            Href: "/docs/idempotency",
            Tags: ["idempotencia", "SHA-256", ".NET 9"],
            Es: new DocCopy(
                Title: "Idempotencia y huellas SHA-256",
                Summary:
                "Claves derivadas del evento normalizado. Por qué Posted no se reescribe y cómo depurar duplicados en cola.",
                Audience: "Ingeniería",
                Category: "Seguridad"),
            En: new DocCopy(
                Title: "Idempotency and SHA-256 fingerprints",
                Summary:
                "Keys derived from the normalized event. Why Posted is never rewritten, and how to debug queue duplicates.",
                Audience: "Engineering",
                Category: "Security"))
    ];

    public DocsCatalogService(
        IMemoryCache cache,
        IWebHostEnvironment env,
        ILogger<DocsCatalogService> logger)
    {
        _cache = cache;
        _env = env;
        _logger = logger;
    }

    public static void WarmCache(IServiceProvider services)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                var docs = services.GetRequiredService<DocsCatalogService>();
                foreach (var code in UiCulture.SupportedCodes)
                {
                    var culture = UiCulture.ParseOrDefault(code).ToCultureInfo();
                    var prevCulture = CultureInfo.CurrentCulture;
                    var prevUi = CultureInfo.CurrentUICulture;
                    try
                    {
                        CultureInfo.CurrentCulture = culture;
                        CultureInfo.CurrentUICulture = culture;
                        await docs.GetAllAsync().ConfigureAwait(false);
                        foreach (var def in Manuals)
                        {
                            await docs.GetBodyHtmlAsync(def.Id).ConfigureAwait(false);
                        }
                    }
                    finally
                    {
                        CultureInfo.CurrentCulture = prevCulture;
                        CultureInfo.CurrentUICulture = prevUi;
                    }
                }
            }
            catch
            {
                // Best-effort warm; request path still populates cache on miss.
            }
        });
    }

    public Task<IReadOnlyList<DocManual>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var culture = CurrentCultureCode();
        var key = $"docs:all:{culture}";
        if (_cache.TryGetValue(key, out IReadOnlyList<DocManual>? cached) && cached is not null)
        {
            return Task.FromResult(cached);
        }

        IReadOnlyList<DocManual> list = Manuals.Select(d => ToManual(d, culture)).ToList();
        _cache.Set(key, list, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = CacheDuration,
            Priority = CacheItemPriority.High
        });
        return Task.FromResult(list);
    }

    public Task<DocManual?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (string.IsNullOrWhiteSpace(id))
        {
            return Task.FromResult<DocManual?>(null);
        }

        var culture = CurrentCultureCode();
        var key = $"docs:one:{culture}:{id.ToLowerInvariant()}";
        if (_cache.TryGetValue(key, out DocManual? cached))
        {
            return Task.FromResult(cached);
        }

        var def = Manuals.FirstOrDefault(m => string.Equals(m.Id, id, StringComparison.OrdinalIgnoreCase));
        var manual = def is null ? null : ToManual(def, culture);
        _cache.Set(key, manual, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = CacheDuration,
            Priority = CacheItemPriority.High
        });
        return Task.FromResult(manual);
    }

    public async Task<IReadOnlyList<DocManual>> GetTeasersAsync(int count = 3, CancellationToken cancellationToken = default)
    {
        var all = await GetAllAsync(cancellationToken).ConfigureAwait(false);
        var take = Math.Clamp(count, 1, all.Count);
        if (take == all.Count)
        {
            return all;
        }

        return all.Take(take).ToList();
    }

    public async Task<string> GetBodyHtmlAsync(string id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var culture = CurrentCultureCode();
        var key = $"docs:body:{culture}:{id.ToLowerInvariant()}";
        if (_cache.TryGetValue(key, out string? cached) && cached is not null)
        {
            return cached;
        }

        var fromDisk = await TryReadBodyFromDiskAsync(id, culture, cancellationToken).ConfigureAwait(false);
        var html = fromDisk ?? DocsArticleBodies.Render(id);
        _cache.Set(key, html, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = CacheDuration,
            Priority = CacheItemPriority.NeverRemove
        });
        return html;
    }

    private async Task<string?> TryReadBodyFromDiskAsync(
        string id,
        string culture,
        CancellationToken cancellationToken)
    {
        var dir = Path.Combine(_env.ContentRootPath, "Content", "docs");
        if (!Directory.Exists(dir))
        {
            return null;
        }

        var candidates = new[]
        {
            Path.Combine(dir, $"{id}.{culture}.html"),
            Path.Combine(dir, $"{id}.html")
        };

        foreach (var path in candidates)
        {
            if (!File.Exists(path))
            {
                continue;
            }

            try
            {
                return await File.ReadAllTextAsync(path, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to read docs body {Path}", path);
            }
        }

        return null;
    }

    private static string CurrentCultureCode() =>
        UiCulture.From(CultureInfo.CurrentUICulture).Code;

    private static DocManual ToManual(DocManualDef def, string culture)
    {
        var copy = culture == "en" ? def.En : def.Es;
        return new DocManual
        {
            Id = def.Id,
            Title = copy.Title,
            Summary = copy.Summary,
            Audience = copy.Audience,
            Category = copy.Category,
            Href = def.Href,
            Tags = def.Tags
        };
    }

    private sealed record DocCopy(string Title, string Summary, string Audience, string Category);

    private sealed record DocManualDef(
        string Id,
        string Href,
        IReadOnlyList<string> Tags,
        DocCopy Es,
        DocCopy En);
}
