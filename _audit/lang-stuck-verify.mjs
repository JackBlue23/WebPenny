import { chromium } from "playwright-core";

const BASE = "http://127.0.0.1:5421";

async function snap(page) {
  return page.evaluate(() => ({
    url: location.pathname + location.search + location.hash,
    htmlLang: document.documentElement.lang,
    cookie: document.cookie.includes("c%3Den")
      ? "en"
      : document.cookie.includes("c%3Des")
        ? "es"
        : document.cookie || "(none)",
    label: document.querySelector("[data-lang-switch] .lang-switch-label")?.textContent?.trim(),
    titleEs: /Middleware PGC/.test(document.title),
    titleEn: /double-entry middleware/.test(document.title),
    hasDataLangSet: !!document.querySelector("[data-lang-set]"),
    interactiveAttr: document.querySelector("[data-lang-switch]")?.getAttribute("b-") || "static-ok",
  }));
}

async function switchTo(page, code) {
  const root = page.locator("[data-lang-switch]").first();
  await root.locator("summary").click();
  await page.waitForTimeout(100);
  const link = root.locator(`[data-lang-set="${code}"]`);
  // Prefer real click on plain GET <a>; fall back to JS assign if needed.
  const nav = page.waitForNavigation({ timeout: 10000 });
  await link.click();
  await nav;
  await page.waitForTimeout(300);
  return snap(page);
}

const browser = await chromium.launch({ headless: true });
const page = await browser.newPage();
const results = [];

function pass(name, ok, detail) {
  results.push({ name, ok, detail });
  console.log(ok ? "PASS" : "FAIL", name, detail || "");
}

// 1) Fresh → EN → ES → EN
await page.goto(BASE + "/", { waitUntil: "networkidle" });
await page.context().clearCookies();
await page.goto(BASE + "/", { waitUntil: "networkidle" });
let s = await snap(page);
pass("start ES", s.htmlLang === "es" && s.label === "ESP", s);

s = await switchTo(page, "en");
pass("switch EN", s.htmlLang === "en" && s.label === "ENG" && s.titleEn, s);

s = await switchTo(page, "es");
pass("switch ES from EN", s.htmlLang === "es" && s.label === "ESP" && s.titleEs, s);

s = await switchTo(page, "en");
pass("switch EN again", s.htmlLang === "en" && s.label === "ENG", s);

// 2) Enhanced nav then ES
await page.click('a[href="/docs"]');
await page.waitForTimeout(800);
s = await switchTo(page, "es");
pass("Docs EN→ES keeps /docs", s.htmlLang === "es" && s.url.startsWith("/docs"), s);

// 3) Circuit dead: block _blazor websocket — switch must still work via native JS
await page.goto(BASE + "/culture/set?culture=en&returnUrl=%2F", { waitUntil: "networkidle" });
await page.route("**/_blazor**", (route) => route.abort());
await page.reload({ waitUntil: "domcontentloaded" });
await page.waitForTimeout(500);
s = await snap(page);
pass("EN with blazor blocked", s.htmlLang === "en" && s.hasDataLangSet, s);
s = await switchTo(page, "es");
pass("ES while circuit blocked", s.htmlLang === "es" && s.label === "ESP" && s.titleEs, s);

// 4) Same-culture click closes menu (no nav)
await page.unroute("**/_blazor**");
await page.goto(BASE + "/culture/set?culture=en&returnUrl=%2F", { waitUntil: "networkidle" });
const root = page.locator("[data-lang-switch]").first();
await root.locator("summary").click();
await root.locator('[data-lang-set="en"]').click();
await page.waitForTimeout(300);
const open = await page.evaluate(() => document.querySelector("[data-lang-switch]")?.open);
pass("same-culture click closes", open === false, { open, ...(await snap(page)) });

await browser.close();

const failed = results.filter((r) => !r.ok);
console.log("\nSUMMARY", results.length - failed.length + "/" + results.length);
process.exit(failed.length ? 1 : 0);
