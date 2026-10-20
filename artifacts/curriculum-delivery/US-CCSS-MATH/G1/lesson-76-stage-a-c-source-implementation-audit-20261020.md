# Grade 1 lesson 76 (U04 L11) — A–C evidence, 2026-10-20

**Source:** Kendall Hunt 2021 teacher lesson, read-only `unit-4/lesson-11/lesson.html` on Our-CS; historical registered teacher HTML SHA-256 `142ba60b69b021a9f6122d5df8b3d160a71bf399fccc25c4bcabda7e827ec1a8` (not rehashed).

## Stage A — source teacher/student task review

Original mathematical warm-up evaluates 80+5=5+80 (true), 70+1=80+1 (false), and 20+6=6+30 (false). Original Activity 1's five sums are 37+20=57, 60+23=83, 48+50=98, blank=54+20 (74) and 30+65=95.

Original Activity 2 public image accessibility text actually reveals both maths tasks: 56+□0=96 (digit 4, so 56+40=96) and □3+30=74 (false for every hidden tens digit because 3 ones cannot equal 4 ones). The source HTML accessible image text was directly inspected; publisher image files were not copied. The protected 2021 five-minute "Add Tens" Cool-down remains unavailable and unverified.

## Stage B — Edulytics learner instruction

Reused and supplemented first-party teaching with accurate mathematical interpretation of both source-accessible images; removed the prior unsupported assertion that their maths were unreadable. Retained independent 46+30=76 formative assessment and original activity context. Lesson-specific persisted translation version updated from v7 to v8 with guarded prior v7 migration. Other lessons unaffected.

## Stage C — Edulytics numeric practice and verifier

Existing `UsCcssG1FullGradeTwoDigitOperationsQuestions.AddTens` family retains fixed source math modes 4–7 and first-party formative mode 8, plus independently designed alternative modes 9–10. Added source-aligned first-party **mode 11** for 56+□0=96, answer `DIGIT=4;EQUATION=56+40=96;TRUE=YES`; **mode 12** for □3+30=74 being impossible, answer `TRUE=NO;ONES_LEFT=3;ONES_RIGHT=4`. Exact mode and operand guards reject unauthorized parameters. Positive/negative numeric answer and seeded generation tests were authored in `UsCcssG1FullGradeTwoDigitOperationsPracticeTests.cs` and teaching migration assertions were amended in `UsCcssG1FullGradeStartingTeachingTests.cs`.

**Review limit:** source original publisher Cool-down not reproduced; numeric verification does not certify teacher-observed pupil drawings, actual persisted learner grading, source artwork licensing or child-classroom work. The new tests are authored but **not executed**.

**A–C authoring:** `READY_FOR_STAGE_D` on owner-approved independent formative basis. Stage D actual .NET/CI, authenticated persistence, visual/rights review and academic acceptance deferred until whole-grade freeze. Ledger `IN_PROGRESS`; grade release `NO_GO`. Next lesson ordinal **77**.
