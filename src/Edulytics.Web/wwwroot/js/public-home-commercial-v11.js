(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  const supported = ['en', 'pl', 'ar'];
  const storageKey = 'edulytics.public.siteLanguage';
  const serverLanguage = (document.documentElement.lang || 'en').toLowerCase().startsWith('pl') ? 'pl' : 'en';

  const readStoredLanguage = () => {
    try {
      const value = window.localStorage.getItem(storageKey);
      return supported.includes(value) ? value : null;
    } catch {
      return null;
    }
  };

  const storeLanguage = value => {
    try {
      if (value === 'ar') window.localStorage.setItem(storageKey, value);
      else window.localStorage.removeItem(storageKey);
    } catch {
      // Storage can be unavailable in strict privacy modes. Server language still works for EN/PL.
    }
  };

  const language = readStoredLanguage() === 'ar' ? 'ar' : serverLanguage;
  root.dataset.siteLanguage = language;
  root.classList.toggle('is-site-ar', language === 'ar');

  const sharedStrengths = {
    en: [
      ['Learning Built for Understanding', '#experience'],
      ['Results Backed by Data', '#platform'],
      ['Support You Can Rely On', '/contact']
    ],
    pl: [
      ['Nauka oparta na zrozumieniu', '#experience'],
      ['Wyniki potwierdzone danymi', '#platform'],
      ['Wsparcie, na którym możesz polegać', '/contact']
    ],
    ar: [
      ['تعلّم قائم على الفهم', '#experience'],
      ['نتائج تثبتها البيانات', '#platform'],
      ['دعم يمكنك الاعتماد عليه', '/contact']
    ]
  };

  const copy = {
    en: {
      nav: { product: 'Product', teachers: 'Teachers', parents: 'Parents', leaders: 'Schools & Education Leaders', contact: 'Contact', login: 'Log in', try: 'Try Edulytics' },
      labels: { apart: 'What sets us apart', explore: 'Ways to Explore', subjects: 'Subjects', overview: 'Overview', teaching: 'Teaching with Edulytics', learning: 'Learning with Edulytics', transform: 'Transform Education' },
      product: {
        leadTitle: 'Explore Edulytics',
        leadBody: 'Discover learning, assessment, progress and the tools that connect them in one mathematics platform.',
        groups: [
          ['What sets us apart', sharedStrengths.en],
          ['Ways to Explore', [['Explore Student Portal', '#experience'], ['Math Quizzes', '#ai'], ['Weekly Challenge', '#experience']]],
          ['Subjects', [['Mathematics', '#curricula'], ['Financial Literacy', '#experience']]],
          ['Overview', [['Product Features', '#platform'], ['AI Assistant', '#ai'], ['Multilingual Editions', '#curricula'], ['Technical Requirements', '/contact']]]
        ]
      },
      teachers: {
        leadTitle: 'Start as a Teacher',
        leadBody: 'Bring engaging, evidence-informed mathematics learning and efficient assessment into your classroom.',
        groups: [
          ['What sets us apart', sharedStrengths.en],
          ['Teaching with Edulytics', [['Why Edulytics for Teachers?', '#teachers'], ['AI Assistant', '#ai'], ['Activities & Curriculum', '#curricula']]]
        ]
      },
      parents: {
        leadTitle: 'Start as a Parent',
        leadBody: 'Support your child with engaging mathematics learning and clearer visibility into progress.',
        groups: [
          ['What sets us apart', [...sharedStrengths.en, ['Explore Student Portal', '#experience']]],
          ['Learning with Edulytics', [['Why Edulytics for Home?', '#parents'], ['Activities & Curriculum', '#curricula']]]
        ]
      },
      leaders: {
        leadTitle: 'Start as an Education Leader',
        leadBody: 'Connect curriculum, teaching, assessment and learning evidence across your school or education network.',
        groups: [
          ['What sets us apart', sharedStrengths.en],
          ['Transform Education', [['Why Edulytics for Education Leaders?', '#schools'], ['AI for Teachers', '#ai'], ['Global Partnerships', '/contact']]]
        ]
      }
    },
    pl: {
      nav: { product: 'Produkt', teachers: 'Nauczyciele', parents: 'Rodzice', leaders: 'Szkoły i liderzy edukacji', contact: 'Kontakt', login: 'Zaloguj się', try: 'Wypróbuj Edulytics' },
      labels: { apart: 'Co nas wyróżnia', explore: 'Sposoby odkrywania', subjects: 'Tematy', overview: 'Przegląd', teaching: 'Nauczanie z Edulytics', learning: 'Nauka z Edulytics', transform: 'Transformacja edukacji' },
      product: {
        leadTitle: 'Poznaj Edulytics',
        leadBody: 'Poznaj naukę, ocenianie, postępy i narzędzia, które łączą je w jednej platformie matematycznej.',
        groups: [
          ['Co nas wyróżnia', sharedStrengths.pl],
          ['Sposoby odkrywania', [['Poznaj portal ucznia', '#experience'], ['Quizy matematyczne', '#ai'], ['Cotygodniowe wyzwanie', '#experience']]],
          ['Tematy', [['Matematyka', '#curricula'], ['Edukacja finansowa', '#experience']]],
          ['Przegląd', [['Funkcje produktu', '#platform'], ['Asystent AI', '#ai'], ['Wersje wielojęzyczne', '#curricula'], ['Wymagania techniczne', '/contact']]]
        ]
      },
      teachers: {
        leadTitle: 'Zacznij jako nauczyciel',
        leadBody: 'Wprowadź do swojej klasy angażującą naukę matematyki i sprawniejsze ocenianie oparte na danych.',
        groups: [
          ['Co nas wyróżnia', sharedStrengths.pl],
          ['Nauczanie z Edulytics', [['Dlaczego Edulytics dla nauczycieli?', '#teachers'], ['Asystent AI', '#ai'], ['Aktywności i program nauczania', '#curricula']]]
        ]
      },
      parents: {
        leadTitle: 'Zacznij jako rodzic',
        leadBody: 'Wspieraj dziecko w nauce matematyki i lepiej rozumiej jego postępy.',
        groups: [
          ['Co nas wyróżnia', [...sharedStrengths.pl, ['Poznaj portal ucznia', '#experience']]],
          ['Nauka z Edulytics', [['Dlaczego Edulytics w domu?', '#parents'], ['Aktywności i program nauczania', '#curricula']]]
        ]
      },
      leaders: {
        leadTitle: 'Zacznij jako lider edukacji',
        leadBody: 'Połącz program, nauczanie, ocenianie i dane o uczeniu się w szkole lub sieci edukacyjnej.',
        groups: [
          ['Co nas wyróżnia', sharedStrengths.pl],
          ['Transformacja edukacji', [['Dlaczego Edulytics dla liderów edukacji?', '#schools'], ['AI dla nauczycieli', '#ai'], ['Partnerstwa globalne', '/contact']]]
        ]
      }
    },
    ar: {
      nav: { product: 'المنتج', teachers: 'المعلمون', parents: 'أولياء الأمور', leaders: 'المدارس والقيادات التعليمية', contact: 'تواصل معنا', login: 'تسجيل الدخول', try: 'جرّب Edulytics' },
      labels: { apart: 'ما الذي يميزنا؟', explore: 'طرق الاستكشاف', subjects: 'المواضيع', overview: 'نظرة عامة', teaching: 'التدريس باستخدام Edulytics', learning: 'التعلّم مع Edulytics', transform: 'تطوير التعليم' },
      product: {
        leadTitle: 'استكشف Edulytics',
        leadBody: 'تعرّف على التعلّم والتقييم والتقدّم والأدوات التي تربطها في منصة واحدة للرياضيات.',
        groups: [
          ['ما الذي يميزنا؟', sharedStrengths.ar],
          ['طرق الاستكشاف', [['استكشف بوابة الطالب', '#experience'], ['اختبارات الرياضيات', '#ai'], ['التحدي الأسبوعي', '#experience']]],
          ['المواضيع', [['الرياضيات', '#curricula'], ['المعرفة المالية', '#experience']]],
          ['نظرة عامة', [['ميزات المنتج', '#platform'], ['مساعد الذكاء الاصطناعي', '#ai'], ['إصدارات متعددة اللغات', '#curricula'], ['المتطلبات الفنية', '/contact']]]
        ]
      },
      teachers: {
        leadTitle: 'ابدأ كمعلم',
        leadBody: 'أثرِ صفك بتعلّم رياضيات أكثر تفاعلًا وتقييم أكثر كفاءة قائم على البيانات.',
        groups: [
          ['ما الذي يميزنا؟', sharedStrengths.ar],
          ['التدريس باستخدام Edulytics', [['لماذا Edulytics للمعلمين؟', '#teachers'], ['مساعد الذكاء الاصطناعي', '#ai'], ['الأنشطة والمناهج الدراسية', '#curricula']]]
        ]
      },
      parents: {
        leadTitle: 'ابدأ كولي أمر',
        leadBody: 'ادعم تعلّم طفلك للرياضيات وافهم تقدّمه بصورة أوضح.',
        groups: [
          ['ما الذي يميزنا؟', [...sharedStrengths.ar, ['استكشف بوابة الطالب', '#experience']]],
          ['التعلّم مع Edulytics', [['لماذا Edulytics للمنزل؟', '#parents'], ['الأنشطة والمناهج الدراسية', '#curricula']]]
        ]
      },
      leaders: {
        leadTitle: 'ابدأ كقائد تعليمي',
        leadBody: 'اربط المناهج والتدريس والتقييم وبيانات التعلّم عبر مدرستك أو منظومتك التعليمية.',
        groups: [
          ['ما الذي يميزنا؟', sharedStrengths.ar],
          ['تطوير التعليم', [['لماذا Edulytics للقيادات التعليمية؟', '#schools'], ['الذكاء الاصطناعي للمعلمين', '#ai'], ['الشراكات العالمية', '/contact']]]
        ]
      }
    }
  };

  const text = copy[language];

  const escapeHtml = value => String(value)
    .replaceAll('&', '&amp;')
    .replaceAll('<', '&lt;')
    .replaceAll('>', '&gt;')
    .replaceAll('"', '&quot;')
    .replaceAll("'", '&#039;');

  const groupHtml = ([title, links]) => `
    <section class="ed-home-v11-mega-group">
      <h3>${escapeHtml(title)}</h3>
      ${links.map(([label, href]) => `<a href="${escapeHtml(href)}">${escapeHtml(label)}</a>`).join('')}
    </section>`;

  const megaHtml = (key, data) => `
    <div class="ed-home-v11-mega ${key === 'product' ? 'is-product' : ''}" id="ed-home-v11-mega-${key}">
      <div class="ed-home-v11-mega-grid">
        <section class="ed-home-v11-mega-lead">
          <strong>${escapeHtml(data.leadTitle)}</strong>
          <p>${escapeHtml(data.leadBody)}</p>
        </section>
        ${data.groups.map(groupHtml).join('')}
      </div>
    </div>`;

  const desktopNav = root.querySelector('.ed-home-links');
  if (desktopNav) {
    const items = [
      ['product', text.nav.product, text.product],
      ['teachers', text.nav.teachers, text.teachers],
      ['parents', text.nav.parents, text.parents],
      ['leaders', text.nav.leaders, text.leaders]
    ];

    desktopNav.classList.add('ed-home-v11-links');
    desktopNav.innerHTML = items.map(([key, label, data]) => `
      <div class="ed-home-v11-nav-item" data-v11-menu="${key}">
        <button type="button" class="ed-home-v11-nav-trigger" aria-expanded="false" aria-controls="ed-home-v11-mega-${key}">
          <span>${escapeHtml(label)}</span><span class="ed-home-v11-chevron" aria-hidden="true">▾</span>
        </button>
        ${megaHtml(key, data)}
      </div>`).join('') + `<a class="ed-home-v11-contact-link" href="/contact">${escapeHtml(text.nav.contact)}</a>`;
  }

  const closeDesktopMenus = except => {
    root.querySelectorAll('.ed-home-v11-nav-item').forEach(item => {
      if (item === except) return;
      item.classList.remove('is-open');
      item.querySelector('.ed-home-v11-nav-trigger')?.setAttribute('aria-expanded', 'false');
    });
  };

  root.querySelectorAll('.ed-home-v11-nav-trigger').forEach(trigger => {
    trigger.addEventListener('click', event => {
      event.stopPropagation();
      const item = trigger.closest('.ed-home-v11-nav-item');
      const opening = !item.classList.contains('is-open');
      closeDesktopMenus(item);
      item.classList.toggle('is-open', opening);
      trigger.setAttribute('aria-expanded', opening ? 'true' : 'false');
    });
  });

  document.addEventListener('click', event => {
    if (!event.target.closest('.ed-home-v11-nav-item')) closeDesktopMenus();
  });

  document.addEventListener('keydown', event => {
    if (event.key === 'Escape') closeDesktopMenus();
  });

  const switchLanguage = target => {
    if (!supported.includes(target)) return;
    if (target === 'ar') {
      storeLanguage('ar');
      window.location.reload();
      return;
    }

    storeLanguage(target);
    const form = [...root.querySelectorAll('.ed-home-lang form')]
      .find(candidate => candidate.querySelector('input[name="culture"]')?.value === target);
    if (form) form.requestSubmit();
  };

  const languageWrap = root.querySelector('.ed-home-lang');
  if (languageWrap) {
    languageWrap.querySelectorAll('button').forEach(button => button.classList.remove('is-active'));
    const existingTarget = languageWrap.querySelector(`input[name="culture"][value="${language}"]`)?.closest('form')?.querySelector('button');
    if (existingTarget) existingTarget.classList.add('is-active');

    if (!languageWrap.querySelector('[data-v11-language="ar"]')) {
      const arButton = document.createElement('button');
      arButton.type = 'button';
      arButton.className = `ed-home-v11-lang-button${language === 'ar' ? ' is-active' : ''}`;
      arButton.dataset.v11Language = 'ar';
      arButton.textContent = 'AR';
      arButton.setAttribute('aria-label', 'العربية');
      arButton.addEventListener('click', () => switchLanguage('ar'));
      languageWrap.appendChild(arButton);
    }

    languageWrap.querySelectorAll('form button').forEach(button => {
      const target = button.closest('form')?.querySelector('input[name="culture"]')?.value;
      button.addEventListener('click', () => target && storeLanguage(target));
    });
  }

  const langLabel = root.querySelector('.ed-home-lang-label');
  if (langLabel) langLabel.textContent = language.toUpperCase();
  const polishFlag = root.querySelector('.ed-home-flag');
  if (polishFlag) polishFlag.hidden = language !== 'pl';

  if (language === 'ar') {
    const login = root.querySelector('.ed-home-actions>.ed-home-login');
    const topCta = root.querySelector('.ed-home-actions>.ed-home-cta');
    if (login) login.textContent = text.nav.login;
    if (topCta) text.nav.try && (topCta.textContent = text.nav.try);
  }

  const mobilePanel = root.querySelector('.ed-home-mobile-panel');
  if (mobilePanel) {
    const mobileGroup = (label, data) => `
      <details class="ed-home-v11-mobile-group">
        <summary>${escapeHtml(label)}<span aria-hidden="true">＋</span></summary>
        <div class="ed-home-v11-mobile-submenu">
          <strong>${escapeHtml(data.leadTitle)}</strong>
          ${data.groups.map(([title, links]) => `<strong>${escapeHtml(title)}</strong>${links.map(([itemLabel, href]) => `<a href="${escapeHtml(href)}">${escapeHtml(itemLabel)}</a>`).join('')}`).join('')}
        </div>
      </details>`;

    mobilePanel.innerHTML =
      mobileGroup(text.nav.product, text.product) +
      mobileGroup(text.nav.teachers, text.teachers) +
      mobileGroup(text.nav.parents, text.parents) +
      mobileGroup(text.nav.leaders, text.leaders) +
      `<a href="/contact">${escapeHtml(text.nav.contact)}</a>
       <div class="ed-home-v11-mobile-actions">
         <div class="ed-home-v11-mobile-languages" aria-label="Website language">
           <button type="button" data-v11-mobile-lang="en" class="${language === 'en' ? 'is-active' : ''}">EN</button>
           <button type="button" data-v11-mobile-lang="pl" class="${language === 'pl' ? 'is-active' : ''}">PL</button>
           <button type="button" data-v11-mobile-lang="ar" class="${language === 'ar' ? 'is-active' : ''}">AR</button>
         </div>
         <a class="ed-home-v11-mobile-login" href="/Account/Login">${escapeHtml(text.nav.login)}</a>
         <a class="ed-home-v11-mobile-cta" href="/contact">${escapeHtml(text.nav.try)}</a>
       </div>`;

    mobilePanel.querySelectorAll('[data-v11-mobile-lang]').forEach(button => {
      button.addEventListener('click', () => switchLanguage(button.dataset.v11MobileLang));
    });
  }

  const hero = root.querySelector('.ed-home-v6-hero');
  if (!hero) return;

  const visual = hero.querySelector('.ed-home-v6-visual');
  const dots = [...hero.querySelectorAll('.ed-home-v6-dot')];
  const next = hero.querySelector('.ed-home-v6-next');
  const prev = hero.querySelector('.ed-home-v6-prev');
  if (!visual || dots.length < 2 || !next || !prev) return;

  const brightPhotoUrl = 'https://images.unsplash.com/photo-1509062522246-3755977927d7?auto=format&fit=crop&q=92&w=1800';
  const mascotSeed = '<div class="ed-home-v6-cartoon" aria-label="Edulytics mascot family"><div class="ed-home-v11-mascot-seed" aria-hidden="true"></div></div>';

  const arabicSlides = [
    {
      kicker: 'الرياضيات للحياة اليومية',
      title: 'رياضيات يستطيع الطلاب استخدامها فعلًا.',
      body: 'تجربة تعلّم تفاعلية تحوّل الأفكار الرياضية ومهارات إدارة المال إلى تحديات واقعية جذابة، وتساعد الطلاب على التفكير واتخاذ قرارات مالية أذكى وبناء مهاراتهم بثقة.',
      note: '',
      primary: 'جرّب Edulytics',
      secondary: 'تحدث معنا'
    },
    {
      kicker: 'دعم المعلمين • البيانات • الذكاء الاصطناعي',
      title: 'مكّن المعلمين. وابنِ مستقبلًا أفضل للتعلّم.',
      body: 'نجمع بين التدريس المستند إلى البحث وبيانات التعلّم والذكاء الاصطناعي لمساعدة المعلمين على فهم احتياجات كل طالب بصورة أفضل، واتخاذ قرارات تعليمية أدق، وتقديم تعلّم رياضيات أكثر تخصيصًا.',
      note: 'يبقى المعلم مسؤولًا عن التقييم والنشر.',
      primary: 'أولياء الأمور — جرّب مجانًا',
      secondary: 'للمعلمين — اسأل الآن'
    }
  ];

  const activeIndex = () => {
    const index = dots.findIndex(dot => dot.classList.contains('is-active'));
    return index < 0 ? 0 : index;
  };

  const applyArabicHeroCopy = index => {
    if (language !== 'ar') return;
    const slide = arabicSlides[index] || arabicSlides[0];
    const kicker = hero.querySelector('.ed-home-v6-kicker');
    const title = hero.querySelector('.ed-home-v6-copy h1');
    const body = hero.querySelector('.ed-home-v6-body');
    const note = hero.querySelector('.ed-home-v6-note');
    const primary = hero.querySelector('.ed-home-v6-primary');
    const secondary = hero.querySelector('.ed-home-v6-secondary');
    if (kicker) kicker.textContent = slide.kicker;
    if (title) title.textContent = slide.title;
    if (body) body.textContent = slide.body;
    if (note) {
      note.textContent = slide.note;
      note.hidden = !slide.note;
    }
    if (primary) primary.textContent = slide.primary;
    if (secondary) secondary.textContent = slide.secondary;
  };

  const replayMotion = () => {
    hero.classList.remove('ed-home-v11-enter');
    void hero.offsetWidth;
    hero.classList.add('ed-home-v11-enter');
  };

  const syncHero = () => {
    const index = activeIndex();
    if (index === 0) {
      visual.innerHTML = mascotSeed;
    } else {
      visual.innerHTML = `<div class="ed-home-v6-photo ed-home-v11-bright-photo" style="background-image:url('${brightPhotoUrl}')" role="img" aria-label="${language === 'ar' ? 'طلاب يتعلمون في فصل دراسي' : language === 'pl' ? 'Uczniowie podczas nauki w jasnej klasie' : 'Students learning in a bright classroom'}"></div>`;
    }
    applyArabicHeroCopy(index);
    replayMotion();
  };

  const dotObserver = new MutationObserver(() => queueMicrotask(syncHero));
  dots.forEach(dot => dotObserver.observe(dot, { attributes: true, attributeFilter: ['class'] }));

  let touchStartX = null;
  let touchStartY = null;
  hero.addEventListener('touchstart', event => {
    const touch = event.changedTouches?.[0];
    if (!touch) return;
    touchStartX = touch.clientX;
    touchStartY = touch.clientY;
  }, { passive: true });

  hero.addEventListener('touchend', event => {
    const touch = event.changedTouches?.[0];
    if (!touch || touchStartX === null || touchStartY === null) return;
    const dx = touch.clientX - touchStartX;
    const dy = touch.clientY - touchStartY;
    touchStartX = null;
    touchStartY = null;
    if (Math.abs(dx) < 52 || Math.abs(dx) < Math.abs(dy)) return;
    if (dx < 0) next.click();
    else prev.click();
  }, { passive: true });

  window.setTimeout(syncHero, 0);

  window.addEventListener('pagehide', () => dotObserver.disconnect(), { once: true });
})();
