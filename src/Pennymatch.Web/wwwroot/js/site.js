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

  const LANG_SCROLL_KEY = "pm.lang.scroll";

  // Before /culture/set full reload: stash pathname+search+hash+scrollY.
  document.addEventListener(
    "click",
    (event) => {
      const link = event.target?.closest?.("a[data-lang-preserve]");
      if (!link) return;
      try {
        sessionStorage.setItem(
          LANG_SCROLL_KEY,
          JSON.stringify({
            path: location.pathname + location.search,
            hash: location.hash || "",
            y: window.scrollY || 0
          })
        );
      } catch {
        /* private mode / quota — culture switch still works without restore */
      }
    },
    true
  );

  // After forceLoad redirect: restore hash (if stripped) and exact scrollY.
  try {
    const raw = sessionStorage.getItem(LANG_SCROLL_KEY);
    if (!raw) return;
    sessionStorage.removeItem(LANG_SCROLL_KEY);
    const data = JSON.parse(raw);
    const here = location.pathname + location.search;
    if (data.path && data.path !== here) return;

    if (data.hash && !location.hash) {
      history.replaceState(null, "", here + data.hash);
    }

    const y = Number(data.y);
    if (!Number.isFinite(y) || y <= 0) return;

    const restore = () => window.scrollTo(0, y);
    // Defer past Blazor/CSS layout; avoid fighting scroll-behavior: smooth.
    requestAnimationFrame(() => {
      restore();
      requestAnimationFrame(restore);
      setTimeout(restore, 0);
      setTimeout(restore, 50);
    });
  } catch {
    /* ignore corrupt payload */
  }
})();
