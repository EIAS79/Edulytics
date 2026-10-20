# Grade 1 ordinal 59 — U03 L22 / Subtract from Teen Numbers — original 2021 A–C closure

**Lesson:** `PED:US-CCSS-MATH:G1:U03:L22`  
**A–C outcome:** `READY_FOR_STAGE_D`; not an academic acceptance or any assertion of a passing CI/.NET run.

## A — local read-only 2021 original source

Inspected Our-CS cached Kendall Hunt 2021 teacher source at
`C:\Users\khali\Downloads\New folder (2)\Common Core\01_Grade_1_IM_K5\unit-3\lesson-22\lesson.html`.
The source-register SHA-256 `a75f5ec320130a0ff353b152a864971f432f6b3f04c3d07ea8aca07a32944c46` was independently recorded in the existing prior source-cache audit, **not rehashed in this work**.
Teacher preparation:
https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-22/preparation.html

Read both activities and the student-facing fragments of the original cached teacher HTML:
- Warm-up Number Talk: `14−4=10`, `14−5=9`, `17−7=10`, `17−9=8`; pupils relate one difference to the next.
- Activity 1, Subtraction Methods: Elena has **16 crayons** and gives Diego **7**; student solves **16−7=9** using drawings, numbers or words, builds a poster and discusses multiple approaches during a gallery walk. One decomposition: `16−6=10`, then `10−1=9`.
- Activity 2, Number Card Subtraction: a teen-number card (11–19) and a subtractor card from **0–9**. Original lesson explicitly instructs the second card range **0–9** (although the gathered materials are labelled cards 0–10), then requests the difference, an equation and a chosen method. It allows subtraction *and* an addition-with-missing-addend explanation. The original 2021 deck includes 11–20 but the learner is instructed to choose a *teen* value; 20 is not a teen number.
- The published teacher HTML refers to a **Unit 3 Section D Checkpoint, 0 minutes**, behind the publisher login. This is a section checkpoint, not a separate, printed learner task required by this lesson's 40-minute activities; the original activities and their mathematical tasks were reviewed. No fictional authenticated checkpoint activity is claimed.
- Source diagrams emphasize teen quantities as ten and extra ones; first-party textual explanations and ten-frame modelling are supplied, without copying publisher illustrations. Student posters, pencil drawings and peer discussions remain teacher-led.

**Found Stage C defect:** The existing digitally auto-gradeable `TeenSubtract` family was restricted to `b >= a−9` and `b >= 1`; it represented **only crossing-ten removals**. The actual student number-card activity permits **zero** and **non-crossing-ten** subtractions (for example `19−0=19`, `17−4=13`). A forced `REACH10`/negative `REST` answer for such cases is mathematically misleading.

## B — repaired learner-visible source content

Reused the existing source-scoped Rich V2 teaching content in
`src/Edulytics.Data/Seeding/CommonCoreG1FullGradeStartingContentCorrections.cs` including Elena's original 16−7 story, all four number-talk expressions, `17−4=13` and ten-frame decomposition.

Explicitly added original **0–9** card bounds and legal zero-/noncrossing examples to teaching, with the allowance for a related addition/missing-addend explanation. The scoped lesson content version moved from `us-ccss-g1-u03-l22-teen-2021-source-v6` to `...-v7`; the content upgrader allows *this lesson only* to migrate from persisted v6 to v7. This ensures the actual learner-facing seed will not remain at the previously stale version after later verified deployment.

Teaching assertions for original card bounds, both subtraction methods, Elena source and guarded upgrade were authored in
`tests/Edulytics.Tests/MathematicsIntelligence/UsCcssG1FullGradeStartingTeachingTests.cs`.

## C — independent numeric generator/verifier source review

Kept the previously registered family identifier `usccss.g1.u03.l22.teen.subtract_cross_ten` (identity must not drift in persisted Practice contracts) while extending its **actual mathematics** to both source-authorized subskill cases. Source path:
`src/Edulytics.Services/Mathematics/UsCcssG1FullGradeNextQuestions.cs`.

- Mode 3 preserves exactly source Elena 16−7 and its ten-decomposition answer fields.
- Other TeenSubtract modes now include original teen cards **11..19** and subtractor cards **0..9**, with no imaginary ten split for a card that removes only ones.
- The score contract for a no-cross case is `DIFFERENCE=<number>;EQUATION=<teen>-<digit>=<number>`. For a case that reaches/crosses ten, it additionally requires the meaningful `REACH10` and non-negative `REST` values. Both modes are checked by the existing exact answer verifier. The separate `TenSubtract` family for ordinal 60 was preserved without changing its bounds.
- Independently authored xUnit regression `Lesson59OriginalTeenSubtractionCoversCrossTenNoCrossAndZeroCardWithoutFalseMethods` tests `17−4=13`, `19−0=19`, `14−6=8`, Elena `16−7=9`, exact source/card ranges, malformed or forced-cross false answers, and deterministic generated instances.
- Physical number cards, poster/gallery walk, individual drawing, strategy choice and addition-with-missing-addend written reflections remain classroom activities; these are **not** represented as automatically observed or numerically graded.

## Closure boundary

All identified A/B/C original source goals, available student tasks, source-specific learner teaching and digitally verifiable numeric examples have been independently compared and repaired. No known A–C defect remains after the source-code review, and the existing tests are prepared.

**All .NET tests, CI, authenticated rendering/score persistence, production/SHA checks remain UNRUN**, as specifically requested during grade-wide authoring. These requirements belong to **Stage D and later**, at the one frozen whole-grade content validation. No internal academic acceptance, merge, release or real classroom participation is claimed. Keep the lesson in `READY_FOR_STAGE_D` until the final grade freeze and do not redo its A–C repair without an actual proven dependency regression.
