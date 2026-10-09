# Phase 11–12: Full-product acceptance run (2026-10-09)

**Execution branch:** `test/phase11-12-production-acceptance-20261009`

## Baseline at start
- Render production application: `srv-dakq5n2fngtc73a62i10`, https://edulytics-4346.onrender.com
- Neon production: `tiny-lab-44877119` / `br-frosty-block-b52tnjky`
- Preflight read-only SQL: 0 users, 0 schools, 5,110 canonical content metadata records, 0 prose translation rows. Therefore authenticated all-role E2E **cannot yet be claimed**.
- Initial Render monitoring: CPU and memory samples available; production HTTP latency/request series sparse. This is a baseline, **not** an egress improvement claim.
- Existing historical curriculum scope audit 62/64 accepted as known nonrelease curriculum gap; no change to original lessons or standards.

## Controlled workflow to validate
1. **Safe fixture setup:** use a disposable isolated PostgreSQL-18 database for destructive and cross-tenant E2E first. Do not provision synthetic identities in production without a documented, reviewed lifecycle and cleanup plan. Confirm that any eventual production fixture data is marked synthetic and never mixed with school customers.
2. **Identity and isolation:** create test superadmin (out-of-band one-time bootstrap), two school tenants, two school admins, two supervisors, four teachers and twenty students. Verify disabled/wrong-role attempts, tenant isolation, session persistence, password reset and access control.
3. **Academic setup:** years/terms/grades/classes, framework adoption, assignments, official outcomes and lesson mapping.
4. **Teacher flows:** lesson content from JSON, teacher-created assessment, questions and outcome mapping, worksheet preview/print/export, assessment assignment.
5. **Student flows:** lesson authorization, practice, adaptive practice, worksheet, assessment submission, explicit result release, student progress.
6. **Supervisor/admin flows:** reports, analytics aggregation and permitted view/export, school isolation.
7. **Security:** cross-tenant request ID switching, direct URL and API access, unauthorized role mutations, CSRF where applicable. Require all negative cases denied.
8. **Performance:** fixed payload A/B/C candidate workloads with sample sizes, warm/cold p50/p95, DB query counts, idle poll frequency, memory, bandwidth and Neon egress/compute measurements. Record timestamps and provider quota periods; do not extrapolate from database byte size alone.
9. **Operational:** controlled restore/backout to a prior Render revision, database compatibility and smoke checks. Never substitute a merge or HTTP 200 for rollback proof.
10. **Acceptance:** capture CI link, test output, production synthetic fixtures/cleanup reconciliation, performance thresholds, owner-approved final disposition and signed Phase 12 report.

## Release invariants
- Lesson bodies remain embedded JSON; production `CurriculumLessonContentTranslations=0`.
- Existing canonical 5,110 lesson identities and outcome links unchanged.
- Do not recreate the old users/schools or import old Neon data.
- Do not silently switch Neon branches or create staging resources.
- Do not claim phases 11–12 completed until authenticated full-cycle tests and usage evidence exist.

## Evidence checklist
| Evidence | Initial status |
|---|---|
| App deploy, ready endpoint and public login | Verified on earlier deployment |
| 5,110 JSON canonical content parity | Passed via GitHub CI |
| Two-tenant synthetic live accounts and role E2E | **Open** |
| Assessments, practice, adaptive, worksheets, results and reports live run | **Open** |
| Actual Neon idle/load egress time-series and p95 | **Open** |
| Rollback drill and cleanup proof | **Open** |
| Final sign-off | **Open** |
