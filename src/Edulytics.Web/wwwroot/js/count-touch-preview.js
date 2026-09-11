(() => {
    'use strict';

    const root = document.querySelector('[data-count-touch-game]');
    if (!root) return;

    const expectedLessonCode = 'PED:CAMBRIDGE-INTL-MATH:S1:L01';
    if (root.dataset.lessonCode !== expectedLessonCode) {
        root.innerHTML = '<div class="cg-preview-error">This game profile is not configured for the selected lesson.</div>';
        return;
    }

    const activity = window.EdulyticsGameActivities?.['count-touch-and-check-v1'];
    const engine = window.EdulyticsCountingGroveV1;
    if (!activity || !engine) {
        root.innerHTML = '<div class="cg-preview-error">Counting Grove could not start. Refresh and try again.</div>';
        return;
    }

    try {
        window.__countTouchPreviewRuntime = engine.mount(root, activity, {
            preview: true,
            lessonLanguage: root.dataset.lessonLanguage || activity.lessonLanguage
        });
    } catch (error) {
        console.error('Counting Grove preview failed.', error);
        root.innerHTML = '<div class="cg-preview-error">Counting Grove could not start. Refresh and try again.</div>';
    }
})();
