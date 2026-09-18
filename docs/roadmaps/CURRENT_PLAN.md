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
- **Stage 22 authority rule:** for every currently migrated READY_VERIFIED exact game route, the server owns problem generation and answer correctness; the browser owns rendering and interaction only.
- **Stage 22 exact scope:** all 14 Stage 18 exact lesson SkillContracts across 5 mechanics.
- **Stage 22 source of truth:** src/Edulytics.Core/Mathematics/Curriculum/stage22-game-runtime-migration-manifest.v1.json.
- **Stage 22 CI exit gate:** tools/math_intelligence/stage22_game_runtime_closure_audit.py.
- **Next active stage after Stage 22 is live on Render:** Programme Stage 23 — IGCSE Extended gate.

## Update rule

Whenever the programme advances to another stage, update this file in the same PR (or immediately following PR) so the repository always records the active stage and next stage.

Do not treat a roadmap stage as complete solely because one implementation PR exists. Completion requires the relevant exit criteria/acceptance evidence in the governing plan and repository.
