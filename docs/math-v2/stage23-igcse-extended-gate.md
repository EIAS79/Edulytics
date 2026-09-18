# Stage 23 — IGCSE Extended gate

## Status

**COMPLETE AS A CAPABILITY GATE**

This stage does not claim that Cambridge IGCSE Mathematics (0580) Extended is globally verified.

It establishes the gate that prevents such a claim until every required piece of evidence exists.

## Source inventory

The current Extended pedagogical inventory contains:

- 70 Level 10 supporting lessons;
- 70 Level 11 consolidation lessons;
- 140 lessons total.

The source blueprint currently reports no formal Cambridge OutcomeCode/FormalTarget mappings for these supporting lessons.

Therefore Stage 23 distinguishes:

- **VERIFIED** — formal curriculum mapping, benchmark, solve, verify, grade, difficulty calibration and approved academic review all exist;
- **CONTEXTUAL** — exact Mathematics V2 engine capability overlaps the supporting lesson, but formal curriculum mapping and/or approved academic review are not yet complete;
- **UNSUPPORTED** — no reviewed exact SkillContract mapping is approved for the lesson.

## Current truthful status

- VERIFIED: 0
- CONTEXTUAL: 28
- UNSUPPORTED: 112
- formal curriculum mappings in these Extended packs: 0

This is deliberate fail-closed behavior.

An engine having a quadratic solver, for example, is not by itself evidence that every Cambridge lesson with a quadratic-related title is curriculum-verified.

## Edulytics benchmark corpus

Stage 23 adds an engine benchmark corpus for contextual overlap.

Each benchmark is required to execute:

```text
generate
→ solve
→ independent verify
→ grade with Answer Equivalence V2
→ difficulty calibrate
```

across all three existing UI difficulty bands.

The benchmark corpus currently contains 16 exact engine slices, including reviewed support in areas such as:

- linear equations;
- simultaneous linear equations;
- linear inequalities;
- quadratic equations;
- exact function evaluation;
- arithmetic and geometric sequences;
- rectangle/triangle measurement;
- Pythagoras;
- exact special-angle trigonometry;
- vector addition;
- simple probability;
- arithmetic mean;
- frequency-table mean.

Passing this corpus proves engine capability only. It does not promote the Cambridge curriculum claim.

## Academic review rule

A lesson may not become VERIFIED unless:

1. an explicit formal curriculum mapping exists;
2. the mapped SkillContract is exact;
3. the benchmark generation path passes;
4. the solver passes;
5. the independent verifier passes;
6. answer grading passes;
7. difficulty calibration passes;
8. academic review is explicitly APPROVED.

The current academic review state for Cambridge verification remains **REQUIRED**.

## Claim policy

Until approved evidence exists:

```text
globalIgcseExtendedCapabilityClaimAllowed = false
productRoutingEnabled = false
```

Title similarity alone can never promote a lesson.

## Source of truth

Gate manifest:

`src/Edulytics.Core/Mathematics/Curriculum/stage23-igcse-extended-gate-manifest.v1.json`

Benchmark corpus:

`src/Edulytics.Core/Mathematics/Curriculum/stage23-igcse-extended-benchmark-corpus.v1.json`

Acceptance tests:

`tests/Edulytics.Tests/MathematicsIntelligence/Stage23IgcseExtendedGateTests.cs`

Closure audit:

`tools/math_intelligence/stage23_igcse_extended_gate_audit.py`

## Exit criteria

Stage 23 is complete as a gate when:

- all 140 current Extended supporting lessons are inventoried;
- every lesson has VERIFIED / CONTEXTUAL / UNSUPPORTED status;
- title-only inference cannot promote a lesson;
- every contextual engine claim references an explicit benchmark;
- the benchmark corpus executes generate/solve/verify/grade/difficulty-calibrate;
- VERIFIED promotion requires formal mapping and approved academic review;
- the current zero-formal-mapping state cannot produce a VERIFIED Cambridge claim;
- product routing remains disabled;
- Stage 17–22 closure gates remain green;
- Stage 23 closure and full repository CI are green;
- the merged commit is live on Render and runtime health checks pass.

## Next programme stage

After this gate is live, execution proceeds to **Programme Stage 24 — AS/A-Level 9709 gate**.
