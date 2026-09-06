(() => {
    "use strict";

    const keyName = "_idempotencyKey";

    function newKey() {
        if (globalThis.crypto?.randomUUID) {
            return globalThis.crypto.randomUUID();
        }

        const bytes = new Uint8Array(16);
        globalThis.crypto.getRandomValues(bytes);
        return Array.from(bytes, x => x.toString(16).padStart(2, "0")).join("");
    }

    function ensureRound2Stylesheet() {
        if (document.querySelector('link[data-round2-product-fixes]')) return;
        const link = document.createElement("link");
        link.rel = "stylesheet";
        link.href = "/css/round2-product-fixes.css";
        link.dataset.round2ProductFixes = "true";
        document.head.appendChild(link);
    }

    function wireConfirmationForms() {
        document.querySelectorAll("form[data-confirm]").forEach(form => {
            form.addEventListener("submit", event => {
                const message = form.dataset.confirm;
                if (message && !globalThis.confirm(message)) event.preventDefault();
            });
        });
    }

    function wirePrintButtons() {
        document.querySelectorAll("[data-print-report]").forEach(button => {
            button.addEventListener("click", () => globalThis.print());
        });
    }

    function wireReportKindFilters() {
        document.querySelectorAll("[data-report-kind-filter]").forEach(select => {
            const form = select.closest("form[data-report-filter-form]");
            if (form) select.addEventListener("change", () => form.requestSubmit());
        });
    }

    function wireSchoolCountryTimeZones() {
        document.querySelectorAll("[data-school-country]").forEach(country => {
            const form = country.closest("form");
            const timeZone = form?.querySelector("[data-school-time-zone]");
            if (!timeZone) return;
            const sync = () => {
                const option = country.selectedOptions?.[0];
                timeZone.value = option?.dataset.timeZone ?? "";
            };
            country.addEventListener("change", sync);
            sync();
        });
    }

    function wireStudentWorkflowCleanup() {
        const students = document.getElementById("students");
        if (!students) return;
        students.classList.add("round2-students-section");
        students.querySelectorAll("table").forEach(table => {
            table.classList.add("round2-readable-table");
            table.parentElement?.classList.add("round2-table-scroll");
        });

        const profileForm = Array.from(students.querySelectorAll("form"))
            .find(form => (form.action || "").toLowerCase().includes("createstudentprofile"));
        const enrollmentForm = Array.from(students.querySelectorAll("form"))
            .find(form => (form.action || "").toLowerCase().includes("createstudentenrollment"));

        if (profileForm) {
            profileForm.hidden = true;
            profileForm.setAttribute("aria-hidden", "true");
            const language = (document.documentElement.lang || "en").toLowerCase();
            const panel = document.createElement("div");
            panel.className = "academic-card";
            const heading = document.createElement("h3");
            heading.textContent = language.startsWith("pl") ? "Utwórz konto ucznia" : "Create a student account";
            const description = document.createElement("p");
            description.className = "academic-help";
            description.textContent = language.startsWith("pl")
                ? "Nowych uczniów twórz w zarządzaniu użytkownikami. Profil ucznia i pierwsze przypisanie do klasy są wtedy tworzone i łączone automatycznie."
                : "Create new students in User Management. Their student profile and first class enrollment are created and linked automatically.";
            const link = document.createElement("a");
            link.className = "school-button school-button-primary";
            link.href = "/School/Users/Create";
            link.textContent = language.startsWith("pl") ? "Utwórz ucznia" : "Create student";
            panel.append(heading, description, link);
            profileForm.insertAdjacentElement("beforebegin", panel);
        }

        if (enrollmentForm) {
            const language = (document.documentElement.lang || "en").toLowerCase();
            const heading = enrollmentForm.querySelector("h3");
            if (heading) heading.textContent = language.startsWith("pl")
                ? "Zmień przypisanie ucznia do klasy"
                : "Change student class enrollment";
            const studentSelect = enrollmentForm.querySelector("select[name='studentProfileId']");
            if (studentSelect && !studentSelect.nextElementSibling?.classList.contains("round2-enrollment-help")) {
                const help = document.createElement("p");
                help.className = "academic-help round2-enrollment-help";
                help.textContent = language.startsWith("pl")
                    ? "Wybierz istniejącego ucznia, a następnie jego nową klasę. Nie twórz tutaj ponownie profilu ucznia."
                    : "Select an existing student, then choose the new class. Do not recreate the student profile here.";
                studentSelect.insertAdjacentElement("afterend", help);
            }
        }
    }

    function classFirstLabel(label) {
        const parts = String(label || "").split("·").map(part => part.trim()).filter(Boolean);
        if (parts.length < 2) return label;
        return `${parts.at(-1)} — ${parts.slice(0, -1).join(" · ")}`;
    }

    async function wireAcademicClassRelationships() {
        const teacherClass = document.getElementById("teacher-class");
        const enrollmentClass = document.getElementById("enroll-class");
        if (!teacherClass && !enrollmentClass) return;

        let response;
        try {
            response = await fetch("/school/academic-structure/phase39/class-options", {
                headers: { "Accept": "application/json" }
            });
        } catch {
            return;
        }
        if (!response.ok) return;

        const classOptions = await response.json();
        const labels = new Map(classOptions.map(item => [String(item.id).toLowerCase(), item.label]));
        [teacherClass, enrollmentClass].filter(Boolean).forEach(select => {
            Array.from(select.options).forEach(option => {
                if (!option.value) return;
                option.textContent = classFirstLabel(labels.get(option.value.toLowerCase()) || option.textContent);
                option.title = option.textContent;
            });
        });

        if (teacherClass) {
            const form = teacherClass.closest("form");
            if (form) form.action = "/school/academic-structure/phase39/teacher-assignments";
            teacherClass.multiple = true;
            teacherClass.name = "classGroupIds";
            teacherClass.size = Math.min(10, Math.max(4, teacherClass.options.length - 1));
            teacherClass.classList.add("round2-class-multiselect");
            const placeholder = Array.from(teacherClass.options).find(option => !option.value);
            if (placeholder) {
                placeholder.selected = false;
                placeholder.disabled = true;
                placeholder.hidden = true;
            }
            if (!document.getElementById("teacher-class-multi-help")) {
                const help = document.createElement("p");
                help.className = "academic-help";
                help.id = "teacher-class-multi-help";
                const language = (document.documentElement.lang || "en").toLowerCase();
                help.textContent = language.startsWith("pl")
                    ? "Wybierz jedną lub więcej klas. Użyj Ctrl/Cmd, aby zaznaczyć kilka klas."
                    : "Select one or more classes. Use Ctrl/Cmd to select multiple classes.";
                teacherClass.setAttribute("aria-describedby", "teacher-class-multi-help");
                teacherClass.insertAdjacentElement("afterend", help);
            }
        }

        document.querySelector("#teachers .academic-table")?.querySelectorAll("tr").forEach(row => {
            const subjectCell = row.children.item(2);
            if (subjectCell) subjectCell.hidden = true;
        });
    }

    function wireCurriculumLevelMultiSelect() {
        const levelKey = document.getElementById("level-key");
        const levelYear = document.getElementById("level-year");
        const levelProgram = document.getElementById("level-program");
        if (!levelKey || !levelYear || !levelProgram) return;
        const form = levelKey.closest("form");
        if (!form) return;

        levelKey.multiple = true;
        levelKey.name = "curriculumLevelKeys";
        levelKey.size = 10;
        levelKey.classList.add("round2-curriculum-level-multiselect");
        form.action = "/school/academic-structure/curriculum-levels/bulk";
        Array.from(levelKey.options).find(option => !option.value)?.remove();

        if (!form.querySelector(".round2-multi-help")) {
            const language = (document.documentElement.lang || "en").toLowerCase();
            const help = document.createElement("p");
            help.className = "academic-help round2-multi-help";
            help.textContent = language.startsWith("pl")
                ? "Wybierz wszystkie poziomy, które chcesz dodać. Użyj Ctrl/Cmd, aby zaznaczyć kilka pozycji."
                : "Select all Curriculum Levels you want to add. Use Ctrl/Cmd to select multiple items.";
            levelKey.insertAdjacentElement("afterend", help);
        }

        const submit = form.querySelector("button[type='submit']");
        if (submit) {
            const language = (document.documentElement.lang || "en").toLowerCase();
            submit.textContent = language.startsWith("pl") ? "Dodaj wybrane poziomy" : "Add selected Curriculum Levels";
        }
    }

    function simplifyStudentLearningCta() {
        const language = (document.documentElement.lang || "en").toLowerCase();
        const labels = new Set(["View this curriculum in My learning", "Zobacz ten program w Mojej nauce"]);
        document.querySelectorAll("a").forEach(anchor => {
            if (labels.has(anchor.textContent?.trim())) {
                anchor.textContent = language.startsWith("pl") ? "Otwórz Moją naukę" : "Open My learning";
            }
        });
    }

    function normalizeWholeMarkInputs() {
        document.querySelectorAll("input[name='maxScore'], input[name='maxScorePerQuestion']").forEach(input => {
            input.step = "1";
            input.inputMode = "numeric";
            if (!input.value) return;
            const value = Number(input.value);
            if (Number.isFinite(value) && Number.isInteger(value)) input.value = String(value);
        });
    }

    function clarifyLearningOutcomeUx() {
        const language = (document.documentElement.lang || "en").toLowerCase();
        const isPolish = language.startsWith("pl");
        document.querySelectorAll(".ed-ai-capability-badge.is-manual").forEach(badge => {
            badge.textContent = isPolish ? "Generator AI w przygotowaniu" : "AI generator pending";
            badge.classList.add("is-pending");
        });
        document.querySelectorAll(".ed-outcome-fieldset").forEach(fieldset => {
            if (fieldset.querySelector(".round2-outcome-help")) return;
            const help = document.createElement("p");
            help.className = "assessment-info round2-outcome-help";
            help.textContent = isPolish
                ? "Wybierz efekt uczenia się, który mierzy to pytanie. Krótki kod jest identyfikatorem w programie, a opis poniżej pokazuje dostępny kontekst Edulityks."
                : "Choose the learning outcome this question measures. The short code is the curriculum locator; the text below shows the Edulytics context currently available for that outcome.";
            fieldset.querySelector("legend")?.insertAdjacentElement("afterend", help);
        });
    }

    function wireAssessmentDeliverySafety() {
        const pathMatch = window.location.pathname.match(/^\/school\/assessments\/([0-9a-f-]{36})\/builder\/?$/i);
        if (!pathMatch) return;

        const language = (document.documentElement.lang || "en").toLowerCase();
        const isPolish = language.startsWith("pl");
        const deliveryForm = document.getElementById("delivery-settings-form");

        if (deliveryForm) {
            const tracked = ["targetType", "targetStudentProfileId", "deliveryMode", "difficultyBand"];
            const baseline = () => tracked.map(name => `${name}:${deliveryForm.elements[name]?.value ?? ""}`).join("|");
            const initial = baseline();
            let dirty = false;
            const refreshDirty = () => { dirty = baseline() !== initial; };
            deliveryForm.addEventListener("input", refreshDirty);
            deliveryForm.addEventListener("change", refreshDirty);
            deliveryForm.addEventListener("submit", () => { dirty = false; });

            document.querySelectorAll("form").forEach(form => {
                if (!(form.action || "").toLowerCase().endsWith("/publish")) return;
                form.addEventListener("submit", event => {
                    refreshDirty();
                    if (!dirty) return;
                    event.preventDefault();
                    globalThis.alert(isPolish
                        ? "Najpierw zapisz ustawienia sposobu przeprowadzenia sprawdzianu, a następnie opublikuj sprawdzian."
                        : "Save the assessment delivery settings first, then publish the assessment.");
                    deliveryForm.scrollIntoView({ behavior: "smooth", block: "center" });
                });
            });
            return;
        }

        const offlineBadge = document.querySelector(".ed-delivery-badge.is-offline");
        const headerActions = document.querySelector(".assessment-header .assessment-actions");
        if (!offlineBadge || !headerActions || headerActions.querySelector("[data-round2-online-correction]")) return;

        const link = document.createElement("a");
        link.className = "school-button school-button-primary";
        link.href = `/school/assessments/${pathMatch[1]}/delivery`;
        link.dataset.round2OnlineCorrection = "true";
        link.textContent = isPolish ? "Zmień na sprawdzian Online" : "Switch to Online assessment";
        headerActions.appendChild(link);
    }

    document.addEventListener("DOMContentLoaded", () => {
        ensureRound2Stylesheet();
        wirePrintButtons();
        wireReportKindFilters();
        wireConfirmationForms();
        wireSchoolCountryTimeZones();
        wireStudentWorkflowCleanup();
        void wireAcademicClassRelationships();
        wireCurriculumLevelMultiSelect();
        simplifyStudentLearningCta();
        normalizeWholeMarkInputs();
        clarifyLearningOutcomeUx();
        wireAssessmentDeliverySafety();

        document.querySelectorAll("form").forEach(form => {
            if ((form.method || "get").toLowerCase() !== "post") return;
            if (form.querySelector(`input[name="${keyName}"]`)) return;
            const input = document.createElement("input");
            input.type = "hidden";
            input.name = keyName;
            input.value = newKey();
            form.appendChild(input);
        });
    });
})();
