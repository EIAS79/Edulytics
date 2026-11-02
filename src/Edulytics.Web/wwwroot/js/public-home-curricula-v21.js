(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  const selectedLanguage = (root.dataset.siteLanguage || document.documentElement.lang || 'en').toLowerCase();
  const language = selectedLanguage.startsWith('ar') ? 'ar' : selectedLanguage.startsWith('pl') ? 'pl' : 'en';

  // Verified against the staging curriculum data on 2026-09-08.
  // Lesson totals: Cambridge 566, Common Core 1,560, Polish 1,569, UAE 758.
  const curriculumCorrections = {
    en: [
      {
        description: 'A complete mathematics pathway from Primary through AS & A Level.',
        count: '566',
        pathway: 'Primary 0096 · Stages 1–6  |  Lower Secondary 0862 · Stages 7–9  |  IGCSE 0580 · Core & Extended  |  AS & A Level 9709'
      },
      {
        description: 'Grades 1–8 plus complete secondary mathematics pathways through advanced courses.',
        count: '1,560',
        pathway: 'Grades 1–8  |  Traditional: Algebra 1 · Geometry · Algebra 2 · Traditional Advanced Supplements  |  Fourth Courses: Advanced Algebra & Functions · Advanced Trigonometry & Geometry · Complex Numbers · Probability & Decision Making · Vectors & Matrices'
      },
      {
        description: 'A complete mathematics pathway from primary school through Liceum and Technikum.',
        count: '1,569',
        pathway: 'Szkoła podstawowa: Klasa I–VIII  |  Liceum ogólnokształcące: Klasa I–IV  |  Technikum: Klasa I–V'
      },
      {
        description: 'A complete mathematics pathway across Grades 1–12, including General and Advanced tracks.',
        count: '758',
        pathway: 'Grades 1–12  |  Common: Grades 1–4  |  General & Advanced: Grades 5–12'
      }
    ],
    pl: [
      {
        description: 'Pełna ścieżka matematyki od Primary do AS & A Level.',
        count: '566',
        pathway: 'Primary 0096 · Stages 1–6  |  Lower Secondary 0862 · Stages 7–9  |  IGCSE 0580 · Core & Extended  |  AS & A Level 9709'
      },
      {
        description: 'Zakres Grades 1–8 oraz pełne ścieżki matematyki na poziomie szkoły średniej, w tym kursy zaawansowane.',
        count: '1 560',
        pathway: 'Grades 1–8  |  Traditional: Algebra 1 · Geometry · Algebra 2 · Traditional Advanced Supplements  |  Fourth Courses: Advanced Algebra & Functions · Advanced Trigonometry & Geometry · Complex Numbers · Probability & Decision Making · Vectors & Matrices'
      },
      {
        description: 'Pełna ścieżka matematyki od szkoły podstawowej przez liceum i technikum.',
        count: '1 569',
        pathway: 'Szkoła podstawowa: Klasa I–VIII  |  Liceum ogólnokształcące: Klasa I–IV  |  Technikum: Klasa I–V'
      },
      {
        description: 'Pełna ścieżka matematyki dla Grades 1–12, obejmująca ścieżki General i Advanced.',
        count: '758',
        pathway: 'Grades 1–12  |  Common: Grades 1–4  |  General & Advanced: Grades 5–12'
      }
    ],
    ar: [
      {
        description: 'مسار رياضيات كامل من Primary حتى AS & A Level.',
        count: '566',
        pathway: 'Primary 0096 · Stages 1–6  |  Lower Secondary 0862 · Stages 7–9  |  IGCSE 0580 · Core & Extended  |  AS & A Level 9709'
      },
      {
        description: 'تغطية Grades 1–8 مع مسارات رياضيات ثانوية كاملة وصولًا إلى المقررات المتقدمة.',
        count: '1,560',
        pathway: 'Grades 1–8  |  Traditional: Algebra 1 · Geometry · Algebra 2 · Traditional Advanced Supplements  |  Fourth Courses: Advanced Algebra & Functions · Advanced Trigonometry & Geometry · Complex Numbers · Probability & Decision Making · Vectors & Matrices'
      },
      {
        description: 'مسار رياضيات كامل من المدرسة الأساسية عبر Liceum وTechnikum.',
        count: '1,569',
        pathway: 'Szkoła podstawowa: Klasa I–VIII  |  Liceum ogólnokształcące: Klasa I–IV  |  Technikum: Klasa I–V'
      },
      {
        description: 'مسار رياضيات كامل من Grade 1 حتى Grade 12، ويشمل المسارين General وAdvanced.',
        count: '758',
        pathway: 'Grades 1–12  |  Common: Grades 1–4  |  General & Advanced: Grades 5–12'
      }
    ]
  };

  const corrections = curriculumCorrections[language];
  let attempts = 0;

  const patchActivePanel = () => {
    const section = root.querySelector('#curricula');
    const panel = section?.querySelector('.ed-home-v20-panel');
    const tabs = [...(section?.querySelectorAll('.ed-home-v20-tab') || [])];
    if (!panel || tabs.length !== 4) return false;

    const activeIndex = Math.max(0, tabs.findIndex(tab => tab.classList.contains('is-active')));
    const data = corrections[activeIndex];
    if (!data) return true;

    const description = panel.querySelector('.ed-home-v20-lead > p');
    const count = panel.querySelector('.ed-home-v20-count strong');
    const pathway = panel.querySelector('.ed-home-v20-detail-row p');

    if (description) description.textContent = data.description;
    if (count) count.textContent = data.count;
    if (pathway) pathway.textContent = data.pathway;
    return true;
  };

  const initialize = () => {
    const section = root.querySelector('#curricula');
    const tabs = [...(section?.querySelectorAll('.ed-home-v20-tab') || [])];
    if (tabs.length !== 4 || !section?.querySelector('.ed-home-v20-panel')) {
      attempts += 1;
      if (attempts < 80) window.setTimeout(initialize, 100);
      return;
    }

    patchActivePanel();
    tabs.forEach(tab => {
      tab.addEventListener('click', () => window.setTimeout(patchActivePanel, 0));
      tab.addEventListener('keydown', () => window.setTimeout(patchActivePanel, 0));
    });
  };

  initialize();
})();
