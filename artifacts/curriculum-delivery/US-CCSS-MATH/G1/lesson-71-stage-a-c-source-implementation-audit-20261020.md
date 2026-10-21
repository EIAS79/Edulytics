# G1 ordinal 71 — U04 L06 Count Larger Collections — original 2021 tasks, independently authored teaching and math A–C review

**Authoring disposition:** `READY_FOR_STAGE_D` after reviewing the accessible original mathematical tasks and improving first-party lesson-specific assessment. This **does not** represent actual executed xUnit/.NET tests, verified teacher/student participation, source-publisher Cool-down reproduction, CI, or final grade acceptance.

## Stage A — inspected original 2021 student and teacher materials, READ-ONLY

Our-CS cached original: `C:\Users\khali\Downloads\New folder (2)\Common Core\01_Grade_1_IM_K5\unit-4\lesson-6\lesson.html`. Previously registered original 2021 full teacher HTML SHA-256 `8210b93ef8034620be513eb55889b1f509b267f285ed999e9606423c7849dbe3` (not rehashed during this pass). © Illustrative Mathematics 2021 source and illustrations retained in the read-only collection; no copyrighted figures copied into Edulytics.

Original mathematics and classroom sequence:
- Warm-up **Choral Count: Count to 81**, count **by 1 starting at 50**, stop at **81** and discuss changes around 59→60 and 69→70.
- Activity 1 **Count Another Collection** (15 min): physically count exactly **52 loose connecting cubes**. Students can make **five full groups of ten plus two ones** and explain how they organize and represent the count (`5×10+2=52`). Count objects only once; partner drawings and real group activity require live teacher review.
- Activity 2 **Noah Counts a Collection** (10 min): an illustration shows **four complete tens and a final incomplete group of eight**. Noah claims it contains **50** cubes; actual total **40+8=48**. A successful response is **DISAGREE** and a correct mathematical explanation that **eight does not make a complete ten**. The numeric total alone is not full evidence of the original story comprehension, so Edulytics separately scores the decision and math; a drawing/explanation is teacher-reviewed.
- Activity 3 **Centers: Choice Time** (15 min): source Check It Off centers and classroom partner choices. In synthesis, teacher asks for ways to check off **30**, e.g. `20+10=30` or `90−60=30`; student participation not digitally inferred.
- Lesson synthesis: **three towers of ten and seven single cubes** represent **37**, said aloud as *thirty-seven*.
- Last source title: **Unit 4 Section B Checkpoint (0 minutes)**; detailed publisher assessment requires login and is NOT copied, verified verbatim or an A–C representation claim.

## Stage B — first-party teaching

Existing `CommonCoreG1FullGradeStartingContentCorrections.cs` already mentioned the original 52, Noah 48 and 37 source examples. Reused and strengthened it with a distinct **independently worded Edulytics reasoning check** requiring **DECISION=DISAGREE;TENS=4;ONES=8;TOTAL=48;CLAIM=50**, the source **52** collection and **37** closing example in teachable progression. Learner instructions explicitly separate counted cubes from full groups of ten and require human observation of sorting, drawings, reasoning, reading aloud and center game.

Avoided changing the shared `UnitFourLaterSourceContentVersion` for U04 L07–L15. Only **L06** gets `UnitFourCountLargerSourceContentVersion=us-ccss-g1-u04-l06-original-student-activities-v7` and a guarded upgrade from L06's original common `...-v6` through `CanUpgradeExisting`. Preserves first-party content and previously accepted original 50/new-six lessons.

## Stage C — generated source-goal problem, scoring and tests

Updated `src/Edulytics.Services/Mathematics/UsCcssG1FullGradeTensAndOnesQuestions.cs` **only** for the existing L06 `MixedCollection` family `usccss.g1.u04.l06.collection.tens_ones`:
- mode `4`: fixed source 52 cubes → `TENS=5;ONES=2;TOTAL=52`.
- mode `5`: fixed source Noah's **50** claim with **48** actual objects → `DECISION=DISAGREE;TENS=4;ONES=8;TOTAL=48;CLAIM=50`. Verifier must reject **AGREE**, `TENS=5`, `TOTAL=50`, missing decision, duplicate fields or malformed values.
- mode `6`: original 3 tens+7 ones → `TENS=3;ONES=7;TOTAL=37`.
- Modes `0..3` remain regular Edulytics mixed-collection practice. `Valid` forbids modes 4–6 in unrelated L07–L10 families and forbids changing any fixed source-specific operands. Existing response parser matches all required keys and values without inventing teacher observation.
- Added focused `Lesson71NoahIncompleteTowerIsNotAFullTen` to `UsCcssG1FullGradeTensAndOnesPracticeTests.cs` with exact true/false mathematical assertions and isolation check. Added first-party explanation/version guard expectations in `UsCcssG1FullGradeStartingTeachingTests.cs`. **All tests authored, not executed.** Stage D must run final frozen-SHA compilation, xUnit, authenticated student display, actual saved scoring/persistence, and rights/accessibility checks.

## Closure decision and limits

**A–C authoring CLOSED, `READY_FOR_STAGE_D`** for the accessible original mathematical source requirements and independently authored Edulytics digital subset. The original **publisher-protected 0-minute Section B checkpoint** is not claimed available or reproduced. Original classroom sorting, partner explanation, drawing, physical Check It Off and reading aloud are **teacher-only**, not certified by the numeric result. The source source-figure pixel parity is not claimed or needed for Edulytics first-party assessments.

**Academic ledger: `IN_PROGRESS`; grade release: `NO_GO`.** Do not run CI, .NET, authenticated D or merge during authoring. Next canonical A–C ordinal: **72**, U04 L07.
