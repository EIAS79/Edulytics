# Edulytics Practice Assessment Composition V2 — Master Execution Plan

**Status:** ACTIVE — GOVERNING PRACTICE QUALITY PROGRAMME  
**Branch:** `practice-assessment-composition-v2`  
**Baseline production commit:** `fe75a2b37ae869863a01011c2515a34e7ecf3458`  
**Created:** 2026-09-23

## 1. Why this programme exists

The existing Edulytics Mathematics Intelligence stack is strong at:

- exact lesson/curriculum alignment;
- verified Lesson Practice contracts;
- deterministic Mathematics generation;
- exact solving;
- independent answer verification;
- exposure fingerprints;
- fail-closed unsupported mathematics.

However, learner Practice currently assembles sessions primarily by cycling through
`AllowedQuestionFamilies` and generating parameter/wording variants until the requested
question count is filled. Technical uniqueness is therefore stronger than pedagogical
uniqueness.

The production failure that triggered this programme is representative:

- lesson contract allowed only `supporting.geometry.shape_dimension`;
- a 10-question Personal Practice requested 10 items;
- the generator cycled the same family;
- variants 0–7 used square, rectangle, triangle, circle, cube, cuboid, sphere and cylinder;
- variants 8–9 reused square and rectangle with different wording;
- the visual renderer exposed the correct answer by writing `2D shape` / `3D shape` under the diagram.

The system produced mathematically correct items, but the assessment session was not
pedagogically acceptable.

## 2. Governing principle

A requested question count is a maximum target, not permission to manufacture repetition.

Edulytics must prefer:

1. exact curriculum alignment;
2. mathematical correctness;
3. independent verification;
4. semantic uniqueness;
5. cognitive/question-form diversity;
6. honest difficulty;
7. no learner-visible answer leakage;
8. a shorter valid session over a padded invalid session.

## 3. Target architecture

```text
Lesson
  -> READY_VERIFIED Lesson Practice Contract
  -> Practice Assessment Composer
  -> Session Blueprint
       - question family
       - question form
       - cognitive operation
       - difficulty
       - semantic identity budget
  -> Exact Mathematics Question Engine
  -> Exact Solver
  -> Independent Verifier
  -> Session Quality Validator
       - semantic duplicates
       - family/form concentration
       - cognitive diversity
       - difficulty progression
       - visual/prompt answer leakage
       - curriculum alignment
       - exact verifier result
  -> READY learner Practice session
```

## 4. Non-goals

This programme does not replace:

- the Mathematics solver;
- the independent verifier;
- Lesson Practice contract mapping;
- official curriculum/outcome mapping;
- historical exposure tracking.

It composes and validates sessions above those layers.

---

# Phase 0 — Production Hotfix

**Status: COMPLETE**

**Closure:** Learner-visible shape dimension labels were removed; wording-only shape duplicates are rejected; Personal Practice may return a shorter exact session instead of padding; actual item count drives MaxScore. Regression, Mathematics, PostgreSQL, SAST and production-container gates passed.

## Goal

Remove the currently learner-visible failure modes immediately.

## Scope

- Remove answer-bearing `2D shape` / `3D shape` visual captions.
- Introduce semantic identity for `shape_dimension` so wording-only variants of the same
  shape are not considered unique.
- Permit a valid session to contain fewer than the requested maximum when a finite
  meaningful pool is exhausted.
- Keep exact solver/verifier gates unchanged.
- Add regression tests for the production failure.

## Acceptance criteria

- No `shape_dimension` SVG contains the correct dimension label.
- Square cannot appear twice in one `shape_dimension` session merely because wording changed.
- A request for 10 narrow questions may return 8 valid questions rather than fabricate 10.
- Attempt `MaxScore` reflects actual generated item count.
- Existing broad Practice generation remains exact and verified.

---

# Phase 1 — Semantic Question Identity

**Status: COMPLETE**

**Closure:** A deterministic semantic identity contract now exists for every runtime Practice family. Synthetic `variant` parameters do not contribute to learner-meaningful identity. Shape-dimension runtime enforcement uses the shared policy. Full Mathematics and Phase16 CI gates passed.

