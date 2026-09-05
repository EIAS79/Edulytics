(() => {
    const link = document.getElementById('open-learning-context');
    const scope = document.getElementById('practice-scope');
    const lesson = document.getElementById('practice-lesson');
    const unit = document.getElementById('practice-unit');

    if (!link || !scope || !lesson || !unit) return;

    const baseHref = link.getAttribute('href');
    if (!baseHref) return;

    const guidPattern = /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i;

    const focusIds = select => {
        const raw = select.selectedOptions[0]?.dataset.learningFocusNodeIds ?? '';
        return raw
            .split(',')
            .map(value => value.trim())
            .filter(value => guidPattern.test(value))
            .slice(0, 100);
    };

    const updateLink = () => {
        const url = new URL(baseHref, window.location.origin);
        url.searchParams.delete('focusNodeIds');

        const selectedIds = scope.value === 'Lesson'
            ? focusIds(lesson)
            : scope.value === 'Unit'
                ? focusIds(unit)
                : [];

        selectedIds.forEach(id => url.searchParams.append('focusNodeIds', id));
        url.hash = selectedIds.length > 0
            ? 'selected-learning-node'
            : 'selected-curriculum-context';

        link.setAttribute('href', `${url.pathname}${url.search}${url.hash}`);
    };

    scope.addEventListener('change', updateLink);
    lesson.addEventListener('change', updateLink);
    unit.addEventListener('change', updateLink);
    updateLink();
})();
