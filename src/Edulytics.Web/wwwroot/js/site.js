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

        wireConfirmationForms();

        wireSchoolCountryTimeZones();

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
