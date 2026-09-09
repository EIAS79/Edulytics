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
    const gameStage = root.querySelector('[data-preview-game-stage]');
    const finish = root.querySelector('[data-preview-finish]');
    const finalScore = root.querySelector('[data-preview-final-score]');
    const finalDetail = root.querySelector('[data-preview-final-detail]');
    const restart = root.querySelector('[data-preview-restart]');

    let questions = [];
    let index = 0;
    let correct = 0;
    let locked = false;

    function setTab(name) {
        tabs.forEach(tab => {
            const active = tab.dataset.previewTab === name;
            tab.classList.toggle('is-active', active);
            tab.setAttribute('aria-selected', active ? 'true' : 'false');
        });
        panels.forEach(panel => {
            panel.hidden = panel.dataset.previewPanel !== name;
        });
        if (name === 'practice' && answer) answer.focus();
    }

    function buildQuestions() {
        const result = [];
        const seen = new Set();
        let guard = 0;

        while (result.length < 10 && guard < 500) {
            guard += 1;
            const left = Math.floor(Math.random() * 10) + 1;
            const right = Math.floor(Math.random() * 10) + 1;
            const sum = left + right;
            if (sum > 20) continue;
            const key = `${left}:${right}`;
            if (seen.has(key)) continue;
            seen.add(key);
            result.push({ left, right, sum });
        }

        while (result.length < 10) {
            const left = result.length + 1;
            const right = Math.max(1, 10 - result.length);
            result.push({ left, right, sum: left + right });
        }

        return result;
    }

    function renderQuestion() {
        const current = questions[index];
        if (!current) {
            showFinish();
            return;
        }

        locked = false;
        answer.value = '';
        answer.disabled = false;
        check.disabled = false;
        feedback.hidden = true;
        feedback.className = 'preview-game-feedback';
        questionText.textContent = `${current.left} + ${current.right} = ?`;
        questionNumber.textContent = `Question ${index + 1}`;
        counter.textContent = `Question ${index + 1} / ${questions.length}`;
        const completed = index;
        const percent = Math.round(completed * 100 / questions.length);
        progress.style.width = `${percent}%`;
        progress.parentElement.setAttribute('aria-valuenow', String(percent));
        answer.focus();
    }

    function showFinish() {
        gameStage.hidden = true;
        feedback.hidden = true;
        finish.hidden = false;
        progress.style.width = '100%';
        progress.parentElement.setAttribute('aria-valuenow', '100');
        counter.textContent = 'Complete';
        const percent = Math.round(correct * 100 / questions.length);
        finalScore.textContent = `${percent}%`;
        finalDetail.textContent = `${correct} correct out of ${questions.length}`;
    }

    function submitAnswer() {
        if (locked || !questions[index]) return;
        const value = answer.value.trim();
        if (value === '') {
            answer.focus();
            return;
        }

        locked = true;
        const expected = questions[index].sum;
        const isCorrect = Number(value) === expected;
        if (isCorrect) correct += 1;

        answer.disabled = true;
        check.disabled = true;
        feedback.hidden = false;
        feedback.className = `preview-game-feedback ${isCorrect ? 'is-correct' : 'is-incorrect'}`;
        feedback.textContent = isCorrect
            ? 'Correct!'
            : `Not quite. The answer is ${expected}.`;

        window.setTimeout(() => {
            index += 1;
            renderQuestion();
        }, 800);
    }

    function resetGame() {
        questions = buildQuestions();
        index = 0;
        correct = 0;
        gameStage.hidden = false;
        finish.hidden = true;
        renderQuestion();
    }

    tabs.forEach(tab => {
        tab.addEventListener('click', () => setTab(tab.dataset.previewTab));
    });

    root.querySelectorAll('[data-practice-key]').forEach(button => {
        button.addEventListener('click', () => {
            if (locked || answer.disabled) return;
            const key = button.dataset.practiceKey;
            if (key === 'clear') answer.value = '';
            else if (key === 'backspace') answer.value = answer.value.slice(0, -1);
            else if (/^\d$/.test(key) && answer.value.length < 3) answer.value += key;
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
    restart.addEventListener('click', resetGame);

    resetGame();
    setTab('lesson');
})();
