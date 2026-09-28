# Adaptive Practice V2 — Baseline Inventory

Baseline branch point: `main@6d63763df0955143a98f27403b765105fac062ca`.

## Existing production paths that are protected

- `StudentPracticeController`
- `StudentPrivatePracticeService`
- `PracticeService`
- `Stage18SkillContractPracticeEngine`
- `LessonPracticeCapabilityResolver`
- `LessonPracticeContractRegistry`
- exact Mathematics generation + solver/verifier
- shared Mathematics answer equivalence
- Practice exposure fingerprints / semantic uniqueness
- Student My Progress
- Student 360 / Analytics
- Assessment Builder review/approve/publish workflow
- formal Teacher Assessments
- Exam Generation
- Weakness Recovery / Equivalent Reassessment
- realtime Analytics / SignalR
- curriculum level identity

## Confirmed current Practice execution model

Current learner Practice creates an attempt with a preselected/generated item set.
`PracticeService.AnswerAsync` evaluates and persists the current answer; it does
not choose and create a new next item from that newly observed response.

Adaptive V2 is therefore implemented as an additive path rather than by mutating
the existing attempt semantics.

## CI/build policy during implementation

Automatic execution of the heavy Mathematics Intelligence and Evaluation
Production workflows is disabled on the implementation branch and retained as
manual `workflow_dispatch` verification.

Runtime correctness gates are **not** disabled:

- READY_VERIFIED capability resolution;
- exact SkillContract restrictions;
- solver/verifier;
- answer equivalence;
- tenant/access boundaries;
- fail-closed generation behaviour.

The public Render readiness endpoint/polling is not a CI gate and remains
unchanged because it protects application availability.

## Rollout boundary

Adaptive V2 is OFF by default.

Even when enabled, eligibility requires:

1. Mathematics scope;
2. valid school/curriculum/lesson identity;
3. logical Primary level 1–6;
4. explicit curriculum-level allow-list;
5. optional school allow-list;
6. optional lesson allow-list;
7. READY_VERIFIED lesson Practice capability.

Any failure remains on Practice V1.
