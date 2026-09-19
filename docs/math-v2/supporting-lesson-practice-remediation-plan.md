# Supporting Lesson Practice Remediation Plan

> **Completion authority (2026-09-19):** This remediation plan remains the historical/foundation plan. The canonical completion tracker is [`MATHEMATICS_PRACTICE_COMPLETION_MASTER_PLAN.md`](./MATHEMATICS_PRACTICE_COMPLETION_MASTER_PLAN.md). Under the new completion rule, `EXPLICITLY_BLOCKED` is a temporary protective state, not programme completion; Practice-eligible lessons must reach `READY_VERIFIED`.

**Repository:** `EIAS79/Edulytics`  
**Status:** Approved execution plan  
**Baseline commit:** `2c761c2e231da741a117352f9f4de489a4fe3cbc`  
**Baseline date:** 2026-09-18  
**Scope:** Supporting / pedagogical mathematics lessons, plus the shared Teacher Assessment Builder and Student Private Practice generation paths required to keep mathematical capability consistent across the product.  
**Primary goal:** Make Practice and AI-generated mathematics lesson/outcome-aligned, mathematically correct, grade-appropriate, independently verifiable, and fail-closed without requiring an official curriculum outcome mapping for Supporting lessons.

---

## 1. Why this plan exists

Edulytics has a large population of mathematics lessons that are valid pedagogical lessons but do not have an independent official curriculum outcome mapping.

For these lessons, the absence of an official outcome must **not** mean that Practice is impossible.

A Supporting lesson may still have:

- a trusted pedagogical source;
- an exact mathematical target;
- strong worked examples;
- a precise Edulytics SkillContract;
- safe question families;
- solver capability;
- independent verification.

The current architectural weakness is that some Practice and game availability paths depend on exact routes or formal mappings that are narrower than the mathematical evidence available in the lesson.

The target architecture is therefore:

```text
Supporting Lesson
        ↓
Trusted Source / Provenance
        ↓
Exact Mathematical Target
        ↓
LessonSkillProfile
        ↓
LessonPracticeContract
        ↓
Allowed Question Families
        ↓
Structured Problem Generation
        ↓
Lesson Alignment Validation
        ↓
Solver
        ↓
Independent Verifier
        ↓
Practice
```

An official curriculum outcome is **not** a prerequisite for this chain.

Official curriculum mappings remain separate and are never synthesized or inferred as facts.

---

## 2. Audited baseline

The current baseline audit treats a lesson as part of this remediation population when it has no official `OutcomeCodes`, matching the current runtime Supporting fallback policy based on zero official outcomes.

At baseline:

| Population | Count |
|---|---:|
| Supporting / pedagogical lessons without official outcome mappings | 1,349 |
| Mathematics readiness `READY_VERIFIED` | 7 |
| Already covered by the exact Stage 18 Practice-contract path | 6 |
| Verified mathematically but not yet covered by the exact Stage 18 Practice-contract set | 1 |
| Not yet exact-Practice ready | 1,342 |

Skill-resolution state among the unresolved population:

| Resolution state | Count |
|---|---:|
| `ONTOLOGY_GAP` | 1,200 |
| `REVIEW_REQUIRED` | 95 |
| `HIGH_CONFIDENCE_CANDIDATE` | 33 |
| `AMBIGUOUS` | 14 |

The semantic-content audit also reports **335 `CONTENT_WEAK` Supporting lessons**. This is an overlapping quality dimension, not an additional 335 lessons.

The remediation plan must therefore solve two independent questions for every lesson:

1. **What exact mathematics does this lesson teach?**
2. **Is the lesson content strong enough to prove and support that target?**

---

## 3. Governing principles

### 3.1 Supporting does not mean mathematically untrusted

`Supporting` is provenance/mapping status, not a quality judgment.

A Supporting lesson can become `READY_VERIFIED` for Practice without receiving a fake official curriculum outcome.

### 3.2 No broad-topic Practice authorization

The following path must not authorize production lesson Practice:

```text
lesson title / unit title
→ broad semantic keyword
→ generic contextual generator
```

Broad semantic detection may assist offline analysis, but it is not sufficient production authorization.

### 3.3 No 1,349 custom generators

The scalable architecture is:

