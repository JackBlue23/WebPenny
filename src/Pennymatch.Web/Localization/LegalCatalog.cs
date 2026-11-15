namespace Pennymatch.Web.Localization;

/// <summary>
/// Marketing-site legal copy aligned with Pennymatch Client <c>LegalCopy</c> (rc1).
/// AOT-friendly static strings — no reflection. Placeholders ready for Go-Live NIF/domicilio.
/// </summary>
internal static class LegalCatalog
{
    // Go-Live placeholders (match product AppCatalog Legal_* keys).
    public const string OperatorName = "PENNYMATCH, S.L.U.";
    public const string OperatorNif = "B########";
    public const string OperatorAddress =
        "Calle del Local Propio 1, Planta Baja, 32500, O Carballiño, Orense, España.";
    public const string OperatorEmail = "soporte@pennymatch.com";
    public const string OperatorPrivacyEmail = "privacy@pennymatch.com";
    public const string MercantileRegister =
        "Registro Mercantil de Orense, Tomo [n], Folio [n], Hoja [OU-n], Inscripción 1ª.";

    public const string AccountingDisclaimerEs =
        "Pennymatch opera estrictamente como un middleware técnico e inmutable de transformación de datos. El software se limita a procesar webhooks y estructurar ficheros CSV universales compatibles con Xero, QuickBooks y Holded de acuerdo al PGC como estándar riguroso de partida doble. Pennymatch NO es un ERP ni software de facturación, NO emite facturas, NO presenta modelos fiscales ni ejerce asesoría tributaria. La auditoría, validación y presentación de la contabilidad es responsabilidad exclusiva del Tenant y su asesor homologado.";

    public const string AccountingDisclaimerEn =
        "Pennymatch operates strictly as an immutable technical data-transformation middleware. The software is limited to processing webhooks and structuring universal CSV files compatible with Xero, QuickBooks and Holded under the PGC as a rigorous double-entry standard. Pennymatch is NOT an ERP, is NOT invoicing software, does NOT issue invoices, does NOT file tax returns and does not provide tax advice. Audit, validation and filing of the accounts remain the exclusive responsibility of the Tenant and its licensed tax adviser.";

    public static string Disclaimer(UiCulture culture) =>
        culture.Code == "en" ? AccountingDisclaimerEn : AccountingDisclaimerEs;

