(function () {
    'use strict';

    var root = document.querySelector('[data-fraction-forge-game]');
    if (!root) return;

    var expectedLessonCode = 'PED:CAMBRIDGE-INTL-MATH:S5:5N-2:BUILD';
    if (root.getAttribute('data-lesson-code') !== expectedLessonCode) {
        root.innerHTML = '<div class="gw-error">This fraction game profile is not configured for the selected lesson.</div>';
        return;
    }

    var guide = '/images/game/v9/eddy-guide.webp';
    var seeds = [
        { n: 1, d: 2 }, { n: 1, d: 3 }, { n: 2, d: 3 }, { n: 1, d: 4 },
        { n: 3, d: 4 }, { n: 2, d: 5 }, { n: 3, d: 5 }, { n: 4, d: 5 }
    ];
    var state = { rounds: [], roundIndex: 0, score: 0, soundOn: true, locked: false, hintTimer: null };

    function shuffle(values) {
        var a = values.slice();
        for (var i = a.length - 1; i > 0; i -= 1) {
            var j = Math.floor(Math.random() * (i + 1));
            var t = a[i]; a[i] = a[j]; a[j] = t;
        }
        return a;
    }

    function gcd(a, b) { while (b) { var t = b; b = a % b; a = t; } return Math.abs(a); }
    function sameFraction(a, b) { return a.n * b.d === b.n * a.d; }
    function label(f) { return f.n + '/' + f.d; }

    function makeDistractor(target, scale, delta) {
        var d = target.d * scale;
        var n = Math.max(1, Math.min(d - 1, target.n * scale + delta));
        if (n === target.n * scale) n = Math.max(1, n - 1);
        return { n: n, d: d };
    }

    function makeRounds() {
        return shuffle(seeds).map(function (target, index) {
            var scale = 2 + (index % 3);
            var answer = { n: target.n * scale, d: target.d * scale };
            var options = [answer, makeDistractor(target, scale, 1), makeDistractor(target, scale, -1)];
            if (options[1].n === options[2].n) options[2] = makeDistractor(target, scale + 1, 1);
            return { target: target, answer: answer, options: shuffle(options), mode: index % 3 };
        });
    }

    state.rounds = makeRounds();

    root.innerHTML = '' +
        '<section class="gw-game" aria-label="Fraction Forge game">' +
            '<header class="gw-hud"><div class="gw-brand"><span class="gw-brand-mark" aria-hidden="true">▦</span><span><strong>FRACTION FORGE</strong><small>Bridge workshop</small></span></div><div class="gw-hud-right"><div class="gw-pill">★ <span data-gw-score>0</span></div><div class="gw-pill" data-gw-round>1 / 8</div><button class="gw-icon" type="button" data-gw-sound>🔊</button><button class="gw-icon" type="button" data-gw-fullscreen>⛶</button></div></header>' +
            '<div class="gw-progress"><span data-gw-progress></span></div>' +
            '<main class="gw-stage">' +
                '<div class="gw-mission"><span>YOUR MISSION</span><strong data-gw-question>Repair the fraction bridge</strong><small data-gw-subquestion>Choose a tile that covers the same amount.</small></div>' +
                '<div class="gw-fraction-board">' +
                    '<section class="gw-fraction-target"><h3>Bridge opening</h3><div class="gw-frac-label" data-target-label></div><div class="gw-frac-bar" data-target-bar></div></section>' +
                    '<section class="gw-fraction-options" data-fraction-options aria-label="Equivalent fraction choices"></section>' +
                '</div>' +
            '</main>' +
            '<div class="gw-eddy"><img src="' + guide + '" alt="Eddy" /><div><span>EDDY</span><p data-gw-eddy>Equivalent fractions name the same amount even when the pieces are split differently.</p></div></div>' +
            '<div class="gw-complete" data-gw-complete hidden><div class="gw-complete-card"><img src="' + guide + '" alt="Eddy celebrating" /><p>ADVENTURE COMPLETE</p><h2>Bridge restored!</h2><p>You matched equivalent fractions by comparing the amount, not just the numbers.</p><strong data-gw-final-score></strong><br/><button class="gw-primary" type="button" data-gw-replay>Play again</button></div></div>' +
        '</section>';

    var game = root.querySelector('.gw-game');
    var question = root.querySelector('[data-gw-question]');
    var subquestion = root.querySelector('[data-gw-subquestion]');
    var targetLabel = root.querySelector('[data-target-label]');
    var targetBar = root.querySelector('[data-target-bar]');
    var optionsNode = root.querySelector('[data-fraction-options]');
    var eddy = root.querySelector('[data-gw-eddy]');
    var scoreNode = root.querySelector('[data-gw-score]');
    var roundNode = root.querySelector('[data-gw-round]');
    var progress = root.querySelector('[data-gw-progress]');
    var complete = root.querySelector('[data-gw-complete]');
    var finalScore = root.querySelector('[data-gw-final-score]');
    var soundButton = root.querySelector('[data-gw-sound]');
    var fullscreenButton = root.querySelector('[data-gw-fullscreen]');

    function currentRound() { return state.rounds[state.roundIndex]; }
    function clearHint() { if (state.hintTimer !== null) { window.clearTimeout(state.hintTimer); state.hintTimer = null; } }

    function narrate(text) {
        if (!state.soundOn || !window.speechSynthesis || !window.SpeechSynthesisUtterance) return;
        try {
            window.speechSynthesis.cancel();
            var u = new window.SpeechSynthesisUtterance(text); u.lang = 'en-GB'; u.rate = .92; u.pitch = 1.15;
            window.speechSynthesis.speak(u);
        } catch (ignore) { }
    }

    function setEddy(text, speak) { eddy.textContent = text; if (speak) narrate(text); }

    function fillBar(node, fraction) {
        node.innerHTML = '';
        node.style.gridTemplateColumns = 'repeat(' + fraction.d + ', 1fr)';
        for (var i = 0; i < fraction.d; i += 1) {
            var part = document.createElement('span');
            part.className = 'gw-frac-segment' + (i < fraction.n ? ' filled' : '');
            node.appendChild(part);
        }
    }

    function optionCard(fraction) {
        var card = document.createElement('button');
        card.type = 'button';
        card.className = 'gw-fraction-option';
        card.setAttribute('data-n', fraction.n);
        card.setAttribute('data-d', fraction.d);
        var value = document.createElement('div'); value.className = 'gw-frac-label'; value.textContent = label(fraction);
        var bar = document.createElement('div'); bar.className = 'gw-frac-bar'; fillBar(bar, fraction);
        card.appendChild(value); card.appendChild(bar);
        return card;
    }

    function scheduleHint(round) {
        clearHint();
        state.hintTimer = window.setTimeout(function () {
            if (!state.locked) setEddy('Compare the shaded part of each bar. Equivalent fractions cover exactly the same amount.', true);
        }, 9000);
    }

    function renderRound() {
        clearHint(); state.locked = false; optionsNode.innerHTML = '';
        var round = currentRound();
        roundNode.textContent = (state.roundIndex + 1) + ' / ' + state.rounds.length;
        progress.style.width = ((state.roundIndex / state.rounds.length) * 100) + '%';
        targetLabel.textContent = label(round.target); fillBar(targetBar, round.target);

        if (round.mode === 0) {
            question.textContent = 'Which bridge tile is equivalent to ' + label(round.target) + '?';
            subquestion.textContent = 'Choose the tile that covers the same fraction of the whole.';
            setEddy('The pieces can be smaller, but the shaded amount must stay the same.', false);
        } else if (round.mode === 1) {
            question.textContent = 'Find another name for ' + label(round.target); 
            subquestion.textContent = 'Use the bars to compare the fractions.';
            setEddy('Equivalent fractions can have different numerators and denominators.', false);
        } else {
            question.textContent = 'Which tile will fit this opening exactly?';
            subquestion.textContent = 'Match the shaded amount, not the size of each small piece.';
            setEddy('Look at the whole bar first, then compare how much of it is shaded.', false);
        }
        narrate(question.textContent);
        round.options.forEach(function (fraction) { optionsNode.appendChild(optionCard(fraction)); });
        scheduleHint(round);
    }

    function nextRound() {
        state.roundIndex += 1;
        if (state.roundIndex >= state.rounds.length) {
            progress.style.width = '100%'; finalScore.textContent = 'Stars ' + state.score; complete.hidden = false;
            narrate('Bridge restored. Excellent fraction reasoning!'); return;
        }
        renderRound();
    }

    optionsNode.addEventListener('click', function (event) {
        if (state.locked) return;
        var card = event.target.closest('.gw-fraction-option'); if (!card) return;
        var selected = { n: Number(card.getAttribute('data-n')), d: Number(card.getAttribute('data-d')) };
        var round = currentRound();
        if (sameFraction(selected, round.target)) {
            clearHint(); state.locked = true; state.score += 200; scoreNode.textContent = state.score; card.classList.add('is-correct');
            var factor = selected.d / round.target.d;
            setEddy('Correct. ' + label(round.target) + ' and ' + label(selected) + ' cover the same amount' + (Number.isInteger(factor) ? ' because both parts were scaled by ' + factor + '.' : '.'), true);
            window.setTimeout(nextRound, 950);
        } else {
            card.classList.add('is-wrong');
            setEddy('Not quite. Compare the shaded lengths. The correct tile must cover the same part of the whole.', true);
            window.setTimeout(function () { card.classList.remove('is-wrong'); }, 700);
        }
    });

    soundButton.addEventListener('click', function () {
        state.soundOn = !state.soundOn; soundButton.textContent = state.soundOn ? '🔊' : '🔇';
        if (!state.soundOn && window.speechSynthesis) window.speechSynthesis.cancel();
    });
    fullscreenButton.addEventListener('click', function () {
        if (!document.fullscreenElement && game.requestFullscreen) game.requestFullscreen(); else if (document.exitFullscreen) document.exitFullscreen();
    });
    root.querySelector('[data-gw-replay]').addEventListener('click', function () {
        state.rounds = makeRounds(); state.roundIndex = 0; state.score = 0; scoreNode.textContent = '0'; complete.hidden = true; renderRound();
    });

    renderRound();
}());
