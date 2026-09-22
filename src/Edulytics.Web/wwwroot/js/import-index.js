(() => {
    const importType = document.getElementById("importType");
    const academicYearField = document.querySelector("[data-classes-academic-year]");
    const academicYear = document.getElementById("academicYearId");

    const syncClassesFields = () => {
        if (!importType || !academicYearField || !academicYear) {
            return;
        }

        const isClasses = importType.value === "Classes";
        academicYearField.hidden = !isClasses;
        academicYear.required = isClasses;

        if (!isClasses) {
            academicYear.value = "";
        }
    };

    importType?.addEventListener("change", syncClassesFields);
    syncClassesFields();

    const builder = document.querySelector("[data-assessment-results-template]");
    if (!builder) {
        return;
    }

    const yearSelect = document.getElementById("assessmentResultAcademicYearId");
    const assessmentSelect = document.getElementById("assessmentResultAssessmentId");
    const classInput = document.getElementById("assessmentResultClass");
    const download = builder.querySelector("[data-assessment-results-download]");

    if (!yearSelect || !assessmentSelect || !classInput || !download) {
        return;
    }

    const templateUrl = builder.dataset.templateUrl || "";
    const allOptions = Array.from(assessmentSelect.options)
        .slice(1)
        .map(option => ({
            value: option.value,
            text: option.textContent || "",
            yearId: option.dataset.yearId || "",
            className: option.dataset.className || "",
            assessmentDate: option.dataset.assessmentDate || ""
        }));

    const resetDownload = () => {
        download.removeAttribute("href");
        download.setAttribute("aria-disabled", "true");
    };

    const renderAssessments = () => {
        const selectedYear = yearSelect.value;
        assessmentSelect.innerHTML = "";

        const placeholder = document.createElement("option");
        placeholder.value = "";
        placeholder.textContent = "Select the assessment";
        assessmentSelect.appendChild(placeholder);

        for (const item of allOptions.filter(x => x.yearId === selectedYear)) {
            const option = document.createElement("option");
            option.value = item.value;
            option.textContent = item.text;
            option.dataset.className = item.className;
            option.dataset.assessmentDate = item.assessmentDate;
            assessmentSelect.appendChild(option);
        }

        assessmentSelect.disabled = selectedYear.length === 0 ||
            assessmentSelect.options.length <= 1;
        classInput.value = "";
        resetDownload();
    };

    const syncAssessment = () => {
        const selected = assessmentSelect.selectedOptions[0];
        if (!selected || !selected.value) {
            classInput.value = "";
            resetDownload();
            return;
        }

        classInput.value = selected.dataset.className || "";
        if (!templateUrl) {
            resetDownload();
            return;
        }

        const separator = templateUrl.includes("?") ? "&" : "?";
        download.href = templateUrl + separator +
            "assessmentId=" + encodeURIComponent(selected.value);
        download.removeAttribute("aria-disabled");
    };

    yearSelect.addEventListener("change", renderAssessments);
    assessmentSelect.addEventListener("change", syncAssessment);

    renderAssessments();
})();