    public static IReadOnlyList<LegalSection> Notice(UiCulture culture) => culture.Code == "en"
        ?
        [
            new("1. General information",
                $"[Company name]: {OperatorName}. [Tax ID / NIF]: {OperatorNif}. [Registered office]: {OperatorAddress} [Contact email]: {OperatorEmail}. {MercantileRegister} Single-member limited company (S.L.U.) formed under Spanish Act 18/2022 (Crea y Crece) with share capital of one euro. Pennymatch is a B2B SaaS operator. This notice is published under Spanish Act 34/2002 (LSSI-CE)."),
            new("2. Terms of use and purpose",
                "Pennymatch is accounting middleware / bridge: it maps sales and collection events into immutable PGC double-entry journal drafts and optional connectors (QuickBooks Online, Xero, Holded). Lemon Squeezy, LLC is the primary Merchant of Record for the SaaS fee; Paddle Payments Ltd. may act as authorised contingency MoR under a feature flag. Access is limited to authorised tenant users. Misuse, scraping or reverse engineering is prohibited."),
            new("3. Intellectual property",
                "Software, trademarks, documentation, UI, connectors and know-how remain the exclusive property of Pennymatch or its licensors. The subscription grants a non-exclusive, non-transferable, tenant-scoped right of use. The customer retains ownership of its ledger data."),
            new("4. Disclaimer", AccountingDisclaimerEn),
            new("5. Governing law and jurisdiction",
                "This notice and the use of the service are governed by the laws of Spain, excluding conflict-of-law rules. Subject to mandatory EU consumer or data-protection venues, the courts of Ourense have exclusive jurisdiction over B2B disputes.")
        ]
        :
        [
            new("1. Información general",
                $"[Nombre de la Empresa]: {OperatorName}. [CIF/NIF]: {OperatorNif}. [Domicilio Social]: {OperatorAddress} [Email de Contacto]: {OperatorEmail}. {MercantileRegister} Sociedad Limitada Unipersonal constituida al amparo de la Ley 18/2022 (Crea y Crece), con capital social de 1 euro. Pennymatch es un operador SaaS B2B. Este aviso se publica al amparo de la Ley 34/2002 (LSSI-CE)."),
            new("2. Condiciones de uso y objeto",
                "Pennymatch es un middleware / puente contable: traduce eventos de ventas y cobros a borradores de asientos PGC (partida doble) y, en su caso, a conectores (QuickBooks Online, Xero, Holded). Lemon Squeezy, LLC es el Merchant of Record principal de la cuota SaaS; Paddle Payments Ltd. puede actuar como MoR de contingencia autorizado bajo un feature flag. El acceso queda limitado a usuarios autorizados del inquilino. Quedan prohibidos el uso indebido, el scraping y la ingeniería inversa."),
            new("3. Propiedad intelectual",
                "El software, las marcas, la documentación, la interfaz, los conectores y el know-how son propiedad exclusiva de Pennymatch o de sus licenciantes. La suscripción otorga un derecho de uso no exclusivo, intransferible y limitado al inquilino. El cliente conserva la titularidad de los datos de su libro."),
            new("4. Exclusión de responsabilidad", AccountingDisclaimerEs),
            new("5. Legislación aplicable y jurisdicción",
                "Este aviso y el uso del servicio se rigen por la legislación española, con exclusión de las normas de conflicto de leyes. Sin perjuicio de los fueros imperativos de la UE, los tribunales de Ourense son competentes de forma exclusiva para los litigios B2B.")
        ];

    public static IReadOnlyList<LegalSection> Privacy(UiCulture culture) => culture.Code == "en"
        ?
        [
            new("Controller and lawful basis",
                "Nature of the Service: Pennymatch operates as pure technical middleware in the B2B domain that transforms raw Stripe, Shopify and Wise events into immutable PGC double-entry journal drafts, exporting universal CSV files compatible with Xero, QuickBooks and Holded. Source platforms belong to the Tenant’s ecosystem; Pennymatch acts as processor of the data the Tenant integrates and as controller of account credentials. Processing is based on contract performance (Art. 6(1)(b) GDPR) and, where required, legitimate interests in securing the platform (Art. 6(1)(f))."),
            new("Data categories and retention",
                "We store identity (email, preferred language), tenant billing meters, OAuth tokens encrypted at rest with AES-256-GCM, inbound webhook payloads, journal drafts and immutable accounting audit logs. Append-only accounting audit logs are retained for six (6) years from period-end or tenant offboarding, under Spanish Commercial Code art. 30, unless a longer statutory period applies."),
            new("Rights",
                $"Data subjects may request access, rectification, erasure, restriction, portability and objection by writing to {OperatorPrivacyEmail}. Supervisory authority complaints may be lodged with the AEPD (Spain) or the authority of the data subject's habitual residence."),
            new("Processor role (Art. 28 GDPR)",
                "For tenant ledger, webhook and quota data, Pennymatch acts as processor. Authorised subprocessors include PostgreSQL, Redis/Garnet and Hangzhou DeepSeek Artificial Intelligence Co., Ltd. (DeepSeek) for the Penny assistant (latent while IsPennyAiEnabled is false). Lemon Squeezy, LLC and Paddle Payments Ltd. act as Merchant of Record (independent controllers) for the B2B SaaS fee. A material change of subprocessors will be notified in-product. Full list: /subprocessors."),
            new("Technical middleware disclaimer", AccountingDisclaimerEn)
        ]
        :
        [
            new("Responsable y base jurídica",
                "Naturaleza del Servicio: Pennymatch opera como un middleware técnico puro en el ámbito B2B que transforma eventos brutos de Stripe, Shopify y Wise en borradores de asientos PGC inmutables (partida doble), exportando archivos CSV universales compatibles con Xero, QuickBooks y Holded. Las plataformas de origen pertenecen al ecosistema del Tenant; Pennymatch actúa como encargado de los datos que el Tenant integra y como responsable de las credenciales de cuenta. Base: ejecución del contrato (art. 6.1.b RGPD) y, cuando proceda, interés legítimo de proteger la plataforma (art. 6.1.f)."),
            new("Categorías y conservación",
                "Almacenamos identidad (email, idioma preferido), contadores de facturación, tokens OAuth cifrados en reposo con AES-256-GCM, payloads de webhooks, asientos en borrador y registros de auditoría contable inmutables. Los registros de auditoría contable son de solo inserción y se conservan seis (6) años desde el cierre del ejercicio o la baja del inquilino, conforme al art. 30 del Código de Comercio, salvo plazo legal superior."),
            new("Derechos",
                $"Los interesados pueden solicitar acceso, rectificación, supresión, limitación, portabilidad y oposición en {OperatorPrivacyEmail}. Pueden reclamar ante la AEPD o la autoridad de su residencia habitual."),
            new("Encargo de tratamiento (art. 28 RGPD)",
                "Sobre el libro del inquilino, los webhooks y las cuotas, Pennymatch actúa como Encargado del Tratamiento. Subencargados autorizados: PostgreSQL, Redis/Garnet y Hangzhou DeepSeek Artificial Intelligence Co., Ltd. (DeepSeek) para el asistente Penny (latente mientras IsPennyAiEnabled sea falso). Lemon Squeezy, LLC y Paddle Payments Ltd. actúan como Merchant of Record (responsables independientes) para la cuota del SaaS B2B. Un cambio material de subencargados se notificará en el producto. Lista: /subencargados."),
            new("Exención de responsabilidad del middleware técnico", AccountingDisclaimerEs)
        ];

