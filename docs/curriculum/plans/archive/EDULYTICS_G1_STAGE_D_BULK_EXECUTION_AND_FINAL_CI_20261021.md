# EDULYTICS — Grade 1 Stage D Bulk Execution and Final CI
**Authorization:** User authorized saving this plan and beginning Stage D / CI on 2026-10-21. **Not authorized:** automatic merge to `main`, Render deployment, production database changes, bypassing security/release controls.


## Owner publication policy amendment — 2026-10-21

The owner explicitly removed **academic certification, teacher approval, school approval, and external institutional signoff as prerequisites for publication of an Edulytics-authored lesson**. The publication standard is internal, reproducible source-goal alignment and technical evidence: correct lesson/SkillContract routing; first-party learner content; deterministic/independent mathematical positive, wrong and malformed grading when digitally gradeable; an actual authenticated learner page and persisted test attempts against a disposable database; accessible rendering, source/licensing compliance and green applicable CI on the candidate SHA. No teacher/learner account participation or actual classroom activity is required to sign off release readiness.

A physical, oral, partner, free-drawing or learner-authored classroom activity remains **instructional content**, not a gate on publication. For these activities, verify that accurate learner-facing instructions and an appropriate first-party task/rubric render; do **not** pretend an actual child authored or completed the activity, that a teacher observed it, or that a numeric proxy assesses the original open-ended task. Rubric metadata may be preserved as instructional guidance but **cannot block publication for lack of a teacher signature, observed pupil work, or academic committee certification**.

This does **not** waive mathematical correctness, honest source/copyright provenance, security, authentic CI evidence, or the requirement for a separate owner merge/deployment decision. The 96-lesson full-grade technical gate stays fail closed until its real required technical and source evidence passes. Previous human/academic waiting labels in Stage D records are historical under the superseded policy; report technical blockers separately from optional classroom facilitation. Historical 51–56 V1 internal acceptance must retain its original traceability.

## Authoritative state and recovery
- Repository: `EIAS79/Edulytics`; continue working branch: `curriculum/g1-continuous-authoring-final-ci-20261019`.
- Last independently verified authoring handoff commit when plan drafted: `4a80de954cd802ddc032d0482b46341b73a46785`. Read the **latest live GitHub HEAD** on every resumption; this SHA is not a pinned instruction to revert newer valid commits.
- Scope: `US-CCSS-MATH` Grade 1 ordinal lessons **51–146** inclusive (96 lessons). Already historically released lessons 1–50 must not be re-authored.
- Previous A–C handoff: `artifacts/curriculum-delivery/US-CCSS-MATH/G1/lessons-076-146-stage-a-c-authoring-handoff-20261021.md`.
- Existing grade progress: `artifacts/curriculum-delivery/US-CCSS-MATH/G1/full-grade-progress.json`; may lag lesson checkpoint evidence, showing `IN_PROGRESS` for lessons after 56.
- Individual evidence: `artifacts/curriculum-delivery/US-CCSS-MATH/G1/lesson-{N}-v2-stage-a-d-checkpoint.json`. Some 51–56 checkpoint files are historical only; consult separate V1 records and grade progress.
- Lessons **51–56** have prior `INTERNALLY_ACCEPTED` original V1 A–D evidence, but their candidate changes are **not** deployed to Render. Retain historical evidence while validating current affected code.
- Lessons **57–146** record `READY_FOR_STAGE_D` as A–C authoring closure. This is **not** compile, test, CI, authenticated learner, academic or production approval. New tests were written but not executed at the authoring head.
- Grade release remains `NO_GO` until fully evidenced and specifically authorized for release.

## Mandatory operating style
Actively implement and test. **Do not replan, restart A–C, or pause for per-lesson reports/permission.** Read real GitHub evidence; do not trust chat summaries over the repository. Record incremental work in the repository and make commits with exact scope; new conversations must be able to resume from the ledger without repeating completed valid work. Communicate succinctly in Arabic only on significant transitions/blockers and give one comprehensive final report, not 96 reports.

**Strict sequential lesson closure:** 51, 52, 53, ... 146. **Parallel test execution is allowed**, but the ledger's ordered academic closure is not random. A missing or invalid lesson result cannot be marked passed because another lesson passed.

## Three validation batches — no separate 32-lesson release approval
- D1: 51–82 (32)
- D2: 83–114 (32)
- D3: 115–146 (32)

Use batch boundaries to shard tests and localize failures. Existing `.github/workflows/us-ccss-full-grade-gate.yml` contract is **ONE_WHOLE_GRADE / 96**; keep it fail-closed. A 32-lesson shard passing does not certify the whole grade.

