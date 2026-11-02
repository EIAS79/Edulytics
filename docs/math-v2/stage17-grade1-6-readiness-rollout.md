# Stage 17 — Grade 1–6 readiness-driven migration

## Status

**COMPLETE**

Stage 17 is treated as one Grade 1–6 migration stage. Completion does not mean that every primary lesson is forced into learner-facing Mathematics V2 routing. Completion means every Grade 1–6 lesson is accounted for by the deterministic readiness system:

- **READY_VERIFIED** lessons with an approved exact SkillContract and reviewed exact mechanic are production-routed.
- Every other Grade 1–6 lesson remains fail-closed with an explicit readiness reason.
- **ShadowVerified** solver families never become learner-facing production routes merely because they pass shadow verification.
- Title similarity cannot promote a lesson.

The machine-readable source of truth is:

src/Edulytics.Core/Mathematics/Curriculum/stage17-grade1-6-production-manifest.v1.json

The CI closure gate is:

tools/math_intelligence/stage17_grade1_6_closure_audit.py

## Production-routed Grade 1–6 lessons

| Domain | Lesson code | SkillContract | Mechanic |
| --- | --- | --- | --- |
| algebra-reasoning | PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-4:APPLY | algebra.relationships.two_unknowns | TWO_UNKNOWNS |
| measurement | PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:APPLY | measurement.scale.read_equal_intervals | SCALE_READING |
| fractions | PED:CAMBRIDGE-INTL-MATH:S6:6F-3:BUILD | fractions.compare.unlike_denominators | FRACTION_COMPARE_UNLIKE |
| fractions | PED:CAMBRIDGE-INTL-MATH:S6:6F-3:APPLY | fractions.compare.unlike_denominators | FRACTION_COMPARE_UNLIKE |
| fractions | PED:CAMBRIDGE-INTL-MATH:S5:5F-2:BUILD | fractions.equivalent | FRACTION_EQUIVALENT |
| fractions | PED:CAMBRIDGE-INTL-MATH:S5:5F-2:APPLY | fractions.equivalent | FRACTION_EQUIVALENT |
| fractions | PED:US-CCSS-MATH:G3:U05:L10 | fractions.equivalent | FRACTION_EQUIVALENT |
| fractions | PED:US-CCSS-MATH:G3:U05:L11 | fractions.equivalent | FRACTION_EQUIVALENT |
| fractions | PED:US-CCSS-MATH:G3:U05:L12 | fractions.equivalent | FRACTION_EQUIVALENT |
| fractions | PED:US-CCSS-MATH:G4:U02:L07 | fractions.equivalent | FRACTION_EQUIVALENT |
| fractions | PED:US-CCSS-MATH:G4:U02:L08 | fractions.equivalent | FRACTION_EQUIVALENT |
| fractions | PED:US-CCSS-MATH:G4:U02:L10 | fractions.equivalent | FRACTION_EQUIVALENT |
| fractions | PED:US-CCSS-MATH:G4:U02:L11 | fractions.equivalent | FRACTION_EQUIVALENT |
| ratio | PED:US-CCSS-MATH:G6:U03:L07 | ratio.unit_rate | UNIT_RATE |

All entries above are required to be **READY_VERIFIED**, Grade 1–6, exact-code matched, and UsesV2ShadowSolver=false.

## Exact runtime coverage

The accepted learner-facing runtime supports exactly the Stage 17 mechanics needed by the production manifest:

- TWO_UNKNOWNS
- SCALE_READING
- FRACTION_COMPARE_UNLIKE
- FRACTION_EQUIVALENT
- UNIT_RATE

Equivalent-fraction lessons use representation-specific variants when the exact lesson target requires a number line or factor reduction. Unit-rate practice verifies both the rate per one unit and equivalent ratios preserving that rate.

## Runtime gate

Environment variable:

EDULYTICS_MATH_V2_GRADE1_6_ROLLOUT

Values true or 1 enable the approved Stage 17 production list. Any other explicit value disables the Stage 17 production classification. If unset, EDULYTICS_MATH_V2_PRODUCT_MIGRATION_PILOT remains the backward-compatible fallback. An explicit Stage 17 false or 0 overrides the legacy flag.

## Closure invariants

Stage 17 CI fails if any of the following occurs:

- a Grade 1–6 lesson is **READY_VERIFIED** but is absent from the production manifest;
- a lesson appears in the production manifest but is not **READY_VERIFIED**;
- a manifest entry has no approved exact lesson-skill mapping;
- the manifest SkillId does not match the approved primary SkillContract;
- the manifest mechanic does not match the reviewed exact legacyGameMechanic;
- any production entry uses a shadow-only solver;
- any approved Grade 1–6 mapping is missing from the manifest;
- a fail-closed Grade 1–6 lesson has no readiness reason.

This makes the Stage 17 exit fail-closed and auditable without forcing unresolved, semantically weak, ambiguous, or unsupported lessons into production.

## Next programme stage

With the Stage 17 closure gate green on main and the same commit live on Render, execution proceeds to **Programme Stage 18 — Practice migration**.
