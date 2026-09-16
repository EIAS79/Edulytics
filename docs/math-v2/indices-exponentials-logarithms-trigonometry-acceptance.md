# Acceptance criteria

The slice is acceptable only when all of the following are true:

- exact index powers support bounded positive/negative integer exponents and fail closed on undefined/resource-limit cases;
- same-base exponentials return only independently verified integer exponents;
- logarithms return only independently verified integer values;
- special-angle trigonometric answers are canonical exact AST values without floating point;
- mutated solver answers are rejected by independent verifiers;
- deterministic factories pass for difficulty bands 1–3;
- registry integrity passes;
- all new question families remain shadow-only with `productionRouting: false`;
- no curriculum or official outcome mapping changes occur.
