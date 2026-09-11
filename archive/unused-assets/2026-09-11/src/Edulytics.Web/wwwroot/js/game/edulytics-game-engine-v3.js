(function (global) {
    'use strict';

    const VERSION = '0.3.0';

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
            throw new Error(`EdulyticsGameEngineV3: unsupported generator "${rules.type}".`);
        }

        const pairs = [];
        for (let left = rules.minOperand ?? 1; left <= (rules.maxOperand ?? 6); left += 1) {
            for (let right = rules.minOperand ?? 1; right <= (rules.maxOperand ?? 6); right += 1) {
                const sum = left + right;
                if (sum < (rules.minSum ?? 2) || sum > (rules.maxSum ?? 12)) continue;
                pairs.push({ left, right, sum });
            }
        }
        if (!pairs.length) throw new Error('EdulyticsGameEngineV3: generator produced no rounds.');

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
        return (trimmed.split(/\s+/u)[0] || '').slice(0, 40);
    }

    function childVoiceScore(voice, locale, kind) {
        const name = (voice.name || '').toLowerCase();
        const lang = (voice.lang || '').toLowerCase();
        const requested = (locale || '').toLowerCase();
        const base = requested.split('-')[0];
        let score = 0;

        if (lang === requested) score += 100;
        else if (base && lang.startsWith(base)) score += 70;
        else score -= 140;

        const childMarkers = ['child', 'kid', 'young', 'junior', 'teen', 'girl', 'boy', 'ana'];
        if (childMarkers.some(marker => name.includes(marker))) score += 160;

        const neuralMarkers = ['natural', 'neural', 'online', 'premium', 'enhanced'];
        if (neuralMarkers.some(marker => name.includes(marker))) score += 28;

        const girlMarkers = ['girl', 'female', 'ana', 'jenny', 'aria', 'zira', 'sonia', 'libby', 'susan', 'hazel', 'zofia', 'agnieszka'];
        const boyMarkers = ['boy', 'male', 'ryan', 'george', 'guy', 'david', 'mark', 'marek', 'adam', 'kamil'];
        if (kind === 'girl') {
            if (girlMarkers.some(marker => name.includes(marker))) score += 50;
            if (boyMarkers.some(marker => name.includes(marker))) score -= 25;
        } else {
            if (boyMarkers.some(marker => name.includes(marker))) score += 50;
            if (girlMarkers.some(marker => name.includes(marker))) score -= 25;
        }
        return score;
    }

    function studentSvg() {
        return `<svg viewBox="0 0 180 280" role="img" aria-label="Student character" class="edu3-student-svg">
            <defs>
                <linearGradient id="edu3Skin" x1="0" y1="0" x2="0" y2="1"><stop offset="0" stop-color="#f6c39a"/><stop offset="1" stop-color="#dda170"/></linearGradient>
                <linearGradient id="edu3Shirt" x1="0" y1="0" x2="1" y2="1"><stop offset="0" stop-color="#31c2c6"/><stop offset="1" stop-color="#0c7f98"/></linearGradient>
            </defs>
            <ellipse cx="90" cy="266" rx="55" ry="10" fill="rgba(16,61,78,.16)"/>
            <g class="edu3-student-body">
                <path d="M55 183 C57 211 55 238 58 255 L79 255 L86 202 Z" fill="#344a72"/>
                <path d="M96 202 L104 255 L126 255 C129 232 126 207 120 182 Z" fill="#2d4169"/>
                <path d="M49 251 Q65 246 81 251 L81 264 Q59 269 44 260 Q44 255 49 251Z" fill="#f5f7f8" stroke="#253a57" stroke-width="3"/>
                <path d="M103 251 Q120 246 136 252 L137 262 Q117 269 98 264 L98 255Z" fill="#f5f7f8" stroke="#253a57" stroke-width="3"/>
                <path d="M47 132 C50 109 67 98 90 98 C116 98 134 111 137 135 L130 194 C118 204 104 209 89 209 C73 209 59 203 49 194 Z" fill="url(#edu3Shirt)" stroke="#075f73" stroke-width="4"/>
                <path d="M78 120 H102 L106 166 Q91 176 75 166 Z" fill="#f7fafb" opacity=".95"/>
                <path d="M84 129 H96 L101 160 Q90 166 79 160 Z" fill="#ffd45b"/>
                <path d="M50 136 Q36 151 31 178 Q29 189 39 194 Q49 196 52 184 L62 151Z" fill="url(#edu3Skin)" stroke="#b97e58" stroke-width="3" class="edu3-arm edu3-arm-left"/>
                <path d="M132 135 Q147 146 151 171 Q154 182 146 188 Q137 190 133 179 L121 151Z" fill="url(#edu3Skin)" stroke="#b97e58" stroke-width="3" class="edu3-arm edu3-arm-right"/>
            </g>
            <g class="edu3-student-head">
                <rect x="80" y="89" width="20" height="22" rx="9" fill="url(#edu3Skin)"/>
                <ellipse cx="90" cy="63" rx="47" ry="45" fill="url(#edu3Skin)" stroke="#b77c56" stroke-width="3"/>
                <ellipse cx="43" cy="68" rx="9" ry="13" fill="#e7aa7c"/><ellipse cx="137" cy="68" rx="9" ry="13" fill="#e7aa7c"/>
                <path d="M46 55 Q47 13 87 10 Q121 8 138 38 Q128 29 118 30 Q117 43 109 50 Q105 34 96 30 Q89 44 73 49 Q77 33 67 28 Q58 42 46 55Z" fill="#37282b"/>
                <path d="M60 52 Q70 47 80 52" fill="none" stroke="#6e493e" stroke-width="3" stroke-linecap="round"/>
                <path d="M100 52 Q110 47 120 52" fill="none" stroke="#6e493e" stroke-width="3" stroke-linecap="round"/>
                <ellipse cx="71" cy="64" rx="7" ry="9" fill="#24384b"/><ellipse cx="109" cy="64" rx="7" ry="9" fill="#24384b"/>
                <circle cx="73" cy="61" r="2" fill="#fff"/><circle cx="111" cy="61" r="2" fill="#fff"/>
                <path d="M90 66 Q87 73 90 75" fill="none" stroke="#b97863" stroke-width="2.5" stroke-linecap="round"/>
                <path class="edu3-student-mouth" d="M73 83 Q90 95 108 83 Q104 101 90 103 Q77 101 73 83Z" fill="#7c4045"/>
                <path d="M79 86 Q90 91 102 86" fill="none" stroke="#fff" stroke-width="5" stroke-linecap="round"/>
                <circle cx="57" cy="80" r="7" fill="#ed9b8d" opacity=".5"/><circle cx="124" cy="80" r="7" fill="#ed9b8d" opacity=".5"/>
            </g>
        </svg>`;
    }

    class GameRuntime {
        constructor(host, config, options) {
            this.host = host;
            this.config = config;
            this.options = options || {};
            this.rounds = buildRounds(config);
            this.studentFirstName = safeFirstName(this.options.studentFirstName || host.dataset.studentFirstName || '');
            this.language = (this.options.lessonLanguage || config.lessonLanguage || 'en').toLowerCase();
            this.voiceLocale = config.voiceLocale || 'en-GB';
            this.state = {
                voiceKind: 'girl', soundOn: true, narrationOn: true,
                roundIndex: 0, phase: 'intro', dialogueIndex: 0,
                wrongAttempts: 0, xp: 0, stars: 0,
                startedAt: Date.now(), roundStartedAt: 0
            };
            this.currentSpeech = '';
            this.preferredVoice = null;
            this.audioContext = null;
            this.dragCleanup = [];
            this.resizeObserver = null;
            this.boundVoiceRefresh = () => this.refreshVoice();
        }

        copy() { return this.config.copy || {}; }
        studentName() { return this.studentFirstName || this.copy().studentFallback || 'Student'; }
        text(value, round) { return typeof value === 'function' ? value(round, { studentName: this.studentName() }) : (value || ''); }

        mount() {
            this.host.classList.add('edu3-host');
            this.host.dataset.lessonLanguage = this.language;
            this.renderShell();
            this.bindControls();
            this.refreshVoice();
            global.speechSynthesis?.addEventListener?.('voiceschanged', this.boundVoiceRefresh);
            if ('ResizeObserver' in global) {
                this.resizeObserver = new ResizeObserver(entries => {
                    const box = entries[0]?.contentRect;
                    if (!box) return;
                    this.el.classList.toggle('edu3-short', box.height < 680);
                    this.el.classList.toggle('edu3-narrow', box.width < 760);
                });
                this.resizeObserver.observe(this.host);
            }
            this.startIntro();
            this.emit('activity_started', { engineVersion: VERSION, lessonLanguage: this.language, preview: Boolean(this.options.preview) });
            return this;
        }

        destroy() {
            this.stopSpeech();
            this.cleanupDragHandlers();
            this.resizeObserver?.disconnect();
            global.speechSynthesis?.removeEventListener?.('voiceschanged', this.boundVoiceRefresh);
            this.host.classList.remove('edu3-host');
            this.host.replaceChildren();
        }

        emit(type, detail) {
            this.options.onEvent?.({
                type,
                activityId: this.config.activityId,
                lessonCode: this.config.curriculum?.lessonCode,
                learningOutcomeCodes: this.config.curriculum?.learningOutcomeCodes || [],
                lessonLanguage: this.language,
                round: this.state.roundIndex + 1,
                timestamp: new Date().toISOString(),
                ...detail
            });
        }

        renderShell() {
            const c = this.copy();
            const guideName = this.config.characters?.guide?.name || 'Eddy';
            this.host.innerHTML = `<div class="edu3-game" data-edu3-game lang="${this.language}" dir="${this.config.direction || 'ltr'}">
                <div class="edu3-backdrop" aria-hidden="true">
                    <div class="edu3-sky-glow"></div><div class="edu3-cloud c1"></div><div class="edu3-cloud c2"></div>
                    <div class="edu3-mountain m1"></div><div class="edu3-mountain m2"></div><div class="edu3-far-hill h1"></div><div class="edu3-far-hill h2"></div>
                    <div class="edu3-river"></div><div class="edu3-foreground"></div><div class="edu3-firefly-dust"></div>
                </div>
                <header class="edu3-hud">
                    <div class="edu3-brand"><span class="edu3-logo">E</span><div><strong>Edulytics</strong><small>${c.activityTitle || this.config.title || ''}</small></div></div>
                    <div class="edu3-progress-wrap"><div class="edu3-progress"><span data-ui="progress"></span></div><b data-ui="round" dir="ltr"></b></div>
                    <div class="edu3-controls">
                        <button class="edu3-chip" data-action="voice" type="button"><span data-ui="voiceIcon">👧</span><span data-ui="voiceLabel">${c.girlVoice || 'Girl voice'}</span></button>
                        <button class="edu3-icon" data-action="sound" type="button" aria-label="${c.sound || 'Sound'}">🔊</button>
                        <button class="edu3-icon" data-action="fullscreen" type="button" aria-label="${c.fullscreen || 'Full screen'}">⛶</button>
                    </div>
                </header>
                <main class="edu3-stage">
                    <section class="edu3-mission" data-ui="mission"><small>${c.missionLabel || 'Mission'}</small><h1 data-ui="missionTitle"></h1><p data-ui="missionInstruction"></p></section>
                    <section class="edu3-world" data-ui="world">
                        <div class="edu3-character-slot edu3-student-slot"><div class="edu3-student is-listening" data-ui="student">${studentSvg()}</div><span class="edu3-nameplate" data-ui="studentName"></span></div>
                        <div class="edu3-source edu3-source-left" data-ui="leftSource"><span class="edu3-sign" data-ui="leftCount"></span><div class="edu3-bush"><div class="edu3-fireflies" data-ui="leftFireflies"></div></div></div>
                        <div class="edu3-crossing">
                            <div class="edu3-lantern-zone" data-ui="lanternZone"><div class="edu3-lantern"><div class="edu3-lantern-handle"></div><div class="edu3-lantern-glass" data-ui="lanternGlass"></div><div class="edu3-lantern-base"></div></div><b class="edu3-count" data-ui="joinedCount" dir="ltr"></b></div>
                            <div class="edu3-bridge" data-ui="bridge" aria-hidden="true">${Array.from({length:8},(_,i)=>`<i style="--i:${i}"></i>`).join('')}</div>
                        </div>
                        <div class="edu3-source edu3-source-right" data-ui="rightSource"><span class="edu3-sign" data-ui="rightCount"></span><div class="edu3-bush"><div class="edu3-fireflies" data-ui="rightFireflies"></div></div></div>
                        <div class="edu3-character-slot edu3-guide-slot"><button class="edu3-guide" data-action="replay" type="button" aria-label="${c.replayGuide || 'Replay guide'}"><span class="edu3-guide-ring"></span><img src="${this.config.assets?.guide || ''}" alt="${guideName}" /></button><span class="edu3-nameplate">${guideName}</span></div>
                        <div class="edu3-world-effects" data-ui="effects" aria-hidden="true"></div>
                    </section>
                    <section class="edu3-answer" data-ui="answer" aria-live="polite">
                        <div class="edu3-answer-copy"><small data-ui="questionLabel"></small><strong data-ui="equation" dir="ltr"></strong><p data-ui="answerInstruction"></p></div>
                        <div class="edu3-stones" data-ui="stones"></div>
                    </section>
                    <section class="edu3-dialogue" data-ui="dialogue" aria-live="polite">
                        <div class="edu3-avatar"><img src="${this.config.assets?.guide || ''}" alt="" data-ui="dialogueAvatar" /></div>
                        <div class="edu3-dialogue-copy"><b data-ui="speaker"></b><p data-ui="dialogueText"></p><small data-ui="hint"></small></div>
                        <div class="edu3-dialogue-actions"><button class="edu3-secondary" data-action="skip" type="button">${c.skip || 'Skip'}</button><button class="edu3-primary" data-action="continue" type="button">${c.continue || 'Continue'}</button></div>
                    </section>
                </main>
            </div>`;

            this.el = this.host.querySelector('[data-edu3-game]');
            this.world = this.el.querySelector('[data-ui="world"]');
            this.student = this.el.querySelector('[data-ui="student"]');
            this.guide = this.el.querySelector('.edu3-guide');
            this.bridge = this.el.querySelector('[data-ui="bridge"]');
            this.answer = this.el.querySelector('[data-ui="answer"]');
            this.dialogue = this.el.querySelector('[data-ui="dialogue"]');
            this.el.querySelector('[data-ui="studentName"]').textContent = this.studentName();
            this.updateProgress();
        }

        bindControls() {
            this.el.querySelector('[data-action="voice"]').addEventListener('click', () => {
                this.state.voiceKind = this.state.voiceKind === 'girl' ? 'boy' : 'girl';
                this.refreshVoice();
                const c = this.copy();
                this.el.querySelector('[data-ui="voiceIcon"]').textContent = this.state.voiceKind === 'girl' ? '👧' : '👦';
                this.el.querySelector('[data-ui="voiceLabel"]').textContent = this.state.voiceKind === 'girl' ? (c.girlVoice || 'Girl voice') : (c.boyVoice || 'Boy voice');
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
                    if (!document.fullscreenElement) await this.host.requestFullscreen?.(); else await document.exitFullscreen?.();
                } catch (_) { }
            });
            this.el.querySelector('[data-action="replay"]').addEventListener('click', () => this.currentSpeech && this.speak(this.currentSpeech));
            this.el.querySelector('[data-action="continue"]').addEventListener('click', () => this.advanceIntro());
            this.el.querySelector('[data-action="skip"]').addEventListener('click', () => this.beginRound());
        }

        refreshVoice() {
            if (!global.speechSynthesis) return;
            this.preferredVoice = global.speechSynthesis.getVoices()
                .map(voice => ({ voice, score: childVoiceScore(voice, this.voiceLocale, this.state.voiceKind) }))
                .sort((a, b) => b.score - a.score)[0]?.voice || null;
        }

        speak(text) {
            this.currentSpeech = text || '';
            if (!text || !this.state.soundOn || !this.state.narrationOn || !global.speechSynthesis) return;
            global.speechSynthesis.cancel();
            const u = new SpeechSynthesisUtterance(text);
            u.lang = this.voiceLocale;
            if (this.preferredVoice) u.voice = this.preferredVoice;
            u.rate = this.state.voiceKind === 'girl' ? 0.96 : 0.93;
            u.pitch = this.state.voiceKind === 'girl' ? 1.36 : 1.26;
            u.onstart = () => this.guide.classList.add('is-speaking');
            u.onend = u.onerror = () => this.guide.classList.remove('is-speaking');
            global.speechSynthesis.speak(u);
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
            const notes = kind === 'correct' ? [523.25,659.25,783.99] : kind === 'collect' ? [620] : [260,220];
            notes.forEach((frequency, index) => {
                const osc = ctx.createOscillator();
                const gain = ctx.createGain();
                osc.type = kind === 'wrong' ? 'triangle' : 'sine';
                osc.frequency.value = frequency;
                gain.gain.setValueAtTime(.0001, now + index * .07);
                gain.gain.exponentialRampToValueAtTime(.10, now + .02 + index * .07);
                gain.gain.exponentialRampToValueAtTime(.0001, now + .18 + index * .07);
                osc.connect(gain).connect(ctx.destination);
                osc.start(now + index * .07); osc.stop(now + .22 + index * .07);
            });
        }

        startIntro() {
            this.state.phase = 'intro';
            this.state.dialogueIndex = 0;
            this.answer.classList.remove('is-ready');
            this.dialogue.classList.remove('is-compact');
            this.showIntroLine();
        }

        showIntroLine() {
            const lines = this.copy().introDialogue || [];
            const line = lines[this.state.dialogueIndex];
            if (!line) return this.beginRound();
            const text = this.text(line.text, null);
            this.setDialogue(line.speaker, text, '');
            const continueButton = this.el.querySelector('[data-action="continue"]');
            continueButton.hidden = false;
            continueButton.textContent = this.state.dialogueIndex === lines.length - 1 ? (this.copy().start || 'Start') : (this.copy().continue || 'Continue');
            this.el.querySelector('[data-action="skip"]').hidden = false;
            this.speak(text);
        }

        advanceIntro() {
            const lines = this.copy().introDialogue || [];
            if (this.state.dialogueIndex >= lines.length - 1) return this.beginRound();
            this.state.dialogueIndex += 1;
            this.showIntroLine();
        }

        setDialogue(speakerKey, text, hint) {
            const speaker = this.config.characters?.[speakerKey] || {};
            const speakerName = speakerKey === 'student' ? this.studentName() : (speaker.name || speakerKey || '');
            this.el.querySelector('[data-ui="speaker"]').textContent = speakerName;
            this.el.querySelector('[data-ui="dialogueText"]').textContent = text || '';
            this.el.querySelector('[data-ui="hint"]').textContent = hint || '';
            const holder = this.el.querySelector('.edu3-avatar');
            if (speakerKey === 'student') {
                holder.innerHTML = `<span class="edu3-student-avatar">${studentSvg()}</span>`;
            } else {
                holder.innerHTML = `<img src="${this.config.assets?.guide || ''}" alt="" data-ui="dialogueAvatar" />`;
            }
        }

        beginRound() {
            this.stopSpeech();
            this.cleanupDragHandlers();
            this.state.phase = 'task';
            this.state.wrongAttempts = 0;
            this.state.roundStartedAt = Date.now();
            this.dialogue.classList.add('is-compact');
            this.el.querySelector('[data-action="continue"]').hidden = true;
            this.el.querySelector('[data-action="skip"]').hidden = true;
            this.bridge.classList.remove('is-open');
            this.student.className = 'edu3-student is-ready';
            this.guide.classList.remove('is-celebrating');
            this.renderRound();
        }

        renderRound() {
            const q = this.rounds[this.state.roundIndex];
            const c = this.copy();
            this.updateProgress();
            this.el.style.setProperty('--checkpoint', String(this.state.roundIndex));
            this.el.querySelector('[data-ui="missionTitle"]').textContent = c.taskTitle || 'Bring the two groups together';
            this.el.querySelector('[data-ui="missionInstruction"]').textContent = this.text(c.taskInstruction, q);
            this.el.querySelector('[data-ui="leftCount"]').textContent = q.left;
            this.el.querySelector('[data-ui="rightCount"]').textContent = q.right;
            this.el.querySelector('[data-ui="joinedCount"]').textContent = `0 / ${q.sum}`;
            this.el.querySelector('[data-ui="lanternGlass"]').replaceChildren();
            this.el.querySelector('[data-ui="leftFireflies"]').replaceChildren();
            this.el.querySelector('[data-ui="rightFireflies"]').replaceChildren();
            this.el.querySelector('[data-ui="stones"]').replaceChildren();
            this.el.querySelector('[data-ui="questionLabel"]').textContent = c.questionLabel || 'Open the bridge';
            this.el.querySelector('[data-ui="equation"]').textContent = `${q.left} + ${q.right} = ?`;
            this.el.querySelector('[data-ui="answerInstruction"]').textContent = c.collectFirst || 'Bring every firefly to the lantern first.';
            this.answer.classList.remove('is-ready','is-correct');
            this.answer.hidden = false;
            this.world.classList.remove('is-lit','is-success');
            this.world.style.setProperty('--light-progress', '0');
            this.bridge.style.setProperty('--bridge-progress', '0');
            this.buildFireflies(q);
            const intro = this.text(c.roundIntro, q);
            this.setDialogue('guide', intro, '');
            this.speak(intro);
            this.emit('round_started', { left: q.left, right: q.right, sum: q.sum });
        }

        updateProgress() {
            const total = this.rounds.length;
            const current = this.state.roundIndex + 1;
            const pct = this.state.phase === 'complete' ? 100 : (this.state.roundIndex / total) * 100;
            this.el.querySelector('[data-ui="progress"]').style.width = `${pct}%`;
            this.el.querySelector('[data-ui="round"]').textContent = `${current} / ${total}`;
        }

        cleanupDragHandlers() {
            this.dragCleanup.forEach(fn => fn());
            this.dragCleanup = [];
        }

        buildFireflies(q) {
            const make = (container, count, kind) => {
                for (let i = 0; i < count; i += 1) {
                    const fly = document.createElement('button');
                    fly.type = 'button';
                    fly.className = `edu3-firefly is-${kind}`;
                    fly.setAttribute('aria-label', this.copy().fireflyLabel || 'Move firefly to lantern');
                    fly.style.setProperty('--x', `${18 + ((i * 31) % 64)}%`);
                    fly.style.setProperty('--y', `${22 + ((i * 37) % 56)}%`);
                    fly.style.setProperty('--delay', `${-(i % 4) * .28}s`);
                    container.appendChild(fly);
                    this.bindFirefly(fly);
                }
            };
            make(this.el.querySelector('[data-ui="leftFireflies"]'), q.left, 'gold');
            make(this.el.querySelector('[data-ui="rightFireflies"]'), q.right, 'violet');
        }

        bindFirefly(fly) {
            let startX = 0, startY = 0, dx = 0, dy = 0, dragging = false, moved = false;
            const onDown = event => {
                if (this.state.phase !== 'task') return;
                startX = event.clientX; startY = event.clientY; dx = 0; dy = 0; moved = false; dragging = true;
                fly.setPointerCapture?.(event.pointerId);
                fly.classList.add('is-dragging');
                this.student.classList.add('is-pointing');
            };
            const onMove = event => {
                if (!dragging) return;
                dx = event.clientX - startX; dy = event.clientY - startY;
                if (Math.abs(dx) + Math.abs(dy) > 8) moved = true;
                fly.style.transform = `translate(-50%,-50%) translate3d(${dx}px,${dy}px,0) scale(1.12)`;
            };
            const onUp = event => {
                if (!dragging) return;
                dragging = false;
                fly.releasePointerCapture?.(event.pointerId);
                fly.classList.remove('is-dragging');
                this.student.classList.remove('is-pointing');
                const lanternRect = this.el.querySelector('[data-ui="lanternZone"]').getBoundingClientRect();
                const inside = event.clientX >= lanternRect.left - 36 && event.clientX <= lanternRect.right + 36 && event.clientY >= lanternRect.top - 36 && event.clientY <= lanternRect.bottom + 36;
                fly.style.transform = '';
                if (inside || !moved) this.collectFirefly(fly);
                else fly.animate([{transform:`translate(-50%,-50%) translate3d(${dx}px,${dy}px,0)`},{transform:'translate(-50%,-50%) translate3d(0,0,0)'}],{duration:280,easing:'cubic-bezier(.2,.8,.3,1)'});
            };
            fly.addEventListener('pointerdown', onDown);
            fly.addEventListener('pointermove', onMove);
            fly.addEventListener('pointerup', onUp);
            fly.addEventListener('pointercancel', onUp);
            this.dragCleanup.push(() => {
                fly.removeEventListener('pointerdown', onDown); fly.removeEventListener('pointermove', onMove);
                fly.removeEventListener('pointerup', onUp); fly.removeEventListener('pointercancel', onUp);
            });
        }

        collectFirefly(fly) {
            if (this.state.phase !== 'task' || fly.disabled) return;
            fly.disabled = true;
            const from = fly.getBoundingClientRect();
            const lantern = this.el.querySelector('[data-ui="lanternGlass"]');
            const to = lantern.getBoundingClientRect();
            const ghost = fly.cloneNode(true);
            ghost.classList.add('edu3-flying-ghost');
            ghost.style.left = `${from.left + from.width / 2}px`;
            ghost.style.top = `${from.top + from.height / 2}px`;
            document.body.appendChild(ghost);
            fly.classList.add('is-collected');
            const tx = to.left + to.width / 2 - (from.left + from.width / 2);
            const ty = to.top + to.height / 2 - (from.top + from.height / 2);
            const anim = ghost.animate([
                { transform:'translate(-50%,-50%) scale(1)', opacity:1, offset:0 },
                { transform:`translate(calc(-50% + ${tx * .45}px),calc(-50% + ${ty * .35 - 42}px)) scale(1.35)`, opacity:1, offset:.55 },
                { transform:`translate(calc(-50% + ${tx}px),calc(-50% + ${ty}px)) scale(.45)`, opacity:.25, offset:1 }
            ], { duration:620, easing:'cubic-bezier(.2,.75,.25,1)' });
            anim.onfinish = () => { ghost.remove(); this.finishCollect(fly.classList.contains('is-violet') ? 'violet' : 'gold'); };
            this.tone('collect');
            this.emit('object_collected');
        }

        finishCollect(kind) {
            const glass = this.el.querySelector('[data-ui="lanternGlass"]');
            const light = document.createElement('i');
            light.className = `edu3-lantern-light is-${kind}`;
            light.style.setProperty('--x', `${22 + Math.random() * 56}%`);
            light.style.setProperty('--y', `${22 + Math.random() * 56}%`);
            glass.appendChild(light);
            const collected = glass.children.length;
            const q = this.rounds[this.state.roundIndex];
            this.el.querySelector('[data-ui="joinedCount"]').textContent = `${collected} / ${q.sum}`;
            this.world.style.setProperty('--light-progress', String(collected / q.sum));
            this.world.classList.add('is-lit');
            this.bridge.style.setProperty('--bridge-progress', String(collected / q.sum));
            if (collected >= q.sum) this.readyForAnswer();
        }

        readyForAnswer() {
            const q = this.rounds[this.state.roundIndex];
            const c = this.copy();
            this.state.phase = 'answer';
            this.answer.classList.add('is-ready');
            this.el.querySelector('[data-ui="answerInstruction"]').textContent = c.answerInstruction || 'Choose the stepping stone that opens the path.';
            const values = shuffle([q.sum, clamp(q.sum - 1, 1, 12), clamp(q.sum + 1, 1, 12)]).filter((v,i,a)=>a.indexOf(v)===i);
            while (values.length < 3) values.push(q.sum + values.length + 1);
            const stones = this.el.querySelector('[data-ui="stones"]');
            values.slice(0,3).forEach((value,index) => {
                const button = document.createElement('button');
                button.type = 'button';
                button.className = 'edu3-stone';
                button.style.setProperty('--i', index);
                button.textContent = String(value);
                button.addEventListener('click', () => this.handleAnswer(value, button));
                stones.appendChild(button);
            });
            this.student.className = 'edu3-student is-thinking';
            const prompt = this.text(c.answerPrompt, q);
            this.setDialogue('guide', prompt, '');
            this.speak(prompt);
            this.emit('interaction_completed', { left: q.left, right: q.right });
        }

        handleAnswer(value, button) {
            if (this.state.phase !== 'answer') return;
            const q = this.rounds[this.state.roundIndex];
            const c = this.copy();
            const correct = Number(value) === q.sum;
            this.emit('answer_submitted', { answer:Number(value), correct, attempt:this.state.wrongAttempts + 1 });
            if (!correct) {
                this.state.wrongAttempts += 1;
                button.classList.add('is-wrong');
                setTimeout(() => button.classList.remove('is-wrong'), 720);
                this.tone('wrong');
                this.student.className = 'edu3-student is-unsure';
                const hints = c.hints || [];
                const hintIndex = clamp(this.state.wrongAttempts - 1, 0, Math.max(0, hints.length - 1));
                const hint = this.text(hints[hintIndex], q);
                const message = c.tryAgain || 'Almost. Try another stone.';
                this.setDialogue('guide', message, hint);
                this.speak(`${message} ${hint}`.trim());
                this.world.classList.add('is-ripple');
                setTimeout(() => this.world.classList.remove('is-ripple'), 650);
                this.emit('hint_used', { level:this.state.wrongAttempts });
                return;
            }

            this.state.phase = 'feedback';
            this.state.xp += this.config.rewards?.xpPerRound ?? 10;
            this.state.stars += this.config.rewards?.starsPerRound ?? 1;
            button.classList.add('is-correct');
            this.answer.classList.add('is-correct');
            this.bridge.classList.add('is-open');
            this.world.classList.add('is-success');
            this.student.className = 'edu3-student is-celebrating';
            this.guide.classList.add('is-celebrating');
            this.tone('correct');
            this.spawnCelebration();
            const message = this.text(c.correct, q);
            this.setDialogue('guide', message, `+${this.config.rewards?.xpPerRound ?? 10} XP`);
            this.speak(message);
            this.emit('round_completed', { correct:true, wrongAttempts:this.state.wrongAttempts, elapsedMs:Date.now()-this.state.roundStartedAt });
            setTimeout(() => this.crossBridge(), 520);
        }

        crossBridge() {
            this.student.className = 'edu3-student is-crossing';
            setTimeout(() => this.nextRound(), 1650);
        }

        spawnCelebration() {
            const effects = this.el.querySelector('[data-ui="effects"]');
            effects.innerHTML = Array.from({length:18},(_,i)=>`<i style="--x:${Math.random()*100}%;--d:${Math.random()*.5}s;--r:${i*31}deg"></i>`).join('');
            effects.classList.add('is-active');
            setTimeout(() => { effects.classList.remove('is-active'); effects.replaceChildren(); }, 1400);
        }

        nextRound() {
            if (this.state.roundIndex >= this.rounds.length - 1) return this.renderComplete();
            this.state.roundIndex += 1;
            this.state.phase = 'transition';
            this.world.classList.add('is-transitioning');
            setTimeout(() => { this.world.classList.remove('is-transitioning'); this.beginRound(); }, 620);
        }

        renderComplete() {
            this.state.phase = 'complete';
            this.cleanupDragHandlers();
            this.stopSpeech();
            this.el.querySelector('[data-ui="progress"]').style.width = '100%';
            const c = this.copy();
            this.el.querySelector('[data-ui="missionTitle"]').textContent = c.complete || 'Lantern Path complete!';
            this.el.querySelector('[data-ui="missionInstruction"]').textContent = c.completeCopy || '';
            this.world.innerHTML = `<section class="edu3-finish"><div class="edu3-finish-stars">★ ★ ★</div><h2>${c.complete || 'Adventure complete!'}</h2><p>${c.completeCopy || ''}</p><div class="edu3-rewards"><strong>⭐ ${this.state.stars}</strong><strong>⚡ ${this.state.xp} XP</strong></div><button class="edu3-primary" data-play-again type="button">${c.playAgain || 'Play again'}</button></section>`;
            this.answer.hidden = true;
            this.dialogue.classList.add('is-compact');
            const speech = c.completeSpeech || c.complete || '';
            this.setDialogue('guide', speech, '');
            this.speak(speech);
            this.world.querySelector('[data-play-again]').addEventListener('click', () => global.location.reload());
            this.emit('activity_completed', { xp:this.state.xp, stars:this.state.stars, elapsedMs:Date.now()-this.state.startedAt, preview:Boolean(this.options.preview) });
        }
    }

    global.EdulyticsGameEngineV3 = {
        version: VERSION,
        mount(host, config, options) {
            if (!host) throw new Error('EdulyticsGameEngineV3: host is required.');
            if (!config) throw new Error('EdulyticsGameEngineV3: config is required.');
            return new GameRuntime(host, config, options).mount();
        }
    };
}(window));