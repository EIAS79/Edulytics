# Edulytics Integrated Mathematics Intelligence Master Execution Plan
## Track A: Curriculum/Lesson Intelligence + Track B: Advanced Mathematics Solver

**Repository:** `EIAS79/Edulytics`  
**Master purpose:** Execute the 1500-lesson curriculum alignment programme and the advanced mathematics solver programme in the correct dependency order without breaking current production behavior.

---

# 1. The two-track model

Edulytics needs two coordinated systems.

## Track A — Curriculum & Lesson Intelligence

Answers:

```text
What exact mathematical skill is this lesson teaching?
Is the content sufficient?
Which question families are allowed?
Which are forbidden?
What capabilities are required?
Is this lesson ready for generation?
```

## Track B — Mathematics Intelligence Kernel

Answers:

```text
Can Edulytics represent this mathematics?
Can it solve it?
Can it verify the answer?
Can it grade equivalent answers?
Can it generate diverse valid questions?
Can it explain the solution?
```

The tracks meet at the SkillContract.

---

# 2. System boundary

```text
TRACK A
Curriculum/Lesson
     ↓
SkillContract
     ↓
Required capabilities
     │
     ▼
TRACK B
Math IR
Solver
Verifier
Answer evaluator
Question generator
```

Track A decides **what** mathematics belongs to the lesson.

Track B decides **how** that mathematics is solved, verified and generated.

---

# 3. Why both tracks are required

Track A without Track B:

```text
Correct skill mapping
but
weak solver/generator
```

Result:

```text
lesson understood correctly
but question capability limited
```

Track B without Track A:

```text
powerful solver
but
wrong lesson mapping
```

Result:

```text
mathematically correct question
but wrong topic/skill
```

Therefore production quality requires both.

---

# 4. What remains unchanged initially

The programme must preserve:

```text
Curriculum hierarchy
Academic years
School setup
Roles
Student accounts
Teacher accounts
Existing official outcomes
Existing lesson identities
Existing assessment approval workflow
Existing mastery history
Historical generated items
Current production routes
```

No big-bang rewrite.

---

# 5. Master workstreams

```text
W1 Catalogue inventory
W2 Content audit
W3 Skill ontology
W4 Lesson → Skill mapping
W5 Question-family registry
W6 Math IR
W7 Solver
W8 Verifier
W9 Answer equivalence
W10 Difficulty intelligence
W11 Practice migration
W12 Assessment migration
W13 Game migration
W14 IGCSE gate
W15 A-Level gate
W16 IB advanced gate
W17 Security/performance
W18 Production rollout
```

---

# 6. Dependency map

```text
W1 Catalogue inventory
     ↓
W2 Content audit
     ↓
W3 Skill ontology
     ↓
W4 Lesson mapping
     ↓
W5 Question families
     ↓
     ├─────────────┐
     │             │
     ▼             ▼
W6 Math IR      W10 Difficulty model
     ↓
W7 Solver
     ↓
W8 Verifier
     ↓
W9 Answer equivalence
     ↓
W11 Practice migration
     ↓
W12 Assessment migration
     ↓
W13 Game migration
     ↓
W14 IGCSE
     ↓
W15 A-Level
     ↓
W16 IB advanced
```

---

# 7. Programme Stage 0 — Baseline lock

## Objective

Do not change behavior yet.

## Actions

- capture current main SHA;
- inventory current generator families;
- inventory current canonical skills;
- inventory contextual topic fallbacks;
- inventory lesson catalogue;
- capture representative fixed-seed outputs;
- document current Practice/Assessment routing.

## Output

```text
CurrentSystemBaseline
```

## Exit gate

We can reproduce the current behavior in CI.

---

# 8. Programme Stage 1 — 1500-lesson inventory

## Objective

Replace approximate catalogue knowledge with exact numbers.

For every lesson store:

```text
LessonId
LessonCode
Curriculum
Level
Unit
Topic
Title
OfficialOutcomeCount
IsSupporting
ContentVersion
HasExplanation
HasExamples
HasMisconceptions
HasSummary
CurrentGenerationPath
```

## Exit gate

100% of lessons inventoried.

---

# 9. Programme Stage 2 — Content quality audit

## Objective

Find lessons that are unsafe to use as generation sources.

## Audit dimensions

```text
Target clarity
Explanation quality
Worked-example validity
Solution validity
Misconception relevance
Representation quality
Internal consistency
Generation sufficiency
```

## Output statuses

```text
PASS
PASS_WITH_WARNINGS
FAIL
AMBIGUOUS
```

## Exit gate

Every lesson has a content status.

---

# 10. Programme Stage 3 — Canonical Skill Ontology v2

## Objective

Build the shared mathematical vocabulary.

Do not create 1500 skills by default.

Deduplicate equivalent lesson targets.

Example:

