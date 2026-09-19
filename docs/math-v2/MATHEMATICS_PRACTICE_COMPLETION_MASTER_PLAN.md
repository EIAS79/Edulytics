# Edulytics Mathematics Practice Completion Master Plan

**Repository:** `EIAS79/Edulytics`  
**Status:** ACTIVE — canonical completion tracker  
**Baseline commit:** `c4a4592d1873af56da790235224195c48e3f92cc`  
**Baseline date:** 2026-09-19  
**Scope:** Full mathematics lesson catalogue, with an immediate hard-completion target for all 1,349 Supporting lessons and systematic Practice-readiness review for all 4,453 mathematics lessons.  
**Supersedes:** the old interpretation that a lesson reaching `EXPLICITLY_BLOCKED` is a successful terminal completion state.  
**Preserves:** the existing fail-closed architecture, exact SkillContract approach, shared mathematics kernel, solver/verifier separation, source provenance, and prohibition on invented official outcome mappings.

---

## 0. Purpose of this document

This is the single operational reference for completing Edulytics mathematics Practice.

It must answer, at any point in time:

1. How many lessons exist?
2. How many are truly `READY_VERIFIED`?
3. How many are blocked?
4. Why is each blocked lesson blocked?
5. What reusable capability will remove each blocker?
6. Which content still needs repair?
7. Which skills still lack sufficient question variety?
8. Which questions require diagrams/graphs/vectors/visual representations?
9. Which workstreams are complete?
10. What remains before Practice can be called complete?

This document is a **living execution tracker**. Every PR that materially changes curriculum Practice readiness must update the status tables and baseline counts in this file.

---

# 1. Non-negotiable completion rule

The previous remediation programme correctly introduced fail-closed behavior, but its terminal-state definition allowed:

```text
READY_VERIFIED
or
EXPLICITLY_BLOCKED
= terminally classified
```

That is useful for safety, but it is **not** sufficient as the product completion definition.

For this plan:

```text
BLOCKED = temporary protective state
READY_VERIFIED = successful Practice completion state
```

For the 1,349 Supporting lessons in this programme:

```text
1349 Supporting lessons
→ 1349 exact lesson targets
→ 1349 approved SkillContracts
→ required reusable Question Families
→ required representations/diagrams
→ required Solver capability
→ independent Verifier capability
→ grade/difficulty validation
→ alignment validation
→ 1349 READY_VERIFIED
→ 0 BLOCKED
```

No milestone may be declared fully complete merely because every lesson has been classified.

---

# 2. Baseline at plan start

## 2.1 Full mathematics catalogue

| Metric | Baseline |
|---|---:|
| Mathematics lessons | 4,453 |
| `READY_VERIFIED` | 78 |
| Not `READY_VERIFIED` | 4,375 |
| Canonical skills in registry | 65 |
| Question families in registry | 89 |
| Capabilities in registry | 86 |
| Skills with exactly one question family | 48 |
| Skills with zero question families | 6 |

The 4,453-lesson catalogue is the review scope. Not every catalogue node must automatically be treated as a standalone Practice target, but **every lesson must be explicitly classified** as either:

- `PRACTICE_ELIGIBLE`; or
- `NON_STANDALONE` with evidence explaining why it should not have independent Practice.

Every `PRACTICE_ELIGIBLE` lesson must eventually reach `READY_VERIFIED`.

## 2.2 Supporting lesson hard target

| Metric | Baseline |
|---|---:|
| Supporting lessons | 1,349 |
| `READY_VERIFIED` | 69 |
| `EXPLICITLY_BLOCKED` | 1,280 |
| Terminal-state classification coverage | 100% |
| Actual Supporting Practice readiness | 69 / 1,349 = 5.1% |

Current blocker breakdown:

| Blocker | Count | Required remediation |
|---|---:|---|
| `CONTENT_WEAK` | 329 | Repair lesson content and rerun semantic audit |
| `ACADEMIC_REVIEW_REQUIRED` | 453 | Resolve/approve exact mathematical target and SkillContract |
| `SOLVER_CAPABILITY_MISSING` | 497 | Build reusable skill/family/solver/verifier capability |
| `REPRESENTATION_MISSING` | 1 | Build required representation support |
| **Total** | **1,280** | Convert to `READY_VERIFIED` |

## 2.3 Current foundation already completed

The following foundation is already present and must be preserved:

