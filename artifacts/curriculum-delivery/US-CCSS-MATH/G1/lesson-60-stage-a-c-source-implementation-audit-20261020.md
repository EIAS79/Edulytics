# Grade 1 ordinal 60 — U03 L23 / Use a Ten to Subtract — source and code review

**Lesson code:** `PED:US-CCSS-MATH:G1:U03:L23`  
**Status:** `A_C_EXPLICIT_GAP` (**not READY_FOR_STAGE_D**, do not jump to lesson 61). A/B/C repairs made; the actual 2021 original five-minute assessment task still requires independent source verification.

## A. Read-only original 2021 source

Original full teacher source read directly from authorized Our-CS **without writing to that device**:
`C:\Users\khali\Downloads\New folder (2)\Common Core\01_Grade_1_IM_K5\unit-3\lesson-23\lesson.html`.
The canonical source register's prior confirmed 2021 teacher HTML SHA-256 is
`102284813648fcb3cf670ad78479e0ee7b4562075544972d9afa5a906d4d1c95`; this pass did not claim to rehash the file.

Original public 2021 source:
https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-23/lesson.html
Teacher preparation:
https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-23/preparation.html

Reviewed actual student/teacher tasks, mathematical representations and instructions:
- Warm-up: `17−7=10`, `17−7−1=9`, `17−8=9`, `17−9=8`.
- Activity 1: **Number Card Subtraction with 10-frames** is the same teen-number/digit-card game from lesson 22. Select a teen value from 11–19 and a subtractor 0–9; show the starting quantity on a double ten-frame, write an equation, choose a round and explain the strategy. Source permits subtraction equations or equivalent missing-addend equations, uses removal/counting-on/decomposition and is partner- and teacher-facilitated.
- Activity 2: students complete and compare **Diego** and **Andre** methods for `15−8=7`. Diego crosses out five extra ones, leaving ten, then removes three more. Andre removes eight from the full ten, leaving two, then combines the remaining five ones to make seven. Follow-up `14−5=9` and `13−6=7` allow students to show either method with drawings, numbers, or words; source figures are ten-frame representations and are paraphrased with first-party descriptions.
- Teacher source warns not to write false equality `13−3=10−3=7`: the intermediate equations must be expressed accurately.
- The **five-minute original cool-down** is titled **Subtract from 14**, but its actual 2021 student task is hidden behind publisher login in the cached original and public 2021 teacher HTML. A later CKMath guide appears to contain a 14-minus-six problem, but **later-edition coincidence is not sufficient to assert original 2021 task identity**. Do not auto-add the guessed assessment, fake source completeness, or mark A–C closed without valid 2021-edition evidence.

## B. Actual learner-source correction already committed

Reused the original first-party lesson content in
`src/Edulytics.Data/Seeding/CommonCoreG1FullGradeStartingContentCorrections.cs`.
Updated teacher-source faithful examples to cover **both Diego and Andre's valid ten-frame methods** and the true 0–9 digit-card range. Explicitly explained that legal subtractions `17−4=13` and `19−0=19` do not cross ten and must not invent a negative ten-remainder. Retained accurate warm-up, original 15−8, 14−5, 13−6 examples. Advanced the lesson-specific materializer content identifier `us-ccss-g1-u03-l23-ten-2021-source-v6` → `...-v7`, with a guarded persisted v6→v7 upgrade that targets only this lesson. Added source-content and migration assertions to
`tests/Edulytics.Tests/MathematicsIntelligence/UsCcssG1FullGradeStartingTeachingTests.cs`.

## C. Independent mathematics code correction and prepared oracles

Reused the registered `usccss.g1.u03.l23.ten.subtract_in_steps` family and existing exact-operand grading. The original family incorrectly excluded subtracting 0 or other no-cross-ten source card cases by enforcing `b >= a−9`.

Corrected its `mode!=2` card choices to teen values 11–19 and subtrahend digits 0–9. Source Diego `mode=2` remains **exactly** `15−8`. For a legal no-cross case the short-answer output is only `DIFFERENCE;EQUATION`, avoiding fabricated negative `REST` values. When the subtraction reaches/crosses ten, the meaningful `TO10;REST;DIFFERENCE;EQUATION` structure is retained. Digital Practice does **not** pretend the pupil used a physical ten-frame or personally performed Andre's method; the teacher-led source activity preserves those explanations.

Added `Lesson60TenFrameNumberCardSubtractionAcceptsNoCrossAndExactOriginalDiegoTasks` to
`tests/Edulytics.Tests/MathematicsIntelligence/UsCcssG1FullGradeNextPracticeTests.cs`.
It covers valid original 15−8, 14−5, 13−6, 17−4, 19−0, positive and false/malformed responses, impossible card values 20 and 10, original Diego mode guard, and multiple generated source-skill instances.

**Do not claim a test PASS:** .NET/CI/authenticated rendering/saved scores are deliberately deferred to the final grade freeze.

## Open A–C source blocker

Resolve the actual **Kendall Hunt 2021, Grade 1 Unit 3 Lesson 23, five-minute `Subtract from 14` cool-down student task**, using an authorized authenticated original 2021 publisher asset, a cached original-edition print copy, or an independent publication explicitly documenting this same **2021 edition**. A later-edition apparent match must remain a source lead, not proof.

After source verification, independently compare it with first-party teaching/Practice, repair any confirmed missing task, and close A/B/C only if the actual source/task requirement has no open defect. Do not switch to lesson 61 while this lesson is not closed. No CI, merge, or publication has been performed.
