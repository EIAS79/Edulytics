# Stage 20 — Diagnostic / Adaptive migration

## Status

**COMPLETE**

Stage 20 migrates exact Mathematics diagnostic/adaptive decisions away from a coarse
Easy / Medium / Challenging-only policy.

For exact Stage 19 Outcome/SkillContracts, the adaptive engine now consumes:

- skill mastery;
- prerequisite mastery;
- mathematical complexity;
- misconception history;
- representation fluency.

The UI may continue to expose Easy / Medium / Challenging, but that band is now a
projection of a richer internal adaptive decision.

## Mathematics-aware decision path

Exact Stage 19 Outcome
→ SkillContract
→ skill mastery
→ prerequisite mastery
→ representation fluency
→ misconception history
→ current mathematical complexity
→ MathematicsDifficultyEngine recommendation
→ bounded target complexity
→ target representation
→ target question family
→ UI difficulty projection

The decision retains:

- TargetLearningOutcomeId;
- TargetSkillId;
- TargetComplexityScore;
- TargetQuestionFamily;
- TargetRepresentation;
- MisconceptionFocusId;
- NextDifficulty;
- EvidenceCreditMultiplier.

## Exact migrated outcomes

Stage 20 migrates the same exact official Outcome/SkillContracts approved by Stage 19:

- CCSS:3.NF.A.3 → fractions.equivalent
- CCSS:4.NF.A.1 → fractions.equivalent
- CCSS:6.RP.A.2 → ratio.unit_rate
- CCSS:6.RP.A.3 → ratio.unit_rate

No new curriculum mapping is invented by Stage 20.

## Prerequisite policy

Stage 20 deliberately does **not** invent a prerequisite graph.

Prerequisite mastery must be supplied by an audited curriculum/learning-state source.
The adaptive engine consumes the value but does not infer prerequisite SkillIds from
lesson titles or topic text.

## Representation policy

Representation fluency is explicit and family-bound.

Every representation row must map to one or more question families already allowed by
the exact Stage 19 Outcome/SkillContract.

The engine may therefore target the learner's weakest or currently failing
representation without silently switching to an unrelated family.

## Misconception policy

Misconception history is explicit, counted and ordered.

An active incorrect-response misconception is prioritized when it is still present in
the audited misconception history. A misconception may target a question family only
when that family is already allowed by the exact Stage 19 contract.

## Mathematical complexity

Stage 20 retains an internal target complexity score independently from the UI band.

The current accepted range is 0–200.

A single adaptive decision may move complexity by at most 12 points. Incorrect
responses can lower the target immediately; successful responses can move upward only
within the configured bound.

The final UI difficulty remains a projection:

- 0–24 → Easy
- 25–55 → Medium
- 56+ → Challenging

## Fail-closed boundaries

Stage 20 fails closed when:

- an exact Outcome is missing mathematics-aware state;
- exact and non-exact Outcomes are mixed in one adaptive scope;
- OutcomeCode and SkillContract disagree;
- supplied skill mastery disagrees with the authoritative student mastery profile;
- prerequisite mastery is outside [0,1];
- complexity is outside the accepted range;
- representation fluency references a disallowed family;
- misconception history references a disallowed family.

## Backward compatibility

Non-migrated Outcomes retain the existing Phase 35 adaptive behavior.

The legacy path remains:

mastery/confidence
→ previous correctness
→ Easy / Medium / Challenging

and keeps formula version:

phase35-v1

The exact migrated path uses:

stage20-math-aware-v1

## Source of truth

Machine-readable manifest:

src/Edulytics.Core/Mathematics/Curriculum/stage20-diagnostic-adaptive-migration-manifest.v1.json

Adaptive contracts:

src/Edulytics.Core/AdaptiveAssessment/AdaptiveAssessmentContracts.cs

Adaptive runtime engine:

src/Edulytics.Services/AdaptiveAssessment/AdaptiveDiagnosticAssessmentEngine.cs

Shared mathematical difficulty intelligence:

src/Edulytics.Services/Mathematics/Difficulty/MathematicsDifficultyEngine.cs

CI closure gate:

tools/math_intelligence/stage20_diagnostic_adaptive_closure_audit.py

Acceptance coverage:

tests/Edulytics.Tests/MathematicsIntelligence/Stage20DiagnosticAdaptiveMigrationTests.cs

## Exit criteria

Stage 20 is complete only when:

- every Stage 19 exact Outcome is present in Stage 20 with the same SkillContract;
- all five governing adaptive signals are consumed;
- internal target complexity is retained separately from the UI difficulty band;
- representation targeting is bound to allowed exact question families;
- misconception targeting is bound to allowed exact question families;
- exact Outcomes cannot fall back to incomplete or mixed adaptive state;
- authoritative mastery cannot be overridden by inconsistent supplied state;
- legacy non-migrated behavior remains compatible;
- Stage 17, Stage 18 and Stage 19 closure gates remain green;
- Stage 20 closure and full repository CI are green;
- the merged commit is live on Render and runtime startup/HTTP checks pass.

## Next programme stage

After this Stage 20 commit is live on Render, execution proceeds to **Programme Stage 21 — Reassessment migration**.
