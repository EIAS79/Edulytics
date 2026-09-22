# Assessment Results Single Entry + Unified Paper View — Execution Plan

## Objective

Consolidate Offline assessment score entry into one workflow and make the Bulk Data Import student detail view match the Assessment Results "View paper" presentation.

## Confirmed product behavior

- The Assessment Results page remains available for both Online and Offline assessments.
- Online assessments keep using Edulytics-captured student responses and automatically recorded results.
- Offline assessments use Bulk Data Import as the only score-entry workflow.
- The Assessment Results page becomes read-only for Offline results after import.
- The Bulk Data Import preview must present the same student paper structure used by Assessment Results:
  - Question
  - Student answer
  - Correct / expected answer
  - Score
  - Final score
  - Percentage
- For Offline Bulk preview, Student answer is shown as "No submitted answer recorded" because the workbook contains scores only.

## Phase 1 — Scope lock and plan

- Branch from current main.
- Record this plan.
- Preserve current Assessment Results Bulk selector:
  Academic Year -> Assessment -> derived read-only Class -> Download XLSX.
- Preserve Offline/Open eligibility and existing score calculation/persistence rules.

## Phase 2 — Remove duplicate Offline score-entry workflow from Assessment Results

- Remove the direct Assessment Results Excel upload/download card from the Results page.
- Remove direct `results-workbook.xlsx` download and `results-workbook` import controller actions so there is no alternate score-entry route.
- Keep the Results page and View paper for both Online and Offline assessments.
- For Teacher + Offline assessments, show a single "Import scores" action that opens Bulk Data Import preselected to that assessment.
- Do not show that import action for Online assessments.
- Do not change Online submission/result behavior.

## Phase 3 — Unified Bulk preview paper view

- Allow Bulk Data Import Index to accept an optional assessmentId for preselection.
- Preselect the matching Academic Year + Assessment + derived Class and enable Download XLSX.
- Enrich Assessment Results batch details with authoritative assessment question metadata from the Assessment service, including correct/expected answers.
- Rebuild the Bulk student detail panel using the same visual/content contract as Results View paper:
  - same paper header;
  - same question table structure;
  - same labels;
  - same score badges;
  - same final score/percentage presentation.
- Offline Bulk preview Student answer remains "No submitted answer recorded".
- Keep one student summary row per assessment/student and keep Confirm behavior unchanged.

## Phase 4 — Regression, security, and CI

Add or update contract coverage for:

- Results remains accessible for Online and Offline published assessments.
- Results no longer exposes direct Excel result import/download.
- Offline Results exposes Bulk Data Import CTA; Online Results does not.
- Bulk import supports assessment preselection.
- Bulk preview uses the same View paper contract and exposes expected answers.
- Existing Bulk import score aggregation and persistence remain unchanged.
- Existing authorization, anti-forgery, tenant isolation, and Online assessment flows remain unchanged.

Run the full repository CI suite:
- quality/regression;
- PostgreSQL;
- SAST/CodeQL;
- container/Trivy.

## Phase 5 — Merge, deploy, and smoke verification

- Review final diff and PR feedback.
- Merge only after all checks pass.
- Verify main CI on the exact merge commit.
- Verify Render deploy reaches LIVE on the exact merge commit.
- Verify no post-deploy error/critical/fatal logs and no HTTP 5xx.
- Smoke targets:
  1. Online assessment still opens Results and View paper;
  2. Online Results has no score-import Excel workflow;
  3. Offline Results has Import scores -> Bulk Data Import;
  4. Bulk page opens with correct assessment preselected;
  5. Bulk preview View matches Results paper structure;
  6. Confirm import still persists scores and percentage exactly as before.
