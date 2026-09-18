# Stage 25 — IB AA HL-style gate

## Status
**COMPLETE AS A CAPABILITY GATE**

This stage does **not** claim official International Baccalaureate curriculum alignment, endorsement, certification, or full Mathematics: Analysis and Approaches HL coverage.

The gate measures the Mathematics Intelligence Kernel against six advanced-demand classes required by the governing execution plan:
- Routine
- Multi-step
- Modelling
- Reasoning
- Proof
- Unfamiliar transfer

Current truthful capability status:
- Routine: EVIDENCED — 5 narrow executable engine slices
- Multi-step: EVIDENCED — 4 narrow executable engine slices
- Modelling: EVIDENCED — 4 narrow executable engine slices
- Reasoning: EVIDENCED — 2 narrow sequence-pattern slices
- Proof: UNSUPPORTED
- Unfamiliar transfer: UNSUPPORTED
- Total executable benchmark slices: 15
- Formal IB curriculum mappings: 0

Every executable benchmark must pass all three UI difficulty bands through:
generate → solve → independent verify → grade → strategy plan → difficulty calibrate.

The Reasoning evidence is deliberately narrow and does not establish general argument/proof capability. Proof and unfamiliar-transfer capability remain fail-closed until dedicated production-grade benchmark corpora and independent verification exist.

Global IB AA HL capability claims and product routing remain disabled. Engine evidence alone cannot promote an official curriculum claim.

## Source of truth
- `src/Edulytics.Core/Mathematics/Curriculum/stage25-ib-aa-hl-style-gate-manifest.v1.json`
- `src/Edulytics.Core/Mathematics/Curriculum/stage25-ib-aa-hl-style-benchmark-corpus.v1.json`
- `tests/Edulytics.Tests/MathematicsIntelligence/Stage25IbAaHlStyleGateTests.cs`
- `tools/math_intelligence/stage25_ib_aa_hl_style_gate_audit.py`

## Next programme stage
After this gate is live, execution proceeds to **Programme Stage 26 — Security and resource controls**.
