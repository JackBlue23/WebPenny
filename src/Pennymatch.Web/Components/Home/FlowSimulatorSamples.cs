using Pennymatch.Web.Localization;

namespace Pennymatch.Web.Components.Home;

/// <summary>AOT-safe static webhook samples. No reflection — pick ES/EN by <see cref="UiCulture"/>.</summary>
internal static class FlowSimulatorSamples
{
    internal sealed record SourceDef(
        string Id,
        string Name,
        string EventType,
        string Tenant,
        string RawJson,
        string Amount,
        string Currency);

    /// <summary>CSV / ERP destination (kept as MorDef shape so FlowSimulator UX stays identical).</summary>
    internal sealed record MorDef(string Id, string Name, string ProductKey);

    // --- Stripe raw payloads (culture-specific) ---
    private const string StripeRawEs =
        """
        {
          "id": "evt_stripe_441",
          "type": "charge.succeeded",
          "data": {
            "object": {
              "id": "ch_1Px",
              "amount": 49900,
              "currency": "eur",
              "balance_transaction": {
                "fee": 145,
                "net": 49755,
                "currency": "eur"
              }
            }
          }
        }
        """;

    private const string StripeRawEn =
        """
        {
          "id": "evt_stripe_982",
          "type": "charge.succeeded",
          "data": {
            "object": {
              "id": "ch_1Qz",
              "amount": 11500,
              "currency": "usd",
              "balance_transaction": {
                "fee": 63,
                "net": 11437,
                "currency": "usd",
                "exchange_rate": 0.9214
              }
            }
          }
        }
        """;

    private const string ShopifyRawEs =
        """
        {
          "id": 5829103341,
          "name": "#1042",
          "total_price": "129.00",
          "total_tax": "22.41",
          "currency": "EUR",
          "financial_status": "paid",
          "email": "ops@north.example"
        }
        """;

    private const string ShopifyRawEn =
        """
        {
          "id": 5829109988,
          "name": "#2042",
          "total_price": "129.00",
          "total_tax": "0.00",
          "currency": "USD",
          "financial_status": "paid",
          "email": "ops@global.example"
        }
        """;

    private const string WiseRawEs =
        """
        {
          "data": {
            "resource": {
              "id": 884422,
              "type": "transfer",
              "status": "outgoing_payment_sent"
            },
            "current_state": "outgoing_payment_sent",
            "amount": 2500.00,
            "currency": "EUR"
          }
        }
        """;

    private const string WiseRawEn =
        """
        {
          "data": {
            "resource": {
              "id": 991133,
              "type": "transfer",
              "status": "outgoing_payment_sent"
            },
            "current_state": "outgoing_payment_sent",
            "amount": 2500.00,
            "currency": "USD"
          }
        }
        """;

    private static readonly SourceDef[] SourcesEs =
    [
        new("stripe", "Stripe", "charge.succeeded", "tn_acme", StripeRawEs, "499.00", "EUR"),
        new("shopify", "Shopify", "orders/paid", "tn_north", ShopifyRawEs, "129.00", "EUR"),
        new("wise", "Wise", "transfers#state-change", "tn_orbit", WiseRawEs, "2500.00", "EUR")
    ];

    private static readonly SourceDef[] SourcesEn =
    [
        new("stripe", "Stripe", "charge.succeeded", "tn_global", StripeRawEn, "115.00", "USD"),
        new("shopify", "Shopify", "orders/paid", "tn_global", ShopifyRawEn, "129.00", "USD"),
        new("wise", "Wise", "transfers#state-change", "tn_global", WiseRawEn, "2500.00", "USD")
    ];

    /// <summary>Third-column toggles: ERP CSV destinations (CSS class names stay flow-mor*).</summary>
    internal static readonly MorDef[] Mors =
    [
        new("holded", "Holded", "pennymatch_report.csv"),
        new("xero", "Xero", "pennymatch-xero-journals.csv"),
        new("quickbooks", "QuickBooks", "pennymatch-quickbooks-journals.csv")
    ];

    internal static SourceDef[] GetSources(UiCulture culture) =>
        culture.Code == "en" ? SourcesEn : SourcesEs;

    internal static string BuildPgcDraftJson(
        SourceDef source,
        MorDef destination,
        string? fingerprint,
        UiCulture culture)
    {
        var en = culture.Code == "en";
        var lines = source.Id switch
        {
            "shopify" when source.Currency == "EUR" =>
                """
                  {"account":"430","side":"D","amount":"129.00"},
                  {"account":"705","side":"H","amount":"106.59"},
                  {"account":"477","side":"H","amount":"22.41","note":"VAT from Shopify source"}
                """,
            "shopify" =>
                """
                  {"account":"430","side":"D","amount":"129.00"},
                  {"account":"705","side":"H","amount":"129.00"}
                """,
            "wise" =>
                """
                  {"account":"573","side":"D","amount":"2500.00"},
                  {"account":"572","side":"H","amount":"2500.00"}
                """,
            _ when source.Currency == "USD" =>
                """
                  {"account":"572","side":"D","amount":"105.38"},
                  {"account":"626","side":"D","amount":"0.58"},
                  {"account":"705","side":"H","amount":"105.96"},
                  {"account":"fx_rate":"0.9214","source":"BCE","tolerance_eur":"0.01"}
                """,
            _ =>
                """
                  {"account":"572","side":"D","amount":"497.55"},
                  {"account":"626","side":"D","amount":"1.45"},
                  {"account":"705","side":"H","amount":"499.00"}
                """
        };

        var note = en
            ? "Not an ERP export of official books; universal CSV for your accountant"
            : "No es el libro oficial; CSV universal para tu gestor";

        return
            "{\n" +
            "  \"journal\": \"PGC/Draft\",\n" +
            "  \"status\": \"Ready\",\n" +
            "  \"source\": \"" + source.Id + "\",\n" +
            "  \"tenant_id\": \"" + source.Tenant + "\",\n" +
            "  \"base_currency\": \"EUR\",\n" +
            "  \"source_amount\": \"" + source.Amount + "\",\n" +
            "  \"source_currency\": \"" + source.Currency + "\",\n" +
            "  \"fx_tolerance_eur\": \"0.01\",\n" +
            "  \"fingerprint\": \"" + fingerprint + "\",\n" +
            "  \"csv_target\": \"" + destination.Id + "\",\n" +
            "  \"csv_file\": \"" + destination.ProductKey + "\",\n" +
            "  \"lines\": [\n" + lines + "\n  ],\n" +
            "  \"immutable_when\": \"Posted\",\n" +
            "  \"cache\": \"hot/O(1)\",\n" +
            "  \"note\": \"" + note + "\"\n" +
            "}";
    }
}
