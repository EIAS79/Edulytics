(function (global) {
    'use strict';

    const VERSION = '0.2.0';
    const registry = new Map();

    function registerInteraction(type, adapter) {
        if (!type || !adapter || typeof adapter.mount !== 'function') {
            throw new Error('EdulyticsGameEngineV2: invalid interaction adapter.');
        }
        registry.set(type, adapter);
    }

    function clamp(value, min, max) {
        return Math.min(max, Math.max(min, value));
    }

    function shuffle(items) {
        const copy = items.slice();
        for (let i = copy.length - 1; i > 0; i -= 1) {
            const j = Math.floor(Math.random() * (i + 1));
            [copy[i], copy[j]] = [copy[j], copy[i]];
        }
        return copy;
    }

    function buildRounds(config) {
        const rules = config.generator || {};
        if (rules.type !== 'addition-pairs') {
            throw new Error(`EdulyticsGameEngineV2: unsupported generator "${rules.type}".`);
        }

        const pairs = [];
        for (let left = rules.minOperand ?? 1; left <= (rules.maxOperand ?? 6); left += 1) {
            for (let right = rules.minOperand ?? 1; right <= (rules.maxOperand ?? 6); right += 1) {
                const sum = left + right;
                if (sum < (rules.minSum ?? 2) || sum > (rules.maxSum ?? 12)) continue;
                pairs.push({ left, right, sum });
            }
        }

        if (!pairs.length) throw new Error('EdulyticsGameEngineV2: generator produced no rounds.');

        const desired = config.progression?.rounds ?? 10;
        const pool = shuffle(pairs);
        const rounds = [];
        while (rounds.length < desired && pool.length) rounds.push(pool.shift());
        while (rounds.length < desired) rounds.push(pairs[rounds.length % pairs.length]);
        return rounds;
    }

    function safeFirstName(value) {
        if (typeof value !== 'string') return '';
        const trimmed = value.trim();
        if (!trimmed || trimmed.includes('@')) return '';
        const first = trimmed.split(/\s+/u)[0] || '';
        return first.slice(0, 40);
    }

    function childVoiceScore(voice, language, kind) {
        const name = (voice.name || '').toLowerCase();
        const lang = (voice.lang || '').toLowerCase();
        const requested = (language || '').toLowerCase();
        const base = requested.split('-')[0];
        let score = 0;

        if (lang === requested) score += 90;
        else if (base && lang.startsWith(base)) score += 62;
        else score -= 130;

        const childMarkers = ['child', 'kid', 'young', 'junior', 'teen', 'girl', 'boy', 'ana'];
        if (childMarkers.some(marker => name.includes(marker))) score += 150;

        const neuralMarkers = ['natural', 'neural', 'online', 'premium', 'enhanced'];
        if (neuralMarkers.some(marker => name.includes(marker))) score += 28;

        const girlMarkers = ['girl', 'female', 'ana', 'jenny', 'aria', 'zira', 'sonia', 'libby', 'susan', 'hazel', 'zofia', 'agnieszka'];
        const boyMarkers = ['boy', 'male', 'ryan', 'george', 'guy', 'david', 'mark', 'marek', 'adam', 'kamil'];
        if (kind === 'girl') {
            if (girlMarkers.some(marker => name.includes(marker))) score += 48;
            if (boyMarkers.some(marker => name.includes(marker))) score -= 24;
        } else {
            if (boyMarkers.some(marker => name.includes(marker))) score += 48;
            if (girlMarkers.some(marker => name.includes(marker))) score -= 24;
        }

        if (voice.localService) score += 5;
        return score;
    }

    function studentSvg(compact) {
        const cls = compact ? 'edu2-student-svg is-compact' : 'edu2-student-svg';
        return `<svg class="${cls}" viewBox="0 0 180 280" role="img" aria-label="Student character">
            <defs>
                <linearGradient id="edu2Skin" x1="0" y1="0" x2="0" y2="1"><stop offset="0" stop-color="#f6c39a"/><stop offset="1" stop-color="#dda170"/></linearGradient>
                <linearGradient id="edu2Hoodie" x1="0" y1="0" x2="1" y2="1"><stop offset="0" stop-color="#26b6bd"/><stop offset="1" stop-color="#14879c"/></linearGradient>
                <linearGradient id="edu2Bag" x1="0" y1="0" x2="1" y2="1"><stop offset="0" stop-color="#ffb45f"/><stop offset="1" stop-color="#f47d48"/></linearGradient>
            </defs>
            <ellipse cx="91" cy="264" rx="55" ry="11" fill="rgba(20,55,70,.16)"/>
            <g class="student-body-group">
                <path d="M61 184 C57 212 56 235 58 253 L79 253 L86 203 Z" fill="#344a72"/>
                <path d="M98 203 L105 253 L126 253 C128 231 125 207 120 183 Z" fill="#2e4269"/>
                <path d="M52 250 Q68 246 82 251 L82 263 Q59 269 45 260 Q46 254 52 250Z" fill="#f1f4f7" stroke="#263853" stroke-width="3"/>
                <path d="M104 251 Q120 246 134 252 L137 261 Q118 269 99 263 L99 254Z" fill="#f1f4f7" stroke="#263853" stroke-width="3"/>
                <path d="M47 132 C50 110 67 99 90 99 C116 99 133 111 136 135 L129 194 C118 204 104 209 89 208 C73 208 59 202 50 193 Z" fill="url(#edu2Hoodie)" stroke="#0d6979" stroke-width="4"/>
                <path d="M65 108 Q89 128 115 108 Q111 95 90 94 Q70 95 65 108Z" fill="#0d6f80" opacity=".78"/>
                <path d="M81 120 L99 120 L105 164 Q91 174 76 164 Z" fill="#f4f8fa" opacity=".95"/>
                <path d="M85 130 L95 130 L101 159 Q90 165 80 159 Z" fill="#ffd75e"/>
                <path d="M50 136 Q36 150 31 179 Q30 190 39 194 Q48 196 51 185 L62 151Z" fill="url(#edu2Skin)" stroke="#ba7f58" stroke-width="3" class="student-arm-left"/>
                <path d="M132 135 Q146 146 150 171 Q154 182 146 187 Q137 190 133 179 L121 151Z" fill="url(#edu2Skin)" stroke="#ba7f58" stroke-width="3" class="student-arm-right"/>
                <path d="M42 116 Q33 139 39 161" fill="none" stroke="url(#edu2Bag)" stroke-width="11" stroke-linecap="round"/>
                <path d="M134 113 Q144 137 141 160" fill="none" stroke="url(#edu2Bag)" stroke-width="11" stroke-linecap="round"/>
            </g>
            <g class="student-head-group">
                <rect x="80" y="89" width="20" height="23" rx="9" fill="url(#edu2Skin)"/>
                <ellipse cx="90" cy="63" rx="47" ry="45" fill="url(#edu2Skin)" stroke="#b77c56" stroke-width="3"/>
                <ellipse cx="43" cy="68" rx="9" ry="13" fill="#e7aa7c"/><ellipse cx="137" cy="68" rx="9" ry="13" fill="#e7aa7c"/>
                <path d="M47 54 Q48 13 87 10 Q121 8 137 38 Q127 30 118 30 Q117 43 109 50 Q105 34 96 30 Q89 44 73 49 Q77 33 67 28 Q59 41 47 54Z" fill="#3b2b2d"/>
                <path d="M48 52 Q52 29 72 22" fill="none" stroke="#50363a" stroke-width="7" stroke-linecap="round"/>
                <path d="M108 22 Q128 31 135 48" fill="none" stroke="#50363a" stroke-width="7" stroke-linecap="round"/>
                <path d="M61 52 Q70 47 79 52" fill="none" stroke="#6f483e" stroke-width="3" stroke-linecap="round"/>
                <path d="M101 52 Q110 47 119 52" fill="none" stroke="#6f483e" stroke-width="3" stroke-linecap="round"/>
                <ellipse cx="71" cy="64" rx="7" ry="9" fill="#26384b"/><ellipse cx="109" cy="64" rx="7" ry="9" fill="#26384b"/>
                <circle cx="73" cy="61" r="2" fill="#fff"/><circle cx="111" cy="61" r="2" fill="#fff"/>
                <path d="M90 65 Q87 72 90 75" fill="none" stroke="#b97863" stroke-width="2.5" stroke-linecap="round"/>
                <path class="student-mouth" d="M72 82 Q90 96 109 82 Q104 103 90 104 Q76 102 72 82Z" fill="#7f3f45"/>
                <path d="M78 85 Q90 91 103 85" fill="none" stroke="#fff" stroke-width="5" stroke-linecap="round"/>
                <circle cx="57" cy="80" r="7" fill="#ed9b8d" opacity=".5"/><circle cx="124" cy="80" r="7" fill="#ed9b8d" opacity=".5"/>
            </g>
        </svg>`;
    }

    class GameRuntime {
        constructor(host, config, options) {
            this.host = host;
            this.config = config;
            this.options = options || {};
            this.state = {
                language: config.defaultLanguage || 'en',
                voiceKind: 'girl',
                soundOn: true,
                narrationOn: true,
                roundIndex: 0,
                phase: 'intro',
                dialogueIndex: 0,
                wrongAttempts: 0,
                xp: 0,
                stars: 0,
                startedAt: Date.now(),
                roundStartedAt: null
            };
            this.studentFirstName = safeFirstName(this.options.studentFirstName || host.dataset.studentFirstName || config.studentFirstName || '');
            this.rounds = buildRounds(config);
            this.currentSpeech = '';
            this.preferredVoice = null;
            this.audioContext = null;
            this.interactionCleanup = null;
            this.telemetry = [];
            this.boundVoiceRefresh = () => this.refreshVoice();
        }

        locale() {
            return this.config.locales?.[this.state.language] || this.config.locales?.en || {};
        }

        studentName() {
            return this.studentFirstName || this.locale().studentFallback || 'Student';
        }

        context(round) {
            return { studentName: this.studentName(), round, runtime: this };
        }

        resolveText(value, round) {
            return typeof value === 'function' ? value(round, this.context(round)) : (value || '');
        }

        mount() {
            this.host.classList.add('edu2-game-host');
            this.preloadAssets();
            this.render();
            this.bindControls();
            this.refreshVoice();
            global.speechSynthesis?.addEventListener?.('voiceschanged', this.boundVoiceRefresh);
            this.startDialogue();
            this.emit('activity_started', { engineVersion: VERSION, preview: Boolean(this.options.preview) });
            return this;
        }

        destroy() {
            this.stopSpeech();
            this.interactionCleanup?.();
            global.speechSynthesis?.removeEventListener?.('voiceschanged', this.boundVoiceRefresh);
            this.host.classList.remove('edu2-game-host');
            this.host.replaceChildren();
        }

        preloadAssets() {
            Object.values(this.config.assets || {}).forEach(src => {
                if (typeof src !== 'string' || !src) return;
                const img = new Image();
                img.decoding = 'async';
                img.src = src;
            });
        }

        emit(type, detail) {
            const event = {
                type,
                activityId: this.config.activityId,
                lessonCode: this.config.curriculum?.lessonCode,
                learningOutcomeCodes: this.config.curriculum?.learningOutcomeCodes || [],
                round: this.state.roundIndex + 1,
                timestamp: new Date().toISOString(),
                ...detail
            };
            this.telemetry.push(event);
            this.options.onEvent?.(event);
        }

        render() {
            this.host.innerHTML = `<div class="edu2-game" data-edu2-game lang="${this.state.language}" dir="ltr">
                <header class="edu2-hud">
                    <div class="edu2-brand"><span class="edu2-logo" aria-hidden="true">E</span><div class="edu2-brand-copy"><strong>Edulytics</strong><small data-ui="activityTitle"></small></div></div>
                    <div class="edu2-hud-center"><div class="edu2-progress" aria-label="Activity progress"><span data-ui="progress"></span></div><span class="edu2-round" data-ui="round"></span></div>
                    <div class="edu2-controls">
                        <select class="edu2-select" data-action="language" aria-label="Language"><option value="en">EN</option><option value="pl">PL</option><option value="ar">AR</option></select>
                        <button class="edu2-control" data-action="voice" type="button" aria-label="Switch child voice"><span data-ui="voiceIcon">👧</span><span class="edu2-control-text" data-ui="voiceWord"></span></button>
                        <button class="edu2-control is-icon" data-action="sound" type="button" aria-label="Sound">🔊</button>
                        <button class="edu2-control is-icon" data-action="fullscreen" type="button" aria-label="Full screen">⛶</button>
                    </div>
                </header>
                <main class="edu2-stage">
                    <section class="edu2-mission-card"><small data-ui="missionLabel"></small><h2 data-ui="missionTitle"></h2><p data-ui="missionInstruction"></p></section>
                    <section class="edu2-world-grid" data-ui="worldGrid">
                        <div class="edu2-lane edu2-student-lane"><div class="edu2-student is-listening" data-ui="student">${studentSvg(false)}</div></div>
                        <div class="edu2-lane edu2-source-lane" data-ui="leftLane"></div>
                        <div class="edu2-lane edu2-center-lane" data-ui="centerLane"></div>
                        <div class="edu2-lane edu2-source-lane" data-ui="rightLane"></div>
                        <div class="edu2-lane edu2-guide-lane"><button class="edu2-guide" data-ui="guide" data-action="replay" type="button" aria-label="Replay guide"><img src="${this.config.assets?.guide || ''}" alt="${this.config.characters?.guide?.name || 'Edulytics guide'}" /></button></div>
                    </section>
                    <section class="edu2-answer-zone" data-ui="answerZone" aria-live="polite"><div class="edu2-question"><small data-ui="questionLabel"></small><strong class="edu2-equation" data-ui="equation"></strong><p data-ui="questionText"></p></div><div data-ui="answerArea"></div></section>
                    <section class="edu2-dialogue" data-ui="dialogue" aria-live="polite">
                        <div class="edu2-avatar" data-ui="avatar"></div>
                        <div class="edu2-dialogue-copy"><strong data-ui="speaker"></strong><p data-ui="dialogueText"></p><small class="edu2-hint" data-ui="hint"></small></div>
                        <div class="edu2-dialogue-actions"><button class="edu2-btn secondary" data-action="skip" type="button"></button><button class="edu2-btn primary" data-action="continue" type="button"></button></div>
                    </section>
                    <div class="edu2-celebration" data-ui="celebration" aria-hidden="true"></div>
                </main>
            </div>`;

            this.el = this.host.querySelector('[data-edu2-game]');
            this.student = this.el.querySelector('[data-ui="student"]');
            this.guide = this.el.querySelector('[data-ui="guide"]');
            this.leftLane = this.el.querySelector('[data-ui="leftLane"]');
            this.centerLane = this.el.querySelector('[data-ui="centerLane"]');
            this.rightLane = this.el.querySelector('[data-ui="rightLane"]');
            this.answerArea = this.el.querySelector('[data-ui="answerArea"]');
            this.celebrationLayer = this.el.querySelector('[data-ui="celebration"]');
            this.updateStaticCopy();
            this.setDirection();
        }

        bindControls() {
            this.el.querySelector('[data-action="language"]').addEventListener('change', event => {
                this.state.language = event.target.value;
                this.refreshVoice();
                this.setDirection();
                this.updateStaticCopy();
                if (this.state.phase === 'intro') this.showDialogue();
                else if (this.state.phase === 'task' || this.state.phase === 'answer') this.renderRound(true);
                else if (this.state.phase === 'complete') this.renderComplete();
                this.emit('language_changed', { language: this.state.language });
            });

            this.el.querySelector('[data-action="voice"]').addEventListener('click', () => {
                this.state.voiceKind = this.state.voiceKind === 'girl' ? 'boy' : 'girl';
                this.refreshVoice();
                this.updateStaticCopy();
                if (this.currentSpeech) this.speak(this.currentSpeech);
                this.emit('voice_changed', { voiceKind: this.state.voiceKind, voiceName: this.preferredVoice?.name || null });
            });

            this.el.querySelector('[data-action="sound"]').addEventListener('click', event => {
                this.state.soundOn = !this.state.soundOn;
                this.state.narrationOn = this.state.soundOn;
                event.currentTarget.textContent = this.state.soundOn ? '🔊' : '🔇';
                if (!this.state.soundOn) this.stopSpeech();
            });

            this.el.querySelector('[data-action="fullscreen"]').addEventListener('click', async () => {
                try {
                    if (!document.fullscreenElement) await this.host.requestFullscreen?.();
                    else await document.exitFullscreen?.();
                } catch (_) { }
            });

            this.el.querySelector('[data-action="continue"]').addEventListener('click', () => this.advanceDialogue());
            this.el.querySelector('[data-action="skip"]').addEventListener('click', () => this.beginRound());
            this.guide.addEventListener('click', () => { if (this.currentSpeech) this.speak(this.currentSpeech); });
        }

        setDirection() {
            const rtl = this.state.language === 'ar';
            this.el.dir = rtl ? 'rtl' : 'ltr';
            this.el.lang = this.state.language;
        }

        updateStaticCopy() {
            const locale = this.locale();
            this.el.querySelector('[data-ui="activityTitle"]').textContent = locale.activityTitle || this.config.title || '';
            this.el.querySelector('[data-ui="round"]').textContent = locale.round?.(this.state.roundIndex + 1, this.rounds.length) || `${this.state.roundIndex + 1}/${this.rounds.length}`;
            this.el.querySelector('[data-ui="progress"]').style.width = `${(this.state.roundIndex / this.rounds.length) * 100}%`;
            this.el.querySelector('[data-ui="voiceIcon"]').textContent = this.state.voiceKind === 'girl' ? '👧' : '👦';
            this.el.querySelector('[data-ui="voiceWord"]').textContent = this.state.voiceKind === 'girl' ? (locale.girlVoiceWord || 'Girl') : (locale.boyVoiceWord || 'Boy');
            this.el.querySelector('[data-action="continue"]').textContent = locale.continue || 'Continue';
            this.el.querySelector('[data-action="skip"]').textContent = locale.skip || 'Skip';
            this.el.querySelector('[data-action="language"]').value = this.state.language;
        }

        refreshVoice() {
            if (!global.speechSynthesis) return;
            const requested = this.config.voiceLocales?.[this.state.language] || 'en-GB';
            const voices = global.speechSynthesis.getVoices();
            this.preferredVoice = voices
                .map(voice => ({ voice, score: childVoiceScore(voice, requested, this.state.voiceKind) }))
                .sort((a, b) => b.score - a.score)[0]?.voice || null;
        }

        speak(text) {
            this.currentSpeech = text || '';
            if (!this.state.soundOn || !this.state.narrationOn || !text || !global.speechSynthesis) return;
            global.speechSynthesis.cancel();
            const utterance = new SpeechSynthesisUtterance(text);
            const locale = this.config.voiceLocales?.[this.state.language];
            if (locale) utterance.lang = locale;
            if (this.preferredVoice) utterance.voice = this.preferredVoice;
            utterance.rate = this.state.voiceKind === 'girl' ? 0.95 : 0.92;
            utterance.pitch = this.state.voiceKind === 'girl' ? 1.42 : 1.3;
            utterance.volume = 1;
            utterance.onstart = () => this.guide.classList.add('is-speaking');
            utterance.onend = utterance.onerror = () => this.guide.classList.remove('is-speaking');
            global.speechSynthesis.speak(utterance);
        }

        stopSpeech() {
            global.speechSynthesis?.cancel();
            this.guide?.classList.remove('is-speaking');
        }

        tone(kind) {
            if (!this.state.soundOn) return;
            const AudioCtx = global.AudioContext || global.webkitAudioContext;
            if (!AudioCtx) return;
            this.audioContext ||= new AudioCtx();
            const ctx = this.audioContext;
            const now = ctx.currentTime;
            const notes = kind === 'correct' ? [523.25, 659.25, 783.99] : kind === 'collect' ? [620] : [260, 220];
            notes.forEach((frequency, index) => {
                const osc = ctx.createOscillator();
                const gain = ctx.createGain();
                osc.type = kind === 'wrong' ? 'triangle' : 'sine';
                osc.frequency.value = frequency;
                gain.gain.setValueAtTime(0.0001, now + index * 0.07);
                gain.gain.exponentialRampToValueAtTime(0.11, now + 0.02 + index * 0.07);
                gain.gain.exponentialRampToValueAtTime(0.0001, now + 0.18 + index * 0.07);
                osc.connect(gain).connect(ctx.destination);
                osc.start(now + index * 0.07);
                osc.stop(now + 0.22 + index * 0.07);
            });
        }

        setMission(round) {
            const locale = this.locale();
            this.el.querySelector('[data-ui="missionLabel"]').textContent = locale.missionLabel || 'Mission';
            this.el.querySelector('[data-ui="missionTitle"]').textContent = locale.taskTitle || '';
            this.el.querySelector('[data-ui="missionInstruction"]').textContent = this.resolveText(locale.taskInstruction, round);
        }

        setQuestion(round, ready) {
            const locale = this.locale();
            this.el.querySelector('[data-ui="questionLabel"]').textContent = ready ? (locale.questionLabel || 'Solve to continue') : (locale.collectLabel || 'First step');
            this.el.querySelector('[data-ui="equation"]').textContent = ready ? `${round.left} + ${round.right} = ?` : `${round.left} + ${round.right}`;
            this.el.querySelector('[data-ui="questionText"]').textContent = ready ? this.resolveText(locale.answerInstruction, round) : (locale.collectPrompt || 'Bring both groups into the lantern.');
            if (!ready) {
                this.answerArea.innerHTML = `<div class="edu2-answer-placeholder">${locale.answerLocked || 'The answer stones appear when both groups are together.'}</div>`;
            }
        }

        setDialogue(speakerKey, text, hint) {
            const locale = this.locale();
            const speaker = this.config.characters?.[speakerKey] || {};
            const speakerName = speakerKey === 'student' ? this.studentName() : (speaker.name || speakerKey || '');
            this.el.querySelector('[data-ui="speaker"]').textContent = speakerName;
            this.el.querySelector('[data-ui="dialogueText"]').textContent = text || '';
            this.el.querySelector('[data-ui="hint"]').textContent = hint || '';
            const avatar = this.el.querySelector('[data-ui="avatar"]');
            avatar.replaceChildren();
            if (speakerKey === 'guide' && this.config.assets?.guide) {
                const img = document.createElement('img');
                img.src = this.config.assets.guide;
                img.alt = '';
                avatar.appendChild(img);
                this.student.classList.add('is-listening');
            } else {
                avatar.innerHTML = studentSvg(true);
                this.student.classList.remove('is-listening');
            }
            if (locale.directionHint) avatar.setAttribute('title', locale.directionHint);
        }

        startDialogue() {
            this.state.phase = 'intro';
            this.state.dialogueIndex = 0;
            this.leftLane.replaceChildren();
            this.centerLane.replaceChildren();
            this.rightLane.replaceChildren();
            const locale = this.locale();
            this.setMission({ left: 0, right: 0, sum: 0 });
            this.el.querySelector('[data-ui="missionTitle"]').textContent = locale.introTitle || locale.activityTitle || this.config.title;
            this.el.querySelector('[data-ui="missionInstruction"]').textContent = locale.introSubtitle || '';
            this.el.querySelector('[data-ui="questionLabel"]').textContent = locale.readyLabel || 'Ready?';
            this.el.querySelector('[data-ui="equation"]').textContent = '✦';
            this.el.querySelector('[data-ui="questionText"]').textContent = locale.readyPrompt || '';
            this.answerArea.innerHTML = `<div class="edu2-answer-placeholder">${locale.readyAnswer || 'Listen to Eddy, then start the adventure.'}</div>`;
            this.showDialogue();
        }

        showDialogue() {
            const locale = this.locale();
            const sequence = locale.introDialogue || [];
            const item = sequence[this.state.dialogueIndex] || sequence[sequence.length - 1];
            if (!item) return this.beginRound();
            const text = typeof item.text === 'function' ? item.text(this.context(null)) : item.text;
            this.setDialogue(item.speaker, text, '');
            const continueButton = this.el.querySelector('[data-action="continue"]');
            const skipButton = this.el.querySelector('[data-action="skip"]');
            continueButton.hidden = false;
            skipButton.hidden = false;
            continueButton.textContent = this.state.dialogueIndex >= sequence.length - 1 ? (locale.start || 'Start') : (locale.continue || 'Continue');
            skipButton.textContent = locale.skip || 'Skip';
            this.speak(text);
        }

        advanceDialogue() {
            const sequence = this.locale().introDialogue || [];
            if (this.state.dialogueIndex >= sequence.length - 1) return this.beginRound();
            this.state.dialogueIndex += 1;
            this.showDialogue();
        }

        beginRound() {
            this.stopSpeech();
            this.el.querySelector('[data-action="skip"]').hidden = true;
            this.el.querySelector('[data-action="continue"]').hidden = true;
            this.state.phase = 'task';
            this.state.wrongAttempts = 0;
            this.state.roundStartedAt = Date.now();
            this.renderRound(false);
        }

        renderRound(fromLanguageChange) {
            this.interactionCleanup?.();
            this.leftLane.replaceChildren();
            this.centerLane.replaceChildren();
            this.rightLane.replaceChildren();
            this.answerArea.replaceChildren();
            const round = this.rounds[this.state.roundIndex];
            this.updateStaticCopy();
            this.setMission(round);
            this.setQuestion(round, false);
            const adapter = registry.get(this.config.interaction?.type);
            if (!adapter) throw new Error(`EdulyticsGameEngineV2: no adapter for "${this.config.interaction?.type}".`);
            const locale = this.locale();
            const context = {
                runtime: this,
                round,
                locale,
                config: this.config,
                leftLane: this.leftLane,
                centerLane: this.centerLane,
                rightLane: this.rightLane,
                answerArea: this.answerArea,
                onCollected: detail => { this.tone('collect'); this.emit('object_collected', detail); },
                onReadyForAnswer: () => {
                    this.state.phase = 'answer';
                    this.setQuestion(round, true);
                    const text = this.resolveText(locale.answerPrompt, round);
                    this.setDialogue('guide', text, '');
                    this.student.classList.add('is-thinking');
                    this.speak(text);
                    this.emit('interaction_completed', { left: round.left, right: round.right });
                },
                onAnswer: (value, element) => this.handleAnswer(value, element)
            };
            this.interactionCleanup = adapter.mount(context) || null;
            if (!fromLanguageChange) {
                const intro = this.resolveText(locale.roundIntro, round);
                this.setDialogue('guide', intro, '');
                this.student.classList.remove('is-thinking');
                this.student.classList.add('is-listening');
                this.speak(intro);
                this.emit('round_started', { left: round.left, right: round.right, sum: round.sum });
            }
        }

        handleAnswer(value, element) {
            if (this.state.phase !== 'answer') return;
            const round = this.rounds[this.state.roundIndex];
            const locale = this.locale();
            const correct = Number(value) === round.sum;
            this.emit('answer_submitted', { answer: Number(value), correct, attempt: this.state.wrongAttempts + 1 });

            if (!correct) {
                this.state.wrongAttempts += 1;
                element?.classList.add('is-wrong');
                setTimeout(() => element?.classList.remove('is-wrong'), 650);
                this.tone('wrong');
                const hints = locale.hints || [];
                const index = clamp(this.state.wrongAttempts - 1, 0, Math.max(0, hints.length - 1));
                const hint = typeof hints[index] === 'function' ? hints[index](round, this.context(round)) : hints[index];
                const message = locale.tryAgain || 'Try again.';
                this.setDialogue('guide', message, hint || '');
                this.student.classList.add('is-thinking');
                this.speak(`${message} ${hint || ''}`.trim());
                this.pulseLantern();
                this.emit('hint_used', { level: this.state.wrongAttempts });
                return;
            }

            this.state.phase = 'feedback';
            element?.classList.add('is-correct');
            this.state.xp += this.config.rewards?.xpPerRound ?? 10;
            this.state.stars += this.config.rewards?.starsPerRound ?? 1;
            this.student.classList.remove('is-thinking');
            this.tone('correct');
            this.celebrate();
            const message = this.resolveText(locale.correct, round) || 'Excellent!';
            this.setDialogue('student', message, `+${this.config.rewards?.xpPerRound ?? 10} XP`);
            this.speak(message);
            this.emit('round_completed', { correct: true, wrongAttempts: this.state.wrongAttempts, elapsedMs: Date.now() - this.state.roundStartedAt });
            this.student.classList.add('is-crossing');
            setTimeout(() => {
                this.student.classList.remove('is-crossing');
                this.nextRound();
            }, 1650);
        }

        pulseLantern() {
            Array.from(this.centerLane.querySelectorAll('.edu2-lantern-light')).forEach((light, index) => {
                light.style.animationDelay = `${index * 0.1}s`;
                light.classList.remove('is-counting');
                requestAnimationFrame(() => light.classList.add('is-counting'));
            });
        }

        celebrate() {
            this.guide.classList.add('is-celebrating');
            this.student.classList.add('is-celebrating');
            this.celebrationLayer.innerHTML = Array.from({ length: 22 }, (_, index) => `<i style="--i:${index};--x:${Math.random() * 100}%;--delay:${Math.random() * 0.35}s"></i>`).join('');
            setTimeout(() => {
                this.guide.classList.remove('is-celebrating');
                this.student.classList.remove('is-celebrating');
                this.celebrationLayer.replaceChildren();
            }, 1500);
        }

        nextRound() {
            if (this.state.roundIndex >= this.rounds.length - 1) return this.renderComplete();
            this.state.roundIndex += 1;
            this.state.phase = 'task';
            this.state.wrongAttempts = 0;
            this.renderRound(false);
        }

        renderComplete() {
            this.state.phase = 'complete';
            this.interactionCleanup?.();
            this.leftLane.replaceChildren();
            this.rightLane.replaceChildren();
            const locale = this.locale();
            this.centerLane.innerHTML = `<section class="edu2-finish"><div class="stars" aria-hidden="true">★ ★ ★</div><h2 data-finish-title></h2><p data-finish-copy></p><div class="edu2-rewards"><span class="edu2-reward">⭐ ${this.state.stars}</span><span class="edu2-reward">⚡ ${this.state.xp} XP</span></div><button class="edu2-btn primary" type="button" data-play-again></button></section>`;
            this.centerLane.querySelector('[data-finish-title]').textContent = locale.complete || 'Adventure complete!';
            this.centerLane.querySelector('[data-finish-copy]').textContent = this.resolveText(locale.completeCopy, null);
            this.centerLane.querySelector('[data-play-again]').textContent = locale.playAgain || 'Play again';
            this.centerLane.querySelector('[data-play-again]').addEventListener('click', () => {
                this.rounds = buildRounds(this.config);
                this.state.roundIndex = 0;
                this.state.xp = 0;
                this.state.stars = 0;
                this.state.startedAt = Date.now();
                this.startDialogue();
            });
            this.el.querySelector('[data-ui="missionLabel"]').textContent = locale.completeLabel || 'Completed';
            this.el.querySelector('[data-ui="missionTitle"]').textContent = locale.complete || 'Adventure complete!';
            this.el.querySelector('[data-ui="missionInstruction"]').textContent = '';
            this.el.querySelector('[data-ui="questionLabel"]').textContent = locale.rewardLabel || 'Rewards';
            this.el.querySelector('[data-ui="equation"]').textContent = '★';
            this.el.querySelector('[data-ui="questionText"]').textContent = '';
            this.answerArea.innerHTML = `<div class="edu2-answer-placeholder">${locale.completeAnswer || 'Great work. Your adventure is complete.'}</div>`;
            this.setDialogue('guide', locale.completeSpeech || locale.complete || '', '');
            this.speak(locale.completeSpeech || locale.complete || '');
            this.el.querySelector('[data-ui="progress"]').style.width = '100%';
            this.emit('activity_completed', { xp: this.state.xp, stars: this.state.stars, elapsedMs: Date.now() - this.state.startedAt, preview: Boolean(this.options.preview) });
        }
    }

    registerInteraction('grouping', {
        mount(context) {
            const { round, locale, leftLane, centerLane, rightLane, answerArea, onCollected, onReadyForAnswer, onAnswer } = context;
            const total = round.left + round.right;
            let collected = 0;
            let ready = false;
            let dragGhost = null;
            let draggingButton = null;
            let dragged = false;
            const moved = new Set();
            const cleanups = [];

            leftLane.innerHTML = `<div class="edu2-source"><span class="edu2-sign">${round.left}</span><div class="edu2-bush" data-source="left"></div><span class="edu2-source-caption">${locale.leftGroup || 'Sunset group'}</span></div>`;
            rightLane.innerHTML = `<div class="edu2-source"><span class="edu2-sign">${round.right}</span><div class="edu2-bush" data-source="right"></div><span class="edu2-source-caption">${locale.rightGroup || 'Moonlight group'}</span></div>`;
            centerLane.innerHTML = `<div class="edu2-lantern-zone" data-drop-zone><div class="edu2-lantern"><span class="edu2-lantern-handle"></span><div class="edu2-lantern-glass" data-lights></div><span class="edu2-lantern-base"></span></div><strong class="edu2-count" data-count>0 / ${total}</strong><span class="edu2-drop-copy">${locale.dropHere || 'Bring them here'}</span></div>`;

            const left = leftLane.querySelector('[data-source="left"]');
            const right = rightLane.querySelector('[data-source="right"]');
            const dropZone = centerLane.querySelector('[data-drop-zone]');
            const lights = centerLane.querySelector('[data-lights]');
            const countLabel = centerLane.querySelector('[data-count]');

            function addLight(side) {
                const light = document.createElement('span');
                light.className = 'edu2-lantern-light';
                const index = collected - 1;
                const columns = 3;
                const row = Math.floor(index / columns);
                const col = index % columns;
                light.style.setProperty('--lx', `${27 + col * 23}%`);
                light.style.setProperty('--ly', `${24 + row * 24}%`);
                if (side === 'right') light.style.filter = 'hue-rotate(42deg)';
                lights.appendChild(light);
                requestAnimationFrame(() => light.classList.add('is-counting'));
            }

            function finishCollectionIfReady() {
                countLabel.textContent = `${collected} / ${total}`;
                if (collected < total || ready) return;
                ready = true;
                dropZone.classList.add('is-bright');
                renderChoices();
                onReadyForAnswer();
            }

            function animateToLantern(button, side) {
                if (!button || moved.has(button.dataset.fireflyId)) return;
                moved.add(button.dataset.fireflyId);
                const from = button.getBoundingClientRect();
                const to = dropZone.getBoundingClientRect();
                const flying = document.createElement('span');
                flying.className = `edu2-flying-firefly${side === 'right' ? ' is-purple' : ''}`;
                flying.style.left = `${from.left + from.width / 2}px`;
                flying.style.top = `${from.top + from.height / 2}px`;
                document.body.appendChild(flying);
                button.disabled = true;
                button.style.opacity = '0';
                const dx = to.left + to.width / 2 - (from.left + from.width / 2);
                const dy = to.top + to.height * 0.56 - (from.top + from.height / 2);
                requestAnimationFrame(() => {
                    flying.style.transform = `translate(${dx}px,${dy}px) scale(.62)`;
                    flying.style.opacity = '.25';
                });
                setTimeout(() => {
                    flying.remove();
                    button.remove();
                    collected += 1;
                    addLight(side);
                    onCollected({ side, collected, total });
                    finishCollectionIfReady();
                }, 390);
            }

            function makeFireflies(container, count, side) {
                const positions = [
                    [27,35],[53,28],[73,43],[38,58],[63,64],[22,70],[80,68],[48,80]
                ];
                for (let index = 0; index < count; index += 1) {
                    const button = document.createElement('button');
                    button.type = 'button';
                    button.className = `edu2-firefly${side === 'right' ? ' is-purple' : ''}`;
                    button.dataset.fireflyId = `${side}-${index}`;
                    button.dataset.side = side;
                    const pos = positions[index % positions.length];
                    button.style.setProperty('--x', `${pos[0]}%`);
                    button.style.setProperty('--y', `${pos[1]}%`);
                    button.style.setProperty('--delay', `${-(index * 0.23)}s`);
                    button.setAttribute('aria-label', locale.fireflyLabel || 'Move firefly to lantern');
                    button.addEventListener('click', () => {
                        if (dragged) { dragged = false; return; }
                        animateToLantern(button, side);
                    });
                    button.addEventListener('pointerdown', event => {
                        if (button.disabled || ready) return;
                        draggingButton = button;
                        dragged = false;
                        button.setPointerCapture?.(event.pointerId);
                        button.classList.add('is-dragging');
                        dragGhost = document.createElement('span');
                        dragGhost.className = `edu2-drag-ghost${side === 'right' ? ' is-purple' : ''}`;
                        dragGhost.style.left = `${event.clientX}px`;
                        dragGhost.style.top = `${event.clientY}px`;
                        document.body.appendChild(dragGhost);
                    });
                    button.addEventListener('pointermove', event => {
                        if (draggingButton !== button || !dragGhost) return;
                        dragged = true;
                        dragGhost.style.left = `${event.clientX}px`;
                        dragGhost.style.top = `${event.clientY}px`;
                        const rect = dropZone.getBoundingClientRect();
                        const inside = event.clientX >= rect.left && event.clientX <= rect.right && event.clientY >= rect.top && event.clientY <= rect.bottom;
                        dropZone.classList.toggle('is-drop-target', inside);
                    });
                    const endDrag = event => {
                        if (draggingButton !== button) return;
                        const rect = dropZone.getBoundingClientRect();
                        const inside = event.clientX >= rect.left && event.clientX <= rect.right && event.clientY >= rect.top && event.clientY <= rect.bottom;
                        button.classList.remove('is-dragging');
                        dropZone.classList.remove('is-drop-target');
                        dragGhost?.remove();
                        dragGhost = null;
                        draggingButton = null;
                        if (inside) animateToLantern(button, side);
                    };
                    button.addEventListener('pointerup', endDrag);
                    button.addEventListener('pointercancel', event => {
                        button.classList.remove('is-dragging');
                        dropZone.classList.remove('is-drop-target');
                        dragGhost?.remove();
                        dragGhost = null;
                        draggingButton = null;
                        dragged = false;
                    });
                    container.appendChild(button);
                }
            }

            function makeChoices() {
                const candidates = shuffle([round.sum, round.sum - 1, round.sum + 1, round.sum + 2, round.sum - 2, round.sum + 3]);
                const values = [];
                for (const value of candidates) {
                    if (value < 1 || value > 12 || values.includes(value)) continue;
                    values.push(value);
                    if (values.length === 4) break;
                }
                let fallback = 1;
                while (values.length < 4) {
                    if (!values.includes(fallback)) values.push(fallback);
                    fallback += 1;
                }
                return shuffle(values);
            }

            function renderChoices() {
                answerArea.replaceChildren();
                const stones = document.createElement('div');
                stones.className = 'edu2-stones';
                makeChoices().forEach(value => {
                    const stone = document.createElement('button');
                    stone.type = 'button';
                    stone.className = 'edu2-stone';
                    stone.textContent = value;
                    stone.setAttribute('aria-label', `${locale.answerChoiceLabel || 'Answer'} ${value}`);
                    stone.addEventListener('click', () => onAnswer(value, stone));
                    stones.appendChild(stone);
                });
                answerArea.appendChild(stones);
            }

            makeFireflies(left, round.left, 'left');
            makeFireflies(right, round.right, 'right');

            return () => {
                cleanups.forEach(fn => fn());
                dragGhost?.remove();
                document.querySelectorAll('.edu2-flying-firefly').forEach(node => node.remove());
            };
        }
    });

    global.EdulyticsGameEngineV2 = {
        version: VERSION,
        registerInteraction,
        mount(host, config, options) {
            if (!(host instanceof Element)) throw new Error('EdulyticsGameEngineV2: host element is required.');
            if (!config?.activityId) throw new Error('EdulyticsGameEngineV2: activity configuration is required.');
            return new GameRuntime(host, config, options).mount();
        }
    };
}(window));