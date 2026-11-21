# UAE MOE Grade 9 General — A–B–C to Stage D handoff

**Scope:** `UAE-MOE-MATH` / Grade **9** / General / `ar` / RTL / academic target 2026–2027.
**Owner-selected target:** 13 units, 82 numbered lessons. This is a composite General-source catalog, **not** a claim that the entire set is an official 2026–2027 textbook index.

## Source records
- `source-inventory.json`, `five-book-inventory.json` and `source-index.json` record five owner-local source PDFs: three General source editions (`01.pdf`, `02-1.pdf`, `03-1.pdf`) and two Applied **comparison-only** books (`02.pdf`, `03.pdf`).
- Source fingerprint: `SHA256SOURCESET:5e9c781829dd99b6fcd4449b5052930d15a28756abd78303466b5a5ec3fe3adc`.
- Raw General index: **86** entries across the source editions. Canonical A–B–C *authoring target*: **82** lessons in 13 units, as recorded in `grade9-82-target-index.json`; `SOURCE_INDEX_COMPLETE=false`.
- Edition conflict: modern General `01.pdf` has four geometry lessons in Unit 7, the older General `02-1.pdf` has nine different quadratic Unit 7 lessons. Four modern geometry rows are represented as supplementary/overlapping source entries, with distinct objectives added to existing U11L01, U11L03 and U12L06 as recorded in `grade9-unit7-edition-resolution.md`.
- File existence, SHA checks and opening-page raster inspections **do not establish** exhaustive per-page source-element comparisons or official 2026–2027 source equivalence.

## A–B–C artifacts and authoring status
- Lesson ordinals **1–12** have prepared closure records in `first-12-stage-a-c-closure.json`; ordinals **13–82** have `lesson-N-stage-a-c-checkpoint.json` with `closed=true`, `READY_FOR_STAGE_D` for **authoring scope only**.
- `full-grade-progress.json` reports `abcClosedLessonCount=82`, and `stageD=NOT_STARTED` at A–B–C handoff baseline.
- Arabic student pack, per-lesson RichV2, cooldowns, practice mappings and prepared tests are referenced in those checkpoints. The relevant Node and .NET test specifications were **WRITTEN_NOT_RUN** at authoring closure; no automatic `D_VERIFIED` is implied.
- `semantic-audit-ledger.json` has 86 raw entries but **zero** `fullLessonAuditPassed=true` and zero positive `elementCountDenominator` entries. `grade9-82-target-index.json` has **zero** `ElementCoverage=FULL_SOURCE_ELEMENT_1_TO_1_VERIFIED` entries. This is an explicit **source parity evidence gap**, not proof the authored material is incorrect.
- Unit 7 supplement inventories are in `u07-new-geometry-7-1-source-element-preparation.json`, `...7-2...`, `...7-3...`, and `u07-new-geometry-7-4-vs-11-3-source-comparison.json`. Source 7-3 numbered exercise 76 was not visually located in earlier review.

## Stage D transfer decision
- `A_B_C_AUTHORING_READY=82/82`.
- `GRADE9_STAGE_D_HANDOFF_GATE=D_PENDING_A_C_EVIDENCE`; **not** full academic-source verification.
- `SOURCE_INDEX_COMPLETE=false`, exact per-item 1:1 parity `UNKNOWN`.
- **First D-verifiable lesson:** ordinal 1 after the source-evidence gate is satisfied, not ordinal 13 or 82.
- Stage D can run technical diagnostics and record failures while blocked; it must not authorize deployment or downgrade source gate requirements.
- Grade 9 owner academic content approval and explicit release intent are documented separately in `grade9-owner-academic-release-decision.md`. This does not establish third-party image licensing or satisfy technical and E2E tests.
- No live student records or production database should be modified to simulate test completion.

**Evidence references:** `source-inventory.json`, `edition-diff.json`, `source-index-lock-evidence.md`, `grade9-82-target-index.json`, `first-12-stage-a-c-closure.json`, `full-grade-progress.json`, `semantic-audit-ledger.json`, `grade9-stage-d-execution-ledger.json`.

**Correct grade:** 9. Stray `Grade: 8` in the uploaded Stage D template is a template typo and must not enter runtime metadata.