- [x] Supporting lessons can have exact Practice without fake official OutcomeCodes.
- [x] `LessonPracticeContract` exists as the runtime exact-Practice authority.
- [x] Exact generation is fail-closed.
- [x] Exact lesson Practice forbids broad contextual fallback after an exact contract exists.
- [x] Solver verification and alignment validation are part of the exact path.
- [x] Shared exact mathematics capability routing exists across major generation surfaces.
- [x] Initial high-school Geometry/Trigonometry capability families exist.
- [x] 69 Supporting lessons are genuinely learner-facing `READY_VERIFIED`.
- [x] The 1,349 Supporting lessons have machine-readable terminal-state classification.
- [ ] The 1,280 blocked Supporting lessons are remediated.
- [ ] Full-catalogue Practice readiness is complete.
- [ ] Visual mathematics rendering is complete.
- [ ] Question-family depth is sufficient across all grade bands.

---

# 3. Target architecture

```text
Curriculum Lesson
      ↓
Practice Eligibility Classification
      ↓
Trusted source + lesson evidence
      ↓
Exact Mathematical Target
      ↓
Canonical Skill / SkillContract
      ↓
Allowed Question Families
      ↓
Difficulty / Grade Constraints
      ↓
Representation Requirements
      ├─ Symbolic
      ├─ Table
      ├─ Graph
      ├─ Geometry Diagram
      ├─ Vector Diagram
      ├─ Statistical Visual
      └─ Other approved representation
      ↓
Structured Problem / Math IR
      ↓
Solver
      ↓
Independent Verifier
      ↓
Lesson Alignment Validator
      ↓
Renderer
      ↓
Student Practice
```

The browser renders the question. The server remains authoritative for mathematics.

---

# 4. Global lesson readiness model

Every lesson must have an auditable readiness record containing at least:

```text
LessonCode
Curriculum
Level
Course / Pathway
Unit
Title
SourceType
SourceAuthority
SourceLocator
ContentVersion

PracticeEligibility
ExactTarget
PrimarySkillIds
SecondarySkillIds
PrerequisiteSkillIds

ContentStatus
MappingStatus
QuestionFamilyStatus
SolverStatus
VerifierStatus
RepresentationStatus
DifficultyStatus
AlignmentStatus

AllowedQuestionFamilies
ForbiddenQuestionFamilies
RequiredRepresentations
Readiness
BlockerCodes
Evidence
```

## Required states

```text
DISCOVERED
AUDITED
CONTENT_READY
MAPPED
GENERATION_READY
VISUAL_READY
READY_VERIFIED
```

A blocker may exist between any two states, but `BLOCKED` is not a success state.

---

# 5. Workstream P1 — Full 4,453-lesson Practice Matrix

**Status:** [ ] NOT COMPLETE

## Objective

Create one machine-readable row for every mathematics lesson and expose the actual reason it is or is not Practice-ready.

## Required output

```text
artifacts/math-intelligence/full-practice-completion-matrix.json
```

For every lesson, report:

- Practice eligibility;
- exact target;
- content status;
- SkillIds;
- mapping evidence;
- question-family coverage;
- solver/verifier coverage;
- representation requirements;
- grade/difficulty coverage;
- readiness;
- blocker codes;
- remediation cluster.

## Exit gate

- [ ] 4,453 / 4,453 lessons are present.
- [ ] No unknown/null readiness reason.
- [ ] Every blocked lesson has at least one actionable blocker code.
- [ ] Every blocker belongs to a remediation cluster.
- [ ] Counts reconcile exactly with source catalogues and CI.

---

# 6. Workstream P2 — Repair `CONTENT_WEAK` lessons

**Status:** [ ] NOT COMPLETE  
**Immediate Supporting baseline:** 329 lessons

## Objective

Repair the lesson itself when the content is insufficient, mixed, generic, off-target, or too weak to support exact Practice.

## Repair is not generic rewriting

For each weak lesson, use:

```text
Lesson title
+ Unit context
+ Level / pathway
+ Trusted pedagogical source
+ Official target evidence when available
+ Existing explanation
+ Existing examples
+ Existing solutions
```

Then produce a target-specific content contract.

## Required lesson content checks

A Practice-eligible lesson must have sufficient evidence for:

1. explicit learning target;
2. concept explanation tied to that target;
3. mathematical rule/relationship where applicable;
4. target-specific worked example;
5. mathematically verified step-by-step solution;
6. common misconception or error where pedagogically useful;
7. appropriate representation;
8. sufficient depth for the lesson level;
9. no unrelated copied/mixed content;
10. enough information to determine permitted Practice targets.

## Fine-grained content blocker codes

Replace a generic `CONTENT_WEAK` where possible with actionable causes:

