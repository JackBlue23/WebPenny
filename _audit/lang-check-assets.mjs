import { chromium } from "playwright-core";

const BASE = "http://127.0.0.1:5421";
const browser = await chromium.launch({ headless: true });
const page = await browser.newPage();
await page.goto(BASE + "/", { waitUntil: "networkidle" });

const info = await page.evaluate(async () => {
  const scripts = [...document.scripts].map((s) => s.src).filter((s) => s.includes("site"));
  const anchors = [...document.querySelectorAll("[data-lang-set]")].map((x) => ({
    tag: x.tagName,
    href: x.getAttribute("href"),
    set: x.getAttribute("data-lang-set"),
  }));
  const src = scripts[0];
  const r = src ? await fetch(src) : null;
  return {
    scripts,
    anchors,
    cache: r ? r.headers.get("cache-control") : null,
    status: r ? r.status : null,
  };
});
console.log(JSON.stringify(info, null, 2));

// Simulate stale JS: remove pennyLangSwitch click path by deleting the helper,
// then click the plain <a> — must still leave EN.
await page.goto(BASE + "/culture/set?culture=en&returnUrl=%2F", { waitUntil: "networkidle" });
await page.evaluate(() => {
  // Break JS switcher entirely — only native <a href> must remain.
  window.pennyLangSwitch = undefined;
  document.querySelectorAll("*").forEach(() => {});
});
// Remove capture listeners by cloning document? Can't easily. Instead navigate via href.
const href = await page.evaluate(() => document.querySelector('[data-lang-set="es"]')?.href);
console.log("native href", href);
await page.goto(href, { waitUntil: "networkidle" });
const after = await page.evaluate(() => ({
  lang: document.documentElement.lang,
  label: document.querySelector(".lang-switch-label")?.textContent?.trim(),
  title: document.title,
}));
console.log("after native GET without relying on switchCulture", after);
const ok = after.lang === "es" && after.label === "ESP";
console.log(ok ? "PASS native-GET-no-JS-helper" : "FAIL native-GET-no-JS-helper");
await browser.close();
process.exit(ok ? 0 : 1);
