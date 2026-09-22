# Current Edulytics Programme Plan

**Status:** ACTIVE — GOVERNING EXECUTION PLAN

**Current governing plan:** [`EDULYTICS_RICH_LESSON_CONTENT_V2_MASTER_EXECUTION_PLAN.md`](./EDULYTICS_RICH_LESSON_CONTENT_V2_MASTER_EXECUTION_PLAN.md)

This document is the repository pointer to the active Edulytics execution programme.

## Current programme

### Rich Lesson Content V2

The active programme is the catalogue-wide enrichment of learner-visible mathematics lesson content across every supported curriculum, official and supporting.

The programme preserves existing curriculum objectives, official OutcomeCodes, lesson identities, mappings, assessment history, mastery history and production routes.

Its execution order is:

1. **R1 — Exact catalogue and content-quality audit:** COMPLETE
2. **R2 — Rich Lesson Content Quality Contract:** COMPLETE
3. **R3 — Source and research acquisition pipeline:** COMPLETE
4. **R4 — Rich Content V2 model and renderer:** COMPLETE
5. **R5 — Instructional visual system:** COMPLETE
6. **R6 — Curated Video Help:** COMPLETE
7. **R7 — Pilot enrichment:** COMPLETE — merged, CI green and production deployed
8. **R8 — Catalogue-wide rollout and permanent quality gate:** IN PROGRESS

**Current execution position:** R1–R7 are complete. The 26-lesson controlled pilot is merged on `main`, passed both Phase16 CI and Mathematics Intelligence CI, and is live in production. R8 now expands Rich V2 beyond curated sidecars through a fail-closed catalogue-wide compiler. Curated Rich V2 remains authoritative; eligible English lessons may be compiled only from existing canonical content plus a `READY_VERIFIED` Practice contract and worked examples generated, solved and independently verified by the exact Mathematics engine. Non-English lessons are never silently replaced with English generated prose and remain explicitly classified for reviewed localized authoring. A permanent R8 status artifact classifies every one of the **4,453** lessons as Rich-ready or an explicit blocker.

**Evidence:** `docs/lesson-content-v2/catalogue-summary.md`, `docs/lesson-content-v2/R3_R7_PILOT_IMPLEMENTATION.md`, `RichLessonContentQualityAudit`, `RichLessonSourceDossierFactory`, `RichLessonContentV2Registry`, the shared Rich V2 renderer, and generated `artifacts/lesson-content-v2/*` CI outputs.

## Execution hierarchy

1. **Repository code, tests, CI and accepted stage documents** define what has actually been implemented.
2. **The Rich Lesson Content V2 Master Execution Plan** defines the current governing programme and what should be executed next.
3. **The completed Integrated Mathematics Intelligence Master Execution Plan** remains the accepted architectural and mathematical foundation.
4. **The Advanced Mathematics Engine roadmap and Curriculum Intelligence roadmap** remain supporting technical specifications.
5. Older `PHASE_*_IMPLEMENTATION_PLAN.md` documents are historical phase records unless the current governing plan explicitly references them.

## Completed predecessor programme

The prior governing plan, [`EDULYTICS_INTEGRATED_MATH_CURRICULUM_MASTER_EXECUTION_PLAN.md`](./EDULYTICS_INTEGRATED_MATH_CURRICULUM_MASTER_EXECUTION_PLAN.md), is complete as an execution programme.

Its completed work remains authoritative for:

- curriculum/lesson intelligence foundations;
- SkillContracts;
- Practice migration;
- Assessment Builder generation;
- diagnostic/adaptive and reassessment routing;
- game runtime migration;
- mathematics solver/verifier capability;
- advanced capability gates;
- security/resource controls;
- observability;
- production rollout.

The new Rich Lesson Content V2 programme builds on those foundations; it does not replace or reopen them.

### Accepted predecessor closure markers

The following closure statements remain part of the repository's accepted mathematics baseline and are retained verbatim for CI/audit continuity:

- **Programme Stage 26 — Security and resource controls: COMPLETE.**
- **Programme Stage 27 — Observability: COMPLETE.**
- **Programme Stage 28 — Production rollout: COMPLETE.**
- **Integrated Mathematics Intelligence Master Execution Plan: COMPLETE** as an execution programme.

These statements describe the completed predecessor programme only. They do not change the active Rich Lesson Content V2 execution position.

## Update rule

Whenever the active programme advances to another phase, update this file in the same PR (or immediately following PR) so the repository always records:

- the current governing plan;
- the current active phase;
- the next implementation step.

Do not mark a phase complete solely because one implementation PR exists. Completion requires the exit criteria and repository evidence defined by the governing plan.
