# US-CCSS Phase 04 — prioritized independent lesson review queue

**Status: NO-GO — 0/1,560 independently reviewed.** This queue is an
engineering triage artifact, not a source-based academic sign-off.

| Priority | Lesson count | Reason |
|---|---:|---|
| P0 | 1 | Original source shows explicit skill/procedure conflict |
| P1 | 19 | Original source has encoded HTML entities; verify actual learner presentation before editing |
| P2 | 1400 | Identical long step procedures shared by four or more lessons; manual specificity review |
| P3 | 140 | No current machine flag; still requires academic lesson review |

These categories are **review priorities, not defect counts or approvals**.
P0 is the historical G4 U01 L01 raw-source mismatch: a guarded correction
candidate exists on PR #464, but this does not establish deployed content,
real-user presentation, independent sign-off, or full lesson certification.
P1 includes source encoding warnings even where learner UI may already decode
entities. Do not rewrite canonical source or alter ContentVersion just to
eliminate these warnings.

## Immediate P0/P1 reviewer queue

| Priority | Lesson | Publisher reference | Required next check |
|---|---|---|---|
| P0 | `PED:US-CCSS-MATH:G4:U01:L01` | [Source](https://im.kendallhunt.com/k5/teachers/grade-4/unit-1/lesson-1/preparation.html) | Verify guarded runtime correction, student-facing render and independent academic review |
| P1 | `PED:US-CCSS-MATH:G2:U04:L03` | [Source](https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-3/preparation.html) | Inspect source notation vs rendered text; do not assume learner-visible failure |
| P1 | `PED:US-CCSS-MATH:G2:U06:L19` | [Source](https://im.kendallhunt.com/k5/teachers/grade-2/unit-6/lesson-19/preparation.html) | Inspect source notation vs rendered text; do not assume learner-visible failure |
| P1 | `PED:US-CCSS-MATH:G2:U09:L04` | [Source](https://im.kendallhunt.com/k5/teachers/grade-2/unit-9/lesson-4/preparation.html) | Inspect source notation vs rendered text; do not assume learner-visible failure |
| P1 | `PED:US-CCSS-MATH:G4:U02:L03` | [Source](https://im.kendallhunt.com/k5/teachers/grade-4/unit-2/lesson-3/preparation.html) | Inspect source notation vs rendered text; do not assume learner-visible failure |
| P1 | `PED:US-CCSS-MATH:G4:U02:L17` | [Source](https://im.kendallhunt.com/k5/teachers/grade-4/unit-2/lesson-17/preparation.html) | Inspect source notation vs rendered text; do not assume learner-visible failure |
| P1 | `PED:US-CCSS-MATH:G4:U05:L04` | [Source](https://im.kendallhunt.com/k5/teachers/grade-4/unit-5/lesson-4/preparation.html) | Inspect source notation vs rendered text; do not assume learner-visible failure |
| P1 | `PED:US-CCSS-MATH:G4:U06:L01` | [Source](https://im.kendallhunt.com/k5/teachers/grade-4/unit-6/lesson-1/preparation.html) | Inspect source notation vs rendered text; do not assume learner-visible failure |
| P1 | `PED:US-CCSS-MATH:G4:U06:L22` | [Source](https://im.kendallhunt.com/k5/teachers/grade-4/unit-6/lesson-22/preparation.html) | Inspect source notation vs rendered text; do not assume learner-visible failure |
| P1 | `PED:US-CCSS-MATH:G4:U06:L25` | [Source](https://im.kendallhunt.com/k5/teachers/grade-4/unit-6/lesson-25/preparation.html) | Inspect source notation vs rendered text; do not assume learner-visible failure |
| P1 | `PED:US-CCSS-MATH:G4:U07:L04` | [Source](https://im.kendallhunt.com/k5/teachers/grade-4/unit-7/lesson-4/preparation.html) | Inspect source notation vs rendered text; do not assume learner-visible failure |
| P1 | `PED:US-CCSS-MATH:G4:U07:L14` | [Source](https://im.kendallhunt.com/k5/teachers/grade-4/unit-7/lesson-14/preparation.html) | Inspect source notation vs rendered text; do not assume learner-visible failure |
| P1 | `PED:US-CCSS-MATH:G5:U01:L10` | [Source](https://im.kendallhunt.com/k5/teachers/grade-5/unit-1/lesson-10/preparation.html) | Inspect source notation vs rendered text; do not assume learner-visible failure |
| P1 | `PED:US-CCSS-MATH:G5:U04:L14` | [Source](https://im.kendallhunt.com/k5/teachers/grade-5/unit-4/lesson-14/preparation.html) | Inspect source notation vs rendered text; do not assume learner-visible failure |
| P1 | `PED:US-CCSS-MATH:G5:U05:L11` | [Source](https://im.kendallhunt.com/k5/teachers/grade-5/unit-5/lesson-11/preparation.html) | Inspect source notation vs rendered text; do not assume learner-visible failure |
| P1 | `PED:US-CCSS-MATH:G5:U05:L25` | [Source](https://im.kendallhunt.com/k5/teachers/grade-5/unit-5/lesson-25/preparation.html) | Inspect source notation vs rendered text; do not assume learner-visible failure |
| P1 | `PED:US-CCSS-MATH:G5:U06:L08` | [Source](https://im.kendallhunt.com/k5/teachers/grade-5/unit-6/lesson-8/preparation.html) | Inspect source notation vs rendered text; do not assume learner-visible failure |
| P1 | `PED:US-CCSS-MATH:G5:U08:L02` | [Source](https://im.kendallhunt.com/k5/teachers/grade-5/unit-8/lesson-2/preparation.html) | Inspect source notation vs rendered text; do not assume learner-visible failure |
| P1 | `PED:US-CCSS-MATH:G5:U08:L05` | [Source](https://im.kendallhunt.com/k5/teachers/grade-5/unit-8/lesson-5/preparation.html) | Inspect source notation vs rendered text; do not assume learner-visible failure |
| P1 | `PED:US-CCSS-MATH:G5:U08:L09` | [Source](https://im.kendallhunt.com/k5/teachers/grade-5/unit-8/lesson-9/preparation.html) | Inspect source notation vs rendered text; do not assume learner-visible failure |

## Remaining review queue

- All 1,560 individual lessons, including P2 and P3, are enumerated in
  `04-prioritized-lesson-review-queue.csv` with source file, publisher URL,
  raw-source flags and review status.
- The exact repeated-procedure population is re-derived from canonical source
  strings and checked against the existing 1,420-lesson source audit.
- A reviewer must validate content, examples, representations, exact skill
  alignment, question-family suitability, math correctness, and provenance.
- No row is approved automatically. Human sign-off and a candidate-SHA
  release check remain mandatory before declaring any phase gate PASS.

Source baseline SHA: `fd72849823ae31b92e58fb420a373218fc25b328`.
