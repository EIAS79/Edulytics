# American Supervisor 3 / Coder 9 — 2026-10-29 release-gate handoff

**Course:** `US-CCSS-MATH` / `CCSSM-2010` / `ADV-TRIG-GEO` / `Fourth Courses`. **Unit:** one; **lesson IDs:** `PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L01` through `L05`.

- Repository: `EIAS79/Edulytics`
- Draft PR: https://github.com/EIAS79/Edulytics/pull/586
- Branch: `us-ccss/supervisor3/coder9-adv-trig-geo`
- Original main base: `39c757630d2bd67217a1468fce099653ed0f3f53`
- **Content and test evidence freeze SHA:** `d63bc51ceba7c7233119eae8545f7f75d87d3491`
- Subsequent commits only update review/evidence files. **Current branch head should be checked at handoff**, never presume this text's SHA remains current.
- **State:** `CONTENT_IMPLEMENTED`, `TESTED_FOCUSED_LOCAL`, `SOURCE_TEXT_VERIFIED_WITH_ACADEMIC_EQUIVALENCE_PENDING`, `PRACTICE_BLOCKED_BY_INTEGRATION_OWNER`, `CI_PARITY_BLOCKED`, `RELEASE_BLOCKED`.

## Exact executed tests

**Our-CS, isolated disposable git clone; original user worktrees unchanged.**

1. `node --test tools/curriculum/coder-9/UsCcssHsADVTRIGGEO5LESSONS.test.cjs tools/curriculum/coder-9/UsCcssHsADVTRIGGEOPracticeFixtures.test.cjs tools/curriculum/coder-9/UsCcssHsADVTRIGGEOSourceTasks.test.cjs` — **20/20 PASS**, 0 failures, exit `0`, exact content/test SHA `d63bc51ceba7c7233119eae8545f7f75d87d3491`.
2. `dotnet test <absolute isolated checkout>/tests/Edulytics.Tests/Edulytics.Tests.csproj --configuration Release --filter FullyQualifiedName~Phase29CanonicalContentPackPipelineTests --verbosity minimal --no-restore` — **17/17 PASS**, 0 failures, exit `0`, `net10.0`, same SHA `d63bc51ceba7c7233119eae8545f7f75d87d3491`.
3. Repository `global.json` requests `10.0.200`, unavailable on Our-CS. The SDK was selected with a **separate temporary external `global.json`** pointing at locally installed `10.0.301`; the repository's `global.json` was **not changed**. First attempt with the repository pin could not start. All reported .NET PASS values are from the actual successful 10.0.301 run.

Tests are **scoped**, not exhaustive full solution CI or user-login/effective Practice validation. No database-backed protected/authenticated E2E was run locally.

## Direct original-source comparisons

Four primary MVP PDF binaries, covering five lessons, were downloaded on Our-CS into an isolated source directory and individually SHA-256 checked against the canonical pack's `sourceSha256`. All **4/4 matches**; original pages and teacher notes read directly (not just URL/metadata). The sole content pack retained original source hashes/IDs and original `canonicalBodySha256` as historical records; the revised body bytes are separately pinned.

| Lesson | Original PDF student/teacher pages | Target; source-specific first-party learner repairs | Academic unresolved |
|---|---|---|---|
| L01 Maintaining Your Identity | `sec3mod7tn718.pdf` 50–53 | `HSF-TF.A.3/.4`; unit-circle cofunction, odd/even, `π−x,π+x,2π−x`, exact special angles and Pythagorean identity; diagram-independent original worked example | Original source diagrams and publisher equivalence not independently signed off |
| L02 Amazing Inverse Trig Function Race | `sec3mod7seh718.pdf` 54–59 | `HSF-TF.B.6`; monotonic intervals/principal inverse, two-clue quadrant/radius location, multiple/contradictory location cases | Publisher game/card/graph equivalence not certified |
| L03 Hidden Identities | `sec3mod7tn718.pdf` 59–62 | `HSF-TF.B.7`; original identity+inverse+graph approaches, doubled-frequency four-root example, independent signal time-model `{1s,5s}` | Original publisher task is equation-focused; independently authored real-world modeling extension requires academic approval and a real technology/graph workflow |
| L04 Cavelieri to the Rescue | `m2_mod7_tn_52017f.pdf` 105–111 | `HSG-GMD.A.2`; equal-base/height triangle, sheared cylindrical layer volumes, all corresponding cross sections, sphere volume from hemisphere/cylinder-minus-cone | Source calls itself `7.12 Cavalieri`; canonical `Cavelieri` and original `M05:T12` remain unchanged pending source/blueprint governance, figures not copied |
| L05 Operating on a Shoestring | `g1_mod6h_tn_82017f.pdf` 81–88 | `HSG-GPE.A.3`; constant focal-distance sum for ellipse, string/focal variation, vertical and translated ellipses, independently authored hyperbola difference | MVP 6.10H ellipse-specific; its 6.11H hyperbola is a separate task. Hyperbola standard extension is first-party, **not** source equivalence |

