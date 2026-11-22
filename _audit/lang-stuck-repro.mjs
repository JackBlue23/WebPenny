import { chromium } from "playwright-core";

const BASE = "http://127.0.0.1:5421";

async function dump(page, tag) {
  const info = await page.evaluate(() => {
    const htmlLang = document.documentElement.lang;
    const switches = [...document.querySelectorAll("[data-lang-switch]")].map((root, i) => {
      const label = root.querySelector(".lang-switch-label")?.textContent?.trim();
      const options = [...root.querySelectorAll("[data-lang-set]")].map((b) => ({
        tag: b.tagName,
        text: b.textContent?.trim(),
        lang: b.getAttribute("lang"),
        href: b.getAttribute("href"),
        active: b.classList.contains("is-active"),
        data: b.getAttribute("data-lang-set"),
      }));
      return { i, open: root.open, label, options };
    });
    const cookie = document.cookie;
    const hasPenny = typeof window.pennyLangSwitch?.switchCulture === "function";
    const siteJs = [...document.scripts].map((s) => s.src).filter((s) => s.includes("site.js"));
    return { htmlLang, cookie, hasPenny, siteJs, switches, title: document.title };
  });
  console.log(`\n=== ${tag} ===`);
  console.log(JSON.stringify(info, null, 2));
  return info;
}

async function clickLang(page, code) {
  const root = page.locator("[data-lang-switch]").first();
  await root.locator("summary").click();
  await page.waitForTimeout(200);
  const btn = root.locator(`[data-lang-set="${code}"]`);
  const before = await btn.evaluate((b) => ({
    tag: b.tagName,
    href: b.getAttribute("href"),
    active: b.classList.contains("is-active"),
    text: b.textContent,
  }));
  console.log(`click ${code} state`, before);

  const cultureReqs = [];
  const onReq = (r) => {
    if (r.url().includes("/culture/set")) cultureReqs.push(r.url());
  };
  page.on("request", onReq);

  const navPromise = page.waitForNavigation({ timeout: 10000 }).catch((e) => e.message);
  await btn.click({ timeout: 3000 });
  const nav = await navPromise;
  page.off("request", onReq);
  console.log(`after click ${code}: nav=`, typeof nav === "string" ? nav : page.url(), "reqs=", cultureReqs);
  await page.waitForTimeout(400);
}

const browser = await chromium.launch({ headless: true });
const context = await browser.newContext();
const page = await context.newPage();

page.on("console", (m) => {
  if (m.type() === "error") console.log("BROWSER-ERR:", m.text());
});
page.on("pageerror", (e) => console.log("PAGEERROR:", e.message));
page.on("response", (r) => {
  if (r.url().includes("culture")) {
    console.log("RES", r.status(), r.url(), "set-cookie=", r.headers()["set-cookie"]);
  }
});

await page.goto(BASE + "/", { waitUntil: "networkidle" });
await context.clearCookies();
await page.goto(BASE + "/", { waitUntil: "networkidle" });
await page.waitForTimeout(800);
await dump(page, "fresh ES");

await clickLang(page, "en");
await dump(page, "after EN click");

await clickLang(page, "es");
await dump(page, "after ES click");

await clickLang(page, "en");
await dump(page, "after EN again");

await clickLang(page, "es");
await dump(page, "after ES again");

// No-JS path: strip site.js handlers by evaluating with preventDefault removed —
// navigate via plain href attribute alone.
await page.goto(BASE + "/culture/set?culture=en&returnUrl=%2F", { waitUntil: "networkidle" });
await dump(page, "forced EN");
const href = await page.evaluate(() => {
  const a = document.querySelector('[data-lang-set="es"]');
  return a?.getAttribute("href") || null;
});
console.log("plain ES href=", href);
await page.goto(BASE + (href || "/culture/set?culture=es&returnUrl=%2F"), { waitUntil: "networkidle" });
await dump(page, "after plain GET ES href");

await browser.close();