## Stage D execution
1. Read branch HEAD; A–C handoff, existing checks and workflows, and Stage D execution ledger. Resume the earliest not-current-head-verified ordinal. Check for pending workflows to avoid redundant CI triggers. Pin an exact candidate SHA for every test result.
2. Execute **real** .NET restore/build; targeted source-math, generation, grading, correct/wrong/malformed/boundary/multiple-valid-answer tests; shared-engine full regression; original student-task negative controls. Execute safe parallel shards as possible; do not duplicate costly full builds without cause.
3. Run integration and authenticated HTTPS E2E against **disposable** database and test schools/roles: learner rendering, right family, generated questions, multiple valid answers, wrong/malformed answers, saved grades and attempts, isolation, accessibility and visual checks, fail-closed on missing evidence. Keep machine-readable per-lesson proof indexed by ordinal/family, with exact tested SHA and URLs.
4. On a failure, identify whether the cause is lesson content, shared engine, test assumption or environment. Fix specifically on the working branch. Rerun affected tests and relevant regression; preserve unaffected valid evidence when code identity/dependency analysis warrants it. After code changes, re-establish validity at final exact candidate SHA.
5. Inspect the existing CI configuration first. Prefer GitHub Actions for official Stage D / CI, use independent jobs and shared restore/build artifacts where safe. Local .NET tests may accelerate diagnosis but **never substitute for required GitHub CI evidence**.
6. Perform a final complete eligible GitHub Actions CI / full-grade gate for lessons 51–146 at the final candidate SHA (and required PR merge ref when applicable); verify results and run IDs. A green **negative-control contract** or records schema test alone is **not** a successful academic validation.
7. Close each lesson sequentially with detailed per-lesson status and evidence, distinguishing `D_PENDING`, `D_RUNNING`, `D_BLOCKED`, `D_TESTS_PASSED`, `D_VERIFIED`, `APPROVAL_PENDING` and `APPROVED`. Teacher-reviewed physical construction, peer discussion, original publisher art rights or human-authored tasks are **not** automatically certified by digital/math tests. Mark only claims that available evidence proves.

## Durable Stage D progress (GitHub)
Create/resume a JSON ledger under `artifacts/curriculum-delivery/US-CCSS-MATH/G1/`, e.g. `stage-d-bulk-execution-ledger.json`. Include schema, actual branch HEAD and tested SHA, batch, highest contiguous per-lesson verified ordinal, next unverified ordinal, 96 ordinal-indexed rows, each row status, existing A–C evidence, current-head technical evidence, source/figure/human limits, failed tests, GitHub Actions run URLs, dependencies and blockers, next action, candidate release status. Commit useful checkpoints after meaningful changes, not a separate human-readable report for every lesson. Treat ledger-only commits distinctly from tested **code SHA** and identify whether a new code commit invalidated tests. Avoid excessive CI triggers from ledger-only changes.

## Our-CS desktop computer
May read Illustrative Mathematics / Kendall Hunt 2021 sources and student/teacher books locally (**READ ONLY**), and may run .NET tests on an **isolated synchronized clone/worktree**. GitHub remains the authoritative code history and execution ledger. **Never overwrite or reset an old local clone or the original source tree**. Check for dirty local edits; do not discard or commit unrelated work. Do not claim source figures/licences or genuine teacher/learner participation based on numeric stand-ins. Avoid touching production/test databases except isolated disposable fixtures.

## GitHub Actions starting points
Review current workflow definitions under `.github/workflows/`, notably `us-ccss-full-grade-gate.yml`, `us-ccss-authenticated-acceptance.yml`, `v2-phase11-12-acceptance.yml`, `evaluation-production.yml`, `us-ccss-academic-certification-evidence.yml`. Evaluate workflow triggers/path filters, required checks, concurrency, reproducible SHA, actions quotas and artifacts. Don't disable checks merely to get green CI. Final success requires actual applicable suites green and correct technical + academic evidence at the final candidate.

## Safety and delivery
- Never force-push, silently rewrite others' files, merge to `main`, deploy to Render, or touch production database without separate authorization.
- Don't record `APPROVED` or change `NO_GO` solely due to green digital CI. Record genuine outstanding human/rights/real-student tasks as pending.
- Write incremental technical evidence and resume from latest GitHub ledger. If blocked, log exact blocker and keep working on independent cases.
- Return **one comprehensive final summary** of ordinals 51–146: latest candidate SHA, actual test commands/results, applicable successful/failed Actions with links, per-lesson D coverage and status counts, unverified/human-dependent tasks, open blockers and merge/deployment decision. No fabricated times or success.

**Execution instruction:** Begin immediately with current GitHub, create/resume ledger and Stage D; do not substitute a narrative audit for code/test execution.
