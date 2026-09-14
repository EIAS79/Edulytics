(() => {
    "use strict";

    const eddyImage = document.querySelector("[data-cg-eddy-panel] img");
    if (!eddyImage) {
        return;
    }

    // Keep Eddy's hint artwork deterministic while busting stale browser/CDN copies.
    eddyImage.src = "/images/game/v9/eddy-hint.webp?v=20260914-1";
    eddyImage.alt = "Eddy giving a hint";
})();
