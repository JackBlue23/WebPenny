using System.Globalization;
using Pennymatch.Web.Localization;

namespace Pennymatch.Web.Services;

internal static class DocsArticleBodies
{
    public static string Render(string id)
    {
        var en = UiCulture.From(CultureInfo.CurrentUICulture).Code == "en";
        return id switch
        {
            "quickstart" => en ? QuickEn : QuickEs,
            "webhooks-hmac" => en ? WebhooksEn : WebhooksEs,
            "pgc-engine" => en ? PgcEn : PgcEs,
            "fx-bce" => en ? FxEn : FxEs,
            "erp-csv" => en ? ErpEn : ErpEs,
            "scope-limits" or "no-erp" => en ? ScopeEn : ScopeEs,
            "security-billing" or "consumption-cache" or "mor-delivery" => en ? SecEn : SecEs,
            "plan-mapping" => en ? PlanEn : PlanEs,
            "idempotency" => en ? IdemEn : IdemEs,
            _ => en
                ? "<p>Content in preparation.</p>"
                : "<p>Contenido en preparación.</p>"
        };
    }

    private const string QuickEs = """
        <h2>Objetivo</h2>
        <p>En minutos: cuenta de prueba → fuente conectada → primer asiento <strong>Ready</strong> → CSV hacia Holded, Xero o QuickBooks.</p>
        <h2>Pasos</h2>
        <ol>
          <li>Regístrese en el producto Pennymatch (rol <strong>Admin</strong>).</li>
          <li>En <strong>Conexiones</strong>, configure el webhook HMAC de Stripe y/o Shopify; mapee Wise si liquida por ahí.</li>
          <li>En <strong>Mapeo de cuentas</strong>, asigne el catálogo PGC (430, 572, 626, 705, 477, 668, 768, 129…).</li>
          <li>Espere un evento real o de laboratorio; revise la ficha en <strong>Asientos</strong> (Draft → Ready).</li>
          <li>Exporte <code>pennymatch_report.csv</code>, Xero o QuickBooks e impórtelo en su ERP (véase el manual CSV).</li>
        </ol>
        <h2>Planes</h2>
        <p>Growth 1.000 tx/mes · Pro 5.000 · Enterprise 20.000 (techo finito). Alerta preventiva al 90&nbsp;%; gracia +10&nbsp;% antes del bloqueo duro. Facturación SaaS vía <strong>Lemon Squeezy</strong> (MoR activo; Paddle Billing v2 en contingencia).</p>
        <h2>Qué NO incluye</h2>
        <p>Pennymatch <strong>no es software de facturación</strong>: no emite facturas ni presenta modelos fiscales, y no sustituye Holded, Xero ni QuickBooks.</p>
        """;

    private const string QuickEn = """
        <h2>Goal</h2>
        <p>In minutes: trial account → connected source → first <strong>Ready</strong> journal → CSV into Holded, Xero, or QuickBooks.</p>
        <h2>Steps</h2>
        <ol>
          <li>Sign up in the Pennymatch product (<strong>Admin</strong> role).</li>
          <li>Under <strong>Connections</strong>, configure Stripe and/or Shopify HMAC webhooks; map Wise if you settle there.</li>
          <li>Under <strong>Account mapping</strong>, assign the PGC chart (430, 572, 626, 705, 477, 668, 768, 129…).</li>
          <li>Wait for a live or lab event; review the card under <strong>Journals</strong> (Draft → Ready).</li>
          <li>Export <code>pennymatch_report.csv</code>, Xero, or QuickBooks and import into your ERP (see the CSV manual).</li>
        </ol>
        <h2>Plans</h2>
        <p>Growth 1,000 tx/month · Pro 5,000 · Enterprise 20,000 (finite ceiling). Preventive alert at 90%; +10% grace before hard block. SaaS billing via <strong>Lemon Squeezy</strong> (active MoR; Paddle Billing v2 as contingency).</p>
        <h2>What it does not include</h2>
        <p>Pennymatch is <strong>not invoicing software</strong>: it does not issue invoices or file tax returns, and it does not replace Xero, QuickBooks, or Holded.</p>
        """;

