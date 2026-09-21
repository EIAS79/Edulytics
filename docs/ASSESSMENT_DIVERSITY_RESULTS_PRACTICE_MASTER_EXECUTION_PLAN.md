# Assessment Diversity, Results Workflow and Practice Integrity Master Execution Plan

## Objective

Close the remaining gaps discovered during live teacher/student validation without changing the already-working multi-lesson assessment scope.

The implementation must:

1. converge assessment-results import on the assessment-aware XLSX workflow;
2. preserve teacher selection of multiple lessons;
3. restore mixed item difficulty inside an At Class Level assessment instead of labeling every generated item Easy;
4. add a reusable Question Variant layer beneath Question Family, with up to 16 pedagogically distinct variants per family;
5. enforce diversity across variants/families and reject duplicate or near-duplicate generated prompts;
6. apply the same diversity architecture to teacher Assessment generation and Student AI/Private Practice;
7. fix learner-facing mathematics visuals so a visual is shown only when it semantically matches the generated question;
8. ship with regression, security and deployment verification.

## Non-goals

- Do not make At Class Level dynamically derive its base level from current class mastery.
- Do not remove or change multi-lesson assessment selection.
- Do not force every family to have 16 variants. Sixteen is a maximum, not a quota.
- Do not weaken solver/verifier requirements, lesson provenance, outcome alignment, or teacher review before publish.

## Architecture

Generation becomes:

```
Lesson
  -> SkillContract
      -> Question Family
          -> Question Variant
              -> Parameters
                  -> Solver
                      -> Verifier
```

A Question Family represents one mathematical task structure. A Question Variant changes the learner-facing formulation, representation, context, or interaction without pretending a materially different mathematical task is the same family.

### Variant policy

- maximum variants per family: 16;
- minimum production diversity target: 4 useful variants where the mathematics allows it;
- preferred target: 8+ useful variants for rich skills;
- variants must be pedagogically distinct, not cosmetic paraphrases;
- materially different answer/solver structure remains a separate Question Family.

## Phases

### Phase 1 — Baseline and execution plan

- Record the agreed scope in the repository.
- Preserve the current multi-lesson builder behavior.
- Add regression coverage for the defects reproduced from live data.

### Phase 2 — Assessment-results workflow convergence

- Remove Assessment Results from the generic Bulk Data Import surface.
- Keep the assessment-aware XLSX workflow at Assessment -> Results as the single teacher workflow.
- Preserve internal assessment/question/student IDs in workbook metadata.
- Add tests preventing the generic CSV template from being exposed for assessment results.

### Phase 3 — At Class Level difficulty mix

- Keep the meaning of At Class Level as a teacher-selected assessment band.
- Replace the current one-to-one mapping `AtClassLevel -> Easy` for every generated item with an item-level difficulty distribution.
- At Class Level must create a balanced mix of Easy, Medium and Challenging items when the question count permits.
- Stretch and Challenge remain explicit teacher overrides with progressively harder distributions.
- Preserve exact-engine difficulty semantics and marks distribution.

### Phase 4 — Question Variant core and diversity planner

- Introduce a versioned Question Variant contract/registry.
- Support up to 16 variants per family.
- Keep family IDs stable.
- Generate/persist a variant ID with each generated item.
- Add a diversity planner that:
  - rotates families where several are allowed;
  - rotates variants inside a family;
  - avoids duplicate normalized prompts within one generation batch;
  - avoids reusing recent exposure fingerprints where possible;
  - fails closed when requested diversity cannot be produced safely.

### Phase 5 — Teacher Assessment integration

- Route lesson-scoped assessment generation through the variant-aware diversity planner.
- Retain LessonId, SkillId, FamilyId, VariantId, difficulty and solver/verifier metadata.
- Ensure multi-lesson allocation remains intact.
- Add regression tests proving one selected lesson can yield structurally different questions rather than only parameter substitution.

### Phase 6 — Student AI/Private Practice and visual integrity

- Route READY_VERIFIED private practice through the same variant-aware selection rules.
- Prevent identical/near-identical prompts inside one attempt.
- Correct geometry `shape_dimension` presentation so visual output matches the semantic shape, or suppress the visual when the required semantic parameters are absent.
- Add acceptance tests reproducing the prior square/cube repetition and trapezoid mismatch.

### Phase 7 — Verification, merge and production deployment

- Run build, regression, PostgreSQL, security/SAST, container and migration gates.
- Review any new CodeQL/high findings and remediate before merge.
- Merge the PR to `main`.
- Verify Render auto-deploys the merge commit.
- Confirm deployment reaches LIVE and inspect post-live error/critical logs.

## Acceptance criteria

The work is complete only when all of the following are true:

- Bulk Data Import no longer offers Assessment Results CSV to teachers.
- The assessment-aware XLSX workflow remains functional.
- Multiple lessons can still be selected in one assessment.
- At Class Level no longer marks every generated item Easy when a mixed distribution is possible.
- Generated items persist a Question Variant identifier.
- A batch does not contain exact duplicate prompts.
- Representative families demonstrate multiple learner-facing variants.
- Student Practice no longer repeats the same square/cube prompt multiple times in one attempt.
- A `shape_dimension` question never renders an unrelated generic trapezoid/base-height diagram.
- Existing lesson/outcome provenance and solver verification remain intact.
- Required CI gates pass.
- The merged commit is LIVE on Render with no new critical/error logs after rollout.
