# US Common Core risk-decision answer-diversity repair — 2026-10-11

## Finding

The exact Practice family `usccss.probability.expected_vs_worst_case` had a deterministic answer cue. Its previous parameter rule forced Plan B to have lower expected cost, but Plan A to have lower worst-case cost, **for every generated scenario**. A student could select the correct answer from the objective wording alone without performing expected-value calculations. The existing regression test reinforced this issue by asserting that fixed pattern.

## Repair

The existing source-grounded HSS-MD.B.5 decision lesson now samples three disjoint numerical regimes:

1. Plan A costs less than Plan B's expected cost (A is optimal for both criteria).
2. Plan A lies strictly between Plan B's expected and worst-case costs (B optimal for expected cost, A for worst case).
3. Plan A costs more than Plan B's worst case (B optimal for both criteria).

The incident probability remains `1/denom`; Plan B's incident surcharge is `denom * step`, so expected cost is exactly `baseCost + step` without rounding. No ties are generated. The student-facing worked solution now shows both exact comparisons numerically, not only generic advice. The stored family contract is bumped to **version 2**; fresh questions record `scenarioVersion: 2` in the parameter payload. Historical question parameter bags without this field remain solvable and gradeable via the unchanged original-parameter recomputation formula.

The updated regression independently computes the correct answer from original parameters, enforces strict non-ties/positive costs, and checks that **both A and B can be correct under both decision criteria** across the fixed-seed corpus. The family remains numeric/fixed-response. This does **not** grade original written arguments or establish full academic certification of HSS-MD.B.5.a/b.

## Release constraints

- No edits to any of the 1,560 US lesson bodies, official outcome codes, source fingerprints or 2,882 links.
- No schema migration, account, mastery, score or student record updates.
- Require full .NET regressions and successful pull-request checks before merge; post-merge main checks and exact Render deployment evidence before marking it live.
- All 121 subsidiary-clause academic signoffs remain open; the 106 exact publisher references / 15 parent-only reference inventory is unchanged.
