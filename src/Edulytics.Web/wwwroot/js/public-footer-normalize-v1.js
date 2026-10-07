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
            help: "Help center",
            tagline: "Mathematics. Learning. Progress.",
            headings: ["Platform", "For schools", "For users", "Company"],
            labels: {
                "/#platform": "Principles",
                "/product/curricula": "Curricula",
                "/product/edulytics-ai": "Edulytics AI",
                "/schools/overview": "Overview",
                "/product/student-portal": "Student experience",
                "/contact/request-demo": "Request a demo",
                "/teachers/overview": "Teachers",
                "/students/overview": "Students",
                "/account/login": "Log in",
                "/company/about": "About",
                "/contact": "Contact",
                "/help": "Help center"
            }
        },
        pl: {
            legalLabel: "Informacje prawne",
            privacy: "Prywatność",
            terms: "Regulamin",
            content: "Licencje treści",
            dpa: "Powierzenie danych",
            help: "Centrum pomocy",
            tagline: "Matematyka. Nauka. Postęp.",
            headings: ["Platforma", "Dla szkół", "Dla użytkowników", "Firma"],
            labels: {
                "/#platform": "Zasady",
                "/product/curricula": "Programy nauczania",
                "/product/edulytics-ai": "Edulytics AI",
                "/schools/overview": "Przegląd",
                "/product/student-portal": "Doświadczenie ucznia",
                "/contact/request-demo": "Poproś o demo",
                "/teachers/overview": "Nauczyciele",
                "/students/overview": "Uczniowie",
                "/account/login": "Zaloguj się",
                "/company/about": "O nas",
                "/contact": "Kontakt",
                "/help": "Centrum pomocy"
            }
        },
        ar: {
            legalLabel: "معلومات قانونية",
            privacy: "الخصوصية",
            terms: "الشروط",
            content: "مصادر المحتوى والتراخيص",
            dpa: "اتفاقية معالجة البيانات",
            help: "مركز المساعدة",
            tagline: "الرياضيات. التعلّم. التقدم.",
            headings: ["المنصة", "للمدارس", "للمستخدمين", "الشركة"],
            labels: {
                "/#platform": "المبادئ",
                "/product/curricula": "المناهج",
                "/product/edulytics-ai": "Edulytics AI",
                "/schools/overview": "نظرة عامة",
                "/product/student-portal": "تجربة الطالب",
                "/contact/request-demo": "اطلب عرضًا تجريبيًا",
                "/teachers/overview": "المعلمون",
                "/students/overview": "الطلاب",
                "/account/login": "تسجيل الدخول",
                "/company/about": "عن Edulytics",
                "/contact": "تواصل معنا",
                "/help": "مركز المساعدة"
            }
        }
    }[language];

    const brandTagline = footer.querySelector(".ed-home-footer-brand p");
    if (brandTagline) brandTagline.textContent = copy.tagline;

    const columns = [...footer.querySelectorAll(
        ".ed-home-footer-grid > div:not(.ed-home-footer-brand)")];

    columns.forEach((column, index) => {
        const heading = column.querySelector("h3");
        if (heading && copy.headings[index]) heading.textContent = copy.headings[index];

        column.querySelectorAll("a").forEach(anchor => {
            let key = anchor.getAttribute("href") || "";
            try {
                const url = new URL(key, window.location.origin);
                key = url.origin === window.location.origin
                    ? `${url.pathname}${url.hash || ""}`
                    : key;
            } catch { }

            const translated = copy.labels[key];
            if (translated) anchor.textContent = translated;
        });
    });

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