namespace Pennymatch.Web.Localization;

/// <summary>
/// AOT-friendly static UI dictionaries keyed by <see cref="UiCulture"/>.
/// No reflection, no NuGet i18n packages.
/// </summary>
internal static class SiteCatalog
{
    private static readonly Dictionary<string, string> Es = BuildEs();
    private static readonly Dictionary<string, string> En = BuildEn();

    public static string Get(UiCulture culture, string key)
    {
        var table = culture.Code == "en" ? En : Es;
        if (table.TryGetValue(key, out var value))
        {
            return value;
        }

        if (Es.TryGetValue(key, out var fallback))
        {
            return fallback;
        }

        return key;
    }

    /// <summary>Keys present in ES but missing in EN (for parity checks).</summary>
    public static IReadOnlyList<string> MissingEnKeys() =>
        Es.Keys.Where(k => !En.ContainsKey(k)).OrderBy(k => k, StringComparer.Ordinal).ToList();

    /// <summary>Keys present in EN but missing in ES (for parity checks).</summary>
    public static IReadOnlyList<string> MissingEsKeys() =>
        En.Keys.Where(k => !Es.ContainsKey(k)).OrderBy(k => k, StringComparer.Ordinal).ToList();

    private static Dictionary<string, string> BuildEs() => new(StringComparer.Ordinal)
    {
        // Nav / chrome
        ["Nav.Aria"] = "Principal",
        ["Nav.Simulator"] = "Simulador",
        ["Nav.Advantages"] = "Ventajas",
        ["Nav.Docs"] = "Docs",
        ["Nav.Blog"] = "Blog",
        ["Nav.Login"] = "Iniciar Sesión",
        ["Nav.Register"] = "Iniciar Prueba Gratuita",
        ["Nav.Product"] = "Ir al producto",
        ["Nav.Menu"] = "Menú",
        ["Nav.OpenMenu"] = "Abrir menú",
        ["Lang.Switch"] = "Idioma",
        ["Lang.Esp"] = "ESP — Español",
        ["Lang.Eng"] = "ENG — English",

        // Hero
        ["Hero.Title"] = "De Stripe, Shopify y Wise a asientos PGC inmutables — sin Excel",
        ["Hero.Lead"] =
            "Middleware técnico de alto rendimiento: captura payouts, comisiones y FX, los asienta en partida doble española (668 / 768, tolerancia 0,01 €, BCE en 3 capas) y exporta CSV universales a Holded, Xero o QuickBooks. No es un ERP. No liquida IVA ni AEAT/SII.",
        ["Hero.CtaRegister"] = "Iniciar Prueba Gratuita",
        ["Hero.CtaDocs"] = "Ver Manuales de Integración",
        ["Hero.PipelineAria"] = "Flujo producto: evento crudo, asiento PGC y CSV hacia el ERP",
        ["Hero.Pipe1Label"] = "Evento crudo",
        ["Hero.Pipe1Code"] = "stripe.charge.succeeded",
        ["Hero.Pipe1Meta"] = "HMAC ✓ · fuente: Stripe",
        ["Hero.Pipe2Label"] = "Asiento PGC",
        ["Hero.Pipe2Code"] = "Ready · 572/626/705",
        ["Hero.Pipe2Meta"] = "sha256 · Δ ≤ 0,01 €",
        ["Hero.Pipe3Label"] = "CSV / ERP",
        ["Hero.Pipe3Code"] = "Holded · Xero · QB",
        ["Hero.Pipe3Meta"] = "exportado · hot cache O(1)",

        // Simulator
        ["Sim.Kicker"] = "Simulador transaccional",
        ["Sim.Title"] = "Del evento crudo al asiento PGC en tres latidos",
        ["Sim.Lead"] =
            "Elige la fuente, observa el pipeline Native AOT en .NET 9 y el borrador inmutable listo para CSV hacia Holded, Xero o QuickBooks — con hot cache O(1).",
        ["Sim.Aria"] = "Simulador de flujo transaccional",
        ["Sim.Source"] = "Fuente",
        ["Sim.SourceHint"] = "Evento transaccional crudo",
        ["Sim.SelectSource"] = "Seleccionar fuente",
        ["Sim.Processor"] = ".NET 9 AOT",
        ["Sim.ProcessorHint"] = "PGC + huella SHA-256",
        ["Sim.StepHmac"] = "Verificar HMAC",
        ["Sim.StepMap"] = "Mapear → asiento PGC",
        ["Sim.StepSeal"] = "Sellar SHA-256",
        ["Sim.StepCache"] = "Escribir hot cache",
        ["Sim.Run"] = "Ejecutar pipeline",
        ["Sim.Running"] = "Procesando…",
        ["Sim.Mor"] = "ERP / CSV",
        ["Sim.MorHint"] = "Borrador inmutable · hot cache O(1)",
        ["Sim.MorAria"] = "Destino contable CSV",
        ["Sim.DeliveryReady"] = "Asiento listo",
        ["Sim.DeliveryIdle"] = "Ejecuta el pipeline para ver el borrador PGC hacia {0}.",
        ["Sim.Ready"] = "Listo para AOT",
        ["Sim.StatusCompile"] = "Compilando pipeline AOT…",
        ["Sim.StatusHmac"] = "HMAC verificado",
        ["Sim.StatusDto"] = "Asiento PGC materializado",
        ["Sim.StatusSeal"] = "Huella SHA-256 sellada",
        ["Sim.StatusCache"] = "Hot cache O(1) actualizada",

        // Advantages
        ["Adv.Kicker"] = "Ventajas competitivas",
        ["Adv.Title"] = "El eslabón que el ecommerce y el SaaS español siguen haciendo en Excel",
        ["Adv.Lead"] =
            "Payouts de Stripe, pedidos Shopify y liquidaciones Wise llegan con comisiones y FX. Pennymatch los convierte en PGC nativo — no en otro conector genérico que te deja el céntimo a ti.",
        ["Adv.Card1Kicker"] = "PGC nativo",
        ["Adv.Card1Title"] = "668 / 768, 0,01 € y partida doble",
        ["Adv.Card1Body"] =
            "Diferencias negativas (668) y positivas (768) de cambio, cierre contra 129, IVA de origen en 477 cuando Shopify lo trae. Tolerancia de un céntimo: por encima es descuadre, no «redondeo creativo».",
        ["Adv.Card2Kicker"] = "FX BCE",
        ["Adv.Card2Title"] = "Tres capas: consulta, asiento y cierre",
        ["Adv.Card2Body"] =
            "Widget analítico sin tocar el libro, tipo del día en cada asiento y cierre trimestral inmutable con BCE. Multi-divisa al céntimo — sin pegar tipos del periódico en un Excel.",
        ["Adv.Card3Kicker"] = "Velocidad y auditoría",
        ["Adv.Card3Title"] = "O(1), Native AOT y SHA-256 encadenado",
        ["Adv.Card3Body"] =
            "Ingesta O(1), idempotencia atómica, asientos Posted inmutables, cifrado y cadena SHA-256 lista para auditoría. CSV universal a Holded, Xero y QuickBooks — sin sustituirlos.",
        ["Adv.Disclaimer"] =
            "Pennymatch <strong>no es un ERP</strong>: no factura, no liquida IVA ni presenta AEAT/SII, y no reemplaza Holded, Xero ni QuickBooks. Es middleware de partida doble hacia tu libro. La suscripción SaaS se cobra vía Lemon Squeezy (MoR activo; Paddle Billing v2 en contingencia).",

        // Docs teaser
        ["DocsTeaser.Kicker"] = "Manuales",
        ["DocsTeaser.Title"] = "Docs para CTOs, contables y PMs",
        ["DocsTeaser.Lead"] =
            "Quickstart, conectores HMAC, motor PGC 668/768/129, FX BCE, import CSV al ERP, límites de alcance y seguridad — copy operativo, no marketing vacío.",
        ["DocsTeaser.All"] = "Ver todos los manuales",

        // CTA band
        ["Cta.Kicker"] = "Prueba gratuita",
        ["Cta.Title"] = "Asienta tu primer payout en minutos",
        ["Cta.Lead"] =
            "Regístrate, conecta Stripe, Shopify o Wise y publica el primer borrador PGC hacia CSV — sin scripts frágiles ni céntimos perdidos en Excel.",

        // Home meta
        ["Home.Title"] = "Pennymatch — Middleware PGC para Stripe, Shopify y Wise",
        ["Home.Description"] =
            "Stripe, Shopify y Wise → asientos PGC inmutables (668/768, 0,01 €, BCE) y CSV para Holded, Xero y QuickBooks. No es un ERP ni liquida IVA/AEAT.",
        ["Home.OgDescription"] =
            "Partida doble PGC desde payouts reales. CSV a Holded/Xero/QB. Sin sustituir tu ERP.",

        // Docs hub
        ["Docs.Title"] = "Manuales de integración — Pennymatch",
        ["Docs.Description"] =
            "Manuales Pennymatch: quickstart, webhooks HMAC, motor PGC, FX BCE, CSV a ERP, límites de alcance, seguridad y facturación MoR Lemon Squeezy.",
        ["Docs.OgDescription"] = "Guías operativas del middleware PGC hacia Holded, Xero y QuickBooks.",
        ["Docs.Kicker"] = "Documentación",
        ["Docs.Heading"] = "Manuales de integración y operaciones",
        ["Docs.Lead"] =
            "Pennymatch es middleware técnico: de eventos de pago a asientos PGC inmutables y CSV. Estas guías sirven para integrar ya — no para sustituir tu ERP ni liquidar impuestos.",
        ["Docs.BackHome"] = "Volver al inicio",
        ["Docs.CatalogAria"] = "Catálogo de manuales",
        ["Docs.NotFoundTitle"] = "Manual no encontrado — Pennymatch",
        ["Docs.NotFoundHeading"] = "Manual no encontrado",
        ["Docs.NotFoundLead"] = "Ese identificador no está en el catálogo. Revisa el índice de documentación.",
        ["Docs.ViewAll"] = "Ver manuales",
        ["Docs.AllManuals"] = "Todos los manuales",
        ["Docs.MoreGuides"] = "Más guías",
        ["Docs.Preparing"] = "Contenido en preparación.",

        // Blog chrome
        ["Blog.Title"] = "Blog — Pennymatch",
        ["Blog.Description"] =
            "Artículos sobre middleware PGC: webhooks HMAC, 668/768 multi-divisa, CSV a ERP y seguridad multi-tenant — sin OpenAI y sin vender un ERP.",
        ["Blog.OgDescription"] =
            "Guías prácticas de partida doble española, FX BCE e idempotencia SHA-256.",
        ["Blog.Kicker"] = "Blog",
        ["Blog.Heading"] = "Ingeniería del puente al PGC",
        ["Blog.Lead"] = "Webhooks, asientos inmutables y multi-divisa al céntimo — transparencia sobre lo que Pennymatch no es.",
        ["Blog.ArticlesAria"] = "Artículos",
        ["Blog.NotFoundTitle"] = "Artículo no encontrado — Pennymatch",
        ["Blog.NotFoundHeading"] = "Artículo no encontrado",
        ["Blog.NotFoundLead"] = "El slug no existe. Vuelve al",
        ["Blog.Back"] = "blog",
        ["Blog.ReadingMin"] = "{0} min de lectura",
        ["Blog.Recommended"] = "Recomendados",
        ["Blog.SidebarAria"] = "Contenido relacionado",
        ["Newsletter.Title"] = "Newsletter",
        ["Newsletter.Lead"] = "Recibe guías de PGC, webhooks y seguridad multi-tenant. Sin spam.",
        ["Newsletter.Thanks"] = "Gracias. Te avisaremos cuando publiquemos el próximo artículo.",
        ["Newsletter.Email"] = "Email",
        ["Newsletter.Placeholder"] = "tu@empresa.com",
        ["Newsletter.Invalid"] = "Introduce un email válido.",
        ["Newsletter.Submit"] = "Suscribirme",

        // Footer
        ["Footer.Blurb"] =
            "Middleware técnico: Stripe, Shopify y Wise → asientos PGC inmutables → CSV para Holded, Xero y QuickBooks. No es un ERP; no liquida IVA ni AEAT/SII. SaaS facturado vía Lemon Squeezy.",
        ["Footer.Product"] = "Producto",
        ["Footer.Resources"] = "Recursos",
        ["Footer.Manuals"] = "Manuales",
        ["Footer.Trial"] = "Prueba gratuita",
        ["Footer.Login"] = "Iniciar sesión",
        ["Footer.Register"] = "Registrarse",
        ["Footer.Legal"] = "Legal",
        ["Footer.LegalNotice"] = "Aviso legal",
        ["Footer.Privacy"] = "Privacidad",
        ["Footer.Terms"] = "Términos",
        ["Footer.Cookies"] = "Cookies",
        ["Footer.Subprocessors"] = "Subencargados",
        ["Footer.Copy"] = "© {0} Pennymatch. Todos los derechos reservados.",
    };

