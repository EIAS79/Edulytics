# Edulytics Game Mechanic Coverage Report v1

Status: architecture audit complete; per-lesson runtime manifest not yet materialized.

## Scope

The audit covers the four mathematics frameworks already present in Edulytics for the requested primary grades/stages 1–6.

| Framework | Lessons |
| --- | ---: |
| US Common Core (`US-CCSS-MATH`) Grade 1–6 | 879 |
| Polish National (`PL-NATIONAL-MATH`) Klasa I–VI | 372 |
| UAE MoE (`UAE-MOE-MATH`) Grade 1–6 | 204 |
| Cambridge International (`CAMBRIDGE-INTL-MATH`) Primary Stage 1–6 | 169 |
| **Total** | **1,624** |

No lesson is excluded from the audit scope.

## What the audit changed

The original assumption of one reusable visual game is rejected. Lantern Isles V9 `Join Groups to Add` is now classified as one mechanic (`OPERATIONS / JOIN_COMBINE`) inside a larger game system.

The required hierarchy is:

`Lesson -> curriculum evidence -> Skill Profile -> GameProfile -> Workspace -> Mechanic -> Interaction Mode -> Scene -> Generated Rounds`

The shared Edulytics shell remains reusable; the mathematical workspace is not fixed.

## Workspace layer

Ten workspace templates are defined in `GAME_WORKSPACE_TAXONOMY_V1.json`:

1. Object & Counting World
2. Number & Place Value Lab
3. Operations & Relationships Workshop
4. Fraction, Decimal & Percent Studio
5. Geometry & Spatial Lab
6. Measurement & Scale Lab
7. Time & Money World
8. Data & Statistics Lab
9. Ratio, Proportion & Algebra Lab
10. Reasoning & Modelling Studio

These are interaction/UI primitives, not ten lesson types. Each contains multiple mechanics and interaction modes.

## Direct vs deep routing

A first safe pass found:

- **1,252** lessons whose unit structure can establish the workspace directly.
- **372** lessons that require lesson-level/deeper routing because their unit is mixed or broad.
- **1,624 / 1,624** lessons remain accounted for.

Deep-routing demand by framework:

| Framework | Deep-routing lessons |
| --- | ---: |
| Cambridge | 17 |
| Polish | 81 |
| UAE | 69 |
| US | 205 |
| **Total** | **372** |

Deep routing uses lesson title, linked outcomes/standards, source locator and lesson content. It does not use a generic quiz fallback.

## Composite sessions

The audit found that `one lesson = one mechanic` is also not always true.

For example, US Grade 1/2 `Center Day` lessons intentionally offer several previously learned activities. At least 13 Center Day lessons occur inside the currently identified US mixed-unit set. These are routed as `COMPOSITE_SESSION`, which orchestrates multiple child GameProfiles rather than forcing one mechanic.

Composite sessions can use `SEQUENCE`, `STUDENT_CHOICE` or `ADAPTIVE_CHOICE` orchestration.

## Polish curriculum source resolution

All 372 Polish lesson titles in the current pedagogical dataset are generic (`... — Lesson NN`), so lesson title text alone is not a safe mechanic selector.

The database does, however, preserve an official ELI source URL and a `SourceLocator` containing the curriculum strand plus its numbered requirement. The audit therefore maps Polish lessons from the official requirement key instead of the generated title.

Verification result:

- Polish lessons in scope: **372**
- Source keys matched: **372**
- Source keys unmatched: **0**
- Distinct requirement keys: **124**

The mapping is recorded in `POLISH_PRIMARY_GAME_MECHANIC_SOURCE_MAP_V1.json`.

Examples of the resulting distinction include separate mechanics for number lines, written algorithms, divisibility/factors, fraction equivalence, angle measurement/drawing, triangle construction, perimeter, area, solid nets, time/calendar calculations, unit conversion, data interpretation and multi-step word problems.

## Mechanic taxonomy expansion

The curriculum audit expanded the original mechanic list. Important additions demanded by the real curriculum include:

- `NUMERAL_SYSTEM_CONVERT`
- `OPERATION_PROPERTY`
- `FACTOR_MULTIPLE_ARRAY`
- `PRIME_FACTOR_BUILD`
- `EXPRESSION_ORDER`
- `SHARE_REMAINDER`
- `INTEGER_CONTEXT`
- `DISTANCE_ON_NUMBER_LINE`
- `FRACTION_CONVERT`
- `FRACTION_OPERATIONS`
- `DECIMAL_OPERATIONS`
- `LINE_RELATIONS`
- `SHAPE_NET_BUILD`
- `CIRCLE_CONSTRUCT`
- `AREA_CALCULATE`
- `SURFACE_AREA_BUILD`
- `TEMPERATURE_SCALE`
- `SPEED_DISTANCE_TIME`
- `STRATEGY_GAME`

This list is curriculum-driven; it is not a claim that every mechanic must be implemented as a separate engine. Related mechanics should share workspace primitives.

## Coverage integrity rules

The router must enforce the following:

- Never turn `NEEDS_REVIEW` or `UNSUPPORTED` into a multiple-choice fallback.
- A scene/background is presentation, not a mechanic.
- The same mathematical skill can reuse a mechanic across curricula while preserving curriculum-specific language, outcome, difficulty and content.
- Question/round order must not be fixed between attempts.
- Choice positions must not be predictable.
- Spatial layouts should vary when mathematically valid.
- Randomisation must never change the target learning outcome.
- Curriculum language remains automatic; there is no in-game language selector.

## Current implementation state

Implemented game mechanic:

- Cambridge Stage 1 `Join Groups to Add` -> `OPERATIONS / JOIN_COMBINE` -> Lantern Isles V9.

Next concept-specific pilot:

- Cambridge Stage 1 `Count, Touch, and Check` -> `OBJECT_COUNTING / COUNT_TOUCH` + `REARRANGE_RECOUNT`.

The next pilot must reuse the shared game shell where appropriate, but must not reuse the yellow/blue Join Groups activity layout as the mathematical interaction.

## Remaining audit work after v1

This report establishes the global workspace/orchestration architecture and resolves the Polish source-granularity problem. The next audit artifact will materialize the final per-lesson `GameProfile` manifest and mechanic-level coverage totals across all 1,624 lessons.

Until that manifest exists, `workspace coverage` and `mechanic implementation coverage` must not be presented as the same metric.