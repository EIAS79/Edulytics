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
      form.action = destination;
      routing = true;
      // Submit the original role/email/password fields as an ordinary browser
      // form POST. Never send credentials to a third-party origin.
      HTMLFormElement.prototype.submit.call(form);
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
