# Unified Practice Consolidation — U0 to U5

Status: implementation plan and production closure contract.

## Objective

Edulytics exposes one learner-facing Lesson Practice product. Version labels
such as V1/V2 are implementation details and must not determine the visual
experience.

The authoritative flow is:

```text
Lesson
  -> READY_VERIFIED capability
  -> Unified Practice start
  -> adaptive decision
  -> verified question generation
  -> answer equivalence
  -> misconception / representation evidence
  -> remediation / confirmation / progression
  -> next verified question
```

## U0 — Audit and freeze

Findings:
- lesson Start Practice is already gated by `LessonPracticeCapabilityResolver`;
- the old split was introduced by the Adaptive per-lesson allow-list and by
  controller fallback order;
- legacy `LessonAttempt` remains necessary only for historical attempts and
  schools/curricula not yet in the rollout;
- no new learner feature may be added to the legacy LessonAttempt path.

## U1 — Unified routing

`RouteAllReadyVerifiedLessons=true` makes the curriculum-level and school
allow-lists the rollout boundary. Per-lesson allow-listing is no longer
required.

The lesson itself still must:
- resolve through `LessonPracticeCapabilityResolver`;
- be `READY_VERIFIED`;
- remain inside Primary logical levels 1–6;
- belong to an explicitly allowed curriculum level and school.

This is not broad keyword fallback.

## U2 — Unified runtime

`StudentPracticeController.StartLessonPractice` invokes the adaptive runtime
before legacy presentation routing.

Inside the rollout:
- successful start -> Unified Practice;
- verified generation/persistence failure -> visible safe failure, never a
  silent legacy session;
- next questions exclude prior exposure fingerprints and semantic identity
  keys;
- misconception/remediation/confirmation all use the persisted Adaptive
  evidence stream.

Legacy runtime is compatibility-only outside the rollout.

## U3 — Unified UI/UX

The active Lesson Practice page uses `_StudentLayout`:
- same sidebar/topbar as the Student Portal;
- one `Edulytics Practice` brand;
- no learner-facing V1/V2 terminology;
- consistent question, feedback, progress and completion cards;
- Question Log link on completion when enabled.

## U4 — Full product integration

The unified runtime continues to write the same evidence used by:
- My Next Steps;
- Question Log;
- Live Classroom Intelligence;
- Diagnostic V2;
- Live Group Session;
- Psychometric Readiness;
- Research aggregates.

No duplicate mastery/evidence store is introduced.

## U5 — Legacy retirement and production rollout

New Lesson-scoped generation routes through `StartLessonPractice`.
The legacy lesson-pilot start also routes through Unified Practice.

Legacy `LessonAttempt` is retained only for:
- already-created historical attempts;
- explicit compatibility outside the production rollout.

Production activation is curriculum/school scoped and can be rolled back with
one feature flag. Advanced Mathematics V3 remains independently gated.