    public static IReadOnlyList<LegalSection> Terms(UiCulture culture) => culture.Code == "en"
        ?
        [
            new("1. Object of the service",
                "Nature of the Service: Pennymatch operates as pure technical middleware in the B2B domain that transforms raw Stripe, Shopify and Wise events into immutable PGC double-entry journal drafts, exporting universal CSV files compatible with Xero, QuickBooks and Holded. Source platforms belong exclusively to the Tenant; Pennymatch acts solely as a processor of the data the Tenant integrates."),
            new("2. Intellectual property",
                "All software, trademarks, documentation, UI, connectors and know-how of Pennymatch remain the exclusive property of Pennymatch or its licensors. The subscription grants a non-exclusive, non-transferable, tenant-scoped right to use the service during the paid or trial term. Reverse engineering, scraping, resale of access or removal of proprietary notices is prohibited."),
            new("3. Data / GDPR",
                $"Pennymatch acts as processor of tenant ledger data and as controller of account credentials. Processing is based on contract performance (Art. 6(1)(b) GDPR) and, where required, legitimate interests in securing the platform (Art. 6(1)(f)). Data subjects may exercise rights via {OperatorPrivacyEmail}."),
            new("4. Usage limits",
                "Each plan has a monthly posted-transaction ceiling (Growth: 1,000; Pro: 5,000; Enterprise: 20,000 finite). A 10% grace margin applies; exceeding the absolute lock freezes further billable ingest until the next UTC billing cycle or a SuperAdmin unlock."),
            new("5. Technical middleware disclaimer", AccountingDisclaimerEn),
            new("6. Penny AI fair use",
                "If IsPennyAiEnabled is on, each tenant has a hard cap of 1,000 queries per UTC month. Excess queries are refused (HTTP 429). The assistant runs on DeepSeek — not OpenAI/GPT."),
            new("7. Governing law",
                "These terms are governed by the laws of Spain. Subject to mandatory venues, the courts of Ourense shall have exclusive jurisdiction over B2B disputes.")
        ]
        :
        [
            new("1. Objeto del servicio",
                "Naturaleza del Servicio: Pennymatch opera como un middleware técnico puro en el ámbito B2B que transforma eventos brutos de Stripe, Shopify y Wise en borradores de asientos PGC inmutables (partida doble), exportando archivos CSV universales compatibles con Xero, QuickBooks y Holded. Las plataformas de origen pertenecen exclusivamente al Tenant; Pennymatch actúa únicamente como procesador de los datos que este integra."),
            new("2. Propiedad intelectual",
                "El software, las marcas, la documentación, la interfaz, los conectores y el know-how de Pennymatch son propiedad exclusiva de Pennymatch o de sus licenciantes. La suscripción otorga un derecho no exclusivo, intransferible y limitado al inquilino. Quedan prohibidos la ingeniería inversa, el scraping y la reventa de acceso."),
            new("3. Datos / RGPD",
                $"Pennymatch actúa como encargado del tratamiento de los datos del libro del inquilino y como responsable de las credenciales de cuenta. Los interesados pueden ejercer derechos en {OperatorPrivacyEmail}."),
            new("4. Límites de uso",
                "Cada plan tiene un techo mensual de transacciones publicadas (Growth: 1.000; Pro: 5.000; Enterprise: 20.000 finito). Un margen de gracia del 10% aplica; superar el bloqueo absoluto congela la ingestión facturable hasta el siguiente ciclo UTC o un desbloqueo SuperAdmin."),
            new("5. Exención de responsabilidad del middleware técnico", AccountingDisclaimerEs),
            new("6. Uso justo de Penny IA",
                "Si IsPennyAiEnabled está activo, cada inquilino dispone de un máximo de 1.000 consultas al mes (UTC). Superado el tope, el backend denegará nuevas consultas (HTTP 429). El asistente opera con DeepSeek — no OpenAI/GPT."),
            new("7. Ley aplicable",
                "Estos términos se rigen por la legislación española. Sin perjuicio de fueros imperativos, los tribunales de Ourense serán competentes de forma exclusiva para los litigios B2B.")
        ];

