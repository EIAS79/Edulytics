# GEO V13 — Supervisor 2 course/central acceptance handoff (2026-10-29)

## Exact scope and ownership

**PR:** https://github.com/EIAS79/Edulytics/pull/583 (Draft, NO_GO)
**Course:** `US-CCSS-MATH:HS:TRAD:GEO:IM-1E`. Existing branch `curriculum/us-ccss-geo-coder6-source-repairs-20261028`. Authoring: 124/124 original Rich V2 candidates, not 124 academic acceptances. The V13 changes retain prior V11/V12 repairs and all 372 worked-example slots.

**V13 source/content repairs**
- `U06:L15 — Weighted Averages`: original student activity "Part Way: Quadrilateral" requires a shared dilation centre and coordinate partition; an original 1:4 task now includes B′=(14/5,3), C′=(16/5,21/5), D′=(12/5,21/5) about A=(2,3), scale 1/5. Positive factor/ratio and labelled graph require teacher review.
- `U07:L13 — Using Radians`: original "An Arc Length Shortcut" activity requires plotting radius versus arc length, interpreting slope as the central angle in radians; added an original 4-pair graph task with s=1.5r, radius 20→arc 30. The previous sector-area formula content is preserved.
- Task-specific public source reviews recorded for U01:L01, U05:L11, U06:L15, U07:L13, including precise source activity titles and remaining digital/teacher evidence gaps. No restricted cool-down or teacher guide accessed. The V12 39+18+67 ledger partition remains unchanged.

## Independent mathematical result certificates

Course test `tools/curriculum/us-ccss-geo-v13-independent-math-oracles.test.cjs` recomputes **27 named worked-example numerical answers** using independently coded arithmetic, geometry, probability sample-space enumerations and tolerance checks. The examples belong to `U05:L11, U06:L07, U06:L15, U07:L02, U07:L06, U07:L12, U07:L13, U08:L04, U08:L08, U08:L11`. This is **27/372 numerical answer slots** checked in V13, **345/372 not independently certified by these tests**. This is NOT full theorem-proof/figure/teacher-assessment certification for all 27; it cannot authorize academic A–D acceptance.

The complete V13 course Node run on Anas_Nitro / Personal at code SHA `f8297fa45cf273ae1cef13b30e0604497cc32f6d` returned **46 PASS / 0 FAIL / 0 SKIP / exit 0**, running seven explicit Geometry tests:
`node --test tools/curriculum/us-ccss-geo-richv2-contract.test.cjs tools/curriculum/us-ccss-geo-v11-conditional-integrity.test.cjs tools/curriculum/us-ccss-geo-v11-svg-assets.test.cjs tools/curriculum/us-ccss-geo-v12-source-math-contract.test.cjs tools/curriculum/us-ccss-geo-v12-human-scoring-contract.test.cjs tools/curriculum/us-ccss-geo-v13-independent-math-oracles.test.cjs tools/curriculum/us-ccss-geo-v13-teacher-and-tenant-fixtures.test.cjs`.

## Actual visual and assessment contracts still requiring central writer

**B — safe accessible first-party SVG visual references.** V13 adds two original accessible SVGs:
1. `src/Edulytics.Web/wwwroot/geometry/geo/v13/u06-l15-quadrilateral-dilation-grid.svg`
2. `src/Edulytics.Web/wwwroot/geometry/geo/v13/u07-l13-arc-length-radius-linear-graph.svg`

Standalone SVG XML: **2/2 parsed successfully** on authorized workstation, with title/description/accessibility and numerical plotting tests. Earlier V11 has 3 additional SVGs. **Five course-owned SVG candidates exist, zero authenticated Rich V2 SVG renderings proven.** The shared `RichLessonVisualKind` and `_RichLessonVisual.cshtml` do not expose an allowlisted external asset reference. Central owner must add narrowly scoped enum/parser/renderer contract or a reviewed equivalent, validating trusted local/static paths against a maintained list, rejecting `javascript:`, data/HTTP(S) external sources, `..`, scripts and event handlers. Use a DOM/browser test for student and teacher roles, accessibility description, existing ten visual kinds and content-version precedence. Coder 6 did **not** modify the shared engine.

**C — human-scored geometric assessment:** V13 added **two** authoring-only 8-point teacher rubrics, for U06:L15 and U07:L13, on top of four V12 authoring-only rubrics. Paths under `src/Edulytics.Core/Curriculum/LessonContent/Assessment/`. Six authored tasks; **zero actually submitted/reviewed/persisted**. Students must submit diagrams or graphs and proof/explanations, authorized teacher must score with criteria and save feedback. Do not use blanket `READY_VERIFIED`, numeric auto-grade or generic family fallback. The central `lesson-skill-mappings.v1.json` has **zero exact GEO rows**; use source-validated skill/family mapping and independent correct/wrong/equivalent/malformed/edge grader tests before activation. Shared family/generator/verifier/registry belongs to Supervisor 2.

**D — real synthetic two-school API/database checks**: staged `artifacts/curriculum-delivery/US-CCSS-MATH/GEO/geo-v13-synthetic-two-school-negative-matrix-20261029.json` with 2 schools, 4 users, Traditional GEO/ALG1 adoptions, 2 synthetic attempts, 4 positive, 8 negative and 3 persistence/grade/idempotence scenarios. Its JSON and policy-content tests pass, but **the HTTP endpoints, disposable database transactions and actual role enforcement have NOT been exercised**. Supervisor 2 / [Issue #453](https://github.com/EIAS79/Edulytics/issues/453) owns the one-writer course adoption/auth/DB integration, and must run these fixtures against an authorized disposable environment. Do not use live tenant data.

## Remaining protected CI/.NET blockers

- No `dotnet` binary on Anas_Nitro; `global.json` remains pinned to 10.0.200. Therefore Coder 6 V13 .NET Release and E2E are **NOT_RUN**. Requested exact approved-runner command: `dotnet --info && dotnet test Edulytics.sln -c Release`, with protected synthetic DB/HTTPS tests separately provisioned. This does not mean a full-suite PASS.
- Supervisor 2's separate Our-CS check **at the earlier V12 checkpoint**, not V13 HEAD: 121/121 targeted Geometry .NET Release PASS; full solution 6,793 PASS + **2 global Rich V2 fixed-count FAIL**, exit 1. Those two tests are owned by independent, unmerged [PR #598](https://github.com/EIAS79/Edulytics/pull/598). Coder 6 did not change, merge or weaken shared test files.
- At V13 source-code SHA `f8297fa45cf273ae1cef13b30e0604497cc32f6d`, **seven** Actions workflows still showed `queued`, not PASS. Example `Mathematics Intelligence Foundation` [run 40909235075](https://github.com/EIAS79/Edulytics/actions/runs/40909235075) and authenticated academic acceptance [run 40909235248](https://github.com/EIAS79/Edulytics/actions/runs/40909235248). Check latest exact head separately.

**A/B/C/D accepted = 0/124 each.** Do not merge PR #583 or deploy. No real school/student/teacher data has been accessed or written.
