# Mathematics V2 — Vectors, Matrices, and Complex Numbers

## Status

Shadow-only implementation. All registered question families remain `ShadowVerified` with `productionRouting:false`.

## Supported exact subset

- Vector componentwise addition for vectors of 1–4 exact integer/rational components.
- Vector dot product for vectors of 1–4 exact integer/rational components.
- Matrix multiplication for rectangular matrices up to 3×3 where dimensions are compatible.
- Determinant evaluation for 2×2 exact integer/rational matrices.
- Complex addition and multiplication with canonical `complex_exact(real, imaginary)` representation and exact integer/rational parts.
- Deterministic solver-grounded question generation for difficulty bands 1–3.

## Verification

Every supported operation has an independent verifier implementation that recomputes the answer from the original request. The verifier does not consume or trust solver trace data. It verifies both the reported `ExactResult` and the one-value `FiniteSolutionSet` representation.

## Resource and safety boundaries

- Exact scalar numerator and denominator are limited to 4096 bits.
- Intermediate arithmetic is bounded before materialization.
- Zero-denominator rationals fail closed.
- Vector dimension mismatch fails closed.
- Matrix multiplication dimension mismatch fails closed.
- Oversized vectors or matrices fail closed.
- Non-canonical complex input fails closed.
- No floating-point approximation or numerical fallback is permitted.

## Curriculum and routing

This slice does not create or change lesson mappings, curriculum mappings, standards, or official OutcomeCodes. No learner-facing Practice route is enabled by this work. Production routing remains disabled until the later audited migration/cutover stages.

## Acceptance gate

The slice is acceptable only when:

1. exact solver tests pass for all six operations;
2. independent verifiers accept valid results and reject mutations;
3. malformed, dimension-incompatible, non-canonical, and resource-limit inputs fail closed;
4. generators are deterministic and independently verified across difficulty bands 1–3;
5. skill, capability, and question-family registry integrity passes;
6. Mathematics Intelligence and Phase16 CI gates pass before merge;
7. post-merge main CI passes and the exact merge SHA is deployed to Render;
8. live, ready/database, and homepage runtime checks pass after deployment.
