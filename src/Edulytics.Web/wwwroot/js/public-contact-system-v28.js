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

  if (language === 'ar') {
    document.documentElement.lang = 'ar';
    document.documentElement.dir = 'rtl';
    document.documentElement.classList.add('ed-site-ar');
    root.classList.add('is-site-ar');
    root.querySelectorAll('[data-contact-key]').forEach(node => {
      const value = ar[node.dataset.contactKey];
      if (value) node.textContent = value;
    });
    root.querySelectorAll('[data-contact-placeholder]').forEach(node => {
      const value = ar[node.dataset.contactPlaceholder];
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
