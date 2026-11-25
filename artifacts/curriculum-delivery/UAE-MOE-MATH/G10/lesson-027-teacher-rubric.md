# Rubric — G10-U03-L03 البرمجة الخطية

Source: Arabic Advanced 2025–2026 Vol1 `01متقدم_.pdf`, PDF pp41–47 (printed 24–30). Canonical General ordinal27.

| Criteria | Secure (2) | Partial (1) | Incorrect (0) |
|---|---|---|---|
| Formulating original constraints | All numerical limits, units, nonnegativity and direction signs valid | One constraint incomplete | Confuses objective with constraint |
| Feasible region graph | Accurately plots every boundary, shades common region, labels vertices | Minor coordinate/label mistake | Uses one inequality or wrong half-plane |
| Corner evaluation | Checks all valid vertices and calculates objective correctly | One arithmetic error | Compares unverified or infeasible points |
| Max/min and uniqueness | Correct optimum, location and ties between adjacent vertices | Correct value without reasoning | Claims only a unique optimum when a whole edge is optimal |
| Unbounded/empty cases | Distinguishes lack of maximum from lack of feasible solutions | Correct graph, weak interpretation | Claims unbounded means no optimum of either type |
| Business interpretation | Explains product quantities, units, integer restrictions and limits | Partial interpretation | Impossible or negative production quantity |

Original checks: `س≥٠، ص≥٠، ٢س+ص≤١٢`, `ز=٣س+٢ص`: values `٠،١٨،٢٤`; maximum 24 at (0,12). `س+ص≥٦` in first quadrant has minimum 6 for objective `س+ص` but no maximum.

**Grade10 exact Practice boundary:** bounded triangular region only, integer-coefficient objective; numeric optimum and a genuinely unique optimum vertex code may be machine graded, subject to Stage D tests. General polygon plotting, unbounded cases, ties along edges, real contexts and graph shading are **TEACHER_REVIEW_REQUIRED**. Tests `WRITTEN_NOT_RUN`; no authenticated UI or deployment claim.
