# Edulytiks Coder 5 — Algebra 1 V12 Execution Evidence (2026-10-29)

## Source and checkpoint
- Existing Draft PR #589, course-owned branch `feat/us-ccss-alg1-coder5-full-course-20261028`.
- V12 started from verified GitHub/local HEAD `29a47bfe76e5f7712bd56badc27a40acee86a9fe`, with the original 132 lesson IDs preserved.
- All original 132 Rich V2 candidates remain unique, valid and source-title matched.
- 129 remaining teacher preparation and student activity pairs were fetched from Illustrative Mathematics 2019–2021 edition with **258 HTTP 200** responses. Compact source activity headings and SHA256 digests were captured. **This is an automated inventory only and must not be counted as independent stage-A review.**
- Manual source-targeted V11 and V12 comparison covers 17 candidate lessons (3 in V11, 14 V12), across seven units. Another 115 require independent detailed comparison of each teacher goal, each student task, mathematical examples and authentic interaction modality.
- V12 source and math remediations: `U01:L04`, `U01:L05`, `U02:L01`, `U02:L02`, `U03:L01`, `U03:L02`, `U04:L01`, `U04:L02`, `U05:L01`, `U05:L02`, `U06:L01`, `U06:L02`, `U07:L01`, `U07:L02`. These append course-owned first-party source-aligned tasks without deleting legacy worked examples.
- Representative meaningful corrections include optional prerequisite statistics and MAD, ten dot-plot/frequency matching cards, multi-plan cost modeling, partner information gap, conditional segmented frequency comparisons, nonfunction input uniqueness, three named function graphs, tripling versus doubling, time-step-10 exponential crossover, garden area and geometric picture framing, and algebraic-versus-graph solving limitations.

## Student visuals and teacher rubrics
- New original static SVG files: five in `src/Edulytics.Web/wwwroot/images/curriculum/alg1-v12/`, covering distribution dot plots, function/nonfunction points, additive versus exponential growth, area of a fixed-perimeter garden, and uniform picture frame.
- The Algebra 1-only `_UsCcssAlgebra1GraphGallery.cshtml` partial includes code-exact file paths but **the shared Rich V2 host remains unchanged in V12**. These five assets are **NOT confirmed displayed to an authenticated student**. Only Supervisor 2/shared-view owner may coordinate extending the host routing after review.
- Previous V11 3 SVG visuals and host allowlist remain unchanged.
- All 132 lessons have authored teacher rubric narrative. The V12 teacher assessment matrix documents **17 source-specific structured rubric DRAFTS (3 from V11 + 14 from V12)** and **115 unstructured lesson-specific reviewer tasks**. These are not authorized teacher assignments, student submissions or persisted scores.
- Do not confuse an independent sample graph or typed rubric with a completed original interactive/partner activity or teacher grade.

## Practice integration and segregation of duties
- Course-specific `US_CCSS_ALG1_CODER5_V12_SUPERVISOR2_PRACTICE_CONTRACT_HANDOFF_20261029.json` contains **132 exact LessonCodes**, publisher source URLs, source activity title candidates where retrieved and proposed assessment/teacher-only modalities.
- 14 contracts contain source-specific proposed mechanics; the remaining are V11 review candidates or explicitly labeled **UNIT PLACEHOLDERS pending full lesson review**, not source-accepted contracts.
- Existing V10 fail-closed Algebra 1 Practice safety gate remains. **No V12 file modifies `Edulytics.Core/Mathematics/Practice`, `Edulytics.Services/Practice`, or the shared Rich V2 host.** The central shared integration owner must implement verified exact contracts and teacher feedback persistence.
- Supervisor 2 must validate course-specific adoption and tenant isolation on disposable two-school HTTPS and DB fixtures under issue #453, and coordinate shared CI fix PR #598.

## Acceptance ledger status
| Gate | Verified accepted | Unverified |
| --- | ---: | ---: |
| Stage A academic approval | 0/132 | 132/132 |
| Stage B effective authenticated rendering | 0/132 | 132/132 |
| Stage C exact Practice and teacher grade/feedback | 0/132 | 132/132 |
| Stage D authorized two-school persistence | 0/132 | 132/132 |

The ledger `US_CCSS_ALG1_CODER5_V10_ACCEPTANCE_LEDGER_20261029.json` records the V12 source retrieval, per-lesson preliminary repair status, graph delivery evidence, teacher grading status and remaining prerequisites without false promotion. All changes remain Draft/NO_GO. No production deployment, data mutation, main merge or authentic teacher UAT claim.

## Verification
- Source/identity script: `node tools/curriculum/us-ccss-alg1-coder5-check-sidecars.cjs`.
- Source-fetched audit, normalizer and 14 source-activity patch scripts: `tools/curriculum/us-ccss-alg1-coder5-v12-*`.
- New independent mathematics, XML safety and contract-matrix xUnit test `tests/Edulytics.Tests/MathematicsIntelligence/UsCcssAlgebra1V12SourceTaskMathematicsTests.cs`.
- Relevant previous course / private Practice / lesson-content service tests and `Edulytics.Web` Release build must be green before pushing. The repository SDK pin `global.json` must be restored unchanged.
