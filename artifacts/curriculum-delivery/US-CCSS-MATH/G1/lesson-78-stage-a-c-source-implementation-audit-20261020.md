# Grade 1 lesson 78 (U04 L13) — independent source/teaching/Practice Stage A–C review (2026-10-20)

**Authoring classification: `READY_FOR_STAGE_D`.** This is evidence-backed authoring readiness **only**. GitHub CI, .NET execution, authenticated student rendering, true persisted grading, center-game classroom observation, figure rights and final academic acceptance are **NOT** claimed. The authoritative grade ledger remains `IN_PROGRESS` / `NO_GO`.

## Stage A — verified original 2021 student and teacher tasks

Read-only Our-CS original 2021 teacher `unit-4/lesson-13/lesson.html` SHA-256 `ed9502bae50434a90c6d02fc65174df640774f14a71aab3ce932f2650a954ac4`, exact 2021 preparation `Exact_Sources/im.kendallhunt.com_k5_teachers_grade-1_unit-4_lesson-13_preparation.html` SHA-256 `863dc31852ab4bff585e28839806c52eab34a4ee0705a831aec4293830bc7708`, and canonical `Student_Public_Mirror/unit-4/lesson-13/lesson.html` SHA-256 `2a1eaca8bf9cde20a32da210b3a773f2561e3d5ab752795300785819cf9160dd` **all match the pre-existing checkpoint**. Original publisher files are left untouched.

The full teacher HTML's embedded **Student Facing** content, teacher narrative and classroom synthesis were inspected (not inferred from a source-goal summary):
- Warm-up Number Talk within 20: `10+4=14`, `9+4=13`, `9+6=15`, `15−6=9`; teacher elicits mental make-ten strategy.
- Activity 1 (tens/ones center choice): **Grab and Count Stage 2**, **Five in a Row Stages 1–4**, **Check It Off Stages 1–3**. Partner choice and switch are physical classroom tasks. Original teacher math example uses `70` and `20` to check off **50 or 90**, subtract/add.
- Activity 2 (within-20 center choice): **How Close? Stages 1–2**, **Number Puzzles Stages 1–2**, **Shake and Spill Stages 3–5**. Original teacher math example selects three distinct cards from **8, 4, 1, 7, 5**; `8+7+5=20` achieves the target. Learner-facing center menu is choice, not a digitally gradeable single board outcome.
- Lesson synthesis requests pupil reflection on partner collaboration; teacher assesses authentic classroom interaction. The inspected 2021 teacher lesson contains **no separate Cool-down** for U04 L13, so no protected assessment was copied or claimed.

Original publisher center thumbnails/gameboards were **not** copied or passed off as licensed first-party visuals. The source's visible activity choices and numeric teacher examples are represented in first-party Edulytics teaching; original board performance and partner participation remain teacher-supervised.

## Stage B — first-party learner teaching parity

Reviewed `src/Edulytics.Data/Seeding/CommonCoreG1FullGradeStartingContentCorrections.cs` U04 L13 repair. Its prior numeric examples and activity scope already covered mental warm-up, tens/ones and within-20 strategies, including Check It Off `70±20` and How Close `8+7+5`. Gap: the six **actual named center choices and stages** were missing from source-grounded learner instruction. Fixed in commit `5f1285a27fb52dba4b0bad44601ea00c5a9eed52`; now explicit original first-party activity menus and named physical-vs-digital boundary. Created **lesson-specific** content version `us-ccss-g1-u04-l13-original-student-activities-v7`, migrated only from the existing U04 L06–L15 later-content v6. Historical Grade 1 1–50 untouched; other U04 lesson versions retained.

Teaching source regression assertions added in `tests/Edulytics.Tests/MathematicsIntelligence/UsCcssG1FullGradeStartingTeachingTests.cs` (commit `54776c82be98e05f24ba5cc6e15a063785067ce2`). These assertions cover all six center menus, original within-20 warm-up, and predecessor migration. Authenticated learner UI inspection remains Stage D.

## Stage C — scoped digital mathematics and teacher-only separation

Independently statically inspected `src/Edulytics.Services/Mathematics/UsCcssG1FullGradeTwoDigitOperationsQuestions.cs` family `CenterReview`:
- Mode 4 fixes `a=70,b=20` and exactly grades `ADD=90;SUBTRACT=50`.
- Mode 5 fixes `a=8,b=7` and exactly grades `TOTAL=20;CARDS=8+7+5`; prompt requires three **distinct** source-hand cards, not duplicating a card.
- Mode 6 fixes `a=9,b=6` and exactly grades `ADD=15;SUBTRACT=9`, supporting the source's related fact.
- Modes 0–3 reuse within-20 addition/subtraction, two-digit plus full tens, and numeric tens/ones counting with parameter guards and bounded results.
- `Valid` rejects wrong modes, wrong fixed operands and unexpected parameters. `Solve`, `Verify`, and persisted answer comparison require the declared answer contract; malformed/extra fields and tampered operands are negative cases.
- Existing test method `Lesson78OriginalCenterNumericTasksAndDistinctCardChoiceAreVerified` authors fixed source oracles, deliberate negatives and a 1,400-seed generator sample, **not yet executed**. New teaching parity tests likewise **authored but not run** per continuous final-CI policy.

Neither automatic numeric grading nor a correct card sum certifies physical center activity, partner cooperation, pupil explanation, or original publisher board artwork. Those boundaries are teacher-observed; later Stage D visual/rights/accessibility and authenticated numeric grader verification are still mandatory.

## A–C conclusion and retained release gates

Original full teacher student-facing prompts and center rules independently matched; all known academic content and static generator/verifier defects were resolved/reused without inventing teacher observations. **`aToCClosure.outcome=READY_FOR_STAGE_D`**, with proof in this review and the targeted code/tests, and **zero open A–C defects**. `testsAuthoredButNotExecuted=true`, `stageDExecuted=false`. No CI/Stage D run, academic ledger approval, merge, deployment, owner UAT or publication took place. At final frozen candidate, execute original mandatory tests/authenticated runtime and review visual/rights evidence independently before any `INTERNALLY_ACCEPTED` designation.