## Goal

Define learner-meaningful uniqueness independently from technical fingerprints.

## Deliverables

- `PracticeSemanticQuestionIdentity` contract.
- Deterministic semantic key builder.
- Per-family semantic identity policy.
- Session-level semantic duplicate rejection.
- Certification across all runtime Practice families.

## Acceptance criteria

Two items that measure the same operation on the same mathematical subject/target cannot
coexist in one session only because wording or variant IDs differ.

---

# Phase 2 — Question Forms and Cognitive Operations

**Status: COMPLETE**

**Closure:** Question-form, cognitive-operation and cognitive-difficulty taxonomy is implemented. Every routed runtime family resolves to a capability. Shape dimension now implements four genuine forms (Identify, Classify, ErrorAnalysis, Transfer), and a ten-item session demonstrates semantic/form diversity while remaining exact-solver verified. Full Mathematics and Phase16 CI gates passed.

## Goal

Separate the mathematical family from how mastery is elicited.

## Taxonomy

At minimum:

- Recognize
- Select
- Classify
- Sort
- Compare
- Calculate
- Apply
- Explain
- ErrorAnalysis
- Transfer

Not every family must support every form.

## Deliverables

- `PracticeQuestionForm`
- `PracticeCognitiveOperation`
- capability registry per family
- explicit difficulty capability per form
- initial richer shape-properties forms

---

# Phase 3 — Practice Assessment Composer

**Status: IN PROGRESS**

## Goal

Build a blueprint before generating questions.

## Deliverables

- `PracticeAssessmentBlueprint`
- `PracticeAssessmentComposer`
- concentration limits
- form/cognitive distribution
- graceful narrow-skill handling
- deterministic plan from lesson contract + requested count + difficulty policy

## Acceptance criteria

The composer never pads a session with wording-only duplicates to satisfy a count.

---

# Phase 4 — Honest Difficulty Progression

## Goal

Difficulty must change cognitive demand, not only labels or number ranges.

## Deliverables

- per-form difficulty capability;
- Standard / Stretch / Challenge blueprint rules;
- fallback only to valid lower-demand form when explicitly allowed;
- no label claiming a level the generated task does not implement.

---

# Phase 5 — Session Quality and Answer-Leak Validation

## Goal

Certify the complete learner-visible session before persistence.

## Deliverables

- `PracticeSessionQualityValidator`
- `PracticeAnswerLeakValidator`
- semantic duplicate gate
- visual/prompt/options/hint answer leakage gate
- diversity/concentration gate
- alignment + solver + verifier confirmation

## Result states

- `READY_BALANCED`
- `READY_NARROW`
- `BLOCKED_INSUFFICIENT_VARIETY`
- `BLOCKED_GENERATION`
- `BLOCKED_PEDAGOGICAL_QA`

---

# Phase 6 — Full 4,453-Lesson Certification

## Goal

Audit the entire learner-visible Mathematics catalogue.

## Required outputs

For every Lesson Practice contract:

- requested maximum;
- generated valid count;
- distinct semantic identities;
- family coverage;
- form coverage;
- cognitive-operation coverage;
- difficulty coverage;
- answer-leak result;
- exact verifier result;
- final readiness status.

## Programme closure target

All **4,453** lessons are either:

- able to generate a certified learner Practice session; or
- explicitly blocked/narrow with a truthful reason.

There must be:

- 0 silent semantic duplicates;
- 0 learner-visible answer leaks;
- 0 false difficulty labels;
- 0 count-padding by wording-only variants;
- 0 mathematically unverified items.

---

# Phase 7 — Production Rollout

## Release gates

- Phase16 CI green
- Mathematics Intelligence CI green
- PostgreSQL/migration gate green
- CodeQL/SAST green
- production container/Trivy green
- full catalogue Practice V2 audit green
- Render deploy reaches LIVE
- startup/runtime logs contain no new error/critical/fatal events

## Closure report

The final report must include:

- PR number;
- merge commit;
- CI run IDs;
- catalogue certification totals;
- blocked/narrow counts if any;
- Render deploy ID;
- LIVE status;
- runtime error-log check;
- remaining known limitations.
