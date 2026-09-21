# Assessment, Evidence, Analytics & Results — Master Execution Plan

Status: ACTIVE IMPLEMENTATION  
Execution mode: one uninterrupted delivery branch, one PR, one merge, one deployment.  
Baseline: main @ 8df94efbd761e833fbb42dc5400b9284035fa73d

## Objective

Correct the current Outcome-only assessment authoring path and the downstream evidence/reporting gaps so that Edulytics can author, deliver, score, import, and analyse assessments using explicit instructional scope while preserving official curriculum alignment.

The implementation must keep these concepts separate:

- **Lesson** = the exact instructional target the teacher selected.
- **SkillContract** = the mathematical capability and allowed question families used to generate/verify the item.
- **Learning Outcome** = the official curriculum evidence/alignment target.
- **Question Family** = the structural form of the problem.
- **Assessment administration** = one delivery/results instance for one class or targeted student population.
- **Assessment definition/template** = reusable authored question content where multi-class reuse is required.

A Lesson label must never be inferred after the fact from a broad Topic/Outcome and presented as lesson mastery unless the item persisted explicit Lesson provenance.

## Phase 1 — Scope and provenance foundation

1. Add first-class Assessment generation scope:
   - specific lesson(s)
   - unit(s)
   - full adopted curriculum/course
2. Resolve every selected scope to explicit pedagogical Lesson IDs.
3. Resolve each lesson to official Learning Outcomes where official mappings exist.
4. Preserve supporting lessons even when they have no standalone official outcome; they must be generated only when an exact lesson SkillContract/capability exists.
5. Persist generation provenance on every generated item:
   - CurriculumPedagogicalLessonId
   - LearningOutcome mapping(s), when available
   - exact Skill ID / generation family in generation metadata
   - difficulty profile/version
6. Do not silently downgrade an exact lesson request to a broad contextual Topic generator.

Acceptance:
- Builder no longer presents Outcomes as if they are the generation scope.
- A generated assessment item can be traced back to the selected lesson when a lesson-scoped path was used.

## Phase 2 — Assessment Builder scope UX and multi-class model

1. Replace the AI generation scope control with an explicit selector:
   - Lessons
   - Units
   - Full curriculum/course
2. Show lesson title, unit, and official outcome alignment separately.
3. Keep manual Outcome mapping available for formal evidence editing.
4. Add reusable Assessment Definition/Template identity and per-class Administration semantics without breaking existing assessments:
   - current Assessment rows remain the delivery/result boundary
   - shared template/definition identity groups reusable content across classes
   - each class keeps independent roster, delivery status, results, and analytics
5. Ensure a teacher with multiple assigned classes can reuse the same definition without mixing class results.

Acceptance:
- Scope selection is instructional, not an Outcome-only multiselect.
- Results remain class-specific even when content is reused.

## Phase 3 — Generation diversity and cognitive difficulty

1. Allocate question count across selected Lessons/Skills before generation.
2. Within each Lesson, rotate allowed Question Families and reject avoidable structural duplicates.
3. Keep exposure fingerprint protection and add normalized structural-family diversity checks.
4. Make visible difficulty materially change cognitive demand:
   - Easy: direct/low-step
   - Medium: additional representation/step/transfer
   - Challenge: multi-step/reasoning/constraint burden where the SkillContract allows it
5. Remove Primary mappings where Medium and Challenge collapse to the same effective difficulty unless the skill itself has no safe higher level.
6. Fail closed when requested diversity/difficulty cannot be generated safely.

Acceptance:
- A 10-question assessment does not degenerate into the same template with changed numbers when multiple allowed families exist.
- Challenge is not implemented as larger numbers only.

## Phase 4 — Structured item delivery and MCQ correction

1. Stop embedding A/B/C/D choices in Prompt text.
2. Persist structured answer options in generation metadata with a stable schema.
3. Expose ItemType and structured Choices in student delivery contracts.
4. Render:
   - radio buttons for MultipleChoice
   - appropriate text/numeric input for other item types
5. Validate submitted MCQ value against the structured option set before scoring.
6. Keep backward compatibility for legacy items.

Acceptance:
- Student UI never shows MCQ options embedded in prompt plus a freeform textarea for newly generated MCQs.

## Phase 5 — Assessment-aware XLSX results workflow

1. Replace the teacher-facing generic CSV workflow for assessment result entry with an assessment-aware XLSX workbook.
2. Workbook shape:
   - one student per row
   - pre-populated class roster
   - question columns
   - visible student name
   - hidden/protected stable identifiers
3. Import by Assessment + hidden StudentProfileId, not manual StudentNumber typing.
4. Validate roster membership, question identity, score bounds, duplicate rows, and stale workbook metadata.
5. Keep legacy CSV import compatibility only where needed for old workflows.

Acceptance:
- Teacher does not need to know or repeat StudentNumber to enter results.

## Phase 6 — Lesson evidence, weakness analytics, and reports

1. Add Lesson-level evidence aggregation using explicit AssessmentItem.CurriculumPedagogicalLessonId.
2. Keep separate analytics dimensions:
   - Lesson mastery
   - Outcome mastery
   - Topic/Unit aggregate mastery
3. Never relabel current Topic mastery as Lesson mastery.
4. UI should use human-readable lesson titles and explicit metric labels such as:
   - Class mastery: 37%
   - evidence/student counts
   - mastery band
5. Add class and student PDF exports using the existing PDFsharp/MigraDoc dependency.
6. Reports must state scope and evidence counts so percentages are interpretable.

Acceptance:
- A statement such as “Compare fractions with different denominators — Class mastery 37%” is emitted only from lesson-linked evidence.

## Phase 7 — Persistence, migration, compatibility, and tests

1. Add only the schema required for reusable assessment definitions/administrations and any new provenance metadata not already represented.
2. Backfill conservatively:
   - never invent Lesson IDs from Topic names
   - leave legacy provenance null when it cannot be proven
3. Extend unit/integration/contract tests for:
   - scope resolver
   - lesson/outcome mapping
   - diversity
   - difficulty
   - MCQ delivery
   - XLSX import/export
   - lesson analytics
   - PDF endpoints
   - multi-class isolation
4. Run repository CI gates.

Acceptance:
- No fabricated historical lesson mastery.
- Existing assessments remain readable/deliverable.
- CI is green.

## Phase 8 — Merge, deploy, and production verification

1. Open one PR containing the complete implementation.
2. Require passing CI before merge.
3. Merge to main.
4. Let Render auto-deploy main when enabled; otherwise trigger exactly one deployment.
5. Verify build/deploy status and application health/logs.
6. Record final implementation/deployment state in the PR/closeout documentation.

## Non-negotiable invariants

- Outcome and Lesson are not interchangeable.
- Supporting lessons do not receive invented official Outcome codes.
- Lesson analytics require explicit persisted Lesson provenance.
- One class administration cannot leak or combine another class roster/results.
- AI-generated formal assessment questions remain teacher-reviewable before publication.
- Generated mathematics must remain solver/verification grounded.
- Private student practice remains separate from official school mastery evidence unless explicitly promoted by product policy.