    public static IReadOnlyList<LegalSection> Cookies(UiCulture culture) => culture.Code == "en"
        ?
        [
            new("1. No advertising or analytics cookies",
                "This marketing site does not use Hotjar, Google Analytics or advertising cookies. Product checkout scripts (Lemon Squeezy / Paddle) are not injected on this site at boot."),
            new("2. Strictly necessary storage",
                "Strictly necessary: the ASP.NET Core culture cookie (.AspNetCore.Culture) to remember ESP/ENG, and Blazor Server circuit / antiforgery cookies required to render interactive components (e.g. the transactional simulator). Exempt from a commercial consent banner (LSSI art. 22.2) for these technical cookies."),
            new("3. Third-party fonts",
                "Typography is loaded from Google Fonts (fonts.googleapis.com / fonts.gstatic.com) for display and UI typefaces. No advertising profile is created by Pennymatch from that request."),
            new("4. Product payment overlay",
                "If you start a paid plan inside the product app, Lemon Squeezy or Paddle may set cookies on their own domains after checkout is clicked. Those cookies stay in the payment overlay and are not marketing analytics.")
        ]
        :
        [
            new("1. Sin cookies de publicidad ni analítica",
                "Este sitio de marketing no usa Hotjar, Google Analytics ni cookies publicitarias. Los scripts de checkout del producto (Lemon Squeezy / Paddle) no se inyectan al arrancar este sitio."),
            new("2. Almacenamiento estrictamente necesario",
                "Estrictamente necesario: la cookie de cultura ASP.NET Core (.AspNetCore.Culture) para recordar ESP/ENG, y cookies de circuito Blazor Server / antiforgery necesarias para componentes interactivos (p. ej. el simulador). Exento de banner comercial (art. 22.2 LSSI) para estas cookies técnicas."),
            new("3. Tipografías de tercero",
                "La tipografía se carga desde Google Fonts (fonts.googleapis.com / fonts.gstatic.com). Pennymatch no crea un perfil publicitario a partir de esa petición."),
            new("4. Overlay de pago del producto",
                "Si inicia un plan de pago dentro de la app de producto, Lemon Squeezy o Paddle pueden fijar cookies en su propio dominio tras pulsar checkout. Quedan encapsuladas en la pasarela; no son analítica de marketing.")
        ];

