# G1 Unit 01 Lesson 01 — Evidence-Based Single-Lesson Pilot (V2)

**Scope:** ONE lesson only — `PED:US-CCSS-MATH:G1:U01:L01` / **Count and Add**.  
**Execution policy:** `EDULYTICS-INTERNAL-SOURCE-VERIFIED-CERTIFICATION-V2`.  
**Date:** 2026-10-12.  
**Baseline `main` commit:** `2b1a1252814f584b81d57e9e6b6d02a0c949d764`.  
**PR:** [#488](https://github.com/EIAS79/Edulytics/pull/488) — *pending CI and merge at creation of this report*.  
**Final decision:** `REWORK_REQUIRED / NOT_CERTIFIED / GRADE_NO-GO`. No L7 internal acceptance has been granted.  
**Scope protection:** No other lesson was intentionally changed. This pilot report is **not** the required final consolidated 146-lesson G1 report.

## 1. Verified source identity and version

| Field | Evidence |
|---|---|
| Publisher/edition | Illustrative Mathematics / Kendall Hunt, IM K–5 Math Grade 1, First Edition (2021) |
| Teacher preparation | [First-edition preparation](https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-1/preparation.html) |
| Full published lesson | [First-edition lesson](https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-1/lesson.html) |
| Official standard | [CCSS Grade 1 Operations and Algebraic Thinking](https://www.thecorestandards.org/Math/Content/1/OA/) |
| Assigned outcomes | `CCSS:1.OA.C.5`, `CCSS:1.OA.C.6`; both listed as addressing in publisher lesson |
| Publisher-derived embedded pack | `src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g1-phase29-v1.lesson-content-pack.json` |
| Source pack Git blob SHA | `25cc1603a15fb5ceb862100c44ed1368f957b235` |
| Original lesson `canonicalBodySha256` | `89f1b716ffccfec4511ce19d3a1fb433ea06db8577f3357f00769a030e4a6bb0` |
| Original pack `sourceSha256` | `2ffdf5bbf28f5f823f7ae841d47d9100e6a99213555a1d963e2cdd20658c1642` |
| Actual materialized content SHA | **PENDING** — do not substitute the source-pack canonical hash for the effective body SHA |
| Licensing | First-edition page carries CC BY 4.0 with branding/trademark and third-party image exclusions. Asset-by-asset rights and attribution in Edulytics are NOT YET verified. The `Student_Public_Mirror` v.360 evidence is **not** an approved first-edition source or a blanket commercial license. |

The first-edition lesson purpose is **count and represent objects and relate counting to addition**. Activity 1 counts collections **up to 20** and depicts those quantities. Activity 2 rolls two dot cubes, finds the total **up to 10** and writes an addition expression. The original lesson teaches strategies through actual objects, drawings, and dot cubes, not just text describing a generic sum.

## 2. Specific defect and guarded correction

**Observed raw source-pack deficiency (before correction):** The embedded learner explanation gives the broad objective, but its only worked example shows `5 + 3 = 8`. The pack has **no worked example of counting and representing a collection up to 20**, despite this being a substantive component of Activity 1. Its steps also start immediately with two addends and omit 1:1 object-counting. This is a **demonstrable gap in embedded lesson prose**; it is **not yet proof of what every student sees after rendering**.

**Pilot repair:** Add a narrowly scoped `CommonCoreG1CountAndAddPilotCorrection` for just `G1:U01:L01`. Keep the publisher-derived pack unchanged. Require source URL, exact canonical SHA, official outcomes, exact original prose or already-reviewed repaired prose, and a distinct effective content version `us-ccss-g1-u01-l01-count-collections-dots-v1`. Refuse unexpected persisted changes.

Revised English learner content explicitly:
1. Counts and represents **16 counters**, showing ten on a ten-frame plus six more; checks `10 + 6 = 16`.
2. Counts cubes showing **5** and **3** dots, then demonstrates `5 + 3 = 8` via counting-on `6, 7, 8`.
3. Adds cubes with **4** and **2** dots, `4 + 2 = 6`, and explains the Add to 10 reroll condition.
4. Fixes step-by-step explanations and one-to-one counting misconceptions.

This is an **instructional content repair**, not a claim that a diagram is present in the browser, that every Practice family is semantically correct, or that the lesson is internally certified.

## 3. Eight-dimension lesson inspection

| Dimension | Review finding | Pilot status |
|---|---|---|
| 1. Title, grade, topic | `Count and Add`, G1/U01/L01 correspond to the publisher lesson | `SOURCE_VERIFIED` |
| 2. CCSS outcomes and source | Publisher **addresses** 1.OA.C.5 and 1.OA.C.6; same in pack. Per-asset licenses unresolved | `SOURCE_VERIFIED / RIGHTS_PENDING` |
| 3. Mathematical correctness | `10 + 6 = 16`, `5 + 3 = 8`, `4 + 2 = 6` independently checked; actual deployed content not checked | `STATIC_EXAMPLES_VERIFIED` |
| 4. Explanation / progression | Original embedded text lacks a worked counting collection; proposed guarded repair restores 1:1 counting → representation → dot-cube addition | `REWORK_IN_PR` |
| 5. Worked examples / solution steps | New count-to-16 and 5+3/4+2 worked examples; guard/unit test submitted, CI verdict pending | `REWORK_IN_PR` |
| 6. Diagrams / representations | Text refers to 10-frame/dot cubes; **no authenticated Rich V2 screen/interactive diagram proof** | `BLOCKED_MISSING_EVIDENCE` |
| 7. Misconceptions | Addresses skipped/double-counted objects, starting addend counted twice, mismatched totals | `REWORK_IN_PR` |
| 8. Questions, generation, answer keys, grading | Engine has general tests, but **exact lesson's generator routes, edge cases, free-form answers and actual grading not certified here** | `BLOCKED_MISSING_EVIDENCE` |

## 4. Gates — V2 internal acceptance

| Gate | Status | Why |
|---|---|---|
| L1: Standards & Provenance | `IN_PROGRESS` | Lesson and official standard mapped; third-party visual rights, actual use/attribution unresolved |
| L2: Mathematical Truth | `IN_PROGRESS` | Static examples arithmetically reviewed; source-bound CI and effective runtime proof pending |
| L3: Instructional Quality | `IN_PROGRESS` | Identified and repaired embedded content gap; final runtime comparison pending |
| L4: Examples & Representations | `BLOCKED` | Publisher-aligned examples authored, but real visual rendering missing |
| L5: Practice & Assessment | `BLOCKED` | No authenticated per-lesson generation, incorrect-answer and rubric acceptance tests here |
| L6: Learner Delivery | `BLOCKED` | No authenticated `Edulytics` Render staging, browser/screenshots or persisted digest on target SHA |
| L7: Internal Evidence Acceptance | `BLOCKED` | L1–L6 incomplete; **no signature required and no internal PASS granted** |

**Policy and schema limitation:** `artifacts/academic-us-ccss/04-lesson-certification-records.json` still has historical V1 human-signature validation. The V2 policy document directs migration before any lesson receives official `INTERNALLY_CERTIFIED`; this pilot does not edit 1,560 historical records or fabricate a L7 status.

## 5. Regression plan and delivery boundary

- C# regression file: `tests/Edulytics.Tests/MathematicsIntelligence/CommonCoreG1CountAndAddPilotCorrectionTests.cs`.
- Covers exact lesson identity/outcomes, corrected text, arithmetic, idempotent materialization, rejection of SHA/source and partial-content drift, versioned existing-body upgrade guard, and conditional Rich V2 composer smoke.
- Implementation files: `src/Edulytics.Data/Seeding/CommonCoreG1CountAndAddPilotCorrection.cs`, `src/Edulytics.Data/Seeding/CanonicalLessonContentMaterializer.cs`, `src/Edulytics.Data/Seeding/MathematicsCanonicalLessonContentSeeder.cs`.
- Actual GitHub Actions outcomes and candidate commit SHA **must be attached/verified separately**. A test source file does not by itself prove test execution.
- No live Render deployment, live Neon changes, school data writes, license inference, or full G1 closure are authorized by this one-lesson test.

## 6. Resumable checkpoint for this pilot

```yaml
policyId: EDULYTICS-INTERNAL-SOURCE-VERIFIED-CERTIFICATION-V2
currentGrade: G1
currentLesson: PED:US-CCSS-MATH:G1:U01:L01
uniqueGradeLessons: 146
pilotLessonsAttempted: 1
pilotLessonsInternallyCertified: 0
existingG1GuardedCorrectionsOnBaseline: 25
pilotCorrectionOnPR: 1
gradeDecision: GRADE_NO-GO
lastCompletedOperation: >
  Compared exact first-edition publisher lesson to the raw embedded G1 L01 body;
  authored SHA-guarded targeted repair and C# regression tests in PR #488.
nextExactOperation: >
  Check PR #488 CI and resolve any failures. Verify actual materialized
  English SHA, lesson SkillContract family behavior, answer grading,
  publisher visual rights, and authenticated Rich V2 staging before L1–L7 PASS.
```

**This pilot ends at this lesson only, by explicit user instruction.** It does not start `G1:U01:L02` or `G2`.
