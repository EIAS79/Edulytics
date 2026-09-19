# Polish National Mathematics — 1,569 Lesson Practice Remediation Master Plan

**Repository:** `EIAS79/Edulytics`  
**Status:** ACTIVE — canonical remediation plan for the 1,569 Polish National Mathematics lessons currently excluded from exact lesson Practice  
**Programme target:** Convert the current 1,569 `PL-NATIONAL-MATH` outcome-backed published lessons from `NON_STANDALONE_WITH_EVIDENCE` into exact, outcome-faithful learner Practice wherever they remain learner-facing lessons.  
**Catalogue target:** `4,453 / 4,453` learner-facing mathematics lessons Practice-ready.  
**Preserves:** fail-closed exact Practice, source provenance, no invented official outcomes, shared SkillContract architecture, deterministic solver/verifier, deterministic visuals, grade-aware difficulty, and cross-surface capability consistency.

---

# 0. Why this plan exists

The Mathematics Practice Completion programme currently reports:

```text
4,453 mathematics catalogue lessons
2,884 Practice-eligible / READY_VERIFIED
1,569 NON_STANDALONE_WITH_EVIDENCE
```

The 1,569 excluded rows are not generic empty tree nodes. They correspond to the Polish National Mathematics Phase 29 outcome-backed lesson set:

```text
PackCode: PL-NATIONAL-MATH
PedagogicalSourceType: OfficialFrameworkOnly
TitleProvenance: EdulyticsDerivedFromOfficialOutcome
IsSupporting: false
Status: Published
one official OutcomeCode per deterministic lesson identity
```

These lessons have learner-facing canonical content records and are seeded into the pedagogical lesson/content model. However, many lesson bodies were authored from broad domain-level fallback patterns. Multiple different official outcomes may therefore receive substantially similar explanations, examples, worked solutions, summaries, or difficulty treatment.

The defect is not that the official OutcomeCode is untrusted. The defect is that the Edulytics lesson body and exact Practice target may not yet exploit the specific semantics of that OutcomeCode with sufficient precision.

This plan therefore reopens full-catalogue Practice completion for those 1,569 lessons.

---

# 1. Non-negotiable product rule

A learner-facing published lesson must not be excluded from Practice merely because its current authored body is broad, repetitive, ambiguous, or insufficiently target-specific.

The remediation path is:

```text
Official OutcomeCode
    ↓
verified source evidence + exact source locator
    ↓
exact mathematical meaning of that outcome
    ↓
outcome-faithful Edulytics lesson target
    ↓
lesson-specific explanation / concepts / worked examples / solutions
    ↓
canonical SkillContract
    ↓
approved Question Families
    ↓
Solver
    ↓
independent Verifier
    ↓
required Visual / Representation
    ↓
grade-aware Difficulty
    ↓
alignment validation
    ↓
READY_VERIFIED
```

The following shortcut is prohibited:

```text
broad/repetitive lesson body
→ mark NON_STANDALONE
→ declare programme complete
```

If a row is proven not to be a learner-facing lesson at all, it must be removed from the learner lesson surface or re-modelled as curriculum metadata. It must not remain a published learner-facing lesson while being permanently excluded from Practice by classification alone.

---

# 2. Programme success criteria

The target state is:

```text
Polish outcome-backed lesson set:      1,569
Outcome evidence resolved:             1,569 / 1,569
Exact mathematical targets approved:   1,569 / 1,569
Lesson bodies target-specific:         1,569 / 1,569
SkillContracts approved:               1,569 / 1,569
Question-family routing approved:      1,569 / 1,569
Solver-ready:                          1,569 / 1,569
Verifier-ready:                        1,569 / 1,569
Visual-ready where required:           100%
Grade/difficulty-ready:                1,569 / 1,569
Alignment-ready:                       1,569 / 1,569
READY_VERIFIED:                        1,569 / 1,569
Permanent Practice exclusions:         0 learner-facing lessons
```

Full catalogue target:

```text
4,453 / 4,453 learner-facing lessons READY_VERIFIED
0 learner-facing lessons excluded only because of generic authored content
0 silent broad fallback
0 invented official outcome wording or semantics
```

