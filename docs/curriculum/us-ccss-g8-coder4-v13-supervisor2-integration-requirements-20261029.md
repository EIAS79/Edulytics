# Grade 8 — V13 shared integration requirements for Supervisor 2

**Repository:** `EIAS79/Edulytics` | **Existing PR:** #593 (Draft, NO_GO)
**Course:** `US-CCSS-MATH:G8:OUR-IM-2017` | **Owner:** Coder 4 for course materials; Supervisor 2 for shared engine/renderer/DB/adoption.

This is a technical contract handoff, **not** permission for Coder 4 to edit shared files, and not academic acceptance.

## Grounded current course scope
- 131 canonical lessons, 9 units, 393 preserved worked examples, 36 original local SVG files. New V13 course-owned figures: U01:L08, L09, L10, L14, L16; prior 31 preserved.
- Source figure priority 113 lessons: 34 have a candidate first-party SVG, 79 still require individual representation decisions. **Zero authenticated SVG render confirmations.**
- U01 17/17 public student/teacher lesson pages spot-checked with targeted first-party additions; a **spot check is not the full item-by-item Stage A acceptance**.
- 22 standards-role discrepancies and 12 lessons with at least one static unmapped target remain for explicit review; 18 are potential multi-target lessons. Neither group proves an actual generated Practice fault.
- 131 original draft rubrics and 117 teacher-only Cool-down tasks in existing audit; **zero teacher-scored, persisted and authenticated validations.**

## Shared single-writer tasks and acceptance tests

1. **Rich V2 asset security and actual learner rendering.** Adopt course-local SVG allowlist/manifest without copying publisher artwork, resolving files securely in authenticated student HTML. Verify the five new V13 figures and one prior V12 figure are visible only in correct course/lesson contexts. Test alt/desc, mobile and keyboard accessibility, wrong lesson/course/tenant denial and broken URL fallback. Existing V11 SVG metadata explicitly says assets are not attached. This task belongs to Supervisor 2 or their integration writer.
2. **Geometry interaction equivalence.** The U01 lesson source requires open constructions (isometric-grid moves, reset, card sort, midpoint/180° segment, labeled line/triangle correspondence, drawing a line through the upper vertex). A static SVG or automatic numeric check is insufficient evidence of a student-built construction. Provide an accessible drawing/geometry submission or teacher-scored alternative with explicit task/lesson binding. Coordinate version rollouts and content adoption; do not silently make the static figure an automatically graded response.
3. **Actual selected Practice contracts.** For all 131 lesson IDs load the production-selected Practice family, generated question and actual evaluator. Compare to the authored objective/activity, not solely broad CCSS. Test correct, wrong, equivalent, malformed, boundary, unauthorized, wrong course/lesson and wrong school. Prioritize the source crosswalk's 12 static-unmapped and 18 potential multi-target lessons, plus all U01 construction and explanation tasks. **Do not equate a static registry mapping with a verified generator or saved attempt.** Coder 4 has not changed shared families.
4. **Teacher rubrics / Cool-down pipeline.** Bind course-specific existing draft tasks to authenticated teacher workflow, preserve manually judged justifications and diagram evidence, enforce teacher-only scoring and feedback. Test two synthetic schools and correct/wrong role, grade visibility, score persistence and HTTPS readback on an isolated PostgreSQL fixture; never use real student data.
5. **Standards-role review.** Reconcile 22 edition-matched source role flags with reviewer decision; preserve Addressing / Building Towards distinctions. Do not mechanically rewrite standards mappings.
6. **U03:L12 shared ordered-pair membership.** Supervisor 2 to review PR #597 against Grade 8 current course branch, not cherry-pick against its earlier base; the textual `Ax+By=C` membership verifier is not a coordinate graph or human-marked construction.
7. **Version and protected CI.** Coordinate atomic content-version transitions with existing adopters/tenant isolation, run required protected CI against a frozen actual SHA, protect other curricula. No PR merge, deployment, owner UAT or academic approval can be inferred from green local tests.

## Evidence references
- `docs/curriculum/us-ccss-g8-coder4-v13-u01-complete-spot-review-ledger-20261029.json`
- `docs/curriculum/us-ccss-g8-coder4-v13-u01-complete-source-practice-teacher-audit-20261029.json`
- `docs/curriculum/us-ccss-g8-coder4-v13-u01-l01-l07-source-evidence-20261029.json`
- `docs/curriculum/us-ccss-g8-coder4-v13-u01-l11-l17-source-evidence-20261029.json`
- `docs/curriculum/us-ccss-g8-coder4-v13-u01-source-geometry-evidence-20261029.json`
- Course-specific regression tests under `tests/academic-us-ccss-g8-coder4-v13-*.test.cjs`.

**Status:** `NO_GO` — A/B/C/D independent academic acceptance 0/131. The course-owned branch is a candidate, not a publication.