```text
MISSING_LEARNING_TARGET
OFF_TOPIC_CONTENT
MIXED_LESSON_CONTENT
NO_WORKED_EXAMPLE
WORKED_EXAMPLE_TARGET_MISMATCH
INVALID_WORKED_EXAMPLE
INSUFFICIENT_MATHEMATICAL_DETAIL
GRADE_DEPTH_TOO_LOW
MISSING_MISCONCEPTION_SUPPORT
REPRESENTATION_EVIDENCE_MISSING
SOURCE_EVIDENCE_INSUFFICIENT
```

## Repair policy

- Preserve provenance.
- Do not invent official curriculum wording.
- Do not mass-copy one generic template across unrelated lessons.
- Reuse mathematical structures only where the skills are genuinely shared.
- Verify every numeric/symbolic worked example through the mathematics engine.
- Rerun semantic-content audit after repair.

## Exit gate

- [ ] No Practice-ready lesson remains `CONTENT_WEAK`.
- [ ] All repaired worked examples independently verify.
- [ ] Cross-lesson contamination checks pass.
- [ ] Grade-depth validation passes.

---

# 7. Workstream P3 — Resolve academic mapping and exact targets

**Status:** [ ] NOT COMPLETE  
**Immediate Supporting baseline:** 453 `ACADEMIC_REVIEW_REQUIRED`

## Objective

Turn uncertain lesson-to-skill mapping into explicit, evidence-backed SkillContracts.

## Evidence packet

For every unresolved lesson:

```text
title
unit
level/pathway
source
explanation
key concepts
worked examples
solutions
summary
official mapping evidence if any
candidate skills
positive evidence
negative evidence
competing candidates
recommended primary/secondary/prerequisite roles
recommended allowed families
recommended forbidden families
```

## Decision rules

- A broad domain such as `geometry`, `algebra`, or `fractions` is never a final exact target.
- Mentioned skills are not automatically Primary skills.
- Prerequisites must not authorize unrelated Practice.
- Multiple Primary skills are allowed only when the lesson genuinely teaches multiple measurable targets.
- Supporting lessons may receive Edulytics SkillContracts without official OutcomeCodes.

## Exit gate

- [ ] All 453 current Supporting review-required lessons have approved mappings.
- [ ] No unresolved ambiguous primary-skill competition remains.
- [ ] Every approved mapping has evidence.
- [ ] No fake official outcomes are created.

---

# 8. Workstream P4 — Expand the canonical Skill Ontology

**Status:** [~] FOUNDATION EXISTS; COVERAGE INCOMPLETE

## Objective

Represent the real mathematical targets present in all supported curricula without creating one skill per lesson.

Current registry baseline: **65 skills**.

## Method

```text
lesson targets
→ semantic normalization
→ target clustering
→ reuse existing canonical SkillIds
→ create missing reusable SkillIds
→ attach prerequisites / confusable skills
→ map lessons
```

## Required domains to audit

At minimum:

- number;
- fractions/decimals/percentages;
- ratio/proportion;
- algebra;
- functions;
- sequences;
- geometry;
- coordinate geometry;
- trigonometry;
- vectors;
- matrices;
- complex numbers;
- probability;
- statistics;
- calculus;
- numerical methods;
- mechanics;
- measurement.

## Exit gate

- [ ] No Practice-eligible lesson is blocked merely because the ontology lacks a reusable target.
- [ ] Every new skill has semantics, prerequisites, families, answer types, representations and verification requirements.
- [ ] Equivalent targets across curricula reuse the same canonical skill where mathematically appropriate.

---

# 9. Workstream P5 — Question Family Coverage and Diversity

**Status:** [~] FOUNDATION EXISTS; DEPTH INCOMPLETE

Current baseline:

- 89 question families;
- 48/65 registered skills have exactly one family;
- 6/65 registered skills have zero families.

## Objective

A skill must not be considered complete merely because one template can generate one mathematically correct item.

Question-family coverage must represent the ways the skill is actually assessed at its grade/course level.

## Family dimensions

Where pedagogically applicable, support:

```text
direct
reverse
missing-value
representation interpretation
representation construction
reasoning
multi-step
contextual modelling
algebraic parameter
error analysis
proof / justification
combined-prerequisite
unfamiliar transfer
```

Not every skill requires every dimension. Coverage is defined by an approved **Skill Coverage Manifest**, not by an arbitrary fixed family count.

## Upper-grade quality rule

For Grade 10+ / IGCSE / high-school / AS/A-Level-targeted skills, changing only the numeric values does **not** count as sufficient diversity.

Difficulty must vary by:

- reasoning burden;
- number of dependent steps;
- representation switching;
- hidden relationships;
- strategy choice;
- algebraic complexity;
- modelling burden;
- proof burden where applicable.