```text
Cambridge:
Compare unlike fractions

Polish:
Porównywanie ułamków o różnych mianownikach

Edulytics:
Compare fractions with different denominators
```

All map to:

```text
fractions.compare.unlike_denominators
```

## Exit gate

Every lesson can point to either:

```text
one or more exact SkillIds
```

or:

```text
explicit unresolved status
```

---

# 11. Programme Stage 4 — Lesson Skill Mapping

## Objective

Assign exact SkillContracts to the catalogue.

## Mapping evidence

Use:

```text
title
content
examples
official outcomes
source focus
existing mechanics
```

## Rule

Broad domains like:

```text
fractions
algebra
geometry
```

cannot be final exact lesson skills.

## Exit gate

No silent broad mapping.

---

# 12. Programme Stage 5 — Readiness matrix

For every lesson compute:

```text
ContentReady
SkillMapped
QuestionFamilyReady
SolverReady
VerifierReady
AnswerEvaluatorReady
RepresentationReady
GenerationReady
```

## Output

```text
READY_VERIFIED
READY_CONTEXTUAL
BLOCKED
REVIEW_REQUIRED
```

---

# 13. Programme Stage 6 — Math IR foundation

This begins the core Track B implementation.

Implement:

```text
MathNode
ExactInteger
Rational
Symbol
Add
Multiply
Divide
Power
Root
Equation
Inequality
Set
Interval
Vector
Matrix
Derivative
Integral
```

No product cutover.

---

# 14. Programme Stage 7 — Domain and assumption system

Implement:

```text
Real
Complex
Integer
Positive
NonZero
Intervals
Variable constraints
Solution sets
```

Required before advanced solving.

---

# 15. Programme Stage 8 — Parser/renderers

Implement:

```text
controlled parser
canonical renderer
Unicode renderer
LaTeX renderer
```

With security limits.

---

# 16. Programme Stage 9 — Algebra engine

Implement:

```text
normalize
collect like terms
expand
factor
polynomial representation
polynomial evaluation
division
complete square
```

This is the main bridge into IGCSE.

---

# 17. Programme Stage 10 — Equation/system solver

Implement:

```text
linear
multi-step linear
fractional linear
2x2 simultaneous
quadratic
linear-quadratic
rational
radical
```

With typed solution sets.

---

# 18. Programme Stage 11 — Independent verifier

Required before production solver promotion.

Examples:

```text
equation → substitute
factorization → re-expand
integral → differentiate
inverse matrix → multiply to identity
inequality → boundary/sign test
```

---

# 19. Programme Stage 12 — Answer Equivalence v2

Unify:

```text
Practice answer checking
Assessment answer checking
Math answer equivalence
```

Support:

```text
rational
decimal
percentage
surds
expressions
sets
intervals
vectors
matrices
complex values
```

---

# 20. Programme Stage 13 — Solver-grounded generation

Replace:

```text
template first
```

with:

```text
problem structure
→ solve
→ verify
→ render
```

Question family manifests become authoritative.

---

# 21. Programme Stage 14 — Misconception engine

Create reviewed misconception transformations.

Examples:

```text
wrong sign
wrong denominator rule
forget ±
wrong quadrant
missing chain factor
```

Every distractor must fail verification.

---

# 22. Programme Stage 15 — Advanced difficulty engine

Keep UI:

```text
Easy / Medium / Challenging
```

but calculate internal complexity using:

```text
step count
AST depth
unknown count
branching
domain restrictions
strategy count
representation switching
proof burden
modelling burden
```

---

# 23. Programme Stage 16 — Migrate current Primary exact skills

First migrations:

```text
TWO_UNKNOWNS
SCALE_READING
FRACTION_COMPARE_UNLIKE
```

Use them as proof that:

```text
old exact mechanic
==
new SkillContract + engine output
```

---

# 24. Programme Stage 17 — Grade 1–6 migration

Move domain by domain:

```text
whole numbers
place value
operations
fractions
decimals
percentages
ratio
measurement
geometry
statistics
probability
early algebra
```

Do not migrate all lessons at once.

---

# 25. Programme Stage 18 — Practice migration

Practice selection becomes:

```text
Lesson
→ SkillContract
→ allowed families
→ generated problem
→ solver
→ verifier
```

No broad fallback for `READY_VERIFIED`.

---

# 26. Programme Stage 19 — Assessment Builder migration

Teacher workflow remains:

```text
Select
Generate
Review
Approve
Publish
```

Only the generation intelligence changes.

---

# 27. Programme Stage 20 — Diagnostic/adaptive migration

Adaptive system consumes:

```text
Skill mastery
Prerequisite mastery
Mathematical complexity
Misconception history
Representation fluency
```

not only `Easy/Medium/Challenging`.

---

# 28. Programme Stage 21 — Reassessment migration

Freshness becomes mathematical.

Vary:

