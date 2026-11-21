# Grade 9 General — resolution of Unit 7 across editions (Stage A)

**Academic target:** 13 units / 82 lessons. **State:** structural reconciliation recorded; NOT an accepted semantic-source lock.

## Source discrepancy

- The newer General textbook `01.pdf` (2025–2026) contains 39 numbered lessons in Units 1–6 and four newer geometry lessons in Unit 7:
  `7-1` النقاط والمستقيمات والمستويات, `7-2` القياس الخطي, `7-3` المسافة ونقاط المنتصف, `7-4` إثبات العلاقات بين القطع المستقيمة.
- The older General textbook `02-1.pdf` (2019–2020) contains **nine** historical Unit 7 lessons, from polynomial operations through quadratic factorization, which the owner requires retaining. Units 8–10 contain 15 additional lessons, and `03-1.pdf` supplies 19 lessons for Units 11–13.
- The target structure uses 39 + 9 + 34 = **82 numbered lessons**. Combined raw source indexes yield 86 entries because they retain both different Unit 7 sets. Neither set may be silently deleted from the learning objectives.

## Explicit four-entry supplementary mapping

| Newer source lesson | Proposed historical receiving lesson | Decision and source-evidence gate |
|---|---|---|
| `01.pdf:7-1` points, lines and planes | `03-1.pdf:11-1` axioms and free-form proofs | **Proposed complementary objectives**. Must preserve point/line/plane vocabulary and all unique representations. Not proven to be a duplicate. |
| `01.pdf:7-2` linear measurement | `03-1.pdf:11-3` segment relationship proof | **Proposed supplemental learning**. Must verify measurement techniques and exercises individually; not a proven duplicate. |
| `01.pdf:7-3` distance and midpoint | `03-1.pdf:12-6` perpendiculars and distances | **Proposed supplemental learning**. Must verify coordinate midpoint constructions and distance tasks and add if missing; not a proven duplicate. |
| `01.pdf:7-4` segment relationships | `03-1.pdf:11-3` segment relationship proof | **Confirmed semantic duplicate** after seven-page source comparison, with distinct exercise variations still to be preserved. |

The first three mappings are **placement proposals**, not permission to discard skills or close the mapped historical lesson.

## Four distinct gate results

- `TARGET_STRUCTURE_13_UNITS_82_ROWS`: prepared; see `grade9-82-target-index.json`.
- `RAW_SOURCE_TRACEABILITY_86_OF_86`: represented as primary or supplementary refs; says nothing about semantic equivalence.
- `FULL_FIVE_BOOK_SOURCE_AUDIT`: five PDFs have registered fingerprints and roles, but exhaustive comparison remains pending.
- `G9_ARABIC_INDEX_LOCK`: **FAIL/PENDING** pending source element inventories, page comparison and remaining three geometry placements.

**Next action:** examine all source examples, exercise categories and visuals for `01.pdf:7-1`, `7-2`, `7-3` against the proposed historical receivers; preserve the unique tasks in Arabic before marking any `MERGED` outcome. No CI / test execution in Stage A–B–C.

## Targeted content repair — newer 7-3 number-line skill (20 November 2026)

- Existing receiving lesson `03-1.pdf:12-6` already covered perpendicular distance, two-point coordinate-plane distance and coordinate midpoint in Arabic RichV2.
- The previously under-explicit *number-line* formulas from newer `01.pdf:7-3` are now authored in the **existing** lesson `PED:UAE-MOE-MATH:L9:GENERAL:12:06:PERPENDICULAR-DISTANCE-MIDPOINTS`: `|b-a|` distance and `(a+b)/2` midpoint, with worked example endpoints -3 and 5, check, common mistake, and a CalculationChain visual.
- Source-specific conceptual basis: `grade9-unit7-opening-page-evidence.json`, newer printed page 385. This **does not** independently certify the remaining scanned pages, every source exercise/visual or 1:1 source-item parity.
- Content commit: `288dc0d66dad6ec4b6d52563943b4144f2ce0760`. Prepared Node regression: `86db8a8307157a321dbd2e581e5c9ac269eaa564`, **WRITTEN_NOT_RUN** (Stage D).
- The proposed mapping `01.pdf:7-3 -> 03-1.pdf:12-6` stays `PARTIAL_SUPPLEMENT_AUTHORED_NOT_FULLY_AUDITED`; do not count it as a proven semantic duplicate. No increase or decrease to target lesson count is authorized on this evidence.
- `SOURCE_INDEX_COMPLETE=false` until the remaining source-element inventories and the other Unit 7 supplemental gaps are audited.

## Targeted 7-2 measuring supplement repair — 20 November 2026

