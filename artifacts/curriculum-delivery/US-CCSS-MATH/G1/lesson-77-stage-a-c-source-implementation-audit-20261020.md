# Grade 1 lesson 77 (U04 L12) — A–C incremental source/code audit, 2026-10-20

**Status: A_C_EXPLICIT_GAP — NOT READY_FOR_STAGE_D.** This record does not claim execution of tests, authenticated learner rendering, publisher assessment access, or academic acceptance.

## Stage A — evidence already recorded
The existing checkpoint records the 2021 teacher/student public tasks: mental warm-up 3+10, 10+5, 13−10, 15−10; ten-more/less equations including 67+10 and 67−10; repeated-ten sequences starting at 67 and 99; and the Stage 2 Write Numbers by 10 center. The existing first-party teaching also records 39±10, 52±10, 75±10, 45+10+10, 45−10−10 and 45+10−10. The protected original 2021 publisher Cool-down and any unreviewed visual assets must not be represented as verified.

## Stage B — teaching source review
The existing first-party lesson body in `CommonCoreG1FullGradeStartingContentCorrections.cs` describes changing tens while preserving ones, source equations, repeated-ten sequences and teacher-led Write Numbers gameplay. It explicitly does not claim that numeric grading proves physical center participation. No new learner-teaching content version or authenticated UI verification is claimed by this audit.

## Stage C — implemented, not runtime-tested
The `UsCcssG1FullGradeTwoDigitOperationsQuestions.TenMoreLess` family originally generated single +10/−10 numeric items. Source-aligned repeated-ten numeric modes were added:
- mode 2: 67+10+10+10 = 97, stored as a=67, b=30, expected `RESULT=97`.
- mode 3: 99−10−10−10 = 69, stored as a=99, b=30, expected `RESULT=69`.

The generator's `Valid` checks require exact operands for modes 2/3. Negative and positive test assertions were authored in `UsCcssG1FullGradeTwoDigitOperationsPracticeTests.cs`; **these tests have not been executed**. Numeric practice does not grade pupils' mental explanation or real Write Numbers center participation.

## Explicit A–C closure gaps
1. Independently reconcile every accessible original student/teacher task and visual prompt against the teaching and Practice/rubric evidence; existing checkpoint excerpts alone are insufficient for full parity.
2. Independently review generator/verifier and teaching parity for this lesson, including mode coverage, teacher-only center requirements, and answer-oracle negative cases.
3. Decide and document an owner-approved independent first-party Cool-down if the original protected 2021 publisher assessment remains inaccessible; never claim its verbatim verification.
4. Only after those reviews show zero open A–C defects, record `aToCClosure.outcome=READY_FOR_STAGE_D` with exact evidence. Stage D, final CI, authenticated persistence, visual/rights review and academic approval remain deferred.

**Next lesson to close: 77.** The grade academic-acceptance cursor `nextOrdinal:57` remains unchanged and is not the A–C authoring cursor.
