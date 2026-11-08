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

## Targeted runtime schema compatibility correction

Converted only 72 Grade12 JSON document SchemaVersion values from 2 to 1, preserving lesson data byte-for-byte except the version marker. Educational step/concept/mistake deficiencies remain open; Stage D still NO_GO.

## Targeted legacy RichV2 contract recovery

Counter({'worked_examples_existing_method_check_reused': 616, 'key_concepts_extracted_from_existing_lesson': 41, 'common_mistakes_derived_from_existing_math_check': 31}); changed 71 lesson resources. This is a deterministic repackaging of existing lesson Method, Check, Explanation and Summary texts, NOT evidence of independent mathematical review. Runtime and human QA still pending.

## Verified GitHub Actions runtime milestone — 2026-11-07

- Workflow: https://github.com/EIAS79/Edulytics/actions/runs/42410721273
- Tested **PR synthetic merge** SHA: `d7ad3c522fdeb457fc3bb6ebe8a2eaf45ae4f7e8` (branch head content SHA `17896e87fc4511ef4860711881eda9ee14f5dc89`).
- Shared .NET Release build: **PASS**.
- Grade12 Section1.1 source/renderer contract tests: **23/23 PASS**.
- Grade12 73-lesson embedded RichV2 runtime registry tests: **74/74 PASS** (73 lesson theories + one inventory test).
- Grade12 static content shards `G12-D1`, `G12-D2`, `G12-D3`: **PASS**, 28+21+24=73 lesson authoring resources.
- Structural source inventory gate: **PASS**.
- Final release gate: **FAIL, intentionally fail closed**. No authenticated learner/teacher E2E, classroom grading, per-lesson visual accessibility, source-element parity denominator, or release approval has been validated.
- Ledger: `D_TESTS_PASSED` for 72 lessons, `D_BLOCKED` for U01:L01 due unresolved handoff/source/rights/runtime assessment evidence; `D_VERIFIED=0`; `APPROVED=0`.
- This code evidence does not retroactively validate A–B–C semantic or source parity; prior deterministic reformatting reused existing authored Method/Check text and still requires educator quality review.


## Fail-closed authenticated student deployment check — 2026-11-07

- Run: https://github.com/EIAS79/Edulytics/actions/runs/42413952553
- Tested PR merge SHA: `8415ea3251c6f953be3bfb2b2a2d1522a8849107`; corresponding branch head `8c9559c1f665c98438074b34ac5ec7d6399ae9c1`.
- Disposable PostgreSQL migration and HTTPS build/fixture succeeded; actual authenticated learner page **ordinal 1**, `PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L01`, returned **HTTP 404**. No learner page verified for publication and no authenticated G12 attempt/gradebook sequence completed.
- `GetPublishedForStudentAsync` requires a matching student-course context, canonical body metadata/status, and a published canonical translation. A RichV2 registry load does not meet these conditions.
- Existing grade12 canonical source document `us-ccss-math-g12-precalculus-u01-l01-abc-draft.lesson-content-pack.json` explicitly remains `Draft`, with `ReviewEvidence: A-C INCOMPLETE`; remaining 72 canonical Grade12 content bodies have not been established in the inventory.
- Stage D record updated: **all 73 lessons D_BLOCKED for production**, `D_VERIFIED=0`, `NO_GO_TECHNICAL`; no merge or Render deployment.

## Mathematical correctness correction — Section 11.1

- Commit [`fd20cbad`](https://github.com/EIAS79/Edulytics/commit/fd20cbad880b78c8efd9e1e98086c7eddc06977a) corrects **12** alternate-sequence teacher answer keys with signs initially calculated from the wrong sequence index, synchronizes **2** worked examples and **1** Try It, and introduces an independent `n=1..5` regression oracle.
- Targeted local oracle PASS. This does not certify all source exercise keys, and does not authorize publishing Section 11.1.
- Source parity counts/denominators, human review, public lesson status, complete authenticated G12 student/teacher E2E and full product regressions remain unresolved.