## Exit gate

- [ ] Every Practice-eligible skill has an approved coverage manifest.
- [ ] Zero required skills with no question family.
- [ ] No high-school skill is represented only by trivial number substitution when the curriculum requires deeper reasoning.
- [ ] Fixed-seed diversity tests prove structural variation, not only numeric variation.

---

# 10. Workstream P6 — Geometry and Trigonometry completion

**Status:** [~] INITIAL CAPABILITY EXISTS; COVERAGE INCOMPLETE

Current exact families include a foundation for:

- coordinate gradient;
- parallel/supplementary angles;
- similarity lengths/scale factors;
- congruence criterion;
- rectangular-prism surface area/volume;
- rectangle area/perimeter;
- Pythagoras;
- right-triangle ratios;
- right-triangle missing side;
- special-angle missing angle;
- simple contextual trigonometry.

This is not full upper-grade coverage.

## Required audit/expansion areas

As demanded by curriculum evidence:

```text
angle reasoning
parallel-line configurations
polygons
congruence
similarity
length/area/volume scale factors
transformations
constructions
loci
bearings
coordinate geometry
distance/midpoint/line equations
circle geometry
circle theorems
mensuration
composite area/volume
3D geometry
Pythagoras
right-triangle trigonometry
sine rule
cosine rule
triangle area with trig
elevation/depression
3D trigonometry
trig graphs
trig equations
identities
proof/reasoning
contextual modelling
```

Only implement a target when curriculum evidence requires it.

## Exit gate

- [ ] Grade/course-appropriate Geometry/Trig skills have sufficient families.
- [ ] All required diagrams are generated from question data.
- [ ] Solver/verifier coverage exists for every enabled family.
- [ ] Multi-step and reasoning coverage exists where the curriculum requires it.

---

# 11. Workstream P7 — Vectors completion

**Status:** [~] VERY LIMITED FOUNDATION

Current exact vector capability is primarily:

```text
vectors.add.exact_rational
vectors.dot.exact_rational
```

## Required curriculum-driven expansion

Where required by supported curricula:

```text
vector subtraction
scalar multiplication
magnitude
unit vectors
position vectors
vector from point A to point B
unknown component
parallel vectors
perpendicular vectors
ratio/section on a line
midpoint via vectors
vector paths
geometric vector reasoning
vector proof
2D coordinate vectors
3D vectors
angle between vectors
contextual displacement/resultant problems
```

## Visual requirement

Vector questions that depend on geometry must support:

- points;
- directed arrows;
- labels;
- coordinate axes/grids;
- path diagrams;
- parallelogram/triangle configurations;
- 2D and approved schematic 3D representations.

## Exit gate

- [ ] Vector curriculum targets are mapped to exact reusable skills.
- [ ] Families cover direct, geometric and reasoning forms required by curricula.
- [ ] Required vector diagrams render correctly.
- [ ] Solver and independent verifier support all enabled families.

---

# 12. Workstream P8 — Other upper-grade domain completion

**Status:** [ ] NOT COMPLETE

Review and expand, according to curriculum evidence:

## Algebra / Functions

Examples of potential required coverage:

```text
expression manipulation
factorisation
formula rearrangement
algebraic fractions
quadratic forms
systems
inequalities
function evaluation
domain/range
inverse/composite functions
graph interpretation
graph transformations
piecewise behavior
polynomial/rational/exponential/logarithmic relationships
```

## Calculus

Current foundation is narrow. Audit for:

```text
differentiation rules
gradient/tangent/normal
stationary points
optimization
integration
area
kinematics links
course-specific advanced techniques
```

## Probability / Statistics

Audit for:

```text
combined events
conditional probability
tree diagrams
sample spaces
distributions
expected value
frequency representations
histograms
scatter/correlation
variance/standard deviation
statistical interpretation
```

## Matrices / Complex / Mechanics / Numerical Methods

Expand only to the exact depth required by the supported programmes, with multiple family forms where assessment expectations require them.

## Exit gate

- [ ] Every Practice-eligible upper-grade target has an exact capability decision.
- [ ] No broad contextual family substitutes for a missing exact skill.
- [ ] Unsupported features remain fail-closed until implemented.

---

# 13. Workstream P9 — Math Diagram and Visual Representation Engine

**Status:** [ ] NOT COMPLETE

This is a core mathematics capability, not decorative UI.

## 13.1 Architecture

```text
Question Family
      ↓
Structured mathematical parameters
      ↓
DiagramSpec / VisualSpec
      ↓
Deterministic validator
      ↓
SVG / graph renderer
      ↓
Student UI / Assessment / Print
```

