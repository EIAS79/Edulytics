(() => {
  const root = document.querySelector('.ed-legal-page');
  if (!root) return;

  const frontdoorLanguageKey = 'edulytics.frontdoor.language';
  const legacyLanguageKey = 'edulytics.public.siteLanguage';
  const documentLanguage = (document.documentElement.lang || 'en').toLowerCase();
  let language = documentLanguage.startsWith('ar')
    ? 'ar'
    : documentLanguage.startsWith('pl')
      ? 'pl'
      : 'en';

  try {
    const selected = window.localStorage.getItem(frontdoorLanguageKey)
      || window.localStorage.getItem(legacyLanguageKey);
    if (selected === 'en' || selected === 'pl' || selected === 'ar') {
      language = selected;
    }
  } catch {
    // The server/static snapshot language remains the fallback.
  }

  document.documentElement.lang = language;
  document.documentElement.dir = language === 'ar' ? 'rtl' : 'ltr';
  document.documentElement.classList.toggle('ed-site-ar', language === 'ar');
  root.classList.toggle('is-site-ar', language === 'ar');

  if (language !== 'ar') {
    root.querySelectorAll('[data-public-l10n]').forEach(node => {
      const value = node.dataset[language];
      if (typeof value === 'string') node.textContent = value;
    });
  } else {
    const eyebrow = root.querySelector('.ed-legal-eyebrow');
    if (eyebrow) eyebrow.textContent = 'معلومات قانونية';
  }

  const title = language === 'ar'
    ? root.dataset.legalTitleAr
    : language === 'pl'
      ? root.dataset.legalTitlePl
      : root.dataset.legalTitleEn;

  if (title) {
    document.title = `${title} | ${language === 'pl' ? 'Edulityks' : 'Edulytics'}`;
  }
})();