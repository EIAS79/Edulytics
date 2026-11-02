# Edulytics Game Mechanic Coverage Architecture

Status: Draft v0.2 — curriculum coverage audit foundation

## Purpose

Edulytics must not scale one visual activity (for example, Lantern Isles `Join Groups to Add`) across every mathematics lesson. The game system must select an interaction model from the mathematical skill and learning evidence required by the lesson.

The architecture is therefore:

`Curriculum Lesson -> Skill Profile -> Workspace Template -> Mechanic Family -> Interaction Mode -> Scene Configuration -> Generated Rounds`

The public game identity can stay consistent (Eddy, student avatar, story, narration, HUD, progress, rewards, accessibility and responsive/fullscreen behaviour), while the activity workspace changes with the mathematics.

## Audited curriculum scope

The audit covers grades/stages 1–6 only for the four mathematics frameworks already present in Edulytics staging:

| Framework | Levels | Lessons in scope |
| --- | --- | ---: |
| American Mathematics — Common Core State Standards (`US-CCSS-MATH`) | Grade 1–6 | 879 |
| Polish National Curriculum Mathematics (`PL-NATIONAL-MATH`) | Klasa I–VI | 372 |
| UAE Ministry of Education Mathematics (`UAE-MOE-MATH`) | Grade 1–6 | 204 |
| Cambridge International Mathematics (`CAMBRIDGE-INTL-MATH`) | Primary Stage 1–6 | 169 |
| **Total** | | **1,624** |

Important scope rules:

- US Grade 1–6 is selected by `NativeLevel`, because internal logical levels are shifted by Kindergarten (Grade 1 is logical level 2; Grade 6 is logical level 7).
- Polish primary scope is selected by `LogicalLevelFrom BETWEEN 1 AND 6`. `Klasa I/II/III/IV` names also occur in Liceum/Technikum and must not be selected by name alone.
- The current British-facing framework in the database is Cambridge International Mathematics; there is no separate `British National Curriculum` framework in the audited database.
- Curriculum language remains curriculum-owned. The game router must not introduce an in-game language selector.

## Routing quality discovered by the audit

A lesson cannot always be routed safely from `UnitTitle` alone. The first coarse pass understated this problem, so the audit was tightened: broad units that mix mathematical domains are now deliberately sent to deep routing rather than being force-mapped.

| Routing class | Lessons | Meaning |
| --- | ---: | --- |
| Direct workspace routing | 1,252 | Unit structure is sufficiently specific to establish the mathematical workspace before mechanic-level routing. |
| Deep routing required | 372 | Unit mixes domains or its pedagogical title/content is too generic; route from lesson title, learning outcomes, official source locator and lesson content. |
| **Total** | **1,624** | Every in-scope lesson remains accounted for. |

Deep-routing count by framework:

- Cambridge: 17 lessons.
- Polish: 81 lessons.
- UAE: 69 lessons.
- US Common Core: 205 lessons.

Examples that correctly require deep routing include:

- Cambridge: `Number Sense and Sequences`, `Shape, Measure and Position`, `Fractions and Money`.
- Polish: `Obliczenia praktyczne`, early mathematical-text reading/application strands, and broad early number/spatial strands.
- UAE: `Measurement`, `Number Sense`, `Number`, `Number and Calculation`, `Number and Operations`, `Geometry and Data`.
- US: `Putting It All Together`, `Adding, Subtracting, and Working with Data`, `Geometry and Time`, `Geometry, Time, and Money`, mixed measurement units, and other cross-domain units.

**Rule:** a mixed unit is never assigned a fallback quiz. The router must resolve the underlying skill; if the evidence is insufficient, the lesson becomes `NEEDS_REVIEW`.

## Provisional direct-workspace distribution

These counts are only the safe direct-routing set. The 372 deep-routing lessons will be added to these workspaces after lesson-level analysis.

| Workspace | Direct lessons currently routed |
| --- | ---: |
| Operations & Relationships | 374 |
| Fraction, Decimal & Percent | 257 |
| Geometry & Spatial | 259 |
| Number & Place Value | 147 |
| Ratio, Proportion & Algebra | 102 |
| Data & Statistics | 49 |
| Measurement & Scale | 35 |
| Reasoning & Modelling | 27 |
| Time & Money | 2 |
| Object & Counting | resolved at lesson/mechanic level inside broad early-number units |
| **Deep routing pending** | **372** |

The distribution is not a statement that there are only these lesson types. It identifies the top-level UI/workspace primitive; individual mechanics inside each workspace remain distinct.

## Workspace Templates v1

