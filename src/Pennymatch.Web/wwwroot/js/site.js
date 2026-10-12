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
})();