```text
1,349 supporting lessons
        ↓
specific lesson mappings
        ↓
shared canonical skills
        ↓
shared question families
        ↓
shared mathematics engine
```

### 3.4 Fail closed

If Edulytics cannot prove lesson alignment or mathematical correctness, it must not silently fall back to a nearby topic.

### 3.5 Curriculum truth remains separate

Practice readiness and official curriculum alignment are separate claims.

A lesson may be:

```text
Practice readiness: READY_VERIFIED
Official curriculum outcome mapping: NONE
```

This is valid.

### 3.6 Mentioned skill is not automatically a Practice target

Skills found in lesson text must be classified as:

- **Primary** — the lesson is intended to teach/measure this;
- **Secondary** — meaningfully used inside the lesson but not the main target;
- **Prerequisite** — assumed prior knowledge needed to solve the target;
- **Incidental** — mentioned but not a Practice target.

Practice generation is authorized primarily from Primary skills.

If a lesson genuinely has multiple Primary skills, the contract may allocate explicit coverage percentages across them.

---

## 4. Core model

### 4.1 Keep `LessonSkillProfile` as the academic mapping layer

The existing `LessonSkillProfile` already supports:

- source type;
- primary skills;
- secondary skills;
- prerequisites;
- allowed question families;
- forbidden question families;
- mapping confidence;
- content-audit status;
- generation readiness;
- evidence.

This remains the canonical lesson-to-mathematics mapping model.

### 4.2 Add a runtime `LessonPracticeContract` projection

Do not create a second competing academic mapping database.

`LessonPracticeContract` should be a validated runtime contract derived from:

```text
LessonSkillProfile
+ Question Family Registry
+ Solver Capability Registry
+ Verifier Capability Registry
+ Representation Registry
+ Content Audit
+ Practice policy
```

It should contain, at minimum:

```text
lessonCode
contractVersion
sourceType
sourceAuthority
sourceLocator
contentVersion

primarySkillIds
secondarySkillIds
prerequisiteSkillIds

allowedQuestionFamilies
forbiddenQuestionFamilies

answerTypes
allowedRepresentations
difficultyBounds
lessonMode            # e.g. BUILD / APPLY when applicable

alignmentRules
requiredCapabilities

readiness
readinessReasons
```

A contract exists for runtime generation only after all required checks pass.

---

## 5. Workstream R1 — Admit the already-safe lessons

### Scope

- 6 Supporting lessons already covered by exact Stage 18 Practice contracts;
- 1 additional Supporting lesson already `READY_VERIFIED` mathematically but outside the current exact Practice-contract set.

### Work

1. Resolve all 7 through the same general Supporting Practice resolver.
2. Remove special-case dependence on Stage 18-only membership as the long-term authority.
3. Preserve the current exact solver/verifier behavior.
4. Add regression tests proving that these lessons never fall back to broad contextual generation.

### Exit criteria

- 7/7 resolve to an exact `LessonPracticeContract`;
- all generated items pass alignment + solver + independent verifier;
- UI Practice availability is driven by Practice readiness, not game routing.

---

## 6. Workstream R2 — Promote the 33 high-confidence candidates

### Scope

`HIGH_CONFIDENCE_CANDIDATE = 33`.

These are not production mappings yet, but the deterministic resolver already has strong evidence for a likely exact SkillId.

### Process

For each candidate:

1. Read title, explanation, key concepts, worked examples, solutions, summary, and source provenance.
2. Confirm or correct:
   - Primary Skill;
   - Secondary Skills;
   - Prerequisites.
3. Define the lesson's allowed and forbidden question families.
4. Verify that every allowed family has:
   - required solver capability;
   - independent verifier;
   - answer evaluator;
   - valid representation support.
5. Promote the mapping to an approved `LessonSkillProfile`.
6. Generate a `LessonPracticeContract`.
7. Run property/batch tests across the allowed families.

### Exit criteria

Every promoted lesson is either:

- `READY_VERIFIED`; or
- explicitly blocked with a concrete reason.

No candidate remains silently unresolved.

---

## 7. Workstream R3 — Resolve the 95 review-required lessons

### Scope

`REVIEW_REQUIRED = 95`.

### Review model

The system should prepare an evidence packet automatically. The academic reviewer should not reconstruct the lesson from scratch.

