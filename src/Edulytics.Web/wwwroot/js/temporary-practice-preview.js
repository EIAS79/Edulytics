(() => {
    const root = document.querySelector('[data-temporary-practice-preview]');
    if (!root) return;

    const tabs = Array.from(root.querySelectorAll('[data-preview-tab]'));
    const panels = Array.from(root.querySelectorAll('[data-preview-panel]'));
    const questionText = root.querySelector('[data-preview-question]');
    const questionNumber = root.querySelector('[data-preview-question-number]');
    const counter = root.querySelector('[data-preview-counter]');
    const progress = root.querySelector('[data-preview-progress]');
    const answer = root.querySelector('[data-preview-answer]');
    const feedback = root.querySelector('[data-preview-feedback]');
    const check = root.querySelector('[data-preview-check]');
    const joinButton = root.querySelector('[data-preview-join]');
    const gameStage = root.querySelector('[data-preview-game-stage]');
    const answerDock = root.querySelector('[data-preview-answer-dock]');
    const finish = root.querySelector('[data-preview-finish]');
    const finalScore = root.querySelector('[data-preview-final-score]');
    const finalDetail = root.querySelector('[data-preview-final-detail]');
    const restart = root.querySelector('[data-preview-restart]');
    const soundToggle = root.querySelector('[data-preview-sound]');
    const speech = root.querySelector('[data-preview-speech]');
    const mascot = root.querySelector('[data-preview-mascot]');
    const dockTitle = root.querySelector('[data-preview-dock-title]');
    const dockCopy = root.querySelector('[data-preview-dock-copy]');
    const leftLabel = root.querySelector('[data-preview-left-label]');
    const rightLabel = root.querySelector('[data-preview-right-label]');
    const leftGroup = root.querySelector('[data-preview-left-group]');
    const rightGroup = root.querySelector('[data-preview-right-group]');
    const joinedGroup = root.querySelector('[data-preview-joined-group]');
    const leftCard = root.querySelector('[data-preview-left-card]');
    const rightCard = root.querySelector('[data-preview-right-card]');
    const joinedCard = root.querySelector('[data-preview-joined-card]');
    const keyButtons = Array.from(root.querySelectorAll('[data-practice-key]'));

    let questions = [];
    let index = 0;
    let correct = 0;
    let locked = false;
    let joined = false;
    let soundOn = true;
    let audioContext = null;
    let speechLanguage = inferSpeechLanguage();
    let currentSpeechText = '';
    let preferredVoice = null;

    const copy = {
        en: {
            countJoin: current => `Question ${index + 1}. Bring the two groups together. Count ${current.left} plus ${current.right}, then tap Join the groups.`,
            afterJoin: current => `${current.left} plus ${current.right}. How many are there altogether?`,
            correct: current => `Brilliant! ${current.left} plus ${current.right} equals ${current.sum}.`,
            incorrect: current => `Good try. Count the joined group once more. There are ${current.sum} altogether.`,
            finish: percent => `Math adventure complete. You scored ${percent} percent. Great work!`
        },
        pl: {
            countJoin: current => `Pytanie ${index + 1}. Połącz dwie grupy. Policz ${current.left} plus ${current.right}, a potem naciśnij przycisk połącz grupy.`,
            afterJoin: current => `${current.left} plus ${current.right}. Ile jest razem?`,
            correct: current => `Świetnie! ${current.left} plus ${current.right} równa się ${current.sum}.`,
            incorrect: current => `Dobra próba. Policz połączoną grupę jeszcze raz. Razem jest ${current.sum}.`,
            finish: percent => `Przygoda matematyczna zakończona. Twój wynik to ${percent} procent. Świetna robota!`
        }
    };

    installPreviewMotionStyles();
    prepareMascotForSpeech();
    refreshVoices();

    function inferSpeechLanguage() {
        const htmlLanguage = (document.documentElement.lang || '').toLowerCase();
        if (htmlLanguage.startsWith('pl')) return 'pl-PL';

        const chips = Array.from(root.querySelectorAll('.preview-chip'))
            .map(chip => (chip.textContent || '').toLowerCase())
            .join(' ');
        if (/polish|polska|polski|podstawa/.test(chips)) return 'pl-PL';

        return 'en-GB';
    }

    function activeCopy() {
        return speechLanguage.toLowerCase().startsWith('pl') ? copy.pl : copy.en;
    }

    function installPreviewMotionStyles() {
        if (document.getElementById('temporary-practice-motion-styles')) return;
        const style = document.createElement('style');
        style.id = 'temporary-practice-motion-styles';
        style.textContent = `
            .kid-game-shell {
                background-size: 100% 190%;
                animation: preview-sky-shift 9s ease-in-out infinite alternate;
            }
            .kid-cloud-one { animation: preview-cloud-drift-one 14s ease-in-out infinite alternate; }
            .kid-cloud-two { animation: preview-cloud-drift-two 18s ease-in-out infinite alternate; }
            .kid-sun { animation: preview-sun-float 6.5s ease-in-out infinite; }
            .kid-game-shell::before { animation: preview-hill-breathe-one 8s ease-in-out infinite alternate; }
            .kid-game-shell::after { animation: preview-hill-breathe-two 10s ease-in-out infinite alternate; }
            .kid-mascot {
                clip-path: inset(0 12% 0 12% round 26px);
                transform-origin: 50% 88%;
                will-change: transform;
            }
            .kid-mascot:not(.is-celebrating):not(.is-talking) {
                animation: preview-mascot-idle 3.2s ease-in-out infinite;
            }
            .kid-mascot.is-talking {
                animation: preview-mascot-talk .34s ease-in-out infinite alternate;
            }
            .kid-speech.is-speaking {
                animation: preview-speech-pulse .68s ease-in-out infinite alternate;
                box-shadow: 0 12px 28px rgba(40,90,112,.18), 0 0 0 5px rgba(255,255,255,.28);
            }
            .kid-mascot[role="button"] { cursor: pointer; }
            .kid-mascot[role="button"]:focus-visible {
                outline: 4px solid rgba(23,127,145,.35);
                outline-offset: 4px;
            }
            @keyframes preview-sky-shift {
                0% { background-position: 50% 0%; }
                100% { background-position: 50% 38%; }
            }
            @keyframes preview-cloud-drift-one {
                0% { transform: translate3d(-26px, 0, 0) scale(1); }
                50% { transform: translate3d(44px, -10px, 0) scale(1.04); }
                100% { transform: translate3d(92px, 4px, 0) scale(.98); }
            }
            @keyframes preview-cloud-drift-two {
                0% { transform: translate3d(28px, 0, 0) scale(.78); }
                50% { transform: translate3d(-36px, 8px, 0) scale(.82); }
                100% { transform: translate3d(-86px, -5px, 0) scale(.76); }
            }
            @keyframes preview-sun-float {
                0%,100% { transform: translateY(0) rotate(0deg) scale(1); }
                50% { transform: translateY(-12px) rotate(4deg) scale(1.045); }
            }
            @keyframes preview-hill-breathe-one {
                0% { transform: translateX(-12px) rotate(-7deg) scale(1); }
                100% { transform: translateX(22px) rotate(-5deg) scale(1.04); }
            }
            @keyframes preview-hill-breathe-two {
                0% { transform: translateX(15px) rotate(8deg) scale(1); }
                100% { transform: translateX(-18px) rotate(6deg) scale(1.035); }
            }
            @keyframes preview-mascot-idle {
                0%,100% { transform: translateY(0) rotate(-.8deg) scale(1); }
                50% { transform: translateY(-9px) rotate(.8deg) scale(1.018); }
            }
            @keyframes preview-mascot-talk {
                0% { transform: translateY(-2px) rotate(-1.8deg) scale(1.01, .995); }
                100% { transform: translateY(-9px) rotate(1.8deg) scale(1.025, 1.015); }
            }
            @keyframes preview-speech-pulse {
                0% { transform: translateY(0) scale(1); }
                100% { transform: translateY(-3px) scale(1.012); }
            }
            @media (prefers-reduced-motion: reduce) {
                .kid-game-shell,
                .kid-cloud-one,
                .kid-cloud-two,
                .kid-sun,
                .kid-game-shell::before,
                .kid-game-shell::after,
                .kid-mascot,
                .kid-speech { animation: none !important; }
            }
        `;
        document.head.appendChild(style);
    }

    function prepareMascotForSpeech() {
        if (!mascot) return;
        mascot.setAttribute('role', 'button');
        mascot.setAttribute('tabindex', '0');
        mascot.setAttribute('aria-label', 'Read the current question aloud');
        mascot.title = 'Tap the character to hear the question again';
    }

    function setTab(name) {
        tabs.forEach(tab => {
            const active = tab.dataset.previewTab === name;
            tab.classList.toggle('is-active', active);
            tab.setAttribute('aria-selected', active ? 'true' : 'false');
        });
        panels.forEach(panel => {
            panel.hidden = panel.dataset.previewPanel !== name;
        });
    }

    function ensureAudioContext() {
        if (!soundOn) return null;
        const Context = window.AudioContext || window.webkitAudioContext;
        if (!Context) return null;
        if (!audioContext) audioContext = new Context();
        if (audioContext.state === 'suspended') audioContext.resume().catch(() => {});
        return audioContext;
    }

    function tone(frequency, duration = 0.08, delay = 0, volume = 0.045, type = 'sine') {
        const context = ensureAudioContext();
        if (!context) return;

        const oscillator = context.createOscillator();
        const gain = context.createGain();
        const start = context.currentTime + delay;
        const end = start + duration;

        oscillator.type = type;
        oscillator.frequency.setValueAtTime(frequency, start);
        gain.gain.setValueAtTime(0.0001, start);
        gain.gain.exponentialRampToValueAtTime(volume, start + 0.012);
        gain.gain.exponentialRampToValueAtTime(0.0001, end);
        oscillator.connect(gain);
        gain.connect(context.destination);
        oscillator.start(start);
        oscillator.stop(end + 0.02);
    }

    function playClick() {
        tone(520, 0.045, 0, 0.025, 'triangle');
    }

    function playJoin() {
        tone(330, 0.08, 0, 0.035, 'triangle');
        tone(440, 0.08, 0.07, 0.035, 'triangle');
        tone(550, 0.1, 0.14, 0.04, 'triangle');
    }

    function playCorrect() {
        tone(523.25, 0.13, 0, 0.045, 'triangle');
        tone(659.25, 0.13, 0.09, 0.045, 'triangle');
        tone(783.99, 0.18, 0.18, 0.05, 'triangle');
    }

    function playIncorrect() {
        tone(330, 0.11, 0, 0.03, 'sine');
        tone(277, 0.14, 0.12, 0.028, 'sine');
    }

    function playFinish() {
        tone(523.25, 0.14, 0, 0.045, 'triangle');
        tone(659.25, 0.14, 0.11, 0.045, 'triangle');
        tone(783.99, 0.14, 0.22, 0.05, 'triangle');
        tone(1046.5, 0.25, 0.34, 0.055, 'triangle');
    }

    function updateSoundToggle() {
        if (!soundToggle) return;
        soundToggle.textContent = soundOn ? '🔊 Sound on' : '🔇 Sound off';
        soundToggle.setAttribute('aria-pressed', soundOn ? 'true' : 'false');
        soundToggle.setAttribute('aria-label', soundOn ? 'Turn game sounds and voice off' : 'Turn game sounds and voice on');
    }

    function refreshVoices() {
        if (!('speechSynthesis' in window)) return;
        const voices = window.speechSynthesis.getVoices();
        preferredVoice = chooseFriendlyVoice(voices, speechLanguage);
    }

    function chooseFriendlyVoice(voices, language) {
        if (!voices || voices.length === 0) return null;
        const prefix = language.toLowerCase().slice(0, 2);
        const matching = voices.filter(voice => (voice.lang || '').toLowerCase().startsWith(prefix));
        const pool = matching.length ? matching : voices;
        const preferredNames = prefix === 'pl'
            ? ['zofia', 'paulina', 'agnieszka', 'ewa', 'marek', 'google polski', 'polish']
            : ['sonia', 'libby', 'ava', 'jenny', 'samantha', 'hazel', 'google uk english female', 'google english'];

        const scored = pool.map(voice => {
            const name = (voice.name || '').toLowerCase();
            let score = 0;
            preferredNames.forEach((preferred, position) => {
                if (name.includes(preferred)) score += 40 - position;
            });
            if (/natural|neural|enhanced|premium/.test(name)) score += 20;
            if (/female/.test(name)) score += 6;
            if ((voice.lang || '').toLowerCase() === language.toLowerCase()) score += 10;
            if (voice.localService) score += 2;
            return { voice, score };
        });

        scored.sort((a, b) => b.score - a.score);
        return scored[0]?.voice || pool[0] || null;
    }

    function stopSpeaking() {
        if ('speechSynthesis' in window) window.speechSynthesis.cancel();
        mascot?.classList.remove('is-talking');
        speech?.classList.remove('is-speaking');
    }

    function speak(text) {
        currentSpeechText = text || '';
        if (!soundOn || !currentSpeechText || !('speechSynthesis' in window)) return;

        stopSpeaking();
        refreshVoices();
        const utterance = new SpeechSynthesisUtterance(currentSpeechText);
        utterance.lang = speechLanguage;
        utterance.rate = speechLanguage.toLowerCase().startsWith('pl') ? 0.88 : 0.9;
        utterance.pitch = 1.16;
        utterance.volume = 0.96;
        if (preferredVoice) utterance.voice = preferredVoice;

        utterance.onstart = () => {
            mascot?.classList.add('is-talking');
            speech?.classList.add('is-speaking');
        };
        const finishSpeaking = () => {
            mascot?.classList.remove('is-talking');
            speech?.classList.remove('is-speaking');
        };
        utterance.onend = finishSpeaking;
        utterance.onerror = finishSpeaking;

        window.speechSynthesis.speak(utterance);
    }

    function repeatCurrentSpeech() {
        if (!soundOn) return;
        if (currentSpeechText) speak(currentSpeechText);
    }

    function buildQuestions() {
        const result = [];
        const seen = new Set();
        let guard = 0;

        while (result.length < 10 && guard < 700) {
            guard += 1;
            const difficulty = result.length < 3 ? 4 : result.length < 7 ? 6 : 8;
            const left = Math.floor(Math.random() * difficulty) + 1;
            const right = Math.floor(Math.random() * difficulty) + 1;
            const sum = left + right;
            if (sum > 15) continue;
            const key = `${left}:${right}`;
            if (seen.has(key)) continue;
            seen.add(key);
            result.push({ left, right, sum });
        }

        while (result.length < 10) {
            const left = (result.length % 5) + 1;
            const right = ((result.length + 2) % 5) + 1;
            result.push({ left, right, sum: left + right });
        }

        return result;
    }

    function createTokens(container, count, className) {
        container.replaceChildren();
        for (let i = 0; i < count; i += 1) {
            const token = document.createElement('span');
            token.className = `kid-token ${className}`;
            token.setAttribute('aria-hidden', 'true');
            container.appendChild(token);
        }
    }

    function createJoinedTokens(current) {
        joinedGroup.replaceChildren();
        const total = current.sum;
        for (let i = 0; i < total; i += 1) {
            const token = document.createElement('span');
            token.className = 'kid-token kid-token-joined';
            token.setAttribute('aria-hidden', 'true');
            if (i < current.left) token.style.background = 'linear-gradient(145deg,#ffbd59,#ff8e4f)';
            else token.style.background = 'linear-gradient(145deg,#a795ff,#745fe7)';
            joinedGroup.appendChild(token);
        }
    }

    function setMascot(state) {
        if (!mascot) return;
        const src = state === 'correct'
            ? mascot.dataset.mascotCorrect
            : state === 'try'
                ? mascot.dataset.mascotTry
                : mascot.dataset.mascotIdle;
        if (src) mascot.src = src;

        mascot.classList.remove('is-celebrating');
        if (state === 'correct') {
            void mascot.offsetWidth;
            mascot.classList.add('is-celebrating');
        }
    }

    function setInputEnabled(enabled) {
        answer.disabled = !enabled;
        check.disabled = !enabled;
        keyButtons.forEach(button => {
            button.disabled = !enabled;
        });
    }

    function renderQuestion() {
        const current = questions[index];
        if (!current) {
            showFinish();
            return;
        }

        stopSpeaking();
        locked = false;
        joined = false;
        answer.value = '';
        feedback.hidden = true;
        feedback.className = 'kid-feedback';
        joinedCard.style.display = 'none';
        leftCard.style.display = '';
        rightCard.style.display = '';
        leftCard.style.opacity = '1';
        rightCard.style.opacity = '1';
        leftCard.style.transform = '';
        rightCard.style.transform = '';
        joinButton.hidden = false;
        joinButton.disabled = false;
        setInputEnabled(false);
        setMascot('idle');

        leftLabel.textContent = String(current.left);
        rightLabel.textContent = String(current.right);
        createTokens(leftGroup, current.left, 'kid-token-left');
        createTokens(rightGroup, current.right, 'kid-token-right');
        joinedGroup.replaceChildren();

        questionText.textContent = `${current.left} + ${current.right} =`;
        questionNumber.textContent = `Question ${index + 1}`;
        counter.textContent = `${index + 1} of ${questions.length}`;
        speech.textContent = 'Count each group, then tap “Join the groups!” to bring them together.';
        dockTitle.textContent = 'First, join the groups';
        dockCopy.textContent = 'Then use the number buttons to enter your answer.';
        currentSpeechText = activeCopy().countJoin(current);

        const percent = Math.round(index * 100 / questions.length);
        progress.style.width = `${percent}%`;
        progress.parentElement.setAttribute('aria-valuenow', String(percent));

        const practicePanel = panels.find(panel => panel.dataset.previewPanel === 'practice');
        if (practicePanel && !practicePanel.hidden && soundOn) {
            window.setTimeout(() => speak(currentSpeechText), 160);
        }
    }

    function joinGroups() {
        if (locked || joined || !questions[index]) return;
        joined = true;
        const current = questions[index];
        stopSpeaking();
        playJoin();

        createJoinedTokens(current);
        leftCard.style.opacity = '.18';
        rightCard.style.opacity = '.18';
        leftCard.style.transform = 'translateX(18px) scale(.96)';
        rightCard.style.transform = 'translateX(-18px) scale(.96)';
        joinButton.disabled = true;

        window.setTimeout(() => {
            leftCard.style.display = 'none';
            rightCard.style.display = 'none';
            joinedCard.style.display = 'block';
            joinButton.hidden = true;
            setInputEnabled(true);
            speech.textContent = 'Nice! Now count how many are together and choose your answer.';
            dockTitle.textContent = 'How many altogether?';
            dockCopy.textContent = 'Use the number buttons, then check your answer.';
            currentSpeechText = activeCopy().afterJoin(current);
            answer.focus();
            speak(currentSpeechText);
        }, 300);
    }

    function showFinish() {
        stopSpeaking();
        gameStage.hidden = true;
        answerDock.hidden = true;
        finish.hidden = false;
        progress.style.width = '100%';
        progress.parentElement.setAttribute('aria-valuenow', '100');
        counter.textContent = 'Complete!';
        const percent = Math.round(correct * 100 / questions.length);
        finalScore.textContent = `${percent}%`;
        finalDetail.textContent = `${correct} correct out of ${questions.length}. Great work joining groups!`;
        currentSpeechText = activeCopy().finish(percent);
        playFinish();
        window.setTimeout(() => speak(currentSpeechText), 420);
    }

    function submitAnswer() {
        if (locked || !joined || !questions[index]) return;
        const value = answer.value.trim();
        if (value === '') {
            answer.focus();
            return;
        }

        stopSpeaking();
        locked = true;
        const current = questions[index];
        const expected = current.sum;
        const isCorrect = Number(value) === expected;
        if (isCorrect) correct += 1;

        setInputEnabled(false);
        feedback.hidden = false;
        feedback.className = `kid-feedback ${isCorrect ? 'is-correct' : 'is-incorrect'}`;

        if (isCorrect) {
            feedback.textContent = `Yes! ${current.left} + ${current.right} = ${expected}.`;
            speech.textContent = 'Brilliant! You joined the groups and counted them correctly!';
            currentSpeechText = activeCopy().correct(current);
            setMascot('correct');
            playCorrect();
        } else {
            feedback.textContent = `Almost! Together there are ${expected}.`;
            speech.textContent = `Good try! Count the joined group once more — there are ${expected} altogether.`;
            currentSpeechText = activeCopy().incorrect(current);
            setMascot('try');
            playIncorrect();
        }

        window.setTimeout(() => speak(currentSpeechText), 140);

        const completed = index + 1;
        const percent = Math.round(completed * 100 / questions.length);
        progress.style.width = `${percent}%`;
        progress.parentElement.setAttribute('aria-valuenow', String(percent));

        window.setTimeout(() => {
            index += 1;
            renderQuestion();
        }, isCorrect ? 1900 : 2350);
    }

    function resetGame() {
        stopSpeaking();
        questions = buildQuestions();
        index = 0;
        correct = 0;
        gameStage.hidden = false;
        answerDock.hidden = false;
        finish.hidden = true;
        renderQuestion();
    }

    tabs.forEach(tab => {
        tab.addEventListener('click', () => {
            playClick();
            const target = tab.dataset.previewTab;
            setTab(target);
            if (target === 'practice' && soundOn && questions[index]) {
                currentSpeechText = joined
                    ? activeCopy().afterJoin(questions[index])
                    : activeCopy().countJoin(questions[index]);
                window.setTimeout(() => speak(currentSpeechText), 140);
            } else if (target !== 'practice') {
                stopSpeaking();
            }
        });
    });

    joinButton.addEventListener('click', joinGroups);

    keyButtons.forEach(button => {
        button.addEventListener('click', () => {
            if (locked || answer.disabled) return;
            playClick();
            const key = button.dataset.practiceKey;
            if (key === 'clear') answer.value = '';
            else if (key === 'backspace') answer.value = answer.value.slice(0, -1);
            else if (/^\d$/.test(key) && answer.value.length < 2) answer.value += key;
            answer.focus();
        });
    });

    check.addEventListener('click', submitAnswer);
    answer.addEventListener('keydown', event => {
        if (event.key === 'Enter') {
            event.preventDefault();
            submitAnswer();
        }
    });

    restart.addEventListener('click', () => {
        playClick();
        resetGame();
    });

    soundToggle.addEventListener('click', () => {
        soundOn = !soundOn;
        updateSoundToggle();
        if (soundOn) {
            ensureAudioContext();
            window.setTimeout(playClick, 15);
            const practicePanel = panels.find(panel => panel.dataset.previewPanel === 'practice');
            if (practicePanel && !practicePanel.hidden) {
                window.setTimeout(repeatCurrentSpeech, 120);
            }
        } else {
            stopSpeaking();
        }
    });

    if ('speechSynthesis' in window) {
        window.speechSynthesis.addEventListener?.('voiceschanged', refreshVoices);
    }

    mascot?.addEventListener('click', repeatCurrentSpeech);
    mascot?.addEventListener('keydown', event => {
        if (event.key === 'Enter' || event.key === ' ') {
            event.preventDefault();
            repeatCurrentSpeech();
        }
    });

    updateSoundToggle();
    resetGame();
    setTab('lesson');
})();
