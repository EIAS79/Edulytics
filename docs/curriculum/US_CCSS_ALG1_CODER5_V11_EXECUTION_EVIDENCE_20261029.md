# Edulytiks Coder 5 — Algebra 1 V11 execution evidence (2026-10-29)

Course: `US-CCSS-MATH:HS:TRAD:ALG1:IM-1E` (Traditional ALG1); existing draft PR #589, branch `feat/us-ccss-alg1-coder5-full-course-20261028`. Scope is exclusively course content, source-specific teacher rubric drafts and a narrowly gated lesson graph UI. **No shared Practice engine/contract file was modified in V11.**

## Preserved baseline and honest gates

- All **132/132** canonical lesson Rich V2 candidates remain present and uniquely matched to the lesson blueprint. Seven-unit distribution **16, 26, 10, 18, 21, 17, 24**.
- V10 evidence ledger initially showed **A_ACCEPTED=0/132, B_ACCEPTED=0/132**, also **C_VERIFIED=0/132, D_VERIFIED=0/132**. V11 does **not** promote any of these counters.
- All 132 had authored prose mentioning a rubric. This is not the same as an executed, approved teacher assignment.
- Existing V10 source activity link counts were 76 explicit student URLs and 132 teacher preparation URLs; mere presence of these links does not prove source-task alignment.

## Source-specific V11 implementation

1. **U06:L11 — Graphing from the Factored Form.** Official teacher goal is to graph quadratics in factored form and explain vertex/vertical intercept; student tasks compare curves and use graphs to verify predictions. First-party function `g(x)=-2(x-1)(x-5)` is now compared with `h(x)=2(x-1)(x-5)`. Shared zeros `1,5`, opposite vertices `(3,8),(3,-8)` and opposite y-intercepts `(0,-10),(0,10)` are checked. An original accessible SVG has both curves and labeled points. Learner is still required to plot their own comparison and justify it; the prepared figure is not marked as a completed student activity.
   - Teacher: https://im.kendallhunt.com/HS/teachers/1/6/11/preparation.html
   - Student: https://im.kendallhunt.com/HS/students/1/6/11/index.html
2. **U06:L16 — Graphing from the Vertex Form.** Official teacher goal includes graph with vertex and y-intercept. Our original `g(x)=-0.5(x+2)^2+6` and `q(x)=0.5(x+2)^2+6` share vertex `(-2,6)`, with y-intercepts `(0,4)` and `(0,8)`; opposite maximum/minimum. The static accessible SVG shows both. In accordance with the publisher preparation, teacher rubric does **not** grade a formal proof of coefficient-sign reasoning, although it assesses the correct graph and extrema.
   - Teacher: https://im.kendallhunt.com/HS/teachers/1/6/16/preparation.html
   - Student: https://im.kendallhunt.com/HS/students/1/6/16/index.html
3. **U07:L24 — Culminating Modeling.** Publisher lesson has substantial independent modeling and parabola–line intersection problems, optionally with group presentations/gallery walk. Our original examples `H(t)=-t^2+6t+2`, `P(t)=t+6` preserve those learning goals without copying figures; exact height intersections `(1,7)` and `(4,10)`, maximum `(3,11)`, and positive landing time `3+sqrt(11)` are checked. Teacher draft rubric explicitly demands independent problem selection, mathematical verification, physical domain, and a non-collision caveat.
   - Teacher: https://im.kendallhunt.com/HS/teachers/1/7/24/preparation.html
   - Student: https://im.kendallhunt.com/HS/students/1/7/24/index.html

## Delivery and testing scope

- Graph SVG generator: `tools/curriculum/us-ccss-alg1-coder5-v11-visuals.cjs`, output assets in `src/Edulytics.Web/wwwroot/images/curriculum/alg1-v11/`.
- Exact-code course partial: `src/Edulytics.Web/Views/Shared/_UsCcssAlgebra1GraphGallery.cshtml`. Minimal allowlisted invocation in `_RichLessonContentV2.cshtml` affects only these three Algebra 1 lesson codes. **Supervisor 2/shared-view code-owner review required** prior to merging.
- Three source-comparison teacher rubric candidates: `docs/curriculum/US_CCSS_ALG1_CODER5_V11_TEACHER_RUBRIC_CANDIDATES_20261029.json`. Each has four human-scored criteria (0–2 points; eight total) and explicitly enumerated authentication, teacher assignment, school isolation, grading, feedback and persistence requirements. **No actual student submission or teacher score is represented as having occurred**.
- All-course draft task/rubric inventory: `docs/curriculum/US_CCSS_ALG1_CODER5_V11_ALL_TEACHER_TASK_INVENTORY_20261029.json`. It extracts existing authored narrative material for all **132** lessons, while only the three above have structured/source-spot-checked rubrics; other **129** need individual teacher/source verification. This does not constitute a completed learning management/gradebook workflow.
- Test suite: `tests/Edulytics.Tests/MathematicsIntelligence/UsCcssAlgebra1V11GraphAndTeacherRubricTests.cs` verifies exact modeled points, real SVG paths, accessible SVG metadata, narrow view allowlist, source-specific task language and that 132/132 lesson identities/rubrics remain present without fake grades.
- All V11 differences must pass `git diff --check`, the source/identity sidecar reconciler, focused .NET regression, and `Edulytics.Web` Razor compilation before push. Hosted exact-SHA full CI remains separate.

## Stage A–D decisions (no release approval)

| Gate | Accepted / 132 | V11 candidate evidence, not academic acceptance |
| --- | ---: | --- |
| A source and academic | 0 | 3 direct teacher/student source comparisons; 129 still require manual review; all need final independent acceptance |
| B effective student delivery | 0 | 3 first-party SVG graphs and guarded Razor partial; no authenticated learner browser/DB evidence |
| C exact Practice / grading | 0 | 3 structured human-scoring rubric drafts, all-course narrative inventory; no verified authorized teacher score/feedback/persistence |
| D two-school tenant/HTTPS | 0 | Requires disposable school/course/class fixture and authenticated wrong-course/wrong-school rejection tests under issue #453 |

Dependencies: Supervisor 2 shared Rich V2 CI test fix [PR #598](https://github.com/EIAS79/Edulytics/pull/598), [high-school adoption issue #453](https://github.com/EIAS79/Edulytics/issues/453), source-specific Practice contract implementation and teacher-scored submissions with audit trail. **Draft PR #589 / NO_GO. No main merge, production deployment, student record writes or simulated UAT signoff.**
