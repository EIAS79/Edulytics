# Grade 4 — Stage D / Final CI

Start baseline: 149 A-C handoff checkpoints on e06060b22b7c8b51a2400694f4c2acb88f587285. Stage D 0/149 accepted; human approval 0/149.

On 2026-10-27 corrected 49 stale Node assertions that treated READY_FOR_STAGE_D as false after A-C closure, without modifying Stage D flags.

Live source mapping registry contains 49/49 entries for ordinals 101–149. The older g4-101-149-richv2-practice-audit.json claims 0/49 and is stale. The live registry declares 13 READY_VERIFIED and 36 STAGE_C_AUTHORED_PENDING_STAGE_D. Presence of metadata is not runtime evidence.

Stage D diagnostics: tools/curriculum/g4-stage-d-audit.cjs preflight, CI workflow .github/workflows/us-ccss-g4-stage-d.yml, 3 Node shards D1 1–50, D2 51–100, D3 101–149 plus scoped .NET and Rich V2 checks.

Missing gates NOT waived: per-lesson source-element 1:1/rights, graphics/keyboard and accessibility, authenticated student/teacher routes, Grade 4 Practice correct/wrong/malformed and persisted score/tenant isolation, assessment lifecycle, final full-grade regression, academic reviewer approval.

Academic and release status NO_GO until evidence is bound to immutable candidate SHA, all mandatory gates pass and authorized release decision is recorded. No Neon production write, Render deploy or merge is authorized by digital diagnostics alone.
