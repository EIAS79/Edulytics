(() => {
  const root = document.querySelector('.ed-home');
  const section = root?.querySelector('#experience');
  if (!root || !section) return;

  const selectedLanguage = (root.dataset.siteLanguage || document.documentElement.lang || 'en').toLowerCase();
  const language = selectedLanguage.startsWith('ar') ? 'ar' : selectedLanguage.startsWith('pl') ? 'pl' : 'en';

  const copy = {
    en: {
      tabs: ['Student experience', 'Parent experience', 'Teacher experience'],
      items: [
        {
          kicker: 'STUDENT EXPERIENCE',
          title: 'Understand more. Practice smarter. Progress with confidence.',
          intro: 'Edulytics gives students a complete learning experience that combines understanding, personalized practice and assessment, helping them see what they have mastered and what to work on next.',
          points: [
            ['Learn through understanding', 'Explore mathematical concepts through explanations, examples and activities that encourage thinking and make the solution process clear step by step.'],
            ['Practice what you actually need', 'Edulytics provides personalized practice based on each student’s level and performance, focusing attention on the skills and concepts that need more practice.'],
            ['Know what you’ve mastered and what comes next', 'Edulytics analyzes performance across skills and Learning Outcomes to identify what is secure, what needs strengthening and what the next learning step should be.']
          ],
          visual: { lesson: 'Fractions', lessonLabel: 'Lesson', progress: '75%', mastery: '84%', masteryLabel: 'Mastery', next: 'Personalized practice', nextLabel: 'Next step', mode: 'Practice' }
        },
        {
          kicker: 'PARENT EXPERIENCE',
          title: 'Understand your child’s learning at a glance.',
          intro: 'Edulytics turns learning data into a clear picture of what your child is learning, what they have mastered and where they may need more support.',
          points: [
            ['Know what your child has learned', 'See mastered skills, concepts currently being learned and areas that need more practice.'],
            ['Understand where support is needed', 'Clear learning insights help you identify strengths, learning gaps and areas that deserve more attention.'],
            ['Follow progress without following every step', 'Clear summaries bring learning, practice, completion and mastery together in one easy-to-understand view, without requiring you to monitor every activity or answer.']
          ],
          visual: { child: 'Student overview', progress: '78%', progressLabel: 'Overall progress', lessons: '12', lessonsLabel: 'Lessons completed', skill1: 'Number sense', skill2: 'Fractions', skill3: 'Geometry', weekly: 'Weekly progress' }
        },
        {
          kicker: 'TEACHER EXPERIENCE',
          title: 'Turn learning data into better decisions.',
          intro: 'Edulytics gives teachers a clearer view of what has been taught and assessed, what students have mastered and where they need support, making instruction more precise and responsive to each learner.',
          points: [
            ['Assess faster while keeping control', 'Use AI to create questions and assessments quickly, then review, edit and approve them before they reach students.'],
            ['Know what each student needs', 'Skill- and Learning Outcome-level analysis shows what each student has mastered, where learning gaps exist and where more practice or support is needed.'],
            ['Plan the next step', 'Prepare activities, assignments and assessments in advance, monitor completion and progress, and use learning data to choose the right intervention at the right time.']
          ],
          visual: { classLabel: 'Class overview', students: '24 students', avg: '78%', avgLabel: 'Average score', mastery: '+26%', masteryLabel: 'Mastery growth', ai: 'AI assessment ready', action: 'Review & approve' }
        }
      ]
    },
    pl: {
      tabs: ['Doświadczenie ucznia', 'Doświadczenie rodzica', 'Doświadczenie nauczyciela'],
      items: [
        {
          kicker: 'DOŚWIADCZENIE UCZNIA',
          title: 'Rozum więcej. Ćwicz mądrzej. Rób postępy z pewnością.',
          intro: 'Edulytics zapewnia uczniowi spójne doświadczenie łączące zrozumienie, spersonalizowaną praktykę i ocenianie, pomagając zobaczyć, co zostało opanowane i nad czym warto pracować dalej.',
          points: [
            ['Ucz się poprzez zrozumienie', 'Poznawaj pojęcia matematyczne dzięki wyjaśnieniom, przykładom i aktywnościom, które wspierają myślenie i pokazują sposób rozwiązania krok po kroku.'],
            ['Ćwicz dokładnie to, czego potrzebujesz', 'Edulytics zapewnia spersonalizowaną praktykę dopasowaną do poziomu i wyników ucznia, koncentrując się na umiejętnościach i pojęciach wymagających dalszego ćwiczenia.'],
            ['Wiedz, co już opanowałeś i jaki jest następny krok', 'Edulytics analizuje wyniki na poziomie umiejętności i efektów uczenia się, aby wskazać, co zostało opanowane, co wymaga wzmocnienia i na czym skupić się dalej.']
          ],
          visual: { lesson: 'Ułamki', lessonLabel: 'Lekcja', progress: '75%', mastery: '84%', masteryLabel: 'Opanowanie', next: 'Spersonalizowana praktyka', nextLabel: 'Następny krok', mode: 'Ćwiczenia' }
        },
        {
          kicker: 'DOŚWIADCZENIE RODZICA',
          title: 'Zrozum naukę swojego dziecka na pierwszy rzut oka.',
          intro: 'Edulytics zamienia dane o nauce w czytelny obraz tego, czego dziecko się uczy, co już opanowało i gdzie może potrzebować dodatkowego wsparcia.',
          points: [
            ['Zobacz, czego nauczyło się Twoje dziecko', 'Sprawdź opanowane umiejętności, aktualnie poznawane pojęcia i obszary wymagające dalszych ćwiczeń.'],
            ['Zrozum, gdzie potrzebne jest wsparcie', 'Czytelne informacje o nauce pomagają dostrzec mocne strony, luki oraz obszary wymagające większej uwagi.'],
            ['Śledź postępy bez kontrolowania każdego kroku', 'Przejrzyste podsumowania łączą naukę, ćwiczenia, realizację zadań i poziom opanowania w jednym widoku, bez potrzeby monitorowania każdej aktywności i odpowiedzi.']
          ],
          visual: { child: 'Przegląd ucznia', progress: '78%', progressLabel: 'Ogólny postęp', lessons: '12', lessonsLabel: 'Ukończone lekcje', skill1: 'Liczby', skill2: 'Ułamki', skill3: 'Geometria', weekly: 'Postęp tygodniowy' }
        },
        {
          kicker: 'DOŚWIADCZENIE NAUCZYCIELA',
          title: 'Zamieniaj dane o nauce w lepsze decyzje.',
          intro: 'Edulytics daje nauczycielowi wyraźniejszy obraz tego, co zostało nauczone i ocenione, co uczniowie opanowali oraz gdzie potrzebują wsparcia, dzięki czemu nauczanie może być bardziej precyzyjne i dopasowane do potrzeb każdego ucznia.',
          points: [
            ['Oceniaj szybciej, zachowując pełną kontrolę', 'Wykorzystaj AI do szybkiego tworzenia pytań i ocen, a następnie sprawdź, edytuj i zatwierdź je przed udostępnieniem uczniom.'],
            ['Poznaj potrzeby każdego ucznia', 'Analiza na poziomie umiejętności i efektów uczenia się pokazuje, co każdy uczeń opanował, gdzie występują luki oraz gdzie potrzebne są dodatkowe ćwiczenia lub wsparcie.'],
            ['Zaplanuj następny krok', 'Twórz z wyprzedzeniem aktywności, zadania i oceny, śledź realizację i postępy oraz wykorzystuj dane o nauce, aby dobrać właściwe wsparcie we właściwym czasie.']
          ],
          visual: { classLabel: 'Przegląd klasy', students: '24 uczniów', avg: '78%', avgLabel: 'Średni wynik', mastery: '+26%', masteryLabel: 'Wzrost opanowania', ai: 'Ocena AI gotowa', action: 'Sprawdź i zatwierdź' }
        }
      ]
    },
    ar: {
      tabs: ['تجربة الطالب', 'تجربة ولي الأمر', 'تجربة المعلّم'],
      items: [
        {
          kicker: 'تجربة الطالب',
          title: 'افهم أكثر. تدرّب بذكاء. وتقدّم بثقة.',
          intro: 'يمنح Edulytics الطالب تجربة تعلّم متكاملة تجمع بين الفهم، والتدريب الشخصي، والتقييم، وتساعده على معرفة ما أتقنه وما يحتاج إلى العمل عليه بعد ذلك.',
          points: [
            ['تعلّم من خلال الفهم', 'استكشف المفاهيم الرياضية من خلال الشرح، والأمثلة، والأنشطة التي تساعدك على التفكير وفهم طريقة الحل خطوة بخطوة.'],
            ['تدرّب على ما تحتاجه فعلًا', 'يقدّم Edulytics تدريبًا شخصيًا موجّهًا وفق مستوى الطالب وأدائه، ليركّز على المهارات والمفاهيم التي تحتاج إلى مزيد من الممارسة.'],
            ['اعرف ما أتقنته وما خطوتك التالية', 'يحلّل Edulytics أداءك عبر المهارات ونواتج التعلّم ليحدد ما أتقنته، وما يحتاج إلى تقوية، ويوجّهك نحو الخطوة التالية في تعلّمك.']
          ],
          visual: { lesson: 'الكسور', lessonLabel: 'الدرس', progress: '75%', mastery: '84%', masteryLabel: 'الإتقان', next: 'تدريب شخصي موجّه', nextLabel: 'الخطوة التالية', mode: 'تدريب' }
        },
        {
          kicker: 'تجربة ولي الأمر',
          title: 'افهم تعلّم طفلك في لمحة واحدة.',
          intro: 'يحوّل Edulytics بيانات التعلّم إلى صورة واضحة تساعدك على فهم ما يتعلّمه طفلك، وما أتقنه، وأين قد يحتاج إلى مزيد من الدعم.',
          points: [
            ['اعرف ما تعلّمه طفلك', 'اطّلع على المهارات التي أتقنها، والمفاهيم التي يتعلّمها حاليًا، والجوانب التي تحتاج إلى مزيد من التدريب.'],
            ['افهم أين يحتاج إلى الدعم', 'رؤى تعليمية واضحة تساعدك على التعرف على نقاط القوة وفجوات التعلّم والمجالات التي تحتاج إلى مزيد من الاهتمام.'],
            ['تابع التقدّم دون متابعة كل خطوة', 'ملخصات واضحة تجمع التعلّم، والتدريب، والإنجاز، والإتقان في صورة سهلة الفهم، دون الحاجة إلى متابعة كل نشاط أو كل إجابة.']
          ],
          visual: { child: 'ملخص تعلّم الطالب', progress: '78%', progressLabel: 'التقدّم العام', lessons: '12', lessonsLabel: 'دروس مكتملة', skill1: 'الأعداد', skill2: 'الكسور', skill3: 'الهندسة', weekly: 'التقدّم الأسبوعي' }
        },
        {
          kicker: 'تجربة المعلّم',
          title: 'حوّل بيانات التعلّم إلى قرارات أفضل.',
          intro: 'يمنح Edulytics المعلّم رؤية أوضح لما تم تدريسه وتقييمه، وما أتقنه الطلاب، وأين يحتاجون إلى دعم، حتى يصبح التدريس أكثر دقة واستجابة لاحتياجات كل طالب.',
          points: [
            ['قيّم بشكل أسرع مع بقاء القرار بيدك', 'استخدم الذكاء الاصطناعي لإنشاء الأسئلة والتقييمات بسرعة، ثم راجعها وعدّلها واعتمدها قبل تقديمها للطلاب.'],
            ['اعرف احتياجات كل طالب', 'يوضح تحليل المهارات ونواتج التعلّم ما أتقنه كل طالب، وأين توجد فجوات التعلّم، وما يحتاج إلى مزيد من التدريب أو الدعم.'],
            ['خطط للخطوة التالية', 'أنشئ الأنشطة والواجبات والتقييمات مسبقًا، وتابع الإنجاز والتقدّم، واستخدم بيانات التعلّم لتحديد التدخل المناسب في الوقت المناسب.']
          ],
          visual: { classLabel: 'ملخص الفصل', students: '24 طالبًا', avg: '78%', avgLabel: 'متوسط النتائج', mastery: '+26%', masteryLabel: 'نمو الإتقان', ai: 'التقييم بالذكاء الاصطناعي جاهز', action: 'راجع واعتمد' }
        }
      ]
    }
  };

  const data = copy[language];
  const isArabic = language === 'ar';

  const studentVisual = (v) => `
    <div class="ed-exp-v20-scene student-scene" aria-hidden="true">
      <span class="ed-exp-v20-shape s1"></span><span class="ed-exp-v20-shape s2"></span><span class="ed-exp-v20-shape s3"></span>
      <div class="ed-exp-v20-browser">
        <div class="browser-top"><i></i><i></i><i></i><strong>Edulytics</strong></div>
        <div class="student-board">
          <div class="visual-card lesson-card"><small>${v.lessonLabel}</small><strong>${v.lesson}</strong><div class="mini-progress"><span></span></div><b>${v.progress}</b></div>
          <div class="visual-card mastery-card"><small>${v.masteryLabel}</small><strong>${v.mastery}</strong><div class="bars"><i></i><i></i><i></i><i></i></div></div>
          <div class="student-avatar"><span class="head"></span><span class="body"></span><span class="screen"></span></div>
          <div class="visual-card next-card"><small>${v.nextLabel}</small><strong>${v.next}</strong><span class="target">◎</span></div>
          <span class="fraction-bubble">¾</span><span class="mode-badge">${v.mode}</span>
        </div>
      </div>
    </div>`;

  const parentVisual = (v) => `
    <div class="ed-exp-v20-scene parent-scene" aria-hidden="true">
      <span class="ed-exp-v20-shape s1"></span><span class="ed-exp-v20-shape s2"></span><span class="ed-exp-v20-shape s3"></span>
      <div class="ed-exp-v20-browser">
        <div class="browser-top"><i></i><i></i><i></i><strong>Edulytics</strong></div>
        <div class="parent-board">
          <div class="parent-heading"><span class="avatar-dot">S</span><div><small>${v.child}</small><strong>${v.weekly}</strong></div></div>
          <div class="parent-kpis">
            <div><span class="ring">${v.progress}</span><small>${v.progressLabel}</small></div>
            <div><strong>${v.lessons}</strong><small>${v.lessonsLabel}</small></div>
          </div>
          <div class="skill-panel"><div><span>${v.skill1}</span><b style="--w:88%"></b></div><div><span>${v.skill2}</span><b style="--w:72%"></b></div><div><span>${v.skill3}</span><b style="--w:90%"></b></div></div>
          <div class="weekly-bars"><i></i><i></i><i></i><i></i><i></i><i></i><i></i></div>
          <div class="floating-summary"><span>✓</span><strong>${v.progress}</strong><small>${v.progressLabel}</small></div>
        </div>
      </div>
    </div>`;

  const teacherVisual = (v) => `
    <div class="ed-exp-v20-scene teacher-scene" aria-hidden="true">
      <span class="ed-exp-v20-shape s1"></span><span class="ed-exp-v20-shape s2"></span><span class="ed-exp-v20-shape s3"></span>
      <div class="ed-exp-v20-browser">
        <div class="browser-top"><i></i><i></i><i></i><strong>Edulytics</strong></div>
        <div class="teacher-board">
          <div class="teacher-heading"><div><small>${v.classLabel}</small><strong>${v.students}</strong></div><span>⋯</span></div>
          <div class="teacher-kpis"><div><strong>${v.avg}</strong><small>${v.avgLabel}</small></div><div><strong>${v.mastery}</strong><small>${v.masteryLabel}</small></div></div>
          <div class="student-table"><div><span>Student A</span><b style="--w:92%"></b><em>92%</em></div><div><span>Student B</span><b style="--w:78%"></b><em>78%</em></div><div><span>Student C</span><b style="--w:67%"></b><em>67%</em></div><div><span>Student D</span><b style="--w:85%"></b><em>85%</em></div></div>
          <div class="floating-ai"><span>✦</span><div><small>${v.ai}</small><strong>${v.action}</strong></div></div>
        </div>
      </div>
    </div>`;

  const visualTemplates = [studentVisual, parentVisual, teacherVisual];

  section.className = `ed-home-section ed-home-experience ed-exp-v20${isArabic ? ' is-ar' : ''}`;
  if (isArabic) section.setAttribute('dir', 'rtl'); else section.removeAttribute('dir');
  section.innerHTML = `
    <div class="ed-home-container ed-exp-v20-shell">
      <div class="ed-exp-v20-tabs" role="tablist" aria-label="Experience">
        ${data.tabs.map((tab, i) => `<button type="button" role="tab" aria-selected="${i === 0 ? 'true' : 'false'}" class="${i === 0 ? 'is-active' : ''}" data-exp-index="${i}">${tab}</button>`).join('')}
      </div>
      <div class="ed-exp-v20-stage" role="tabpanel"></div>
    </div>`;

  const stage = section.querySelector('.ed-exp-v20-stage');
  const tabs = [...section.querySelectorAll('.ed-exp-v20-tabs button')];

  const render = (index) => {
    const item = data.items[index];
    const visual = visualTemplates[index](item.visual);
    stage.innerHTML = `
      <div class="ed-exp-v20-grid">
        <div class="ed-exp-v20-visual">${visual}</div>
        <div class="ed-exp-v20-copy">
          <span class="ed-exp-v20-kicker">${item.kicker}</span>
          <h2>${item.title}</h2>
          <p class="ed-exp-v20-intro">${item.intro}</p>
          <div class="ed-exp-v20-points">
            ${item.points.map(([title, body]) => `<div class="ed-exp-v20-point"><h3>${title}</h3><p>${body}</p></div>`).join('')}
          </div>
        </div>
      </div>`;
    tabs.forEach((tab, i) => {
      const active = i === index;
      tab.classList.toggle('is-active', active);
      tab.setAttribute('aria-selected', active ? 'true' : 'false');
      tab.tabIndex = active ? 0 : -1;
    });
  };

  tabs.forEach((tab, index) => {
    tab.addEventListener('click', () => render(index));
    tab.addEventListener('keydown', (event) => {
      if (!['ArrowLeft', 'ArrowRight'].includes(event.key)) return;
      event.preventDefault();
      const direction = event.key === 'ArrowRight' ? 1 : -1;
      const next = (index + direction + tabs.length) % tabs.length;
      tabs[next].focus();
      render(next);
    });
  });

  render(0);

  if (!document.getElementById('ed-exp-v20-style')) {
    const style = document.createElement('style');
    style.id = 'ed-exp-v20-style';
    style.textContent = `
      .ed-home .ed-exp-v20{padding:62px 0 76px;background:#fff;overflow:hidden}
      .ed-home .ed-exp-v20-shell{width:min(1500px,calc(100% - 48px));margin:0 auto}
      .ed-home .ed-exp-v20-tabs{display:grid;grid-template-columns:repeat(3,1fr);border-bottom:1px solid #dfe6ef;margin:0 auto 54px;max-width:900px}
      .ed-home .ed-exp-v20-tabs button{appearance:none;border:0;background:transparent;padding:0 18px 18px;color:#7d8ba1;font-size:16px;font-weight:800;line-height:1.2;position:relative;cursor:pointer;transition:color .2s ease}
      .ed-home .ed-exp-v20-tabs button::after{content:"";position:absolute;left:0;right:0;bottom:-1px;height:3px;background:#ff6b1a;transform:scaleX(0);transform-origin:center;transition:transform .22s ease}
      .ed-home .ed-exp-v20-tabs button:hover,.ed-home .ed-exp-v20-tabs button:focus-visible,.ed-home .ed-exp-v20-tabs button.is-active{color:#ff6413;outline:none}
      .ed-home .ed-exp-v20-tabs button.is-active::after{transform:scaleX(1)}
      .ed-home .ed-exp-v20-grid{display:grid;grid-template-columns:minmax(0,1.03fr) minmax(0,.97fr);gap:72px;align-items:center}
      .ed-home .ed-exp-v20-copy{max-width:680px}
      .ed-home .ed-exp-v20-kicker{display:block;margin-bottom:18px;color:#ff6413;font-size:14px;font-weight:900;letter-spacing:.13em;text-transform:uppercase}
      .ed-home .ed-exp-v20-copy h2{margin:0;color:#10213c;font-size:clamp(38px,3.7vw,62px);line-height:1.08;font-weight:900;letter-spacing:-.035em;text-wrap:balance}
      .ed-home .ed-exp-v20-intro{margin:20px 0 24px;color:#405a7c;font-size:18px;line-height:1.62;font-weight:500}
      .ed-home .ed-exp-v20-points{border-top:1px solid #e4eaf2}
      .ed-home .ed-exp-v20-point{padding:20px 0;border-bottom:1px solid #e4eaf2}
      .ed-home .ed-exp-v20-point h3{margin:0 0 7px;color:#11233d;font-size:17px;line-height:1.35;font-weight:900}
      .ed-home .ed-exp-v20-point p{margin:0;color:#526b8b;font-size:16px;line-height:1.62;font-weight:500}
      .ed-home .ed-exp-v20-visual{min-width:0}
      .ed-home .ed-exp-v20-scene{position:relative;min-height:520px;display:grid;place-items:center}
      .ed-home .ed-exp-v20-browser{position:relative;width:min(100%,690px);min-height:455px;border-radius:30px;background:linear-gradient(145deg,#f0f6ff,#f8fbff);border:1px solid #d9e7f7;box-shadow:0 28px 60px rgba(27,57,96,.15);overflow:visible}
      .ed-home .ed-exp-v20-browser::before{content:"";position:absolute;inset:0;border-radius:30px;background:radial-gradient(circle at 80% 12%,rgba(48,111,241,.09),transparent 28%);pointer-events:none}
      .ed-home .browser-top{height:52px;padding:0 22px;display:flex;align-items:center;gap:8px;border-bottom:1px solid #dce7f3;color:#607592;font-size:12px;position:relative;z-index:2}
      .ed-home .browser-top i{width:9px;height:9px;border-radius:50%;background:#ff8068}.ed-home .browser-top i:nth-child(2){background:#ffc548}.ed-home .browser-top i:nth-child(3){background:#45c98d}.ed-home .browser-top strong{margin-inline-start:auto;color:#286df0;font-weight:850}
      .ed-home .ed-exp-v20-shape{position:absolute;display:block;z-index:0;filter:drop-shadow(0 8px 12px rgba(43,62,92,.08))}
      .ed-home .ed-exp-v20-shape.s1{width:54px;height:54px;border-radius:50%;background:#7148f6;left:0;top:30px}
      .ed-home .ed-exp-v20-shape.s2{width:48px;height:48px;background:#ffd735;left:24px;bottom:45px;transform:rotate(18deg);border-radius:14px}
      .ed-home .ed-exp-v20-shape.s3{width:42px;height:42px;background:#42c879;right:0;top:105px;transform:rotate(35deg);border-radius:12px}
      .ed-home .student-board,.ed-home .parent-board,.ed-home .teacher-board{position:relative;min-height:402px;padding:28px;box-sizing:border-box}
      .ed-home .visual-card{position:absolute;background:#fff;border:1px solid #d9e6f4;border-radius:18px;box-shadow:0 12px 26px rgba(39,69,106,.09);padding:18px 20px;color:#11233d}
      .ed-home .visual-card small{display:block;margin-bottom:8px;color:#356ceb;font-size:11px;font-weight:900;text-transform:uppercase;letter-spacing:.05em}.ed-home .visual-card strong{display:block;font-size:22px;line-height:1.15;font-weight:900}.ed-home .lesson-card{left:42px;top:42px;width:218px}.ed-home .mastery-card{right:42px;top:38px;width:190px}.ed-home .next-card{right:55px;bottom:52px;width:220px;background:#fff4e8;border-color:#ffe0c1}.ed-home .next-card small{color:#e56d16}.ed-home .next-card .target{position:absolute;right:18px;bottom:12px;color:#ff8a2c;font-size:34px}.ed-home .mini-progress{height:9px;border-radius:999px;background:#e7eef8;margin:12px 42px 0 0;overflow:hidden}.ed-home .mini-progress span{display:block;width:75%;height:100%;background:#2e6ef0;border-radius:999px}.ed-home .lesson-card b{position:absolute;right:18px;bottom:15px;font-size:12px;color:#60738f}.ed-home .mastery-card strong{color:#08a66d;font-size:38px}.ed-home .bars{display:flex;gap:5px;align-items:flex-end;height:36px;position:absolute;right:18px;bottom:18px}.ed-home .bars i{width:9px;border-radius:5px;background:#7fdcaf}.ed-home .bars i:nth-child(1){height:12px}.ed-home .bars i:nth-child(2){height:19px}.ed-home .bars i:nth-child(3){height:27px}.ed-home .bars i:nth-child(4){height:35px;background:#10a76f}
      .ed-home .student-avatar{position:absolute;left:50%;bottom:48px;transform:translateX(-55%);width:180px;height:220px}.ed-home .student-avatar .head{position:absolute;width:86px;height:86px;border-radius:50%;background:#f2a16c;left:47px;top:6px;box-shadow:inset 0 25px 0 #553326}.ed-home .student-avatar .body{position:absolute;width:145px;height:126px;border-radius:72px 72px 20px 20px;background:#2f69e9;left:18px;bottom:0}.ed-home .student-avatar .screen{position:absolute;width:126px;height:82px;border-radius:10px;background:#fff;border:4px solid #aec6e5;left:30px;bottom:-2px;box-shadow:0 9px 18px rgba(32,64,104,.12)}
      .ed-home .fraction-bubble{position:absolute;left:78px;bottom:82px;width:58px;height:58px;border-radius:50%;display:grid;place-items:center;background:#7148f6;color:#fff;font-size:25px;font-weight:900}.ed-home .mode-badge{position:absolute;left:42px;bottom:24px;padding:8px 14px;border-radius:999px;background:#eef4ff;color:#2e6ef0;font-size:12px;font-weight:850}
      .ed-home .parent-heading,.ed-home .teacher-heading{display:flex;align-items:center;justify-content:space-between;gap:14px;margin-bottom:18px}.ed-home .parent-heading>div,.ed-home .teacher-heading>div{display:flex;flex-direction:column}.ed-home .parent-heading small,.ed-home .teacher-heading small{color:#7990ad;font-size:11px;font-weight:800}.ed-home .parent-heading strong,.ed-home .teacher-heading strong{font-size:22px;color:#11233d}.ed-home .avatar-dot{width:44px;height:44px;border-radius:50%;display:grid;place-items:center;background:#2f6bea;color:#fff;font-weight:900}
      .ed-home .parent-kpis,.ed-home .teacher-kpis{display:grid;grid-template-columns:1fr 1fr;gap:14px}.ed-home .parent-kpis>div,.ed-home .teacher-kpis>div{min-height:105px;background:#fff;border:1px solid #dde8f4;border-radius:16px;padding:16px;display:flex;flex-direction:column;justify-content:center;box-shadow:0 8px 18px rgba(39,69,106,.06)}.ed-home .parent-kpis strong,.ed-home .teacher-kpis strong{font-size:30px;color:#16315d}.ed-home .parent-kpis small,.ed-home .teacher-kpis small{margin-top:6px;color:#6d829d;font-size:11px;font-weight:750}.ed-home .ring{width:64px;height:64px;border-radius:50%;display:grid;place-items:center;background:conic-gradient(#13b273 78%,#e8eef7 0);position:relative;color:#10213c;font-weight:900}.ed-home .ring::before{content:"";position:absolute;inset:8px;border-radius:50%;background:#fff}.ed-home .ring{isolation:isolate}.ed-home .ring::after{content:attr(data-value)}.ed-home .ring{font-size:0}.ed-home .ring::after{content:'78%';font-size:14px;position:relative;z-index:2}
      .ed-home .skill-panel{margin-top:16px;background:#fff;border:1px solid #dde8f4;border-radius:16px;padding:16px}.ed-home .skill-panel>div{display:grid;grid-template-columns:110px 1fr;align-items:center;gap:12px;margin:10px 0}.ed-home .skill-panel span{color:#405a7c;font-size:11px;font-weight:750}.ed-home .skill-panel b,.ed-home .student-table b{height:8px;border-radius:999px;background:linear-gradient(90deg,#2f6ef0 var(--w),#e8eef7 var(--w));display:block}
      .ed-home .weekly-bars{position:absolute;right:34px;bottom:28px;width:180px;height:70px;display:flex;align-items:flex-end;gap:8px}.ed-home .weekly-bars i{flex:1;border-radius:5px 5px 0 0;background:#77aaf8}.ed-home .weekly-bars i:nth-child(1){height:28%}.ed-home .weekly-bars i:nth-child(2){height:58%}.ed-home .weekly-bars i:nth-child(3){height:43%}.ed-home .weekly-bars i:nth-child(4){height:72%}.ed-home .weekly-bars i:nth-child(5){height:92%}.ed-home .weekly-bars i:nth-child(6){height:65%}.ed-home .weekly-bars i:nth-child(7){height:82%}.ed-home .floating-summary{position:absolute;right:-28px;top:72px;width:160px;background:#fff;border:1px solid #dde8f4;border-radius:18px;padding:14px;box-shadow:0 18px 34px rgba(38,69,107,.15);display:grid;grid-template-columns:auto 1fr;gap:2px 10px;align-items:center}.ed-home .floating-summary span{grid-row:1/3;width:34px;height:34px;border-radius:50%;display:grid;place-items:center;background:#eaf9f2;color:#0ea970;font-weight:900}.ed-home .floating-summary strong{font-size:22px;color:#0ea970}.ed-home .floating-summary small{font-size:10px;color:#758aa4}
      .ed-home .teacher-kpis{grid-template-columns:1fr 1fr;margin-bottom:16px}.ed-home .student-table{background:#fff;border:1px solid #dde8f4;border-radius:16px;padding:12px 16px}.ed-home .student-table>div{display:grid;grid-template-columns:100px 1fr 42px;align-items:center;gap:12px;padding:10px 0;border-bottom:1px solid #edf1f6}.ed-home .student-table>div:last-child{border-bottom:0}.ed-home .student-table span{font-size:11px;color:#405a7c;font-weight:750}.ed-home .student-table em{font-style:normal;font-size:11px;color:#6c819c;font-weight:800}.ed-home .floating-ai{position:absolute;right:-32px;bottom:22px;width:230px;background:#fff8e8;border:1px solid #ffe2ab;border-radius:18px;padding:16px;display:flex;gap:12px;align-items:center;box-shadow:0 18px 34px rgba(50,68,94,.14)}.ed-home .floating-ai>span{width:40px;height:40px;border-radius:12px;display:grid;place-items:center;background:#ffbe34;color:#fff;font-size:20px}.ed-home .floating-ai div{display:flex;flex-direction:column}.ed-home .floating-ai small{font-size:10px;color:#9b6b17;font-weight:800}.ed-home .floating-ai strong{margin-top:3px;font-size:13px;color:#70480d}
      .ed-home .ed-exp-v20.is-ar .ed-exp-v20-copy{text-align:right}.ed-home .ed-exp-v20.is-ar .ed-exp-v20-kicker{letter-spacing:0;text-transform:none;font-size:16px}.ed-home .ed-exp-v20.is-ar .ed-exp-v20-copy h2{letter-spacing:0;line-height:1.45}.ed-home .ed-exp-v20.is-ar .ed-exp-v20-intro,.ed-home .ed-exp-v20.is-ar .ed-exp-v20-point p{line-height:1.9}.ed-home .ed-exp-v20.is-ar .ed-exp-v20-tabs{direction:rtl}.ed-home .ed-exp-v20.is-ar .ed-exp-v20-grid{direction:rtl}.ed-home .ed-exp-v20.is-ar .ed-exp-v20-visual{direction:ltr}.ed-home .ed-exp-v20.is-ar .ed-exp-v20-copy{direction:rtl}
      @media(max-width:1100px){.ed-home .ed-exp-v20-grid{grid-template-columns:1fr;gap:38px}.ed-home .ed-exp-v20-copy{max-width:820px;margin:0 auto}.ed-home .ed-exp-v20-visual{order:2}.ed-home .ed-exp-v20-copy{order:1}.ed-home .ed-exp-v20-scene{min-height:500px}.ed-home .ed-exp-v20-browser{max-width:720px}}
      @media(max-width:720px){.ed-home .ed-exp-v20{padding:42px 0 54px}.ed-home .ed-exp-v20-shell{width:min(100% - 28px,1500px)}.ed-home .ed-exp-v20-tabs{overflow-x:auto;display:flex;gap:18px;margin-bottom:34px;scrollbar-width:none}.ed-home .ed-exp-v20-tabs::-webkit-scrollbar{display:none}.ed-home .ed-exp-v20-tabs button{flex:0 0 auto;min-width:155px;padding:0 10px 14px;font-size:14px}.ed-home .ed-exp-v20-copy h2{font-size:clamp(34px,10vw,48px)}.ed-home .ed-exp-v20-intro{font-size:16px}.ed-home .ed-exp-v20-point p{font-size:15px}.ed-home .ed-exp-v20-scene{min-height:390px}.ed-home .ed-exp-v20-browser{transform:scale(.78);transform-origin:center;width:660px;min-height:450px}.ed-home .ed-exp-v20-visual{height:390px;display:grid;place-items:center}.ed-home .ed-exp-v20-shape.s1{left:6px}.ed-home .ed-exp-v20-shape.s2{left:12px}.ed-home .ed-exp-v20-shape.s3{right:6px}}
      @media(max-width:480px){.ed-home .ed-exp-v20-browser{transform:scale(.62)}.ed-home .ed-exp-v20-visual{height:315px}.ed-home .ed-exp-v20-scene{min-height:315px}.ed-home .ed-exp-v20-point{padding:17px 0}.ed-home .ed-exp-v20-tabs button{min-width:138px}}
    `;
    document.head.appendChild(style);
  }
})();
