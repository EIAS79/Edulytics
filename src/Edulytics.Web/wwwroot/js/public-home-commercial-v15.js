(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  /* Remove the parent free-trial CTA from the teacher slide in every language.
     The second slide is the teacher slide for EN / PL / AR, so this remains copy-independent. */
  const teacherActions = root.querySelector('.ed-home-v12-slide:nth-child(2) .ed-home-v12-actions');
  if (teacherActions) {
    teacherActions.querySelector('.ed-home-v12-primary')?.remove();
    teacherActions.classList.add('is-v15-single');
  }

  /* Arabic localisation for the audience cards and the five product-strength signals. */
  const ar = (root.dataset.siteLanguage || document.documentElement.lang || '').toLowerCase().startsWith('ar');
  if (!ar) return;

  const cardCopy = [
    ['المدارس', 'إدارة المنهج الدراسي والفصول والنتائج والمتابعة الأكاديمية في بيئة واحدة متكاملة.'],
    ['المعلّمون', 'أنشئ التقييمات، واستفد من الذكاء الاصطناعي، واكتشف ما يحتاج الطلاب إلى التركيز عليه في الخطوة التالية.'],
    ['أولياء الأمور', 'تعرّف على كيف يساعد Edulytics الطلاب على التدريب، وفهم نتائجهم، وبناء الثقة في قدراتهم.']
  ];

  root.querySelectorAll('.ed-home-v6-audience-card').forEach((card, index) => {
    const copy = cardCopy[index];
    if (!copy) return;
    const content = card.querySelector('.ed-home-v6-audience-photo')?.nextElementSibling;
    const title = card.querySelector('h3');
    const body = card.querySelector('p');
    const link = card.querySelector('a');
    const photo = card.querySelector('.ed-home-v6-audience-photo');

    card.style.direction = 'rtl';
    if (content) {
      content.style.direction = 'rtl';
      content.style.textAlign = 'right';
    }
    if (title) {
      title.textContent = copy[0];
      title.style.textAlign = 'right';
    }
    if (body) {
      body.textContent = copy[1];
      body.style.textAlign = 'right';
    }
    if (link) {
      link.textContent = 'اعرف المزيد ←';
      link.style.textAlign = 'right';
    }
    if (photo) photo.setAttribute('aria-label', copy[0]);
  });

  const signalCopy = [
    ['تعلّم متوافق مع المنهج الدراسي', 'محتوى وتقييمات مبنية في سياق المنهج الدراسي.'],
    ['تقييمات مدعومة بالذكاء الاصطناعي', 'إنشاء الأسئلة بالذكاء الاصطناعي مع بقاء المعلم صاحب القرار والتحكم.'],
    ['تدريب شخصي للطلاب', 'تدريب فردي موجّه وفق احتياجات كل طالب.'],
    ['رؤى حول الإتقان والتقدم', 'النتائج، ونقاط الضعف، وخطوة واضحة لما ينبغي العمل عليه بعد ذلك.'],
    ['تحليلات على مستوى المدرسة', 'بيانات ورؤى تعليمية تدعم المعلمين والمتابعة والإشراف الأكاديمي.']
  ];

  root.querySelectorAll('.ed-home-v6-signal').forEach((signal, index) => {
    const copy = signalCopy[index];
    if (!copy) return;
    const title = signal.querySelector('strong');
    const body = signal.querySelector('span');
    signal.style.direction = 'rtl';
    if (title) title.textContent = copy[0];
    if (body) body.textContent = copy[1];
  });

  root.querySelector('.ed-home-v6-signals')?.setAttribute('aria-label', 'أبرز مزايا Edulytics');
})();
