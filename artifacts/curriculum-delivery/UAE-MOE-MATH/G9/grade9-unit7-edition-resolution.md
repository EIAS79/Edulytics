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
