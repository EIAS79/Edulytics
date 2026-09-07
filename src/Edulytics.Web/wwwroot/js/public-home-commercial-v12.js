(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  const language = root.dataset.siteLanguage || ((document.documentElement.lang || 'en').toLowerCase().startsWith('pl') ? 'pl' : 'en');
  const oldHero = root.querySelector('.ed-home-v6-hero');
  if (!oldHero) return;

  const copy = {
    en: [
      {
        kicker: 'MATHEMATICS FOR REAL LIFE',
        title: 'Mathematics students can actually use.',
        body: 'An interactive learning experience turns mathematical ideas and money-management skills into engaging real-life challenges, helping students think, make smarter financial decisions and build their skills with confidence.',
        note: '',
        primary: 'Try Edulytics',
        secondary: 'Talk to us'
      },
      {
        kicker: 'TEACHER SUPPORT • DATA • AI',
        title: 'Empower teachers. Build a better future for learning.',
        body: 'We combine research-informed teaching, learning data and artificial intelligence to help teachers understand every student’s needs, make smarter instructional decisions and deliver more personalised mathematics learning.',
        note: 'Teachers remain in control of assessment and publishing.',
        primary: 'Parents — try it free',
        secondary: 'Ask now — for teachers'
      }
    ],
    pl: [
      {
        kicker: 'MATEMATYKA W PRAWDZIWYM ŻYCIU',
        title: 'Matematyka, którą uczniowie potrafią wykorzystać.',
        body: 'Interaktywne doświadczenie edukacyjne zamienia pojęcia matematyczne i umiejętności zarządzania pieniędzmi w angażujące wyzwania z życia codziennego. Pomaga uczniom myśleć, podejmować mądrzejsze decyzje finansowe i rozwijać umiejętności z większą pewnością siebie.',
        note: '',
        primary: 'Wypróbuj Edulytics',
        secondary: 'Porozmawiaj z nami'
      },
      {
        kicker: 'WSPARCIE NAUCZYCIELI • DANE • AI',
        title: 'Wspieramy nauczycieli. Budujemy lepszą przyszłość nauki.',
        body: 'Łączymy nauczanie oparte na badaniach, analizę danych i sztuczną inteligencję, aby pomóc nauczycielom lepiej rozumieć potrzeby każdego ucznia, podejmować trafniejsze decyzje dydaktyczne i tworzyć bardziej spersonalizowaną naukę matematyki.',
        note: 'Nauczyciel zachowuje kontrolę nad ocenianiem i publikacją.',
        primary: 'Rodzice — wypróbuj bezpłatnie',
        secondary: 'Zapytaj teraz — dla nauczycieli'
      }
    ],
    ar: [
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
    ]
  };

  const slides = copy[language] || copy.en;
  const rtl = language === 'ar';

  const mascot = `
    <div class="ed-home-v12-mascot" role="img" aria-label="Edulytics cartoon mascot family">
      <svg viewBox="0 0 920 600" aria-hidden="true">
        <defs>
          <linearGradient id="mBg" x1="0" y1="0" x2="1" y2="1"><stop offset="0" stop-color="#f9fdff"/><stop offset="1" stop-color="#f3efff"/></linearGradient>
          <linearGradient id="mBlue" x1="0" y1="0" x2="1" y2="1"><stop offset="0" stop-color="#54a6ff"/><stop offset="1" stop-color="#1768f2"/></linearGradient>
          <linearGradient id="mPurple" x1="0" y1="0" x2="1" y2="1"><stop offset="0" stop-color="#a47cff"/><stop offset="1" stop-color="#7047ee"/></linearGradient>
          <linearGradient id="mGreen" x1="0" y1="0" x2="1" y2="1"><stop offset="0" stop-color="#67dca3"/><stop offset="1" stop-color="#20b775"/></linearGradient>
          <linearGradient id="mOrange" x1="0" y1="0" x2="1" y2="1"><stop offset="0" stop-color="#ffbf58"/><stop offset="1" stop-color="#ff7a1a"/></linearGradient>
          <filter id="mShadow" x="-30%" y="-30%" width="160%" height="180%"><feDropShadow dx="0" dy="14" stdDeviation="14" flood-color="#6c86aa" flood-opacity=".18"/></filter>
        </defs>
        <rect width="920" height="600" rx="28" fill="url(#mBg)"/>
        <circle cx="86" cy="82" r="23" fill="#ffd34d"/>
        <path d="M790 88l34 20-18 35-38-12z" fill="#ff8b3d"/>
        <circle cx="820" cy="492" r="25" fill="#56c98b"/>
        <path d="M65 442c38-30 77-31 116-2" stroke="#6f47ee" stroke-width="11" fill="none" stroke-linecap="round" opacity=".35"/>

        <g transform="translate(535 76)" filter="url(#mShadow)">
          <rect width="300" height="170" rx="28" fill="#fff" stroke="#dfe9f6" stroke-width="4"/>
          <text x="30" y="42" font-size="18" font-weight="900" fill="#1768f2">EDULYTICS</text>
          <text x="30" y="78" font-size="27" font-weight="900" fill="#102540">Math that adapts</text>
          <rect x="30" y="100" width="108" height="48" rx="14" fill="#eef5ff"/>
          <text x="45" y="120" font-size="11" font-weight="800" fill="#64758d">MASTERY</text>
          <text x="45" y="141" font-size="24" font-weight="900" fill="#17a46e">82%</text>
          <rect x="154" y="100" width="116" height="48" rx="14" fill="#fff4e8"/>
          <text x="169" y="120" font-size="11" font-weight="800" fill="#99612d">NEXT</text>
          <text x="169" y="141" font-size="16" font-weight="900" fill="#102540">Fractions</text>
        </g>

        <ellipse cx="447" cy="515" rx="326" ry="32" fill="#dfeaf7"/>

        <g transform="translate(92 174)" filter="url(#mShadow)">
          <ellipse cx="104" cy="72" rx="70" ry="66" fill="#fff" stroke="#b8d7ff" stroke-width="6"/>
          <path d="M56 61c15-43 80-58 108-15 10 15 12 34 5 53-30-18-78-18-113-4-7-11-8-22 0-34z" fill="url(#mBlue)"/>
          <circle cx="78" cy="72" r="11" fill="#16345a"/><circle cx="131" cy="72" r="11" fill="#16345a"/>
          <circle cx="81" cy="68" r="4" fill="#fff"/><circle cx="134" cy="68" r="4" fill="#fff"/>
          <path d="M79 104c17 15 36 15 53 0" stroke="#ff7f65" stroke-width="6" fill="none" stroke-linecap="round"/>
          <rect x="39" y="130" width="131" height="127" rx="52" fill="url(#mBlue)"/>
          <circle cx="104" cy="176" r="24" fill="#fff"/><text x="104" y="185" text-anchor="middle" font-size="28" font-weight="900" fill="#1768f2">e</text>
          <path d="M39 172c-39 18-45 42-49 68" stroke="#1768f2" stroke-width="18" fill="none" stroke-linecap="round"/>
          <path d="M170 172c42 8 56 34 68 52" stroke="#1768f2" stroke-width="18" fill="none" stroke-linecap="round"/>
          <path d="M70 251l-11 68M139 251l13 68" stroke="#1768f2" stroke-width="20" stroke-linecap="round"/>
          <circle cx="-12" cy="240" r="12" fill="#fff" stroke="#1768f2" stroke-width="6"/><circle cx="240" cy="225" r="12" fill="#fff" stroke="#1768f2" stroke-width="6"/>
        </g>

        <g transform="translate(315 198)" filter="url(#mShadow)">
          <circle cx="92" cy="68" r="67" fill="url(#mPurple)"/>
          <circle cx="68" cy="61" r="12" fill="#fff"/><circle cx="116" cy="61" r="12" fill="#fff"/>
          <circle cx="68" cy="63" r="5" fill="#26304c"/><circle cx="116" cy="63" r="5" fill="#26304c"/>
          <path d="M68 91c17 14 33 14 49 0" stroke="#fff" stroke-width="6" fill="none" stroke-linecap="round"/>
          <rect x="30" y="128" width="124" height="112" rx="48" fill="url(#mPurple)"/>
          <text x="92" y="193" text-anchor="middle" font-size="32" font-weight="900" fill="#fff">π</text>
          <path d="M30 165c-29 0-45 15-58 35M154 165c32 5 47 18 58 37" stroke="#7047ee" stroke-width="16" fill="none" stroke-linecap="round"/>
          <path d="M61 238l-8 58M123 238l9 58" stroke="#7047ee" stroke-width="18" stroke-linecap="round"/>
        </g>

        <g transform="translate(535 252)" filter="url(#mShadow)">
          <circle cx="78" cy="55" r="57" fill="url(#mGreen)"/>
          <circle cx="58" cy="50" r="10" fill="#fff"/><circle cx="98" cy="50" r="10" fill="#fff"/>
          <circle cx="58" cy="52" r="4" fill="#24364c"/><circle cx="98" cy="52" r="4" fill="#24364c"/>
          <path d="M58 78c13 11 27 11 41 0" stroke="#fff" stroke-width="5" fill="none" stroke-linecap="round"/>
          <rect x="24" y="105" width="108" height="96" rx="42" fill="url(#mGreen)"/>
          <text x="78" y="163" text-anchor="middle" font-size="30" font-weight="900" fill="#fff">%</text>
          <path d="M24 142c-28 4-39 17-49 33M132 142c30 3 42 15 51 31" stroke="#20b775" stroke-width="14" fill="none" stroke-linecap="round"/>
          <path d="M51 200l-5 50M105 200l7 50" stroke="#20b775" stroke-width="16" stroke-linecap="round"/>
        </g>

        <g transform="translate(705 330)" filter="url(#mShadow)">
          <circle cx="58" cy="42" r="47" fill="url(#mOrange)"/>
          <circle cx="42" cy="38" r="8" fill="#fff"/><circle cx="74" cy="38" r="8" fill="#fff"/>
          <circle cx="42" cy="39" r="3.5" fill="#26364a"/><circle cx="74" cy="39" r="3.5" fill="#26364a"/>
          <path d="M43 61c10 8 20 8 30 0" stroke="#fff" stroke-width="4" fill="none" stroke-linecap="round"/>
          <rect x="13" y="85" width="90" height="82" rx="36" fill="url(#mOrange)"/>
          <text x="58" y="135" text-anchor="middle" font-size="24" font-weight="900" fill="#fff">3/4</text>
          <path d="M13 116c-22 2-31 12-38 25M103 116c22 2 31 11 39 24" stroke="#ff7a1a" stroke-width="12" fill="none" stroke-linecap="round"/>
          <path d="M37 165l-4 39M80 165l5 39" stroke="#ff7a1a" stroke-width="14" stroke-linecap="round"/>
        </g>
      </svg>
    </div>`;

  const brightClassroom = `
    <div class="ed-home-v12-photo" role="img" aria-label="${language === 'ar' ? 'طلاب يتعلمون في فصل دراسي مشرق' : language === 'pl' ? 'Uczniowie uczący się w jasnej klasie' : 'Students learning in a bright classroom'}">
      <div class="ed-home-v12-photo-image"></div>
      <div class="ed-home-v12-photo-wash"></div>
    </div>`;

  const esc = value => String(value)
    .replaceAll('&', '&amp;')
    .replaceAll('<', '&lt;')
    .replaceAll('>', '&gt;')
    .replaceAll('"', '&quot;')
    .replaceAll("'", '&#039;');

  const slideHtml = (slide, index) => `
    <article class="ed-home-v12-slide" data-slide-index="${index}" ${rtl ? 'dir="rtl"' : ''}>
      <div class="ed-home-v12-slide-grid">
        <div class="ed-home-v12-copy">
          <div class="ed-home-v12-kicker">${esc(slide.kicker)}</div>
          <h1>${esc(slide.title)}</h1>
          <p class="ed-home-v12-body">${esc(slide.body)}</p>
          ${slide.note ? `<p class="ed-home-v12-note">${esc(slide.note)}</p>` : ''}
          <div class="ed-home-v12-actions">
            <a class="ed-home-v12-primary" href="/contact">${esc(slide.primary)}</a>
            <a class="ed-home-v12-secondary" href="/contact">${esc(slide.secondary)}</a>
          </div>
        </div>
        <div class="ed-home-v12-visual">${index === 0 ? mascot : brightClassroom}</div>
      </div>
    </article>`;

  const hero = document.createElement('section');
  hero.className = `ed-home-v12-hero${rtl ? ' is-rtl' : ''}`;
  hero.setAttribute('aria-roledescription', 'carousel');
  hero.innerHTML = `
    <div class="ed-home-v12-viewport">
      <div class="ed-home-v12-track">
        ${slideHtml(slides[0], 0)}
        ${slideHtml(slides[1], 1)}
      </div>
    </div>
    <button type="button" class="ed-home-v12-arrow ed-home-v12-prev" aria-label="Previous slide"><span aria-hidden="true">‹</span></button>
    <button type="button" class="ed-home-v12-arrow ed-home-v12-next" aria-label="Next slide"><span aria-hidden="true">›</span></button>
    <div class="ed-home-v12-dots" role="tablist" aria-label="Hero slides">
      <button type="button" class="ed-home-v12-dot is-active" aria-label="Slide 1" aria-selected="true"></button>
      <button type="button" class="ed-home-v12-dot" aria-label="Slide 2" aria-selected="false"></button>
    </div>`;

  oldHero.replaceWith(hero);

  const track = hero.querySelector('.ed-home-v12-track');
  const dots = [...hero.querySelectorAll('.ed-home-v12-dot')];
  const prev = hero.querySelector('.ed-home-v12-prev');
  const next = hero.querySelector('.ed-home-v12-next');
  let index = 0;
  let timer = null;
  let locked = false;

  const setIndex = (target, userInitiated = false) => {
    if (locked) return;
    const nextIndex = (target + 2) % 2;
    if (nextIndex === index && userInitiated) return;
    index = nextIndex;
    locked = true;
    track.style.transform = `translate3d(${-100 * index}%,0,0)`;
    dots.forEach((dot, dotIndex) => {
      const active = dotIndex === index;
      dot.classList.toggle('is-active', active);
      dot.setAttribute('aria-selected', active ? 'true' : 'false');
    });
    window.setTimeout(() => { locked = false; }, 680);
  };

  const stop = () => {
    if (timer) window.clearInterval(timer);
    timer = null;
  };

  const start = () => {
    stop();
    if (window.matchMedia('(prefers-reduced-motion: reduce)').matches) return;
    timer = window.setInterval(() => setIndex(index + 1), 7000);
  };

  prev.addEventListener('click', () => { setIndex(index - 1, true); start(); });
  next.addEventListener('click', () => { setIndex(index + 1, true); start(); });
  dots.forEach((dot, dotIndex) => dot.addEventListener('click', () => { setIndex(dotIndex, true); start(); }));

  hero.addEventListener('mouseenter', stop);
  hero.addEventListener('mouseleave', start);
  hero.addEventListener('focusin', stop);
  hero.addEventListener('focusout', start);

  let touchX = null;
  let touchY = null;
  hero.addEventListener('touchstart', event => {
    const touch = event.changedTouches?.[0];
    if (!touch) return;
    touchX = touch.clientX;
    touchY = touch.clientY;
  }, { passive: true });

  hero.addEventListener('touchend', event => {
    const touch = event.changedTouches?.[0];
    if (!touch || touchX === null || touchY === null) return;
    const dx = touch.clientX - touchX;
    const dy = touch.clientY - touchY;
    touchX = null;
    touchY = null;
    if (Math.abs(dx) < 48 || Math.abs(dx) <= Math.abs(dy)) return;
    setIndex(dx < 0 ? index + 1 : index - 1, true);
    start();
  }, { passive: true });

  start();
})();
