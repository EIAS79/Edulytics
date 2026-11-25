# Grade10 U03L05 — original rubric for matrix operations

**Source:** Advanced Arabic 2025–2026 Vol1 PDF57–63 «المصفوفات» + PDF64–71 «العمليات على المصفوفات». Both are represented in canonical one General lesson «العمليات على المصفوفات»; PDF72 starts next independent source lesson matrix multiplication.

| Skill | 2 — secure | 1 — partial | 0 — incorrect |
|---|---|---|---|
| Dimensions / indexing | Correct row×column rank and a_ij orientation | One indexing slip | Reverses every dimension or index |
| Matrix equality | Checks rank and all corresponding entries | One entry overlooked | Claims different ranks equal |
| Addition and subtraction | Same-rank check plus all correct elementwise operations | One arithmetic sign slip | Uses incompatible sizes or mixed indices |
| Scalar multiplication | Multiplies every matrix cell and preserves dimensions | One cell error | Multiplies only one row |
| Properties | Proves 2(A+B)=2A+2B or A+0=A using all entries | Correct result, weak explanation | Falsely assumes subtraction is commutative |
| Data interpretation | Aligns periods/products and uses correct units | One table labeling issue | Adds unrelated quantities or swaps row labels |

Original check: A=[1,2;−3,4], B=[5,−2;0,7]. A+B=[6,0;−3,11]; A−B=[−4,4;−3,−3]; −2A=[−2,−4;6,−8]. Graphical tables, full 2×3 matrices, equality equations and contextual reasoning **TEACHER_REVIEW_REQUIRED**. Machine scoring only 2×2 **one target entry** of sum/difference/scalar, exact integers, tests WRITTEN_NOT_RUN, productionRouting=false.
