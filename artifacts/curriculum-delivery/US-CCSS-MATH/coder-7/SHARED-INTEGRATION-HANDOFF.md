# US-CCSS-MATH ALG2 — Coder 7 shared integration handoff

Status: **OPEN / NOT VERIFIED**. Owner: designated global integration authority, coordinated through American Supervisor 3. Source branch: `us-ccss/supervisor3/coder7-alg2`.

## A. Explicit high-school adoption — existing issue #453

- [Issue #453](https://github.com/EIAS79/Edulytics/issues/453) remains open at this checkpoint.
- The canonical course is `US-CCSS-MATH:HS:TRAD:ALG2:IM-1E` with `CourseCode=ALG2` and `Pathway=Traditional`; the generic high-school placeholder is not an accepted course selection.
- Integration owner must implement an administrator-selected, persisted, validated course/pathway choice, fail-closed incomplete-selection diagnostics, and a safe migration for existing placeholder school adoptions. Do not silently assign Traditional to an unknown school.
- Acceptance request: with disposable independent US schools, prove ALG2 course-specific student/teacher lesson retrieval, negative cross-school and cross-course isolation, exact lesson-scoped Practice, answer scoring/persistence, and no empty lesson list from ambiguous adoption. Attach exact commit SHA, run URL and screenshot/test evidence. Do not mutate Neon production.

## B. Course-wide exact Practice gaps

- Only U02:L17 (rational graphs; five approved question families) and U05:L10 (function combinations; two families) have explicit rows in the shared `lesson-skill-mappings.v1.json` baseline. Both are routing metadata, not authenticated student proof. Other lesson Practice may resolve through official-outcome rules; **enumerate effective runtime contracts for all 120**, then verify mathematical equivalence of taught skills and assessed skills.
- Global skill maps, question-family registry, generator, normalizer/verifier, persistence and `LessonPracticeContracts.cs` are shared-integration-only. Coder 7 has not edited them.
- Request exact per-lesson map: lesson ID, eligible family IDs, resolved source, fallback choice, sample generated prompt, expected canonical answer, positive/negative/malformed/domain exclusions, persistence status, independent oracle, and evidence SHA. Explicitly classify proof/drawing/discussion lessons as rubric/human review where required.

## C. Materialization, version predecessor and source fingerprint

- Course-owned content pack has new student-directed Unit 1 text on this branch, while the baseline `contentVersion=phase30-common-core-rich-content-v1` and existing `sourceSha256`/`canonicalBodySha256` fields remain unchanged to preserve source provenance. **Do not treat these historical fields as the checksum of newly revised effective content without verifying their contract.**
- Shared owner must examine `CanonicalLessonContentMaterializer.ComputeLessonFingerprint`, `GetEffectiveContentVersion`, and the seeder's existing-row upgrade rules. Establish a verified predecessor/content version and a reversible, non-destructive migration into disposable DB first. Distinguish effective materialized body from raw JSON body. Prove the real student route sees the new explanation while unrelated US/Cambridge/UAE/Poland rows do not change.
- Current open cross-curriculum paths: Cambridge [PR #399](https://github.com/EIAS79/Edulytics/pull/399) touches shared Practice map/materializer and [UAE PR #394](https://github.com/EIAS79/Edulytics/pull/394) touches the shared content seeder. Compare actual HEAD path/symbol and run affected cross-curriculum regressions before integration.

## D. Release gates and safe rollback

- Block Stage E release eligibility until all 120 lessons have independently reviewed teaching-to-assessment evidence, compiled/tested affected functionality, and owner-approved whole-course candidate.
- Stage F remains the designated integration/release owner's responsibility: protected CI, authorized staging, actual deployed Render SHA, real authenticated course selection and owner UAT. No Coder deploy or direct Neon writes.
- Roll back by reverting the isolated Coder 7 course PR and separately reverting any owner-approved shared migration with a verified predecessor; do not infer that Git revert reverses persisted database changes.

**Action requested:** Supervisor 3 assigns a single named shared owner to paths, supplies the owner with this handoff, and records accepted integration SHA/results in the 120-row ledger. Independent Coder 7 content work continues on its own branch.

---

## V9 authoring completion handoff — 2026-10-29 (latest checkpoint)

**Course-only authoring status:** 120/120 lesson records have substantive source-linked learner content; 118 new revisions and 2 existing reviewed/reused lessons (`U02:L17`, `U05:L10`). First Edition source-to-student second pass has also been performed for formerly pending U04 L01–L15. There are **0 remaining baseline authoring and 0 pending second-pass U04 source rows** in the 120-row ledger. This does **not** mean the 120 lessons were independently academic-approved, rendered in a live classroom, or mapped to exact authenticated Practice.

**Evidence:** `U07-L01-L16-source-to-learner-evidence.md` and `U04-L01-L15-second-pass-source-review.md`; `tests/coder-7/*.test.cjs` ran on isolated Our-CS branch at SHA `bb64f231bcaf2b90241b4c19383f2cd8b396d4d9` with **116 passed / 0 failed** across 12 suites.

**Concrete failing .NET gate:** `dotnet test tests/Edulytics.Tests/Edulytics.Tests.csproj --filter FullyQualifiedName~UsCommonCoreExpandedAcademicLessonBodyTests.RepairedContentHasVerifiedDigestAndSpecificWorkedMathematics` compiled and executed with **5 passes / 1 failure**. Failed lesson is `PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U02:L18`; the test recomputes a canonical SHA256 from the seven learner translation fields and compares to the historical `canonicalBodySha256` field. Failure: historical expected digest begins `7cbc61c03b9ab3dcd69a13aed4fd3b580850f5444`, revised-body actual begins `2ef725ec1a74d379838e2483a831dc377f02d08bb`. The source's paperback context was added to an otherwise correct rational horizontal-asymptote lesson; **do not revert the academically useful context without review** and **do not falsify any historical hash or disable the assertion**. The designated academic content-version owner must verify full before/after body digests, source/academic evidence, approved successor version and backward-compatible materialization, then update appropriate protected acceptance artifacts and rerun exact-head tests. Ensure the natural `hole` and `horizontal asymptote` requirements remain covered.

**Integration owner responsibilities remain unresolved:** school-specific Traditional ALG2 persisted adoption ([#453](https://github.com/EIAS79/Edulytics/issues/453)), every lesson's mathematically faithful question family/verifier/persistence contract (explicit mapping rows historically only `U02:L17` and `U05:L10`), two independent school negative tenant/cross-course checks, safe content successor/materialization, academic review, final .NET and protected CI, staging browser checks and owner UAT. Shared work stays on `us-ccss/supervisor3/hs-shared-integration-20261029` with Supervisor 3 coordination. Relevant shared scope issues: [#584](https://github.com/EIAS79/Edulytics/issues/584), [#587](https://github.com/EIAS79/Edulytics/issues/587), and [#453](https://github.com/EIAS79/Edulytics/issues/453). **Never merge PR #588 or deploy this unapproved content based on course-owned Node success.**

Status: `COURSE_CONTENT_AUTHORED / STAGE_D_AND_SHARED_INTEGRATION_OPEN / PR_DRAFT / NO_GO`.
