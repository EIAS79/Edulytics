# Grade 9 General — mandatory A–B–C execution rules

**Owner mandate:** source-based correction, **not** wholesale redevelopment or generic generated curriculum. Work on branch `fix/g9-general-full-year-scope-86`, draft PR #650, and adhere to the owner's latest uploaded `EDULYTICS_UAE_GRADE_9_STAGE_A_B_C_100_PERCENT_EXECUTION_PLAN.md` (19 November 2026). This is the controlling execution plan when older PR language disagrees.

## Scope and source precedence

- `UAE-MOE-MATH` / Grade 9 / **General**, target 2026–2027, **ar/RTL**.
- First choice for the unit and lesson index is the **newest General student book supplied by the owner**. Add unique objectives/skills from the older eligible General books without duplicates. Different edition years do not block coverage.
- Five available PDFs: General `01.pdf`, `02-1.pdf`, `03-1.pdf`; Applied `02.pdf`, `03.pdf` for cross-reference only. Advanced excluded.
- Authorized local source root: `C:\Users\khali\Downloads\New folder (2)\emirates\Grade_09`; student books under `01_Student_Books`. Read scanned PDFs locally; do not browse for books already present. Teacher guide may be used as supporting evidence if it actually exists; do not invent one.
- **Latest owner-approved target is 13 units and 82 lessons**, not the older PR's 14/86 target. The 86 source entries across 14 raw edition units are comparative evidence **only**. Build and verify the 13/82 canonical target from the five-book audit and the historical 9-lesson Unit 7; resolve all edition collisions and duplicate skills without fabricating pages or lessons. Until 82 unique evidenced rows have been locked, `SOURCE_INDEX_COMPLETE=false` and report any `SOURCE_GAP` honestly. One semantic duplicate is documented, which alone does not reconcile 86 to 82.
- Old Edulytics registry content is an existing implementation to repair, **not academic authority**; preserve correct teaching/content/engine code. Do not import the American or Polish curriculum or Applied/Advanced lessons into General.

## Mandatory sequence — no skip, no random edits

`G9_FIVE_BOOK_AUDIT → G9_13_UNITS_82_LESSONS_LOCK → first genuinely unclosed lesson → open existing lesson → local source pages → objective/example/activity/exercise/diagram comparison → retain correct parts → repair missing/incorrect parts (A,B,C) → source-specific cool-down if needed → source evidence and rubric → checkpoint → one GitHub closure commit → next lesson`.

- Current first unclosed lesson: **2-5** (source ordinal 13 of the raw pre-reconciliation index); preserve this cursor until canonical index crosswalk establishes its final ordinal. The previously prepared first 12 lessons remain intact, subject to the final source crosswalk.
- Do **not** advance the ordinal while `A_C_EXPLICIT_GAP` is recorded. A lesson may become `READY_FOR_STAGE_D` only after its A–B–C evidence and actual content/SkillContract/visual requirements are complete.
- Read source pages in batches and cache verified SHA + page locators; minimize Desktop Commander calls. Use the connected `Our-CS` device rather than TinyFish.
- For each mathematical skill use **Reuse → Extend → New Engine**. No duplicate generator for already supported answer semantics; use an extension only if existing cases genuinely lack coverage.
- Create missing lessons only after confirming no equivalent exists in the candidate package/lesson registry. Preserve all correct existing examples and assets.
- Author an original Arabic **cool-down** from the source learning objectives if a teacher-gated book section cannot be read, with separate teacher review for drawings and proof.
- Target at most three minutes of A–B–C work per lesson as a performance goal, not an excuse for missing evidence. Record true timestamps and call counts; do not insert fictional durations. Prefetch source excerpts for a full unit and issue fewer desktop calls. Do not run tests or CI during A–B–C; write targeted cases for Stage D.
- One clear closing GitHub commit per fully closed lesson, batching edits within that commit. Do not scatter needless intermediary commits. Do not generate a progress report after every lesson. Continue through the unit before unit-level reporting; avoid stopping merely to request further approval.

## Stage split from attached plan

- **A–B–C:** inspect source; author/correct lessons; write specific test cases/fixtures; register `TestsRun=WRITTEN_NOT_RUN`. Never intentionally launch full build/restore, tests, Playwright, CI, merge, Neon production writes or deployment under A–B–C.
- **Stage D only:** execute tests, CI and E2E, adjudicate content/runtime validity, reconcile isolated database and production deployment.
- The owner's academic approval is noted, but content provenance/rights and the actual test/deployment statuses must still be reported accurately.
- Checkpoints include `SourceSnapshotId`, lesson code, grade/pathway/language, actual source page evidence, reused/extended practice families, teacher-reviewed activities, GitHub commit, `ClosedUtc` only when truly closed.

**No completion shortcut:** a written contract is not a passed generator; a static activity is not learner-runtime verification; a draft lesson is not a closed lesson; and an unverified 86-entry source union is not a locked final unique catalog.

## 19 November 2026 owner-plan precedence and source integrity correction

- Uploaded target plan supersedes older PR #650 summaries that label 14/86 as an approved final catalog. Raw 14/86 inventory stays preserved as provenance, not a final deliverable or reason to invent/defer skills. Report discrepancies through exact source-to-target mapping; do not silently change 13/82 to 14/86.
- The latest 2025–2026 General student book prevails where editions conflict; older General volumes supply evidenced unique lessons. Applied volumes are reviewed but never imported merely by matching topic. Verify local physical and printed page numbers, not inferred offsets.
- The student book is the primary source for learner examples, exercises and lesson index; teacher material, when available, is supplementary. A book's publication year does not veto a skill-based match.
- Closure order: source page/objective/exercise/visual audit → retain correct live lesson content → surgical correction → independent practice-family contract and original Cool-down → authored targeted tests (not executed here) → evidence ledger → checkpoint → closing commit → next lesson. Do not advance while `A_C_EXPLICIT_GAP` remains.
- Use one unit report only after finishing that unit and then continue sequentially; no scheduling and no unnecessary GitHub or desktop polling. An incomplete lesson remains open irrespective of elapsed time.
