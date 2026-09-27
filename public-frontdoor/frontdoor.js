(() => {
  const APP_ORIGIN = 'https://staging.edulytiks.com';
  const LANGUAGE_KEY = 'edulytics.frontdoor.language';
  let backendReady = false;
  let wakePromise = null;

  const statusCopy = {
    en: {
      preparing: 'Preparing the Edulytics workspace…',
      delayed: 'The workspace is taking longer than usual. Please try again.'
    },
    pl: {
      preparing: 'Przygotowujemy środowisko Edulytics…',
      delayed: 'Uruchamianie trwa dłużej niż zwykle. Spróbuj ponownie.'
    },
    ar: {
      preparing: 'جارٍ تجهيز مساحة Edulytics…',
      delayed: 'استغرق تشغيل البرنامج وقتًا أطول من المعتاد. حاول مرة أخرى.'
    }
  };

  const sleep = ms => new Promise(resolve => setTimeout(resolve, ms));

  function language() {
    const value = (document.documentElement.lang || 'pl').toLowerCase();
    return value.startsWith('ar') ? 'ar' : value.startsWith('en') ? 'en' : 'pl';
  }

  async function probeReady() {
    const controller = new AbortController();
    const timeout = window.setTimeout(() => controller.abort(), 5000);

    try {
      const response = await fetch(
        `${APP_ORIGIN}/health/ready?frontdoor=${Date.now()}`,
        {
          method: 'GET',
          mode: 'cors',
          cache: 'no-store',
          credentials: 'omit',
          signal: controller.signal
        });

      if (!response.ok) return false;
      const payload = await response.json();
      return payload?.status === 'Healthy';
    } catch {
      return false;
    } finally {
      window.clearTimeout(timeout);
    }
  }

  async function ensureBackendReady(maxWaitMs = 90000) {
    if (backendReady) return true;
    if (wakePromise) return wakePromise;

    wakePromise = (async () => {
      const started = Date.now();

      while (Date.now() - started < maxWaitMs) {
        if (await probeReady()) {
          backendReady = true;
          document.documentElement.dataset.backendReady = 'true';
          return true;
        }

        await sleep(1500);
      }

      return false;
    })();

    const result = await wakePromise;
    wakePromise = null;
    return result;
  }

  function ensureStatus() {
    let host = document.querySelector('[data-frontdoor-status]');
    if (host) return host;

    host = document.createElement('aside');
    host.setAttribute('data-frontdoor-status', '');
    host.setAttribute('role', 'status');
    host.setAttribute('aria-live', 'polite');
    host.hidden = true;
    host.style.cssText = [
      'position:fixed',
      'z-index:2147483647',
      'inset-inline-end:20px',
      'bottom:20px',
      'max-width:min(420px,calc(100vw - 32px))',
      'padding:14px 18px',
      'border-radius:14px',
      'background:#151522',
      'color:#fff',
      'box-shadow:0 18px 50px rgba(0,0,0,.24)',
      'font:500 14px/1.45 system-ui,-apple-system,Segoe UI,sans-serif'
    ].join(';');

    document.body.appendChild(host);
    return host;
  }

  function showStatus(key) {
    const host = ensureStatus();
    const copy = statusCopy[language()] || statusCopy.pl;
    host.textContent = copy[key] || copy.preparing;
    host.hidden = false;
  }

  function hideStatus() {
    const host = document.querySelector('[data-frontdoor-status]');
    if (host) host.hidden = true;
  }

  function staticLanguagePath(targetLanguage) {
    if (targetLanguage === 'en') return '/en/';
    if (targetLanguage === 'ar') return '/ar/';
    return '/';
  }

  function installLanguageRouting() {
    document.addEventListener('submit', event => {
      const form = event.target;
      if (!(form instanceof HTMLFormElement)) return;

      const cultureInput = form.querySelector('input[name="culture"]');
      if (!cultureInput) return;

      const target = cultureInput.value;
      if (target !== 'pl' && target !== 'en' && target !== 'ar') return;

      event.preventDefault();
      try { window.localStorage.setItem(LANGUAGE_KEY, target); } catch { /* no-op */ }
      window.location.assign(staticLanguagePath(target));
    }, true);
  }

  function shouldStayStatic(url) {
    if (url.origin !== window.location.origin) return true;

    if (url.pathname === '/' && url.hash) return true;
    if (url.pathname === '/' && !url.search && !url.hash) return true;

    return url.pathname === '/pl/' ||
           url.pathname === '/en/' ||
           url.pathname === '/ar/';
  }

  function installBackendRouting() {
    document.addEventListener('click', async event => {
      if (event.defaultPrevented || event.button !== 0 ||
          event.metaKey || event.ctrlKey || event.shiftKey || event.altKey) {
        return;
      }

      const anchor = event.target.closest?.('a[href]');
      if (!anchor) return;

      const raw = anchor.getAttribute('href');
      if (!raw || raw.startsWith('#') ||
          raw.startsWith('mailto:') || raw.startsWith('tel:') ||
          raw.startsWith('javascript:')) {
        return;
      }

      const url = new URL(raw, window.location.href);

      // Keep every application URL on edulytiks.com. Render's static-site
      // rewrite forwards missing dynamic paths to staging.edulytiks.com
      // without changing the address shown in the browser.
      const isBackend =
        url.origin === window.location.origin &&
        !shouldStayStatic(url);

      if (!isBackend) return;

      event.preventDefault();
      showStatus('preparing');

      const ready = await ensureBackendReady();
      if (!ready) {
        showStatus('delayed');
        return;
      }

      hideStatus();
      window.location.assign(
        `${url.pathname}${url.search}${url.hash}`);
    }, true);
  }

  function restorePreferredLanguage() {
    if (window.location.pathname !== '/') return;

    try {
      const preferred = window.localStorage.getItem(LANGUAGE_KEY);
      if (preferred === 'en' || preferred === 'ar') {
        window.location.replace(staticLanguagePath(preferred));
      }
    } catch {
      // Polish remains the default when storage is unavailable.
    }
  }

  restorePreferredLanguage();

  document.addEventListener('DOMContentLoaded', () => {
    installLanguageRouting();
    installBackendRouting();

    // Wake the free application immediately without blocking the public page.
    ensureBackendReady();
  });
})();
