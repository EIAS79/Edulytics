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

## Direct local source integrity check — 19 November 2026

`five-book-page-raster-audit.json` records a newly executed read-only local audit:

- All five file SHA-256 hashes independently recomputed on the authorized Our-CS computer and matching the source manifest.
- Every one of the 86 raw General lesson opening physical PDF page references is within its source PDF and renders a nonblank grayscale image.
- Each opening page has a reproducible thumbnail pixel SHA-256 fingerprint, allowing later comparisons against the same scan without repeated desktop reads.
- Page accessibility and nonblank appearance **do not certify** printed-page heading correspondence, the accuracy of each exercise, or semantic duplicate decisions.

`SOURCE_INDEX_COMPLETE` therefore remains **false**.

## Source-reconciliation progress — 20 November 2026

Prepared source-target skill supplements have been committed **without claiming full semantic parity**:

| Newer General source | Existing receiving lesson | Targeted retained skill | Authoring commit | Atomic source parity |
|---|---|---|---|---|
| `01.pdf:7-1` printed 365 | `03-1.pdf:11-1` | Plane naming by three noncollinear points; collinear vs coplanar distinction, in addition to old proof postulates | `a3761466ff8c1f0876cc36c2b25cf65fcd87ad28` | PENDING |
| `01.pdf:7-2` printed 374 | `03-1.pdf:11-3` | Calibrated ruler length with nonzero starting reading, unit and precision | `30777bcd2e054edb2af84acf0f86ab40a247efde` | PENDING |
| `01.pdf:7-3` printed 385 | `03-1.pdf:12-6` | Number-line distance and midpoint, distinct from point-to-line perpendicular distance | `288dc0d66dad6ec4b6d52563943b4144f2ce0760` | PENDING |
| `01.pdf:7-4` printed 398 | `03-1.pdf:11-3` | Previously recorded segment-relations overlap; supplemental exercise variants must be retained | existing prior evidence | PENDING |

The source image for `01.pdf` printed p.365 (physical PDF page 376) was reopened on the authorized Our-CS device and directly inspected; it confirms explicit point/line/plane representation and the naming/collinearity vocabulary. This is **single-page evidence only**, not a complete source-lesson exercise audit.

Prepared focused tests: `academic-uae-grade9-arabic-u11l01.test.cjs`, `...u11l03.test.cjs`, `...u12l06.test.cjs`; execution reserved for Stage D.

An additional **deliberately strict not-yet-passing** academic source gate is prepared in `tests/academic-uae-grade9-strict-source-index-gate.test.cjs` (commit `9394b47c960c68e4ef172e9a1cbf09f7bf28cccd`), requiring explicit 86-source atomic verification, 82-target source-element parity, and unambiguous supplement placement before `SOURCE_INDEX_COMPLETE` can be true. It is **WRITTEN_NOT_RUN**.

**Do not declare the A–B–C source index or semantic audit complete merely because all 82 authoring checkpoints say READY_FOR_STAGE_D.** The per-lesson authoring records are distinct from the independent source parity gate. No Stage D, CI, merge or production writes are claimed.
