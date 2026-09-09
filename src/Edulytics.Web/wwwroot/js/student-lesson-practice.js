(() => {
    "use strict";

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
            } else if (/^\d$/.test(key) && input.value.length < 32) {
                input.value += key;
            }

            input.focus();
            input.dispatchEvent(new Event("input", { bubbles: true }));
        });
    }
})();
