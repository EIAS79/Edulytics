# Student Evaluation Baseline

## Purpose

This document freezes the pre-Evaluation-Engine behavior before the new
Student Evaluation & Learning Analytics subsystem is introduced.

## Existing authoritative sources

1. `StudentAnswer` + `AssessmentQuestion` + `QuestionLearningOutcome`
   represent scored formal-assessment evidence.
2. `LearningEvidence` represents verified Practice evidence.
3. `StudentOutcomeMastery` is the current persisted official mastery
   projection.
4. `ClassOutcomeSummary`, `ClassTopicSummary`,
   `ClassAssessmentTrend`, and `SchoolAnalyticsSnapshot` are existing
   aggregate projections.
5. Private Practice attempts are intentionally excluded from staff-facing
   official analytics and reports.

## Existing mastery behavior

`MasteryEvidenceEngine` combines formal assessment evidence and eligible
non-private Practice evidence using recency weighting. Practice evidence also
uses difficulty weighting. Evidence mapped to multiple official outcomes has
its effective weight divided across those outcome mappings.

The current formula version is `phase31-v2`.

## Existing UI limitations

The staff Analytics surface primarily exposes:

- overall mastery;
- official outcome mastery;
- topic mastery;
- assessment trends;
- lesson assessment mastery when a scored item is linked to a pedagogical
  lesson;
- students below the current risk threshold.

The current student PDF/report does not provide a full evaluation model:
coverage, evidence confidence, short/long trend, retention, Practice versus
Assessment transfer, prerequisite health, criticality explanation, or
recommended next-step intelligence are not represented as one coherent model.

## Privacy invariant

Private student Practice remains private. The new evaluation subsystem must
not make a private Practice attempt visible to a teacher, supervisor,
administrator, class report, or school report unless the product privacy
contract is explicitly changed in a separate reviewed change.

Student-facing evaluation may use the student's own private Practice in a
separate self-only path.

## Migration invariant

The Evaluation Engine is additive. Existing mastery history and existing
projection tables are not overwritten or reinterpreted in place. New
evaluation behavior must remain explainable back to source evidence.
