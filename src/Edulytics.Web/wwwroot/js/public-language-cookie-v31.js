(() => {
  const cookieName = 'Edulytics.PublicLanguage';
  const storageKey = 'edulytics.public.siteLanguage';
  const secure = window.location.protocol === 'https:' ? '; Secure' : '';

  const write = (value, maxAge) => {
    document.cookie = `${cookieName}=${encodeURIComponent(value)}; Path=/; Max-Age=${maxAge}; SameSite=Lax${secure}`;
  };
  const clear = () => write('', 0);

  try {
    if (window.localStorage.getItem(storageKey) === 'ar') write('ar', 31536000);
  } catch { /* storage can be blocked */ }

  document.addEventListener('click', event => {
    const arabic = event.target.closest?.('[data-public-arabic-switch]');
    if (arabic) {
      write('ar', 31536000);
      return;
    }

    const button = event.target.closest?.('form button');
    const form = button?.closest('form');
    const culture = form?.querySelector('input[name="culture"]')?.value;
    if (culture === 'en' || culture === 'pl') clear();
  }, true);

  document.addEventListener('submit', event => {
    const culture = event.target?.querySelector?.('input[name="culture"]')?.value;
    if (culture === 'en' || culture === 'pl') clear();
  }, true);
})();
