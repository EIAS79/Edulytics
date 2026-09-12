(() => {
  const root = document.querySelector('.ed-home');
  const section = root?.querySelector('.ed-home-philosophy');
  if (!root || !section) return;

  const selectedLanguage = (root.dataset.siteLanguage || document.documentElement.lang || 'en').toLowerCase();
  const language = selectedLanguage.startsWith('ar') ? 'ar' : selectedLanguage.startsWith('pl') ? 'pl' : 'en';

  const copy = {
    en: {
      title: 'The Edulytics Learning Philosophy',
      intro: 'Five principles that shape how Edulytics connects curriculum, teaching, practice, assessment and measurable progress.',
      hub: 'Learning Philosophy',
      cards: [
        ['Curriculum at the Core', 'Every lesson, assessment and learning insight starts from the curriculum and its learning outcomes.'],
        ['Understanding Before Scores', 'Learning is not just about getting the right answer. Edulytics puts understanding at the center of how progress is measured.'],
        ['Teachers Stay in Control', 'Technology supports the teacher’s judgment. Formal assessments are reviewed, approved and published by the teacher.'],
        ['Practice Is for Learning', 'Students can practise, make mistakes and strengthen weak areas without confusing practice with official school grades.'],
        ['Every Result Leads to a Next Step', 'Assessment evidence identifies mastery, weakness and progress, then guides the student toward the most relevant next action.']
      ]
    },
    pl: {
      title: 'Filozofia uczenia się Edulytics',
      intro: 'Pięć zasad, które określają, jak Edulytics łączy program nauczania, pracę nauczyciela, ćwiczenia, ocenianie i mierzalne postępy.',
      hub: 'Filozofia uczenia się',
      cards: [
        ['Program nauczania w centrum', 'Każda lekcja, ocena i informacja o postępach w nauce wynika z programu nauczania i powiązanych z nim efektów uczenia się.'],
        ['Zrozumienie przed oceną', 'Uczenie się to coś więcej niż udzielenie poprawnej odpowiedzi. Edulytics stawia zrozumienie w centrum sposobu mierzenia postępów.'],
        ['Nauczyciel zachowuje kontrolę', 'Technologia wspiera decyzje nauczyciela. Formalne sprawdziany są przez niego przeglądane, zatwierdzane i publikowane.'],
        ['Ćwiczenie służy nauce', 'Uczniowie mogą ćwiczyć, popełniać błędy i wzmacniać słabsze obszary bez mylenia ćwiczeń z oficjalnymi ocenami szkolnymi.'],
        ['Każdy wynik prowadzi do kolejnego kroku', 'Dane z oceniania pokazują poziom opanowania materiału, słabsze obszary i postępy, a następnie kierują ucznia do najbardziej odpowiedniego kolejnego działania.']
      ]
    },
    ar: {
      title: 'فلسفة التعلّم في Edulytics',
      intro: 'خمسة مبادئ تحدد كيف يربط Edulytics بين المنهج، والتدريس، والتدريب، والتقييم، والتقدم القابل للقياس.',
      hub: 'فلسفة التعلّم',
      cards: [
        ['المنهج في صميم التعلّم', 'كل درس وتقييم ومؤشر للتقدم يبدأ من المنهج ونواتج التعلّم المرتبطة به.'],
        ['الفهم قبل الدرجات', 'التعلّم لا يقتصر على الوصول إلى إجابة صحيحة. يضع Edulytics الفهم في صميم قياس التقدم.'],
        ['المعلّم يبقى صاحب القرار', 'تدعم التقنية قرارات المعلّم ولا تستبدلها. يراجع المعلّم التقييمات الرسمية ويعتمدها وينشرها.'],
        ['التدريب من أجل التعلّم', 'يمكن للطلاب التدريب وارتكاب الأخطاء وتقوية جوانب الضعف دون الخلط بين التدريب والدرجات المدرسية الرسمية.'],
        ['كل نتيجة تقود إلى الخطوة التالية', 'تُظهر بيانات التقييم مستوى الإتقان ونقاط الضعف والتقدم، ثم توجه الطالب إلى الإجراء التالي الأكثر ملاءمة.']
      ]
    }
  }[language];

  const heading = section.querySelector('.ed-philosophy-head h2');
  const intro = section.querySelector('.ed-philosophy-head p');
  const hub = section.querySelector('.ed-philosophy-hub strong');
  const cards = [...section.querySelectorAll('.ed-philosophy-card')];

  if (heading) heading.textContent = copy.title;
  if (intro) intro.textContent = copy.intro;
  if (hub) hub.textContent = copy.hub;

  cards.forEach((card, index) => {
    const cardCopy = copy.cards[index];
    if (!cardCopy) return;
    const title = card.querySelector('h3');
    const body = card.querySelector('p');
    if (title) title.textContent = cardCopy[0];
    if (body) body.textContent = cardCopy[1];
  });

  if (language === 'ar') {
    section.classList.add('is-ar');
    section.setAttribute('dir', 'rtl');
    section.setAttribute('lang', 'ar');
  } else {
    section.classList.remove('is-ar');
    section.removeAttribute('dir');
    section.removeAttribute('lang');
  }
})();
