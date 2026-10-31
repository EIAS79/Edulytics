# Coder 9 — U01:L01 implementation evidence (2026-10-31)

**Branch / PR:** `us-ccss/supervisor3/coder9-adv-trig-geo` / #586. **Starting HEAD:** `9922eec073d6d8f1acbd0fa5e394c086d70b0aa4`. One active lesson: `PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L01`. L02–L05 implementation remains unchanged. This packet accompanies **real lesson-local code/content edits**, not a lesson-closure assertion.

## A and B — implemented first-party representation additions

- Retains original source-task #1–#5 comparison and prior verified MVP source SHA-256 `8213e7498696cc71320ae254d21606083b0d24fbe73acc8a27e1588f0bc1d790`, pp. 50–53, existing right-triangle, generic reflection, six angle identities, algebraic tangent proofs, CFU, and original source lineage. No source PDF or third-party diagram copied.
- Added an accessible text-first labelled right-triangle construction: at right vertex A with angle B=x and C=90°−x, leg AC=a and hypotenuse BC=h justify `sin x=a/h=cos(90°−x)` for **all acute x**. Explicitly distinguishes the general unit-circle proof for all real x.
- Added two complete exact-value table rows, for `x=pi/6` and `x=7pi/6`, with base, `pi−x`, `pi+x`, `2pi−x` sine/cosine pairs. Includes negative angles, quadrants and text-first nonvisual alternatives to plotting; plots/tables are supporting evidence, **not** a general-angle proof.
- English L01 `stepByStepSolutions` edited in original Rich V2 pack. Other lesson payloads and immutable source/provenance metadata retained. New pack Git blob at local content checkpoint `037d627e6ac3b856f4d87af8579e7e3dc6b223f8` (compute again on final commit).
- Existing old learner-body successor proposal is now a **historical candidate only** and cannot authenticate the new teaching body. Updated version, allowed predecessor/successor digest, protected body-parity entry, actual renderer/accessibility QA and academic visual acceptance remain **PENDING appropriate owner**. Do not copy the old proposed digest into the canonical `canonicalBodySha256`.

## C — course-only exact-answer implementation

- Existing non-routable `UsCcssHsAdvTrigGeoU01L01PracticeCandidate` retains fail-closed `INTEGRATION_PENDING` and exact-only behavior. Keeps the earlier digit-whitespace safeguard: `sqrt(1 2)` never normalizes into `sqrt(12)`.
- Three scorable fixed cases retain original approved exact answers; reviewed and added exact aliases: `-sqrt(27)/6`, `-3/sqrt(12)` (negative `sqrt(3)/2` questions), and `sqrt(18)/6`, `2/sqrt(8)` (positive `sqrt(2)/2` case). Both fixture manifests and course-only C# reviewer classifier updated together. This is an **explicit finite allowlist**, not general CAS equivalence.
- Radian-only input-mode guard rejects an undeclared/wrong angle mode. Wrong signs, malformed expressions, decimal approximations (including scientific notation), nonfinite values and invalid lesson/case pairs remain rejected; tangent pole yields `OUT_OF_DOMAIN` with no numeric credit.
- Arbitrary-angle explanation and diagram proof remain under the existing **6-point `HUMAN_REQUIRED`** scoring rubric. No automatic human-proof grading.
- Added focused C# positive/negative/cross-case/angle-mode and whitespace tests, and text-table source check. Tests: **AUTHORED / NOT_RUN** during A–B–C. Only `git diff --check` and non-running JSON/static assertions performed; no suite, .NET build, CI, E2E or Stage D executed.

## Current completion gates (not waived)

`A_CLOSED=false` / `B_CLOSED=false`: source-goal scope reviewed previously, but actual rendered graphical/accessibility acceptance and approved content predecessor/successor not established.

`C_PREPARED=true`, `C_CLOSED=false`: the candidate is course-local and **NOT connected to the selected learner Practice route**. PR #600's near-equal decimal acceptance is incompatible with `EXACT_SYMBOLIC_ONLY_ALLOWLIST`. Supervisor 3 / unique integration writer must obtain a valid, explicitly serialized shared-file lease via authoritative [Issue #605](https://github.com/EIAS79/Edulytics/issues/605), then wire and statically verify exactly the correct L01 selection/routing and teacher contract. #453 school adoption and final authenticated persistence are separate later integration/Stage D requirements.

**Cursor:** `LAST_CONFIRMED_CLOSED_LESSON=NONE`; `ACTIVE_LESSON=U01:L01`; `BLOCKER=SOURCE_VERSION_VISUAL_APPROVAL_AND_EXCLUSIVE_SELECTED_PRACTICE`; `CLOSURE_SHA=NONE`; `READY_FOR_STAGE_D=false`; `UNIT_ABC_CLOSED=false`. Preserve L02–L05 untouched until all three L01 stages satisfy their gates.
