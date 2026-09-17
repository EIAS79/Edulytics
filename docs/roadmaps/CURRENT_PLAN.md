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
- **Completion model:** one fail-closed Grade 1–6 migration stage, not a set of separately completed sub-phases. Every primary lesson is either production-routed because it is READY_VERIFIED with an approved exact SkillContract and reviewed exact mechanic, or remains fail-closed with a readiness reason.
- **Production source of truth:** src/Edulytics.Core/Mathematics/Curriculum/stage17-grade1-6-production-manifest.v1.json.
- **CI exit gate:** tools/math_intelligence/stage17_grade1_6_closure_audit.py.
- **Next active stage after Stage 17 is live on Render:** Programme Stage 18 — Practice migration.

## Update rule

Whenever the programme advances to another stage, update this file in the same PR (or immediately following PR) so the repository always records the active stage and next stage.

Do not treat a roadmap stage as complete solely because one implementation PR exists. Completion requires the relevant exit criteria/acceptance evidence in the governing plan and repository.
