# Algebra 1 U02:L12 — A–B–C closure

**Lesson:** Writing and Graphing Systems of Linear Equations
**State:** `READY_FOR_STAGE_D`

## Local sources
Teacher preparation SHA-256: `214d403f0c2c1de720272eaf05fc93bc02e3ad1da144fa7763e85298db3f4ef2`
Teacher lesson SHA-256: `c3e6ebd8a62d1ccd486db74ae8d04487f8387b133f60078114a17133020a01ac`
Teacher practice SHA-256: `d3b5a6f9c39547978849afb45bee2b7ec603d05282ee8b4ad065b64ca2ad215f`
Student lesson SHA-256: `52d5613f8bc214d39cc94bd6bb91c7dc94b951c975e144f793ad5ee850c1dec8`
Student practice SHA-256: `f9511230135afa76c74dde99d173f7b65c9f3fed1be7607f47206cadb9393154`

A/B: preserved the existing Rich V2 for modeling two constraints, graph intersection, and simultaneous solution.

C: **Extend existing exact question route**, using the established `algebra.linear.systems.two_by_two.integer_coefficients` family identifier with independent deterministic coefficient fixtures and a Cramer's-rule verifier. This implementation does not call the solver-backed factory directly; it reuses its contract/family, rather than pretending solver-backed execution took place. It grades the objective ordered pair; graph construction, contextual system setup, and method justification stay `TEACHER_REVIEW_REQUIRED`.

Targeted unit tests **AUTHORED_NOT_RUN**. No .NET build, CI, Stage D or deployment executed in A–B–C. AccessIM/v360 excluded.
