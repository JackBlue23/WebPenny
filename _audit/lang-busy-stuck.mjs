import { chromium } from "playwright-core";

const BASE = "http://127.0.0.1:5421";

async function snapshot(page) {
  return page.evaluate(() => {
    const root = document.querySelector("[data-lang-switch]");
    return {
      htmlLang: document.documentElement.lang,
      label: root?.querySelector(".lang-switch-label")?.textContent?.trim(),
      open: root?.open ?? false,
      options: [...document.querySelectorAll("[data-lang-switch] .lang-switch-option")].slice(0, 2).map((b) => ({
        lang: b.getAttribute("lang"),
        disabled: b.disabled,
        active: b.classList.contains("is-active"),
      })),
    };
  });
}

const browser = await chromium.launch({ headless: true });
const page = await browser.newPage();

await page.goto(BASE + "/culture/set?culture=en&returnUrl=%2F", { waitUntil: "networkidle" });
await page.waitForTimeout(1200);
console.log("start", await snapshot(page));

// Abort culture/set so location.assign fails after Blazor sets _busy=true
await page.route("**/culture/set**", async (route) => {
  console.log("aborting", route.request().url());
  await route.abort("failed");
});

const root = page.locator("[data-lang-switch]").first();
await root.locator("summary").click();
await page.waitForTimeout(100);
await root.locator('button[lang="es"]').click();
await page.waitForTimeout(800);
console.log("after aborted switch to ES", await snapshot(page));

await page.unroute("**/culture/set**");

// Can we switch now?
await root.locator("summary").click().catch(() => {});
await page.waitForTimeout(100);
const open = await page.evaluate(() => document.querySelector("[data-lang-switch]")?.open);
if (!open) await root.locator("summary").click();
await page.waitForTimeout(100);

const esBtn = root.locator('button[lang="es"]');
const enBtn = root.locator('button[lang="en"]');
console.log(
  "button disabled?",
  await esBtn.evaluate((b) => b.disabled),
  await enBtn.evaluate((b) => b.disabled)
);

const reqs = [];
page.on("request", (r) => {
  if (r.url().includes("/culture/set")) reqs.push(r.url());
});

try {
  await esBtn.click({ force: true, timeout: 2000 });
} catch (e) {
  console.log("click ES failed", e.message);
}
await page.waitForTimeout(1000);
console.log("reqs after retry", reqs);
console.log("final", await snapshot(page));

// Also verify: does InvokeVoidAsync throw on aborted assign?
await page.goto(BASE + "/culture/set?culture=en&returnUrl=%2F", { waitUntil: "networkidle" });
await page.waitForTimeout(1200);
await page.route("**/culture/set**", (route) => route.abort("failed"));
const jsErr = await page.evaluate(async () => {
  try {
    window.pennyLangSwitch.switchCulture("es");
    return "returned-ok";
  } catch (e) {
    return "threw:" + e;
  }
});
console.log("direct JS after route abort:", jsErr);
await page.waitForTimeout(500);
console.log("after direct JS", await snapshot(page).catch((e) => e.message));

await browser.close();
