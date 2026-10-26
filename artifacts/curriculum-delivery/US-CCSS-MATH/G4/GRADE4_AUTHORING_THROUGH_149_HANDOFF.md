# Edulytics Grade 4 — authoring through lesson 149, NOT release readiness

**Branch:** `curriculum/g4-sequential-abc-20261026`  
**Grade inventory:** 149 canonical lessons.  
**Historical technical ledger before this continuation:** ordinals 1–100 marked `READY_FOR_STAGE_D` (Stage D was NOT executed).  
**This continuation:** 101 already repaired separately; 102–149 inspected against original local 2021 teacher preparation + student-facing material, then repaired/extended in the learner content pack and Rich V2, with first-party Cool-down and two focused static tests each. Source hashes and activity/excerpt evidence are in `lesson-102-149-original-source-intake.json`.

## What is actually established
- All 49 ordinals 101–149 have lesson-specific first-party authoring, distinct mathematical examples, checkpoint artifacts, and focused Node tests **written**.
- Every ordinal 102–149 has both teacher preparation and 2021 teacher-page SHA-256 values recorded; the latter includes real **Student Facing** excerpts and lesson activity names. These do not certify independent student PDF or the authenticated publisher Cool-down.
- Topic-specific defects corrected: division/base-ten/partial quotients, real-life remainder contexts, measurement, lines/angles/protractors, shape classification, symmetry, perimeter, fractions and decimal fractions, multiplication algorithms, original word-problem design, reasoning and Number Talks.
- Existing learning-goal introductions and source task excerpts retained rather than replacing with generic off-topic answers.
- Corrected an authored floating-point test for fractional equality in L138 to an exact integer cross-product check. Corrected L140's first-party question to ask for the second runner's time, not the difference.
- GitHub Actions branch search returned **zero workflow runs** for this working branch at this checkpoint.

## What is NOT established
- **No 101–149 lesson is READY_FOR_STAGE_D or internally academically approved.** Grade 4 latest confirmed technical closure stays at ordinal 100.
- `node --test` focused tests for 101–149, compiled app checks, Stage D original quality checks, independent exact Practice/answer-verifier tests, real authenticated tenant/student scoring and score persistence have **not** run.
- Original publisher 2021 gated Cool-down content was not verified. Edulytics Cool-downs are new first-party items, not source-identical substitutions.
- Actual original charts/diagrams, card sorts, drawing, protractor measurement, folding, or student-built tasks need source-figure and teacher review. Rich V2 `Visuals: []` means no original figure fidelity is claimed.
- Four inspected saved standalone student mirrors at U06:L13–L16 contain `404 | Section`. Historical local file hashes cannot prove a valid student source.
- No PR merge, production database migration, Render LIVE rollout, or owner UAT.

## Required completion sequence
1. Run the focused `tests/academic-us-ccss-g4-ordinal-101-...149-local-source-guard.test.cjs` Node test files on an authorized development/CI runner; fix failures and bind results to exact SHA.
2. Inspect grade 4 actual Rich V2 materialization in the student view; verify original visual source/figure and rights/attribution, fix missing required figures.
3. Resolve per-lesson exact SkillContracts and allowed/forbidden Practice question families; prove correct, incorrect, malformed and contextual remainders or drawings in independent verifier tests.
4. Run authenticated student Practice and persistence with tenant isolation, preserving prior grades and school adoption behavior.
5. Only after all lessons truly meet the required A–D source/Practice criteria, freeze candidate, run protected full-grade CI, bind verified test/academic evidence, then consider grade-wide merge and owner-confirmed Render UAT.

**Accounting:** Content authoring reached ordinal 149; technical lesson closure remains 100/149. These are different numbers and must never be conflated.
