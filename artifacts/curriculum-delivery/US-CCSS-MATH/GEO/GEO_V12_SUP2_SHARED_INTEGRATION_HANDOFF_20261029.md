# Geometry V12 — course-local implementation results and precise shared integration handoff

Course: `US-CCSS-MATH:HS:TRAD:GEO:IM-1E` (124 lessons). Owner: American Coder 6 / Supervisor 2. Course PR: [#583](https://github.com/EIAS79/Edulytics/pull/583) **DRAFT / NO_GO**.

## A — source task parity

The original V11 source manifest had 85 initial teacher/student comparison rows, 6 teacher-preparation-only rows and 33 syllabus-only rows. During V12:
- Public first-edition teacher preparation AND student activities inspected for 33+6 Unit 6–8 rows. All 39 updated to `SOURCE_TEACHER_STUDENT_ACTIVITIES_READ_PARITY_UNVERIFIED`.
- Re-read public teacher/student pages for original 18 Unit 5 records and recorded task/figure gaps as `SOURCE_TEACHER_STUDENT_REAUDIT_TASK_GAPS_PENDING`. Remaining 67 Unit 1–4 rows retain `SOURCE_TEACHER_STUDENT_COMPARE_INITIAL`.
- New source-grounded tasks authored and numerically checked for `U06:L03` (similar right triangles in dilation), `U07:L02` (AA-similar intersecting chord triangles), `U07:L06` (incenter locus proof motivated by salt ridges), `U07:L10` (do not teach radians before formal introduction), `U07:L12` (double number line/sector shading), `U08:L02` (duplicate name slips and sampling), `U08:L04` (independent two-dice outcomes).
- The 124/124 authored lesson identity and original alignment metadata have not been changed. These source checks do **not** prove task-by-task matching for every original investigation, source restricted teacher material, full image parity, or academic acceptance. All A/B approvals remain **0/124**.

## B — renderer-integrated genuine accessible geometry pictures

Three original SVG candidates are staged in:
`src/Edulytics.Web/wwwroot/geometry/geo/v11/` for U05:L01, U06:L07, U07:L11.
Their structured manifest is `artifacts/curriculum-delivery/US-CCSS-MATH/GEO/geo-v11-three-original-svg-candidates-20261029.json`.
- Actual `RichLessonVisualKind` enum: `src/Edulytics.Core/Curriculum/RichLessonContentV2.cs` currently has only `CalculationChain,NumberLine,DoubleNumberLine,FractionBars,RatioTable,PlaceValueChart,Scale,EquationSet,ShapeDecomposition,Grade4Unit1AreaFigures`; no safe external image/SVG asset reference kind.
- `src/Edulytics.Web/Views/Shared/_RichLessonVisual.cshtml` does not handle a generic SVG reference.
- Designated shared writer only: add allowlisted local asset visual capability with strict path validation (no data URL, script or arbitrary external origin), accessible description and actual role-aware authenticated student/teacher render tests. Preserve existing 10 visual kinds. Three standalone SVG assets are **NOT** currently student-visible evidence.

## C — exact lesson Practice and teacher-graded tasks

Static inspection of `src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json` shows 238 mappings, **zero exact Geometry lesson codes**; review `src/Edulytics.Core/Mathematics/Practice/LessonPracticeContractProjection.cs` (`READY_VERIFIED`, allowed family routing, verification policy), `LessonPracticeCapabilityResolver.cs`, `LessonPracticeContracts.cs`, and `src/Edulytics.Services/Practice/StudentPrivatePracticeService.cs`. No exact GEO override was found in central Mathematics/Practice source. Do not infer actual user runtime outcome from static inspection alone or bypass fail-closed resolution.

Four **source-specific, authoring-only** teacher-reviewed tasks (U06:L03, U07:L02, U07:L12, U08:L04) are in:
`src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-geo-v12-human-scored-source-activity-candidates.json`.
- Every task has 4 explicit criteria (0–2 each; max 8), numerical oracle subchecks, WRONG/MALFORMED/BOUNDARY/EQUIVALENT examples, drawing/proof rubric and fail-closed authoring status.
- `TeacherSubmissionPersistence=NOT_IMPLEMENTED`; no rubric is wired to actual student submission, teacher role review, save or grade.
- Required central work: approve source-accurate per-lesson skills/families, implement/reuse secure human review of constructed figures and evidence, generate and verify correct/wrong/malformed/equivalent/boundary outcomes, persist authenticated synthetic student and teacher results. Do **not** mark `READY_VERIFIED` from an authored rubric alone.

## D — central school/teacher authorization and deployment gates

Issue [#453](https://github.com/EIAS79/Edulytics/issues/453) owns Traditional/GEO course adoption, actual effective content-version selection and tenant isolation.
Required disposable synthetic 2-school checks:
1. Enroll student A in school A Traditional/GEO, student B in school B Traditional/GEO, teacher A authorized only for school A; do not touch production tenants.
2. Verify GET learner and teacher lesson for correct `GEO` `LessonCode`, actual Rich V2 `ContentVersion`, original visual and exact Practice family.
3. Submit numeric right/wrong/equivalent/malformed/boundary responses and separately submit human-assessed proof/drawing; verify teacher A can review/grade only A and saved feedback remains scoped.
4. Deny school B access to A attempts, wrong ALG1 course, wrong lesson code, unrelated student, unauthorized teacher, and direct cross-tenant object IDs.
5. Verify persistence and replay of grades/feedback, duplicate submission idempotence, mobile/keyboard/screen-reader rendered content.
No actual authenticated environment/authorized synthetic database or stored grader results were provided to Coder 6. **C/D acceptance remains 0.**

## Executed tests and CI

`node --test tools/curriculum/us-ccss-geo-richv2-contract.test.cjs tools/curriculum/us-ccss-geo-v11-conditional-integrity.test.cjs tools/curriculum/us-ccss-geo-v11-svg-assets.test.cjs tools/curriculum/us-ccss-geo-v12-source-math-contract.test.cjs tools/curriculum/us-ccss-geo-v12-human-scoring-contract.test.cjs`.

On authorized `Anas_Nitro / Personal` clean recovery checkout, V12 Node assertions cover 124 lesson structures, 372 work-example field completeness (NOT 372 math certifications), source task ledger and targeted independent math oracles; test results must be bound to final branch SHA.

**.NET blocker:** `dotnet` is not installed in the authorized device's PATH. The repo root `global.json` pins SDK `10.0.200`; do not modify it. On an approved .NET 10.0.200 runner use `dotnet --info` and `dotnet test Edulytics.sln -c Release` plus relevant authenticated acceptance tests against disposable fixture services. Never claim these run from existing Node evidence.

**CI blocker:** Seven GitHub Actions runs are queued at inspected course SHAs (no green conclusion), and shared historic Rich V2 fixed-count repair is separately in [#598](https://github.com/EIAS79/Edulytics/pull/598), not merged by Coder 6. Queue/runner capacity and shared test repair are owned by Supervisor 2. Do not weaken CI or modify other curricula.

**Decision:** `AUTHORING_COMPLETE / SOURCE_REVIEW_PROGRESS / PRACTICE_AND_RENDERING_BLOCKED / NO_GO`. Do not merge or deploy PR #583.
