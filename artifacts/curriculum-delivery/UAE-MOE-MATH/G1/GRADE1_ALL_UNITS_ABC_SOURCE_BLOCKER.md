# UAE Grade 1 / Stage A–B–C — source coverage and sequential closure gate

Updated after the 2025–2026 Arabic guide and older Arabic student-book volumes became available.

**Authoritative inventory:** [grade1-verified-arabic-source-baseline-20261108.md](grade1-verified-arabic-source-baseline-20261108.md).

- Local `01.pdf`: 2025–2026 Arabic student book, 368 PDF pages; teaching through printed U3.
- Local `02.pdf`: older 2019–2020 Arabic student book, 82 pages, U4.
- Local `03.pdf`: older 2019–2020 Arabic student book, 301 pages, teaching U7–U10.
- Arabic 2025–2026 MOE comprehensive guide, term 2: U4 (8 lessons), U5 (15).
- Arabic 2025–2026 MOE comprehensive guide, term 3: U6 (8), U7 (6), U8 (9), U9 (10), U10 (4).
- The 11 **technical** units include the introductory A unit; the printed course has 10 numbered units. Existing count 102 is **structure-crosschecked**, not certified for completed source element coverage.
- **New local candidate verified 2026-11-08:** `Grade_01/01_Student_Books/02-1.pdf` (194 PDF pages; SHA-256 `B9E92BD0A3E66A57DF48DA85D569ED95A1CBA2D0F30232DFF79048950C30E4F3`). Visual inspection of PDF pages 1–2 confirms Arabic Grade 1 Common **2019–2020**; PDF pages 4–6 contain a TOC naming printed U5 `القيمة المكانية` (15 lessons) and U6 `جمع الأعداد المكونة من رقمين وطرحها` (8 lessons). This establishes an **older student-book candidate** for previously missing U5/U6, not 2025–2026-equivalent page-by-page evidence. The actual lesson teaching/exercise pages and newer-edition differences must still be examined before closing affected lessons. Do not import protected pages into GitHub.
- **Superseded closure note:** earlier in this file, the first six lessons were recorded as gaps. Later per-lesson checkpoints and `grade1-all-11-units-abc-coverage-ledger.json` on this branch explicitly close lessons 1–9 as `READY_FOR_STAGE_D` with targeted tests `WRITTEN_NOT_RUN`; this older note must not override those newer artifacts.
- **Current documented cursor: lesson 10**, `PED:UAE-MOE-MATH:L1:COMMON:02:04` (`Add Zero`). It remains unclosed until A–B–C source comparison, scoped content/questions and evidence/commit are complete.
- Stage D tests / CI / full builds not run. Never label `WRITTEN_NOT_RUN` as passed.

Source filenames are newly organized under `Grade_01\01_Student_Books`; previous local file paths in old artifacts are stale and must be reconciled on a source-specific basis.
