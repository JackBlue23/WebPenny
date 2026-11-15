# Pennymatch.Web (WebPenny)

B2B SaaS conversion landing + docs hub for **Pennymatch** — ASP.NET Core 9 Blazor SSR (Razor Components) with optional Interactive Server for the transactional flow simulator.

Pennymatch is the **no-code bridge**: raw transactional events (Stripe, Shopify, Wise) → immutable unified DTOs → clean delivery to the Merchant of Record (Lemon Squeezy / Paddle). It is **not** an ERP, does not invoice, and does not replace Holded/Xero.

## Requirements

- Any recent [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (e.g. 9.0.310+). **No `global.json`**.
- Node.js 20+ (optional for local Tailwind rebuilds). Committed `wwwroot/css/tailwind.css` is enough to run.

## Run

From the repo root:

```bash
dotnet run --project src/Pennymatch.Web --launch-profile http
```

- Marketing site: [http://127.0.0.1:5421](http://127.0.0.1:5421)
- HTTPS profile: `https://localhost:7421`

Explicit bind:

```bash
dotnet run --project src/Pennymatch.Web --urls http://127.0.0.1:5421
```

### Tailwind (optional rebuild)

```bash
cd src/Pennymatch.Web
npm install
npm run css:build
```

The `.csproj` also runs `npm run css:build` before compile when `node_modules` is present.

## Configuration (`App`)

| Key | Purpose | Dev default |
| --- | --- | --- |
| `App:ApiBaseUrl` | Pennymatch.Api | `http://localhost:5199/` |
| `App:ProductBaseUrl` | Pennymatch.Client origin | `http://localhost:5288` |
| `App:LoginPath` | Client login (secondary CTA) | `/login?returnUrl=dashboard` |
| `App:RegisterPath` | Primary product / trial CTA | `/login?returnUrl=dashboard` |
| `App:DemoPath` | Secondary CTA (manuals) | `/docs` |
| `App:SessionCookieName` | Optional same-site session cookie | _(empty)_ |

Resolved CTAs in Development:

- Primary / trial → `http://localhost:5288/login?returnUrl=dashboard`
- Login (nav secondary) → `http://localhost:5288/login?returnUrl=dashboard`
- Manuals → `/docs` (also `/manuales`)

Blank Client `/auth/register` is intentionally unused until that page ships.

## Brand

Dark emerald: background `#0f172a`, accent `#34d399`, light slate/white text. Tokens in `wwwroot/css/tokens.css`; Tailwind theme mirrors them in `Styles/input.css`.

## Routes

| Path | Content |
| --- | --- |
| `/` | Hero, interactive flow simulator, advantages, docs teaser, trial CTA |
| `/docs` · `/manuales` | Integration / ops manuals hub |
| `/docs/{slug}` | Manual article |
| `/blog` · `/blog/{slug}` | Technical blog |
| `/culture/set?culture=es|en&returnUrl=/…` | Sets the ASP.NET Core culture cookie, then redirects |
| `/#simulador` · `/#ventajas` · `/#docs` | Landing anchors |

## Localization (ES / EN)

Native ASP.NET Core RequestLocalization (default **`es`**) with `CookieRequestCultureProvider`. No NuGet i18n packages and no reflection — UI copy lives in injectable static dictionaries keyed by `UiCulture` (`es` / `en`).

- Navbar language switcher (ESP / ENG, globe + chevron, no flags) posts a navigation to `/culture/set`, which writes `.AspNetCore.Culture` and redirects so `/`, `/docs`, and `/blog` re-render in the chosen language without flicker.
- Verify: open the site → top-right **ESP** → choose **ENG — English** → hero/docs/nav should be English; reload `/docs` and `/blog` — language persists via cookie. Switch back to **ESP — Español**.

## Content

- Blog Markdown: `src/Pennymatch.Web/Content/blog/`
- Docs catalog: `DocsCatalogService` (static, culture-aware, no reflection)
- UI strings: `Localization/SiteCatalog.cs`
