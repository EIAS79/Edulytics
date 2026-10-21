# Grade 1 ordinal 57 — U03 L20 / A Trip to the Zoo: A–C closure evidence

**Lesson code:** `PED:US-CCSS-MATH:G1:U03:L20`
**Authoring decision:** `READY_FOR_STAGE_D` (**A/B/C teaching + Practice repair complete; D, CI, and final academic acceptance NOT executed**).

## Stage A — original 2021 source independently reconciled

Read-only original source on Our-CS:
`C:\Users\khali\Downloads\New folder (2)\Common Core\01_Grade_1_IM_K5\unit-3\lesson-20\lesson.html` (629-line original Kendall Hunt teacher HTML; inspected without modifying the source file). Other original preparation file:
`...\Exact_Sources\im.kendallhunt.com_k5_teachers_grade-1_unit-3_lesson-20_preparation.html`.

Online original edition for identity and citations:
- https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-20/lesson.html
- https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-20/preparation.html
- Independent **2021-edition** review, with the original primate cool-down's student task reproduced: https://edreports.org/reports/detail/kendall-hunts-illustrative-mathematics-2021/first-grade/gateway-two

The 2021 source HTML's existing repository-recorded digest is `b52d02ff8f09926ec8e838c59bc9b924949226cfc7f2be8c0de05909a2338011`; the digest was recorded and checked in an earlier read-only integrity pass and is not newly claimed to be recalculated now. The teacher HTML itself shows its login-gated five-minute **A Visit with the Primates** assessment at the end, while EdReports' review of **Kendall Hunt 2021 first grade** separately reproduces the matching 8-monkeys/4-gorillas/7-orangutans student assessment and request to show a drawing, numbers, or words. The protected attachment itself was **not opened**. This is a qualified two-source corroboration of the actual original task, NOT a claim that the authenticated publisher attachment was retrieved; no later-edition text is being used to certify 2021.

2021 original student task inventory verified for this lesson:
- Mental warm-up expressions: `8+2+4=14`, `8+6=14`, `7+3+6=16`, `7+9=16`.
- Activity 1 Jada's reptiles: **8 snakes + 7 iguanas + 5 frogs = 20**. *Frogs*, not the later v.360's turtles.
- Activity 2 birds: **3 herons + 6 hawks + 7 hummingbirds = 16**.
- Activity 2 large cats: **8 lions + 4 tigers + 3 cheetahs = 15**.
- Activity 2 petting zoo: **8 goats + 7 sheep + 4 pigs = 19**.
- Original five-minute primate assessment (2021 EdReports reproduction): **8 monkeys + 4 gorillas + 7 orangutans = 19**.
- Student section summary: three-addend `4+8+6=10+8=18`; two-addend `4+8=4+6+2=10+2=12`, each illustrated with two ten-frames.
- Teacher-only and paper assessment boundaries: first-party digital numeric verification does not assert that student freehand drawings, partner discussion, counters, or classroom presentations actually occurred.

**Source illustrations and rights:** The original HTML alt descriptions confirm the double-ten-frame counter groupings; the animal photo bears third-party MabelAmber/Pixabay attribution. Lesson quantities and teaching strategies can be explained with independent first-party text, without copying publisher images. Exact publisher-art pixel equivalence and actual classroom observation are **not** required by the original academic contract; they are not claimed. The source task semantics and accessible alternatives, not the image pixels, were reviewed.

**A verdict:** Source-goal, teacher/student mathematical tasks, the section summary and the originally protected cool-down (independently documented in the 2021 review) reconciled; no known original A-stage task omission remains in this lesson.

## Stage B — effective source-grounded teaching repaired

Reused the previously authored Rich V2 repair `src/Edulytics.Data/Seeding/CommonCoreG1FullGradeStartingContentCorrections.cs` rather than rewriting the lesson. Existing examples retained correct warm-up plus four animal stories. Restored both original student section summary representations using independent accessible text. Added the primate cool-down with its exact 2021-edition quantities, using an explicitly attributed independent 2021 review. The numeric solution is `8+4+7=19`, including decomposition `8+2+2+7=10+9=19`.

The **lesson-57-specific** content identifier is now `us-ccss-g1-u03-l20-zoo-2021-source-v8`, and `CanUpgradeExisting` permits scoped persisted v6→v8 and v7→v8 migration while preserving unrelated grades/lessons. The teaching assertions and version guards were authored in `tests/Edulytics.Tests/MathematicsIntelligence/UsCcssG1FullGradeStartingTeachingTests.cs`. These are tests **prepared for final verification, not test PASS claims**.

**B verdict:** Known original learner-teaching omissions repaired in the actual scoped content materializer and guarded content upgrade; no known outstanding B-stage repair. Actual authenticated rendering at a final SHA belongs to Stage D.

## Stage C — source-specific numeric Practice and independent correctness assertions

The existing exact-skill Practice family `usccss.g1.u03.l20.zoo.three_addends` was reused in `src/Edulytics.Services/Mathematics/UsCcssG1FullGradeNextQuestions.cs`. Added source-approved mode 4 for the original primate cool-down without changing other lesson families. Source family cases in the 2021 curriculum are now separately represented by all four original student problems plus the primate assessment; mode 0/1 also offer valid generic three-addend practice instances within 20.

`VerifySourceGroupEquation` validates the exact three original operands (as a multiset), exact numeric total, and parseable complete response; the scorer accepts valid commutative ordering but rejects dropped groups, false operands having the same total, wrong totals, duplicate keys and malformed answers. An independently authored test `Lesson57FourOriginalZooStoriesAndPublishedPrimateCoolDownRejectWrongGroups` covers **all five** exact problems, both accepted and rejected examples, additional mode-4 generator output, and invalid out-of-family mode. Test file: `tests/Edulytics.Tests/MathematicsIntelligence/UsCcssG1FullGradeNextPracticeTests.cs`.

**C verdict:** Full known source-specific *numeric* Practice task and assessment coverage, with classroom-only representations described but not falsely marked machine-graded; math/verifier reviewed in source, tests authored for future execution.

## Closure semantics / boundaries

- **A/B/C are source/code reviewed and repaired:** `READY_FOR_STAGE_D`.
- **The test assertions have not been run**, as the owner explicitly requires no intermediate CI and final .NET/authenticated tests only after whole-grade content freeze. No CI was dispatched from this authoring work.
- This A–C closure does NOT mean Stage D passed, production learner-facing HTML was authenticated against the final SHA, persisted scores were validated, or this lesson was internally accepted, merged, or published.
- A later proven source/code/shared dependency regression can reopen a completed lesson; otherwise the A–C execution queue must skip lesson 57.

**Independent audit source separation:** The original publicly visible 2021 teacher page and the local original Kendall Hunt 2021 HTML establish tasks and images; the restricted primate **content** is separately corroborated by EdReports' 2021 review rather than inferred from a 2023 CKMath guide. Review does not assert copyright transfer or full assessment-file access.

**Open A–C defects:** none identified after this source/code review. **Deferred D gate:** compile and execute the actual .NET tests, complete authenticated student rendering and grading/persistence checks, verify source/visual/security requirements and bind outcomes to final frozen code SHA.
