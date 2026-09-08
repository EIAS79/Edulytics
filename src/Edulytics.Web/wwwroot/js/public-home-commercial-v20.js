(() => {
  const root = document.querySelector('.ed-home');
  const section = root?.querySelector('#curricula');
  const container = section?.querySelector('.ed-home-container');
  if (!root || !section || !container) return;

  const selectedLanguage = (root.dataset.siteLanguage || document.documentElement.lang || 'en').toLowerCase();
  const language = selectedLanguage.startsWith('ar') ? 'ar' : selectedLanguage.startsWith('pl') ? 'pl' : 'en';

  const copy = {
    en: {
      kicker: 'CURRICULUM COVERAGE',
      title: 'Supported curriculum pathways',
      intro: 'Explore the complete mathematics pathway available in Edulytics for each curriculum.',
      lessons: 'ready lessons',
      pathway: 'Pathway',
      structure: 'Curriculum structure',
      available: 'Available in Edulytics',
      structureValue: 'Units · Topics · Lessons · Learning outcomes',
      availableValue: 'Lessons · Examples · Practice · Assessments · AI question generation · Progress tracking',
      tabs: [
        {
          label: 'Cambridge',
          provider: 'Cambridge International Education',
          title: 'Cambridge International Mathematics',
          description: 'A complete mathematics pathway from Primary through AS & A Level.',
          count: '566',
          pathway: 'Primary 0096 · Stages 1–6  |  Lower Secondary 0862 · Stages 7–9  |  IGCSE 0580  |  AS & A Level 9709'
        },
        {
          label: 'Common Core',
          provider: 'NGA Center / CCSSO',
          title: 'Common Core State Standards Mathematics',
          description: 'Grade 1–8 coverage with advanced secondary mathematics pathways.',
          count: '1,560',
          pathway: 'Grades 1–8  |  Algebra 1  |  Geometry  |  Algebra 2  |  Advanced Algebra & Functions  |  Advanced Trigonometry & Geometry'
        },
        {
          label: 'Polish Curriculum',
          provider: 'Polish education authorities',
          title: 'Polish National Curriculum Mathematics',
          description: 'A complete mathematics pathway across Klasy I–VIII.',
          count: '1,569',
          pathway: 'Klasa I · Klasa II · Klasa III · Klasa IV · Klasa V · Klasa VI · Klasa VII · Klasa VIII'
        },
        {
          label: 'UAE',
          provider: 'UAE Ministry of Education',
          title: 'UAE Ministry of Education Mathematics',
          description: 'A complete mathematics pathway across Grades 1–12.',
          count: '758',
          pathway: 'Grade 1 · Grade 2 · Grade 3 · Grade 4 · Grade 5 · Grade 6 · Grade 7 · Grade 8 · Grade 9 · Grade 10 · Grade 11 · Grade 12'
        }
      ]
    },
    pl: {
      kicker: 'ZAKRES PROGRAMÓW',
      title: 'Obsługiwane ścieżki programów nauczania',
      intro: 'Poznaj pełną ścieżkę matematyki dostępną w Edulytics dla każdego programu nauczania.',
      lessons: 'gotowych lekcji',
      pathway: 'Ścieżka',
      structure: 'Struktura programu',
      available: 'Dostępne w Edulytics',
      structureValue: 'Działy · Tematy · Lekcje · Efekty uczenia się',
      availableValue: 'Lekcje · Przykłady · Ćwiczenia · Oceny · Generowanie pytań AI · Śledzenie postępów',
      tabs: [
        {
          label: 'Cambridge',
          provider: 'Cambridge International Education',
          title: 'Cambridge International Mathematics',
          description: 'Pełna ścieżka matematyki od Primary do AS & A Level.',
          count: '566',
          pathway: 'Primary 0096 · Stages 1–6  |  Lower Secondary 0862 · Stages 7–9  |  IGCSE 0580  |  AS & A Level 9709'
        },
        {
          label: 'Common Core',
          provider: 'NGA Center / CCSSO',
          title: 'Common Core State Standards Mathematics',
          description: 'Zakres Grades 1–8 oraz zaawansowane ścieżki matematyki na poziomie secondary.',
          count: '1 560',
          pathway: 'Grades 1–8  |  Algebra 1  |  Geometry  |  Algebra 2  |  Advanced Algebra & Functions  |  Advanced Trigonometry & Geometry'
        },
        {
          label: 'Program polski',
          provider: 'Polskie władze oświatowe',
          title: 'Polska podstawa programowa — Matematyka',
          description: 'Pełna ścieżka matematyki dla klas I–VIII.',
          count: '1 569',
          pathway: 'Klasa I · Klasa II · Klasa III · Klasa IV · Klasa V · Klasa VI · Klasa VII · Klasa VIII'
        },
        {
          label: 'UAE',
          provider: 'UAE Ministry of Education',
          title: 'UAE Ministry of Education Mathematics',
          description: 'Pełna ścieżka matematyki dla Grades 1–12.',
          count: '758',
          pathway: 'Grade 1 · Grade 2 · Grade 3 · Grade 4 · Grade 5 · Grade 6 · Grade 7 · Grade 8 · Grade 9 · Grade 10 · Grade 11 · Grade 12'
        }
      ]
    },
    ar: {
      kicker: 'تغطية المناهج',
      title: 'مسارات المناهج التعليمية المدعومة',
      intro: 'استعرض مسار الرياضيات الكامل المتاح في Edulytics لكل منهج تعليمي.',
      lessons: 'درسًا جاهزًا',
      pathway: 'المسار',
      structure: 'هيكل المنهج',
      available: 'المتاح في Edulytics',
      structureValue: 'وحدات · موضوعات · دروس · نواتج تعلّم',
      availableValue: 'دروس · أمثلة · تدريب · تقييمات · توليد أسئلة بالذكاء الاصطناعي · متابعة التقدم',
      tabs: [
        {
          label: 'Cambridge',
          provider: 'Cambridge International Education',
          title: 'Cambridge International Mathematics',
          description: 'مسار رياضيات كامل من Primary حتى AS & A Level.',
          count: '566',
          pathway: 'Primary 0096 · Stages 1–6  |  Lower Secondary 0862 · Stages 7–9  |  IGCSE 0580  |  AS & A Level 9709'
        },
        {
          label: 'Common Core',
          provider: 'NGA Center / CCSSO',
          title: 'Common Core State Standards Mathematics',
          description: 'تغطية Grades 1–8 مع مسارات رياضيات متقدمة للمرحلة الثانوية.',
          count: '1,560',
          pathway: 'Grades 1–8  |  Algebra 1  |  Geometry  |  Algebra 2  |  Advanced Algebra & Functions  |  Advanced Trigonometry & Geometry'
        },
        {
          label: 'المنهج البولندي',
          provider: 'الجهات التعليمية البولندية',
          title: 'المنهج الوطني البولندي للرياضيات',
          description: 'مسار رياضيات كامل من Klasa I حتى Klasa VIII.',
          count: '1,569',
          pathway: 'Klasa I · Klasa II · Klasa III · Klasa IV · Klasa V · Klasa VI · Klasa VII · Klasa VIII'
        },
        {
          label: 'UAE',
          provider: 'UAE Ministry of Education',
          title: 'UAE Ministry of Education Mathematics',
          description: 'مسار رياضيات كامل من Grade 1 حتى Grade 12.',
          count: '758',
          pathway: 'Grade 1 · Grade 2 · Grade 3 · Grade 4 · Grade 5 · Grade 6 · Grade 7 · Grade 8 · Grade 9 · Grade 10 · Grade 11 · Grade 12'
        }
      ]
    }
  }[language];

  section.classList.add('ed-home-v20-curricula');
  if (language === 'ar') {
    section.classList.add('is-ar');
    section.setAttribute('dir', 'rtl');
  }

  container.innerHTML = `
    <header class="ed-home-v20-head">
      <span class="ed-home-v20-kicker">${copy.kicker}</span>
      <h2>${copy.title}</h2>
      <p>${copy.intro}</p>
    </header>
    <div class="ed-home-v20-tablist" role="tablist" aria-label="${copy.title}">
      ${copy.tabs.map((tab, index) => `
        <button class="ed-home-v20-tab${index === 0 ? ' is-active' : ''}" type="button" role="tab" id="ed-curriculum-tab-${index}" aria-controls="ed-curriculum-panel" aria-selected="${index === 0 ? 'true' : 'false'}" tabindex="${index === 0 ? '0' : '-1'}" data-index="${index}">${tab.label}</button>
      `).join('')}
    </div>
    <div id="ed-curriculum-panel" class="ed-home-v20-panel" role="tabpanel" aria-labelledby="ed-curriculum-tab-0" tabindex="0"></div>
  `;

  const panel = container.querySelector('.ed-home-v20-panel');
  const tabs = [...container.querySelectorAll('.ed-home-v20-tab')];

  const renderPanel = (index) => {
    const tab = copy.tabs[index];
    panel.setAttribute('aria-labelledby', `ed-curriculum-tab-${index}`);
    panel.innerHTML = `
      <div class="ed-home-v20-lead">
        <span class="ed-home-v20-provider">${tab.provider}</span>
        <h3>${tab.title}</h3>
        <p>${tab.description}</p>
        <div class="ed-home-v20-count"><strong>${tab.count}</strong><span>${copy.lessons}</span></div>
      </div>
      <div class="ed-home-v20-details">
        <div class="ed-home-v20-detail-row">
          <span>${copy.pathway}</span>
          <p>${tab.pathway}</p>
        </div>
        <div class="ed-home-v20-detail-row">
          <span>${copy.structure}</span>
          <p>${copy.structureValue}</p>
        </div>
        <div class="ed-home-v20-detail-row">
          <span>${copy.available}</span>
          <p>${copy.availableValue}</p>
        </div>
      </div>`;
  };

  const activate = (index, moveFocus = false) => {
    tabs.forEach((tab, tabIndex) => {
      const active = tabIndex === index;
      tab.classList.toggle('is-active', active);
      tab.setAttribute('aria-selected', active ? 'true' : 'false');
      tab.tabIndex = active ? 0 : -1;
    });
    renderPanel(index);
    if (moveFocus) tabs[index].focus();
  };

  tabs.forEach((tab, index) => {
    tab.addEventListener('click', () => activate(index));
    tab.addEventListener('keydown', (event) => {
      let next = index;
      if (event.key === 'ArrowRight') next = (index + 1) % tabs.length;
      else if (event.key === 'ArrowLeft') next = (index - 1 + tabs.length) % tabs.length;
      else if (event.key === 'Home') next = 0;
      else if (event.key === 'End') next = tabs.length - 1;
      else return;
      event.preventDefault();
      activate(next, true);
    });
  });

  renderPanel(0);

  if (!document.getElementById('ed-home-v20-curricula-style')) {
    const style = document.createElement('style');
    style.id = 'ed-home-v20-curricula-style';
    style.textContent = `
      .ed-home .ed-home-v20-curricula{padding:82px 28px 90px;background:#fff}
      .ed-home .ed-home-v20-curricula .ed-home-container{width:min(1480px,100%);margin:0 auto}
      .ed-home .ed-home-v20-head{text-align:center;max-width:900px;margin:0 auto}
      .ed-home .ed-home-v20-kicker{display:block;margin-bottom:12px;color:#286df0;font-size:13px;line-height:1;font-weight:900;letter-spacing:.14em;text-transform:uppercase}
      .ed-home .ed-home-v20-head h2{margin:0;color:#10243f;font-size:clamp(34px,3vw,52px);line-height:1.12;font-weight:900;letter-spacing:-.035em}
      .ed-home .ed-home-v20-head p{margin:17px auto 0;max-width:790px;color:#6d7f96;font-size:18px;line-height:1.6;font-weight:500}
      .ed-home .ed-home-v20-tablist{margin:48px auto 0;display:grid;grid-template-columns:repeat(4,minmax(0,1fr));border-bottom:1px solid #dfe6ef}
      .ed-home .ed-home-v20-tab{appearance:none;border:0;border-bottom:3px solid transparent;background:transparent;margin:0 0 -1px;padding:0 18px 18px;color:#718096;font:inherit;font-size:20px;line-height:1.2;font-weight:800;text-align:center;cursor:pointer;transition:color .18s ease,border-color .18s ease}
      .ed-home .ed-home-v20-tab:hover{color:#244e89}
      .ed-home .ed-home-v20-tab.is-active{color:#10243f;border-bottom-color:#286df0}
      .ed-home .ed-home-v20-tab:focus-visible{outline:3px solid rgba(40,109,240,.22);outline-offset:5px;border-radius:4px}
      .ed-home .ed-home-v20-panel{display:grid;grid-template-columns:minmax(300px,.88fr) minmax(0,1.55fr);gap:72px;padding:52px 0 10px;outline:none}
      .ed-home .ed-home-v20-lead{padding-inline-end:28px}
      .ed-home .ed-home-v20-provider{display:block;margin-bottom:11px;color:#286df0;font-size:13px;line-height:1.35;font-weight:850;letter-spacing:.08em;text-transform:uppercase}
      .ed-home .ed-home-v20-lead h3{margin:0;color:#10243f;font-size:clamp(29px,2.3vw,42px);line-height:1.12;font-weight:900;letter-spacing:-.03em}
      .ed-home .ed-home-v20-lead>p{margin:17px 0 0;color:#5d718b;font-size:17px;line-height:1.62;font-weight:500;max-width:520px}
      .ed-home .ed-home-v20-count{display:flex;align-items:baseline;gap:11px;margin-top:28px;color:#286df0}
      .ed-home .ed-home-v20-count strong{font-size:clamp(40px,3.4vw,58px);line-height:.95;font-weight:900;letter-spacing:-.04em}
      .ed-home .ed-home-v20-count span{color:#677990;font-size:14px;font-weight:750;text-transform:uppercase;letter-spacing:.06em}
      .ed-home .ed-home-v20-details{border-top:1px solid #dfe6ef}
      .ed-home .ed-home-v20-detail-row{display:grid;grid-template-columns:180px minmax(0,1fr);gap:34px;padding:24px 0;border-bottom:1px solid #dfe6ef;align-items:start}
      .ed-home .ed-home-v20-detail-row>span{color:#10243f;font-size:14px;line-height:1.45;font-weight:900;text-transform:uppercase;letter-spacing:.055em}
      .ed-home .ed-home-v20-detail-row p{margin:0;color:#506781;font-size:16px;line-height:1.7;font-weight:550}
      .ed-home .ed-home-v20-curricula.is-ar .ed-home-v20-head,
      .ed-home .ed-home-v20-curricula.is-ar .ed-home-v20-lead,
      .ed-home .ed-home-v20-curricula.is-ar .ed-home-v20-detail-row{text-align:right}
      .ed-home .ed-home-v20-curricula.is-ar .ed-home-v20-tablist{direction:rtl}
      .ed-home .ed-home-v20-curricula.is-ar .ed-home-v20-provider,
      .ed-home .ed-home-v20-curricula.is-ar .ed-home-v20-kicker{letter-spacing:0;text-transform:none}
      .ed-home .ed-home-v20-curricula.is-ar .ed-home-v20-count{direction:rtl;justify-content:flex-start}
      .ed-home .ed-home-v20-curricula.is-ar .ed-home-v20-count span{text-transform:none;letter-spacing:0}
      @media(max-width:1050px){
        .ed-home .ed-home-v20-curricula{padding:68px 24px 74px}
        .ed-home .ed-home-v20-tab{font-size:18px;padding-inline:10px}
        .ed-home .ed-home-v20-panel{grid-template-columns:1fr;gap:34px;padding-top:42px}
        .ed-home .ed-home-v20-lead{padding-inline-end:0;max-width:760px}
      }
      @media(max-width:720px){
        .ed-home .ed-home-v20-curricula{padding:54px 18px 60px}
        .ed-home .ed-home-v20-head{text-align:start}
        .ed-home .ed-home-v20-head h2{font-size:34px}
        .ed-home .ed-home-v20-head p{font-size:16px}
        .ed-home .ed-home-v20-tablist{display:flex;overflow-x:auto;gap:24px;margin-top:34px;scrollbar-width:none;white-space:nowrap}
        .ed-home .ed-home-v20-tablist::-webkit-scrollbar{display:none}
        .ed-home .ed-home-v20-tab{flex:0 0 auto;padding:0 2px 14px;font-size:17px}
        .ed-home .ed-home-v20-panel{padding-top:34px}
        .ed-home .ed-home-v20-lead h3{font-size:30px}
        .ed-home .ed-home-v20-detail-row{grid-template-columns:1fr;gap:8px;padding:20px 0}
        .ed-home .ed-home-v20-detail-row p{font-size:15.5px}
        .ed-home .ed-home-v20-count strong{font-size:46px}
      }
    `;
    document.head.appendChild(style);
  }
})();
