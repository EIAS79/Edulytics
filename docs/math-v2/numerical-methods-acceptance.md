# Numerical Methods V2 shadow acceptance

## Scope

This slice introduces deterministic, bounded numerical-method operations while preserving exact-rational arithmetic at every computational step. The method output is **not** reclassified as an exact mathematical root or exact integral merely because the arithmetic is exact.

Supported operations:

- fixed-iteration bisection for bounded single-variable polynomial expressions over exact rational endpoints;
- fixed-iteration Newton method for bounded single-variable polynomial expressions over an exact rational starting value;
- fixed-subdivision trapezoidal-rule estimation for bounded single-variable polynomial expressions over exact rational bounds.

## Result semantics

- Bisection returns the exact-rational **bracket after N iterations**. If an evaluated point is an exact root, the bracket collapses to `[r, r]`.
- Newton returns the exact-rational **iterate `x_N`**. It is not labelled as the exact root unless the fixed iteration lands on an exact root.
- Trapezoidal rule returns the exact-rational **numerical estimate produced by the declared subdivision count**. It is not labelled as the exact definite integral.

## Bounds and fail-closed rules

- exact integer/rational scalar parameters only;
- polynomial AST subset only: constants, one declared variable, negation, addition, multiplication, division by a non-zero scalar constant, and integer powers `0..8`;
- maximum AST node/depth budget: 128 nodes / 24 levels;
- scalar numerator and denominator budget: 4096 bits with bounded intermediates;
- bisection iterations: `1..8` and endpoints must bracket a sign change or contain an exact endpoint root;
- Newton iterations: `1..6`; a zero derivative before completion fails closed;
- trapezoidal subdivisions: `1..8` with `lower < upper`;
- malformed rationals, unsupported symbols/shapes, invalid iteration counts, division by zero and resource-limit violations fail closed;
- no floating-point fallback and no tolerance-based convergence claims.

## Verification

Each operation has an independent verifier that reconstructs the fixed-step method from the original request using a separate evaluator/arithmetic implementation. The verifier does not trust the solver trace or the solver-computed output.

Mutation tests require altered method outputs to be rejected.

## Routing

All Numerical Methods V2 skills and question families remain `ShadowVerified` with `productionRouting:false`.

This slice adds no curriculum mapping, no official outcome code and no learner-facing Practice/Assessment cutover.
