# Evaluation System Production Readiness

## Scope

This is the production-readiness contract for the Evaluation Engine and the
student/staff analytics surfaces built on it.

The release unit includes:

- Student 360;
- All Students;
- Topics & Skills;
- Assessment-to-assessment comparison;
- term comparison;
- Practice analytics;
- Practice -> Assessment transfer analysis;
- retention detection;
- prerequisite analysis;
- intervention priority;
- student-owned My Progress;
- supervisor cross-class subject analytics;
- student/class evaluation PDFs;
- teacher-created targeted intervention checks.

## Non-negotiable correctness invariants

1. Missing evidence is never converted to 0% mastery.
2. Assessment and Practice remain separate dimensions.
3. Private student Practice never enters staff analytics or staff PDF reports.
4. A student-facing evaluation resolves the student from the authenticated
   account; the browser cannot select another student profile.
5. Subject Supervisors can only read subjects assigned to them.
6. Teachers can only access students in classes/subjects already authorized by
   the existing Analytics scope.
7. Comparable growth uses common evaluated skills rather than treating
   unrelated assessments as equivalent.
8. Targeted intervention checks are only available when the selected
   LearningOutcome has a verified exact Assessment SkillContract matching the
   evaluated skill.
9. Generated intervention questions stay Draft until a teacher reviews and
   approves them.
10. Failed intervention generation must not leave an orphan draft assessment.
11. Evaluation results remain traceable to source evidence.
12. Existing official mastery history is not rewritten by the evaluation layer.

## Required CI gates

A release is not production-ready until all of the following pass on the exact
release SHA:

- Release build;
- full .NET regression suite;
- Phase44 Evaluation regression suite;
- Phase43 privacy/report boundary suite;
- PostgreSQL migration apply;
- EF Core pending-model-change check;
- localization parity;
- architecture gate;
- tenant/IDOR regression gate;
- dependency vulnerability gate;
- evaluation staging-smoke self-test.

## Staging acceptance

Before customer production cutover, deploy the exact merged SHA to the approved
staging environment and verify:

- /health/live = 200;
- /health/ready = 200;
- anonymous access cannot read staff Analytics;
- anonymous access cannot read Student My Progress;
- anonymous POST cannot create an intervention check;
- EN/PL login flow remains valid;
- no new error burst appears in application logs after deploy.

Authenticated acceptance must cover at least one account for each of:

- Teacher;
- Subject Supervisor;
- School Administrator;
- Student.

For those accounts verify:

### Teacher

- Student 360 opens only inside assigned scope;
- All Students contains the complete class roster;
- Topics & Skills drills into affected students;
- exact-contract intervention button appears only when supported;
- targeted check opens Assessment Builder as Draft;
- generated questions require teacher approval before Publish.

### Subject Supervisor

- subject overview contains all authorized classes;
- cross-class gaps show affected student/class counts;
- an unassigned subject is denied.

### Student

- My Progress shows only that student's data;
- private Practice is visible to that student only;
- personalized checks are marked in assessment history;
- another student cannot be selected through route/query manipulation.

### Reports

- Student evaluation PDF renders multiple sections/pages correctly;
- Class evaluation PDF includes all students, not only at-risk students;
- private Practice does not appear in staff reports.

## Performance acceptance

Evaluation cohort operations must continue to normalize the official evidence
stream once per request and reuse it across students. Any change that
re-normalizes the full evidence set inside the per-student loop is a release
blocker.

For large-school qualification, measure:

- Student 360;
- All Students for a normal class;
- Topics & Skills for a normal class;
- Supervisor Subject Overview across all eligible classes;
- Student PDF;
- Class PDF.

Performance evidence belongs with the release artifacts. Do not introduce an
opaque cache that can serve stale mastery without an explicit invalidation
contract.

## Deployment policy

This subsystem follows the repository-wide production policy in
docs/PRODUCTION_DEPLOYMENT.md.

Current project policy still uses the approved staging environment for final
pre-production qualification. Production DNS/hosting cutover is a separate
repository-wide release decision and must not be silently performed by this
feature.

## Rollback

The Evaluation Engine is additive. If a production defect is found:

1. disable/revert the affected Evaluation UI/action commit;
2. do not rewrite StudentOutcomeMastery history;
3. do not delete underlying Assessment/Practice evidence;
4. preserve already published assessments/results;
5. roll back application code to the last accepted SHA;
6. re-run health, privacy and migration gates.

No Evaluation release in this phase requires a destructive database migration.
