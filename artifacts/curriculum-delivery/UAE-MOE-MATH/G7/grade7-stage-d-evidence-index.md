# UAE Grade 7 General Arabic — Stage D evidence index
**PR:** https://github.com/EIAS79/Edulytics/pull/638

**Baseline A–B–C:** `fa2e7ed41bf6ec61b8cc8f765ba3cc08373ee6d1` (73 annual Arabic + 5 extra alternate-edition candidate lessons).
**Source snapshot ID:** `G7-AR-SOURCE-INDEX-f7b22196bd05c6bfcad83cfefaea9e78dbddb2cb`.
**Diagnostic workflow:** `.github/workflows/uae-g7-stage-d.yml`.
**Executable gate:** `tools/curriculum/uae_g7_stage_d_gate.py`.

Evidence present: `source-index.json`, `multi-edition-source-gap-audit-20261115.json`, lesson-001...078 atomic coverage and A–B–C checkpoint files, 78 corresponding Node test files, 78 Rich V2 lesson files.
Evidence not yet present or proven: complete literal source-element denominator, all 78 true runtime Arabic learner pages, negative English fallback, correct/wrong persisted Practice attempts, authenticated four-role E2E, isolated PostgreSQL old-62 migration, exact-SHA final CI and live Render verification.

Display-order repair: source `ordinal` and `LessonCode` preserved; source `displayOrder` and blueprint `SortOrder` now follow UnitNumber / LessonNumber; the extra five Grade 7 Unit 8 lessons display before Unit 9. Do **not** equate A–C checkpoint ordinals with on-screen positions.

No successful CI, math accuracy, academic source parity or deployment is claimed until linked GitHub run evidence and audited denominators exist.
