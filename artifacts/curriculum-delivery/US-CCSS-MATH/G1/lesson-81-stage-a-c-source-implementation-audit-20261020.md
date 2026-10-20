# Grade 1 lesson 81 (U04 L16) — original-source A–C continuation, 2026-10-20

**Classification: `A_C_EXPLICIT_GAP`, not `READY_FOR_STAGE_D`.** No full CI, Stage D, academic approval, merge or deployment performed.

## Stage A — original teacher and student task source

Verified the existing source checkpoint against exact read-only 2021 Our-CS paths: full teacher `unit-4/lesson-16/lesson.html` SHA-256 `9b253f98a275d7d5bfc748ae8b09bf99a984df736851e4099cb3e2e8ba6387d4`, teacher preparation `Exact_Sources/im.kendallhunt.com_k5_teachers_grade-1_unit-4_lesson-16_preparation.html` SHA-256 `be018f07997b639f971c5b68899d5694a72cbc94f170fe5a5e3c595af4c145f0`, and source student HTML `Student_Public_Mirror/unit-4/lesson-16/lesson.html` SHA-256 `80e2fabe1b92b8ed634546b8d9da43d7d2ff650da4cdec6bc812649d683fb459`.

The original 2021 full teacher source **Student Facing** tasks were independently inspected. Warm-up notices that `49>45` and `45<49` express the same comparison. Activity 1 introduces the physical **Greatest of Them All** two-digit game: pupil draws digit cards 0–9, places each selected digit permanently in tens or ones position, compares completed two-digit numbers; winning/strategic play remains teacher/partner assessed. Activity 2 has five **fixed symbol blanks**: `56 _ 26`, `72 _ 78`, `6 _ 55`, `92 _ 29`, `23 _ 23`; the corresponding symbols are >, <, <, >, =. It ALSO contains **five number-box prompts**: `□>78`, `39<□`, `13=□`, `□<□`, `□>□`. Except equality, these have multiple correct answers. The original publisher **Make Comparison Statements** five-minute Cool-down is protected; verbatim access is not claimed. Original game sheets, partner drawings, classroom outcomes and assessment images are not redistributed.

## Stage B — first-party content boundary repair

Original `CommonCoreG1FullGradeFinalUnit4Corrections.cs` teaching already encoded place-value language, reversibility, five fixed signs, three partly open expressions, and teacher-only physical card placements. Static review found the last two **both-blank source tasks** were missing and the source one-blank tasks' genuine multiple valid answers were not explicitly stated. Commit `4853c21b3707c2aa1cbe37b84084002bc9111652` updates **lesson 81 only** with all five source boxes, exact answer ranges, the equality special case and teacher-only caveat. Version `us-ccss-g1-u04-l16-original-student-activities-v3` safely upgrades its existing original-comparisons v2 without changing lesson 82 or 83 content versions. `UsCcssG1FullGradeFinalUnit4TeachingTests.cs` regression asserts all five boxes and old-v2 upgrade (authored commit `989c19a0896c89082a3c365d0a409b3b4d0de59b`); tests unexecuted.

## Stage C — strict original answers without fake singleton answers

Previously `UsCcssG1FullGradeUnit4FinalQuestions` `write_comparison` offered only random comparisons with three generic two-digit operands. It **could not represent the original fixed 6 vs 55 task** and **could not automatically verify source number-box openness**.

Commit `207d7a0c6e2f0c7ced419ac560ef42cae2e78458` adds modes 4–8 for all five source symbols with exact source operands; mode 6 intentionally permits a source 6 despite the ordinary two-digit generic bounds. Modes 9–13 cover the five source number-box tasks. First-party bounded open tasks accept any matching `NUMBER=n` for `□>78` (79–99), any `NUMBER=n` for `39<□` (40–99), only 13 for `13=□`; for `□<□` and `□>□`, `LEFT=n;RIGHT=n` accepts any **two-digit** ordered pair with correct inequality. `Solve` returns one reproducible exemplar; `Verify` independently validates the full set of valid responses rather than comparing against that one exemplar. `Valid` remains exact to source mode parameters and leaves other Unit 4 lesson families unchanged.

Independent source-aligned xUnit correctness, wrong/mutated operand, false relationship, out-of-range, multiple-valid-answer and 3,600-seed generator assertions were **authored**, not run, in `UsCcssG1FullGradeUnit4FinalPracticeTests.cs` (commit `fcee38c3091446dc4acb92fb6b381b4c0807522e`). Previously persisted modes 0–3 and answer contracts were not modified. Expanding seeded mode selection requires subsequent exact-source frozen-candidate regression review.

## Remaining gates

The five-minute protected original 2021 publisher Cool-down is inaccessible and no lesson-81-specific owner-approved independent assessment replacement was established. A SHA match is not visual/source PDF parity. Final Stage D must verify mathematical assertions in .NET, actual signed-in learner/teacher rendering, database-persisted grading, source images/rights and seeded generator changes before academic acceptance. Keep `A_C_EXPLICIT_GAP`, `IN_PROGRESS`, grade `NO_GO`; do not invent copyright, classroom or testing evidence.
