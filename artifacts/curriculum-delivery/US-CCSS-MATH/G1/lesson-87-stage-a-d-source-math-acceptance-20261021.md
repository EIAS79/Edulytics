# Grade 1 — lesson 87 (U04 L22) independent first-party A–D source/math audit
**Code:** `PED:US-CCSS-MATH:G1:U04:L22`. **Scope:** first-party value comparisons/teacher-directed center instructions plus honest digital Practice verification.

## A. Original 2021 teacher/student source — independently rechecked 2026-10-21
Read-only Our-CS `01_Grade_1_IM_K5/unit-4/lesson-22/lesson.html`, observed SHA-256 `2a56a19b52f47b36b1faffcf9e56ea5c0c0ba2838a4544c77a2c1862b50986a5`, matching the source register; original preparation hash `1f7a2aab38415a47d79c4bdc814431bd7b7a7c9e28bd71d57091841bb7222218`. Public page https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-22/lesson.html.

- Warm-up source true/false: `92=90+2` TRUE; `90+2>80+12` FALSE because both are 92; `20+13<30+13` TRUE, 33<43.
- Activity 1 Mystery Number Stage 1: two numeral cards form a two-digit mystery number. One child gives truthful clues; partner guesses after each clue; guesser receives one point per clue, repeat five rounds, lowest total wins. The number cards and partner performance are physical classroom activities; not proven by numeric answers.
- Activity 2 learner chooses Mystery Number, Get Your Numbers in Order, or Greatest of Them All, then compares methods with partner. Source supplies teacher instructions and center choices, not a single automatically gradable student-authored center result.

## B. First-party Edulytics teaching
Reused `src/Edulytics.Data/Seeding/CommonCoreG1FullGradeUnit5ContentCorrections.cs` version `us-ccss-g1-u04-l22-exact-three-comparisons-v4` with exact warm-up truth values, "two number cards", five rounds/lowest score, all three named center choices. The source-specific test checks a guarded upgrade from the known v3 text and does not certify copied original gameboards.

## C. Digital source-exact math and limits
Family `usccss.g1.u04.l22.center_place_value`: source original warm-up modes 4–6 require exact, validated `TRUE`, `LEFT`, `RIGHT` values, reject wrong/malformed/extra-key answers and invalid parameters. `UsCcssG1Lesson87MysteryNumberSourceTests.cs` checks these three original expressions and ensures source choices and scoring language are visible to learners. The child's partner clue invention or five-round scores remain instructor-led, not falsely machine-scored.

## D. Authenticated independent evidence
Original selected source task text plus learner HTML, and real correct/wrong/malformed grading persistence observed in same-run CI [39600111939](https://github.com/EIAS79/Edulytics/actions/runs/39600111939), artifacts 12345398813 and 12346166910; tested application code head `5fb0d3786d52379e60de01e3773297afc34d3721`, 2 selected original source-text fragments. Exact authenticated learner HTML SHA-256 `f95d289b4e4f986f01fde3f5d4442aafb09a8ffc37792b2f4294c07be36d9324`. Mathematics Foundation CI run 39622317973 passed its main suite. Future release candidate revalidation is separate.

**Decision scope:** First-party mathematics and source-aligned center teaching ready for internal acceptance; no claim physical center activity occurred, no automatic teacher signoff or image copying, no whole-grade release.
