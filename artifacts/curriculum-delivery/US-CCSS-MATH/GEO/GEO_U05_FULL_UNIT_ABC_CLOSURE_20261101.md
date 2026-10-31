# Geometry U05 — consolidated Stage A–B–C authoring closure (2026-11-01)

**Repository:** EIAS79/Edulytics
**Original course PR:** #583 (Draft; no merge)
**Branch:** `curriculum/us-ccss-geo-coder6-source-repairs-20261028`
**Canonical manifest:** Unit 5 = **18** lessons, from `U05:L01` to `U05:L18`.
**Unit academic authoring closures:** **18/18**; total course **82/124**; remaining **42**.
**Stage D:** `NOT_RUN`; CI/build/E2E/merge/deploy: **not executed**.

## Individual lesson content and evidence commits (same original PR)

| Lesson | Title | Implementation commit |
|---|---|---|
| U05:L01 | Solids of Rotation | `9b514cecb40735565f3b699f8739ac5b72359b89` |
| U05:L02 | Slicing Solids | `0e6d8cb377c8e8f584337dcc59344386a997690c` |
| U05:L03 | Creating Cross Sections by Dilating | `e1d2efbcd45f624457bcf563fd7c4d7491cb6c5b` |
| U05:L04 | Scaling and Area | `f03c4261afe6b2299dda6344995a43ac5f5001c1` |
| U05:L05 | Scaling and Unscaling | `2a2acdc19b33cbbf2e922b2ce6d1f335917ad325` |
| U05:L06 | Scaling Solids | `965dea751df7c802321f7cfb2be1673cbcb55eca` |
| U05:L07 | The Root of the Problem | `ea09fdb13d068ce3cab0a64c1e4d8d79c4d583f6` |
| U05:L08 | Speaking of Scaling | `9aafaa35453f6f8f67c8ab24e0df106d8d8d1b06` |
| U05:L09 | Cylinder Volumes | `8afbbb66d88dc3c1d247d36783e26c8c67270981` |
| U05:L10 | Cross Sections and Volume | `8cdb3a234d07d217d866454c6fb690c2d48ad7a6` |
| U05:L11 | Prisms Practice | `5d1da3d97fd9ed32c1b6b7089c9c6f41f5d10b3d` |
| U05:L12 | Prisms and Pyramids | `b56bae8b9d3dc2b0498c5c0e97e85719543899a8` |
| U05:L13 | Building a Volume Formula for a Pyramid | `10ee8cf501e73170c0d0f982a3310c28cfae40e4` |
| U05:L14 | Working with Pyramids | `d84e69f37e255a7299315d02efcd1befcd6a78a7` |
| U05:L15 | Putting All the Solids Together | `a57d774f63fcf11fe646a98d942e7ccbdf6e0876` |
| U05:L16 | Surface Area and Volume | `aebbe27f5d766cb617c97871386c27aefac414cb` |
| U05:L17 | Volume and Density | `a5689e531d0eabe75bebb94624fc303b7a494d4a` |
| U05:L18 | Volume and Graphing | `117b72368bc3d9dff06a468b8760c71fb45b856f` |

**Final lesson closure commit:** `2e6a789348db305122f3d8d586b0de749d43a290` (U05:L18). Per-lesson closure SHA and source SHA-256 evidence are retained in the ordered ledger and individual U05 checkpoints.

## A–B–C evidence boundary

- **A:** Each lesson is crosswalked to original locally preserved teacher preparation, teacher activities, student activities and Practice; SHA-256 values stored in each respective lesson checkpoint. Source-only image labels and locked cooldown remain teacher-review / not verified rather than invented.
- **B:** Lesson-owned RichV2 source-aligned additions preserve preexisting correct worked examples. Source-specific mathematical errors/ambiguities are addressed in lesson-owned materials. Shared contracts/engines/renderer and other courses were not edited.
- **C:** Lesson-selected, human-teacher-scored first-party assessment routes with 4×0/1/2 rubrics and authored correct/equivalent/wrong/boundary/malformed fixtures. Lessons L09–L18 include student printable worksheet markdown explicitly linked to their assessment contracts.
- **Checks:** static JSON validation, authoring-file JavaScript syntax check, and Git staged diff check; authored tests remain `NOT_RUN`, not Stage D proof.
- **Unverified:** publisher-protected originals/graphics, runtime grade persistence, tenant permission security, broader UI wiring/production acceptance and all Stage D tests. Teacher-grade Stage D ownership remains outside this unit closure.

**Metadata corrected:** ledger `Coverage.UnitLessonCount` now reflects canonical U06 count 17 after U05 completion (U05 was 18); `Coverage.NextUnit=U06`, `Coverage.CurrentLesson=...:U06:L01`. Previous original stale U05 `16` and `U03` were corrected at U05:L09 closure.

**Next active canonical lesson:** `PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U06:L01`. This report is unit-level academic authoring closure only, not production verification or permission to merge.
