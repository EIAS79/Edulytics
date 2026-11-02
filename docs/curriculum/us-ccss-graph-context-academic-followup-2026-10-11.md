# US Common Core — Graph and Contextual Reasoning Follow-up (2026-10-11)

## Basis and release boundaries

Source audit: the embedded lesson JSON, official parent-standard mapping, current exact Practice question families and verified student-facing examples in `EIAS79/Edulytics`. This incremental repair does **not** assert complete certification of all 121 Common Core subsidiary clauses.

The existing US lesson source inventory contains 1,560 lessons and 2,882 lesson/outcome links. Child references remain 106 exact publisher child-code links and 15 parent-only references; this change neither fabricates a publisher reference nor modifies the underlying lesson JSON.

## Five new narrowly scoped question families

| Family | Existing reviewed lesson | What the question genuinely verifies | What it does not verify |
|---|---|---|---|
| `usccss.hs.rational_hole_or_pole` | Rational Functions Part 1 | Distinguishes a canceled factor (removable hole) from an uncanceled vertical asymptote; retains excluded original inputs | Freehand graph of both branches and a correct hole marker |
| `usccss.hs.rational_branch_limit_sign` | Rational Functions Part 1 | Determines whether the graph rises to positive or falls to negative infinity from a stated side | Continuous graph rendering or symbolic limit proofs |
| `usccss.vector.head_tail_endpoint_y` | The Arithmetic of Vectors | Computes head-to-tail endpoint coordinate for a pair of displacement arrows; matches parallelogram resultant | Freehand arrow positioning and slope drawing |
| `usccss.vector.scalar_direction_class` | The Arithmetic of Vectors | Recognizes same/opposite/undefined direction for positive, negative and zero scalar | Interpreting arbitrary vector diagrams or scaling by unknown variable |
| `usccss.probability.expected_vs_worst_case` | Expected Value in Games and Decisions | Makes a different plan choice for minimum expectation versus minimum worst-case cost | Grading original written justification, financial advice, or broader risk preferences |

All families are answer-checked by independent computation from the original sampled question parameters, across all three difficulties. The choices are encoded as integers and described explicitly in each prompt; they do not silently pretend to score graphical or essay answers.

## Scope protections

- Three source-reviewed lesson mappings receive additional allowed families; all other lessons keep their original mappings.
- No lesson text, publisher artifact hash, content authority digest, lesson ID, outcome link, database table, account or student grade is touched.
- Prior fixed-route tests are extended; a new test checks both positive and negative cases, hole and pole, positive/negative/zero scalar cases, and both decision objectives.
- The 15 child-clause source-reference gaps are **not** declared closed. Graphics, proof, diagram and written-response scoring remain separately open.
- User explicitly requested postponing authenticated learner and teacher UI tests until curriculum completion.

### Release gate

Do not merge until the focused + complete test suite and the exact PR commit's Actions pass. After merging wait for Render automatic deployment and verify live service readiness, the exact deployment SHA and clean error logs. Document failure explicitly rather than claiming a successful release from an earlier live version.
