# Stage 21 — Reassessment migration

## Status

**COMPLETE**

Stage 21 changes equivalent-reassessment freshness from wording-level variation to
mathematical variation for exact Stage 19 Outcome/SkillContracts.

The governing freshness dimensions are:

- coefficients;
- representation;
- strategy;
- context;
- misconception trap;
- cognitive demand.

A reassessment is not considered fresh merely because the prompt text or numbers look
different.

## Exact reassessment path

Exact Stage 19 Outcome
→ SkillContract
→ equivalent reassessment blueprint
→ mathematical freshness history
→ allowed exact question family
→ shared exact Mathematics kernel
→ solver
→ independent verifier
→ mathematical freshness signature
→ fresh reassessment item
→ existing Phase 36 recovery validation

## Mathematical freshness signature

Every exact Stage 21 item records a reconstructable signature containing:

- ExposureFingerprint;
- QuestionFamily;
- CoefficientSignature;
- Representation;
- Strategy;
- Context;
- MisconceptionTrap;
- CognitiveDemand.

The exposure fingerprint itself includes the mathematical dimensions, not the wording
alone.

## Exact migrated outcomes

Stage 21 continues the exact Outcome/SkillContracts already approved in Stages 19–20:

- CCSS:3.NF.A.3 → fractions.equivalent
- CCSS:4.NF.A.1 → fractions.equivalent
- CCSS:6.RP.A.2 → ratio.unit_rate
- CCSS:6.RP.A.3 → ratio.unit_rate

No new curriculum mapping is invented by Stage 21.

## Prior-exposure policy

For an exact migrated Outcome, when prior exposure exists, the recovery request must
provide mathematical signatures for the exact history.

The following are insufficient on their own:

- old prompt text;
- normalized prompt shape;
- exposure fingerprint without its mathematical signature.

If prior exact exposure exists but no mathematical signature is available, Stage 21
fails closed rather than claiming mathematical freshness.

Each supplied signature must also belong to the declared prior exposure history.

## Coefficient freshness

Stage 21 rejects:

- a coefficient set already used in prior mathematical history;
- a duplicate coefficient set within the current reassessment;
- a duplicate Stage 21 mathematical exposure fingerprint.

Coefficient variation is therefore explicit rather than being inferred from different
surface wording.

## Representation freshness

For reassessments containing more than one question, the exact batch must vary
representation.

Examples currently supported by the exact families include:

- symbolic equivalence;
- fraction-strip reasoning;
- area-model reasoning;
- fraction-family construction;
- number-line reasoning;
- common-factor structure;
- rate statement;
- ratio table;
- double number line;
- unit-rate or proportion equation.

Representations are never allowed to introduce a question family outside the approved
SkillContract.

## Strategy freshness

Stage 21 varies mathematical strategy, including:

- scale-factor reasoning;
- cross-product checking;
- multiplicative scaling;
- same-point number-line equivalence;
- common-factor reduction;
- divide-to-one unit-rate reasoning;
- scaling both terms of an equivalent ratio.

## Context freshness

Context is treated as a mathematical-generation dimension, not just a vocabulary swap.

Reviewed ratio contexts include:

- production;
- travel;
- cost;
- recipe.

Fraction contexts include symbolic, model, number-line and factor-structure forms.

## Misconception-trap freshness

Exact items carry the misconception structure they are designed to expose, for example:

- additive instead of multiplicative equivalence;
- denominator-size-only comparison;
- scaling only one term;
- subtracting instead of reducing by a common factor;
- using the total as the unit rate.

A batch with more than one item must vary the misconception-trap dimension.

## Cognitive demand

Cognitive demand is retained separately from the visible
Easy / Medium / Challenging blueprint distribution.

Stage 21 uses:

- Standard;
- Stretch;
- Challenge.

The visible blueprint difficulty distribution is preserved for comparability while
internal exact parameter generation varies cognitive demand.

## Mathematical-distance rule

Against every supplied prior mathematical signature:

1. the coefficient signature must differ; and
2. at least two additional non-coefficient mathematical dimensions must differ.

Those dimensions are drawn from:

- question family;
- representation;
- strategy;
- context;
- misconception trap;
- cognitive demand.

This prevents a reassessment from passing merely because one number or sentence changed.

## Solver and verifier

Every exact Stage 21 item is generated through the shared
ExactSkillContractQuestionEngine.

Before the item is accepted:

- the exact solver produces the answer;
- the independent exact verifier must accept the answer;
- the question family must remain allowed by the Outcome/SkillContract.

Metadata records:

- mathematicalFreshnessVerified = true
- wordingOnlyFreshness = false
- solverVerified = true

## Legacy compatibility

Non-migrated Outcomes keep the existing Phase 36 reassessment generator.

That legacy route may continue using fingerprint and prompt-shape freshness until the
Outcome is explicitly migrated.

Once an Outcome is part of the exact Stage 19 registry, the Stage 21 exact route is
authoritative and executes before the legacy wording-freshness path.

## Source of truth

Machine-readable manifest:

src/Edulytics.Core/Mathematics/Curriculum/stage21-reassessment-migration-manifest.v1.json

Freshness contracts:

src/Edulytics.Core/Recovery/WeaknessRecoveryContracts.cs

Exact reassessment engine:

src/Edulytics.Services/Recovery/Stage21ExactReassessmentEngine.cs

Reassessment orchestrator:

src/Edulytics.Services/Recovery/EquivalentReassessmentGenerator.cs

CI closure gate:

tools/math_intelligence/stage21_reassessment_closure_audit.py

Acceptance coverage:

tests/Edulytics.Tests/MathematicsIntelligence/Stage21ReassessmentMigrationTests.cs

## Exit criteria

Stage 21 is complete only when:

- every Stage 20 exact Outcome remains mapped to the same SkillContract;
- all six required mathematical freshness dimensions are represented;
- prior exact exposure cannot be treated as fresh without reconstructable mathematical history;
- coefficient reuse is rejected;
- every new exact item differs mathematically from every supplied prior signature;
- multi-question exact reassessment batches vary representation, strategy, context, misconception trap and cognitive demand;
- only allowed SkillContract question families are used;
- every generated answer passes the independent exact verifier;
- Phase 36 equivalent-reassessment scope and visible difficulty-distribution validation remain green;
- non-migrated Outcomes retain legacy compatibility;
- Stages 17–20 closure gates remain green;
- Stage 21 closure and full repository CI are green;
- the merged commit is live on Render and startup/HTTP checks pass.

## Next programme stage

After Stage 21 is live on Render, execution proceeds to
**Programme Stage 22 — Game runtime migration**.
