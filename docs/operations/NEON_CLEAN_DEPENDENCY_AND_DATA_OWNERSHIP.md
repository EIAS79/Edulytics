# Clean Neon rebuild — dependency and ownership map

**Repository:** EIAS79/Edulytics
**Transition branch:** feat/neon-clean-bootstrap-json-content-20261008
**2026-10-08 — source-level review** (requires CI/integration verification; not production parity evidence).

## Storage and data classification

| Objects / code | Scope | New database action | Dependency / acceptance gate |
|---|---|---|---|
| Identity tables, role definitions and grants | platform definitions, new users only | migrate schema, roles may be bootstrapped, no old users | bootstrap zero-user check; future SuperAdmin is separate |
| Schools, programs, years, grades, classes, enrollments, staff assignments | school tenant | schema only, zero old rows | create new records through product onboarding only |
| CurriculumFramework, CurriculumFrameworkVersion, CurriculumPackContentNode, CurriculumPackNodeLink, CurriculumPackImportState | approved shared source | import from embedded curriculum-pack JSON | exact framework/version and source node checks |
| CurriculumPedagogicalLesson, CurriculumPedagogicalLessonOutcome | approved shared source | materialize from accepted blueprints and official outcomes | pedagogy/outcome mapping and official source validation |
| CurriculumLessonContent | shared status/version index | metadata only in clean JSON mode | compare ContentVersion and published status against JSON index |
| CurriculumLessonContentTranslation | shared lesson prose | **zero rows in clean JSON mode** | controlled, authorized JSON-backed read path |
| LearningLesson, LearningOutcome, CurriculumTopic and school curriculum adoptions | school-scoped materials | schema only, zero old rows | never convert school-edited content into immutable platform JSON |
| Assessments, assessment items/questions/results, attempts, answers | school and student | schema only | E2E tests use throwaway test tenant |
| PracticeAttempt, PracticeResponse, mastery and adaptive states | student/school | schema only | Practice/Adaptive Practice still map to canonical lesson and outcomes |
| ClassOutcomeSummary, analytics projections, refresh state | school/operations | schema only/empty runtime state | worker and aggregation tests |
| BillingInvoice, billing payments/subscriptions, demo requests, outbound notifications | customer operations | schema only | no old financial/contact data |
| AuditLog, Outbox, IdempotencyRecord, DataProtectionKeys | operational | fresh events/keys only | do not migrate old job queue or encryption state |

## Runtime call paths

1. `MathematicsCurriculumPackSeeder`: core framework/version/source JSON → accepted records in PostgreSQL.
2. `MathematicsPedagogicalLessonSeeder`: pedagogy blueprints and official standards → shared lesson IDs and mappings.
3. `MathematicsCanonicalLessonContentSeeder.SeedMetadataOnlyAsync` (new opt-in): canonical pack JSON → only `CurriculumLessonContent` publication/version metadata. It **must** validate exact official outcome codes, just like legacy body seeding.
4. `EmbeddedCanonicalLessonContentIndex` (new): bundled, materialized JSON docs → immutable in-memory `LessonCode` mapping, corrections + validation.
5. `LessonContentRepository.ListCanonicalContentsAsync` (new opt-in): metadata from PostgreSQL + verified lesson identities → JSON content bodies without querying `CurriculumLessonContentTranslations`.
6. `LessonContentService` keeps access control: staff roles and school adoption, enrolled student access and learner publication restrictions.
7. Practice, Adaptive Practice, student results and analytics remain PostgreSQL-native with no legacy tenant rows.

## Explicit clean-mode configuration

Enable on a **new, independent database** only:

    Edulytics__Deployment__CleanBootstrap=true
    Edulytics__LessonContent__ReadFromJson=true
    Edulytics__Deployment__RunStartupDataMaintenance=false
    Edulytics__PresentationDemo__Provision=false
    Edulytics__MeetingDemo__ResetAndSeed=false

No `Edulytics:SuperAdmin:Email` or `Password` during clean initialization. The feature refuses contradictory settings. In clean mode startup maintenance defaults to disabled; the explicit `tools/Edulytics.CleanSeed` command performs initial seeding separately after migrations.

**Do not set `ReadFromJson=true` on the old Neon deployment or before metadata-only content has been created in the new database.** Without matching canonical metadata the lesson API must not expose missing content.

## One-time isolated seed

1. Apply EF Core migrations to a disposable Postgres 18 database.
2. Confirm exactly which account/project/branch/database is targeted.
3. Supply `EDULYTICS_CLEAN_DATABASE_CONNECTION` as a secret; set `EDULYTICS_CLEAN_DATABASE_NAME` to its actual database name and `EDULYTICS_CLEAN_SEED_CONFIRM=I_CONFIRM_NEW_EMPTY_DATABASE`.
4. Run `dotnet run --project tools/Edulytics.CleanSeed/Edulytics.CleanSeed.csproj`.
5. Require 4 curriculum frameworks, canonical metadata count equal JSON index count (5,110 on initial baseline), 0 prose rows, 0 users/schools.
6. No old credentials, snapshots or cross-account transfers needed.

## Known unsatisfied production gates

- PR #418 worker idle-poll backoff is still separate, with incomplete full-suite verification.
- A passing offline JSON count is not proof of educational QA, outcome correctness or customer-facing content.
- Existing school/auth/PQ dependencies cannot be fully certified without isolated database end-to-end tests.
- New Neon account is not presently connected through the available Neon action; old Neon remains quota-blocked and must not be touched.
- Render active `Edulytics` service exists in My Workspace; its actual configuration must not be changed until source/DB/CI gates pass.
- Free plan network egress limits can still be reached. Production consumption can only be assessed after measured traffic.

## Rollback and monitoring

Keep old Render service and old Neon untouched until a safe switch, retain branch/tag and approved DB baseline, record migration version. Monitor idle DB polling, PostgreSQL query result size, network egress, CU-hours, connection counts, error delays, worker lock contention and per-lesson response time. If any invariant fails, stop new release without modifying old Neon.
