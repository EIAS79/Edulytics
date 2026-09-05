(() => {
    "use strict";

    const wireErrorBackNavigation = () => {
        document
            .querySelectorAll("[data-ed-error-back]")
            .forEach((link) => {
                link.addEventListener("click", (event) => {
                    if (!document.referrer || window.history.length <= 1) {
                        return;
                    }

                    try {
                        const referrer = new URL(
                            document.referrer,
                            window.location.href);

                        if (referrer.origin !== window.location.origin) {
                            return;
                        }

                        event.preventDefault();
                        window.history.back();
                    } catch {
                        // Keep the server-rendered fallback URL.
                    }
                });
            });
    };

    const wireDoubleSubmitProtection = () => {
        document.addEventListener("submit", (event) => {
            const form =
                event.target instanceof HTMLFormElement
                    ? event.target
                    : null;

            if (!form?.matches("[data-prevent-double-submit]")) {
                return;
            }

            if (form.dataset.submitting === "true") {
                event.preventDefault();
                return;
            }

            form.dataset.submitting = "true";

            window.setTimeout(() => {
                form
                    .querySelectorAll(
                        'button[type="submit"], input[type="submit"]')
                    .forEach((control) => {
                        control.disabled = true;
                    });
            }, 0);
        });
    };

    const enhanceTeacherClassSelector = () => {
        const select =
            document.getElementById(
                "curriculumTeacherAssignmentClass");

        if (!(select instanceof HTMLSelectElement)) {
            return;
        }

        let observer;

        const enhance = () => {
            if (
                select.dataset.checkboxEnhanced === "true" ||
                !select.multiple ||
                select.name !== "classGroupIds" ||
                select.options.length === 0) {
                return;
            }

            select.dataset.checkboxEnhanced = "true";
            observer?.disconnect();

            const selectedValues =
                new Set(
                    Array.from(select.selectedOptions)
                        .map((option) => option.value));

            const list = document.createElement("div");
            list.id = "curriculumTeacherAssignmentClassOptions";
            list.className = "ed-class-checkbox-list";
            list.setAttribute("role", "group");
            list.setAttribute(
                "aria-label",
                "Select one or more classes");

            Array.from(select.options).forEach(
                (option, index) => {
                    const label = document.createElement("label");
                    label.className = "ed-class-checkbox-option";

                    const checkbox = document.createElement("input");
                    checkbox.type = "checkbox";
                    checkbox.name = "classGroupIds";
                    checkbox.value = option.value;
                    checkbox.checked =
                        selectedValues.has(option.value);
                    checkbox.id =
                        `curriculumTeacherAssignmentClass_${index}`;

                    const text = document.createElement("span");
                    text.textContent = option.textContent ?? "";
                    text.title = option.textContent ?? "";

                    checkbox.addEventListener("change", () => {
                        checkbox.setCustomValidity("");
                    });

                    label.append(checkbox, text);
                    list.append(label);
                });

            select.disabled = true;
            select.hidden = true;
            select.removeAttribute("required");
            select.insertAdjacentElement("afterend", list);

            const form = select.closest("form");

            form?.addEventListener("submit", (event) => {
                const checkboxes =
                    Array.from(
                        list.querySelectorAll(
                            'input[name="classGroupIds"]'));

                const anyChecked =
                    checkboxes.some(
                        (checkbox) => checkbox.checked);

                if (anyChecked || checkboxes.length === 0) {
                    return;
                }

                event.preventDefault();

                const first = checkboxes[0];
                first.setCustomValidity(
                    "Select at least one class.");
                first.reportValidity();
            });
        };

        observer =
            new MutationObserver(enhance);

        observer.observe(
            select,
            {
                attributes: true,
                childList: true
            });

        enhance();
    };

    document.addEventListener(
        "DOMContentLoaded",
        () => {
            wireErrorBackNavigation();
            wireDoubleSubmitProtection();
            enhanceTeacherClassSelector();
        });
})();
