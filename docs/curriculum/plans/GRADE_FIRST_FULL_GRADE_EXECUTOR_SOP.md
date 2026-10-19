# Full-grade executor SOP — retain original V1 stage quality

**Governing plan:** [EDULYTICS_GRADE_FIRST_FULL_GRADE_EXECUTION_PLAN_V2.md](EDULYTICS_GRADE_FIRST_FULL_GRADE_EXECUTION_PLAN_V2.md) (`EDULYTICS-GRADE-FIRST-FULL-GRADE-V2`). Supersedes **only future delivery size** of the five-lesson V1 policy; never overwrite prior evidence.

## Before doing new lesson work

Read live `main`, current candidate, the ordered canonical source grade manifest and the separately versioned progress ledger. Preserve unpublished work in existing workspaces. **Do not use Our-CS for software worktrees, branches, program writes, tests or deploys**; Our-CS is restricted to read-only inspection of source textbooks and lesson references.

For G1, historical ordinals 1–50 are already published under the predecessor and remain untouched. The **academic-evidence cursor is 51** (since lessons 51–80 are not yet individually accepted); the **new implementation cursor is 81** (because 51–80 are already `IN_PROGRESS` and must be preserved). Neither cursor is the manifest's stale `nextDeliveryChunk=G1-004`. Current scope is **exactly 96 new lessons 51–146**. The previous batch evidence is retained as immutable historical coverage, never rewritten to claim new tests.

## Whole-grade execution, not five-by-five

Treat `node tools/curriculum/us-ccss-full-grade-worklist.cjs` as the **single Grade 1 scope** containing every ordinal **51–146 (96)**. It must never be partitioned into execution batches of five, ten, twenty, thirty, etc. `--remaining` lists every lesson needing academic evidence (currently 51–146), and **`--new` reports only records still marked unstarted in the ledger and does not authorize reauthoring candidates already present in code**. There is no size cap. Continue source-and-test work on 51–80; do not rewrite or discard their existing changes. The worklist does not manufacture course content or publish anything; it prevents scope truncation.

Work through the genuine per-lesson V1 A–D source/teaching/Practice/test checks on the whole grade. Save source-verified progress as necessary for resumability, not as five-lesson approvals. If a step is blocked, retain a truthful blocked state; do not bypass academic tests just to reach the end of the worklist. After all **96 genuinely** receive independent academic acceptance and required evidence, perform **one** final grade-level E/F gate, protected CI, exact Render SHA verification, and owner UAT. Historical 1–50 stay untouched.

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

## V1-equivalent evidence and approval rules

Carry forward the original V1 academic gate for **each new lesson 51–146**: original goal and student task, effective real teaching content, permitted Practice where gradeable, independent right/wrong mathematical tests, authenticated generation/grading/persisted attempts where applicable, honest residual teacher-only activity limits, test/commit linkage and all current security/deploy protections. No mandatory **external** academic approval, actual child submission, teacher-observed classroom evidence, or owner UAT before release. Grade 1 ordinal 65 requires a source-grounded authored-story learner task and teacher review rubric, instead of manufactured numeric grading. Ordinal 146 requires both the source-authored four-clue number riddle teaching/rubric and independently verified digital Practice. A rubric alone does not prove the actual learner page contains the authored assignment. Run the exact-head positive eligibility check when the grade is actually ready for final release review; on Draft/WIP PRs, run scoped correctness and negative control without treating intentional NO_GO as a software build failure. NEVER merge or release while the actual 96/96 gate is not green. Never touch the historic 1–50 lessons.

## Release rules

- No new Grade 1 release while `NO_GO`; a successful "gate refused incomplete content" negative-control is NOT approval.
- Keep the original 1–50 old batch proofs untouched and do not rerun individual academic certification for them. Existing mandatory cross-project CI remains in force.
- Do not assert original PDF or proprietary illustration fidelity without inspection/rights; no synthetic classroom participation claim.
- Do not mark an unreviewed lesson `INTERNALLY_ACCEPTED`, create fake proof artifacts or use a code compilation PASS as a substitute for lesson academic checks.
- Reconcile all 146 grade codes on closure (50 historical, 96 new). Only after actual Grade 1 release/owner UAT advance to Grade 2; its own source inventory determines its lesson count.

## Explicit original-V1 acceptance parity and evidence portability

This is a **single grade-wide E/F aggregate**, not an additional academic standard. The predecessor `EDULYTICS-GRADE-FIRST-5-V1` §5 A–D is controlling: original 2021 source/actual student task and lawful attribution; learner-visible source-grounded teaching; every required source-specific allowed Practice family, independent correct/wrong/malformed answer verification; scoped real authenticated student/teacher generation, score persistence, affected UI/tenant/security; honest teacher-only work limitations. Internal Edulytics acceptance is test/evidence based; **no outside academic signature, classroom child work, publisher figure pixel parity or pre-deployment owner UAT is required**. Unverified licensing or an actual missing original assessment remains a blocker.

**Evidence is defined by substance and identity, not by its storage suffix.** The grade gate continues to accept its existing SHA-256-checked local files. It also accepts a `GIT_FILE` proof object (`{kind,commitSha,path,sha256}`) for an already committed original-source review, grading audit or test; the gate independently verifies Git ancestry, file bytes/hash and lack of changes to that evidence file at the release SHA. Supply it as `sourceReviewReference`, `independentGradingProofReference`, `runtimePracticeProofReference` or `tests[].reference`. **A bare URL, boolean, screenshot label or a matrix entry is not sufficient evidence**, and these alternatives do not excuse absent actual source review or authenticated grading/persistence. Some genuine CI artifacts are external to Git; their run URLs, original tested SHA and scope are indexed separately in the academic evidence matrix as **TECHNICAL_ONLY**, without turning technical PASS into automatic lesson approval.

**Original-source Practice cardinality is unrestricted.** The source-vetted candidate map may specify `candidateFamilies` (one or more); until individually reviewed and registered, the existing `candidateFamily` remains authoritative. The grade gate rejects unrelated/duplicate/omitted families. Every newly listed family must also be integrated into the real C# lesson contract and authenticated scoring; the runtime registry test verifies exact set parity. Authored-only ordinal 65 has zero numeric Practice families plus teacher review rubric. Mixed authored/numeric ordinals require both kinds of evidence, never fictitious observed student work.

**V1 scoped exact-SHA freshness:** an acceptance record may list `affectedCodePaths` containing exact, previously reviewed source/test files relevant to that lesson. Git verifies that none changed since `testedCodeSha`; absent this field, the older conservative shared-code comparison remains. Choose paths based on the **actual affected skill, renderer, grader, student runtime, source/task verification and dependencies**, not merely the easiest file to keep green. Protected exact-head GitHub checks and the actual runtime grading/tenant gates remain mandatory regardless of per-lesson proof reuse. The final gate remains all **96/96 independently evidenced new lessons**, references to historical **1–50 without recertification**, one merge/deploy decision, exact Render LIVE SHA, then owner production UAT marked as performed or pending.

No item in this section declares lessons 51–146 approved. The current CI index may be reused only as a scoped technical lead for further academic review; a new tested commit must be checked for changes relevant to that evidence.
