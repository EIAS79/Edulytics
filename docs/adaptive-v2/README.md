# Adaptive Practice V2

This directory tracks the additive Adaptive Practice V2 programme.

The implementation rule is simple:

> Existing Practice V1 remains authoritative unless the fail-closed V2
> eligibility resolver explicitly allows a session.

Initial rollout order:

1. Off
2. Shadow
3. internal Canary
4. Primary Stage 1
5. Stage 2
6. Stage 3
7. Stage 4
8. Stage 5
9. Stage 6

Secondary / IGCSE / AS / A-Level levels are not automatically included.

## Canary safety contract

Learner-facing `Canary` mode is fail-closed and requires all three explicit
allow-lists to be non-empty:

- curriculum level;
- lesson code;
- school ID.

An empty school or lesson allow-list never means "all" in Canary mode.
`On` mode retains the broader rollout semantics for future controlled
production expansion.

See `BASELINE_INVENTORY.md` for the protected production baseline.


## Companion programme: Advanced Mathematics Capability Expansion

Advanced mathematics is intentionally implemented as a separate companion programme.

Adaptive V2 controls learner sequencing and remediation. The advanced-mathematics
programme controls which deeper mathematical capabilities are safe to generate,
solve, verify, render and expose in Practice/Assessment/Exam.

Adaptive V2 may consume a newly expanded family only after that family is formally
curriculum-mapped and READY_VERIFIED for the exact learner level/lesson.

See:

`docs/math-v3/ADVANCED_MATHEMATICS_CAPABILITY_EXPANSION.md`

This companion programme does not expand the Primary 1–6 rollout boundary.
