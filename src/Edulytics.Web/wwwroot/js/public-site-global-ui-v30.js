(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  const storageKey = 'edulytics.public.siteLanguage';
  const serverLanguage = (document.documentElement.lang || 'en').toLowerCase().startsWith('pl') ? 'pl' : 'en';
  let storedLanguage = null;
  try { storedLanguage = window.localStorage.getItem(storageKey); } catch { /* storage may be blocked */ }
  const language = storedLanguage === 'ar' ? 'ar' : serverLanguage;

  const normalizePath = href => {
    if (!href) return '';
    try {
      const url = new URL(href, window.location.origin);
      if (url.origin !== window.location.origin) return href;
      return `${url.pathname}${url.hash || ''}`;
    } catch {
      return href;
    }
  };

  const contactRouteFor = (anchor, href) => {
    const haystack = `${href} ${anchor.textContent || ''}`.toLowerCase();
    if (/demo|prezent|عرض|تجرب/.test(haystack)) return '/contact/request-demo';
    if (/support|help|pomoc|دعم|مساعدة/.test(haystack)) return '/contact/support';
    if (/sales|sprzeda|مبيعات/.test(haystack)) return '/contact/sales-enquiry';
    return '/contact/message';
  };

  // No public-page email action is allowed to hand the visitor to an OS mail client.
  root.querySelectorAll('a[href^="mailto:"]').forEach(anchor => {
    anchor.setAttribute('href', contactRouteFor(anchor, anchor.getAttribute('href') || ''));
  });

  const clearArabicOverride = () => {
    try { window.localStorage.removeItem(storageKey); } catch { /* no-op */ }
  };

  const currentReturnUrl = () =>
    `${window.location.pathname}${window.location.search}${window.location.hash}` || '/';

  // EN/PL are server cultures. Keep every public language form pinned to the
  // POST endpoint even if an earlier navbar script or stale markup mutates it.
  // This prevents the browser from navigating to GET /set-culture (404).
  root.querySelectorAll('form input[name="culture"]').forEach(input => {
    const form = input.closest('form');
    if (!form || !['en', 'pl'].includes(input.value)) return;

    form.setAttribute('method', 'post');
    form.setAttribute('action', '/set-culture');

    let returnUrl = form.querySelector('input[name="returnUrl"]');
    if (!returnUrl) {
      returnUrl = document.createElement('input');
      returnUrl.type = 'hidden';
      returnUrl.name = 'returnUrl';
      form.appendChild(returnUrl);
    }
    returnUrl.value = currentReturnUrl();

    form.addEventListener('submit', () => {
      clearArabicOverride();
      form.setAttribute('method', 'post');
      form.setAttribute('action', '/set-culture');
      returnUrl.value = currentReturnUrl();
    });

    const button = form.querySelector('button');
    if (button) {
      button.type = 'submit';
      button.addEventListener('click', event => {
        event.preventDefault();
        clearArabicOverride();
        form.setAttribute('method', 'post');
        form.setAttribute('action', '/set-culture');
        returnUrl.value = currentReturnUrl();

        if (typeof form.requestSubmit === 'function') {
          form.requestSubmit();
        } else {
          HTMLFormElement.prototype.submit.call(form);
        }
      });
    }
  });

  const installArabicSwitch = languageHost => {
    if (!languageHost || languageHost.querySelector('[data-public-arabic-switch]')) return;

    const button = document.createElement('button');
    button.type = 'button';
    button.textContent = 'AR';
    button.setAttribute('data-public-arabic-switch', 'true');
    button.setAttribute('aria-label', 'العربية');
    button.classList.toggle('is-active', language === 'ar');
    button.addEventListener('click', () => {
      try { window.localStorage.setItem(storageKey, 'ar'); } catch { /* no-op */ }
      window.location.reload();
    });

    languageHost.appendChild(button);
  };

  const removeDuplicateArabicSwitches = languageHost => {
    if (!languageHost) return;
    const canonical = languageHost.querySelector('[data-public-arabic-switch]');
    if (!canonical) return;

    Array.from(languageHost.children).forEach(child => {
      if (child === canonical || child.contains(canonical)) return;
      if ((child.textContent || '').trim().toUpperCase() === 'AR') child.remove();
    });
  };

  root.querySelectorAll('.ed-home-lang').forEach(languageHost => {
    installArabicSwitch(languageHost);
    removeDuplicateArabicSwitches(languageHost);
  });

  if (language !== 'ar') return;

  document.documentElement.lang = 'ar';
  document.documentElement.dir = 'rtl';
  document.documentElement.classList.add('ed-site-ar');
  root.classList.add('is-site-ar');
  root.dataset.siteLanguage = 'ar';

  root.querySelectorAll('.ed-home-lang button').forEach(button => {
    button.classList.toggle('is-active', button.hasAttribute('data-public-arabic-switch'));
  });

  // The compact AR label beside the logo is intentionally preserved.
  root.querySelectorAll('.ed-home-lang-label').forEach(label => { label.textContent = 'AR'; });
  root.querySelectorAll('.ed-home-flag').forEach(flag => {
    flag.hidden = true;
    flag.setAttribute('aria-hidden', 'true');
  });

  const navigationLabels = new Map([
    ['/product/features', 'المنتج'],
    ['/#platform', 'المنصة'],
    ['/#schools', 'للمدارس'],
    ['/#teachers', 'للمعلمين'],
    ['/#students', 'للطلاب'],
    ['/#curricula', 'المناهج'],
    ['/#contact', 'تواصل معنا'],
    ['/#ai', 'الذكاء الاصطناعي'],
    ['/#experience', 'التجربة'],
    ['/teachers/overview', 'المعلمون'],
    ['/parents/overview', 'أولياء الأمور'],
    ['/schools/overview', 'المدارس والقيادات التعليمية'],
    ['/students/overview', 'الطلاب'],
    ['/contact', 'تواصل معنا']
  ]);

  root.querySelectorAll('.ed-home-links a, .ed-home-mobile-panel a').forEach(anchor => {
    const path = normalizePath(anchor.getAttribute('href') || '');
    const translated = navigationLabels.get(path);
    if (translated) anchor.textContent = translated;
  });

  root.querySelectorAll('.ed-home-login').forEach(link => { link.textContent = 'تسجيل الدخول'; });

  root.querySelectorAll('.ed-home-header .ed-home-cta').forEach(link => {
    const text = (link.textContent || '').trim();
    link.textContent = /try|wypróbuj/i.test(text) ? 'جرّب Edulytics' : 'اطلب عرضًا تجريبيًا';
    link.setAttribute('href', '/contact/request-demo');
  });

  const staticArabic = {
    contentSourcesTitle: 'مصادر المحتوى والتراخيص',
    contentSourcesLead: 'توثق هذه الصفحة مركزيًا مصادر المناهج، ومعلومات الحقوق المطلوبة، ولغة المحتوى الأكاديمي لكل منهج. لا يتم تكرار هذه البيانات الوصفية في صفحات الدروس الفردية.',
    academicLanguage: 'اللغة الأكاديمية',
    rightsAttribution: 'الحقوق / الإسناد المطلوب',
    reuseBasis: 'أساس إعادة الاستخدام'
  };

  root.querySelectorAll('[data-public-ar]').forEach(node => {
    const translated = staticArabic[node.dataset.publicAr];
    if (translated) node.textContent = translated;
  });

  const footer = root.querySelector('.ed-home-footer');
  if (!footer) return;

  const footerHeadings = ['المنصة', 'للمدارس', 'للمستخدمين', 'الشركة'];
  footer.querySelectorAll('.ed-home-footer-grid > div:not(.ed-home-footer-brand) > h3').forEach((heading, index) => {
    if (footerHeadings[index]) heading.textContent = footerHeadings[index];
  });

  footer.querySelector('.ed-home-footer-brand p')?.replaceChildren(document.createTextNode('الرياضيات. التعلّم. التقدم.'));

  const footerLabels = new Map([
    ['/#platform', 'المبادئ'],
    ['/product/curricula', 'المناهج'],
    ['/product/edulytics-ai', 'Edulytics AI'],
    ['/schools/overview', 'نظرة عامة'],
    ['/product/student-portal', 'تجربة الطالب'],
    ['/contact/request-demo', 'اطلب عرضًا تجريبيًا'],
    ['/teachers/overview', 'المعلمون'],
    ['/students/overview', 'الطلاب'],
    ['/account/login', 'تسجيل الدخول'],
    ['/company/about', 'عن Edulytics'],
    ['/contact', 'تواصل معنا'],
    ['/help', 'مركز المساعدة'],
    ['/legal/content-sources', 'مصادر المحتوى والتراخيص']
  ]);

  footer.querySelectorAll('a').forEach(anchor => {
    const path = normalizePath(anchor.getAttribute('href') || '');
    const translated = footerLabels.get(path);
    if (translated) anchor.textContent = translated;
  });

  const bottom = footer.querySelectorAll('.ed-home-footer-bottom > span');
  if (bottom.length > 1) bottom[1].textContent = 'الخصوصية · الشروط · تراخيص المحتوى';
})();
