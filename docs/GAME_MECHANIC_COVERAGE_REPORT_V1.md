# Edulytics Game Mechanic Coverage Report v1

Status: workspace architecture audit complete; per-lesson mechanic manifest remains a separate implementation artifact.

## Scope

The audit covers the four mathematics frameworks already present in Edulytics for grades/stages 1–6.

| Framework | Lessons |
| --- | ---: |
| US Common Core (`US-CCSS-MATH`) Grade 1–6 | 879 |
| Polish National (`PL-NATIONAL-MATH`) Klasa I–VI | 372 |
| UAE MoE (`UAE-MOE-MATH`) Grade 1–6 | 204 |
| Cambridge International (`CAMBRIDGE-INTL-MATH`) Primary Stage 1–6 | 169 |
| **Total** | **1,624** |

No lesson is excluded from the audit scope.

## Architecture decision

Lantern Isles V9 `Join Groups to Add` is one mechanic (`OPERATIONS / JOIN_COMBINE`), not a universal game template.

The required hierarchy is:

`Lesson -> curriculum evidence -> Skill Profile -> GameProfile -> Workspace -> Mechanic -> Interaction Mode -> Scene -> Generated Rounds`

The shared Edulytics shell can remain consistent while the mathematical workspace and interaction change with the learning objective.

## Workspace templates

Ten reusable workspace templates are defined in `GAME_WORKSPACE_TAXONOMY_V1.json`:

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

These are interaction/UI primitives, not ten lesson types. Each workspace owns multiple mechanics and modes.

## Safe routing result

The first pass intentionally split the curriculum into:

- **1,252** lessons whose unit structure safely establishes a workspace.
- **372** lessons requiring deeper lesson-level routing because the unit is broad, mixed, or the pedagogical title is too generic.

All **1,624 / 1,624** lessons remained in the audit.

Deep-routing demand was:

| Framework | Deep-routing lessons |
| --- | ---: |
| Cambridge | 17 |
| Polish | 81 |
| UAE | 69 |
| US | 205 |
| **Total** | **372** |

The deeper pass resolved Cambridge and UAE mixed lessons from lesson-level evidence. The US 205 mixed-unit lessons are deterministically routed by lesson title, standard evidence and exact overrides for misleading surface titles; the verification query returns **0 `NEEDS_REVIEW`** for that mixed set. Thirteen of those lessons are intentionally `COMPOSITE_SESSION` rather than one-mechanic lessons.

## Composite sessions

`One lesson = one mechanic` is not universally true.

US Grade 1/2 `Center Day` lessons intentionally combine previously learned activities. In the audited US mixed-unit set, **13** such lessons are represented as `COMPOSITE_SESSION` and orchestrate multiple child GameProfiles instead of forcing one mechanic.

Supported orchestration modes are `SEQUENCE`, `STUDENT_CHOICE` and `ADAPTIVE_CHOICE`.

## Polish curriculum source resolution

All **372** Polish pedagogical lesson titles in the current dataset are generic (`... — Lesson NN`), so titles cannot safely determine a specific mechanic.

The database preserves the official Polish ELI source and a `SourceLocator` containing the curriculum strand and numbered requirement. The Polish mechanic map therefore routes from that exact source key instead of the generated lesson title.

Verification result:

- Polish lessons in scope: **372**
- Source keys matched: **372**
- Source keys unmatched: **0**
- Distinct requirement keys: **124**

The source-key mapping is in `POLISH_PRIMARY_GAME_MECHANIC_SOURCE_MAP_V1.json`; reproducible verification is in `scripts/polish-game-mechanic-source-audit.sql`.

This resolves requirements into distinct interactions such as number lines, place value, operations, divisibility/factors, fraction models, angle measurement/drawing, triangle construction, perimeter, area, solid nets, time/calendar, money, measurement conversion, statistics and multi-step reasoning.

## US mixed-unit verification

