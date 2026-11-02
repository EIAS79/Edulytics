# US Common Core — High-school academic content remediation (2026-10-11)

## Scope and evidence

Source basis: Edulytics learner-facing US Common Core JSON packs, their publisher lesson blueprint source URLs, the lesson Practice routing registry, and the previously catalogued 121 child standard clauses.

This correction set repairs **six existing high-school lessons** at source-preserving, hash-pinned JSON level. It changes neither official standard IDs nor lesson identities, lesson→outcome links, student scores, database tables, nor authentication. The publisher source artifact fingerprints (`sourceSha256`) remain unchanged; the seven-field corrected student teaching body is recorded with a new `canonicalBodySha256`.

| Existing lesson | Academic issue found in checked-in JSON | Corrected learning evidence |
| --- | --- | --- |
| Algebra 1 U04 L15, Inverse Functions | Extracted activity fragment with no completed inverse steps | Currency conversion 100 USD ↔ 1932 pesos; Caesar shift undoing; two-way linear inverse check |
| Algebra 1 U04 L16, Finding and Interpreting Inverse Functions | Source activity questions with generic procedural steps | Celsius/Fahrenheit inverse with a 68°F→20°C calculation; bookstore cost inverse and whole-book domain |
| Algebra 1 U04 L17, Writing Inverse Functions to Solve Problems | Unworked water tank source text and generic procedural steps | w(t)=80−2.5t, inverse w⁻¹(v)=(80−v)/2.5, with 0–32 minute and 0–80 liter restrictions |
| Algebra 2 U02 L17, Graphs of Rational Functions (Part 1) | Generic worked approaches; irrelevant instructions about clock hands | t(r)=10/r speed-time graph, asymptote r=0, contextual units, points, and a separate (x−2)/(x+1) graph |
| Algebra 2 U02 L18, Graphs of Rational Functions (Part 2) | Original activity extract, no full worked graph/end-behavior solution | Horizontal/vertical asymptotes of (2x+3)/(x−1), correct fraction rewriting and removable-hole counterexample |
| Algebra 2 U05 L10, Combining Functions | Generic procedures and unrelated data-display advice | Sums/products/quotients, shared-domain restrictions, graph-derived sums, books per capita, and explicit arithmetic-vs-composition distinction |

### Mathematical verifications

- `F(C)=(9/5)C+32`; `F(20)=68`, `F⁻¹(68)=20`.
- `w(13)=47.5`, `w⁻¹(47.5)=13`; `w⁻¹(5)=30`; physically permitted inverse volume inputs are 0 through 80.
- `t(8)=10/8=1.25` hours; 10 miles in 45 minutes requires 10/0.75=40/3 miles per hour.
- `f(x)=(x−2)/(x+1)`: f(0)=−2, f(1)=−1/2, f(−2)=4, x-intercept 2, vertical asymptote −1, horizontal asymptote 1.
- `g(x)=(2x+3)/(x−1)=2+5/(x−1)`: g(0)=−3, g(2)=7, x-intercept −3/2, vertical asymptote 1, horizontal asymptote 2.
- In 2010, 2530 million books / 309.35 million people ≈ 8.178 books per person.
- The work on combining functions **does not establish mastery of composition** `f(g(x))`: its publisher-source lesson covers arithmetic on functions. Do not claim official `HSF-BF.A.1.c` certified from it.
- A physical interval restriction on a linear tank example is **not equivalent to** `HSF-BF.B.4.d`, which requires an inverse of a *non-invertible* function by restricting its domain. That separate standard remains open.

### Academic blockers still outstanding

The current audit has 121 child clauses, 106 with direct publisher child-code alignments and 15 without such exact evidence. The six lesson corrections improve explanation quality but do **not** change publisher alignment records or count as complete independent proof of all 15 clauses. Remaining demonstrations include composition from source-grounded lessons, restricting a many-to-one function to make its inverse well-defined, graph drawing/shape and end behavior grading, head-to-tail/parallelogram vector diagrams, and constructed explanations comparing uncertain strategies. Numeric exact-answer Practice alone is insufficient to certify these modes.

### Release conditions

- Six content-repair rows are appended to `us-ccss-verified-body-repairs.v1.json` after the existing two, with exact prior/new teaching-body digests and unchanged source hashes.
- `verify_main_content_parity.py` must allow only these declared transitions; unchanged prior approved lessons must remain pinned to their new hashes, and all unlisted changes remain rejected.
- Run focused and full .NET regression, PR CI, post-merge main CI and Render health checks. The change updates embedded JSON but should not require SQL mutation. Read-from-JSON behavior requires explicit code/config evidence; don't pretend to have used logged-in student/teacher sessions (those are deferred per user request).