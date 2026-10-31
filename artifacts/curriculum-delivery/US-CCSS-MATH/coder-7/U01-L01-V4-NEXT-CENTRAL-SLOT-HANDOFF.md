# Algebra 2 U01:L01 — V4 bounded SOLO course-owned closure / central-slot waiting

**Date:** 30 October 2026
**Repository / PR:** `EIAS79/Edulytics`, [PR #588](https://github.com/EIAS79/Edulytics/pull/588), branch `us-ccss/supervisor3/coder7-alg2`.
**Verified starting HEAD:** `2d97794bc9859a6070bdb64fa1680b0c2e6b118d`.
**Unit:** U01, **11 official lessons**. `FirstOpenLesson=U01:L01`; `LastVerifiedClosedLesson=null`; `LastClosureSHA=null`; `UnitStatus=UNIT_BLOCKED`; `UnitReadyForD=false`. No progress on L02/L03 or later.

## Source evidence retained

- LessonCode `PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U01:L01`; source `IM-HS:C3:U01:L01`, teacher `11_HS_Algebra_2/1/1` and student `11_HS_Algebra_2_Student/1/1`, 2019–2021 First Edition.
- Original source HTML SHA-256 five files from previous V3 ledger **5/5 match again in the V4 focused static/mathematical check**. See `algebra2-120-lesson-progress.json` first lesson `localSourceFilesSha256`; original historical source/canonical hashes preserved.
- A/B `A_CLOSED` / `B_CLOSED` **previous accepted course-owned evidence**, not a new closure. Existing lesson/figures/maths kept intact. Teacher publisher Cool-down remains `PUBLISHER_COOLDOWN_UNVERIFIED`; Edulytiks-authored adapted version is not asserted to be the publisher's.

## Concrete course-owned changes in V4

`src/Edulytics.Core/Mathematics/Practice/UsCcssAlg2U01L01PracticeAdapter.cs`:

1. Corrected Jada 3-disc → 4-disc teacher rubric: **7 + 1 + 7 = 15**, not the previous incorrect 3 + 1 + 3. Teacher requires the three-disc transfers around moving the largest disc and explanation of the lower bound.
2. Strengthened optional checker minimum proof: **n²** opposite-color pair-crossing jumps plus **2n** forward single-step slides gives `n(n+2)` legal moves as the lower bound; teacher must check legal construction, no backwards or same-color jumps.
3. Added concrete two-distinct-rules evidence for source Practice problem 4 starting `0,5`: arithmetic `0,5,10,15,20` vs quadratic `0,5,15,30,50`; two differently named equivalent rules are insufficient.
4. Added learner-only `LearnerQuestion` projection, a `ProjectForLearner` guard, `[JsonIgnore]` on secret expected answer and teacher rubric, and a redacted `Candidate.ToString()`. A canonical-candidate regeneration comparison rejects forged ItemId/family/answer/rubric variants before auto-grading. These are **source-level controls**, not yet E2E proof.
5. Added precise family dispatch `TryCreateForFamily`, `TryCreateAutoGradableForFamily` and narrow public **server-use** APIs `TryPrepareExactNumericQuestion(family, variantSlot, n, out prompt, out correctAnswer, out parameters)`, `VerifyExactNumericQuestion(family, IReadOnlyDictionary<string,int>{n,variant}, answer)`; correctAnswer MUST remain server-only. Machine-only variants exclude human-scored Jada, Noah and two-rule explanation. Unknown family/invalid parameter/extra parameter keys/forged family fail closed. Question ids and family ids remain course/lesson-scoped.
6. Prior BigInteger n1..64, rational/decimal equivalence, affine backwards and Fibonacci code was retained and reused. Source-specific xUnit tests added for projection/JSON/logging leaks, exact parameter maps, 64-disc value, malformed/forged cases, and teacher-review separation.

`tests/Edulytics.Tests/Acceptance/UsCcssAlg2U01L01PracticeAdapterTests.cs` is **AUTHORED, NOT_RUN**. The V4 focused Python static/math check directly reread the source and derived Hanoi n1..64, checkers n1..7, recurrences and two-rule distinctness; this is NOT a C# compilation nor integrated Practice test. Prior Node 16/16 PASS applies to old SHA `1894f9d` only.

## Read-only inspection of the current core/services shared API

- `LessonPracticeCapabilityResolver.TryResolve` only returns `READY_VERIFIED` contracts from `LessonPracticeContractRegistry.TryResolve`. At inspected baseline, exact U01:L01 LessonCode and all `usccss.alg2.u01.l01.*` families have **no registered route**. Do not publish it as student-available.
- `ExactSkillContractQuestionEngine.SupportsFamily` does not include these three ids. `Generate` accepts allowed families, deterministic int `seed`, unique fingerprint, and produces `ExactSkillGeneratedQuestion(Family,Prompt,Solution,ItemType,CorrectAnswer,IReadOnlyDictionary<string,int> Parameters,ExposureFingerprint)`. Course API supplies `family`, exact integer `n`, explicit `variant`, prompt and private answer, but does **not** yet construct an engine `ExactProblem` or runtime `AssessmentItem`.
- `BuildProblem` and `Solve` require a course dispatch, plus `Verify` must dispatch **before generic `int.TryParse`** for `2^64−1 = 18446744073709551615` and rational equivalents. A registry entry without those hooks would be **false-ready**. Numeric-only readiness is distinct from teacher-marked open reasoning.
- `LessonPracticeContractProjection.Load` requires explicit `practiceReadiness=READY_VERIFIED`, one registered `primarySkills` entry and approved `allowedQuestionFamilies` with `lessonPracticeRouting` and verifier policy. The hand-authored `LessonPracticeContracts.cs` path is an alternative, but still requires actual supported generation/verifier families. Do not prematurely mark any family READY.
- [PR #600](https://github.com/EIAS79/Edulytics/pull/600) and [PR #602](https://github.com/EIAS79/Edulytics/pull/602) were **Open/Draft/Unmerged at read-only check**. Preserve the #602 `All` READY-only and `PendingForAudit` fail-closed behavior. The integration handoff and prior #600 false-ready warnings are not permission to merge or copy those PRs.

## Lease and exact remaining gates

**Lease status: NEXT_EXCLUSIVE_SLOT_APPROVED_NOT_TRANSFERRED.** Supervisor 3 reserves `LessonPracticeContracts.cs` and related central changes under Coder 8's current exclusive slot. Coder 7 must **not** edit them until Coder 8's final HEAD/touched-file list/release and Supervisor 3 conflict review/explicit transfer. There is **no Shared Coder handoff**; Coder 7 owns the L01 implementation after exclusive grant.

**Minimum central edits after transfer (each separately owned and conflict-checked):**

1. `src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs`: add exact L01 contract only with genuinely supported families, `READY_VERIFIED` only after runtime engine supports, without changing non-Algebra2 or `PendingForAudit` fail-closed semantics.
2. `src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs`: add narrow family `SupportsFamily`, `BuildProblem`, `Solve` and early `Verify` hooks (before `int.TryParse`), no generic int narrowing for 64 discs; avoid autograding teacher-only variants.
3. `src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json`: register only implemented numeric family capabilities/verification policies and checked `lessonPracticeRouting`; may need reviewed skill metadata.
4. `src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json`: only if approved projection path is used; a direct exact contract may bypass this file, so **do not edit speculatively**.

**Additional academic acceptance:** the teacher reasoning rubrics must be independently reviewed; do not equate the numeric answers with a fully approved explanation rubric. Legacy source/canonical version predecessor/successor approval is a separate owner gate.

**Stage-D-only checks**, not blockers for independent course-owned C preparation: authenticated student/teacher E2E, persisted gradebook, two-school tenant isolation, adopted-school rendering and full CI.

**Unit decision:** `UNIT_BLOCKED`; `C_PREPARED` / `C_CLOSED=0` / `ABC_READY_FOR_D=0`; `lastClosureSHA=null`. No L02 activity this session, no shared/central edits, no merge/deploy/production writes. The next action is exclusive file lease transfer and precise engine integration for L01.
