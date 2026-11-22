---
cursor:
  subagentId: "bc-c2be057e-82ac-5c81-a245-562f329c442e"
---

# WebPenny × Pennymatch v1.0.0-rc1 cross-audit

**Date:** 2026-11-15  
**Source of truth (READ-ONLY):** `C:\Pennymatch` @ tag `v1.0.0-rc1` (`9d56c20` Code Freeze)  
**Target (MODIFY):** `C:\WebPenny`  
**Task:** PW-21

## Executive verdict

WebPenny currently sells the **wrong product story**: a “no-code bridge that delivers DTOs to Lemon Squeezy / Paddle (MoR)”.  
Real rc1 LegalCopy + manuals define Pennymatch as **pure B2B technical middleware**: Stripe / Shopify / Wise → **immutable Spanish PGC double-entry drafts** → **universal CSV** for Xero / QuickBooks / Holded.  
Lemon Squeezy (active) and Paddle Billing v2 (contingency) are **Pennymatch SaaS billing MoR**, not the tenant product destination.

**Severity:** Critical positioning drift across Hero, Simulator third column, Advantages, Docs catalog/bodies, Blog, Footer meta.

---

## 1. Pennymatch rc1 — verified facts

### 1.1 Nature of the service (LegalCopy)

From `src/Pennymatch.Client/Localization/LegalCopy.cs`:

- Middleware técnico puro B2B: eventos brutos Stripe / Shopify / Wise → borradores de asientos **PGC español inmutables** → CSV universales **Xero / QuickBooks / Holded**.
- **NO** es ERP; **NO** emite facturas art. 24; **NO** presenta liquidaciones IVA / Sociedades; **NO** asesora fiscalmente.
- Tenant = responsable del libro; Pennymatch = encargado (Art. 28).
- Subencargados legales: PostgreSQL, Redis/Garnet; DeepSeek latente para Penny (`IsPennyAiEnabled`); **Lemon Squeezy / Paddle = MoR independientes** solo para la **cuota SaaS B2B**.
- Tests `SubprocessorCopyTests` **prohiben** OpenAI en copy legal.

### 1.2 Manuals (user language)

`docs/manual-usuario/ManualUsuario_Cap01_Introduccion.md` §1.3–1.4:

- Puente contable internacional + middleware sin código.
- Destino = PGC mapeado + CSV / OAuth a Holded / Xero / QBO.
- **No** ERP, **no** facturación certificada, **no** presenta SII/AEAT por el cliente, **no** sustituye gestor.
- Posted = libro interno inmutable; export CSV = facilidad, no libros oficiales.

### 1.3 Accounting engine (Domain)

| Fact | Location |
| --- | --- |
| FX tolerance **0.01 EUR** | `AccountingRules.FxRoundingTolerance` |
| Residual FX → **668** (neg) / **768** (pos) | `PgcFxAccounts`, `JournalEntry` |
| Quarterly close vs **129** | `PgcFxAccounts.QuarterResult`, Cap. 4 |
| Shopify VAT → account **477** from source | Cap. 4 chart + `ShopifyJournalDraftIngest` |
| Circuit breaker after 3 consecutive imbalance fails | Stripe/Shopify thresholds in `AccountingRules` |
| CSV exporter Xero/QB/Holded | `UniversalJournalCsvExporter`, Cap. 5 |

### 1.4 BCE FX — three layers (Cap. 4.11)

1. **Consulta** — Panel multi-currency widget (does **not** write Posted).  
2. **Asiento del día** — source amount + rate to 4 decimals on Draft/Ready/Posted.  
3. **Cierre trimestral** — immutable Posted batch on **668 / 768 / 129** via `FxClosingEngine`.

### 1.5 Connectors

| Family | Role |
| --- | --- |
| Stripe / Shopify | HMAC ingest → PGC drafts (no OAuth on those cards) |
| Wise | Settlements mapped into PGC |
| Xero / QuickBooks Online | OAuth destination + CSV; Holded via universal CSV |
| Lemon Squeezy | (a) SaaS subscription MoR active; (b) optional MoR sales ingest for tenant MoR sales |
| Paddle | Platform contingency / Billing v2 prepared (`IsPaddleConnectorEnabled`) |

### 1.6 Plan limits (commercial)

| Plan | Max tx/month | Soft→grace | EUR MRR | USD list |
| --- | ---: | ---: | ---: | ---: |
| Growth | 1 000 | 1 100 hard | 79 € | $89 |
| Pro | 5 000 | +10% | 199 € | $219 |
| Enterprise | 20 000 (finite) | +10% | 399 € | $439 |

Preventive alert at **90%** of commercial ceiling. Quota warning settings at 80%.

### 1.7 Security / performance differentiators

- O(1) hot paths: `ExecuteUpdateAsync`, aggregations, `IAsyncEnumerable`, Detached after batch; billing hot cache.
- Atomic idempotency middleware (`IdempotencyCoordinator` / mapping).
- Immutable Posted journals; chained SHA-256 audit (`SuperAdminAuditChain`, export columns).
- Multi-tenant `HasQueryFilter`; `xmin` concurrency.
- `BorderJsonShieldMiddleware` (413 > 256 KB, depth > 4 → 400).
- Native AOT client (`RunAOTCompilation`, `TrimMode=full`, `WasmStripILAfterAOT`).
- Encryption / MFA / dunning tokens in Domain security surface.

