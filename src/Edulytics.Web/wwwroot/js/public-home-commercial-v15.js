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

  const selectedLanguage = (root.dataset.siteLanguage || document.documentElement.lang || 'en').toLowerCase();
  const language = selectedLanguage.startsWith('ar') ? 'ar' : selectedLanguage.startsWith('pl') ? 'pl' : 'en';

  /* Keep the impact cards visually compact and close to the approved reference proportions. */
  if (!document.getElementById('ed-home-v18-compact-style')) {
    const compactStyle = document.createElement('style');
    compactStyle.id = 'ed-home-v18-compact-style';
    compactStyle.textContent = `
      .ed-home .ed-home-v18-impact{padding:48px 30px 58px}
      .ed-home .ed-home-v18-impact-intro,
      .ed-home .ed-home-v18-impact-card{min-height:0}
      .ed-home .ed-home-v18-impact-intro{padding:28px 32px 26px}
      .ed-home .ed-home-v18-impact-intro h2{max-width:none;margin:0 0 10px;font-size:clamp(28px,1.75vw,34px);line-height:1.18}
      .ed-home .ed-home-v18-impact-intro p{max-width:none;font-size:clamp(17px,1vw,20px);line-height:1.48}
      .ed-home .ed-home-v18-intro-icon{width:56px;height:56px;margin-top:18px}
      .ed-home .ed-home-v18-impact-card{padding:28px 32px 26px}
      .ed-home .ed-home-v18-impact-card h3{min-height:58px;font-size:clamp(23px,1.4vw,28px);line-height:1.2}
      .ed-home .ed-home-v18-impact-metric{margin:8px 0 18px;font-size:clamp(50px,3.5vw,66px)}
      .ed-home .ed-home-v18-impact-card p{font-size:clamp(16px,.95vw,19px);line-height:1.48}
      @media(max-width:1280px){
        .ed-home .ed-home-v18-impact{padding:44px 28px 54px}
        .ed-home .ed-home-v18-impact-intro,
        .ed-home .ed-home-v18-impact-card{min-height:0}
      }
      @media(max-width:720px){
        .ed-home .ed-home-v18-impact{padding:36px 18px 44px}
        .ed-home .ed-home-v18-impact-intro,
        .ed-home .ed-home-v18-impact-card{padding:26px 24px}
        .ed-home .ed-home-v18-impact-intro h2{font-size:29px}
        .ed-home .ed-home-v18-impact-intro p{font-size:17px}
        .ed-home .ed-home-v18-intro-icon{width:50px;height:50px;margin-top:16px}
        .ed-home .ed-home-v18-impact-card h3{min-height:0;font-size:24px}
        .ed-home .ed-home-v18-impact-metric{margin:10px 0 16px;font-size:54px}
        .ed-home .ed-home-v18-impact-card p{font-size:16px}
      }
    `;
    document.head.appendChild(compactStyle);
  }

  const impactCopy = {
    en: {
      title: 'Measurable progress. Meaningful impact.',
      body: 'Edulytics turns learning data into actionable insights that help teachers and schools support better student outcomes.',
      cards: [
        ['Understand learning gaps', '47%', 'More clarity on the areas where students need additional support.'],
        ['Support teachers', '93%', 'Teachers can quickly identify what students should work on next.'],
        ['Build student confidence', '72%', 'Students show stronger engagement when practice is tailored to their learning needs.']
      ]
    },
    ar: {
      title: 'تقدّم قابل للقياس. وتأثير ملموس.',
      body: 'يحوّل Edulytics بيانات التعلّم إلى رؤى عملية تساعد المعلّمين والمدارس على دعم نتائج أفضل للطلاب.',
      cards: [
        ['فهم فجوات التعلّم', '47%', 'وضوح أكبر حول المجالات التي يحتاج فيها الطلاب إلى دعم إضافي.'],
        ['دعم المعلّمين', '93%', 'يستطيع المعلّمون تحديد ما يحتاج الطلاب إلى العمل عليه بعد ذلك بسرعة ووضوح.'],
        ['بناء ثقة الطلاب', '72%', 'يُظهر الطلاب تفاعلًا أكبر عندما يكون التدريب مخصصًا لاحتياجاتهم التعليمية.']
      ]
    },
    pl: {
      title: 'Mierzalne postępy. Realny wpływ.',
      body: 'Edulytics zamienia dane dotyczące nauki w praktyczne informacje, które pomagają nauczycielom i szkołom wspierać lepsze wyniki uczniów.',
      cards: [
        ['Lepsze rozumienie luk w nauce', '47%', 'Większa przejrzystość obszarów, w których uczniowie potrzebują dodatkowego wsparcia.'],
        ['Wsparcie dla nauczycieli', '93%', 'Nauczyciele mogą szybko określić, nad czym uczniowie powinni pracować w kolejnym kroku.'],
        ['Budowanie pewności siebie uczniów', '72%', 'Uczniowie wykazują większe zaangażowanie, gdy ćwiczenia są dopasowane do ich potrzeb edukacyjnych.']
      ]
    }
  };

  /* The impact section belongs directly below the complete mathematics teaching environment band. */
  const impactAnchor = root.querySelector('.ed-home-value-band');
  if (impactAnchor && !root.querySelector('.ed-home-v18-impact')) {
    const copy = impactCopy[language];
    const section = document.createElement('section');
    section.className = `ed-home-v18-impact${language === 'ar' ? ' is-ar' : ''}`;
    section.setAttribute('aria-label', copy.title);
    if (language === 'ar') section.setAttribute('dir', 'rtl');

    const introIcon = `
      <svg class="ed-home-v18-intro-icon" viewBox="0 0 72 72" aria-hidden="true">
        <path d="M12 56V40M30 56V28M48 56V18M9 58h51" fill="none" stroke="currentColor" stroke-width="4" stroke-linecap="round"/>
        <path d="M14 34l15-12 15 5 14-14" fill="none" stroke="currentColor" stroke-width="4" stroke-linecap="round" stroke-linejoin="round"/>
        <path d="M50 13h8v8" fill="none" stroke="currentColor" stroke-width="4" stroke-linecap="round" stroke-linejoin="round"/>
      </svg>`;

    section.innerHTML = `
      <div class="ed-home-v18-impact-grid">
        <article class="ed-home-v18-impact-intro">
          <div>
            <h2>${copy.title}</h2>
            <p>${copy.body}</p>
          </div>
          ${introIcon}
        </article>
        ${copy.cards.map(card => `
          <article class="ed-home-v18-impact-card">
            <h3>${card[0]}</h3>
            <div class="ed-home-v18-impact-metric"><span aria-hidden="true">↑</span>${card[1]}</div>
            <p>${card[2]}</p>
          </article>`).join('')}
      </div>`;

    impactAnchor.insertAdjacentElement('afterend', section);
  }

  /* Arabic localisation for the audience cards and the five product-strength signals. */
  const ar = language === 'ar';
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
