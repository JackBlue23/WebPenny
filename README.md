# Pennymatch.Web (WebPenny)

Corporate marketing site + technical blog for **Pennymatch** — ASP.NET Core 9 Blazor SSR (Razor Components), same .NET family as `Pennymatch.Client`.

## Requirements

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (see `global.json`)

## Run

From the repo root (`WebPenny`):

```bash
dotnet run --project src/Pennymatch.Web --launch-profile http
```

- Marketing site: [http://localhost:5421](http://localhost:5421)
- HTTPS profile also available: `https://localhost:7421`

Or open `Pennymatch.Web.sln` in Visual Studio / Rider and run the `Pennymatch.Web` project.

## Configuration (`App` section)

Auth CTAs open the **Client UI**, not the API host.

| Key | Purpose | Dev default |
| --- | --- | --- |
| `App:ApiBaseUrl` | Pennymatch.Api (from Client `ApiBaseUrl`) | `http://localhost:5199/` |
| `App:ProductBaseUrl` | **Pennymatch.Client** origin (Login / Register) | `http://localhost:5288` |
| `App:LoginPath` | Client login (supports `returnUrl` stub) | `/login?returnUrl=` |
| `App:RegisterPath` | Client register | `/register` |
| `App:DemoPath` | Demo CTA — `#demo`, `mailto:…`, path, or absolute URL | `#demo` |
| `App:SessionCookieName` | Optional same-site session cookie for nav CTA swap | _(empty)_ |

**Do not** point `ProductBaseUrl` at `5199` (API). Auth CTAs always hit the Client UI.

Example resolved CTAs in Development:

- Login → `http://localhost:5288/login?returnUrl=` + URL-encoded WebPenny origin (e.g. `http://localhost:5421/`)
- Register → `http://localhost:5288/register`
- Demo → `#demo` (in-page anchor on the marketing site)

## Brand tokens

`wwwroot/css/tokens.css` mirrors the Client dark fintech + emerald palette (`#34d399`, slate-950 surfaces). Comments mark what to swap when Client CSS is shared formally.

## Routes

| Path | Content |
| --- | --- |
| `/` | Landing — hero, ledger, webhooks, security, pricing |
| `/blog` | Article grid |
| `/blog/{slug}` | Article + sidebar + newsletter |
| `/#precios` | Pricing |
| `/#caracteristicas` | Technical core |
| `/#demo` | Demo CTA anchor |

## Blog content

Markdown with YAML front matter under `src/Pennymatch.Web/Content/blog/`.
