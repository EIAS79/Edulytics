# Grade 1 ordinal 61 — U03 Lesson 24 — A–C source / first-party content / math audit (2026-10-20)

**Lesson code:** `PED:US-CCSS-MATH:G1:U03:L24`
**Status:** `READY_FOR_STAGE_D` **for Edulytics lesson authoring and accessible 2021 original mathematical tasks only**. This is **not** an executed xUnit/CI pass, Stage D acceptance, or verbatim protected publisher Section D checkpoint verification.

## A. Authorized read-only original edition and exact student tasks

Read original `C:\Users\khali\Downloads\New folder (2)\Common Core\01_Grade_1_IM_K5\unit-3\lesson-24\lesson.html` on device **Our-CS**. No code was edited or run on Our-CS. The previous source-register 2021 HTML SHA-256 is `7727fab9724ea22ea3c6632acfada0efc1e50d371ce75de2d581cfc2fda81c6e` (**previously registered; not rehashed on this pass**). Original lesson heading: **Relate Counting to Addition and Subtraction** (without invented “PLC Activity” in the student title).

- Warm-up: Choral Count **backward by one** beginning at **50**, stopping at **20**; teacher discusses patterns.
- Activity 1: Original double-ten-frame representations contrast **Diego's taking away** and **Tyler's counting on**. Learners compare both methods for **16−3=13**, **17−8=9**, **18−15=3**; teacher synthesis explicitly shows `18−15=3` and `15+3=18`. In particular, limiting the subtractor to 0–9 would incorrectly exclude the source 18−15 problem.
- Activity 2: Exact student-facing **Set A**, preserving the actual blank positions and order:
  1. `12−7=□` (5)
  2. `18−5=□` (13)
  3. `□=14−6` (8)
  4. `15−12=□` (3)
  5. `13−4=□` (9)
- Exact student-facing **Set B**, preserving different unknown positions:
  1. `5+□=18` (13)
  2. `13=□+6` (7)
  3. `20=15+□` (5)
- The last source heading is **Cool-down: Unit 3, Section D Checkpoint (0 minutes)**. Original material at the authenticated link is not exposed in the cached public 2021 HTML. **Do not assert protected checkpoint text was inspected**, invent a copied five-minute assessment, or infer drawing performance from digital numeric grading. The owner previously required independently authored Edulytics content, not copying protected publisher assessments.

Original activity 1 images are described at the **mathematical representation** level, without copying source artwork. Full figure/artwork reproduction and final visual rights/accessibility checks remain Stage D/E boundary, not academic proof inferred here.

## B. Learner-facing first-party content and migration

Existing source-informed rich lesson v6 was actually inspected rather than reauthored. `src/Edulytics.Data/Seeding/CommonCoreG1FullGradeStartingContentCorrections.cs` changes:
- Correct the title to **Relate Counting to Addition and Subtraction** and remove misleading assertion that the source lesson is literally titled “PLC Activity”.
- Preserve counting back **50→20**, complete **Diego/Tyler 16−3, 17−8, 18−15**, and all **eight** original Set A/B equation blanks, with source-specific note that `15−12` and `20=15+□` require two-digit subtractors/addends and blank positions must be retained.
- Keep classroom comparison, pupil drawings and teacher-led Section D checkpoint distinct from what Edulytics grades digitally.
- Bump only this lesson's persisted authored content from `us-ccss-g1-u03-l24-original-activities-v6` to `...-v7`; `CanUpgradeExisting` allows the guarded predecessor without globally rewriting other lessons. Assertion updated in `UsCcssG1FullGradeStartingTeachingTests.cs`.

## C. Generator, solver, persisted answer verifier and regression tests

Reviewed `src/Edulytics.Services/Mathematics/UsCcssG1FullGradeStrategyAndTensQuestions.cs`, `CountOnOrAway` numeric family `usccss.g1.u03.l24.strategy.count_on_or_away`:
- **Confirmed defect fixed:** the shared `CountOnOrAway or FlexibleSubtract` validation limited `b<=9`. `CountOnOrAway` now accepts a nonnegative grade-appropriate difference with `a=11..20`, `1<=b<a`, including `15−12`, `18−15` and `20−15`; neighboring **L25 FlexibleSubtract retains its original b<=9 restriction** until its own independent A–C audit.
- Modes `4`, `5`, `6` are specifically guarded:
  - `mode=4`: exactly **five** original Set A operand pairs, including the `□=14−6` orientation.
  - `mode=5`: exactly **three** Set B pairs, with `5+□=18`, `13=□+6`, `20=15+□` orientationally correct; all answers checked as `UNKNOWN=...;EQUATION=...`.
  - `mode=6`: exactly the original **16−3, 17−8, 18−15** double-method comparison; numeric difference and additive check are scored, and teacher reviews actual alternative-method reasoning.
- Earlier source-goal-adapted modes `2`, `3` retain their old compatibility but are **not falsely labelled** as the exact original student Set A/B.
- Original Set B response `5+13=18` must not be conflated with `13=7+6` or `20=15+5`; each has a separately exact, independently verified expected string.
- Wrong unknowns, malformed extra fields, incorrect operand substitutions and mode/operand spoofing are refused by existing persisted exact-answer contract; xUnit source-specific positive/negative oracles added in `UsCcssG1FullGradeStrategyAndTensPracticeTests.cs`. Seeded generation oracles include each of the 5+3+3 exact tasks.
- Review is **static source/math review**. This environment did not execute .NET, authenticate a child lesson or submit persisted attempts; no test pass is claimed.

## Decision / defer

**A–C content authoring review complete** based on the fully accessible original lesson student/teacher tasks, corrections to first-party content, and independently reviewed source-exact maths and generated variants. The **0-minute publisher-authenticated checkpoint is not claimed verbatim verified**, and teacher-observed drawing/discussion cannot be digitally certified. Edulytics uses its independent practice and teaching to measure the established goals; the owner's prior non-copying decision applies.

**`READY_FOR_STAGE_D`; academic ledger `IN_PROGRESS`; grade release `NO_GO`.** CI, .NET tests, authenticated lesson visuals, actual grading/persistence and final rights checks remain pending until whole-grade freeze. Next A–C authoring lesson, in strict order: **ordinal 62, U03 L25**.
