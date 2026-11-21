# UAE Grade 9 General - Stage D / Final CI gate status

- Checked UTC: 2026-11-21T02:51:32Z
- PR #650 branch SHA at ORIGINAL source-entry snapshot: 97e399bee1b898dc112d5b8a5c3942cc47fce598. Current branch SHA must be queried from GitHub; the self-referential commit hash is not embedded in its own commit.
- Final candidate SHA: NOT ESTABLISHED. Live main: 4c50d2ca762cb58bcaa4fd500f465060bad4e36a
- Verified local PDF SHA and page counts: 5/5, including Applied used for comparison only; this does NOT establish full-page element inventories.
- Required A-C handoff PRESENT, but explicitly D_PENDING_A_C_EVIDENCE.
- 86 raw source lessons: 0 full atomic source audits with positive denominator. 82 canonical authoring targets: 0 full 1:1 source parity records. Semantic coverage UNKNOWN.
- Original source-selection ownerTarget was wrongly populated from raw 14/86, now corrected to authoring 13/82; raw source inventory remains 14/86, semantic dedup and Unit 7 equivalence remain unverified.
- Hosted G9 run 44857599478: originally recorded as queued at the INITIAL snapshot, subsequently superseded. Release gate must query latest PR-head workflow; initial queued status is NOT current evidence. Earlier local tests are not final candidate CI or authenticated 82-lesson E2E proof.
- Neon live G9 General: zero lessons and zero nodes. Render live commit: 4c50d2ca762cb58bcaa4fd500f465060bad4e36a. No production writes, merge or deployment.
- Owner academic approval recorded; teacher source sign-off and third-party source asset rights remain separate.

## Exact plan decision

GRADE9_STAGE_D_HANDOFF_GATE = D_PENDING_A_C_EVIDENCE

releaseDecision = NO_GO_SOURCE_100

First lesson U01L01 D_PENDING, next lessons not closed. Grade is 9; Grade 8 field in supplied template is an acknowledged copy error in the handoff.

Local preparation test recheck: 357/357 Node PASS on worktree, source atomic gate NO_GO (exit 1). This is not hosted final CI acceptance.

## D.4 local technical progress (not final CI)

- U01L01 Q48 source printed page 9 uses mm:ss race-time notation. Existing independent lesson/task updated to 14 authored mm:ss results and manual Q1/median/Q3 boxplot input; source numerical results were not copied.
- Real local Microsoft Edge 61-task preview and Arabic mm:ss SVG/chart draft export: PASS (ungraded, local only). Grade9 Node diagnostic 361/361 PASS; scoped native .NET 74/74 PASS.
- Source atomic denominator still unknown and strict gate still NO_GO: 0/86 raw lessons, 0/82 canonical; Stage D entry D_PENDING_A_C_EVIDENCE; Final CI not passed, 82-lesson authenticated E2E not performed.

## Source counts reconciliation (metadata only; Stage D remains pending)

- The historical source-selection file incorrectly populated ownerTarget with raw source TOC 14 units/86 entries. Corrected to the owner-selected A-C handoff scope 13 units/82 lessons, while retaining raw unitCountRaw=14, lessonCountRaw=86, unitCountUnique=null, lessonCountUnique=null, semanticDedupGate=PENDING_NOT_LOCKED.
- The original 14/86 values remain preserved as an explicitly named historical raw-scope record, not removed. No lesson was created, deleted, or certified; source SHA/snapshot unchanged and NO_GO_SOURCE_100 remains.

## U01L01 partial source-element traceability

- Compiled the 61 separately identified numbered exercise containers and 21 known instruction elements on scanned printed pages 5-9 into a nonexhaustive, SHA-grounded source-element manifest. Known partial IDs=82, **not** 82 audited lessons or a verified raw lesson denominator. Exact publisher task parity and independent teacher review remain unverified.

## Local full .NET regression after metadata reconciliation

