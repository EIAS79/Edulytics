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
3. **R3 — Source and research acquisition pipeline:** NEXT
4. **R4 — Rich Content V2 model and renderer:** NOT STARTED
5. **R5 — Instructional visual system:** NOT STARTED
6. **R6 — Curated Video Help:** NOT STARTED
7. **R7 — Pilot enrichment:** NOT STARTED
8. **R8 — Catalogue-wide rollout and permanent quality gate:** NOT STARTED

**Current execution position:** R1 and R2 are complete. The exact effective catalogue contains **4,453** learner-visible mathematics lessons: **3,104 official** and **1,349 supporting/no-formal-outcome**. The first-pass Rich Content V2 audit classifies **139 Good**, **2,743 NeedsExpansion**, **2 Generic**, and **1,569 SourceResearchRequired**. Those 1,569 lessons are the Polish framework-only population and require a pedagogical-source research pass before source-driven enrichment. **2,763** lessons have only generic-fallback/no explicit instructional visual evidence, and no lesson yet has a curated Video Help resource. The next phase is **R3 — Source and research acquisition pipeline**.

**R1/R2 evidence:** `docs/lesson-content-v2/catalogue-summary.md`, `RichLessonContentQualityAudit`, and the generated `artifacts/lesson-content-v2/*` CI outputs.

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
