# Phase 29 — Full Curriculum Closure Contract

Phase 29 is **not closed** until the supported Mathematics curriculum catalogue and lesson-content engine are complete across every product scope below.

## Supported curriculum packs

1. `US-CCSS-MATH` — American / Common Core Mathematics
2. `CAMBRIDGE-INTL-MATH` — Cambridge International Mathematics
3. `UAE-MOE-MATH` — UAE Ministry of Education Mathematics
4. `PL-NATIONAL-MATH` — Polish National Curriculum Mathematics

## Required product scopes

### Common Core

The existing accepted full Common Core lesson rollout remains the baseline. No regression in lesson count, official alignment, provenance, or canonical bodies is permitted.

### Cambridge International Mathematics

- Cambridge Primary Stages 1–6
- Cambridge Lower Secondary Stages 7–9
- Cambridge IGCSE Mathematics 0580 — Core and Extended kept separate
- Cambridge International AS Level Mathematics 9709
- Cambridge International A Level Mathematics 9709

Cambridge official wording remains reference-only. Edulytics lesson bodies must be independently authored or adapted from separately reusable pedagogical sources. Core and Extended must never be mixed.

### UAE MoE Mathematics

- Grade 1–4 — Common
- Grade 5–12 — General
- Grade 5–12 — Advanced

The complete lesson sequence must be source-backed for the served academic period. A source-catalog row by itself does not count as completed curriculum content. General and Advanced must remain isolated.

### Polish National Mathematics

- Primary: Klasa I–VIII
- Upper secondary Liceum: Klasa I–IV
- Upper secondary Technikum: Klasa I–V

Canonical academic content is Polish. Native school pathways must remain separate.

## Per-scope closure requirements

Every required scope must have all of the following:

1. A stable curriculum identity: pack + logical level + pathway.
2. Source provenance/version evidence.
3. A non-synthetic pedagogical lesson sequence (blueprint or verified official lesson graph).
4. Every published aligned lesson mapped only to outcomes from the same curriculum scope/pathway.
5. Canonical lesson content in the curriculum academic language.
6. Every canonical lesson body contains:
   - Explanation
   - Key concepts and rules
   - Worked examples
   - Step-by-step solutions
   - Common mistakes
   - Quick summary
7. No placeholder/generic lesson bodies presented as completed content.
8. Student scope resolves through `Enrollment -> Class -> CurriculumAdoptionId`.
9. Teacher scope is restricted to assigned classes/adoptions.
10. Lesson Content browsing is filtered by Academic Year -> Program/Stream -> Curriculum Level.

## Final acceptance

Phase 29 may be marked CLOSED only when:

- repository content coverage audit reports zero missing required scopes;
- full solution build passes with zero warnings/errors;
- full regression, PostgreSQL, security/SAST, localization, tenant/IDOR, and container gates pass;
- the final PR is merged to `main`;
- staging deploys the merged `main` commit;
- staging readiness and manual/browser acceptance pass;
- staging operational/test school data is reset for clean re-entry without deleting schema, migrations, or canonical curriculum/reference content.
