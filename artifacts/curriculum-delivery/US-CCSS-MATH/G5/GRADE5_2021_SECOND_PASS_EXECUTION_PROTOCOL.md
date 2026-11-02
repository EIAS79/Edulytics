# Grade 5 — IM First Edition 2021 — SECOND A–B–C REVIEW PASS
## Owner-approved operating protocol — 2026-11-02

**Scope:** EIAS79/Edulytics, draft PR #596, exactly one course: Illustrative Mathematics — IM K–5 Grade 5 First Edition (2021), 8 units / 148 lessons. **Do not use Illustrative_Mathematics_v360 as source.**

### Two INDEPENDENT lesson statuses
1. `READY_FOR_STAGE_D` / `ready for stage D`: historical first-pass A–B–C authoring handoff, already recorded for 148 lessons. **Preserve its existing value.** It is NOT proof that 2021-only second-pass review occurred.
2. `REVIEW_COMPLETE_2021` (display label: **REVIEW compelete 2021**, owner spelling retained): **new, independent second-pass completion status, initially NOT COMPLETE for every lesson** until a verifiable second-pass 2021 source comparison, necessary corrections, and closure evidence are individually recorded. Do not bulk-infer or import completion from old `READY_FOR_STAGE_D`, prior 2026-11-01 authoring, or 2026-11-02 static revalidation summaries. Existing summaries may be REUSED as evidence after validating their scope.

Both statuses are stored separately; an open second-pass item never silently erases the old Stage D readiness flag. It can, however, prohibit claiming the second-pass/full-course 2021 acceptance is complete.

### Required chronological workflow for EACH lesson
1. Start at U01:L01 and follow official lesson ordering, proceeding to the next lesson **only after recording the current lesson's second-pass close**. Do not jump to a later lesson from old cursors. Existing correct lesson work must NOT be recreated.
2. Read the existing Edulytics lesson/Blueprint/RichV2/Practice and the original First Edition 2021 `lesson.html`, `preparation.html`, Student Facing and educationally meaningful assets from the archive at `C:\Users\khali\Downloads\New folder (2)\Common Core\Illustrative_Mathematics_First_Edition_2021\05_Grade_5_IM_K5`. Original local source: READ ONLY.
3. Compare lesson goals, sequence, examples, activities, every significant question/representation, relevant CCSS/skills, card diagrams, Cool-down and selected question family, grader/teacher rubric. Check for wrong-edition v360 residue. Reuse valid 2021 work; correct/remove only **proven** v360 mismatch; extend an existing engine before proposing a new engine.
4. Original publisher-gated Cool-down may be replaced with an independently authored, source-skill-aligned Edulytiks exit and teacher rubric, while explicitly preserving `PUBLISHER_ORIGINAL_UNAVAILABLE` and **never** claiming publisher-verbatim parity. Handle inaccessible images/resources, third-party rights and teacher-only tasks as explicit evidence limits, not manufactured matches.
5. **Write** targeted fixtures and inspect changed code and static evidence; per attached Stage A–B–C plan **do not execute .NET, Node tests, E2E, Stage D, Final CI, merge or deploy**. Mark fixtures `WRITTEN_NOT_RUN`. No claim of passing runtime tests.
6. Record one lesson-level second-pass closure on GitHub with **verified branch HEAD, source IDs/edition, source element evidence, inspected existing code, changed files, specific corrections or REUSE_ONLY reason, selected Practice family and teacher review, exceptions, written tests, UTC start/end, commit reference/commit-linked evidence and next lesson**. Only then set `REVIEW_COMPLETE_2021=true`. Historical Stage D ready remains intact.
7. Advance immediately to the next lesson without requesting permission or presenting long interim reports. At **every complete unit**, create concise unit report, then continue into next unit. At full grade end, deliver one aggregate report with statuses and proven coverage, not unsupported 100% source-element claims.

### Resume and state precedence
- Current PR branch/HEAD and **the new second-pass ledger** take precedence over historical first-pass acceptance cursor, legacy `v5StrictUnit`, older `returnBacklog` or static 148/148 source-review reports for SECOND-PASS progress.
- `148 READY_FOR_STAGE_D` is baseline only. `0 REVIEW_COMPLETE_2021` is the INITIAL second-pass state until new closure evidence is committed.
- Track `lastSecondPassClosed`, `nextSecondPassLesson`, `perLessonReviewStatus`, `sourceEdition`, `evidencePath`, `commitSha`, and blockers. Never change status merely because a file date is newer.
- Preserve all prior commits; avoid reset/force-push, unrelated code changes and repeated full scans or unnecessary tests.
- Source 100% accuracy must honor the attached plan's atomic denominator and SOURCE_UNAVAILABLE/RIGHTS_RESTRICTED distinctions.

### GitHub Actions versus local source
- Prefer GitHub Actions **as compute/execution for repository-only static review and status validation**, not the Our-CS shell for builds or tests.
- **Hosted GitHub runners cannot directly read `C:\Users\khali\...`**. Source comparison needs a permitted read-only bridge, authorized self-hosted runner with source access, or a safe versioned reference bundle made available to Actions. **Do not expose private machine paths, credentials, third-party restricted assets or copyrighted original books through CI artifacts.**
- No workflow that runs Stage D or broad CI tests under this second pass. Any dedicated manual workflow must only verify evidence/status integrity and be explicitly distinguishable from stage-D testing.
- Source inspection through the authorized machine connection is allowed *read-only* until an equivalent source bridge exists; code and evidence writes go directly to PR #596.

### Throughput
Prefetch sources for multiple lessons, reuse previous valid evidence, group read calls, minimize GH writes while preserving one logical per-lesson closure, aim for ~3 minutes/lesson **only when the comparison and correction genuinely support it**. Do not sacrifice evidence or claim completion to meet an arbitrary timer.

### Final acceptance
148/148 `REVIEW_COMPLETE_2021` requires 148 new independently grounded closure records and 8 unit reports; old `READY_FOR_STAGE_D` entries must remain unchanged. A blocker must be visible and cannot be reclassified as `REVIEW_COMPLETE_2021` without satisfactory evidence. Stage D remains a separate owner-approved plan.
