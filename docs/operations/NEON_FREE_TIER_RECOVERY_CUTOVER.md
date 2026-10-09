# Neon free-tier recovery and safe database cutover

Updated 2026-10-08. **No database writes, connection-string switches, service redeploys, or billable plan changes have been performed under this runbook.**

## Verified identities (do not infer from similar names)

- Code source of truth: `EIAS79/Edulytics` on GitHub.
- Production/staging application hosting: Render **Edulytics** service `srv-dakq5n2fngtc73a62i10` in the workspace shown as `khalidkhalifa79@gmail.com`. Public site **Edulytics Public** is a separate Render service and must not be substituted.
- Current database's **actual endpoint**, obtained from Render application error logs: `ep-twilight-grass-a5wy6b75.us-east-2.aws.neon.tech`.
- Neon project owning that exact endpoint: `gentle-star-10318776`, **Edulytics-Staging**, under Neon organization **Our-CS** (`info.ourcs@gmail.com`).
- Actual default database branch for that project: `br-mute-hall-a5piyqte` (`staging`).
- Neon plan: `free_v3`. Recorded project outbound data transfer: `5519077633` bytes. Its PostgreSQL error `53000` reports quota exhausted.
- A second project named `Edulytics` also exists in the old Neon org; it does **not** host the failing endpoint and must not be chosen just because its display name matches.

## Phase A: Stop the excessive idle polling (implemented on feature branch)

- Change outbox and analytics idle polling from 300ms/250ms to a minimum of 5 seconds, exponentially increasing to 15 seconds after consecutive empty claims.
- Apply this in the worker implementation and appsettings/default options; sub-second environment overrides will not bypass the new minimum.
- Reset waiting interval when actual queue work is found.
- Back off errors instead of querying rapidly during outages; retain outbox heartbeat readiness (15-second idle cap remains within 60-second stale threshold).
- Run and pass test suite, production startup checks, and inspect Neon data-transfer graphs after restoring capacity. The previous hot polling is a credible contributor, **not proof it caused the full 5.52GB**. Examine query traces, request loads, response sizes, seeding/analytics and any other background loops before claiming the issue closed.
- Never deploy this branch merely to work around a database error; verify tests first.

## Phase B: If a new Neon workspace is legitimately required

A new Neon *account* cannot be created by the Edulytics repository or Neon project API connector. The account owner must register and verify it and connect the intended Neon account to the assistant. Creating multiple free accounts just to evade an existing account's quota might conflict with Neon service terms; verify permitted usage before doing so. Prefer repairing usage and waiting for the existing provider quota reset when possible.

**Data safety:** The only manual snapshot currently listed in the source project is dated 2026-09-14, so it cannot establish preservation of all changes made through October. Do not restore from it as the only copy or replace the live branch. Do not delete the old project, role, branch, or account.

Once database access is available (after quota reset or an allowed provider-side recovery):

1. Confirm source database, schema names, row counts, migration history, school/user/lesson mappings, and migration compatibility. Arrange a maintenance/read-only window so writes do not diverge.
2. Take a **new** time-consistent source backup using PostgreSQL 18-compatible `pg_dump --format=custom --no-owner --no-privileges` (dump connection via a safely handled credential, never embedded in logs). Record SHA-256 and ensure the dump can be read with `pg_restore --list`.
3. Provision a new Neon project in the explicitly selected and authorized workspace, with a verified destination database and role. Keep source and destination separate.
4. Restore with `pg_restore --no-owner --no-privileges --exit-on-error` to the destination and inspect migration table, critical-table row counts, permissions and indexes.
5. Run migrations and application tests against **destination**, compare curriculum lesson counts by stage and registered outcomes, verify sign-in for role-specific accounts and assessment/practice screens. Never call GitHub lesson JSON parity a substitute for reading the actual running app.
6. Back up and verify Render environment variable names before modifying them. Change only the Edulytics **application** service connection strings after explicit destination confirmation; preserve cookie/data-protection keys and secrets. Use direct/non-pooled endpoint for schema migrations where required.
7. Confirm live application health and Render deploy, monitor new Neon egress at least through a representative workday; set alerts/budget controls if available.
8. Retain old database intact for rollback until owner explicitly approves retirement.

