# U01:L01 — Independent academic signoff request

Reviewer: Coder 5 (source comparison only). Approval owner: Supervisor 2 / independent academic reviewer; not yet approved.

Source: local Algebra 1 First Edition teacher preparation, teacher lesson index, student lesson index, and student practice HTML under the original Common Core folder. Original source file paths and SHA256 hashes are recorded in the existing U01:L01 checkpoint. Authored first-party Rich V2 content version: `alg1-u01-l01-local-source-aligned-20261030-v3`.

Learning goals: explain statistical versus non-statistical questions and numerical versus categorical responses, orally and in writing. Standards roles: 6.SP.A.1 Building On; HSS-ID.A.1 Building Towards (not Addressing).

## Activity-by-activity comparison and signoff requests

1. **Warm-up 1.1:** Original four chip questions contrast numeric/categorical and individual fixed/classwide statistical answers. First-party four shelf questions retain four distinct contrasts and invite a defensible explanation for every option, not only one 'correct' outlier. Review: approve changed context and reasoning depth.
2. **Survey investigation 1.2:** Original teacher assigns three investigation types (numerical, categorical, fixed/nonstatistical); student invents fourth and drafts four individual survey questions. First-party version preserves the triad, independent fourth, individual-versus-class-level distinction and group discussion. Review: approve task sequence.
3. **15-respondent activity:** Original collects four questions from 15 classmates, summarizes each, sorts response kinds and stores data for later graph work. First-party requires permission, 15 anonymous responses, grouped summaries, fourth-question classification and data preservation. Fictional data are explicitly NOT COMPLETED actual participation. Review: approve privacy-driven omission of original named-responder field.
4. **Synthesis:** Original numerical/categorical; statistical/nonstatistical and classwide averages versus individual response. First-party explanation correctly distinguishes a typical class commute from one student's commute. Review: approve pedagogical parity.
5. **Optional extension:** Original asks for one numerical-data news article and one categorical-data news article. Equivalent first-party prompt also asks students to identify the statistical question and claims' limitations. Review: approve optional scope.
6. **Practice P1/P2:** Original asks open numerical- and categorical-response survey questions. First-party prompts require explanations and use human-scored criteria, not synthetic automated grades. Review: approve open-response modality.
7. **Practice P3:** Original asks select-all across seven prompts. New first-party task uses six different prompts A–F, correct answer set A,C,E; mathematical variability classification remains, but it is NOT verbatim publisher parity. Review: specifically approve six-option substitute or request revision.
8. **Cool-down:** Publisher teacher login required, protected original questions unavailable locally. Original Edulytiks three-prompt substitute covers statistical/numerical, nonstatistical/categorical and classwide categorical variation with 6-point human-scored rubric. Record `ORIGINAL_2021_COOLDOWN_NOT_VERIFIED; FIRST_PARTY_EQUIVALENT_AUTHORED`. Review: accept equivalent or identify exact-original requirement.

Independent academic reviewer should record reviewer identity, date, explicit per-activity APPROVED or CHANGES_REQUIRED and rationale. **Stage A is not accepted until that decision.** No students were surveyed, no teacher grades or saved feedback occurred, and no word-for-word protected Cool-down comparison was claimed.

## Stage B static selector evidence

The Core project embeds Rich V2 JSON under `Curriculum/LessonContent/RichV2/*.rich-lesson-v2.json`. The service's lesson-content selection uses `RichLessonContentV2Registry.Find(lesson.Code, translation.CultureCode)` before its runtime composer when a canonical academic body is available. Static tests exercise the embedded version, English fallback and preservation of 132 distinct Algebra 1 lessons. The authenticated student application and actual tenant routing remain Stage D, not a prerequisite for local Stage B acceptance.

## Stage C exact integration handoff

The source-specific first-party multi-select draft must accept case-insensitive, whitespace-trimmed and order-independent choices A, C and E. It must reject any extra, missing, duplicate (including differing case), invalid, blank, malformed or null submission. A draft oracle is tested independently; it is **not** a production scorer. The current `StudentPrivatePracticeService.GenerateAsync` returns `NoSupportedOutcomes` for ALG1 before the general `LessonPracticeContractRegistry` dispatcher. No verified lesson-specific multiple-answer grading family or submitted teacher score is established. Supervisor 2 must decide reuse/extension and provide human scoring for the open P1/P2 questions, survey and Cool-down without disabling fail-closed. Stage D owns persisted attempts, grades, two-school isolation and authenticated end-to-end proof.
