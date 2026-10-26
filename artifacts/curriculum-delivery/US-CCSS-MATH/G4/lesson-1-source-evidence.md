# Grade 4 L01 (ordinal 1): Original-source evidence and narrow teaching repair

**Result:** `A_C_EXPLICIT_GAP`. NOT closed or Stage D ready. Base `8431f9afe4a7207f4df3d53a3c800d2ed7532fad`; work branch `curriculum/g4-sequential-abc-20261026`. **Stop here until evidence gates pass.**

## Read locally on authorized Our-CS
* First-edition 2021 teacher preparation `unit-1/lesson-1/preparation.html`, SHA256 `99f65b13229ca30d502ac323d86b49da08607e0b9c054f3e1772255daf35f138`: *Multiples of a Number*, 4.OA.B.4; use rectangles, widths, multiples; inch tiles, centimeter grid paper; 36 tiles per pair.
* First-edition 2021 teacher `unit-1/lesson-1/lesson.html`, SHA256 `b5a8cfbbd20d70cb98616a6a4ea5d628aac3ad5b679eb0330b0fd229e3f52a63`: warm-up four figures **Which One Doesn't Belong**; Activity 1 five distinct rectangles for each width 2, 3, 4 with area table/prediction; Activity 2 Elena width=3, area<=30, explain 28 impossible, explore areas >30 and impossible areas; lesson synthesis 28 a multiple of 4; independently protected Cool-down.
* `accessim_student/unit-1/section-a/lesson-1.html`, SHA256 `1ff4d198c44514df48e063283fbab46ecd93fed429e1a610d191e8c9c9c7cf61` is **not verified as the original 2021 task PDF**, and instead labels warm-up *Which Three Go Together*. Do not silently conflate editions. Images (including figure labels) have not been audited individually.
* Directory `04_Grade_4_IM_K5` includes **0 local PDF files**. Preparation advertises Student Task Statements PDF; its original content was not read. Teacher cool-down requires sign-in. Neither can be claimed complete.

## Item-by-item comparison
| Element | Observation / remedy | Classification |
|---|---|---|
| TITLE | Pack/Blueprint contained erroneous suffix `PLC Activity`; both corrected to source title *Multiples of a Number* | CORRECTED_PENDING_RUNTIME |
| GOALS | Existing grade3-area prerequisite and multiples concept retained; fixed confusing "your area" wording, removed generic geometry classifications | CORRECTED_PENDING_RUNTIME |
| WARM-UP | Figures A–D and original 2021 warm-up not reproduced/verified; newer student mirror differs | SOURCE_CONFLICT |
| ACTIVITY 1 | Five rectangles per width 2/3/4, area table and oral observations now explained mathematically; physical/visual student completion unverified | MISMATCH_BLOCKED |
| ACTIVITY 2 | Width 3 with maximum 30; possible areas 3..30 multiples of 3, rejects 28; examples 33/36 and rejected 34 above 30. Original drawing and assessment flow unverified | MISMATCH_BLOCKED |
| COOL-DOWN / PDF | Protected cool-down and original source-task PDF missing from reviewed local archive | SOURCE_UNAVAILABLE |
| PRACTICE / DISPLAY | No independent student-authenticated Rich V2, skill-family, grader, image or teacher-reviewed evidence | MISMATCH_BLOCKED |

## Implementation and checks
Corrected **only G4 U01 L01** in existing Grade 4 content pack and Blueprint, preserving rest of 149 lessons and all standards, IDs and publisher-provenance hashes; original source fingerprints are not asserted to be hashes of new authored text. This is a first-party pedagogical explanation repair, not republication of publisher figures.

Authored focused `tests/academic-us-ccss-g4-u01-l01-source-guard.test.cjs`, **WRITTEN_NOT_RUN**. Targeted in-memory checks passed: valid JSON, 149 lessons, unchanged content of ordinals 2–149, stable lesson code, source title, and width-3 arithmetic checks; **runtime / .NET / CI NOT_RUN**. No main merge, production DB mutation, Render deploy or school demo changes.

**Sequential cursor stays at ordinal 1.** Do NOT advance to ordinal 2 or report 1/50 closed without original-edition student/PDF/visual, teacher/manual workflow and Practice/grade/render evidence.


## 2026-10-26 continuation: original PDF located using existing local preparation HTML
- Not found by scanning folder for files because no `.pdf` is stored there. The archived 2021 preparation HTML contains its **direct original 3-page Student Task Statements PDF** href: `https://cms-assets.illustrativemathematics.org/8qvojxum833zkwf43q6ahvral1r4`. Retrieved on authorized Our-CS, SHA256 `b5f8272ce99b0579280376b0ba0b89800599cbf8b5765150ea2e83ee83e43a4a`. **All 3 pages' text was reviewed**. Temporary PDF copy is not committed to GitHub.
- Page 1: warm-up **Which One Doesn't Belong: All Kinds of Area**, with original labeled A/B/C/D source figures. Figures' image geometry and licensing have NOT been individually inspected/reproduced.
- Page 2: `1.1 Build Rectangles and Find Area`: 5 different rectangles for EACH width 2, 3 and 4 tiles; table of areas; partner row observations; predictions for each width with explanations.
- Page 3: `1.2 What Areas Can You Build?`: Elena's width-3, area ≤ 30 construction/drawings and labels, list areas, note pattern; separately explain 28 impossibility, find 2 possible above 30 and one other impossible area.
- NEW first-party `src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g4-u01-l01-2021.rich-lesson-v2.json` authored and embedded via the existing `Edulytics.Core.csproj` RichV2 glob. It distinguishes publisher-backed TASK requirements, original Edulytics mathematics explanations and teacher-led / machine-graded scope, preserves the original task subparts, avoids claiming a copied figure or verified original Cool-down.
- The previously listed absence of original Student Task Statements PDF is **resolved**. Remaining original cool-down/figure and applied student/teacher assessment coverage gaps are **not resolved**; therefore ordinal 1 stays `A_C_EXPLICIT_GAP`, no advance to ordinal 2.
- The separate `node:test` source guard was extended to assert this 2021 first-party Rich V2 sidecar is well structured, reads all three source-task sections, records distinct tasks, and does not misrepresent 28 with width 3. Per Stage A–B–C, automated test suite itself is **WRITTEN_NOT_RUN**; in-memory structural assertions were run before committing.
