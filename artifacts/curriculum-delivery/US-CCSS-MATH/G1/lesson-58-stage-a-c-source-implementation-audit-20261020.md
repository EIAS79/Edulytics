# Grade 1 ordinal 58 — U03 L21 / Center Day 3 — A–C closure evidence

**Lesson:** `PED:US-CCSS-MATH:G1:U03:L21`; original 2021 Kendall Hunt Unit 3 Lesson 21.  
**Authoring status:** `READY_FOR_STAGE_D`, not an assertion that deferred Stage D/.NET/CI passed.

## A — original teacher/student material, center rules and boundaries

Our-CS original source was read-only inspected at
`C:\Users\khali\Downloads\New folder (2)\Common Core\01_Grade_1_IM_K5\unit-3\lesson-21\lesson.html`.
Previously recorded original full teacher HTML SHA-256: `26cd4b1a20f8958d83c62453efc87429f3244e3f7a8b61a4a8e7039ac5bfffe4`; it is not newly rehashed in this authoring pass. The original cached center source files were also read:
`center--how-close-1-5\center.html` and `center--five-in-a-row-addition-and-subtraction-1-2\center.html`, in the same Grade 1 root directory.

Original source preparation: https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-21/preparation.html  
Original public lesson: https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-21/lesson.html

- Warm-up (10 min): open-ended **What do you know about 20?** The teacher asks students for representations, not a single auto-graded predetermined answer. Existing Rich V2 already preserves examples `10+10=20`, `13+7=20` and two filled ten-frames.
- Activity 1 (20 min), **How Close? Stage 1, Add to 20**: original number cards **0–10** are supplied, *all 10-value cards removed* for this game, five cards displayed, students select **two or three** of them to write an equation with a sum close to 20, and teacher/partners compare rounds and scores. The original center overview describes selecting three; this exact lesson explicitly allows two or three. The lesson controls this lesson's teaching.
- Activity 2 (20 min), **Five in a Row, Stage 3**: draw a number card **0–10** (not 1–11), choose to add **7, 8, or 9**, mark its exact sum on the gameboard, first to five markers in a row wins. The center's original general game guide confirms the same card range and mechanic.
- The required mathematical gradeable subskill is numeric two-/three-addend addition within 20; the five displayed cards, card selection strategy, all physical counter placement, board layout, winner/partner turns and pupils' actual verbal explanations are **teacher-facilitated**, not certified by digital Practice.
- The scanned teacher HTML and original center rules establish the needed representation/task semantics; no copyrighted gameboard or source art is copied. No lesson-specific cool-down task is listed in this source lesson.

## B — original Rich V2 teaching, repaired only where required

Existing first-party Rich V2 lesson 58 narrative, classroom game boundaries, learner goals, warm-up and sample sums were preserved in
`src/Edulytics.Data/Seeding/CommonCoreG1FullGradeStartingContentCorrections.cs`.

Verified mismatch: the original center requires digit cards **0–10** for Five in a Row, while the registered Practice generator accepted a non-existent **11** and omitted **0**. Updated the learner teaching/source instructions to state both real card sets explicitly: How Close uses 0–9 **after removing 10**, Five in a Row uses 0–10 **without an 11 card**. The scoped content version advanced from
`us-ccss-g1-u03-l21-centers-2021-source-v6` to `...-v7`. The v6→v7 persisted upgrade is guarded by exact lesson identity; no unrelated lesson is reauthored.

Source-content tests assert the exact lesson-specific version, both card domains and original warm-up; tests are written, not yet executed.

## C — real numeric Practice and independent equation validation

Reused `usccss.g1.u03.l21.center.two_three_addends` implemented in
`src/Edulytics.Services/Mathematics/UsCcssG1FullGradeNextQuestions.cs`.
The mathematical-only generator/verifier were reviewed against the original sources:
- `mode=3` Five in a Row uses one source card **0..10** with one addend **7..9**, computed sum **7..19** (no 11-card or impossible sum of 20 from 11+9).
- `mode=0,1,2` How Close numeric two-/three-card problems use original allowable card values **0..9**, non-negative numeric answers with sum at most 20 as the designated digitally gradable subskill. An original classroom game round can have selection/winning context beyond this bounded numeric subskill; this digital contract **does not certify game strategy, card inventory, or closest-sum winner**.
- `VerifySourceGroupEquation` accepts correct numeric sum with exactly the selected card values irrespective of commutative order, rejecting omitted/replaced operands, wrong sums, malformed answer objects and duplicate keys.
- The independent source test `OriginalHowCloseAndFiveInARowStayDifferentSourceGames` in `tests/Edulytics.Tests/MathematicsIntelligence/UsCcssG1FullGradeNextPracticeTests.cs` now asserts valid source card edges 0 and 10, rejects 11 and the invalid 6-addend in Five in a Row, permits 0/9 in How Close and rejects How Close's forbidden 10, and reviews generated digit bounds and correct/wrong exact-equation verification.

## A–C outcome and deferred final verification

**Known A/B/C source-teaching-numeric practice gaps: NONE** after the targeted code repairs and independent source audit. Mark this lesson `READY_FOR_STAGE_D` and remove it from the A–C rework queue. No arbitrary publisher illustration pixel parity or fabricated classroom observations are claimed.

**Not run yet:** final SHA .NET tests, CI, authenticated learner/teacher rendering, real grading + saved attempts, rights/accessibility/security checks. These are Stage D and later academic release requirements, deferred by the owner until the whole grade code/content freeze. This lesson is still `IN_PROGRESS` for academic acceptance; no merge, release or production UAT claimed.
