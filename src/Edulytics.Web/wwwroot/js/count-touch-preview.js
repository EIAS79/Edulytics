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
        countPool: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
        variants: ['sunbug', 'leafbug', 'berrybug'],
        conceptModes: ['number-change', 'what-changed', 'what-stayed'],
        guide: '/images/game/v9/eddy-guide.webp'
    };

    var copy = {
        intro: 'The Counting Grove is waking up. Count carefully, then watch what happens when the glowbugs move.',
        start: 'Start counting',
        firstPrompt: 'Touch every glowbug once. Count as you go.',
        firstCheck: 'How many glowbugs did you count?',
        zeroPrompt: 'How many glowbugs are in the empty clearing?',
        countStory: 'The glowbugs are waking up. Give each one a count number.',
        zeroStory: 'This clearing looks quiet. Take a careful look.',
        breeze: 'Whoosh! The glowbugs moved to new places.',
        conceptStory: 'The breeze has settled. Look closely at the group.',
        duplicate: 'That glowbug is already counted. Find one without a number.',
        wrongCount: 'Not quite. Look at the numbered glowbugs and count them again.',
        countHint: 'Try one glowbug at a time. Each glowbug should get exactly one number.',
        completeTitle: 'Counting Grove restored!',
        completeBody: 'You counted carefully, used zero, and discovered what changes and what stays the same when objects move.'
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
        var nonZero = shuffle(config.countPool.filter(function (value) { return value !== 0; }))
            .slice(0, config.roundCount - 1);
        var values = shuffle([0].concat(nonZero));
        var variantOffset = Math.floor(Math.random() * config.variants.length);
        var conceptOrder = shuffle(config.conceptModes);
        var conceptIndex = 0;

        return values.map(function (value, index) {
            var conceptMode = 'zero';
            if (value !== 0) {
                if (conceptIndex > 0 && conceptIndex % conceptOrder.length === 0) {
                    conceptOrder = shuffle(config.conceptModes);
                }
                conceptMode = conceptOrder[conceptIndex % conceptOrder.length];
                conceptIndex += 1;
            }

            return {
                count: value,
                variant: config.variants[(index + variantOffset) % config.variants.length],
                conceptMode: conceptMode
            };
        });
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
                '<div class="cg-number-dock" data-cg-number-dock hidden aria-label="Choose an answer"></div>' +
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

    function setEddy(message) {
        eddyText.textContent = message;
    }

    function narrate(message) {
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

    function present(question, eddyMessage) {
        setEddy(eddyMessage);
        narrate(question);
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

    function conceptDefinition(round) {
        var context = 'You counted ' + String(round.count) + ' glowbugs before. A breeze moved them to new places.';

        if (round.conceptMode === 'what-changed') {
            return {
                question: 'What changed after the breeze?',
                context: context,
                choices: ['Places', 'Number'],
                correct: 'Places',
                hint: 'Compare where the glowbugs are with how many glowbugs there are.',
                success: 'Correct. Their places changed, but there are still ' + String(round.count) + ' glowbugs.'
            };
        }

        if (round.conceptMode === 'what-stayed') {
            return {
                question: 'What stayed the same?',
                context: context,
                choices: ['Number', 'Places'],
                correct: 'Number',
                hint: 'Think about the total you counted before the breeze.',
                success: 'Correct. The number stayed ' + String(round.count) + ' even though the glowbugs moved.'
            };
        }

        return {
            question: 'Did the number of glowbugs change?',
            context: context,
            choices: ['Yes', 'No'],
            correct: 'No',
            hint: 'Did any glowbug leave or join the group?',
            success: 'Correct. There are still ' + String(round.count) + ' glowbugs. Moving them did not change the total.'
        };
    }

    function scheduleHint() {
        clearHint();
        state.hintTimer = window.setTimeout(function () {
            if (state.phase === 'count') {
                var objects = field.querySelectorAll('.cg-glowbug');
                var remaining = [];
                var i;
                for (i = 0; i < objects.length; i += 1) {
                    if (!state.counted[objects[i].getAttribute('data-cg-object')]) remaining.push(objects[i]);
                }
                if (remaining.length) {
                    remaining[Math.floor(Math.random() * remaining.length)].classList.add('is-hint');
                    setFeedback(copy.countHint, 'info');
                }
                return;
            }

            if (state.phase === 'concept-confirm') {
                var definition = conceptDefinition(currentRound());
                setFeedback(definition.hint, 'info');
                narrate(definition.hint);
            }
        }, 9000);
    }

    function answerChoices(answer) {
        var candidates = shuffle([
            answer,
            clamp(answer - 1, 0, 10),
            clamp(answer + 1, 0, 10),
            clamp(answer - 2, 0, 10),
            clamp(answer + 2, 0, 10),
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10
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
        state.phase = 'count-confirm';
        state.locked = false;
        promptNode.textContent = answer === 0 ? copy.zeroPrompt : copy.firstCheck;
        subpromptNode.textContent = answer === 0 ? 'Look carefully at the clearing.' : 'Choose the matching number stone.';
        numberDock.innerHTML = '';
        answerChoices(answer).forEach(function (value) {
            var button = document.createElement('button');
            button.type = 'button';
            button.className = 'cg-number-stone';
            button.textContent = String(value);
            button.setAttribute('aria-label', 'Choose ' + String(value));
            button.addEventListener('click', function () {
                chooseCountAnswer(value, button);
            });
            numberDock.appendChild(button);
        });
        numberDock.hidden = false;
        feedback.hidden = true;
        present(promptNode.textContent, answer === 0 ? copy.zeroStory : copy.countStory);
    }

    function showConceptCheck() {
        var definition = conceptDefinition(currentRound());
        state.phase = 'concept-confirm';
        state.locked = false;
        counter.hidden = true;
        promptNode.textContent = definition.question;
        subpromptNode.textContent = definition.context;
        numberDock.innerHTML = '';
        feedback.hidden = true;

        shuffle(definition.choices).forEach(function (label) {
            var button = document.createElement('button');
            button.type = 'button';
            button.className = 'cg-number-stone';
            button.textContent = label;
            button.setAttribute('aria-label', 'Choose ' + label);
            button.addEventListener('click', function () {
                chooseConceptAnswer(label, button);
            });
            numberDock.appendChild(button);
        });

        numberDock.hidden = false;
        present(definition.question, copy.conceptStory);
        scheduleHint();
    }

    function touchObject(button) {
        if (state.locked || state.phase !== 'count') return;
        var id = button.getAttribute('data-cg-object');
        if (state.counted[id]) {
            button.classList.remove('is-nudge');
            void button.offsetWidth;
            button.classList.add('is-nudge');
            setFeedback(copy.duplicate, 'gentle');
            narrate(copy.duplicate);
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
            objects[i].setAttribute('aria-label', 'Glowbug ' + String(i + 1));
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

    function renderCountPhase() {
        state.phase = 'count';
        state.counted = {};
        state.countedTotal = 0;
        state.locked = false;
        numberDock.hidden = true;
        numberDock.innerHTML = '';
        feedback.hidden = true;
        renderObjects(currentRound().count, currentRound().variant, false);

        if (currentRound().count === 0) {
            counter.hidden = true;
            promptNode.textContent = copy.zeroPrompt;
            subpromptNode.textContent = 'Look carefully at the clearing.';
            state.locked = true;
            setEddy(copy.zeroStory);
            window.setTimeout(showNumberStones, 500);
            return;
        }

        counter.hidden = false;
        countValue.textContent = '0';
        promptNode.textContent = copy.firstPrompt;
        subpromptNode.textContent = 'Tap each glowbug once.';
        setEddy(copy.countStory);
        narrate(copy.firstPrompt);
        scheduleHint();
    }

    function beginBreeze() {
        state.phase = 'breeze';
        clearHint();
        promptNode.textContent = copy.breeze;
        subpromptNode.textContent = 'Watch where the glowbugs land.';
        numberDock.hidden = true;
        counter.hidden = true;
        feedback.hidden = true;
        game.classList.add('is-breezy');
        renderObjects(currentRound().count, currentRound().variant, true);
        setEddy('Hold on — a breeze is sweeping through the grove!');
        narrate(copy.breeze);

        window.setTimeout(function () {
            game.classList.remove('is-breezy');
            showConceptCheck();
        }, 950);
    }

    function finishGame() {
        state.phase = 'complete';
        clearHint();
        root.querySelector('[data-cg-final-score]').textContent = String(state.score);
        complete.hidden = false;
        progressNode.style.width = '100%';
        progressBar.setAttribute('aria-valuenow', '100');
        setEddy('You restored the Counting Grove. Brilliant work!');
        narrate(copy.completeBody);
    }

    function finishRound() {
        state.roundIndex += 1;
        if (state.roundIndex >= state.rounds.length) {
            finishGame();
            return;
        }
        syncHud();
        renderCountPhase();
    }

    function chooseCountAnswer(value, button) {
        if (state.locked || state.phase !== 'count-confirm') return;
        var answer = currentRound().count;
        if (value !== answer) {
            button.classList.remove('is-wrong');
            void button.offsetWidth;
            button.classList.add('is-wrong');
            setFeedback(copy.wrongCount, 'try');
            narrate(copy.wrongCount);
            return;
        }

        state.locked = true;
        button.classList.add('is-correct');
        chime(660, 0.11, 0);
        chime(880, 0.14, 0.09);

        if (answer === 0) {
            state.score += 250;
            syncHud();
            var zeroMessage = 'Correct. Zero means there are no glowbugs in the clearing.';
            setFeedback(zeroMessage, 'success');
            setEddy('Exactly — an empty group has a total of zero.');
            narrate(zeroMessage);
            window.setTimeout(finishRound, 1250);
            return;
        }

        state.score += 100;
        syncHud();
        var firstMessage = 'Correct — you counted ' + String(answer) + ' glowbugs.';
        setFeedback(firstMessage, 'success');
        setEddy('Great counting. Now watch what the breeze does.');
        narrate(firstMessage);
        window.setTimeout(beginBreeze, 1050);
    }

    function chooseConceptAnswer(label, button) {
        if (state.locked || state.phase !== 'concept-confirm') return;
        var definition = conceptDefinition(currentRound());

        if (label !== definition.correct) {
            button.classList.remove('is-wrong');
            void button.offsetWidth;
            button.classList.add('is-wrong');
            setFeedback(definition.hint, 'try');
            narrate(definition.hint);
            clearHint();
            scheduleHint();
            return;
        }

        state.locked = true;
        clearHint();
        button.classList.add('is-correct');
        state.score += 150;
        syncHud();
        chime(660, 0.11, 0);
        chime(880, 0.14, 0.09);
        setFeedback(definition.success, 'success');
        setEddy('Excellent thinking!');
        narrate(definition.success);
        window.setTimeout(finishRound, 1350);
    }

    function startGame() {
        if (state.phase !== 'intro') return;
        eddyPanel.classList.add('is-compact');
        startButton.hidden = true;
        renderCountPhase();
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
        renderCountPhase();
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
        version: '1.3.0',
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