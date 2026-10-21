# Grade 1 ordinal 60 — U03 L23 / Use a Ten to Subtract — source and code review

**Lesson code:** `PED:US-CCSS-MATH:G1:U03:L23`
**Status:** `READY_FOR_STAGE_D` for **Edulytics independently authored academic A–C content** under the owner's explicit October 20 approval; **not** a claim of verbatim 2021 publisher Cool-down parity, D validation or 96/96 acceptance.

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
- The publisher's original **five-minute Cool-down** is titled **Subtract from 14** in the cached 2021 teacher HTML, but its actual authenticated task remains **unavailable and unverified**. Later-edition tasks are NOT treated as 2021 proof. The Edulytics owner expressly authorized designing and accepting an **independent, source-goal-aligned formative assessment** rather than copying the protected publisher question. This approval changes the Edulytics authoring requirement for this one inaccessible assessment; it does NOT establish or claim original protected-question parity.

## B. Actual learner-source correction already committed

Reused the original first-party lesson content in
`src/Edulytics.Data/Seeding/CommonCoreG1FullGradeStartingContentCorrections.cs`.
Updated teacher-source faithful examples to cover **both Diego and Andre's valid ten-frame methods** and the true 0–9 digit-card range. Explicitly explained that legal subtractions `17−4=13` and `19−0=19` do not cross ten and must not invent a negative ten-remainder. Retained accurate warm-up, original 15−8, 14−5, 13−6 examples. Advanced the lesson-specific materializer content identifier `us-ccss-g1-u03-l23-ten-2021-source-v6` → `...-v7`, with a guarded persisted v6→v7 upgrade that targets only this lesson. Added source-content and migration assertions to
`tests/Edulytics.Tests/MathematicsIntelligence/UsCcssG1FullGradeStartingTeachingTests.cs`.

## C. Independent mathematics code correction and prepared oracles

Reused the registered `usccss.g1.u03.l23.ten.subtract_in_steps` family and existing exact-operand grading. The original family incorrectly excluded subtracting 0 or other no-cross-ten source card cases by enforcing `b >= a−9`.

Corrected its `mode!=2` card choices to teen values 11–19 and subtrahend digits 0–9. Source Diego `mode=2` remains **exactly** `15−8`. For a legal no-cross case the short-answer output is only `DIFFERENCE;EQUATION`, avoiding fabricated negative `REST` values. When the subtraction reaches/crosses ten, the meaningful `TO10;REST;DIFFERENCE;EQUATION` structure is retained. Digital Practice does **not** pretend the pupil used a physical ten-frame or personally performed Andre's method; the teacher-led source activity preserves those explanations.

The actual `mode=3` generator was separately hardened to select **only** the two source-authored follow-ups `14−5=9` and `13−6=7`. Its prompt now specifically names Diego's and Andre's methods; mode 3 refuses unrelated operands rather than relabelling them as original student tasks. Positive/negative xUnit assertions cover both deterministic seeded source instances and rejection of a mismatched original-student mode. No new runtime test execution is claimed.

Added `Lesson60TenFrameNumberCardSubtractionAcceptsNoCrossAndExactOriginalDiegoTasks` to
`tests/Edulytics.Tests/MathematicsIntelligence/UsCcssG1FullGradeNextPracticeTests.cs`.
It covers valid original 15−8, 14−5, 13−6, 17−4, 19−0, positive and false/malformed responses, impossible card values 20 and 10, original Diego mode guard, and multiple generated source-skill instances.

**Do not claim a test PASS:** .NET/CI/authenticated rendering/saved scores are deliberately deferred to the final grade freeze.

## Owner-authorized independent Cool-down, mathematical and academic review

The owner explicitly confirmed on October 20 that **Edulytics is not copying the source curriculum**: when the lesson learning goal and teaching strategy are established, an independently designed assessment that measures the goal can be authored and approved. This is a scoped content choice for the inaccessible original Cool-down, **not** permission to claim we viewed or copied the protected 2021 assessment.

