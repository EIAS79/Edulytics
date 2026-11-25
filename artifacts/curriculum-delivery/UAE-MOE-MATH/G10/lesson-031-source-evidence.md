# Grade10 G10-U03-L07 «قاعدة كرامر والمحددات» — A/B/C source evidence

Canonical ordinal **31/148**. Owner original Advanced Arabic 2025–2026 Vol1 `01متقدم_.pdf` SHA256 `0a554589f4cc72537007f3229098c1ab46775d7bf2301bae59f38fe6861a9815`, original Unit1 Lesson1-8 PDF82–90 printed65–73. PDF91 begins inverse matrix lesson1-9, preserved for canonical32; avoid duplicate independent inverses.

## Stage A — 25 textbook educational element and assessment categories

| Element | PDF | Printed | Goal/problem type | Original authored destination | Status |
|---|---:|---:|---|---|---|
| `G10-U03L07-E01` | 82 | 65 | second-order determinant formula ad−bc, including signed entries | RichV2 KeyConcepts & first three examples | `CONTENT_AUTHORED` |
| `G10-U03L07-E02` | 82 | 65 | evaluate determinant2 with negative diagonal products | RichV2 determinant2 source-equivalent golden | `CONTENT_AUTHORED` |
| `G10-U03L07-E03` | 82 | 65 | numeric determinant with arbitrary signed coefficients | new exact determinant2 family not run | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-U03L07-E04` | 83 | 66 | third-order determinant determinant3 and diagonal rule | RichV2 third-order explanation and proof example | `CONTENT_AUTHORED` |
| `G10-U03L07-E05` | 83 | 66 | cofactor expansion +−+ for 3x3 | RichV2 determinant3 key concept and visual | `CONTENT_AUTHORED` |
| `G10-U03L07-E06` | 83 | 66 | evaluate third-order determinant with all signs | RichV2 third-order 3x3 golden sample | `CONTENT_AUTHORED` |
| `G10-U03L07-E07` | 84 | 67 | geometry area and satellite/GPS coordinate reasoning with determinants | RichV2 original triangle-area example, teacher rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L07-E08` | 84 | 67 | apply determinant in geometry problem | RichV2 triangle area source-shaped original | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L07-E09` | 85 | 68 | Cramer's rule for unique linear solution when D!=0 | RichV2 Cramer concept and original examples | `CONTENT_AUTHORED` |
| `G10-U03L07-E10` | 85 | 68 | replace one variable's column by RHS constants | RichV2 D/Dx/Dy grid visual | `CONTENT_AUTHORED` |
| `G10-U03L07-E11` | 85 | 68 | find exact x using Cramer's rule | new exact cramer_x family, tests unrun | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-U03L07-E12` | 85 | 68 | find exact y using Cramer's rule | new exact cramer_y family, tests unrun | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-U03L07-E13` | 86 | 69 | extend Cramer to 3x3 independent system with determinants | RichV2 Cramer3 example using D, Dx, Dy, Dz | `CONTENT_AUTHORED` |
| `G10-U03L07-E14` | 86 | 69 | solve 3-variable linear equation system by Cramer | RichV2 3-variable original check and human rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L07-E15` | 86 | 69 | nonzero determinant implies unique solution | RichV2 determinant criterion | `CONTENT_AUTHORED` |
| `G10-U03L07-E16` | 87 | 70 | identify determinant nonzero vs zero systems | RichV2 zero examples and matrix rank rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L07-E17` | 87 | 70 | distinguish inconsistent system from infinite solutions when determinant=0 | RichV2 pair of zero determinant examples | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L07-E18` | 87 | 70 | coordinates and geometry with signed determinant | RichV2 original triangle area geometric model | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L07-E19` | 88 | 71 | calculate determinant2 and determinant3 for new coefficients | RichV2 examples1–6 and exact 2x2 subset | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-U03L07-E20` | 88 | 71 | Cramer's rule in contextual table/word problem | RichV2 original equations and teacher rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L07-E21` | 89 | 72 | mixed problems solving equations with Cramer and checking | RichV2 original Cramer examples and teacher rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L07-E22` | 89 | 72 | recognize cases with determinant zero | RichV2 no/inf pair and teacher rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L07-E23` | 89 | 72 | explain why variable columns are replaced but not rows | RichV2 Cramer method and CommonMistakes | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L07-E24` | 90 | 73 | end-of-lesson Cramer reasoning and application checks | RichV2 final reflection and teacher rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L07-E25` | 90 | 73 | inverse matrix solution is separate next source lesson 1-9 | RichV2 Cramer / inverse distinction | `CONTENT_AUTHORED` |

Categories reflect distinct learning outcomes and exercise families, not a promise to reproduce copyrighted problems or every numbered practice.

## Stage B — original Arabic teaching

- RichV2 `src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u03-l07-ar.rich-lesson-v2.json` supplies **12** original worked examples and **6** diagrams/chains, including determinant2 formula ad−bc, determinant3 cofactor +−+, Cramer D/Dx/Dy with 2×2 and 3×3 explanations, D=0 dependent versus inconsistent systems and determinant triangle area.
- Golden source-aligned independent example: determinant [4,5;−3,6]=39; system 2x+y=7 and x−y=−1 has D=−3, Dx=−6, Dy=−9, so x=2,y=3. Teacher-reviewed determinant3/geometry/nonunique/word-problem rubric at `artifacts/curriculum-delivery/UAE-MOE-MATH/G10/lesson-031-teacher-rubric.md`.

## Stage C — bounded exact primary skill

- `algebra.matrices.cramer_determinants`; exact families `uae.g10.general.u03.l07.determinant2`, `uae.g10.general.u03.l07.cramer_x`, `uae.g10.general.u03.l07.cramer_y`, `IntegerKeypad`. Independent verifier recalculates D,Dx,Dy and both original equations; refuses D=0, fractional/out-of-range solutions and wrong Arabic/Western digits.
- 3×3 Cramer expansion, zero determinants with free variables, geometry area and original graphic/written reasoning receive **teacher scoring**, not falsely marked digital exact practice. Full inverse matrices belong to lesson32.
- Stage D tests `tests/academic-uae-grade10-arabic-u03l07.test.cjs`, `tests/Edulytics.Tests/MathematicsIntelligence/UaeG10GeneralCramerDeterminantsTests.cs` **WRITTEN_NOT_RUN**; `productionRouting=false`, CI/authenticated RTL student practice/Neon deployment NOT RUN.

**Exit:** this lesson Stage A–B–C content and exact contract `READY_FOR_STAGE_D`, never StageD verified or published.
