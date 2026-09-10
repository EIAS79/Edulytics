(() => {
    'use strict';

    const root = document.querySelector('[data-temporary-practice-preview]');
    if (!root) return;
    const tabs = Array.from(root.querySelectorAll('[data-preview-tab]'));
    const panels = Array.from(root.querySelectorAll('[data-preview-panel]'));
    const gameShell = root.querySelector('.kid-game-shell');
    if (!gameShell) return;
    let runtime = null;
    let loading = null;

    function activateTab(name) {
        tabs.forEach(tab => {
            const active = tab.dataset.previewTab === name;
            tab.classList.toggle('is-active', active);
            tab.setAttribute('aria-selected', active ? 'true' : 'false');
        });
        panels.forEach(panel => { panel.hidden = panel.dataset.previewPanel !== name; });
        if (name === 'practice') ensureGame();
    }

    function loadScript(src) {
        return new Promise((resolve, reject) => {
            const existing = document.querySelector(`script[data-edulytics-game-src="${src}"]`);
            if (existing) {
                if (existing.dataset.loaded === 'true') resolve();
                else existing.addEventListener('load', resolve, { once: true });
                return;
            }
            const script = document.createElement('script');
            script.src = src;
            script.async = false;
            script.dataset.edulyticsGameSrc = src;
            script.addEventListener('load', () => { script.dataset.loaded = 'true'; resolve(); }, { once: true });
            script.addEventListener('error', reject, { once: true });
            document.head.appendChild(script);
        });
    }

    function loadStyle(href) {
        if (document.querySelector(`link[data-edulytics-game-style="${href}"]`)) return;
        const link = document.createElement('link');
        link.rel = 'stylesheet';
        link.href = href;
        link.dataset.edulyticsGameStyle = href;
        document.head.appendChild(link);
    }

    function preloadImages(sources) {
        const unique = [...new Set(sources.filter(Boolean))];
        return Promise.all(unique.map(src => new Promise((resolve, reject) => {
            const image = new Image();
            image.decoding = 'async';
            image.onload = () => resolve(src);
            image.onerror = () => reject(new Error(`Game asset failed to load: ${src}`));
            image.src = src;
            if (image.complete && image.naturalWidth > 0) resolve(src);
        })));
    }

    function studentFirstName() {
        const meta = document.querySelector('meta[name="edulytics-student-first-name"]')?.content || '';
        const raw = gameShell.dataset.studentFirstName || root.dataset.studentFirstName || meta;
        const value = (raw || '').trim();
        if (!value || value.includes('@')) return '';
        return value.split(/\s+/u)[0].slice(0, 40);
    }

    async function mountV7() {
        loadStyle('/css/edulytics-game-experience-v7.css?v=0.7.0');
        await loadScript('/js/game/activities/join-groups-to-add.v7.activity.js?v=0.7.0');
        const activity = window.EdulyticsGameActivities?.['join-groups-to-add-v7'];
        if (!activity) throw new Error('Lantern Isles V7 activity configuration is unavailable.');

        await preloadImages([
            activity.assets.background,
            activity.assets.terrain,
            activity.assets.foreground,
            activity.assets.student,
            activity.assets.guide
        ]);

        await loadScript('/js/game/edulytics-game-v7-renderer.js?v=0.7.0');
        await loadScript('/js/game/edulytics-game-engine-v4.js?v=0.4.0');
        const engine = window.EdulyticsGameEngineV4;
        if (!engine || !window.EdulyticsGameV7Renderer) throw new Error('Lantern Isles V7 game runtime failed to load.');

        return engine.mount(gameShell, activity, {
            preview: true,
            studentFirstName: studentFirstName(),
            lessonLanguage: activity.lessonLanguage,
            onEvent(event) {
                gameShell.dispatchEvent(new CustomEvent('edulytics:game-event', { detail: event }));
            }
        });
    }

    async function mountV4Fallback() {
        loadStyle('/css/edulytics-game-experience-v4.css?v=0.4.0');
        await loadScript('/js/game/edulytics-game-v4-renderer.js?v=0.4.0');
        await loadScript('/js/game/edulytics-game-engine-v4.js?v=0.4.0');
        await loadScript('/js/game/activities/join-groups-to-add.v4.activity.js?v=0.4.0');
        const engine = window.EdulyticsGameEngineV4;
        const activity = window.EdulyticsGameActivities?.['join-groups-to-add-v4'];
        if (!engine || !activity) throw new Error('V4 fallback unavailable.');
        return engine.mount(gameShell, activity, {
            preview: true,
            studentFirstName: studentFirstName(),
            lessonLanguage: activity.lessonLanguage,
            onEvent(event) {
                gameShell.dispatchEvent(new CustomEvent('edulytics:game-event', { detail: event }));
            }
        });
    }

    async function mountV3Fallback() {
        loadStyle('/css/edulytics-game-experience-v3.css?v=0.3.0');
        await loadScript('/js/game/edulytics-game-engine-v3.js?v=0.3.0');
        await loadScript('/js/game/activities/join-groups-to-add.v3.activity.js?v=0.3.0');
        const engine = window.EdulyticsGameEngineV3;
        const activity = window.EdulyticsGameActivities?.['join-groups-to-add-v3'];
        if (!engine || !activity) throw new Error('V3 fallback unavailable.');
        return engine.mount(gameShell, activity, {
            preview: true,
            studentFirstName: studentFirstName(),
            lessonLanguage: activity.lessonLanguage,
            onEvent(event) {
                gameShell.dispatchEvent(new CustomEvent('edulytics:game-event', { detail: event }));
            }
        });
    }

    async function ensureGame() {
        if (runtime || loading) return loading;
        loading = mountV7()
            .then(instance => { runtime = instance; return instance; })
            .catch(async error => {
                console.error('V7 preview failed, falling back to V4.', error);
                try {
                    runtime = await mountV4Fallback();
                    return runtime;
                } catch (v4Error) {
                    console.error('V4 fallback failed, falling back to V3.', v4Error);
                    try {
                        runtime = await mountV3Fallback();
                        return runtime;
                    } catch (v3Error) {
                        console.error(v3Error);
                        gameShell.innerHTML = '<div style="padding:32px;background:#fff;border-radius:20px"><strong>Game preview could not start.</strong><p>Please refresh and try again.</p></div>';
                        loading = null;
                        return null;
                    }
                }
            });
        return loading;
    }

    tabs.forEach(tab => tab.addEventListener('click', () => activateTab(tab.dataset.previewTab)));
    const initial = tabs.find(tab => tab.classList.contains('is-active'))?.dataset.previewTab || 'lesson';
    activateTab(initial);
})();
