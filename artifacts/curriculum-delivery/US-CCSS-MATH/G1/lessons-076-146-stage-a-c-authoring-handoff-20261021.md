# Edulytics Grade 1 US-CCSS Mathematics: Lessons 076–146 A–C authoring handoff

**Recorded:** 2026-10-21 · **Repository:** `EIAS79/Edulytics` · **Authoring branch:** `curriculum/g1-continuous-authoring-final-ci-20261019`
**Pre-handoff commit:** `287df4ce4d806d38a06ede02096bb9e91a7dd3f0`
**Scope:** Strict chronological lessons **76 through 146 inclusive (71 lessons)**, not all of Grade 1.
**Checkpoint inspection:** Each of the 71 existing files `lesson-{N}-v2-stage-a-d-checkpoint.json` was checked for `aToCClosure.outcome`. All 71 currently report **`READY_FOR_STAGE_D`**, with `openACDefects: []`. This is the **authoring checkpoint state**, not verified live academic acceptance.

## What was implemented or rechecked

- Lessons **76–106**: previous source-scoped authoring checkpoints re-read, including the restored original lesson-77 provenance reconciliation; numbered 76–106 each records `READY_FOR_STAGE_D`.
- Lessons **107–119 / Unit 6**: existing 2021-source corrections and numerically valid first-party proxy questions were reviewed in grade order. New standalone tests: `tests/Edulytics.Tests/MathematicsIntelligence/UsCcssG1Lessons107To119SourceScopedRegressionTests.cs` (`9952aad859e17cb957ba4af07dd9337be60f27c0`). Covers paper clips without gaps, three-unit same-length counts, counts over 100, four original named shoe stories, comparison bracelets, missing-start beads, six original two-equation stories, origami and source number talk. **Real length measurements, original pictures, paper shapes and pupil explanations require a teacher.**
- Lesson **120 / Unit 7 Lesson 1**: solids attribute digital check re-reviewed; tactile sorting, bags, group rationale and genuine class-made shapes remain human-assessed.
- Lessons **121–136 / Unit 7**: geometric composing, flat-shape groups, six triangle/nontriangle drawings, 16 rectangle/square drawings, halves/fourths, center partner work, clock arithmetic and clock drawing. Authored `UsCcssG1Lessons121To136SourceMathTests.cs` (`8891bd6bedca4d0181affb726e111db462eaab15`). Added original four three-addend number-talk examples to ordinal **127** (`5ed9081995f054be6929291392792ac93bb372ba`) and a clock card with **hour hand only** (132), plus **all 12 valid half-past outcomes** when the source card provides only a minute hand at six (134) (`2a315430a4c3d6f27be61e360adc1557406616e0`). Independent tests were authored but **not executed**.
- Lessons **137–146 / Unit 8**: new source-specific authored tests in `UsCcssG1Lessons137To146SourceMathTests.cs` and `UsCcssG1OriginalMissingWarmupEquationsTests.cs`. Source-matched missing-number family for **138** extended; **139** grades any one correct source addition expression paired with any one correct subtraction expression, not just a single preselected pair; **140** adds original three equal-to-17 warmups; **141** adds three exact true/false calculations. Previously authored pupil-created ride stories (142) remain teacher assessed. **144** now genuinely accepts the student's favorite number **20–100** and any **three distinct valid addition expressions** (including commutatively equivalent expressions being recognized as duplicates); prefilled source-math examples statically checked for every integer 20–100. **145** preserves all three candidates 34, 44, 54 before final clue selects 44; **146** grades a first-party scaffold for four consistent clues with final addition expression while the actual pupil-authored card set and peer solving require teacher review.
- The static A–C correction caught an extra closing parenthesis in `UsCcssG1FullGradeUnit8ReviewQuestions.Verify` (`c53993becb5ee994d200c7cc37d6fd9f7ff6fdcf`) and aligned older Unit 8 test mode counts and student-riddle test expectations (`43beadd6ccfc0aca84fe42f8564c3f539f92bc02`). Unit 8 lesson 144 now consistently rejects number 101 in this source number-book task, while other lessons still use 1–120 where specified.

## Exact source-vs-digital evidence boundary

The 2021 curriculum source has **teacher-facilitated physical activities, real student artwork, construction and cut/fold tasks, illustrated source cards, pupil-authored questions, oral justifications and group games**. Text/numeric mathematics aligned to an original lesson goal is **NOT** evidence of a pupil completing those genuine tasks. In these 71 lessons:
1. First-party Edulytics numeric models, textual instructions, explicit mathematical oracle answers, negative-answer tests and separate teacher-review requirements can be **authored and statically reviewed** in stages A–C.
2. Original visual/source-figure identity, licensed artwork, authenticated learner displays, actual physical measurement, original pupil drawings, actual classroom partner interaction, original protected publisher cooldowns, and full real lesson acceptance are **NOT certified** by the present A–C checkpoint.
3. `READY_FOR_STAGE_D` means **the narrowly documented source-scoped authoring candidate is ready to enter the withheld verification gate**. It does **not** mean .NET compilation, unit tests, runtime correctness, CI, human signoff or release succeeded.

## Stage D gate — intentionally not started

- **No `dotnet build`, `dotnet test` or GitHub Actions manually triggered** during this A–C authoring continuation. Thus the numerous committed tests are **prepared, not proven green**.
- **No authenticated end-to-end learner/teacher browser inspection**, persistence check, screenshot/source-picture/license validation or final academic acceptance was performed at this head.
- **No merge, deployment, schema migration applied to running environment or production promotion.** Existing grade release stays `NO_GO`; do not interpret any authoring checkpoint as whole-grade signoff.
- Recommended **future Stage D only after the user authorizes the gate**: freeze exact SHA; compile, run the targeted and broader .NET tests, repair regressions; verify source-goal learner rendering and actual saved grades under authenticated roles; independently inspect figure provenance and accessibility; arrange teacher/subject-supervisor acceptance for nonnumeric tasks; then revisit release.

## Continuation rules

Only the connected **GitHub repository/authoring branch** is used for writes; do not edit/read a Windows `Our-CS` project clone as a substitute for GitHub. Respect canonical ordinal order. Do not restart the already authored 76–146 sequence or claim `Stage D` without explicit authorization. If an error is found during a future validation, record the **specific regression** and fix it on a controlled follow-up; never silently assert tests passed. Keep source-derived numeric facts separate from first-party invented examples, teacher-only tasks and unverified original images.

**Verified checkpoint outcome:** 71 / 71 authoring checkpoints recorded `READY_FOR_STAGE_D`.
**Execution verification:** NOT RUN. **Release:** `NO_GO`. **Stage D:** ON HOLD.