Do not use generative AI images as the mathematical source of truth.

## 13.2 Required representation modes

Each family declares:

```text
NONE
OPTIONAL
REQUIRED
```

If a family is `REQUIRED` and the visual cannot be produced and validated, the item is rejected.

## 13.3 Initial diagram types

Geometry:

- triangle;
- right triangle;
- isosceles/equilateral triangle;
- rectangle;
- parallelogram;
- trapezium;
- polygon;
- circle/chord/tangent/radius/sector;
- parallel lines and transversal;
- similarity/congruence diagrams;
- angle configurations;
- coordinate geometry;
- basic solids and 3D schematics.

Vectors/graphs:

- coordinate plane;
- vector arrows;
- point-to-point vectors;
- vector paths;
- line graphs;
- function graphs;
- transformations.

Probability/statistics:

- probability tree;
- frequency table rendering;
- scatter plot;
- bar chart/histogram where required;
- other curriculum-required statistical visuals.

## 13.4 DiagramSpec minimum contract

```text
visualType
coordinateSystem
points
segments
arrows
curves
shapes
angleMarks
parallelMarks
rightAngleMarks
labels
values
units
visibilityRules
accessibilityDescription
notToScale flag where appropriate
```

## 13.5 Validation

Validate at minimum:

- all referenced labels exist;
- required values are displayed;
- arrows have correct direction;
- geometric relationship marks match the mathematical model;
- critical labels do not overlap;
- no misleading scale assumption is introduced;
- visual and question parameters agree;
- mobile rendering remains readable;
- accessible text alternative exists.

## 13.6 UI integration

Practice and assessment presentation must support:

```text
prompt
+ mathematical notation
+ visual/diagram
+ answer control
+ feedback/solution visual when useful
```

## Exit gate

- [ ] Required family visuals render deterministically.
- [ ] Visual data comes from the same parameters used by solver/verifier.
- [ ] No `READY_VERIFIED` required-visual family can render as text-only.
- [ ] Visual validation is part of CI.

---

# 14. Workstream P10 — Solver, Verifier and Answer Equivalence completion

**Status:** [~] FOUNDATION EXISTS; COVERAGE INCOMPLETE

Every enabled Question Family requires:

```text
structured problem
→ solver
→ expected answer
→ independent verifier
→ answer-equivalence policy
```

## Rules

- Generator output is not trusted merely because it produced an answer.
- Verifier logic must be independently derived where practical.
- Equivalent mathematical forms must be accepted when the family allows them.
- Domain restrictions and assumptions must be checked.
- Generated distractors must fail verification.

## Exit gate

- [ ] 100% of enabled exact families have solver support.
- [ ] 100% have independent verification.
- [ ] 100% declare answer type/equivalence policy.
- [ ] No learner-facing exact item is persisted before verification.

---

# 15. Workstream P11 — Grade-aware difficulty and depth

**Status:** [~] FOUNDATION EXISTS; CALIBRATION INCOMPLETE

Difficulty cannot be represented only by larger numbers.

## Internal dimensions

```text
step count
AST depth
unknown count
branching
strategy choice
representation switching
algebraic complexity
proof burden
modelling burden
prerequisite load
cognitive demand
```

## Example

```text
Foundation:
  direct method
  explicit data
  one core concept

Standard:
  method selection
  reverse forms
  two or more linked steps

Challenge:
  hidden relationship
  multiple concepts
  algebraic reasoning
  visual interpretation
  unfamiliar transfer
  proof/modelling where appropriate
```

## Exit gate

- [ ] Grade bounds are declared for relevant families.
- [ ] Challenge questions are structurally harder, not merely numerically larger.
- [ ] Fixed-seed calibration corpus exists for key grade bands.
- [ ] Academic review confirms level appropriateness for promoted families.

---

# 16. Workstream P12 — Remediate current Supporting blockers to zero

**Status:** [ ] NOT COMPLETE

This workstream consumes P2–P11 and is the hard Supporting completion gate.

## Current queue

```text
329 CONTENT_WEAK
453 ACADEMIC_REVIEW_REQUIRED
497 SOLVER_CAPABILITY_MISSING
1   REPRESENTATION_MISSING
----------------------------
1280 EXPLICITLY_BLOCKED
```

## Processing model

```text
Blocked lesson
   ↓
identify blocker(s)
   ↓
assign remediation cluster
   ↓
repair shared capability/content/mapping
   ↓
rerun audit
   ↓
generate fixed-seed samples
   ↓
solver verification
   ↓
alignment verification
   ↓
representation verification
   ↓
READY_VERIFIED
```

