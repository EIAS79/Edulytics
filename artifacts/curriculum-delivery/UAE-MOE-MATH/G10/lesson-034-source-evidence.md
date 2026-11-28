# G10-U04-L01 «تمثيل الدوال التربيعية بيانيًا» — source-to-app Stage A/B/C academic evidence

**Owner-approved canonical ordinal 34 / 148**; unit 4 «الدوال والمعادلات التربيعية». Unique `LessonCode` `PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U04:L01:C034`.

**Original authoritative source:** `01متقدم_.pdf`, UAE Grade10 Advanced Arabic **2025–2026 Volume1**, original Unit2 Lesson2-1, **physical PDF112–119 / printed95–102**. Related technology lab (linear vs quadratic data modeling) is **PDF121 / printed104**. **PDF120 / printed103** is a separate general cumulative test and deliberately excluded from the lesson. SHA256 `0a554589f4cc72537007f3229098c1ab46775d7bf2301bae59f38fe6861a9815`. All new student content and graph data are original Edulytics authorship.

## Stage A — 23 individual curricular source goal/exercise types

| Original element | PDF page | Mathematical skill/graphic/assessment | Edulytics evidence | Status |
|---|---:|---|---|---|
| `G10-ADV-U02L01-E01` | 112 | definition quadratic function f(x)=ax²+bx+c (a nonzero) and graph parabola | RichV2 original Arabic explanation/key concepts | `CONTENT_AUTHORED` |
| `G10-ADV-U02L01-E02` | 112 | graph construction from an x/y table for a quadratic function | RichV2 CartesianPlot 1 and symmetric rows | `CONTENT_AUTHORED` |
| `G10-ADV-U02L01-E03` | 113 | x-intercepts and y-intercept, vertex and parabola axis from graph | RichV2 KeyConcepts intercepts / axis / vertex and original graph | `CONTENT_AUTHORED` |
| `G10-ADV-U02L01-E04` | 113 | axis of symmetry formula x=-b/(2a) for general quadratic | RichV2 numeric vertex and symmetry proof | `CONTENT_AUTHORED` |
| `G10-ADV-U02L01-E05` | 113 | vertex formula, point symmetry, intercepts and plotted scale | three original CartesianPlots and diagram rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-ADV-U02L01-E06` | 114 | find vertex and axis for non-trivial quadratic using its coefficients | RichV2 original graph example d(x)=x²+4x−3 | `CONTENT_AUTHORED` |
| `G10-ADV-U02L01-E07` | 114 | compute exact horizontal vertex coordinate x=-b/(2a) | Grade10 exact vertex_x digital source-aligned family | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-ADV-U02L01-E08` | 114 | evaluate selected x points for the table and curve | Grade10 value_at_x digital family, independent verifier | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-ADV-U02L01-E09` | 115 | opening direction from sign of a, upward/downward parabolas | RichV2 direction rules and graph visual | `CONTENT_AUTHORED` |
| `G10-ADV-U02L01-E10` | 115 | maximum or minimum at vertex, range for real-number domain | RichV2 maximum/minimum and range rules | `CONTENT_AUTHORED` |
| `G10-ADV-U02L01-E11` | 115 | exact vertical vertex value and max/min from coefficients | Grade10 vertex_y digital family; true context teacher reviewed | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-ADV-U02L01-E12` | 115 | classify opening direction by leading coefficient sign | Grade10 opens_up digital family | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-ADV-U02L01-E13` | 116 | contextual quadratic with feasible domain and optimal income/profit | RichV2 original first-party profit model and teacher rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-ADV-U02L01-E14` | 116 | compute vertex value and verify original algebraic function | RichV2 profit maximum original proof | `CONTENT_AUTHORED` |
| `G10-ADV-U02L01-E15` | 117 | mixed practice identify coefficients, symmetry axis and vertex of multiple quadratics | RichV2 twelve worked examples + exact family blueprint, StageD not run | `CONTENT_AUTHORED` |
| `G10-ADV-U02L01-E16` | 117 | check range, min/max and sign of opening for several original functions | RichV2 KeyConcepts and independent original checks | `CONTENT_AUTHORED` |
| `G10-ADV-U02L01-E17` | 118 | complete original parabola plot with labelled axes and vertex | RichV2 three CartesianPlot datasets; teacher rubric for rendering/scale | `TEACHER_REVIEW_REQUIRED` |
| `G10-ADV-U02L01-E18` | 118 | interpret quadratic vertex within domain restrictions and meaningful units | RichV2 original contextual gain / area examples, rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-ADV-U02L01-E19` | 119 | explain how two quadratic graphs share or differ in vertices, intercepts, opening | RichV2 original comparison graph and CommonMistakes | `TEACHER_REVIEW_REQUIRED` |
| `G10-ADV-U02L01-E20` | 119 | identify incorrect axis formula and verify by symmetric points | RichV2 worked example error detection + teacher rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-ADV-U02L01-E21` | 121 | related technology lab compares linear and quadratic regression on measured data | teacher-rubric lab; explanation explicitly notes regression and graphing UI not auto-graded | `TEACHER_REVIEW_REQUIRED` |
| `G10-ADV-U02L01-E22` | 121 | use a time/measurement data table to evaluate regression model visually | teacher-rubric first-party data table activity | `TEACHER_REVIEW_REQUIRED` |
| `G10-ADV-U02L01-E23` | 121 | interpret real-world prediction from quadratic regression and test if plausible | teacher-rubric extrapolation/units and original data reasoning | `TEACHER_REVIEW_REQUIRED` |

