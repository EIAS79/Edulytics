(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;
  const pl = (document.documentElement.lang || '').toLowerCase().startsWith('pl');

  root.querySelectorAll('.ed-home-v5-story,.ed-home-commercial-ribbon').forEach(x => x.remove());

  const desktopNav = root.querySelector('.ed-home-links');
  const mobileNav = root.querySelector('.ed-home-mobile-panel');
  const navItems = pl ? [
    ['#platform','O produkcie'],['#teachers','Nauczyciele'],['#parents','Rodzice'],['/contact','Kontakt']
  ] : [
    ['#platform','About the product'],['#teachers','Teachers'],['#parents','Parents'],['/contact','Contact']
  ];
  const navHtml = navItems.map(([href,label]) => `<a href="${href}">${label}</a>`).join('');
  if (desktopNav) desktopNav.innerHTML = navHtml;
  if (mobileNav) mobileNav.innerHTML = navHtml;
  const topCta = root.querySelector('.ed-home-actions > .ed-home-cta');
  if (topCta) { topCta.textContent = pl ? 'Wypróbuj Edulytics' : 'Try Edulytics'; topCta.href = '/contact'; }

  const cartoon = `
  <div class="ed-home-v6-cartoon" aria-label="Edulytics teacher and student learning illustration">
    <svg viewBox="0 0 900 600" role="img" aria-label="Teacher using Edulytics AI with student learning data">
      <defs>
        <linearGradient id="v6bg" x1="0" y1="0" x2="1" y2="1"><stop offset="0" stop-color="#eef7ff"/><stop offset="1" stop-color="#f7f1ff"/></linearGradient>
        <linearGradient id="v6shirt" x1="0" y1="0" x2="1" y2="1"><stop offset="0" stop-color="#2f66e8"/><stop offset="1" stop-color="#6e47ed"/></linearGradient>
      </defs>
      <rect width="900" height="600" fill="url(#v6bg)"/>
      <circle cx="770" cy="95" r="38" fill="#ffd84d"/><rect x="95" y="86" width="54" height="54" rx="15" fill="#ff8a3d" transform="rotate(-18 122 113)"/><circle cx="110" cy="500" r="27" fill="#53c98a"/>
      <rect x="205" y="80" width="575" height="370" rx="32" fill="#fff" stroke="#d9e6f5" stroke-width="4"/>
      <rect x="232" y="108" width="154" height="314" rx="22" fill="#133b72"/><circle cx="267" cy="145" r="14" fill="#fff"/><rect x="294" y="137" width="60" height="15" rx="7" fill="#fff" opacity=".9"/><rect x="258" y="190" width="96" height="11" rx="6" fill="#8db7f1"/><rect x="258" y="224" width="78" height="11" rx="6" fill="#8db7f1"/><rect x="258" y="258" width="89" height="11" rx="6" fill="#8db7f1"/>
      <text x="420" y="135" font-size="20" font-weight="900" fill="#2f66e8">EDULYTICS</text><text x="420" y="166" font-size="28" font-weight="900" fill="#10244a">Generate with Edulytics AI</text>
      <rect x="420" y="195" width="155" height="88" rx="18" fill="#eef5ff"/><text x="442" y="224" font-size="13" font-weight="800" fill="#65758f">ASSESSMENT</text><text x="442" y="260" font-size="25" font-weight="900" fill="#10244a">20 questions</text>
      <rect x="594" y="195" width="155" height="88" rx="18" fill="#effaf5"/><text x="616" y="224" font-size="13" font-weight="800" fill="#65758f">MASTERY</text><text x="616" y="260" font-size="31" font-weight="900" fill="#15a66f">82%</text>
      <rect x="420" y="304" width="329" height="88" rx="18" fill="#fff4e9"/><text x="442" y="334" font-size="13" font-weight="800" fill="#b26325">NEXT STEP</text><text x="442" y="369" font-size="23" font-weight="900" fill="#10244a">Fractions · targeted practice</text>
      <ellipse cx="510" cy="535" rx="235" ry="30" fill="#dbe8f7"/>
      <circle cx="455" cy="425" r="54" fill="#efb181"/><path d="M404 421c7-57 91-68 106 4-30-14-70-15-106-4Z" fill="#49302a"/><circle cx="438" cy="432" r="5" fill="#222"/><circle cx="474" cy="432" r="5" fill="#222"/><path d="M438 453c15 11 29 8 40-1" stroke="#9c5148" stroke-width="4" fill="none" stroke-linecap="round"/><path d="M384 492c14-55 124-61 145 4l9 76H371Z" fill="url(#v6shirt)"/>
      <circle cx="645" cy="463" r="43" fill="#f3fbff" stroke="#9ecaf0" stroke-width="4"/><rect x="617" y="439" width="56" height="43" rx="15" fill="#194f8c"/><circle cx="635" cy="458" r="5" fill="#6fe7ff"/><circle cx="656" cy="458" r="5" fill="#6fe7ff"/><path d="M633 472c10 7 19 7 29 0" stroke="#6fe7ff" stroke-width="3" fill="none"/><path d="M645 420v-20M600 463h-20M690 463h20" stroke="#7aa9d1" stroke-width="5" stroke-linecap="round"/>
      <g transform="translate(720 370)"><circle r="48" fill="#fff" stroke="#d8e5f4" stroke-width="3"/><text text-anchor="middle" y="10" font-size="35" font-weight="900" fill="#2f66e8">%</text></g>
      <g transform="translate(150 330)"><circle r="48" fill="#fff" stroke="#d8e5f4" stroke-width="3"/><text text-anchor="middle" y="10" font-size="32" font-weight="900" fill="#ff8a3d">3/4</text></g>
    </svg>
  </div>`;

  const photoUrl = 'https://images.unsplash.com/photo-1604933834215-2a64950311bd?auto=format&fit=crop&fm=jpg&ixlib=rb-4.1.0&q=82&w=1800';
  const slides = pl ? [
    {
      kicker:'MATEMATYKA W PRAWDZIWYM ŻYCIU',
      title:'Matematyka, którą uczniowie potrafią wykorzystać.',
      body:'Interaktywne doświadczenie edukacyjne zamienia pojęcia matematyczne i umiejętności zarządzania pieniędzmi w angażujące wyzwania z życia codziennego. Pomaga uczniom myśleć, podejmować mądrzejsze decyzje finansowe i rozwijać umiejętności z większą pewnością siebie.',
      note:'Dla uczniów klas 1–6.',
      primary:'Wypróbuj Edulytics', secondary:'Porozmawiaj z nami', visual:`<div class="ed-home-v6-photo" style="background-image:url('${photoUrl}')" role="img" aria-label="Digital mathematics learning in a school environment"></div>`
    },
    {
      kicker:'WSPARCIE NAUCZYCIELI • DANE • AI',
      title:'Wspieramy nauczycieli. Budujemy lepszą przyszłość nauki.',
      body:'Łączymy nauczanie oparte na badaniach, analizę danych i sztuczną inteligencję, aby pomóc nauczycielom lepiej rozumieć potrzeby każdego ucznia, podejmować trafniejsze decyzje dydaktyczne i tworzyć bardziej spersonalizowaną naukę matematyki.',
      note:'Nauczyciel zachowuje kontrolę nad ocenianiem i publikacją.',
      primary:'Rodzice — wypróbuj bezpłatnie', secondary:'Zapytaj teraz — dla nauczycieli', visual:cartoon
    }
  ] : [
    {
      kicker:'MATHEMATICS FOR REAL LIFE',
      title:'Mathematics students can actually use.',
      body:'An interactive learning experience turns mathematical ideas and money-management skills into engaging real-life challenges, helping students think, make smarter financial decisions and build their skills with confidence.',
      note:'Designed for students in Grades 1–6.',
      primary:'Try Edulytics', secondary:'Talk to us', visual:`<div class="ed-home-v6-photo" style="background-image:url('${photoUrl}')" role="img" aria-label="Digital mathematics learning in a school environment"></div>`
    },
    {
      kicker:'TEACHER SUPPORT • DATA • AI',
      title:'Empower teachers. Build a better future for learning.',
      body:'We combine research-informed teaching, learning data and artificial intelligence to help teachers understand every student’s needs, make smarter instructional decisions and deliver more personalised mathematics learning.',
      note:'Teachers remain in control of assessment and publishing.',
      primary:'Parents — try it free', secondary:'Ask now — for teachers', visual:cartoon
    }
  ];

  const oldHero = root.querySelector('.ed-home-hero');
  if (oldHero) {
    const hero = document.createElement('section');
    hero.className = 'ed-home-v6-hero';
    hero.innerHTML = `<button class="ed-home-v6-arrow ed-home-v6-prev" aria-label="Previous slide">‹</button><button class="ed-home-v6-arrow ed-home-v6-next" aria-label="Next slide">›</button><div class="ed-home-v6-hero-grid"><div class="ed-home-v6-copy"><div class="ed-home-v6-kicker"></div><h1></h1><p class="ed-home-v6-body"></p><p class="ed-home-v6-note"></p><div class="ed-home-v6-actions"><a class="ed-home-v6-primary" href="/contact"></a><a class="ed-home-v6-secondary" href="/contact"></a></div></div><div class="ed-home-v6-visual"></div></div><div class="ed-home-v6-dots"><button class="ed-home-v6-dot is-active" aria-label="Slide 1"></button><button class="ed-home-v6-dot" aria-label="Slide 2"></button></div>`;
    oldHero.replaceWith(hero);
    const kicker = hero.querySelector('.ed-home-v6-kicker'), title = hero.querySelector('h1'), body = hero.querySelector('.ed-home-v6-body'), note = hero.querySelector('.ed-home-v6-note'), visual = hero.querySelector('.ed-home-v6-visual'), primary = hero.querySelector('.ed-home-v6-primary'), secondary = hero.querySelector('.ed-home-v6-secondary'), dots = [...hero.querySelectorAll('.ed-home-v6-dot')];
    let current = 0, timer;
    const apply = i => { current=(i+2)%2; const s=slides[current]; kicker.textContent=s.kicker; title.textContent=s.title; body.textContent=s.body; note.textContent=s.note; primary.textContent=s.primary; secondary.textContent=s.secondary; visual.innerHTML=s.visual; dots.forEach((d,n)=>d.classList.toggle('is-active',n===current)); };
    const restart = () => { clearInterval(timer); if (!matchMedia('(prefers-reduced-motion: reduce)').matches) timer=setInterval(()=>apply(current+1),8000); };
    hero.querySelector('.ed-home-v6-prev').addEventListener('click',()=>{apply(current-1);restart();}); hero.querySelector('.ed-home-v6-next').addEventListener('click',()=>{apply(current+1);restart();}); dots.forEach((d,i)=>d.addEventListener('click',()=>{apply(i);restart();})); hero.addEventListener('mouseenter',()=>clearInterval(timer)); hero.addEventListener('mouseleave',restart); hero.addEventListener('focusin',()=>clearInterval(timer)); hero.addEventListener('focusout',restart); apply(0); restart();
  }

  const oldAudience = root.querySelector('.ed-home-audience-band');
  if (oldAudience) {
    const shell = document.createElement('div');
    const schoolImg='https://images.unsplash.com/photo-1604933834215-2a64950311bd?auto=format&fit=crop&fm=jpg&ixlib=rb-4.1.0&q=80&w=900';
    const teacherImg='https://images.unsplash.com/photo-1758270704021-361c165d68fd?auto=format&fit=crop&fm=jpg&ixlib=rb-4.1.0&q=80&w=900';
    const parentImg='https://images.unsplash.com/photo-1752652011885-b0d36aeb3047?auto=format&fit=crop&fm=jpg&ixlib=rb-4.1.0&q=80&w=900';
    const cards = pl ? [
      ['schools','Szkoły','Program nauczania, klasy, wyniki i nadzór akademicki w jednym środowisku.','#platform',schoolImg],
      ['teachers','Nauczyciele','Twórz oceny, korzystaj z AI i zobacz, czego uczniowie potrzebują dalej.','#ai',teacherImg],
      ['parents','Rodzice','Zobacz, jak Edulytics pomaga uczniowi ćwiczyć, rozumieć wyniki i budować pewność siebie.','/contact',parentImg]
    ] : [
      ['schools','Schools','Curriculum, classes, results and academic oversight in one environment.','#platform',schoolImg],
      ['teachers','Teachers','Create assessments, use AI and see what students need to work on next.','#ai',teacherImg],
      ['parents','Parents','See how Edulytics helps students practise, understand results and build confidence.','/contact',parentImg]
    ];
    const signalData = pl ? [
      ['Zgodność z programem','Treści i oceny osadzone w kontekście programu'],['AI w ocenianiu','Generowanie pytań pod kontrolą nauczyciela'],['Praktyka ucznia','Prywatne ćwiczenia dopasowane do potrzeb'],['Mastery i postęp','Widoczny wynik, obszary słabsze i następny krok'],['Analityka szkoły','Dane dla nauczycieli i nadzoru akademickiego']
    ] : [
      ['Curriculum-aligned learning','Content and assessment grounded in curriculum context'],['AI-powered assessment','Question generation with teacher control'],['Personalised student practice','Private practice targeted to student needs'],['Mastery & progress insights','Results, weaker areas and a clear next step'],['School analytics','Learning data for teachers and academic oversight']
    ];
    shell.innerHTML = `<section class="ed-home-v6-audience"><div class="ed-home-v6-audience-grid">${cards.map(c=>`<article id="${c[0]}" class="ed-home-v6-audience-card"><img src="${c[4]}" alt="" loading="lazy"><div><h3>${c[1]}</h3><p>${c[2]}</p><a href="${c[3]}">${pl?'Dowiedz się więcej':'Learn more'} →</a></div></article>`).join('')}</div></section><section class="ed-home-v6-signals" aria-label="Edulytics product strengths"><div class="ed-home-v6-signals-grid">${signalData.map(s=>`<div class="ed-home-v6-signal"><strong>${s[0]}</strong><span>${s[1]}</span></div>`).join('')}</div></section>`;
    oldAudience.replaceWith(shell);
  }

  root.querySelectorAll('.ed-home-mobile-panel a').forEach(a => a.addEventListener('click',()=>root.querySelector('.ed-home-mobile-menu')?.removeAttribute('open')));
})();
