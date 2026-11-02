# Calculus Mathematics V2 — Acceptance

Calculus V2 is accepted for merge only when all of the following hold:

- exact first-order polynomial derivatives are solved and independently verified;
- exact definite polynomial integrals are solved and independently verified from original integrand and bounds;
- verifier rejects mutated answers;
- malformed rationals, unsupported forms, missing integral bounds, higher derivative orders and oversized inputs fail closed;
- deterministic generators pass for difficulty bands 1–3;
- skill, capability and question-family registries are internally consistent;
- all new families remain `productionRouting:false`;
- Mathematics Intelligence catalogue/readiness/foundation tests pass;
- Phase16 quality, PostgreSQL, SAST and container gates pass;
- no curriculum mapping or official OutcomeCode is added by inference.