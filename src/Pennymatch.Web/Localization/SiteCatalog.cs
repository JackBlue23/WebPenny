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
        ["Nav.Pricing"] = "Precios",
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
            "Middleware técnico de alto rendimiento: captura payouts, comisiones y FX, los asienta en partida doble rigurosa (668 / 768, tolerancia 0,01 €, tipos BCE en 3 capas) y exporta CSV universales a Holded, Xero o QuickBooks. No es un ERP ni software de facturación: no emite facturas ni presenta modelos fiscales.",
        ["Hero.CtaRegister"] = "Iniciar Prueba Gratuita",
        ["Hero.CtaDocs"] = "Ver Manuales de Integración",
        ["Hero.PipelineAria"] = "Flujo producto: evento bruto, asiento PGC y CSV hacia el ERP",
        ["Hero.Pipe1Label"] = "Evento bruto",
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
        ["Sim.Title"] = "Del evento bruto al asiento contable en tres pasos",
        ["Sim.Lead"] =
            "Elige la fuente, observa el pipeline Native AOT en .NET 9 y el borrador inmutable listo para CSV hacia Holded, Xero o QuickBooks — con hot cache O(1).",
        ["Sim.Aria"] = "Simulador de flujo transaccional",
        ["Sim.Source"] = "Fuente",
        ["Sim.SourceHint"] = "Evento transaccional bruto",
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
        ["Adv.Title"] = "El trabajo contable que ecommerce y SaaS globales aún resuelven en hojas de cálculo",
        ["Adv.Lead"] =
            "Payouts de Stripe, pedidos Shopify y liquidaciones Wise llegan con comisiones y FX. Pennymatch los convierte en asientos PGC de partida doble — no en otro conector genérico que deja el céntimo sin asentar.",
        ["Adv.Card1Kicker"] = "PGC nativo",
        ["Adv.Card1Title"] = "668 / 768, 0,01 € y partida doble inmutable",
        ["Adv.Card1Body"] =
            "Los payouts multi-divisa descuadran el libro cuando comisiones y FX se asientan a mano. Pennymatch aplica diferencias 668/768, cierre contra 129 e impuesto de origen en 477 (Shopify), con tolerancia de 0,01 €. Resultado: borradores inmutables listos para auditoría, sin redondeos ad hoc.",
        ["Adv.Card2Kicker"] = "FX BCE",
        ["Adv.Card2Title"] = "Tres capas: consulta, asiento y cierre",
        ["Adv.Card2Body"] =
            "Tipos inconsistentes entre panel, asiento y cierre generan descuadres. Pennymatch aplica BCE en tres capas — widget analítico, tipo del día por asiento y cierre trimestral inmutable — sin buscar ni introducir manualmente tipos de cambio en hojas de cálculo. Multi-divisa al céntimo con rastro auditable.",
        ["Adv.Card3Kicker"] = "Velocidad y auditoría",
        ["Adv.Card3Title"] = "O(1), Native AOT y SHA-256 encadenado",
        ["Adv.Card3Body"] =
            "Scripts frágiles y exports sin huella no resisten una revisión. Pennymatch ingiere en O(1) con idempotencia atómica, sella asientos Posted con cadena SHA-256 y exporta CSV a Holded, Xero y QuickBooks. Operación auditable sin sustituir tu ERP.",
        ["Adv.Disclaimer"] =
            "<strong>Qué NO incluye:</strong> Pennymatch <strong>no es un ERP ni software de facturación</strong>: no emite facturas, no presenta modelos fiscales y no reemplaza Holded, Xero ni QuickBooks. Es middleware de partida doble hacia tu libro. La suscripción SaaS se cobra vía Lemon Squeezy (MoR activo; Paddle Billing v2 en contingencia).",

        // Pricing (aligned with Pennymatch Client TarifasCatalog)
        ["Pricing.Title"] = "Planes transparentes con topes de seguridad garantizados",
        ["Pricing.TrialNote"] =
            "Pruébalo gratis durante el primer mes. Se requiere tarjeta para activar la automatización mediante Stripe Checkout, pero puedes cancelar con un solo clic en cualquier momento antes del día 30.",
        ["Pricing.StartTrial"] = "Empezar prueba gratis",
        ["Pricing.TrialHint"] = "30 días gratis · Sin cargos hoy",
        ["Pricing.BillingAria"] = "Ciclo de facturación",
        ["Pricing.CurrencyAria"] = "Moneda",
        ["Pricing.Monthly"] = "Mensual",
        ["Pricing.Yearly"] = "Anual",
        ["Pricing.SaveBadge"] = "-17%",
        ["Pricing.CurrencyEur"] = "EUR (€)",
        ["Pricing.CurrencyUsd"] = "USD ($)",
        ["Pricing.PerMonth"] = "/ mes",
        ["Pricing.PerYear"] = "/ año",
        ["Pricing.Recommended"] = "Recomendado",
        ["Pricing.Footnote"] =
            "Precios regionalizados. Las suscripciones se procesan en la moneda local de tu pasarela de pago según la ubicación de tu empresa a través de Stripe.",
        ["Pricing.Growth.Name"] = "Plan Growth",
        ["Pricing.Growth.Summary"] = "Ideal para tiendas en crecimiento.",
        ["Pricing.Growth.F1"] = "Hasta 1.000 transacciones/mes",
        ["Pricing.Growth.F2"] = "Facturación < 50.000 €/mes",
        ["Pricing.Growth.F3"] = "Conciliación Shopify ↔ Stripe en un clic",
        ["Pricing.Growth.F4"] = "Tope de seguridad que pausa la ingesta al límite",
        ["Pricing.Growth.F5"] = "Exportación CSV a Holded, Xero y QuickBooks",
        ["Pricing.Pro.Name"] = "Plan Pro",
        ["Pricing.Pro.Summary"] = "Para operaciones que ya cruzan umbrales multi-divisa.",
        ["Pricing.Pro.F1"] = "Hasta 5.000 transacciones/mes",
        ["Pricing.Pro.F2"] = "Facturación 50k € – 250k €/mes",
        ["Pricing.Pro.F3"] = "Soporte multi-divisa y descuadres Stripe",
        ["Pricing.Pro.F4"] = "Prioridad de cola frente a Growth",
        ["Pricing.Pro.F5"] = "Exportación CSV a Holded, Xero y QuickBooks",
        ["Pricing.Enterprise.Name"] = "Plan Enterprise",
        ["Pricing.Enterprise.Summary"] = "Blindaje técnico de capacidad para marcas consolidadas.",
        ["Pricing.Enterprise.F1"] = "Hasta 20.000 transacciones/mes",
        ["Pricing.Enterprise.F2"] = "Facturación 250k € – 1M €/mes",
        ["Pricing.Enterprise.F3"] = "Capacidad reservada para picos de campaña",
        ["Pricing.Enterprise.F4"] = "Techo finito: nunca un plan ilimitado opaco",
        ["Pricing.Enterprise.F5"] = "Exportación CSV a Holded, Xero y QuickBooks",

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
            "Stripe, Shopify y Wise → asientos PGC inmutables (668/768, 0,01 €, BCE) y CSV para Holded, Xero y QuickBooks. No es ERP ni software de facturación.",
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
            "Pennymatch es middleware técnico country-agnostic: de eventos de pago a asientos PGC inmutables y CSV hacia Xero, QuickBooks u Holded. Estas guías sirven para integrar ya — no para sustituir tu ERP ni emitir facturas.",
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
            "Guías prácticas de partida doble PGC, FX BCE e idempotencia SHA-256.",
        ["Blog.Kicker"] = "Blog",
        ["Blog.Heading"] = "Guías técnicas de middleware PGC",
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
            "Middleware técnico: Stripe, Shopify y Wise → asientos PGC inmutables → CSV para Holded, Xero y QuickBooks. No es un ERP ni software de facturación; no emite facturas ni presenta modelos fiscales. SaaS facturado vía Lemon Squeezy.",
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
        ["Nav.Pricing"] = "Pricing",
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

        ["Hero.Title"] = "From Stripe, Shopify, and Wise to immutable double-entry journals — no Excel",
        ["Hero.Lead"] =
            "High-performance technical middleware: capture multi-currency payouts, fees, and FX, post immutable double-entry drafts (PGC accounts 668 / 768, 0.01 tolerance, three-layer ECB rates), and export universal CSVs to Xero, QuickBooks, or Holded. Not an ERP. Not invoicing software — and never posts duplicate entries.",
        ["Hero.CtaRegister"] = "Start free trial",
        ["Hero.CtaDocs"] = "View integration manuals",
        ["Hero.PipelineAria"] = "Product flow: raw payment event, PGC journal, and CSV toward the ERP",
        ["Hero.Pipe1Label"] = "Raw payment event",
        ["Hero.Pipe1Code"] = "stripe.charge.succeeded",
        ["Hero.Pipe1Meta"] = "HMAC ✓ · source: Stripe",
        ["Hero.Pipe2Label"] = "PGC journal",
        ["Hero.Pipe2Code"] = "Ready · 572/626/705",
        ["Hero.Pipe2Meta"] = "sha256 · Δ ≤ 0.01 EUR",
        ["Hero.Pipe3Label"] = "CSV / ERP",
        ["Hero.Pipe3Code"] = "Holded · Xero · QB",
        ["Hero.Pipe3Meta"] = "exported · hot cache O(1)",

        ["Sim.Kicker"] = "Transactional simulator",
        ["Sim.Title"] = "From raw payment event to double-entry journal in three steps",
        ["Sim.Lead"] =
            "Choose the source, watch the .NET 9 Native AOT pipeline, and see the immutable draft ready for CSV to Holded, Xero, or QuickBooks — with O(1) hot cache.",
        ["Sim.Aria"] = "Transactional flow simulator",
        ["Sim.Source"] = "Source",
        ["Sim.SourceHint"] = "Raw payment event",
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
        ["Adv.Title"] = "The accounting work global ecommerce and SaaS still finish in spreadsheets",
        ["Adv.Lead"] =
            "Stripe payouts, Shopify orders, and Wise settlements arrive with fees and FX. Pennymatch turns them into native PGC double-entry — not another generic connector that leaves the cent unposted.",
        ["Adv.Card1Kicker"] = "PGC double-entry",
        ["Adv.Card1Title"] = "668 / 768, 0.01, immutable journals",
        ["Adv.Card1Body"] =
            "Multi-currency payouts break the books when fees and FX are posted by hand. Pennymatch applies 668/768 differences, period close against 129, and origin tax on 477 when Shopify sends it, with 0.01 tolerance. Result: immutable drafts ready for audit — duplicates rejected, no ad-hoc rounding.",
        ["Adv.Card2Kicker"] = "ECB FX",
        ["Adv.Card2Title"] = "Three layers: query, journal, close",
        ["Adv.Card2Body"] =
            "Inconsistent rates across dashboard, journal, and close create imbalances. Pennymatch applies ECB rates in three layers — analytics widget, per-day rate on each journal, and immutable quarterly close — without manually looking up exchange rates in spreadsheets. Multi-currency to the cent with an auditable trail.",
        ["Adv.Card3Kicker"] = "Speed and audit",
        ["Adv.Card3Title"] = "O(1), Native AOT, chained SHA-256",
        ["Adv.Card3Body"] =
            "Fragile scripts and unsigned exports fail review. Pennymatch ingests in O(1) with atomic idempotency, seals Posted journals with a SHA-256 chain, and exports CSV to Xero, QuickBooks, and Holded. Auditable operations without replacing your ERP.",
        ["Adv.Disclaimer"] =
            "<strong>What it is not:</strong> Pennymatch is <strong>not an ERP and not invoicing software</strong>: it does not issue invoices, file tax returns, or replace Xero, QuickBooks, or Holded. It is double-entry middleware into your books. The SaaS fee is billed via Lemon Squeezy (active MoR; Paddle Billing v2 as contingency).",

        ["Pricing.Title"] = "Transparent plans with guaranteed safety caps",
        ["Pricing.TrialNote"] =
            "Try it free for the first month. A card is required to activate automation via Stripe Checkout, but you can cancel in one click any time before day 30.",
        ["Pricing.StartTrial"] = "Start free trial",
        ["Pricing.TrialHint"] = "30 days free · No charges today",
        ["Pricing.BillingAria"] = "Billing cycle",
        ["Pricing.CurrencyAria"] = "Currency",
        ["Pricing.Monthly"] = "Monthly",
        ["Pricing.Yearly"] = "Yearly",
        ["Pricing.SaveBadge"] = "-17%",
        ["Pricing.CurrencyEur"] = "EUR (€)",
        ["Pricing.CurrencyUsd"] = "USD ($)",
        ["Pricing.PerMonth"] = "/ mo",
        ["Pricing.PerYear"] = "/ yr",
        ["Pricing.Recommended"] = "Recommended",
        ["Pricing.Footnote"] =
            "Regionalized pricing. Subscriptions are processed in your payment gateway’s local currency based on your company location via Stripe.",
        ["Pricing.Growth.Name"] = "Growth plan",
        ["Pricing.Growth.Summary"] = "Ideal for growing stores.",
        ["Pricing.Growth.F1"] = "Up to 1,000 transactions/month",
        ["Pricing.Growth.F2"] = "Billing under $50,000/month",
        ["Pricing.Growth.F3"] = "Shopify ↔ Stripe reconciliation in one click",
        ["Pricing.Growth.F4"] = "Safety cap that pauses ingest at the limit",
        ["Pricing.Growth.F5"] = "CSV export to Holded, Xero, and QuickBooks",
        ["Pricing.Pro.Name"] = "Pro plan",
        ["Pricing.Pro.Summary"] = "For operations that already cross multi-currency thresholds.",
        ["Pricing.Pro.F1"] = "Up to 5,000 transactions/month",
        ["Pricing.Pro.F2"] = "Billing $50k – $250k/month",
        ["Pricing.Pro.F3"] = "Multi-currency support and Stripe imbalances",
        ["Pricing.Pro.F4"] = "Queue priority over Growth",
        ["Pricing.Pro.F5"] = "CSV export to Holded, Xero, and QuickBooks",
        ["Pricing.Enterprise.Name"] = "Enterprise plan",
        ["Pricing.Enterprise.Summary"] = "Capacity shielding for established brands.",
        ["Pricing.Enterprise.F1"] = "Up to 20,000 transactions/month",
        ["Pricing.Enterprise.F2"] = "Billing $250k – $1M/month",
        ["Pricing.Enterprise.F3"] = "Reserved capacity for campaign peaks",
        ["Pricing.Enterprise.F4"] = "Finite ceiling: never an opaque unlimited plan",
        ["Pricing.Enterprise.F5"] = "CSV export to Holded, Xero, and QuickBooks",

        ["DocsTeaser.Kicker"] = "Manuals",
        ["DocsTeaser.Title"] = "Docs for CTOs, bookkeepers, and PMs",
        ["DocsTeaser.Lead"] =
            "Quickstart, HMAC connectors, PGC 668/768/129 engine, ECB FX, ERP CSV import, scope limits, and security — operational copy, not empty marketing.",
        ["DocsTeaser.All"] = "View all manuals",

        ["Cta.Kicker"] = "Free trial",
        ["Cta.Title"] = "Post your first payout in minutes",
        ["Cta.Lead"] =
            "Sign up, connect Stripe, Shopify, or Wise, and publish the first PGC draft to CSV — no fragile scripts, no cents lost in Excel.",

        ["Home.Title"] = "Pennymatch — double-entry middleware for Stripe, Shopify, and Wise",
        ["Home.Description"] =
            "Multi-currency Stripe, Shopify, and Wise → immutable PGC journals (668/768, 0.01, ECB rates) and CSV for Xero, QuickBooks, and Holded. Not an ERP; not invoicing software.",
        ["Home.OgDescription"] =
            "Immutable double-entry from real payouts. CSV to Xero/QB/Holded. Does not replace your ERP.",

        ["Docs.Title"] = "Integration manuals — Pennymatch",
        ["Docs.Description"] =
            "Pennymatch manuals: quickstart, HMAC webhooks, PGC engine, ECB FX, ERP CSV, scope limits, security, and Lemon Squeezy MoR billing.",
        ["Docs.OgDescription"] = "Operational guides for PGC middleware toward Xero, QuickBooks, and Holded.",
        ["Docs.Kicker"] = "Documentation",
        ["Docs.Heading"] = "Integration and operations manuals",
        ["Docs.Lead"] =
            "Pennymatch is country-agnostic technical middleware: payment events to immutable PGC journals and CSV into Xero, QuickBooks, or Holded. These guides help you integrate now — not replace your ERP or issue invoices.",
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
            "Practical guides for PGC double-entry, ECB FX, and SHA-256 idempotency.",
        ["Blog.Kicker"] = "Blog",
        ["Blog.Heading"] = "Technical guides for PGC middleware",
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
            "Technical middleware: multi-currency Stripe, Shopify, and Wise → immutable PGC journals → CSV for Xero, QuickBooks, and Holded. Not an ERP or invoicing software; no duplicate entries. SaaS billed via Lemon Squeezy.",
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