Evidence packet:

```text
Lesson code/title
Source authority
Source locator
Content version
Explanation
Key concepts
Worked examples
Step-by-step solutions
Common mistakes
Quick summary

Candidate primary skills
Candidate secondary skills
Candidate prerequisites
Positive evidence
Negative evidence
Competing candidates
Suggested allowed families
Suggested forbidden families
Capability coverage
Content-audit findings
```

### Reviewer actions

The review interface/workflow must support:

- Approve candidate;
- Change Primary Skill;
- Mark a skill Secondary;
- Mark a skill Prerequisite;
- Reject a candidate;
- Request content strengthening;
- Block lesson Practice with a reason.

### Reviewer role

This is a centralized Edulytics academic mathematics review, not a per-school teacher responsibility.

### Persistence

The decision is versioned and stored once. It is not repeated for every generated question.

### Exit criteria

All 95 receive an explicit approved mapping or an explicit blocked state.

---

## 8. Workstream R4 — Resolve the 14 ambiguous lessons correctly

### Scope

`AMBIGUOUS = 14`.

### Critical rule

Do **not** generate from every detected candidate simply because the skill appears in the lesson text.

Example:

```text
Lesson: Add and subtract fractions

Primary:
  fractions.add_subtract

Secondary:
  fractions.equivalent

Prerequisite:
  number.whole.add_subtract
```

A generic whole-number addition item must not be generated merely because whole-number addition appears as a prerequisite.

### Multi-primary lessons

If evidence proves that two or more skills are true learning targets, encode them explicitly:

```yaml
primaryCoverage:
  skill.a: 60
  skill.b: 40
```

Question selection must respect the configured coverage.

### Exit criteria

Every ambiguous lesson has a deterministic Primary/Secondary/Prerequisite classification and no unresolved equal candidate competition.

---

## 9. Workstream R5 — Close the 1,200 ontology gaps

### Root cause

`ONTOLOGY_GAP` does not mean that the lesson has no mathematical target.

It means the current exact-skill registry and resolution rules do not yet represent or recognize that target precisely enough.

At baseline, almost all ontology-gap lessons have no positive exact-skill rule match.

### Required approach

Do not create one SkillId per lesson.

Instead:

```text
1,200 lessons
        ↓
extract candidate mathematical targets
        ↓
normalize equivalent targets
        ↓
cluster by mathematical meaning
        ↓
map to existing canonical skills where possible
        ↓
create missing reusable canonical skills only when necessary
```

### For every new canonical skill

A SkillId is incomplete unless the system also defines:

1. exact semantics/domain;
2. resolution evidence rules;
3. prerequisite relationships;
4. allowed question families;
5. forbidden/confusable families;
6. answer types;
7. representations;
8. solver requirements;
9. independent verification requirements;
10. difficulty features;
11. misconception/negative discriminators where needed.

### Clustering policy

Clusters must be based on mathematical semantics, not string similarity alone.

AI/LLM analysis may propose clusters offline, but deterministic evidence and/or academic approval authorizes the final mapping.

### Reuse rule

If many lessons teach the same mathematical skill, they must share the same core mathematical implementation.

Lesson-specific differences belong in:

- allowed-family subset;
- parameter constraints;
- representation requirements;
- lesson mode;
- context/language constraints;
- difficulty boundaries.

### Exit criteria

No Supporting lesson remains `ONTOLOGY_GAP` merely because the registry lacks a reusable skill.

Any remaining blocked lesson must have an explicit non-ontology reason.

---

## 10. Workstream R6 — Repair the 335 content-weak lessons

### Meaning of `CONTENT_WEAK`

A lesson may have a recognizable target while its worked examples fail to demonstrate that target specifically.

Generic content such as:

```text
Read the problem.
Choose an operation.
Calculate.
Check your answer.
```

is not target-specific evidence.

### Repair policy

Content must be strengthened using the trusted source plus the exact approved mathematical target.

For each weak lesson:

1. preserve source provenance;
2. identify the exact target;
3. add target-specific explanation;
4. add at least one target-specific worked example;
5. add exact step-by-step solution reasoning;
6. add a target-specific misconception/common mistake;
7. add an independent check or alternative representation when appropriate;
8. rerun semantic-content audit.

### Mathematical safety

