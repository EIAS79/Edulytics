# Grade 1 lesson 79 (U04 L14) — original source and code review, 2026-10-20

**Authoring status: `A_C_EXPLICIT_GAP` — NOT `READY_FOR_STAGE_D`.** No .NET/CI, authenticated grading, academic signoff or deployment claimed.

## Stage A — canonical 2021 source, read-only

The three canonical Our-CS files match the existing original IM/Kendall Hunt 2021 checkpoint:
- Full teacher `unit-4/lesson-14/lesson.html`: `d085cf009cac851f80ca7335a1b230ea2702c30a7b1764370ce8869531166b89`.
- Preparation `Exact_Sources/im.kendallhunt.com_k5_teachers_grade-1_unit-4_lesson-14_preparation.html`: `28c110215f8fc9764f408486415ade1af7f5c4d48a1022c00b52d62dfc3ca5e9`.
- Canonical student snapshot `Student_Public_Mirror/unit-4/lesson-14/lesson.html`: `714e294a8720b92a70e455ec27acdd609ca2105e0834150148b9f8144f63e115`.

Original full 2021 teacher/student-facing lesson was independently read: start at 3, count forward by 10 to 93 and reverse; compare **93 vs 26**, **62 vs 64** on their place values; Activity 1's spinner board is partner/drawing assessment not auto-graded; Activity 2 asks pupils to critique Elena's **75 > 65** justification and Noah's **39 > 41** mistaken ones-first reasoning. Activity 3 presents a choice of **Write Numbers, Grab and Count, Five in a Row**. Mai's original Grab and Count source prompt asks **4 towers of 10 + 3 ones plus 2 towers of 10 = 63**, and teacher synthesis compares **44/64** and **59/54**. The original Section C Checkpoint is publisher-protected (0 minutes), not independently accessed or reproduced. Source spinner artwork and partner performance have not been treated as digital scoring evidence.

## Stage B — verified learner-text correction

The previous first-party L14 lesson accurately explained tens-first comparison, Elena and Noah, and that formal symbols are in the next lesson, but did not explicitly describe original choral counting, the two teacher source comparison prompts, actual three choices or Mai's source card arithmetic. In `src/Edulytics.Data/Seeding/CommonCoreG1FullGradeStartingContentCorrections.cs`, commit `0b5c4a9e463a185642f3e9e4bda9896c6d5e1473` added these source-grounded first-party tasks, correct answers and teacher-only gameplay distinction. Lesson-specific content version `us-ccss-g1-u04-l14-original-student-activities-v7` includes allowed predecessor upgrade from the shared v6. Original L15 symbol teaching is unchanged. Source content/migration tests authored in `6640b2a71c8754e7a6dbd78f8f761a64328ca308`; not executed.

## Stage C — source word relations and guards

Existing `CompareWords` source-specific mode 4 correctly grades Elena (`RELATION=GREATER` at 75,65); mode 5 grades Noah's true relationship (`RELATION=LESS` at 39,41) without automatically grading his rationale. First-party repair `f2400d2ed194754b364a83fdb4701d2b8c9e929e` added exact, operand-guarded **mode 6: 93 vs 26 → `RELATION=GREATER`** and **mode 7: 62 vs 64 → `RELATION=LESS`**, leaving original modes 0–5 answer contracts intact. New positive, wrong relation, modified-operand and seed-coverage source regression assertions authored in `f1dec606fd541fa3524391f1059ebab9c17f2f32`; not executed. Changing random mode distribution affects future seed samples and remains a final regression concern.

Original partner spinner play, drawings, center turns and critique of another student's thinking cannot be certified by one relational word answer. Actual publisher spinner/gameboard figures were not copied. The original protected Section C assessment is **unverified**, and no lesson-79-specific approved substitute assessment was established.

## Open A–C items

1. Verify independent effective student-facing/visual spinner/center activity parity at the appropriate source-task boundary; source cache checksums alone cannot establish this.
2. Resolve protected Section C Checkpoint assessment boundary with verifiable evidence or explicit owner decision; do not invent publisher assessment content or consent.
3. Final-SHA Stage D must run the authored tests, seed/regression and authenticated student/teacher/runtime proofs before academic acceptance.

**Decision:** `A_C_EXPLICIT_GAP` and `IN_PROGRESS` / `NO_GO`. No Stage D or full GitHub Actions CI triggered; no merge or publication.
