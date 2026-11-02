# Mathematics V2 — Geometry Shadow Slice

This slice is intentionally exact, fail-closed, and shadow-only.

## Exact supported subset

- rectangle area from positive exact rational width and height;
- rectangle perimeter from positive exact rational width and height;
- triangle area from positive exact rational base and perpendicular height;
- right-triangle Pythagorean side calculation when the requested side is an exact rational value.

## Safety rules

- all geometric dimensions must be positive exact scalars;
- malformed rationals fail closed;
- irrational Pythagorean results are not approximated and fail closed;
- exact arithmetic remains bounded by the Mathematics V2 scalar/resource limits;
- solver and verifier use separate implementations and independently recompute from the original problem;
- generated questions are solver-grounded and must pass independent verification before they are emitted;
- no floating-point fallback is used for this slice;
- no lesson mapping, curriculum hierarchy, official OutcomeCode, or learner-facing routing is changed;
- every new question family remains `productionRouting: false`.
