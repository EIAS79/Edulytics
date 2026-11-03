# Edulytiks curriculum source guard — user-pinned IM First Edition 2021 (2026-11-03)

## MANDATORY SOURCE OF TRUTH — newest owner directive (overrides the 2026-11-01 historical-source rules below)

- **Source root:** `C:\Users\khali\Downloads\New folder (2)\Common Core\Illustrative_Mathematics_First_Edition_2021`.
- Academic claims of “Illustrative Mathematics First Edition 2021” MUST be based on actual source pages/files from that exact folder, not the sibling `Illustrative_Mathematics_v360`, older `06_Grade_6_OUR_IM`, or inferred curriculum content. Never treat a compatible topic/title as proof of edition identity.
- **Verified on Our-CS (2026-11-03):** this pinned folder has IM K–5 sources for Kindergarten through Grade 5. It has **no Grade 6 directory**. At the sibling Common Core root, `06_Grade_6_OUR_IM` exists, but it is a different older middle-school source and is **not authorized as a substitute for 2021**.
- **Grade 6 fail-closed rule:** PR #591 currently targets 147 Grade 6 OUR/IM lessons over nine units, based on different archive lineage. Existing commits, lesson-specific engines, and the historical U01:L01 authoring closure must be preserved, but their alignment with the pinned 2021 source is **NOT_VERIFIED**. Do not label any Grade 6 lesson `REVIEW_COMPLETE_2021`, `READY_FOR_STAGE_D`, `SOURCE_2021_MATCHED`, or equivalent without verifiable exact-edition Grade 6 source evidence. Do not infer Grade 6 has 148 lessons/8 units just because a K–5 plan says so.
- Freeze further source-alignment changes, selected-Practice shared-file writes, Stage A–B–C closure and movement to U01:L03 for Grade 6 until the actual owner-approved Grade 6 2021 course/source identity and matching lesson artifacts are verified. The owner-required source folder is the decisive acceptance gate; there is no fallback to the older archive.
- This is a documentation/source-integrity guard, **not** a destructive reset, deletion or claim that any prior runtime functionality was tested or deployed. GitHub Issue #605 records release of the former three-file lease without changes on 2026-11-03.

## Historical execution notes — retained as implementation history, NOT 2021-source authority

The original Grade 6 course branch `curriculum/us-ccss-g6-full-grade-20261028` / PR #591 contains earlier OUR/IM Middle School Grade 6 work based on `C:\Users\khali\Downloads\New folder (2)\Common Core\06_Grade_6_OUR_IM`. That historical source is separate from the now-required 2021 folder. The prior plan's identification of Grade 6 as IM K–5 2021 / 148 lessons / eight units was also unsupported by the available Grade 6 files; neither historical claim overrides the pinned-source gate.

1. Preserve existing grade-owned and shared code, full Git history, and the historical U01:L01 evidence; do not reset, force push, or merge unrelated course branches.
2. When—and only when—the correct 2021 Grade 6 source is available and positively mapped, begin with the first 2021-unverified lesson. Inspect source teacher/student goals, worked examples, activities, images and problems, repair proven defects only, and preserve correct functionality.
3. Implement Practice by reusing or narrowly extending proven math kernels and lesson-local adapters. Non-numeric constructions and mathematical explanations require teacher review. Do not fabricate publisher Cool-down text.
4. Author targeted Stage A/B/C fixtures, but follow the existing project plan's intentional AUTHORED_NOT_RUN policy. Do not claim functional-test success, Stage D, Final CI or production deployment.
5. For global shared files, obtain fresh exact single-writer ownership under Issue #605. No previous lease is automatically renewed. Do not advance to the next lesson without documented and source-verified closure.
6. Grade 6 U01:L02 tangram work at PR #591 HEAD `fffae6bf95ba743bbbc9fffa4bf874598afdb4c3` remains an unclosed historical candidate, **not** 2021-approved material. No `READY_FOR_STAGE_D` or `REVIEW_COMPLETE_2021` claim is permitted on that basis.
