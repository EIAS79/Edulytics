# Grade 10 Stage D / Final CI

**Release decision: NO_GO_TECHNICAL. No merge or production deployment.**

- Owner academic authorization: recorded and accepted as owner decision in `grade10-owner-academic-authorization-20261106.json`; separate third-party asset rights status NOT VERIFIED.
- Current candidate code SHA: `2a7129acdbb7722e3b4314a27398acc04202a564`.
- Last fully observed Grade 10 candidate SHA: `df4723500c034fd634cc8406fd98d1edade0c977` (previous D CI RUN `42129708817`, 121/121 .NET targeted PASS and D1/D2/D3 Node PASS, with source and route jobs FAIL).
- GitHub Actions on current head: NOT OBSERVED at time of record; do not transfer old PASS to the new SHA.
- Grade 10 authenticated HTTPS regression workflow added: `.github/workflows/us-ccss-g10-geometry-https.yml`; results pending observation.
- Local source inventory: 496/496 HTML documents present, 7,480 headings and 3,067 distinct-within-page image references; **no asset bytes or rights clearance inferred**.
- Activity identifier crosswalk: 382 of 498 source teacher activity IDs referenced by A-C checkpoint/assessment maps; 116 not explicitly referenced. Identifier reference is not semantic equivalence.
- Source student Practice: 955 problem headings inventoried; individual first-party equivalents NOT VERIFIED.
- Runtime Practice: 0/124 exact `READY_VERIFIED` Geometry mappings on last checked registry; teacher-scored submission/persistence not yet verified as an alternative.
- Lessons D_VERIFIED: 0/124. Human academic owner approval alone does not indicate student/teacher operational readiness.
- Blocker: https://github.com/EIAS79/Edulytics/issues/623
- PR: https://github.com/EIAS79/Edulytics/pull/583

Latest reviewed source-visual audit: 65 RichV2 structured visuals with zero external visual URLs; this is not graphic/semantic equivalence. Some learner-facing lesson paragraphs still include internal source/editorial notes.

Local latest candidate `ea94f9a891f657dae83e30fed9b2ff0d97234f10`: 133 Geometry Node files = 885/885 PASS; .NET 121/121 targeted PASS; pinned SDK 10.0.200 CiFixtures/Web build PASS 0 warnings/errors. See `grade10-stage-d-local-build-and-tests-20261106.json`. These are not authenticated runtime or final GitHub checks.
