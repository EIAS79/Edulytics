(function (global) {
    'use strict';

    if (!global.EdulyticsGameEngineV9 || global.__edulyticsV9StoryVoicePatched) return;
    global.__edulyticsV9StoryVoicePatched = true;

    const baseMount = global.EdulyticsGameEngineV9.mount.bind(global.EdulyticsGameEngineV9);

    function targetLanguage(instance) {
        const language = instance?.config?.lessonLanguage;
        if (language === 'pl') return 'pl-PL';
        if (language === 'ar') return 'ar-SA';
        return 'en-GB';
    }

    function voiceScore(voice, targetLang) {
        const name = (voice?.name || '').toLowerCase();
        const lang = (voice?.lang || '').toLowerCase();
        const target = targetLang.toLowerCase();
        const targetBase = target.slice(0, 2);
        let score = 0;

        if (lang === target) score += 45;
        if (lang.startsWith(`${targetBase}-`) || lang === targetBase) score += 65;

        if (/child|young|kid|jenny|aria|libby|sonia|zira|samantha|ava|emma|ana|hazel|susan|heera|zofia|paulina|hoda|salma|female|girl|woman/i.test(name)) score += 90;
        if (/natural|neural|online|enhanced|premium/i.test(name)) score += 24;
        if (/microsoft|google|apple/i.test(name)) score += 8;
        if (voice?.localService) score += 4;

        if (/david|mark|guy|george|daniel|james|ryan|thomas|christopher|male|man/i.test(name)) score -= 140;

        return score;
    }

    function chooseVoice(voices, lang) {
        if (!voices?.length) return null;
        const base = lang.slice(0, 2).toLowerCase();
        const sameLanguage = voices.filter(voice => (voice.lang || '').toLowerCase().startsWith(base));
        const pool = sameLanguage.length ? sameLanguage : voices;
        return [...pool].sort((a, b) => voiceScore(b, lang) - voiceScore(a, lang))[0] || null;
    }

    function speakWithYoungVoice(instance, message) {
        if (!instance?.soundOn || !message || !('speechSynthesis' in global)) return;

        const synthesis = global.speechSynthesis;
        synthesis.cancel();
        let started = false;

        const speakNow = () => {
            if (started || !instance.soundOn) return;
            started = true;

            const lang = targetLanguage(instance);
            const utterance = new SpeechSynthesisUtterance(message);
            const selected = chooseVoice(synthesis.getVoices(), lang);

            if (selected) {
                utterance.voice = selected;
                utterance.lang = selected.lang || lang;
            } else {
                utterance.lang = lang;
            }

            // A lighter, younger delivery while keeping numbers easy to understand.
            utterance.rate = .93;
            utterance.pitch = 1.34;
            utterance.volume = 1;
            synthesis.speak(utterance);
        };

        if (synthesis.getVoices().length) {
            speakNow();
            return;
        }

        const fallbackTimer = global.setTimeout(speakNow, 320);
        synthesis.addEventListener('voiceschanged', () => {
            global.clearTimeout(fallbackTimer);
            speakNow();
        }, { once: true });
    }

    function ensureStoryBeat(instance) {
        if (instance.storyBeat || !instance.game) return;

        const panel = document.createElement('div');
        panel.className = 'lumen9-storybeat';
        panel.setAttribute('aria-live', 'polite');
        panel.setAttribute('aria-hidden', 'true');
        panel.innerHTML = `
            <img class="lumen9-storybeat-avatar" alt="" aria-hidden="true" />
            <div class="lumen9-storybeat-copy">
                <span class="lumen9-storybeat-speaker">EDDY</span>
                <span class="lumen9-storybeat-text"></span>
            </div>`;

        panel.querySelector('.lumen9-storybeat-avatar').src = instance.config.assets.guide;
        instance.game.appendChild(panel);
        instance.storyBeat = panel;
        instance.storyBeatText = panel.querySelector('.lumen9-storybeat-text');
    }

    function showStoryBeat(instance, message, duration) {
        ensureStoryBeat(instance);
        if (!instance.storyBeat || !instance.storyBeatText) return;

        instance.storyBeatText.textContent = message;
        instance.storyBeat.classList.add('show');
        instance.storyBeat.setAttribute('aria-hidden', 'false');
        global.clearTimeout(instance.storyBeatTimer);
        instance.storyBeatTimer = global.setTimeout(() => {
            instance.storyBeat?.classList.remove('show');
            instance.storyBeat?.setAttribute('aria-hidden', 'true');
        }, duration);
    }

    global.EdulyticsGameEngineV9.mount = async function patchedMount(root, config, options) {
        const instance = await baseMount(root, config, options);
        ensureStoryBeat(instance);

        instance.speak = function patchedSpeak(message) {
            speakWithYoungVoice(this, message);
        };

        instance.say = function patchedSay(message) {
            const round = this.currentRound?.();
            let narration = typeof message === 'function' ? message(round) : message;

            if (narration === 'Choose the total.' && round) {
                narration = this.config.copy.question
                    ? this.config.copy.question(round)
                    : `Great! You joined the lights. ${round.a} plus ${round.b} equals what? Choose the total.`;
            }

            if (!narration) return;

            const storyDuration = this.scene === 'collect' ? 7200 : 5600;
            showStoryBeat(this, narration, storyDuration);
            this.showToast?.(narration);
            if (this.soundOn) this.speak(narration);
        };

        const originalDestroy = instance.destroy?.bind(instance);
        instance.destroy = function patchedDestroy() {
            global.clearTimeout(this.storyBeatTimer);
            return originalDestroy?.();
        };

        return instance;
    };
}(window));
