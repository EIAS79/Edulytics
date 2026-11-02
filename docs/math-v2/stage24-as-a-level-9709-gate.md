# Stage 24 — AS/A-Level 9709 gate

## Status
**COMPLETE AS A CAPABILITY GATE**

This stage does not claim that Cambridge International AS & A Level Mathematics (9709) is globally verified.

Coverage is reported per domain/paper route across:
- Pure Mathematics
- Mechanics
- Probability & Statistics

The current source inventory contains 57 supporting lessons across 6 routes:
AS-PURE, AS-MECHANICS, AS-PROBSTAT, A-PURE, A-MECHANICS, A-PROBSTAT.

Current truthful status:
- VERIFIED: 0
- CONTEXTUAL: 25
- UNSUPPORTED: 32
- formal OutcomeCode/FormalTarget mappings in the current packs: 0

A lesson may become VERIFIED only after explicit formal curriculum mapping, exact SkillContract mapping, benchmark generation, solving, independent verification, answer grading, difficulty calibration, and APPROVED academic review.

Stage 24 adds a 25-slice engine benchmark corpus spanning all three required domains. Every benchmark must pass all three UI difficulty bands through:
generate → solve → independent verify → grade → difficulty calibrate.

Engine capability evidence remains ENGINE_EVIDENCE_ONLY and cannot promote a Cambridge 9709 curriculum claim on its own.

Global 9709 claims and product routing remain disabled. Title similarity alone never promotes a lesson.

## Source of truth
- `src/Edulytics.Core/Mathematics/Curriculum/stage24-as-a-level-9709-gate-manifest.v1.json`
- `src/Edulytics.Core/Mathematics/Curriculum/stage24-as-a-level-9709-benchmark-corpus.v1.json`
- `tests/Edulytics.Tests/MathematicsIntelligence/Stage24AsALevel9709GateTests.cs`
- `tools/math_intelligence/stage24_as_a_level_9709_gate_audit.py`

## Next programme stage
After this gate is live, execution proceeds to **Programme Stage 25 — IB AA HL-style gate**.
