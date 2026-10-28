# Grade 2 — strict A→B→C sequential execution (user-approved clarification)

**Authority:** This supplements the Grade 2 A–B–C execution plan. It replaces the former gap-skipping exception for this run. The exact original lesson remains the instructional reference; Edulytics creates independently authored assessment and visual material.

## Non-negotiable sequence
1. Start from the first not-completed ordinal in the GitHub Grade 2 academic ledger. The current first unclosed ordinal is **1**.
2. For the SAME lesson, inspect the authentic 2021 IM teacher-facing and student-facing learning goals and tasks (Stage A), author/fix Rich V2 and representations (Stage B), then author EVERY assessable source skill with valid checking/rubric and write tests without running them (Stage C).
3. **Close THIS lesson before entering the next**: only after source alignment, lesson teaching, Practice, all independent assessments and representations are ready, set `aCStatus=A_C_COMPLETED`, `closure.status=READY_FOR_STAGE_D`, update checkpoint and manifest and provide GitHub commit proof. The `READY_FOR_STAGE_D` label is only an *authoring completion checkpoint*, not final acceptance or proof of test success.
4. Never use `A_C_EXPLICIT_GAP` as a completion state or justify moving to an unfinished higher ordinal. A genuine irreducible blocker remains `IN_PROGRESS_BLOCKED`, the cursor stays at that exact lesson, and no lesson is falsely closed.
5. Existing partial edits for ordinals 2–8 are retained on the branch and rechecked **only when their ordinal is reached**. A static scan of all 146 is not a substitute for individual A→B→C closure.
6. Do not run tests, .NET integration, CI or Stage D now; do not merge, publish, deploy, or mark a PR ready.

## Independent Edulytics Cool-down and visuals

- If the original publisher Cool-down is login-restricted, **author a new Edulytics Cool-down** targeting the independently verified mathematics goal and skill; do not invent the unpublished original question or pretend to reproduce it.
- Original numerically and pedagogically equivalent tasks, independently written examples and new visualizations ARE acceptable authoring solutions. Use the source as a curricular reference, not as copied text/image assets.
- Each Cool-down must have learner questions; correct answer checking or teacher rubric for open explanations; boundary/wrong-answer handling; accessible representations; linkage to the lesson and test files. A standalone unintegrated draft is not sufficient to mark Stage C fully complete.
- Multiple correct student-authored equations, drawings, visual reasoning, partner activities and classroom discussion require the appropriate input modality or teacher-scored rubric; a numerical score cannot falsely certify those parts.
- Source citations/attribution accurately explain educational alignment but are not permission to copy protected expression. General mathematics ideas, goals, operations, and techniques are not proprietary publisher artwork or prose.

## Source and status checks
- Course authority: `src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-grade2-im-k5-2021.lesson-blueprint.json`.
- Grade ledger: `artifacts/curriculum-delivery/US-CCSS-MATH/G2/full-grade-progress.json`.
- Grade manifest: `artifacts/curriculum-delivery/US-CCSS-MATH/G2/grade-manifest.json`.
- Set no `A_C_COMPLETED` or `READY_FOR_STAGE_D` without a verified code + documentation + checkpoint diff on the active GitHub branch.

**One report at the end of the requested scope, not a progress-only completion claim.**
