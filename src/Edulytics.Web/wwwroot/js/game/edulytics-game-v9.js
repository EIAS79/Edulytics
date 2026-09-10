(function (global) {
'use strict';

const clamp = (value, min, max) => Math.max(min, Math.min(max, value));
const ease = value => 1 - Math.pow(1 - value, 3);

function loadImage(src) {
    return new Promise((resolve, reject) => {
        const image = new Image();
        image.decoding = 'async';
        image.onload = () => resolve(image);
        image.onerror = () => reject(new Error(`Game asset failed to load: ${src}`));
        image.src = src;
    });
}

class LumenTrailGame {
    constructor(root, config, options = {}) {
        this.root = root;
        this.config = config;
        this.options = options;
        this.roundIndex = 0;
        this.scene = 'intro';
        this.collected = new Set();
        this.dragged = null;
        this.hover = null;
        this.wrongAttempts = 0;
        this.score = 0;
        this.particles = [];
        this.images = {};
        this.running = true;
        this.soundOn = true;
        this.view = { x: 960, y: 540, zoom: 1 };
        this.fromView = { ...this.view };
        this.toView = { ...this.view };
        this.cameraStart = performance.now();
        this.cameraDuration = 900;
        this.lastFrame = performance.now();
        this.time = 0;
        this.reducedMotion = matchMedia('(prefers-reduced-motion: reduce)').matches;
        this.worldW = 1920;
        this.worldH = 1080;
    }

    async mount() {
        this.root.innerHTML = `
<div class="lumen9-game" data-scene="intro">
    <canvas class="lumen9-canvas" aria-label="Interactive Join Groups to Add adventure"></canvas>
    <div class="lumen9-topshade" aria-hidden="true"></div>

    <div class="lumen9-hud">
        <div class="lumen9-brand">
            <span class="lumen9-brandcopy">
                <span class="lumen9-world"></span>
                <strong class="lumen9-zone"></strong>
            </span>
        </div>
        <div class="lumen9-progress" aria-label="Activity progress"><span class="lumen9-progress-fill"></span></div>
        <div class="lumen9-score">
            <span class="lumen9-score-star" aria-hidden="true">★</span>
            <span class="lumen9-score-value">0</span>
            <span class="lumen9-count">1 / ${this.config.rounds.length}</span>
        </div>
    </div>

    <div class="lumen9-controls">
        <button class="lumen9-iconbtn lumen9-sound" type="button" aria-label="Toggle narration">♪</button>
        <button class="lumen9-iconbtn lumen9-fullscreen" type="button" aria-label="Full screen">⛶</button>
    </div>

    <div class="lumen9-question" aria-live="polite">
        <span class="lumen9-question-label">Your mission</span>
        <strong class="lumen9-equation"></strong>
    </div>

    <div class="lumen9-answer-panel" aria-hidden="true" aria-live="polite">
        <span class="lumen9-answer-title">Choose the total</span>
        <strong class="lumen9-answer-expression"></strong>
        <div class="lumen9-answer-options" role="group" aria-label="Choose the answer"></div>
    </div>

    <div class="lumen9-dialogue">
        <img class="lumen9-avatar" alt="Eddy" />
        <div class="lumen9-dialogue-copy">
            <div class="lumen9-speaker">EDDY</div>
            <div class="lumen9-copy"></div>
        </div>
        <div class="lumen9-actions">
            <button class="lumen9-secondary lumen9-listen" type="button">Listen</button>
            <button class="lumen9-primary lumen9-next" type="button">Start adventure</button>
        </div>
    </div>

    <div class="lumen9-toast" aria-live="polite"></div>
    <div class="lumen9-help">Drag or tap the glowing lights into the lantern.</div>
</div>`;

        this.game = this.root.querySelector('.lumen9-game');
        this.canvas = this.root.querySelector('.lumen9-canvas');
        this.ctx = this.canvas.getContext('2d', { alpha: false });
        this.worldLabel = this.root.querySelector('.lumen9-world');
        this.zoneLabel = this.root.querySelector('.lumen9-zone');
        this.progressFill = this.root.querySelector('.lumen9-progress-fill');
        this.scoreValue = this.root.querySelector('.lumen9-score-value');
        this.countLabel = this.root.querySelector('.lumen9-count');
        this.question = this.root.querySelector('.lumen9-question');
        this.equation = this.root.querySelector('.lumen9-equation');
        this.answerPanel = this.root.querySelector('.lumen9-answer-panel');
        this.answerTitle = this.root.querySelector('.lumen9-answer-title');
        this.answerExpression = this.root.querySelector('.lumen9-answer-expression');
        this.answerOptions = this.root.querySelector('.lumen9-answer-options');
        this.dialogue = this.root.querySelector('.lumen9-dialogue');
        this.copy = this.root.querySelector('.lumen9-copy');
        this.nextButton = this.root.querySelector('.lumen9-next');
        this.listenButton = this.root.querySelector('.lumen9-listen');
        this.toast = this.root.querySelector('.lumen9-toast');
        this.soundButton = this.root.querySelector('.lumen9-sound');
        this.fullscreenButton = this.root.querySelector('.lumen9-fullscreen');
        this.help = this.root.querySelector('.lumen9-help');

        this.root.querySelector('.lumen9-avatar').src = this.config.assets.guide;

        const assets = this.config.assets;
        [this.images.world, this.images.student, this.images.guide] = await Promise.all([
            loadImage(assets.world),
            loadImage(assets.student),
            loadImage(assets.guide)
        ]);

        this.worldLabel.textContent = this.config.worldTitle;
        this.zoneLabel.textContent = 'Adventure begins';
        const name = (this.options.studentFirstName || '').trim();
        this.copy.textContent = this.config.copy.intro(name);
        this.help.classList.add('hide');

        this.resize();
        this.resizeObserver = new ResizeObserver(() => this.resize());
        this.resizeObserver.observe(this.game);
        this.fullscreenHandler = () => requestAnimationFrame(() => this.resize());
        document.addEventListener('fullscreenchange', this.fullscreenHandler);

        this.bindEvents();
        requestAnimationFrame(now => this.loop(now));
        return this;
    }

    bindEvents() {
        this.nextButton.addEventListener('click', () => {
            if (this.scene === 'complete') {
                this.restart();
                return;
            }
            this.startRound();
        });

        this.listenButton.addEventListener('click', () => this.speak(this.copy.textContent));

        this.soundButton.addEventListener('click', () => {
            this.soundOn = !this.soundOn;
            this.soundButton.textContent = this.soundOn ? '♪' : '×';
            this.soundButton.setAttribute('aria-label', this.soundOn ? 'Mute narration' : 'Enable narration');
            if (!this.soundOn && 'speechSynthesis' in global) global.speechSynthesis.cancel();
        });

        this.fullscreenButton.addEventListener('click', () => this.toggleFullscreen());

        this.answerOptions.addEventListener('click', event => {
            const button = event.target.closest('button[data-answer-index]');
            if (!button || this.scene !== 'answer') return;
            this.chooseAnswer(Number(button.dataset.answerIndex), button);
        });

        this.canvas.addEventListener('pointermove', event => this.onPointerMove(event));
        this.canvas.addEventListener('pointerdown', event => this.onPointerDown(event));
        this.canvas.addEventListener('pointerup', event => this.onPointerUp(event));
        this.canvas.addEventListener('pointercancel', () => this.cancelDrag());
    }

    resize() {
        if (!this.game || !this.canvas) return;

        const fullscreen = document.fullscreenElement === this.game;
        const rootRect = this.root.getBoundingClientRect();
        const gameRect = this.game.getBoundingClientRect();

        const width = Math.max(
            280,
            Math.floor(fullscreen ? (gameRect.width || global.innerWidth) : (rootRect.width || global.innerWidth))
        );

        let height;
        if (fullscreen) {
            height = Math.max(280, Math.floor(gameRect.height || global.innerHeight));
        } else {
            const preferred = width * 9 / 16;
            const maxHeight = Math.max(320, Math.min(780, global.innerHeight - 110));
            const desiredMin = width < 480 ? 420 : width < 760 ? 460 : 500;
            const minHeight = Math.min(desiredMin, maxHeight);
            height = Math.round(clamp(preferred, minHeight, maxHeight));
        }

        this.game.style.height = `${height}px`;

        const dpr = Math.min(2, global.devicePixelRatio || 1);
        this.canvas.width = Math.floor(width * dpr);
        this.canvas.height = Math.floor(height * dpr);
        this.canvas.style.width = `${width}px`;
        this.canvas.style.height = `${height}px`;
        this.width = width;
        this.height = height;
        this.dpr = dpr;
    }

    currentRound() {
        return this.config.rounds[Math.min(this.roundIndex, this.config.rounds.length - 1)];
    }

    currentZone() {
        return this.config.zones[this.currentRound().zone];
    }

    setView(target, duration = 900) {
        this.fromView = { ...this.view };
        this.toView = { x: target.focusX, y: target.focusY, zoom: Math.max(1, target.zoom || 1) };
        this.cameraStart = performance.now();
        this.cameraDuration = this.reducedMotion ? 1 : duration;
    }

    updateView(now) {
        const progress = clamp((now - this.cameraStart) / this.cameraDuration, 0, 1);
        const t = ease(progress);
        this.view.x = this.fromView.x + (this.toView.x - this.fromView.x) * t;
        this.view.y = this.fromView.y + (this.toView.y - this.fromView.y) * t;
        this.view.zoom = this.fromView.zoom + (this.toView.zoom - this.fromView.zoom) * t;
    }

    transform() {
        const zoom = Math.max(1, this.view.zoom || 1);
        const cover = Math.max(this.width / this.worldW, this.height / this.worldH);
        const scale = cover * zoom;

        const visibleW = this.width / scale;
        const visibleH = this.height / scale;
        const halfW = Math.min(this.worldW / 2, visibleW / 2);
        const halfH = Math.min(this.worldH / 2, visibleH / 2);

        const centerX = visibleW >= this.worldW
            ? this.worldW / 2
            : clamp(this.view.x, halfW, this.worldW - halfW);
        const centerY = visibleH >= this.worldH
            ? this.worldH / 2
            : clamp(this.view.y, halfH, this.worldH - halfH);

        return {
            scale,
            ox: this.width / 2 - centerX * scale,
            oy: this.height / 2 - centerY * scale,
            centerX,
            centerY
        };
    }

    worldToScreen(x, y) {
        const t = this.transform();
        return { x: x * t.scale + t.ox, y: y * t.scale + t.oy, scale: t.scale };
    }

    screenToWorld(x, y) {
        const t = this.transform();
        return { x: (x - t.ox) / t.scale, y: (y - t.oy) / t.scale };
    }

    layout() {
        const zone = this.currentZone();
        const x = zone.taskX;
        const y = zone.taskY;
        return {
            left: { x: x - 170, y: y - 35 },
            right: { x: x + 20, y: y - 42 },
            altar: { x: x + 235, y: y + 5 }
        };
    }

    tokens() {
        const round = this.currentRound();
        const layout = this.layout();
        const tokens = [];

        for (let i = 0; i < round.a; i += 1) {
            tokens.push({
                id: `a${i}`,
                group: 'a',
                x: layout.left.x + (i % 3) * 50,
                y: layout.left.y + Math.floor(i / 3) * 48
            });
        }

        for (let i = 0; i < round.b; i += 1) {
            tokens.push({
                id: `b${i}`,
                group: 'b',
                x: layout.right.x + (i % 3) * 50,
                y: layout.right.y + Math.floor(i / 3) * 48
            });
        }

        return tokens;
    }

    startRound() {
        if (this.roundIndex >= this.config.rounds.length) this.roundIndex = 0;

        const round = this.currentRound();
        const zone = this.currentZone();

        this.scene = 'collect';
        this.game.dataset.scene = 'collect';
        this.collected.clear();
        this.dragged = null;
        this.wrongAttempts = 0;
        this.hover = null;
        this.hideAnswers();
        this.dialogue.classList.add('is-hidden');
        this.question.classList.add('show');
        this.equation.textContent = `${round.a} + ${round.b}`;
        this.zoneLabel.textContent = zone.name;
        this.help.classList.remove('hide');
        this.setView(zone, 1050);
        this.syncHud();
        this.say(this.config.copy.collect(round));
    }

    restart() {
        this.roundIndex = 0;
        this.score = 0;
        this.scene = 'intro';
        this.game.dataset.scene = 'intro';
        this.question.classList.remove('show');
        this.dialogue.classList.remove('is-hidden');
        this.nextButton.textContent = 'Start adventure';
        this.zoneLabel.textContent = 'Adventure begins';
        this.scoreValue.textContent = '0';
        this.progressFill.style.width = '0%';
        this.countLabel.textContent = `1 / ${this.config.rounds.length}`;
        this.hideAnswers();
        this.setView({ focusX: 960, focusY: 540, zoom: 1 }, 700);
        const name = (this.options.studentFirstName || '').trim();
        this.copy.textContent = this.config.copy.intro(name);
        this.help.classList.add('hide');
    }

    syncHud() {
        this.countLabel.textContent = `${Math.min(this.roundIndex + 1, this.config.rounds.length)} / ${this.config.rounds.length}`;
        this.progressFill.style.width = `${(this.roundIndex / this.config.rounds.length) * 100}%`;
        this.scoreValue.textContent = String(this.score);
    }

    pointerPosition(event) {
        const rect = this.canvas.getBoundingClientRect();
        return { x: event.clientX - rect.left, y: event.clientY - rect.top };
    }

    tokenAt(worldPoint) {
        const t = this.transform();
        const radius = 34 / t.scale;
        return this.tokens().find(token =>
            !this.collected.has(token.id) &&
            Math.hypot(worldPoint.x - token.x, worldPoint.y - token.y) <= radius
        ) || null;
    }

    onPointerMove(event) {
        if (this.scene !== 'collect') return;
        const screen = this.pointerPosition(event);
        const world = this.screenToWorld(screen.x, screen.y);

        if (this.dragged) {
            this.dragged.x = world.x;
            this.dragged.y = world.y;
            return;
        }

        this.hover = this.tokenAt(world)?.id || null;
    }

    onPointerDown(event) {
        if (this.scene !== 'collect') return;
        const screen = this.pointerPosition(event);
        const world = this.screenToWorld(screen.x, screen.y);
        const token = this.tokenAt(world);
        if (!token) return;

        this.dragged = {
            id: token.id,
            group: token.group,
            originX: token.x,
            originY: token.y,
            x: world.x,
            y: world.y
        };

        this.canvas.classList.add('is-dragging');
        this.canvas.setPointerCapture?.(event.pointerId);
    }

    onPointerUp(event) {
        if (!this.dragged) return;

        const screen = this.pointerPosition(event);
        const world = this.screenToWorld(screen.x, screen.y);
        const altar = this.layout().altar;
        const nearAltar = Math.hypot(world.x - altar.x, world.y - altar.y) < 115;
        const moved = Math.hypot(
            this.dragged.x - this.dragged.originX,
            this.dragged.y - this.dragged.originY
        ) > 16;

        if (nearAltar || !moved) this.collectToken(this.dragged.id);
        this.cancelDrag();
    }

    cancelDrag() {
        this.dragged = null;
        this.canvas.classList.remove('is-dragging');
    }

    collectToken(id) {
        if (this.collected.has(id)) return;
        this.collected.add(id);
        this.chime(420 + this.collected.size * 55, .08);

        const round = this.currentRound();
        if (this.collected.size >= round.sum) {
            this.scene = 'answer';
            this.game.dataset.scene = 'answer';
            this.equation.textContent = this.config.copy.choose(round);
            this.help.classList.add('hide');
            this.showAnswers(round);
            this.say('Choose the total.');
        }
    }

    showAnswers(round) {
        this.answerTitle.textContent = 'Choose the total';
        this.answerExpression.textContent = `${round.a} + ${round.b} = ?`;
        this.answerOptions.innerHTML = round.answers.map((value, index) =>
            `<button type="button" class="lumen9-answer-option" data-answer-index="${index}" aria-label="Answer ${value}">${value}</button>`
        ).join('');
        this.answerPanel.classList.add('show');
        this.answerPanel.setAttribute('aria-hidden', 'false');
        requestAnimationFrame(() => this.answerOptions.querySelector('button')?.focus({ preventScroll: true }));
    }

    hideAnswers() {
        if (!this.answerPanel) return;
        this.answerPanel.classList.remove('show');
        this.answerPanel.setAttribute('aria-hidden', 'true');
        this.answerOptions.innerHTML = '';
    }

    chooseAnswer(index, button) {
        const round = this.currentRound();
        const value = round.answers[index];

        if (value !== round.sum) {
            this.wrongAttempts += 1;
            this.chime(170, .12);
            button?.classList.add('is-wrong');
            this.answerTitle.textContent = 'Try another answer';
            setTimeout(() => {
                button?.classList.remove('is-wrong');
                if (this.scene === 'answer') this.answerTitle.textContent = 'Choose the total';
            }, 650);

            const hint = this.wrongAttempts >= 2
                ? this.config.copy.hint2(round)
                : this.config.copy.hint1;
            this.say(hint);
            return;
        }

        this.scene = 'success';
        this.game.dataset.scene = 'success';
        this.score += 1;
        this.scoreValue.textContent = String(this.score);
        this.progressFill.style.width = `${((this.roundIndex + 1) / this.config.rounds.length) * 100}%`;

        this.answerTitle.textContent = 'Correct!';
        button?.classList.add('is-correct');
        this.answerOptions.querySelectorAll('button').forEach(item => { item.disabled = true; });

        this.spawnCelebration(this.layout().altar.x, this.layout().altar.y);
        this.chime(660, .12);
        setTimeout(() => this.chime(880, .16), 120);
        this.say(this.config.copy.correct);

        setTimeout(() => {
            this.roundIndex += 1;
            if (this.roundIndex >= this.config.rounds.length) this.complete();
            else this.startRound();
        }, this.reducedMotion ? 450 : 1300);
    }

    complete() {
        this.scene = 'complete';
        this.game.dataset.scene = 'complete';
        this.question.classList.remove('show');
        this.dialogue.classList.remove('is-hidden');
        this.copy.textContent = this.config.copy.complete;
        this.nextButton.textContent = 'Play again';
        this.zoneLabel.textContent = 'Trail restored';
        this.progressFill.style.width = '100%';
        this.help.classList.add('hide');
        this.hideAnswers();
        this.setView({ focusX: 960, focusY: 540, zoom: 1 }, 900);
        this.spawnCelebration(960, 540, 70);
        this.speak(this.config.copy.complete);
    }

    say(message) {
        this.showToast(message);
        if (this.soundOn && (this.scene === 'answer' || this.wrongAttempts > 0)) this.speak(message);
    }

    showToast(message) {
        this.toast.textContent = message;
        this.toast.classList.add('show');
        clearTimeout(this.toastTimer);
        this.toastTimer = setTimeout(() => this.toast.classList.remove('show'), 1700);
    }

    speak(message) {
        if (!this.soundOn || !('speechSynthesis' in global) || !message) return;

        global.speechSynthesis.cancel();
        const utterance = new SpeechSynthesisUtterance(message);
        utterance.lang = this.config.lessonLanguage === 'pl'
            ? 'pl-PL'
            : this.config.lessonLanguage === 'ar'
                ? 'ar'
                : 'en-GB';

        const voices = global.speechSynthesis.getVoices();
        const language = utterance.lang.slice(0, 2).toLowerCase();
        const candidates = voices.filter(voice => voice.lang.toLowerCase().startsWith(language));
        const preferred = candidates.find(voice => /child|young|jenny|aria|libby|sonia/i.test(voice.name)) || candidates[0];

        if (preferred) utterance.voice = preferred;
        utterance.rate = .96;
        utterance.pitch = 1.16;
        global.speechSynthesis.speak(utterance);
    }

    chime(frequency, duration) {
        if (!this.soundOn) return;
        try {
            this.audioContext = this.audioContext || new (global.AudioContext || global.webkitAudioContext)();
            const oscillator = this.audioContext.createOscillator();
            const gain = this.audioContext.createGain();

            oscillator.type = 'sine';
            oscillator.frequency.value = frequency;
            gain.gain.setValueAtTime(.0001, this.audioContext.currentTime);
            gain.gain.exponentialRampToValueAtTime(.06, this.audioContext.currentTime + .015);
            gain.gain.exponentialRampToValueAtTime(.0001, this.audioContext.currentTime + duration);
            oscillator.connect(gain).connect(this.audioContext.destination);
            oscillator.start();
            oscillator.stop(this.audioContext.currentTime + duration + .02);
        } catch (_) { /* optional sound */ }
    }

    async toggleFullscreen() {
        try {
            if (!document.fullscreenElement) await this.game.requestFullscreen?.();
            else await document.exitFullscreen?.();
            requestAnimationFrame(() => this.resize());
        } catch (_) { /* browser may deny */ }
    }

    spawnCelebration(x, y, count = 34) {
        for (let i = 0; i < count; i += 1) {
            this.particles.push({
                x,
                y,
                vx: (Math.random() - .5) * 280,
                vy: -80 - Math.random() * 230,
                life: .8 + Math.random() * .7,
                age: 0,
                size: 5 + Math.random() * 8,
                hue: 38 + Math.random() * 150
            });
        }
    }

    drawWorld(ctx) {
        const t = this.transform();
        ctx.save();
        ctx.translate(t.ox, t.oy);
        ctx.scale(t.scale, t.scale);
        ctx.drawImage(this.images.world, 0, 0, this.worldW, this.worldH);
        ctx.restore();

        const pulse = .5 + .5 * Math.sin(this.time * 1.8);
        ctx.save();
        ctx.globalCompositeOperation = 'screen';

        const glowPoints = [[935,550], [1510,650], [640,855], [1430,150], [455,270]];
        for (const [x, y] of glowPoints) {
            const p = this.worldToScreen(x, y);
            const radius = (28 + 10 * pulse) * t.scale;
            const glow = ctx.createRadialGradient(p.x, p.y, 0, p.x, p.y, radius);
            glow.addColorStop(0, 'rgba(255,229,123,.33)');
            glow.addColorStop(1, 'rgba(255,229,123,0)');
            ctx.fillStyle = glow;
            ctx.beginPath();
            ctx.arc(p.x, p.y, radius, 0, Math.PI * 2);
            ctx.fill();
        }
        ctx.restore();
    }

    drawCharacter(ctx, image, x, y, height, bob, flip = false) {
        if (!image?.naturalWidth || !image?.naturalHeight) return;
        const width = height * image.naturalWidth / image.naturalHeight;

        ctx.save();
        ctx.translate(x, y + Math.sin(this.time * 2 + bob) * 4);
        if (flip) ctx.scale(-1, 1);
        ctx.drawImage(image, -width / 2, -height, width, height);
        ctx.restore();
    }

    drawCharacters(ctx) {
        const compact = this.width < 680;
        const short = this.height < 430;

        if (this.scene === 'intro' || this.scene === 'complete') {
            const studentH = compact
                ? clamp(this.height * .38, 125, 210)
                : clamp(this.height * .46, 200, 350);
            const guideH = compact
                ? clamp(this.height * .25, 90, 145)
                : clamp(this.height * .28, 130, 215);

            const studentX = compact ? this.width * .18 : clamp(this.width * .17, 105, 235);
            const guideX = compact ? this.width * .40 : clamp(this.width * .34, 205, 470);
            const baseY = this.height - Math.max(8, this.height * .025);

            this.drawCharacter(ctx, this.images.student, studentX, baseY, studentH, 0);
            this.drawCharacter(ctx, this.images.guide, guideX, baseY - (compact ? 2 : 8), guideH, 1.2);
            return;
        }

        const studentH = compact
            ? clamp(this.height * .27, 92, 138)
            : clamp(this.height * .25, 145, 215);
        const guideH = compact
            ? clamp(this.height * .19, 68, 102)
            : clamp(this.height * .17, 96, 145);

        if (compact && (this.scene === 'answer' || this.scene === 'success')) {
            const baseY = short ? this.height * .64 : this.height * .72;
            this.drawCharacter(ctx, this.images.student, 48, baseY, studentH, 0);
            this.drawCharacter(ctx, this.images.guide, this.width - 46, baseY, guideH, 1.1, true);
            return;
        }

        const baseY = this.height - Math.max(10, this.height * .025);
        this.drawCharacter(ctx, this.images.student, compact ? 54 : 82, baseY, studentH, 0);
        this.drawCharacter(ctx, this.images.guide, compact ? 126 : 170, baseY - 2, guideH, 1.1);
    }

    drawToken(ctx, token) {
        const p = this.worldToScreen(token.x, token.y);
        const dragging = this.dragged?.id === token.id;
        const dragPoint = dragging ? this.worldToScreen(this.dragged.x, this.dragged.y) : p;
        const x = dragPoint.x;
        const y = dragPoint.y;
        const radius = clamp(17 * p.scale, 11, 24) * (this.hover === token.id ? 1.15 : 1);
        const color = token.group === 'a' ? '#ffd55d' : '#8eeeff';

        ctx.save();
        ctx.globalCompositeOperation = 'screen';
        ctx.shadowColor = color;
        ctx.shadowBlur = radius * 2.2;
        ctx.fillStyle = color;
        ctx.beginPath();
        ctx.arc(x, y + Math.sin(this.time * 4 + token.x) * 4, radius, 0, Math.PI * 2);
        ctx.fill();

        ctx.fillStyle = 'rgba(255,255,255,.92)';
        ctx.beginPath();
        ctx.arc(x - radius * .25, y - radius * .25, radius * .27, 0, Math.PI * 2);
        ctx.fill();
        ctx.restore();
    }

    drawAltar(ctx, round) {
        const point = this.worldToScreen(this.layout().altar.x, this.layout().altar.y);
        const size = clamp(56 * point.scale, 42, 76);
        const ratio = round.sum ? this.collected.size / round.sum : 0;

        ctx.save();
        ctx.translate(point.x, point.y);
        ctx.shadowColor = '#ffe17a';
        ctx.shadowBlur = 22 + ratio * 26;

        const gradient = ctx.createRadialGradient(0, 0, 4, 0, 0, size);
        gradient.addColorStop(0, `rgba(255,247,188,${.35 + .45 * ratio})`);
        gradient.addColorStop(.58, 'rgba(255,197,73,.72)');
        gradient.addColorStop(1, 'rgba(255,181,56,.08)');
        ctx.fillStyle = gradient;
        ctx.beginPath();
        ctx.arc(0, 0, size, 0, Math.PI * 2);
        ctx.fill();

        ctx.strokeStyle = 'rgba(255,244,190,.9)';
        ctx.lineWidth = 3;
        ctx.beginPath();
        ctx.arc(0, 0, size * .62, 0, Math.PI * 2);
        ctx.stroke();

        ctx.fillStyle = '#fff8d4';
        ctx.font = `900 ${clamp(18 * point.scale, 15, 24)}px Inter,Arial`;
        ctx.textAlign = 'center';
        ctx.textBaseline = 'middle';
        ctx.fillText(`${this.collected.size}/${round.sum}`, 0, 0);
        ctx.restore();
    }

    drawParticles(ctx, dt) {
        this.particles = this.particles.filter(particle => {
            particle.age += dt;
            if (particle.age >= particle.life) return false;

            particle.vy += 260 * dt;
            particle.x += particle.vx * dt;
            particle.y += particle.vy * dt;

            const p = this.worldToScreen(particle.x, particle.y);
            ctx.save();
            ctx.globalAlpha = 1 - particle.age / particle.life;
            ctx.fillStyle = `hsl(${particle.hue} 90% 68%)`;
            ctx.translate(p.x, p.y);
            ctx.rotate(particle.age * 5);
            ctx.fillRect(-particle.size / 2, -particle.size / 2, particle.size, particle.size);
            ctx.restore();
            return true;
        });
    }

    draw(now, dt) {
        const ctx = this.ctx;
        ctx.setTransform(this.dpr, 0, 0, this.dpr, 0, 0);
        ctx.clearRect(0, 0, this.width, this.height);

        this.updateView(now);
        this.drawWorld(ctx);

        if (this.scene === 'collect' || this.scene === 'answer' || this.scene === 'success') {
            const round = this.currentRound();
            for (const token of this.tokens()) {
                if (!this.collected.has(token.id)) this.drawToken(ctx, token);
            }
            this.drawAltar(ctx, round);
        }

        this.drawParticles(ctx, dt);
        this.drawCharacters(ctx);
    }

    loop(now) {
        if (!this.running) return;
        const dt = Math.min(.035, Math.max(0, (now - this.lastFrame) / 1000));
        this.lastFrame = now;
        this.time += dt;
        this.draw(now, dt);
        requestAnimationFrame(next => this.loop(next));
    }

    destroy() {
        this.running = false;
        this.resizeObserver?.disconnect();
        if (this.fullscreenHandler) document.removeEventListener('fullscreenchange', this.fullscreenHandler);
        if ('speechSynthesis' in global) global.speechSynthesis.cancel();
    }
}

global.EdulyticsGameEngineV9 = {
    mount: (root, config, options) => new LumenTrailGame(root, config, options).mount()
};

}(window));
