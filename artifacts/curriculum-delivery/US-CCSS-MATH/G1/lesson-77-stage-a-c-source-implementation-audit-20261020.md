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

## Follow-up implementation evidence — lesson 77 only

- First-party independent formative exit added as TenMoreLess mode 4: 46+10=56 and 46−10=36, with exact answer `PLUS=56;MINUS=36`. This is explicitly **not** the inaccessible original 2021 publisher Cool-down. Implementation commits `37128e5eccb85c46fc5614d039c692948bc088ba` and `459fafebe8d2422146cef142a6c11824687e8727`.
- Positive, wrong-answer, incomplete-answer, invalid-parameter, and generator-coverage tests authored in `9f039ec3168427217d63332d4778523d7315e6ee`; not executed.
- Teaching text in `CommonCoreG1FullGradeStartingContentCorrections.cs` already describes all four source 10-more/less pairs (67, 39, 52, 75), 67 and 99 repeated tens, 45 bidirectional sequences, and teacher-supervised Stage 2 Write Numbers. Independent exit asks for both +10 and −10, while oral reasoning and center gameplay remain teacher-only.
- Remaining A–C blockers: original accessible teacher/student visual and task-by-task parity has not been independently reconciled beyond indexed excerpts; independently reviewing all authored numeric mode and teaching behavior remains necessary. Consequently `A_C_EXPLICIT_GAP` is retained and lesson 78 must not be declared closed by inference.

## Additional source-pair parity implementation
The four original two-way tasks have now been encoded individually as TenMoreLess modes 5–8: 67±10, 39±10, 52±10 and 75±10. Each returns both numeric results in the strict `PLUS=...;MINUS=...` contract. Code commit `6a45d6960bb17e99ced9106968f04a9429f45ce8`, tests commit `6d9d57396d659930ce008412db55624f1aaef9b0`. Tests check correct results, incorrect subtraction results, wrong operands, and generator reachability; they remain unexecuted. The independent exit (mode 4) is separate from these original exercises.

The read-only original-task inventory records **two abbreviated student-facing excerpts**, no inspected original PDF artwork, and no independent original-task review (`recordStage=READ_ONLY_ORIGINAL_HTML_TASKS_INDEXED_INDEPENDENT_REVIEW_PENDING`). That inventory cannot establish full accessible visual parity. Consequently no A–C closure claim is made; source/visual reconciliation is the blocking step, not lack of numeric implementation.
