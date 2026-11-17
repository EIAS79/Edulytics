# UAE Grade 8 General Arabic — A–B–C completion / Stage D handoff

**Scope:** UAE-MOE-MATH, Grade 8, General, target school year 2026–2027, Arabic RTL.
**Branch:** `curriculum/uae-g8-general-ar-student-first-abc-20261116`. **Source of truth:** owner-selected local Student Books under `C:\Users\khali\Downloads\New folder (2)\emirates\Grade_08\01_Student_Books`.

## Actual recorded state (2026-11-17)

- Source-selected numbered index: **12 units / 76 lessons**. 76/76 have individually committed A–B–C candidate checkpoints, Arabic authoring, lesson evidence and written (NOT_RUN) tests. Source element/task **atomic one-to-one parity is not independently verified**, and teacher-reviewed open tasks are flagged. No CI, full build, or published render tests were run for A–B–C.
- Before this cleanup the old pack and manifest listed **77** records, all with English translations, and `AcademicLanguage=en`. The final extra record `PED:UAE-MOE-MATH:L8:GENERAL:12:05:INFINITE-SETS` does not appear in selected local Student Book Unit12. The source `03-9-12.pdf` identifies lesson 12:04 as its last numbered lesson (PDF 275–284), then unit review/test/projects (PDF 285–289). The selected source contains **four**, not five, numbered lessons in Unit12.
- This branch-only cleanup removes the **legacy 77th lesson** and old English translation content from all 76 selected Grade8 General lessons, sets language `ar`, and aligns the unit titles and manifest. **No production Neon records or student progress were deleted or mutated.** It does **not** change Grade8 Advanced or US-CCSS.
- Repository metadata's original `Status=Published` was inherited from the old content pack and is **not** evidence that the new Arabic candidate has passed release validation.

## Non-negotiable Stage D blockers

1. Existing academic approval establishes owner permission to publish authored lessons, but **does not prove authorization to reproduce publisher content**. Only first-party explanations, tasks, tables, diagrams and cooldowns may ship unless separately licensed.
2. `SemanticGraphSha256` / `SemanticSha256` hashes are from the old content registry and **must be regenerated** as part of canonical source-index migration. Existing hash strings are stale for the 76-lesson candidate.
3. Review any production Grade8 General rows linked to 12:05, dependencies, attempts, submissions, and migrations before permanent removal. Perform transactional rollback-capable retirement, do not silently orphan records.
4. Check actual lesson rendering, RTL, zero English fallbacks, 76 Arabic pages, answer engine contracts and teacher-reviewed open tasks on the final SHA. Tests authored during A–B–C are `WRITTEN_NOT_RUN`.
5. Run Stage D validation/CI and deployment readiness on that exact SHA. **Do not merge/deploy based solely on 76/76 A–B–C checkpoints.**

**Source index evidence:** `artifacts/curriculum-delivery/UAE-MOE-MATH/G8/source-index-76-arabic-reconciliation.json`.
