# Geometry course — shared integration request (not an authorization to merge)

**2026-10-28 · American Coder 6 → Supervisor 2 / designated global integration owner**
**Scope:** `US-CCSS-MATH:HS:TRAD:GEO:IM-1E`, 124 canonical lessons, 8 units
**State:** unintegrated; no production release, persisted data mutation, test or acceptance claim.

## Request 1 — effective Rich V2 source/seed version boundary

Course-owned first-party Rich V2 candidate resources now exist in:
- `src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u01-l01-l03-2021.rich-lesson-v2.json`
- `src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u01-l04-l06-2021.rich-lesson-v2.json`

Shared paths and symbols requiring the **designated single writer** to investigate:
- `src/Edulytics.Data/Seeding/CanonicalLessonContentMaterializer.cs`, specifically `GetEffectiveContentVersion`, `IsReviewedCorrectionTarget` and the materialization precedence. Existing code applies common-core-specific revisions after the canonical pack.
- `src/Edulytics.Data/Seeding/MathematicsCanonicalLessonContentSeeder.cs`, specifically `SeedApprovedProductionCorrectionsAsync`, `FindReviewedProductionParityMismatchesAsync`, persisted translation/version checks and guarded upgrade behavior.
- Rich V2 embed/load/render pipeline (identify the precise loader and route before patching). `Edulytics.Core.csproj` already embeds `RichV2/*.rich-lesson-v2.json`, but embedding alone does not establish effective learner-visible precedence.

**Why local-only work cannot prove success:** changing course-owned JSON does not prove that a previously seeded school receives the new translation, that historic attempts remain intact, or that a second Rich V2 resource with a matching key is selected deterministically. An unreviewed shared seeder change could affect American grades, Cambridge, UAE and Polish lessons.

**Required integration design:** inventory existing GEO Rich V2 records, define explicit source/lesson/culture ownership, guard current source URL and version/hash expectations, refuse unknown persisted body drift, permit safe idempotent upgrades only from verified predecessor versions, preserve historical attempts and existing teacher edits, and log/rollback precisely. Document actual persisted before/after evidence from **disposable** PostgreSQL, not live Neon.

## Request 2 — explicit course adoption, Practice and human assessment

Existing issue: https://github.com/EIAS79/Edulytics/issues/453.

Shared candidates for review, **not Coder 6 edit targets**:
- `src/Edulytics.Data/Repositories/StudentPrivatePracticeRepository.cs` — verify actual `GetContextAsync` high-school course-selection/lesson filtering behavior.
- `src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json` — no exact GEO lesson prefix in inspected baseline; this is not proof of no fallback Practice.
- `src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs` and the actual course-aware generator/verifier/Adaptive/Builder selection paths (locate exact implementation before modifying).
- Teacher-reviewed evidence route for actual constructions, proofs and student-drawn figures. Do not introduce fake numeric autograding merely to increase coverage.

**Required regression matrix:** correct explicit `CurriculumPack=US-CCSS-MATH`, `Pathway=Traditional`, `CourseCode=GEO`; reject ALG1, unspecified Traditional and invalid/legacy course adoptions; student lesson and exact Practice/adaptive/teacher Builder/Assessment flow; saved grade/attempt; two independent school tenants, cross-school forged IDs denied; canonical seed idempotency and upgrade rollback; non-GEO American levels and Cambridge/UAE/Polish regression; keyboard and construction-diagram accessibility.

## Ownership and release boundary

American Supervisor 2 should assign one reviewed shared-path primary writer and issue an integration PR separate from or coordinated with [the Geometry course draft PR](https://github.com/EIAS79/Edulytics/pull/583). No shared changes should be merged merely because the candidate exists. Finish course-owned source teaching and unit tests in parallel. Final grade/course acceptance requires all 124 individual A–D evidence records at the frozen commit, exact protected CI, separately verified deployment and owner UAT.
