# American Common Core — authenticated learner/teacher rehearsal (2026-10-11)

## Motivation and boundaries

The existing disposable HTTPS two-school acceptance exercise was Cambridge-only. It proved the shared login and role framework but not that an American Common Core school could adopt the checked-in US curriculum and complete a student/teacher workflow. This change introduces a **distinct US-CCSS smoke job** without changing the production service or database.

## Isolation contract

- The CI fixture still requires `GITHUB_ACTIONS=true`, exact database `edulytics_clean_ci` and localhost PostgreSQL `127.0.0.1:5432`; all credentials are generated randomly and masked.
- The special `EDULYTICS_CI_REHEARSAL_PROGRAMME=US-CCSS` switch is honored **only if the existing disposable CI guards have passed**, selecting `commoncore-primary` and `commoncore-secondary` instead of Cambridge. Permanent-demo and production school selection retain their original paths.
- The separate job seeds canonical curriculum metadata, then two synthetic US schools, runs the real HTTPS ASP.NET application and validates CSRF-protected logins for all four roles, student assessment submission, teacher assessment creation, private Practice generation and cross-school denial of student assessment and Practice attempt IDs.
- The existing Cambridge two-school acceptance job remains triggered by the edited shared script and retains the original `CAMBRIDGE` default.
- No authentication credentials are printed. No production or Neon write is permitted. Any failure in US rehearsal fails CI and blocks the pull request.

## Academic status

Authenticated workflow evidence and exact Practice generation are distinct from Common Core mathematical standards certification. The 121 child clauses remain `NOT_FULLY_CERTIFIED`; 106 have publisher child-code evidence and 15 are parent-only. Graph drawing, original written justification, vector construction and official clause semantic signoff remain open. Live staging account testing still requires isolated, explicitly authorized test users there; **this CI job does not claim to use staging identities**.
