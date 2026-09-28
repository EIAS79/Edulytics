# Advanced Mathematics Capability Expansion

Status: companion programme to Adaptive Practice V2.

## Architecture decision

Do not merge advanced-mathematics expansion into the Primary Adaptive V2 rollout.

Adaptive V2 answers:

> Which already-verified question should the learner receive next?

Advanced Mathematics answers:

> Which mathematical domains/question forms can Edulytics safely generate, solve, independently verify, render, collect answers for, and map to the curriculum?

Both programmes share the existing Mathematics V2 core, but retain independent rollout and certification boundaries.

## Current baseline

The current repository already contains substantial exact Mathematics V2 capability, including:

- ExactCalculusSolvers / verifier pair
- ExactFunctionsGraphsSequencesSolvers
- ExactGeometrySolvers
- ExactIndicesExponentialsLogTrigSolvers
- ExactVectorsMatricesComplexSolvers
- ExactProbabilityStatisticsSolvers
- ExactMechanicsModelSolver
- ExactNumericalMethodsSolvers
- exact linear equation / inequality / 2x2 system solvers
- exact quadratic solver
- MathematicsAnswerEquivalenceV2
- question-family registry
- complexity/difficulty infrastructure
- reviewed misconception infrastructure

Current supporting/exact families already cover narrow slices such as derivative value, simple definite integration, optimization, differential-equation values, trig rules/graphs, vectors, determinant2, complex operations, functions, coordinate intersections, series, mechanics and numerical methods.

The remaining problem is therefore not "advanced mathematics is absent". The problem is depth, breadth, graph/visual interaction, multi-step structure, and formal curriculum-to-surface coverage.

## Non-negotiable integration contract

Adaptive V2 may consume a family only when ALL required capability is closed:

1. exact solver exists;
2. independent verifier exists;
3. generator exists;
4. answer-equivalence policy exists;
5. learner interaction exists;
6. visual renderer exists where required;
7. exact curriculum/lesson mapping exists;
8. LessonPracticeContract explicitly permits the family;
9. capability is READY_VERIFIED;
10. the learner curriculum level is explicitly enabled.

An engine existing in code is not sufficient.

## Main target domains

- functions and graph interpretation/construction
- differentiation
- integration
- area under/between curves
- limits/infinity/continuity where formally mapped
- sine/cosine/tangent graphs and equations
- analytic geometry
- vectors
- matrices
- complex numbers
- sequences/series
- geometry
- probability/statistics
- mechanics
- numerical methods

## Required visual architecture

Current Practice supports typed scalar/structured inputs and server SVG visuals. Advanced capability requires first-class typed visuals:

- CoordinatePlane
- FunctionGraph
- MultiFunctionGraph
- GeometryDiagram
- VectorDiagram
- MatrixDisplay
- ArgandDiagram
- RegionOfIntegration
- StatisticalPlot

Visuals must be deterministic from stored mathematical parameters. Do not persist arbitrary AI-authored SVG/HTML.

## Required advanced interactions

Potential typed response contracts:

- CoordinatePoint
- CoordinateMultiPoint
- LineOnGraph
- CurveChoice
- GraphFeatureSelection
- IntervalSelection
- RegionSelection
- MatrixEntry
- VectorEntry
- ExpressionEntry
- EquationEntry
- SetIntervalEntry
- MultiPartStructured

The server remains mathematical authority.

## Area-between-curves target

Required mathematical pipeline:

parse f,g
→ solve relevant intersections
→ determine upper/lower curve by interval
→ split when ordering changes
→ integrate |f-g| piecewise
→ independently verify
→ render both curves + shaded region
→ generate structured solution trace

Do not implement as a cosmetic text template.

## Capability certification

Use separate readiness surfaces:

- PRACTICE_READY_VERIFIED
- ASSESSMENT_READY_VERIFIED
- EXAM_READY_VERIFIED

A question family may be valid for Practice before it is certified for a formal Exam.

## Production phases

M0 — re-audit current capability and generate a full capability matrix.

M1 — first-class MathematicalVisualSpec and deterministic graph/diagram renderer.

M2 — advanced learner interaction contracts and server-side validation.

M3 — functions/graphs: transformations, intercepts, turning points, asymptotes, intersections, graph matching/interpretation.

M4 — calculus I: derivative rules, tangent/normal, stationary points, monotonicity, optimization, graph-derived derivative problems.

M5 — calculus II: indefinite/definite integration, initial conditions, area to axis, area between curves, signed/geometric area, differential equations.

M6 — trigonometry: exact values, unit circle, sin/cos/tan graphs, transformations, equations, identities, radians, triangle applications.

M7 — analytic geometry/conics: lines, circles, tangents, loci, intersections, coordinate proofs, mapped conics.

M8 — vectors/matrices/complex: vector geometry, bounded matrix operations/transforms, complex-plane representation.

M9 — limits/infinity/sequences/series where curriculum evidence explicitly requires them.

M10 — multi-part reasoning and structured solution traces.

M11 — Assessment Builder integration while preserving draft/review/approve/publish.

M12 — formal Exam certification, mark scheme validation, print/PDF graph rendering.

M13 — exact advanced-curriculum closure and academic review.

M14 — future Adaptive V2 compatibility for explicitly approved secondary programmes; does NOT expand Primary 1–6.

M15 — fuzz/property/differential tests, resource budgets, security, observability and rollback.

## Definition of done for one advanced family

A family is not complete until:

- canonical skill
- curriculum mapping
- problem contract
- generator
- solver
- independent verifier
- answer equivalence
- misconceptions
- complexity calibration
- representation/visual
- interaction
- accessibility
- solution trace
- exposure fingerprint
- semantic identity
- property/golden/resource tests
- Practice/Assessment/Exam certification decision

are all explicitly closed.

## Primary boundary

Nothing in this programme changes the current Primary Adaptive V2 rollout boundary. Advanced families must never become eligible in Primary merely because the underlying solver supports them.
