# G1 ordinal 72 — U04 L07 Numbers With Tens and Ones — 2021 accessible source mathematics, first-party teaching and Practice A–C

**Disposition:** `READY_FOR_STAGE_D` **for academically reviewed accessible source tasks and Edulytics-authored assessments**. Not a claimed original protected publisher checkpoint, real child exercise, test run, Stage D or grade release.

## A. Original edition and student/teacher tasks

Read the original 2021 teacher HTML on Our-CS read-only at `C:\Users\khali\Downloads\New folder (2)\Common Core\01_Grade_1_IM_K5\unit-4\lesson-7\lesson.html`. Registered full teacher source SHA-256 `d3c251a9cfaa06ec9e0b71ea89fe3a3bbefe9615dfe4353fa520f6295a0759c9` is prior source-register evidence (not rehashed on this pass). No publisher illustrations copied.

- Warm-up **Notice and Wonder: One- and Two-digit Numbers**. Student-facing Set A includes **0–9**; Set B **10, 23, 45, 76, 89**. Teacher introduces a two-digit number and explains that digits represent **tens and ones**. The digits' order matters in numeral notation.
- Activity 1 **Make It: Tens and Ones** (15 min). Partners select TWO distinct 0–9 number cards, **remove the 10 card** from the source deck, make a two-digit number, say it, and build corresponding ten-rods and single cubes; both draw and record tens/ones. Teacher illustration uses source **3 and 5**: either **35** or **53** is a valid learner choice. A card with 0 cannot be the tens digit for a two-digit number. Teacher synthesis shows **Tyler wrote 24 but represented 4 tens and 2 ones**, which is actually **42**, not 24. Real card handling and pupil drawing are teacher-checked.
- Activity 2 **Who Do You Agree With?** (10 min). A model has **8 full tens and 6 loose ones**, even when the ones appear to the left of the tens. Clare says **68**; Diego says **86**. **Diego is correct**: 80+6=86; placement of rods in a drawing does not change place-value units, but the numeral's digit order does.
- Activity 3 **Centers: Choice Time** (15 min). Physical play/turn taking remains teacher-observed. Final lesson synthesis with six visible tens and unknown ones allows **any number from 60 through 69**; not one prescribed number.
- Original teacher material ends with **Unit 4 Section B Checkpoint (0 min)**. Publisher-protected details remain inaccessible. This audit does not claim its exact original student item or teacher responses.

## B. Source-aligned first-party lesson text

Reused rich Edulytics source-informed U04 L07 lesson in `src/Edulytics.Data/Seeding/CommonCoreG1FullGradeStartingContentCorrections.cs`. Added explicit pupil-facing reasoning for the 3/5 source card choice, two viable card orders, Diego's **86**, Tyler's **42**, and the **60–69** hidden-ones range. Explicitly distinguished legitimate alternatives, two-digit versus single-digit card ordering, numerical place-value from the physical placement of cubes, and teacher-only student reading/model-building.

Changed **only U04 L07's** persisted content version to `us-ccss-g1-u04-l07-original-student-activities-v7` using a guarded previous `us-ccss-g1-u04-l06-l15-original-student-activities-v6` identity/version upgrade. Existing U04 L06 v7 stays unchanged; U04 L08–L15 still remain on the shared old version pending their own A–C audits.

## C. Independent digital math and scoring boundary

Reused `UsCcssG1FullGradeTensAndOnesQuestions.ReadNumber`, existing `usccss.g1.u04.l07.read.two_digit` family. Preserved generic number-naming mode 0–3, and added strict, source-goal-specific **Edulytics independently authored** modes:
- `mode=4`: source demonstration of cards **3,5**, answer may be `DIGITS=3,5;NUMBER=35;TENS=3;ONES=5;NAME=thirty-five` **OR** `DIGITS=3,5;NUMBER=53;TENS=5;ONES=3;NAME=fifty-three`. **Both alternatives are accepted by actual parameterized `Verify` and by `GradePersistedAnswer`**. Swapped tens/ones not consistent with the selected numeral or mismatched name must be rejected.
- `mode=5`: Diego **86** vs Clare **68**, `AGREE=DIEGO;TENS=8;ONES=6;NUMBER=86;NAME=eighty-six`; false choice rejected.
- `mode=6`: Tyler's represented **42**, not written 24: `DECISION=DISAGREE;DRAWN=42;CLAIM=24;TENS=4;ONES=2`.
- Source-specific parameters are fixed and `Valid` forbids injecting these modes into L06 or unrelated L08–L10 families. The first-party digital contract grades values and numeric judgments, NOT teacher-observed cards, drawing, oral reading or group participation.

Added source-specific wrong/right/persisted-answer oracles in `UsCcssG1FullGradeTensAndOnesPracticeTests.cs`, and teaching/migration assertions in `UsCcssG1FullGradeStartingTeachingTests.cs`. **Neither .NET tests nor final authenticated runtime was run.**

## Closure

**A–C teaching/academic math source-goal authoring CLOSED**; the protected original checkpoint is unverified, and digitally proving pupil oral reading/physical activities is not claimed. `READY_FOR_STAGE_D` for this existing original Edulytics implementation. **Ledger `IN_PROGRESS`; release `NO_GO`.** .NET/CI, persistent child grading, teacher verification, trademark/artwork/accessibility and final grade acceptance are deferred to whole-grade freeze. Next ordinal **73 — U04 L08**.