Any numeric/symbolic worked example generated during repair must be passed through the mathematics engine and independent verifier before publication.

### Template-reuse control

Repeated generic worked-example and solution templates across unrelated targets must be flagged.

Reuse is allowed only when the mathematics is genuinely shared.

### Exit criteria

A lesson cannot become `READY_VERIFIED` while its content remains `CONTENT_WEAK`.

---

## 11. Workstream R7 — Build reusable Question Families

Question families must be curriculum-neutral and reusable.

Example:

```text
Skill: fractions.simplify

Families:
  fractions.simplify.numeric
  fractions.simplify.missing_factor
  fractions.simplify.identify_simplest_form
  fractions.simplify.reasoning
```

A lesson contract chooses the subset appropriate for that lesson.

### Question-family contract

Every family must define:

```text
familyId
version
skillId
structured problem model / Math IR
parameter constraints
answer type
required capabilities
verification policy
representations
difficulty features
misconception controls
```

### Prohibition

No family may be enabled for a lesson merely because it belongs to the same broad topic.

---

## 12. Workstream R8 — Structured generation, not free-text mathematics

Production generation should create a structured problem object first.

Example:

```json
{
  "family": "fractions.simplify.numeric",
  "targetSkill": "fractions.simplify",
  "numerator": 18,
  "denominator": 24
}
```

Then:

```text
structured problem
→ solver
→ independent verifier
→ answer evaluator contract
→ renderer / wording
```

LLM assistance may vary wording or context only within the contract.

It must not choose an unauthorized mathematical target.

---

## 13. Workstream R9 — Independent lesson-alignment validation

Mathematical correctness and lesson alignment are different checks.

Every generated item must pass both.

### Gate A — Lesson Alignment

Validate:

- target SkillId is Primary for the lesson;
- question family is explicitly allowed;
- family is not forbidden;
- lesson-mode requirements are satisfied;
- parameter constraints are satisfied;
- required representation is valid;
- no unrelated Primary skill is introduced;
- any secondary/prerequisite skill remains subordinate to the target.

### Gate B — Mathematical Verification

Validate:

- problem instance is mathematically valid;
- solution is correct;
- answer representation is valid;
- equivalent answers are graded correctly;
- constraints/domain assumptions hold.

An item can be mathematically correct and still fail lesson alignment.

Such an item must be rejected.

---

## 14. Workstream R10 — Fix Practice availability and UI authority

Practice availability must no longer be equivalent to game-renderer availability.

Target policy:

```text
Lesson
↓
Resolve LessonPracticeContract
↓
READY_VERIFIED / approved READY_CONTEXTUAL?
    ├─ No  → no exact Practice
    └─ Yes → Practice available
              ↓
              Game renderer available?
              ├─ Yes → interactive/game presentation allowed
              └─ No  → standard Practice presentation
```

### Required architectural change

The authoritative resolver should live in Core/Services, not in the Web view/controller.

UI, Student Private Practice, game runtime, assessment planning, diagnostic, and reassessment should consume the same lesson capability decision so that visibility and generation cannot disagree.

### Broad fallback policy

Once a lesson has an approved exact contract, broad contextual generation is forbidden.

For Supporting lesson-scoped Practice without an approved contract, do not silently generate a nearby-topic item.

---

## 15. Workstream R11 — Unify Supporting Practice, Teacher AI, and Student AI on one capability authority

### Why this is required

The current product has three related generation surfaces:

- Supporting lesson Practice;
- Teacher Assessment Builder AI generation;
- Student Private Practice AI generation.

Teacher Assessment Builder and Student Private Practice already share the `UniversalMathematicsQuestionGenerationEngine`, but the current universal engine can still route recognisable mathematics through broad contextual topic detection. Supporting Practice also has separate exact-contract/game-routing logic.

That creates a consistency risk:

```text
same mathematical target
→ Supporting Practice makes one capability decision
→ Teacher AI makes another
→ Student AI makes another
```

The target architecture is one shared capability decision:

```text
Lesson / Outcome
        ↓
Exact Skill / SkillContract
        ↓
Allowed Question Families
        ↓
Required Solver + Verifier + Representation
        ↓
Capability / Readiness decision
        ├─ Supporting Practice
        ├─ Teacher Assessment Builder
        └─ Student Private Practice
```

