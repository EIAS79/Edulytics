# G1 ordinal 69 — U04 L04 “More Addition and Subtraction with Tens” — A–C original 2021 source and independently authored academic math

**Status:** `READY_FOR_STAGE_D` content authoring; no CI, .NET result, authenticated child score persistence, teacher observation, original publisher-protected assessment parity or final acceptance claimed.

## A. Source 2021 inspected read-only on Our-CS

Source file `C:\Users\khali\Downloads\New folder (2)\Common Core\01_Grade_1_IM_K5\unit-4\lesson-4\lesson.html`. Prior source register original teacher HTML SHA-256 `b5484d49647426674fb667d3d9edad0bf562f0c5ff1f069afbef006604f02c1a` (reused registry fingerprint; **not freshly rehashed**).

- Actual publisher source title **More Addition and Subtraction with Tens**; previous Edulytics first-party label incorrectly appended “PLC Activity”.
- Source warm-up: `30+10=40`, `40+10=50`, `80−10=70`, `70−10=60`.
- Activity 1 **How Many Are in the Bag?**: Jada 30 cubes plus 2 towers of ten = **50**; Tyler 7 towers of ten minus 40 cubes = **30**. Both ask students to show how they know with drawings/numbers/words.
- Activity 2 **Add and Subtract Tens**: the public Student Facing text shows two diagram tasks titled **Add 20** and **Subtract 20** (source image contents not recreated from assumptions), plus six independently readable arithmetic tasks: `4+5 tens=9 tens=90`, `8−3 tens=5 tens=50`, `20+60=80`, `70−20=50`, `90−70=20`, `40+40=80`. Synthesis compares `4+3` and `40+30` to distinguish ones and tens.
- The original five-minute **Cool-down: Add or Subtract Tens** is behind publisher sign-in; its exact 2021 student question is **unverified and NOT copied**. Edulytics owner approved first-party formative assessment, not verbatim publisher duplication.

## B. First-party learner teaching + guarded correction

Reused the existing source-goal math lesson in `src/Edulytics.Data/Seeding/CommonCoreG1FullGradeStartingContentCorrections.cs`. Removed incorrect “PLC Activity” title suffix. Reinforced all six source problems, transparently identified two image-backed questions without inventing original source drawing values, and added an independently written Edulytics exit `50+30=80` and `80−30=50` showing inverse operations with groups of tens. Real student representations need teacher review.

Only `UnitFourMultipleTensSourceContentVersion` changes from `us-ccss-g1-u04-l04-original-activities-v6` to `...-v7`; matching source identity and old v6 version are required for persisted upgrade.

## C. Math engine / exact test review

Reviewed `UsCcssG1FullGradeStrategyAndTensQuestions.AddRemoveMultipleTens`, the registered family `usccss.g1.u04.l04.tens.add_remove_multiples`.

- Preserved source modes `0` (Jada 30+20=50) and `1` (Tyler 70−40=30).
- Preserved legitimate source-goal variation in modes 2/3, but distinguished these as Edulytics-adapted Practice rather than verbatim source student equations.
- Added `mode=4`: **exact** pairs `4+5,2+6,4+4` in tens. Added `mode=5`: **exact** pairs `8−3,7−2,9−7` in tens. These cover all six original readable Student Facing arithmetic tasks, including `90−70=20`. Strict mode/operand guards reject alternative forged source tasks.
- Added `mode=6` owner-approved original Edulytics inverse formative: `50+30=80` and `80−30=50`, answer `ADDITION=80;SUBTRACTION=50`. Exact-answer verification rejects missing second result, wrong tens unit and malformed extra fields; teacher reviews any student-created drawings.
- Authored `Lesson69ExactSixSourceTensEquationsAndOriginalEdulyticsInverseExit` source-specific math regression in `UsCcssG1FullGradeStrategyAndTensPracticeTests.cs`, and learner-title/content/version guard assertions in `UsCcssG1FullGradeStartingTeachingTests.cs`. **Tests written, not executed**.

## Handoff

**A–C reviewed as source-informed independently authored academic content with publisher original protected Cool-down flagged unverified.** Stage D actual tests/CI, authenticated learner render/grade persistence, teacher review and rights/accessibility are deferred until entire grade freeze. Current ledger `IN_PROGRESS`, release `NO_GO`. Next lesson **ordinal 70, U04 L05**, in sequence.
