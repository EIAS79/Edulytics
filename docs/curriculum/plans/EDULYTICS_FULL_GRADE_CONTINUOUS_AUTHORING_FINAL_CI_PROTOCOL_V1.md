# Edulytics — Continuous Whole-Grade Authoring / Final-CI Protocol V1

**Status:** Owner-requested execution-throughput amendment, 2026-10-19. Applied on an independent unpublished authoring branch, pending normal GitHub review and merge. Not a release approval.
**Governing academic quality:** Original `EDULYTICS-GRADE-FIRST-5-V1`, especially original student-facing tasks, A–D, source-faithful teaching, genuine Practice and independent correctness tests.
**Release policy:** `EDULYTICS-GRADE-FIRST-FULL-GRADE-V2`; one whole grade, no partial publication.
**Purpose:** Stop full GitHub CI, disposable PostgreSQL/HTTPS/browser testing and git-SHA evidence reconciliation from restarting on every intermediate lesson edit. Change execution timing and branch topology, **not** criteria or protections.

## 1. Branch topology (mandatory for continuous authoring)

1. Preserve the original open Draft candidate **PR #567** and its branch `curriculum/full-grade-g1-l51-20261018`. Never reset, clean, overwrite, force-push or prematurely merge it. Preserve historical 1–50 unchanged.
2. For the remaining Grade 1 scope (new ordinals 51–146, 96 total), use **`curriculum/g1-continuous-authoring-final-ci-20261019`** as the separate unpublished working branch, created at the exact PR #567 head `351b91e163690976cf33582c81e5c9aba9dc8741`. Do not open a PR for this working branch or push work-in-progress edits to PR #567.
3. Commit code, source-task gap repairs, tests, cache-reuse notes and per-lesson source review checkpoints to the working branch. Do not run the full PR CI after each authoring commit; GitHub PR-triggered workflows will not be started merely by editing a branch with no PR. **Do not disable or change any required GitHub workflow, status check, branch protection or production trigger**. If a repository-wide push workflow is independently configured to run, it remains authoritative.
4. At content freeze, check that original PR #567 has not advanced unexpectedly. If changed, compare and reconcile without losing either side. Promote the **completed candidate** by a safe, non-force fast-forward from the working branch to the protected-review candidate branch. Open no parallel competing grade-release PR. Let the existing Draft PR enter final review only when actual evidence supports it.
5. The working branch is NOT a release branch or a proof of passing checks, and is NOT deployed. The first full-grade CI after promoting final content to PR #567 is part of the mandatory final validation.

## 2. One continuous grade pass: reuse before repair

- Inventory all 146 canonical Grade 1 codes **once**; preserve old released 1–50 and reuse all existing new 51–146 Rich V2, Practice, tests, sources and checkpoints. No rewriting a candidate because its ledger says `IN_PROGRESS`.
- Use **only the existing cached 2021 IM/Kendall Hunt source textbook on Our-CS (read-only)** when its verified content is available. Reuse original recorded file SHA and teacher/student activity audits. No repeated online browsing/downloading and no repeated extraction of a full grade for each lesson; access external official sources only to resolve a genuine missing/conflicting original source.
- Perform each original V1 A (student/teacher source and activity comparison), B (learner-visible authored teaching), C (source-specific Practice/gradeable and teacher-only activity boundary), and D (mathematics, visual and rights evidence) substantively. Record exact existing evidence for each lesson as it is reviewed, and repair only documented differences. An unchanged verified implementation is reused.
- Group cross-lesson work **by shared capability or source issue**, not by a per-lesson stop/commit/check/re-run ceremony. A shared math-engine repair should be done once, with a dependency list and affected mathematical assertions; NEVER count one general CI pass as 96 academic acceptances.
- No five-lesson **release** chunks, no per-lesson user progress reports, no time quotas. Source notes remain lesson-specific and fully auditable.

## 3. Lightweight authoring checks versus expensive final gates

