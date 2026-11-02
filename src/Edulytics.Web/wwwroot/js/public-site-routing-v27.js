(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  const normalize = value => String(value || '').replace(/\s+/g, ' ').trim();
  const currentPath = window.location.pathname.replace(/\/+$/, '') || '/';

  const labelRoutes = new Map([
    ['Learning Built for Understanding','/product/learning-built-for-understanding'],
    ['Nauka oparta na zrozumieniu','/product/learning-built-for-understanding'],
    ['تعلّم قائم على الفهم','/product/learning-built-for-understanding'],
    ['Results Backed by Data','/product/results-backed-by-data'],
    ['Wyniki potwierdzone danymi','/product/results-backed-by-data'],
    ['نتائج تثبتها البيانات','/product/results-backed-by-data'],
    ['Support You Can Rely On','/product/support-you-can-rely-on'],
    ['Wsparcie, na którym możesz polegać','/product/support-you-can-rely-on'],
    ['دعم يمكنك الاعتماد عليه','/product/support-you-can-rely-on'],
    ['Explore Student Portal','/product/student-portal'],
    ['Poznaj portal ucznia','/product/student-portal'],
    ['استكشف بوابة الطالب','/product/student-portal'],
    ['Student experience','/product/student-portal'],
    ['Doświadczenie ucznia','/product/student-portal'],
    ['تجربة الطالب','/product/student-portal'],
    ['Product Features','/product/features'],
    ['Funkcje produktu','/product/features'],
    ['ميزات المنتج','/product/features'],
    ['AI Assistant','/product/edulytics-ai'],
    ['Asystent AI','/product/edulytics-ai'],
    ['مساعد الذكاء الاصطناعي','/product/edulytics-ai'],
    ['Edulytics AI','/product/edulytics-ai'],
    ['AI for Teachers','/product/edulytics-ai'],
    ['AI dla nauczycieli','/product/edulytics-ai'],
    ['الذكاء الاصطناعي للمعلمين','/product/edulytics-ai'],
    ['Multilingual Editions','/product/languages'],
    ['Wersje wielojęzyczne','/product/languages'],
    ['إصدارات متعددة اللغات','/product/languages'],
    ['Technical Requirements','/product/technical-requirements'],
    ['Wymagania techniczne','/product/technical-requirements'],
    ['المتطلبات الفنية','/product/technical-requirements'],
    ['Mathematics','/product/mathematics'],
    ['Matematyka','/product/mathematics'],
    ['الرياضيات','/product/mathematics'],
    ['Why Edulytics for Teachers?','/teachers/overview'],
    ['Dlaczego Edulytics dla nauczycieli?','/teachers/overview'],
    ['لماذا Edulytics للمعلمين؟','/teachers/overview'],
    ['Why Edulytics for Home?','/parents/overview'],
    ['Dlaczego Edulytics w domu?','/parents/overview'],
    ['لماذا Edulytics للمنزل؟','/parents/overview'],
    ['Why Edulytics for Education Leaders?','/schools/overview'],
    ['Dlaczego Edulytics dla liderów edukacji?','/schools/overview'],
    ['لماذا Edulytics للقيادات التعليمية؟','/schools/overview'],
    ['Global Partnerships','/company/partnerships'],
    ['Partnerstwa globalne','/company/partnerships'],
    ['الشراكات العالمية','/company/partnerships'],
    ['Teachers','/teachers/overview'],
    ['Nauczyciele','/teachers/overview'],
    ['المعلمون','/teachers/overview'],
    ['Students','/students/overview'],
    ['Uczniowie','/students/overview'],
    ['الطلاب','/students/overview'],
    ['About','/company/about'],
    ['O nas','/company/about'],
    ['من نحن','/company/about'],
    ['Curricula','/product/curricula'],
    ['Programy nauczania','/product/curricula'],
    ['المناهج','/product/curricula']
  ]);

  const replacements = new Map([
    ['Math Quizzes',['Assessments & Practice','/product/assessment-and-practice']],
    ['Quizy matematyczne',['Ocenianie i ćwiczenia','/product/assessment-and-practice']],
    ['اختبارات الرياضيات',['التقييم والتدريب','/product/assessment-and-practice']],
    ['Weekly Challenge',['Mastery & Next Step','/product/mastery-and-next-step']],
    ['Cotygodniowe wyzwanie',['Mastery i kolejny krok','/product/mastery-and-next-step']],
    ['التحدي الأسبوعي',['الإتقان والخطوة التالية','/product/mastery-and-next-step']],
    ['Financial Literacy',['Supported Curricula','/product/curricula']],
    ['Edukacja finansowa',['Obsługiwane programy','/product/curricula']],
    ['المعرفة المالية',['المناهج المدعومة','/product/curricula']],
    ['Global Partnerships',['Partnerships','/company/partnerships']],
    ['Partnerstwa globalne',['Partnerstwa','/company/partnerships']],
    ['الشراكات العالمية',['الشراكات','/company/partnerships']]
  ]);

  const groupHeadReplacements = new Map([
    ['Subjects','Learning scope'],
    ['Tematy','Zakres nauki'],
    ['المواضيع','نطاق التعلّم']
  ]);

  root.querySelectorAll('.ed-home-v11-mega-group h3, .ed-home-v11-mobile-submenu>strong').forEach(heading => {
    const replacement = groupHeadReplacements.get(normalize(heading.textContent));
    if (replacement) heading.textContent = replacement;
  });

  const resolveAudience = anchor => {
    const desktop = anchor.closest('[data-v11-menu]')?.dataset.v11Menu;
    if (desktop) return desktop;
    const mobile = anchor.closest('.ed-home-v11-mobile-group');
    const summary = normalize(mobile?.querySelector(':scope>summary')?.textContent).replace('＋','').trim().toLowerCase();
    if (/teacher|nauczyc|معلم/.test(summary)) return 'teachers';
    if (/parent|rodzic|ولي|أولياء/.test(summary)) return 'parents';
    if (/school|lider|szko|مدارس|قيادات/.test(summary)) return 'leaders';
    if (/product|produkt|المنتج/.test(summary)) return 'product';
    return '';
  };

  root.querySelectorAll('a').forEach(anchor => {
    const label = normalize(anchor.textContent);

    const replacement = replacements.get(label);
    if (replacement) {
      anchor.textContent = replacement[0];
      anchor.setAttribute('href', replacement[1]);
      return;
    }

    if (label === 'Activities & Curriculum' || label === 'Aktywności i program nauczania' || label === 'الأنشطة والمناهج الدراسية') {
      const audience = resolveAudience(anchor);
      if (audience === 'teachers') {
        anchor.textContent = label === 'Activities & Curriculum' ? 'Assessment & Curriculum' : label === 'Aktywności i program nauczania' ? 'Ocenianie i program nauczania' : 'التقييم والمنهج';
        anchor.setAttribute('href', '/teachers/assessment-and-curriculum');
      } else {
        anchor.setAttribute('href', '/product/curricula');
      }
      return;
    }

    const mapped = labelRoutes.get(label);
    if (mapped) {
      anchor.setAttribute('href', mapped);
      return;
    }

    if (currentPath !== '/') {
      const href = anchor.getAttribute('href');
      if (href && href.startsWith('#')) anchor.setAttribute('href', `/${href}`);
    }
  });

  /* Footer navigation should use the new internal pages when those pages exist. */
  root.querySelectorAll('.ed-home-footer a').forEach(anchor => {
    const label = normalize(anchor.textContent);
    const footerRoutes = {
      'Principles':'/#platform','Zasady':'/#platform','المبادئ':'/#platform',
      'Overview':'/schools/overview','Przegląd':'/schools/overview','نظرة عامة':'/schools/overview',
      'Experience':'/product/student-portal','Doświadczenie':'/product/student-portal','التجربة':'/product/student-portal'
    };
    if (footerRoutes[label]) anchor.setAttribute('href', footerRoutes[label]);
  });
})();
