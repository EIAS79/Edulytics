# Rich Lesson Content V2 — R1 Catalogue Baseline

**Audit authority:** `RichLessonContentQualityAudit` against the effective learner-facing canonical lesson bodies materialized by `MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()`.

**Baseline date:** 2026-09-22

## Exact catalogue

| Measure | Count |
|---|---:|
| Learner-visible mathematics lessons | 4,453 |
| Supported curriculum packs | 4 |
| Officially mapped lessons | 3,104 |
| Supporting / no-formal-outcome lessons | 1,349 |
| Source Policy v2 coverage | 4,453 |
| Lessons requiring pedagogical-source research before enrichment | 1,569 |
| Lessons with no curated Video Help resource | 4,453 |

## Quality baseline

| Overall Rich Content V2 status | Count |
|---|---:|
| Good | 131 |
| Needs expansion | 2,751 |
| Generic | 2 |
| Source research required | 1,569 |

The result confirms the programme premise: **4,314 of 4,453 lessons are not yet Rich Content V2 ready** under the first-pass deterministic structural standard. Of those, **1,569** require a pedagogical-source research pass because the current pack is framework-only.

## By curriculum

| Curriculum | Lessons | Official | Supporting | Good | Needs expansion | Generic | Source research |
|---|---:|---:|---:|---:|---:|---:|---:|
| CAMBRIDGE-INTL-MATH | 566 | 27 | 539 | 0 | 564 | 2 | 0 |
| PL-NATIONAL-MATH | 1,569 | 1,569 | 0 | 0 | 0 | 0 | 1,569 |
| UAE-MOE-MATH | 758 | 42 | 716 | 0 | 758 | 0 | 0 |
| US-CCSS-MATH | 1,560 | 1,466 | 94 | 131 | 1,429 | 0 | 0 |

## Section-level structural findings

| Section | Strong | Needs expansion | Generic |
|---|---:|---:|---:|
| Explanation | 1,458 | 2,983 | 12 |
| Key Concepts and Rules | 1,524 | 2,926 | 3 |
| Worked Examples | 1,475 | 2,978 | 0 |
| Step-by-Step Solutions | 1,504 | 2,947 | 2 |
| Common Mistakes | 394 | 4,059 | 0 |
| Quick Summary | 1,277 | 3,176 | 0 |

## Visual baseline

| Visual evidence | Count |
|---|---:|
| Explicit instructional visual | 265 |
| Topic-specific visual evidence | 1,425 |
| Generic-fallback-only / no explicit lesson visual | 2,763 |

## Source baseline

All 4,453 effective lesson bodies currently resolve through Source Policy v2.

Pedagogical source types in the effective catalogue:

- Open Educational Resource: 2,842
- Official Framework Only: 1,569
- Current Official Textbook: 42

This means R3 does **not** begin from zero. Existing provenance and rights metadata can be reused as the starting source dossier. However, the 1,569 Polish lessons currently use `OfficialFrameworkOnly`; under Rich Lesson Content V2 they are explicitly classified as requiring a pedagogical-source research pass before source-driven enrichment.

## Interpretation

This baseline does not say that 4,314 lessons are mathematically wrong. It says they do not yet satisfy the Rich Lesson Content V2 teaching-depth standard.

The audit is deliberately stricter than the legacy requirement that six fields merely be non-empty.

It identifies the exact workload without modifying:

- curriculum identities;
- official outcomes;
- mappings;
- lesson identities;
- assessments;
- practice history;
- mastery history;
- analytics history.

## Generated CI artifacts

Every Phase16 full-regression run now emits and preserves:

- `artifacts/lesson-content-v2/catalogue-inventory.json`
- `artifacts/lesson-content-v2/content-quality-audit.json`
- `artifacts/lesson-content-v2/catalogue-summary.md`

These machine-readable files are the execution baseline for R3–R8.
