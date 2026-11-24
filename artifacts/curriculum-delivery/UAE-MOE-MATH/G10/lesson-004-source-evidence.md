# Grade 10 General — G10-U01-L04 Stage A–B–C source-to-app closure evidence

**Canonical unit:** «كثيرات الحدود والتحليل». **Lesson:** «نواتج الضرب الخاصة». **LessonCode:** `PED:UAE-MOE-MATH:L10:GENERAL:01:04:SPECIAL-PRODUCTS`.

**Original source:** Owner-provided Arabic `01.pdf`, Grade10 General 2025–2026 Volume1, physical PDF pages **41–46** / printed pages **28–33**, SHA-256 `d7e492a960675c30ac7eebb0144ed2dc51d1f4e9c91c9834ea05638f614a3c72`. This is a source-books audit of identifiable teaching goals and assessment types; copyrighted publisher wording and figures are not embedded. The canonical 18-unit/148-lesson index was owner approved. This is lesson-specific completion, not full-grade publication or Stage D validation.

## A: Visual-source elements — individual goal and assessment mapping

| Source element | Physical page | Printed | Category | Source mathematical objective / exercise type | Original Edulytics destination | Status |
|---|---:|---:|---|---|---|---|
| `G10-GEN-2025-U01L04-E01` | 41 | 28 | concept | square of a binomial with plus sign; identify cross terms | RichV2 KeyConcepts square of sum | `CONTENT_AUTHORED` |
| `G10-GEN-2025-U01L04-E02` | 41 | 28 | visual | algebra-tile model for square of a sum | RichV2 original ExpressionStripBars four areas | `TEACHER_REVIEW_REQUIRED` |
| `G10-GEN-2025-U01L04-E03` | 41 | 28 | worked_example | use (a+b)²=a²+2ab+b² with coefficients | RichV2 (س+٤)² and (٢س+٣)² examples | `CONTENT_AUTHORED` |
| `G10-GEN-2025-U01L04-E04` | 42 | 29 | concept | square of a difference with negative middle term and positive last term | RichV2 KeyConcepts square of difference | `CONTENT_AUTHORED` |
| `G10-GEN-2025-U01L04-E05` | 42 | 29 | worked_example | signed binomial square including nonunity coefficient | RichV2 (٢س−٣)² and (٤س−١)² | `CONTENT_AUTHORED` |
| `G10-GEN-2025-U01L04-E06` | 42 | 29 | assessment | compute coefficient from square of a binomial | Existing Grade9 exact coefficient family; StageD pending | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-GEN-2025-U01L04-E07` | 42 | 29 | visual | difference-square area/algebra tiles and explain sign | Original model plus teacher-specified sign partition rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-GEN-2025-U01L04-E08` | 43 | 30 | concept | product of conjugates cancels the two equal cross terms | RichV2 KeyConcepts conjugate products | `CONTENT_AUTHORED` |
| `G10-GEN-2025-U01L04-E09` | 43 | 30 | worked_example | multiply two conjugate binomials | RichV2 (٣س+٢)(٣س−٢) and (س+٥)(س−٥) | `CONTENT_AUTHORED` |
| `G10-GEN-2025-U01L04-E10` | 43 | 30 | assessment | compute a signed product of conjugates as an exact quadratic | New Grade10 full special-product exact grader; StageD pending | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-GEN-2025-U01L04-E11` | 43 | 30 | visual | geometric interpretation of two squares with different areas | RichV2 rectangle and teacher rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-GEN-2025-U01L04-E12` | 44 | 31 | assessment | choose square sum vs square difference vs conjugate family | RichV2 original compare example; exact family condition encoded | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-GEN-2025-U01L04-E13` | 44 | 31 | assessment | simplify and check multiple special products with coefficients | RichV2 9 original worked examples; test templates | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-GEN-2025-U01L04-E14` | 44 | 31 | application | area partition with variable side lengths | RichV2 original square area decomposition and teacher rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-GEN-2025-U01L04-E15` | 45 | 32 | application | compare geometric ring/removed area and difference of squares | Teacher rubric + conjugate area reasoning | `TEACHER_REVIEW_REQUIRED` |
| `G10-GEN-2025-U01L04-E16` | 45 | 32 | reasoning | justify disappearance of middle term in conjugates | RichV2 CalculationChain positive/negative cross-term cancellation | `TEACHER_REVIEW_REQUIRED` |
| `G10-GEN-2025-U01L04-E17` | 45 | 32 | assessment | diagnose missing middle term in square of sum | RichV2 worked error (س+٣)² falsely lacking 6س | `TEACHER_REVIEW_REQUIRED` |
| `G10-GEN-2025-U01L04-E18` | 46 | 33 | application | apply special products to measured areas and contexts | RichV2 area/model with units plus teacher rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-GEN-2025-U01L04-E19` | 46 | 33 | assessment | reflect on numeric substitution to verify identities independently | RichV2 original numerical check in each worked example | `CONTENT_AUTHORED` |
| `G10-GEN-2025-U01L04-E20` | 46 | 33 | writing | explain distinct formula patterns and identify when shortcut applies | RichV2 SummaryPoints + teacher rubric | `TEACHER_REVIEW_REQUIRED` |