- Used the previously recorded direct source-opening evidence `grade9-unit7-opening-page-evidence.json`: newer `01.pdf:7-2` printed p.374 / PDF p.385 prioritizes reading calibrated rulers and estimating lengths, while historical `03-1.pdf:11-3` focuses on segment-addition proofs.
- The existing `U11L03` Arabic RichV2 already described units and ruler graduations; repaired the under-explicit **nonzero ruler-start offset** skill by adding `|end-start|`, an original worked example (1.2 to 5.6 cm => 4.4 cm), a precision caveat, visual calculation chain and typical misconception. Preserved segment-addition and congruence proofs. Also corrected an Arabic conditional grammatical error.
- RichV2 content commit: `30777bcd2e054edb2af84acf0f86ab40a247efde`. Regression authored in `tests/academic-uae-grade9-arabic-u11l03.test.cjs` commit `22c2d676af8c6dd06b7b5d77eee57b7558a1e705`, **NOT RUN** under Stage A–B–C.
- Source-device `01.pdf` page range 385–387 was reopened via the authorized Our-CS PDF reader, but it returned scanned image pages without extracted text; this is **not** a complete exercise-by-exercise comparison or a claim to have validated the newly authored example against exact source exercise wording. The example is original and skill-aligned.
- Mapping `01.pdf:7-2 -> 03-1.pdf:11-3` remains **PARTIAL_SUPPLEMENT_AUTHORED_NOT_FULLY_AUDITED**. `SOURCE_INDEX_COMPLETE=false`; full source examples/exercises/visuals must still be checked before locking.

## Direct full-source geometry-page review and later repairs — 20 November 2026

The authorized Our-CS device provided original scans directly. The following **entire printed lesson page spans** were visually inspected (this is source-page evidence, not automatic proof of exact generated question parity):

- `01.pdf:7-1`, printed pp.365–373 (PDF 376–384). New source inventory `u07-new-geometry-7-1-source-element-preparation.json` records source items 1–87 by section: 61 lesson-linked, 26 standardized/prerequisite/spiral. Original Arabic source skills integrated into existing `U11L01`: naming planes, collinearity/coplanarity, and line-vs-plane intersection reasoning. Original proof curriculum retained. Content commits `a3761466`, `58dd01a2`; test commits `35bcc6c7`, `ff9dfef5`.
- `01.pdf:7-2`, printed pp.374–384 (PDF 385–395). Source inventory `u07-new-geometry-7-2-source-element-preparation.json` records 60 main numbered items (41 lesson-linked / 19 review) and the distinct **23-item accuracy-and-precision extension** on pp.382–384. Newly preserved in existing `U11L03`: nonzero-start ruler measurement, measurement precision versus accuracy, maximum rounding error, significant figures and relative error. Content commits `30777bcd`, `95a5e609`; test commits `22c2d676`, `a9a9a56b`.
- `01.pdf:7-3`, printed pp.385–395 (PDF 396–406). Source inventory `u07-new-geometry-7-3-source-element-preparation.json` records 85 individually visible numbered positions in ranges 1–75 and 77–86, with **number 76 not visually located** and intentionally flagged for verification (do not fabricate its wording). Source 7-3 includes number-line and plane distance, midpoint, unknown endpoint from known midpoint, segment-bisector constructions, spreadsheet and map tasks. Existing `U12L06` retains source 12-6 perpendicular-distance skills and adds the newer distinct midpoint skills. Content commits `288dc0d6`, `4d588534`; tests `86db8a83`, `de1ef3e2`.
- `01.pdf:7-4`, printed pp.398–404 (PDF 409–415) was visually paired page-for-page with older `03-1.pdf:11-3`, printed pp.624–630 (PDF 40–46). Their lesson-level segment-sum/congruence proof objectives substantially overlap, but newer **29** and older **36** numbered exercises are distinct inventories. `u07-new-geometry-7-4-vs-11-3-source-comparison.json` keeps both. One additional original proof variant is authored within `U11L03` (commit `3de1a396`); regression `328448b7`.

Do not infer that these source exercises now all have exact individual digital generated-question matches: the page/section inventories document observed scope and grouped teacher review routes, not that still-missing verification. All focused tests remain **WRITTEN_NOT_RUN** in A–B–C.

The source count and proposed target remain 86 raw / 82 target, with `SOURCE_INDEX_COMPLETE=false`. The new academic source gate `tests/academic-uae-grade9-strict-source-index-gate.test.cjs` intentionally cannot pass until 86 atomic source lesson audits, 82 target source-element checks and supplement routing are evidenced. It has not been executed.

**Next source blocker:** resolve exact exercise coverage for geometry supplements (including the visually unlocated 7-3 question 76) and the rest of the 86-source / 82-target element-level audits; neither editorial authoring nor file raster checks suffice for full semantic lock.
