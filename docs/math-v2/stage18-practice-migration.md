# Stage 18 — Practice migration

## Status

**COMPLETE**

Stage 18 migrates READY_VERIFIED lesson-scoped Student Private Practice to the Mathematics Intelligence path:

Lesson
→ SkillContract
→ allowed question family
→ generated problem
→ solver
→ independent verifier
→ persisted Practice item

There is no broad contextual fallback for any READY_VERIFIED Stage 17 lesson.

## Source of truth

Machine-readable Practice migration manifest:

src/Edulytics.Core/Mathematics/Curriculum/stage18-practice-migration-manifest.v1.json

Runtime SkillContract registry:

src/Edulytics.Core/Mathematics/Practice/Stage18PracticeSkillContracts.cs

Exact server-side generator:

src/Edulytics.Services/Practice/Stage18SkillContractPracticeEngine.cs

CI closure audit:

tools/math_intelligence/stage18_practice_closure_audit.py

## Production contract

All 14 READY_VERIFIED Grade 1–6 lessons from the Stage 17 production manifest are covered one-for-one by Stage 18 Practice contracts.

Each Practice contract specifies:

- exact LessonCode;
- exact SkillId;
- reviewed mechanic;
- allowed question families only.

The exact Practice engine:

- generates parameters server-side;
- solves the mathematical problem;
- independently verifies the solver result;
- rejects any item that fails verification;
- stores reconstruction metadata;
- stores solver/verifier identity;
- records solverVerified=true;
- records broadFallbackUsed=false;
- enforces exposure-fingerprint uniqueness.

If exact generation fails for a READY_VERIFIED lesson, Practice returns GenerationFailed. It must not retry through contextual generation.

## Allowed family coverage

Stage 18 supports the exact families required by the current READY_VERIFIED lessons:

- algebra.relationships.two_unknowns.total_difference
- measurement.scale.equal_intervals.read_value
- fractions.compare.unlike.common_denominator
- fractions.equivalent.missing_value
- fractions.equivalent.recognize
- fractions.equivalent.generate_multiple
- fractions.equivalent.number_line
- fractions.equivalent.reduce_common_factor
- ratio.unit_rate.direct
- ratio.unit_rate.equivalent_ratio

## Answer checking

Practice answer checking now uses the shared Mathematics answer-equivalence implementation rather than a separate numeric-only comparator. Equivalent scalar forms such as fractions, decimals and percentages are therefore graded consistently.

## Fail-closed boundary

Lessons that are not READY_VERIFIED may continue to use the existing contextual/native Practice paths according to their existing capability status.

For READY_VERIFIED lessons, Stage 18 forbids:

- title-only selection;
- broad semantic family inference;
- CurriculumContextCheck fallback;
- persistence before solver verification;
- question families outside the lesson SkillContract.

## Exit criteria

Stage 18 is complete when:

- every Stage 17 READY_VERIFIED lesson exists in the Stage 18 Practice manifest;
- no Stage 18 Practice lesson exists outside the Stage 17 verified manifest;
- SkillId and mechanic remain identical to Stage 17;
- every lesson has at least one allowed question family;
- every allowed family is executable by the exact Practice engine;
- generated items are solver-verified before persistence;
- READY_VERIFIED lesson generation precedes and bypasses contextual fallback;
- Practice scoring uses shared Mathematics equivalence;
- the Stage 18 closure audit and full repository CI are green;
- the merged commit is live on Render and runtime startup/HTTP checks pass.

## Next programme stage

After this Stage 18 commit is live on Render, execution proceeds to **Programme Stage 19 — Assessment Builder migration**.
