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