---

# 3. Source-of-truth rule

For every Polish lesson, the authoritative reconstruction order is:

1. exact `OutcomeCode`;
2. exact `SourceLocator`;
3. accepted Polish official curriculum graph;
4. official ZPE / ELI source referenced by the curriculum pack;
5. surrounding official section/domain context needed to interpret the outcome;
6. existing Edulytics lesson body only as secondary evidence, never as authority over the official outcome.

Do not infer exact Practice from the generated title alone.

Do not treat repeated Phase 29 examples as proof of the intended mathematical target.

Do not invent a narrower official requirement than the source supports.

Edulytics may author pedagogical wording, explanations, examples, and exercises, but the mathematical target must remain fully supported by the official outcome evidence.

---

# 4. Remediation data contract

Every one of the 1,569 lessons must receive a machine-readable remediation record containing at least:

```text
LessonCode
OutcomeCode
Level
Pathway
OfficialDomain
SourceUrl
SourceLocator
OfficialEvidenceFingerprint

CurrentTitle
CurrentExplanationFingerprint
CurrentWorkedExampleFingerprint
CurrentSimilarityCluster

ExactOutcomeMeaning
ExactMathematicalTarget
TargetBoundary
Prerequisites
ConfusableTargets

ContentRepairStatus
PrimarySkillIds
SecondarySkillIds
AllowedQuestionFamilies
ForbiddenQuestionFamilies
RequiredRepresentations

SolverStatus
VerifierStatus
DifficultyStatus
AlignmentStatus
VisualStatus

FinalReadiness
BlockerCodes
Evidence
ReviewTrace
```

---

# 5. Stage P15 — Baseline and forensic inventory

## Objective

Create an authoritative inventory of all 1,569 Polish outcome-backed lessons and prove exactly why each was classified as non-standalone.

## Required work

- enumerate all 17 Polish curriculum scopes;
- reconcile exactly 1,569 lesson identities;
- capture each lesson's OutcomeCode, source locator, level, pathway and content fingerprint;
- detect duplicate or near-duplicate explanations, examples, solutions and summaries;
- detect grade-inappropriate examples;
- detect lessons whose current examples do not demonstrate the attached outcome;
- identify repeated broad-domain fallback patterns;
- identify any row that is genuinely not learner-facing.

## Required artifact

`artifacts/math-intelligence/pl-1569-remediation-baseline.json`

## Exit gate

- [ ] 1,569 / 1,569 rows inventoried.
- [ ] Every row has exact OutcomeCode and source evidence pointer.
- [ ] Duplicate-content clusters are quantified.
- [ ] Every row has a current defect classification.
- [ ] No row remains unexplained.

---

# 6. Stage P16 — Official outcome evidence reconstruction

## Objective

Resolve the exact mathematical semantics of every official OutcomeCode before rewriting content or authorizing Practice.

## Required work

For each lesson:

- retrieve/parse the exact official outcome evidence already accepted by the curriculum graph;
- use the surrounding official section only where needed to interpret scope;
- distinguish:
  - exact student action;
  - mathematical object;
  - conditions/constraints;
  - expected representation;
  - expected reasoning depth;
  - grade/pathway boundary;
- create a concise Edulytics `ExactOutcomeMeaning`;
- define `TargetBoundary` so nearby skills cannot silently leak into Practice;
- record ambiguity explicitly and resolve it from source evidence before promotion.

## Rules

- no mapping from title keywords alone;
- no generic domain target such as only "numbers", "geometry", or "algebra";
- no narrowing beyond what official evidence supports;
- no copied legal wording required in learner content;
- outcome semantics remain traceable to the official source.

## Required artifact

`artifacts/math-intelligence/pl-outcome-evidence-matrix.json`

## Exit gate

- [ ] 1,569 / 1,569 OutcomeCodes have approved exact semantics.
- [ ] 0 unresolved source ambiguities.
- [ ] 0 invented official targets.
- [ ] Every target has positive and negative scope boundaries.

---

# 7. Stage P17 — Rebuild lesson content to match the outcome

