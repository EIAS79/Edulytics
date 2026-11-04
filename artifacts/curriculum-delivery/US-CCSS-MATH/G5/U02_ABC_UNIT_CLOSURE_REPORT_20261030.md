# Edulytics — Grade 5 Unit 02 | A–B–C sequential completion (2026-10-30)

**Repository:** `EIAS79/Edulytics` · **PR:** [#596](https://github.com/EIAS79/Edulytics/pull/596) (Draft; unmerged)  
**Verified pre-report HEAD:** `eb1f2adcb3ed6ae0f6ddb62ba6389f527a3bf3cf`  
**Original:** IM K–5 First Edition (2021), from local `Our-CS` under `C:\Users\khali\Downloads\New folder (2)\Common Core\05_Grade_5_IM_K5\unit-2`; original teacher `lesson.html`, `preparation.html`, and Student Facing sections contained in teacher HTML. No internet book search or TinyFish.  
**Scope:** unit 02 only, academic authoring A–B–C. No Stage D/CI/runtime testing.

## A–B–C closure: 17/17 lessons, strictly L01 → L17

Every lesson has an individual GitHub closure commit, original-local-source references, a lesson-specific evidence JSON `u02-lXX-abc-sequential-closure-20261030.json`, a selected exact Practice **digital subskill**, teacher-led rubric boundary, and an authored (not executed) targeted test.

| Lesson | Original lesson | Source-specific digital Practice family | A–B–C authoring | Targeted tests |
|---|---|---|---|---|
| 01 | Share Sandwiches | `usccss.g5.u02.l01.source_fair_share_choice` | `READY_FOR_STAGE_D` | `WRITTEN_NOT_RUN` |
| 02 | Share More Sandwiches | `usccss.g5.u02.l02.unit_share_equivalence` | `READY_FOR_STAGE_D` | `WRITTEN_NOT_RUN` |
| 03 | Interpret Equations | `usccss.g5.u02.l03.fraction_division_direction` | `READY_FOR_STAGE_D` | `WRITTEN_NOT_RUN` |
| 04 | Division Situations | `usccss.g5.u02.l04.blueberry_division_context` | `READY_FOR_STAGE_D` | `WRITTEN_NOT_RUN` |
| 05 | Relate Division and Fractions | `usccss.g5.u02.l05.fraction_division_generalization` | `READY_FOR_STAGE_D` | `WRITTEN_NOT_RUN` |
| 06 | Relate Division and Multiplication | `usccss.g5.u02.l06.relay_division_unit_fraction` | `READY_FOR_STAGE_D` | `WRITTEN_NOT_RUN` |
| 07 | Divide to Multiply Unit Fractions | `usccss.g5.u02.l07.unit_fraction_race_distances` | `READY_FOR_STAGE_D` | `WRITTEN_NOT_RUN` |
| 08 | Divide to Multiply Non-unit Fractions | `usccss.g5.u02.l08.nonunit_fraction_of_whole` | `READY_FOR_STAGE_D` | `WRITTEN_NOT_RUN` |
| 09 | Relate Area to Multiplication | `usccss.g5.u02.l09.area_whole_by_unit_fraction` | `READY_FOR_STAGE_D` | `WRITTEN_NOT_RUN` |
| 10 | Fractional Side Lengths Less Than 1 | `usccss.g5.u02.l10.area_whole_by_nonunit_fraction` | `READY_FOR_STAGE_D` | `WRITTEN_NOT_RUN` |
| 11 | Fractional Side Lengths Greater Than 1 | `usccss.g5.u02.l11.area_fraction_greater_than_one` | `READY_FOR_STAGE_D` | `WRITTEN_NOT_RUN` |
| 12 | Decompose Area | `usccss.g5.u02.l12.decomposed_mixed_rectangle_area` | `READY_FOR_STAGE_D` | `WRITTEN_NOT_RUN` |
| 13 | Area and Properties of Operations | `usccss.g5.u02.l13.distributive_area_expressions` | `READY_FOR_STAGE_D` | `WRITTEN_NOT_RUN` |
| 14 | Area Situations | `usccss.g5.u02.l14.fraction_multiplication_info_gap` | `READY_FOR_STAGE_D` | `WRITTEN_NOT_RUN` |
| 15 | Multiply More Fractions | `usccss.g5.u02.l15.multiply_mixed_source_number_bank` | `READY_FOR_STAGE_D` | `WRITTEN_NOT_RUN` |
| 16 | Estimate Products (optional) | `usccss.g5.u02.l16.estimate_mixed_area_products` | `READY_FOR_STAGE_D` | `WRITTEN_NOT_RUN` |
| 17 | Mosaic Pictures (optional) | `usccss.g5.u02.l17.mosaic_rectangle_area_cost` | `READY_FOR_STAGE_D` | `WRITTEN_NOT_RUN` |

## Source comparison and targeted repairs

The source-correct Rich V2 content already in Edulytics was **reused**, not rewritten from scratch. The new grade-owned extensions are one reusable question generation/independent verification dispatch (`UsCcssG5Unit2SourceQuestions.cs`), exact per-lesson Practice contracts (`UsCcssG5Unit2SourcePracticeContracts.cs`), registry metadata for 17 individually authorized question families, and the appended targeted source guard tests.

Each lesson's option questions use true source quantities and mathematical relationships; wrong/reversed fraction values and incorrect dimensional claims are explicit negative distractors. A question bank is only a **numeric multiple-choice subskill**—it does NOT replace the original source's fair-share diagrams, partner discourse, drawing/card sorting, number-line reasoning, information-gap questioning, gardens, optional estimation or mosaic construction.

### Learning scope covered

- **L01–L05:** equal sandwich shares, quotient-as-fraction, dancer-water reversal, six learners sharing four pounds of blueberries and the generalization `a÷b=a/b` for `b≠0`.
- **L06–L08:** relay distance and fraction-of-whole models, including 3÷2, one fourth of nine/seven miles, and non-unit-fraction multiplication.
- **L09–L13:** exact area of fractional-side rectangles, correct source dimensions and distributive decomposition of mixed-number gardens. Open WODB diagrams and blackline-master cards stay teacher-assessed; full original card picture inventories are not claimed verified.
- **L14–L15:** teacher-only Information Gap source protocol and selected public Student Facing multiplication facts; six-number use-once bank with rug, puzzle and whiteboard, and four equivalent 4×5 2/3 expressions.
- **L16 (optional):** original 36-square-foot garden estimates, distinguishing viable material budget from closeness to 36.
- **L17 (optional):** original collaborative mosaic model and $5/$3/$2-per-square-unit material comparisons, teacher review of construction and overlap.

## Testing, source and publication limitations

- **Written tests: `WRITTEN_NOT_RUN`.** This unit did not run .NET, Node, CI, automated academic tests, complete application/seed/authentication, or live learner/teacher grading.
- Stage C **contracts and source-targeted generator logic are authored but runtime-selected Practice, teacher-scored submission UI and actual grading are not verified**. Do not claim deployable lesson readiness from A–B–C statuses.
- Publisher-gated Cool-downs were **not** presented as inspected; existing independently authored Edulytiks source-skill-aligned exits are retained by the owner's direction. Publisher artwork, independent Student Task PDFs and unseen original card image binary/pixel parity are **not** certified/reproduced.
- Existing legacy unit source reports can describe earlier blockers; the authoritative current *A–B–C authoring cursor* is `g5-abc-lesson-ledger-v1.json`. Their historical observations should not be silently erased.
- No merge, deployment, or production GO.

## Unit checkpoint

- **U01:** 12/12 academic A–B–C authoring closures.
- **U02:** 17/17 academic A–B–C authoring closures.
- **Grade 5 total:** `29/148` recorded ready for later Stage D; `119` lessons remain in A–B–C authoring.
- **Last closed:** `PED:US-CCSS-MATH:G5:U02:L17`.
- **Next mandatory cursor:** `PED:US-CCSS-MATH:G5:U03:L01`.
- **Stage D:** not started; not until the separately defined full-grade A–B–C gate.

This is a unit **authoring** closure, not evidence that any unexecuted tests pass.
