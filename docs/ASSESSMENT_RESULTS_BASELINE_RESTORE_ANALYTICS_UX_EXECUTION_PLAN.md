# Assessment Results Baseline Restore and Analytics UX Execution Plan

## Objective

Restore the pre-remediation Assessment Results bulk-import workflow exactly for that feature only, without changing any other role permissions or import responsibilities, then apply the separately agreed Analytics presentation clean-up.

This plan deliberately separates restoration from redesign. The assessment-aware XLSX workflow introduced later is not merged into the restored bulk-import path in this work. The goal is to re-establish the known baseline first so the original Assessment Results problem can be reproduced and analysed accurately.

## Locked scope

### Assessment Results import

- Restore the Teacher-facing `Assessment Results` option in `/school/imports`.
- Restore the same Teacher permission and visibility rules that existed immediately before PR #247.
- Restore the same generic Assessment Results template, upload normalization, validation, preview, confirm and history behavior that existed before PR #247.
- Keep the existing Assessments-page `Import assessment results` navigation pointing to the Bulk Data Import page.
- Do not change permissions for School Administrator, Subject Supervisor, Teacher, Student, or any other role outside this restored Assessment Results path.
- Do not redesign or merge the newer assessment-aware XLSX workflow into Bulk Data Import in this restoration.
- Do not alter multi-lesson assessment generation, assessment difficulty distribution, question variants, practice generation, or visual rendering.

### Analytics UX

- Class selectors display the class name only; internal class codes remain internal.
- Replace technical lesson-evidence wording with user-facing language while preserving the existing mastery calculations.
- Do not expose internal outcome identifiers such as `CAM:OUT:...` in user-facing Analytics/PDF output.
- Convert recognized outcome references to a clean label such as `Cambridge 6Nf.11`.
- Replace technical/reference-only outcome descriptions with a concise Edulytics-authored user-facing description while preserving non-technical descriptions.

## Phases

### Phase 1 — Baseline lock and execution plan

- Record this plan in the repository.
- Use the commit immediately before PR #247 as the authoritative baseline for the Assessment Results bulk-import behavior.
- Confirm that only the Assessment Results retirement changes are reverted.

### Phase 2 — Restore Assessment Results bulk import

- Restore `AssessmentResults` support in `MathOnlyImportAdapter`.
- Restore Teacher `AssessmentResults` permission in `DataImportService.CanImportType`.
- Restore the pre-#247 friendly Assessment Results CSV headers and normalization behavior.
- Preserve the existing Bulk Data Import controller/view and all unrelated import permissions.
- Replace the retirement regression test with a restoration regression test.

### Phase 3 — Analytics presentation clean-up

- Remove internal class codes from class filter labels.
- Add a shared Analytics outcome presentation formatter.
- Present recognized Cambridge outcome references as `Cambridge <locator>`.
- Replace technical/reference-only outcome descriptions with user-facing Edulytics wording.
- Replace lesson-evidence technical wording in PDF/UI without changing analytics calculations.

### Phase 4 — Regression and quality gates

- Add/adjust automated tests for:
  - Teacher Assessment Results bulk-import permission;
  - Assessment Results template exposure;
  - preservation of unrelated role/import permissions;
  - class filter labels without internal class codes;
  - outcome-code sanitization and technical-description sanitization;
  - user-facing lesson-evidence wording.
- Run repository CI/build/test/security gates through the pull request.

### Phase 5 — Merge and deployment verification

- Review the final diff for scope containment.
- Merge only after required checks pass.
- Verify the deployed commit and perform focused smoke checks on:
  - Teacher Bulk Data Import;
  - Assessment Results option/template;
  - Analytics class selector;
  - Student Analytics PDF presentation.

## Acceptance criteria

The work is complete only when:

1. A Teacher sees and can use `Assessment Results` in Bulk Data Import exactly as before PR #247.
2. No unrelated role permission changes are introduced.
3. The old Assessment Results import template/normalization/confirm workflow is restored as the baseline.
4. The newer assessment-aware XLSX workflow is not merged into that restored path in this change.
5. Analytics class selectors no longer display internal class codes.
6. User-facing Analytics/PDF output does not expose `CAM:OUT:...`-style internal identifiers.
7. Technical reference-only/copyright implementation wording is not shown to users.
8. Lesson-mastery calculations remain unchanged; only presentation wording changes.
9. Automated regression checks pass.