**Denominator: 23 of 23 identified source educational/assessment categories**, coverage `{"CONTENT_AUTHORED":10,"TEACHER_REVIEW_REQUIRED":9,"AUTHORED_TEST_DIGITAL_SCORING_PENDING":4}`. Individual copyrighted textbook exercise statements or original figures were not copied; the source's rendering/proof/regression objectives remain separate human tasks.

## Stage B — Arabic original quadratic graphs

- Core Arabic content in `src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json` and `src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u04-l01-ar.rich-lesson-v2.json`. **12 original worked examples, 6 original diagram models including 3 real `CartesianPlot` point datasets.**
- Exact symbolic identities for quadratic `f(x)=ax²+bx+c`, `a≠0`: axis `x=−b/(2a)`, vertex `(h,f(h))`; `a>0` minimum and `a<0` maximum. Example `f(x)=x²−4x+3` has axis `x=2`, vertex `(2,−1)`, and x-intercepts `1,3`; numerical independent symmetry check `f(x)=f(4−x)`.
- Original second plot `f(x)=−2x²+8x−3` has maximum `(2,5)` and symmetric points `(1,3)`, `(3,3)`.
- The source's applied-profit and graph-construction tasks use the human-assessed `artifacts/curriculum-delivery/UAE-MOE-MATH/G10/lesson-034-teacher-rubric.md`, not a hidden automated drawing evaluation. Original technology lab PDF121 compares linear/quadratic regression of data and is **human-scored**; the exact numeric generator does not claim a regression model fitting feature.

## Stage C — bounded exact skill and Stage D deferred handoff

- Primary: `algebra.quadratic.graph_features`, question families `uae.g10.general.u04.l01.vertex_x`, `uae.g10.general.u04.l01.vertex_y`, `uae.g10.general.u04.l01.opens_up`, `uae.g10.general.u04.l01.value_at_x`.
- New bounded first-party exact generator `BuildG10QuadraticGraph` constructs `a(x−h)²+k` with a nonzero, integer h/k, and independently checks x/y vertex, opening sign and value of a specified point from the raw standard-form coefficients. Arabic-Indic and Western digit inputs, wrong answers and invalid parameter shapes are covered by test **source**, not tested yet.
- `IntegerKeypad` for exact integer response. Hand-drawn graph shapes, labels, arbitrary non-integer vertex/intercepts, domain/range arguments and data-regression lab remain `TEACHER_REVIEW_REQUIRED`.
- Source and skill contract `artifacts/curriculum-delivery/UAE-MOE-MATH/G10/lesson-034-stage-c-exact-contract.json`. **Tests WRITTEN_NOT_RUN:** `tests/academic-uae-grade10-arabic-u04l01.test.cjs`, `tests/Edulytics.Tests/MathematicsIntelligence/UaeG10GeneralQuadraticGraphTests.cs`. `productionRouting=false` and `practiceReadiness!=READY_VERIFIED` until Stage D.

**Lesson-only exit:** source types traced and covered, original Arabic 12 examples and Cartesian models, bounded independent exact question generation and graders, handwritten/teacher boundaries and not-run tests. `READY_FOR_STAGE_D` means A/B/C authored for **this lesson**, not production publish.
