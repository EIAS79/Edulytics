(() => {
    "use strict";

    const voice = window.EdulyticsPracticeVoice || null;
    const locale = document.documentElement.lang || "en";
    const question = document.querySelector("[data-practice-question]");
    const hint = document.querySelector("[data-practice-hint]");
    const feedback = document.querySelector("[data-practice-feedback]");
    const soundButton = document.querySelector("[data-practice-sound]");

    function currentSpeech() {
        const parts = [];
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

    if (voice) {
        window.setTimeout(() => {
            voice.speakMany(currentSpeech(), locale);
        }, 120);
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