**Edulytics student task** (authored here, not taken from the original publisher): represent **14** counters with two ten-frames, remove **8**, demonstrate the intermediate ten and the remaining removal, report how many remain, and explain a valid ten strategy using drawings, numbers, or words. The independently reviewable numeric response contract is:

- `14−8=6`, verified by `6+8=14`.
- First remove the **4** loose ones to reach **10**; then remove the remaining **4** to get **6**.
- Exact numeric machine-checkable answer: `TO10=4;REST=4;DIFFERENCE=6;EQUATION=14-8=6`.
- This tests **both the correct difference and the intermediate strategy**; a bare `DIFFERENCE=6` does not prove the use of a ten. Hand-drawn explanations or a distinct mathematically valid method are supported for teacher review, not falsely auto-graded as the same structured numeric method.
- Wrong stage `TO10=5;REST=3`, a wrong difference, a miscopied equation, malformed input and duplicate keys must be rejected.
- The new `mode=4` is restricted to **this lesson's** existing `usccss.g1.u03.l23.ten.subtract_in_steps` family with `a=14,b=8,c=0`. Other original 2021 lesson modes remain distinct and unchanged. A random lesson practice session can generate this independent formative task without claiming it was the original 2021 question.

**Actual scoped code and test files:**

- `src/Edulytics.Services/Mathematics/UsCcssG1FullGradeNextQuestions.cs`: fifth dedicated mode and arithmetic verifier using the existing exact score contract.
- `src/Edulytics.Data/Seeding/CommonCoreG1FullGradeStartingContentCorrections.cs`: learner teaching includes this clearly labelled independent Cool-down; persisted L60 content version moves from `...-v7` to `us-ccss-g1-u03-l23-ten-2021-source-v8`, and allows the guarded L60-only v6/v7 upgrade.
- `tests/Edulytics.Tests/MathematicsIntelligence/UsCcssG1FullGradeNextPracticeTests.cs`: independent correct/wrong/strategy and deterministic generated mode-4 assertions; `tests/Edulytics.Tests/MathematicsIntelligence/UsCcssG1FullGradeStartingTeachingTests.cs`: learner-facing independent task and upgrade assertions.

**Stage A academic decision:** The independently reviewed original **public** 2021 warm-up, original Activities 1–2, both Diego/Andre interpretations, actual mathematical scope and grade-standard goals are covered. Verbatim parity for the publisher's **protected assessment is unresolved** and recorded separately; instead the **owner-approved independent Edulytics task** meets the known goal. The replacement is not represented as a source-original student task.

**Stage B academic decision:** Learner teaching and examples include the original public mathematical goal, and a distinct first-party Cool-down under the lesson 60 scope.

**Stage C academic decision:** Practice includes the original public arithmetic families plus the independent numeric Cool-down. The split-to-ten strategy and invalid answers are independently reviewed in code, with xUnit assertions prepared (not executed).

## Closure boundary

**A/B/C content authoring is approved as `READY_FOR_STAGE_D` on the owner-authorized independent Edulytics assessment basis.** The source register must explicitly retain **protectedOriginalCooldownVerbatimVerified=false**. Never claim pixel parity, original publisher test reproduction, child classroom participation, or any licensed publisher assessment extraction.

**Stage D is still PENDING**, including actual .NET tests, exact-SHA CI, authenticated learner rendering, persistent grading/attempt evidence and final grade release gates. These must run **only after the entire grade is frozen**, per the owner's instruction. The academic acceptance ledger remains **`IN_PROGRESS`**, and grade release **`NO_GO`**, until D plus formal E/F gates actually pass.

Now continue A/B/C work sequentially at ordinal 61; reopen ordinal 60 only for a proven defect or relevant changed dependency, not merely because the publisher's copyrighted Cool-down is inaccessible.
