# G10-U03-L08 «حل أنظمة المعادلات باستخدام المصفوفات العكسية» — Stage A–B–C source evidence

**Canonical ordinal 32/148**; unique General lesson code `PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U03:L08:C032`; canonical unit «الأنظمة الخطية والمصفوفات». The previous lesson G10-U03-L07 «قاعدة كرامر والمحددات» was closed before preparing this lesson.

**Original authoritative textbook:** owner-provided `01متقدم_.pdf`, 2025–2026 Advanced Arabic Volume1 original Unit1 Lesson1-9, PDF **91–97** / printed **74–80**, SHA-256 `0a554589f4cc72537007f3229098c1ab46775d7bf2301bae59f38fe6861a9815`. This source is merged into the **General-only** 148-lesson canon; there is no separate Advanced pathway.

## Stage A — independently sourced goals and assessment types

| Element | Original PDF page | Source mathematical goal or exercise type | Edulytics evidence location | Coverage |
|---|---:|---|---|---|
| `G10-U03L08-E01` | 91 | matrix identity I and inverse product conditions | RichV2 original identity and inverse definitions | `CONTENT_AUTHORED` |
| `G10-U03L08-E02` | 91 | matrix invertible only if determinant nonzero | RichV2 inverse condition and zero example | `CONTENT_AUTHORED` |
| `G10-U03L08-E03` | 91 | matrix equation applied to meal/price bundles | RichV2 original shop quantities and human rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L08-E04` | 92 | multiply proposed inverse both ways to verify identity | RichV2 two-sided check example | `CONTENT_AUTHORED` |
| `G10-U03L08-E05` | 92 | formula for inverse of 2x2 using determinant | RichV2 determinant, adjugate and fraction example | `CONTENT_AUTHORED` |
| `G10-U03L08-E06` | 92 | determine whether a matrix is invertible | new inverse2_exists exact numeric family | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-U03L08-E07` | 92 | invertible 2x2 matrix with rational entries | RichV2 exact fraction example | `CONTENT_AUTHORED` |
| `G10-U03L08-E08` | 93 | correct reversal of diagonal and sign of off-diagonal | RichV2 inverse calculation chain | `CONTENT_AUTHORED` |
| `G10-U03L08-E09` | 93 | original 2x2 inverse exact rational result | RichV2 example [1,2;3,4] inverse | `CONTENT_AUTHORED` |
| `G10-U03L08-E10` | 93 | one exact rational entry of matrix inverse | new inverse2_entry exact family with BigInteger verifier | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-U03L08-E11` | 94 | represent two equations as AX=B | RichV2 AX=B explanation and original system example | `CONTENT_AUTHORED` |
| `G10-U03L08-E12` | 94 | solve X=A^-1 B by left multiplication in correct order | RichV2 linear system using 2x2 inverse | `CONTENT_AUTHORED` |
| `G10-U03L08-E13` | 94 | recover solution of 2x2 system with fractions in inverse | RichV2 equation x+2y=5;3x+4y=11 | `CONTENT_AUTHORED` |
| `G10-U03L08-E14` | 94 | why B A^-1 not valid general solution of AX=B | RichV2 CommonMistakes and teacher rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L08-E15` | 95 | real meal deals, bundle charges, ticket count matrix methods | RichV2 original shop example | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L08-E16` | 95 | solve 3x3 systems by matrix inversion only when determinant nonzero | RichV2 inverse3 concept, teacher scoring | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L08-E17` | 95 | use 2x2 inverse to solve different numeric systems | RichV2 multiple original worked examples | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-U03L08-E18` | 95 | interpret negative or fractional answers as quantities | RichV2 original check plus teacher rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L08-E19` | 96 | evaluate system with singular coefficient matrix | RichV2 no-inverse and dependent system examples | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-U03L08-E20` | 96 | interpret inverse in graphics and transformation | RichV2 geometry/identity original models | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L08-E21` | 96 | check solution by verifying both original equations | RichV2 explicit substitution checks | `CONTENT_AUTHORED` |
| `G10-U03L08-E22` | 97 | mixed cumulative standard tests on inverse systems | RichV2 source-equivalent original problems, rubric | `TEACHER_REVIEW_REQUIRED` |
| `G10-U03L08-E23` | 97 | explain inverse method and when not to use it | RichV2 summary and teacher rubric | `TEACHER_REVIEW_REQUIRED` |

**Audited 23/23 educational-element categories**, split {"CONTENT_AUTHORED":11,"TEACHER_REVIEW_REQUIRED":8,"AUTHORED_TEST_DIGITAL_SCORING_PENDING":4}. This audit establishes coverage of the identified source teaching/evaluation types, **not** a claim of copied publisher exercises, all human activities auto-graded, or successful runtime tests.

## Stage B — original Arabic content and examples

- Original Arabic explanation with **12 worked examples** and **6 visual models** in `src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u03-l08-ar.rich-lesson-v2.json` and its `ar` translation in the course content pack.
- Correct 2×2 inverse requires nonzero determinant `D=ad−bc`, followed by `A^{-1}=1/D [d,−b;−c,a]`. The original source also teaches 3×3 inverses, representing `AX=B`, singular systems and contextual interpretation; these stay in Arabic examples/teacher rubric and are **not claimed digitally auto-graded**.
- The original source PDF98 starts a distinct **technology exploration of augmented matrices**. It is reserved for the approved next canonical lesson 33 to avoid double counting.
- Teacher assessment: `artifacts/curriculum-delivery/UAE-MOE-MATH/G10/lesson-032-teacher-rubric.md` for matrix dimensions, 3×3 proof, singular cases and complete contextual modeling.

## Stage C — exact contract and safe test handoff

- Primary `algebra.matrices.inverse`, mapped to exact family `uae.g10.general.u03.l08.inverse2_entry` (requested rational 2×2 inverse entry, `StructuredEntry`) and `uae.g10.general.u03.l08.inverse2_exists` (0/1 invertibility decision, `IntegerKeypad`).
- `UaeG10InverseMatrixResponse.Verify` independently checks `D≠0`, entry numerator and exact rational equality by `BigInteger` cross-multiplication, instead of trusting the generated answer. Arabic-Indic digits and improper/negative fractions are handled by the bounded parser; the primary reference does not promise an unrestricted symbolic CAS.
- Exact tests are **written but not run**: `tests/Edulytics.Tests/MathematicsIntelligence/UaeG10GeneralInverseMatrixTests.cs` and `tests/academic-uae-grade10-arabic-u03l08.test.cjs`.
- `productionRouting=false`, `practiceReadiness=PARTIAL_REUSE_ONLY_NOT_READY_FOR_STAGE_D`; CI, authenticated RTL interface checks and Stage D release gates remain future work under a separate stage, not fake A/B/C failures.

**A–B–C exit:** source atomic categories mapped, first-party Arabic authoring and visuals attached to the unique original canonical identity, exact skill and teacher boundaries documented, tests authored `WRITTEN_NOT_RUN`. Eligible to record `READY_FOR_STAGE_D` for **this lesson alone**.
