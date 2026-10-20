# G1 ordinal 66 — Unit 4 Lesson 1 “Count Large Collections” — source / first-party A–C audit (2026-10-20)

**Authoring decision:** `READY_FOR_STAGE_D` for accessible 2021 edition math instruction and source-meaningful independently graded tens arithmetic, **not** a claim of original physical bag handling, completed teacher observation, final .NET test pass or academic sign-off.

## A. Original 2021 textbook source inspected READ ONLY

Authorized Our-CS source `C:\Users\khali\Downloads\New folder (2)\Common Core\01_Grade_1_IM_K5\unit-4\lesson-1\lesson.html`. Prior original source register lists the full teacher HTML SHA-256 `af5e0f582b63ea811e8c856108975f99de19dc694f31162d76342f5232a9be08`; this is **existing registry hash, not newly recomputed**. Original edition ©2021; original figures not copied.

- **Warm-up 10 minutes, Choral Count Count by 10:** Begin at **30**, count by tens **to 100**, then count **back down to 30**; discuss patterns in tens and ones digits. Do not imply the initial number is 0 or 10.
- **Activity 1, Count a Collection (10 minutes):** One partner group receives exactly **40 physical objects**; arrange in tens, groups and/or containers. Each child chooses a counting method and records count / representation. Four groups of ten give **40**, not four objects.
- **Activity 2, Count More Than One Collection (15 minutes):** Source labeled bag quantities **A=20, B=40, C=50, D=60, E=30, F=10**. Two students count, document individually and **exchange bags with another group**, count a second collection, explain how to avoid double-counting.
- **Activity 3, Centers: Choice Time (15 minutes):** teacher-supervised return to known addition/subtraction-within-20 centers; choosing a center or participating with a partner is not certified by arithmetic.
- Ending heading is **Unit 4 Section A Checkpoint (0 minutes)**. The teacher's linked publisher-only checkpoint question is not exposed by the public cached teacher HTML; no copyrighted protected assessment wording is claimed reproduced.

The first-party independent math assessment checks the source learning goal (count complete tens up to 60), not publisher's hidden checkpoint, in keeping with the owner's approval that Edulytics is its own course. Original paper drawings, physical bags, and class participation remain teacher-observed.

## B. Existing teaching repaired, not replaced

Source file: `src/Edulytics.Data/Seeding/CommonCoreG1FullGradeStartingContentCorrections.cs`. Reused the registered lesson-specific rich content, corrected the exact **30→100→30** warm-up, provided the complete A–F source bag-to-quantity map, addressed correct group-of-ten representation and bag trade, and kept real center work teacher-supervised. Mathematical original source examples remain accessible, while protected artwork and original questions are not duplicated.

**Version guard:** Only `UnitFourCollectionSourceContentVersion` advanced from `us-ccss-g1-u04-l01-original-activities-v6` to `...-v7`, with `CanUpgradeExisting` permitting that exact source-identity-bound v6 predecessor. Other lessons are not falsely upgraded.

## C. Original math Practice and independent review

Source-registered `UsCcssG1FullGradeStrategyAndTensQuestions.CountTens`, family `usccss.g1.u04.l01.count.groups_of_ten`. Existing generator and verifier were inspected; **no inaccurate generator modification was needed**:

- `mode=0` demands `a=4,b=c=0`, representing the original first activity of exactly **40** objects; expected `GROUPS=4;TOTAL=40`.
- `mode=1` uses `a∈{1,2,3,4,5,6}` and source bag map `1→F,2→A,3→E,4→B,5→C,6→D`; totals are `10a`, i.e. **F=10, A=20, E=30, B=40, C=50, D=60**.
- Modes 2/3 offer original-goal-inspired representations as complete ten-frames or towers (clearly distinct from a physical exchange). No claims that picking the right digital number proves physical grouping or social interaction.
- The family validates `a=1..6`, `b=c=0`, and permitted modes; the existing persisted answer verifier demands both independent `GROUPS` and `TOTAL` values correctly with no extra fabricated fields.

New regression `Lesson66Original2021FortyObjectsAndAllSixTensBagsHaveExactMathContracts` in `tests/Edulytics.Tests/MathematicsIntelligence/UsCcssG1FullGradeStrategyAndTensPracticeTests.cs` statically checks all six original bag labels and totals, 40 collection, invalid zero/seven groups, manipulated parameters, wrong-unit answers, extra answer fields and generated family bounds. Learner content assertions in `UsCcssG1FullGradeStartingTeachingTests.cs` check exact source warm-up and A–F map, and v6→v7 guarded upgrade.

**Only source/code mathematical review and tests AUTHORED**: .NET xUnit, CI, authentic learner pages, persisted student submissions and teacher review were **not** run.

## Closure / handoff

**A–C content teaching and numeric math audit complete; `READY_FOR_STAGE_D`.** Protected original 0-minute checkpoint remains **unverified**, but Edulytics does not copy that protected assessment; the accessible mathematical goal is covered by its own tasks. Real bags, recorded pupil representations, partner bag exchanges, chosen centers, rights, accessibility and original Stage D verification remain for final freeze. The academic ledger remains `IN_PROGRESS`, grade release `NO_GO`. Continue strictly at ordinal **67 / U04 L02**, leaving accepted/published predecessors intact. CI remains deferred.
