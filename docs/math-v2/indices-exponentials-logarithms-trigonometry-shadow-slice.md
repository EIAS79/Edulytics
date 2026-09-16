# Mathematics V2 — Indices / Exponentials / Logarithms / Trigonometry Shadow Slice

This slice is intentionally production-safe and shadow-only.

## Exact supported subset

- integer powers of exact integer/rational bases, including bounded negative integer exponents;
- exact same-base exponential inversion when the answer is an integer exponent;
- exact logarithm evaluation when the argument is an integer power of the supplied positive base;
- exact sine/cosine/tangent values for the supported degree special-angle set.

## Safety rules

- no floating-point mathematics is used for exact trigonometric answers;
- unsupported/non-exact logarithms and exponential inversions fail closed;
- undefined tangent values fail closed;
- exponent magnitude is bounded;
- exact scalar results remain subject to the 4096-bit result budget;
- solver and verifier use separate implementations;
- generated questions are solver-grounded and must pass independent verification;
- no lesson mapping, official OutcomeCode, curriculum hierarchy, or learner-facing routing is changed;
- every new family remains `productionRouting: false`.
