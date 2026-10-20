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

## Direct read-only teacher/student source recheck (2021 local HTML, 2026-10-20)
Read `accessim_teacher/unit-4/section-b/lesson-12.html` and matching `accessim_student` file on Our-CS **without modifying the device**. The teacher source independently confirms the 10-minute warm-up, 20-minute Write Numbers Stage 2 (partners count by tens forward or backward from either endpoint, writing 1–3 numbers per turn), 20-minute paired ±10 equations, and the concluding comparisons 67↔77 and 82↔62. The teacher synthesis specifically contrasts ten more/less with twenty more/less; the latter was missing from the first-party explanation and was restored in code commit `c73dd088d7eea60a8eeca40879e1bf3538b230f1`. The student HTML has math-rendered/embedded equation content not reliably recovered by a plain-text tag strip; therefore full equation-by-equation parity cannot be certified from this extraction alone. The protected original publisher assessment is still unavailable. The source evidence supports partial Stage A/B verification, **not** full A–C closure. Additional negative and exact sequence tests were authored in `5f1ada1374db2afc9fcdb1282839b76bf761f7d8`, unexecuted.

## Continuation audit — cached MathJax equation extraction and graded intermediate steps

Read-only inspection on Our-CS recovered numeric equation structures directly from MathJax `data-latex` attributes in the currently cached `accessim_student/unit-4/section-b/lesson-12.html` (rather than relying on plain-text HTML stripping). The page contains the four paired ±10 number tasks, progressive +10 steps starting at 67, progressive −10 steps starting at 99, and three mixed/two-step tens operations starting at 45. The warm-up is also present, but has NOT been independently reconciled against the generator and visual student view. The physical Write Numbers Stage 2 and mathematical explanations remain teacher-assessed, not digitally certified.

**Source byte-integrity blocker discovered:** On this independent read, SHA-256 of the *currently accessible local cache* differs from the pre-existing `sourceBasis` hashes in `lesson-77-v2-stage-a-d-checkpoint.json`:
- Teacher checkpoint: `9aeaacd09e4d34c6d7cd9138b3e40c33b79d310bedb0d7abde2360a184ad93c7`; observed current cache: `3ed6c38a906c3e986ceaa8f0c98224bde0aec784daf976ae56ce3fb23e02a93a`.
- Student checkpoint: `bc498182c4cdb1c2221310f0abf40469780a0c0f09542b4a4a52b4f59c962b96`; observed current cache: `afdafd80cec2df36be4fbbe0924e77de9834e4186f578dbbb1b483c11749e773` (remove the display-space before using as hex).

These hashes describe *different byte snapshots*, not proof of a different mathematical syllabus: dynamic website HTML/build metadata may change. However source-snapshot identity must be reconciled before exact-source parity or academic approval can be claimed. No cached publisher file was edited.

**First-party implementation only:** Newly added `TenMoreLess` modes 9 and 10 require all three intermediate numeric results, respectively `STEP1=77;STEP2=87;STEP3=97` and `STEP1=89;STEP2=79;STEP3=69`. Existing modes 0–8 and their persisted answer contracts are left unchanged. Implementation commit `c04427eb23487ae7cd10a8a7a007e7d3a74c8d59`; positive, negative, missing-step and mutated-parameter xUnit tests authored in `df72a44ff85edadded2a63f57a6eb70f5168643c`, NOT executed. Changing the generator's random mode range may alter fixed-seed future samples; this requires version/seed regression review before a production cutover.

**Open gates:** independently resolve source snapshot hashes/edition, reconcile all warm-up/math/center/visual/teacher tasks and actual learner-facing UI, review verifier semantics and seeded generation compatibility, run the tests only in the agreed frozen-candidate Stage D, and review a first-party formative exit without treating it as the protected publisher Cool-down. `A_C_EXPLICIT_GAP` and `NO_GO` remain binding. This is not academic acceptance or production verification.

## Independent continuation — canonical-source correction and learner repair (2026-10-20)

**Supersedes the earlier unresolved byte mismatch, but NOT the overall A–C NO-GO.** The exact registered canonical source files were rehashed read-only on Our-CS, all matching the pre-existing checkpoint: full original 2021 teacher `unit-4/lesson-12/lesson.html` = `9aeaacd09e4d34c6d7cd9138b3e40c33b79d310bedb0d7abde2360a184ad93c7`; 2021 teacher preparation `Exact_Sources/im.kendallhunt.com_k5_teachers_grade-1_unit-4_lesson-12_preparation.html` = `f079b91b320cbbdaed28a10e1f5cf41205488205ce56f8966767d3822965c382`; canonical Student_Public_Mirror `unit-4/lesson-12/lesson.html` = `bc498182c4cdb1c2221310f0abf40469780a0c0f09542b4a4a52b4f59c962b96`. The earlier reported `3ed6...` / `afdaf...` differences do **not** represent these canonical path bytes and must not be used as evidence of an edition mismatch. A matching SHA is source-file integrity only, not original student-view or licensed artwork proof.

Read-only teacher HTML independently reconfirmed the original mental Number Talk **3+10=13, 10+5=15, 13−10=3, 15−10=5**, the four paired ±10 numbers **67,39,52,75**, the **67→77→87→97** and **99→89→79→69** source sequences, the mixed **45→65,25,45** calculations, and the Stage 2 Write Numbers center's actual **forward/backward count by ten; write one, two or three consecutive tens steps per turn**. The teacher synthesis compares 67/77 and 82/62. Its 0-minute Section B Checkpoint/Cool-down is protected; no original protected assessment wording was obtained or reproduced.

**Stage B authoring correction committed** as `852e77d8a4a5ee01b13cb38cab399e65c354bc71`: learner-visible first-party text now explicitly teaches the four source warm-up answers and each intermediate step of the 67 and 99 sequences instead of only aggregate totals, while retaining all paired tasks, 45 mixed sequences, oral explanation limits and teacher-supervised physical center. Lesson-specific content version `us-ccss-g1-u04-l12-original-student-activities-v7` upgrades from the existing U04 later-content v6 without revving lessons 78–80. A targeted source-content regression was authored at `18116954ecbd28f63d82a49cd82ab28c2045681c`, **not executed**.

**Stage C static review** cross-checked the existing `TenMoreLess` mode guards and exact answer contracts against all fixed source numeric tasks, including each `STEP1/STEP2/STEP3`, wrong/missing-step rejection, source-pair numeric modes 5–8, source sequence mode 3, and independent mode 4. Existing xUnit assertions were inspected as source, not executed. Legacy modes 0–8 were not edited. Modes 9/10 changed the seeded draw distribution; this must be checked at the later frozen-candidate verification stage. Neither computed grades nor oral teacher evaluation nor original gameboard artwork has been authenticated.

**Remaining A–C blockers:** independent full original student-facing/visual center-gameboard parity is not yet established (the canonical Student_Public_Mirror SHA matches, but its large page/script snapshot is not proof of visible task parity); the original protected Section B Checkpoint is unavailable and no specific owner decision to replace it has been established. The 2021 source, first-party text, code and test assertions are distinct proof types. Thus **`A_C_EXPLICIT_GAP`, not `READY_FOR_STAGE_D`** remains accurate. No CI, Stage D, lesson acceptance, merge or deployment was triggered.
