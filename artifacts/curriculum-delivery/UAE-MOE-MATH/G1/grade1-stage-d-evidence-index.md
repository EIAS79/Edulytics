# UAE Grade 1 Stage D — Live evidence index

- Scope: 102 existing Grade 1/Common Arabic Blueprint lessons, 11 technical units; 10 numbered printed units.
- Stage A–B–C base SHA: 78bded7ad1427e08c345455d44d3110fdc27efb1
- Handoff source snapshot: sha256:571ca1f4d8687100dfec0c239a4c9718f0104a5b65d81731398de6d21e1f1718
- ContentVersion: p29-uae-g1-common-t1-ogl-v1-v1
- Stage D branch: curriculum/uae-g1-stage-d-20261108
- Stage D workflow: .github/workflows/uae-g1-stage-d.yml
- Technical tests: **NOT_RUN** at ledger creation.
- Authenticated four-role E2E, isolated DB and Arabic RTL/visual/accessibility: **NOT_RUN**.
- Semantic source numerator/denominator: **UNKNOWN for 102/102 lessons**.
- Newer Arabic student edition parity for lessons 43–102: **UNVERIFIED**; 2019–2020 source editions retained and disclosed.
- Human and rights review: **NOT_VERIFIED**. Existing owner academic approval does not waive rights and source-version gate.
- Initial candidateSHA: **UNSET**; GitHub Actions run(s) on a fresh exact ref will be recorded later, not fabricated.
- Stage D initial state: **D_PENDING_A_C_EVIDENCE** for 102/102; this is independent of recorded ABC READY_FOR_STAGE_D.
- Release: **NO_GO_TECHNICAL** until actual compiler, grader tests, isolated multi-role E2E, RTL, CI, semantic/edition and rights gates.

The Stage D gate is fail-closed. The structural job may pass while the separate academic-source handoff and final release gates correctly fail. A green isolated .NET test job alone does not constitute full-grade D verification. Preserve last stable A–B–C branch and do not merge or deploy.

## First actual GitHub-hosted Stage D attempt
- Run ID 42508673780 on SHA b806fa6e37196a9cd7e2b6d911a6e83c7af4ee6b.
- Structural Arabic/source identity checks PASS; source-element parity gate FAIL CLOSED (unknown denominator/newer student volumes).
- Restore PASS; full solution Build FAILED due CS0103 / CS0136 / CS0165 in UAE Grade 1 verifier, so subsequent targeted tests SKIPPED.
- Repair: change eight UAE Unit4 verifier branches to use actual parameters dictionary, and rename shadowed parser variable from value to parsedUaeValue. No test is declared passed on the new repair SHA.
- Link: https://github.com/EIAS79/Edulytics/actions/runs/42508673780
- Final gate remains NO_GO; authenticated multi-role E2E not run.

## Second actual GitHub-hosted Stage D attempt
- Run 42508979016, exact branch SHA 42a885219ba718e5494c42f1b758cf3b95adb8a5.
- Restore PASS and full .NET solution Build PASS.
- Grade 1 focused .NET tests: 110 executed; **47 PASS, 63 FAIL**, no skipped tests in selected suite. All 63 were first route-resolution assertions for new lesson families, not yet math correctness outcomes.
- Confirmed source: official Practice projection chose the new Arabic academic translation while reviewed target-rule title patterns were anchored to original English metadata, even though each rule has an exact LessonCode anchor.
- Repair: preserve Arabic as canonical, attempt reviewed exact-code English metadata only for scoped Practice routing after canonical lookup fails. Preserve fail-closed skill/family and code-anchor validation.
- Source evidence gate remains BLOCKED; authenticated E2E remains NOT_RUN. No D_VERIFIED claims.
- Run URL: https://github.com/EIAS79/Edulytics/actions/runs/42508979016

