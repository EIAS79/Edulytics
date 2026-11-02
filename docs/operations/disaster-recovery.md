# Edulytics Disaster Recovery Runbook

## Scope

This runbook applies to the current Render staging service and the Neon database it actually uses.

- Render service: `edulytics-staging` (`srv-da1o4url550s73aecsn0`)
- Neon project: `Edulytics-Staging` (`gentle-star-10318776`)
- Region: `aws-us-east-2`
- Database: `neondb`
- Active Render database endpoint: `ep-twilight-grass-a5wy6b75.us-east-2.aws.neon.tech`
- Current default branch after the Phase 18 restore drill: `staging` (`br-mute-hall-a5piyqte`)
- Neon history retention observed during the drill: 21,600 seconds (6 hours)

Do not use the older `Edulytics` project (`steep-smoke-19024913`) for this staging recovery procedure unless Render is deliberately repointed and verified first.

## Backup state

The project supports manual Neon snapshots. During the Phase 18 drill, the project did not permit creation of an automatic snapshot schedule (`backup schedule creation is not enabled for this project`). Therefore scheduled backup is a platform/plan capability gap and must not be represented as enabled.

A manual PITR snapshot was created successfully for `2026-09-14T23:10:00Z`:

- Snapshot: `phase18-pitr-20260914-2310z`
- Snapshot ID: `snap-tiny-queen-a5mi8iw1`

Keep at least one verified recovery point before a risky production/staging database operation. Automatic backup scheduling should be enabled when the Neon project/plan exposes that capability.

## Restore drill evidence

Phase 18 executed two restore paths.

### Current-state snapshot restore

A manual snapshot taken immediately before the drill was restored successfully. The restored database contained:

- 73 public tables
- latest EF migration: `20260914100000_AutoAssignMathSupervisorsAndRefreshMastery`

The original branch was retained rather than deleted. The initial drill used Neon finalize behavior, which swaps branch naming/compute attachment. For future non-emergency drills, do **not** finalize the restore.

### Point-in-time restore

A PITR snapshot for `2026-09-14T23:10:00Z` was restored with `finalize: false` to a separate branch:

- Restore branch: `phase18-pitr-restore-check`
- Restore branch ID: `br-square-bar-a591mk44`

Verification against the restored database returned:

- database: `neondb`
- 73 public tables
- latest EF migration: `20260914100000_AutoAssignMathSupervisorsAndRefreshMastery`

This proves that a recovery point inside the configured history-retention window can be materialized and queried independently without replacing the active branch.

## Standard backup procedure

1. Confirm the active Render service and its current Neon endpoint before touching Neon.
2. Resolve the endpoint to the exact Neon project and branch.
3. Confirm the source branch is `ready`.
4. Create a manual snapshot immediately before a high-risk database operation.
5. Record the snapshot ID, source branch ID, timestamp/LSN, and purpose in the change record.
6. Do not delete the most recent known-good recovery point until the change has been verified.

## Standard PITR verification procedure

1. Confirm the requested timestamp is inside Neon's current history-retention window.
2. Create a snapshot from the source branch at the requested timestamp.
3. Restore the snapshot to a **new branch with `finalize: false`**.
4. Verify at minimum:
   - database can be opened;
   - expected application schema exists;
   - EF migration history is present and expected;
   - selected integrity/read-only smoke queries succeed.
5. Never point Render at the drill branch.
6. Never finalize a drill restore.
7. Delete drill branches only through an explicitly approved cleanup operation after evidence has been recorded.

## Emergency recovery procedure

1. Stop or isolate write traffic if continued writes could worsen corruption.
2. Confirm the last known-good time/LSN and ensure it is inside the available retention window or represented by a retained snapshot.
3. Restore to a new branch first.
4. Run read-only integrity checks and application smoke checks against the restored branch.
5. Only after validation, deliberately switch the application connection or use Neon's finalize operation as part of the approved incident procedure.
6. Verify Render starts, database/background workers connect, and HTTP health/runtime checks pass.
7. Preserve the displaced branch until post-incident review is complete.

## Current limitations and required follow-up

- Automatic snapshot scheduling is not available on the current Neon project capability/plan.
- The observed PITR/history window is 6 hours; a longer recovery window requires a Neon capability/plan change.
- Restore-test branches created during the Phase 18 drill are intentionally retained until cleanup is explicitly approved.
- Database credentials must never be written to this runbook, source control, PR comments, or deployment logs.