## Clustering rule

Do not create 1,280 custom implementations.

Use target clusters so that one reusable skill/family/capability can unlock many lessons across Cambridge, UAE, US and other curricula.

## Exit gate — hard requirement

```text
supportingLessonCount == 1349
supportingReadyVerifiedCount == 1349
supportingBlockedCount == 0
supportingUnresolvedCount == 0
supportingBroadFallbackCount == 0
```

Until all five conditions are true, Supporting Practice completion is **not complete**.

---

# 17. Workstream P13 — Full catalogue Practice completion

**Status:** [ ] NOT COMPLETE

After/alongside Supporting remediation, apply the same architecture to the entire 4,453-lesson catalogue.

## Required classification

Every lesson:

```text
PRACTICE_ELIGIBLE
or
NON_STANDALONE_WITH_EVIDENCE
```

For `PRACTICE_ELIGIBLE`:

```text
ContentReady
SkillMapped
FamilyReady
SolverReady
VerifierReady
RepresentationReady
DifficultyReady
AlignmentReady
= READY_VERIFIED
```

## Exit gate

- [ ] 4,453/4,453 lessons have an eligibility classification.
- [ ] 100% of Practice-eligible lessons are `READY_VERIFIED`.
- [ ] 0 Practice-eligible lessons remain blocked.
- [ ] 0 silent broad fallback.
- [ ] Every non-standalone classification has evidence and review traceability.

---

# 18. Workstream P14 — CI, dashboards and anti-regression gates

**Status:** [ ] NOT COMPLETE

## Required CI failures

CI must fail when:

```text
a READY lesson has no exact Skill
a READY lesson uses an unauthorized family
a required family has no solver
a required family has no verifier
a required visual has no DiagramSpec/renderer
a diagram disagrees with question parameters
a Practice lesson is CONTENT_WEAK
a lesson silently falls back to a broad topic
a high-school family violates its approved difficulty/grade bounds
a generated answer fails independent verification
a lesson marked READY has unresolved alignment conflict
Supporting blocked count increases without an approved baseline update
```

## Dashboard metrics

At minimum publish:

```text
total lessons
Practice-eligible lessons
non-standalone lessons
READY_VERIFIED count
blocked count
blocked by reason
content-ready %
exact-mapped %
question-family-ready %
solver-ready %
verifier-ready %
visual-ready %
grade/difficulty-ready %
broad-fallback count
readiness by curriculum
readiness by level
readiness by domain
readiness by skill cluster
```

## Exit gate

- [ ] CI enforces readiness invariants.
- [ ] Dashboard is generated from source-of-truth audits.
- [ ] Counts reconcile exactly.
- [ ] Regressions are visible on every relevant PR.

---

# 19. Cross-surface product consistency

The same exact capability authority must be consumed by:

```text
Student Lesson Practice
Student Private Practice
Teacher Assessment Builder
Diagnostic / Adaptive
Reassessment
Game runtime where applicable
```

The surfaces may render differently, but they must not disagree about:

- SkillId;
- allowed families;
- solver;
- verifier;
- representation requirement;
- readiness.

Once an exact contract exists, no surface may silently downgrade to a broad contextual generator.

---

# 20. Execution order

The work is executed as one programme, but dependencies should be respected.

```text
P1 Full Practice Matrix
      ↓
P2 Content Repair ───────────────┐
P3 Academic Mapping ─────────────┤
P4 Skill Ontology ───────────────┤
P5 Question Families ────────────┤
P6 Geometry/Trig ────────────────┤
P7 Vectors ──────────────────────┤
P8 Other Upper-grade Domains ────┤
P9 Visual Engine ────────────────┤
P10 Solver/Verifier ──────────────┤
P11 Difficulty ──────────────────┘
                ↓
P12 Supporting blockers → zero
                ↓
P13 Full catalogue completion
                ↓
P14 Permanent CI/dashboard gates
```

Implementation should prioritize reusable capabilities that unlock the largest valid lesson clusters, while never bypassing academic evidence.

---

# 21. Prioritization algorithm

For each remediation cluster, calculate:

```text
affectedLessonCount
× academicConfidence
× curriculumReuse
× learnerImpact
÷ implementationRisk
```

Use the score only to order work, not to authorize mappings.

Recommended early shared-capability targets are those that:

- unblock many lessons;
- have strong exact mathematical semantics;
- can be deterministically solved and verified;
- recur across multiple curricula;
- remove a current high-volume blocker.

---

# 22. Required audit artifacts

The programme should generate and retain:

