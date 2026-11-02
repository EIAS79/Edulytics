# Grade 1 lesson 90 (U05 L02) — independent Edulytics original-goal A–D source/math audit

**Lesson:** `PED:US-CCSS-MATH:G1:U05:L02`, no-composing addition and explanations.

## A. Original 2021 source identity and tasks
Re-hashed read-only source on Our-CS 2026-10-21: `01_Grade_1_IM_K5/unit-5/lesson-2/lesson.html`, SHA-256 `8ecf6f24057ae9f0b20dbe809b5de3421758f9087598a8e679552bcf2b33b3bc` matches the existing independent source register. Public page https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-2/lesson.html.

- Original activity "Find the Value": `23+45=68`, drawings/numbers/words.
- Original Elena and Andre representations of `63+25=88`: tens then ones `60+20=80`, `3+5=8`; or add 20 then 5 to 63, `63+20=83`, `83+5=88`.
- Original activity "Five in a Row, Add Within 100 without Composing" uses actual gameboard/partner center rules as instructional activities, not electronically certified pupil actions. Original source figure/handwriting content is not copied.

## B. First-party learner teaching
Existing `src/Edulytics.Data/Seeding/CommonCoreG1FullGradeUnit5ContentCorrections.cs` explains both mathematical strategies; this is a separate Edulytics authored text, not a reproduction of an original image.

## C. Independently tested numeric exercise
Registered family `usccss.g1.u05.l02.explain_no_compose_equations`. Source modes `4` `23+45`, `5` `63+25` and Edulytics first-party strategy mode `6` require `ELENA_TENS=80;ELENA_ONES=8;ANDRE_FIRST=83;ANDRE_SECOND=88;TOTAL=88`. Test `tests/Edulytics.Tests/MathematicsIntelligence/UsCcssG1Lesson90TwoAdditionStrategiesTests.cs` checks correct intermediate steps, wrong answers, exact operands, generated mode reachability and case guards. A machine-scored structured explanation is **not** evidence the child drew source figures or explained to a real teacher.

## D. Authenticated HTTPS evidence
Same-run source-scoped learner HTML and persisted correct/malformed/well-formed-wrong grading verified in https://github.com/EIAS79/Edulytics/actions/runs/39600111939, artifact 12345398813 and reconciling artifact 12346166910. Tested code SHA `5fb0d3786d52379e60de01e3773297afc34d3721`. Ordinal 90 sampled two source text fragments; authenticated unescaped learner HTML SHA-256 `803f309a35ef65b18c7185e195b47a8d3dc4f47bb5c19c9194a48cbb77ca7c12`. 1800/1800 main .NET tests passed in Maths Foundation https://github.com/EIAS79/Edulytics/actions/runs/39622317973.

**Boundary:** Original math goals, first-party lessons and digitised subskills internally evidence-ready; no publisher image copying, child physical game outcomes, actual handwriting, teacher signoff or production deploy claimed. Whole grade NO_GO.
