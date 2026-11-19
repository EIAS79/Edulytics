# Grade 9 General — source index lock evidence

Target: `UAE-MOE-MATH/G9/General/ar`, 2026–2027, 13 units, 82 numbered lessons.

## Files physically present and SHA-reconciled

- General `01.pdf` (2025–2026): 43 raw entries, including four newer Unit 7 geometry lessons.
- General `02-1.pdf` (2019–2020): 24 raw entries including nine historical Unit 7 polynomial/quadratic lessons.
- General `03-1.pdf` (2019–2020): 19 raw entries, Units 11–13.
- Applied `02.pdf` and `03.pdf`: source comparison only; no General lesson imported solely from their contents.
- File hashes, editions, page counts and source snapshot are in `five-book-inventory.json`.

## Exact target-index counts

`grade9-82-target-index.json`: 82 source-located target rows / 13 units. Unit counts:
`8, 9, 6, 4, 6, 6, 9, 6, 5, 4, 4, 6, 9` = 82.

`five-book-source-crosswalk.csv`: 86 General raw source entries accounted for plus two Applied reference-file rows. The four newer geometry rows are supplementary references in the proposed 82 target, not erased or represented as independently numbered additional lessons.

## Where source skills remain unverified

The target-index *structure* is complete, but the strict `G9_ARABIC_INDEX_LOCK` is **not passed**. The proposed positions of 01.pdf:7-1, 7-2 and 7-3 relative to older 11-1, 11-3 and 12-6 are not full-semantic duplicates. Direct comparison of their opening scanned pages confirms overlapping but distinct objectives:

- 7-1 defines point/line/plane and intersection; 11-1 develops axioms and free-form proofs.
- 7-2 emphasizes ruler measurement and estimation; 11-3 covers segment-addition postulates and segment proofs.
- 7-3 teaches distance and midpoint *on a number line*; 12-6 teaches the perpendicular *distance from a point to a line*. These must not be conflated.
- 7-4 vs 11-3 is previously confirmed as semantically duplicate at lesson scope, with source exercise variants to retain.

Reference evidence is `grade9-unit7-opening-page-evidence.json` and `dedup-crosswalk.json`; full per-exercise source comparison and Arabic integration are still required. Applied source-page pairs have also been compared, but their exhaustive element-level audit remains incomplete.

## Current gates

| Gate | Status |
|---|---|
| Required five source PDFs present/fingerprinted | VERIFIED |
| Target 13 units and 82 rows with sourced start pages | PREPARED |
| Every raw General lesson traced to primary/supplementary source | 86/86 traced |
| Five-book exhaustive content and exercise comparison | PENDING |
| Original newer geometry skill integration without omissions | PENDING |
| Source element 1:1 coverage across 82 lessons | PENDING |
| `SOURCE_INDEX_COMPLETE` | **false** |
| CI and Stage D | NOT_RUN |

No source title or new skill should be deleted to force numeric compliance.
