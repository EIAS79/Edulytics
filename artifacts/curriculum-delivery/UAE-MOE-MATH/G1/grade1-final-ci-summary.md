# UAE Grade 1 — Owner-approved Stage D / release candidate

## Academic decision and scope

- Scope: UAE-MOE-MATH Grade 1 Common, 102 Arabic lessons, 11 technical units across three school terms.
- Product owner's academic decision: **APPROVED FOR PUBLICATION OF FIRST-PARTY EDULYTICS CONTENT**, explicitly requested on November 8, 2026.
- Authorization evidence: `grade1-owner-academic-publication-authorization-20261108.json`.
- Approval is NOT UAE Ministry of Education certification or third-party/publisher source clearance.

## Last proven independent technical candidate (before owner authorization commit)

- SHA: `2fea8f8521d4561469477a6ab6332178ef307996`
- GitHub Actions: https://github.com/EIAS79/Edulytics/actions/runs/42515500494
- Full .NET solution Build: **PASS**
- Grade 1 generator, verifier, lesson-route and localized reader tests: **122/122 PASS**
- UAE official reference/policy: **8/8 PASS**
- Ephemeral PostgreSQL migration and seeded curriculum: **PASS**
- Authenticated student pages: **102/102 PASS**
- Stored graded responses: **102/102 correct + 102/102 malformed rejected, 204 total PASS**
- Four authenticated school roles and cross-tenant lesson denial: **PASS**
- Chromium learner HTML/RTL and layout: **102 desktop + 102 mobile PASS**, eight screenshots; full human WCAG audit not certified.
- Technical Stage D with grade-wide academic owner acceptance: **102/102 D_VERIFIED**, with source-equivalence caveat below.

## What has NOT been independently certified

- Source semantic 1:1 numerator/denominator: **UNKNOWN across 102 lessons**; no claims of 100% exercise, example, or figure equivalence.
- Selected student editions: 42 lessons from 2025–2026 Arabic edition and 60 from 2019–2020 Arabic editions. No independent latest-year Arabic student exercise-equivalence claim for lessons 43–102.
- Third-party asset rights: owner's first-party authorship declaration, not blanket authorization to copy externally owned figures/text. A source asset or license review is still applicable if a future lesson includes such materials.
- Human full WCAG/a11y and real classroom learner UAT: not claimed.
- Grade 1 is an independently Edulytics-authored mathematics offering aligned to selected UAE sources; **not** an official textbook reproduction or ministry-certified 2026–2027 identical exercise edition.

## Release control

The owner authorization is recorded, but the new release candidate must pass full solution build, Grade 1 tests, UAE policy tests, existing US presentation regressions, real 102-lesson authenticated HTTPS, all 204 persisted grading attempts and real Chromium desktop/mobile on its exact SHA. These results are never inferred merely from the owner's decision.

**Release deployment status: NOT YET PUBLISHED** until the exact new SHA is tested, merged, and Render reports a live commit. CI source gate documents independently unverified source parity as an explicitly owner-accepted academic limitation, not as a falsified 100% source coverage claim.
