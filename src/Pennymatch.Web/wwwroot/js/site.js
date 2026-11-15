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

    /**
     * Infallible culture switch: capture live URL + scroll, then full-document navigate
     * to /culture/set with returnUrl built NOW (not from a render-time <a href>).
     * Never falls back to NavigateTo("/") from C#.
     */
    switchCulture(culture) {
      const code = String(culture || "").toLowerCase() === "en" ? "en" : "es";
      try {
        if ("scrollRestoration" in history) {
          history.scrollRestoration = "manual";
        }
      } catch {
        /* ignore */
      }

      this.savePosition();
      const returnUrl = encodeURIComponent(currentReturnUrl());
      // Full document load so the culture cookie is applied server-side on the next request.
      window.location.assign(`/culture/set?culture=${encodeURIComponent(code)}&returnUrl=${returnUrl}`);
    },

    closeSwitcher(el) {
      const root = el && el.closest ? el.closest("[data-lang-switch]") : el;
      if (root && root.tagName === "DETAILS") {
        root.open = false;
      }
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

  // Legacy capture for any remaining data-lang-preserve anchors (defensive).
  document.addEventListener(
    "click",
    (event) => {
      const link = event.target?.closest?.("a[data-lang-preserve]");
      if (!link) return;
      // Rewrite href returnUrl from live location so a stale SSR href cannot send the user to /.
      event.preventDefault();
      const culture = link.getAttribute("hreflang") || link.getAttribute("lang") || "es";
      window.pennyLangSwitch.switchCulture(culture);
    },
    true
  );

  window.pennyLangSwitch.restorePosition();

  // Enhanced navigation (in-app links): focus h1 without scrolling the viewport.
  document.addEventListener("enhancedload", () => {
    window.pennyLangSwitch.focusMainHeading();
  });
})();
