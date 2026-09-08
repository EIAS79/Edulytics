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

  /* Recast the complete teaching environment row as compact Edulytics stat cards.
     4,400+ reflects the 4,453 curriculum lesson-content records currently available in staging. */
  const featureCopy = {
    en: [
      ['4', 'Curricula', 'Cambridge • Common Core • Polish • UAE'],
      ['4,400+', 'Ready Lessons', 'Across supported curricula'],
      ['Millions', 'AI Question Variations', 'Generated dynamically'],
      ['50', 'Questions per Assessment', 'AI-assisted generation'],
      ['3', 'Learning Modes', 'Learn • Practice • Assessment'],
      ['3', 'Oversight Roles', 'Admin • Supervisor • Teacher']
    ],
    ar: [
      ['4', 'مناهج تعليمية', 'كامبردج • Common Core • المنهج البولندي • UAE'],
      ['4,400+', 'دروس جاهزة', 'عبر المناهج التعليمية المدعومة'],
      ['ملايين', 'تنويعات أسئلة بالذكاء الاصطناعي', 'يتم توليدها ديناميكيًا'],
      ['50', 'سؤالًا في التقييم الواحد', 'إنشاء بمساعدة الذكاء الاصطناعي'],
      ['3', 'أوضاع للتعلّم', 'تعلّم • تدريب • تقييم'],
      ['3', 'أدوار للإشراف', 'مدير المدرسة • مشرف المادة • المعلّم']
    ],
    pl: [
      ['4', 'Programy nauczania', 'Cambridge • Common Core • Polski • UAE'],
      ['4 400+', 'Gotowe lekcje', 'W obsługiwanych programach'],
      ['Miliony', 'Wariantów pytań AI', 'Generowane dynamicznie'],
      ['50', 'Pytań na ocenę', 'Generowanie wspierane przez AI'],
      ['3', 'Tryby nauki', 'Nauka • Ćwiczenia • Ocena'],
      ['3', 'Role nadzoru', 'Administrator • Supervisor • Nauczyciel']
    ]
  };

  const featureIllustrations = [
    `<svg viewBox="0 0 112 86" aria-hidden="true"><path d="M24 20h52l12 10v38H24z" fill="none" stroke="currentColor" stroke-width="4" stroke-linejoin="round"/><path d="M36 12h50l10 9v39" fill="none" stroke="currentColor" stroke-width="4" stroke-linecap="round" stroke-linejoin="round" opacity=".45"/><path d="M34 34h36M34 45h28M34 56h32" fill="none" stroke="currentColor" stroke-width="4" stroke-linecap="round"/></svg>`,
    `<svg viewBox="0 0 112 86" aria-hidden="true"><rect x="22" y="12" width="68" height="62" rx="8" fill="none" stroke="currentColor" stroke-width="4"/><path d="M34 27h28M34 39h44M34 52h18" fill="none" stroke="currentColor" stroke-width="4" stroke-linecap="round"/><path d="M64 55l7 7 13-15" fill="none" stroke="currentColor" stroke-width="4" stroke-linecap="round" stroke-linejoin="round"/></svg>`,
    `<svg viewBox="0 0 112 86" aria-hidden="true"><path d="M56 10l5 15 15 5-15 5-5 15-5-15-15-5 15-5z" fill="currentColor" opacity=".9"/><path d="M27 56h24M27 67h15M67 54h18M76 45v18" fill="none" stroke="currentColor" stroke-width="4" stroke-linecap="round"/><circle cx="86" cy="67" r="5" fill="currentColor" opacity=".45"/></svg>`,
    `<svg viewBox="0 0 112 86" aria-hidden="true"><rect x="24" y="10" width="64" height="66" rx="8" fill="none" stroke="currentColor" stroke-width="4"/><path d="M35 28l5 5 9-10M35 46l5 5 9-10M57 28h20M57 46h20M35 64h42" fill="none" stroke="currentColor" stroke-width="4" stroke-linecap="round" stroke-linejoin="round"/></svg>`,
    `<svg viewBox="0 0 112 86" aria-hidden="true"><circle cx="29" cy="43" r="14" fill="none" stroke="currentColor" stroke-width="4"/><circle cx="56" cy="43" r="14" fill="none" stroke="currentColor" stroke-width="4" opacity=".7"/><circle cx="83" cy="43" r="14" fill="none" stroke="currentColor" stroke-width="4" opacity=".45"/><path d="M26 43h6M53 43h6M80 43h6" stroke="currentColor" stroke-width="4" stroke-linecap="round"/></svg>`,
    `<svg viewBox="0 0 112 86" aria-hidden="true"><rect x="40" y="10" width="32" height="23" rx="5" fill="none" stroke="currentColor" stroke-width="4"/><circle cx="24" cy="65" r="9" fill="none" stroke="currentColor" stroke-width="4"/><circle cx="56" cy="65" r="9" fill="none" stroke="currentColor" stroke-width="4"/><circle cx="88" cy="65" r="9" fill="none" stroke="currentColor" stroke-width="4"/><path d="M56 33v15M24 48h64M24 48v8M56 48v8M88 48v8" fill="none" stroke="currentColor" stroke-width="4" stroke-linecap="round"/></svg>`
  ];

  const valueBand = root.querySelector('.ed-home-value-band');
  const valueGrid = valueBand?.querySelector('.ed-home-value-icons');
  if (valueGrid && !valueGrid.classList.contains('ed-home-v19-feature-grid')) {
    const cards = featureCopy[language];
    valueGrid.className = `ed-home-v19-feature-grid${language === 'ar' ? ' is-ar' : ''}`;
    if (language === 'ar') valueGrid.setAttribute('dir', 'rtl');
    valueGrid.innerHTML = cards.map((card, index) => `
      <article class="ed-home-v19-feature-card feature-${index + 1}">
        <div class="ed-home-v19-card-top">
          <span class="ed-home-v19-metric${card[0].length > 4 ? ' is-wide' : ''}">${card[0]}</span>
          <span class="ed-home-v19-card-index">0${index + 1}</span>
        </div>
        <div class="ed-home-v19-illustration">${featureIllustrations[index]}</div>
        <div class="ed-home-v19-card-copy">
          <h3>${card[1]}</h3>
          <p>${card[2]}</p>
        </div>
      </article>`).join('');
  }

  if (!document.getElementById('ed-home-v19-feature-style')) {
    const featureStyle = document.createElement('style');
    featureStyle.id = 'ed-home-v19-feature-style';
    featureStyle.textContent = `
      .ed-home .ed-home-value-band{padding-bottom:72px}
      .ed-home .ed-home-v19-feature-grid{
        width:min(1740px,100%);margin:42px auto 0;display:grid;
        grid-template-columns:repeat(6,minmax(0,1fr));gap:18px;align-items:stretch
      }
      .ed-home .ed-home-v19-feature-card{
        --accent:#2468f2;--soft:#edf4ff;position:relative;isolation:isolate;
        min-height:304px;padding:22px 20px 31px;box-sizing:border-box;overflow:hidden;
        background:linear-gradient(155deg,#fff 0%,#fff 52%,var(--soft) 100%);
        clip-path:polygon(13% 0,87% 0,100% 8%,100% 86%,50% 100%,0 86%,0 8%);
        color:#0d2a59;filter:drop-shadow(0 12px 18px rgba(20,45,88,.12));
        transition:transform .22s ease,filter .22s ease
      }
      .ed-home .ed-home-v19-feature-card::before{
        content:"";position:absolute;inset:0 0 auto;height:7px;background:var(--accent);opacity:.95
      }
      .ed-home .ed-home-v19-feature-card::after{
        content:"";position:absolute;width:128px;height:128px;border-radius:50%;right:-55px;top:56px;
        background:var(--accent);opacity:.055;z-index:-1
      }
      .ed-home .ed-home-v19-feature-card:hover{transform:translateY(-5px);filter:drop-shadow(0 17px 24px rgba(20,45,88,.17))}
      .ed-home .ed-home-v19-feature-card.feature-1{--accent:#286df0;--soft:#eef5ff}
      .ed-home .ed-home-v19-feature-card.feature-2{--accent:#00a9c7;--soft:#eafafd}
      .ed-home .ed-home-v19-feature-card.feature-3{--accent:#7651f5;--soft:#f2efff}
      .ed-home .ed-home-v19-feature-card.feature-4{--accent:#1769d8;--soft:#eef5ff}
      .ed-home .ed-home-v19-feature-card.feature-5{--accent:#9a50f8;--soft:#f8efff}
      .ed-home .ed-home-v19-feature-card.feature-6{--accent:#e7a91c;--soft:#fff8e5}
      .ed-home .ed-home-v19-card-top{display:flex;align-items:flex-start;justify-content:space-between;gap:8px;min-height:58px}
      .ed-home .ed-home-v19-metric{font-size:clamp(42px,3vw,58px);line-height:.95;font-weight:900;letter-spacing:-2.5px;color:var(--accent)}
      .ed-home .ed-home-v19-metric.is-wide{font-size:clamp(29px,2vw,38px);letter-spacing:-1.2px;line-height:1.05}
      .ed-home .ed-home-v19-card-index{font-size:12px;line-height:1;font-weight:800;letter-spacing:.13em;color:#91a0b7;padding-top:3px}
      .ed-home .ed-home-v19-illustration{height:105px;display:flex;align-items:center;justify-content:center;color:var(--accent);margin:3px 0 5px}
      .ed-home .ed-home-v19-illustration svg{width:104px;height:80px;display:block;filter:drop-shadow(0 7px 12px rgba(36,104,242,.08))}
      .ed-home .ed-home-v19-card-copy{text-align:center;padding:0 5px}
      .ed-home .ed-home-v19-card-copy h3{margin:0;color:#0d2a59;font-size:18px;line-height:1.2;font-weight:850;min-height:43px;display:flex;align-items:center;justify-content:center}
      .ed-home .ed-home-v19-card-copy p{margin:5px auto 0;color:#68778f;font-size:12.5px;line-height:1.35;font-weight:650;max-width:210px}
      .ed-home .ed-home-v19-feature-grid.is-ar .ed-home-v19-card-copy{direction:rtl}
      .ed-home .ed-home-v19-feature-grid.is-ar .ed-home-v19-card-top{direction:ltr}
      .ed-home .ed-home-v19-feature-grid.is-ar .ed-home-v19-card-copy h3{font-size:19px}
      .ed-home .ed-home-v19-feature-grid.is-ar .ed-home-v19-card-copy p{font-size:13.5px}
      @media(max-width:1500px){
        .ed-home .ed-home-v19-feature-grid{grid-template-columns:repeat(3,minmax(0,1fr));max-width:1020px;gap:24px}
        .ed-home .ed-home-v19-feature-card{min-height:300px;padding-left:28px;padding-right:28px}
      }
      @media(max-width:820px){
        .ed-home .ed-home-value-band{padding-bottom:56px}
        .ed-home .ed-home-v19-feature-grid{grid-template-columns:repeat(2,minmax(0,1fr));gap:16px;margin-top:30px}
        .ed-home .ed-home-v19-feature-card{min-height:282px;padding:20px 18px 29px}
      }
      @media(max-width:560px){
        .ed-home .ed-home-v19-feature-grid{grid-template-columns:1fr;max-width:330px;gap:18px}
        .ed-home .ed-home-v19-feature-card{min-height:286px;padding:22px 24px 31px}
        .ed-home .ed-home-v19-card-copy h3{min-height:0}
      }
    `;
    document.head.appendChild(featureStyle);
  }

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