## Third actual GitHub-hosted Stage D attempt
- Run ID 42509613157, tested SHA 716780e3e2e80a3f0345985a316ea4caadbfd2a5.
- Restore and full solution build PASS; 110 focused tests executed: 91 PASS, 19 FAIL. The 19 failures include narrow finite exact question pools and two unresolved English metadata route anchors. Official source parity still blocked; E2E/RTL pending.
- This commit expands genuinely different source skill cases (two-sided ordinals, ten-frames and tens/coins, shapes, halves/quarters, repeated solids and patterns) instead of lowering tests, fixes two exact anchored legacy English title regexes and retains Arabic as canonical.
- Test repair on this new SHA must be re-executed; no new PASS is claimed.
- Evidence: https://github.com/EIAS79/Edulytics/actions/runs/42509613157

## Fourth actual GitHub-hosted Stage D attempt
- Run ID 42510442246 on source SHA acc20b2fa9fa30ab173d0df197ebcc713abd12b6.
- Restore PASS, full solution Build PASS; 110 focused Grade 1 tests executed: **108 PASS, 2 FAIL**. Finite pool defects from prior attempt repaired; the two failures are original lesson 56 and 88 exact Practice route resolution.
- Scope of corrective patch: allow one and only one runtime-ready reviewed rule with a literal anchored LessonCode for UAE Grade 1, after Arabic canonical metadata routing fails. No arbitrary keyword/title fallback or test count reduction.
- New patch requires actual GitHub-hosted retest; it is not pre-marked PASS.
- Evidence: https://github.com/EIAS79/Edulytics/actions/runs/42510442246

## Fifth actual GitHub-hosted Stage D attempt — independent technical gates green
- SHA: 57553141a4be9974c076f97aec2caf589f73d9c2. Run: 42510784139 (push).
- Solution Build PASS. Grade 1 family/verifier suite: **110/110 PASS**. UAE policy/reference checks: **8/8 PASS**.
- 102-lesson structural source identity and Arabic canonical content gate PASS; source-element parity gate remains FAIL CLOSED because lesson-specific source-element denominators are not independently evidenced.
- New CI-only authenticated E2E job created to test 102 actual Arabic student pages and 204 persisted practice attempts on an ephemeral PostgreSQL server, using a dedicated UAE Grade 1 primary school and cross-school control. Results **NOT_RUN** until new commit workflow.
- Human Arabic RTL/a11y/browser screenshots, copyright/source editions, owner release rights and real deployment **UNVERIFIED**; no D_VERIFIED.
- Evidence: https://github.com/EIAS79/Edulytics/actions/runs/42510784139

## Sixth actual GitHub-hosted Stage D attempt — isolated HTTPS surfaced Arabic display bug
- Run: 42511577450. SHA: 80c3813ac18d5dfa2dbee7b2ffe1ff8863366eec.
- Full solution Build, 110 Grade 1 targeted exact tests and 8 reference/policy tests remained PASS.
- Disposable PostgreSQL 18 migration and clean seed PASS (5183 catalogue lessons); isolated UAE and unrelated foreign school fixtures PASS. Four authenticated role logins and student HTTP 200 first lesson confirmed.
- **FAILED** when the learner page did not show the Arabic canonical title for lesson 1; 102 pages/204 persisted grades NOT executed. Failure trace is an actual source-language selection bug.
- Root fix: scope academic language to UAE Grade 1 Common ar in LessonContentService, while preserving en for unaffected UAE levels; suppress English RichV2 fallback and render all six authored Arabic canonical teaching sections inside RTL wrapper. Test assertion updated to reject missing Arabic title/sections and foreign leakage.
- Final gate changed from unconditional failure to evidence-driven fail-closed conditions; absent source parity, human/rights approvals, RTL browser evidence still block publication.
- Source evidence gate remains FAIL; no D_VERIFIED or publication.
- Evidence: https://github.com/EIAS79/Edulytics/actions/runs/42511577450

