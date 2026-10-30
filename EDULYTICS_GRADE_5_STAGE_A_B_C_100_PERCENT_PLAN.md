# EDULYTICS GRADE 5 ? STAGE A?B?C SEQUENTIAL EXECUTION PLAN

Course: US-CCSS-MATH:G5:IM-K5-2021. 148 existing lessons / 8 units (12, 17, 20, 21, 26, 21, 13, 18).
Work only on `curriculum/us-ccss-g5-full-grade-20261028`, Draft PR #596. Never modify other curricula, shared engines, master or production database. Shared integration changes belong to designated Supervisor 1 writer.

## Execution loop (mandatory)
1. Resume strictly at the earliest lesson not closed in all A, B and C. The V13 *content review* cursor was U03:L01; this is not evidence of acceptance of U01?U02.
2. Open the **existing** lesson in the system and read original 2021 teacher lesson/preparation and available student files from `C:\Users\khali\Downloads\New folder (2)\Common Core\05_Grade_5_IM_K5` on **Our-CS** using Desktop Commander, never TinyFish. Prefetch adjacent sources locally without approving them.
3. Compare learning goal, math, worked examples, Student Facing activity prompts, diagrams, accessibility, standards and cool-down. Reuse correct content. Fix only demonstrated differences. For teacher-gated cool-down, author a separate non-copied equivalent plus teacher rubric; never claim the protected original was read.
4. Prefer Reuse existing question family, then Extend existing generator for a concrete missing mechanic, then New Engine only when irreducibly different. Shared engine or selector changes must be delegated to authorized single-writer supervisor.
5. Validate only changed lesson's local JSON/math and its directly affected targeted test once (or record why not run). Do not run broad .NET build, repeating entire tests or CI without an identified technical need.
6. **A:** document local source/evidence; **B:** verify actual effective student/teacher render, not merely a RichV2 JSON sidecar; **C:** verify selected Practice answer/generator/verifier/feedback and teacher-scored responses in the running app. The status is NOT CLOSED while any required acceptance check remains unverified.
7. Record timestamps, source references, scoped repairs, result, blocked checks and Git commit SHA for that lesson. Push a single atomic lesson closure commit when feasible. If unfinished, mark BLOCKED/PENDING truthfully; do not leapfrog the blocked lesson.
8. On successful A?B?C closure, proceed immediately to the next sequential lesson without seeking another chat instruction. Report aggregated checkpoints periodically, not per lesson.

## Current execution checkpoint
- Historical candidate: 148/148 authored RichV2 sidecars. This does **not** mean 148/148 academically accepted.
- V13 historical **content-only** reviews reached U02:L17. This is NOT the A?B?C closure cursor. **Strict mandatory A?B?C cursor is U01:L01** because 0/148 are recorded as fully academically accepted.
- A V14 U03:L01 isolated content-source fix was already committed before the strict closure cursor was rechecked; treat it as a PARKED, NOT-CLOSED candidate, not authority to skip lessons. Do not make further out-of-order repairs. Complete U01:L01, then every subsequent lesson in order (U01:L02, etc.). At U03:L01, review the parked fix against its source evidence before claiming closure.
- The remaining 147 source lesson records and prior fixes must be preserved unchanged.
- Stage D, main merge, deploy, live DB migration, human UAT are separate explicit approval gates.
- U01:L01 already has a first-party exit check and a teacher rubric; **do not recreate them**. Its source cube-construction activity is not adequately assessed by the currently explicit numeric-only `usccss.g5.unit_cube_count` Practice family. **Shared integration issue #592** and authenticated runtime selected-contract/human-scoring verification block Stage C. Preserve A/B evidence, reuse correct content and request the existing Supervisor 1 shared single writer for the contract integration.
- **The filename says 100_PERCENT because that is the target, not the current status.** The present verified full A?B?C acceptance is 0/148. Do not relabel static source edits, passing local math tests or a sidecar as closed lessons.
