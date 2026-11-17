# Edulytics — UAE MOE Mathematics Grade 8 General — student-book-first A–B–C execution policy

**Owner instruction date:** 2026-11-16. **Target academic year:** 2026–2027. **Curriculum:** `UAE-MOE-MATH`. **LogicalLevel:** `8`. **Pathway:** `General` (the established persistence identity; `Common` in prose never authorizes changing existing IDs). **Academic language:** `ar` / Arabic RTL.

This policy is the mandatory Grade 8 implementation companion to the owner's Grade 8 A–B–C plan. It documents *execution rules*, not completed lessons or an independently certified annual textbook edition. Do not convert instructions into fabricated implementation or test evidence.

## Verified starting facts (GitHub main inspected on 2026-11-16)

- `docs/curriculum/UAE_G8_GENERAL_OFFICIAL_MANIFEST.json` already exists: **12 units / 77 English-title records**, last unit 12 contains five lessons, including `Infinite Sets` as 12:05. The owner-selected Arabic source table of contents in `03-9-12.pdf` reports **12 units / 76 numbered lessons**, last unit 12 contains four. **Treat the 77th item as `SOURCE_INDEX_CONFLICT` pending page-level review; do not silently delete a persisted identity, student progress or assignments.**
- Existing Grade 8 General blueprint: `src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g8-general-t1-ogl-v1.lesson-blueprint.json` (77 lessons). Existing content: `src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g8-general-t1-ogl-v1.lesson-content-pack.json` (**`AcademicLanguage: en`**, 77 lessons, `Status: Published`). These are **existing English materials to inspect, preserve any correct original mathematics, then replace with Arabic lesson-specific content before activation**. Neither `Published` nor a nominal 2026–2027 `VersionCode` proves source fidelity.
- No Grade 8 UAE A–B–C closure ledger was present in `artifacts/curriculum-delivery/UAE-MOE-MATH/G8/` on inspected main. Cursor therefore starts at the first unaccepted source-index lesson, initially **U01:L01 — الأعداد النسبية**, unless a newer Grade 8 PR/ledger demonstrably closes it.
- Separate **UAE Grade 8 Advanced** and **US-CCSS Grade 8** are out of scope and must be untouched.

## Binding local source policy

**Read-only Our-CS source root:** `C:\Users\khali\Downloads\New folder (2)\emirates\Grade_08`. **Student source:** `01_Student_Books`. Student book is controlling; teacher guide is supplementary. Files verified present: `01-1-4.pdf`, `02-5-7.pdf`, `03-8.pdf`, `03-9-12.pdf`. The newer `01-1-4.pdf` bears 2025–2026 evidence; the 12-unit TOC in `03-9-12.pdf` bears 2019–2020 evidence. The scanned 2025 file has no extractable text on inspected TOC pages; **empty text extraction is not absence of content**.

Select the **newest available compatible Arabic Student Book for each matched lesson**. For lessons/units not present in that edition, use the other owner-supplied Arabic books with explicit `SelectedSourceEdition`, `SelectedSourceFile`, PDF/printed page and SHA-256. Do not combine different TOCs without an edition crosswalk. The *working scope* is the owner's locally selected **12/76** index, but do not assert that it is the independently verified 2026–2027 official distribution. The 2025 TOC, 2019 TOC and live 77-lesson manifest must be reconciled without losing a lesson from the chosen Arabic source. Source age does not block learning content when its objectives and skill are still selected by the owner.

The printed source index begins U01:L01 `الأعداد النسبية` at printed page 7, U01:L02 `القوى والأسس` at 15. The newer book's local PDF page 5 visibly lists the same first-unit sequence including numbered lessons 1–10 and required discovery labs; labs and formative assessments count as lesson **elements**, not extra numbered lessons by default.

## Strict execution algorithm