    private static Dictionary<string, string> BuildEn() => new(StringComparer.Ordinal)
    {
        ["Nav.Aria"] = "Primary",
        ["Nav.Simulator"] = "Simulator",
        ["Nav.Advantages"] = "Advantages",
        ["Nav.Docs"] = "Docs",
        ["Nav.Blog"] = "Blog",
        ["Nav.Login"] = "Sign in",
        ["Nav.Register"] = "Start free trial",
        ["Nav.Product"] = "Open product",
        ["Nav.Menu"] = "Menu",
        ["Nav.OpenMenu"] = "Open menu",
        ["Lang.Switch"] = "Language",
        ["Lang.Esp"] = "ESP — Español",
        ["Lang.Eng"] = "ENG — English",

        ["Hero.Title"] = "From Stripe, Shopify, and Wise to immutable Spanish PGC journals — no Excel",
        ["Hero.Lead"] =
            "High-performance technical middleware: capture payouts, fees, and FX, post Spanish double-entry drafts (668 / 768, 0.01 EUR tolerance, three-layer BCE FX), and export universal CSVs to Holded, Xero, or QuickBooks. Not an ERP. No VAT settlement or AEAT/SII filing.",
        ["Hero.CtaRegister"] = "Start free trial",
        ["Hero.CtaDocs"] = "View integration manuals",
        ["Hero.PipelineAria"] = "Product flow: raw event, PGC journal, and CSV toward the ERP",
        ["Hero.Pipe1Label"] = "Raw event",
        ["Hero.Pipe1Code"] = "stripe.charge.succeeded",
        ["Hero.Pipe1Meta"] = "HMAC ✓ · source: Stripe",
        ["Hero.Pipe2Label"] = "PGC journal",
        ["Hero.Pipe2Code"] = "Ready · 572/626/705",
        ["Hero.Pipe2Meta"] = "sha256 · Δ ≤ 0.01 EUR",
        ["Hero.Pipe3Label"] = "CSV / ERP",
        ["Hero.Pipe3Code"] = "Holded · Xero · QB",
        ["Hero.Pipe3Meta"] = "exported · hot cache O(1)",

        ["Sim.Kicker"] = "Transactional simulator",
        ["Sim.Title"] = "From raw event to PGC journal in three heartbeats",
        ["Sim.Lead"] =
            "Choose the source, watch the .NET 9 Native AOT pipeline, and see the immutable draft ready for CSV to Holded, Xero, or QuickBooks — with O(1) hot cache.",
        ["Sim.Aria"] = "Transactional flow simulator",
        ["Sim.Source"] = "Source",
        ["Sim.SourceHint"] = "Raw transactional event",
        ["Sim.SelectSource"] = "Select source",
        ["Sim.Processor"] = ".NET 9 AOT",
        ["Sim.ProcessorHint"] = "PGC + SHA-256 footprint",
        ["Sim.StepHmac"] = "Verify HMAC",
        ["Sim.StepMap"] = "Map → PGC journal",
        ["Sim.StepSeal"] = "Stamp SHA-256",
        ["Sim.StepCache"] = "Write hot cache",
        ["Sim.Run"] = "Execute pipeline",
        ["Sim.Running"] = "Processing…",
        ["Sim.Mor"] = "ERP / CSV",
        ["Sim.MorHint"] = "Immutable draft · O(1) hot cache",
        ["Sim.MorAria"] = "CSV accounting destination",
        ["Sim.DeliveryReady"] = "Journal ready",
        ["Sim.DeliveryIdle"] = "Execute the pipeline to see the PGC draft heading to {0}.",
        ["Sim.Ready"] = "Ready for AOT",
        ["Sim.StatusCompile"] = "Compiling AOT pipeline…",
        ["Sim.StatusHmac"] = "HMAC verified",
        ["Sim.StatusDto"] = "PGC journal materialized",
        ["Sim.StatusSeal"] = "SHA-256 footprint stamped",
        ["Sim.StatusCache"] = "O(1) hot cache updated",

        ["Adv.Kicker"] = "Competitive advantages",
        ["Adv.Title"] = "The missing link Spanish ecommerce and SaaS still finish in Excel",
        ["Adv.Lead"] =
            "Stripe payouts, Shopify orders, and Wise settlements arrive with fees and FX. Pennymatch turns them into native PGC — not another generic connector that leaves the cent to you.",
        ["Adv.Card1Kicker"] = "Native PGC",
        ["Adv.Card1Title"] = "668 / 768, 0.01 EUR, double-entry",
        ["Adv.Card1Body"] =
            "Negative (668) and positive (768) FX differences, quarter close against 129, source-sourced VAT on 477 when Shopify sends it. One-cent tolerance: above that is imbalance, not creative rounding.",
        ["Adv.Card2Kicker"] = "BCE FX",
        ["Adv.Card2Title"] = "Three layers: query, journal, close",
        ["Adv.Card2Body"] =
            "Analytics widget that never writes the book, per-day rate on each journal, and immutable quarterly close against BCE. Multi-currency to the cent — no newspaper rates pasted into a spreadsheet.",
        ["Adv.Card3Kicker"] = "Speed and audit",
        ["Adv.Card3Title"] = "O(1), Native AOT, chained SHA-256",
        ["Adv.Card3Body"] =
            "O(1) ingest, atomic idempotency, immutable Posted journals, encryption, and a SHA-256 chain built for audit. Universal CSV to Holded, Xero, and QuickBooks — without replacing them.",
        ["Adv.Disclaimer"] =
            "Pennymatch is <strong>not an ERP</strong>: it does not invoice, settle VAT, or file AEAT/SII, and it does not replace Holded, Xero, or QuickBooks. It is double-entry middleware into your books. The SaaS fee is billed via Lemon Squeezy (active MoR; Paddle Billing v2 as contingency).",

        ["DocsTeaser.Kicker"] = "Manuals",
        ["DocsTeaser.Title"] = "Docs for CTOs, bookkeepers, and PMs",
        ["DocsTeaser.Lead"] =
            "Quickstart, HMAC connectors, PGC 668/768/129 engine, BCE FX, ERP CSV import, scope limits, and security — operational copy, not empty marketing.",
        ["DocsTeaser.All"] = "View all manuals",

        ["Cta.Kicker"] = "Free trial",
        ["Cta.Title"] = "Post your first payout in minutes",
        ["Cta.Lead"] =
            "Sign up, connect Stripe, Shopify, or Wise, and publish the first PGC draft to CSV — no fragile scripts, no cents lost in Excel.",

        ["Home.Title"] = "Pennymatch — PGC middleware for Stripe, Shopify, and Wise",
        ["Home.Description"] =
            "Stripe, Shopify, and Wise → immutable Spanish PGC journals (668/768, 0.01 EUR, BCE) and CSV for Holded, Xero, and QuickBooks. Not an ERP; no VAT/AEAT.",
        ["Home.OgDescription"] =
            "Spanish double-entry from real payouts. CSV to Holded/Xero/QB. Does not replace your ERP.",

        ["Docs.Title"] = "Integration manuals — Pennymatch",
        ["Docs.Description"] =
            "Pennymatch manuals: quickstart, HMAC webhooks, PGC engine, BCE FX, ERP CSV, scope limits, security, and Lemon Squeezy MoR billing.",
        ["Docs.OgDescription"] = "Operational guides for PGC middleware toward Holded, Xero, and QuickBooks.",
        ["Docs.Kicker"] = "Documentation",
        ["Docs.Heading"] = "Integration and operations manuals",
        ["Docs.Lead"] =
            "Pennymatch is technical middleware: payment events to immutable PGC journals and CSV. These guides help you integrate now — not replace your ERP or settle taxes.",
        ["Docs.BackHome"] = "Back to home",
        ["Docs.CatalogAria"] = "Manuals catalog",
        ["Docs.NotFoundTitle"] = "Manual not found — Pennymatch",
        ["Docs.NotFoundHeading"] = "Manual not found",
        ["Docs.NotFoundLead"] = "That id is not in the catalog. Check the documentation index.",
        ["Docs.ViewAll"] = "View manuals",
        ["Docs.AllManuals"] = "All manuals",
        ["Docs.MoreGuides"] = "More guides",
        ["Docs.Preparing"] = "Content in preparation.",

        ["Blog.Title"] = "Blog — Pennymatch",
        ["Blog.Description"] =
            "Articles on PGC middleware: HMAC webhooks, multi-currency 668/768, ERP CSV, and multi-tenant security — no OpenAI, no ERP pitch.",
        ["Blog.OgDescription"] =
            "Practical guides for Spanish double-entry, BCE FX, and SHA-256 idempotency.",
        ["Blog.Kicker"] = "Blog",
        ["Blog.Heading"] = "Engineering the bridge to the PGC",
        ["Blog.Lead"] = "Webhooks, immutable journals, and multi-currency to the cent — clear about what Pennymatch is not.",
        ["Blog.ArticlesAria"] = "Articles",
        ["Blog.NotFoundTitle"] = "Article not found — Pennymatch",
        ["Blog.NotFoundHeading"] = "Article not found",
        ["Blog.NotFoundLead"] = "That slug does not exist. Back to the",
        ["Blog.Back"] = "blog",
        ["Blog.ReadingMin"] = "{0} min read",
        ["Blog.Recommended"] = "Recommended",
        ["Blog.SidebarAria"] = "Related content",
        ["Newsletter.Title"] = "Newsletter",
        ["Newsletter.Lead"] = "Get PGC, webhook, and multi-tenant security guides. No spam.",
        ["Newsletter.Thanks"] = "Thanks. We will notify you when the next article ships.",
        ["Newsletter.Email"] = "Email",
        ["Newsletter.Placeholder"] = "you@company.com",
        ["Newsletter.Invalid"] = "Enter a valid email.",
        ["Newsletter.Submit"] = "Subscribe",

        ["Footer.Blurb"] =
            "Technical middleware: Stripe, Shopify, and Wise → immutable PGC journals → CSV for Holded, Xero, and QuickBooks. Not an ERP; no VAT or AEAT/SII filing. SaaS billed via Lemon Squeezy.",
        ["Footer.Product"] = "Product",
        ["Footer.Resources"] = "Resources",
        ["Footer.Manuals"] = "Manuals",
        ["Footer.Trial"] = "Free trial",
        ["Footer.Login"] = "Sign in",
        ["Footer.Register"] = "Sign up",
        ["Footer.Legal"] = "Legal",
        ["Footer.LegalNotice"] = "Legal notice",
        ["Footer.Privacy"] = "Privacy",
        ["Footer.Terms"] = "Terms",
        ["Footer.Cookies"] = "Cookies",
        ["Footer.Subprocessors"] = "Subprocessors",
        ["Footer.Copy"] = "© {0} Pennymatch. All rights reserved.",
    };
}