```text
coefficients
representation
strategy
context
misconception trap
cognitive demand
```

not only wording.

---

# 29. Programme Stage 22 — Game runtime migration

Goal:

```text
server owns mathematics
browser owns interaction/rendering
```

Game JS should not be the final authority for answer correctness.

---

# 30. Programme Stage 23 — IGCSE Extended gate

Before claiming IGCSE-level capability:

```text
map syllabus skills
build Edulytics benchmark corpus
solve
verify
grade
generate
difficulty calibrate
academic review
```

Status per skill:

```text
VERIFIED
CONTEXTUAL
UNSUPPORTED
```

---

# 31. Programme Stage 24 — AS/A-Level 9709 gate

Expand into:

```text
Pure Mathematics
Mechanics
Probability & Statistics
```

Coverage claimed per domain/paper, not one global boolean.

---

# 32. Programme Stage 25 — IB AA HL-style gate

Add benchmark categories:

```text
Routine
Multi-step
Modelling
Reasoning
Proof
Unfamiliar transfer
```

Advanced intelligence requires more than topic coverage.

---

# 33. Programme Stage 26 — Security and resource controls

Before accepting arbitrary mathematical expressions:

```text
input length limit
token limit
AST depth limit
node limit
polynomial degree limit
matrix dimension limit
solver timeout
memory budget
concurrency budget
```

---

# 34. Programme Stage 27 — Observability

Metrics:

```text
generation success
solver success
verification failure
alignment rejection
fallback rate
unsupported rate
timeout
answer-equivalence disagreement
content mapping conflict
```

---

# 35. Programme Stage 28 — Production rollout

Feature flags:

```text
LegacyOnly
ShadowV2
V2VerifiedOnly
V2Preferred
V2Only
```

Roll out per:

```text
domain
skill
curriculum
grade
```

not globally.

---

# 36. Recommended PR sequence

## PR 1

Catalogue inventory + baseline reports.

No behavior change.

## PR 2

Skill registry primitives.

No behavior change.

## PR 3

Lesson audit contracts.

No behavior change.

## PR 4

Lesson evidence extraction.

No behavior change.

## PR 5

Candidate skill resolver + reports.

No behavior change.

## PR 6

Deterministic mapping validator.

No behavior change.

## PR 7

Readiness matrix.

No behavior change.

## PR 8

Math AST foundation.

No behavior change.

## PR 9

Exact rational + domains + solution sets.

No behavior change.

## PR 10

Solver/verifier interfaces.

No behavior change.

## PR 11

Legacy family AST adapters + shadow verification.

Still no user-facing cutover.

## PR 12

General linear solver.

Shadow only.

## PR 13

Polynomial normalization.

## PR 14

Quadratic vertical slice.

## PR 15

Answer equivalence v2.

## PR 16

Unify Practice grading.

## PR 17

Question family registry v2.

## PR 18

SkillContract-backed Practice shadow mode.

## PR 19

First production cutover for selected verified skills.

---

# 37. What must never happen

Do not:

```text
rewrite all 1500 lessons blindly
create 1500 generators
invent official OutcomeCodes
map lessons only from keywords
claim solver support because topic text contains a word
silently fall back to unrelated generic questions
replace current production engine in one release
use LLM output as sole mathematics verifier
lose reconstructability of historical items
```

---

# 38. Immediate implementation priority

The first real milestone should be:

```text
MILESTONE 1
Catalogue Intelligence Foundation
```

Deliver:

```text
exact lesson inventory
source classification
content audit contracts
SkillId registry structure
LessonSkillProfile schema
readiness status schema
baseline generation report
```

Then:

```text
MILESTONE 2
Math Intelligence Foundation
```

Deliver:

```text
Math AST
Rational
SolutionSet
Solver interfaces
Verifier interfaces
Answer evaluator interfaces
legacy compatibility
```

Then:

```text
MILESTONE 3
First end-to-end exact vertical slice
```

Use:

```text
quadratic equations
```

or one currently problematic Primary skill for migration validation.

---

# 39. Final programme outcome

When both tracks are complete:

```text
1500 current lessons
+
future curricula
        ↓
exact SkillContracts
        ↓
audited content
        ↓
reusable question families
        ↓
shared Math Intelligence Kernel
        ↓
solve
verify
grade
explain
adapt
        ↓
Practice / Assessment / Diagnostic / Reassessment
```

The website keeps the academic curriculum structure.

The new architecture changes the intelligence behind it.

---

# 40. Definition of programme success

Success is not:

```text
"we generated a question"
```

Success is:

```text
the lesson target is known
the generated question measures that exact target
the mathematics is correct
the answer is solved independently
the answer is verified
equivalent student answers are recognized
difficulty is appropriate
the source/curriculum mapping is truthful
unsupported cases are blocked instead of guessed
```

That is the standard the combined roadmap is designed to reach.
