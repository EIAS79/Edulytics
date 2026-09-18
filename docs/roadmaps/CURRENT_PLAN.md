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
- **Next active stage after Stage 25 is live on Render:** Programme Stage 26 — Security and resource controls.

## Update rule

Whenever the programme advances to another stage, update this file in the same PR (or immediately following PR) so the repository always records the active stage and next stage.

Do not treat a roadmap stage as complete solely because one implementation PR exists. Completion requires the relevant exit criteria/acceptance evidence in the governing plan and repository.