- Complete local Release binary reuse test produced verified TRX: **17,427 passed, 0 failed, 0 not executed**; process exit 0. SDK 10.0.300 used temporarily on the owner machine; repository global.json restored to 10.0.200. No new C# build performed in this run; no production changes.
- Grade9 prepared Node diagnostics **368/368 passed**. Source gate remains NO_GO (0/86 raw, 0/82 target), hosted current-candidate CI still has no passing result and authenticated final release E2E has not been completed. These local checks do **not** authorize merge/deploy.

## U01L01 printed Q35 correction (Stage D.4 local-only evidence)

- Reopened scanned 01.pdf printed p8/PDF p19. Confirmed Q35 asks to **describe** a fraction of the whole, not take **half** of that fraction. Original source correct model 3/4 of 28 = 21. Existing Edulytics independent student task previously introduced an unasked-for factor 1/2; it now models independent 3/5 of 50 = 30 with two corresponding parts, no duplicated task ID.
- Original SVG/lesson authoring source retained; local Edge 61-task student page displayed corrected Q35 verbatim and exported an ungraded teacher-review response. Q47 bar/Q48 box interactive graph regression also passed, including Arabic mm:ss input.
- 11 explicitly printed a/b/c children under Q34?Q37 mapped with source pages; parent numbered container count remains 61 and known instructional elements 21; total original source atomic denominator remains UNKNOWN, never 82/93 assumed automatically.
- Local Grade9 Node 372/372 PASS, scoped .NET 74/74 PASS verified from TRX. Earlier full .NET suite 17,427/17,427 passed on unchanged C# assembly. Hosted latest candidate CI still unpassed; independent source gate NO_GO (0/86 raw, 0/82 canonical). U01L01 remains D_PENDING, no production writes or merge/deploy.

## Stage D 21 November G9_ARABIC_PATHWAY_GUARD policy repair (local-only acceptance)
- Root: the UAE framework defaults to English and the Grade9 General identity was omitted from student/staff LessonContentService.SelectAcademicContent Arabic selectors. Added the same exact-contract fail-closed policy as Grade7 to student/staff API selection. Draft/unapproved Grade9 records, including old English, stay hidden; no authorization or schema change.
- Local rebuilt Release .NET test suite 17,432 passed, 0 failed, 0 skipped (verified TRX g9-test-results/g9-arabic-pathway-global-regression.trx, not checked into Git); the 11 targeted Grade9/Grade7 policy tests passed. Grade9 prepared Node suite 372/372 passed. Repository SDK pin 10.0.200 restored after local runner 10.0.300 was used.
- The dedicated Grade9 hosted CI workflow now includes UaeGrade9ArabicPathwayPolicyTests AND the existing Grade7 regression in scoped native filter, ensuring the new guard is not omitted. Workflow YAML parses and four workflow-wiring Node tests pass. The strict source gates remain unchanged and fail closed.
- This is NOT full G9_ARABIC_PATHWAY_GUARD acceptance: authenticated G9 student/staff API, negative missing-Arabic published fixture, DOM screenshot, school isolation, all 82 Arabic lesson titles and production HTTPS/E2E are still NOT_RUN. Stage D entry remains D_PENDING_A_C_EVIDENCE, first lesson D_PENDING, source 0/82, release NO_GO_SOURCE_100. No merge, production database writes or Render deploy.

## Stage D G9 82-draft-identity negative pathway test extension
- From the actual checked-in 82-lesson Grade9 General draft pack, exercised all 82 distinct G9 identities in the existing xUnit policy/selector path: neither unverified Arabic drafts nor English fallback may be exposed. Added 82 xUnit cases, giving 93/93 targeted G9+G7 tests PASS from a fresh local Release build; zero failed/skipped and repository SDK pin restored. Verified local TRX g9-test-results/g9-arabic-pathway-all82.trx, not a hosted CI proof.
- Re-read printed lesson 1 Q35 from physical PDF page 19 at high magnification: verb is "????" (describe), NOT "????" (halve), so existing 3/4 ? 28 = 21 analog fix remains correct. No change to its graded math, source numerator/denominator or lesson identity.
- Still NO_GO_SOURCE_100; source 0/82 complete 1:1 audited, Unit7 source conflict open, G9 Arabic guard authenticated API/DOM/tenant checks not run, hosted final CI awaiting runners. No lesson marked D_VERIFIED, merged, or deployed.


