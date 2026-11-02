(() => {
    "use strict";

    const page = document.querySelector(".assessment-results-page");
    const roster = page?.querySelector(".assessment-results-roster");
    if (!page || !roster || page.querySelector(".assessment-results-filter-bar")) return;

    const rows = Array.from(roster.querySelectorAll(".assessment-result-roster-row"));
    if (rows.length === 0) return;

    const language = (document.documentElement.lang || "en").toLowerCase();
    const isPolish = language.startsWith("pl");
    const isArabic = language.startsWith("ar");
    const labels = isPolish
        ? {
            search: "Szukaj ucznia po imieniu lub nazwisku",
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
                search: "ابحث باسم الطالب",
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
                search: "Search by student name",
                all: "All students",
                withResult: "With result",
                withoutResult: "No result yet",
                perPage: "Per page",
                previous: "Previous",
                next: "Next",
                empty: "No students match these filters.",
                showing: (start, end, total) => `Showing ${start}–${end} of ${total} students`
            };

    const toolbar = document.createElement("div");
    toolbar.className = "assessment-results-filter-bar";

    const search = document.createElement("input");
    search.type = "search";
    search.className = "assessment-results-search";
    search.placeholder = labels.search;
    search.setAttribute("aria-label", labels.search);

    const resultStatus = document.createElement("select");
    resultStatus.className = "assessment-results-status-filter";
    [["all", labels.all], ["with-result", labels.withResult], ["without-result", labels.withoutResult]].forEach(([value, text]) => {
        const option = document.createElement("option");
        option.value = value;
        option.textContent = text;
        resultStatus.appendChild(option);
    });

    const pageSizeLabel = document.createElement("label");
    pageSizeLabel.className = "assessment-results-page-size";
    pageSizeLabel.append(document.createTextNode(`${labels.perPage} `));
    const pageSize = document.createElement("select");
    [5, 10, 20].forEach(value => {
        const option = document.createElement("option");
        option.value = String(value);
        option.textContent = String(value);
        pageSize.appendChild(option);
    });
    pageSize.value = "5";
    pageSizeLabel.appendChild(pageSize);

    const summary = document.createElement("div");
    summary.className = "assessment-results-filter-summary";

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
    empty.hidden = true;
    empty.textContent = labels.empty;

    toolbar.append(search, resultStatus, pageSizeLabel, summary, pager);
    roster.closest(".assessment-results-roster-shell")?.insertAdjacentElement("beforebegin", toolbar);
    roster.closest(".assessment-results-roster-shell")?.insertAdjacentElement("afterend", empty);

    let currentPage = 1;
    const searchableText = row => (row.textContent || "").toLocaleLowerCase();
    const hasResult = row => row.dataset.hasResult === "true";

    const render = () => {
        const query = search.value.trim().toLocaleLowerCase();
        const status = resultStatus.value;
        const size = Number(pageSize.value) || 5;
        const matching = rows.filter(row => {
            if (query && !searchableText(row).includes(query)) return false;
            const resultExists = hasResult(row);
            if (status === "with-result" && !resultExists) return false;
            if (status === "without-result" && resultExists) return false;
            return true;
        });

        const pageCount = Math.max(1, Math.ceil(matching.length / size));
        currentPage = Math.min(Math.max(1, currentPage), pageCount);
        const startIndex = (currentPage - 1) * size;
        const visible = new Set(matching.slice(startIndex, startIndex + size));

        rows.forEach(row => { row.hidden = !visible.has(row); });
        empty.hidden = matching.length !== 0;
        summary.textContent = matching.length === 0
            ? labels.showing(0, 0, 0)
            : labels.showing(startIndex + 1, Math.min(startIndex + size, matching.length), matching.length);
        previous.disabled = currentPage <= 1;
        next.disabled = currentPage >= pageCount || matching.length === 0;
    };

    search.addEventListener("input", () => { currentPage = 1; render(); });
    resultStatus.addEventListener("change", () => { currentPage = 1; render(); });
    pageSize.addEventListener("change", () => { currentPage = 1; render(); });
    previous.addEventListener("click", () => { currentPage -= 1; render(); });
    next.addEventListener("click", () => { currentPage += 1; render(); });

    render();
})();