```text
full-practice-completion-matrix.json
supporting-practice-completion.json
content-repair-audit.json
lesson-skill-mapping-audit.json
question-family-coverage-audit.json
visual-representation-coverage-audit.json
solver-verifier-coverage-audit.json
difficulty-grade-coverage-audit.json
practice-readiness-by-curriculum.json
practice-readiness-by-domain.json
practice-readiness-by-level.json
```

Each artifact must include the commit SHA that produced it.

---

# 23. Progress tracker

Update this table in every material remediation PR.

| Workstream | Status | Baseline | Target | Last verified |
|---|---|---|---|---|
| P1 Full Practice Matrix | ✅ Complete | 4,453-row completion matrix generated; internal blockers 0 | 4,453 complete rows | 2026-09-19 PR #213 audit run #217 |
| P2 Content Repair | ⬜ Not complete | 329 Supporting `CONTENT_WEAK`; broader catalogue weaknesses exist | 0 Practice-eligible `CONTENT_WEAK` | 2026-09-19 |
| P3 Academic Mapping | ⬜ Not complete | 453 Supporting academic-review blockers | 0 unresolved Practice-eligible mappings | 2026-09-19 |
| P4 Skill Ontology | 🟨 Partial | 69 skills; current registered skills have family metadata | Full reusable target coverage | 2026-09-19 PR #213 |
| P5 Question Families | 🟨 Partial | 100 families; zero registered skills without family metadata | Approved coverage manifests complete | 2026-09-19 PR #213 |
| P6 Geometry/Trig | 🟨 Partial | Initial exact families implemented | Curriculum-complete exact/visual coverage | 2026-09-19 |
| P7 Vectors | 🟨 Partial | Add, subtract, scalar multiply, dot, magnitude, between-points + deterministic vector SVG | Curriculum-complete vector coverage | 2026-09-19 PR #213 |
| P8 Other upper-grade domains | ⬜ Not complete | Narrow exact coverage | Curriculum-driven complete coverage | 2026-09-19 |
| P9 Diagram/Visual Engine | 🟨 Partial | Deterministic SVG runtime wired to Practice for current geometry/trig/vector/fraction families | Required visuals deterministic + validated | 2026-09-19 PR #213 |
| P10 Solver/Verifier/Equivalence | 🟨 Partial | Shared kernel exists | 100% enabled-family coverage | 2026-09-19 |
| P11 Grade-aware Difficulty | 🟨 Partial | Difficulty bands exist | Structural grade-aware calibration | 2026-09-19 |
| P12 Supporting blockers → zero | 🟨 Partial | 81 ready / 1,268 blocked | 1,349 ready / 0 blocked | 2026-09-19 PR #213 audit run #217 |
| P13 Full catalogue completion | 🟨 Partial | 90 ready / 4,363 not ready | 100% Practice-eligible ready | 2026-09-19 PR #213 audit run #217 |
| P14 CI + Dashboard | 🟨 Partial | Full completion matrix + Supporting artifact generated in Mathematics Intelligence CI | Permanent completion gates | 2026-09-19 PR #213 |

Legend:

```text
✅ Complete
🟨 Partial / foundation exists
⬜ Not complete
🛑 Blocked by external evidence/review
```

---

# 24. Definition of Done — individual lesson

A Practice-eligible lesson is done only when all are true:

- [ ] trusted source/provenance is recorded;
- [ ] exact learning target is known;
- [ ] content audit passes;
- [ ] Primary SkillId mapping is approved;
- [ ] Secondary/prerequisite roles are correct;
- [ ] allowed question families are explicit;
- [ ] forbidden/confusable families are explicit where needed;
- [ ] required solver capabilities exist;
- [ ] independent verifier exists;
- [ ] answer equivalence is defined;
- [ ] grade/difficulty bounds are valid;
- [ ] required representation/diagram exists and validates;
- [ ] fixed-seed generated samples pass;
- [ ] lesson-alignment validator passes;
- [ ] broad fallback is disabled for the exact contract;
- [ ] runtime Student Practice resolves the contract;
- [ ] final readiness is `READY_VERIFIED`.

---

# 25. Definition of Done — Supporting programme

The Supporting programme is complete only when:

- [ ] Supporting lesson count reconciles to 1,349 or an intentionally versioned new catalogue count;
- [ ] 100% Supporting lessons have approved exact targets;
- [ ] 100% have sufficient content;
- [ ] 100% have required families;
- [ ] 100% have solver/verifier support;
- [ ] 100% have required representation support;
- [ ] 100% pass alignment validation;
- [ ] 100% are learner-facing `READY_VERIFIED`;
- [ ] 0 remain `EXPLICITLY_BLOCKED`;
- [ ] 0 remain unresolved;
- [ ] 0 use broad fallback;
- [ ] CI enforces these conditions.

