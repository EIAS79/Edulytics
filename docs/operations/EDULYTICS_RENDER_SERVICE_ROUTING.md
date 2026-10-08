# Edulytics production routing and release gates

Verified from Render service metadata on 2026-10-08. Repository: `EIAS79/Edulytics`.

| Purpose | Render service | Service ID | Runtime | URL | Deploy trigger |
| --- | --- | --- | --- | --- | --- |
| Public marketing/frontdoor website | Edulytics Public | `srv-dased717lnhs738r0q1g` | Static site; `bash public-frontdoor/build.sh`; publish `public-frontdoor/dist` | https://edulytics-public.onrender.com | `main` commits |
| Logged-in learning application and API | Edulytics | `srv-dakq5n2fngtc73a62i10` | Docker; `./Dockerfile` | https://edulytics-4346.onrender.com | `main` after passing checks |

**Do not confuse the two services.** Both watch the same GitHub repository and `main` branch; a merged curriculum commit may cause both to rebuild, even when only the app curriculum was changed. Do not manually trigger deploys while auto-deploy is enabled. Keep curriculum changes in a feature branch until reviews and CI pass.

## Observed production blocker (2026-10-08)

- Most recent public site deployment listed as `live` (commit `18cd29f6b97583f90f10e416cd1d2c6c7f051e6c`).
- Recent application deployments had status `update_failed`; Render events repeatedly reported instance process exit code 1.
- Application logs show database migrations executed before web startup, then PostgreSQL SQLSTATE `53000` from the Neon endpoint: `Your account or project has exceeded the quota. Upgrade your plan to increase limits.`
- This indicates a database-provider quota/billing restriction, **not a proven curriculum-code defect**. Production publishing is blocked until database capacity/access is restored, a deployment succeeds, and application health checks pass.
- Do not change connection strings, provider plan, secrets, or database migration behavior to mask the provider-side quota. Changes requiring billing must be explicitly approved by the account owner.

## Cambridge curriculum release checklist

1. Validate primary Stage 1–9 and secondary IGCSE/AS-A Level learner-book-to-lesson section crosswalk, with independent coverage and depth evidence.
2. Preserve approved Cambridge official outcomes and existing accepted mapping schema (`OutcomeCodes`, `Alignments`, and other valid course-specific fields); introduce no synthetic identifiers.
3. For split lessons, provide original explanations, worked examples, common errors, lesson-specific practice, assessments and accessibility-appropriate visuals. Do not reproduce copyrighted book body text.
4. Verify blueprint/content parity, lesson sequence, official mappings, seeding, practice/assessment links and academic review; fix failures before merge.
5. Run full repo lint, relevant .NET tests, database migration checks, security gates and CI.
6. Confirm Neon is operational; merge to `main` only when green. Verify *both* Render services' resulting deploys and logged-in application smoke tests. Never represent a branch commit as a successful production deployment.
