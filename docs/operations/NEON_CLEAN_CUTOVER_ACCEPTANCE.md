# Edulytics clean Neon rebuild — status and go/no-go

Date: 2026-10-08
Repository: EIAS79/Edulytics
Implementation PR: https://github.com/EIAS79/Edulytics/pull/419 (draft)
Worker backoff PR: https://github.com/EIAS79/Edulytics/pull/418 (integrated into transition **branch**, not merged into main)
Transition branch: feat/neon-clean-bootstrap-json-content-20261008
Old Neon: **never accessed or modified during this execution**

## Evidence to date

- Offline inventory: four curricula, 69 canonical JSON packs, 50 blueprints, two RichV2 resources, **5,110 distinct canonical LessonCodes** (0 duplicates/structural warnings under the inventory validator).
- Isolated PostgreSQL 18 build: https://github.com/EIAS79/Edulytics/actions/runs/37847439326 **green**.
- Output of clean seed on disposable PostgreSQL:
  - `frameworks=4 pedagogicalLessons=5110 metadata=5110 jsonBodies=5110`
  - `postgresProseRows=0 users=0 schools=0`
  - `CLEAN_POSTGRES_GATE_PASS 0|0|0|5110`
- JSON-backed application reader was implemented as opt-in. Data is served from embedded, reviewed source documents after contextual authorization, and DB keeps only lesson metadata in clean mode.
- Focused .NET build and 14 tests (bootstrap + JSON content retrieval + curriculum order) passed in https://github.com/EIAS79/Edulytics/actions/runs/37847573664.
- PR #418's idle backoff implementation is included in the transition branch via two-parent merge; check and benchmark it with the full suite before merging to main.

## Unresolved content QA gate

The strict Phase29 64-scope audit reports **62/64 complete**. It identifies precisely:
- UAE-MOE-MATH:L05:Advanced
- UAE-MOE-MATH:L06:Advanced

The project's code generator proposes generic OGL-based *unaligned* supporting lessons for these scopes; it cannot replace source-verified official outcomes and mathematical/pedagogical signoff. **Do not silently mark them as published or change the audit to hide them.** Final curriculum QA is BLOCKED pending accepted content or explicit scope decision.

## Stages / status

| Stage | State | Evidence / remaining gate |
|---|---|---|
| 1. Architecture review | implemented | Dependency map: `docs/operations/NEON_CLEAN_DEPENDENCY_AND_DATA_OWNERSHIP.md`. Full runtime audit still needed |
| 2. Neon idle/egress optimization | implemented on branch; operational verification pending | PR #418 branch merge. No real Neon egress measurement possible yet |
| 3. Source inventory and closure | 5,110 unique lesson identities verified; QA blocked | Phase29 strict: two UAE advanced gaps |
| 4. JSON-backed lesson content | implemented opt-in and focused-tested | Full E2E, translations, authorization and performance still required |
| 5. Fresh Postgres schema | verified on ephemeral PostgreSQL 18 | production project pending |
| 6. Curriculum-only seed | verified on ephemeral PostgreSQL 18 | acceptance on target Neon pending |
| 7. Zero-account/zero-demo startup | implemented and focused-tested | real target double-check pending |
| 8. Full CI/E2E gates | in progress | full test workflow and dependency/security gates |
| 9. New Neon account/project | BLOCKED — target credentials/account not available | User must connect/select **new** Neon account/project, not old |
| 10. Render cutover | BLOCKED — no new target DB or successful final gates | Render service `Edulytics` (`srv-dakq5n2fngtc73a62i10`) in `My Workspace`; public static site is separate |
| 11. Live consumption audit | BLOCKED — no new live DB | Measure egress/CU-hours, idle worker traffic, response sizes, failure rates after deploy |
| 12. Production acceptance | BLOCKED — depends on stages 3/8/9/10/11 | No go-live signoff until verification is complete |

## Secure execution order for remaining stages

1. Resolve advanced Grade 5 and Grade 6 UAE scope missing-source gate without inventing official mappings.
2. Complete full test suite, real PostgreSQL integration/tenant isolation and SQL performance regressions.
3. Obtain and confirm access to the **new Neon account/project**, apply reviewed migrations. Secrets must never be pasted into chat or committed.
4. Run `tools/Edulytics.CleanSeed` with exact empty target database name and explicit confirmation; verify version/canonical identity counts.
5. Confirm Render workspace/service and make a controlled environment-variable cutover only after gates green. Do not touch the separate static public site.
6. Establish live monitoring, restore evidence and final go/no-go report. Keep old Neon intact.

## Hard no-go conditions

