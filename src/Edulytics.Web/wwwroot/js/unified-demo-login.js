(() => {
  'use strict';

  // A single public login form serves existing schools and the two permanent
  // demonstration schools. Only these explicitly provisioned demo email
  // namespaces go through the isolated, same-origin demo application.
  const demoSchoolPrefixes = [
    'cambridge-primary.',
    'cambridge-middle.'
  ];
  const form = document.getElementById('login-form');
  if (!form) return;

  let routing = false;
  form.addEventListener('submit', async event => {
    if (routing) return;
    const emailField = form.querySelector('input[name="Email"]');
    const email = (emailField?.value || '').trim().toLowerCase();
    const isDemo = demoSchoolPrefixes.some(prefix =>
      email.startsWith(prefix) && email.endsWith('@edulytiks.com'));
    if (!isDemo) return;

    event.preventDefault();
    const submit = form.querySelector('[type="submit"]');
    if (submit) submit.disabled = true;
    try {
      const initial = new URL(form.action, window.location.href);
      const culture = ['en', 'pl', 'ar'].includes(
        initial.searchParams.get('culture'))
        ? initial.searchParams.get('culture')
        : 'en';
      const demoPath = '/__frontdoor-live/demo/account/login';
      const destination = demoPath + '?culture=' + encodeURIComponent(culture);
      // Demo antiforgery tokens are cryptographically scoped to the demo
      // service, so acquire the matching token/cookie before POST.
      const response = await fetch(destination, {
        method: 'GET',
        credentials: 'same-origin',
        cache: 'no-store',
        redirect: 'follow'
      });
      if (!response.ok || new URL(response.url).origin !== location.origin)
        throw new Error('Demo sign-in unavailable');

      const html = new DOMParser().parseFromString(
        await response.text(), 'text/html');
      const token = html.querySelector(
        '#login-form input[name="__RequestVerificationToken"]');
      if (!token || !token.value)
        throw new Error('Demo security token unavailable');

      const field = form.querySelector(
        'input[name="__RequestVerificationToken"]');
      if (!field) throw new Error('Form security token unavailable');

      field.value = token.value;

      // Exchange the demo credential on the SAME official origin. A native
      // form POST would expose /__frontdoor-live/demo in the address bar.
      // Resolve the authenticated destination and navigate to its ORIGINAL
      // application path instead.
      const credentials = new URLSearchParams(new FormData(form));
      const signedIn = await fetch(destination, {
        method: 'POST',
        credentials: 'same-origin',
        body: credentials,
        cache: 'no-store',
        redirect: 'follow'
      });
      const finalUrl = new URL(signedIn.url, location.href);
      const prefix = '/__frontdoor-live/demo';
      if (!signedIn.ok || finalUrl.origin !== location.origin ||
          !finalUrl.pathname.startsWith(prefix + '/') ||
          finalUrl.pathname.toLowerCase().includes('/account/login')) {
        throw new Error('Demo credentials rejected');
      }

      const originalRoute = finalUrl.pathname.slice(prefix.length);
      if (!originalRoute.startsWith('/student/') &&
          !originalRoute.startsWith('/school/') &&
          !originalRoute.startsWith('/platform/')) {
        throw new Error('Unexpected demo redirect');
      }
      routing = true;
      window.location.assign(
        originalRoute + finalUrl.search + finalUrl.hash);
    } catch (_) {
      routing = false;
      if (submit) submit.disabled = false;
      const message = document.createElement('p');
      message.setAttribute('role', 'alert');
      message.className = 'field-validation';
      message.textContent = cultureError();
      form.prepend(message);
    }
  });

  function cultureError() {
    const lang = new URL(form.action, location.href)
      .searchParams.get('culture');
    return lang === 'ar'
      ? 'تعذّر الاتصال ببيئة العرض. يرجى المحاولة مرة أخرى.'
      : lang === 'pl'
        ? 'Nie można połączyć z kontem demo. Spróbuj ponownie.'
        : 'The demo account is temporarily unavailable. Please try again.';
  }
})();