**Denominator:** 20 identified source element/exercise categories: 7 directly authored, 4 digital Stage-C questions/tests authored awaiting execution, and 9 teacher-scored original drawing, modeling, and reasoning exercises. This does **not** assert that every individual original numbered exercise has an identical replacement or that source images can be published.

## B: Arabic original authored lesson

- Canonical `ar` lesson under the original PED identity in `src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json`. The old English fallback is preserved until a whole-course AcademicLanguage cutover.
- `src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u01-l04-ar.rich-lesson-v2.json`: **9 worked examples** with original checks and **5 visual models**, including a four-part square-area diagram, symbolic expansion chains, and cross-term cancellation in conjugates.
- Mathematical targets distinguished: `(أ+ب)²=أ²+٢أب+ب²`, `(أ−ب)²=أ²−٢أب+ب²` and `(أ+ب)(أ−ب)=أ²−ب²`.
- Independent numerical spot checks included: at `س=٢`, `(٢س−٣)²=١` and `(٣س+٢)(٣س−٢)=٣٢`.
- Author-created teacher assessment: `artifacts/curriculum-delivery/UAE-MOE-MATH/G10/lesson-004-teacher-rubric.md`; geometric diagrams, tile manipulation, area units and proof are **not** represented as automatically scored questions.

## C: Exact skill/renderer and deferred Stage D

- Primary skill: `algebra.polynomial.special_products`.
- Existing correct Grade9 single-coefficient families reused: `uae.g9.general.u07.l04.square_binomial_coefficient` and `uae.g9.general.u07.l04.conjugate_product_coefficient`, both with numeric input. They do **not** claim full-expression scoring.
- Authored new full-expression `uae.g10.general.u01.l04.full_special_product` using source-bound cases of squared sum, squared difference and conjugates. It uses the prior bounded independent product convolution verifier rather than a new unrestricted algebra engine. Its UI contract is `StructuredEntry`.
- Tests written but **NOT RUN** during A–B–C: `tests/academic-uae-grade10-arabic-u01l04.test.cjs` and `tests/Edulytics.Tests/MathematicsIntelligence/UaeG10GeneralSpecialProductsTests.cs`.
- Practice mapping stays `PARTIAL_REUSE_ONLY_NOT_READY_FOR_STAGE_D`, `productionRouting=false`; no CI, authenticated student rendering, merge, or production deployment has been claimed.
- Source-scoped exact contract: `artifacts/curriculum-delivery/UAE-MOE-MATH/G10/lesson-004-stage-c-exact-contract.json`.

**Checkpoint interpretation:** when recorded as `READY_FOR_STAGE_D`, it refers *only* to this lesson's Stage A–B–C authoring and evidence. The source book's later lesson `G10-U01-L08` on difference-of-squares factorization is not merged into U01L04; here the identity is used in its forward multiplication direction.