1. **One grade-wide read, one source-preparation batch per unit.** Capture filenames, size, edition, SHA-256, TOC, printed/PDF offsets and relevant goal/example/task/diagram/page locations. Cache evidence by hash. Minimize Our-CS Desktop Commander calls; do not download local books or run general web searches for them.
2. **Inspect actual app records first** (source manifest + blueprint + canonical pack + optional read-only DB/Neon evidence), including `LessonCode`, pathway, language, status, linked attempts and dependent references. Categorize `KEEP`, `FIX`, `ADD_MISSING`, `LEGACY_ENGLISH`, `DUPLICATE`, `SOURCE_INDEX_CONFLICT` and `SOURCE_MISSING`. Existing code uses `L8:GENERAL` and descriptive suffixes: **never rename existing identities to the proposed `L8:COMMON` pattern without migration**.
3. **Sequential cursor:** resume at the first lesson not actually closed in the current PR/ledger; process A → B → C without switching to another lesson. Stage A compares goals, definitions, examples, visuals, activities, warm-up, practice, formative questions and source-linked labs. Stage B surgically corrects existing instruction, with original Arabic worked examples/diagrams and clear `ar` + RTL. Stage C maps exact `SkillContract` / generator / renderer / independent grader or human-reviewed rubric.
4. **Reuse → Extend → New Engine**, in that order. No new engine if the existing exercise family can support the response and checking semantics. Add a pedagogically equivalent original cooldown using the lesson goal/skill when the teacher-only cooldown is inaccessible; record it as `EDULYTICS_AUTHORED_COOLDOWN`, not verbatim textbook content.
5. **Close before continuing:** update source-element ledger, coverage, lesson checkpoint, and one GitHub commit. An atomic commit may include the lesson's changes and evidence; one commit per lesson is preferred. Do not mark `READY_FOR_STAGE_D` when material objectives, activities, diagram coverage or language remain missing.
6. **Move immediately to the next lesson.** Prepare all sources for a unit in advance, but close lessons in book order; issue a consolidated report after each completed unit and continue without asking permission.
7. **A–B–C lightweight checks only:** inspect JSON validity, math invariants and references; write lesson-level cases for Stage D as `WRITTEN_NOT_RUN`. Do not run full CI, full build, Playwright or Stage D per lesson. Three minutes/lesson is an efficiency target, never fabricated elapsed time or a reason to bypass academic requirements. Record real start, close, duration and tool calls where measurable.

## Existing English content and deletion boundary

The old English Grade 8 General curriculum is **not** an acceptable learner-facing Arabic fallback. Do not merely change `AcademicLanguage: en` to `ar` while leaving English learner text. Migrate/replace the actual lessons and their referenced material, establish canonical Arabic registry resolution and enforce no English fallback. Retire superseded English-only blueprint/content on the candidate **only after** Arabic replacement and path/identity review. Do not delete Advanced/US packs or mutate live Neon, student records, attempts or tenant adoptions as part of A–B–C. Guarded production retirement, database migration, exact-head CI and release belong to Stage D.

## Evidence and status

Under `artifacts/curriculum-delivery/UAE-MOE-MATH/G8/`, store or reuse canonical `source-selection.json`, `source-inventory.json`, `source-index.json`, `edition-diff.json`, source-element coverage, per-lesson checkpoint and per-unit summary. Do not duplicate an existing authoritative ledger. Every element needs source file/hash/page, objective/task type, proposed first-party equivalence, artifact, open gap and review status. Accept `A_C_PENDING`, `A_C_IN_PROGRESS`, `A_C_EXPLICIT_GAP`, `READY_FOR_STAGE_D`; never fabricate `D_VERIFIED`, `GO`, exact item 100%, CI passes, textbook rights or human reviews.

Separate **owner academic approval** from evidence of source-element parity, rights to reproduce third-party assets and tested release readiness. Use original Edulytics images, wording, diagrams and assessments unless specifically licensed otherwise.

**Unit order for the owner-selected 12/76 plan:** 1 real numbers (10), 2 single-variable equations (5), 3 two-variable equations (8), 4 functions (9), 5 triangles and Pythagoras (7), 6 transformations (4), 7 congruence/similarity (7), 8 volume/surface area (6), 9 scatter/data (6), 10 geometry tools (4), 11 quadrilaterals (6), 12 sets (4). Current registered manifest is 77; reconcile extra U12:L05 with the local selected source instead of treating it as a new book lesson by assumption.

**No grade-wide release or deploy under this policy.** Stage D requires a separate plan and evidence on the final SHA.