## Stage D.4 — U01L01 exact corrective evidence on PR #650 (not release)
- Inspected the existing Arabic RichV2 lesson and A-C checkpoint. Fixed an actual **comparative versus subtraction** misconception: `ن أقل من ٢٠` is a comparison, not inherently the expression `٢٠−ن`. The `أقل من` vocabulary rule is now contextual rather than a blanket subtraction mapping; explicit difference expressions remain `٢٠−ن` and `ن−٢٠`.
- The A-C checkpoint had `sourceElementDenominatorVerified=true` although the partial source inventory records `rawTotalAtomicSourceElementDenominator=null`. Corrected the checkpoint to `false`; documented two printed goals versus four derived competencies. No numerator, denominator, lesson identity, or source certification was fabricated.
- Added regression cases in the existing Grade9 U01L01 RichV2 and handoff Node test files. Independent read-back content assertions: **13/13 PASS** (not Node runner, not CI). Changed-file regression code awaits hosted execution; workflow [44870044832](https://github.com/EIAS79/Edulytics/actions/runs/44870044832) last observed queued. Candidate after code/test edits: `2a8af19bc2b285c661100f53ef4797d1a0d05d49`; this report commit is not itself a tested release SHA.
- Source full atomic parity: **0/86 raw / 0/82 target**, denominator unknown, Unit 7 edition conflict open. U01L01 remains `D_PENDING`; no lesson was closed. `D_PENDING_A_C_EVIDENCE` and `NO_GO_SOURCE_100` remain mandatory. No authenticated 82-lesson E2E, merger, Neon writes, or Render deploy asserted.


## Stage D G9 U01L01 — verified isolated diagnostics (21 Nov 2026)

- **Exact tested checkout SHA:** `5b3fda1a3eee3b4f28b44183c2c5fabd995bf0ef` (PR #650 branch). These are actual local process results, not hosted GitHub Actions / Final CI or authenticated student E2E.
- **Node tests:** 377/377 passed, 0 failed, 0 skipped across 94 prepared G9 files; focused U01L01 32/32. Strict Node source gate remained intentionally red, Python gate exit 1 (0/86 raw independent audits, 0/82 canonical 1:1 parity).
- **.NET:** solution restore/build passed (0 errors, 2 nonblocking xUnit2012 warnings); 167/167 Grade9+Grade7 scoped, then **17,514/17,514** full native tests passed, 0 failed/skipped; local TRX files under `%TEMP%/g9-StageD-TRX/`.
- **SDK constraint:** repository `global.json` pins 10.0.200 but local device has 10.0.300; the *isolated temporary Git checkout only* used 10.0.300. Pin restored after tests and working tree clean. Hosted CI with exact declared SDK remains mandatory.
- **Local Web publish check:** `dotnet publish --no-build --no-restore` succeeded; `Edulytics.Web.dll` present, **no draft G9 HTML/JS** in publishable wwwroot. No Render deployment occurred.
- **Live read-only production inspection:** Neon grade9 General lesson/content/pack-node counts 0/0/0; Render remains live on `4c50d2ca` deploy `dep-dbv8f4nf3r2c73fjf8rg`. PR #650 remains Draft/unmerged.
- **Current hosted gate:** GitHub Actions [#44871005498](https://github.com/EIAS79/Edulytics/actions/runs/44871005498) pending, jobs queued when inspected. **No hosted CI green claim.**
- **Academic release:** owner approval documented; Unit 7 edition/source crosswalk and atomic source inventories not complete, third-party asset rights and authenticated E2E not independently passed. First incomplete lesson remains **U01L01 D_PENDING**; decision **NO_GO_SOURCE_100**, no merge or production writes.
- The ledger/checkpoint/report commits following this tested SHA are evidence-only commits, **not themselves tested release SHAs**.

## Stage D.4 U01L01 sports-career source learning gap (tested Node 92e13181d3)

- Original Arabic professional-context teaching in existing RichV2 now models sports-marketing costs `٢٤م+٣٥`; at `م=٤`, total `١٣١`, with fixed design fee counted once; no publisher photo/QR copied.
- Source structural item `EXTERNAL_SKILLS_CAREER_MEDIA` has an independent locally checked mathematical analogy, but external publisher illustration/QR equivalence is *unverified* and complete lesson/source-element parity remains `UNKNOWN`.
- Actual isolated local Node run at `92e13181d3916f2a7eeb9fa16d7947fc41ae9dc6`: **378/378 PASS**, 94 Grade 9 files, no skipped. Strict Node and Python source gates each exited 1, expected `NO_GO_SOURCE_100` (raw 0/86, target 0/82); do not convert these tests into 100% source coverage.
- Last full 17,514 .NET test pass is anchored to earlier `5b3fda1`; .NET has **not** been rerun after the RichV2-only content edit. Hosted GitHub Actions still not confirmed PASS; authenticated E2E/rights and full source handoff remain blocked. No Neon writes, no Render deploy, no merge.

## U01L01 D.2/D.4 exact technical regression — 385 Node / 167 scoped .NET

- Tested candidate **`93a6f8e0e4701e60f6d5e89d331ae46f6e1168e7`** on isolated local checkout. Node Grade9 prepared **385/385 PASS**, 95 files, 0 skipped/failed. Teacher-oracle regression **7/7 PASS** with all 61 student prompts retained as manual teacher-review activities.
- Incremental .NET build PASS on this exact SHA (diagnostic local SDK 10.0.300; repository pin 10.0.200 restored); Grade9/Grade7 scoped **167/167 PASS** on this exact SHA. Independent full native regression **17,514/17,514 PASS** belongs to earlier `7b5e8664` SHA and is **not** hosted Final CI for latest SHA. Local web publish was also tested on the earlier SHA and had no G9 draft static-asset leak.
- Corrected Q44 teacher rubric for all perimeter distractors; new original U01L01 sports-marketing career algebra; no authoring lesson was recreated, no source photos or linked publisher material copied. Source Q7/Q25 ambiguity, source atomic denominator, teacher sign-off and media rights still unresolved.
- Strict Node and Python source gates both intentionally EXIT 1: 0/86 raw audited, 0/82 canonical one-to-one parity. This is not a code-test pass substitute; first lesson remains `D_PENDING`, release `NO_GO_SOURCE_100`.
- Hosted [GitHub Actions #44873103973](https://github.com/EIAS79/Edulytics/actions/runs/44873103973) last seen pending. No authenticated G9 student/teacher/supervisor/admin E2E, no Neon writes; Render remains `4c50d2ca`, Neon Grade9 General 0/0/0. These report commits after the tested SHA are **evidence-only**, not additionally tested final release refs.


## D.4 U01L01 direct source Q7/Q25/Q41 repair and independent tests — e86c715980

- Direct visual inspection of owner-local scanned `01.pdf`, printed pages 7–9 (PDF pages 18–20), in order. Two source phrase ambiguities (Q7 and Q25) retain uncertified publisher intent, no original source images uploaded. Q41 independent analog explicitly separates the rejected zero algebraic root from positive physical cube-side length. Existing 61 independent teacher-reviewed activities were preserved.
- Latest locally tested **exact SHA** `e86c715980b934f70355ccd9dfc79cd46926b043`: prepared Grade 9 Node **388/388 PASS**, 95 files, 0 failed/skipped. Dedicated strict source Node and Python each exited 1 as required: independent full source audits **0/86**, canonical 1:1 source parity **0/82**. `SOURCE_INDEX_COMPLETE=false`, no full source denominator certified.
- Earlier .NET scoped 167/167 PASS belongs to `93a6f8e0`, and full native 17,514/17,514 PASS to `7b5e8664`: they are not same-SHA Final CI results for this code candidate; no authenticated production student E2E. GitHub Actions [#44874539153](https://github.com/EIAS79/Edulytics/actions/runs/44874539153) was last seen queued.
- Live Render still on `4c50d2ca`; Neon Grade9 General pedagogy/content/nodes 0/0/0. Owner academic permission remains recorded, third-party rights and all required academic gates incomplete. First lesson `D_PENDING`, decision `NO_GO_SOURCE_100`. These audit-only files create a later SHA without reclassifying code test results as passing at the later SHA.
