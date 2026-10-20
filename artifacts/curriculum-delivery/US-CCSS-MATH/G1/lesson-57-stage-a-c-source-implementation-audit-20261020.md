# G1 ordinal 57 / U03 L20 — original-2021 A–C audit (in-progress, not D acceptance)

Lesson: **A Trip to the Zoo** `PED:US-CCSS-MATH:G1:U03:L20`. This is an individual lesson source/teaching/Practice review, not a cohort, CI, D-stage, or academic-acceptance assertion.

## A. Original source and identified scope

- Official 2021 teacher preparation: https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-20/preparation.html
- Official 2021 complete public lesson: https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-20/lesson.html
- Original teacher lesson HTML SHA-256 recorded in the repository source register: `b52d02ff8f09926ec8e838c59bc9b924949226cfc7f2be8c0de05909a2338011`. This is the **previously recorded** cached-source checksum, not a claim to have rehashed the remote source during this change.
- Standards: `1.OA.A.2`, `1.OA.C.6`. Goal: three-addend story problems within 20 using different valid strategies. Original activity 1 specifically requires strategies that do not have a ten-pair without decomposition.
- Warm-up: `8+2+4=14`, `8+6=14`, `7+3+6=16`, `7+9=16`.
- Reptile task: eight snakes, seven iguanas, five **frogs**, total **20**. Do not substitute the turtles of the separate later v.360 edition. Source problem allows drawings, numbers, or words and teacher-led strategy comparison.
- Bird task: three herons, six hawks, seven hummingbirds, total **16**.
- Large-cat task: eight lions, four tigers, three cheetahs, total **15**.
- Petting-zoo task: eight goats, seven sheep, four pigs, total **19**.
- Original 2021 student section summary: `4+8+6=10+8=18` and `4+8=4+6+2=10+2=12`, accompanied by double-ten-frame counter figures. These **two summary models** were absent from the source-grounded learner examples before the current repair and are restored as first-party accessible text.
- Original lesson shows reptile and petting-zoo artwork. The source page provides an explicit third-party photograph attribution (MabelAmber/Pixabay); no artwork is copied into Edulytics. The photograph and reptile image have not been inspected at source-pixel fidelity in this pass. The original teacher page also contains a login-only five-minute primates cool-down; its actual task is not available in the publicly reviewed lesson. **Do not assert full original source task/figure audit completion until these are independently resolved.**

## B. Actual teaching code review and repair

- Original source-specific Rich V2 content in `src/Edulytics.Data/Seeding/CommonCoreG1FullGradeStartingContentCorrections.cs` already represented the four animal group totals and the mental warm-up; it was reused.
- Added missing two-addend and three-addend **student section summary**, with accessible independent ten-frame descriptions for 18 and 12 instead of copying publisher diagrams.
- Updated only `ZooSourceContentVersion` from `us-ccss-g1-u03-l20-zoo-2021-source-v6` to `...-v7`; preserved a guarded upgrade from persisted lesson-57 v6 so the corrected teaching does not silently remain stale in existing installations.
- The content materializer and the `CanUpgradeExisting` hook are covered by targeted source-assertion and scoped-version-upgrade tests in `tests/Edulytics.Tests/MathematicsIntelligence/UsCcssG1FullGradeStartingTeachingTests.cs`.
- Authenticated first-party learner rendering at frozen final commit SHA is a later Stage D check and **has not been claimed**.

## C. Existing Practice verification and limits

- Existing first-party digital family `usccss.g1.u03.l20.zoo.three_addends` in `src/Edulytics.Services/Mathematics/UsCcssG1FullGradeNextQuestions.cs` is preserved. Its four original-source cases are mode 2 (reptiles), mode 3 (birds), mode 0 (large cats), and mode 1 (petting zoo). Other cases are generic three-group addition, always with total at most 20.
- `VerifySourceGroupEquation` independently checks **the exact multiset of all three source group values** and the total. It permits order-equivalent regrouping, but not a fabricated equation with the same total, an omitted third group, or a malformed/duplicate key.
- Added an independent xUnit test `Lesson57OriginalFourZooStoriesRejectWrongGroupsAndMalformedResponses` in `tests/Edulytics.Tests/MathematicsIntelligence/UsCcssG1FullGradeNextPracticeTests.cs`, with correct answers, commuted source-group order, dropped groups, false operands, wrong totals, and malformed strings for **each** of the four source tasks.
- Original pencil-and-paper drawings, personal explanations, partner discussions, photographs, physical counters and teacher-observed activity are NOT automatically graded or claimed as observed.
- The new tests are **written but not run**: authoring-phase GitHub CI and final-SHA .NET/DB/authenticated checks are deliberately deferred under the owner-approved protocol.

## Outstanding before affirmatively recording READY_FOR_STAGE_D

1. Inspect or resolve source image/representation fidelity and licensing bounds, explicitly deciding which source figures are decorative vs essential student task representations. A first-party textual alternative is present but does not prove publisher illustration visual parity.
2. Resolve the access-restricted original primates cool-down's actual learner task, or document an authorized source-specific reason it is outside the learner target, without inventing a question.
3. Recheck the actual repaired source and Practice evidence at lesson level, remove the identified A–C gaps, and **only then** set all affirmative `aToCClosure` fields and `openACDefects: []`.

Current decision: **NOT READY_FOR_STAGE_D**; original teaching repair and source-group Practice tests committed but A–C original-source audit remains open. No CI dispatched, no Stage D, no internal academic acceptance, no merge, no release.
