# G10-U04-L02 «حل المعادلات التربيعية بالتمثيل البياني» — Stage A/B/C source evidence

**Approved canonical ordinal 35 / 148** in unified Grade10 General Unit4, separate from canonical34 parabola vertex/shape. Unique `LessonCode` `PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U04:L02:C035`.

**Original source:** `01متقدم_.pdf`, 2025–2026 Grade10 Arabic Advanced Vol1 original Unit2 Lesson2-2 PDF **122–129** printed**105–112**, adjacent graphing calculator LAB PDF **130** printed113; next original lesson2-3 begins PDF131. SHA256 `0a554589f4cc72537007f3229098c1ab46775d7bf2301bae59f38fe6861a9815`. All student-facing examples and graph point datasets are Edulytics original; source publisher images/prose not reproduced.

## Stage A: original mathematical goals and exercise-type coverage

| Element ID | PDF page | Source mathematical objective | Authored digital / teacher route | Status |
|---|---:|---|---|---|
| `G10-ADV-U02L02-E01` | 122 | roots of a quadratic as parabola intersections with x-axis | Arabic original explanation, first worked example | `CONTENT_AUTHORED` |
| `G10-ADV-U02L02-E02` | 122 | solve x²-x-6=0 from x-intercept figure | RichV2 original two-root plot and substitution check | `CONTENT_AUTHORED` |
| `G10-ADV-U02L02-E03` | 122 | plot parabola and accurate horizontal intercepts | CartesianPlot1 and teacher graph rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-ADV-U02L02-E04` | 123 | two real distinct vs repeated tangent vs no real crossing | RichV2 discriminant and three source-shaped diagrams | `CONTENT_AUTHORED` |
| `G10-ADV-U02L02-E05` | 123 | read repeated tangency and count only one distinct intersection | RichV2 tangent x²-6x+9 original example | `CONTENT_AUTHORED` |
| `G10-ADV-U02L02-E06` | 123 | exact distinct root count 0/1/2 | Grade10 real_root_count bounded family; tests unrun | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-ADV-U02L02-E07` | 124 | quadratic with discriminant negative and no real solution | RichV2 x²+4x+5 plot with vertex above axis | `CONTENT_AUTHORED` |
| `G10-ADV-U02L02-E08` | 124 | verify no real crossing using integer coefficients | Root count family with no-real generation; StageD pending | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-ADV-U02L02-E09` | 124 | graph/table coordinate precision for root reading | Original plot and human figure rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-ADV-U02L02-E10` | 125 | calculator intersection/zero graph utility for real roots | Original graphing lab discussion, teacher rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-ADV-U02L02-E11` | 125 | approximate irrational root and check f(x) near zero | RichV2 original x²−2 example and decimal accuracy | `CONTENT_AUTHORED` |
| `G10-ADV-U02L02-E12` | 125 | find left real integer intercept of a quadratic graph | New left_intercept exact family | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-ADV-U02L02-E13` | 126 | source visual graphs to identify zeros | Four original coordinate plots and teacher rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-ADV-U02L02-E14` | 126 | decimal estimates from shape and scale | Approximate root reasoning teacher reviewed | `TEACHER_REVIEW_REQUIRED` |
| `G10-ADV-U02L02-E15` | 126 | find right real integer intercept | New right_intercept exact family | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-ADV-U02L02-E16` | 127 | find real zeros from function table | RichV2 x²−8x+15 tabulated visual example | `CONTENT_AUTHORED` |
| `G10-ADV-U02L02-E17` | 127 | distinguish number of zeros from graphic | RichV2 double and zero-root examples | `CONTENT_AUTHORED` |
| `G10-ADV-U02L02-E18` | 127 | source quadratic trajectory and real time roots | RichV2 original height model and domain restriction | `TEACHER_REVIEW_REQUIRED` |
| `G10-ADV-U02L02-E19` | 128 | solve ballistic equation and interpret physical time | RichV2 height at ground positive time and teacher rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-ADV-U02L02-E20` | 128 | contextual graphs with numerical approximation | Teacher context rubric and original examples | `TEACHER_REVIEW_REQUIRED` |
| `G10-ADV-U02L02-E21` | 128 | compare plot estimate with independent substitution | RichV2 error analysis and check | `TEACHER_REVIEW_REQUIRED` |
| `G10-ADV-U02L02-E22` | 129 | transfer diagram intercept into solving equation | RichV2 original reasoning and discriminant comparison | `CONTENT_AUTHORED` |
| `G10-ADV-U02L02-E23` | 129 | mixed polynomial zeros and number of real roots | Three exact families authored, not run | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-ADV-U02L02-E24` | 130 | source lab Y1=x²−8x+15 and Y2=0 finds x=3,5 | RichV2 original graph-lab example and teacher rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-ADV-U02L02-E25` | 130 | source calculator approximate roots to one decimal, twelve tasks | Teacher scoring calculator technology and accuracy | `TEACHER_REVIEW_REQUIRED` |

**Source coverage:** 25/25 reviewed learning-goal, visual, and assessment-type categories; {"CONTENT_AUTHORED":9,"TEACHER_REVIEW_REQUIRED":11,"AUTHORED_TEST_DIGITAL_SCORING_PENDING":5}. This does **not** claim a one-to-one copy of every publisher practice question, and the graphing calculator and irrational roots are human-reviewed, not marked auto-graded.

## Stage B: Arabic original student explanation, graph and examples

- Arabic content pack + `src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u04-l02-ar.rich-lesson-v2.json`: **12** independent worked examples and **4** genuine `CartesianPlot` points datasets (two intersections, tangent one root, no real intersection, and book-lab intersection), plus two symbolic chains.
- Fundamental math: graph solving `ax²+bx+c=0` means *x-coordinate of each crossing with y=0*, not the vertex or y-intercept. Number of **distinct real** crossings determined by discriminant `b²−4ac` sign >0 / =0 / <0.
- Original checked cases: `x²−x−6=0` roots −2 and3; `x²−6x+9=0` repeats3 with one unique tangent; `x²+4x+5=0` has no real root; source lab `x²−8x+15=0` intersects x-axis at3 and5.
- Real context `h(t)=−16t²+64t+80` has algebraic zeros t=−1 and5, but physical time t≥0 requires **5 seconds**, as separately evaluated by a teacher.
- Source original graphing lab asks for decimal approximations to tenths, and freehand graphs; this is **not** claimed covered by bounded integer-only Stage C generator. Human rubric: `artifacts/curriculum-delivery/UAE-MOE-MATH/G10/lesson-035-teacher-rubric.md`.

## Stage C: bounded exact x-intercept families, Stage D deferred

- Primary `algebra.quadratic.solve_by_graphing`. Three new exact families `uae.g10.general.u04.l02.real_root_count`, `uae.g10.general.u04.l02.left_intercept`, `uae.g10.general.u04.l02.right_intercept`. The generator composes source-accurate quadratics with two different integer zeros, one repeated integer zero, or no real zeros; left/right endpoints are only generated where an integer real intercept exists.
- `UaeG10QuadraticGraphRootsResponse.Verify` independently validates original coefficients and discriminant plus endpoint parameters. Responding with text `خطأ` or out-of-bound parameters must fail. Integer input may use Arabic-Indic digits, tested by written .NET/Node Stage D tests.
- `IntegerKeypad` on these exact categories only. Source approximate noninteger zeros, graph drawing and calculator operations remain `TEACHER_REVIEW_REQUIRED`.
- Tests `tests/academic-uae-grade10-arabic-u04l02.test.cjs`, `tests/Edulytics.Tests/MathematicsIntelligence/UaeG10GeneralQuadraticGraphRootsTests.cs`: **WRITTEN_NOT_RUN**; `productionRouting=false`, `practiceReadiness=PARTIAL_REUSE_ONLY_NOT_READY_FOR_STAGE_D`.

**Eligibility:** all Stage A source types, authored Arabic and Cartesian graph illustrations, independently bounded exact question families and teacher rubric documented. Closing A/B/C is a **handoff** to Stage D; it is not a Stage D PASS, CI PASS, live account verification or publication.