### Required changes

1. Introduce one authoritative mathematics capability resolver in Core/Services.
2. Make Supporting Practice, Teacher Assessment Builder, and Student Private Practice consume the same exact skill/family/capability registry.
3. Route exact supported mathematics through the shared structured generation kernel.
4. Preserve official-outcome provenance for Teacher Assessment Builder where an official outcome exists.
5. Preserve Supporting provenance without inventing an official outcome.
6. Once an exact contract exists, forbid broad contextual fallback for all three surfaces.
7. If no exact contract exists, any contextual path must remain explicitly `AiAssisted` and must not be presented as exact lesson/outcome alignment.
8. Add cross-surface regression tests proving the same target resolves to the same SkillId, family constraints, solver, verifier, and readiness decision.

### Exit criteria

- one capability resolver is authoritative for all three surfaces;
- no duplicate curriculum-specific allowlists contradict each other;
- exact-contract mathematics never drops back to `CurriculumContextCheck`;
- generation metadata states whether an item is `READY_VERIFIED` or contextual-assisted;
- Teacher and Student generation cannot silently produce a broader nearby topic when an exact target was requested.

---

## 16. Workstream R12 — Complete grade-aware Geometry and high-school visual mathematics coverage

### Current audited weakness

Geometry generation exists today, but the contextual generator is much broader and shallower than the Grade 10–12 curriculum content.

The current universal contextual path can recognise broad topics such as:

- geometry;
- angle;
- Pythagorean/trigonometric text;
- coordinate/slope;
- measurement.

However, current contextual generation can reduce several distinct high-school targets to generic items such as:

- rectangle area/perimeter;
- complementary/supplementary angles;
- 3-4-5 Pythagorean triples.

This is insufficient for Grade 10–12 lessons such as:

- coordinates and straight-line graphs;
- angle relationships;
- congruence and similarity;
- perimeter and area;
- surface area and volume;
- Pythagoras theorem;
- trigonometric ratios;
- extended trigonometric modelling.

In particular, broad `TRIGONOMET...` context must not collapse into a Pythagorean question when the target is `sin/cos/tan`, and `surface area and volume` must not collapse into rectangle area.

### Required exact skill expansion

Create or complete reusable canonical skills for the high-school geometry domain, including at minimum the targets actually present in the catalogues:

```text
geometry.coordinate.straight_line
geometry.angles.relationships
geometry.congruence
geometry.similarity
geometry.perimeter_area
geometry.surface_area
geometry.volume
geometry.right_triangle.pythagorean
trigonometry.right_triangle.sin_cos_tan
trigonometry.right_triangle.solve_side
trigonometry.right_triangle.solve_angle
trigonometry.modelling
```

The exact final SkillIds may differ if equivalent canonical skills already exist; duplicate semantics must not be introduced.

### Required question-family coverage

Each production-ready geometry skill must have explicit reusable families.

Examples:

```text
geometry.similarity.find_missing_length
geometry.similarity.scale_factor
geometry.congruence.identify_criterion

geometry.surface_area.prism
geometry.volume.prism
geometry.volume.composite

geometry.angles.parallel_lines
geometry.angles.polygon
geometry.angles.reasoning

geometry.coordinate.gradient_between_points
geometry.coordinate.line_equation

trigonometry.right_triangle.find_side
trigonometry.right_triangle.find_angle
trigonometry.modelling.contextual
```

### Grade-aware difficulty

Difficulty must not be implemented merely by increasing integers.

The capability contract must support grade/level-appropriate features such as:

- number domain and precision;
- one-step versus multi-step reasoning;
- exact versus rounded answers;
- compound/composite shapes;
- missing-side versus missing-angle trigonometry;
- representation complexity;
- algebraic reasoning inside geometry;
- modelling/context complexity.

A Grade 10 or Grade 12 request must not receive a primary-level rectangle question solely because the word `geometry` matched.

### Geometry representations and diagrams

Where the target depends on a figure, the question family must define a structured geometry representation rather than relying only on prose.

The representation contract should be capable of storing:

```text
points / coordinates
segments
angles
parallel/perpendicular constraints
shape type
labelled dimensions
unknown marker
diagram metadata
```

