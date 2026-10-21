# G1 ordinal 73 — U04 L08 Different Representations of Tens and Ones — Stage A–C independent content and mathematics review

**Authoring decision:** `READY_FOR_STAGE_D` for reviewed accessible 2021 source mathematics and Edulytics first-party lesson. No .NET/xUnit execution, final CI, actual child lesson acceptance, publication, or publisher Cool-down reproduction is claimed.

## A. Original 2021 teacher/student requirements actually inspected, read-only

The authorized Our-CS original file is `C:\Users\khali\Downloads\New folder (2)\Common Core\01_Grade_1_IM_K5\unit-4\lesson-8\lesson.html`. Previously registered original full HTML SHA-256 `c5a8fdd8bfd16ee332095959eae1b68950aee11abb9f692fea75b11bc045084e` is historical source-register evidence, **not rehashed** in this authoring pass. Original title: **Different Representations of Tens and Ones**, without invented “PLC Activity” suffix.

- Warm-up: **Estimation Exploration: How Many?** Original estimation by looking at the physical image (too-low/about-right/too-high) needs authentic pupil responses and teacher observation, never fabricated from generated arithmetic.
- Activity 1 **Compare Representations of a Collection** (10 min). Original original student work shows Clare's and Han's drawings plus **Kiran “3 ones and 7 tens”** and **Priya `70+3`**. Both number descriptions equal **73**, independent of the physical layout of the illustration. Source figures are not redistributed; real drawing analysis stays teacher-reviewed.
- Activity 2 **Card Sort: Base-ten Representations** (15 min). In-class groups assemble **three cards for each same number**, potentially pictures, `__ tens __ ones` words, expanded additions and numeral cards. Original demonstration distinguishes **4 tens+1 one = 41** from one ten+four ones **14** (the digital card example is independently worded rather than publisher artwork copied). Actual source lesson synthesis presents **J and S both show 3 tens+2 ones =32** while **E shows 2 tens+3 ones =23**; E does NOT match. Separate synthesis number card **95** means **9 tens and 5 ones**.
- Activity 3 **Introduce Grab and Count, Ones Cubes** (15 min). Learners grab loose cube handfuls, partner combines and estimates, physically counts and records estimate/actual. Real counting, partnership and prior estimates are teacher-observed and cannot be certified from a numeric text response.
- Source closes with **Unit 4 Section B Checkpoint (0 minutes)**; publisher-protected detailed question is not available in cached public original. This audit neither claims it was viewed nor uses it as authority for fabricated publisher parity.

## B. Original Edulytics learner content and safe version

In `src/Edulytics.Data/Seeding/CommonCoreG1FullGradeStartingContentCorrections.cs` the preexisting source-goal lesson already covered 73, base-ten cards and estimation. Corrected the learner title by removing the non-original “PLC Activity” suffix; added explicit, **independently authored** conceptual examples and guided steps for Kiran/Priya=73, 41 not14, J/S=32 not E=23, and the number 95. The card game and real cubes remain teacher only.

Bumped content **only for this lesson**, `UnitFourRepresentationsSourceContentVersion=us-ccss-g1-u04-l08-original-student-activities-v7`, and allowed its previous shared `us-ccss-g1-u04-l06-l15-original-student-activities-v6` version through a lesson-source guard. No global version bump on U04 L09–L15; no accepted earlier teaching overwritten.

## C. Digital math and exact response verification

Reused `UsCcssG1FullGradeTensAndOnesQuestions.MatchRepresentations`, exact registered source-goal family `usccss.g1.u04.l08.match.tens_ones_expression`. Original generic modes 0–3 continue matching regular tens/ones with a two-digit numeral. Added separately guarded Edulytics tasks:
- `mode=4` has source Kiran/Priya: `MATCH=YES;TENS=7;ONES=3;NUMBER=73;EXPANDED=70+3`.
- `mode=5` uses first-party wording for the source-style card distinction: `MATCH=NO;FIRST=41;OTHER=14`.
- `mode=6` uses source letter roles: `MATCH=J,S;NUMBER=32;OTHER=23`; claiming J/E match is incorrect.
- `mode=7` uses source 95: `TENS=9;ONES=5;NUMBER=95;EXPANDED=90+5`.
These source-mode parameters are validated as **only** L08 and fixed to known operands so they cannot contaminate L06/L07 or unrelated L09/L10. Persisted response comparison enforces all field values, rejecting missing, false, reversed and extra keys. No automatic grade claims originality of a real card sort, actual estimate, peer collaboration, source illustration observation or teacher judgment.

Added positive/negative/fixed-mode/generated-case tests in `UsCcssG1FullGradeTensAndOnesPracticeTests.cs` and rich-lesson/title/version assertions in `UsCcssG1FullGradeStartingTeachingTests.cs`. All are **written but not executed**, awaiting final whole-grade CI policy.

## Decision and protected boundaries

**A–C source-specific mathematical/first-party authoring is complete; `READY_FOR_STAGE_D`.** This is not publisher card-art reproduction and does not certify an unobserved pupil's estimating, sorting or explaining. Protected original Section B Checkpoint is **unverified**. Stage D must separately check compilation, authenticated student delivery, exact persisted grading, visual rights/accessibility and real teacher participation evidence at the eventual frozen grade SHA. Academic ledger stays **`IN_PROGRESS`**, grade release **`NO_GO`**. Next ordinal **74 (U04 L09)**.
