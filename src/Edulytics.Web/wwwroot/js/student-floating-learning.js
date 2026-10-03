(() => {
    const field = document.querySelector("[data-floating-lesson-field]");
    if (!field || window.matchMedia("(prefers-reduced-motion: reduce)").matches) {
        return;
    }

    const cards = Array.from(field.querySelectorAll("[data-floating-lesson]"));
    if (!cards.length) {
        return;
    }

    const clearSceneFocus = () => {
        field.classList.remove("is-card-focused");
        cards.forEach(card => card.classList.remove("is-scene-focus"));
    };

    cards.forEach(card => {
        const activate = () => {
            field.classList.add("is-card-focused");
            cards.forEach(item => item.classList.toggle("is-scene-focus", item === card));
        };

        card.addEventListener("pointerenter", activate, { passive: true });
        card.addEventListener("pointerleave", clearSceneFocus, { passive: true });
        card.addEventListener("focusin", activate);
        card.addEventListener("focusout", event => {
            if (!card.contains(event.relatedTarget)) {
                clearSceneFocus();
            }
        });
    });

    let frame = 0;
    let lastX = 0;
    let lastY = 0;

    const applyParallax = () => {
        frame = 0;
        const rect = field.getBoundingClientRect();
        const nx = Math.max(-1, Math.min(1, ((lastX - rect.left) / rect.width - 0.5) * 2));
        const ny = Math.max(-1, Math.min(1, ((lastY - rect.top) / rect.height - 0.5) * 2));

        cards.forEach((card, index) => {
            if (card.matches(":hover") || card.matches(":focus-within")) {
                card.style.setProperty("--parallax-x", "0px");
                card.style.setProperty("--parallax-y", "0px");
                return;
            }

            const depth = 2.25 + (index % 5) * 0.65;
            card.style.setProperty("--parallax-x", (nx * depth).toFixed(2) + "px");
            card.style.setProperty("--parallax-y", (ny * depth * 0.62).toFixed(2) + "px");
        });
    };

    field.addEventListener("pointermove", event => {
        if (event.pointerType && event.pointerType !== "mouse") {
            return;
        }

        lastX = event.clientX;
        lastY = event.clientY;

        if (!frame) {
            frame = requestAnimationFrame(applyParallax);
        }
    }, { passive: true });

    field.addEventListener("pointerleave", () => {
        cards.forEach(card => {
            card.style.setProperty("--parallax-x", "0px");
            card.style.setProperty("--parallax-y", "0px");
        });
    });
})();