The following are UI/gameplay workspaces, not curriculum categories. A workspace owns the interaction canvas/layout primitives needed for a family of mathematical actions. Each workspace contains multiple mechanic families and modes.

### 1. Object & Counting World (`OBJECT_COUNTING`)

For early one-to-one counting, cardinality, zero, subitising, sorting/classifying, ordinal position and concrete quantity comparison.

Example mechanics: `COUNT_TOUCH`, `SUBITIZE_PATTERN`, `SORT_CLASSIFY`, `ORDINAL_POSITION`, `REARRANGE_RECOUNT`.

Example: Cambridge Stage 1 `Count, Touch, and Check` -> `OBJECT_COUNTING / COUNT_TOUCH`.

### 2. Number & Place Value Lab (`NUMBER_SYSTEM`)

For number lines, place value, composing/decomposing base-ten quantities, natural/integer/rational positioning, sequences, powers of ten, scale intervals, comparing and ordering numerical magnitude.

Example mechanics: `NUMBER_LINE_SEQUENCE`, `PLACE_VALUE_BUILD`, `PLACE_VALUE_SCALE`, `COMPARE_ORDER`, `POWER_OF_TEN_SHIFT`, `READ_SCALE_INTERVALS`.

### 3. Operations & Relationships Workshop (`OPERATIONS`)

For addition, subtraction, multiplication, division, number facts, inverse relationships, equal groups, arrays, written algorithms and operation-based story structures.

Example mechanics: `JOIN_COMBINE`, `TAKE_AWAY`, `DIFFERENCE_COMPARE`, `EQUAL_GROUPS_ARRAY`, `SHARE_DIVIDE`, `FACT_FLUENCY`, `WRITTEN_ALGORITHM`, `RELATIONSHIP_BALANCE`, `DISTRIBUTIVE_BUILD`.

Current Lantern Isles V9 `Join Groups to Add` belongs here as one mechanic template: `OPERATIONS / JOIN_COMBINE`. V9 is **not** the global game template.

### 4. Fraction, Decimal & Percent Studio (`FRACTION_DECIMAL_PERCENT`)

For equal partitioning, fraction models, fractions on a line, equivalence, comparison, mixed/improper forms, fraction operations, decimal representations and percentages.

Example mechanics: `FRACTION_PARTITION`, `FRACTION_MODEL`, `FRACTION_NUMBER_LINE`, `FRACTION_EQUIVALENCE`, `FRACTION_COMPARE`, `FRACTION_DECIMAL_LINK`, `PERCENT_MODEL`.

### 5. Geometry & Spatial Lab (`GEOMETRY`)

For 2D/3D properties, construction/composition, polygons, lines, angles, symmetry, coordinates, transformations, perimeter, area and volume when the shape itself is the central object of interaction.

Example mechanics: `SHAPE_IDENTIFY_COMPARE`, `SHAPE_BUILDER`, `ANGLE_LAB`, `SYMMETRY_MIRROR`, `COORDINATE_SHAPE`, `PERIMETER_TRACE`, `AREA_TILE`, `VOLUME_BUILD`.

A perimeter or area lesson must manipulate the geometric object; it must not reuse the coloured-circle addition activity.

### 6. Measurement & Scale Lab (`MEASUREMENT`)

For length, mass, capacity, measurement instruments, physical scales, estimation, unit selection and conversion when measurement is the primary concept.

Example mechanics: `MEASURE_COMPARE`, `MEASURE_TOOL`, `RULER_ALIGN`, `SCALE_READ`, `CAPACITY_FILL`, `MASS_BALANCE`, `UNIT_CONVERSION`, `ESTIMATE_MEASURE_CHECK`.

### 7. Time & Money World (`TIME_MONEY`)

For clocks, elapsed/ordered time, calendars, coins/notes, monetary value and change. Local currency assets/content are curriculum/context configuration, not engine logic.

Example mechanics: `CLOCK_FACE`, `TIMELINE`, `CALENDAR_ORDER`, `MONEY_VALUE`, `MAKE_AMOUNT`, `CHANGE_TRANSACTION`.

### 8. Data & Statistics Lab (`DATA_STATISTICS`)

For classifying data, collecting/organising records, tables, pictograms, bar/line plots, charts, distributions, comparison and statistical summaries.

Example mechanics: `DATA_SORT`, `TABLE_BUILD`, `PICTOGRAM_BUILD`, `BAR_CHART_BUILD`, `PLOT_READ`, `DISTRIBUTION_COMPARE`, `STATISTIC_CALCULATE`.

