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

    function wireConfirmationForms() {
        document.querySelectorAll("form[data-confirm]").forEach(form => {
            form.addEventListener("submit", event => {
                const message = form.dataset.confirm;

                if (message && !globalThis.confirm(message)) {
                    event.preventDefault();
                }
            });
        });
    }

    function wirePrintButtons() {
        document.querySelectorAll("[data-print-report]").forEach(button => {
            button.addEventListener("click", () => {
                globalThis.print();
            });
        });
    }

    function wireReportKindFilters() {
        document.querySelectorAll("[data-report-kind-filter]")
            .forEach(select => {
                const form = select.closest("form[data-report-filter-form]");

                if (!form) {
                    return;
                }

                select.addEventListener("change", () => {
                    form.requestSubmit();
                });
            });
    }

    function wireSchoolCountryTimeZones() {
        document.querySelectorAll("[data-school-country]").forEach(country => {
            const form = country.closest("form");
            const timeZone = form?.querySelector(
                "[data-school-time-zone]");

            if (!timeZone) {
                return;
            }

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
        if (!students) {
            return;
        }

        const profileForm = Array.from(students.querySelectorAll("form"))
            .find(form =>
                (form.action || "")
                    .toLowerCase()
                    .includes("createstudentprofile"));
        const enrollmentForm = Array.from(students.querySelectorAll("form"))
            .find(form =>
                (form.action || "")
                    .toLowerCase()
                    .includes("createstudentenrollment"));

        if (profileForm) {
            profileForm.hidden = true;
            profileForm.setAttribute("aria-hidden", "true");

            const language = (document.documentElement.lang || "en")
                .toLowerCase();
            const panel = document.createElement("div");
            panel.className = "academic-card";

            const heading = document.createElement("h3");
            heading.textContent = language.startsWith("pl")
                ? "Utwórz konto ucznia"
                : "Create a student account";

            const description = document.createElement("p");
            description.className = "academic-help";
            description.textContent = language.startsWith("pl")
                ? "Nowych uczniów twórz w zarządzaniu użytkownikami. Profil ucznia i pierwsze przypisanie do klasy są wtedy tworzone i łączone automatycznie."
                : "Create new students in User Management. Their student profile and first class enrollment are created and linked automatically.";

            const link = document.createElement("a");
            link.className = "school-button school-button-primary";
            link.href = "/School/Users/Create";
            link.textContent = language.startsWith("pl")
                ? "Utwórz ucznia"
                : "Create student";

            panel.append(heading, description, link);
            profileForm.insertAdjacentElement("beforebegin", panel);
        }

        if (enrollmentForm) {
            const language = (document.documentElement.lang || "en")
                .toLowerCase();
            const heading = enrollmentForm.querySelector("h3");
            if (heading) {
                heading.textContent = language.startsWith("pl")
                    ? "Zmień przypisanie ucznia do klasy"
                    : "Change student class enrollment";
            }

            const studentSelect = enrollmentForm.querySelector(
                "select[name='studentProfileId']");
            if (studentSelect) {
                const help = document.createElement("p");
                help.className = "academic-help";
                help.textContent = language.startsWith("pl")
                    ? "Wybierz istniejącego ucznia, a następnie jego nową klasę. Nie twórz tutaj ponownie profilu ucznia."
                    : "Select an existing student, then choose the new class. Do not recreate the student profile here.";
                studentSelect.insertAdjacentElement("afterend", help);
            }
        }
    }

    async function wireAcademicClassRelationships() {
        const teacherClass = document.getElementById("teacher-class");
        const enrollmentClass = document.getElementById("enroll-class");

        if (!teacherClass && !enrollmentClass) {
            return;
        }

        let response;
        try {
            response = await fetch(
                "/school/academic-structure/phase39/class-options",
                { headers: { "Accept": "application/json" } });
        } catch {
            return;
        }

        if (!response.ok) {
            return;
        }

        const classOptions = await response.json();
        const labels = new Map(
            classOptions.map(item => [
                String(item.id).toLowerCase(),
                item.label
            ]));

        [teacherClass, enrollmentClass]
            .filter(Boolean)
            .forEach(select => {
                Array.from(select.options).forEach(option => {
                    if (!option.value) {
                        return;
                    }

                    const label = labels.get(option.value.toLowerCase());
                    if (label) {
                        option.textContent = label;
                    }
                });
            });

        if (teacherClass) {
            const form = teacherClass.closest("form");
            if (form) {
                form.action =
                    "/school/academic-structure/phase39/teacher-assignments";
            }

            teacherClass.multiple = true;
            teacherClass.name = "classGroupIds";
            teacherClass.size = Math.min(
                10,
                Math.max(4, teacherClass.options.length - 1));

            const placeholder = Array.from(teacherClass.options)
                .find(option => !option.value);
            if (placeholder) {
                placeholder.selected = false;
                placeholder.disabled = true;
                placeholder.hidden = true;
            }

            const help = document.createElement("p");
            help.className = "academic-help";
            help.id = "teacher-class-multi-help";
            const language = (document.documentElement.lang || "en")
                .toLowerCase();
            help.textContent = language.startsWith("pl")
                ? "Wybierz jedną lub więcej klas. Użyj Ctrl/Cmd, aby zaznaczyć kilka klas."
                : "Select one or more classes. Use Ctrl/Cmd to select multiple classes.";
            teacherClass.setAttribute(
                "aria-describedby",
                "teacher-class-multi-help");
            teacherClass.insertAdjacentElement("afterend", help);
        }

        const teacherTable = document.querySelector(
            "#teachers .academic-table");
        teacherTable?.querySelectorAll("tr").forEach(row => {
            const subjectCell = row.children.item(2);
            if (subjectCell) {
                subjectCell.hidden = true;
            }
        });
    }

    document.addEventListener("DOMContentLoaded", () => {
        wirePrintButtons();

        wireReportKindFilters();

        wireConfirmationForms();

        wireSchoolCountryTimeZones();

        wireStudentWorkflowCleanup();

        void wireAcademicClassRelationships();

        document.querySelectorAll("form").forEach(form => {
            if ((form.method || "get").toLowerCase() !== "post") {
                return;
            }

            if (form.querySelector(`input[name="${keyName}"]`)) {
                return;
            }

            const input = document.createElement("input");
            input.type = "hidden";
            input.name = keyName;
            input.value = newKey();
            form.appendChild(input);
        });
    });
})();