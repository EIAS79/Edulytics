(() => {
    "use strict";

    const eddyImage = document.querySelector("[data-cg-eddy-panel] img");
    if (!eddyImage) {
        return;
    }

    eddyImage.src = "/images/game/v9/eddy-hint.webp";
    eddyImage.alt = "Eddy giving a hint";
})();
