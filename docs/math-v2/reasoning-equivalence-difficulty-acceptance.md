# Mathematics V2 reasoning, equivalence, misconception and difficulty acceptance

## Scope

This shadow-only slice closes four kernel dependencies that sit between exact domain solvers and product migration:

1. deterministic Strategy Planner / multi-step reasoning metadata;
2. shared Answer Equivalence V2;
3. reviewed misconception/distractor transformations;
4. multidimensional difficulty and adaptive recommendation.

It does not change Practice, Assessment, Diagnostic or learner-facing routing.

## Strategy Planner

The planner is bounded and deterministic. It inspects the typed Math AST, assumptions and structural burden, then selects only reviewed strategy identifiers. It does not solve mathematics and cannot invent capabilities.

Required safeguards:

- AST node and depth budgets;
- bounded candidate strategy count;
- deterministic priority ordering;
- explicit Unsupported/ResourceLimit statuses;
- complexity metadata covering nodes, depth, transformations, strategies, branching, unknowns, constraints, representation switching, proof, modelling and multipart dependency.

## Answer Equivalence V2

`MathematicsAnswerEquivalenceV2` becomes the shared exact evaluator foundation for Mathematics V2. The reviewed subset includes:

- exact integers and rationals;
- exact arithmetic expression evaluation;
- bounded integer powers and exact roots;
- commutative symbolic Add/Multiply normalization;
- equations, inequalities and function-call structures;
- ordered vectors and matrices;
- derivative/integral structures;
- finite solution sets, intervals and unions.

Finite solution sets are unordered; vectors remain ordered. Exact mode never uses floating point as an oracle. Optional decimal tolerance is used only when a policy explicitly requests non-exact comparison and both exact rationals can be represented safely as decimal values.

## Misconception engine

Distractors are generated only from reviewed deterministic transformations such as sign reversal, off-by-one arithmetic, reciprocal confusion, ordered-component reversal and selected structural mistakes. Every candidate is passed through Answer Equivalence V2 and is discarded if it is equivalent to the correct answer.

This phase establishes the invariant:

> A distractor cannot be returned unless the shared evaluator proves it is not the correct answer in the supported equivalence subset.

## Difficulty / adaptive intelligence

The public UI can continue to show Easy / Medium / Challenging, while internal selection uses the existing `MathematicsComplexityVector` dimensions. The projection uses bounded deterministic scoring. Adaptive recommendation consumes:

- skill mastery;
- prerequisite mastery;
- representation fluency;
- recent misconception count;
- recent successful items.

Mastery inputs are constrained to [0,1]. The recommendation is guidance for later product migration, not a learner-facing cutover in this PR.

## Routing

All existing question families remain at their current routing state. This slice introduces no `productionRouting:true` change and no global solver switch. Product migration and curriculum gates are handled in subsequent PRs after CI and deployment of this foundation.
