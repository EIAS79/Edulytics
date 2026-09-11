(function () {
    'use strict';

    var root = document.querySelector('[data-count-touch-game]');
    if (!root) return;

    var expectedLessonCode = 'PED:CAMBRIDGE-INTL-MATH:S1:L01';
    if (root.getAttribute('data-lesson-code') !== expectedLessonCode) {
        root.innerHTML = '<div class="cg-preview-error">This game profile is not configured for the selected lesson.</div>';
        return;
    }

    var config = {
        roundCount: 10,
        countPool: [3, 4, 5, 6, 7, 8, 9, 10],
        variants: ['sunbug', 'leafbug', 'berrybug'],
        guide: '/images/game/v9/eddy-guide.webp'
    };

    var copy = {
        intro: 'Touch each glowbug once and count as you go. Then a breeze will move the same glowbugs. Count them again to check whether the total changed.',
        start: 'Start counting',
        firstPrompt: 'Touch every glowbug once. Count as you go.',
        firstCheck: 'How many glowbugs did you count? Choose the number stone.',
        breeze: 'Whoosh! The same glowbugs moved to new places. None flew away.',
        recountPrompt: 'Count the same glowbugs again. Touch each one once.',
        secondCheck: 'How many are there now? Choose the number stone.',
        duplicate: 'That glowbug is already counted. Find one without a number.',
        wrong: 'Not quite. Look at the numbered glowbugs and count them again.',
        hint: 'Try one glowbug at a time. Each glowbug should get exactly one number.',
        completeTitle: 'Counting Grove restored!',
        completeBody: 'You counted carefully and checked that changing the arrangement does not change the total.'
    };

    function shuffle(values) {
        var result = values.slice();
        var i;
        for (i = result.length - 1; i > 0; i -= 1) {
            var j = Math.floor(Math.random() * (i + 1));
            var temp = result[i];
            result[i] = result[j];
            result[j] = temp;
        }
        return result;
    }

    function clamp(value, min, max) {
        return Math.max(min, Math.min(max, value));
    }

    function makeRounds() {
        var rounds = [];
        var bag = [];
        while (rounds.length < config.roundCount) {
            if (!bag.length) bag = shuffle(config.countPool);
            var value = bag.shift();
            if (rounds.length && rounds[rounds.length - 1].count === value) {
                bag.push(value);
                continue;
            }
            rounds.push({
                count: value,
                variant: config.variants[rounds.length % config.variants.length]
            });
        }
        return rounds;
    }

    function layoutPoints(count) {
        var base = [
            { x: 11, y: 24 }, { x: 27, y: 20 }, { x: 44, y: 27 }, { x: 61, y: 19 }, { x: 79, y: 26 },
            { x: 18, y: 50 }, { x: 35, y: 55 }, { x: 53, y: 48 }, { x: 70, y: 56 }, { x: 87, y: 49 },
            { x: 31, y: 70 }, { x: 66, y: 69 }
        ];
        return shuffle(base).slice(0, count).map(function (point) {
            return {
                x: clamp(point.x + (Math.random() * 6 - 3), 8, 90),
                y: clamp(point.y + (Math.random() * 6 - 3), 16, 72)
            };
        });
    }

    var state = {
        rounds: makeRounds(),
        roundIndex: 0,
        phase: 'intro',
        counted: {},
        countedTotal: 0,
        score: 0,
        soundOn: true,
        locked: false,
        hintTimer: null,
        audioContext: null
    };

    root.innerHTML = '' +
        '<section class="cg-game" aria-label="Counting Grove game">' +
            '<div class="cg-sky" aria-hidden="true"><span></span><span></span><span></span></div>' +
            '<div class="cg-hills cg-hills-back" aria-hidden="true"></div>' +
            '<div class="cg-hills cg-hills-front" aria-hidden="true"></div>' +
            '<header class="cg-hud">' +
                '<div class="cg-world-title">' +
                    '<span class="cg-lantern-mark" aria-hidden="true"><i></i></span>' +
                    '<span><strong>COUNTING GROVE</strong><small>Discovery clearing</small></span>' +
                '</div>' +
                '<div class="cg-hud-right">' +
                    '<div class="cg-score" aria-label="Score"><span aria-hidden="true">★</span><strong data-cg-score>0</strong></div>' +
                    '<div class="cg-round" data-cg-round>1 / 10</div>' +
                    '<button class="cg-icon-btn" type="button" data-cg-sound aria-pressed="true" aria-label="Turn sound off">🔊</button>' +
                    '<button class="cg-icon-btn" type="button" data-cg-fullscreen aria-label="Enter fullscreen">⛶</button>' +
                '</div>' +
            '</header>' +
            '<div class="cg-progress" role="progressbar" aria-valuemin="0" aria-valuemax="100" aria-valuenow="0"><span data-cg-progress></span></div>' +
            '<main class="cg-stage">' +
                '<div class="cg-mission" aria-live="polite">' +
                    '<span class="cg-mission-kicker">YOUR MISSION</span>' +
                    '<strong data-cg-prompt>Wake the Counting Grove</strong>' +
                    '<small data-cg-subprompt>Touch each glowbug once and count as you go.</small>' +
                '</div>' +
                '<div class="cg-field" data-cg-field aria-label="Interactive counting area"></div>' +
                '<div class="cg-counter-orb" data-cg-counter hidden><span>Counted</span><strong data-cg-count-value>0</strong></div>' +
                '<div class="cg-number-dock" data-cg-number-dock hidden aria-label="Choose the total"></div>' +
                '<div class="cg-feedback" data-cg-feedback aria-live="polite" hidden></div>' +
            '</main>' +
            '<div class="cg-eddy-panel" data-cg-eddy-panel>' +
                '<img src="' + config.guide + '" alt="Eddy, the Edulytics guide" />' +
                '<div><span>EDDY</span><p data-cg-eddy-text>' + copy.intro + '</p></div>' +
                '<button class="cg-primary" type="button" data-cg-start>' + copy.start + '</button>' +
            '</div>' +
            '<div class="cg-complete" data-cg-complete hidden>' +
                '<div class="cg-complete-card">' +
                    '<img src="' + config.guide + '" alt="Eddy celebrating" />' +
                    '<p class="cg-complete-kicker">ADVENTURE COMPLETE</p>' +
                    '<h2>' + copy.completeTitle + '</h2>' +
                    '<p>' + copy.completeBody + '</p>' +
                    '<div class="cg-final-score"><span>Stars</span><strong data-cg-final-score>0</strong></div>' +
                    '<button class="cg-primary" type="button" data-cg-replay>Play again</button>' +
                '</div>' +
            '</div>' +
        '</section>';

    var game = root.querySelector('.cg-game');
    var field = root.querySelector('[data-cg-field]');
    var promptNode = root.querySelector('[data-cg-prompt]');
    var subpromptNode = root.querySelector('[data-cg-subprompt]');
    var counter = root.querySelector('[data-cg-counter]');
    var countValue = root.querySelector('[data-cg-count-value]');
    var numberDock = root.querySelector('[data-cg-number-dock]');
    var feedback = root.querySelector('[data-cg-feedback]');
    var eddyPanel = root.querySelector('[data-cg-eddy-panel]');
    var eddyText = root.querySelector('[data-cg-eddy-text]');
    var scoreNode = root.querySelector('[data-cg-score]');
    var roundNode = root.querySelector('[data-cg-round]');
    var progressNode = root.querySelector('[data-cg-progress]');
    var progressBar = root.querySelector('.cg-progress');
    var complete = root.querySelector('[data-cg-complete]');
    var startButton = root.querySelector('[data-cg-start]');
    var soundButton = root.querySelector('[data-cg-sound]');
    var fullscreenButton = root.querySelector('[data-cg-fullscreen]');
    var replayButton = root.querySelector('[data-cg-replay]');

    function currentRound() {
        return state.rounds[state.roundIndex];
    }

    function clearHint() {
        if (state.hintTimer !== null) {
            window.clearTimeout(state.hintTimer);
            state.hintTimer = null;
        }
    }

    function setFeedback(message, kind) {
        feedback.textContent = message;
        feedback.setAttribute('data-kind', kind || 'info');
        feedback.hidden = false;
    }

    function speak(message) {
        eddyText.textContent = message;
        if (!state.soundOn || !window.speechSynthesis || !window.SpeechSynthesisUtterance) return;
        try {
            window.speechSynthesis.cancel();
            var utterance = new window.SpeechSynthesisUtterance(message);
            utterance.lang = 'en-GB';
            utterance.rate = 0.92;
            utterance.pitch = 1.18;
            window.speechSynthesis.speak(utterance);
        } catch (ignore) {
            // Voice is enhancement only.
        }
    }

    function chime(frequency, duration, delay) {
        if (!state.soundOn) return;
        try {
            var AudioCtor = window.AudioContext || window.webkitAudioContext;
            if (!AudioCtor) return;
            if (!state.audioContext) state.audioContext = new AudioCtor();
            var ctx = state.audioContext;
            var oscillator = ctx.createOscillator();
            var gain = ctx.createGain();
            var start = ctx.currentTime + (delay || 0);
            oscillator.type = 'sine';
            oscillator.frequency.setValueAtTime(frequency, start);
            gain.gain.setValueAtTime(0.0001, start);
            gain.gain.exponentialRampToValueAtTime(0.10, start + 0.012);
            gain.gain.exponentialRampToValueAtTime(0.0001, start + duration);
            oscillator.connect(gain);
            gain.connect(ctx.destination);
            oscillator.start(start);
            oscillator.stop(start + duration + 0.02);
        } catch (ignore) {
            // Audio is enhancement only.
        }
    }

    function syncHud() {
        scoreNode.textContent = String(state.score);
        roundNode.textContent = String(Math.min(state.roundIndex + 1, state.rounds.length)) + ' / ' + String(state.rounds.length);
        var progress = Math.round((state.roundIndex / state.rounds.length) * 100);
        progressNode.style.width = String(progress) + '%';
        progressBar.setAttribute('aria-valuenow', String(progress));
    }

    function scheduleHint() {
        clearHint();
        state.hintTimer = window.setTimeout(function () {
            if (state.phase !== 'count' && state.phase !== 'recount') return;
            var objects = field.querySelectorAll('.cg-glowbug');
            var remaining = [];
            var i;
            for (i = 0; i < objects.length; i += 1) {
                if (!state.counted[objects[i].getAttribute('data-cg-object')]) remaining.push(objects[i]);
            }
            if (remaining.length) {
                remaining[Math.floor(Math.random() * remaining.length)].classList.add('is-hint');
                setFeedback(copy.hint, 'info');
            }
        }, 9000);
    }

    function answerChoices(answer) {
        var candidates = shuffle([
            answer,
            clamp(answer - 1, 1, 10),
            clamp(answer + 1, 1, 10),
            clamp(answer - 2, 1, 10),
            clamp(answer + 2, 1, 10),
            3, 4, 5, 6, 7, 8, 9, 10
        ]);
        var unique = [];
        var i;
        for (i = 0; i < candidates.length && unique.length < 4; i += 1) {
            if (unique.indexOf(candidates[i]) === -1) unique.push(candidates[i]);
        }
        if (unique.indexOf(answer) === -1) unique[0] = answer;
        return shuffle(unique);
    }

    function showNumberStones() {
        var answer = currentRound().count;
        var wasRecount = state.phase === 'recount';
        state.phase = wasRecount ? 'recount-confirm' : 'count-confirm';
        state.locked = false;
        promptNode.textContent = wasRecount ? copy.secondCheck : copy.firstCheck;
        subpromptNode.textContent = wasRecount ? 'Compare this count with the first count.' : 'The numbered glowbugs show the order you counted.';
        numberDock.innerHTML = '';
        answerChoices(answer).forEach(function (value) {
            var button = document.createElement('button');
            button.type = 'button';
            button.className = 'cg-number-stone';
            button.textContent = String(value);
            button.setAttribute('aria-label', 'Choose ' + String(value));
            button.addEventListener('click', function () {
                chooseAnswer(value, button);
            });
            numberDock.appendChild(button);
        });
        numberDock.hidden = false;
        setFeedback('Choose the total below.', 'info');
    }

    function touchObject(button) {
        if (state.locked || (state.phase !== 'count' && state.phase !== 'recount')) return;
        var id = button.getAttribute('data-cg-object');
        if (state.counted[id]) {
            button.classList.remove('is-nudge');
            void button.offsetWidth;
            button.classList.add('is-nudge');
            setFeedback(copy.duplicate, 'gentle');
            return;
        }

        state.counted[id] = true;
        state.countedTotal += 1;
        button.classList.add('is-counted');
        button.classList.remove('is-hint');
        button.querySelector('.cg-order').textContent = String(state.countedTotal);
        button.setAttribute('aria-label', 'Glowbug counted as ' + String(state.countedTotal));
        countValue.textContent = String(state.countedTotal);
        chime(260 + state.countedTotal * 26, 0.05, 0);
        clearHint();

        if (state.countedTotal === currentRound().count) {
            state.locked = true;
            window.setTimeout(showNumberStones, 420);
        } else {
            scheduleHint();
        }
    }

    function renderObjects(count, variant, keepSameObjects) {
        var objects = field.querySelectorAll('.cg-glowbug');
        var i;
        if (!keepSameObjects || objects.length !== count) {
            field.innerHTML = '';
            for (i = 0; i < count; i += 1) {
                var button = document.createElement('button');
                button.type = 'button';
                button.className = 'cg-glowbug cg-' + variant;
                button.setAttribute('data-cg-object', String(i));
                button.setAttribute('aria-label', 'Glowbug ' + String(i + 1) + ', not counted');
                button.innerHTML = '<span class="cg-wings" aria-hidden="true"></span><span class="cg-glow" aria-hidden="true"></span><span class="cg-order" aria-hidden="true"></span>';
                button.addEventListener('click', (function (node) {
                    return function () { touchObject(node); };
                }(button)));
                field.appendChild(button);
            }
        }

        objects = field.querySelectorAll('.cg-glowbug');
        var points = layoutPoints(count);
        for (i = 0; i < objects.length; i += 1) {
            objects[i].classList.remove('is-counted', 'is-hint', 'is-nudge', 'is-breeze');
            objects[i].querySelector('.cg-order').textContent = '';
            objects[i].setAttribute('aria-label', 'Glowbug ' + String(i + 1) + ', not counted');
            objects[i].style.setProperty('--x', String(points[i].x) + '%');
            objects[i].style.setProperty('--y', String(points[i].y) + '%');
            objects[i].style.setProperty('--delay', String((i % 5) * -0.17) + 's');
            if (keepSameObjects) objects[i].classList.add('is-breeze');
        }
        if (keepSameObjects) {
            window.setTimeout(function () {
                var moved = field.querySelectorAll('.cg-glowbug');
                var j;
                for (j = 0; j < moved.length; j += 1) moved[j].classList.remove('is-breeze');
            }, 850);
        }
    }

    function renderCountPhase(isRecount) {
        state.phase = isRecount ? 'recount' : 'count';
        state.counted = {};
        state.countedTotal = 0;
        state.locked = false;
        counter.hidden = false;
        countValue.textContent = '0';
        numberDock.hidden = true;
        numberDock.innerHTML = '';
        feedback.hidden = true;
        promptNode.textContent = isRecount ? copy.recountPrompt : copy.firstPrompt;
        subpromptNode.textContent = isRecount ? 'The objects moved, but the group is the same.' : 'Each glowbug should receive exactly one count number.';
        renderObjects(currentRound().count, currentRound().variant, isRecount);
        scheduleHint();
    }

    function beginBreeze() {
        state.phase = 'breeze';
        promptNode.textContent = copy.breeze;
        subpromptNode.textContent = 'Watch the same glowbugs change places.';
        numberDock.hidden = true;
        counter.hidden = true;
        game.classList.add('is-breezy');
        window.setTimeout(function () {
            game.classList.remove('is-breezy');
            renderCountPhase(true);
            speak(copy.recountPrompt);
        }, 900);
    }

    function finishGame() {
        state.phase = 'complete';
        clearHint();
        root.querySelector('[data-cg-final-score]').textContent = String(state.score);
        complete.hidden = false;
        progressNode.style.width = '100%';
        progressBar.setAttribute('aria-valuenow', '100');
        speak(copy.completeBody);
    }

    function finishRound() {
        state.roundIndex += 1;
        if (state.roundIndex >= state.rounds.length) {
            finishGame();
            return;
        }
        syncHud();
        renderCountPhase(false);
        speak(copy.firstPrompt);
    }

    function chooseAnswer(value, button) {
        if (state.locked || (state.phase !== 'count-confirm' && state.phase !== 'recount-confirm')) return;
        var answer = currentRound().count;
        if (value !== answer) {
            button.classList.remove('is-wrong');
            void button.offsetWidth;
            button.classList.add('is-wrong');
            setFeedback(copy.wrong, 'try');
            speak(copy.wrong);
            return;
        }

        state.locked = true;
        button.classList.add('is-correct');
        chime(660, 0.11, 0);
        chime(880, 0.14, 0.09);

        if (state.phase === 'count-confirm') {
            state.score += 100;
            syncHud();
            var firstMessage = 'Yes — ' + String(answer) + '. Now watch what happens when they move.';
            setFeedback(firstMessage, 'success');
            speak(firstMessage);
            window.setTimeout(beginBreeze, 1150);
        } else {
            state.score += 150;
            syncHud();
            var secondMessage = 'Exactly ' + String(answer) + '. Moving the glowbugs did not change how many there are.';
            setFeedback(secondMessage, 'success');
            speak(secondMessage);
            window.setTimeout(finishRound, 1250);
        }
    }

    function startGame() {
        if (state.phase !== 'intro') return;
        eddyPanel.classList.add('is-compact');
        startButton.hidden = true;
        renderCountPhase(false);
        speak(copy.firstPrompt);
    }

    function replayGame() {
        state.rounds = makeRounds();
        state.roundIndex = 0;
        state.score = 0;
        state.counted = {};
        state.countedTotal = 0;
        state.locked = false;
        complete.hidden = true;
        syncHud();
        renderCountPhase(false);
        speak(copy.firstPrompt);
    }

    function toggleSound() {
        state.soundOn = !state.soundOn;
        soundButton.setAttribute('aria-pressed', state.soundOn ? 'true' : 'false');
        soundButton.setAttribute('aria-label', state.soundOn ? 'Turn sound off' : 'Turn sound on');
        soundButton.textContent = state.soundOn ? '🔊' : '🔇';
        if (!state.soundOn && window.speechSynthesis) window.speechSynthesis.cancel();
    }

    function syncFullscreenLabel() {
        var active = document.fullscreenElement === game;
        fullscreenButton.textContent = active ? '⤢' : '⛶';
        fullscreenButton.setAttribute('aria-label', active ? 'Exit fullscreen' : 'Enter fullscreen');
    }

    function toggleFullscreen() {
        try {
            if (!document.fullscreenElement && game.requestFullscreen) {
                var entering = game.requestFullscreen();
                if (entering && entering.catch) entering.catch(function () { setFeedback('Fullscreen is not available in this browser.', 'info'); });
            } else if (document.fullscreenElement && document.exitFullscreen) {
                var leaving = document.exitFullscreen();
                if (leaving && leaving.catch) leaving.catch(function () { setFeedback('Fullscreen is not available in this browser.', 'info'); });
            }
        } catch (ignore) {
            setFeedback('Fullscreen is not available in this browser.', 'info');
        }
    }

    startButton.addEventListener('click', startGame);
    soundButton.addEventListener('click', toggleSound);
    fullscreenButton.addEventListener('click', toggleFullscreen);
    replayButton.addEventListener('click', replayGame);
    document.addEventListener('fullscreenchange', syncFullscreenLabel);
    syncHud();

    window.__countTouchPreviewRuntime = {
        version: '1.1.0',
        destroy: function () {
            clearHint();
            document.removeEventListener('fullscreenchange', syncFullscreenLabel);
            if (window.speechSynthesis) window.speechSynthesis.cancel();
            try {
                if (state.audioContext && state.audioContext.close) state.audioContext.close();
            } catch (ignore) { }
            root.innerHTML = '';
        }
    };
}());
