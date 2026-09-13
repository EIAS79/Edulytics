(() => {
    const importType = document.getElementById("importType");
    const academicYearField = document.querySelector("[data-classes-academic-year]");
    const academicYear = document.getElementById("academicYearId");

    if (!importType || !academicYearField || !academicYear) {
        return;
    }

    const syncClassesFields = () => {
        const isClasses = importType.value === "Classes";
        academicYearField.hidden = !isClasses;
        academicYear.required = isClasses;

        if (!isClasses) {
            academicYear.value = "";
        }
    };

    importType.addEventListener("change", syncClassesFields);
    syncClassesFields();
})();
