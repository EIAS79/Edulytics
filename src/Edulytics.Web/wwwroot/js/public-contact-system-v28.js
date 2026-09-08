(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  const storageKey = 'edulytics.public.siteLanguage';
  let language = (document.documentElement.lang || 'en').toLowerCase().startsWith('pl') ? 'pl' : 'en';
  try {
    if (window.localStorage.getItem(storageKey) === 'ar') language = 'ar';
  } catch { }

  const ar = {
    contactEyebrow: 'تواصل مع Edulytics',
    contactTitle: 'كيف يمكننا مساعدتك؟',
    contactLead: 'سواء كنت تستكشف Edulytics لمدرسة أو فصل دراسي أو تحتاج إلى دعم، اختر المسار الأنسب وسنوجّهك إلى الخطوة التالية.',
    audienceTeachers: 'المعلمون',
    audienceLeaders: 'المدارس والقيادات التعليمية',
    audienceParents: 'أولياء الأمور والطلاب',
    visualTitle: 'اختر نقطة البداية',
    visualSales: 'أسئلة حول المنتج والخطط والاستخدام في المدرسة',
    visualDemo: 'شاهد كيف تعمل المنصة في سياقك التعليمي',
    visualHelp: 'مساعدة في الوصول والاستخدام والدعم الفني',
    waysTitle: 'اختر ما تحتاج إليه',
    waysLead: 'قسمنا التواصل إلى مسارات واضحة حتى لا تضطر إلى إرسال نفس الطلب إلى أكثر من مكان.',
    salesTitle: 'استفسار المبيعات',
    salesBody: 'اسأل عن Edulytics، أو متطلبات مدرستك، أو كيفية البدء باستخدام المنصة.',
    salesAction: 'أرسل استفسارًا',
    demoTitle: 'اطلب عرضًا توضيحيًا',
    demoBody: 'اطلب جولة توضح كيف يربط Edulytics المنهج والتقييم والتدريب وقياس التقدم.',
    demoAction: 'اطلب العرض',
    helpTitle: 'مركز المساعدة',
    helpBody: 'اعثر على إرشادات حول الحسابات والوصول والمناهج والتقييم والتدريب والدعم الفني.',
    helpAction: 'استكشف المساعدة',
    directTitle: 'تفضّل التواصل مباشرة؟',
    directBody: 'يمكنك مراسلتنا أو الاتصال بنا باستخدام بيانات التواصل الرسمية لـ Edulytics.',
    emailAction: 'راسلنا بالبريد',
    callAction: 'اتصل بنا',

    salesEyebrow: 'استفسار المبيعات',
    salesPageTitle: 'تحدث معنا عن Edulytics.',
    salesPageLead: 'أخبرنا عن مدرستك أو احتياجك، وسنساعدك على تحديد ما إذا كان Edulytics مناسبًا وكيف يمكن البدء.',
    demoEyebrow: 'عرض Edulytics',
    demoPageTitle: 'شاهد Edulytics في سياق مدرستك.',
    demoPageLead: 'شاركنا معلومات أساسية عن دورك ومدرستك حتى نتمكن من جعل العرض أكثر ارتباطًا بما تريد تقييمه.',
    backContact: 'العودة إلى التواصل',
    trustCurriculum: 'مبني حول المنهج ونتائج التعلّم',
    trustTeacher: 'المعلم يظل صاحب القرار في التقييمات الرسمية',
    trustPractice: 'التدريب منفصل عن الدرجات المدرسية الرسمية',
    trustNext: 'النتائج تقود إلى خطوة تعليمية تالية واضحة',
    formSalesTitle: 'أخبرنا بما تحتاج إليه',
    formSalesLead: 'املأ البيانات التالية لفتح رسالة بريد منظّمة إلى فريق Edulytics.',
    formDemoTitle: 'اطلب عرضًا توضيحيًا',
    formDemoLead: 'املأ البيانات التالية وسنجهّز طلبك في رسالة بريد جاهزة للإرسال.',
    firstName: 'الاسم',
    lastName: 'اسم العائلة',
    email: 'البريد الإلكتروني للعمل',
    organisation: 'المدرسة أو المؤسسة',
    role: 'الدور',
    rolePlaceholder: 'اختر الدور',
    roleTeacher: 'معلم',
    roleLeader: 'قائد تعليمي / إدارة مدرسة',
    roleSupervisor: 'مشرف مادة',
    roleParent: 'ولي أمر',
    roleOther: 'أخرى',
    country: 'الدولة',
    students: 'عدد الطلاب التقريبي',
    message: 'ما الذي تريد مناقشته؟',
    messageDemo: 'ما الذي تريد رؤيته في العرض؟',
    submitSales: 'جهّز استفسار المبيعات',
    submitDemo: 'جهّز طلب العرض',
    mailNote: 'عند المتابعة سيتم فتح تطبيق البريد لديك برسالة مُعبأة مسبقًا. لن يتم إرسال أي شيء دون موافقتك.',

    helpEyebrow: 'مركز مساعدة Edulytics',
    helpPageTitle: 'ما الذي تحتاج إلى مساعدة فيه؟',
    helpLead: 'ابدأ بالبحث أو اختر قسمًا. ركّزنا المركز على المهام الأساسية داخل Edulytics بدل إنشاء مكتبة مقالات وهمية.',
    helpSearch: 'ابحث في موضوعات المساعدة…',
    helpGettingTitle: 'البدء واستخدام الحساب',
    helpGettingBody: 'تسجيل الدخول، إعداد كلمة المرور، الوصول إلى الحساب، وفهم نقطة البداية الصحيحة.',
    helpAccountsTitle: 'الحسابات والأدوار',
    helpAccountsBody: 'المدير والمشرف والمعلم والطالب: من يرى ماذا، ومن المسؤول عن كل إجراء.',
    helpCurriculumTitle: 'المناهج والدروس',
    helpCurriculumBody: 'اختيار المنهج والمستوى، نتائج التعلّم، وبنية الدروس الرسمية داخل المنصة.',
    helpAssessmentTitle: 'التقييمات وضوابط المعلم',
    helpAssessmentBody: 'إنشاء التقييمات ومراجعة الأسئلة واعتمادها ونشر التقييم الرسمي.',
    helpPracticeTitle: 'تدريب الطالب والإتقان',
    helpPracticeBody: 'التدريب الشخصي، نقاط الضعف، الإتقان، والخطوة التالية بعد النتيجة.',
    helpTechnicalTitle: 'الدعم الفني',
    helpTechnicalBody: 'المتصفح، الوصول، المشكلات التقنية، وكيفية التواصل معنا إذا لم تجد الحل.',
    contactSupport: 'تواصل مع الدعم',
    noHelpResults: 'لم نجد قسمًا مطابقًا. جرّب كلمات أخرى أو تواصل معنا مباشرة.'
  };

  if (language === 'ar') {
    document.documentElement.dir = 'rtl';
    root.querySelectorAll('[data-contact-key]').forEach(node => {
      const value = ar[node.dataset.contactKey];
      if (value) node.textContent = value;
    });
    root.querySelectorAll('[data-contact-placeholder]').forEach(node => {
      const value = ar[node.dataset.contactPlaceholder];
      if (value) node.setAttribute('placeholder', value);
    });
  }

  root.querySelectorAll('[data-contact-mail-form]').forEach(form => {
    form.addEventListener('submit', event => {
      event.preventDefault();
      if (!form.reportValidity()) return;

      const recipient = form.dataset.recipient || '';
      const subject = language === 'ar'
        ? (form.dataset.subjectAr || form.dataset.subject || 'Edulytics')
        : language === 'pl'
          ? (form.dataset.subjectPl || form.dataset.subject || 'Edulytics')
          : (form.dataset.subject || 'Edulytics');

      const data = new FormData(form);
      const lines = [];
      for (const [key, value] of data.entries()) {
        const field = form.elements.namedItem(key);
        if (!field || !String(value).trim()) continue;
        let label = key;
        const id = field.id;
        if (id) {
          const labelNode = form.querySelector(`label[for="${CSS.escape(id)}"]`);
          if (labelNode) label = labelNode.textContent.trim();
        }
        lines.push(`${label}: ${String(value).trim()}`);
      }

      const body = lines.join('\n');
      window.location.href = `mailto:${encodeURIComponent(recipient)}?subject=${encodeURIComponent(subject)}&body=${encodeURIComponent(body)}`;
    });
  });

  const search = root.querySelector('[data-help-search]');
  if (search) {
    const categories = [...root.querySelectorAll('.ed-help-category')];
    const empty = root.querySelector('.ed-help-empty');
    const filter = () => {
      const query = search.value.trim().toLocaleLowerCase();
      let visible = 0;
      categories.forEach(card => {
        const match = !query || card.textContent.toLocaleLowerCase().includes(query);
        card.hidden = !match;
        if (match) visible += 1;
      });
      empty?.classList.toggle('is-visible', visible === 0);
    };
    search.addEventListener('input', filter);
  }
})();
