(() => {
    "use strict";

    const root = document.querySelector(".ed-home");
    const footer = root?.querySelector(".ed-home-footer");

    if (!root || !footer || root.querySelector("[data-ourcs-trust]")) {
        return;
    }

    let storedLanguage = null;
    try {
        storedLanguage = window.localStorage.getItem(
            "edulytics.public.siteLanguage");
    } catch {
        // Storage can be disabled by the browser.
    }

    const documentLanguage =
        (document.documentElement.lang || "en").toLowerCase();

    const language =
        storedLanguage === "ar"
            ? "ar"
            : documentLanguage.startsWith("pl")
                ? "pl"
                : "en";

    const copy = {
        en: {
            title: "Technical assurance by OUR-CS Software",
            body: "Edulytics has undergone structured testing and technical review by OUR-CS Software, covering security, performance, reliability, and operational stability.",
            meta: "Security · Performance · Reliability"
        },
        pl: {
            title: "Weryfikacja techniczna przez OUR-CS Software",
            body: "Edulytics przeszedł ustrukturyzowany proces przeglądu technicznego i testów przeprowadzony przez OUR-CS Software, obejmujący bezpieczeństwo, wydajność, niezawodność i stabilność operacyjną.",
            meta: "Bezpieczeństwo · Wydajność · Niezawodność"
        },
        ar: {
            title: "ضمان تقني من OUR-CS Software",
            body: "خضعت Edulytics لعملية مراجعة تقنية واختبارات منهجية أجرتها OUR-CS Software، شملت الأمان والأداء والموثوقية والاستقرار التشغيلي.",
            meta: "الأمان · الأداء · الموثوقية"
        }
    }[language];

    const section = document.createElement("section");
    section.className = "ed-home-trust";
    section.dataset.ourcsTrust = "true";
    section.setAttribute("aria-labelledby", "ourcs-trust-title");

    const container = document.createElement("div");
    container.className = "ed-home-container ed-home-trust-inner";

    const mark = document.createElement("div");
    mark.className = "ed-home-trust-mark";
    mark.setAttribute("aria-hidden", "true");
    mark.textContent = "OUR-CS";

    const copyBlock = document.createElement("div");
    copyBlock.className = "ed-home-trust-copy";

    const heading = document.createElement("h2");
    heading.id = "ourcs-trust-title";
    heading.textContent = copy.title;

    const body = document.createElement("p");
    body.textContent = copy.body;

    copyBlock.append(heading, body);

    const meta = document.createElement("span");
    meta.className = "ed-home-trust-meta";
    meta.textContent = copy.meta;

    container.append(mark, copyBlock, meta);
    section.appendChild(container);
    footer.insertAdjacentElement("beforebegin", section);
})();