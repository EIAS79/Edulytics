# Grade10 General U01L07 — Teacher-reviewed nonmonic factorization, rational roots and contextual interpretation

**Canonical unit:** «كثيرات الحدود والتحليل». **Lesson:** «حل ax² + bx + c = 0». **Primary source:** General Arabic Student `01.pdf` 2025–2026 Volume1 physical PDF pp65–69 / printed52–56. **Crossbooks:** Advanced `01متقدم_.pdf` PDF131–137 and Applied `رياضيات كتاب الطالب تطبيقي 10 8.pdf` PDF49–55, nonmonic goal items C08/C12–C15, crosswalked to this canonical identity.

| Student evidence | 2 points: correct and justified | 1: partial | 0: incorrect |
|---|---|---|---|
| AC-method identification | For `أ س²+ب س+ج` explicitly computes `أ×ج` and selects two signed integers with the correct product and sum | Product correct but one sign or sum slip | Mistakenly uses `ج` alone as product |
| Middle-term decomposition | Splits `ب س` exactly and preserves all terms | Correct split but a minor arithmetic step slips | Drops a term or changes an exponent |
| Four-term grouping | Correct GCF in each pair and identical binomial factor; re-expansion proves equality | Correct factor candidates with incomplete grouping | Factors do not reproduce original |
| Signed/negative leading factor | Handles `أ<٠` by factoring `−١` or correctly signed pair selection | Right roots but weak sign explanation | Unjustified negative or dropped leading coefficient |
| Fractional/rational roots | Correctly solves each nonunit linear factor and reduces fractions | Correct method but fraction unreduced | Excludes roots because not integers or accepts wrong signs |
| Integer-prime vs real roots | Clearly states integer factorability and differentiates it from real roots/discriminant | Correct yes/no without qualification | Incorrectly says no real roots merely due to integer-primality |
| Original graph/model | Coordinates and intercepts agree with independently checked factors, drawing to scale correctly labels axes | One labeling or scale error | Inconsistent intercepts or curve |
| Real-life area/trajectory | Correct positive dimensions/meaningful time and rejects algebraic roots violating domain | Acceptable positive solution without explicit domain | Treats negative length or time as physical result |
| Mathematical writing | Explains and independently verifies why AC/grouping is necessary when `أ≠١`, with term-by-term distribution | Correct algebra but no explanation | False shortcut or unsupported statement |

## Original teacher-scored tasks

Factor `٦س²+١١س+٣` to obtain `(٣س+١)(٢س+٣)`; independently expand to check the middle coefficient is ١١. Explain why `(٦س+١)(س+٣)` is wrong (middle coefficient ١٩). Solve the equation and justify rational roots `−١/٣` and `−٣/٢`.

Factor `−٦س²+٧س+٣` as `−(٣س+١)(٢س−٣)`, taking care of the overall sign. Contrast `٢س²+س−٢`, which has no integer-coefficient binomial factorization, despite positive discriminant ١٧ and two irrational real roots.

For a rectangle with length `٢س+٣`, width `س+٤` and area `٤٢` square meters, solve `٢س²+١١س−٣٠=٠` to obtain `س=٢` and `س=−٧٫٥`, then explain why the negative root is inadmissible. The positive dimensions are ٧ m and ٦ m. Make a correct original area diagram with units.

For the independent projectile model `h(t)=−١٠t²+٣٠t+٤٠`, solve height zero to obtain `t=−١` and `t=٤`, and explain why ٤ seconds is the return time after launch. This is not a request to copy the source textbook's printed word problem.

## Assessment boundary

Source-exact bounded C# `nonmonic_full_factor`, `nonmonic_rational_roots`, `nonmonic_prime_over_integers` families can grade only their documented one-variable integer-coefficient/rational-root formats after Stage D. Full manual grouping proofs, negative-leading expressions outside bounded generated form, original physical graphing, diagrams and open application explanations stay `TEACHER_REVIEW_REQUIRED`.

The tests are authored `WRITTEN_NOT_RUN`; `productionRouting=false` and the lesson `practiceReadiness` remains explicitly fail-closed pending Stage D verification.