The reproducible US mixed-unit audit is in `scripts/us-mixed-game-workspace-routing-audit.sql`.

Expected current snapshot for the 205 mixed lessons:

- `COMPOSITE_SESSION`: 13
- `DATA_STATISTICS`: 19
- `FRACTION_DECIMAL_PERCENT`: 34
- `GEOMETRY`: 26
- `MEASUREMENT`: 14
- `NUMBER_SYSTEM`: 6
- `OBJECT_COUNTING`: 2
- `OPERATIONS`: 53
- `RATIO_ALGEBRA`: 11
- `REASONING_MODELING`: 7
- `TIME_MONEY`: 20
- `NEEDS_REVIEW`: **0**

Exact overrides are used where a title is misleading. Examples include data lessons with `Shape` in the title, equal-partition lessons linked to geometry standards, and place-value lessons that also carry operations standards.

## Coverage integrity rules

The router must enforce all of the following:

- Never convert `NEEDS_REVIEW` or `UNSUPPORTED` into a generic multiple-choice fallback.
- A scene/background is presentation, not the mechanic.
- The same mathematical skill can reuse a mechanic across curricula while preserving curriculum-specific language, outcome, content and difficulty.
- A curriculum lesson may be a `COMPOSITE_SESSION` when the source genuinely contains multiple activities or domains.
- Round/question order must vary between attempts when valid variants exist.
- Choice positions must not be predictable.
- Spatial layouts should vary when mathematically valid.
- Randomisation must preserve the target learning outcome.
- Curriculum language remains automatic; there is no in-game language selector.

## Mechanic taxonomy

The audit expanded the taxonomy from real curriculum demand rather than inventing one template per lesson. The current machine-readable list is in `GAME_WORKSPACE_TAXONOMY_V1.json` and includes, among others:

`COUNT_TOUCH`, `REARRANGE_RECOUNT`, `NUMBER_LINE_SEQUENCE`, `PLACE_VALUE_BUILD`, `JOIN_COMBINE`, `TAKE_AWAY`, `EQUAL_GROUPS_ARRAY`, `SHARE_DIVIDE`, `WRITTEN_ALGORITHM`, `FACTOR_MULTIPLE_ARRAY`, `FRACTION_PARTITION`, `FRACTION_EQUIVALENCE`, `FRACTION_OPERATIONS`, `DECIMAL_OPERATIONS`, `ANGLE_LAB`, `PERIMETER_TRACE`, `AREA_TILE`, `VOLUME_BUILD`, `SHAPE_NET_BUILD`, `RULER_ALIGN`, `UNIT_CONVERSION`, `CLOCK_FACE`, `MONEY_VALUE`, `BAR_CHART_BUILD`, `RATIO_SCALE`, `BALANCE_EQUATION`, `MULTI_STEP_SCENARIO` and `DESIGN_CONSTRAINT_TASK`.

Related mechanics are expected to share workspace primitives; this is not a proposal to create a separate engine for every mechanic.

## Current implementation state

Already implemented:

- Cambridge Stage 1 `Join Groups to Add` -> `OPERATIONS / JOIN_COMBINE` -> Lantern Isles V9.

Next concept-specific pilot:

- Cambridge Stage 1 `Count, Touch, and Check` -> `OBJECT_COUNTING / COUNT_TOUCH` + `REARRANGE_RECOUNT`.

The new pilot may reuse the shared shell, Eddy, avatar, story, audio, progress, responsiveness and rewards, but it must not reuse the yellow/blue Join Groups mathematical layout.

## Metric distinction

`Workspace routing coverage`, `mechanic mapping coverage`, and `implemented game coverage` are separate metrics.

This v1 audit establishes the workspace/orchestration architecture and reproducible source routing. A generated per-lesson `GameProfile` manifest is the next machine artifact; implementation coverage will increase only as the corresponding reusable mechanics are actually built and tested.