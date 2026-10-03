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

    const AudioContextClass = window.AudioContext || window.webkitAudioContext;
    let audioContext = null;
    let soundUnlocked = false;

    const unlockHoverSound = async () => {
        if (!AudioContextClass) {
            return;
        }

        try {
            audioContext ??= new AudioContextClass();
            if (audioContext.state === "suspended") {
                await audioContext.resume();
            }
            soundUnlocked = audioContext.state === "running";
        } catch {
            soundUnlocked = false;
        }
    };

    const playHoverSound = () => {
        if (!soundUnlocked || !audioContext) {
            return;
        }

        try {
            const now = audioContext.currentTime;
            const master = audioContext.createGain();
            const tone = audioContext.createOscillator();
            const shimmer = audioContext.createOscillator();
            const shimmerGain = audioContext.createGain();

            tone.type = "sine";
            tone.frequency.setValueAtTime(510, now);
            tone.frequency.exponentialRampToValueAtTime(760, now + 0.12);

            shimmer.type = "triangle";
            shimmer.frequency.setValueAtTime(980, now);
            shimmer.frequency.exponentialRampToValueAtTime(1240, now + 0.1);

            master.gain.setValueAtTime(0.0001, now);
            master.gain.exponentialRampToValueAtTime(0.055, now + 0.018);
            master.gain.exponentialRampToValueAtTime(0.0001, now + 0.16);

            shimmerGain.gain.setValueAtTime(0.0001, now);
            shimmerGain.gain.exponentialRampToValueAtTime(0.016, now + 0.014);
            shimmerGain.gain.exponentialRampToValueAtTime(0.0001, now + 0.11);

            tone.connect(master);
            shimmer.connect(shimmerGain);
            shimmerGain.connect(master);
            master.connect(audioContext.destination);

            tone.start(now);
            shimmer.start(now);
            tone.stop(now + 0.17);
            shimmer.stop(now + 0.12);
        } catch {
            // Hover audio is enhancement-only; interaction must remain unaffected.
        }
    };

    window.addEventListener("pointerdown", unlockHoverSound, { once: true, passive: true });
    window.addEventListener("keydown", unlockHoverSound, { once: true });

    cards.forEach(card => {
        const activate = (playSound = false) => {
            const wasFocused = card.classList.contains("is-scene-focus");
            field.classList.add("is-card-focused");
            cards.forEach(item => item.classList.toggle("is-scene-focus", item === card));

            if (playSound && !wasFocused) {
                playHoverSound();
            }
        };

        card.addEventListener("pointerenter", async event => {
            if (!event.pointerType || event.pointerType === "mouse") {
                if (!soundUnlocked) {
                    await unlockHoverSound();
                }
                activate(true);
            }
        }, { passive: true });
        card.addEventListener("pointerleave", clearSceneFocus, { passive: true });
        card.addEventListener("focusin", () => activate(false));
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