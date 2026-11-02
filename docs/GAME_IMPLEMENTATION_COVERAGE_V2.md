# Grade 1–6 Mathematics Game Practice Coverage V2

Status: implementation baseline for reusable, domain-specific student Practice workspaces.

## Scope

The original four-framework Grade/Stage 1–6 audit contains **1,624 pedagogical lessons**:

| Framework | Lessons |
|---|---:|
| US Common Core Mathematics | 879 |
| Polish National Mathematics | 372 |
| UAE MoE Mathematics | 204 |
| Cambridge International Mathematics | 169 |
| **Total** | **1,624** |

The database also contains **249 UK National Curriculum Mathematics** lessons for Years 1–6. The runtime routes those lessons too, as an additional compatibility surface beyond the original 1,624-lesson scope.

## What “covered” means

Coverage does **not** mean 1,624 separately hand-authored mini-games. It means every current in-scope lesson is routed to a reusable mathematical workspace and a mechanic profile that can launch a student-facing interactive Practice session.

The renderer is selected from the lesson’s curriculum code, unit and lesson title. Polish primary lessons with generic titles are routed by the official requirement/core index encoded in the lesson code, consistent with `POLISH_PRIMARY_GAME_MECHANIC_SOURCE_MAP_V1.json`.

## Runtime workspaces

The playable runtime covers these reusable workspace families:

1. `OBJECT_COUNTING` — touch/count/classify/ordinal representations.
2. `NUMBER_SYSTEM` — number line, place value, compare/order and related number representations.
3. `OPERATIONS` — join/take-away/equal groups/share/array and operation models.
4. `FRACTION_DECIMAL_PERCENT` — partitioned bars, equivalence, decimal/percent links and fraction models.
5. `GEOMETRY` — shapes, angles, perimeter/area and geometric representations.
6. `MEASUREMENT` — ruler/scale and measurement interactions.
7. `TIME_MONEY` — clock and money interactions.
8. `DATA_STATISTICS` — chart/data reading and representation.
9. `RATIO_ALGEBRA` — ratio scaling and equation balance representations.
10. `REASONING_MODELING` — scenario/model-selection reasoning.
11. `COMPOSITE_SESSION` — rotates multiple domain primitives for mixed review/Center Day style lessons.

Three lessons retain richer specialized renderers:
- Counting Grove (`COUNT_TOUCH`)
- Angle Observatory (`ANGLE_LAB`)
- Fraction Forge (`FRACTION_EQUIVALENCE`)

All other currently routed lessons use the shared domain runtime rather than a generic quiz fallback.

## Student launch behavior

For a lesson that belongs to the student’s adopted curriculum:

`Student Lesson → Practice → GameLessonRouter → Workspace/Mechanic → Renderer`

The Student Practice controller validates the student/curriculum/lesson relationship before launch. Curriculum codes are not exposed as student instructions inside the game.

## Language

- Polish National Mathematics → `pl`
- UAE MoE Mathematics → `ar` / RTL
- US CCSS, Cambridge and UK National Curriculum → `en`

Narration uses browser speech synthesis only; no paid external AI API or student API key is required.

## Safety boundaries

This coverage change does not modify:
- database schema or migrations,
- curriculum source data,
- mastery logic,
- assessment-core behavior,
- official learning outcomes.

Unsupported framework codes still fail closed instead of receiving a fabricated generic practice alignment.
