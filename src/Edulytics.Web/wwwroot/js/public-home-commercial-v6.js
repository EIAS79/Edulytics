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
  if (topCta) {
    topCta.textContent = pl ? 'Wypróbuj Edulytics' : 'Try Edulytics';
    topCta.href = '/contact';
  }

  const heroCartoon = `
  <div class="ed-home-v6-cartoon" aria-label="Original Edulytics mathematics learning character scene">
    <svg viewBox="0 0 920 600" role="img" aria-label="Original Edulytics learning character with mathematics, AI and progress elements">
      <defs>
        <linearGradient id="v7bg" x1="0" y1="0" x2="1" y2="1"><stop offset="0" stop-color="#eff8ff"/><stop offset="1" stop-color="#f7f0ff"/></linearGradient>
        <linearGradient id="v7body" x1="0" y1="0" x2="1" y2="1"><stop offset="0" stop-color="#2f66e8"/><stop offset="1" stop-color="#7047ee"/></linearGradient>
      </defs>
      <rect width="920" height="600" fill="url(#v7bg)"/>
      <circle cx="120" cy="92" r="28" fill="#ffcf4b"/><rect x="758" y="82" width="56" height="56" rx="15" fill="#ff8a3d" transform="rotate(17 786 110)"/><circle cx="820" cy="480" r="30" fill="#54c98a"/>
      <g transform="translate(110 95)">
        <rect x="305" y="35" width="355" height="255" rx="28" fill="#fff" stroke="#dce7f5" stroke-width="4"/>
        <text x="335" y="78" font-size="20" font-weight="900" fill="#2f66e8">EDULYTICS</text>
        <text x="335" y="112" font-size="28" font-weight="900" fill="#11243e">Math that adapts</text>
        <rect x="335" y="142" width="126" height="82" rx="18" fill="#eef5ff"/><text x="355" y="172" font-size="13" font-weight="800" fill="#64758d">MASTERY</text><text x="355" y="207" font-size="29" font-weight="900" fill="#16a76e">82%</text>
        <rect x="478" y="142" width="150" height="82" rx="18" fill="#fff4e9"/><text x="498" y="172" font-size="13" font-weight="800" fill="#a75f28">NEXT STEP</text><text x="498" y="205" font-size="19" font-weight="900" fill="#11243e">Fractions</text>
        <rect x="335" y="240" width="293" height="26" rx="13" fill="#e9eef8"/><rect x="335" y="240" width="205" height="26" rx="13" fill="#2f66e8"/>
      </g>
      <g transform="translate(150 190)">
        <ellipse cx="240" cy="342" rx="170" ry="30" fill="#d9e6f6"/>
        <path d="M120 175c8-94 170-105 193 11l20 173H90Z" fill="url(#v7body)"/>
        <circle cx="210" cy="120" r="83" fill="#f1b480"/>
        <path d="M132 112c9-92 155-103 165 7-47-28-113-28-165-7Z" fill="#4b3029"/>
        <circle cx="181" cy="130" r="7" fill="#202735"/><circle cx="239" cy="130" r="7" fill="#202735"/>
        <path d="M178 163c24 20 50 18 71-3" stroke="#9c5147" stroke-width="6" fill="none" stroke-linecap="round"/>
        <rect x="118" y="240" width="235" height="142" rx="16" fill="#fff" stroke="#b9cce3" stroke-width="5"/>
        <circle cx="236" cy="310" r="20" fill="#7047ee"/>
      </g>
      <g transform="translate(690 355)"><circle cx="55" cy="55" r="50" fill="#f4fbff" stroke="#9dcaf0" stroke-width="5"/><rect x="24" y="30" width="62" height="48" rx="17" fill="#194f8d"/><circle cx="45" cy="54" r="6" fill="#6fe7ff"/><circle cx="66" cy="54" r="6" fill="#6fe7ff"/><path d="M43 69c12 8 22 8 34 0" stroke="#6fe7ff" stroke-width="4" fill="none"/><path d="M55 4v-24M4 55h-25M106 55h25" stroke="#78a9d1" stroke-width="6" stroke-linecap="round"/></g>
      <g font-weight="900" fill="#11243e"><text x="90" y="180" font-size="42">3/4</text><text x="760" y="270" font-size="48">%</text><text x="96" y="480" font-size="40">x + 4</text></g>
      <path d="M730 170c28-25 57-25 84 0" stroke="#ff8a3d" stroke-width="9" fill="none" stroke-linecap="round"/>
      <circle cx="727" cy="170" r="10" fill="#ff8a3d"/><circle cx="817" cy="170" r="10" fill="#ff8a3d"/>
    </svg>
  </div>`;

  const photoUrl = 'https://images.unsplash.com/photo-1509062522246-3755977927d7?auto=format&fit=crop&q=82&w=1800';
  const slides = pl ? [
    {
      kicker:'MATEMATYKA W PRAWDZIWYM ŻYCIU',
      title:'Matematyka, którą uczniowie potrafią wykorzystać.',
      body:'Interaktywne doświadczenie edukacyjne zamienia pojęcia matematyczne i umiejętności zarządzania pieniędzmi w angażujące wyzwania z życia codziennego. Pomaga uczniom myśleć, podejmować mądrzejsze decyzje finansowe i rozwijać umiejętności z większą pewnością siebie.',
      note:'',
      primary:'Wypróbuj Edulytics', secondary:'Porozmawiaj z nami', visual:`<div class="ed-home-v6-photo" style="background-image:url('${photoUrl}')" role="img" aria-label="Uczniowie podczas nauki w klasie"></div>`
    },
    {
      kicker:'WSPARCIE NAUCZYCIELI • DANE • AI',
      title:'Wspieramy nauczycieli. Budujemy lepszą przyszłość nauki.',
      body:'Łączymy nauczanie oparte na badaniach, analizę danych i sztuczną inteligencję, aby pomóc nauczycielom lepiej rozumieć potrzeby każdego ucznia, podejmować trafniejsze decyzje dydaktyczne i tworzyć bardziej spersonalizowaną naukę matematyki.',
      note:'Nauczyciel zachowuje kontrolę nad ocenianiem i publikacją.',
      primary:'Rodzice — wypróbuj bezpłatnie', secondary:'Zapytaj teraz — dla nauczycieli', visual:heroCartoon
    }
  ] : [
    {
      kicker:'MATHEMATICS FOR REAL LIFE',
      title:'Mathematics students can actually use.',
      body:'An interactive learning experience turns mathematical ideas and money-management skills into engaging real-life challenges, helping students think, make smarter financial decisions and build their skills with confidence.',
      note:'',
      primary:'Try Edulytics', secondary:'Talk to us', visual:`<div class="ed-home-v6-photo" style="background-image:url('${photoUrl}')" role="img" aria-label="Students learning in a classroom"></div>`
    },
    {
      kicker:'TEACHER SUPPORT • DATA • AI',
      title:'Empower teachers. Build a better future for learning.',
      body:'We combine research-informed teaching, learning data and artificial intelligence to help teachers understand every student’s needs, make smarter instructional decisions and deliver more personalised mathematics learning.',
      note:'Teachers remain in control of assessment and publishing.',
      primary:'Parents — try it free', secondary:'Ask now — for teachers', visual:heroCartoon
    }
  ];

  const oldHero = root.querySelector('.ed-home-hero');
  if (oldHero) {
    const hero = document.createElement('section');
    hero.className = 'ed-home-v6-hero';
    hero.innerHTML = `<button class="ed-home-v6-arrow ed-home-v6-prev" aria-label="Previous slide">‹</button><button class="ed-home-v6-arrow ed-home-v6-next" aria-label="Next slide">›</button><div class="ed-home-v6-hero-grid"><div class="ed-home-v6-copy"><div class="ed-home-v6-kicker"></div><h1></h1><p class="ed-home-v6-body"></p><p class="ed-home-v6-note"></p><div class="ed-home-v6-actions"><a class="ed-home-v6-primary" href="/contact"></a><a class="ed-home-v6-secondary" href="/contact"></a></div></div><div class="ed-home-v6-visual"></div></div><div class="ed-home-v6-dots"><button class="ed-home-v6-dot is-active" aria-label="Slide 1"></button><button class="ed-home-v6-dot" aria-label="Slide 2"></button></div>`;
    oldHero.replaceWith(hero);
    const kicker = hero.querySelector('.ed-home-v6-kicker');
    const title = hero.querySelector('h1');
    const body = hero.querySelector('.ed-home-v6-body');
    const note = hero.querySelector('.ed-home-v6-note');
    const visual = hero.querySelector('.ed-home-v6-visual');
    const primary = hero.querySelector('.ed-home-v6-primary');
    const secondary = hero.querySelector('.ed-home-v6-secondary');
    const dots = [...hero.querySelectorAll('.ed-home-v6-dot')];
    let current = 0;
    let timer;
    const apply = i => {
      current = (i + 2) % 2;
      const s = slides[current];
      kicker.textContent = s.kicker;
      title.textContent = s.title;
      body.textContent = s.body;
      note.textContent = s.note;
      note.hidden = !s.note;
      primary.textContent = s.primary;
      secondary.textContent = s.secondary;
      visual.innerHTML = s.visual;
      dots.forEach((d,n) => d.classList.toggle('is-active', n === current));
    };
    const restart = () => {
      clearInterval(timer);
      if (!matchMedia('(prefers-reduced-motion: reduce)').matches) timer = setInterval(() => apply(current + 1), 8000);
    };
    hero.querySelector('.ed-home-v6-prev').addEventListener('click', () => { apply(current - 1); restart(); });
    hero.querySelector('.ed-home-v6-next').addEventListener('click', () => { apply(current + 1); restart(); });
    dots.forEach((d,i) => d.addEventListener('click', () => { apply(i); restart(); }));
    hero.addEventListener('mouseenter', () => clearInterval(timer));
    hero.addEventListener('mouseleave', restart);
    hero.addEventListener('focusin', () => clearInterval(timer));
    hero.addEventListener('focusout', restart);
    apply(0);
    restart();
  }

  const oldAudience = root.querySelector('.ed-home-audience-band');
  if (oldAudience) {
    const shell = document.createElement('div');
    const schoolImg = 'https://images.unsplash.com/photo-1509062522246-3755977927d7?auto=format&fit=crop&q=80&w=900';
    const teacherImg = 'https://images.unsplash.com/photo-1577896851231-70ef18881754?auto=format&fit=crop&q=80&w=900';
    const parentImg = 'https://images.unsplash.com/photo-1596464716127-f2a82984de30?auto=format&fit=crop&q=80&w=900';
    const cards = pl ? [
      ['schools','Szkoły','Program nauczania, klasy, wyniki i nadzór akademicki w jednym środowisku.','#platform',schoolImg],
      ['teachers','Nauczyciele','Twórz oceny, korzystaj z AI i zobacz, czego uczniowie potrzebują dalej.','#ai',teacherImg],
      ['parents','Rodzice','Zobacz, jak Edulytics pomaga uczniowi ćwiczyć, rozumieć wyniki i budować pewność siebie.','#experience',parentImg]
    ] : [
      ['schools','Schools','Curriculum, classes, results and academic oversight in one environment.','#platform',schoolImg],
      ['teachers','Teachers','Create assessments, use AI and see what students need to work on next.','#ai',teacherImg],
      ['parents','Parents','See how Edulytics helps students practise, understand results and build confidence.','#experience',parentImg]
    ];
    const signalData = pl ? [
      ['Zgodność z programem','Treści i oceny osadzone w kontekście programu'],['AI w ocenianiu','Generowanie pytań pod kontrolą nauczyciela'],['Praktyka ucznia','Prywatne ćwiczenia dopasowane do potrzeb'],['Mastery i postęp','Widoczny wynik, obszary słabsze i następny krok'],['Analityka szkoły','Dane dla nauczycieli i nadzoru akademickiego']
    ] : [
      ['Curriculum-aligned learning','Content and assessment grounded in curriculum context'],['AI-powered assessment','Question generation with teacher control'],['Personalised student practice','Private practice targeted to student needs'],['Mastery & progress insights','Results, weaker areas and a clear next step'],['School analytics','Learning data for teachers and academic oversight']
    ];
    shell.innerHTML = `<section class="ed-home-v6-audience"><div class="ed-home-v6-audience-grid">${cards.map(c => `<article id="${c[0]}" class="ed-home-v6-audience-card"><div class="ed-home-v6-audience-photo" style="background-image:url('${c[4]}')" role="img" aria-label="${c[1]}"></div><div><h3>${c[1]}</h3><p>${c[2]}</p><a href="${c[3]}">${pl ? 'Dowiedz się więcej' : 'Learn more'} →</a></div></article>`).join('')}</div></section><section class="ed-home-v6-signals" aria-label="Edulytics product strengths"><div class="ed-home-v6-signals-grid">${signalData.map(s => `<div class="ed-home-v6-signal"><strong>${s[0]}</strong><span>${s[1]}</span></div>`).join('')}</div></section>`;
    oldAudience.replaceWith(shell);
  }

  const studentVisual = `
    <svg viewBox="0 0 760 520" role="img" aria-label="Student learning journey in Edulytics">
      <rect width="760" height="520" rx="34" fill="#f4f9ff"/>
      <rect x="52" y="72" width="240" height="132" rx="24" fill="#fff" stroke="#dce9f6" stroke-width="3"/><text x="78" y="108" font-size="15" font-weight="900" fill="#2f66e8">LESSON</text><text x="78" y="145" font-size="28" font-weight="900" fill="#11243e">Fractions</text><rect x="78" y="165" width="154" height="13" rx="7" fill="#e5edf7"/><rect x="78" y="165" width="116" height="13" rx="7" fill="#2f66e8"/>
      <rect x="438" y="66" width="250" height="142" rx="24" fill="#fff" stroke="#dce9f6" stroke-width="3"/><text x="468" y="102" font-size="15" font-weight="900" fill="#10a56d">MASTERY</text><text x="468" y="157" font-size="45" font-weight="950" fill="#10a56d">84%</text>
      <rect x="460" y="275" width="215" height="122" rx="24" fill="#fff4e9"/><text x="488" y="311" font-size="14" font-weight="900" fill="#c06b29">NEXT STEP</text><text x="488" y="348" font-size="23" font-weight="900" fill="#11243e">Targeted practice</text>
      <circle cx="337" cy="265" r="67" fill="#f0b17d"/><path d="M274 257c8-70 118-78 126 7-37-21-88-21-126-7Z" fill="#49312a"/><path d="M257 343c14-68 146-76 166 3l13 112H242Z" fill="#3267e8"/><rect x="266" y="350" width="173" height="104" rx="13" fill="#fff" stroke="#bccfe4" stroke-width="4"/>
      <path d="M310 292c18 14 39 13 55-2" stroke="#9d5148" stroke-width="5" fill="none" stroke-linecap="round"/><circle cx="314" cy="268" r="6"/><circle cx="360" cy="268" r="6"/>
      <circle cx="93" cy="383" r="32" fill="#7047ee"/><text x="93" y="394" text-anchor="middle" font-size="29" font-weight="900" fill="#fff">3/4</text><circle cx="675" cy="446" r="27" fill="#53c98a"/>
    </svg>`;

  const parentVisual = `
    <svg viewBox="0 0 760 520" role="img" aria-label="Parent progress summary in Edulytics">
      <rect width="760" height="520" rx="34" fill="#f8f6ff"/>
      <rect x="80" y="54" width="600" height="405" rx="30" fill="#fff" stroke="#e1dcf4" stroke-width="3"/>
      <text x="118" y="102" font-size="18" font-weight="900" fill="#7047ee">WEEKLY LEARNING SUMMARY</text>
      <text x="118" y="145" font-size="31" font-weight="950" fill="#11243e">Progress at a glance</text>
      <rect x="118" y="183" width="168" height="103" rx="22" fill="#eef5ff"/><text x="142" y="216" font-size="13" font-weight="900" fill="#61738b">LEARNING TIME</text><text x="142" y="260" font-size="31" font-weight="950" fill="#2f66e8">3h 20m</text>
      <rect x="307" y="183" width="156" height="103" rx="22" fill="#effaf5"/><text x="331" y="216" font-size="13" font-weight="900" fill="#61738b">MASTERY</text><text x="331" y="260" font-size="31" font-weight="950" fill="#12a66f">82%</text>
      <rect x="484" y="183" width="154" height="103" rx="22" fill="#fff4e9"/><text x="508" y="216" font-size="13" font-weight="900" fill="#61738b">ACTIVITIES</text><text x="508" y="260" font-size="31" font-weight="950" fill="#ff7a20">12</text>
      <text x="118" y="334" font-size="15" font-weight="900" fill="#61738b">SKILLS TO SUPPORT</text><rect x="118" y="353" width="426" height="18" rx="9" fill="#e8edf6"/><rect x="118" y="353" width="286" height="18" rx="9" fill="#7047ee"/>
      <text x="118" y="409" font-size="22" font-weight="900" fill="#11243e">Fractions · multi-step problems</text>
    </svg>`;

  const teacherVisual = `
    <svg viewBox="0 0 760 520" role="img" aria-label="Teacher assessment and learning outcome insights in Edulytics">
      <rect width="760" height="520" rx="34" fill="#f4f9ff"/>
      <rect x="50" y="56" width="660" height="410" rx="30" fill="#fff" stroke="#dae7f4" stroke-width="3"/>
      <rect x="76" y="82" width="152" height="356" rx="24" fill="#143b72"/><circle cx="112" cy="120" r="14" fill="#fff"/><rect x="139" y="112" width="58" height="14" rx="7" fill="#fff" opacity=".9"/><rect x="99" y="166" width="95" height="11" rx="6" fill="#8db7f1"/><rect x="99" y="201" width="79" height="11" rx="6" fill="#8db7f1"/>
      <text x="264" y="115" font-size="18" font-weight="900" fill="#2f66e8">Generate with Edulytics AI</text>
      <rect x="264" y="144" width="182" height="96" rx="20" fill="#eef5ff"/><text x="288" y="177" font-size="13" font-weight="900" fill="#63748c">ASSESSMENT</text><text x="288" y="217" font-size="29" font-weight="950" fill="#11243e">20 questions</text>
      <rect x="468" y="144" width="188" height="96" rx="20" fill="#effaf5"/><text x="492" y="177" font-size="13" font-weight="900" fill="#63748c">CLASS MASTERY</text><text x="492" y="217" font-size="31" font-weight="950" fill="#12a66f">78%</text>
      <text x="264" y="292" font-size="14" font-weight="900" fill="#63748c">LEARNING OUTCOMES</text>
      <rect x="264" y="314" width="392" height="20" rx="10" fill="#e8edf6"/><rect x="264" y="314" width="306" height="20" rx="10" fill="#2f66e8"/>
      <rect x="264" y="354" width="392" height="20" rx="10" fill="#e8edf6"/><rect x="264" y="354" width="228" height="20" rx="10" fill="#ff8a3d"/>
      <rect x="264" y="397" width="170" height="42" rx="12" fill="#7047ee"/><text x="349" y="424" text-anchor="middle" font-size="15" font-weight="900" fill="#fff">Review & approve</text>
    </svg>`;

  const experienceData = pl ? [
    {
      key:'student', tab:'Doświadczenie ucznia', eyebrow:'DOŚWIADCZENIE UCZNIA', title:'Ucz się matematyki w mądrzejszy sposób', intro:'Interaktywne doświadczenie sprawia, że każde pojęcie staje się bardziej zrozumiałe.',
      points:[
        ['Nauka przez działanie','Uczniowie pracują z aktywnościami i wyzwaniami, które pomagają myśleć, odkrywać i rozwiązywać problemy krok po kroku.'],
        ['Nauka dopasowana do ucznia','Edulytics analizuje opanowane umiejętności i obszary wymagające dalszego wsparcia, aby lepiej ukierunkować kolejne działania.'],
        ['Od nauki do mierzalnych wyników','Każda odpowiedź zasila czytelny obraz postępów, dzięki któremu nauczyciel widzi słabsze obszary i może dokładniej reagować.']
      ], visual:studentVisual
    },
    {
      key:'parent', tab:'Doświadczenie rodziców', eyebrow:'DOŚWIADCZENIE RODZICA', title:'Czytelniejsza nauka dziecka. Łatwiejsza orientacja dla rodzica.', intro:'Nie musisz śledzić każdej aktywności, aby rozumieć, jak przebiega nauka.',
      points:[
        ['Nie musisz kontrolować każdego kroku','Edulytics analizuje wyniki, wykrywa słabsze obszary i porządkuje obraz postępów w jednym miejscu.'],
        ['Wiesz, co dziecko już opanowało','Zobacz umiejętności opanowane, wymagające dodatkowej praktyki oraz aktualny poziom postępu.'],
        ['Bądź na bieżąco bez ciągłej obserwacji','Czytelne podsumowania mogą pokazywać czas nauki, ukończone aktywności, poziom opanowania i regularność pracy.']
      ], visual:parentVisual
    },
    {
      key:'teacher', tab:'Doświadczenie nauczyciela', eyebrow:'DOŚWIADCZENIE NAUCZYCIELA', title:'Mniej zarządzania. Więcej nauczania.', intro:'Edulytics ogranicza powtarzalną pracę wokół oceniania i analizy, a nauczyciel zachowuje kontrolę nad decyzjami dydaktycznymi.',
      points:[
        ['Oszczędzaj czas. Zachowuj kontrolę.','Użyj AI do szybszego tworzenia ocen i pytań, a następnie przejrzyj, edytuj i zatwierdź wszystko przed publikacją.'],
        ['Zobacz, czego potrzebuje każdy uczeń','Analiza na poziomie umiejętności i Learning Outcomes pokazuje, co zostało opanowane i gdzie potrzebne jest dodatkowe wsparcie.'],
        ['Planuj i kieruj pracą z wyprzedzeniem','Przygotuj aktywności, zadania i oceny wcześniej, śledź wykonanie oraz wykorzystuj dane do trafniejszych interwencji.']
      ], visual:teacherVisual
    }
  ] : [
    {
      key:'student', tab:'Student experience', eyebrow:'STUDENT EXPERIENCE', title:'Learn mathematics in a smarter way', intro:'An interactive experience makes each concept clearer and easier to understand.',
      points:[
        ['Learn by doing','Students work through activities and learning challenges that encourage thinking, exploration and step-by-step problem solving.'],
        ['Learning that adapts to the student','Edulytics identifies mastered skills and areas that need more support, helping guide the next learning step more appropriately.'],
        ['From learning to measurable results','Every answer contributes to a clearer view of progress, helping teachers spot weaker areas and respond more precisely.']
      ], visual:studentVisual
    },
    {
      key:'parent', tab:'Parent experience', eyebrow:'PARENT EXPERIENCE', title:'Clearer learning for your child. Easier visibility for you.', intro:'You do not need to monitor every activity to understand how learning is progressing.',
      points:[
        ['No need to follow every step','Edulytics analyses performance, identifies weaker areas and brings progress into one clear view.'],
        ['Know what your child has mastered','See mastered skills, concepts that need more practice and the current level of progress.'],
        ['Stay informed without constant monitoring','Clear summaries can surface learning time, completed activities, mastery and consistency.']
      ], visual:parentVisual
    },
    {
      key:'teacher', tab:'Teacher experience', eyebrow:'TEACHER EXPERIENCE', title:'Less administration. More teaching.', intro:'Edulytics reduces repetitive assessment and analysis work while keeping instructional decisions in the teacher’s hands.',
      points:[
        ['Save time. Keep control.','Use AI to create assessments and questions faster, then review, edit and approve everything before it reaches students.'],
        ['Know what each student needs','Skill- and Learning Outcome-level analysis shows what each learner has mastered and where more support is needed.'],
        ['Plan ahead and direct learning','Prepare activities, assignments and assessments in advance, track completion and use clearer data to intervene at the right time.']
      ], visual:teacherVisual
    }
  ];

  const oldExperience = root.querySelector('#experience');
  if (oldExperience) {
    const section = document.createElement('section');
    section.id = 'experience';
    section.className = 'ed-home-v7-experience';
    section.innerHTML = `<div class="ed-home-v7-shell"><div class="ed-home-v7-tabs" role="tablist">${experienceData.map((x,i) => `<button type="button" role="tab" class="ed-home-v7-tab ${i === 0 ? 'is-active' : ''}" data-v7-tab="${i}" aria-selected="${i === 0 ? 'true' : 'false'}">${x.tab}</button>`).join('')}</div><div class="ed-home-v7-grid"><div class="ed-home-v7-visual"></div><div class="ed-home-v7-copy"><span class="ed-home-v7-eyebrow"></span><h2></h2><p class="ed-home-v7-intro"></p><div class="ed-home-v7-points"></div></div></div></div>`;
    oldExperience.replaceWith(section);
    const visual = section.querySelector('.ed-home-v7-visual');
    const eyebrow = section.querySelector('.ed-home-v7-eyebrow');
    const title = section.querySelector('h2');
    const intro = section.querySelector('.ed-home-v7-intro');
    const points = section.querySelector('.ed-home-v7-points');
    const tabs = [...section.querySelectorAll('.ed-home-v7-tab')];
    const applyExperience = index => {
      const item = experienceData[index];
      visual.innerHTML = item.visual;
      eyebrow.textContent = item.eyebrow;
      title.textContent = item.title;
      intro.textContent = item.intro;
      points.innerHTML = item.points.map(p => `<article><h3>${p[0]}</h3><p>${p[1]}</p></article>`).join('');
      tabs.forEach((tab,i) => {
        tab.classList.toggle('is-active', i === index);
        tab.setAttribute('aria-selected', i === index ? 'true' : 'false');
      });
    };
    tabs.forEach((tab,i) => tab.addEventListener('click', () => applyExperience(i)));
    applyExperience(0);
  }

  root.querySelectorAll('.ed-home-mobile-panel a').forEach(a => a.addEventListener('click', () => {
    const menu = root.querySelector('.ed-home-mobile-menu');
    if (menu) menu.open = false;
  }));
  document.addEventListener('keydown', e => {
    if (e.key !== 'Escape') return;
    const menu = root.querySelector('.ed-home-mobile-menu');
    if (menu) menu.open = false;
  });
})();