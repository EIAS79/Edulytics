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
    }

    function wireClassOverviewDialogs() {
        document.querySelectorAll("[data-class-details-open]").forEach(button => {
            button.addEventListener("click", () => {
                const id = button.getAttribute("data-class-details-open");
                if (!id) return;

                const dialog = document.getElementById(id);
                if (!(dialog instanceof HTMLDialogElement)) return;

                if (typeof dialog.showModal === "function") {
                    dialog.showModal();
                }
            });
        });

        document.querySelectorAll(".academic-class-details-dialog").forEach(dialog => {
            if (!(dialog instanceof HTMLDialogElement)) return;

            dialog.querySelectorAll("[data-class-details-close]").forEach(button => {
                button.addEventListener("click", () => dialog.close());
            });

            dialog.addEventListener("click", event => {
                if (event.target !== dialog) return;

                const rect = dialog.getBoundingClientRect();
                const inside =
                    event.clientX >= rect.left &&
                    event.clientX <= rect.right &&
                    event.clientY >= rect.top &&
                    event.clientY <= rect.bottom;

                if (!inside) dialog.close();
            });
        });
    }

    function wireStudentMoveWorkflow() {
        const form = document.getElementById("student-move-form");
        if (!form) return;

        const source = document.getElementById("move-source-class");
        const target = document.getElementById("move-target-class");
        const search = document.getElementById("student-move-search");
        const rows = Array.from(document.querySelectorAll("[data-student-move-row]"));
        const selectAll = document.getElementById("student-move-select-all");
        const empty = document.getElementById("student-move-empty");
        const emptyCopy = empty?.querySelector("strong");
        const selectedCopy = document.getElementById("student-move-selected");
        const headerCount = document.getElementById("student-move-header-count");
        const visibleCount = document.getElementById("student-move-visible-count");
        const readyCopy = document.getElementById("student-move-ready-copy");
        const sourceMeta = document.getElementById("student-move-source-meta");
        const targetHint = document.getElementById("student-move-target-hint");
        const studentsPanel = document.getElementById("student-move-students-panel");
        const targetPanel = document.getElementById("student-move-target-panel");
        const review = document.getElementById("student-move-review");
        const dialog = document.getElementById("student-move-dialog");
        const reviewFrom = document.getElementById("student-move-review-from");
        const reviewTo = document.getElementById("student-move-review-to");
        const reviewCount = document.getElementById("student-move-review-count");
        const reviewStudents = document.getElementById("student-move-review-students");
        const cancel = document.getElementById("student-move-cancel");
        const stepSource = document.getElementById("student-move-step-source");
        const stepStudents = document.getElementById("student-move-step-students");
        const stepTarget = document.getElementById("student-move-step-target");
        const stepStudentsCopy = document.getElementById("student-move-step-students-copy");

        if (!source || !target) return;

        const language = (document.documentElement.lang || "en").toLowerCase();
        const isPolish = language.startsWith("pl");
        const selectedIds = new Set();
        const selectSourceCopy = form.dataset.selectSource || "Select a source class.";
        const noStudentsCopy = form.dataset.noStudents || "No students in this class.";
        const selectedTemplate = form.dataset.selectedTemplate || "{0} selected";
        const targetPlaceholder = target.options[0]?.textContent || "Select";

        const targetCatalog = Array.from(target.options)
            .filter(option => option.value)
            .map(option => ({
                value: option.value,
                text: option.textContent || "",
                yearId: option.dataset.yearId || "",
                programId: option.dataset.programId || "",
                adoptionId: option.dataset.adoptionId || "",
                levelKey: option.dataset.levelKey || "",
                classLabel: option.dataset.classLabel || option.textContent || ""
            }));

        const sourceRows = () =>
            rows.filter(row => row.dataset.classId === source.value);

        const visibleRows = () =>
            sourceRows().filter(row => !row.hidden);

        const selectedRows = () =>
            sourceRows().filter(row => {
                const checkbox = row.querySelector("input[type='checkbox']");
                return checkbox && selectedIds.has(checkbox.value);
            });

        const selectedText = count =>
            selectedTemplate.replace("{0}", String(count));

        const updateSteps = () => {
            const hasSource = Boolean(source.value);
            const hasStudents = selectedIds.size > 0;
            const hasTarget = Boolean(target.value);

            [stepSource, stepStudents, stepTarget].forEach(step => {
                step?.classList.remove("is-active", "is-complete");
            });

            if (!hasSource) {
                stepSource?.classList.add("is-active");
            } else {
                stepSource?.classList.add("is-complete");
                if (!hasStudents) {
                    stepStudents?.classList.add("is-active");
                } else {
                    stepStudents?.classList.add("is-complete");
                    if (!hasTarget) stepTarget?.classList.add("is-active");
                    else stepTarget?.classList.add("is-complete");
                }
            }
        };

        const refreshSelection = () => {
            const count = selectedIds.size;
            const copy = selectedText(count);

            if (selectedCopy) selectedCopy.textContent = copy;
            if (headerCount) headerCount.textContent = copy;
            if (stepStudentsCopy) stepStudentsCopy.textContent = copy;

            rows.forEach(row => {
                const checkbox = row.querySelector("input[type='checkbox']");
                const isSelected = Boolean(checkbox && selectedIds.has(checkbox.value));
                row.classList.toggle("is-selected", isSelected);
                if (checkbox) checkbox.checked = isSelected;
            });

            const visible = visibleRows();
            const visibleSelected = visible.filter(row => {
                const checkbox = row.querySelector("input[type='checkbox']");
                return Boolean(checkbox && selectedIds.has(checkbox.value));
            });

            if (selectAll) {
                selectAll.checked =
                    visible.length > 0 && visibleSelected.length === visible.length;
                selectAll.indeterminate =
                    visibleSelected.length > 0 && visibleSelected.length < visible.length;
            }

            const ready =
                count > 0 &&
                Boolean(source.value) &&
                Boolean(target.value) &&
                source.value !== target.value;

            if (review) review.disabled = !ready;
            if (readyCopy) {
                readyCopy.textContent = ready
                    ? (isPolish
                        ? "Gotowe do sprawdzenia przed przeniesieniem."
                        : "Ready to review before moving.")
                    : (isPolish
                        ? "Wybierz klasę źródłową, uczniów i klasę docelową."
                        : "Choose a source class, students, and a destination class.");
            }

            updateSteps();
        };

        const refreshStudents = (resetSelection = false) => {
            const sourceId = source.value;
            const query = (search?.value || "").trim().toLocaleLowerCase();

            if (resetSelection) selectedIds.clear();

            let visible = 0;
            let total = 0;

            rows.forEach(row => {
                const checkbox = row.querySelector("input[type='checkbox']");
                const inSource = Boolean(sourceId) && row.dataset.classId === sourceId;
                const matches =
                    !query ||
                    (row.dataset.search || "").includes(query);
                const show = inSource && matches;

                row.hidden = !show;
                if (checkbox) {
                    checkbox.disabled = !inSource;
                    if (!inSource) {
                        selectedIds.delete(checkbox.value);
                        checkbox.checked = false;
                    }
                }

                if (inSource) total++;
                if (show) visible++;
            });

            if (search) {
                search.disabled = !sourceId;
                if (!sourceId) search.value = "";
            }

            if (selectAll) {
                selectAll.disabled = !sourceId || visible === 0;
            }

            studentsPanel?.classList.toggle("is-disabled", !sourceId);

            if (empty) {
                empty.hidden = Boolean(sourceId && visible > 0);
                if (emptyCopy) {
                    emptyCopy.textContent = !sourceId
                        ? selectSourceCopy
                        : noStudentsCopy;
                }
            }

            if (visibleCount) {
                visibleCount.textContent = !sourceId
                    ? selectSourceCopy
                    : (isPolish
                        ? visible + " z " + total + " uczniów"
                        : visible + " of " + total + " students");
            }

            refreshSelection();
        };

        const refreshTargets = () => {
            const sourceOption = source.options[source.selectedIndex];
            const sourceId = source.value;
            const yearId = sourceOption?.dataset.yearId || "";
            const programId = sourceOption?.dataset.programId || "";
            const adoptionId = sourceOption?.dataset.adoptionId || "";
            const levelKey = sourceOption?.dataset.levelKey || "";

            target.replaceChildren();
            const placeholder = document.createElement("option");
            placeholder.value = "";
            placeholder.textContent = targetPlaceholder;
            target.appendChild(placeholder);

            const compatible = targetCatalog.filter(item =>
                Boolean(sourceId) &&
                item.value !== sourceId &&
                item.yearId === yearId &&
                item.programId === programId &&
                item.adoptionId === adoptionId &&
                item.levelKey === levelKey);

            compatible.forEach(item => {
                const option = document.createElement("option");
                option.value = item.value;
                option.textContent = item.text;
                option.dataset.classLabel = item.classLabel;
                target.appendChild(option);
            });

            target.disabled = !sourceId || compatible.length === 0;
            targetPanel?.classList.toggle("is-disabled", target.disabled);

            if (targetHint) {
                targetHint.textContent = !sourceId
                    ? selectSourceCopy
                    : compatible.length === 0
                        ? (isPolish
                            ? "Brak zgodnej klasy docelowej dla tego samego roku, programu i poziomu."
                            : "No compatible destination class exists in the same year, program, and level.")
                        : (isPolish
                            ? "Dostępne klasy docelowe: " + compatible.length + "."
                            : compatible.length + " compatible destination class" +
                                (compatible.length === 1 ? "" : "es") + ".");
            }

            refreshSelection();
        };

        const refreshSourceMeta = () => {
            if (!sourceMeta) return;
            const option = source.options[source.selectedIndex];
            sourceMeta.hidden = !source.value;
            sourceMeta.textContent = source.value
                ? (option?.textContent || "")
                : "";
        };

        source.addEventListener("change", () => {
            selectedIds.clear();
            if (search) search.value = "";
            refreshSourceMeta();
            refreshStudents(true);
            refreshTargets();
        });

        target.addEventListener("change", refreshSelection);
        search?.addEventListener("input", () => refreshStudents(false));

        rows.forEach(row => {
            const checkbox = row.querySelector("input[type='checkbox']");
            checkbox?.addEventListener("change", () => {
                if (checkbox.disabled) return;
                if (checkbox.checked) selectedIds.add(checkbox.value);
                else selectedIds.delete(checkbox.value);
                refreshSelection();
            });
        });

        selectAll?.addEventListener("change", () => {
            visibleRows().forEach(row => {
                const checkbox = row.querySelector("input[type='checkbox']");
                if (!checkbox || checkbox.disabled) return;

                checkbox.checked = selectAll.checked;
                if (selectAll.checked) selectedIds.add(checkbox.value);
                else selectedIds.delete(checkbox.value);
            });
            refreshSelection();
        });

        review?.addEventListener("click", () => {
            const sourceOption = source.options[source.selectedIndex];
            const targetOption = target.options[target.selectedIndex];
            const selected = selectedRows();

            if (!sourceOption?.value || !targetOption?.value || selected.length === 0)
                return;

            if (reviewFrom) {
                reviewFrom.textContent =
                    sourceOption.dataset.classLabel ||
                    sourceOption.textContent ||
                    "";
            }
            if (reviewTo) {
                reviewTo.textContent =
                    targetOption.dataset.classLabel ||
                    targetOption.textContent ||
                    "";
            }
            if (reviewCount) reviewCount.textContent = String(selected.length);

            if (reviewStudents) {
                reviewStudents.replaceChildren();
                selected.forEach(row => {
                    const item = document.createElement("li");
                    item.textContent = row.dataset.studentName || "";
                    reviewStudents.appendChild(item);
                });
            }

            if (typeof dialog?.showModal === "function") {
                dialog.showModal();
            }
        });

        cancel?.addEventListener("click", () => dialog?.close());

        dialog?.addEventListener("click", event => {
            if (event.target === dialog) dialog.close();
        });

        form.addEventListener("submit", event => {
            const valid =
                Boolean(source.value) &&
                Boolean(target.value) &&
                source.value !== target.value &&
                selectedRows().length > 0;

            if (!valid || form.dataset.submitting === "true") {
                event.preventDefault();
                return;
            }

            form.dataset.submitting = "true";
            const confirmButton = dialog?.querySelector("button[type='submit']");
            if (confirmButton) confirmButton.disabled = true;
        });

        refreshSourceMeta();
        refreshStudents(true);
        refreshTargets();
    }


    function classFirstLabel(label) {
        const parts = String(label || "").split("·").map(part => part.trim()).filter(Boolean);
        if (parts.length < 2) return label;
        return `${parts.at(-1)} — ${parts.slice(0, -1).join(" · ")}`;
    }

    async function wireAcademicClassRelationships() {
        const teacherClass = document.getElementById("teacher-class");
        if (!teacherClass) return;

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
        Array.from(teacherClass.options).forEach(option => {
            if (!option.value) return;
            option.textContent = classFirstLabel(
                labels.get(option.value.toLowerCase()) || option.textContent);
            option.title = option.textContent;
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

    function wireTeacherAssignmentDirectory() {
        const teacherSearch = document.getElementById("teacher-search");
        const teacherSelect = document.getElementById("teacher-id");
        const teacherClass = document.getElementById("teacher-class");
        if (!teacherSelect || !teacherClass) return;

        const yearFilter = document.getElementById("teacher-year-filter");
        const programFilter = document.getElementById("teacher-program-filter");
        const levelFilter = document.getElementById("teacher-level-filter");
        const assignmentFilter = document.getElementById("teacher-assignment-filter");
        const searchStatus = document.getElementById("teacher-search-status");
        const language = (document.documentElement.lang || "en").toLowerCase();
        let searchTimer = 0;
        let searchSerial = 0;

        const classOptions = () =>
            Array.from(teacherClass.options).filter(option => option.value);

        const applyClassFilters = () => {
            const year = yearFilter?.value || "";
            const program = programFilter?.value || "";
            const levelKey = levelFilter?.value || "";
            const assignment = assignmentFilter?.value || "";

            classOptions().forEach(option => {
                const visible =
                    (!year || option.dataset.yearId === year) &&
                    (!program || option.dataset.programId === program) &&
                    (!levelKey || option.dataset.levelKey === levelKey) &&
                    (!assignment || option.dataset.assignmentState === assignment);

                option.hidden = !visible;
                option.disabled = !visible;
                if (!visible) option.selected = false;
            });

            const visibleCount = classOptions().filter(option => !option.hidden).length;
            teacherClass.size = Math.min(10, Math.max(4, visibleCount));
        };

        [yearFilter, programFilter, levelFilter, assignmentFilter]
            .filter(Boolean)
            .forEach(control => control.addEventListener("change", applyClassFilters));

        const updateTeacherOptions = async () => {
            if (!teacherSearch) return;

            const serial = ++searchSerial;
            const query = teacherSearch.value.trim();
            if (searchStatus) {
                searchStatus.textContent = language.startsWith("pl")
                    ? "Wyszukiwanie nauczycieli…"
                    : "Searching teachers…";
            }

            let response;
            try {
                response = await fetch(
                    "/School/Users/options/teachers?search=" + encodeURIComponent(query) + "&page=1",
                    { headers: { "Accept": "application/json" } });
            } catch {
                if (serial !== searchSerial) return;
                if (searchStatus) {
                    searchStatus.textContent = language.startsWith("pl")
                        ? "Nie udało się wyszukać nauczycieli."
                        : "Teacher search is unavailable.";
                }
                return;
            }

            if (serial !== searchSerial) return;
            if (!response.ok) {
                if (searchStatus) {
                    searchStatus.textContent = language.startsWith("pl")
                        ? "Nie udało się wyszukać nauczycieli."
                        : "Teacher search is unavailable.";
                }
                return;
            }

            const payload = await response.json();
            const selected = teacherSelect.value;
            teacherSelect.replaceChildren();

            const placeholder = document.createElement("option");
            placeholder.value = "";
            placeholder.textContent = language.startsWith("pl")
                ? "Wybierz"
                : "Select";
            teacherSelect.appendChild(placeholder);

            (payload.items || []).forEach(item => {
                const option = document.createElement("option");
                option.value = item.id;
                option.textContent = item.email;
                option.selected =
                    String(item.id).toLowerCase() === String(selected).toLowerCase();
                teacherSelect.appendChild(option);
            });

            if (searchStatus) {
                const count = Number(payload.totalCount || 0);
                searchStatus.textContent = language.startsWith("pl")
                    ? "Znaleziono nauczycieli: " + count + "."
                    : count + " teacher" + (count === 1 ? "" : "s") + " found.";
            }
        };

        if (teacherSearch) {
            teacherSearch.addEventListener("input", () => {
                window.clearTimeout(searchTimer);
                searchTimer = window.setTimeout(updateTeacherOptions, 250);
            });
        }

        applyClassFilters();
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

    function wireAssessmentResultFilters() {
        const page = document.querySelector(".assessment-results-page");
        const grid = page?.querySelector(".assessment-results-grid-v2");
        if (!page || !grid || grid.dataset.studentFilterReady === "true") return;

        const cards = Array.from(grid.querySelectorAll(".assessment-result-card-v2"));
        if (cards.length <= 1) return;
        grid.dataset.studentFilterReady = "true";

        const language = (document.documentElement.lang || "en").toLowerCase();
        const isPolish = language.startsWith("pl");
        const isArabic = language.startsWith("ar");
        const labels = isPolish
            ? {
                search: "Szukaj ucznia po imieniu, nazwisku lub numerze",
                all: "Wszyscy uczniowie",
                withResult: "Z wynikiem",
                withoutResult: "Bez wyniku",
                perPage: "Na stronę",
                previous: "Poprzednia",
                next: "Następna",
                empty: "Brak uczniów pasujących do filtrów.",
                showing: (start, end, total) => `Wyświetlanie ${start}–${end} z ${total} uczniów`
            }
            : isArabic
                ? {
                    search: "ابحث باسم الطالب أو رقمه",
                    all: "كل الطلاب",
                    withResult: "لديهم نتيجة",
                    withoutResult: "بدون نتيجة",
                    perPage: "في الصفحة",
                    previous: "السابق",
                    next: "التالي",
                    empty: "لا يوجد طلاب مطابقون للفلاتر.",
                    showing: (start, end, total) => `عرض ${start}–${end} من ${total} طالب`
                }
                : {
                    search: "Search by student name or number",
                    all: "All students",
                    withResult: "With result",
                    withoutResult: "No result yet",
                    perPage: "Per page",
                    previous: "Previous",
                    next: "Next",
                    empty: "No students match these filters.",
                    showing: (start, end, total) => `Showing ${start}–${end} of ${total} students`
                };

        const toolbar = document.createElement("section");
        toolbar.className = "assessment-results-filter-bar";
        toolbar.setAttribute("aria-label", isPolish ? "Filtrowanie wyników uczniów" : isArabic ? "فلترة نتائج الطلاب" : "Student result filters");

        const search = document.createElement("input");
        search.type = "search";
        search.className = "assessment-results-search";
        search.placeholder = labels.search;
        search.setAttribute("aria-label", labels.search);
        search.autocomplete = "off";

        const status = document.createElement("select");
        status.className = "assessment-results-status-filter";
        status.setAttribute("aria-label", labels.all);
        [
            ["all", labels.all],
            ["with-result", labels.withResult],
            ["without-result", labels.withoutResult]
        ].forEach(([value, label]) => {
            const option = document.createElement("option");
            option.value = value;
            option.textContent = label;
            status.appendChild(option);
        });

        const pageSizeWrap = document.createElement("label");
        pageSizeWrap.className = "assessment-results-page-size";
        const pageSizeLabel = document.createElement("span");
        pageSizeLabel.textContent = labels.perPage;
        const pageSize = document.createElement("select");
        [5, 10, 20].forEach(size => {
            const option = document.createElement("option");
            option.value = String(size);
            option.textContent = String(size);
            pageSize.appendChild(option);
        });
        pageSize.value = "5";
        pageSizeWrap.append(pageSizeLabel, pageSize);

        const summary = document.createElement("span");
        summary.className = "assessment-results-filter-summary";
        summary.setAttribute("aria-live", "polite");

        const pager = document.createElement("div");
        pager.className = "assessment-results-pager";
        const previous = document.createElement("button");
        previous.type = "button";
        previous.className = "school-button";
        previous.textContent = labels.previous;
        const next = document.createElement("button");
        next.type = "button";
        next.className = "school-button";
        next.textContent = labels.next;
        pager.append(previous, next);

        const empty = document.createElement("div");
        empty.className = "assessment-empty assessment-results-filter-empty";
        empty.textContent = labels.empty;
        empty.hidden = true;

        toolbar.append(search, status, pageSizeWrap, summary, pager);
        grid.insertAdjacentElement("beforebegin", toolbar);
        grid.insertAdjacentElement("afterend", empty);

        const metadata = cards.map(card => {
            const name = card.querySelector("h2")?.textContent?.trim() ?? "";
            const number = card.querySelector(".assessment-result-student-header p")?.textContent?.trim() ?? "";
            return {
                card,
                searchText: `${name} ${number}`.toLocaleLowerCase(),
                hasResult: Boolean(card.querySelector(".assessment-result-total-v2"))
            };
        });

        let pageIndex = 0;

        const apply = () => {
            const query = search.value.trim().toLocaleLowerCase();
            const resultFilter = status.value;
            const size = Number.parseInt(pageSize.value, 10) || 5;
            const filtered = metadata.filter(item => {
                const matchesSearch = !query || item.searchText.includes(query);
                const matchesStatus = resultFilter === "all" ||
                    (resultFilter === "with-result" && item.hasResult) ||
                    (resultFilter === "without-result" && !item.hasResult);
                return matchesSearch && matchesStatus;
            });

            const pageCount = Math.max(1, Math.ceil(filtered.length / size));
            pageIndex = Math.min(pageIndex, pageCount - 1);
            const startIndex = pageIndex * size;
            const visible = new Set(filtered.slice(startIndex, startIndex + size).map(item => item.card));
            metadata.forEach(item => { item.card.hidden = !visible.has(item.card); });

            const start = filtered.length === 0 ? 0 : startIndex + 1;
            const end = Math.min(startIndex + size, filtered.length);
            summary.textContent = labels.showing(start, end, filtered.length);
            previous.disabled = pageIndex === 0 || filtered.length === 0;
            next.disabled = pageIndex >= pageCount - 1 || filtered.length === 0;
            empty.hidden = filtered.length !== 0;
        };

        search.addEventListener("input", () => { pageIndex = 0; apply(); });
        status.addEventListener("change", () => { pageIndex = 0; apply(); });
        pageSize.addEventListener("change", () => { pageIndex = 0; apply(); });
        previous.addEventListener("click", () => {
            if (pageIndex > 0) {
                pageIndex--;
                apply();
                toolbar.scrollIntoView({ behavior: "smooth", block: "start" });
            }
        });
        next.addEventListener("click", () => {
            pageIndex++;
            apply();
            toolbar.scrollIntoView({ behavior: "smooth", block: "start" });
        });

        apply();
    }

    function wireUserDirectoryAcademicFilters() {
        const form = document.querySelector("[data-user-directory-filters]");
        if (!form) return;

        const year = form.querySelector("[data-user-filter-year]");
        const program = form.querySelector("[data-user-filter-program]");
        const classSelect = form.querySelector("[data-user-filter-class]");
        if (!year || !program || !classSelect) return;

        const classOptions = Array.from(classSelect.options)
            .filter(option => option.value);
        const programOptions = Array.from(program.options)
            .filter(option => option.value);

        const refresh = () => {
            const yearId = year.value;
            const selectedProgramId = program.value;

            const programsForYear = new Set(
                classOptions
                    .filter(option =>
                        !yearId || option.dataset.yearId === yearId)
                    .map(option => option.dataset.programId)
                    .filter(Boolean));

            programOptions.forEach(option => {
                const visible =
                    !yearId ||
                    programsForYear.has(option.value);
                option.hidden = !visible;
                option.disabled = !visible;
            });

            if (program.value &&
                program.options[program.selectedIndex]?.disabled) {
                program.value = "";
            }

            const programId = program.value || selectedProgramId;
            classOptions.forEach(option => {
                const visible =
                    (!yearId || option.dataset.yearId === yearId) &&
                    (!programId || option.dataset.programId === programId);

                option.hidden = !visible;
                option.disabled = !visible;
            });

            if (classSelect.value &&
                classSelect.options[classSelect.selectedIndex]?.disabled) {
                classSelect.value = "";
            }
        };

        year.addEventListener("change", () => {
            refresh();
        });

        program.addEventListener("change", () => {
            refresh();
        });

        refresh();
    }

    document.addEventListener("DOMContentLoaded", () => {
        ensureRound2Stylesheet();
        wirePrintButtons();
        wireReportKindFilters();
        wireConfirmationForms();
        wireSchoolCountryTimeZones();
        wireUserDirectoryAcademicFilters();
        wireStudentWorkflowCleanup();
        wireClassOverviewDialogs();
        wireStudentMoveWorkflow();
        void wireAcademicClassRelationships().then(() => {
            wireTeacherAssignmentDirectory();
        });
        wireCurriculumLevelMultiSelect();
        simplifyStudentLearningCta();
        normalizeWholeMarkInputs();
        clarifyLearningOutcomeUx();
        wireAssessmentDeliverySafety();
        wireAssessmentResultFilters();

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
