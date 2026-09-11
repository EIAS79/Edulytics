(function (global) {
    'use strict';

    const clamp = (value, min, max) => Math.max(min, Math.min(max, value));

    function shuffle(values) {
        const copy = [...values];
        for (let i = copy.length - 1; i > 0; i -= 1) {
            const j = Math.floor(Math.random() * (i + 1));
            [copy[i], copy[j]] = [copy[j], copy[i]];
        }
        return copy;
    }

    function roundNumbers(pool, count) {
        const values = [];
        let bag = shuffle(pool);
        while (values.length < count) {
            if (!bag.length) bag = shuffle(pool);
            const candidate = bag.shift();
            if (values.length && values[values.length - 1] === candidate) {
                bag.push(candidate);
                continue;
            }
            values.push(candidate);
        }
        return values;
    }

    class CountingGroveGame {
        constructor(root, config, options) {
            this.root = root;
            this.config = config;
            this.options = options || {};
            this.rounds = roundNumbers(config.countPool, config.profile.roundCount || 10)
                .map((count, index) => ({ count, variant: config.visualVariants[index % config.visualVariants.length] }));
            this.roundIndex = 0;
            this.phase = 'intro';
            this.counted = new Set();
            this.score = 0;
            this.mistakes = 0;
            this.soundOn = true;
            this.locked = false;
            this.hintTimer = 0;
            this.audioContext = null;
            this.fullscreenHandler = () => this.syncFullscreenLabel();
        }

        mount() {
            this.root.innerHTML = `
                <section class="cg-game" aria-label="Counting Grove game">
                    <div class="cg-sky" aria-hidden="true"><span></span><span></span><span></span></div>
                    <div class="cg-hills cg-hills-back" aria-hidden="true"></div>
                    <div class="cg-hills cg-hills-front" aria-hidden="true"></div>
                    <header class="cg-hud">
                        <div class="cg-world-title">
                            <span class="cg-lantern-mark" aria-hidden="true"><i></i></span>
                            <span><strong>COUNTING GROVE</strong><small data-cg-zone>Discovery clearing</small></span>
                        </div>
                        <div class="cg-hud-right">
                            <div class="cg-score" aria-label="Score"><span aria-hidden="true">★</span><strong data-cg-score>0</strong></div>
                            <div class="cg-round" data-cg-round>1 / ${this.rounds.length}</div>
                            <button class="cg-icon-btn" type="button" data-cg-sound aria-pressed="true" aria-label="Turn sound off">🔊</button>
                            <button class="cg-icon-btn" type="button" data-cg-fullscreen aria-label="Enter fullscreen">⛶</button>
                        </div>
                    </header>
                    <div class="cg-progress" role="progressbar" aria-valuemin="0" aria-valuemax="100" aria-valuenow="0"><span data-cg-progress></span></div>
                    <main class="cg-stage">
                        <div class="cg-mission" aria-live="polite">
                            <span class="cg-mission-kicker">YOUR MISSION</span>
                            <strong data-cg-prompt>Wake the Counting Grove</strong>
                            <small data-cg-subprompt>Touch each glowbug once and count as you go.</small>
                        </div>
                        <div class="cg-field" data-cg-field aria-label="Interactive counting area"></div>
                        <div class="cg-counter-orb" data-cg-counter hidden>
                            <span>Counted</span><strong data-cg-count-value>0</strong>
                        </div>
                        <div class="cg-number-dock" data-cg-number-dock hidden aria-label="Choose the total"></div>
                        <div class="cg-feedback" data-cg-feedback aria-live="polite" hidden></div>
                    </main>
                    <div class="cg-eddy-panel" data-cg-eddy-panel>
                        <img src="${config.assets.guide}" alt="Eddy, the Edulytics guide" />
                        <div><span>EDDY</span><p data-cg-eddy-text>${config.copy.introBody}</p></div>
                        <button class="cg-primary" type="button" data-cg-start>${config.copy.start}</button>
                    </div>
                    <div class="cg-complete" data-cg-complete hidden>
                        <div class="cg-complete-card">
                            <img src="${config.assets.guide}" alt="Eddy celebrating" />
                            <p class="cg-complete-kicker">ADVENTURE COMPLETE</p>
                            <h2>${config.copy.completeTitle}</h2>
                            <p>${config.copy.completeBody}</p>
                            <div class="cg-final-score"><span>Stars</span><strong data-cg-final-score>0</strong></div>
                            <button class="cg-primary" type="button" data-cg-replay>${config.copy.replay}</button>
                        </div>
                    </div>
                </section>`;

            this.game = this.root.querySelector('.cg-game');
            this.field = this.root.querySelector('[data-cg-field]');
            this.prompt = this.root.querySelector('[data-cg-prompt]');
            this.subprompt = this.root.querySelector('[data-cg-subprompt]');
            this.counter = this.root.querySelector('[data-cg-counter]');
            this.countValue = this.root.querySelector('[data-cg-count-value]');
            this.numberDock = this.root.querySelector('[data-cg-number-dock]');
            this.feedback = this.root.querySelector('[data-cg-feedback]');
            this.eddyText = this.root.querySelector('[data-cg-eddy-text]');
            this.scoreNode = this.root.querySelector('[data-cg-score]');
            this.roundNode = this.root.querySelector('[data-cg-round]');
            this.progressNode = this.root.querySelector('[data-cg-progress]');
            this.progressBar = this.root.querySelector('.cg-progress');
            this.complete = this.root.querySelector('[data-cg-complete]');

            this.root.querySelector('[data-cg-start]').addEventListener('click', () => this.start());
            this.root.querySelector('[data-cg-sound]').addEventListener('click', event => this.toggleSound(event.currentTarget));
            this.root.querySelector('[data-cg-fullscreen]').addEventListener('click', () => this.toggleFullscreen());
            this.root.querySelector('[data-cg-replay]').addEventListener('click', () => this.replay());
            document.addEventListener('fullscreenchange', this.fullscreenHandler);
            this.syncHud();
            return this;
        }

        currentRound() {
            return this.rounds[this.roundIndex];
        }

        start() {
            if (this.phase !== 'intro') return;
            this.root.querySelector('[data-cg-eddy-panel]').classList.add('is-compact');
            this.root.querySelector('[data-cg-start]').hidden = true;
            this.phase = 'count';
            this.renderCountPhase(false);
            this.say(this.config.copy.firstPrompt);
        }

        renderCountPhase(isRecount) {
            const round = this.currentRound();
            this.phase = isRecount ? 'recount' : 'count';
            this.counted.clear();
            this.locked = false;
            this.counter.hidden = false;
            this.countValue.textContent = '0';
            this.numberDock.hidden = true;
            this.numberDock.innerHTML = '';
            this.feedback.hidden = true;
            this.prompt.textContent = isRecount ? this.config.copy.recountPrompt : this.config.copy.firstPrompt;
            this.subprompt.textContent = isRecount
                ? 'The objects moved, but the group is the same.'
                : 'Each glowbug should receive exactly one count number.';
            this.renderObjects(round.count, round.variant, isRecount);
            this.scheduleHint();
        }

        renderObjects(count, variant, isRecount) {
            const existing = Array.from(this.field.querySelectorAll('.cg-glowbug'));
            if (!isRecount || existing.length !== count) {
                this.field.innerHTML = '';
                for (let i = 0; i < count; i += 1) {
                    const button = document.createElement('button');
                    button.type = 'button';
                    button.className = `cg-glowbug cg-${variant}`;
                    button.dataset.cgObject = String(i);
                    button.setAttribute('aria-label', `Glowbug ${i + 1}, not counted`);
                    button.innerHTML = '<span class="cg-wings" aria-hidden="true"></span><span class="cg-glow" aria-hidden="true"></span><span class="cg-order" aria-hidden="true"></span>';
                    button.addEventListener('click', () => this.touchObject(button));
                    this.field.appendChild(button);
                }
            }

            const objects = Array.from(this.field.querySelectorAll('.cg-glowbug'));
            const points = this.generateLayout(objects.length);
            objects.forEach((object, index) => {
                object.classList.remove('is-counted', 'is-hint', 'is-breeze');
                object.setAttribute('aria-label', `Glowbug ${index + 1}, not counted`);
                object.querySelector('.cg-order').textContent = '';
                object.style.setProperty('--x', `${points[index].x}%`);
                object.style.setProperty('--y', `${points[index].y}%`);
                object.style.setProperty('--delay', `${(index % 5) * -0.17}s`);
                if (isRecount) {
                    object.classList.add('is-breeze');
                    global.setTimeout(() => object.classList.remove('is-breeze'), 700 + index * 35);
                }
            });
        }

        generateLayout(count) {
            const base = [
                { x: 11, y: 24 }, { x: 27, y: 20 }, { x: 44, y: 27 }, { x: 61, y: 19 }, { x: 79, y: 26 },
                { x: 18, y: 50 }, { x: 35, y: 55 }, { x: 53, y: 48 }, { x: 70, y: 56 }, { x: 87, y: 49 },
                { x: 31, y: 70 }, { x: 66, y: 69 }
            ];
            return shuffle(base).slice(0, count).map(point => ({
                x: clamp(point.x + (Math.random() * 6 - 3), 8, 90),
                y: clamp(point.y + (Math.random() * 6 - 3), 16, 72)
            }));
        }

        touchObject(button) {
            if (this.locked || !['count', 'recount'].includes(this.phase)) return;
            const id = button.dataset.cgObject;
            if (this.counted.has(id)) {
                button.classList.remove('is-nudge');
                void button.offsetWidth;
                button.classList.add('is-nudge');
                this.setFeedback(this.config.copy.duplicate, 'gentle');
                return;
            }

            this.counted.add(id);
            const order = this.counted.size;
            button.classList.add('is-counted');
            button.classList.remove('is-hint');
            button.querySelector('.cg-order').textContent = String(order);
            button.setAttribute('aria-label', `Glowbug counted as ${order}`);
            this.countValue.textContent = String(order);
            this.chime(260 + order * 26, 0.05);
            this.clearHint();

            if (order === this.currentRound().count) {
                this.locked = true;
                global.setTimeout(() => this.showNumberStones(), 420);
            } else {
                this.scheduleHint();
            }
        }

        showNumberStones() {
            const count = this.currentRound().count;
            this.phase = this.phase === 'recount' ? 'recount-confirm' : 'count-confirm';
            this.locked = false;
            this.prompt.textContent = this.phase === 'count-confirm'
                ? this.config.copy.firstCheck
                : this.config.copy.secondCheck;
            this.subprompt.textContent = this.phase === 'count-confirm'
                ? 'Your touched glowbugs are numbered so you can check.'
                : 'Compare your second count with the first one.';

            const choices = this.answerChoices(count);
            this.numberDock.innerHTML = '';
            choices.forEach(value => {
                const button = document.createElement('button');
                button.type = 'button';
                button.className = 'cg-number-stone';
                button.textContent = String(value);
                button.setAttribute('aria-label', `Choose ${value}`);
                button.addEventListener('click', () => this.chooseAnswer(value, button));
                this.numberDock.appendChild(button);
            });
            this.numberDock.hidden = false;
            this.setFeedback('Choose the total below.', 'info');
        }

        answerChoices(answer) {
            const values = new Set([answer]);
            const candidates = shuffle([
                clamp(answer - 1, 1, 10),
                clamp(answer + 1, 1, 10),
                clamp(answer - 2, 1, 10),
                clamp(answer + 2, 1, 10),
                ...this.config.countPool
            ]);
            candidates.forEach(value => {
                if (values.size < 4) values.add(value);
            });
            return shuffle([...values]);
        }

        chooseAnswer(value, button) {
            if (this.locked || !['count-confirm', 'recount-confirm'].includes(this.phase)) return;
            const answer = this.currentRound().count;
            if (value !== answer) {
                this.mistakes += 1;
                button.classList.remove('is-wrong');
                void button.offsetWidth;
                button.classList.add('is-wrong');
                this.setFeedback(this.config.copy.wrong, 'try');
                this.say(this.config.copy.wrong, false);
                return;
            }

            this.locked = true;
            button.classList.add('is-correct');
            this.chime(660, 0.11);
            this.chime(880, 0.14, 0.09);

            if (this.phase === 'count-confirm') {
                this.score += 100;
                this.syncHud();
                const message = this.config.copy.firstSuccess(answer);
                this.setFeedback(message, 'success');
                this.say(message);
                global.setTimeout(() => this.beginBreeze(), 1150);
                return;
            }

            this.score += 150;
            this.syncHud();
            const message = this.config.copy.roundSuccess(answer);
            this.setFeedback(message, 'success');
            this.say(message);
            global.setTimeout(() => this.finishRound(), 1250);
        }

        beginBreeze() {
            this.phase = 'breeze';
            this.prompt.textContent = this.config.copy.breeze;
            this.subprompt.textContent = 'Watch the same glowbugs change places.';
            this.numberDock.hidden = true;
            this.counter.hidden = true;
            this.game.classList.add('is-breezy');
            global.setTimeout(() => {
                this.game.classList.remove('is-breezy');
                this.renderCountPhase(true);
                this.say(this.config.copy.recountPrompt);
            }, 900);
        }

        finishRound() {
            this.roundIndex += 1;
            if (this.roundIndex >= this.rounds.length) {
                this.finishGame();
                return;
            }
            this.phase = 'count';
            this.syncHud();
            this.renderCountPhase(false);
            this.say(this.config.copy.firstPrompt);
        }

        finishGame() {
            this.phase = 'complete';
            this.clearHint();
            this.root.querySelector('[data-cg-final-score]').textContent = String(this.score);
            this.complete.hidden = false;
            this.progressNode.style.width = '100%';
            this.progressBar.setAttribute('aria-valuenow', '100');
            this.celebrate();
            this.say(this.config.copy.completeBody);
        }

        replay() {
            this.rounds = roundNumbers(this.config.countPool, this.config.profile.roundCount || 10)
                .map((count, index) => ({ count, variant: this.config.visualVariants[index % this.config.visualVariants.length] }));
            this.roundIndex = 0;
            this.score = 0;
            this.mistakes = 0;
            this.complete.hidden = true;
            this.phase = 'count';
            this.syncHud();
            this.renderCountPhase(false);
            this.say(this.config.copy.firstPrompt);
        }

        syncHud() {
            this.scoreNode.textContent = String(this.score);
            this.roundNode.textContent = `${Math.min(this.roundIndex + 1, this.rounds.length)} / ${this.rounds.length}`;
            const progress = Math.round((this.roundIndex / this.rounds.length) * 100);
            this.progressNode.style.width = `${progress}%`;
            this.progressBar.setAttribute('aria-valuenow', String(progress));
        }

        setFeedback(message, kind) {
            this.feedback.textContent = message;
            this.feedback.dataset.kind = kind || 'info';
            this.feedback.hidden = false;
        }

        say(message, speak = true) {
            this.eddyText.textContent = message;
            if (speak && this.soundOn && 'speechSynthesis' in global) {
                global.speechSynthesis.cancel();
                const utterance = new SpeechSynthesisUtterance(message);
                utterance.lang = 'en-GB';
                utterance.rate = 0.92;
                utterance.pitch = 1.18;
                global.speechSynthesis.speak(utterance);
            }
        }

        toggleSound(button) {
            this.soundOn = !this.soundOn;
            button.setAttribute('aria-pressed', this.soundOn ? 'true' : 'false');
            button.setAttribute('aria-label', this.soundOn ? 'Turn sound off' : 'Turn sound on');
            button.textContent = this.soundOn ? '🔊' : '🔇';
            if (!this.soundOn && 'speechSynthesis' in global) global.speechSynthesis.cancel();
        }

        chime(frequency, duration, delay = 0) {
            if (!this.soundOn) return;
            try {
                this.audioContext ??= new (global.AudioContext || global.webkitAudioContext)();
                const ctx = this.audioContext;
                const oscillator = ctx.createOscillator();
                const gain = ctx.createGain();
                const start = ctx.currentTime + delay;
                oscillator.type = 'sine';
                oscillator.frequency.setValueAtTime(frequency, start);
                gain.gain.setValueAtTime(0.0001, start);
                gain.gain.exponentialRampToValueAtTime(0.12, start + 0.012);
                gain.gain.exponentialRampToValueAtTime(0.0001, start + duration);
                oscillator.connect(gain);
                gain.connect(ctx.destination);
                oscillator.start(start);
                oscillator.stop(start + duration + 0.02);
            } catch (_) {
                // Audio is progressive enhancement; gameplay never depends on it.
            }
        }

        scheduleHint() {
            this.clearHint();
            this.hintTimer = global.setTimeout(() => {
                if (!['count', 'recount'].includes(this.phase)) return;
                const remaining = Array.from(this.field.querySelectorAll('.cg-glowbug'))
                    .filter(node => !this.counted.has(node.dataset.cgObject));
                const target = remaining[Math.floor(Math.random() * remaining.length)];
                target?.classList.add('is-hint');
                this.setFeedback(this.config.copy.hint, 'info');
            }, 9000);
        }

        clearHint() {
            global.clearTimeout(this.hintTimer);
            this.hintTimer = 0;
        }

        celebrate() {
            for (let i = 0; i < 20; i += 1) {
                const spark = document.createElement('span');
                spark.className = 'cg-spark';
                spark.style.setProperty('--sx', `${8 + Math.random() * 84}%`);
                spark.style.setProperty('--sd', `${Math.random() * 0.8}s`);
                this.game.appendChild(spark);
                global.setTimeout(() => spark.remove(), 2600);
            }
        }

        async toggleFullscreen() {
            try {
                if (!document.fullscreenElement) await this.game.requestFullscreen();
                else await document.exitFullscreen();
            } catch (_) {
                this.setFeedback('Fullscreen is not available in this browser.', 'info');
            }
        }

        syncFullscreenLabel() {
            const button = this.root.querySelector('[data-cg-fullscreen]');
            if (!button) return;
            const active = document.fullscreenElement === this.game;
            button.textContent = active ? '⤢' : '⛶';
            button.setAttribute('aria-label', active ? 'Exit fullscreen' : 'Enter fullscreen');
        }

        destroy() {
            this.clearHint();
            document.removeEventListener('fullscreenchange', this.fullscreenHandler);
            if ('speechSynthesis' in global) global.speechSynthesis.cancel();
            try { this.audioContext?.close(); } catch (_) { }
            this.root.innerHTML = '';
        }
    }

    global.EdulyticsCountingGroveV1 = {
        mount(root, config, options) {
            if (!root || !config) throw new Error('Counting Grove requires a root element and activity configuration.');
            return new CountingGroveGame(root, config, options).mount();
        }
    };
}(window));