### 9. Ratio, Proportion & Algebra Lab (`RATIO_ALGEBRA`)

For ratios, rates, proportional reasoning, unknowns, expressions, equations, variables and linked additive/multiplicative relationships.

Example mechanics: `RATIO_SCALE`, `DOUBLE_NUMBER_LINE`, `BALANCE_EQUATION`, `EXPRESSION_BUILD`, `UNKNOWN_MODEL`, `FUNCTION_PATTERN`.

### 10. Reasoning & Modelling Studio (`REASONING_MODELING`)

Reserved for lessons whose learning evidence is genuinely multi-step modelling, proof/justification, non-routine problem solving or interpretation of mathematical text. It is **not** a fallback workspace for unresolved lessons.

Example mechanics: `MODEL_SELECT`, `EVIDENCE_ORDER`, `CONSTRAINT_PUZZLE`, `MULTI_STEP_SCENARIO`, `ERROR_ANALYSIS`.

## Shared Game Shell vs activity workspace

The following remain shared across workspaces:

- Edulytics identity and original visual language.
- Eddy guide and student avatar.
- Story/narration system.
- Progress, score/rewards and mastery evidence hooks.
- Immediate feedback and progressive hints.
- Touch + mouse + keyboard accessibility.
- Responsive/fullscreen shell.
- Reduced-motion handling.
- Curriculum-controlled language.
- Analytics event contract.

The following must be workspace/mechanic specific:

- Primary interactive object(s).
- Geometry of the play area.
- Drag/tap/draw/measure/rotate/construct interaction.
- Validation model.
- Misconception detection.
- Hint sequence.
- Round generator.
- Visual scene constraints.

## Required GameProfile contract

Every playable lesson must resolve to a `GameProfile` before rendering:

```json
{
  "lessonCode": "PED:CAMBRIDGE-INTL-MATH:S1:L01",
  "learningOutcomeCodes": ["CAM:OUT:0096:1Nc.01", "CAM:OUT:0096:1Np.01"],
  "workspace": "OBJECT_COUNTING",
  "primaryMechanic": "COUNT_TOUCH",
  "secondaryMechanics": ["REARRANGE_RECOUNT"],
  "interaction": ["tap"],
  "manipulatives": ["scene_objects"],
  "misconceptions": ["double_count", "skip_object", "spacing_changes_quantity"],
  "generator": {
    "roundCount": 10,
    "randomizeRoundOrder": true,
    "randomizeLayout": true,
    "avoidImmediateReplay": true
  },
  "status": "MAPPED"
}
```

Allowed mapping status values for the audit are:

- `MAPPED` — evidence is sufficient and the lesson has an appropriate workspace/mechanic.
- `NEEDS_REVIEW` — curriculum evidence is too generic/ambiguous to choose a mechanic safely.
- `UNSUPPORTED` — a required mathematical interaction is known but no workspace/mechanic exists yet.

No production lesson may silently turn `NEEDS_REVIEW` or `UNSUPPORTED` into a multiple-choice fallback.

## Replay/randomisation requirements

A mechanic template must not hard-code one fixed sequence of questions or one fixed answer position.

At minimum:

- Generate/select rounds from a pool larger than the visible session.
- Shuffle round order per new attempt.
- Randomise valid spatial layouts where the learning objective permits it.
- Shuffle answer choices independently when choices are used.
- Avoid placing the correct answer in a predictable position.
- Avoid immediate replay of the same ordered set when sufficient variants exist.
- Preserve mathematical validity and outcome alignment after randomisation.

## Scene policy

A scene/world is a presentation layer, not the mathematical mechanic.

Lantern Isles may be reused as an Edulytics world, but different mechanics can use different scene packs (forest, bridge, workshop, harbour, observatory, geometry lab, etc.). A background does not need to change every round, but camera target, interactive props, local layout, effects and task geometry must avoid the appearance of the same static worksheet over one image.

## Acceptance criteria for the coverage audit

Before broad implementation begins, the audit must produce:

1. All 1,624 in-scope lessons enumerated.
2. Every lesson assigned a skill profile or explicitly marked `NEEDS_REVIEW`.
3. Every resolved skill assigned a workspace and mechanic family.
4. `UNSUPPORTED` list generated from real curriculum demand, not guessed template lists.
5. Coverage totals by framework and grade.
6. A review queue for ambiguous curriculum records.
7. No fallback quiz used to claim coverage.

The target report is:

`Lessons in scope: 1624`

`Mapped: X`

`Needs review: Y`

`Unsupported mechanics: Z`

Only after this report is stable should Edulytics implement additional reusable workspace engines.