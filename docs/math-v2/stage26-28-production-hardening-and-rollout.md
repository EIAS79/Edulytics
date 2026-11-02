# Stages 26–28 — Production hardening, observability and controlled rollout

## Status
**COMPLETE — CONTROLLED PRODUCTION ROLLOUT**

The final three stages of the Integrated Mathematics Intelligence Master Execution Plan are closed together in one production-hardening change.

This does **not** globally promote every Mathematics V2 capability. The production boundary remains fail-closed: only an explicitly scoped, READY_VERIFIED and production-capable target may own learner-facing V2 output. ShadowVerified or curriculum-contextual capabilities do not become production claims automatically.

## Security and resource controls

Production defaults now enforce:
- input length: 4096 characters
- token count: 512
- AST depth: 64
- AST nodes: 4096
- polynomial degree: 12
- matrix dimension: 12 × 12
- solver timeout: 2 seconds
- estimated AST/request memory: 8 MiB
- concurrent mathematics operations: 8

The guard is iterative for AST shape validation, rejects over-budget work before execution, and exposes a bounded asynchronous execution budget for timeout/concurrency control.

## Observability

The runtime emits a dedicated `Edulytics.Mathematics` meter with monotonic counters for:
- generation success
- solver success
- verification failure
- alignment rejection
- fallback
- unsupported capability
- timeout
- answer-equivalence disagreement
- content-mapping conflict

The exact SkillContract production generator is instrumented directly.

## Production rollout

Supported rollout modes:
- `LegacyOnly`
- `ShadowV2`
- `V2VerifiedOnly`
- `V2Preferred`
- `V2Only`

New explicit non-legacy rollout modes must include at least one selector:
- domain
- skill
- curriculum
- grade

An explicit `V2Only` decision blocks an unsupported target rather than silently downgrading to legacy generation.

The existing Grade 1–6 rollout flag remains backward-compatible. If the new Stage 28 mode is unset, the existing approved Grade 1–6 gate maps to `V2VerifiedOnly`; if the old gate is disabled, routing remains `LegacyOnly`.

## Capability truthfulness

The following remain deliberately blocked:
- proof capability
- unfamiliar-transfer capability
- global IB AA HL capability claim
- global AS/A-Level 9709 capability claim
- global IGCSE Extended capability claim

That is not an incomplete rollout. It is the intended fail-closed production state until those capabilities receive the evidence and academic approval already required by Stages 23–25.

## Source of truth
- `src/Edulytics.Core/Mathematics/Curriculum/stage26-28-production-rollout-manifest.v1.json`
- `src/Edulytics.Core/Mathematics/Runtime/MathematicsResourceLimits.cs`
- `src/Edulytics.Services/Mathematics/Runtime/MathematicsResourceGuard.cs`
- `src/Edulytics.Services/Mathematics/Runtime/MathematicsObservability.cs`
- `src/Edulytics.Services/Mathematics/Rollout/MathematicsProductionRolloutPolicy.cs`
- `tests/Edulytics.Tests/MathematicsIntelligence/Stage26To28ProductionClosureTests.cs`
- `tools/math_intelligence/stage26_28_production_closure_audit.py`

## Programme position

Stages 0–28 of the Integrated Mathematics Intelligence Master Execution Plan are now implemented as repository code, accepted gates, tests and CI evidence.

Capability gaps remain represented explicitly as unsupported/review-required rather than being converted into false production claims.
