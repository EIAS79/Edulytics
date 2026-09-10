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

        panels.forEach(panel => {
            panel.hidden = panel.dataset.previewPanel !== name;
        });

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
            script.addEventListener('load', () => {
                script.dataset.loaded = 'true';
                resolve();
            }, { once: true });
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

    async function ensureGame() {
        if (runtime || loading) return loading;
        loading = (async () => {
            loadStyle('/css/edulytics-game-engine.css?v=0.1.0');
            await loadScript('/js/game/edulytics-game-engine.js?v=0.1.0');
            await loadScript('/js/game/activities/join-groups-to-add.activity.js?v=0.1.0');

            const engine = window.EdulyticsGameEngine;
            const activity = window.EdulyticsGameActivities?.['join-groups-to-add'];
            if (!engine || !activity) throw new Error('Edulytics game engine failed to load.');

            runtime = engine.mount(gameShell, activity, {
                preview: true,
                onEvent(event) {
                    gameShell.dispatchEvent(new CustomEvent('edulytics:game-event', { detail: event }));
                }
            });
        })().catch(error => {
            console.error(error);
            gameShell.innerHTML = '<div style="padding:32px;background:#fff;border-radius:20px"><strong>Game preview could not start.</strong><p>Please refresh and try again.</p></div>';
            loading = null;
        });
        return loading;
    }

    tabs.forEach(tab => {
        tab.addEventListener('click', () => activateTab(tab.dataset.previewTab));
    });

    const initial = tabs.find(tab => tab.classList.contains('is-active'))?.dataset.previewTab || 'lesson';
    activateTab(initial);
})();