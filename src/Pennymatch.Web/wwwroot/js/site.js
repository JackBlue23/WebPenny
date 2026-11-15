(() => {
  const toggle = document.querySelector("[data-nav-toggle]");
  const panel = document.querySelector("[data-nav-mobile]");
  if (toggle && panel) {
    toggle.addEventListener("click", () => {
      const open = panel.classList.toggle("open");
      toggle.setAttribute("aria-expanded", open ? "true" : "false");
    });
  }

  // Cross-origin localStorage/cookies cannot be read from the marketing site.
  // If App:SessionCookieName is same-site and present, hide login/register and show dashboard CTA.
  const cookieName = document.body?.dataset?.sessionCookie;
  const authGuest = document.querySelectorAll("[data-auth-guest]");
  const authSession = document.querySelectorAll("[data-auth-session]");

  const hasSession = (() => {
    if (!cookieName) return false;
    return document.cookie.split(";").some((c) => c.trim().startsWith(cookieName + "="));
  })();

  authGuest.forEach((el) => {
    el.hidden = hasSession;
  });
  authSession.forEach((el) => {
    el.hidden = !hasSession;
  });

  // Language switch: keep pathname/query/hash + scrollY across /culture/set full reload.
  // Destination is ALWAYS read from window.location at click time — never a stale SSR href.
  const LANG_SCROLL_KEY = "penny.lang.scroll";
  const CULTURE_COOKIE = ".AspNetCore.Culture";
  let pendingScroll = null;

  try {
    if ("scrollRestoration" in history) {
      history.scrollRestoration = "manual";
    }
  } catch {
    /* ignore */
  }

  function currentReturnUrl() {
    // Full path + query + hash from the live browser URL (survives enhanced nav).
    const relative = location.pathname + location.search + (location.hash || "");
    return relative && relative.length > 0 ? relative : "/";
  }

  function buildCultureUrl(culture) {
    const code = String(culture || "").toLowerCase() === "en" ? "en" : "es";
    const returnUrl = encodeURIComponent(currentReturnUrl());
    return `/culture/set?culture=${encodeURIComponent(code)}&returnUrl=${returnUrl}`;
  }

  /** Drop conflicting culture cookies so /culture/set is the sole source of truth. */
  function clearCultureCookies() {
    const expire = "Thu, 01 Jan 1970 00:00:00 GMT";
    const paths = ["/", location.pathname || "/"];
    for (const path of paths) {
      document.cookie = `${CULTURE_COOKIE}=; expires=${expire}; path=${path}`;
      document.cookie = `${CULTURE_COOKIE}=; Max-Age=0; path=${path}`;
    }
  }

  function htmlLang() {
    return (document.documentElement.lang || "").toLowerCase().split("-")[0];
  }

  window.pennyLangSwitch = {
    savePosition() {
      const data = {
        path: location.pathname + location.search,
        hash: location.hash || "",
        y: window.scrollY || 0
      };
      pendingScroll = data;
      try {
        sessionStorage.setItem(LANG_SCROLL_KEY, JSON.stringify(data));
      } catch {
        /* private mode / quota — culture switch still works without restore */
      }
      return data;
    },

    buildCultureUrl,

    /**
     * Culture switch built from live location. Prefer plain <a href> navigation;
     * this helper is for programmatic calls and same-culture menu close.
     * No _busy flag — never blocks a second click.
     */
    switchCulture(culture, options) {
      const code = String(culture || "").toLowerCase() === "en" ? "en" : "es";
      const force = !!(options && options.force);
      const current = htmlLang();

      if (!force && current === code) {
        this.closeAllSwitchers();
        return false;
      }

      try {
        if ("scrollRestoration" in history) {
          history.scrollRestoration = "manual";
        }
      } catch {
        /* ignore */
      }

      this.savePosition();
      clearCultureCookies();
      // Full document load so the culture cookie is applied server-side on the next request.
      window.location.assign(buildCultureUrl(code));
      return true;
    },

    closeSwitcher(el) {
      const root = el && el.closest ? el.closest("[data-lang-switch]") : el;
      if (root && root.tagName === "DETAILS") {
        root.open = false;
      }
    },

    closeAllSwitchers() {
      document.querySelectorAll("[data-lang-switch]").forEach((el) => {
        if (el.tagName === "DETAILS") el.open = false;
      });
    },

    restorePosition() {
      try {
        if (!pendingScroll) {
          const raw = sessionStorage.getItem(LANG_SCROLL_KEY);
          if (!raw) return;
          pendingScroll = JSON.parse(raw);
          sessionStorage.removeItem(LANG_SCROLL_KEY);
        }

        const data = pendingScroll;
        if (!data) return;

        const here = location.pathname + location.search;
        if (data.path && data.path !== here) {
          pendingScroll = null;
          return;
        }

        if (data.hash && !location.hash) {
          history.replaceState(null, "", here + data.hash);
        }

        const y = Number(data.y);
        if (!Number.isFinite(y) || y < 0) return;

        const html = document.documentElement;
        const prev = html.style.scrollBehavior;
        html.style.scrollBehavior = "auto";

        const apply = () => window.scrollTo(0, y);
        apply();
        requestAnimationFrame(() => {
          apply();
          setTimeout(() => {
            apply();
            html.style.scrollBehavior = prev;
            pendingScroll = null;
          }, 60);
        });
      } catch {
        /* ignore corrupt payload */
      }
    },

    focusMainHeading() {
      const heading = document.querySelector("main h1, h1");
      if (!heading) return;
      if (!heading.hasAttribute("tabindex")) {
        heading.setAttribute("tabindex", "-1");
      }
      try {
        heading.focus({ preventScroll: true });
      } catch {
        heading.focus();
      }
    }
  };

  /**
   * Capture-phase for [data-lang-set] anchors/buttons.
   * - Different culture: rewrite href from live location, clear cookies, save scroll,
   *   then let the browser follow the GET (no preventDefault on <a>) so a stale
   *   site.js / failed assign can never trap the user on ENG.
   * - Same culture: preventDefault + close menu only.
   * No _busy. No Blazor.
   */
  document.addEventListener(
    "click",
    (event) => {
      const el = event.target?.closest?.("[data-lang-set]");
      if (!el) {
        const legacy = event.target?.closest?.("a[data-lang-preserve]");
        if (!legacy) return;
        const culture = legacy.getAttribute("hreflang") || legacy.getAttribute("lang") || "es";
        const code = String(culture).toLowerCase() === "en" ? "en" : "es";
        if (htmlLang() === code) {
          event.preventDefault();
          window.pennyLangSwitch.closeAllSwitchers();
          return;
        }
        window.pennyLangSwitch.savePosition();
        clearCultureCookies();
        legacy.setAttribute("href", buildCultureUrl(code));
        // Native navigation — do not preventDefault.
        return;
      }

      const code = String(el.getAttribute("data-lang-set") || "es").toLowerCase() === "en" ? "en" : "es";
      if (htmlLang() === code) {
        event.preventDefault();
        window.pennyLangSwitch.closeAllSwitchers();
        return;
      }

      window.pennyLangSwitch.savePosition();
      clearCultureCookies();
      const url = buildCultureUrl(code);

      if (el.tagName === "A") {
        el.setAttribute("href", url);
        // Critical: do NOT preventDefault — plain GET must proceed even if assign fails.
        return;
      }

      // Button fallback (older markup): navigate in JS.
      event.preventDefault();
      window.location.assign(url);
    },
    true
  );

  // Also refresh hrefs when the menu opens so middle-click / open-in-new-tab stay correct.
  document.addEventListener(
    "toggle",
    (event) => {
      const root = event.target;
      if (!root || root.tagName !== "DETAILS" || !root.matches("[data-lang-switch]") || !root.open) {
        return;
      }
      root.querySelectorAll("a[data-lang-set]").forEach((a) => {
        const code = a.getAttribute("data-lang-set") || "es";
        a.setAttribute("href", buildCultureUrl(code));
      });
    },
    true
  );

  window.pennyLangSwitch.restorePosition();

  // Enhanced navigation (in-app links): focus h1 without scrolling; close open lang menus.
  document.addEventListener("enhancedload", () => {
    window.pennyLangSwitch.closeAllSwitchers();
    window.pennyLangSwitch.focusMainHeading();
  });
})();
