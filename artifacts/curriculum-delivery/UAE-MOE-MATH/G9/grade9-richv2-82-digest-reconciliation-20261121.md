# Grade 9 General — independent RichV2 manifest SHA-256 reconciliation

- Scope: UAE-MOE-MATH, Grade 9 General, Arabic RTL, 13 units and 82 canonical first-party lessons
- Input candidate: `0e1f23b3d5f462f124c567b3b6be3d181f64306a`
- Reconciliation commit: `86e059ec3d09a6fa728a57701652559671c5fcd2`
- Evidence source: direct GitHub read of each of the 82 version-pinned RichV2 JSON files, UTF-8 SHA-256; each file was parsed and its `LessonCode` and Arabic `Title` compared with the corresponding existing import-manifest entry
- Changed import-manifest digests: **82/82**, after all 82 legacy digest values were found stale
- RichV2 teaching text, question contracts and lesson identities changed by the reconciliation: **0**
- Modified file: `grade9-82-production-import-candidate-NO_GO.json`, field `items[].richV2Sha256` only
- Source literal publisher parity: `UNKNOWN` (not independently certified)
- First-party owner/structural/RichV2 readiness gate: **PASS on input SHA only**; not a passing hosted GitHub Actions run
- Prepared G9 Node tests on input SHA: **FAIL** (the stale import-manifest hash); new manifest hashes are committed, but exact-new-SHA Node rerun and hosted CI still **NOT VERIFIED**
- Production permission: `allowProductionWrites=false`, `allowAutomaticReseeding=false`, `productionImportExecuted=false`, `mergeAuthorizedByReleaseGates=false`
- Student authenticated HTTPS E2E, isolated PostgreSQL 0-to-82 integration, exact-SHA hosted CI, Render deployment, and Neon publication: **NOT VERIFIED / NOT DONE**

This is a metadata evidence correction and is not a release decision, academic 1:1 publisher-parity certificate, or authorization to bypass missing test results. No live database or Render writes.
