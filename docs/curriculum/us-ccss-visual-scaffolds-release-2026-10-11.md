# US Common Core visual scaffolds for contextual Practice — 2026-10-11

## Scope

The previously deployed CCSS contextual families generated text-only problems for rational-graph interpretation and vector constructions. The standard learner adaptive Practice view already supports a **server-owned SVG** via `PracticeMathVisualRenderer.RenderSvg(question.GenerationFamily, question.GenerationParametersJson)`, but these four family IDs were not routed to it.

This changes **only** the existing deterministic SVG rendering layer, family representation metadata and tests, and does not touch curriculum lesson IDs, standards, JSON teaching prose, student scores, persistence, session contracts or grader.

| Exact existing family | New diagram |
| --- | --- |
| `usccss.hs.rational_hole_or_pole` | Coordinate graph with rational branches, dashed vertical pole and open circle at the canceled factor |
| `usccss.hs.rational_branch_limit_sign` | Coordinate graph of the rational function on both sides of its pole |
| `usccss.vector.head_tail_endpoint_y` | Coordinate plane with two head-to-tail arrow steps from the generated starting point |
| `usccss.vector.scalar_direction_class` | Original and signed-scaled vector arrows; zero scalar is a point at origin |

SVGs are generated entirely from numeric server-owned exact-family parameters, with no injected question text or arbitrary HTML, use the current renderer's output escape/visual-leak screening and are already displayed by `Views/StudentAdaptivePractice/Attempt.cshtml`.

## Quality and boundaries

- The existing `PracticeVisualFamilyCoverageTests` checks **every** lesson-routed visual family (including these new `graph` and `vector` representations) with deterministic engine generation and SVG renderer calls.
- `UsCommonCoreGraphVectorVisualTests` adds a 4-family × 3-difficulty × 12-seed integration audit, deterministic rendering, original-answer re-verification, finite graph samples, null on missing/invalid parameters, and coordinate/shape safeguards.
- Graph SVGs provide *scaffolding and reading practice*, not freehand learner graph grading, drawing input, or proof of ability to construct a graph unaided.
- Vector arrows provide *representation*, not scoring of an independently drawn parallelogram.
- Source reference inventory remains **106 of 121 exact child-code links, 15 parent-only**. The semantic `productionReady:false` gate stays unchanged.
- User explicitly deferred authenticated student/teacher E2E until curriculum review is complete.

## Release gates

Require full .NET regression, successful PR CI, successful main CI, correct Render commit live and post-deploy `/health/live` + `/health/ready`. Do not advertise all 121 child standards as fully certified.
