import { chromium } from "playwright-core";

const browser = await chromium.launch({ headless: true });
const page = await browser.newPage();

page.on("pageerror", (e) => {
  console.log("PAGEERROR", e.message);
  console.log(e.stack);
});
page.on("response", async (r) => {
  if (r.status() >= 400) {
    const ct = r.headers()["content-type"] || "";
    console.log("BAD", r.status(), r.url(), ct);
  }
});

await page.goto("http://127.0.0.1:5421/culture/set?culture=en&returnUrl=%2F", {
  waitUntil: "networkidle",
});
await page.waitForTimeout(2000);

// Check which scripts loaded and if Blazor has errors
const info = await page.evaluate(() => ({
  scripts: [...document.scripts].map((s) => s.src),
  hasPenny: !!window.pennyLangSwitch,
  blazor: typeof Blazor,
}));
console.log(info);

await browser.close();
