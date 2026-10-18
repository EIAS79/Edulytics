# Grade-first five-lesson execution SOP — implementation of unchanged V1

**Governing document:** [EDULYTICS_GRADE_FIRST_FIVE_LESSON_EXECUTION_PLAN_V1.md](EDULYTICS_GRADE_FIRST_FIVE_LESSON_EXECUTION_PLAN_V1.md), `EDULYTICS-GRADE-FIRST-5-V1`.

**Purpose:** Ensure every executor runs the *existing* V1 stages, evidence requirements, and one-grade/five-lesson release rhythm consistently. This is an **execution checklist**, **not** a different plan, additional gate, stricter edition policy or waiver. If this SOP conflicts with V1, **V1 wins**.

## 0. Resume from evidence, never from assumed completion

Read V1 in full; then fetch the current `main` SHA, next grade ordinal/manifest, past accepted batch/crosswalk, available local source index, the actual current Rich V2/effective lesson and Practice contract, and the current CI/deployment state. Compare past work to avoid duplication. Re-intake a grade only for the triggers defined in V1 §4 (edition/source, inventory, hash, mapping or rights change).

Select **no more than five consecutive** canonical lesson codes **in one grade/course**, preserving their source unit and lesson ordering. Last 1–4 lessons are allowed by V1 **at grade end only**; never silently skip blocked ordinals. Work on one five-lesson chunk at a time under existing protected GitHub/Render rules.

## Source and edition: compare substance, not age

Authorized older official sources may remain valid even when no publisher update exists. A newer source may use the same objective/task, a revised context, or different pedagogy. **Do not mark the old source invalid or the new source unusable solely because their version numbers/titles differ.** Make an actual per-activity comparison of:

- exact goal and target unknown; official standards, distinguishing preparation-wide `Addressing`/`Building Towards` from specific activities;
- publisher teacher tasks and Student Facing instructions, representations, accepted methods and answer form;
- newer/alternate source task against the original *where available*: `same task`, `same objective, revised task`, `partial overlap`, or `substantively different`; explain the observable content;
- licensing and graphic provenance; whether the student-book PDF itself was inspected (an HTML **Student Facing** section inside a teacher lesson is valid task evidence but not PDF artwork inspection).

An authorized older source with sufficient objective/activity evidence is usable under V1. If evidence **essential to that lesson** is genuinely absent, record its precise gap and follow existing V1 `BLOCKED_SOURCE`; do not invent an edition-based blocker.

## The repeatable V1 A–F runbook

| V1 stage | Run the same steps on **each** lesson | Evidence to retain |
|---|---|---|
| **A — Select and compare** | Load actual effective student-visible Rich V2, source objectives/tasks/standards and the current Practice family; classify only observed gaps. | source path + SHA256/edition, stable ID, title, lesson-specific goal, expected representation, existing/forbidden families, `READY_REUSE` or exact repair classification |
| **B — Prepare accurate content** | Reuse correct existing explanation/visuals first; repair inaccurate steps, worked examples, diagrams or misconceptions only if source comparison proves a mismatch. No blind publisher prose/artwork copy. | before/after defect and source reason; exact changed files; student-visible effect |
| **C — Connect Practice** | Reuse canonical skill/generator when correct; otherwise repair or add a *shared* math capability. Exercise allowable operation, unknown, domains, varied questions and correct **and** plausible wrong answers. | SkillContract, allowed/forbidden families, reference math, grading tests, visual/student input form |
| **D — Verify scoped change** | Validate source identity, lesson body as actually rendered, generation, exact/wrong/malformed grading, saved student attempts, affected UI/tenant/migrations as specified in V1 §5. Keep current repository-required CI. | per-lesson test names/results, real browser/auth where affected, issue/limitation record |
| **E — Complete five-lesson chunk** | Make an honest five-row decision with each lesson independently evidenced; do **not** translate compile success into academic approval. Preserve older ledgers and history. | grade ordinals/codes, source/content versions, tests, changed file/SHA, accepted/blocked counts |
| **F — Merge, publish, owner UAT** | Merge only under existing protections; Render `checksPass`, exact deployed commit, LIVE, HTTP ready and lesson inventory must be verified; hand over real production UAT to owner. | PR/merge SHA, Render deploy ID/status, HTTP proof and **owner UAT pending/passed/issues**, never assumed |

**No extra gate is introduced here.** This SOP is the *same V1 evidence executed consistently*, with source observations stated explicitly instead of compressed into a success claim.

## Source parity and reusable reporting

Use the [G1-005 locally verified review](../artifacts/curriculum-delivery/US-CCSS-MATH/G1/batches/US-CCSS-G1-005-LOCAL-SOURCE-CROSSWALK.json) as a **level-of-detail example**, not a claim that all its subject matter or number of mathematical families generalizes. For every candidate lesson record keep analogous evidence for:

1. Preparation whole-lesson Addressing and Building Towards; activity-level Addressing and Building Towards, with scope differences separated from true omissions.
2. Actual teacher task, available Student Facing task, original source files/hashes, and any alternate edition's actual task **and** explicit degree of overlap; record `no independent student PDF inspection` where true.
3. Current learner body, misconceptions, worked solutions, allowed Practice and any extra required representation. Do not pretend synthetic data performed a classroom survey or partner work.
4. Tests and residual limitations, including source graphics and owner production UAT. Declare `READY_REUSE` for already-aligned items rather than rewriting to manufacture work.

Report exactly per V1 §10: `Prepared: x/5`, real internal evidence, classification, tests, SHA/PR, LIVE deployed SHA, owner-UAT state, actual blockers and next consecutive batch. Historic artefacts must not be overwritten to create new PASS.

## Non-regression

- Preserve `docs/curriculum/EDULYTICS_GRADE_FIRST_FIVE_LESSON_EXECUTION_PLAN_V1.md` byte-for-byte.
- No changes to five-lesson limit, source standard identity, required GitHub workflows, Render auto-deploy, production data, grading history or cross-tenant controls.
- If a comparison identifies a real mathematical bug, correct it in the smallest shared component and add a failing-before/passing-after test. Do not rework unrelated lessons.
- No automated test can independently certify real classroom discussion, teacher signoff or copyrighted graphic parity.