    private const string WebhooksEs = """
        <h2>Objetivo</h2>
        <p>Recibir eventos brutos de Stripe, Shopify y Wise con verificación HMAC e idempotencia, y materializar borradores PGC — no parsers ad hoc ni spaghetti por proveedor.</p>
        <h2>Flujo recomendado</h2>
        <ol>
          <li>Configure el endpoint de webhook en el panel de Stripe/Shopify (no hay OAuth de Stripe/Shopify en Pennymatch: es HMAC → PGC).</li>
          <li>Verifique la firma HMAC con el secreto rotado.</li>
          <li>El payload de origen alimenta el mapper; el contrato estable es el asiento (cuentas Debe/Haber + correlación SHA-256).</li>
          <li>Rechace duplicados con la huella del evento normalizado.</li>
          <li>Tras 3 descuadres consecutivos (&gt; 0,01 €) el circuit breaker pausa la ingesta hasta reactivación Admin.</li>
        </ol>
        <h2>Wise y MoR de ventas</h2>
        <p>Wise aporta liquidaciones; Lemon Squeezy puede asentar ventas bajo comerciante de registro del <em>tenant</em>. No confunda eso con pagar el plan Growth/Pro/Enterprise de Pennymatch (también Lemon Squeezy / Paddle como MoR SaaS).</p>
        <h2>Qué no hacer</h2>
        <p>No escriba aquí lógica de emisión de facturas ni de modelos fiscales. El destino del evento es el libro PGC interno y el CSV hacia su ERP.</p>
        """;

    private const string WebhooksEn = """
        <h2>Goal</h2>
        <p>Receive raw Stripe, Shopify, and Wise events with HMAC verification and idempotency, then materialize PGC drafts — no per-provider ad-hoc parsers.</p>
        <h2>Recommended flow</h2>
        <ol>
          <li>Configure the webhook endpoint in the Stripe/Shopify dashboard (there is no Stripe/Shopify OAuth in Pennymatch: it is HMAC → PGC).</li>
          <li>Verify the HMAC signature with the rotated secret.</li>
          <li>The raw payload feeds the mapper; the stable contract is the journal (debit/credit accounts + SHA-256 correlation).</li>
          <li>Reject duplicates with the fingerprint of the normalized event.</li>
          <li>After 3 consecutive imbalances (&gt; 0.01 EUR) the circuit breaker pauses ingest until an Admin reactivates it.</li>
        </ol>
        <h2>Wise and sales MoR</h2>
        <p>Wise contributes settlements; Lemon Squeezy can post sales under the tenant’s merchant-of-record. Do not confuse that with paying Pennymatch Growth/Pro/Enterprise (also Lemon Squeezy / Paddle as SaaS MoR).</p>
        <h2>What not to do</h2>
        <p>Do not put invoice issuance or tax-return logic here. The event’s destination is the internal PGC ledger and CSV into your ERP.</p>
        """;

    private const string PgcEs = """
        <h2>Partida doble inmutable</h2>
        <p>Cada hecho genera líneas Debe/Haber en moneda base (normalmente EUR). |debe − haber| ≤ <strong>0,01 €</strong> o el asiento no cuadra. Residuos ≤ 0,01 € pueden derivarse a <strong>668</strong> (diferencias negativas) o <strong>768</strong> (positivas). Por encima: descuadre estructural.</p>
        <h2>Cuentas clave</h2>
        <table>
          <thead><tr><th>Código</th><th>Uso</th></tr></thead>
          <tbody>
            <tr><td>430 / 572 / 573</td><td>Clientes / bancos / bancos moneda extranjera</td></tr>
            <tr><td>626 / 627</td><td>Comisiones de pasarela</td></tr>
            <tr><td>705 / 708</td><td>Ventas / devoluciones</td></tr>
            <tr><td><strong>477</strong></td><td>Impuesto repercutido cuando el origen (p. ej. Shopify) trae tax — mapeo contable, no liquidación fiscal</td></tr>
            <tr><td><strong>668 / 768</strong></td><td>Diferencias negativas / positivas de cambio</td></tr>
            <tr><td><strong>129</strong></td><td>Resultado del ejercicio (cierre FX trimestral)</td></tr>
          </tbody>
        </table>
        <h2>Estados</h2>
        <p><strong>Draft</strong> → <strong>Ready</strong> → <strong>Posted</strong>. Posted es sagrado: no se reescribe. Un error se corrige con un hecho nuevo (reembolso, asiento de cierre).</p>
        """;

