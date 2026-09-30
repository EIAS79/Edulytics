(() => {
    "use strict";

    const voice = window.EdulyticsPracticeVoice || null;
    const locale = document.documentElement.lang || "en";
    const question = document.querySelector("[data-practice-question]");
    const hint = document.querySelector("[data-practice-hint]");
    const feedback = document.querySelector("[data-practice-feedback]");
    const soundButton = document.querySelector("[data-practice-sound]");
    const terminal = document.querySelector("[data-practice-terminal]");
    const review = document.querySelector("[data-practice-review]");
    const retryFeedback = document.querySelector('[data-practice-retry-feedback="true"]');
    const nextButtons = document.querySelectorAll("[data-practice-next]");

    function stopVoice() {
        if (voice) {
            voice.stop();
        }
    }

    // A navigation can happen while SpeechSynthesis still has queued
    // utterances from the previous question. Always clear that queue when
    // this page initializes and whenever the practice page is left/hidden.
    stopVoice();
    window.addEventListener("pagehide", stopVoice);
    window.addEventListener("beforeunload", stopVoice);
    document.addEventListener("visibilitychange", () => {
        if (document.visibilityState === "hidden") {
            stopVoice();
        }
    });

    function currentSpeech() {
        const parts = [];

        // Review pages keep the answered question visible so the learner can
        // inspect it, but the voice should announce only Eddy's result for
        // that completed turn. Re-reading the old question before feedback
        // makes the narration sound as though it belongs to the next turn.
        if (review) {
            const reviewMessage =
                feedback?.textContent.trim() ||
                hint?.textContent.trim() ||
                "";

            if (reviewMessage) {
                parts.push(reviewMessage);
            }

            return parts;
        }

        // Wrong #1 keeps the exact item open. Eddy's answer-aware guidance
        // is spoken first, then the question is repeated so the learner can
        // immediately retry with the guidance still in working memory.
        if (retryFeedback) {
            if (hint && hint.textContent.trim()) {
                parts.push(hint.textContent.trim());
            }
            if (question && question.textContent.trim()) {
                parts.push(question.textContent.trim());
            }
            return parts;
        }

        if (feedback && feedback.textContent.trim()) {
            parts.push(feedback.textContent.trim());
        }
        if (question && question.textContent.trim()) {
            parts.push(question.textContent.trim());
        }
        if (hint && hint.textContent.trim()) {
            parts.push(hint.textContent.trim());
        }
        return parts;
    }

    function updateSoundButton() {
        if (!soundButton || !voice) return;
        const enabled = voice.isEnabled();
        soundButton.textContent = enabled ? "🔊" : "🔇";
        soundButton.setAttribute("aria-pressed", enabled ? "true" : "false");
    }

    if (soundButton && voice) {
        updateSoundButton();
        soundButton.addEventListener("click", () => {
            const enabled = voice.toggle();
            updateSoundButton();
            if (enabled) {
                voice.speakMany(currentSpeech(), locale);
            }
        });
    }

    if (voice && !terminal) {
        window.setTimeout(() => {
            const speech = currentSpeech();
            if (speech.length > 0) {
                voice.speakMany(speech, locale);
            }
        }, 120);
    }

    for (const nextButton of nextButtons) {
        nextButton.addEventListener("click", stopVoice);
    }

    const answerForms = document.querySelectorAll("[data-adaptive-answer-form]");
    for (const form of answerForms) {
        let submitted = false;

        form.addEventListener("submit", (event) => {
            if (submitted) {
                event.preventDefault();
                event.stopImmediatePropagation();
                return;
            }

            if (!form.checkValidity()) {
                return;
            }

            submitted = true;
            stopVoice();
            form.dataset.submitting = "true";
            form.setAttribute("aria-busy", "true");

            for (const button of form.querySelectorAll('button[type="submit"]')) {
                button.setAttribute("aria-disabled", "true");
            }

            const answerInput = form.querySelector('input[name="answer"]');
            if (answerInput instanceof HTMLInputElement) {
                answerInput.readOnly = true;
            }
        });
    }

    const input = document.getElementById("lesson-practice-answer");
    if (!input) return;

    const buttons = document.querySelectorAll("[data-practice-key]");
    for (const button of buttons) {
        button.addEventListener("click", () => {
            const key = button.getAttribute("data-practice-key");
            if (!key) return;

            if (key === "clear") {
                input.value = "";
            } else if (key === "backspace") {
                input.value = input.value.slice(0, -1);
            } else if (
                /^[0-9./()\-+]$/.test(key) &&
                input.value.length < 128
            ) {
                input.value += key;
            }

            input.focus();
            input.dispatchEvent(new Event("input", { bubbles: true }));
        });
    }
})();
