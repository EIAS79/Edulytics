(() => {
    const root = document.querySelector('.ed-home');
    const hero = document.querySelector('.ed-home-hero');
    const copy = document.querySelector('.ed-home-hero-copy');
    if (!root || !hero || !copy) return;

    const h1 = copy.querySelector('h1');
    const body = copy.querySelector(':scope > p');
    const kicker = copy.querySelector('.ed-home-kicker');
    const checks = copy.querySelector('.ed-home-hero-checks');
    const secondary = copy.querySelector('.ed-home-secondary');
    if (!h1 || !body || !kicker || !checks) return;

    const pl = (document.documentElement.lang || '').toLowerCase().startsWith('pl');
    const slides = pl ? [
        {
            kicker: 'PROGRAM • OCENIANIE • AI • ANALITYKA',
            title: 'Jedna platforma. Cały proces nauczania matematyki.',
            body: 'Edulytics łączy program nauczania, treści lekcji, ocenianie, praktykę ucznia i analitykę — tak aby szkoła widziała nie tylko wynik, ale także drogę do niego.',
            checks: ['Programy nauczania w jednym miejscu', 'AI pod kontrolą nauczyciela', 'Postęp ucznia widoczny w czasie'],
            visualClass: 'is-slide-platform'
        },
        {
            kicker: 'GENERATE WITH EDULYTICS AI',
            title: 'Twórz lepsze oceny. Szybciej. Z pełną kontrolą nauczyciela.',
            body: 'Nauczyciel może generować pytania dopasowane do lekcji, jednostki i celów nauczania, zatwierdzać je przed publikacją i zachować pełną kontrolę nad oceną.',
            checks: ['Pytania powiązane z programem', 'Zatwierdzanie przez nauczyciela', 'Do 50 pytań na generację'],
            visualClass: 'is-slide-assessment'
        },
        {
            kicker: 'UCZEŃ • PRAKTYKA • MASTERY',
            title: 'Każdy uczeń dostaje jasny następny krok.',
            body: 'Uczeń widzi lekcje, wyniki i postęp, a prywatna praktyka AI pomaga ćwiczyć słabsze obszary bez mieszania jej z oficjalną oceną szkolną.',
            checks: ['Prywatna praktyka AI', 'Wyniki i mastery', 'Bezpieczne oddzielenie od ocen oficjalnych'],
            visualClass: 'is-slide-student'
        }
    ] : [
        {
            kicker: 'CURRICULUM • ASSESSMENT • AI • ANALYTICS',
            title: 'One platform. The complete mathematics learning workflow.',
            body: 'Edulytics connects curriculum, lesson content, assessment, student practice and analytics so schools can see not only the score, but the learning journey behind it.',
            checks: ['Curricula in one place', 'Teacher-controlled AI', 'Student progress made visible'],
            visualClass: 'is-slide-platform'
        },
        {
            kicker: 'GENERATE WITH EDULYTICS AI',
            title: 'Build better assessments. Faster. With the teacher in control.',
            body: 'Teachers can generate questions aligned to lessons, units and learning outcomes, review them before publishing and keep full control over formal assessment.',
            checks: ['Curriculum-linked questions', 'Teacher approval before publishing', 'Up to 50 questions per generation'],
            visualClass: 'is-slide-assessment'
        },
        {
            kicker: 'STUDENT • PRACTICE • MASTERY',
            title: 'Give every student a clear next step.',
            body: 'Students see lessons, results and progress, while private AI practice supports weaker areas without mixing practice activity into official school grades.',
            checks: ['Private AI practice', 'Results and mastery', 'Separated from official grading'],
            visualClass: 'is-slide-student'
        }
    ];

    const controls = document.createElement('div');
    controls.className = 'ed-home-slider-controls';
    controls.setAttribute('aria-label', pl ? 'Sterowanie slajdami' : 'Hero slides');
    controls.innerHTML = `
        <button type="button" class="ed-home-slide-arrow ed-home-slide-prev" aria-label="${pl ? 'Poprzedni slajd' : 'Previous slide'}">‹</button>
        <div class="ed-home-slide-dots" role="tablist"></div>
        <button type="button" class="ed-home-slide-arrow ed-home-slide-next" aria-label="${pl ? 'Następny slajd' : 'Next slide'}">›</button>`;
    hero.appendChild(controls);

    const dots = controls.querySelector('.ed-home-slide-dots');
    slides.forEach((_, index) => {
        const dot = document.createElement('button');
        dot.type = 'button';
        dot.className = 'ed-home-slide-dot';
        dot.setAttribute('role', 'tab');
        dot.setAttribute('aria-label', `${pl ? 'Slajd' : 'Slide'} ${index + 1}`);
        dot.addEventListener('click', () => go(index, true));
        dots.appendChild(dot);
    });

    let current = 0;
    let timer = null;
    const reduceMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;

    function go(index, manual = false) {
        current = (index + slides.length) % slides.length;
        const slide = slides[current];
        hero.classList.remove('is-slide-platform', 'is-slide-assessment', 'is-slide-student');
        hero.classList.add(slide.visualClass);
        kicker.textContent = slide.kicker;
        h1.textContent = slide.title;
        body.textContent = slide.body;
        checks.innerHTML = slide.checks.map(item => `<span>${item}</span>`).join('');
        [...dots.children].forEach((dot, i) => {
            dot.classList.toggle('is-active', i === current);
            dot.setAttribute('aria-selected', i === current ? 'true' : 'false');
        });
        if (secondary) {
            secondary.textContent = current === 1
                ? (pl ? 'Zobacz ocenianie' : 'Explore assessment')
                : current === 2
                    ? (pl ? 'Zobacz doświadczenie ucznia' : 'Explore student experience')
                    : (pl ? 'Zobacz platformę' : 'Explore the platform');
            secondary.setAttribute('href', current === 1 ? '#ai' : current === 2 ? '#students' : '#platform');
        }
        if (manual) restart();
    }

    function restart() {
        if (timer) window.clearInterval(timer);
        if (!reduceMotion) timer = window.setInterval(() => go(current + 1), 7000);
    }

    controls.querySelector('.ed-home-slide-prev').addEventListener('click', () => go(current - 1, true));
    controls.querySelector('.ed-home-slide-next').addEventListener('click', () => go(current + 1, true));
    hero.addEventListener('mouseenter', () => timer && window.clearInterval(timer));
    hero.addEventListener('mouseleave', restart);

    document.querySelectorAll('.ed-home-mobile-panel a').forEach(link => {
        link.addEventListener('click', () => {
            const details = link.closest('details');
            if (details) details.open = false;
        });
    });

    go(0);
    restart();
})();
