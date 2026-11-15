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
  // FocusOnNavigate (h1) can re-jump after paint — restore again from MainLayout.
  const LANG_SCROLL_KEY = "penny.lang.scroll";
  let pendingScroll = null;

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

  document.addEventListener(
    "click",
    (event) => {
      const link = event.target?.closest?.("a[data-lang-preserve]");
      if (!link) return;
      window.pennyLangSwitch.savePosition();
    },
    true
  );

  window.pennyLangSwitch.restorePosition();

  // Enhanced navigation (in-app links): focus h1 without scrolling the viewport.
  document.addEventListener("enhancedload", () => {
    window.pennyLangSwitch.focusMainHeading();
  });
})();
