(function (global) {
    'use strict';

    const VERSION = '0.1.0';
    const registry = new Map();

    function registerInteraction(type, adapter) {
        if (!type || !adapter || typeof adapter.mount !== 'function') {
            throw new Error('EdulyticsGameEngine: invalid interaction adapter.');
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
            throw new Error(`EdulyticsGameEngine: unsupported generator "${rules.type}".`);
        }

        const pairs = [];
        for (let left = rules.minOperand ?? 1; left <= (rules.maxOperand ?? 6); left += 1) {
            for (let right = rules.minOperand ?? 1; right <= (rules.maxOperand ?? 6); right += 1) {
                const sum = left + right;
                if (sum < (rules.minSum ?? 2) || sum > (rules.maxSum ?? 12)) continue;
                pairs.push({ left, right, sum });
            }
        }

        const desired = config.progression?.rounds ?? 10;
        const pool = shuffle(pairs);
        const rounds = [];
        while (rounds.length < desired && pool.length) rounds.push(pool.shift());
        while (rounds.length < desired) rounds.push(pairs[rounds.length % pairs.length]);
        return rounds;
    }

    function getLocale(config, language) {
        return config.locales?.[language] || config.locales?.en || {};
    }

    function childVoiceScore(voice, language, kind) {
        const name = (voice.name || '').toLowerCase();
        const lang = (voice.lang || '').toLowerCase();
        const requested = language.toLowerCase();
        let score = 0;

        if (lang === requested) score += 80;
        else if (lang.startsWith(requested.split('-')[0])) score += 55;
        else score -= 120;

        const childMarkers = ['child', 'kid', 'young', 'junior', 'teen', 'girl', 'boy', 'ana'];
        if (childMarkers.some(marker => name.includes(marker))) score += 120;

        const neuralMarkers = ['natural', 'neural', 'online', 'premium', 'enhanced'];
        if (neuralMarkers.some(marker => name.includes(marker))) score += 24;

        const girlMarkers = ['girl', 'female', 'ana', 'jenny', 'aria', 'zira', 'sonia', 'libby', 'susan', 'hazel', 'zofia', 'agnieszka'];
        const boyMarkers = ['boy', 'male', 'ryan', 'george', 'guy', 'david', 'mark', 'marek', 'adam', 'kamil'];

        if (kind === 'girl') {
            if (girlMarkers.some(marker => name.includes(marker))) score += 42;
            if (boyMarkers.some(marker => name.includes(marker))) score -= 20;
        } else {
            if (boyMarkers.some(marker => name.includes(marker))) score += 42;
            if (girlMarkers.some(marker => name.includes(marker))) score -= 20;
        }

        if (voice.localService) score += 5;
        return score;
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
            this.rounds = buildRounds(config);
            this.currentSpeech = '';
            this.audioContext = null;
            this.preferredVoice = null;
            this.interactionCleanup = null;
            this.telemetry = [];
            this.boundVoiceRefresh = () => this.refreshVoice();
        }

        mount() {
            this.preloadAssets();
            this.render();
            this.bindGlobalControls();
            this.refreshVoice();
            if ('speechSynthesis' in global) {
                global.speechSynthesis.addEventListener?.('voiceschanged', this.boundVoiceRefresh);
            }
            this.startDialogue();
            this.emit('activity_started');
            return this;
        }

        destroy() {
            this.stopSpeech();
            this.interactionCleanup?.();
            if ('speechSynthesis' in global) {
                global.speechSynthesis.removeEventListener?.('voiceschanged', this.boundVoiceRefresh);
            }
            this.host.replaceChildren();
        }

        preloadAssets() {
            const assets = this.config.assets || {};
            Object.values(assets).forEach(src => {
                if (typeof src !== 'string' || !src) return;
                const image = new Image();
                image.decoding = 'async';
                image.src = src;
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
            this.host.classList.add('edu-game-host');
            this.host.innerHTML = `
                <div class="edu-game" data-edu-game lang="${this.state.language}" dir="ltr">
                    <div class="edu-world" aria-hidden="true">
                        <div class="edu-sky-orb"></div>
                        <div class="edu-cloud edu-cloud-a"></div>
                        <div class="edu-cloud edu-cloud-b"></div>
                        <div class="edu-hill edu-hill-a"></div>
                        <div class="edu-hill edu-hill-b"></div>
                        <div class="edu-stream"></div>
                        <div class="edu-grass"></div>
                        <div class="edu-particles"></div>
                    </div>

                    <header class="edu-hud">
                        <div class="edu-brand">
                            <span class="edu-logo-mark" aria-hidden="true">E</span>
                            <div>
                                <strong>Edulytics</strong>
                                <small data-ui="activityTitle"></small>
                            </div>
                        </div>
                        <div class="edu-hud-center">
                            <div class="edu-progress" aria-label="Activity progress"><span data-ui="progress"></span></div>
                            <span class="edu-round" data-ui="round"></span>
                        </div>
                        <div class="edu-controls">
                            <select class="edu-select" data-action="language" aria-label="Language">
                                <option value="en">EN</option>
                                <option value="pl">PL</option>
                                <option value="ar">AR</option>
                            </select>
                            <button class="edu-chip" type="button" data-action="voice" aria-label="Switch child voice"><span data-ui="voiceLabel"></span></button>
                            <button class="edu-icon" type="button" data-action="sound" aria-label="Sound">🔊</button>
                            <button class="edu-icon" type="button" data-action="fullscreen" aria-label="Full screen">⛶</button>
                        </div>
                    </header>

                    <main class="edu-scene" data-ui="scene">
                        <section class="edu-play-layer" data-ui="playLayer"></section>

                        <div class="edu-character edu-student" data-ui="student" aria-label="Student character">
                            <div class="edu-student-hair"></div>
                            <div class="edu-student-head"><span class="edu-eye left"></span><span class="edu-eye right"></span><span class="edu-smile"></span></div>
                            <div class="edu-student-body"><span class="edu-star">★</span></div>
                            <div class="edu-student-leg left"></div><div class="edu-student-leg right"></div>
                        </div>

                        <button class="edu-character edu-guide" data-ui="guide" data-action="replay" type="button" aria-label="Replay guide">
                            <span class="edu-guide-wave" aria-hidden="true"></span>
                            <img src="${this.config.assets?.guide || ''}" alt="${this.config.characters?.guide?.name || 'Edulytics guide'}" />
                        </button>

                        <div class="edu-celebration" data-ui="celebration" aria-hidden="true"></div>

                        <section class="edu-dialogue" data-ui="dialogue" aria-live="polite">
                            <div class="edu-avatar" data-ui="avatar"></div>
                            <div class="edu-dialogue-copy">
                                <strong data-ui="speaker"></strong>
                                <p data-ui="dialogueText"></p>
                                <small data-ui="hintLevel"></small>
                            </div>
                            <div class="edu-dialogue-actions">
                                <button class="edu-secondary" type="button" data-action="skip"></button>
                                <button class="edu-primary" type="button" data-action="continue"></button>
                            </div>
                        </section>
                    </main>
                </div>`;

            this.el = this.host.querySelector('[data-edu-game]');
            this.playLayer = this.el.querySelector('[data-ui="playLayer"]');
            this.dialogue = this.el.querySelector('[data-ui="dialogue"]');
            this.guide = this.el.querySelector('[data-ui="guide"]');
            this.student = this.el.querySelector('[data-ui="student"]');
            this.celebration = this.el.querySelector('[data-ui="celebration"]');
            this.updateStaticCopy();
            this.setDirection();
        }

        bindGlobalControls() {
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
            this.el.querySelector('[data-action="replay"]').addEventListener('click', () => {
                if (this.currentSpeech) this.speak(this.currentSpeech);
            });
        }

        setDirection() {
            const isRtl = this.state.language === 'ar';
            this.el.dir = isRtl ? 'rtl' : 'ltr';
            this.el.lang = this.state.language;
        }

        updateStaticCopy() {
            const locale = getLocale(this.config, this.state.language);
            this.el.querySelector('[data-ui="activityTitle"]').textContent = locale.activityTitle || this.config.title;
            this.el.querySelector('[data-ui="round"]').textContent = locale.round?.(this.state.roundIndex + 1, this.rounds.length) || `${this.state.roundIndex + 1}/${this.rounds.length}`;
            this.el.querySelector('[data-ui="progress"]').style.width = `${(this.state.roundIndex / this.rounds.length) * 100}%`;
            const voiceText = this.state.voiceKind === 'girl' ? (locale.girlVoice || '👧 Girl voice') : (locale.boyVoice || '👦 Boy voice');
            this.el.querySelector('[data-ui="voiceLabel"]').textContent = voiceText;
            this.el.querySelector('[data-action="continue"]').textContent = locale.continue || 'Continue';
            this.el.querySelector('[data-action="skip"]').textContent = locale.skip || 'Skip';
            this.el.querySelector('[data-action="language"]').value = this.state.language;
        }

        refreshVoice() {
            if (!('speechSynthesis' in global)) return;
            const locale = this.config.voiceLocales?.[this.state.language] || `${this.state.language}-${this.state.language.toUpperCase()}`;
            const voices = global.speechSynthesis.getVoices();
            this.preferredVoice = voices.map(voice => ({ voice, score: childVoiceScore(voice, locale, this.state.voiceKind) })).sort((a, b) => b.score - a.score)[0]?.voice || null;
        }

        speak(text) {
            this.currentSpeech = text || '';
            if (!this.state.soundOn || !this.state.narrationOn || !text || !('speechSynthesis' in global)) return;
            global.speechSynthesis.cancel();
            const utterance = new SpeechSynthesisUtterance(text);
            const locale = this.config.voiceLocales?.[this.state.language];
            if (locale) utterance.lang = locale;
            if (this.preferredVoice) utterance.voice = this.preferredVoice;
            utterance.rate = this.state.voiceKind === 'girl' ? 0.96 : 0.93;
            utterance.pitch = this.state.voiceKind === 'girl' ? 1.38 : 1.28;
            utterance.volume = 1;
            utterance.onstart = () => this.guide.classList.add('is-speaking');
            utterance.onend = utterance.onerror = () => this.guide.classList.remove('is-speaking');
            global.speechSynthesis.speak(utterance);
        }

        stopSpeech() {
            if ('speechSynthesis' in global) global.speechSynthesis.cancel();
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
                gain.gain.exponentialRampToValueAtTime(0.12, now + 0.02 + index * 0.07);
                gain.gain.exponentialRampToValueAtTime(0.0001, now + 0.18 + index * 0.07);
                osc.connect(gain).connect(ctx.destination);
                osc.start(now + index * 0.07);
                osc.stop(now + 0.22 + index * 0.07);
            });
        }

        startDialogue() {
            this.state.phase = 'intro';
            this.state.dialogueIndex = 0;
            this.dialogue.classList.remove('is-compact');
            this.showDialogue();
        }

        showDialogue() {
            const locale = getLocale(this.config, this.state.language);
            const sequence = locale.introDialogue || [];
            const item = sequence[this.state.dialogueIndex] || sequence[sequence.length - 1];
            if (!item) return this.beginRound();
            this.setDialogue(item.speaker, item.text, '');
            this.el.querySelector('[data-action="continue"]').hidden = false;
            this.el.querySelector('[data-action="skip"]').hidden = false;
            this.el.querySelector('[data-action="continue"]').textContent = this.state.dialogueIndex >= sequence.length - 1 ? (locale.start || 'Start') : (locale.continue || 'Continue');
            this.speak(item.text);
        }

        advanceDialogue() {
            const locale = getLocale(this.config, this.state.language);
            const sequence = locale.introDialogue || [];
            if (this.state.dialogueIndex >= sequence.length - 1) return this.beginRound();
            this.state.dialogueIndex += 1;
            this.showDialogue();
        }

        setDialogue(speakerKey, text, hintText) {
            const speaker = this.config.characters?.[speakerKey] || {};
            this.el.querySelector('[data-ui="speaker"]').textContent = speaker.name || speakerKey || '';
            this.el.querySelector('[data-ui="dialogueText"]').textContent = text || '';
            this.el.querySelector('[data-ui="hintLevel"]').textContent = hintText || '';
            const avatar = this.el.querySelector('[data-ui="avatar"]');
            avatar.replaceChildren();
            if (speakerKey === 'guide' && this.config.assets?.guide) {
                const img = document.createElement('img');
                img.src = this.config.assets.guide;
                img.alt = '';
                avatar.appendChild(img);
            } else {
                avatar.textContent = speaker.avatar || '★';
            }
        }

        beginRound() {
            this.stopSpeech();
            this.state.phase = 'task';
            this.state.wrongAttempts = 0;
            this.state.roundStartedAt = Date.now();
            this.dialogue.classList.add('is-compact');
            this.el.querySelector('[data-action="skip"]').hidden = true;
            this.el.querySelector('[data-action="continue"]').hidden = true;
            this.guide.classList.add('is-entering');
            setTimeout(() => this.guide.classList.remove('is-entering'), 800);
            this.renderRound(false);
        }

        renderRound(fromLanguageChange) {
            this.interactionCleanup?.();
            this.playLayer.replaceChildren();
            this.updateStaticCopy();
            const adapter = registry.get(this.config.interaction?.type);
            if (!adapter) throw new Error(`EdulyticsGameEngine: no adapter for "${this.config.interaction?.type}".`);
            const round = this.rounds[this.state.roundIndex];
            const locale = getLocale(this.config, this.state.language);
            const context = {
                runtime: this,
                host: this.playLayer,
                round,
                locale,
                config: this.config,
                onCollected: () => { this.tone('collect'); this.emit('object_collected'); },
                onReadyForAnswer: () => {
                    this.state.phase = 'answer';
                    const text = locale.answerPrompt?.(round) || '';
                    this.setDialogue('guide', text, '');
                    this.speak(text);
                    this.emit('interaction_completed', { left: round.left, right: round.right });
                },
                onAnswer: (value, element) => this.handleAnswer(value, element)
            };
            this.interactionCleanup = adapter.mount(context) || null;
            if (!fromLanguageChange) {
                const intro = locale.roundIntro?.(round) || '';
                this.setDialogue('guide', intro, '');
                this.speak(intro);
                this.emit('round_started', { left: round.left, right: round.right, sum: round.sum });
            }
        }

        handleAnswer(value, element) {
            if (this.state.phase !== 'answer') return;
            const round = this.rounds[this.state.roundIndex];
            const locale = getLocale(this.config, this.state.language);
            const correct = Number(value) === round.sum;
            this.emit('answer_submitted', { answer: Number(value), correct, attempt: this.state.wrongAttempts + 1 });

            if (!correct) {
                this.state.wrongAttempts += 1;
                element?.classList.add('is-wrong');
                setTimeout(() => element?.classList.remove('is-wrong'), 700);
                this.tone('wrong');
                const hints = locale.hints || [];
                const hintIndex = clamp(this.state.wrongAttempts - 1, 0, Math.max(0, hints.length - 1));
                const hint = typeof hints[hintIndex] === 'function' ? hints[hintIndex](round) : hints[hintIndex];
                const message = locale.tryAgain || 'Try again.';
                this.setDialogue('guide', message, hint || '');
                this.speak(`${message} ${hint || ''}`.trim());
                this.pulseLantern();
                this.emit('hint_used', { level: this.state.wrongAttempts });
                return;
            }

            element?.classList.add('is-correct');
            this.state.phase = 'feedback';
            this.state.xp += this.config.rewards?.xpPerRound ?? 10;
            this.state.stars += this.config.rewards?.starsPerRound ?? 1;
            this.tone('correct');
            this.celebrate();
            const message = locale.correct?.(round) || 'Excellent!';
            this.setDialogue('student', message, `+${this.config.rewards?.xpPerRound ?? 10} XP`);
            this.speak(message);
            this.emit('round_completed', { correct: true, wrongAttempts: this.state.wrongAttempts, elapsedMs: Date.now() - this.state.roundStartedAt });
            this.student.classList.add('is-crossing');
            setTimeout(() => {
                this.student.classList.remove('is-crossing');
                this.nextRound();
            }, 1900);
        }

        pulseLantern() {
            const lights = Array.from(this.playLayer.querySelectorAll('.edu-lantern-light'));
            lights.forEach((light, index) => {
                light.style.animationDelay = `${index * 0.12}s`;
                light.classList.remove('is-counting');
                requestAnimationFrame(() => light.classList.add('is-counting'));
            });
        }

        celebrate() {
            this.guide.classList.add('is-celebrating');
            this.student.classList.add('is-celebrating');
            this.celebration.innerHTML = Array.from({ length: 20 }, (_, index) => `<i style="--i:${index};--x:${Math.random() * 100}%;--d:${Math.random() * 0.8}s"></i>`).join('');
            this.celebration.classList.add('is-active');
            setTimeout(() => {
                this.guide.classList.remove('is-celebrating');
                this.student.classList.remove('is-celebrating');
                this.celebration.classList.remove('is-active');
                this.celebration.replaceChildren();
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
            this.playLayer.innerHTML = `<section class="edu-finish-scene"><div class="edu-finish-stars" aria-hidden="true">★ ★ ★</div><h2 data-finish-title></h2><p data-finish-copy></p><div class="edu-reward-row"><strong>⭐ ${this.state.stars}</strong><strong>⚡ ${this.state.xp} XP</strong></div><button class="edu-primary" type="button" data-play-again></button></section>`;
            const locale = getLocale(this.config, this.state.language);
            this.playLayer.querySelector('[data-finish-title]').textContent = locale.complete || 'Adventure complete!';
            this.playLayer.querySelector('[data-finish-copy]').textContent = locale.completeCopy || '';
            this.playLayer.querySelector('[data-play-again]').textContent = locale.playAgain || 'Play again';
            this.playLayer.querySelector('[data-play-again]').addEventListener('click', () => {
                this.rounds = buildRounds(this.config);
                this.state.roundIndex = 0;
                this.state.xp = 0;
                this.state.stars = 0;
                this.startDialogue();
            });
            this.dialogue.classList.add('is-compact');
            this.setDialogue('guide', locale.completeSpeech || locale.complete || '', '');
            this.speak(locale.completeSpeech || locale.complete || '');
            this.el.querySelector('[data-ui="progress"]').style.width = '100%';
            this.emit('activity_completed', { xp: this.state.xp, stars: this.state.stars, elapsedMs: Date.now() - this.state.startedAt, preview: Boolean(this.options.preview) });
        }
    }

    registerInteraction('grouping', {
        mount(context) {
            const { host, round, locale, onCollected, onReadyForAnswer, onAnswer } = context;
            const total = round.left + round.right;
            let collected = 0;
            let answerVisible = false;
            const moved = new Set();

            host.innerHTML = `<section class="edu-task">
                <div class="edu-task-title"><small>${locale.missionLabel || 'Mission'}</small><h2>${locale.taskTitle || 'Bring the groups together'}</h2><p>${locale.taskInstruction?.(round) || ''}</p></div>
                <div class="edu-source edu-source-left"><span class="edu-wood-sign">${round.left}</span><div class="edu-bush" data-source="left"></div></div>
                <div class="edu-lantern-zone" data-drop-zone><div class="edu-lantern"><span class="edu-lantern-handle"></span><div class="edu-lantern-glass" data-lantern-lights></div><span class="edu-lantern-base"></span></div><strong data-joined-count>0 / ${total}</strong></div>
                <div class="edu-source edu-source-right"><span class="edu-wood-sign">${round.right}</span><div class="edu-bush" data-source="right"></div></div>
                <div class="edu-answer-path" data-answer-path hidden><strong>${locale.answerInstruction || 'Choose the stone that opens the path'}</strong><div class="edu-stones" data-stones></div></div>
            </section>`;

            const left = host.querySelector('[data-source="left"]');
            const right = host.querySelector('[data-source="right"]');
            const dropZone = host.querySelector('[data-drop-zone]');
            const lights = host.querySelector('[data-lantern-lights]');
            const countLabel = host.querySelector('[data-joined-count]');
            const answerPath = host.querySelector('[data-answer-path]');
            const stones = host.querySelector('[data-stones]');

            function makeFireflies(container, count, side) {
                for (let index = 0; index < count; index += 1) {
                    const button = document.createElement('button');
                    button.type = 'button';
                    button.className = `edu-firefly ${side === 'right' ? 'is-purple' : ''}`;
                    button.setAttribute('aria-label', locale.fireflyLabel || 'Firefly');
                    button.dataset.id = `${side}-${index}`;
                    button.style.setProperty('--x', `${18 + (index * 29) % 68}%`);
                    button.style.setProperty('--y', `${16 + (index * 37) % 64}%`);
                    button.style.setProperty('--delay', `${-(index * 0.23)}s`);
                    container.appendChild(button);
                    bindDrag(button);
                }
            }

            function bindDrag(button) {
                let startX = 0;
                let startY = 0;
                let movedPx = 0;
                let active = false;
                const reset = () => {
                    button.style.transition = 'transform .25s ease';
                    button.style.transform = '';
                    setTimeout(() => { button.style.transition = ''; }, 280);
                };

                button.addEventListener('pointerdown', event => {
                    if (moved.has(button.dataset.id)) return;
                    active = true;
                    movedPx = 0;
                    startX = event.clientX;
                    startY = event.clientY;
                    button.setPointerCapture?.(event.pointerId);
                    button.classList.add('is-dragging');
                });

                button.addEventListener('pointermove', event => {
                    if (!active) return;
                    const dx = event.clientX - startX;
                    const dy = event.clientY - startY;
                    movedPx = Math.max(movedPx, Math.hypot(dx, dy));
                    button.style.transform = `translate3d(${dx}px,${dy}px,0) scale(1.12)`;
                });

                button.addEventListener('pointerup', event => {
                    if (!active) return;
                    active = false;
                    button.classList.remove('is-dragging');
                    const rect = dropZone.getBoundingClientRect();
                    const inside = event.clientX >= rect.left && event.clientX <= rect.right && event.clientY >= rect.top && event.clientY <= rect.bottom;
                    if (inside) collect(button);
                    else reset();
                });

                button.addEventListener('click', () => {
                    if (movedPx > 6 || moved.has(button.dataset.id)) return;
                    collect(button);
                });
            }

            function collect(button) {
                const id = button.dataset.id;
                if (moved.has(id)) return;
                moved.add(id);
                const from = button.getBoundingClientRect();
                const to = dropZone.getBoundingClientRect();
                const dx = (to.left + to.width / 2) - (from.left + from.width / 2);
                const dy = (to.top + to.height / 2) - (from.top + from.height / 2);
                button.style.transition = 'transform .5s cubic-bezier(.2,.8,.3,1),opacity .45s ease';
                button.style.transform = `translate3d(${dx}px,${dy}px,0) scale(.55)`;
                button.style.opacity = '0';
                setTimeout(() => {
                    button.remove();
                    const light = document.createElement('i');
                    light.className = 'edu-lantern-light';
                    light.style.setProperty('--lx', `${18 + (collected * 31) % 66}%`);
                    light.style.setProperty('--ly', `${15 + (collected * 43) % 70}%`);
                    lights.appendChild(light);
                    collected += 1;
                    countLabel.textContent = `${collected} / ${total}`;
                    dropZone.classList.toggle('is-bright', collected === total);
                    onCollected();
                    if (collected === total && !answerVisible) {
                        answerVisible = true;
                        setTimeout(showAnswers, 550);
                    }
                }, 470);
            }

            function makeChoices() {
                const values = new Set([round.sum]);
                const deltas = shuffle([-3, -2, -1, 1, 2, 3]);
                for (const delta of deltas) {
                    const value = round.sum + delta;
                    if (value >= 1 && value <= 12) values.add(value);
                    if (values.size >= 4) break;
                }
                let candidate = 1;
                while (values.size < 4) {
                    if (candidate !== round.sum) values.add(candidate);
                    candidate += 1;
                }
                return shuffle(Array.from(values));
            }

            function showAnswers() {
                answerPath.hidden = false;
                stones.replaceChildren();
                makeChoices().forEach((value, index) => {
                    const stone = document.createElement('button');
                    stone.type = 'button';
                    stone.className = 'edu-stone';
                    stone.textContent = value;
                    stone.style.setProperty('--stone-index', index);
                    stone.addEventListener('click', () => onAnswer(value, stone));
                    stones.appendChild(stone);
                });
                onReadyForAnswer();
            }

            makeFireflies(left, round.left, 'left');
            makeFireflies(right, round.right, 'right');
            return () => moved.clear();
        }
    });

    global.EdulyticsGameEngine = {
        version: VERSION,
        registerInteraction,
        mount(host, config, options) {
            if (!(host instanceof Element)) throw new Error('EdulyticsGameEngine: host element is required.');
            if (!config?.activityId) throw new Error('EdulyticsGameEngine: activity configuration is required.');
            return new GameRuntime(host, config, options).mount();
        }
    };
}(window));