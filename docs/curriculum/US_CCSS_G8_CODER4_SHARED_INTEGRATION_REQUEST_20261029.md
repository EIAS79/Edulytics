# Grade 8 Coder 4 — Shared integration and release blockers (2026-10-29)

**Course:** `US-CCSS-MATH:G8:OUR-IM-2017` (9 units; 131 lessons).
**Branch:** `curriculum/us-ccss-g8-coder4-ourcs-fullgrade-20261029`.
**Owner of the next shared integration:** American Supervisor 2 designated single writer.

## Verified course-owned work

- 131/131 canonical Grade 8 lesson identities have original Edulytiks Rich V2 lesson candidates (393 worked examples across 11 embedded JSON resources). This is **not** independent source-activity parity or academic acceptance.
- The existing production Rich V2 registry returns all 131 exact lesson records. Unit1–2 and whole-grade targeted .NET tests, including the previous 8.EE.A.1 integer-exponent/no-logarithm regression, passed locally.
- 131/131 publisher teacher-preparation pages were fetched and matched by exact canonical URLs; 22 standards-role differences are flagged in the source crosswalk. Do not rewrite CCSS provenance from this scan alone.
- Source crosswalk: `docs/curriculum/us-ccss-g8-coder4-source-crosswalk-20261029.json`.
- Lesson/quality matrix: `docs/curriculum/us-ccss-g8-coder4-course-evidence-20261029.json`.
- Static Practice audit: `docs/curriculum/us-ccss-g8-coder4-practice-crosswalk-20261029.json`.
- Draft original teacher-scored tasks/rubrics: `docs/curriculum/us-ccss-g8-coder4-teacher-rubric-candidates-20261029.json`.

## Shared technical issue — Rich V2 global catalogue count

In the broader `.NET` test suite, `RichLessonContentV2RolloutTests` assumes exactly **318** existing embedded Rich V2 lessons. The new Coder 4 Grade 8 resources add **131**, making the observed catalogue **449**. The Grade 8 specific test independently confirms all 131 lesson codes are unique and present in `RichLessonContentV2Registry.AllDocuments`.

**Integration requirement:** a single writer should update the shared catalogue regression test to account for Grade 8 **without merely replacing a strict count with an unverified minimum**, preserve all previously covered codes, and coordinate with any other grade that concurrently adds resources. Avoid concurrent modifications to `tests/Edulytics.Tests/MathematicsIntelligence/RichLessonContentV2RolloutTests.cs`. Re-run full .NET suite on the final integrated SHA. The current course PR deliberately does not edit this shared test.

## Practice — source-objective to assessment gap

