(() => {
  const APP_ORIGIN = 'https://staging.edulytiks.com';
  const LIVE_PREFIX = '/__frontdoor-live';
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

  const liveHydrationRoutes = new Set([
    '/account/login',
    '/contact/sales-enquiry',
    '/contact/request-demo',
    '/contact/support',
    '/contact/message'
  ]);

  const sleep = ms => new Promise(resolve => setTimeout(resolve, ms));

  function language() {
    const value = (document.documentElement.lang || 'pl').toLowerCase();
    return value.startsWith('ar') ? 'ar' : value.startsWith('en') ? 'en' : 'pl';
  }

  function preferredLanguage() {
    const query = new URL(window.location.href).searchParams.get('culture');
    if (query === 'pl' || query === 'en' || query === 'ar') return query;

    try {
      const stored = window.localStorage.getItem(LANGUAGE_KEY);
      if (stored === 'pl' || stored === 'en' || stored === 'ar') return stored;
    } catch {
      // Fall back to the language rendered into the snapshot.
    }

    return language();
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
    const copy = statusCopy[preferredLanguage()] || statusCopy.pl;
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

  function withPublicCulture(url) {
    const target = new URL(url.toString());
    const currentLanguage = preferredLanguage();
    const isLoginRoute =
      target.pathname.toLowerCase().replace(/\/+$/, '') === '/account/login';

    target.searchParams.set(
      'culture',
      isLoginRoute && currentLanguage === 'ar'
        ? 'en'
        : currentLanguage);

    return target;
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
      const localizedUrl = withPublicCulture(url);
      window.location.assign(
        `${localizedUrl.pathname}${localizedUrl.search}${localizedUrl.hash}`);
    }, true);
  }

  function normalizePath(pathname) {
    const normalized = pathname.toLowerCase().replace(/\/+$/, '');
    return normalized || '/';
  }

  function shouldHydrateFromLiveApplication() {
    const path = normalizePath(window.location.pathname);
    if (path.startsWith(LIVE_PREFIX)) return false;

    if (liveHydrationRoutes.has(path)) return true;

    // Polish snapshots can remain fully static. EN/AR public pages are
    // refreshed from the live application after wake-up so server-side
    // localization stays exact instead of showing a Polish snapshot.
    return path !== '/' &&
           path !== '/pl' &&
           path !== '/en' &&
           path !== '/ar' &&
           preferredLanguage() !== 'pl';
  }

  function lockInteractiveSnapshot() {
    const path = normalizePath(window.location.pathname);
    if (!liveHydrationRoutes.has(path)) return;

    document.documentElement.dataset.frontdoorHydrating = 'true';

    document
      .querySelectorAll('form input, form select, form textarea, form button, input[form], button[form]')
      .forEach(control => {
        if ('disabled' in control) control.disabled = true;
      });
  }

  function liveBridgeUrl() {
    const current = new URL(window.location.href);
    const target = new URL(
      `${LIVE_PREFIX}${current.pathname}`,
      current.origin);

    for (const [key, value] of current.searchParams.entries()) {
      target.searchParams.append(key, value);
    }

    if (!target.searchParams.has('culture')) {
      target.searchParams.set('culture', preferredLanguage());
    }

    return target;
  }

  function rewritePostFormsToLiveBridge(html) {
    const parser = new DOMParser();
    const parsed = parser.parseFromString(html, 'text/html');

    parsed.querySelectorAll('form').forEach(form => {
      const method = (form.getAttribute('method') || 'get').toLowerCase();
      if (method !== 'post') return;

      const rawAction = form.getAttribute('action') || window.location.pathname;
      const action = new URL(rawAction, window.location.origin);
      if (action.origin !== window.location.origin ||
          action.pathname.startsWith(LIVE_PREFIX)) {
        return;
      }

      action.pathname = `${LIVE_PREFIX}${action.pathname}`;
      form.setAttribute(
        'action',
        `${action.pathname}${action.search}${action.hash}`);
    });

    return '<!DOCTYPE html>\n' + parsed.documentElement.outerHTML;
  }

  async function hydrateFromLiveApplication() {
    if (!shouldHydrateFromLiveApplication()) return;

    lockInteractiveSnapshot();

    if (liveHydrationRoutes.has(normalizePath(window.location.pathname))) {
      showStatus('preparing');
    }

    const ready = await ensureBackendReady();
    if (!ready) {
      showStatus('delayed');
      return;
    }

    try {
      const response = await fetch(liveBridgeUrl(), {
        method: 'GET',
        cache: 'no-store',
        credentials: 'same-origin',
        headers: {
          Accept: 'text/html',
          'X-Edulytics-Frontdoor': 'hydrate'
        }
      });

      if (!response.ok) throw new Error('live hydration failed');

      const html = rewritePostFormsToLiveBridge(await response.text());
      document.open();
      document.write(html);
      document.close();
    } catch {
      showStatus('delayed');
    }
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

    // Every static public page wakes the free application immediately.
    // Interactive snapshots then hydrate from the live application only after
    // readiness, so the visitor never sees Render's cold-start loading page.
    ensureBackendReady();
    hydrateFromLiveApplication();
  });
})();
