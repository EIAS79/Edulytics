# Full-grade executor SOP — retain original V1 stage quality

**Governing plan:** [EDULYTICS_GRADE_FIRST_FULL_GRADE_EXECUTION_PLAN_V1.md](EDULYTICS_GRADE_FIRST_FULL_GRADE_EXECUTION_PLAN_V1.md) (`EDULYTICS-GRADE-FIRST-FULL-GRADE-V1`). Supersedes **only future delivery size** of the five-lesson V1 policy; never overwrite prior evidence.

## Before doing new lesson work

Read live `main`, current candidate, the ordered canonical source grade manifest and the separately versioned progress ledger. Preserve unpublished work in existing workspaces. **Do not use Our-CS for software worktrees, branches, program writes, tests or deploys**; Our-CS is restricted to read-only inspection of source textbooks and lesson references.

For G1, historical ordinals 1–50 are already published under the predecessor and remain untouched. The work cursor is **51**, not the manifest's stale `nextDeliveryChunk=G1-004`. Current scope is **exactly 96 new lessons 51–146**. The previous batch evidence is retained as immutable historical coverage, never rewritten to claim new tests.

## Stages A–F (unchanged in substance)

| Stage | Execution | Evidence |
|---|---|---|
| A Compare | Next unfinished lesson, source teacher/student goal, real effective Rich V2, current Practice; classify proven gaps | Original source link/edition/hashes, explicit lesson goal, permitted/forbidden families |
| B Teach | Reuse passing content; repair inaccurate example, methods, visuals, misconceptions only where proven | Source-specific before/after and lawful media |
| C Practice | Reuse skill/solver/verifier; add **reusable** missing families only if necessary | Actual generated valid and wrong/malformed submissions, numeric/answer bounds |
| D Verify | Check real current student content, source identity, Practice, real grading and persisted attempts, UI/tenant/security when affected | Tests, exact candidate/SHA, browser/DB artifact where needed |
| E Grade accept | Persist each new lesson; aggregate G1 96 new plus 50 historical refs; fail closed on ANY gap | Per-new-lesson source/content/practice evidence; 96/96 `INTERNALLY_ACCEPTED` |
| F Merge/release | All required protected CI + full-grade gate, then exact Render live SHA and authenticated production flow, owner UAT | PR/CI, deploy ID, live SHA, HTTP, owner UAT actual status |

A–D operate on individual **new** lessons to catch failures early. E–F apply to the **whole current grade**, not arbitrary five-lesson chunks. No hard minimum/maximum minutes per lesson; advance only after real correctness evidence.

## Release rules

- No new Grade 1 release while `NO_GO`; a successful "gate refused incomplete content" negative-control is NOT approval.
- Keep the original 1–50 old batch proofs untouched and do not rerun individual academic certification for them. Existing mandatory cross-project CI remains in force.
- Do not assert original PDF or proprietary illustration fidelity without inspection/rights; no synthetic classroom participation claim.
- Do not mark an unreviewed lesson `INTERNALLY_ACCEPTED`, create fake proof artifacts or use a code compilation PASS as a substitute for lesson academic checks.
- Reconcile all 146 grade codes on closure (50 historical, 96 new). Only after actual Grade 1 release/owner UAT advance to Grade 2; its own source inventory determines its lesson count.
