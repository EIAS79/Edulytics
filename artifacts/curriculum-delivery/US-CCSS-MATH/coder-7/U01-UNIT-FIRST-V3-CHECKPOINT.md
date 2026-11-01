# Algebra 2 — U01 Unit-First V3 checkpoint (30 October 2026)

**Repo / PR:** EIAS79/Edulytics, PR #588, branch `us-ccss/supervisor3/coder7-alg2`
**Baseline HEAD checked on Our-CS:** `1894f9d95a7057bb57927c78c3ddf8647a2b0bbb`
**Unit:** U01 Sequences and Functions, official manifest 11 lessons (L01–L11), 120/120 course lessons already authored.
**Status:** `UNIT_BLOCKED`. **First open lesson** is **U01:L01**; no onward L02/L04 work is authorized until U01:L01 closes A–B–C.

## Evidence reused, source rechecked

Original local publisher source remains on `Our-CS` in the 2019–2021 IM Algebra 2 First Edition directories `11_HS_Algebra_2/1/1` and `11_HS_Algebra_2_Student/1/1`. Recomputed SHA-256 of all five original teacher/student preparation/activity/practice HTML files. **5/5 matched** the hashes already in the L01 ledger. The existing corrections for What's Next, Hanoi, optional checkers, the 64-disc extension and four original Practice exercises are retained, not rewritten. Historical `sourceSha256`, `canonicalBodySha256`, `contentVersion`, publisher URLs and earlier independent math fixtures were not altered. **A_CLOSED and B_CLOSED were already supported by earlier evidence**, not claimed as newly closed in this session.

## C reviewed as two separate scopes

- **Course-owned preparation:** added `src/Edulytics.Core/Mathematics/Practice/UsCcssAlg2U01L01PracticeAdapter.cs`. It prepares deterministic, lesson-code-scoped Hanoi, optional checker, recursive double-minus-one/halving/affine reverse/Fibonacci exercises, an exact integer expected answer and structured teacher-only rubrics for Jada's reasoning, Noah's optional checker argument and two distinct rules from 0,5. It reuses `UsCcssAlg2SequencePracticeMath.cs` with BigInteger, rational equivalence and correct backward indexing. The internal candidate is explicitly forbidden from being serialized to learner DTOs because it contains a canonical answer. Domain validation rejects out-of-range questions; teacher narratives return `TeacherReviewRequired`, never an automatic score. New focused xUnit source added at `tests/Edulytics.Tests/Acceptance/UsCcssAlg2U01L01PracticeAdapterTests.cs`.
- **Missing actual course-to-runtime connection:** `LessonPracticeCapabilityResolver.TryResolve` requires `LessonPracticeContractRegistry.TryResolve` to produce `READY_VERIFIED`. Direct inspection of `LessonPracticeContracts.cs` `BuildAllEntries`/`TryResolve`, `question-family-registry.v1.json` and `lesson-skill-mappings.v1.json` shows no exact `PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U01:L01` or `usccss.alg2.u01.l01.*` registration. A detached adapter is not usable student Practice. **C_PREPARED / C_CLOSED=false / ABC_READY_FOR_D=false** is mandatory.

### Central ownership request — for Supervisor 3

**Owner of course-specific implementation remains Coder 7.** Assign **exclusive temporary editing ownership** of `src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs` to Coder 7 (clear any Coder 8 concurrent lease first). If central `question-family-registry.v1.json`, `lesson-skill-mappings.v1.json`, or the central generator routing/engine must be changed, assign exclusive ownership of those exact files separately after examining #600/#602 and #399/#394/#492/#553 diffs. Never copy/cherry-pick whole shared PRs or modify other courses. Implement the smallest verifiable registration/dispatch that uses the course-owned adapter, with a checked `READY_VERIFIED` contract only once its family actually resolves; record the final actual math and rubric readiness.

**Distinguish gates:** actual HTTP attempts, Gradebook persistence, school adoption, two-school isolation and full CI belong to **Stage D** and are **not** cited as the cause for blocking *course-owned C preparation*. The blocking Stage C prerequisite is missing valid **LessonCode → Registry → actual generated/verified family** routing, with separate central-file ownership conflict.

## Checkpoint counters (formal current review, not historical authorship)

`TOTAL=120 | AUTHORED=120 | A_CLOSED=3 | B_CLOSED=2 | C_PREPARED=3 | C_CLOSED=0 | ABC_READY_FOR_D=0 | D_VERIFIED=0 | BLOCKED=3`

**U01 unit counters:** PreviouslyClosed=0, NewlyClosed=0, UnitReadyForD=false, LastVerifiedClosedLesson=none, FirstOpenLesson=U01:L01, NextUnit=NOT_ALLOWED_U02. Existing L02/L03 source/content improvements are preserved but do not authorize skipping the first open lesson.

**Verification boundary:** source-file hashes and code/registry existence inspected directly. New adapter xUnit test **NOT_RUN** in this V3 A–C cycle. Historic targeted Node `16/16 PASS` was on **previous** SHA `1894f9d95a7057bb57927c78c3ddf8647a2b0bbb`, not a test of the new adapter. No manual full build, automated test suite, full CI, authenticated E2E, merge, deployment or production data operations occurred. GitHub Actions may run automatically after push.

**Next action:** stay at U01:L01; obtain exclusive central file ownership and finish the exact Practice routing, then review the actual registration/academic requirements, close L01 only on proof, and only then resume L02 → L03 → L04–L11 in ordinal order.
