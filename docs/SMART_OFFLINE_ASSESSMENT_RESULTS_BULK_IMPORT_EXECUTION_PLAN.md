# Smart Offline Assessment Results Bulk Import — Execution Plan

## Objective

Keep the existing Bulk Data Import workflow for teacher-entered assessment results while removing the two usability risks in the old template:

1. the teacher should never need to know or type the exact stored StudentName or StudentNumber;
2. the workbook must still clearly identify the selected Assessment by its user-facing title, date, and class.

The target workflow remains:

`Assessment Results -> Download workbook -> Fill scores -> Upload -> Validate -> Preview -> Confirm`

This work does **not** move result import back to the Assessment Results page and does **not** modify the assessment-specific XLSX workflow already present there.

## Locked product decisions

### Assessment eligibility and selection

- Assessment Results bulk import is for **Offline** assessments only.
- Online assessments must never appear in the Assessment Results download selector because online student submissions write results directly into Edulytics.
- The signed-in Teacher can only select assessments already visible to that Teacher under existing assignment/scope rules.
- Selection order in Bulk Data Import:
  1. Academic Year
  2. Assessment
  3. Class
  4. Download XLSX
- The Assessment selector is populated only after Academic Year is selected and contains Offline assessments for that year.
- Assessment options must remain unambiguous. Where titles collide, the visible option includes the assessment date.
- Class is derived from the selected assessment and displayed as read-only context, not as an independent target that can be mixed with another assessment.

### Workbook experience

- One downloaded workbook belongs to exactly one Offline Assessment.
- The visible workbook clearly shows:
  - Assessment title
  - Assessment date
  - Class
  - Student names
  - Question columns and each question maximum score
- Students are pre-populated from the authoritative assessment roster.
- The Teacher does not type or select StudentNumber, StudentProfileId, AssessmentId, ClassGroupId, or QuestionId.
- The Teacher edits score cells only.
- Immutable workbook context is protected/read-only for normal Excel use.
- Internal identifiers and concurrency/version metadata remain hidden from the user but are available for server validation.

### Validation behavior

On Upload and Validate, Edulytics must compare the workbook with the authoritative current assessment state.

There are three validation outcomes:

1. **Valid workbook**
   - Continue through the existing preview and Confirm workflow.

2. **Teacher-modified immutable workbook content**
   - Reject validation.
   - Identify the exact field/row that changed.
   - Show the submitted value and the original/expected value where safe and useful.
   - Tell the Teacher either to restore the original value exactly or download a fresh workbook.

3. **Genuinely stale workbook**
   - Examples: roster changed, assessment title/date/class changed in Edulytics, question set/order/max score changed, or result concurrency changed.
   - Reject validation.
   - Do not ask the Teacher to reconstruct the workbook manually.
   - Require a fresh download.

Score validation remains editable and actionable:
- score must be numeric;
- score must be within 0..QuestionMaxScore;
- required score cells must be complete for an imported student row.

## Non-goals / scope protection

- No role-permission changes beyond the already-restored Teacher -> Assessment Results permission.
- No changes to School Administrator, Subject Supervisor, Teacher, or Student responsibilities.
- No changes to Online assessment delivery/scoring.
- No changes to Multi-Lesson.
- No changes to At Class Level difficulty mix.
- No changes to Practice visuals.
- No changes to Question Variant / geometry semantic diversity.
- No changes to Analytics/PDF calculations or presentation from the previous completed scope.
- Do not alter the existing assessment-specific `/school/assessments/{id}/results-workbook.xlsx` workflow as part of this work.
- Do not require the Teacher to know internal GUIDs.

## Architecture rule

The new Bulk Import workbook is an **assessment-aware adapter around the existing Assessment Results import pipeline**, not a second result persistence engine.

After workbook validation succeeds, the adapter must normalize the workbook into the existing canonical Assessment Results import shape so the established:

`Upload -> Validate -> Preview -> Confirm`

pipeline and persistence rules remain authoritative.

This isolates workbook UX and integrity checks from the existing import confirmation engine.

## Phases

### Phase 1 — Scope lock and implementation plan

- Record this plan in the repository.
- Branch from current `main`.
- Lock the existing restored Assessment Results Bulk Import behavior as the baseline.
- Confirm that the assessment-specific XLSX route is outside this change.

