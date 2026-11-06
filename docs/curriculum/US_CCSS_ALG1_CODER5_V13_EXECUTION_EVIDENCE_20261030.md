# Edulytiks — Coder 5 Algebra 1 — V13 verified course-local execution
Date: 2026-10-30 | Reviewer: Coder 5 | Course: `US-CCSS-MATH:HS:TRAD:ALG1:IM-1E`

## Device, repository and immutable recovery
- Device: `Our-CS / Primary`; isolated authorized checkout: `C:\Users\khali\Edulytiks-Coder5-ALG1-20261028`.
- Repository: `EIAS79/Edulytics`; **same OPEN/DRAFT PR #589**: https://github.com/EIAS79/Edulytics/pull/589.
- Branch: `feat/us-ccss-alg1-coder5-full-course-20261028`.
- Clean initial local HEAD matched PR HEAD: `f02c5ad81ad053d79a134e48d882f85221b53415`. No reset, no force push, no alternate branch. Preserve 132/132 Rich V2 lessons, including Unit 7.
- Read `AGENTS.md`, current lesson source metadata, V10 acceptance ledger, V12 source pair audit, per-course handoff and source-specific test contracts.

## Source task comparisons (preliminary educator review **not** completed)
These eight new teacher/student lesson source comparisons are course-specific, with corrected first-party student models. They are NOT authorized independent academic sign-offs.

| Code | Source teaching/student activity discrepancy and performed V13 remediation |
| --- | --- |
| U01:L03 | Publisher gallery includes Notice/Wonder, dot plots, histograms, box plots and critique. Kept existing multi-display task, added independently checked 9-point shifted-dot distribution comparison on common scale. |
| U01:L06 | Publisher's spreadsheet introduction is **optional**. Added Make-24 calculation and controlled live-spreadsheet negative/fractional probes; no fabricated spreadsheet or class attendance. |
| U02:L03 | Existing example compared three kinds of table rule while source targets four equation-matching patterns. Added four completely new table/rule pairs and exact contextual domains, preserving existing inverse-area work. |
| U03:L03 | Added a second association/no-association design, contextual row denominators, 80/30% segmented conditional bars, sampling/causation warnings and ethical student-variable inquiry. |
| U04:L03 | Supplemented correct existing drone height model with independent calendar-year/installation interpretation and a boiling-water constraints-to-graph exercise; graph from finite facts need not be unique. |
| U05:L03 | Added a second fictional exponential model, two-graph comparison, crossover at integer steps 2–3, and optional nonzero-base exponent-law boundary correction. |
| U06:L03 | Supplemented prior correct n(n+4) rectangular area with three different square-figure area cards, growing-square and growing-step patterns, and nonconstant first differences. |
| U07:L03 | Expanded reasoning tasks to classify 0, 1, or 2 real roots and demonstrate independent factor-zero and both-sign square-root methods. |

- Official source: Illustrative Mathematics Algebra 1 First Edition teacher prep `https://im.kendallhunt.com/HS/teachers/1/{unit}/{lesson}/preparation.html` and paired student `https://im.kendallhunt.com/HS/students/1/{unit}/{lesson}/index.html`.
- Earlier V11/V12 substantive preliminary comparisons: **17**. V13 additional: **8**. Total independently source-targeted author comparisons: **25/132**. Another **107** require exhaustive activity review, including teacher goals, source math, modality, context, visuals and standards.
- 129 teacher/student page-pairs fetched in V12 with HTTP 200 and SHA256 hashes remain preliminary **metadata only**, not teacher sign-off or complete source review. The original files and completed prior 17 reviews were retained.
- Course-local changed lesson sidecars under `src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-alg1-*.json`; corrections are append-only to authored explanations with independent new version codes, preserving the previously authored worked examples.
- Detailed per-lesson source URLs, exact new paragraphs, math oracle cases, proposed rubric and Practice families: `docs/curriculum/US_CCSS_ALG1_CODER5_V13_THIRD_CROSSUNIT_SOURCE_TASK_REPAIRS_20261030.json`.

## First-party graphs and authentic delivery limits
- Created three course-owned original accessible SVGs under `src/Edulytics.Web/wwwroot/images/curriculum/alg1-v13/`: `u03-l03-conditional-association.svg`, `u04-l03-temperature-constraints.svg`, `u05-l03-two-growth-models.svg`. SVGs include title/description, axes or clearly labelled bars and fictional context. These are instructor reference figures, not student-created artifacts.
- Preserved all prior V11/V12 graphics. The new V13 assets have not been routed through the shared Rich V2 host, previewed in an authenticated learner session, or accepted as Stage B proof. Supervisor 2/shared renderer owner must approve any host/gallery wiring before course-wide display is claimed.
- No shared Rich V2 renderer, global materializer or StudentPrivatePracticeService was modified in V13.