- Static official-outcome routing and only two explicit lesson-level skill mappings exist; these do **not** establish complete Grade 8 lesson-scoped Practice.
- Some lessons have mathematical-practice-only outcomes or multiple candidate target families. Such rows require source-specific question-family review, not automatic promotion of arbitrary algebra or logarithms.
- For every lesson, exercise the current route and actual generated question content; test correct/incorrect/equivalent answers, malformed submissions, solution or answer leakage, randomized mathematical invariants, and saved attempts. Do this against **disposable** tenant-isolated data, not real school records.
- Retain merged 8.EE.A.1 integer-exponent laws and no-logarithm remediation (#456/#458).
- The draft 131 teacher-scoring rubrics are **authoring specifications**, not connected authenticated student submissions, teacher workflows, or evidence that an actual teacher assigned or graded work. A reviewed, secure teacher-assessment route is needed for oral explanations, drawings, constructions, card sorts and modeling.

## Academic and release gates

- No page-by-page comparison of publisher student activities against the **effective rendered** Edulytiks output has been independently accepted; the preparation-page scan is only source intake.
- 131 authored Rich V2 records do not show that accessible diagrams, figure transformations, graph plotters, rich visual scaffolds or all original tasks are present in the student UI. Renderer support and source-specific authentic learning activity parity require review.
- Authentication, student/teacher Practice, attempt persistence, two-school/wrong-course isolation, final-SHA protected CI, Render deployment, actual owner UAT and independent lesson A–D evidence have not been established.
- Current counts: **131 content candidates; 0 verified lesson-specific Practice; 0 observed human assessments; 0 individual academic acceptances.** This candidate is **NO MERGE / NO DEPLOY** until all original Full-Grade V2 and V1 A–D requirements and protected technical checks pass.

## Safe next integration order

1. Single writer reconciles shared Rich V2 catalogue assertion with all concurrent grade changes and preserves other lessons.
2. Verify each lesson's actual rendered text and visual/accessible representation (no automatic approval from a metadata flag).
3. Review Grade 8 source tasks and exact Practice families, add real grader/rubric workflow wiring where needed.
4. Run full .NET/security/contract/negative maths tests, followed by disposable HTTPS student/teacher two-tenant tests and final GitHub SHA CI.
5. Retain all 131 individual evidence gates and do **not** release or alter real student/school/production data before they genuinely pass.

## V7 resumed local full-suite evidence and single-writer handoff

- Recovered completed full local .NET test output at e644d5e264e078a55e37803aa8aac25763b0bd5c: 6,837 passed, 2 failed, 0 skipped, 6,839 total. The test exit status was 1.
- Both failures are stale global Rich V2 catalogue count assertions expecting 318 instead of 449: RichLessonContentV2RolloutTests at line 117, and RichLessonContentV2PilotTests at line 329. Added Grade 8 count is exactly 131. This is an introduced shared-test integration blocker. Do not weaken a count assertion to a minimum or modify common tests without the Supervisor 2 single-writer coordination.
- Current PR #593 head is e644d5e264e078a55e37803aa8aac25763b0bd5c and Draft. Eleven observed CI checks were queued, so CI success was not demonstrated.
- Twelve lessons with an unmapped outcome were Mathematical Practices (CCSS:MP.1 through MP.8) only, not unmapped Grade 8 content standards. Unmapped formal outcomes = 0. Eighteen lessons have multiple concrete Practice rule families; only two lessons have explicit lesson-level skill mappings.
- The 131-row triage file us-ccss-g8-coder4-v7-lesson-gates-20261029.json flags 88 lessons for visual-scaffolding inspection and 30 for Practice routing review (12 MP-only plus 18 multi-family). Automated flagging does not prove a missing feature or establish source fidelity.
- All 131 lesson candidates remain Source Review Blocked and require a human-assessment plan. Academic acceptance A/B/C/D = 0; lesson-specific generator/grading and authenticated saved attempts = 0 observed.
- Single writer requested: reconcile both global catalogue assertions preserving old 318 and new 131 identities, review source student activities and the 22 standard role differences, validate exact Practice including negative and exponent/no-logarithm cases, integrate teacher-scored submissions only through authorized shared workflow, validate two-school isolation and accessible lesson rendering in disposable data, and pass final frozen-SHA CI before any merge/deploy.

- Concrete Grade 8 semantic Practice mismatch: PED:US-CCSS-MATH:G8:U03:L12 publisher student goal is solving a two-variable linear equation, whereas its explicit lesson skill family is algebra.linear.ax_plus_b_equals_c (one-variable). This is not lesson-exact. Requires shared one-writer correction before Practice acceptance. U04:L05 uses the same family more plausibly; an isolated 8-question smoke test verified correct answer acceptance and malformed/wrong rejection, but did not test authenticated persistence or full source assessment parity.

## V8 Supervisor 2 exact patch integration and full publisher lesson exposure

- The Supervisor 2 PR #594 patch was reviewed as two commits based directly on the Grade 8 branch: f60b9f372205b7fc4a4da0700be482e5a1a2187f (pilot cohorts) and eb0d54bd862e1cafe9e628244de0b006d0320652 (rollout catalogue). Both shared test fixes were **fast-forwarded unchanged** in the isolated Coder 4 clone, not independently re-authored. The original Grade 3 count 143, Grade 4 count 149 and uniqueness checks remain.
- On Our-CS Primary, live public teacher *lesson* and student *lesson* HTML was fetched for all **131/131** canonical lessons, with SHA-256 source snapshots and a total of **411** real student activity headings. The result is in docs/curriculum/us-ccss-g8-coder4-v8-publisher-activity-structure-20261029.json. This is stronger source provenance than preparation-only retrieval but **not source-activity mathematical equivalence approval**.
- Source student pages for **118/131** lessons contained non-site-logo image tags while the 131 Rich V2 authored candidates expose no Visuals objects. Many source activities require graph drawings, shapes, grids or constructed figures; do not promote to academic A/B based on narration-only work. Lesson U08:L07 is a concrete example: source student has four separate activity headings and multiple diagrams; Rich V2 candidate has zero visuals.
- All 131 per-lesson A–D blocker classifications, exact student and teacher source URLs/SHAs, activity heading inventories, figure triage flags, Practice mapping details and draft-only teacher-scoring evidence are in docs/curriculum/us-ccss-g8-coder4-v8-acceptance-ledger-20261029.json.
- Integrated Grade 8 source work cannot be declared production-ready until original accessible course diagrams are actually implemented and verified in the authenticated learner UI. Do not silently substitute any generic registry icon or a static figure merely to increase Visuals counts. Shared visual renderer extensions require a coordinated single writer.
- The general US CCSS isolated two-school GitHub workflow currently tests synthetic school infrastructure and other grade coverage, not 131 authenticated Grade 8 lesson-specific student/teacher Practice and evidence workflows. A passing generic rehearsal is not Grade 8 D acceptance.
