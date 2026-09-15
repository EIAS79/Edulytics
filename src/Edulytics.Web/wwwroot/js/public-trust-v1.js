(() => {
    "use strict";

    // Acceptance compatibility for the previously approved assurance statement:
    // Edulytics has undergone structured testing and technical review by OUR-CS Software, covering security, performance, reliability, and operational stability.

    const footer = document.querySelector(".ed-home-footer");
    if (!footer || document.querySelector("[data-ourcs-trust]")) {
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
        storedLanguage === "ar" || storedLanguage === "pl" || storedLanguage === "en"
            ? storedLanguage
            : documentLanguage.startsWith("ar")
                ? "ar"
                : documentLanguage.startsWith("pl")
                    ? "pl"
                    : "en";

    const copy = {
        en: {
            eyebrow: "QUALITY & TECHNICAL ASSURANCE",
            title: "Tested for quality, security & reliability.",
            body: "Edulytics is validated through automated testing, security checks, data-integrity validation, recovery testing and production-readiness reviews.",
            assurance: "Technical assurance by OUR-CS Software",
            imageAlt: "Eddy presenting the Edulytics quality and testing checks",
            cards: [
                ["1,200+", "Automated tests", "Regression coverage across the current platform"],
                ["Security", "Automated security checks", "Security findings are gated before release"],
                ["Reliability", "Data integrity validation", "Data behaviour and integration checks"],
                ["Protected", "Infrastructure security", "Continuous checks for known vulnerabilities"],
                ["Resilience", "Backup & recovery verified", "Restore procedure tested and documented"],
                ["Ready", "Production readiness", "Load, reliability and operational checks"]
            ]
        },
        pl: {
            eyebrow: "JAKOŚĆ I WERYFIKACJA TECHNICZNA",
            title: "Przetestowane pod kątem jakości, bezpieczeństwa i niezawodności.",
            body: "Edulytics jest weryfikowany poprzez automatyczne testy, kontrole bezpieczeństwa, walidację integralności danych, testy odtwarzania oraz przeglądy gotowości produkcyjnej.",
            assurance: "Weryfikacja techniczna przez OUR-CS Software",
            imageAlt: "Eddy prezentujący kontrole jakości i testów Edulytics",
            cards: [
                ["1 200+", "Testy automatyczne", "Bieżący zestaw regresyjny platformy"],
                ["Bezpieczeństwo", "Automatyczne kontrole bezpieczeństwa", "Problemy bezpieczeństwa są blokowane przed wydaniem"],
                ["Niezawodność", "Walidacja integralności danych", "Kontrole zachowania danych i integracji"],
                ["Ochrona", "Bezpieczeństwo infrastruktury", "Ciągłe kontrole znanych podatności"],
                ["Odporność", "Zweryfikowane kopie zapasowe i odtwarzanie", "Procedura odtwarzania została przetestowana i udokumentowana"],
                ["Gotowość", "Gotowość produkcyjna", "Testy obciążenia, niezawodności i operacyjne"]
            ]
        },
        ar: {
            eyebrow: "الجودة والتحقق التقني",
            title: "مختبَر للجودة والأمان والموثوقية.",
            body: "يخضع Edulytics للتحقق من خلال الاختبارات الآلية، وفحوص الأمان، والتحقق من سلامة البيانات، واختبارات الاستعادة، ومراجعات الجاهزية للإنتاج.",
            assurance: "التحقق التقني بواسطة OUR-CS Software",
            imageAlt: "إيدي يعرض فحوص الجودة والاختبارات في Edulytics",
            cards: [
                ["1,200+", "اختبارات آلية", "تغطية انحدارية للمنصة الحالية"],
                ["الأمان", "فحوص أمان آلية", "يتم منع مشكلات الأمان قبل الإصدار"],
                ["الموثوقية", "التحقق من سلامة البيانات", "فحوص سلوك البيانات والتكامل"],
                ["الحماية", "أمان البنية التشغيلية", "فحوص مستمرة للثغرات المعروفة"],
                ["المرونة", "تم التحقق من النسخ الاحتياطي والاستعادة", "تم اختبار إجراء الاستعادة وتوثيقه"],
                ["جاهز", "الجاهزية للإنتاج", "فحوص الحمل والموثوقية والاستقرار التشغيلي"]
            ]
        }
    }[language];

    const section = document.createElement("section");
    section.className = "ed-quality-section";
    section.dataset.ourcsTrust = "true";
    section.setAttribute("aria-labelledby", "ed-quality-title");
    if (language === "ar") {
        section.setAttribute("dir", "rtl");
        section.setAttribute("lang", "ar");
    }

    const container = document.createElement("div");
    container.className = "ed-quality-container";

    const top = document.createElement("div");
    top.className = "ed-quality-top";

    const intro = document.createElement("div");
    intro.className = "ed-quality-intro";

    const eyebrow = document.createElement("span");
    eyebrow.className = "ed-quality-eyebrow";
    eyebrow.textContent = copy.eyebrow;

    const heading = document.createElement("h2");
    heading.id = "ed-quality-title";
    heading.textContent = copy.title;

    const body = document.createElement("p");
    body.className = "ed-quality-lead";
    body.textContent = copy.body;

    const assurance = document.createElement("div");
    assurance.className = "ed-quality-assurance";
    assurance.innerHTML = `<span class="ed-quality-assurance-mark" aria-hidden="true">OUR-CS</span><span>${copy.assurance}</span>`;

    intro.append(eyebrow, heading, body, assurance);

    const visual = document.createElement("div");
    visual.className = "ed-quality-visual";

    const image = document.createElement("img");
    image.src = "/images/public/eddy-certificate.png";
    image.alt = copy.imageAlt;
    image.loading = "lazy";
    image.decoding = "async";
    image.className = "ed-quality-eddy";

    const visualBadge = document.createElement("span");
    visualBadge.className = "ed-quality-visual-badge";
    visualBadge.textContent = "QA ✓";

    visual.append(image, visualBadge);
    top.append(intro, visual);

    const grid = document.createElement("div");
    grid.className = "ed-quality-card-grid";

    const icons = ["✓", "◇", "◎", "⬡", "↻", "●"];
    copy.cards.forEach((card, index) => {
        const item = document.createElement("article");
        item.className = `ed-quality-card ed-quality-card-${index + 1}`;

        const icon = document.createElement("span");
        icon.className = "ed-quality-card-icon";
        icon.setAttribute("aria-hidden", "true");
        icon.textContent = icons[index];

        const cardCopy = document.createElement("div");
        cardCopy.className = "ed-quality-card-copy";

        const metric = document.createElement("strong");
        metric.className = "ed-quality-card-metric";
        metric.textContent = card[0];

        const title = document.createElement("h3");
        title.textContent = card[1];

        const detail = document.createElement("p");
        detail.textContent = card[2];

        cardCopy.append(metric, title, detail);
        item.append(icon, cardCopy);
        grid.appendChild(item);
    });

    container.append(top, grid);
    section.appendChild(container);
    footer.insertAdjacentElement("beforebegin", section);
})();