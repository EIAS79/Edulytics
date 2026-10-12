# PHASE 03 — Authorized US high-school legacy adoption reconciliation (engineering candidate)

**Academic and release decision: NO-GO.** This runbook records an implemented admin-facing reconciliation workflow, not school approval or completed migration.

## Verified production inventory (read-only, 2026-10-12)

- Eight active legacy school adoption rows remain with exact `CurriculumPathway = "Course/pathway mapping"`: two each at logical levels 10, 11, 12 and 13. Their corresponding levels are displayed as Grades 9, 10, 11 and 12, respectively.
- Live US lessons: 1,560 under `US-CCSS-MATH / CCSSM-2010`. Official import digest is recorded in `00-runtime-evidence-2026-10-12.json`.
- None of those eight rows was changed. A current backup and an authorized, school-specific confirmation remain required before any production write.

## Implemented on PR #464 (not deployed)

- Subject Supervisor may open the school's Academic Structure page, see unresolved `Course/pathway mapping` adoption and choose **one explicit course**. Selection does not default to Algebra 1 or any other course.
- Values come from the committed US Common Core publisher-backed course registry: `ALG1`, `GEO`, `ALG2`, `TRAD-SUPPLEMENT`, `ADV-ALG-FUNC`, `ADV-TRIG-GEO`, `COMPLEX`, `PROB-DECISION`, `VECTOR-MATRIX`. Grade placement is not automatically interpreted as course assignment.
- POST `/school/academic-structure/curriculum-levels/legacy-course` is guarded by school authentication, Subject Supervisor role, antiforgery token, scoped adoption lookup, explicit valid `CourseCode`, matching logical level and active official framework version, subject authorization where configured, duplicate active course check and expected row version/concurrency check. The request cannot select a course on a different school's adoption.
- The mutation affects the existing school's `SchoolCurriculumAdoption` course identity (`CurriculumLevelKey`, label, stage and pathway), but not student attempts, class/grade placement, mastery rows, publisher texts or existing lesson identities. The recorded course selection will affect which course the student's Practice flow can see.
- Tests include a cross-school adoption lookup regression, required course selection in the UI, explicit no-default selection and row-version protection.

## Non-negotiable staging / production acceptance

1. An authorized **representative of each affected school** must independently identify the curriculum course for each adoption. Approval must be attributable to the same school and academic year. Do not infer it from grade, textbook, school name or other schools' choices.
2. Before permitting edits on live school data, confirm current independent backup/recovery with tested rollback and the relevant operational authorization; the previous Neon snapshot request failed at quota. Never bypass the blocker with ad-hoc SQL.
3. On an approved disposable/staging database, exercise `ALG1`, `GEO`, `ALG2` and a `Fourth Courses` option for teacher and student roles with actual authenticated sessions and HTTPS. Check Lesson, Unit and Whole Curriculum Practice, official outcome selection, Assessment, Adaptive and Mastery scopes.
4. Confirm forged-level, stale-row-version, cross-school, unauthorized-role, duplicate-active-course, unresolved legacy and incompatible-grade cases fail without data mutation. Preserve any existing assessment history on rebind.
5. Only after run-specific academic approval and matching SHA CI completion may the release owner schedule one guarded release; then verify post-deploy SHA and persisted course identity without claiming certification prematurely.

**Gate 03: BLOCKED** while eight active school decisions, authenticated HTTPS evidence, issue #453 closure and staging/production operations approval are missing. This engineering branch is draft and must not be merged to trigger a production deployment as an implied authorization.
