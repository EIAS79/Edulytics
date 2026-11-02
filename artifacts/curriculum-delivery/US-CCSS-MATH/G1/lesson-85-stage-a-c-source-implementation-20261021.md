# Grade 1 lesson 85 (U04 L20) — original 2021 source / independent Edulytics A–D internal acceptance

**Identity:** `PED:US-CCSS-MATH:G1:U04:L20`; `Make Two-Digit Numbers in Different Ways PLC Activity`; CCSS 1.NBT.B.2. **Decision:** first-party source-goal, teaching, Practice and exact-run authenticated runtime reviewed; not a teacher signoff, not a claim of physical classroom completion and not a whole-grade release.

## A. Independently checked original first-edition teacher lesson

On 2026-10-21, independently read the 2021 lesson's local read-only Our-CS teacher HTML `unit-4/lesson-20/lesson.html` and rehashed the file to SHA-256 `57008ac700f8814140758da2674b96827731fe205d1f34affdf9da5509b53b09`, matching the checkpoint/source inventory. The associated original teacher preparation hash is `20097467e88523e23a1343d1eed83849ea7be50d2c556fd33e544620e97b550e` in the canonical manifest. Public identity: https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-20/lesson.html.

- Warm-up: estimate a visually disorganized collection, revise after seeing an organized representation, mark **too low**, **about right**, **too high**; after observing representations the teacher discloses **76 cubes**. A numeric answer of 76 is not evidence the child performed visual estimation.
- Original Activity 1: show **94** as tens and ones; acceptable partitions include `9 tens + 4 ones`, `8 tens + 14 ones`, through `0 tens + 94 ones`: ten mathematically distinct nonnegative full-ten counts from 0 through 9. Student explanations/drawings/physical connecting cubes are choices, not electronically observed outcomes.
- Original Activity 2, four exact mystery bags: **A** five tens/two ones = **52**; **B** four tens/twenty-five ones = **65**; **C** 49 cubes of which 29 are ones implies **2 tens**; **D** 36 cubes with two tens implies **16 ones**. These exact numerical subskills are machine-checkable.
- Optional student-written mystery-bag prompt, partner swapping, cube construction, source photographs/figures and freehand visual explanations remain instructional, not falsely certified by the numeric response verifier. The publisher-protected assessment, if unavailable, is not alleged read, adapted or copied.

## B. Existing source-aligned Edulytics first-party teaching

Reviewed `src/Edulytics.Data/Seeding/CommonCoreG1FullGradeFinalUnit4Corrections.cs`, exact lesson content version `us-ccss-g1-u04-l20-estimation-mystery-bags-v2`. The existing learner teaching explicitly requires an estimate before the count is shown, first-party regroupings through zero full tens, source bag A/B/C/D quantities, and optional teacher/partner explanation. These are independent first-party explanations; the original publisher picture is not copied.

## C. Independent original-goal maths and genuine grading

The registered family `usccss.g1.u04.l20.equivalent_tens_ones` has source-specific modes: 94 regroupings and four exact bag cases (4=52, 5=65, 6=2 tens, 7=16 ones). `UsCcssG1Lesson85Original94WaysAndMysteryBagTests.cs` independently tests nine valid alternate models (in addition to the canonical nine-tens form), rejects invalid alternatives and a wrong total number of ways, and asserts correct/wrong/extra-key/non-source operand rejection for all four bag cases. `UsCcssG1FullGradeUnit4FinalPracticeTests.cs` additionally covers exact response verifier boundaries. No false digital scoring of a child's drawing, estimated image response, student-authored mystery bag, or actual classroom participation.

## D. Exact-run authenticated verification

GitHub Actions run https://github.com/EIAS79/Edulytics/actions/runs/39600111939, immutable input artifact **12345398813**, per-lesson independent reconciliation artifact **12346166910**, tested source code `5fb0d3786d52379e60de01e3773297afc34d3721`. The independently reconciled ordinal 85 is `TECHNICAL_CANDIDATE_VERIFIED`, verifies two chosen source-body text fragments, real HTTPS authenticated lesson rendering and source-family correct/malformed/well-formed-wrong digital attempts persisted. The exact unescaped authenticated learner HTML SHA-256 is `7044c739d7e88627fd519e1c267451d0004052e1acf38cdcca0cda255d299571`. Source/math CI is also independently green on the same first-party application source revision; document-only acceptance metadata changes do not substitute for rechecking at an eventual release candidate.

**Internal acceptance boundary:** Edulytics independently authored source-aligned learning and digitally assessable arithmetic. No external, academic, teacher or committee signoff is required for publication. No copied source illustrations, physical pupil exercise completion, optional authoring/grading, production UAT, merger or grade-level `GO` is claimed. Source-checked text fragments establish selected rendered task prompts, not pixel-identical page layout.