## Seventh actual isolated CI run — 102 Arabic pages and 204 persisted grades PASS
- Tested SHA: 430453453cbb2eab6f672b99419e9dbe65b4f9b9. Run ID: 42512661110. Run URL: https://github.com/EIAS79/Edulytics/actions/runs/42512661110
- Full solution Build PASS; 110/110 UAE Grade 1 exact questions/verifier tests PASS; 8/8 UAE reference/policy tests PASS.
- Disposable PostgreSQL 18 migrations and clean seed PASS, 5183 catalogue lessons; 2 isolated school tenants seeded.
- Authenticated student, teacher, supervisor and admin logins PASS. Foreign-school student's access to this learner lesson denied.
- All 102 authenticated canonical Arabic learner pages serve six authored teaching sections under ar/RTL wrapper PASS, recorded by 102 independent page checks.
- 102 server-owned correctly answered Practice questions persisted with correct grading, plus 102 malformed responses persisted as grade zero; 204 real HTTPS/DB attempts PASS.
- Evidence artifacts: uae-g1-stage-d-102-authenticated-https-42512661110, uae-g1-stage-d-dotnet-42512661110.
- Source exercise-level numerator/denominator remains UNKNOWN for 102/102, and newer Arabic T2/T3 student exercise editions remain unverified.
- Browser screenshot, responsive RTL, WCAG/a11y, real classroom UAT, current-edition source parity, human/rights approval and production rollout remain NOT VERIFIED.
- Stage D lesson final classification stays 0/102 D_VERIFIED; recorded per-lesson technical test PASS is not academic clearance.
- Final gate deliberately returned NO_GO because of missing independent evidence.

## Eighth actual GitHub-hosted Stage D run — real Chromium automation passed; reviewer found visual defects
- GitHub Run 42513905658; tested SHA 0db86200383982c53aea617000bfa922835f0b25; https://github.com/EIAS79/Edulytics/actions/runs/42513905658
- Full solution Build and 118 selected .NET tests PASS; 102/102 authenticated Arabic lesson pages and 204/204 persisted Practice correct/malformed grades PASS.
- Actual Playwright Chromium browser **102/102 desktop (1280px)** and **102/102 mobile (390px)** course reader checks PASS; all 8 sample full-page screenshots downloaded and verified with artifact SHA-256.
- Manual inspection of screenshots FOUND English section headings, English Understand–Represent–Check generic concept diagrams, and an unconfigured YouTube discovery pane on Arabic lessons. Therefore visual acceptance **NOT APPROVED** even though automated RTL DOM passed.
- Scope-specific correction authored: Arabic section/navigation headings; lesson parser option stops generic fallback visuals only for UAE G1; suppress unconfigured YouTube on G1. Other programmes' render defaults retained.
- Expanded next run Chromium assertions require Arabic headings and no generic diagram/broken video pane; reviewer has NOT approved that change until the new candidate passes and screenshots are inspected.
- Semantic source exercise denominators still UNKNOWN, newer T2/T3 Arabic student editions not verified, and no rights/human release approval. **NO_GO**.

## Ninth isolated CI run — Arabic lesson sections fixed; hero metadata still required review
- Run [#42514953244](https://github.com/EIAS79/Edulytics/actions/runs/42514953244), tested SHA `6b85e026bd1dfea61d3919684cc195a281488d04`.
- Solution Build / 118 previously selected .NET checks PASS, 102 authenticated student pages and 204 persisted Practice grading probes PASS, Chromium RTL 102 desktop + 102 mobile PASS.
- The real captured screenshots confirm all six core section headings in Arabic, generic English fallback teaching diagrams removed, and unconfigured YouTube discovery removed for UAE Grade 1.
- Remaining visual issue found in images: hero metadata included English lesson/unit subtitle, grade/programme labels; scoped Arabic hero correction authored after this run and requires its own new SHA/CI evidence. Global student navigation follows user UI language settings and is not represented as a canonical Arabic teaching section.
- Historical 2019–2020 term-2/3 student source gap and 102 source exercise-element parity unknown; no rights, human academic approval, or D_VERIFIED.
