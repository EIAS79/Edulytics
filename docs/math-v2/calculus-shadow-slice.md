# Calculus Mathematics V2 — Shadow Slice

## Scope

This slice adds exact polynomial calculus while remaining shadow-only.

Supported operations:
- first derivative of a supported single-variable polynomial with exact rational coefficients;
- definite integral of a supported single-variable polynomial between exact integer/rational bounds;
- deterministic solver-grounded question generation for both skills;
- independent verification from the original AST.

## Exact subset

Expressions are limited to the bounded polynomial subset already used by Mathematics V2: exact integer/rational constants, one declared symbol, addition, negation, multiplication, division by a non-zero exact scalar, and bounded non-negative integer powers.

## Fail-closed exclusions

This slice does not claim support for:
- higher-order derivatives;
- indefinite integration;
- limits;
- transcendental differentiation/integration;
- numerical quadrature or floating-point approximations;
- unsupported/malformed rational values;
- expressions or scalars beyond configured resource budgets.

Unsupported or ambiguous inputs fail closed rather than falling back to an unrelated question family or approximate mathematics.

## Verification and routing

The verifier independently parses and recomputes the requested calculus operation from the original input. It does not trust the solver trace or solver-produced intermediate representation.

All Calculus V2 skills and question families remain `ShadowVerified` with `productionRouting:false`. This change does not add or infer curriculum mappings or OutcomeCodes.