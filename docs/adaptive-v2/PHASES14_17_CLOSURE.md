# Adaptive Practice V2 — Phases 14–17 closure

This closes the remaining Adaptive / Intelligence V2 product plan above the
already-deployed Phase 9 rollout and Phases 10–13 integration.

## Phase 14 — Live Group Session

Phase 14 composes the authorized Live Classroom Intelligence snapshot into
bounded live instructional groups.

Rules:
- maximum group size: 6;
- Remediation groups are first priority;
- Confirmation groups are for learners active in Adaptive Practice without an
  urgent intervention signal;
- Extension groups contain learners without an active intervention signal;
- grouping never widens school/class/subject authorization;
- it reuses the existing realtime Analytics/SignalR infrastructure and does not
  create a competing learner-state source.

## Phase 15 — Psychometric Readiness

The psychometric layer is descriptive and fail-closed.

It reports:
- response count;
- distinct learner count;
- item-family correct rate;
- median response duration;
- top-vs-bottom cohort discrimination estimate when the sample permits it.

Statuses:
- `InsufficientEvidence`;
- `DescriptiveReady`;
- `CalibrationReady`.

The system does not claim psychometric calibration merely because questions
exist. Default minimums are 30 answered responses and 10 learners; stronger
calibration eligibility requires at least 100 responses and 30 learners.

## Phase 16 — Research Programme

Research output is aggregate-only.

Default privacy rules:
- minimum cohort size: 10;
- no learner identifiers in the aggregate contract;
- family-level cells require at least 5 responses;
- only School Admin / Subject Supervisor roles may request research aggregates;
- output remains inside the actor's existing authorized class/subject scope.

## Phase 17 — Production Hardening

The final hardening layer adds:
- explicit Phase 14–16 feature flags;
- startup validation for evidence/resource thresholds;
- maximum intelligence query size;
- Analytics timeout/concurrency gates on staff endpoints;
- no-store responses;
- Adaptive V2 readiness health check on `/health/ready`;
- dependency validation: Live Group Session cannot be enabled without Live
  Classroom Intelligence;
- production telemetry for group planning, psychometric readiness and research
  aggregation;
- rollback remains one configuration change because every new surface is
  feature-gated.

## Default rollout state

All new Phase 14–16 flags remain `false` in repository defaults. Production
activation is allowed only inside the explicit Adaptive V2 rollout boundary.

Advanced Mathematics V3 flags are not changed by this programme.