    private const string PgcEn = """
        <h2>Immutable double-entry</h2>
        <p>Each fact produces debit/credit lines in base currency (usually EUR). |debit − credit| ≤ <strong>0.01 EUR</strong> or the journal does not balance. Residuals ≤ 0.01 EUR may route to <strong>668</strong> (negative FX) or <strong>768</strong> (positive). Above that: structural imbalance.</p>
        <h2>Key accounts</h2>
        <table>
          <thead><tr><th>Code</th><th>Use</th></tr></thead>
          <tbody>
            <tr><td>430 / 572 / 573</td><td>Customers / banks / foreign-currency banks</td></tr>
            <tr><td>626 / 627</td><td>Gateway fees</td></tr>
            <tr><td>705 / 708</td><td>Sales / returns</td></tr>
            <tr><td><strong>477</strong></td><td>Output tax when the source (e.g. Shopify) sends it — ledger mapping, not tax filing</td></tr>
            <tr><td><strong>668 / 768</strong></td><td>Negative / positive exchange differences</td></tr>
            <tr><td><strong>129</strong></td><td>Profit for the year (quarterly FX close)</td></tr>
          </tbody>
        </table>
        <h2>Statuses</h2>
        <p><strong>Draft</strong> → <strong>Ready</strong> → <strong>Posted</strong>. Posted is sacred: never rewritten. Fix errors with a new fact (refund, closing entry).</p>
        """;

    private const string FxEs = """
        <h2>Estrategia BCE en tres capas</h2>
        <table>
          <thead><tr><th>Capa</th><th>Dónde</th><th>¿Escribe Posted?</th><th>Para qué</th></tr></thead>
          <tbody>
            <tr><td>1. Consulta</td><td>Panel · saldos multi-divisa</td><td>No</td><td>Proyección analítica sobre tasas BCE</td></tr>
            <tr><td>2. Asiento del día</td><td>Ficha / moneda origen</td><td>Sí (Draft/Ready/Posted)</td><td>Cobro con tipo a 4 decimales</td></tr>
            <tr><td>3. Cierre trimestre</td><td>Cierre FX</td><td>Sí · lote inmutable 668/768/129</td><td>Diferencia que el día a día no cerró</td></tr>
          </tbody>
        </table>
        <h2>Contingencia</h2>
        <p>Feed BCE en vivo, caché Redis/local y fallback en arranque en frío. Sin cotización usable, un residual &gt; 0,01 € sigue siendo descuadre — no se inventa el tipo.</p>
        <h2>Regla de oro</h2>
        <p>Si el widget del Panel y el asiento Posted discrepan, <strong>gana el asiento</strong>.</p>
        """;

    private const string FxEn = """
        <h2>Three-layer ECB strategy</h2>
        <table>
          <thead><tr><th>Layer</th><th>Where</th><th>Writes Posted?</th><th>Purpose</th></tr></thead>
          <tbody>
            <tr><td>1. Query</td><td>Dashboard · multi-currency balances</td><td>No</td><td>Analytics projection on ECB rates</td></tr>
            <tr><td>2. Day journal</td><td>Card / source currency</td><td>Yes (Draft/Ready/Posted)</td><td>Charge with rate to 4 decimals</td></tr>
            <tr><td>3. Quarter close</td><td>FX close</td><td>Yes · immutable 668/768/129 batch</td><td>Difference the day book did not close</td></tr>
          </tbody>
        </table>
        <h2>Contingency</h2>
        <p>Live ECB feed, Redis/local cache, and cold-start fallback. Without a usable quote, a residual &gt; 0.01 remains an imbalance — the rate is never invented.</p>
        <h2>Golden rule</h2>
        <p>If the dashboard widget and the Posted journal disagree, <strong>the journal wins</strong>.</p>
        """;

    private const string ErpEs = """
        <h2>Tres botones de exportación</h2>
        <ol>
          <li><strong>Exportar CSV</strong> → <code>pennymatch_report.csv</code> (universal; Holded y gestores externos).</li>
          <li><strong>Xero</strong> → <code>pennymatch-xero-journals.csv</code>.</li>
          <li><strong>QuickBooks</strong> → <code>pennymatch-quickbooks-journals.csv</code>.</li>
        </ol>
        <h2>Importar en Holded</h2>
        <ol>
          <li>Descargue el CSV universal del rango de fechas.</li>
          <li>En Holded, use la importación de asientos / diario compatible con columnas Fecha, Cuenta, Debe, Haber, Descripción.</li>
          <li>Valide que |debe − haber| cuadra por asiento antes de cerrar el periodo en Holded.</li>
        </ol>
        <h2>Xero / QuickBooks Online</h2>
        <p>Opcional: OAuth en <strong>Conexiones</strong> para publicar fichas Ready. El CSV sigue siendo la red de seguridad si no conecta OAuth. Un segundo publish de un Posted se rechaza (inmutabilidad).</p>
        <h2>Importante</h2>
        <p>El CSV <strong>no</strong> es una declaración fiscal ni el libro oficial. Es un fichero de trabajo para su ERP / gestor (Holded, Xero o QuickBooks).</p>
        """;

