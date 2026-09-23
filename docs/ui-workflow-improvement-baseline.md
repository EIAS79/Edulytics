# Edulytics UI/Workflow Improvement Baseline

**Baseline main SHA:** `7973b2294e5577b0c0bd2324c6f4c6ba1a717002`  
**Implementation branch:** `feat/ui-workflow-production-pass-20260924`

## Scope

This baseline maps the requested Admin, Supervisor, Teacher and Student workflow improvements to the current implementation before broader UI migration.

| Issue | Current implementation | Root-cause layer | Planned action | Status |
|---|---|---|---|---|
| Practice multi-tab correctness | Standard Practice answers bind `attemptId + attemptItemId`; Stage-22 exact games use an authenticated protected `roundToken` containing actor, adoption, lesson, family and generated parameters | Correctness/regression protection | Preserve server-authoritative design and add explicit concurrent-tab regression tests | Regression coverage added |
| Curriculum Core / Extended duplication | Cambridge IGCSE Core and Extended are intentionally separate `CurriculumLevelIdentity` records with distinct stable keys; selector was rebuilding labels from `Label + Pathway` instead of using canonical `DisplayLabel` | UI projection/presentation | Keep both legitimate pathways and render canonical pathway + logical-level label | Fixed on branch |
| Manage Users scalability | `SchoolUsersController.Index` calls `ListAsync`; repository loads every school user and the view renders one flat table | Query contract + service + UI | Add server-aware search/filter/pagination and role sections; enrich academic context where supported | Open |
| Academic structure / enrollment | Existing Phase-39 work already separates normal student creation from class movement and has explicit curriculum-aware classes | Shared academic UX | Reuse existing explicit curriculum/class model; improve class-first browsing and movement selection rather than replace it | In audit |
| Teacher assignment | Existing Phase-39 workflow supports multiple class assignment and hides redundant subject choice for Mathematics | Shared academic UX + authorization | Reuse existing assignment path; add scalable filtering/context around it | In audit |
| Curriculum overlap | Curriculum/academic screens contain large tables/forms and responsive CSS/runtime layers | Layout/CSS | Reproduce at target widths, fix grid/wrapping/overflow at root layout rules | Open |
| Student dashboard clarity | Current dashboard exposes school/class, active-assessment count, learning links, active assessments and recent results | Information architecture + available data | Separate actionable assessment states and add only metrics backed by workspace data | Open |
| Reporting richness | Phase-43 reporting already enforces scoped report filters and renders student outcome evidence; current student report remains primarily tabular | Presentation + report projection | Add structured summary/visual hierarchy only from stored/derivable report data | Open |
| Authorization boundaries | User management, Practice and student portal services resolve authenticated school/user scope server-side | Authorization regression | Preserve current handlers/service guards; add regression coverage around any new query endpoints | Ongoing |
| Performance | User management currently loads all school users before presentation; some newer report/curriculum paths are already scoped | Repository/query | Introduce paged server-side user query and avoid client-only full-school filtering | Open |

## Practice findings

### Standard Practice

`PracticeService.AnswerAsync` currently:

1. resolves the owned attempt by authenticated student;
2. requires the attempt to be in progress;
3. loads attempt items for that exact attempt;
4. requires the submitted `attemptItemId` to belong to it;
5. resolves the authoritative assessment item;
6. evaluates with `MathematicsAnswerEquivalence`;
7. saves the response against that `PracticeAttemptItemId`.

No browser-global "current question" is authoritative in this path.

### Stage-22 exact game Practice

`Stage22ExactGameRuntime` is stateless across browser tabs. Each generated round returns an opaque protected token. The protected payload binds:

```text
RuntimeVersion
ActorUserId
CurriculumAdoptionId
LessonId
LessonCode
SkillId
Mechanic
QuestionFamily
Generated Parameters
RoundIndex
IssuedAt
```

Answer evaluation unprotects and validates this scope before checking the submitted answer against the token's own generated problem parameters.

Explicit tests have now been added for:

- different lessons open concurrently;
- alternating submissions;
- correct and incorrect answers staying bound to their own tab;
- same lesson with separate concurrent rounds;
- generating additional rounds in one tab not invalidating another tab's token.

## Curriculum pathway finding

The Cambridge registry intentionally contains separate IGCSE records for:

```text
Logical level 10 — Core
Logical level 10 — Extended
Logical level 11 — Core
Logical level 11 — Extended
```

They have different stable identity keys and must not be deduplicated into one curriculum entity.

The UI must therefore distinguish them explicitly rather than hide one. The academic-level selector has been changed to use the canonical `DisplayLabel`, which includes the native label, pathway and logical level.

## Manage Users finding

Current user listing is not production-scalable:

```text
SchoolUsersController.Index
→ SchoolUserManagementService.ListAsync
→ ISchoolUserRepository.ListBySchoolAsync
→ load all users + all role rows for the school
→ render one table
```

The next implementation phase must change the query contract first, then the UI. A JavaScript-only filter over the current full list would violate the scalability requirement.

## Existing architecture to preserve

The current repository already contains useful foundations that should be extended, not rewritten:

- explicit curriculum-level identities;
- Phase-39 curriculum-aware class UX;
- server-side user-management scope resolution;
- server-authoritative Practice grading;
- Phase-43 report privacy/boundary logic;
- student-portal school/profile access checks.

## Immediate next implementation order

1. Complete Phase-1 regression/identity fixes.
2. Add server-aware user-management query contract.
3. Add role/search/status pagination to Manage Users.
4. Reuse explicit academic context for class/student/staff navigation.
5. Strengthen movement/assignment selection UX.
6. Improve student dashboard using actual workspace states.
7. Improve student report hierarchy using real report data.
8. Run CI, fix regressions, merge only when gates pass.
9. Verify production deployment and health after merge.
