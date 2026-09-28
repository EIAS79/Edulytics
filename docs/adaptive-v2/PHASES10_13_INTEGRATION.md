# Adaptive Practice V2 — Phases 10–13 integration

This document closes the product-integration layer that sits on top of the
already-deployed Adaptive Practice V2 runtime.

## Phase 10 — My Next Steps

Source of authority:
- `StudentSelfEvaluationService`
- official mastery/evidence
- prerequisite weakness
- retention concern
- Practice-to-Assessment transfer gap

Adaptive V2 does not invent a second recommendation model. The feature flag
`EnableDirectNextSteps` exposes the existing evidence-driven next-step model
through the Adaptive Intelligence V2 service.

## Phase 11 — Question Log

The log is projected from persisted:
- `AdaptivePracticeSession`
- `AdaptivePracticeTurn`
- `AdaptiveDecisionSnapshot`
- generated `AssessmentItem`

It is owner-only and shows the decision reason, family, representation,
complexity, misconception focus and independent-confirmation status.

## Phase 12 — Live Classroom Intelligence

The live classroom snapshot combines:
- authorized class evaluation from the existing Analytics service;
- Adaptive sessions started in the last 30 minutes;
- latest Adaptive turns;
- unresolved misconception state.

The endpoint remains protected by the existing `AnalyticsRead` policy.
It never widens a teacher/supervisor's class access.

## Phase 13 — Diagnostic V2

The Adaptive Intelligence integration calls the existing
`AdaptiveDiagnosticAssessmentEngine` with exact lesson Outcome scope.
Formal teacher assessments are not adapted item-by-item.

## Rollout safety

All four features require:
- Adaptive V2 globally enabled;
- mode other than `Off`;
- their explicit feature flag;
- existing school/curriculum access checks.

Canary school boundaries remain authoritative. Existing Practice V1 and formal
Assessment flows are unchanged outside the exact V2 scope.