    public static string SubprocessorsIntro(UiCulture culture) => culture.Code == "en"
        ? "Subprocessors: PostgreSQL and Redis/Garnet act under Art. 28(2) GDPR. Hangzhou DeepSeek Artificial Intelligence Co., Ltd. (DeepSeek) powers the Penny assistant when enabled. Lemon Squeezy, LLC / Paddle Payments Ltd. are Merchant of Record (independent controllers) for the B2B SaaS fee — not mere collection subprocessors. No OpenAI/GPT in the authorised subprocessor list."
        : "Subencargados: PostgreSQL y Redis/Garnet actúan bajo el Art. 28.2 RGPD. Hangzhou DeepSeek Artificial Intelligence Co., Ltd. (DeepSeek) potencia el asistente Penny cuando está activo. Lemon Squeezy, LLC / Paddle Payments Ltd. son Merchant of Record (responsables independientes) de la cuota SaaS B2B — no meros subencargados de cobro. No figura OpenAI/GPT en la lista autorizada.";

    public static IReadOnlyList<SubprocessorEntry> Subprocessors(UiCulture culture) => culture.Code == "en"
        ?
        [
            new(
                "Cloud infrastructure (PostgreSQL)",
                "Storage of the accounting pipeline and the immutable log history.",
                "Spain / European Union (Art. 32 GDPR safeguard)."),
            new(
                "High-speed cache (Redis/Garnet)",
                "Homogeneous control of the perimeter middleware and tenant quotas in O(1).",
                "Servers located in the European Economic Area (EEA)."),
            new(
                "Lemon Squeezy, LLC / Paddle Payments Ltd.",
                "Merchant of Record (independent controllers) for B2B SaaS subscription billing in the EEA. Not mere Art. 28(2) collection subprocessors.",
                "European Economic Area (EEA)."),
            new(
                "Hangzhou DeepSeek Artificial Intelligence Co., Ltd. (DeepSeek)",
                "Processing of Penny assistant prompts when IsPennyAiEnabled is true. International transfer outside the EEA under Standard Contractual Clauses (Commission Implementing Decision (EU) 2021/914). Prompts are not used to train DeepSeek models under Pennymatch’s integration settings. No processing while the flag is false.",
                "People’s Republic of China (SCC / Decision 2021/914). Latent until Penny AI is enabled.")
        ]
        :
        [
            new(
                "Proveedor de Infraestructura Cloud (PostgreSQL)",
                "Almacenamiento del pipeline contable e histórico inmutable de logs.",
                "España / Unión Europea (garantía Art. 32 RGPD)."),
            new(
                "Infraestructura de Caché de Alta Velocidad (Redis/Garnet)",
                "Control homogéneo del middleware perimetral y cuotas de Tenant en O(1).",
                "Servidores en el Espacio Económico Europeo (EEE)."),
            new(
                "Lemon Squeezy, LLC / Paddle Payments Ltd.",
                "Merchant of Record (responsables independientes) para la cuota SaaS B2B en el EEE. No son meros subencargados del Art. 28.2 respecto del cobro.",
                "Espacio Económico Europeo (EEE)."),
            new(
                "Hangzhou DeepSeek Artificial Intelligence Co., Ltd. (DeepSeek)",
                "Tratamiento de prompts del asistente Penny cuando IsPennyAiEnabled es verdadero. Transferencia fuera del EEE amparada en Cláusulas Contractuales Tipo (Decisión de Ejecución (UE) 2021/914). Los prompts no se usan para entrenar modelos de DeepSeek bajo la integración de Pennymatch. Sin tratamiento mientras el flag esté apagado.",
                "República Popular China (CCT / Decisión 2021/914). Fila latente mientras Penny IA esté apagada.")
        ];
}

internal readonly record struct LegalSection(string Heading, string Body);

internal readonly record struct SubprocessorEntry(string Name, string Function, string Location);
