(() => {
  const root = document.querySelector('.ed-content-page');
  if (!root) return;

  const storageKey = 'edulytics.public.siteLanguage';
  const serverLanguage = (document.documentElement.lang || 'en').toLowerCase().startsWith('pl') ? 'pl' : 'en';
  let storedLanguage = null;
  try { storedLanguage = window.localStorage.getItem(storageKey); } catch { /* no-op */ }
  const language = storedLanguage === 'ar' ? 'ar' : serverLanguage;
  const pageKey = root.dataset.marketingPage || window.location.pathname.replace(/^\//, '').replace(/\/$/, '');
  const catalog = window.EdulyticsPublicPages || {};
  const page = catalog[language]?.[pageKey] || catalog.en?.[pageKey];
  if (!page) return;

  const ui = {
    en: {home:'Home', request:'Request a demo', back:'Back to home'},
    pl: {home:'Strona główna', request:'Poproś o demo', back:'Wróć na stronę główną'},
    ar: {home:'الرئيسية', request:'اطلب عرضًا تجريبيًا', back:'العودة إلى الرئيسية'}
  }[language];

  const relatedRoutes = {
    'product/results-backed-by-data':'/product/mastery-and-next-step',
    'product/support-you-can-rely-on':'/contact',
    'product/student-portal':'/product/mastery-and-next-step',
    'product/assessment-and-practice':'/product/edulytics-ai',
    'product/mastery-and-next-step':'/product/results-backed-by-data',
    'product/mathematics':'/product/curricula',
    'product/curricula':'/product/mathematics',
    'product/features':'/contact',
    'product/edulytics-ai':'/product/assessment-and-practice',
    'product/languages':'/contact',
    'product/technical-requirements':'/contact',
    'teachers/overview':'/teachers/assessment-and-curriculum',
    'teachers/assessment-and-curriculum':'/product/edulytics-ai',
    'parents/overview':'/product/student-portal',
    'schools/overview':'/contact',
    'students/overview':'/product/student-portal',
    'company/partnerships':'/contact',
    'company/about':'/product/features'
  };

  root.dataset.pageVariant = page.variant || 'product';
  root.style.setProperty('--page-accent', page.accent || '#286df0');
  root.style.setProperty('--page-accent-2', page.accent2 || '#7651f5');
  root.style.setProperty('--page-soft', `color-mix(in srgb, ${page.accent || '#286df0'} 7%, #ffffff)`);
  root.dataset.siteLanguage = language;
  root.classList.toggle('is-site-ar', language === 'ar');
  document.documentElement.dir = language === 'ar' ? 'rtl' : 'ltr';
  document.documentElement.lang = language;
  document.title = `${page.title} | Edulytics`;
  const meta = document.querySelector('meta[name="description"]');
  if (meta) meta.setAttribute('content', page.body);

  const setText = (key, value) => {
    root.querySelectorAll(`[data-page-key="${key}"]`).forEach(node => { node.textContent = value ?? ''; });
  };

  setText('breadcrumbHome', ui.home);
  setText('breadcrumbSection', page.section);
  setText('breadcrumbCurrent', page.title);
  setText('heroKicker', page.kicker);
  setText('heroTitle', page.title);
  setText('heroBody', page.body);
  setText('heroPrimary', page.primary);
  setText('heroSecondary', page.secondary);
  setText('visualHub', page.hub);
  ['A','B','C','D'].forEach((letter, index) => setText(`visual${letter}`, page.visual[index] || ''));
  setText('storyKicker', page.story.k);
  setText('storyTitle', page.story.t);
  setText('storyBody', page.story.b);
  setText('focusKicker', page.focus.k);
  setText('focusTitle', page.focus.t);
  setText('focusBody', page.focus.b);
  setText('flowKicker', page.flow.k);
  setText('flowTitle', page.flow.t);
  setText('flowBody', page.flow.b);
  setText('deepKicker', page.deep.k);
  setText('deepTitle', page.deep.t);
  setText('deepBody', page.deep.b);

  const roleTitles = {
    en:'Who it helps', pl:'Dla kogo', ar:'لمن يفيد'
  };
  const roleBodies = {
    en:'Different users see the information and actions that belong to their responsibility.',
    pl:'Różni użytkownicy widzą informacje i działania zgodne ze swoją odpowiedzialnością.',
    ar:'يرى كل مستخدم المعلومات والإجراءات المرتبطة بمسؤوليته.'
  };
  const roleKickers = {en:'WHO IT HELPS',pl:'DLA KOGO',ar:'لمن يفيد'};
  setText('roleKicker', roleKickers[language]);
  setText('roleTitle', roleTitles[language]);
  setText('roleBody', roleBodies[language]);
  setText('finalTitle', page.final.t);
  setText('finalBody', page.final.b);
  setText('finalPrimary', ui.request);
  setText('finalSecondary', ui.back);

  const heroPrimary = root.querySelector('[data-page-key="heroPrimary"]');
  if (heroPrimary) heroPrimary.setAttribute('href', '#page-story');
  const heroSecondary = root.querySelector('[data-page-key="heroSecondary"]');
  if (heroSecondary) heroSecondary.setAttribute('href', relatedRoutes[pageKey] || '/contact');
  const finalSecondary = root.querySelector('[data-page-key="finalSecondary"]');
  if (finalSecondary) finalSecondary.setAttribute('href', '/');

  const escapeHtml = value => String(value ?? '')
    .replaceAll('&','&amp;')
    .replaceAll('<','&lt;')
    .replaceAll('>','&gt;')
    .replaceAll('"','&quot;')
    .replaceAll("'",'&#039;');

  const cardIcons = ['◎','▤','→'];
  const cards = root.querySelector('[data-page-render="cards"]');
  if (cards) cards.innerHTML = page.cards.map((item,index) => `
    <article class="ed-content-card" data-index="0${index + 1}">
      <div class="ed-content-card-icon" aria-hidden="true">${cardIcons[index] || '•'}</div>
      <h3>${escapeHtml(item[0])}</h3>
      <p>${escapeHtml(item[1])}</p>
    </article>`).join('');

  const renderPoints = (selector, points) => {
    const host = root.querySelector(`[data-page-render="${selector}"]`);
    if (!host) return;
    host.innerHTML = points.map(point => `<div class="ed-content-point">${escapeHtml(point)}</div>`).join('');
  };
  renderPoints('focusPoints', page.focus.p);
  renderPoints('deepPoints', page.deep.p);

  const evidence = root.querySelector('[data-page-render="evidence"]');
  if (evidence) evidence.innerHTML = page.cards.map((item,index) => `
    <div class="ed-content-evidence-row">
      <span class="ed-content-evidence-index">0${index + 1}</span>
      <div class="ed-content-evidence-copy"><strong>${escapeHtml(item[0])}</strong><small>${escapeHtml(item[1])}</small></div>
      <span class="ed-content-evidence-value">✓</span>
    </div>`).join('');

  const flow = root.querySelector('[data-page-render="flow"]');
  if (flow) flow.innerHTML = page.flow.s.map((item,index) => `
    <article class="ed-content-flow-step">
      <b>0${index + 1}</b>
      <strong>${escapeHtml(item[0])}</strong>
      <small>${escapeHtml(item[1])}</small>
    </article>`).join('');

  const context = root.querySelector('[data-page-render="context"]');
  if (context) {
    const contextItems = [
      [page.story.k, page.story.t],
      [page.focus.k, page.focus.t],
      [page.flow.k, page.flow.t],
      [page.deep.k, page.deep.t]
    ];
    context.innerHTML = contextItems.map((item,index) => `
      <article class="ed-content-context-tile">
        <b>0${index + 1}</b>
        <strong>${escapeHtml(item[0])}</strong>
        <span>${escapeHtml(item[1])}</span>
      </article>`).join('');
  }

  const roles = root.querySelector('[data-page-render="roles"]');
  if (roles) roles.innerHTML = page.roles.map((item,index) => `
    <article class="ed-content-role-card">
      <b>0${index + 1}</b>
      <h3>${escapeHtml(item[0])}</h3>
      <p>${escapeHtml(item[1])}</p>
    </article>`).join('');

  root.classList.add('is-ready');
})();