### 1.8 AI / subprocessors — nuance for WebPenny

| Layer | Finding |
| --- | --- |
| Legal + Client tests | DeepSeek listed; **OpenAI forbidden** in customer-facing subprocessor copy |
| Infrastructure residual | `PennymatchAiAssistant` still uses OpenAI-compatible path (`gpt-4o-mini`, `api.openai.com` wired in `Program.cs`); error strings say “OpenAI” |
| **WebPenny rule** | Market as **DeepSeek / Penny**, **ZERO OpenAI** — match LegalCopy, not residual HttpClient names |

### 1.9 What WebPenny must NOT claim

- “Delivers DTOs to Lemon Squeezy/Paddle” as the **product purpose**.
- VAT management / settlement / AEAT / SII filing for the tenant.
- Replaces Holded / Xero / QuickBooks.
- OpenAI / GPT as a subprocessor.
- Unlimited Enterprise plan.
- Stale pricing (e.g. €499 Pro) if pricing is shown — real catalog is Growth 79 € / Pro 199 € / Enterprise 399 €.

Internal SII queue/exporter exists for operator/compliance tooling; manuals + LegalCopy still say **customer does not file SII through Pennymatch**. Landing transparency = **no** SII/AEAT/VAT settlement for tenants.

---

## 2. WebPenny map (current)

| Area | Path | Notes |
| --- | --- | --- |
| Stack | Blazor .NET 9, Tailwind + `tokens.css` / `site.css` emerald dark | Visual system must stay |
| i18n | `Localization/SiteCatalog.cs` ES/EN static dicts | AOT-friendly; cookie culture |
| Home | Hero → **FlowSimulator** → Advantages → DocsTeaser → CTA | No Pricing on Home |
| Simulator | `FlowSimulator.razor` + Samples | 3 cols; **third col = MoR Lemon/Paddle** ← wrong |
| Docs | `DocsCatalogService` + `DocsArticleBodies` | 6 manuals all MoR-framed |
| Blog | 3 markdown posts under `Content/blog` | MoR-bridge narrative |
| Unused stale | `PricingSection.razor` (Starter/€499 Pro) | Not on Home; wrong if revived |
| Branch | Local `feature/web-landing`; genesis target `cursor/webpenny-corporate-site-1e64` | GitHub `JackBlue23/WebPenny` |
| Port | `http://127.0.0.1:5421` | Prior delivery |

### Stale claim matrix

| Claim in WebPenny | rc1 truth |
| --- | --- |
| Bridge to MoR / deliver UnifiedTxn to LS/Paddle | Bridge to **PGC** + CSV/ERP; MoR = SaaS billing |
| “Not a ledger” / “not your general ledger” (Docs plan-mapping) | **Is** immutable internal double-entry PGC; still not official books / ERP |
| Simulator destination Lemon/Paddle | Should demo **journal lines + CSV → Xero/QB/Holded** |
| Advantages: MoR hot-swap as core value | Differentiator = O(1) ingest, PGC 668/768/0.01€, BCE 3-layer, SHA-256 audit, AOT |
| Blog “puente al MoR” | Rewrite to PGC middleware + transparency |

### Design constraints (Phase 2)

1. Do **not** change palette, fonts, layouts, spacing, animations, CSS class names.  
2. Keep Simulator existence, placement, interactive UX; only texts / labels / PGC / currency / calc copy.  
3. Strict bilingual ES+EN via `SiteCatalog` / dual doc bodies / blog strategy; no hardcoded monolingual UI strings; key parity.

---

## 3. Remediation plan (Phase 3)

1. **SiteCatalog** — rewrite Hero, Sim, Adv, Docs chrome, Blog chrome, Footer, CTA to PGC middleware story; mention Lemon Squeezy only as SaaS MoR billing where needed.  
2. **Simulator** — keep `flow-mor*` CSS classes; relabel third column to ERP/CSV destinations (Xero / QuickBooks / Holded); delivery JSON = sealed PGC draft (430/572/626/477/668/768 as relevant); steps Map→PGC, Seal SHA-256.  
3. **Docs** — expand catalog: Quickstart, connectors ingest, PGC engine, FX BCE 3-layer, ERP CSV import, scope limits, security/privacy/MoR billing.  
4. **Blog** — rewrite 3 existing + add deep SEO articles (what it is NOT; multi-currency 668/768 Spain).  
5. **Verify** — build 0 errors; ES/EN key parity; no OpenAI/wrong tax claims; restart `:5421`.

---

## 4. Copy anchors (approved framing)

**ES one-liner:** Middleware técnico que convierte Stripe, Shopify y Wise en asientos PGC inmutables (668/768, tolerancia 0,01 €, BCE en 3 capas) y CSV listos para Holded, Xero o QuickBooks — sin sustituir tu ERP ni liquidar IVA/AEAT.

**EN one-liner:** Technical middleware that turns Stripe, Shopify, and Wise into immutable Spanish PGC journals (668/768, 0.01 EUR tolerance, three-layer BCE FX) and universal CSVs for Holded, Xero, or QuickBooks — not an ERP, not VAT settlement.

**Billing footnote:** SaaS billed via Lemon Squeezy (active MoR); Paddle Billing v2 prepared as contingency.