## Exact Practice requirements and human scoring
- Updated existing 132-row Supervisor 2 Practice handoff with eight *source-specific proposals*, including correct, incorrect, algebraically equivalent, boundary, malformed/unsupported, and human-scored graphical/submission cases. Source-specific proposals are not executable acceptance contracts and do not lift the existing fail-closed guard.
- Updated 132-row teacher assessment matrix: 25 source-specific 4-criterion rubric **drafts**, 107 source-review-dependent candidate rows. There are **zero** teacher-authorized scored submissions, grades, stored feedback, teacher access control executions or two-school scoring proofs.
- Shared issue [#453](https://github.com/EIAS79/Edulytics/issues/453): actual disposable authenticated HTTPS `commoncore-secondary` ALG1 `U04:L15` fixture resolves the lesson but cannot open a Practice attempt; the fail-closed `NoSupportedOutcomes` path must remain secure, not be converted to fallback generic questions. Handoff requires independent central exact-contract integration and persisted two-school authorized student/teacher flow.
- Shared PR [#598](https://github.com/EIAS79/Edulytics/pull/598) is Supervisor 2 scope; old global Rich V2 inventory assertions expected 318 instead of actual 450 and remain an independent merge/review decision. Do not patch global assertions in this branch.
- Baseline HEAD `f02c5ad...`: exact-SHA GitHub Actions 3 success / 6 failure, including shared catalogue and real HTTP acceptance failures; this is **not** a green check.

## Independent verification and acceptance boundaries
- Source + identity checker `node tools/curriculum/us-ccss-alg1-coder5-check-sidecars.cjs`: **132/132 valid**, no duplicate, missing or failed lesson.
- `git diff --check`: zero whitespace errors (Windows CRLF conversion notes are non-fatal).
- Python authoring generator syntax: checked; source repair generator is idempotence-protected and must **NOT** rerun after a complete V13 pass except with `--assets-only`.
- C# new independent math/graph/contract tests: `tests/Edulytics.Tests/MathematicsIntelligence/UsCcssAlgebra1V13SourceTaskMathTests.cs`. Existing V12 course-local integrity test updated only to respect cumulative V13 25/107, while independently preserving recorded V12 baseline 17 and 115. Neither file weakens shared fail-closed or authorization assertions.
- Initial focused local .NET V11/V12/V13 and fail-closed tests: **67/67 PASS**, after updating the course-local V12 cumulative matrix assertions from 17/115 to 25/107 (V12 historical ledger values unchanged).
- Final course + private Practice + official rule regression: **274/274 PASS**, **0 failed**, **0 skipped**, Release, filtered by `UsCcssAlgebra1|StudentPrivatePractice|OfficialLessonPractice|RichLessonContentV2LessonContentService`. This includes the additional V13 source-correction-to-actual-sidecar and malformed/domain-boundary mathematical checks.
- `Edulytics.Web` Release build: **SUCCEEDED**, **0 warnings, 0 errors**.
- .NET SDK `global.json` original 10.0.200 pin must be restored unchanged after temporary isolated local SDK 10.0.300 testing.
- All 132 authored candidates remain **A_ACCEPTED=0/132; B_ACCEPTED=0/132; C_VERIFIED=0/132; D_VERIFIED=0/132**. V13's lesson-level evidence cannot substitute for independent teacher approval, effective authenticated graph display, exact Practice attempt/feedback or authorized wrong-tenant/wrong-course two-school HTTPS data.

## Required handoff and release status
DEVICE=Our-CS / CLONE=isolated ALG1-20261028 / PR=#589 Draft / NEW HEAD=read the PR #589 commit SHA after the V13 push; do not infer CI success from this report / AUTHORING=132/132 / PRELIMINARY SOURCE TASK COMPARISONS=25/132 / CORRECTED V13=8 / ORIGINAL NEW STATIC SVG=3 / HUMAN RUBRIC DRAFTS=25 (SAVED SCORES=0) / EXACT PRACTICE CONTRACTS=8 NEW PROPOSALS, ENGINE NOT INTEGRATED / ALG1 U04:L15 regression=OPEN / ISSUE #453=SHARED / PR #598=SHARED / A_REVIEWED=25 / A_ACCEPTED=0 / B_IMPLEMENTED=STATIC REFERENCE ONLY / B_VERIFIED=0 / C_VERIFIED=0 / D_VERIFIED=0 / CI=BASELINE RED; NEW SHA PENDING.

**Release decision: DRAFT / NO_GO.** No main merge, deployment, production school or student data write, fabricated human scoring, or false stage promotion.
