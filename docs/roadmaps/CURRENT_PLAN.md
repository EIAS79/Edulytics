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
- **Stage 23 current truthful curriculum status:** 140 Extended supporting lessons inventoried; 0 VERIFIED, 28 CONTEXTUAL, 112 UNSUPPORTED, 0 formal OutcomeCode/FormalTarget mappings in the current Extended packs.
- **Stage 23 claim rule:** global Cambridge IGCSE Extended capability claims and product routing remain blocked until formal mapping, benchmark evidence and approved academic review exist.
- **Stage 23 source of truth:** src/Edulytics.Core/Mathematics/Curriculum/stage23-igcse-extended-gate-manifest.v1.json.
- **Stage 23 benchmark corpus:** src/Edulytics.Core/Mathematics/Curriculum/stage23-igcse-extended-benchmark-corpus.v1.json.
- **Stage 23 CI exit gate:** tools/math_intelligence/stage23_igcse_extended_gate_audit.py.
- **Next active stage after Stage 23 is live on Render:** Programme Stage 24 — AS/A-Level 9709 gate.

## Update rule

Whenever the programme advances to another stage, update this file in the same PR (or immediately following PR) so the repository always records the active stage and next stage.

Do not treat a roadmap stage as complete solely because one implementation PR exists. Completion requires the relevant exit criteria/acceptance evidence in the governing plan and repository.
