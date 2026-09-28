# Adaptive Practice V2 — Phase 9 Primary rollout

Status: implementation contract for controlled learner-facing rollout.

Phase 9 expands the already-live internal Canary through reviewed Primary
logical levels 2–6. It does **not** enable secondary, IGCSE, AS/A-Level, or
Advanced Mathematics routing.

## Reviewed rollout pairs

| Wave | Curriculum level | Lesson |
| --- | --- | --- |
| Stage 2 | `CAMBRIDGE-INTL-MATH:L02:SHARED` | `PED:CAMBRIDGE-INTL-MATH:S2:2AS-1:APPLY` |
| Stage 3 | `CAMBRIDGE-INTL-MATH:L03:SHARED` | `PED:CAMBRIDGE-INTL-MATH:S3:3AS-2:APPLY` |
| Stage 4 | `CAMBRIDGE-INTL-MATH:L04:SHARED` | `PED:CAMBRIDGE-INTL-MATH:S4:4NF-1:APPLY` |
| Stage 5 | `CAMBRIDGE-INTL-MATH:L05:SHARED` | `PED:CAMBRIDGE-INTL-MATH:S5:5F-2:APPLY` |
| Stage 6 | `CAMBRIDGE-INTL-MATH:L06:SHARED` | `PED:CAMBRIDGE-INTL-MATH:S6:6F-3:APPLY` |

Each lesson resolves through the authoritative lesson Practice capability layer
as `READY_VERIFIED`.

## Safety contract

- Canary remains school-scoped.
- Canary startup fails if configured level/lesson values are outside the audited
  rollout registry.
- Every configured level must have a configured reviewed lesson pair.
- The Practice repository independently filters lessons by the student's active
  curriculum adoption framework/version/logical level/pathway.
- Existing Practice V1 remains the fallback outside the exact Canary scope.
- Advanced Mathematics flags remain unchanged and disabled.
