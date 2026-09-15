(() => {
    "use strict";

    const root = document.querySelector(".ed-home");
    if (!root) return;

    const footer = root.querySelector(".ed-home-footer");
    if (!footer) return;

    const documentLanguage = (document.documentElement.lang || "en").toLowerCase();
    const language = documentLanguage.startsWith("ar")
        ? "ar"
        : documentLanguage.startsWith("pl")
            ? "pl"
            : "en";

    const copy = {
        en: {
            legalLabel: "Legal",
            privacy: "Privacy",
            terms: "Terms",
            content: "Content licences",
            dpa: "Data Processing Agreement",
            help: "Help center"
        },
        pl: {
            legalLabel: "Informacje prawne",
            privacy: "Prywatność",
            terms: "Regulamin",
            content: "Licencje treści",
            dpa: "Powierzenie danych",
            help: "Centrum pomocy"
        },
        ar: {
            legalLabel: "معلومات قانونية",
            privacy: "الخصوصية",
            terms: "الشروط",
            content: "مصادر المحتوى والتراخيص",
            dpa: "اتفاقية معالجة البيانات",
            help: "مركز المساعدة"
        }
    }[language];

    const bottom = footer.querySelector(".ed-home-footer-bottom");
    if (bottom) {
        let legalNav = bottom.querySelector(".ed-home-footer-legal");
        if (!legalNav) {
            legalNav = document.createElement("nav");
            legalNav.className = "ed-home-footer-legal";

            const legacyLegalText = Array.from(bottom.children).find((element, index) =>
                index > 0 && element.tagName === "SPAN");

            if (legacyLegalText) {
                legacyLegalText.replaceWith(legalNav);
            } else {
                bottom.appendChild(legalNav);
            }
        }

        legalNav.setAttribute("aria-label", copy.legalLabel);
        legalNav.replaceChildren();

        [
            ["/legal/privacy", copy.privacy],
            ["/legal/terms", copy.terms],
            ["/legal/content-sources", copy.content],
            ["/legal/data-processing-agreement", copy.dpa]
        ].forEach(([href, label]) => {
            const anchor = document.createElement("a");
            anchor.href = href;
            anchor.textContent = label;
            legalNav.appendChild(anchor);
        });
    }

    const companyColumns = footer.querySelectorAll(
        ".ed-home-footer-grid > div:not(.ed-home-footer-brand)");

    companyColumns.forEach(column => {
        const heading = (column.querySelector("h3")?.textContent || "")
            .trim()
            .toLowerCase();
        const isCompany = ["company", "firma", "الشركة"].includes(heading);
        if (!isCompany || column.querySelector('a[href="/help"]')) return;

        const help = document.createElement("a");
        help.href = "/help";
        help.textContent = copy.help;

        const contentSources = column.querySelector('a[href="/legal/content-sources"]');
        if (contentSources) {
            contentSources.insertAdjacentElement("beforebegin", help);
        } else {
            column.appendChild(help);
        }
    });
})();