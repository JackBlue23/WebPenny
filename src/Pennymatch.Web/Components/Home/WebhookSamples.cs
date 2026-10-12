namespace Pennymatch.Web.Components.Home;

internal static class WebhookSamples
{
    public const string PaymentSucceeded = """
        {
          "id": "evt_9f2c",
          "type": "payment.succeeded",
          "created_at": "2026-04-01T10:14:02Z",
          "data": {
            "payment_id": "pay_441",
            "amount": 1299,
            "currency": "EUR",
            "tenant_id": "tn_acme",
            "ledger_entry": "je_88af"
          }
        }
        """;
}
