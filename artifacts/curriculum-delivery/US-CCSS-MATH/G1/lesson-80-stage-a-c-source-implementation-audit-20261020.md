# Grade 1 lesson 80 (U04 L15) — source mathematics / Stage A–C review, 2026-10-20

**Latest status: `READY_FOR_STAGE_D` for first-party source-aligned A–C authoring; earlier gap notes are superseded by the boundary resolution below.** Original publisher Cool-down remains protected and **not copied or certified**. Tests authored, **not executed**; Stage D, CI and academic acceptance deferred.

## Stage A — verified canonical 2021 original

The following authorized **read-only** Our-CS SHA-256 values exactly match the existing 2021 Grade 1 source checkpoint: `unit-4/lesson-15/lesson.html` = `03b7fb51dd40741eb9ea181b2441fcf4954451e7e1eb6921ba3f11b45ca088ac`; `Exact_Sources/im.kendallhunt.com_k5_teachers_grade-1_unit-4_lesson-15_preparation.html` = `2f860ae1cd7c3dd963a5ecc39594caaf4c49c25ad67fb9bddcade11a67c1f7c5`; `Student_Public_Mirror/unit-4/lesson-15/lesson.html` = `0ae74d9fe789b1e4539fa545d20716cc369c914894bca5b117521219004d4322`.

Original full teacher page and embedded student-facing lesson tasks were separately read, not inferred from code alone:
- Number Talk: `35+10=45`, `52+10=62`, `52−10=42`, `83−10=73`. The focus is the unchanged ones digit.
- Activity 1: circle the true choice in **nine comparison pairs**. The true source assertions are `17<27`, `34<36`, `25<52`, `54>24`, `29>21`, `85>58`, `45<54`, `74<78`, `21>12`. Original launch `78>45` and `45<78`, teacher discusses `21/12` and `74/78`; oral place-value explanations remain teacher-evaluated.
- Activity 2: evaluate five literal statements: `17<47` **true**; `58=53` **false** (correct `58>53`); `45>63` **false** (correct `45<63`); `39<93` **true**; `4=46` **false** (correct `4<46`). Students explain and optionally correct false statements.
- Teacher synthesis compares `43/48` and `85/65`, spoken and written symbols. The original **True Comparisons** publisher Cool-down is **five minutes, protected**, and its wording was not obtained or copied.

## Stage B — independent first-party teaching comparison

Reviewed `CommonCoreG1FullGradeStartingContentCorrections.cs` U04 L15. It already explicitly teaches <, > and =, lists the nine exact source comparison results, five true/false source statements and their meaning, original warm-up and value-based symbol explanation. It explicitly keeps drawings and oral explanations teacher-assessed. The shared earlier `us-ccss-g1-u04-l06-l15-original-student-activities-v6` was retained; **no unnecessary source teaching re-authoring**. Actual authenticated learner effective body proof is a later Stage D obligation.

## Stage C — existing first-party source exact verification, targeted missing task repair

Statically reviewed `src/Edulytics.Services/Mathematics/UsCcssG1FullGradeTwoDigitOperationsQuestions.cs`. Existing `CompareSymbols` modes 0–3 grade bounded symbols between ordinary two-digit numbers; mode 4 uses original `17/27`; mode 5 original `34/36`; mode 6 original `17/47`. That was insufficient to grade the full original five literal truth statements, especially `4=46` (first operand is single-digit).

- Commit `0d625632ace34b0aefff4a0f0c0677b7f79f9f88` adds **fixed-source modes 7–11**, one per exact original truth assertion. For each, the generated prompt asks whether it is true and, if false, requires a mathematically correct rewrite. Strict normalized answer contracts are, in source order: `TRUE=YES;CORRECT=17<47`, `TRUE=NO;CORRECT=58>53`, `TRUE=NO;CORRECT=45<63`, `TRUE=YES;CORRECT=39<93`, `TRUE=NO;CORRECT=4<46`. Mode 11 explicitly allows the source single-digit 4 **without widening generic two-digit bounds**.
- Added fixed mode 12 for the `21/12` source pair, exact `SYMBOL=>`, preserving existing modes 0–6 and their persisted answer contracts.
- `Valid` requires fixed source operands for modes 7–12. Negative verification rejects false/or wrong answers, extra fields, mutated operands and unsupported mode families. Prompt readability was clarified in commit `e9418c7259a2a18a5782b96521cdc811aba58d5e`; no source answers are placed in the prompts.
- New independent mathematical correctness/wrong-answer/parameter-mutant/seed-coverage assertions authored in `UsCcssG1FullGradeTwoDigitOperationsPracticeTests.cs`, commit `3f9d72b340553b4aa6e0f127cd64986cac32a400`. **These tests have not been executed**. The expanded mode distribution changes future fixed-seed question samples, to be reconciled at frozen-candidate Stage D.

This grades only the source's numeric statement truth and correct inequality, **not** authentic explanations, original illustrations, real pupil drawings or protected publisher assessment.

## Remaining authoring blocker / deferred gates

The original 2021 five-minute publisher **True Comparisons** Cool-down is inaccessible. No lesson-80-specific owner authorization for an independently authored replacement was verified; neither a fabricated original page nor blanket approval is acceptable. Additional learner/visual parity validation and real seeded/authenticated tests are deferred to the frozen candidate; no academic acceptance or release is claimed. **Record `A_C_EXPLICIT_GAP` until the protected assessment boundary is independently resolved.**

## A–C source-aligned authoring closure — independent Edulytics formative (2026-10-20)

**Stage A:** Original IM 2021 accessible teacher Student Facing: 35+10,52+10,52−10,83−10 mental warm-up; nine true comparison-pair decisions; all five original true/false comparison claims, tens-first explanations and teacher synthesis. The original five-minute publisher **True Comparisons Cool-down is protected and not readable**, so Edulytics makes **no claim** to reproduce, grade, or have verified that protected question. Physical drawings/oral justifications still require teacher evaluation.

**Stage B:** Existing source-correct learner explanation, nine accessible comparisons and five literal source truth statements reused; no unrelated teaching change.

**Stage C:** Preserved exact source comparison word/symbol cases and correct/wrong grading contracts. Implemented a separate **Edulytics-authored** formative exit, **68 < 86**, requiring `SYMBOL=<;TENS_LEFT=6;TENS_RIGHT=8` so pupils must identify the magnitude of each tens digit. Guarded mode **13** in the existing `CompareSymbols` family; prior answer contracts for 0–12 are unchanged. Code: `0103ea0341da86d16b55e024acf6310bc3652aeb`. Exact, wrong, partial and altered-parameter regression assertions written (not executed) in `tests/Edulytics.Tests/MathematicsIntelligence/UsCcssG1Lesson80IndependentComparisonExitTests.cs`, commit `5440f7344a0a5c019daec5a71bde0e03a90bfbda`.

**Academic boundary:** The first-party formative exit is an Edulytics-owned independent check of the *same documented mathematical skill*, not a fake copy of the protected publisher Cool-down. Unchanged V1 accepts first-party authored educational work preserving goals without requiring unauthorized source reproduction or an invented separate replacement-approval ceremony. These two outputs must remain distinguishable. Authenticated learner presentation, grader persistence, seeded compatibility, visuals/rights and final-SHA exact acceptance remain **Stage D** tasks. They have not been tested, passed, signed off or released.

**Authoring-only result:** `READY_FOR_STAGE_D`, full-grade `NO_GO`. 
