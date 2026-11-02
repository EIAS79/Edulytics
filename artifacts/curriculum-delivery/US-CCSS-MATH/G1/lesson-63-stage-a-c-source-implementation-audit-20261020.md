# G1 ordinal 63 — U03 L26 “What's the Story?” — A–C exact 2021 source and independent math review (2026-10-20)

**Authoring decision:** `READY_FOR_STAGE_D` for source-aligned Edulytics lesson/Practice authoring; **not** original publisher protected checkpoint parity or academic final acceptance.

## A. 2021 original source inspected read-only

Original file on authorized Our-CS: `C:\Users\khali\Downloads\New folder (2)\Common Core\01_Grade_1_IM_K5\unit-3\lesson-26\lesson.html`. Registered previous 2021 source HTML SHA-256 `b8c56d86a6641a27281083c44e709c7ff04482d53bee94a0a51c596478327b01` is cited from the existing register **without rehashing this pass**.

Teacher/student requirements:
- Warm-up number talk `15−10=5`, `15−12=3`, `16−10=6`, `16−13=3`; subtracting 10 and then extra ones is one reasonable method.
- Activity 1 **Solve Related Story Problems**: (a) Elena has **6** counters, collects an unknown number to reach **18**, so `6+□=18`, unknown **12**; (b) Elena starts with **18**, removes an unknown amount, ends at **6**, so `18−□=6`, unknown **12**. Same missing number, distinct physical events and distinct equation forms.
- Activity 2 **More Story Problems**: four original Han pencil stories:
  1. Han initially has an unknown amount, gets **9** and finishes at **15**: `□+9=15`, **6** original.
  2. Han has **15**, gives away unknown amount and ends with **9**: `15−□=9`, **6** removed.
  3. Han starts with **9**, collects unknown amount to reach **15**: `9+□=15`, **6** added.
  4. Han begins with **15**, gives away **9**: `15−9=□`, **6** remaining.
- Original activities involve partner discussions, ten-frames, counter representations, teacher-led group posters and **student explanations**. They are teacher-observed, not certified by exact numeric grading.
- The teacher source ends in **Unit 3, Section D Checkpoint (0 minutes)**. The linked 2021 checkpoint details remain behind publisher authentication. No unavailable 2021 protected text is alleged reviewed or copied. First-party Practice measures the publicly visible original learning outcomes.

## B. First-party teaching, retained and supplemented

Existing math-rich source content already covers both Elena stories and four Han roles. It was inspected and supplemented with a clear four-way comparison of **unknown start, unknown subtraction, unknown new change, unknown final result**, each illustrated by the correct **15,9,6** equation with the correct missing-value position.

Only `StorySourceContentVersion` in `src/Edulytics.Data/Seeding/CommonCoreG1FullGradeStartingContentCorrections.cs` advanced from `us-ccss-g1-u03-l26-original-activities-v6` to `...-v7`, with a source identity and old-version-guarded upgrade. No original artwork copied. Content/migration assertions added to `UsCcssG1FullGradeStartingTeachingTests.cs`.

## C. Independently reviewed numeric Practice and regression tests

`UsCcssG1FullGradeStrategyAndTensQuestions.StoryUnknowns` registered family:
`usccss.g1.u03.l26.story.unknown_positions`.

Previously existing source-specific modes already include Elena's `6+□=18` and `18−□=6`, and Han's first two `□+9=15` and `15−□=9`. The original third and fourth Han story types were not explicitly generated, **confirmed source-to-Practice gap**.

Added **source-restricted** `mode=6` for `9+□=15` with answer `UNKNOWN=6;EQUATION=9+6=15`; **source-restricted** `mode=7` for `15−9=□` with answer `UNKNOWN=6;EQUATION=15-9=6`. Both validate only `a=9,b=6,c=0`. Existing Han source `mode=2` and `mode=4` remain distinct. Parameter guards refuse false mode claims or impossible arithmetic; persisted exact numeric solver rejects malformed answers or an extra field. Generic unrelated result/start/change unknown exercises remain first-party variants and are not mislabeled as an exact original Han task.

Added `Lesson63Original2021HanFourStoryUnknownPositionsAreDistinctAndVerifiable` in `UsCcssG1FullGradeStrategyAndTensPracticeTests.cs`, with positive/negative mathematical oracles for the four source Han positions, both Elena source stories, and seeded original modes. **Tests authored but not executed**. No classroom explanation, teacher assessment, graphic rights or actual persisted student score is claimed verified.

## Closure and remaining gates

Stages **A/B/C** are complete for accessible original student/teacher activities, independent Edulytics instruction, generator and verifier. The **original 0-minute Section D checkpoint remains inaccessible and not represented as copied**. Detailed figure-by-figure original artwork reproduction is neither attempted nor required for a standalone Edulytics assessment; drawings and peer-led storytelling require teacher review.

**`READY_FOR_STAGE_D`; academic ledger `IN_PROGRESS`; release `NO_GO`.** Do not run .NET, CI, authenticated runtime, final-E/F or deployment until whole grade freeze. Next ordinal in strict authoring order **64 — U03 L27**.
