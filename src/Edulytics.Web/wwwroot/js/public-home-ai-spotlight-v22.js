(() => {
  const root = document.querySelector('.ed-home');
  const section = root?.querySelector('#ai');
  if (!root || !section) return;

  const selectedLanguage = (root.dataset.siteLanguage || document.documentElement.lang || 'en').toLowerCase();
  const language = selectedLanguage.startsWith('ar') ? 'ar' : selectedLanguage.startsWith('pl') ? 'pl' : 'en';

  const copy = {
    en: {
      kicker: 'FEATURE SPOTLIGHT',
      title: 'Create curriculum-aligned assessments in minutes.',
      intro: 'Select a lesson, unit, or curriculum scope, and Edulytics uses that learning context to generate relevant questions. Review, edit, regenerate, and approve before publishing.',
      imageAlt: 'Edulytics AI assessment mascot',
      features: [
        ['Curriculum-aware generation', 'Questions are generated in the context of the selected curriculum, unit, or lesson.'],
        ['Built-in learning context', 'Edulytics uses the selected scope, level, and learning outcomes to keep generation relevant.'],
        ['Teacher review and approval', 'Review, edit, regenerate, and approve questions before they reach students.'],
        ['Generate questions at scale', 'Create multiple question sets while keeping the teacher fully in control.']
      ]
    },
    pl: {
      kicker: 'WYRÓŻNIONA FUNKCJA',
      title: 'Twórz oceny zgodne z programem nauczania w kilka minut.',
      intro: 'Wybierz lekcję, dział lub zakres programu, a Edulytics wykorzysta ten kontekst edukacyjny do wygenerowania odpowiednich pytań. Przejrzyj je, edytuj, wygeneruj ponownie i zatwierdź przed publikacją.',
      imageAlt: 'Maskotka Edulytics przedstawiająca generowanie ocen z AI',
      features: [
        ['Generowanie zgodne z programem', 'Pytania są tworzone w kontekście wybranego programu, działu lub lekcji.'],
        ['Wbudowany kontekst nauczania', 'Edulytics wykorzystuje wybrany zakres, poziom i efekty uczenia się, aby generowane pytania były trafne.'],
        ['Kontrola i zatwierdzanie przez nauczyciela', 'Przejrzyj, edytuj, wygeneruj ponownie i zatwierdź pytania przed udostępnieniem ich uczniom.'],
        ['Generuj pytania na dużą skalę', 'Twórz wiele zestawów pytań, zachowując pełną kontrolę nauczyciela.']
      ]
    },
    ar: {
      kicker: 'ميزة مميزة',
      title: 'أنشئ تقييمات متوافقة مع المنهج في دقائق.',
      intro: 'اختر درسًا أو وحدة أو نطاقًا من المنهج، ويستخدم Edulytics هذا السياق التعليمي لإنشاء أسئلة مناسبة. راجعها وعدّلها وأعد توليدها واعتمدها قبل النشر.',
      imageAlt: 'شخصية Edulytics لتوضيح إنشاء التقييمات بالذكاء الاصطناعي',
      features: [
        ['إنشاء أسئلة متوافق مع المنهج', 'يتم إنشاء الأسئلة في سياق المنهج أو الوحدة أو الدرس المحدد.'],
        ['سياق تعليمي مدمج', 'يستخدم Edulytics النطاق والمستوى ونواتج التعلم المحددة للحفاظ على دقة وملاءمة الأسئلة.'],
        ['مراجعة واعتماد من المعلم', 'راجع الأسئلة وعدّلها وأعد توليدها واعتمدها قبل تقديمها للطلاب.'],
        ['أنشئ عددًا كبيرًا من الأسئلة بسهولة', 'ولّد مجموعات متعددة من الأسئلة مع الحفاظ على تحكم المعلم الكامل.']
      ]
    }
  }[language];

  section.className = `ed-home-section ed-home-ai-spotlight-v22${language === 'ar' ? ' is-ar' : ''}`;
  if (language === 'ar') section.setAttribute('dir', 'rtl');
  else section.removeAttribute('dir');

  section.innerHTML = `
    <div class="ed-home-container ed-ai-v22-grid">
      <div class="ed-ai-v22-copy">
        <span class="ed-ai-v22-kicker">${copy.kicker}</span>
        <h2>${copy.title}</h2>
        <p class="ed-ai-v22-intro">${copy.intro}</p>
        <div class="ed-ai-v22-features">
          ${copy.features.map((feature, index) => `
            <div class="ed-ai-v22-feature">
              <span class="ed-ai-v22-marker" aria-hidden="true">${String(index + 1).padStart(2, '0')}</span>
              <div>
                <h3>${feature[0]}</h3>
                <p>${feature[1]}</p>
              </div>
            </div>
          `).join('')}
        </div>
      </div>
      <div class="ed-ai-v22-visual">
        <img
          src="/images/public/edulytics-math-mascot-2.png"
          alt="${copy.imageAlt}"
          loading="lazy"
          decoding="async" />
      </div>
    </div>`;

  if (!document.getElementById('ed-ai-v22-style')) {
    const style = document.createElement('style');
    style.id = 'ed-ai-v22-style';
    style.textContent = `
      .ed-home .ed-home-ai-spotlight-v22{
        background:#fff;
        padding:78px 28px 84px;
        overflow:hidden;
      }
      .ed-home .ed-home-ai-spotlight-v22 .ed-ai-v22-grid{
        width:min(1480px,100%);
        margin:0 auto;
        display:grid;
        grid-template-columns:minmax(0,.92fr) minmax(420px,1.08fr);
        gap:72px;
        align-items:center;
      }
      .ed-home .ed-ai-v22-copy{
        max-width:690px;
      }
      .ed-home .ed-ai-v22-kicker{
        display:block;
        margin:0 0 14px;
        color:#286df0;
        font-size:13px;
        line-height:1;
        font-weight:900;
        letter-spacing:.14em;
        text-transform:uppercase;
      }
      .ed-home .ed-ai-v22-copy h2{
        margin:0;
        max-width:760px;
        color:#10243f;
        font-size:clamp(38px,3.25vw,56px);
        line-height:1.08;
        font-weight:900;
        letter-spacing:-.04em;
      }
      .ed-home .ed-ai-v22-intro{
        margin:20px 0 0;
        max-width:670px;
        color:#5d718b;
        font-size:18px;
        line-height:1.66;
        font-weight:500;
      }
      .ed-home .ed-ai-v22-features{
        margin-top:30px;
        border-top:1px solid #dfe6ef;
      }
      .ed-home .ed-ai-v22-feature{
        display:grid;
        grid-template-columns:36px minmax(0,1fr);
        gap:16px;
        align-items:start;
        padding:17px 0;
        border-bottom:1px solid #dfe6ef;
      }
      .ed-home .ed-ai-v22-marker{
        display:inline-flex;
        align-items:center;
        justify-content:center;
        width:30px;
        height:30px;
        border-radius:50%;
        background:#eef5ff;
        color:#286df0;
        font-size:11px;
        line-height:1;
        font-weight:900;
        letter-spacing:.04em;
      }
      .ed-home .ed-ai-v22-feature h3{
        margin:0;
        color:#10243f;
        font-size:17.5px;
        line-height:1.35;
        font-weight:850;
      }
      .ed-home .ed-ai-v22-feature p{
        margin:5px 0 0;
        color:#657a93;
        font-size:14.5px;
        line-height:1.56;
        font-weight:500;
      }
      .ed-home .ed-ai-v22-visual{
        min-width:0;
        display:flex;
        align-items:center;
        justify-content:center;
        background:#fff;
      }
      .ed-home .ed-ai-v22-visual img{
        display:block;
        width:min(100%,760px);
        height:auto;
        max-height:650px;
        object-fit:contain;
        object-position:center;
        background:#fff;
      }
      .ed-home .ed-home-ai-spotlight-v22.is-ar .ed-ai-v22-copy{
        text-align:right;
      }
      .ed-home .ed-home-ai-spotlight-v22.is-ar .ed-ai-v22-kicker{
        letter-spacing:0;
        text-transform:none;
      }
      .ed-home .ed-home-ai-spotlight-v22.is-ar .ed-ai-v22-feature{
        grid-template-columns:36px minmax(0,1fr);
      }
      @media(max-width:1120px){
        .ed-home .ed-home-ai-spotlight-v22{
          padding:68px 24px 72px;
        }
        .ed-home .ed-home-ai-spotlight-v22 .ed-ai-v22-grid{
          grid-template-columns:minmax(0,1fr) minmax(360px,.92fr);
          gap:46px;
        }
        .ed-home .ed-ai-v22-copy h2{
          font-size:clamp(36px,4vw,48px);
        }
      }
      @media(max-width:860px){
        .ed-home .ed-home-ai-spotlight-v22 .ed-ai-v22-grid{
          grid-template-columns:1fr;
          gap:36px;
        }
        .ed-home .ed-ai-v22-copy{
          max-width:760px;
        }
        .ed-home .ed-ai-v22-visual{
          order:2;
        }
        .ed-home .ed-ai-v22-visual img{
          width:min(100%,680px);
          max-height:none;
        }
      }
      @media(max-width:620px){
        .ed-home .ed-home-ai-spotlight-v22{
          padding:54px 18px 58px;
        }
        .ed-home .ed-ai-v22-kicker{
          font-size:12px;
        }
        .ed-home .ed-ai-v22-copy h2{
          font-size:34px;
          line-height:1.12;
        }
        .ed-home .ed-ai-v22-intro{
          margin-top:16px;
          font-size:16px;
          line-height:1.62;
        }
        .ed-home .ed-ai-v22-features{
          margin-top:24px;
        }
        .ed-home .ed-ai-v22-feature{
          grid-template-columns:32px minmax(0,1fr);
          gap:12px;
          padding:15px 0;
        }
        .ed-home .ed-ai-v22-marker{
          width:28px;
          height:28px;
          font-size:10px;
        }
        .ed-home .ed-ai-v22-feature h3{
          font-size:16.5px;
        }
        .ed-home .ed-ai-v22-feature p{
          font-size:14px;
        }
      }
    `;
    document.head.appendChild(style);
  }
})();
