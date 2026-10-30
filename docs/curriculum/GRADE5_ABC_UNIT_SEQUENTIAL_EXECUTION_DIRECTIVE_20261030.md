# Edulytics — Grade 5 — Mandatory A–B–C Execution Directive
Date: 2026-10-30
Repository: EIAS79/Edulytics
Course: US-CCSS-MATH:G5:IM-K5-2021
Scope: the current Grade 5 working branch / PR #596, subject to HEAD/checkpoint verification.

## Priority and source
This is an operational directive for Stage A–B–C, not evidence that any lesson has passed. Apply it with the source/rights/data-safety and acceptance gates of the full Grade 5 plan. The current blueprint baseline is **148 lessons / 8 units**; reconcile rather than assume 149.

Use Remote Desktop Commander, **not TinyFish**, to READ the authorized original sources at:
`C:\Users\khali\Downloads\New folder (2)\Common Core\05_Grade_5_IM_K5`
and the corresponding Family directory. Do not edit the original book archive. No internet book searches while the local edition is available; official references may resolve version/licensing conflicts.

## Non-negotiable lesson sequence
1. Read the actual branch, last HEAD and Grade 5 checkpoint. Locate the **last genuinely closed A–B–C lesson** with specific source evidence and commit. “Authored” alone does not count as closed.
2. Start at the first subsequent **unclosed** lesson, strictly by canonical unit/lesson order. Do not jump randomly or reconstruct already-correct lessons.
3. Open its existing Edulytics implementation and original local preparation, teacher/student lesson, activities, images and relevant tasks. Compare objective, concepts, examples, problems, diagrams, skills, and teacher activities 1:1. Preserve what is correct; repair only proved gaps.
4. Complete A → B → C as one continuous pass. Verify the affected math and artifacts with **non-executable focused checks**; document provenance and skill/answer/grading contracts. Write Stage D tests where needed, marked `WRITTEN_NOT_RUN`.
5. Save changed lesson artifacts, ledger, evidence, timing and checkpoint to GitHub, preferentially **one coherent closing commit per lesson**. Record SHA, then move directly to the next lesson. Never report an unclosed lesson as `READY_FOR_STAGE_D`.
6. Do not stop after every lesson to ask for more instructions. Finish the entire **current unit** sequentially, issue **one concise unit report**, then immediately continue to the next unit within the active authorized session; repeat until the final unit. A session is **not** restricted to one lesson. If a real external blocker prevents valid closure, document it and stop that sequence rather than silently skipping it.

## Performance policy
- Prefetch/cache source indexes and independent readings for several lessons **of the current unit** at once, with source fingerprints; do not redo unchanged, verified reads.
- Batch independent desktop/GitHub read calls and stage A–B–C/evidence/checkpoint writes. Avoid repeated GitHub operations and needless commits.
- Target **≤3 actual minutes per lesson** as a measurement goal, **never** as permission to skip essential source, mathematical or rights verification. Capture `StartedUtc`, `ClosedUtc`, `ElapsedSeconds`, `GithubCalls`, `ReusedCount`, `ChangedCount`, and exceptions.
- Apply **Reuse → Extend → New Engine**: reuse the present generator/grader/renderer where correct, extend only the missing family/rule, and create a new engine only if a genuinely different pedagogical mechanism cannot reasonably be extended. Respect shared-code single-writer ownership.
- During A–B–C, **do not run .NET/Node tests, E2E, full builds, CI, full synchronizations or repetitive checks**. Do not claim tests passed; actual executable verification belongs in Stage D. Record any workflow automatically triggered by the GitHub push without calling it Stage D approval.

## Gated original 2021 Cool-down
If the original 2021 Cool-down is teacher-login gated and unavailable, create an **original educational Cool-down** from the verified lesson goal, skill and content, with expected response and grading criteria. Mark it `AUTHOR_CREATED_COOLDOWN`; keep the inaccessible original as `SOURCE_UNAVAILABLE`. Do **not** claim this is a source-authentic matched publisher Cool-down, nor 100% source parity. Document any acceptance blocker honestly.

## Unit report and release boundaries
At the end of each finished unit report lesson range, genuinely closed count, open blockers, source exceptions, last HEAD/commit, timing and GitHub calls. Continue next unit without awaiting a new command when possible. Final report only after the full grade. Do not merge, deploy, modify production DB, or certify Stage D/CI/GO under this directive.