The renderer may present this as SVG/canvas/other deterministic visual output, but the mathematical model must remain structured and solver-verifiable.

A diagram is a representation of the underlying problem model, not the source of mathematical truth.

### Solver and verifier requirements

Every new production geometry family must have:

- a deterministic structured problem model;
- solver support;
- an independent verifier;
- domain/constraint validation;
- answer-evaluator rules;
- representation validation;
- grade/difficulty feature validation.

Current exact Geometry V2 capabilities that remain `ShadowVerified` may be promoted only after their production routing, curriculum/skill mapping, and acceptance gates are satisfied.

### Cross-surface requirement

When a geometry skill becomes production-ready, it must become available through the shared R11 capability authority to:

```text
Supporting Practice
Teacher Assessment Builder
Student Private Practice
```

There must not be a separate geometry implementation for each surface.

### Exit criteria

- Grade 10–12 geometry targets no longer collapse into generic rectangle/angle/Pythagorean fallbacks;
- exact trigonometric-ratio questions are distinct from Pythagorean questions;
- congruence, similarity, surface area, volume, coordinate geometry, angle relationships, Pythagoras, and required right-triangle trigonometry have explicit capability states;
- every enabled family passes structured generation + alignment + solver + independent verifier;
- grade-aware difficulty is enforced by capability features, not only larger numbers;
- figure-dependent questions use validated structured representations;
- Teacher AI and Student AI use the same production-ready geometry families as Supporting Practice.

---

## 17. Source-backed Supporting lessons

For source-backed Supporting lessons, retain exact provenance.

Example:

```text
Curriculum placement:
  Cambridge Primary Stage 6

Pedagogical source:
  UK DfE Mathematics Guidance

Source locator:
  6AS/MD-1

Official Cambridge outcome mapping:
  not established

Edulytics exact mathematical target:
  approved

Practice readiness:
  READY_VERIFIED
```

This permits accurate Practice without falsely claiming an official Cambridge mapping.

Where multiple Edulytics lessons share one trusted source criterion (for example BUILD and APPLY), they may share the same core mathematical target while applying different lesson-mode family constraints.

---

## 18. Versioning and provenance for generated items

Every generated Practice item must preserve enough metadata to reconstruct why it was generated.

Persist at least:

```text
lessonCode
lessonContentVersion
lessonPracticeContractVersion
sourceAuthority
sourceLocator

primarySkillId
questionFamilyId
questionFamilyVersion
structured parameters / Math IR identity

solverId/version
verifierId/version
answerEvaluatorId/version

alignment result
generation seed
difficulty features
timestamp
```

Historical items must remain explainable after registry changes.

---

## 19. CI and automated safety gates

The remediation is incomplete without CI enforcement.

### Registry integrity

CI must fail for:

- unknown SkillIds;
- unknown QuestionFamilyIds;
- family/skill mismatch;
- allowed and forbidden overlap;
- missing required capabilities;
- invalid readiness transitions;
- orphan lesson mappings.

### Generation property tests

For each production-enabled family, generate a sufficiently large deterministic corpus across:

- difficulty bands;
- parameter boundaries;
- representations;
- sign/range/domain variations.

Every item must pass:

```text
Generate
→ Alignment Validate
→ Solve
→ Independent Verify
→ Grade
```

### Negative tests

CI must prove that:

- forbidden families are rejected;
- unrelated-topic questions are rejected;
- prerequisite-only items cannot replace the Primary skill;
- unsupported capabilities fail closed;
- exact-contract lessons cannot use broad fallback.

### Content change gate

When a lesson's target-bearing content changes:

```text
content hash changed
→ compatibility audit
→ if target semantics changed:
     contract requires review
```

---

## 20. Execution order

Execute continuously in this order:

```text
R1  Generalize the 7 already-verified Supporting lessons
↓
R2  Promote the 33 high-confidence candidates
↓
R3  Resolve the 95 review-required mappings
↓
R4  Resolve the 14 ambiguous lessons
↓
R5  Cluster and close the 1,200 ontology gaps
↓
R6  Repair the 335 overlapping content-weak lessons
↓
R7  Complete reusable question-family coverage
↓
R8  Enforce structured generation
↓
R9  Enforce independent alignment + math verification
↓
R10 Make Practice readiness the single runtime/UI authority
↓
R11 Unify Supporting Practice + Teacher AI + Student AI capability authority
↓
R12 Complete grade-aware Geometry and high-school visual mathematics coverage
```

