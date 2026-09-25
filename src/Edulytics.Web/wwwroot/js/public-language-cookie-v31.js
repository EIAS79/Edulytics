(() => {
  const legacyCookieName = 'Edulytics.PublicLanguage';
  const legacyStorageKey = 'edulytics.public.siteLanguage';

  const clearLegacy = () => {
    try { window.localStorage.removeItem(legacyStorageKey); } catch { /* no-op */ }
    document.cookie = `${legacyCookieName}=; Max-Age=0; Path=/; SameSite=Strict${location.protocol === 'https:' ? '; Secure' : ''}`;
  };

  // Arabic is now a canonical server culture. Clear the old client-only state
  // after the server had a chance to migrate it on this request.
  clearLegacy();

  document.addEventListener('submit', event => {
    const form = event.target;
    const culture = form?.querySelector?.('input[name="culture"]')?.value;
    if (culture === 'en' || culture === 'pl' || culture === 'ar') clearLegacy();
  }, true);
})();
