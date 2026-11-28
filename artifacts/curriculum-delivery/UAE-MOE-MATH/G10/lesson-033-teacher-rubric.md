# G10-U03-L09 «المصفوفات الموسعة» — teacher-scored technology lab rubric

**Source** `01متقدم_.pdf`, original Advanced Arabic Grade10 2025–2026 Volume1, **PDF98/printed81**: «مختبر تقنية التمثيل البياني 1-9: المصفوفات الموسعة». This is a calculator **lab** following lesson1-9; it is not a numbered 1-10 textbook lesson.

| Skill | 2 = demonstrated | 1 = partial | 0 = incorrect |
|---|---|---|---|
| Translate system to augmented matrix | All coefficient columns ordered, constants aligned, zero for missing variables | One misplaced entry | Confused variables/constant column |
| Calculator input | Correct 3×4 dimensions, rows and signed entries | One corrected input slip | Missing source column or wrong dimensions |
| RREF interpretation | Recognizes identity left block and reads (−4,3,6) in source example | Correct values with weak reasoning | Incorrect variable order or output |
| Independent verification | Substitutes into each of three original equations: 1,12,−1 | Checks one or two | No or incorrect check |
| Row operation justification | Shows whole-row operation including constants; explains why equivalent | Correct numeric entry without method | Changes coefficients but not constants |
| Uniqueness/consistency | Distinguishes pivot system from dependent/inconsistent systems; no false claim that one zero row decides | Partial analysis | False conclusion about solution count |

**Original mathematical demonstration:** source system `2س+ص+ع=1`, `3س+2ص+3ع=12`, `4س+ص+2ع=−1`. A valid reduced matrix is `[1 0 0|-4;0 1 0|3;0 0 1|6]`. Check: `−8+3+6=1`, `−12+6+18=12`, `−16+3+12=−1`.

**Boundary:** the exact generator covers only source-aligned **2×3 numeric** row operations and integer RREF readings for invertible 2×2 systems. Full 3×4 calculator input, keyboard details, variable-parameter systems, and 3×3 row-reduction proofs are **TEACHER_REVIEW_REQUIRED**. Stage D, CI and RTL student UI tests are **WRITTEN_NOT_RUN**, not passed.
