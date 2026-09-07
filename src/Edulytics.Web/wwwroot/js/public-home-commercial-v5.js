(() => {
    const root = document.querySelector('.ed-home');
    if (!root) return;

    const pl = (document.documentElement.lang || '').toLowerCase().startsWith('pl');
    const hero = root.querySelector('.ed-home-hero');
    const copy = hero?.querySelector('.ed-home-hero-copy');
    const kicker = copy?.querySelector('.ed-home-kicker');
    const title = copy?.querySelector('h1');
    const body = copy?.querySelector(':scope > p');
    const checks = copy?.querySelector('.ed-home-hero-checks');
    const primary = copy?.querySelector('.ed-home-cta');
    const secondary = copy?.querySelector('.ed-home-secondary');

    const slides = pl ? [
        {
            kicker: 'MATEMATYKA • NAUKA • POSTĘP',
            title: 'Ucz matematyki mądrzej. Zobacz, czego naprawdę potrzebuje każdy uczeń.',
            body: 'Edulytics łączy program nauczania, treści lekcji, ocenianie, praktykę ucznia, analizę danych i AI w jednym środowisku — tak aby nauczyciel szybciej podejmował trafne decyzje, a uczeń zawsze znał swój następny krok.',
            checks: ['Program nauczania w centrum', 'AI pod kontrolą nauczyciela', 'Wyniki, mastery i następny krok'],
            secondaryText: 'Zobacz platformę', secondaryHref: '#platform'
        },
        {
            kicker: 'WSPARCIE NAUCZYCIELI • AI • DANE',
            title: 'Wspieramy nauczycieli. Budujemy lepszą przyszłość nauki.',
            body: 'Łączymy kontekst programu nauczania, analizę danych i sztuczną inteligencję, aby pomóc nauczycielom lepiej rozumieć potrzeby każdego ucznia, tworzyć trafniejsze oceny i prowadzić bardziej spersonalizowaną naukę matematyki.',
            checks: ['Generate with Edulytics AI', 'Do 50 pytań na generację', 'Nauczyciel zatwierdza przed publikacją'],
            secondaryText: 'Poznaj Edulytics AI', secondaryHref: '#ai'
        },
        {
            kicker: 'INTERAKTYWNA NAUKA • PRAKTYKA • MASTERY',
            title: 'Matematyka bliżej prawdziwego życia.',
            body: 'Interaktywna nauka pomaga zamieniać pojęcia matematyczne w praktyczne wyzwania — od liczb, procentów i cen po budżetowanie i rozwiązywanie problemów — aby uczniowie myśleli, ćwiczyli i rozwijali pewność siebie.',
            checks: ['Lekcje i przykłady', 'Prywatna praktyka AI', 'Postęp widoczny dla ucznia'],
            secondaryText: 'Zobacz doświadczenie ucznia', secondaryHref: '#experience'
        }
    ] : [
        {
            kicker: 'MATHEMATICS • LEARNING • PROGRESS',
            title: 'Teach mathematics smarter. See what every student needs next.',
            body: 'Edulytics brings curriculum, lesson content, assessment, student practice, learning data and AI into one environment — helping teachers make better decisions faster and giving every student a clear next step.',
            checks: ['Curriculum at the centre', 'Teacher-controlled AI', 'Results, mastery and next step'],
            secondaryText: 'Explore the platform', secondaryHref: '#platform'
        },
        {
            kicker: 'TEACHER SUPPORT • AI • DATA',
            title: 'Empower teachers. Build a better future for learning.',
            body: 'Edulytics combines curriculum context, learning data and artificial intelligence to help teachers understand student needs, build stronger assessments and deliver more personalised mathematics learning.',
            checks: ['Generate with Edulytics AI', 'Up to 50 questions per generation', 'Teacher approval before publishing'],
            secondaryText: 'Explore Edulytics AI', secondaryHref: '#ai'
        },
        {
            kicker: 'INTERACTIVE LEARNING • PRACTICE • MASTERY',
            title: 'Bring mathematics closer to real life.',
            body: 'An interactive learning experience turns mathematical ideas into practical challenges — from numbers, percentages and prices to budgeting and problem solving — helping students think, practise and build confidence.',
            checks: ['Lessons and examples', 'Private AI practice', 'Progress students can see'],
            secondaryText: 'Explore the student experience', secondaryHref: '#experience'
        }
    ];

    if (hero && copy && kicker && title && body && checks) {
        const dots = [...hero.querySelectorAll('.ed-home-slide-dot')];
        const render = index => {
            const slide = slides[index];
            kicker.textContent = slide.kicker;
            title.textContent = slide.title;
            body.textContent = slide.body;
            checks.innerHTML = slide.checks.map(item => `<span>${item}</span>`).join('');
            if (primary) {
                primary.textContent = pl ? 'Poproś o demo →' : 'Request a demo →';
                primary.setAttribute('href', '/contact');
            }
            if (secondary) {
                secondary.textContent = slide.secondaryText;
                secondary.setAttribute('href', slide.secondaryHref);
            }
            dots.forEach((dot, i) => {
                dot.classList.toggle('is-active', i === index);
                dot.setAttribute('aria-selected', i === index ? 'true' : 'false');
            });
        };
        const syncFromClass = () => {
            if (hero.classList.contains('is-slide-assessment')) render(1);
            else if (hero.classList.contains('is-slide-student')) render(2);
            else render(0);
        };
        new MutationObserver(syncFromClass).observe(hero, { attributes: true, attributeFilter: ['class'] });
        hero.querySelectorAll('.ed-home-slide-arrow,.ed-home-slide-dot').forEach(control => control.addEventListener('click', () => window.setTimeout(syncFromClass, 0)));
        syncFromClass();
    }

    root.querySelectorAll('a[href^="mailto:"][href*="demo"], .ed-home-final-cta .ed-home-cta').forEach(link => link.setAttribute('href', '/contact'));

    const audience = root.querySelector('.ed-home-audience-band');
    if (!audience || root.querySelector('.ed-home-v5-story')) return;

    const student = document.createElement('section');
    student.className = 'ed-home-v5-story is-student';
    student.innerHTML = pl ? `
        <div class="ed-home-v5-story-grid">
            <div class="ed-home-v5-story-copy"><span class="eyebrow">MATEMATYKA W PRAKTYCE</span><h2>Od pojęcia do decyzji, którą uczeń rozumie.</h2><p>Edulytics pomaga łączyć matematykę z sytuacjami, które uczniowie rozpoznają z codziennego życia. Procenty stają się rabatem, działania — budżetem, a rozwiązywanie problemów — świadomym wyborem. Uczeń ćwiczy, sprawdza wynik i widzi, nad czym powinien pracować dalej.</p><div class="ed-home-v5-story-actions"><a class="ed-home-cta ed-home-cta-large" href="/contact">Porozmawiaj z nami</a><a class="ed-home-secondary ed-home-cta-large" href="#experience">Zobacz doświadczenie ucznia</a></div></div>
            <div class="ed-home-v5-story-visual" aria-label="Mathematics in real-life situations"><svg viewBox="0 0 720 430" role="img"><rect width="720" height="430" fill="#eaf6ff"/><rect x="64" y="54" width="592" height="318" rx="28" fill="#fff" stroke="#cfe1f5" stroke-width="3"/><rect x="92" y="84" width="238" height="124" rx="20" fill="#fff4e9"/><text x="118" y="118" font-size="16" font-weight="800" fill="#2f66e8">PRAKTYCZNE WYZWANIE</text><text x="118" y="160" font-size="28" font-weight="900" fill="#10244a">25% rabatu</text><text x="118" y="190" font-size="18" fill="#526780">200 zł → ?</text><rect x="366" y="84" width="246" height="124" rx="20" fill="#effaf5"/><text x="392" y="118" font-size="16" font-weight="800" fill="#15a66f">TWÓJ POSTĘP</text><text x="392" y="168" font-size="42" font-weight="900" fill="#15a66f">82%</text><rect x="92" y="238" width="520" height="94" rx="20" fill="#f5f1ff"/><text x="118" y="275" font-size="15" font-weight="800" fill="#6e47ed">EDULYTICS AI PRACTICE</text><text x="118" y="309" font-size="23" font-weight="900" fill="#10244a">Ćwicz słabsze obszary i sprawdź kolejny krok</text></svg><span class="ed-home-v5-story-chip one">Procenty • ceny • budżet</span><span class="ed-home-v5-story-chip two">Wynik → mastery → next step</span></div>
        </div>` : `
        <div class="ed-home-v5-story-grid">
            <div class="ed-home-v5-story-copy"><span class="eyebrow">MATHEMATICS IN PRACTICE</span><h2>From a mathematical idea to a decision students understand.</h2><p>Edulytics helps connect mathematics with situations students recognise from everyday life. Percentages become discounts, operations become budgets, and problem solving becomes a meaningful choice. Students practise, check results and see what to work on next.</p><div class="ed-home-v5-story-actions"><a class="ed-home-cta ed-home-cta-large" href="/contact">Talk to us</a><a class="ed-home-secondary ed-home-cta-large" href="#experience">Explore student experience</a></div></div>
            <div class="ed-home-v5-story-visual" aria-label="Mathematics in real-life situations"><svg viewBox="0 0 720 430" role="img"><rect width="720" height="430" fill="#eaf6ff"/><rect x="64" y="54" width="592" height="318" rx="28" fill="#fff" stroke="#cfe1f5" stroke-width="3"/><rect x="92" y="84" width="238" height="124" rx="20" fill="#fff4e9"/><text x="118" y="118" font-size="16" font-weight="800" fill="#2f66e8">REAL-LIFE CHALLENGE</text><text x="118" y="160" font-size="28" font-weight="900" fill="#10244a">25% discount</text><text x="118" y="190" font-size="18" fill="#526780">200 PLN → ?</text><rect x="366" y="84" width="246" height="124" rx="20" fill="#effaf5"/><text x="392" y="118" font-size="16" font-weight="800" fill="#15a66f">YOUR PROGRESS</text><text x="392" y="168" font-size="42" font-weight="900" fill="#15a66f">82%</text><rect x="92" y="238" width="520" height="94" rx="20" fill="#f5f1ff"/><text x="118" y="275" font-size="15" font-weight="800" fill="#6e47ed">EDULYTICS AI PRACTICE</text><text x="118" y="309" font-size="23" font-weight="900" fill="#10244a">Practise weaker areas and see the next step</text></svg><span class="ed-home-v5-story-chip one">Percentages • prices • budgets</span><span class="ed-home-v5-story-chip two">Result → mastery → next step</span></div>
        </div>`;

    const teacher = document.createElement('section');
    teacher.className = 'ed-home-v5-story is-teacher';
    teacher.innerHTML = pl ? `
        <div class="ed-home-v5-story-grid">
            <div class="ed-home-v5-story-copy"><span class="eyebrow">WSPARCIE NAUCZYCIELI</span><h2>Więcej czasu na nauczanie. Lepszy obraz tego, czego potrzebuje klasa.</h2><p>Edulytics łączy analizę wyników, kontekst programu i sztuczną inteligencję, aby nauczyciel mógł szybciej przygotować ocenę, zobaczyć obszary wymagające uwagi i zaplanować kolejny krok dla uczniów — bez oddawania AI kontroli nad decyzją nauczyciela.</p><div class="ed-home-v5-story-actions"><a class="ed-home-cta ed-home-cta-large" href="/contact">Zapytaj o Edulytics</a><a class="ed-home-secondary ed-home-cta-large" href="#ai">Generate with Edulytics AI</a></div></div>
            <div class="ed-home-v5-story-visual" aria-label="Teacher assessment and analytics workspace"><svg viewBox="0 0 720 430" role="img"><rect width="720" height="430" fill="#f6f2ff"/><rect x="58" y="48" width="604" height="332" rx="28" fill="#fff" stroke="#ddd3f8" stroke-width="3"/><text x="88" y="92" font-size="17" font-weight="900" fill="#6e47ed">Generate with Edulytics AI</text><rect x="88" y="120" width="250" height="70" rx="16" fill="#eef5ff"/><text x="110" y="149" font-size="13" fill="#65758f">SCOPE</text><text x="110" y="176" font-size="20" font-weight="900" fill="#10244a">Lesson · Algebra</text><rect x="366" y="120" width="120" height="70" rx="16" fill="#fff4e9"/><text x="389" y="149" font-size="13" fill="#65758f">QUESTIONS</text><text x="389" y="177" font-size="24" font-weight="900" fill="#10244a">20</text><rect x="510" y="120" width="122" height="70" rx="16" fill="#effaf5"/><text x="533" y="149" font-size="13" fill="#65758f">LEVEL</text><text x="533" y="177" font-size="20" font-weight="900" fill="#10244a">Stage 7</text><rect x="88" y="222" width="544" height="96" rx="18" fill="#f8faff"/><circle cx="120" cy="270" r="19" fill="#2f66e8"/><text x="115" y="277" font-size="18" font-weight="900" fill="#fff">1</text><text x="156" y="257" font-size="13" fill="#65758f">GENERATED QUESTION</text><text x="156" y="291" font-size="27" font-weight="900" fill="#10244a">2(x + 3) = 14</text></svg><span class="ed-home-v5-story-chip one">Review • edit • approve</span><span class="ed-home-v5-story-chip two">Teacher remains in control</span></div>
        </div>` : `
        <div class="ed-home-v5-story-grid">
            <div class="ed-home-v5-story-copy"><span class="eyebrow">TEACHER SUPPORT</span><h2>More time for teaching. A clearer view of what the class needs.</h2><p>Edulytics combines results analysis, curriculum context and artificial intelligence so teachers can prepare assessments faster, identify areas that need attention and plan the next step — without handing instructional control to AI.</p><div class="ed-home-v5-story-actions"><a class="ed-home-cta ed-home-cta-large" href="/contact">Ask about Edulytics</a><a class="ed-home-secondary ed-home-cta-large" href="#ai">Generate with Edulytics AI</a></div></div>
            <div class="ed-home-v5-story-visual" aria-label="Teacher assessment and analytics workspace"><svg viewBox="0 0 720 430" role="img"><rect width="720" height="430" fill="#f6f2ff"/><rect x="58" y="48" width="604" height="332" rx="28" fill="#fff" stroke="#ddd3f8" stroke-width="3"/><text x="88" y="92" font-size="17" font-weight="900" fill="#6e47ed">Generate with Edulytics AI</text><rect x="88" y="120" width="250" height="70" rx="16" fill="#eef5ff"/><text x="110" y="149" font-size="13" fill="#65758f">SCOPE</text><text x="110" y="176" font-size="20" font-weight="900" fill="#10244a">Lesson · Algebra</text><rect x="366" y="120" width="120" height="70" rx="16" fill="#fff4e9"/><text x="389" y="149" font-size="13" fill="#65758f">QUESTIONS</text><text x="389" y="177" font-size="24" font-weight="900" fill="#10244a">20</text><rect x="510" y="120" width="122" height="70" rx="16" fill="#effaf5"/><text x="533" y="149" font-size="13" fill="#65758f">LEVEL</text><text x="533" y="177" font-size="20" font-weight="900" fill="#10244a">Stage 7</text><rect x="88" y="222" width="544" height="96" rx="18" fill="#f8faff"/><circle cx="120" cy="270" r="19" fill="#2f66e8"/><text x="115" y="277" font-size="18" font-weight="900" fill="#fff">1</text><text x="156" y="257" font-size="13" fill="#65758f">GENERATED QUESTION</text><text x="156" y="291" font-size="27" font-weight="900" fill="#10244a">2(x + 3) = 14</text></svg><span class="ed-home-v5-story-chip one">Review • edit • approve</span><span class="ed-home-v5-story-chip two">Teacher remains in control</span></div>
        </div>`;

    const ribbon = root.querySelector('.ed-home-commercial-ribbon');
    const anchor = ribbon || audience;
    anchor.insertAdjacentElement('afterend', student);
    student.insertAdjacentElement('afterend', teacher);
})();