## Objective

Replace broad/repetitive Phase 29 fallback bodies with target-specific, grade-appropriate Edulytics lesson content.

## Required lesson content

Every repaired lesson must contain:

1. title faithful to the target;
2. explicit learning target;
3. explanation specific to the attached outcome;
4. concepts/rules actually needed by that outcome;
5. at least one target-specific worked example;
6. mathematically verified step-by-step solution;
7. common misconception/error specific to the target where pedagogically useful;
8. target-appropriate representation where applicable;
9. concise target-specific summary;
10. grade/pathway-appropriate depth.

## Quality rules

- no mass-copying one generic template across different outcomes;
- no unrelated examples;
- no Grade 1-style arithmetic examples in a Grade 10 lesson unless genuinely prerequisite and explicitly contextualized;
- repeated wording is allowed only when mathematically justified;
- numerical variation alone does not make two lesson bodies distinct;
- all worked examples must be independently verified.

## Required artifact

`artifacts/math-intelligence/pl-lesson-content-repair-audit.json`

## Exit gate

- [ ] 1,569 / 1,569 lesson bodies pass semantic alignment.
- [ ] 0 target-mismatched worked examples.
- [ ] 0 unexplained near-duplicate lesson bodies across distinct outcomes.
- [ ] Grade-depth checks pass.
- [ ] All worked examples verify mathematically.

---

# 8. Stage P18 — Exact SkillContracts and mapping

## Objective

Map every repaired outcome-faithful lesson to reusable canonical mathematics skills without creating one custom skill per lesson.

## Required work

- reuse existing canonical SkillIds wherever semantics match;
- add a new reusable SkillId only when the ontology genuinely lacks the target;
- record Primary, Secondary and Prerequisite roles correctly;
- define allowed and forbidden question families;
- define answer types and representation requirements;
- preserve one shared capability authority across Practice, assessment and AI generation.

## Mapping rule

```text
Official Outcome
+ repaired lesson evidence
→ exact reusable SkillContract
```

not:

```text
generated lesson title
→ broad keyword match
```

## Required artifact

`artifacts/math-intelligence/pl-lesson-skill-mapping-audit.json`

## Exit gate

- [ ] 1,569 / 1,569 approved exact mappings.
- [ ] 0 ambiguous primary-skill mappings.
- [ ] 0 broad-topic fallback.
- [ ] 0 invented official outcomes.
- [ ] Cross-curriculum equivalent targets reuse canonical skills where appropriate.

---

# 9. Stage P19 — Question families, Solver and Verifier completion

## Objective

Make every Polish lesson capable of generating exact, varied, independently verified Practice.

## Required work

For each approved SkillContract:

- ensure sufficient question-family coverage for the outcome;
- implement missing reusable families;
- support direct/reverse/missing-value/reasoning/representation/multi-step forms where the outcome requires them;
- implement Solver support;
- implement independent Verifier support;
- define answer equivalence;
- generate and validate distractors;
- add deterministic fixed-seed tests;
- ensure exact generation never silently falls back to contextual broad-topic questions.

## Required artifacts

- `artifacts/math-intelligence/pl-question-family-coverage-audit.json`
- `artifacts/math-intelligence/pl-solver-verifier-coverage-audit.json`

## Exit gate

- [ ] 1,569 / 1,569 family-ready.
- [ ] 1,569 / 1,569 solver-ready.
- [ ] 1,569 / 1,569 verifier-ready.
- [ ] Every enabled family declares answer-equivalence policy.
- [ ] Structural diversity passes approved fixed-seed tests.

---

# 10. Stage P20 — Visuals, representations and grade-aware difficulty

## Objective

Complete every representation and difficulty requirement needed by the repaired Polish lesson set.

## Required work

Where required by the outcome/family:

- number lines;
- fraction models;
- geometry diagrams;
- coordinate planes;
- graphs;
- vectors;
- tables;
- statistical visuals;
- probability diagrams;
- approved schematic 3D visuals.

Every visual must derive from the same structured parameters used by the Solver/Verifier.

Difficulty must be calibrated by level/pathway and structural cognitive demand, not by larger numbers alone.

