import { chromium } from "playwright-core";

const BASE = "http://127.0.0.1:5421";

async function state(page) {
  return page.evaluate(() => ({
    url: location.href,
    htmlLang: document.documentElement.lang,
    cookie: document.cookie,
    label: document.querySelector("[data-lang-switch] .lang-switch-label")?.textContent?.trim(),
    active: [...document.querySelectorAll("[data-lang-switch] .lang-switch-option.is-active")].map(
      (b) => b.getAttribute("lang")
    ),
    disabled: [...document.querySelectorAll("[data-lang-switch] .lang-switch-option")].map((b) => ({
      lang: b.getAttribute("lang"),
      disabled: b.disabled,
    })),
    title: document.title.slice(0, 60),
  }));
}

async function openAndClick(page, code, { waitCircuit = true } = {}) {
  if (waitCircuit) {
    await page.waitForTimeout(800);
  }
  const root = page.locator("[data-lang-switch]").first();
  await root.locator("summary").click();
  await page.waitForTimeout(150);
  const btn = root.locator(`button[lang="${code}"]`);
  const meta = await btn.evaluate((b) => ({ disabled: b.disabled, active: b.classList.contains("is-active") }));
  const reqs = [];
  const onReq = (r) => {
    if (r.url().includes("/culture/set")) reqs.push(r.url());
  };
  page.on("request", onReq);
  const nav = page.waitForNavigation({ timeout: 5000 }).catch((e) => e.message);
  try {
    await btn.click({ timeout: 2000 });
  } catch (e) {
    page.off("request", onReq);
    return { clickError: String(e), meta, reqs, after: await state(page) };
  }
  const navResult = await nav;
  page.off("request", onReq);
  await page.waitForTimeout(400);
  return { meta, reqs, navResult: typeof navResult === "string" ? navResult : "navigated", after: await state(page) };
}

const browser = await chromium.launch({ headless: true });
const page = await browser.newPage();
page.on("pageerror", (e) => console.log("PAGEERROR", e.message));
page.on("console", (m) => {
  if (m.type() === "error") console.log("CONSOLEERR", m.text());
});

// Track 404s
page.on("response", async (r) => {
  if (r.status() === 404) console.log("404", r.url());
});

console.log("\n-- A: EN then browse Docs/Blog then ES --");
await page.goto(BASE + "/culture/set?culture=en&returnUrl=%2F", { waitUntil: "networkidle" });
console.log("start", await state(page));
await page.click('a[href="/docs"]');
await page.waitForTimeout(1000);
console.log("docs", await state(page));
await page.click('a[href="/blog"]');
await page.waitForTimeout(1000);
console.log("blog", await state(page));
let r = await openAndClick(page, "es");
console.log("switch ES from blog", JSON.stringify(r, null, 2));

console.log("\n-- B: from locked EN feel — click EN (active) then ES --");
await page.goto(BASE + "/culture/set?culture=en&returnUrl=%2F", { waitUntil: "networkidle" });
await page.waitForTimeout(1000);
r = await openAndClick(page, "en"); // click active
console.log("click active EN", JSON.stringify(r, null, 2));
r = await openAndClick(page, "es");
console.log("then ES", JSON.stringify(r, null, 2));

console.log("\n-- C: click ES before circuit ready --");
await page.goto(BASE + "/culture/set?culture=en&returnUrl=%2F", { waitUntil: "domcontentloaded" });
r = await openAndClick(page, "es", { waitCircuit: false });
console.log("early ES", JSON.stringify(r, null, 2));

console.log("\n-- D: enhanced nav home#ventajas then ES --");
await page.goto(BASE + "/culture/set?culture=en&returnUrl=%2F", { waitUntil: "networkidle" });
await page.waitForTimeout(800);
await page.click('a[href="/#ventajas"]');
await page.waitForTimeout(800);
console.log("hash", await state(page));
r = await openAndClick(page, "es");
console.log("ES from hash", JSON.stringify(r, null, 2));

console.log("\n-- E: simulate stuck _busy via double invoke --");
await page.goto(BASE + "/culture/set?culture=en&returnUrl=%2F", { waitUntil: "networkidle" });
await page.waitForTimeout(1000);
// Click ES but intercept navigation to leave circuit with _busy=true
await page.route("**/culture/set**", (route) => route.abort());
const root = page.locator("[data-lang-switch]").first();
await root.locator("summary").click();
await root.locator('button[lang="es"]').click();
await page.waitForTimeout(500);
console.log("after aborted ES", await state(page));
await page.unroute("**/culture/set**");
// Try again — if _busy stuck, no request
r = await openAndClick(page, "es");
console.log("retry ES after abort", JSON.stringify(r, null, 2));

await browser.close();
