# Grade 12 Stage D — evidence index (initial checkpoint)

- PR: https://github.com/EIAS79/Edulytics/pull/626
- Baseline candidate, not yet tested: 1ce84fa2ee97c62feaeb10e3d5708f00e524052f
- Original source SHA256: f0a83745e1ea5b4200bac371afb64615742528e4d31e1e4d241623a38e454e7d
- Source edition: OpenStax Precalculus, Rice University ©2017, Revision PR-2015-002(03/17)-BW, original 2014
- Inventory: 73 sections / 12 chapters; A–B–C progress READY 73/73.
- Handoff document still claims incomplete. Conflict unresolved.
- Source parity: UNKNOWN. Digital coverage: UNKNOWN.
- First Stage D lesson U01:L01: D_BLOCKED; 2–73: D_PENDING.
- D_VERIFIED 0/73, academic approval 0/73.
- No Stage D tests run on this candidate yet.
- No deployment, production data changes, approval or merge.

## First lesson actual blocking evidence
Progress first lesson: SourceElementsIndependentlyMatched=0;
SemanticEquivalenceVerified=false; RightsVerified=false;
ExerciseRoutingRuntimeBound=0 despite READY_FOR_STAGE_D.

## Next step
Run GitHub Actions independent source/static checks and focused G12 .NET tests.
Reconcile only proven source/runtime deficits and record actual run IDs.

## Actual runtime contract failure inventory — 2026-11-07T20:16:41.716891+00:00

Checked against RichLessonContentV2Registry.Validate, not just JSON syntax.
72/73 resources have unsupported SchemaVersion 2; runtime accepts 1.
- RUNTIME_COMMON_MISTAKES_BELOW_MIN_2: 31 lessons
- RUNTIME_KEY_CONCEPTS_BELOW_MIN_3: 21 lessons
- RUNTIME_RICHV2_SCHEMA_VERSION_UNSUPPORTED: 72 lessons
- RUNTIME_WORKED_EXAMPLE_STEPS_BELOW_MIN_3: 71 lessons
No invented filler content; repair documented deficiencies in source-aligned lessons sequentially.
No D_VERIFIED or release decision has been made.