## Required artifacts

- `artifacts/math-intelligence/pl-visual-coverage-audit.json`
- `artifacts/math-intelligence/pl-difficulty-grade-coverage-audit.json`

## Exit gate

- [ ] 100% required visuals deterministic and validated.
- [ ] No required-visual lesson can degrade to text-only.
- [ ] 1,569 / 1,569 difficulty-classified.
- [ ] Challenge items are structurally harder where curriculum depth requires it.
- [ ] Grade/pathway bounds pass calibration.

---

# 11. Stage P21 — Runtime activation and learner-surface verification

## Objective

Make repaired Polish lessons genuinely learner-facing Practice lessons, not merely audit-ready metadata.

## Required work

Verify the same exact contract is consumed by:

- Student Lesson Practice;
- Student Private Practice;
- Teacher Assessment Builder;
- Diagnostic/Adaptive;
- Reassessment;
- game runtime where applicable.

For each promoted lesson:

- Practice button/entry point resolves;
- question generation succeeds;
- answer submission grades correctly;
- solution/feedback is aligned;
- required visual renders;
- no broad fallback is invoked;
- repeated generations preserve target while providing valid variation.

## Sampling policy

Automated coverage must run for all 1,569 lessons. In addition, maintain a stratified human-review corpus covering:

- all 17 Polish scopes;
- each major mathematics domain;
- early primary, primary, lower secondary and upper secondary;
- basic/extended or pathway differences where applicable;
- visual and non-visual families;
- direct and reasoning-heavy families.

## Required artifact

`artifacts/math-intelligence/pl-runtime-practice-acceptance-audit.json`

## Exit gate

- [ ] 1,569 / 1,569 resolve through exact runtime Practice.
- [ ] 0 learner-surface authorization failures.
- [ ] 0 broad fallbacks.
- [ ] 0 answer-verification failures.
- [ ] Stratified human-review corpus approved.

---

# 12. Stage P22 — Full catalogue closure and permanent CI protection

## Objective

Close the programme only when the 1,569 Polish lessons have actually been repaired and promoted.

## Required final state

```text
Catalogue lessons:                  4,453
READY_VERIFIED learner lessons:     4,453
Polish remediated lessons:          1,569 / 1,569
Practice-eligible blocked:          0
Learner-facing NON_STANDALONE:      0
Broad fallback:                     0
Solver coverage:                    100%
Verifier coverage:                  100%
Required visual coverage:           100%
Grade/difficulty coverage:          100%
```

## Permanent CI failures

CI must fail if:

- any of the 1,569 lessons loses approved Outcome evidence;
- repaired content becomes semantically generic or target-mismatched;
- a worked example no longer verifies;
- a lesson loses exact Skill mapping;
- an unauthorized family is used;
- Solver or Verifier coverage disappears;
- a required visual loses deterministic renderer support;
- grade/difficulty bounds regress;
- runtime Practice falls back broadly;
- a learner-facing Polish lesson is reclassified as non-standalone merely to satisfy completion counts.

## Required final artifact

`artifacts/math-intelligence/pl-1569-final-closure-audit.json`

---

# 13. Execution strategy

The programme contains **8 stages: P15–P22**.

```text
P15 Baseline / forensic inventory
 ↓
P16 Official Outcome evidence reconstruction
 ↓
P17 Outcome-faithful lesson content reconstruction
 ↓
P18 Exact SkillContracts / mapping
 ↓
P19 Question Families + Solver + Verifier
 ↓
P20 Visuals + grade-aware difficulty
 ↓
P21 Runtime activation + learner-surface QA
 ↓
P22 Full-catalogue closure + permanent CI
```

Execution may be batched by level/pathway/domain for safe delivery, but the programme is one continuous remediation effort. A stage is not complete merely because work was classified; its exit gate must be proven by audit artifacts.

---

# 14. Recommended batching order

Prioritize batches that maximize reuse while preserving academic correctness:

