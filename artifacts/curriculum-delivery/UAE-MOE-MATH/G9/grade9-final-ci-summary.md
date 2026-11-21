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
