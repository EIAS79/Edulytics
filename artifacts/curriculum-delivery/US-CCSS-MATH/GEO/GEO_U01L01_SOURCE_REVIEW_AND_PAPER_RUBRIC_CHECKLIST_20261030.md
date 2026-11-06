# GEO U01:L01 — independent academic adjudication checklist (30 Oct 2026)

**Lesson:** `PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U01:L01` — Build It. **PR:** #583. **Current code predecessor:** `af4fe6207becd6e73bfe369e4b29473e17e38066`.

**Purpose:** evidence for independent reviewer, **NOT** a completed academic signoff. Publisher 2021 locked Cool-down has not been accessed. Existing first-party candidate text, all three worked examples, older source comparisons, and 124 lesson records are preserved.

## Original local source consulted (no online book download)
- Teacher prep: `C:\Users\khali\Downloads\New folder (2)\Common Core\10_HS_Geometry\1\1\preparation.html`, “Learning Goals”, “Learning Targets”, “CCSS Standards”.
- Teacher activities: `C:\Users\khali\Downloads\New folder (2)\Common Core\10_HS_Geometry\1\1\index.html`, §§1.1–1.3 and synthesis.
- Student activities: `C:\Users\khali\Downloads\New folder (2)\Common Core\10_HS_Geometry_Student\1\1\index.html`, §§1.1–1.3.
- Student practice: `C:\Users\khali\Downloads\New folder (2)\Common Core\10_HS_Geometry_Student\1\1\practice.html`, problems 1–4.
- Original HTML refers to publisher-hosted figure assets. Their visual equality to our first-party text/diagrams is **NOT** certified; no publisher picture was copied.

## Reviewer decisions — all require independent approval

| Source item | Mathematical / pedagogical test against current first-party candidate | Preliminary disposition | Reviewer approval |
|---|---|---|---|
| 1.1 “The Right Tool” | Distinguish freehand shape from circle with transferred distance PQ; mark A and B, circle-centre/radius and straightedge segment | Source-congruent original text; no student-facing applet equivalence demonstrated | PENDING |
| 1.2 “Illegal Construction Moves” | **Source intentionally estimates midpoint** to produce variable triangles; valid discovery task must preserve this before discussing why `JM=MK` cannot be inferred. Exact arcs are a follow-up repair, not a replacement for discovery | Original first-party counterexample and permitted repair present; explicit activity-fidelity decision required | PENDING |
| 1.3 “Can You Make a Perfect Copy?” | Sketch vs regular constructed hexagon, six equal-radius chords, six equilateral central triangles, 6×60°=360°, learner-generated diagram and comparison | Worked example and additional paper evidence task available; original figure tracing/dragging not recreated | PENDING |
| Practice 1–4 | (1) equal-radius reason, (2) circle-derived equality, (3) half-segment argument, (4) two-circle relative-distance argument | Four **original** teacher-reviewed analogues added, not publisher diagram-identical assessments | PENDING |
| Source standards | Addressing: HSG-CO.A.1, HSG-CO.D.12; Building Towards: HSG-CO.D.12, HSG-CO.D.13 | Roles separately recorded; no silent “all Addressing” mapping | PENDING |
| 2021 publisher Cool-down | Teacher-login-protected; exact wording not locally accessible. First-party exemplar + 0–8 rubric must be accepted as *equivalent*, not identical | `ORIGINAL_2021_COOLDOWN_NOT_VERIFIED; FIRST_PARTY_EQUIVALENT_AUTHORED` | PENDING |

## Geometry independent check

Executed **read-only Node math checks** on Our-CS, 30 Oct 2026: **16 PASS / 0 FAIL / 0 SKIP**, process exit 0. Checks: circle transfer `JL=JK=8`; estimated midpoint at 3 on segment length 8 yields unequal 3 and 5; two equal-radius arcs centred 8 apart with radius 5 intersect at (4,±3); strict `r>8/2` requirement (r=4 tangent, r=3 no intersection); six radius-4 chord steps at 60° yield a regular hexagon and angle total 360°. This is an independent, **narrow geometry oracle**, not an execution of the application, renderer, .NET or CI. Historical Geometry test results cannot be transferred to this SHA.

## Delivery/reviewer decision, not an engine demand
- **Permitted proposal:** physical paper construction and written argument, marked by an authorized teacher against the detailed 8-point criteria, if an independent academic reviewer approves paper-mode equivalence.
- **Observed existing general application path:** Homework/Worksheet text responses are saved by `StudentAssessmentDeliveryService.SubmitLearningTaskAsync` to `AssessmentTaskResponse.ResponseText` without automatic numeric grading.
- **Critical limit:** `AssessmentService.SaveStudentResultAsync` is **Exam-only**; `SubmitExamAsync` uses scalar `MathematicsAnswerEquivalence`, therefore cannot be repurposed as an auto-grader for geometry proof/drawing. No validated attachment capture or exact `GEO`-bound graded teacher rubric is demonstrated.
- **Minimum central work:** identify a supported, access-checked lesson-linked assignment/review record or approve paper evidence with verifiable manual teacher rubric record; preserve school/pathway/lesson authorization. Shared owner Supervisor 2 under #453; **not** a blanket request for a new geometry engine. Live persisted submission, teacher grade and two-school negatives stay Stage D.

## Signoff form (unfilled)
- Independent academic reviewer / date / accepted first-party 1.1–1.3 parity: **PENDING**
- Exact original images vs approved first-party paper equivalent: **PENDING**
- Source practice 1–4 analogue coverage: **PENDING**
- First-party Cool-down equivalent approval: **PENDING**
- Paper-based teacher-scored drawing and proof as sufficient assessment mode: **PENDING**
- Lesson-specific C rubric accuracy and assessment modality: **PENDING**

**Decision:** `A_CLOSED=0, B_CLOSED=0, C_PREPARED=1, ABC_READY_FOR_D=0, D_VERIFIED=0`. **BLOCKED: do not advance U01:L02.** No new student data, cross-school tests, merge, deployment or Stage D claim.
