# Current Edulytics Mathematics Programme Plan

**Status:** ACTIVE — GOVERNING EXECUTION PLAN

**Current governing plan:** [`EDULYTICS_INTEGRATED_MATH_CURRICULUM_MASTER_EXECUTION_PLAN.md`](./EDULYTICS_INTEGRATED_MATH_CURRICULUM_MASTER_EXECUTION_PLAN.md)

This document is the repository pointer to the active mathematics/curriculum execution programme.

## Execution hierarchy

1. **Repository code, tests, CI and accepted stage documents** define what has actually been implemented.
2. **The Integrated Mathematics Intelligence Master Execution Plan** defines the governing programme sequence and what should be executed next.
3. **The detailed Advanced Mathematics Engine roadmap** remains the deep technical specification for Track B (Mathematics Intelligence Kernel).
4. **The detailed 1500-Lesson Curriculum Intelligence roadmap** remains the deep curriculum/lesson specification for Track A.
5. Older `PHASE_*_IMPLEMENTATION_PLAN.md` documents are historical phase records unless this current plan explicitly references them.

## Current programme position

- **Programme Stage 17 — Grade 1–6 migration:** COMPLETE.
- **Programme Stage 18 — Practice migration:** COMPLETE.
- **Programme Stage 19 — Assessment Builder migration:** COMPLETE.
- **Programme Stage 20 — Diagnostic / Adaptive migration:** COMPLETE.
- **Programme Stage 21 — Reassessment migration:** COMPLETE.
- **Programme Stage 22 — Game runtime migration:** COMPLETE.
- **Programme Stage 23 — IGCSE Extended gate:** COMPLETE AS A CAPABILITY GATE.
- **Programme Stage 24 — AS/A-Level 9709 gate:** COMPLETE AS A CAPABILITY GATE.
- **Programme Stage 25 — IB AA HL-style gate:** COMPLETE AS A CAPABILITY GATE.
- **Stage 25 truthful status:** 6 advanced-demand classes tracked; Routine, Multi-step, Modelling and narrow Reasoning slices are EVIDENCED through 15 executable benchmarks; Proof and Unfamiliar transfer remain UNSUPPORTED.
- **Stage 25 claim rule:** engine-style evidence does not equal official IB curriculum alignment; global IB AA HL claims and product routing remain blocked, with 0 formal IB mappings and academic review still REQUIRED.
- **Stage 25 source of truth:** src/Edulytics.Core/Mathematics/Curriculum/stage25-ib-aa-hl-style-gate-manifest.v1.json.
- **Stage 25 benchmark corpus:** src/Edulytics.Core/Mathematics/Curriculum/stage25-ib-aa-hl-style-benchmark-corpus.v1.json.
- **Stage 25 CI exit gate:** tools/math_intelligence/stage25_ib_aa_hl_style_gate_audit.py.
- **Programme Stage 26 — Security and resource controls: COMPLETE.**
- **Programme Stage 27 — Observability: COMPLETE.**
- **Programme Stage 28 — Production rollout: COMPLETE.**
- **Final production boundary:** bounded input/AST/solver resources, dedicated Mathematics observability metrics, and scoped rollout modes `LegacyOnly / ShadowV2 / V2VerifiedOnly / V2Preferred / V2Only` are implemented. Explicit non-legacy rollout requires domain/skill/curriculum/grade scope, and unsupported targets fail closed.
- **Backward compatibility:** the existing Grade 1–6 rollout gate remains accepted; when the new rollout mode is unset, enabled approved Grade 1–6 routes map to `V2VerifiedOnly` and disabled routes remain `LegacyOnly`.
- **Capability truthfulness:** ShadowVerified advanced skills are not automatically promoted. Proof and unfamiliar-transfer capability remain UNSUPPORTED, and global IGCSE/9709/IB claims remain blocked pending their existing formal mapping and academic-review gates.
- **Final source of truth:** src/Edulytics.Core/Mathematics/Curriculum/stage26-28-production-rollout-manifest.v1.json.
- **Final CI exit gate:** tools/math_intelligence/stage26_28_production_closure_audit.py.
- **Integrated Mathematics Intelligence Master Execution Plan: COMPLETE** as an execution programme. Future work is continuous curriculum onboarding, academic review of blocked capabilities, regression monitoring, and evidence-based promotion—not another numbered programme stage.

## Update rule

Whenever the programme advances to another stage, update this file in the same PR (or immediately following PR) so the repository always records the active stage and next stage.

Do not treat a roadmap stage as complete solely because one implementation PR exists. Completion requires the relevant exit criteria/acceptance evidence in the governing plan and repository.
