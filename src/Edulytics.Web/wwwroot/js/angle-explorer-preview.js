(function () {
    'use strict';

    var root = document.querySelector('[data-angle-explorer-game]');
    if (!root) return;

    var expectedLessonCode = 'PED:CAMBRIDGE-INTL-MATH:S5:5G-1:BUILD';
    if (root.getAttribute('data-lesson-code') !== expectedLessonCode) {
        root.innerHTML = '<div class="gw-error">This geometry game profile is not configured for the selected lesson.</div>';
        return;
    }

    var guide = '/images/game/v9/eddy-guide.webp';
    var targetAngles = [35, 50, 65, 115, 135, 155, 210, 235, 275, 315];
    var state = {
        rounds: [],
        roundIndex: 0,
        score: 0,
        angle: 45,
        soundOn: true,
        locked: false,
        dragging: false,
        hintTimer: null
    };

    function shuffle(values) {
        var a = values.slice();
        for (var i = a.length - 1; i > 0; i -= 1) {
            var j = Math.floor(Math.random() * (i + 1));
            var t = a[i]; a[i] = a[j]; a[j] = t;
        }
        return a;
    }

    function makeRounds() {
        return shuffle(targetAngles).slice(0, 8).map(function (angle, index) {
            return { angle: angle, mode: index % 2 === 0 ? 'build' : 'classify' };
        });
    }

    function angleKind(angle) {
        if (angle > 0 && angle < 90) return 'Acute';
        if (angle > 90 && angle < 180) return 'Obtuse';
        if (angle > 180 && angle < 360) return 'Reflex';
        return angle === 90 ? 'Right' : 'Straight';
    }

    state.rounds = makeRounds();

    root.innerHTML = '' +
        '<section class="gw-game" aria-label="Angle Observatory game">' +
            '<header class="gw-hud">' +
                '<div class="gw-brand"><span class="gw-brand-mark" aria-hidden="true">◒</span><span><strong>ANGLE OBSERVATORY</strong><small>Signal deck</small></span></div>' +
                '<div class="gw-hud-right"><div class="gw-pill">★ <span data-gw-score>0</span></div><div class="gw-pill" data-gw-round>1 / 8</div><button class="gw-icon" type="button" data-gw-sound aria-label="Toggle sound">🔊</button><button class="gw-icon" type="button" data-gw-fullscreen aria-label="Fullscreen">⛶</button></div>' +
            '</header>' +
            '<div class="gw-progress"><span data-gw-progress></span></div>' +
            '<main class="gw-stage">' +
                '<div class="gw-mission"><span>YOUR MISSION</span><strong data-gw-question>Calibrate the observatory</strong><small data-gw-subquestion>Move the golden signal ray.</small></div>' +
                '<div class="gw-angle-workspace">' +
                    '<section class="gw-dial-card">' +
                        '<svg class="gw-angle-svg" viewBox="0 0 600 360" role="img" aria-label="Interactive angle dial" data-angle-svg>' +
                            '<circle cx="300" cy="185" r="145" fill="rgba(255,255,255,.035)" stroke="rgba(190,238,247,.2)" stroke-width="3"></circle>' +
                            '<line class="gw-angle-base" x1="300" y1="185" x2="455" y2="185"></line>' +
                            '<path class="gw-angle-arc" data-angle-arc></path>' +
                            '<line class="gw-angle-ray" x1="300" y1="185" x2="410" y2="75" data-angle-ray></line>' +
                            '<circle class="gw-angle-center" cx="300" cy="185" r="10"></circle>' +
                            '<circle class="gw-angle-handle" r="14" data-angle-handle></circle>' +
                        '</svg>' +
                        '<div class="gw-angle-value"><span data-angle-value>45</span>°</div>' +
                    '</section>' +
                    '<aside class="gw-side-card">' +
                        '<h3 data-angle-panel-title>Signal controls</h3>' +
                        '<p data-angle-panel-copy>Drag the golden handle or use the controls to set the ray.</p>' +
                        '<div class="gw-nudge" data-angle-nudges><button type="button" data-nudge="-5">− 5°</button><button type="button" data-nudge="5">+ 5°</button></div>' +
                        '<div class="gw-choice-row" data-angle-choices></div>' +
                    '</aside>' +
                '</div>' +
            '</main>' +
            '<div class="gw-eddy"><img src="' + guide + '" alt="Eddy" /><div><span>EDDY</span><p data-gw-eddy>Let’s tune the signal rays. Angles tell us how far a ray has turned.</p></div></div>' +
            '<div class="gw-complete" data-gw-complete hidden><div class="gw-complete-card"><img src="' + guide + '" alt="Eddy celebrating" /><p>ADVENTURE COMPLETE</p><h2>Observatory online!</h2><p>You built and identified acute, obtuse and reflex angles.</p><strong data-gw-final-score></strong><br/><button class="gw-primary" type="button" data-gw-replay>Play again</button></div></div>' +
        '</section>';

    var game = root.querySelector('.gw-game');
    var svg = root.querySelector('[data-angle-svg]');
    var ray = root.querySelector('[data-angle-ray]');
    var handle = root.querySelector('[data-angle-handle]');
    var arc = root.querySelector('[data-angle-arc]');
    var angleValue = root.querySelector('[data-angle-value]');
    var question = root.querySelector('[data-gw-question]');
    var subquestion = root.querySelector('[data-gw-subquestion]');
    var eddy = root.querySelector('[data-gw-eddy]');
    var choices = root.querySelector('[data-angle-choices]');
    var nudges = root.querySelector('[data-angle-nudges]');
    var scoreNode = root.querySelector('[data-gw-score]');
    var roundNode = root.querySelector('[data-gw-round]');
    var progress = root.querySelector('[data-gw-progress]');
    var complete = root.querySelector('[data-gw-complete]');
    var finalScore = root.querySelector('[data-gw-final-score]');
    var soundButton = root.querySelector('[data-gw-sound]');
    var fullscreenButton = root.querySelector('[data-gw-fullscreen]');

    function currentRound() { return state.rounds[state.roundIndex]; }

    function clearHint() {
        if (state.hintTimer !== null) { window.clearTimeout(state.hintTimer); state.hintTimer = null; }
    }

    function narrate(text) {
        if (!state.soundOn || !window.speechSynthesis || !window.SpeechSynthesisUtterance) return;
        try {
            window.speechSynthesis.cancel();
            var u = new window.SpeechSynthesisUtterance(text);
            u.lang = 'en-GB'; u.rate = 0.92; u.pitch = 1.15;
            window.speechSynthesis.speak(u);
        } catch (ignore) { }
    }

    function setEddy(text, speak) {
        eddy.textContent = text;
        if (speak) narrate(text);
    }

    function pointForAngle(angle, radius) {
        var r = angle * Math.PI / 180;
        return { x: 300 + Math.cos(r) * radius, y: 185 - Math.sin(r) * radius };
    }

    function renderAngle() {
        var p = pointForAngle(state.angle, 145);
        ray.setAttribute('x2', p.x.toFixed(2));
        ray.setAttribute('y2', p.y.toFixed(2));
        handle.setAttribute('cx', p.x.toFixed(2));
        handle.setAttribute('cy', p.y.toFixed(2));
        angleValue.textContent = Math.round(state.angle);

        var start = pointForAngle(0, 72);
        var end = pointForAngle(state.angle, 72);
        var large = state.angle > 180 ? 1 : 0;
        arc.setAttribute('d', 'M ' + start.x.toFixed(2) + ' ' + start.y.toFixed(2) + ' A 72 72 0 ' + large + ' 0 ' + end.x.toFixed(2) + ' ' + end.y.toFixed(2));
    }

    function setAngle(value) {
        var normalized = value % 360;
        if (normalized < 0) normalized += 360;
        if (normalized < 5) normalized = 5;
        if (normalized > 355) normalized = 355;
        state.angle = normalized;
        renderAngle();
    }

    function makeButton(label, value) {
        var button = document.createElement('button');
        button.type = 'button';
        button.className = 'gw-choice';
        button.textContent = label;
        button.setAttribute('data-answer', value);
        return button;
    }

    function scheduleHint(message) {
        clearHint();
        state.hintTimer = window.setTimeout(function () {
            if (!state.locked) setEddy(message, true);
        }, 9000);
    }

    function renderRound() {
        clearHint();
        state.locked = false;
        choices.innerHTML = '';
        roundNode.textContent = (state.roundIndex + 1) + ' / ' + state.rounds.length;
        progress.style.width = ((state.roundIndex / state.rounds.length) * 100) + '%';
        var round = currentRound();

        if (round.mode === 'build') {
            var start = round.angle + (Math.random() > .5 ? 45 : -35);
            setAngle(start);
            question.textContent = 'Build an angle of about ' + round.angle + '°';
            subquestion.textContent = 'Drag the golden signal ray, then lock your angle.';
            nudges.hidden = false;
            var lock = makeButton('Lock angle', 'lock');
            choices.appendChild(lock);
            setEddy('Estimate first, then use the degree display to refine your angle.', false);
            narrate(question.textContent);
            scheduleHint('A full turn is 360 degrees. Move the ray closer to ' + round.angle + ' degrees.');
        } else {
            setAngle(round.angle);
            question.textContent = 'What kind of angle is the signal showing?';
            subquestion.textContent = 'Look at how far the golden ray has turned.';
            nudges.hidden = true;
            ['Acute', 'Obtuse', 'Reflex'].forEach(function (kind) { choices.appendChild(makeButton(kind, kind)); });
            setEddy('Use 90° and 180° as landmarks when you compare the angle.', false);
            narrate(question.textContent);
            scheduleHint('Acute is less than 90°. Obtuse is between 90° and 180°. Reflex is more than 180°.');
        }
    }

    function correct(button, message) {
        clearHint(); state.locked = true; state.score += 200;
        if (button) button.classList.add('is-correct');
        scoreNode.textContent = state.score;
        setEddy(message, true);
        window.setTimeout(nextRound, 900);
    }

    function wrong(button, message) {
        if (button) button.classList.add('is-wrong');
        setEddy(message, true);
        window.setTimeout(function () { if (button) button.classList.remove('is-wrong'); }, 650);
    }

    function nextRound() {
        state.roundIndex += 1;
        if (state.roundIndex >= state.rounds.length) {
            progress.style.width = '100%';
            finalScore.textContent = 'Stars ' + state.score;
            complete.hidden = false;
            narrate('Observatory online. Brilliant angle work!');
            return;
        }
        renderRound();
    }

    choices.addEventListener('click', function (event) {
        if (state.locked) return;
        var button = event.target.closest('[data-answer]');
        if (!button) return;
        var round = currentRound();
        if (round.mode === 'build') {
            var difference = Math.abs(state.angle - round.angle);
            if (difference <= 6) correct(button, 'Great calibration. Your angle is close to ' + round.angle + ' degrees.');
            else wrong(button, 'Not yet. Compare your degree reading with the target and adjust the ray.');
            return;
        }
        var expected = angleKind(round.angle);
        if (button.getAttribute('data-answer') === expected) correct(button, 'Correct. ' + round.angle + ' degrees is an ' + expected.toLowerCase() + ' angle.');
        else wrong(button, 'Look at the degree measure and compare it with 90 degrees and 180 degrees.');
    });

    root.querySelectorAll('[data-nudge]').forEach(function (button) {
        button.addEventListener('click', function () { if (!state.locked) setAngle(state.angle + Number(button.getAttribute('data-nudge'))); });
    });

    function angleFromPointer(event) {
        var rect = svg.getBoundingClientRect();
        var x = (event.clientX - rect.left) * (600 / rect.width);
        var y = (event.clientY - rect.top) * (360 / rect.height);
        var degrees = Math.atan2(185 - y, x - 300) * 180 / Math.PI;
        if (degrees < 0) degrees += 360;
        return degrees;
    }

    svg.addEventListener('pointerdown', function (event) {
        if (state.locked || currentRound().mode !== 'build') return;
        state.dragging = true; svg.setPointerCapture(event.pointerId); setAngle(angleFromPointer(event));
    });
    svg.addEventListener('pointermove', function (event) { if (state.dragging && !state.locked) setAngle(angleFromPointer(event)); });
    svg.addEventListener('pointerup', function (event) { state.dragging = false; try { svg.releasePointerCapture(event.pointerId); } catch (ignore) { } });

    soundButton.addEventListener('click', function () {
        state.soundOn = !state.soundOn;
        soundButton.textContent = state.soundOn ? '🔊' : '🔇';
        soundButton.setAttribute('aria-pressed', state.soundOn ? 'true' : 'false');
        if (!state.soundOn && window.speechSynthesis) window.speechSynthesis.cancel();
    });

    fullscreenButton.addEventListener('click', function () {
        if (!document.fullscreenElement && game.requestFullscreen) game.requestFullscreen();
        else if (document.exitFullscreen) document.exitFullscreen();
    });

    root.querySelector('[data-gw-replay]').addEventListener('click', function () {
        state.rounds = makeRounds(); state.roundIndex = 0; state.score = 0; scoreNode.textContent = '0'; complete.hidden = true; renderRound();
    });

    renderAngle();
    renderRound();
}());
