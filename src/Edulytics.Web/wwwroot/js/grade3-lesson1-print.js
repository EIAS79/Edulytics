"use strict";

document.addEventListener("DOMContentLoaded", () => {
    const printButton = document.querySelector("[data-grade3-print-page]");
    if (printButton) {
        printButton.addEventListener("click", () => window.print());
    }
});
