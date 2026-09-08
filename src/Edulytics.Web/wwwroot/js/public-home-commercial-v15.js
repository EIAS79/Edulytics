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
  const ar = (document.documentElement.lang || '').toLowerCase().startsWith('ar');
  if (!ar) return;

  const cardCopy = [
    ['المدارس', 'المناهج، والفصول، والنتائج، والإشراف الأكاديمي في بيئة واحدة.'],
    ['المعلمون', 'أنشئ التقييمات، واستخدم الذكاء الاصطناعي، وتعرّف على ما يحتاج الطلاب إلى العمل عليه بعد ذلك.'],
    ['أولياء الأمور', 'تعرّف على كيفية مساعدة Edulytics للطلاب على التدرّب، وفهم النتائج، وبناء الثقة.']
  ];

  root.querySelectorAll('.ed-home-v6-audience-card').forEach((card, index) => {
    const copy = cardCopy[index];
    if (!copy) return;
    const title = card.querySelector('h3');
    const body = card.querySelector('p');
    const link = card.querySelector('a');
    const photo = card.querySelector('.ed-home-v6-audience-photo');
    if (title) title.textContent = copy[0];
    if (body) body.textContent = copy[1];
    if (link) link.textContent = 'تعرّف على المزيد →';
    if (photo) photo.setAttribute('aria-label', copy[0]);
  });

  const signalCopy = [
    ['تعلّم متوافق مع المنهج', 'محتوى وتقييمات مرتبطة بسياق المنهج'],
    ['تقييم مدعوم بالذكاء الاصطناعي', 'توليد أسئلة تحت إشراف المعلم'],
    ['تدريب مخصص للطالب', 'تدريبات خاصة موجهة لاحتياجات الطالب'],
    ['رؤى الإتقان والتقدم', 'النتائج، ونقاط الضعف، وخطوة تالية واضحة'],
    ['تحليلات المدرسة', 'بيانات تعلم للمعلمين والإشراف الأكاديمي']
  ];

  root.querySelectorAll('.ed-home-v6-signal').forEach((signal, index) => {
    const copy = signalCopy[index];
    if (!copy) return;
    const title = signal.querySelector('strong');
    const body = signal.querySelector('span');
    if (title) title.textContent = copy[0];
    if (body) body.textContent = copy[1];
  });

  root.querySelector('.ed-home-v6-signals')?.setAttribute('aria-label', 'أبرز مزايا Edulytics');
})();
