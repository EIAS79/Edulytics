# Grade 1 ordinal 64 — U03 L27 Center Day 4 — original 2021 source / math / independently authored lesson A–C

**A–C authoring outcome:** `READY_FOR_STAGE_D` (**physical center play not digitally certified**). No final tests, internal acceptance, protected CI, live grading or publication performed.

## A. Original 2021 source, Our-CS read-only

Source: `C:\Users\khali\Downloads\New folder (2)\Common Core\01_Grade_1_IM_K5\unit-3\lesson-27\lesson.html`. Previously registered 2021 full teacher HTML SHA-256 `dafd276a86a3631edcd0a6a6c7872dee5e804d6ae7eacd9483f3d14bab1969d3`; **registry evidence reused, no hash recomputation this pass**. Teacher source activity text was independently inspected; source figures are not copied or represented as automatically accessible.

Actual student-facing warmup: `20−10=10`, `20−10−1=9`, `18−10−4=4`, `18−14=4`. Teacher asks why `20−10−1` also subtracts 11 from 20.

**Activity 1 — How Close? Subtract from 20 (15 minutes)**. A hand includes **four numeral cards** from the original center's 0–10 number-card range. Students pick **two or three** distinct cards to subtract in sequence from 20 and approach zero, write the equation, compare results with a partner, earn a point for a closer nonnegative result and discuss strategy. Teacher original synthesis hand is **6,4,9,8**; choosing **6,4,9** yields `20−6−4−9=1`, with the 8 left unused. Other source valid choices may be less close to zero. No claim to simulate physical four-card dealing or all the competition rules.

**Activity 2 — Compare, Add and Subtract Within 20 (25 minutes)**. Learners receive cards carrying addition or subtraction expressions, independently compute both, and compare which value is greater; classroom turn-taking, collecting cards and winner designation are not verified by a standalone numeric question. Exact 2021 synthesis example compares `9+6=15` with `19−6=13`, so the **addition card's value is greater**. The original 2021 source ends in a lesson synthesis about center strategies and **does not have a copied numeric Cool-down**.

## B. First-party instructional repairs

Preserved the already useful canonical rich Edulytics lesson in `src/Edulytics.Data/Seeding/CommonCoreG1FullGradeStartingContentCorrections.cs`, including the true original warmup. Added independently written learner/teacher examples of the actual source hand and comparison, clarified that zero is in the card deck, selected cards in a physical hand are distinct, and card-point totals and winning games require supervised participation. Teacher-reviewed representation and partner explanation are not passed automatically by computing an answer.

Source-identity-guarded lesson-specific authored translation version advanced from `us-ccss-g1-u03-l27-original-activities-v6` to `...-v7`; a persisted v6→v7 upgrade guard and tests are prepared only for this lesson.

## C. Generator, exact math and independent review

`UsCcssG1FullGradeStrategyAndTensQuestions.CenterWithin20`, family `usccss.g1.u03.l27.center.add_subtract`, was reused and repaired:
- `mode=0`: original two selected numeral cards 0–10 inclusive, distinct cards, `20−a−b` nonnegative and exact chosen equation.
- `mode=2`: original three cards 0–10 inclusive, **pairwise distinct**, sum at most 20, including valid **zero** card cases. Recorded equation includes all **three** terms.
- `mode=1`: within-20 addition-card arithmetic, allowing general valid values.
- `mode=3`: within-20 subtraction-card arithmetic, now admits original `19−6=13` and all legal `11..20` starting values / nonnegative differences (old generator restricted starts to 11..17 and the subtractor to `20−a`).
- `mode=4`: exact source comparison: `LEFT=15;RIGHT=13;GREATER=LEFT` for `9+6` versus `19−6`. Correct result only is insufficient: both expressions and comparison must be right.
- `mode=5`: exact 2021 four-card source synthesis hand **6,4,9,8**, chosen **6,4,9**; `RESULT=1;EQUATION=20-6-4-9=1`.
- All modes have scoped operand guards and persisted answer equality checks. No other lesson's family or prior accepted content was changed.

`Lesson64SourceCenterZeroCardsDistinctHandAndTwoExpressionComparisonAreCorrect` was authored in `UsCcssG1FullGradeStrategyAndTensPracticeTests.cs`. It covers exact source examples, valid zero cards, distinct cards, invalid repeat/negative totals, original 19−6, malformed or wrong comparisons, and deterministic generated modes. Source-specific teaching and v6→v7 migration tests are added in `UsCcssG1FullGradeStartingTeachingTests.cs`. **xUnit, .NET, actual learner grading/persistence have not been executed** and are not claimed PASS.

## Closure and limitations

**A–C academic content source/mathematics review closed.** Source instruction about partner gameplay, choosing centers, four-card dealing, competing to zero, winning and drawing explanation remains **teacher-observed**, not an Edulytics auto-scored attendance/performance claim. Rights/visual/authenticated accessibility evidence is deferred. The source has no inaccessible individual 5-minute Cool-down requirement for this lesson.

**`READY_FOR_STAGE_D`; source/progress academic ledger stays `IN_PROGRESS`; G1 release `NO_GO`.** Final exact-SHA CI, staged .NET tests, authenticated child-session experience and evidence only after whole grade freeze. Continue in canonical order to ordinal **65**, no CI during authoring.
