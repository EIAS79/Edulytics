(() => {
  const root = document.querySelector('.ed-home');
  if (!root || root.classList.contains('ed-content-page')) return;

  const storageKey = 'edulytics.public.siteLanguage';
  let language = (document.documentElement.lang || 'en').toLowerCase().startsWith('ar') ? 'ar' : 'en';
  try {
    if (window.localStorage.getItem(storageKey) === 'ar') language = 'ar';
  } catch { /* storage may be blocked */ }
  if (language !== 'ar') return;

  document.documentElement.lang = 'ar';
  document.documentElement.dir = 'rtl';
  document.documentElement.classList.add('ed-site-ar');
  root.classList.add('is-site-ar');
  root.dataset.siteLanguage = 'ar';

  const setText = (selector, value) => {
    const node = root.querySelector(selector);
    if (node) node.textContent = value;
  };
  const setTexts = (selector, values) => {
    root.querySelectorAll(selector).forEach((node, index) => {
      if (values[index] !== undefined) node.textContent = values[index];
    });
  };

  // Arabic public pages use the international Edulytics brand, never the Polish product mark.
  root.querySelectorAll('.localized-brand-logo').forEach(image => {
    image.setAttribute('src', '/images/brand/edulytics-en.png');
    image.setAttribute('alt', 'Edulytics');
  });

  document.title = 'Edulytics | منصة تعلّم وتقييم الرياضيات';
  const meta = document.querySelector('meta[name="description"]');
  if (meta) {
    meta.setAttribute('content', 'Edulytics منصة رياضيات تربط المنهج والدروس والتقييم والتدريب وتحليل الإتقان والتقدم للمدارس والمعلمين والطلاب.');
  }

  root.querySelectorAll('.ed-home-links').forEach(nav => nav.setAttribute('aria-label', 'التنقل الرئيسي'));
  root.querySelectorAll('.ed-home-mobile-menu > summary').forEach(summary => summary.setAttribute('aria-label', 'القائمة'));

  // Hero.
  setText('.ed-home-hero .ed-home-kicker', 'الرياضيات • الذكاء الاصطناعي • الإتقان');
  setText('.ed-home-hero h1', 'من المنهج إلى نتائج حقيقية في الرياضيات.');
  setText('.ed-home-hero-copy > p', 'يربط Edulytics المنهج والدروس الجاهزة والتقييم وتوليد الأسئلة بمساعدة الذكاء الاصطناعي والتدريب الشخصي للطالب في منصة واحدة مترابطة.');
  setText('.ed-home-hero-actions .ed-home-cta', 'اطلب عرضًا تجريبيًا ←');
  setText('.ed-home-hero-actions .ed-home-secondary', 'استكشف المنصة');
  setTexts('.ed-home-hero-checks span', [
    'متوافق مع المنهج',
    'ذكاء اصطناعي تحت تحكم المعلّم',
    'للمدارس والمعلمين والطلاب'
  ]);

  // Audiences.
  setText('#schools .ed-home-center-head h2', 'لمن صُمم Edulytics؟');
  const audienceCopy = [
    ['للمدارس', 'المناهج والفصول والنتائج والإشراف الأكاديمي في مكان واحد.', 'اعرف المزيد ←'],
    ['للمعلمين', 'أنشئ التقييمات، واستخدم الذكاء الاصطناعي، وتابع تقدم الطلاب.', 'أنشئ باستخدام Edulytics AI ←'],
    ['للطلاب', 'تعلّم من الدروس، وأكمل التقييمات، وتدرّب بشكل شخصي باستخدام الذكاء الاصطناعي.', 'استكشف تجربة الطالب ←'],
    ['للقيادات الأكاديمية', 'تابع تنفيذ المنهج والنتائج وجودة العملية التعليمية.', 'اعرف المزيد ←']
  ];
  root.querySelectorAll('#schools .audience-card').forEach((card, index) => {
    const copy = audienceCopy[index];
    if (!copy) return;
    const title = card.querySelector('h3');
    const body = card.querySelector('p');
    const action = card.querySelector('a');
    if (title) title.textContent = copy[0];
    if (body) body.textContent = copy[1];
    if (action) action.textContent = copy[2];
  });

  // Value proposition.
  setText('.ed-home-value-band .ed-home-center-head h2', 'بيئة متكاملة لتعليم الرياضيات');
  setTexts('.ed-home-value-icons span', [
    'المنهج',
    'دروس جاهزة',
    'توليد الأسئلة بالذكاء الاصطناعي',
    'التقييمات والنتائج',
    'تدريب شخصي',
    'إشراف المدرسة'
  ]);

  // Curriculum section. Keep curriculum/provider names in their official forms where appropriate.
  setText('#curricula .ed-home-center-head h2', 'مسارات المناهج التعليمية المدعومة');
  setText('#curricula .ed-home-center-head p', 'اختر المنهج والمستوى، وسيحافظ Edulytics على هذا السياق عبر الدروس والتقييمات وتحليل التقدم.');
  setTexts('#curricula .ed-home-tabs span', ['Cambridge', 'Common Core', 'المنهج البولندي', 'UAE']);
  setText('#curricula .curriculum-intro h3', 'رياضيات Cambridge');
  setText('#curricula .curriculum-intro p', 'المستوى والدروس وسياق المنهج في عرض واحد مترابط.');
  const curriculumLessons = [
    ['الأعداد الصحيحة والأعداد الموجّهة', ['درس', 'أمثلة', 'تدريب']],
    ['التعبيرات الجبرية', ['درس', 'تدريب', 'اختبار']],
    ['المعادلات والمتباينات', ['درس', 'تدريب', 'اختبار']]
  ];
  root.querySelectorAll('#curricula .curriculum-lessons article').forEach((article, index) => {
    const copy = curriculumLessons[index];
    if (!copy) return;
    const heading = article.querySelector('h4');
    if (heading) heading.textContent = copy[0];
    article.querySelectorAll('.lesson-tags span').forEach((tag, tagIndex) => {
      if (copy[1][tagIndex] !== undefined) tag.textContent = copy[1][tagIndex];
    });
  });

  // Experience section.
  setText('#experience .ed-home-center-head h2', 'تجربة Edulytics في الاستخدام اليومي');
  setTexts('#experience .ed-home-experience-tabs span', ['الطالب', 'المعلّم', 'القائد الأكاديمي', 'المدير']);
  setText('#experience .ed-home-experience-copy .ed-home-kicker', 'تجربة الطالب');
  setText('#experience .ed-home-experience-copy h2', 'تعلّم يقود إلى الخطوة التالية.');
  setTexts('#experience .ed-home-checks span', [
    'دروس تتضمن شرحًا وأمثلة',
    'تدريب شخصي مدعوم بالذكاء الاصطناعي',
    'نتائج وجوانب تحتاج إلى تحسين',
    'تدريب مرتبط بنطاق التعلّم المحدد'
  ]);
  setText('#experience .ed-home-experience-copy > a', 'استكشف بوابة الطالب ←');

  // Edulytics AI section.
  setText('#ai .ed-home-ai-copy h2', 'أنشئ التقييمات باستخدام Edulytics AI');
  setText('#ai .ed-home-ai-copy > p', 'أنشئ أسئلة للدروس والوحدات والمنهج الكامل. يراجع المعلّم الأسئلة ويعدّلها ويعتمدها ثم ينشرها.');
  setTexts('#ai .ed-home-ai-copy .ed-home-checks span', [
    'توليد أسئلة في سياق المنهج',
    'ربط تلقائي بنطاق التعلّم',
    'تعديل وإعادة توليد واعتماد',
    'حتى 50 سؤالًا في عملية توليد واحدة'
  ]);
  setText('#ai .ai-top strong', 'أنشئ باستخدام Edulytics AI');
  setText('#ai .ai-top span', 'مدعوم بالذكاء الاصطناعي');
  setTexts('#ai .ai-fields small', ['النطاق', 'عدد الأسئلة', 'المستوى']);
  setText('#ai .ai-fields > div:first-child b', 'درس · الجبر');
  setText('#ai .ai-question small', 'سؤال تم إنشاؤه');
  setTexts('#ai .ai-actions span', ['تعديل', 'إعادة توليد']);
  setText('#ai .ai-actions b', 'اعتماد');

  // The Learning Philosophy section is translated by public-home-philosophy-v34.js.

  // Start/sign-in section.
  setText('.ed-home-start .ed-home-center-head h2', 'ابدأ استخدام Edulytics');
  setText('.ed-home-start .ed-home-center-head p', 'اختر دورك وانتقل إلى تسجيل الدخول.');
  setTexts('.ed-home-start .role-grid strong', ['المعلّم', 'قائد المدرسة', 'الطالب', 'الموظف / المدير']);

  // Final CTA.
  setText('#contact h2', 'اربط المنهج والدروس والتقييم وتدريب الطالب في منصة واحدة.');
  setTexts('#contact .ed-home-final-actions a', ['اطلب عرضًا تجريبيًا ←', 'تواصل معنا']);

  // Homepage footer: global UI handles most public links; finish the hash-based homepage variant here.
  const footerColumns = root.querySelectorAll('.ed-home-footer-grid > div:not(.ed-home-footer-brand)');
  const footerCopy = [
    ['المنصة', ['المبادئ', 'المناهج', 'Edulytics AI']],
    ['للمدارس', ['نظرة عامة', 'تجربة الطالب', 'اطلب عرضًا تجريبيًا']],
    ['للمستخدمين', ['المعلمون', 'الطلاب', 'تسجيل الدخول']],
    ['الشركة', ['عن Edulytics', 'تواصل معنا', 'مصادر المحتوى والتراخيص']]
  ];
  footerColumns.forEach((column, index) => {
    const copy = footerCopy[index];
    if (!copy) return;
    const heading = column.querySelector('h3');
    if (heading) heading.textContent = copy[0];
    column.querySelectorAll('a').forEach((anchor, anchorIndex) => {
      if (copy[1][anchorIndex] !== undefined) anchor.textContent = copy[1][anchorIndex];
    });
  });
  setText('.ed-home-footer-brand p', 'الرياضيات. التعلّم. التقدم.');
  const footerBottom = root.querySelectorAll('.ed-home-footer-bottom > span');
  if (footerBottom.length > 1) footerBottom[1].textContent = 'الخصوصية · الشروط · تراخيص المحتوى';
})();