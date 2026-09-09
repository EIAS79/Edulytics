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
        soundToggle.setAttribute('aria-label', soundOn ? 'Turn game sounds off' : 'Turn game sounds on');
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

        const percent = Math.round(index * 100 / questions.length);
        progress.style.width = `${percent}%`;
        progress.parentElement.setAttribute('aria-valuenow', String(percent));
    }

    function joinGroups() {
        if (locked || joined || !questions[index]) return;
        joined = true;
        const current = questions[index];
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
            answer.focus();
        }, 300);
    }

    function showFinish() {
        gameStage.hidden = true;
        answerDock.hidden = true;
        finish.hidden = false;
        progress.style.width = '100%';
        progress.parentElement.setAttribute('aria-valuenow', '100');
        counter.textContent = 'Complete!';
        const percent = Math.round(correct * 100 / questions.length);
        finalScore.textContent = `${percent}%`;
        finalDetail.textContent = `${correct} correct out of ${questions.length}. Great work joining groups!`;
        playFinish();
    }

    function submitAnswer() {
        if (locked || !joined || !questions[index]) return;
        const value = answer.value.trim();
        if (value === '') {
            answer.focus();
            return;
        }

        locked = true;
        const expected = questions[index].sum;
        const isCorrect = Number(value) === expected;
        if (isCorrect) correct += 1;

        setInputEnabled(false);
        feedback.hidden = false;
        feedback.className = `kid-feedback ${isCorrect ? 'is-correct' : 'is-incorrect'}`;

        if (isCorrect) {
            feedback.textContent = `Yes! ${questions[index].left} + ${questions[index].right} = ${expected}.`;
            speech.textContent = 'Brilliant! You joined the groups and counted them correctly!';
            setMascot('correct');
            playCorrect();
        } else {
            feedback.textContent = `Almost! Together there are ${expected}.`;
            speech.textContent = `Good try! Count the joined group once more — there are ${expected} altogether.`;
            setMascot('try');
            playIncorrect();
        }

        const completed = index + 1;
        const percent = Math.round(completed * 100 / questions.length);
        progress.style.width = `${percent}%`;
        progress.parentElement.setAttribute('aria-valuenow', String(percent));

        window.setTimeout(() => {
            index += 1;
            renderQuestion();
        }, isCorrect ? 1150 : 1550);
    }

    function resetGame() {
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
            setTab(tab.dataset.previewTab);
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
        }
    });

    updateSoundToggle();
    resetGame();
    setTab('lesson');
})();