# Mathematics V2 product migration pilot acceptance

## Scope

This is the first learner-facing Mathematics V2 product-routing pilot. It is deliberately restricted to the three Cambridge Primary Stage 6 supporting lessons whose lesson identity, content and exact practice semantics were already corrected and audited:

| Lesson code | Canonical SkillId | Existing exact mechanic |
| --- | --- | --- |
| `PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-4:APPLY` | `algebra.relationships.two_unknowns` | `TWO_UNKNOWNS` |
| `PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:APPLY` | `measurement.scale.read_equal_intervals` | `SCALE_READING` |
| `PED:CAMBRIDGE-INTL-MATH:S6:6F-3:BUILD` | `fractions.compare.unlike_denominators` | `FRACTION_COMPARE_UNLIKE` |

No other lesson or mechanic is included in this pilot.

## Routing model

The three skills above are backed by the accepted exact lesson-grounded practice runtime and are already recognized by the generation-readiness audit through their explicit `legacyGameMechanic` bindings. They do not currently have Mathematics V2 ShadowVerified solver/question-family registrations.

Therefore this pilot changes the production routing decision only. It does **not** claim that these three skills have been migrated to a Mathematics V2 solver provider. The accepted exact lesson-grounded renderer remains the contextual generation provider while the pilot proves controlled learner-facing routing, rollback and isolation.

The pilot route uses:

- renderer key: `lesson-grounded-math-v2-pilot`;
- classification source: `math-v2-ready-contextual-pilot`;
- the existing accepted `lesson-grounded-practice-v2.js` runtime.

Existing Mathematics V2 solver families remain at their current ShadowVerified state and this pilot does not change any existing `productionRouting:false` family to true.

## Kill switch

The pilot is controlled by the environment variable:

`EDULYTICS_MATH_V2_PRODUCT_MIGRATION_PILOT`

Only the explicit values `true` (case-insensitive) and `1` enable the pilot. Missing, empty, malformed, `false` and `0` values keep the pilot disabled.

Disabling or removing the variable immediately restores the existing `lesson-grounded` route on the next application start without reverting code or data.

## Fail-closed invariants

A lesson can enter the pilot only when all of the following match the hard allow-list:

1. the exact canonical lesson code;
2. the exact expected mechanic for that lesson;
3. the kill switch is enabled.

A matching title outside the allow-list cannot enter the pilot. A mismatched mechanic for an allow-listed lesson cannot enter the pilot. All non-target routes keep their existing behavior.

## Acceptance gates

The pilot is acceptable only when automated regressions prove:

- all three allow-listed lessons route to the pilot when enabled;
- the kill switch off path preserves the current accepted route;
- a non-allow-listed lesson cannot enter the pilot even with the same title/context;
- a mismatched mechanic fails closed;
- environment parsing is explicit and fail closed;
- the pilot renderer reuses the accepted exact-skill runtime;
- existing exact-lesson alignment regressions continue to pass;
- Mathematics Intelligence CI and all required Phase16 checks pass;
- the merged SHA is deployed by Render and runtime health checks pass.

## Curriculum and data safety

This pilot does not modify curriculum hierarchy, lesson identity, official outcomes, OutcomeCodes, roles, billing, database schema, migrations or seeded academic content. Supporting lessons remain supporting lessons and no official curriculum mapping is invented.