### Phase 2 — Offline Assessment selector in Bulk Data Import

- Extend the import page model with Teacher-visible Assessment Result download options.
- Populate active Academic Years relevant to those assessments.
- Filter candidates to `AssessmentDeliveryMode.Offline` only.
- Add the dependent selector:
  - Academic Year
  - Assessment
  - derived read-only Class
- Keep all other import types unchanged.
- Keep authorization server-side; never trust selector values from the browser alone.

### Phase 3 — Smart Assessment Results XLSX generation

- Add a dedicated Bulk Import Assessment Results workbook component.
- Generate one workbook for one selected Offline Assessment.
- Pre-populate the exact current roster.
- Show title/date/class visibly.
- Show one student per row and one assessment question per score column.
- Hide internal StudentProfileId / AssessmentId / ClassGroupId / QuestionId / row-version metadata.
- Protect immutable visible cells and leave score cells editable.
- Embed a workbook/template version and integrity metadata sufficient for upload reconciliation.

### Phase 4 — Upload reconciliation, actionable errors, and legacy normalization

- Recognize the current smart Assessment Results workbook on upload.
- Validate selected assessment ownership/scope and Offline delivery mode again on the server.
- Compare visible immutable values against the original generated context.
- Compare hidden identity/version metadata against the current database state.
- Distinguish:
  - user-modified immutable content;
  - stale authoritative state;
  - invalid scores.
- Produce specific actionable messages with:
  - what changed;
  - submitted value when applicable;
  - expected/original value when applicable;
  - restore-or-redownload guidance for user edits;
  - mandatory redownload guidance for stale state.
- Normalize a valid workbook into the existing canonical Assessment Results import rows and pass it into the current `IDataImportService.UploadAsync` validation/preview/confirm pipeline.
- Do not create a second result-save path.

### Phase 5 — Regression, security, and UX gates

Add/adjust automated coverage for:

- only Offline assessments appear in the selector;
- Online assessments cannot be downloaded through crafted requests;
- Teacher can only download assessments in existing Teacher scope;
- year -> assessment -> class filtering is correct;
- duplicate titles remain distinguishable;
- workbook contains only the selected assessment roster;
- StudentNumber/StudentProfileId are not teacher-entered values;
- immutable context tampering is detected with an actionable message;
- stale roster/question/assessment state requires a fresh workbook;
- invalid score range returns a precise error;
- valid XLSX normalizes to the legacy Assessment Results canonical rows;
- existing preview and Confirm behavior still works;
- no unrelated import permissions change;
- Students / Teachers / Classes / Subject Supervisors import workflows remain unchanged;
- the assessment-specific Results-page XLSX workflow remains unchanged.

### Phase 6 — CI, merge, deploy, and focused smoke verification

- Review final diff for scope containment.
- Run repository build/test/security/architecture gates.
- Merge only after required checks pass.
- Verify deployed commit.
- Smoke-test as Teacher:
  1. Bulk Data Import -> Assessment Results
  2. Academic Year filter
  3. Offline-only Assessment selector
  4. derived Class
  5. XLSX download
  6. roster/title/date/question display
  7. valid score upload -> Validate -> Preview -> Confirm
  8. changed student/assessment field -> precise restore/redownload error
  9. stale workbook -> mandatory fresh-download error
  10. Online assessment absent from selector

## Acceptance criteria

The work is complete only when all of the following are true:

1. The Teacher remains in the existing Bulk Data Import workflow for Assessment Results.
2. Academic Year filters the Assessment list.
3. Only Teacher-visible Offline assessments are selectable.
4. Class is derived from the exact selected assessment.
5. Download produces one assessment-specific XLSX.
6. The workbook visibly identifies Assessment title, date, and class.
7. The workbook is pre-populated with the correct target roster.
8. The Teacher only needs to enter scores.
9. The Teacher never needs to know/type StudentNumber or internal IDs.
10. Editing immutable context is rejected with the exact reason and expected/original value where appropriate.
11. Genuine stale-state conflicts require a fresh workbook.
12. Valid workbooks still use the existing Bulk Import Validate -> Preview -> Confirm persistence pipeline.
13. Online assessment result delivery/scoring is unchanged.
14. The existing assessment-specific Results-page XLSX flow is unchanged.
15. No unrelated role permissions or import workflows change.
16. Regression and CI gates pass.