See `ADV-TRIG-GEO-source-target-practice-review.md` for task-level comparisons and `ADV-TRIG-GEO-proposed-practice-fixtures.json` for concrete expected, equivalent, incorrect, malformed, excluded-domain and human-rubric cases. Test fixture records **do not create runtime Practice generators**. Original PDF figure assets have **not** been reused; attribution and independent figure rights still need academic/legal review.

## Parity failure and exact transition package

- Old [V2 Original Curriculum and JSON Parity](https://github.com/EIAS79/Edulytics/actions/runs/40775253233) failed at `MAIN_TO_V2_PARITY original=5110 matched=5110 missing=0 changedBodies=5 newlyApproved=0 release=NO_GO`; five unapproved lesson metadata transitions were also reported.
- Old [US CCSS Authenticated Academic Acceptance](https://github.com/EIAS79/Edulytics/actions/runs/40775253144) failed at embedded content parity; actual two-school/Practice jobs were **skipped**.
- `ADV-TRIG-GEO-content-predecessor-transition.json` contains previous/new literal field SHA-256 digests for **all 30 English learner fields**, old/new translation digests, exact original and revised commit anchors, source hash checks, source/metadata invariance, and required change authorization. **Version `phase30-common-core-rich-content-v1` is still unchanged.**
- No self-authorization, hash rewrites, source SHA changes, shared seeder patches, production database operations or parity-test weakening. **One designated global owner** must decide an approved new content version and exact guarded predecessor/materialization transition based on actual persisted values, not merely GitHub JSON.

## Integration-owner actions and dependency status

1. **Global content integration owner (unassigned in this Coder chat):** authorize source/academic transition, versioning, baseline/old persisted predecessor, materializer/seed safe migration and required parity registration. Resolve shared path collisions with Cambridge #399, UAE #394, US #492/#553. Keep any shared patch outside Coder 9's course-only PR unless explicitly assigned.
2. **Global Practice integration owner:** issue [#587](https://github.com/EIAS79/Edulytics/issues/587): exact lesson generator/family, diverse questions, normalizer/verifier, feedback/persistence, malformed/domain handling and teacher rubric. Generic `trigonometry`/`solid-geometry`/`coordinates` mapping alone cannot pass.
3. **School/course adoption owner:** issue [#453](https://github.com/EIAS79/Edulytics/issues/453): persist and route exact `Fourth Courses` + `ADV-TRIG-GEO` choice; authenticated student+teacher role, two disposable schools, five lessons, negative cross-school/cross-course isolation, actual attempts, feedback and persistence.
4. **Academic standards/source reviewer:** approve or reject source task treatment for L03 modeling and L05 first-party hyperbola extension; confirm L04 source spelling/module mapping, CC BY 4.0 attribution and any third-party figures. No approval inferred from test pass.
5. **Protected release owner:** only after a single integrated frozen HEAD, run all required GitHub checks, verify actual deployed SHA and real end-to-end content materialization, rollback plan and owner UAT. Do not directly merge/deploy this draft.

**Rollback constraint:** If the reviewed content transition is rejected, retain the immutable old source/persisted-version evidence and use a guarded rollback only through the designated owner; do not delete or mass-reseed school content. Historical `main` and all other curricula remain untouched.

**Final release recommendation:** `NO_GO` until the above shared and academic dependencies are demonstrably resolved. The course-specific branch and draft PR preserve the complete first-party teaching and scoped regression artifacts for integration, not a claim of a production release.