- Unknown Neon owner/project or pointing at quota-exhausted legacy Neon.
- Any user/school/demo data present on supposedly empty target.
- Database lesson-text rows > 0 in clean JSON mode.
- Failed source, security, access-control or CI gates.
- Missing Render service confirmation, invalid new credentials or absent backup/recovery point.
- No post-cutover quota and query monitoring.

This file is a **status record**, not a claim that all 12 phases have been delivered.


## V2 status update — 2026-10-09

**Architecture choice:** [ADR-0007](../adr/ADR-0007-educational-storage-architecture-evaluation-ar.md) selects JSON-first, minimal-FK hybrid **for staging** only. Production release is still blocked.

**New implementations:**
- Consolidated JSON lesson metadata lookup uses one projected SQL query rather than two; verified for one and 25 lessons on isolated PostgreSQL 18.
- Clean seeding requires explicit project database name and remote host approval. Loopback test database is separately recognized.
- Fail-closed preflight rejects non-reference rows in all application tables, not only the previous short list of tenant tables.
- Fixed whitespace causing the production-regression workflow to fail. New CI verification is pending.
- UAE source-gap report: [Grades 5 and 6 Advanced](./UAE_ADVANCED_GRADES_5_6_SOURCE_GAP_2026_2027_AR.md). A deterministic generator was invoked in an isolated GitHub Action, which correctly stopped at missing `SourceCatalog` for G5 Advanced; no placeholder lessons were merged.

**Still unsatisfied:**
1. Phase29 strict `64/64` educational scope acceptance: source catalogs for Grade 5/6 Advanced not verified, current `62/64`.
2. Final latest-head CI and full end-to-end practice/authorization/performance gates.
3. New independent Neon Free **account/project access**, which is not established by the current connector. Old exhausted project must never be reused.
4. Migration and clean seed on the correct new Neon, Render `Edulytics` (`srv-dakq5n2fngtc73a62i10`) release cutover, post-deploy consumption, monitoring and restore drill.

**What has NOT happened:** No writes to legacy Neon; no new Neon project created through this ChatGPT session; no Render environment edits/deploy; no merge to `main`; no migration of user/school/student/staff/customer data.

**Release gate:** DO NOT MERGE/DEPLOY until all four open groups above pass.


## Identified replacement Neon target — 2026-10-09 (no API access yet)

The account owner's Neon Console screenshot provides:
- Project display name: `Edulytiks`
- Project ID: `tiny-lab-44877119`
- Default branch display name: `production`
- Branch ID: `br-frosty-block-b52tnjky`
- Organization account shown: `247abcnews@gmail.com`
- Region shown: AWS US East 2 (Ohio)
- Console: https://console.neon.tech/app/projects/tiny-lab-44877119/branches/br-frosty-block-b52tnjky

**Connector access check:** Neon `describe_project`, `list_branches` and `list_postgres_databases` with these explicit IDs returned `HTTP 404` authorization/internal-access errors. This **does not establish** a working write connection to this project; cannot migrate/seed or switch Render based on screenshot alone. Ask the operator to authorize/select the new Neon account with matching project access. Do not fall back to the suspended old project.

**No database change:** The new Neon project and the old Neon project have not been accessed through SQL by this plan. The clean PostgreSQL 18 tests are isolated GitHub Actions test databases.

## Verified new Neon connection and isolated staging — 2026-10-09

The Neon connector was reauthorized and these checks **succeeded**:

- New project `tiny-lab-44877119` (`Edulytiks`), owning organization `org-icy-sea-65672687` and owner email `247abcnews@gmail.com`. Current connector project permission: `ADMIN`. PostgreSQL: 18, region `aws-us-east-2`, Free tier.
- Existing default branch `production`: `br-frosty-block-b52tnjky`; default database `neondb` owned by `neondb_owner`.
- `get_database_tables(project, production, neondb)` returned `[]` (no application tables at this moment). This verifies initial schema absence, not future lifecycle readiness.
- Created **isolated, no-compute** staging branch `edulytics-clean-staging-20261009`, ID `br-ancient-smoke-b5694k1f`, copied from empty production. Neon confirmed branch `ready` with no compute time or activity.
- This branch is for migration and smoke-test validation; no SQL migrations or seed were applied to Neon at this point. The original production branch remains untouched.

**Previous authorization error resolved.** Remaining go-live gates: verify UAE mathematics source status for grades 5/6 Advanced without speculation; source coverage/CI tests; allocate staging compute only when required; apply EF migrations and approved JSON metadata-only seeding to the staging branch; production cutover and quotas/rollback after all acceptance tests. Do not use the exhausted old Neon project.