    private const string ErpEn = """
        <h2>Three export buttons</h2>
        <ol>
          <li><strong>Export CSV</strong> → <code>pennymatch_report.csv</code> (universal; Holded and external advisors).</li>
          <li><strong>Xero</strong> → <code>pennymatch-xero-journals.csv</code>.</li>
          <li><strong>QuickBooks</strong> → <code>pennymatch-quickbooks-journals.csv</code>.</li>
        </ol>
        <h2>Import into Holded</h2>
        <ol>
          <li>Download the universal CSV for the date range.</li>
          <li>In Holded, use the journal import compatible with Date, Account, Debit, Credit, Description columns.</li>
          <li>Validate |debit − credit| balances per journal before closing the period in Holded.</li>
        </ol>
        <h2>Xero / QuickBooks Online</h2>
        <p>Optional: OAuth under <strong>Connections</strong> to publish Ready cards. CSV remains the safety net without OAuth. A second publish of a Posted journal is rejected (immutability).</p>
        <h2>Important</h2>
        <p>The CSV is <strong>not</strong> a tax filing or the official books. It is a working file for your ERP / advisor (Xero, QuickBooks, or Holded).</p>
        """;

    private const string ScopeEs = """
        <h2>Qué sí es</h2>
        <p>Middleware técnico B2B de alto rendimiento, country-agnostic: Stripe / Shopify / Wise → asientos PGC inmutables (668 / 768 / 129 / 477) → CSV universales para Xero, QuickBooks y Holded. Multi-divisa al céntimo, BCE en 3 capas, O(1), Native AOT, idempotencia atómica, auditoría SHA-256.</p>
        <h2>Qué NO incluye</h2>
        <ul>
          <li><strong>No es un ERP</strong> — no sustituye Holded, Xero, QuickBooks, Sage ni A3.</li>
          <li><strong>No es software de facturación</strong> — no emite facturas a clientes finales.</li>
          <li><strong>No presenta modelos fiscales</strong> por usted. El impuesto de Shopify se mapea a cuenta <strong>477</strong> cuando viene en el origen (mapeo contable).</li>
          <li><strong>No es la pasarela</strong> ni un banco. Lemon Squeezy / Paddle cobran la cuota SaaS (MoR independientes); el dinero de su tienda lo mueve Stripe/Shopify/Wise/MoR de ventas.</li>
          <li><strong>No usa OpenAI</strong> en el copy legal de subencargados: el asistente Penny declara DeepSeek (latente mientras el flag esté apagado).</li>
        </ul>
        <h2>Planes (techos)</h2>
        <p>Growth 1.000 · Pro 5.000 · Enterprise 20.000 transacciones/mes. Nunca un plan «ilimitado» opaco.</p>
        """;

    private const string ScopeEn = """
        <h2>What it is</h2>
        <p>High-performance, country-agnostic B2B technical middleware: Stripe / Shopify / Wise → immutable PGC double-entry journals (668 / 768 / 129 / 477) → universal CSVs for Xero, QuickBooks, and Holded. Multi-currency to the cent, three-layer ECB FX, O(1), Native AOT, atomic idempotency (no duplicate entries), SHA-256 audit.</p>
        <h2>What it is not</h2>
        <ul>
          <li><strong>Not an ERP</strong> — does not replace Xero, QuickBooks, Holded, Sage, or A3.</li>
          <li><strong>Not invoicing software</strong> — does not issue invoices to end customers.</li>
          <li><strong>Does not file tax returns</strong> for you. Shopify tax maps to account <strong>477</strong> when present on the source (ledger mapping only).</li>
          <li><strong>Not the payment gateway</strong> or a bank. Lemon Squeezy / Paddle collect the SaaS fee (independent MoRs); your store’s money moves via Stripe/Shopify/Wise/sales MoR.</li>
          <li><strong>No OpenAI</strong> in legal subprocessor copy: the Penny assistant declares DeepSeek (latent while the flag is off).</li>
        </ul>
        <h2>Plans (ceilings)</h2>
        <p>Growth 1,000 · Pro 5,000 · Enterprise 20,000 transactions/month. Never an opaque “unlimited” plan.</p>
        """;

