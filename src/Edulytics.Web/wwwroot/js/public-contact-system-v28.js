(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  const storageKey = 'edulytics.public.siteLanguage';
  const frontdoorLanguageKey = 'edulytics.frontdoor.language';
  const documentLanguage = (document.documentElement.lang || 'en').toLowerCase();
  let language = documentLanguage.startsWith('ar') ? 'ar' : documentLanguage.startsWith('pl') ? 'pl' : 'en';
  try {
    const selected = window.localStorage.getItem(frontdoorLanguageKey)
      || window.localStorage.getItem(storageKey);
    if (selected === 'en' || selected === 'pl' || selected === 'ar') language = selected;
  } catch { }

  const en = {
    contactEyebrow: 'CONTACT EDULYTICS',
    contactTitle: 'How can we help?',
    contactLead: 'Whether you are exploring Edulytics for a school or classroom, or you need support, choose the route that best matches what you need.',
    audienceTeachers: 'Teachers',
    audienceLeaders: 'Schools & education leaders',
    audienceParents: 'Parents & students',
    visualTitle: 'Choose a starting point',
    visualSales: 'Questions about the product, plans and school use',
    visualDemo: 'See the platform in your education context',
    visualHelp: 'Help with access, use and technical support',
    waysTitle: 'Choose what you need',
    waysLead: 'We separated contact into clear routes so you can reach the right next step without sending the same request more than once.',
    salesTitle: 'Sales enquiry',
    salesBody: 'Ask about Edulytics, your school\'s requirements, or how to start using the platform.',
    salesAction: 'Send an enquiry',
    demoTitle: 'Request a demo',
    demoBody: 'Request a walkthrough of how Edulytics connects curriculum, assessment, practice and measurable progress.',
    demoAction: 'Request the demo',
    helpTitle: 'Help center',
    helpBody: 'Find guidance for accounts, access, curricula, assessment, student practice and technical support.',
    helpAction: 'Explore help',
    directTitle: 'Prefer to contact us directly?',
    directBody: 'Use the official Edulytics contact routes below.',
    emailAction: 'Send a message',
    callAction: 'Call us',
    salesEyebrow: 'SALES ENQUIRY',
    salesPageTitle: 'Talk to us about Edulytics.',
    salesPageLead: 'Tell us about your school or requirement and we will help you determine whether Edulytics fits and how to get started.',
    demoEyebrow: 'EDULYTICS DEMO',
    demoPageTitle: 'See Edulytics in your school\'s context.',
    demoPageLead: 'Share a little about your role and school so the walkthrough can focus on what you want to evaluate.',
    supportEyebrow: 'SUPPORT REQUEST',
    supportPageTitle: 'How can we help you?',
    supportPageLead: 'Describe the access, account or technical issue and send it directly to the Edulytics team.',
    generalEyebrow: 'CONTACT EDULYTICS',
    generalPageTitle: 'Send us a message.',
    generalPageLead: 'Use this form for a general question that does not fit sales, demo or technical support.',
    backContact: 'Back to contact',
    trustCurriculum: 'Built around curriculum and learning outcomes',
    trustTeacher: 'Teachers remain in control of formal assessment',
    trustPractice: 'Practice stays separate from official school grades',
    trustNext: 'Results lead to a clear next learning step',
    formSalesTitle: 'Tell us what you need',
    formSalesLead: 'Complete the details below and send your enquiry directly from this website.',
    formDemoTitle: 'Request your demo',
    formDemoLead: 'Complete the details below and send your request directly to the Edulytics team.',
    formSupportTitle: 'Tell us what you need help with',
    formSupportLead: 'Complete the form and your support request will be sent securely from this website.',
    formGeneralTitle: 'Send your message',
    formGeneralLead: 'Complete the form and send your message directly from this website.',
    firstName: 'First name',
    lastName: 'Last name',
    email: 'Work email',
    organisation: 'School or organisation',
    role: 'Role',
    rolePlaceholder: 'Choose your role',
    roleTeacher: 'Teacher',
    roleLeader: 'Education leader / school administration',
    roleSupervisor: 'Subject supervisor',
    roleParent: 'Parent',
    roleOther: 'Other',
    country: 'Country',
    students: 'Approximate number of students',
    message: 'What would you like to discuss?',
    messageDemo: 'What would you like to see in the demo?',
    messageSupport: 'What do you need help with?',
    messageGeneral: 'How can we help?',
    turnstileNote: 'This form is protected by Cloudflare Turnstile. The verification is checked again on the server before any email is sent.',
    submitSales: 'Send sales enquiry',
    submitDemo: 'Send demo request',
    submitSupport: 'Send support request',
    submitGeneral: 'Send message',
    mailNote: 'Your message is sent securely from this website and no email application will be opened.',
    helpEyebrow: 'EDULYTICS HELP CENTER',
    helpPageTitle: 'What do you need help with?',
    helpLead: 'Start with search or choose a category. The center focuses on real Edulytics workflows and support topics.',
    helpSearch: 'Search help topics…',
    helpGettingTitle: 'Getting started & sign-in',
    helpGettingBody: 'Signing in, setting a password, accessing your account and understanding the right starting point.',
    helpAccountsTitle: 'Accounts & roles',
    helpAccountsBody: 'Administrator, supervisor, teacher and student: who sees what and who owns each action.',
    helpCurriculumTitle: 'Curricula & lessons',
    helpCurriculumBody: 'Curriculum and level selection, learning outcomes and the lesson structure used by the platform.',
    helpAssessmentTitle: 'Assessments & teacher controls',
    helpAssessmentBody: 'Creating assessments, reviewing questions, approval and publication of formal assessment.',
    helpPracticeTitle: 'Student practice & mastery',
    helpPracticeBody: 'Personal practice, weak areas, mastery and the next step after a result.',
    helpTechnicalTitle: 'Technical support',
    helpTechnicalBody: 'Browser, access and technical issues, plus the direct support route when you cannot find the answer.',
    contactSupport: 'Contact support',
    noHelpResults: 'No matching category found. Try different words or contact us directly.'
  };

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
    directBody: 'استخدم طرق التواصل الرسمية مع Edulytics أدناه.',
    emailAction: 'أرسل رسالة',
    callAction: 'اتصل بنا',

    salesEyebrow: 'استفسار المبيعات',
    salesPageTitle: 'تحدث معنا عن Edulytics.',
    salesPageLead: 'أخبرنا عن مدرستك أو احتياجك، وسنساعدك على تحديد ما إذا كان Edulytics مناسبًا وكيف يمكن البدء.',
    demoEyebrow: 'عرض Edulytics',
    demoPageTitle: 'شاهد Edulytics في سياق مدرستك.',
    demoPageLead: 'شاركنا معلومات أساسية عن دورك ومدرستك حتى نتمكن من جعل العرض أكثر ارتباطًا بما تريد تقييمه.',
    supportEyebrow: 'طلب دعم',
    supportPageTitle: 'كيف يمكننا مساعدتك؟',
    supportPageLead: 'صف مشكلة الوصول أو الحساب أو المشكلة التقنية، وأرسلها مباشرة إلى فريق Edulytics.',
    generalEyebrow: 'تواصل مع Edulytics',
    generalPageTitle: 'أرسل لنا رسالة.',
    generalPageLead: 'استخدم هذا النموذج للأسئلة العامة التي لا تندرج تحت المبيعات أو العرض التوضيحي أو الدعم الفني.',
    backContact: 'العودة إلى التواصل',
    trustCurriculum: 'مبني حول المنهج ونتائج التعلّم',
    trustTeacher: 'المعلم يظل صاحب القرار في التقييمات الرسمية',
    trustPractice: 'التدريب منفصل عن الدرجات المدرسية الرسمية',
    trustNext: 'النتائج تقود إلى خطوة تعليمية تالية واضحة',
    formSalesTitle: 'أخبرنا بما تحتاج إليه',
    formSalesLead: 'املأ البيانات التالية وأرسل استفسارك مباشرة من الموقع.',
    formDemoTitle: 'اطلب عرضًا توضيحيًا',
    formDemoLead: 'املأ البيانات التالية وأرسل طلب العرض مباشرة إلى فريق Edulytics.',
    formSupportTitle: 'أخبرنا بما تحتاج إلى مساعدة فيه',
    formSupportLead: 'املأ النموذج وسيتم إرسال طلب الدعم بأمان من داخل الموقع.',
    formGeneralTitle: 'أرسل رسالتك',
    formGeneralLead: 'املأ النموذج وأرسل رسالتك مباشرة من داخل الموقع.',
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
    messageSupport: 'ما الذي تحتاج إلى مساعدة فيه؟',
    messageGeneral: 'كيف يمكننا مساعدتك؟',
    turnstileNote: 'هذا النموذج محمي بواسطة Cloudflare Turnstile. يتم التحقق مرة أخرى على الخادم قبل إرسال أي رسالة بريد.',
    submitSales: 'إرسال استفسار المبيعات',
    submitDemo: 'إرسال طلب العرض',
    submitSupport: 'إرسال طلب الدعم',
    submitGeneral: 'إرسال الرسالة',
    mailNote: 'يتم إرسال رسالتك بأمان من داخل الموقع ولن يتم فتح أي تطبيق بريد.',

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

  const formMessages = {
    en: {
      sending: 'Sending your message…',
      sent: 'Your message has been sent successfully. We will get back to you soon.',
      validation: 'Please check the form fields and complete the security verification.',
      security: 'Please complete the Cloudflare security verification and try again.',
      rateLimited: 'Too many requests were sent from this connection. Please wait and try again later.',
      failed: 'We could not send your message right now. Please try again later.'
    },
    pl: {
      sending: 'Wysyłanie wiadomości…',
      sent: 'Wiadomość została wysłana. Skontaktujemy się z Tobą wkrótce.',
      validation: 'Sprawdź pola formularza i ukończ weryfikację bezpieczeństwa.',
      security: 'Ukończ weryfikację Cloudflare i spróbuj ponownie.',
      rateLimited: 'Z tego połączenia wysłano zbyt wiele żądań. Odczekaj chwilę i spróbuj ponownie później.',
      failed: 'Nie udało się teraz wysłać wiadomości. Spróbuj ponownie później.'
    },
    ar: {
      sending: 'جارٍ إرسال رسالتك…',
      sent: 'تم إرسال رسالتك بنجاح. سنتواصل معك في أقرب وقت.',
      validation: 'راجع بيانات النموذج وأكمل التحقق الأمني.',
      security: 'أكمل تحقق Cloudflare الأمني ثم حاول مرة أخرى.',
      rateLimited: 'تم إرسال عدد كبير من الطلبات من هذا الاتصال. انتظر قليلًا ثم حاول لاحقًا.',
      failed: 'تعذّر إرسال رسالتك الآن. حاول مرة أخرى لاحقًا.'
    }
  };

  const localizedCopy = language === 'ar' ? ar : language === 'en' ? en : null;

  const pageTitles = {
    en: {
      '/contact': 'Contact | Edulytics',
      '/contact/sales-enquiry': 'Sales enquiry | Edulytics',
      '/contact/request-demo': 'Request a demo | Edulytics',
      '/contact/support': 'Contact support | Edulytics',
      '/contact/message': 'Contact Edulytics | Edulytics',
      '/contact/help': 'Help center | Edulytics',
      '/help': 'Help center | Edulytics'
    },
    pl: {
      '/contact': 'Kontakt | Edulityks',
      '/contact/sales-enquiry': 'Zapytanie sprzedażowe | Edulityks',
      '/contact/request-demo': 'Poproś o demo | Edulityks',
      '/contact/support': 'Skontaktuj się z pomocą | Edulityks',
      '/contact/message': 'Kontakt z Edulytics | Edulityks',
      '/contact/help': 'Centrum pomocy | Edulityks',
      '/help': 'Centrum pomocy | Edulityks'
    },
    ar: {
      '/contact': 'تواصل مع Edulytics | Edulytics',
      '/contact/sales-enquiry': 'استفسار المبيعات | Edulytics',
      '/contact/request-demo': 'اطلب عرضًا تجريبيًا | Edulytics',
      '/contact/support': 'تواصل مع الدعم | Edulytics',
      '/contact/message': 'تواصل مع Edulytics | Edulytics',
      '/contact/help': 'مركز المساعدة | Edulytics',
      '/help': 'مركز المساعدة | Edulytics'
    }
  };
  const normalizedPath = window.location.pathname.replace(/\/+$/, '') || '/';
  const localizedTitle = pageTitles[language]?.[normalizedPath];
  if (localizedTitle) document.title = localizedTitle;
  document.documentElement.lang = language;
  document.documentElement.dir = language === 'ar' ? 'rtl' : 'ltr';
  document.documentElement.classList.toggle('ed-site-ar', language === 'ar');
  root.classList.toggle('is-site-ar', language === 'ar');

  if (localizedCopy) {
    root.querySelectorAll('[data-contact-key]').forEach(node => {
      const value = localizedCopy[node.dataset.contactKey];
      if (value) node.textContent = value;
    });
    root.querySelectorAll('[data-contact-placeholder]').forEach(node => {
      const value = localizedCopy[node.dataset.contactPlaceholder];
      if (value) node.setAttribute('placeholder', value);
    });
  }

  root.querySelectorAll('[data-contact-server-form]').forEach(form => {
    const button = form.querySelector('button[type="submit"]');
    const status = form.querySelector('[data-contact-form-status]');
    if (!button || !status) return;

    const idleButtonText = button.textContent.trim();
    const messages = formMessages[language] || formMessages.en;

    const setStatus = (message, state) => {
      status.textContent = message || '';
      status.dataset.state = state || '';
      status.style.fontWeight = message ? '700' : '';
      status.style.color = state === 'success'
        ? '#176b55'
        : state === 'error'
          ? '#a63737'
          : '#526b86';
    };

    const resetTurnstile = () => {
      try {
        if (window.turnstile) window.turnstile.reset();
      } catch { }
    };

    form.addEventListener('submit', async event => {
      event.preventDefault();

      if (!form.reportValidity()) {
        setStatus(messages.validation, 'error');
        return;
      }

      const formData = new FormData(form);
      const turnstileToken = String(formData.get('cf-turnstile-response') || '').trim();
      if (!turnstileToken) {
        setStatus(messages.security, 'error');
        return;
      }

      button.disabled = true;
      button.setAttribute('aria-busy', 'true');
      button.textContent = messages.sending;
      setStatus(messages.sending, 'busy');

      try {
        const response = await fetch(form.action, {
          method: 'POST',
          body: formData,
          credentials: 'same-origin',
          headers: {
            Accept: 'application/json',
            'X-Requested-With': 'XMLHttpRequest'
          }
        });

        let payload = null;
        try {
          payload = await response.json();
        } catch { }

        if (response.status === 429) {
          resetTurnstile();
          setStatus(messages.rateLimited, 'error');
          return;
        }

        if (response.status === 400) {
          resetTurnstile();
          const securityFailure = payload?.code === 'turnstile_required'
            || payload?.code === 'turnstile_failed';
          setStatus(securityFailure ? messages.security : messages.validation, 'error');
          return;
        }

        if (!response.ok || payload?.success === false) {
          resetTurnstile();
          setStatus(messages.failed, 'error');
          return;
        }

        form.reset();
        resetTurnstile();
        setStatus(messages.sent, 'success');
      } catch {
        resetTurnstile();
        setStatus(messages.failed, 'error');
      } finally {
        button.disabled = false;
        button.removeAttribute('aria-busy');
        button.textContent = idleButtonText;
      }
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
