# Grade 1 ordinal 70 — Unit 4 Lesson 5 Center Day 1 — A–C source/learner/Practice audit

**Status:** `READY_FOR_STAGE_D` only for math/first-party academic authoring; actual classroom games, drawings, authenticated student grading and protected GitHub CI are not claimed.

## A. Original 2021 source, authorized Our-CS read-only inspection

Original cached teacher file `C:\Users\khali\Downloads\New folder (2)\Common Core\01_Grade_1_IM_K5\unit-4\lesson-5\lesson.html`; prior registered full teacher SHA-256 `863d835c8dbd398118116f21cb46c854f6f218ef9897b34302710d3ab00ea245` (previous registry, not recalculated now).

- Warmup **Which One Doesn’t Belong: Tens** (10 min): four pictures showing groups of ten and teacher-facilitated explanations. Source image content and real spoken claims are not invented.
- Activity 1 **Introduce Check It Off, Add or Subtract Tens** (15 min): teacher explicitly calls it **Stage 2**. Players choose two multiple-of-ten numeral cards from **0 through 90**, decide to add or subtract, mark a value within **0–90** on the game sheet and record the expression. Partner card draws, board state and winning counts are teacher-observed. Original synthesis shows **40 and 50** with **90 already marked** and **10 unmarked**: choose subtraction **50−40=10**, not 40+50=90, to cover an available value. A linked source material line happens to be labelled “Check It Off Stage 3 Recording Sheet”, but it does **not** change the stated Activity 1 **Stage 2 game** into Stage 3.
- Activity 2 **Centers: Choice Time** (25 min): students select previously introduced centers including Five in a Row. Authentic physical and partner participation cannot be certified from math scores.
- No separate source five-minute Cool-down exercise is identified on this public lesson HTML; do not invent one.

## B. First-party learner lesson / old content migration

Reused `src/Edulytics.Data/Seeding/CommonCoreG1FullGradeStartingContentCorrections.cs` rich lesson. Added explicit independent card examples involving **zero** and **ninety**, explained true Stage 2 board range and original 40/50 game-state condition, and preserved the teacher boundary for warmup interpretation and gameplay. Source-identified lesson version advances only from `us-ccss-g1-u04-l05-original-activities-v6` to `...-v7` with guarded saved-content upgrade.

## C. Numeric generator and exact verifier review

`UsCcssG1FullGradeStrategyAndTensQuestions.CenterTens`, registered `usccss.g1.u04.l05.center.tens_and_within20`:
- The previous generic numeric modes 0 and 1 incorrectly restricted card operands to **two–seven tens** and usually required a positive second card, excluding valid card **0** and **90**. Mode 0 now accepts `a,b∈[0,9]` and `a+b≤9`; mode 1 accepts `0≤b≤a≤9`. Source card values are tens; corresponding checked sums/differences stay in the actual board's 0–90 numeric domain. These generic numeric tasks do **not** claim a specific hand was physically dealt.
- Preserved the previously source-exact `mode=4`: 40/50 with 90 already checked, 10 available, answer `MOVE=SUBTRACT;COVER=10;EQUATION=50-40=10`. `mode=5` preserves a separate source-goal “available addition box” example. All other family modes and accepted older lessons are left intact.
- Numeric verifier rejects an invalid 100 board result, negative subtraction, wrong available-box choice, missing required answer fields and extra inserted fields.
- New `Lesson70CheckItOffZeroAndNinetyRemainWithinOriginalBoardLimits` asserts 0+30=30, 90+0=90, 90−10=80, 80+20 forbidden on 0–90 board, 0−10 forbidden, exact 40/50 actual source game-state choice and generated card limits. Source teaching/version checks authored in `UsCcssG1FullGradeStartingTeachingTests.cs`. Actual xUnit/CI **not executed**.

**A–C mathematical first-party lesson authoring reviewed and CLOSED.** Teacher-observed speech, real board and games, source artwork, rights, accessibility, real pupil/teacher persistence and all Stage D/E acceptance gates remain **pending**. Grade ledger `IN_PROGRESS`, release `NO_GO`. Resume strict sequence at ordinal **71 (U04 L06)**; do not manually run CI before whole-grade freeze.
