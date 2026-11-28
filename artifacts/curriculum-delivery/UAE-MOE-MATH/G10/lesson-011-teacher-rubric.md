# Grade 10 General G10-U01-L11 — original teacher assessment rubric

**Lesson:** «الدوال كثيرة الحدود». **Source:** `01متقدم_.pdf` owner-uploaded Arabic Student Vol1 (2025–2026), Advanced Unit 3 Lesson 3-3, PDF pp213–220 / printed196–203. Integrated into the owner-approved **General unified curriculum only**.

| Observable mathematical work | 2 points — complete | 1 point — partially correct | 0 points — invalid |
|---|---|---|---|
| Polynomial definition and degree | Rejects negative/fractional powers and variable denominator; identifies correct degree and leading coefficient | Valid classification but missing explanation | Calls a rational function polynomial or chooses largest coefficient as degree |
| Evaluation and composition | Substitutes entire expression, expands consistently, combines terms, checks units | One computational slip but correct structure | Uses input as a coefficient or loses parentheses |
| End behavior proof | Distinguishes odd/even degree and leading sign, correctly labels left/right | Correct endpoints with weak justification | Uses lower-order terms to determine both ends |
| Original Cartesian graph | Plots accurate (x,f(x)) points and labels axes, scale, zero crossings and end arrows; smooth continuous curve | One plot or axis label mismatch | Nonrepresentative graph or absent axes |
| Intercepts and turning points | Distinguishes observable x-intercepts from degree/turning point upper bounds | Counts visible intercepts only with poor justification | Treats every curve crossing as guaranteed or exceeds degree without reasoning |
| Scientific polynomial model | States domain, time range, volume units, plausible interpretation and model limitations | Missing one domain/unit detail | Treats extrapolation to all reals as physical truth |
| Technology comparison | Compares manual values with calculator output, clearly notes approximate graphical roots | Captures chart without mathematical explanation | Reports pixels as exact proof |

**Original graph task A:** Draw and label a smooth curve for `د(س)=س³−٤س` through `(−٢،٠),(−١،٣),(٠،٠),(١،−٣),(٢،٠)`. Explain why the left end falls and the right end rises.

**Original graph task B:** Draw `د(س)=س⁴−٥س²+٤` from points `(−٢،٠),(−١،٠),(٠،٤),(١،٠),(٢،٠)`. Identify that both ends rise, although intermediate values include zero; do not assert that the sample points uniquely determine every turning point.

**Digital exact subset:** Grade10 authored families `polynomial_evaluate_integer`, `polynomial_degree_integer` and `polynomial_end_behavior` handle bounded coefficient lists up to degree 5 and integer inputs −3..3. Original freehand graphs, physical-model restrictions, algebraic substitution and extended visual interpretations remain **TEACHER_REVIEW_REQUIRED**.

**Status:** rubric authored for Stage A–B–C; `productionRouting=false`; Stage D .NET/Node/E2E tests are `WRITTEN_NOT_RUN`. No fake UI or automatic grading claim.