    private const string SecEs = """
        <h2>Perímetro</h2>
        <ul>
          <li>Aislamiento multi-tenant con filtros de consulta; Posted inmutable.</li>
          <li>Idempotencia atómica y huellas SHA-256 por evento / cadena de auditoría.</li>
          <li>Escudo JSON de borde (cuerpo &gt; 256 KB → 413; profundidad &gt; 4 → 400).</li>
          <li>Cliente Native AOT (trim completo, strip IL tras AOT).</li>
          <li>Cifrado y MFA en superficie de seguridad del producto.</li>
        </ul>
        <h2>Privacidad y subencargados</h2>
        <p>PostgreSQL y Redis/Garnet como subencargados. <strong>DeepSeek</strong> para Penny (latente si <code>IsPennyAiEnabled</code> es falso). <strong>Cero OpenAI</strong> en el aviso de subencargados. Lemon Squeezy (activo) y Paddle (contingencia) actúan como MoR independientes para la cuota SaaS B2B — no como destino del producto contable.</p>
        <h2>Hot cache O(1)</h2>
        <p>Límites de plan y uso por tenant se resuelven en caché caliente en el camino del webhook. Ideal para gatekeepers sin materializar waterfalls.</p>
        """;

    private const string SecEn = """
        <h2>Perimeter</h2>
        <ul>
          <li>Multi-tenant isolation with query filters; immutable Posted journals.</li>
          <li>Atomic idempotency and per-event / chained SHA-256 audit fingerprints.</li>
          <li>Border JSON shield (body &gt; 256 KB → 413; depth &gt; 4 → 400).</li>
          <li>Native AOT client (full trim, strip IL after AOT).</li>
          <li>Encryption and MFA on the product security surface.</li>
        </ul>
        <h2>Privacy and subprocessors</h2>
        <p>PostgreSQL and Redis/Garnet as subprocessors. <strong>DeepSeek</strong> for Penny (latent while <code>IsPennyAiEnabled</code> is false). <strong>Zero OpenAI</strong> in the subprocessor notice. Lemon Squeezy (active) and Paddle (contingency) act as independent MoRs for the B2B SaaS fee — not as the accounting product destination.</p>
        <h2>O(1) hot cache</h2>
        <p>Plan limits and per-tenant usage resolve from a hot cache on the webhook path. Ideal for gatekeepers without materializing waterfalls.</p>
        """;

    private const string PlanEs = """
        <h2>Catálogo comercial</h2>
        <table>
          <thead><tr><th>Plan</th><th>Tx/mes</th><th>MRR orientativo</th></tr></thead>
          <tbody>
            <tr><td>Growth</td><td>1.000 (gracia 1.100)</td><td>79 € / $89</td></tr>
            <tr><td>Pro</td><td>5.000</td><td>199 € / $219</td></tr>
            <tr><td>Enterprise</td><td>20.000 (finito)</td><td>399 € / $439</td></tr>
          </tbody>
        </table>
        <p>Alerta preventiva al 90&nbsp;% del techo. Checkout SaaS vía Lemon Squeezy; Paddle Billing v2 preparado como contingencia de plataforma.</p>
        """;

    private const string PlanEn = """
        <h2>Commercial catalog</h2>
        <table>
          <thead><tr><th>Plan</th><th>Tx/month</th><th>Indicative MRR</th></tr></thead>
          <tbody>
            <tr><td>Growth</td><td>1,000 (grace 1,100)</td><td>€79 / $89</td></tr>
            <tr><td>Pro</td><td>5,000</td><td>€199 / $219</td></tr>
            <tr><td>Enterprise</td><td>20,000 (finite)</td><td>€399 / $439</td></tr>
          </tbody>
        </table>
        <p>Preventive alert at 90% of ceiling. SaaS checkout via Lemon Squeezy; Paddle Billing v2 prepared as platform contingency.</p>
        """;

    private const string IdemEs = """
        <h2>Por qué SHA-256</h2>
        <p>La huella se deriva del evento normalizado (fuente + tipo + importes canónicos + versión de asiento). Dos entregas del mismo pago producen la misma clave de correlación.</p>
        <h2>.NET 9 Native AOT</h2>
        <p>Pipeline pensado para AOT: sin reflexión en el camino caliente, serialización estática. Menos superficie de ataque y arranques predecibles.</p>
        <h2>Posted</h2>
        <p>Un asiento Posted no se reescribe ni se reenvía al ERP. Un «duplicate ignored» exige comparar huellas, no forzar un reintento ciego.</p>
        """;

    private const string IdemEn = """
        <h2>Why SHA-256</h2>
        <p>The fingerprint derives from the normalized event (source + type + canonical amounts + journal version). Two deliveries of the same payment produce the same correlation key.</p>
        <h2>.NET 9 Native AOT</h2>
        <p>Pipeline designed for AOT: no reflection on the hot path, static serialization. Smaller attack surface and predictable cold starts.</p>
        <h2>Posted</h2>
        <p>A Posted journal is never rewritten or re-sent to the ERP. A “duplicate ignored” means compare fingerprints — do not force a blind retry.</p>
        """;
}
