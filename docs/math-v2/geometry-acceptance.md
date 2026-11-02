# Mathematics V2 — Geometry Acceptance

Geometry V2 is accepted for the shadow capability layer when all of the following hold:

- exact rectangle area and perimeter solve and independently verify;
- exact triangle base-height area solves and independently verifies;
- Pythagorean hypotenuse and leg cases solve only when the missing side is an exact rational value;
- non-positive dimensions, malformed rationals, invalid right-triangle inputs, and irrational Pythagorean results fail closed;
- verifier mutation tests reject incorrect candidate answers;
- deterministic question factories solve and verify before emitting questions;
- registry integrity, Mathematics Intelligence tests, Phase16 quality/PostgreSQL/SAST/container gates all pass;
- all new geometry skills and question families remain ShadowVerified with `productionRouting: false`;
- no curriculum or learner-facing production routing change is introduced by this slice.
