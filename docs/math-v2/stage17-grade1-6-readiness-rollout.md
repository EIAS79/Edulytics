# Stage 17 — Grade 1–6 readiness-driven rollout

## Scope

Stage 17 is a fail-closed, domain-by-domain production-routing migration for Grade 1–6 Mathematics. It is not a global enablement switch and it does not treat `ShadowVerified` solver families as production-ready by itself.

The approved lesson-skill registry now contains seven mappings in total. Five are Grade 1–6 mappings. Tranche 1 introduced the three previously accepted Cambridge Primary Stage 6 exact lessons. Tranche 2 adds only the two Cambridge Primary Stage 5 `Find equivalent fractions` lessons after deterministic high-confidence skill resolution, `PASS_TARGETED` semantic evidence, and review of a dedicated exact learner-facing mechanic. No title-only or shadow-only promotion is permitted.

| Domain | Lesson code | SkillContract | Mechanic | Readiness | V2 shadow solver used for product routing |
| --- | --- | --- | --- | --- | --- |
| algebra-reasoning | `PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-4:APPLY` | `algebra.relationships.two_unknowns` | `TWO_UNKNOWNS` | `READY_VERIFIED` | no |
| measurement | `PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:APPLY` | `measurement.scale.read_equal_intervals` | `SCALE_READING` | `READY_VERIFIED` | no |
| fractions | `PED:CAMBRIDGE-INTL-MATH:S6:6F-3:BUILD` | `fractions.compare.unlike_denominators` | `FRACTION_COMPARE_UNLIKE` | `READY_VERIFIED` | no |
| fractions | `PED:CAMBRIDGE-INTL-MATH:S5:5F-2:BUILD` | `fractions.equivalent` | `FRACTION_EQUIVALENT` | `READY_VERIFIED` | no |
| fractions | `PED:CAMBRIDGE-INTL-MATH:S5:5F-2:APPLY` | `fractions.equivalent` | `FRACTION_EQUIVALENT` | `READY_VERIFIED` | no |

`READY_VERIFIED` here is the generation-readiness audit status from the accepted exact legacy/native mechanic. It does **not** mean that a `ShadowVerified` Mathematics V2 solver family has been promoted to learner-facing production routing.

## Runtime gate

New Stage 17 environment variable:

`EDULYTICS_MATH_V2_GRADE1_6_ROLLOUT`

Values `true` or `1` enable the approved tranche. Any other explicit value disables it. If this variable is unset, the existing `EDULYTICS_MATH_V2_PRODUCT_MIGRATION_PILOT` value is used for backward compatibility. An explicit Stage 17 `false`/`0` always overrides the legacy pilot flag.

The accepted `lesson-grounded-practice-v2.js` runtime and existing renderer/telemetry keys remain unchanged in tranche 1. Unsupported lessons, wrong mechanics, missing values, Grade 7/8 mappings, title-only matches, and shadow-only solver families fail closed to the existing route.

## Promotion rule for later tranches

A lesson may be added only after its exact lesson-skill mapping is approved in `lesson-skill-mappings.v1.json`, its semantic audit is acceptable, its question family/generation capability satisfies the readiness gate, and its exact learner-facing mechanic is explicitly reviewed. Supporting lessons must remain pedagogical mappings and must not receive invented official outcome codes.

## Acceptance invariants

- Grade boundary is 1–6 for this stage.
- Tranche 1 contains the original three accepted Grade 1–6 entries; tranche 2 adds exactly the two reviewed Stage 5 equivalent-fraction entries.
- Exact lesson code and exact mechanic must both match.
- No `UsesV2ShadowSolver=true` entry can route through Stage 17.
- New Stage 17 flag is explicit and fail-closed.
- Legacy pilot flag remains compatible only when the new flag is unset.
- Existing legacy/lesson-grounded fallback remains available as the kill-switch path.