R5 and R6 should overlap operationally: a lesson may require both a new/reused skill and content strengthening before admission.

R11 should reuse the exact capability contracts created by R1–R10 rather than creating a parallel generation architecture. R12 then expands the shared engine once so the new geometry capabilities become available consistently across all three generation surfaces.

---

## 21. Definition of Done

Every Supporting lesson must end in exactly one of two terminal states.

### A. `READY_VERIFIED`

Required:

- trusted provenance recorded;
- exact mathematical target known;
- approved Primary Skill(s);
- Secondary/Prerequisite classification where needed;
- sufficient target-specific lesson content;
- allowed families defined;
- forbidden families defined;
- answer type defined;
- representation support defined;
- solver capability present;
- independent verifier present;
- alignment validator passes;
- difficulty constraints defined;
- versioned Practice contract available;
- Practice route available;
- broad fallback disabled.

### B. `EXPLICITLY_BLOCKED`

The reason must be machine-readable and human-readable, for example:

- insufficient source evidence;
- academic review unresolved;
- skill definition missing;
- question family missing;
- solver capability missing;
- verifier capability missing;
- representation missing;
- content weak;
- mapping conflict.

No lesson may remain silently unclassified.

---

## 22. Success metrics

The programme is complete when:

- 100% of Supporting lessons have a terminal readiness state;
- 0 Supporting lessons rely on silent broad-topic fallback for lesson-scoped Practice;
- 0 fake official curriculum outcomes are created;
- 100% of production Practice items carry lesson/skill/family provenance;
- 100% of production Practice items pass lesson-alignment validation;
- 100% of production Practice items pass solver + independent verifier;
- all multi-skill lessons have explicit Primary/Secondary/Prerequisite semantics;
- all `CONTENT_WEAK` lessons are repaired or explicitly blocked;
- all `ONTOLOGY_GAP` lessons are resolved into reusable canonical skills or explicitly blocked for a non-ontology reason;
- Practice UI capability and generation capability use the same authoritative resolver;
- Supporting Practice, Teacher Assessment Builder, and Student Private Practice use one shared exact mathematics capability authority;
- exact-contract items never silently fall back to `CurriculumContextCheck`;
- Grade 10–12 geometry requests resolve to grade-appropriate exact families rather than generic rectangle/angle/Pythagorean substitutes;
- all enabled high-school geometry families have solver, independent verifier, alignment checks, and validated representations.

---

## 23. Non-goals

This plan does **not**:

- convert DfE criteria into fake Cambridge outcomes;
- require official outcomes for Supporting Practice;
- create one generator per lesson;
- treat every skill mentioned in a lesson as a Practice target;
- allow title-only generation;
- allow game availability to define mathematical capability;
- automatically approve LLM-proposed mappings without evidence;
- claim that a passing solver alone proves lesson alignment;
- treat the current broad geometry keyword fallback as sufficient high-school geometry coverage;
- use a Pythagorean item as a substitute for a trigonometric-ratio target;
- treat larger numbers alone as evidence of Grade 10–12 difficulty.

---

## 24. First implementation slice

The first code slice after this plan is committed should:

1. introduce/generalize the authoritative Supporting `LessonPracticeContract` resolver;
2. route the 7 already mathematically verified Supporting lessons through it;
3. make Practice availability depend on that resolver;
4. keep game rendering optional;
5. prevent broad fallback for exact-contract lessons;
6. add regression tests for UI/generation consistency;
7. preserve all current official-outcome behavior unchanged.

Only after this shared path is stable should the 33 high-confidence candidates be promoted into it.

---

## 25. Authority of this document

For Supporting Lesson Practice remediation:

1. **Current code + tests + generated audits** remain the authority for what is actually implemented.
2. **This document** is the execution authority for closing the Supporting Practice gap identified on the baseline commit.
3. The broader Integrated Mathematics Curriculum Master Execution Plan and the 1500-Lesson Curriculum Intelligence Roadmap remain the governing architecture and detailed design references.

When implementation changes the baseline counts, update this document or publish a generated progress report rather than preserving stale counts as current truth.