1. Grades 1–3 foundational number/measurement/geometry;
2. Grades 4–6 arithmetic/fractions/measurement/data;
3. Grades 7–8 algebra/geometry/probability/statistics;
4. upper-secondary real numbers/algebra/functions;
5. upper-secondary geometry/trigonometry;
6. upper-secondary probability/statistics;
7. calculus/advanced topics where present;
8. cross-pathway deduplication and final residual queue.

This batching order is operational only. It must not alter official level/pathway boundaries.

---

# 15. Definition of Done — one repaired Polish lesson

A lesson is done only when all are true:

- [ ] official OutcomeCode and source locator verified;
- [ ] exact outcome semantics recorded;
- [ ] target boundary recorded;
- [ ] title and body are outcome-faithful;
- [ ] worked example is target-specific and independently verified;
- [ ] grade/pathway depth is appropriate;
- [ ] Primary SkillId approved;
- [ ] allowed/forbidden families explicit;
- [ ] Solver exists;
- [ ] independent Verifier exists;
- [ ] answer equivalence defined;
- [ ] required visual exists and validates;
- [ ] fixed-seed samples pass;
- [ ] alignment validator passes;
- [ ] runtime Student Practice resolves exact contract;
- [ ] no broad fallback;
- [ ] final status is `READY_VERIFIED`.

---

# 16. Definition of Done — programme

This programme is complete only when:

- [ ] all 1,569 Polish lessons have been individually remediated or, if proven not learner-facing, removed/re-modelled from the learner lesson catalogue rather than hidden behind Practice eligibility classification;
- [ ] 1,569 / 1,569 learner-facing Polish lessons are `READY_VERIFIED`;
- [ ] the full learner-facing mathematics catalogue is `4,453 / 4,453 READY_VERIFIED`;
- [ ] no generic Phase 29 fallback body remains as the final learner content for a distinct official outcome;
- [ ] no target is invented beyond official evidence;
- [ ] CI permanently prevents regression.

---

# 17. What must never happen

Do not:

```text
declare a generic lesson non-standalone merely to close the count
map from the generated title alone
reuse the same broad worked example across unrelated outcomes
invent official wording or a narrower official objective
use a broad contextual generator after an exact contract exists
accept generator answers without independent verification
increase difficulty only through larger numbers
let required visual mathematics degrade to text-only
mark P22 complete without 1,569-row evidence
claim 4,453/4,453 if learner-facing Polish lessons remain excluded from Practice
```

---

# 18. Progress tracker

| Stage | Status | Baseline | Target |
|---|---|---:|---:|
| P15 Baseline / forensic inventory | ⬜ Not started | 1,569 excluded | 1,569 audited |
| P16 Official outcome evidence | ⬜ Not started | broad fallback semantics | 1,569 exact outcome meanings |
| P17 Lesson content reconstruction | ⬜ Not started | repeated/general Phase 29 bodies | 1,569 target-specific bodies |
| P18 SkillContracts / mapping | ⬜ Not started | excluded from exact lesson Practice | 1,569 approved mappings |
| P19 Families / Solver / Verifier | ⬜ Not started | no exact routing for excluded set | 1,569 generation-ready |
| P20 Visuals / Difficulty | ⬜ Not started | not fully evaluated for excluded set | 100% requirements ready |
| P21 Runtime / learner QA | ⬜ Not started | no exact Practice for excluded set | 1,569 exact runtime Practice |
| P22 Catalogue closure / CI | ⬜ Not started | 2,884 / 4,453 exact Practice | 4,453 / 4,453 |

---

# 19. Relationship to the existing Mathematics Practice Master Plan

This document is a corrective continuation of:

`docs/math-v2/MATHEMATICS_PRACTICE_COMPLETION_MASTER_PLAN.md`

The earlier P13/P14 closure remains valid for the 2,884 lessons already verified. However, the full-catalogue completion claim is reopened for the 1,569 Polish outcome-backed published lessons because they were excluded by `OFFICIAL_FRAMEWORK_FALLBACK_IDENTITY` rather than remediated through the same content → target → skill → family → solver/verifier → Practice pipeline.

Until P22 is complete, the product-level full mathematics Practice programme must be treated as **reopened for Polish lesson remediation**.
