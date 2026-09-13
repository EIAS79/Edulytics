(() => {
    "use strict";

    const form =
        document.querySelector(
            "form[data-report-filter-form]");

    if (!form) return;

    ["academicYearId", "classGroupId"]
        .map(id => document.getElementById(id))
        .filter(Boolean)
        .forEach(select => {
            select.addEventListener(
                "change",
                () => form.requestSubmit());
        });
})();
