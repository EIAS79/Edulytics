# G1 / Unit 01 / Lesson 01 — Count and Add — Internal Evidence Report

**Scope:** `PED:US-CCSS-MATH:G1:U01:L01` only. No lesson 2 or G1 grade certification.  
**Policy:** `EDULYTICS-INTERNAL-SOURCE-VERIFIED-CERTIFICATION-V2` — **L1–L6 only, eight review dimensions, no L7/human signature**.  
**Repository:** `EIAS79/Edulytics`, [PR #488](https://github.com/EIAS79/Edulytics/pull/488).  
**Report revised:** 2026-10-13.  
**Current decision:** `NOT_CERTIFIED` pending the final candidate's two-activity HTTPS and content-fingerprint CI results. **Do not mistake earlier green runs or this report for certification.**  
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
| 8. Questions/answers/grading | Two exact families; positive and negative answer verifier tests; HTTPS grading | `TESTED` (both-family HTTPS pending final CI) |

Review-decision labels above describe **specific observations**, not canonical ledger PASS values; final ledger updates require successful candidate SHA checks, time and evidence IDs.

## 4. L1–L6 gates and outstanding actions

| Gate | State as of this revision | Evidence or exact blocker |
|---|---|---|
| L1 — Source & standards | `EVIDENCE_COLLECTED` | Original first-edition URLs, standards, license/attribution verified |
| L2 — Mathematics | `PENDING_SHA_CI` | Source-based mathematical and answer verifiers; exact fingerprint assertion added |
| L3 — Instructional quality | `EVIDENCE_COLLECTED` | The two source activities and revised examples/misconceptions match |
| L4 — Visual examples | `EVIDENCE_COLLECTED` | First-party Rich V2 diagram model and HTML test |
| L5 — Practice and grading | `PENDING_TWO_FAMILY_HTTPS` | Unit positive/negative checks pass on prior candidate; final two-family student attempt added |
| L6 — Learner delivery | `PENDING_FINAL_CANDIDATE_CI` | Prior authenticated disposable HTTPS confirms lesson page, visual markup, protected student access, private practice persistence and grading; final candidate not yet confirmed |

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

**Latest candidate pending:** `029e84ecfda59c15075b3086c8d65410ab330273`, after adding an independent C# fingerprint assertion and a real two-family HTTPS student attempt. The exact latest green runs and SHA must be captured before declaring all L1–L6 `PASS`.

## 6. Durable execution checkpoint

```yaml
currentGrade: G1
currentCourse: null
currentLesson: PED:US-CCSS-MATH:G1:U01:L01
uniqueGradeLessons: 146
pilotLessonsAttempted: 1
pilotLessonsInternallyCertified: 0
status: NOT_CERTIFIED
policyId: EDULYTICS-INTERNAL-SOURCE-VERIFIED-CERTIFICATION-V2
contentVersion: us-ccss-g1-u01-l01-count-collections-dots-v1
contentSha256: 2555913fbe468d3a3733aa5f3ead3642d7ff46767b952c95566fc62256506680
lastVerifiedGreenCandidate: 20094c4f4907b991d82f19ce5327c892d37221a1
pendingEnhancedCandidate: 029e84ecfda59c15075b3086c8d65410ab330273
lastCompletedOperation: All 8 CI workflows green for previous pilot; added fingerprint and both-family HTTPS evidence checks
nextExactOperation: Inspect latest fingerprint and two-family HTTPS CI; if green, update the single canonical lesson ledger with justified L1-L6, eight review dimensions and version-bound evidence. Do not alter other lesson records or claim a production Render deployment.
```

**Only one lesson reviewed in this pilot. No G1 grade certification, PR merge, or Render production deployment is implied.**