**During continuous authoring:** review diffs, static format/syntax, sample mathematical positive/negative cases, source parity, and narrowly scoped deterministic affected tests **only when needed to prevent cascading defects** and when the execution environment supports them. Prefer one focused check after completing a shared component rather than running full CI after each edited lesson. Fail fast on a demonstrated syntax/math/security regression; fix it before building more changes on it.

**Do not repeatedly launch on intermediate commits:** full solution CI matrices, repeat isolated PostgreSQL/seed, 500-lesson login-and-attempt probes, Chromium screenshots, full multi-tenant HTTPS flows, Render redeploys, full original-source rehashing, full-grade positive eligibility, or mass SHA recertification.

**When all grade candidate code/teaching/Practice is finished, content-freeze and run final validation:**
1. Freeze the code and source-derived text. Reconcile full manifest, source-task coverage, allowed Practice families, correctness/negative tests, security and all previously reviewed rights/teacher-only boundaries.
2. Run the existing protected GitHub CI **for the single frozen candidate SHA** after promoting to PR #567; run full .NET, full mathematical oracles, original-source and semantic tests, authenticated student/teacher tests, disposable DB/500 attempts, cross-tenant checks, browser/rendering and fail-closed negative controls required by the repository. Capture actual run IDs, checkout SHA and artifacts.
3. Finalize each new lesson's **individual** internally accepted V1 record using its real original-source evidence, valid Practice/tests and relevant exact-SHA runtime proof. Until verified, keep `IN_PROGRESS` / `BLOCKED` or a truthful non-acceptance review checkpoint, **never** `INTERNALLY_ACCEPTED` by prediction. Previously recorded approvals with changed shared dependencies must be refreshed, not rubber-stamped.
4. Updating the evidence ledger is itself a new commit. Because mandatory checks are tied to the PR **head SHA**, run the required final checks once more on that *evidence-frozen* head if GitHub requires it. This is an **end-of-grade evidence finalization/release cycle**, not 96 per-lesson CI cycles. Re-run only after a real failed check or an actual code/evidence change that invalidates the prior SHA.
5. **Positive release gate remains 96/96 independently evidenced new lessons** plus 50 historical references, complete mandatory green checks, protected review/merge, actual Render LIVE SHA/health/authenticated grading, followed by owner's real production UAT. `NO_GO` cannot be overridden by an author's claim.

The objective is **one concentrated end-of-grade CI phase**; it is not a dishonest promise that exactly one physical workflow run will suffice. Failures or the final evidence-ledger SHA change can require additional full checks. Never reduce required tests just to meet a run count.

## 4. Anti-rework controls and resume handoff

- Maintain a single source-to-implementation gap register per lesson: `REUSE_CONFIRMED | CONTENT_REPAIR | PRACTICE_REPAIR | SHARED_DEPENDENCY | SOURCE_BLOCKED | READY_FOR_FINAL_VERIFICATION`; these are working classifications, **not** academic-acceptance states or replacements for the existing canonical ledger.
- Reuse the current artifacts `full-grade-progress.json`, `full-grade-academic-evidence-matrix.json`, candidate family map and source task audit; do not build a parallel approval schema or a second authoritative ledger. Save only meaningful newly established findings, paths and status.
- Mark a lesson `READY_FOR_FINAL_VERIFICATION` in its working notes only when all original source/teaching/Practice parts exist and all known defects are fixed; hold final acceptance until authentic final-SHA proofs are established.
- Record source rights honestly; first-party SVG does not prove licensed publisher imagery. Teacher-observed authored/classroom activity cannot be inferred from CI.
- Group the user update at substantial **whole-grade milestones** or actual blockers; no one-message-per-lesson, no report after every small commit. The final report lists exact accepted/pending count, checks, deployed commit and owner UAT.

## 5. Non-negotiable invariants

No changes to the original V1 source/task learning objectives, per-lesson A–D evidence meaning, required tests, security, authorizations, student data, published 1–50, curriculum IDs, source file integrity, or full-grade fail-closed gate. No reusing older passing checks for a changed relevant code dependency. No forcing the PR branch forward after a conflicting update. No production deployment, merge to `main`, or invented academic sign-off while release is `NO_GO`. This document optimizes the **when/where** of validation only.
