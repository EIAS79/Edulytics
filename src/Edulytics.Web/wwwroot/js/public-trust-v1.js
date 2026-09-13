(() => {
    "use strict";

    const root = document.querySelector(".ed-home");
    const footer = root?.querySelector(".ed-home-footer");

    if (!root || !footer ||
        root.querySelector("[data-ourcs-trust]")) {
        return;
    }

    let storedLanguage = null;
    try {
        storedLanguage =
            window.localStorage.getItem(
                "edulytics.public.siteLanguage");
    } catch {
        // Storage can be disabled by the browser.
    }

    const documentLanguage =
        (document.documentElement.lang || "en")
            .toLowerCase();

    const language =
        storedLanguage === "ar"
            ? "ar"
            : documentLanguage.startsWith("pl")
                ? "pl"
                : "en";

    const copy = {
        en: {
            title: "Technical assurance by OUR-CS Software",
            body: "Security, performance and operational stability tested by OUR-CS Software."
        },
        pl: {
            title: "Weryfikacja techniczna przez OUR-CS Software",
            body: "Bezpieczeństwo, wydajność i stabilność działania przetestowane przez OUR-CS Software."
        },
        ar: {
            title: "ضمان تقني من OUR-CS Software",
            body: "تم اختبار الأمان والأداء والاستقرار التشغيلي بواسطة OUR-CS Software."
        }
    }[language];

    const section = document.createElement("section");
    section.className = "ed-home-trust";
    section.dataset.ourcsTrust = "true";
    section.setAttribute("aria-labelledby", "ourcs-trust-title");

    const container = document.createElement("div");
    container.className = "ed-home-container ed-home-trust-inner";

    const kicker = document.createElement("span");
    kicker.className = "ed-home-kicker";
    kicker.textContent = "OUR-CS SOFTWARE";

    const heading = document.createElement("h2");
    heading.id = "ourcs-trust-title";
    heading.textContent = copy.title;

    const body = document.createElement("p");
    body.textContent = copy.body;

    container.append(kicker, heading, body);
    section.appendChild(container);
    footer.insertAdjacentElement("beforebegin", section);
})();
