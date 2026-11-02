# G1 ordinal 65 — U03 L28 Around the Room (optional): independent Edulytics A–C authoring and teacher-only rubric

**Decision:** `READY_FOR_STAGE_D` **only as authoring of a genuine teacher-reviewed optional classroom lesson**, NOT as automatic grading, real observed pupil story creation, completed poster gallery, CI, or final grade acceptance.

## A. Original 2021 lesson reviewed on authorized Our-CS, read-only

Original `C:\Users\khali\Downloads\New folder (2)\Common Core\01_Grade_1_IM_K5\unit-3\lesson-28\lesson.html`, actual lesson title **Around the Room (optional)**. Registered original 2021 teacher HTML SHA-256: `cca057a71e8fb653ed6f843b997cf38bba52b62d9b27dcad971fd1629f2ad47e` (**source register only; not independently rehashed this pass**). This review read the full warmup and Activities 1–3 as available in the 2021 cached original. Original trademark/figures were **not copied**.

- **Warm-up (10 min): Notice and Wonder**, counting real things in the classroom, noting objects present more than five / more than ten; does **not** manufacture students' real observed quantities.
- **Activity 1 (20 min): Writing Classroom Story Problems.** Teacher offers the deliberately **QUESTIONLESS** context “Noah had 8 pencils, Elena had 5 pencils, Han had 4 pencils.” Pupils must themselves write **both an addition question and a subtraction question**, and solve/explain them. Edulytics examples `8+5+4=17` and `8−5=3` are **plausible first-party student-authored questions**, not verbatim original question text. Original partner routine: **Partner A chooses a target answer less than 20**, Partner B authors a mathematical story about classroom objects with a question whose answer is that number; they solve and record an equation, then **switch roles**. Teacher monitors counting on, making ten and using known facts.
- **Activity 2 (10 min): Story Problem Posters.** The pair chooses one of its authored questions, makes a legible poster with the **story and question, without displaying the answer**, receives another pair's feedback and revises for clarity.
- **Activity 3 (10 min): Poster Gallery Walk.** Classmates read, draw/write and solve **four** peer-created story problems, each with its own matching equation, then compare methods and language. Real classroom poster/card collaboration is not recreated or automatically certified.
- The optional source ends with a synthesis about reflecting on authoring and solving. There is no separate original numeric five-minute Cool-down to reproduce.

## B. First-party learner/teacher explanation and version safety

Inspected existing source-goal lesson correction in `src/Edulytics.Data/Seeding/CommonCoreG1SourceGroundedInstructionalCorrections.cs`, not replaced with source publication text. The original V1 teaching covered generic addition/subtraction, but lacked the questionless pencil story, required **two types of pupil-created questions**, teacher partner roles/target under20, answer-hidden poster, gallery of FOUR peers and teacher observations.

Retained all V1 wording and added first-party narrative, mathematical examples, sequential instructions, common mistakes and an **explicit 6-part teacher review rubric** covering original scenario, clear unknown question, mathematically matching equation, target-answer consistency, suitable numerical representation, and pupil evidence from answer-hidden poster/gallery.

Advanced only `AroundRoomStoryProblemsVersion` from `us-ccss-g1-g1-u03-l28-source-goal-v1` to `...-v2`. Stored exact prior v1 text remains recognized **only for this source-identified lesson**, with fail-closed per-field previous-body validation and a new assertion rejecting edited/unrecognized v1 stored content. Other accepted lessons and source overlays unchanged.

## C. Real source-required activity vs numeric Practice boundary

The checkpoint's original Stage C declares `TEACHER_ONLY_SOURCE_AUTHORED_STORY`. This is **correct**: the source assesses authorship, meaningful question, story-aligned equation, real partner work and displayed poster—not the number `17` alone. A general generated question family would **not** prove authorship, physical poster preparation, four gallery responses or peer discussion. Existing unrelated numeric story subskills may help a learner but cannot certify the original optional classroom activity.

Reviewed absence of an automatically gradable freehand-authorship contract and designed the explicit teacher-observed rubric in learner/teacher content; both grade report and tests are updated to **identify this as a separate evidence basis**:
- `practiceGeneratorAndVerifierIndependentlyReviewed=false` intentionally;
- `teacherOnlyAuthoredActivityRubricIndependentlyReviewed=true`;
- `verifiedNoMachineGradeableStoryAuthorship=true`;
- `teacherEvidenceIsRequired=true`.

This record **does not claim actual teacher evaluation occurred**. That real observable learner/teacher workflow and evidence must be checked in later D/E tests.

## Test and release boundary

Added `Lesson65Original2021OptionalAroundRoomRequiresAuthenticLearnerAuthorshipAndGallery` and **source-guarded old-v1-to-v2 migration** assertions in `CommonCoreG1SourceGroundedInstructionalCorrectionsTests.cs`. Updated `tests/us-ccss-g1-a-to-c-status.test.cjs` to expect the distinct teaching-only report classification without claiming a fake numeric verifier. **No tests were executed** due to the owner-mandated final whole-grade freeze protocol.

**A–C teacher-only activity and rubric authoring CLOSED; `READY_FOR_STAGE_D`**. Academic ledger remains **IN_PROGRESS**; grade release **NO_GO**. Stage D real child tasks, rubric evidence, authenticated teacher review, persistent completion, copyright/accessibility, final .NET/CI and owner UAT all **PENDING**. Continue ordered work at ordinal **66**, never declare learner participation without evidence.