**Blockers:** The current source Neon endpoint is refusing connections due to quota, and no newly selected authenticated Neon destination account/workspace has been provided. Do not claim a backup, restore, cutover or production success until these steps have actually occurred.


## 2026-10-09 correction: clean V2 staging exists — production still NO-GO

This dated section supersedes the earlier *as-of-2026-10-08* statements that no replacement account/project or staging deployment exists. The earlier section is retained as historical evidence and should not be interpreted as the current state.

- **Clean V2 rebuild is deliberately not a lift-and-shift:** no legacy users, students, schools, accounts, grades, notifications or other tenant/operational data will be migrated, under the architecture-first clean-rebuild plan. The backup-and-restore procedure in Phase B above describes a different data-preserving migration strategy and **must not be executed as this clean V2 plan**.
- New independently verified Neon Free project: `tiny-lab-44877119` (Edulytiks), PostgreSQL 18, Ohio; original default `production` branch `br-frosty-block-b52tnjky` retained empty. Isolated staging `br-ancient-smoke-b5694k1f` receives V2 migrations and reference-only seed. Never use the legacy exhausted project `gentle-star-10318776`.
- V2 staging Render: `srv-db42982d0e5s73fhvpd0` (`edulytics-v2-neon-staging`, Ohio, Free). Its automatic Git deploys and PR previews are disabled. Original `Edulytics` production Render `srv-dakq5n2fngtc73a62i10` remains in Oregon, following `main` with `checksPass` autoDeploy — merging PR #419 would risk immediate production deployment, so **keep PR #419 draft/unmerged** until go/no-go approval.
- Staging bootstrap applied 30 EF migrations. Direct SQL read-back verified 5,110 pedagogical lesson identities, 5,749 lesson-to-outcome links, 0 users, 0 schools, 0 school subscriptions and 0 canonical lesson-prose translation rows. Passing those structural counts does not establish end-to-end authorization, answer correctness or lesson parity.
- Latest PR-head workflows: clean PostgreSQL 18 rebuild, Common Core official closure, evaluation production regression and Mathematics Intelligence Foundation passed; strict offline content inventory remains failed at 62/64 scopes. Missing `UAE-MOE-MATH:L05:Advanced` and `UAE-MOE-MATH:L06:Advanced` must have authoritative source provenance and academic review, or officially evidenced shared-syllabus mapping. Do not fake content, delete the test, or modify the 64-scope denominator just to pass.
- Render bandwidth has shown a limited 0.52840424 MB sampling bucket; that is **not Neon database egress**, not a load-test benchmark, and not quota signoff. Cross-region production Render Oregon ↔ new Neon Ohio must be latency-tested or Render regional placement explicitly revised.

### Required clean V2 go/no-go sequence

1. Close tracked source gap [issue #420](https://github.com/EIAS79/Edulytics/issues/420) with ministry-authorized mathematics source(s), content/outcome provenance, lawful Edulytics-authored explanations, language and mathematical review, and strict `64/64` PASS. Unknown source must remain blocked, not invented.
2. Run current-HEAD full CI, security/dependency scanning and E2E validation with newly generated synthetic test accounts: school isolation, all roles, authentication, Practice, assessment approval, translation and content version parity. Synthetic users only in disposable test environments, not final clean production.
3. Benchmark A/B/C storage alternatives using equivalent datasets and auth rules: query counts, bytes, p50/p95, cold/warm cache, worker idle polls, memory and real Neon egress/CU-hours. Finalize ADR-0007 from measurements, not estimates.
4. Verify restore/rebuild drill and backout steps; preserve the original service and legacy DB, Render configuration and protection keys. Choose the exact new-production Neon database/branch and ensure **zero tenant rows** before approved initialization.
5. Only after checks: approve PR #419 for merge, controlled Render environment cutover, one monitored production deploy and rollback readiness. Confirm health *and* authenticated academic journeys. Monitor at least a representative operating window and usage budgets. Do not mistake a healthy `/health` for release acceptance.

Current decision: **PRODUCTION NO-GO**. See `docs/operations/NEON_CLEAN_CUTOVER_ACCEPTANCE.md` for the principal status record. No automatic database switch or merge is authorized by this document.
