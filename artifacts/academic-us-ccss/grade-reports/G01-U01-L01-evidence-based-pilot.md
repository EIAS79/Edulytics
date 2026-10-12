# G1 / Unit 01 / Lesson 01 — Count and Add — Internal Evidence Report

**Scope:** `PED:US-CCSS-MATH:G1:U01:L01` only. No lesson 2 or G1 grade certification.
**Policy:** `EDULYTICS-INTERNAL-SOURCE-VERIFIED-CERTIFICATION-V2` — **L1–L6 only, eight review dimensions, no L7/human signature**.
**Repository:** `EIAS79/Edulytics`, [PR #488](https://github.com/EIAS79/Edulytics/pull/488).
**Report revised:** 2026-10-13 (post-certification CI consistency closeout).
**Current internal decision:** `INTERNALLY_CERTIFIED` for tested candidate `23786adefd0962e90a074342267baaa5216dddf5`. All L1–L6 and eight dimensions have source-specific versioned evidence; no human signature or L7. This is NOT external, grade-level or Render production certification.
**Publication:** No Render deploy and no merge into `main` recorded.

## 1. Authoritative sources

| Evidence | Reference |
|---|---|
| First edition, teacher lesson | [IM/Kendall Hunt G1 U1 L1](https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-1/lesson.html) |
| First edition, teacher preparation | [IM preparation](https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-1/preparation.html) |
| Official CCSS standards | [CCSS 1.OA.C.5 and 1.OA.C.6](https://www.thecorestandards.org/Math/Content/1/OA/) |
| Publisher-derived original | `src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g1-phase29-v1.lesson-content-pack.json` |
| Original source pack Git blob | `25cc1603a15fb5ceb862100c44ed1368f957b235` |
| Original lesson canonical-body SHA-256 | `89f1b716ffccfec4511ce19d3a1fb433ea06db8577f3357f00769a030e4a6bb0` |
| Corrected effective-content version | `us-ccss-g1-u01-l01-count-collections-dots-v1` |
| Independently computed effective learner-body SHA-256 | `2555913fbe468d3a3733aa5f3ead3642d7ff46767b952c95566fc62256506680` (**must agree with C# hash assertion on tested candidate**) |

Publisher goal: count and represent collections up to 20, then relate counting dots on two cubes to an addition expression. The lesson **addresses** `1.OA.C.5` and `1.OA.C.6`, while building on K.CC.B for the counting collection. These labels must not be confused with full mastery of all of 1.OA.C.6 in one lesson.

The official page licenses its first-edition text under **CC BY 4.0**, excluding the Illustrative Mathematics name/logo and separately licensed images. This repair copies **no publisher images/logos**; its counter and cube representations are code-authored. The student lesson page links the exact source, attribution and license. No rights are assumed for other editions or `CC BY-NC` mirrors.

## 2. Completed correction carried forward from PR #488

Original learner material lacked a worked counting-and-representing collection up to 20, although the original lesson teaches this in Activity 1. A SHA/source/outcome-guarded, idempotent override fixes this **one** lesson, without rewriting the publisher pack, old lesson IDs or other lessons.

Learner explanation now covers counting 16 counters with a ten-frame `10+6=16`, dot cubes `5+3=8`, another example `4+2=6`, steps for counting each object once, and corrections for double-counting and wrong counting-on. Rich V2 displays a code-authored 16-dot arrangement and a 5-dot/3-dot cube display.

Two exact lesson-scoped Practice families replace the earlier overly broad family route:
- `usccss.g1.u01.l01.count_collection_to20` — cardinality 1–20 and visible counter dots.
- `usccss.g1.u01.l01.add_dot_cubes_to10` — two 1–6 dot faces summing at most 10. No unrelated subtraction.

The answer verifier recomputes numerical results independently from parameters and rejects out-of-range/tampered parameters and incorrect answers. Unit tests exercise both families at Standard/Stretch/Challenge, and check dot counts and exact answers.

## 3. Eight required review dimensions

| Dimension | Evidence | Review decision |
|---|---|---|
| 1. Title/grade/topic | Source and unique `LessonCode`, grade logical level 2 | `SOURCE_REVIEWED` |
| 2. Official standard/provenance | IM 2021 teacher lesson/preparation, CCSS links, SHA and attribution | `SOURCE_REVIEWED` |
| 3. Mathematical correctness | Independent verification of 16=10+6, 5+3=8, 4+2=6; randomized exact family tests | `TESTED` |
| 4. Pedagogy/sequence | Count each object once → represent up to 20 → combine cube dots up to 10 | `SOURCE_REVIEWED` |
| 5. Worked examples | Explicit examples and step-by-step explanations; fingerprint guard | `TESTED` |
| 6. Visuals/representations | Source-aligned first-party counter/ten-frame and dot cubes, Rich V2 model/unit tests | `TESTED` |
| 7. Misconceptions | Skip/recount an object; count initial addend as new; invalid `5+3=7` | `SOURCE_REVIEWED` |
| 8. Questions/answers/grading | Two exact families; positive and negative answer verifier tests; HTTPS grading | `TESTED` (both-family HTTPS and persisted answers passed on candidate `23786adef`; certification ledger accepted) |

Review-decision labels above describe **specific observations**. The canonical ledger records `PASS` statuses only with source/content SHA, evidence IDs, and verification timestamps; the eight review dimensions and six gates were validated by the successful certification-evidence checks.

## 4. L1–L6 gates and outstanding actions

| Gate | State as of this revision | Evidence or exact blocker |
|---|---|---|
| L1 — Source & standards | `PASS` | Original first-edition URLs, standards, license/attribution verified |
| L2 — Mathematics | `PASS` | Exact fingerprint assertion and source-bound math verifiers passed in Mathematics Intelligence Foundation |
| L3 — Instructional quality | `PASS` | The two source activities and revised examples/misconceptions match |
| L4 — Visual examples | `PASS` | First-party Rich V2 diagram model and HTML test |
| L5 — Practice and grading | `PASS` | Both exact families scored correctly through authenticated HTTPS on disposable CI; wrong answers rejected by independent exact verifier tests |
| L6 — Learner delivery | `PASS` | Disposable authenticated staging Chromium desktop/mobile visual checks passed; both source-aligned images and attribution shown, screenshots preserved. Render deployment not tested |

No seventh gate exists; **internal certification follows L1–L6 plus all eight documented dimensions, exact SHA and zero unresolved blockers**. CI acceptance here is a **disposable HTTPS staging rehearsal**, not proof that this unpublished PR exists on Render production or that mobile/desktop browser screenshots were inspected.

## 5. Concrete GitHub Actions evidence

Previous full passing candidate `20094c4f4907b991d82f19ce5327c892d37221a1`:

- [US CCSS Authenticated Academic Acceptance — run 38316222097](https://github.com/EIAS79/Edulytics/actions/runs/38316222097): exact G1 authenticated HTML, proper logical level (2), Rich V2 text/visuals and private answer grading.
- [Mathematics Intelligence Foundation — run 38316222038](https://github.com/EIAS79/Edulytics/actions/runs/38316222038): `math-foundation-tests` and `catalogue-skill-audit` both passed.
- [V2 full product regression — run 38316222158](https://github.com/EIAS79/Edulytics/actions/runs/38316222158): passed.
- [Evaluation Production Gate — run 38316222042](https://github.com/EIAS79/Edulytics/actions/runs/38316222042): passed.
- [Clean PostgreSQL 18 — run 38316222019](https://github.com/EIAS79/Edulytics/actions/runs/38316222019): passed.
- [US CCSS Academic Evidence Safety — run 38316222178](https://github.com/EIAS79/Edulytics/actions/runs/38316222178): passed.
- [Common Core Official Closure — run 38316222080](https://github.com/EIAS79/Edulytics/actions/runs/38316222080): passed.
- [Neon Offline Content Inventory — run 38316222002](https://github.com/EIAS79/Edulytics/actions/runs/38316222002): passed.

**Latest fully rendered candidate:** `23786adefd0962e90a074342267baaa5216dddf5` — [G1 authenticated Chromium and two-family HTTPS run 38317821666](https://github.com/EIAS79/Edulytics/actions/runs/38317821666) PASSED, including desktop (1440×900), mobile (390×844), two correct answers and source attribution. Screenshots: artifact `11776903191`, `g1-u01-l01-rich-v2-chromium-screenshots`. An intermediate `Evaluation Production Gate` run failed on five trailing-whitespace lines in this report. They were corrected before certification. All eight PR-triggered workflows subsequently passed on the certification-record commit `8b04ca7470646634962a4b2d3a9e1b54d2ccf1f5`, including the evaluation and evidence-safety gates.

## 6. Durable execution checkpoint

```yaml
currentGrade: G1
currentCourse: null
currentLesson: PED:US-CCSS-MATH:G1:U01:L01
uniqueGradeLessons: 146
pilotLessonsAttempted: 1
pilotLessonsInternallyCertified: 1
status: INTERNALLY_CERTIFIED
policyId: EDULYTICS-INTERNAL-SOURCE-VERIFIED-CERTIFICATION-V2
contentVersion: us-ccss-g1-u01-l01-count-collections-dots-v1
contentSha256: 2555913fbe468d3a3733aa5f3ead3642d7ff46767b952c95566fc62256506680
lastVerifiedGreenCandidate: 8b04ca7470646634962a4b2d3a9e1b54d2ccf1f5
certifiedSourceCandidateSha: 23786adefd0962e90a074342267baaa5216dddf5
lastCompletedOperation: Registered source-candidate-specific internal certification for one lesson and verified all eight GitHub Actions workflows on certification-record commit 8b04ca7470646634962a4b2d3a9e1b54d2ccf1f5
nextExactOperation: Align PR description and confirm CI on any follow-up documentation commit; merge only after confirming no unapproved production auto-deploy. Keep 1559 other lessons queued.
```

**Only one lesson reviewed in this pilot. No G1 grade certification, PR merge, or Render production deployment is implied.**

## 7. Single-lesson evidence decision

**INTERNALLY_CERTIFIED:** `PED:US-CCSS-MATH:G1:U01:L01` on source candidate `23786adefd0962e90a074342267baaa5216dddf5`. Effective lesson SHA-256: `2555913fbe468d3a3733aa5f3ead3642d7ff46767b952c95566fc62256506680` (independently computed and asserted by .NET).

All six gates L1–L6 and eight review dimensions are PASS with evidence references recorded in the canonical 1560-lesson ledger. [Authenticated student browser and two-family grading](https://github.com/EIAS79/Edulytics/actions/runs/38317821666) passed: Chromium desktop 1440×900, mobile 390×844, 16-count illustration, five-plus-three dot cubes, publisher attribution, correct grading/persistence for both exact question families. Visual artifact ID: `11776903191`. [Math foundation verification](https://github.com/EIAS79/Edulytics/actions/runs/38317821642) passed. Copyright source: [IM/Kendall Hunt first edition](https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-1/lesson.html), licensed CC BY 4.0 (separate third-party images/logos not copied).

**Scope:** exactly 1/1560 internally certified on this candidate, 1559 other lesson records still `QUEUED`. G1 grade remains `GRADE_NO-GO`. This source-SHA-specific internal evidence decision is **not** certification of published Render production, the full grade, or an endorsement by Illustrative Mathematics or Common Core. The PR is not merged by this record.
