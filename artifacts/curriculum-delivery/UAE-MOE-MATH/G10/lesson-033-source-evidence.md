# Grade10 General canonical33 «المصفوفات الموسعة» — A/B/C source-based closure evidence

**Owner-approved title** in 148-lesson / 18-unit General-only index: `المصفوفات الموسعة`. **Preserved unique PED code:** `PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U03:L09:C033`, Unit «الأنظمة الخطية والمصفوفات».

**Precise original source identity:** `01متقدم_.pdf`, original Advanced Arabic 2025–2026 Volume1 **PDF98 / printed81**, SHA `0a554589f4cc72537007f3229098c1ab46775d7bf2301bae59f38fe6861a9815`. Page label: **«مختبر تقنية التمثيل البياني 1-9: المصفوفات الموسعة»**. This is a **standalone technology LAB attached to numbered source Lesson1-9**, not a fictional source Lesson1-10; the owner-approved canonical index treats its unique learning activity as lesson33 and the content explicitly preserves that distinction. PDF91–97 inverse matrices is the earlier canonical32, not duplicated.

## Stage A — atomic source goals and original assessment categories

| Source element | PDF page | Mathematical goal or activity family | Original Edulytics target | Evaluation |
|---|---:|---|---|---|
| `G10-ADV-U01LAB1-9-E01` | 98 | Technology laboratory 1-9, not an independent numbered official lesson | Arabic original explanation explicitly identifies 1-9 augmented-matrices technology lab | `CONTENT_AUTHORED` |
| `G10-ADV-U01LAB1-9-E02` | 98 | graphing calculator used to solve a linear system by an augmented matrix | RichV2 Introduction and teacher rubric for calculator navigation | `CONTENT_AUTHORED` |
| `G10-ADV-U01LAB1-9-E03` | 98 | augment coefficient matrix with extra constants column | RichV2 augmented matrix KeyConcept and first example | `CONTENT_AUTHORED` |
| `G10-ADV-U01LAB1-9-E04` | 98 | translate 2x+y+z=1;3x+2y+3z=12;4x+y+2z=-1 to 3x4 matrix | RichV2 three-variable source-context example and diagram (independent original explanation) | `CONTENT_AUTHORED` |
| `G10-ADV-U01LAB1-9-E05` | 98 | preserve variable column order and all constants including negative -1 | RichV2 three-variable model and CommonMistakes | `CONTENT_AUTHORED` |
| `G10-ADV-U01LAB1-9-E06` | 98 | enter 3x4 matrix with calculator matrix editor and determine RREF | teacher-reviewed calculator task, explanation of RREF and row operations | `TEACHER_REVIEW_REQUIRED` |
| `G10-ADV-U01LAB1-9-E07` | 98 | read RREF output identity coefficient block and final solution (-4,3,6) | RichV2 verified 3x4 RREF example; teacher rubric for full calculation | `TEACHER_REVIEW_REQUIRED` |
| `G10-ADV-U01LAB1-9-E08` | 98 | verify three-variable solution against all three original equations | RichV2 original substitution proof on three equations | `CONTENT_AUTHORED` |
| `G10-ADV-U01LAB1-9-E09` | 98 | book exercises 1-4: convert and solve two-variable systems using augmented matrices | original 2x3 authored example; bounded exact RREF and row-operation assessment; teacher rubric for hand method | `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-ADV-U01LAB1-9-E10` | 98 | book exercises 5-6: augmented 3x4 three-variable systems | teacher rubric for 3x4 setup, calculator use, rank and substitution | `TEACHER_REVIEW_REQUIRED` |

**Source laboratory scope = 10 of 10 distinct educational/assessment categories**, with statuses {"CONTENT_AUTHORED":6,"TEACHER_REVIEW_REQUIRED":3,"AUTHORED_TEST_DIGITAL_SCORING_PENDING":1}. This does **not** mean 10 official numbered textbook lessons, automatic grading of full 3×4 source exercises, nor copying original publisher text or screenshots.

## Stage B — independently authored original Arabic teaching

- RichV2: `src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u03-l09-ar.rich-lesson-v2.json`, contains **10** original worked examples, **5** diagrams, clear matrix column roles, elementary row equivalence, rank/consistency examples, and source's distinct **3×4** system with accurate RREF.
- Original source example 3x3 system `2س+ص+ع=1, 3س+2ص+3ع=12, 4س+ص+2ع=−1`, solution `(س,ص,ع)=(−4,3,6)`. Independently check equations: `1`, `12`, `−1`, respectively.
- The student PDF source exercise set includes both two-variable and three-variable systems. The Edulytics first-party 2×3 exact exercises are **explicitly a bounded practice subset**, rather than a purported replacement for all 3×4 calculator tasks.
- Source three-variable calculator operation and full row reduction are assessed with `artifacts/curriculum-delivery/UAE-MOE-MATH/G10/lesson-033-teacher-rubric.md`, including source matrix entry and verifying an RREF output.

## Stage C — bounded skill contract / downstream activation

- Primary skill `algebra.matrices.augmented_row_reduce`; three exact family contracts: `uae.g10.general.u03.l09.row_operation_entry`, `uae.g10.general.u03.l09.rref_solution_x`, `uae.g10.general.u03.l09.rref_solution_y`.
- Generator `BuildG10AugmentedMatrix` composes 2×2 invertible integer-coefficient systems with augmented 2×3 matrices and only a single specified row-operation entry or integer RREF s/x/y output. Independent verifier recomputes entire row operation or uses original coefficient/constant determinant, never the generated answer text.
- Source full 3×4 use of a graphing calculator and 3-variable written proof are **TEACHER_REVIEW_REQUIRED**, not assessed as 2×3.
- Written Stage D tests `tests/academic-uae-grade10-arabic-u03l09.test.cjs` and `tests/Edulytics.Tests/MathematicsIntelligence/UaeG10GeneralAugmentedMatricesTests.cs`: **WRITTEN_NOT_RUN**; `productionRouting=false`, `practiceReadiness=PARTIAL_REUSE_ONLY_NOT_READY_FOR_STAGE_D`.

**Exit:** the owner-approved canonical33 identity, original source lab provenance, lab categories, original Arabic content, bounded exact question contract, human evaluation and not-run tests are authored and traceable. The A/B/C status is eligible for `READY_FOR_STAGE_D` **for this lesson only**.
