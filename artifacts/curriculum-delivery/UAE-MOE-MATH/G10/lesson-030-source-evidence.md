# Grade10 G10-U03-L06 «ضرب المصفوفات» — A/B/C source evidence

**Approved source of truth:** canonical ordinal30/148, unit3/18. Original owner Advanced Arabic `01متقدم_.pdf` SHA256 `0a554589f4cc72537007f3229098c1ab46775d7bf2301bae59f38fe6861a9815`, PDF72–79 printed55–62 + technology lab PDF80–81; next original lesson begins PDF82. **Not the previous lesson's scalar multiplication.**

## Stage A — 26 original mathematical source element types

| Item | PDF | Printed | Source educational target | First-party student/scoring target | Status |
|---|---:|---:|---|---|---|
| `G10-U03L06-E01` | 72 | 55 | matrix product is row-by-column sum, not scalar or elementwise product | RichV2 introduction and inner product rule | `CONTENT_AUTHORED` |
| `G10-U03L06-E02` | 72 | 55 | inner matrix dimension compatibility and resulting outer dimensions | RichV2 shape key concept and exact dimension family | `CONTENT_AUTHORED` |
| `G10-U03L06-E03` | 72 | 55 | row-column correspondence in illustrated 2x2 matrices | RichV2 original four-cell grid | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L06-E04` | 73 | 56 | matrix product of 2×2 arrays | RichV2 AB complete worked example | `CONTENT_AUTHORED` |
| `G10-U03L06-E05` | 73 | 56 | one cell of product as sum of two products | new exact matrix_product_entry | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-U03L06-E06` | 73 | 56 | multiply row i by column j retaining order | RichV2 four-cell original CalculationChain | `CONTENT_AUTHORED` |
| `G10-U03L06-E07` | 74 | 57 | sports/statistics table weighted calculations | RichV2 original price/quantity matrix context | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L06-E08` | 74 | 57 | non-square matrix product 2×3 times 3×2 | RichV2 worked complete non-square multiplication | `CONTENT_AUTHORED` |
| `G10-U03L06-E09` | 74 | 57 | determine when A×B defined but B×A undefined | RichV2 dimension example and practice | `CONTENT_AUTHORED` |
| `G10-U03L06-E10` | 75 | 58 | triple inner products in 2×3 by 3×2 product | RichV2 2×3 by 3×2 full worked example | `CONTENT_AUTHORED` |
| `G10-U03L06-E11` | 75 | 58 | identify valid and invalid matrix-product dimensions | exact matrix_product_compatible; StageD not run | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-U03L06-E12` | 75 | 58 | write the rank of result correctly when dimensions match | RichV2 dimension rule, teacher rationale | `CONTENT_AUTHORED` |
| `G10-U03L06-E13` | 76 | 59 | matrix multiplication not commutative generally | RichV2 AB != BA original example | `CONTENT_AUTHORED` |
| `G10-U03L06-E14` | 76 | 59 | calculate BA and compare with AB | RichV2 reversed-order exact 2×2 example | `CONTENT_AUTHORED` |
| `G10-U03L06-E15` | 76 | 59 | multiply by identity matrix when dimension valid | RichV2 identity example | `CONTENT_AUTHORED` |
| `G10-U03L06-E16` | 77 | 60 | matrix product to compute inventory, sales or prices | RichV2 original matrix-vector example and data rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L06-E17` | 77 | 60 | use associative and distributive laws with matched ranks | RichV2 original A(B+C) demonstration | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L06-E18` | 77 | 60 | distinguish scalar multiplication from row-column product | RichV2 common mistakes and comparison | `CONTENT_AUTHORED` |
| `G10-U03L06-E19` | 78 | 61 | compute complete product with signed entries | RichV2 12 source equivalent examples + rubric | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-U03L06-E20` | 78 | 61 | interpret product with table data and units | RichV2 original matrix-vector data task | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L06-E21` | 78 | 61 | identify common wrong elementwise multiplication answer | RichV2 original error-diagnosis example | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L06-E22` | 79 | 62 | source end-of-lesson multi-dimensional products and powers | RichV2 product of 2x2 and 2x3, A² example | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-U03L06-E23` | 79 | 62 | geometric transformation matrix interpretation | RichV2 original matrix-vector geometry extension | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L06-E24` | 79 | 62 | argue why AB differs from BA | RichV2 compare two products, rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L06-E25` | 80 | 63 | calculator matrix product and repeated multiplication | RichV2 calculator/check tips + teacher rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L06-E26` | 81 | 64 | calculator output verification versus hand multiplication | RichV2 independent cell check and teacher rubric | `TEACHER_REVIEW_REQUIRED` |

All 26 learning/assessment categories are mapped. Original textbook diagrams, exact copyrighted problems are not reproduced. Image-based calculator tech exercises and full large arrays remain human reviewed.

## Stage B — original Arabic teaching and diagrams

- RichV2 `src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u03-l06-ar.rich-lesson-v2.json` provides **12** worked examples and **6** original visuals: product 2×2, 2×3×3×2, row×column, AB vs BA, inner dimensions, identity and transformation contexts.
- Golden answer A=[1,2;3,4], B=[5,−1;2,0] gives AB=[9,−1;23,−3], BA=[2,6;2,4], proving AB≠BA. Full original geometric/technology activities use `artifacts/curriculum-delivery/UAE-MOE-MATH/G10/lesson-030-teacher-rubric.md`.

## Stage C — exact bounded QuestionBank contracts

- Primary skill `algebra.matrices.multiply`. Two allowed exact families `uae.g10.general.u03.l06.matrix_product_entry`, `uae.g10.general.u03.l06.matrix_product_compatible`. One grades a selected entry of product of 2×2 by 2×2, computing row×column independently from original source parameters. The second grades whether dimensions 1..3 are compatible. IntegerKeypad input and independent verifier check localized digits.
- Previous U03L05 elementwise/scalar families are expressly forbidden. Full 2×3 products, matrix-vector geometry, calculator exercise and written proof are teacher-scored.
- Stage D and CI NOT_STARTED; tests authored at `tests/academic-uae-grade10-arabic-u03l06.test.cjs`, `tests/Edulytics.Tests/MathematicsIntelligence/UaeG10GeneralMatrixProductTests.cs` are `WRITTEN_NOT_RUN`; `productionRouting=false`, `practiceReadiness` NOT READY_VERIFIED.

**Exit:** READY_FOR_STAGE_D means Stage A–B–C authoring handoff only, not tested/published curriculum.
