# Stage 19 — Assessment Builder migration

## Status

**COMPLETE**

Stage 19 preserves the teacher workflow exactly:

Select → Generate → Review → Approve → Publish

Only the generation intelligence changes.

## Exact generation contract

When a selected official LearningOutcome has a Stage 19 Assessment SkillContract backed by READY_VERIFIED lesson evidence, generation follows:

Official LearningOutcome
→ Stage19AssessmentSkillContract
→ allowed exact question family
→ shared ExactSkillContractQuestionEngine
→ exact solver
→ independent verifier
→ AssessmentItem
→ teacher Review
→ teacher Approve
→ Publish

The exact path never calls the legacy MathematicsQuestionGenerationEngine.

## Exact official outcomes

The current exact Assessment Builder registry contains:

- CCSS:3.NF.A.3 → fractions.equivalent
- CCSS:4.NF.A.1 → fractions.equivalent
- CCSS:6.RP.A.2 → ratio.unit_rate
- CCSS:6.RP.A.3 → ratio.unit_rate

These contracts are backed by 8 READY_VERIFIED official-mapped pedagogical lessons from the Stage 17/18 registries.

## Fail-closed boundaries

Stage 19 does not invent curriculum mappings.

The following READY_VERIFIED Cambridge supporting lessons remain outside exact Assessment Builder routing because they have no independent official OutcomeCode:

- PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-4:APPLY
- PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:APPLY
- PED:CAMBRIDGE-INTL-MATH:S6:6F-3:BUILD
- PED:CAMBRIDGE-INTL-MATH:S6:6F-3:APPLY
- PED:CAMBRIDGE-INTL-MATH:S5:5F-2:BUILD
- PED:CAMBRIDGE-INTL-MATH:S5:5F-2:APPLY

CCSS:3.OA.B.5 is also explicitly excluded from exact Stage 19 routing. Its pedagogical lesson linkage is not sufficient evidence that the verified fractions.equivalent SkillContract exactly represents that separate operations outcome.

If a teacher generation request mixes exact Stage 19 outcomes with non-exact outcomes, the request fails closed rather than silently routing the exact outcomes through the legacy generator.

## Shared Mathematics kernel

Practice and Assessment Builder now use the same exact Mathematics kernel:

src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs

This kernel owns:

- exact parameter generation;
- deterministic question-family execution;
- solving;
- independent verification;
- reconstructable parameter records;
- exposure fingerprints.

Stage-specific adapters own curriculum alignment and persistence metadata.

## Assessment metadata

Exact Stage 19 items record:

- generationMethod = skill-contract-assessment-solver-verified-v1
- readiness = READY_VERIFIED
- exact SkillContract
- official OutcomeCode
- allowed question family
- solver identifier
- verifier identifier
- solverVerified = true
- broadFallbackUsed = false
- teacherReviewRequired = true
- workflow = Select-Generate-Review-Approve-Publish

## Legacy compatibility

Official outcomes that are not part of Stage 19 exact migration may continue to use the existing native Mathematics generation path when that path already supports them.

Once an outcome is present in the Stage 19 exact registry, it is authoritative and cannot fall back to the legacy/native generator.

## Source of truth

Machine-readable manifest:

src/Edulytics.Core/Mathematics/Curriculum/stage19-assessment-builder-migration-manifest.v1.json

Runtime contract registry:

src/Edulytics.Core/Mathematics/Assessment/Stage19AssessmentSkillContracts.cs

Exact Assessment adapter:

src/Edulytics.Services/Assessments/Stage19AssessmentSkillContractEngine.cs

Shared exact Mathematics kernel:

src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs

CI closure gate:

tools/math_intelligence/stage19_assessment_builder_closure_audit.py

## Exit criteria

Stage 19 is complete only when:

- every exact Assessment outcome is backed by READY_VERIFIED official-mapped lesson evidence;
- every allowed Stage 19 family is already verified by its Stage 18 evidence lessons;
- supporting lessons without official outcomes remain explicitly excluded;
- ambiguous outcome mappings remain explicitly fail-closed;
- exact routing occurs before legacy/native generation;
- mixed exact/non-exact requests fail closed;
- exact Assessment generation uses the shared Mathematics kernel;
- generated items are independently verified before persistence;
- the teacher workflow remains Select → Generate → Review → Approve → Publish;
- Stage 18 closure remains green after the shared-kernel refactor;
- Stage 19 closure and full repository CI are green;
- the merged commit is live on Render and runtime startup/HTTP checks pass.

## Next programme stage

After this Stage 19 commit is live on Render, execution proceeds to **Programme Stage 20 — Diagnostic/adaptive migration**.
