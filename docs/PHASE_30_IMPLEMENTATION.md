
# Phase 30 — Practice & Assessment Item Engine

## Scope

Phase 30 introduces the durable assessment-item and student-practice evidence foundation required by Phases 31–36.
It deliberately does **not** implement mastery scoring, assessment-blueprint intelligence, dynamic question generation,
adaptive testing, or a traditional Question Bank.

## Data model

- `AssessmentItem`: exact reconstructable item content and validation/generation metadata.
- `AssessmentItemOutcome`: explicit Learning Outcome mapping.
- `PracticeAttempt` / `PracticeAttemptItem`: non-grade student practice lifecycle.
- `PracticeResponse`: exact student answer, deterministic correctness, score and solution feedback.
- `LearningEvidence`: raw Outcome-level evidence for the future deterministic mastery engine.
- `StudentItemExposure`: exposure fingerprint history for later duplicate/reassessment exclusion.

## Security / product invariants

- Practice is never an Official Grade and does not create `AssessmentResult` rows.
- Student identity is resolved from `StudentProfile.UserId` server-side.
- Curriculum access is derived through `StudentEnrollment -> ClassGroup -> CurriculumAdoptionId`.
- Items and Outcome mappings must match the student's school and curriculum adoption.
- Attempts are student-owned and cross-student reads are denied.
- Submitted attempts are idempotent and do not duplicate evidence.
- Every used item remains reconstructable.

## Deferred by design

- Mastery calculation: Phase 31.
- Assessment blueprint intelligence: Phase 32.
- Dynamic mathematics generation: Phase 33.
- Full teacher/student exam generation: Phase 34.
- Adaptive/diagnostic behavior: Phase 35.
- Equivalent reassessment: Phase 36.