---

# 26. Definition of Done — full mathematics Practice programme

The full programme is complete only when:

- [ ] all 4,453 current lessons have Practice-eligibility classification;
- [ ] all Practice-eligible lessons are `READY_VERIFIED`;
- [ ] all non-standalone lessons have explicit evidence;
- [ ] no Practice-eligible lesson remains blocked;
- [ ] no exact lesson uses broad fallback;
- [ ] question diversity meets approved Skill Coverage Manifests;
- [ ] required diagrams/graphs/vectors/statistical visuals render correctly;
- [ ] every enabled family solves and independently verifies;
- [ ] equivalent answers grade correctly;
- [ ] difficulty is grade-appropriate;
- [ ] Student Practice and Teacher/Student AI generation use the same capability authority;
- [ ] CI and dashboards prevent silent regression.

---

# 27. What must never happen

Do not:

```text
treat EXPLICITLY_BLOCKED as programme completion
invent official curriculum outcomes
create one custom generator per lesson
rewrite hundreds of lessons with one generic template
map from title keywords alone
enable a family because it is only broadly related
use AI-generated images as mathematical diagrams of record
show text-only geometry when the question requires a diagram
accept generator output without independent verification
raise difficulty only by making numbers larger
allow exact-contract questions to fall back to generic contextual generation
hide unsupported capability behind a nearby-topic question
declare a workstream complete without updated audit evidence
```

---

# 28. Update protocol

Every PR that changes this programme must:

1. update the relevant workstream status;
2. update baseline/current counts if changed;
3. name the audit artifact proving the change;
4. identify newly unblocked lessons;
5. identify any newly introduced blockers;
6. update the changelog below;
7. preserve the hard Definition of Done.

A PR may improve the system without completing a workstream. In that case mark it `🟨 Partial`, not `✅ Complete`.

---

# 29. Changelog

## 2026-09-19 — PR #213 execution checkpoint

Verified on Mathematics Intelligence Foundation run #217 at commit `7347359c380dbdb3d94cf99b334bc37cc994878b`; subsequent commit only closes the remaining three Skill-registry family-metadata gaps and must pass the same CI before merge.

```text
Full catalogue:
  4,453 lessons
  90 READY_VERIFIED
  4,363 blocked/not READY_VERIFIED
  completion 2.02%

Supporting:
  1,349 lessons
  81 READY_VERIFIED
  1,268 EXPLICITLY_BLOCKED

Supporting blockers:
  328 CONTENT_WEAK
  443 ACADEMIC_REVIEW_REQUIRED
  497 SOLVER_CAPABILITY_MISSING
  0 REPRESENTATION_MISSING

Registry:
  69 skills
  100 question families
  104 capabilities
  90 approved mappings
  90 valid projected exact Practice contracts
  0 internal completion-matrix blockers
```

Implemented in this checkpoint:

- deterministic parameter-driven SVG Practice visuals without a database migration;
- exact visual rendering for current geometry/trigonometry/vector/fraction families;
- vector subtraction, scalar multiplication, magnitude and point-to-point vector families;
- whole-number add/subtract, LCM, percentage-of-quantity and fraction-representation capabilities;
- fail-closed projection from approved mapping manifests into learner-facing LessonPracticeContracts;
- explicit family-level `lessonPracticeRouting` authorization;
- full 4,453-row Practice completion matrix and Supporting completion artifact in CI.

The programme is **not complete**: the remaining Supporting blockers continue to be treated as temporary protection, never as successful completion.

---

## 2026-09-19 — Plan created

Baseline locked to:

```text
c4a4592d1873af56da790235224195c48e3f92cc
```

Recorded starting state:

```text
Full catalogue:
  4,453 lessons
  78 READY_VERIFIED
  4,375 not READY_VERIFIED

Supporting:
  1,349 lessons
  69 READY_VERIFIED
  1,280 EXPLICITLY_BLOCKED

Supporting blocker breakdown:
  329 CONTENT_WEAK
  453 ACADEMIC_REVIEW_REQUIRED
  497 SOLVER_CAPABILITY_MISSING
  1   REPRESENTATION_MISSING

Registry baseline:
  65 skills
  89 question families
  86 capabilities
  48 skills with exactly one family
  6 skills with zero families
```

The programme completion rule was strengthened:

```text
Blocked is temporary protection.
READY_VERIFIED is the successful terminal Practice state.
```
