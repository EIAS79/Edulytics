# Assessment PDF Downloads + Student-Centric Results Preview — Execution Plan

## Objective

Implement the two approved UX improvements without changing the Assessment Results selection/download workflow or result-calculation rules:

1. After an Offline assessment is published, surface PDF downloads for:
   - student questions only;
   - teacher questions + correct answers/solutions.
2. Replace the Assessment Results import preview's raw one-row-per-question table with a student-centric summary:
   - one row per student;
   - final score and percentage;
   - validation status;
   - expandable View details showing each question, maximum score, entered score, and final total.

## Scope locks / non-goals

- Do **not** change the current Assessment Results selector:
  `Academic Year -> Assessment -> derived read-only Class -> Download XLSX`.
- Do **not** change Offline/Open eligibility for result import.
- Do **not** change Teacher permissions.
- Do **not** change result persistence, score aggregation, or percentage calculations.
- Do **not** change Online assessment delivery or scoring.
- Do **not** reintroduce row deletion for individual Assessment Results question rows.
- Keep all other Bulk Data Import preview behavior unchanged.

## Phase 1 — Plan and scope lock

- Record this execution plan.
- Branch from current `main`.
- Confirm existing PDF renderer/routes and existing Assessment Results canonical import pipeline.

## Phase 2 — Published Offline assessment PDF actions

- Reuse the existing secure Teacher PDF endpoints:
  - `student-paper.pdf`
  - `answer-key.pdf`
- Add actions to the Assessment details page when:
  - user is Teacher;
  - assessment is Offline;
  - assessment is no longer Draft.
- Use clear labels:
  - Download Questions PDF
  - Download Questions + Answers PDF
- Preserve existing authorization and assessment-scope checks.
- Do not expose answer-key downloads to Student users.

## Phase 3 — Student-centric Assessment Results validation preview

- Keep the canonical normalized import rows unchanged for persistence semantics, but enrich smart-workbook normalization with presentation metadata:
  - QuestionPrompt
  - QuestionMaxScore
  - AssessmentMaxScore
- In the Assessment Results batch preview only:
  - group rows by the hidden stable StudentNumber identity;
  - display one row per student;
  - calculate preview total from entered question scores;
  - calculate preview percentage using AssessmentMaxScore;
  - show Ready / Needs attention state;
  - add an inline View expander with:
    - question order/prompt;
    - question maximum score;
    - entered score;
    - total and percentage.
- Hide internal IDs and StudentNumber from the teacher-facing preview.
- Remove question-row checkboxes / Delete selected controls for Assessment Results.
- Leave all non-Assessment Results preview tables unchanged.

## Phase 4 — Regression and quality gates

Add/adjust automated coverage for:

- smart workbook normalized rows carry the presentation metadata;
- persistence-required Assessment Results columns remain unchanged and valid;
- PDF document factories still include questions and answer key content;
- Assessment details exposes both PDF actions only for published Offline teacher assessments;
- Assessment Results preview is student-centric and does not expose row deletion;
- non-Assessment Results import preview remains on the existing generic table path;
- existing score aggregation remains:
  `Total = sum(question scores)`;
  `Percentage = Total / Assessment.MaxScore * 100`.

Run full repository CI/security/PostgreSQL/container gates.

## Phase 5 — Merge, deploy, and smoke verification

- Review final diff for scope containment.
- Merge after all PR checks pass.
- Verify main CI passes.
- Verify Render deploy uses the exact merge commit and reaches LIVE.
- Confirm no post-deploy error/critical/fatal logs and no HTTP 5xx.
- Manual smoke targets:
  1. published Offline assessment shows both PDF buttons;
  2. Questions PDF downloads;
  3. Questions + Answers PDF downloads;
  4. Assessment Results upload preview shows one row per student;
  5. View expands question-by-question scores and final result;
  6. Confirm import remains unchanged.
