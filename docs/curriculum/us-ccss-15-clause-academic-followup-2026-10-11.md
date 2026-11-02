# US Common Core — 15 child-clause verification follow-up

This is a direct audit of the checked-in student lesson JSON, publisher lesson blueprints, and Practice question-family registry. It does **not** certify an entire standard merely because the parent OutcomeCode appears in a lesson. No student or teacher login was needed.

## Grounding and current judgment

The earlier corrected source-reference inventory contains 121 child clauses: 106 with exact publisher-level child-code references, 15 whose available blueprints name only their parent standard. A missing child-code reference does not automatically mean missing instruction. Source files were read, including `us-ccss-math-*.lesson-content-pack.json`, `lesson-skill-mappings.v1.json`, and `SupportingPracticeCompletionEngine`.

| Clause | Direct lesson-content evidence and targeted changes | Remaining limit |
| --- | --- | --- |
| 3.MD.C.7.a | G3 U02 L08 "Area of Rectangles Without a Grid"; added unit-square area count family | Verify visual tiling and explanations, not only the numeric product |
| 5.MD.C.3.a | G5 U01 L01 "What Is Volume?"; added 1-cubic-unit cube count family | Confirm unit cube visual representation |
| 6.SP.B.5.a | G6 U08 L17 "Using Box Plots"; added reporting count of observations | Verify descriptive summaries and contextual reporting |
| HSF-BF.A.1.c | "Combining Functions"; replaced unrelated generic exponential/logarithm Practice with function arithmetic | General function composition is still not verified as learner-facing in the selected publisher lesson; the new standalone composition family is **not** claimed to cover this clause |
| HSF-BF.B.4.b | "Inverse Functions"; added inverse composition verification family | Verify explanation and graded use of f(g(x)) and g(f(x)) |
| HSF-BF.B.4.c | "Finding and Interpreting Inverse Functions"; added inverse table lookup family | Numeric table lookup does not validate graph reading |
| HSF-BF.B.4.d | Existing inverse lessons are primarily linear; a valid restricted-quadratic inverse family was implemented **but not routed** to an unrelated original lesson | Needs a source-grounded restricted-domain lesson and qualified assessment |
| HSF-IF.C.7.d | "Graphs of Rational Functions (Part 1)"; replaced quadratic-root routing with zero, vertical and horizontal asymptote families | Requires graph-production/shape/end-behavior grading beyond numeric intercepts/asymptotes |
| HSN-VM.B.4.a | Vector lesson repaired in PR #443 with component addition | Head-to-tail diagrams still require visual assessment |
| HSN-VM.B.4.b | Vector lesson repaired in PR #443 with additive/subtractive component cases | Formal parallelogram construction requires visual assessment |
| HSN-VM.B.4.c | Vector lesson repaired in PR #443 with magnitude and direction of resultant | Full magnitude/direction interpretation beyond numeric angle still requires review |
| HSN-VM.B.5.a | Vector lesson repaired in PR #443 with signed scalar multiplication | Verify full geometric interpretation |
| HSN-VM.B.5.b | Vector lesson repaired in PR #443 with ||kv||=|k| ||v|| | Verify direction reversal diagrams |
| HSS-MD.B.5.a | Expected Value lesson repaired in PR #443; game-payoff examples and exact questions | Long-run contextual interpretation remains to be assessed independently |
| HSS-MD.B.5.b | Expected Value lesson repaired in PR #443; expected-cost strategy comparison | Rich decision/risk reasoning beyond numeric selection requires review |

## This change set

- Eleven independently recomputed numeric question families, covering Grade 3 unit-square area, Grade 5 cube volume, Grade 6 number of observations, function arithmetic/composition, inverse composition/table/restricted-domain calculations, and rational-graph intercept/asymptote values.
- **Eight explicit publisher-title-based lesson routes** (not blanket updates to every lesson carrying a secondary official standard).
- Two original misroutes specifically removed: `Graphs of Rational Functions (Part 1)` previously asked for `supporting.algebra.quadratic_larger_root`; `Combining Functions` and `Writing Inverse Functions` previously offered generic exponentials/logarithms.
- A source-less graph/visual or conceptual reasoning gap remains **open**, rather than falsely marking every one of the 15 clauses as certified.
- No changes to lesson identities, content JSON hashes, import-state rows, grading records, or production SQL are requested.

**Release gate:** all targeted tests and full regression must pass, and GitHub checks must be green. Publishing these bounded fixes does not equal full academic signoff for all 121 child clauses.