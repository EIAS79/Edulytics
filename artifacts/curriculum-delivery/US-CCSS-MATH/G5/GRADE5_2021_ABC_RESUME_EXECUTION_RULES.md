# Grade 5 — First Edition 2021 — A–B–C execution rules

**Binding operating instructions for PR #596; source of truth is the user-supplied Grade 5 A–B–C execution plan.** Do not begin Stage D until the user reviews the final A–B–C report and explicitly authorizes Stage D.

## Source / edition separation
- Authoritative lesson material: `C:\Users\khali\Downloads\New folder (2)\Common Core\Illustrative_Mathematics_First_Edition_2021\05_Grade_5_IM_K5\unit-{u}\lesson-{l}\lesson.html`, and `preparation.html` (read-only via Our-CS).
- The original Windows legacy path to `05_Grade_5_IM_K5` is a junction to this First Edition 2021 folder.
- `Illustrative_Mathematics_v360` and any `accessim_student`, `accessim_teacher`, `Student_Public_Mirror` from it are **excluded** from original 2021 source parity evidence. Do not treat a v.360 student page as the original 2021 Student Facing.
- Use original 2021 teacher HTML's embedded Student Facing, teacher preparation, and verified 2021 materials. Do not alter source files.

## Lesson workflow (sequential)
1. Read current GitHub PR #596 HEAD and prior lesson checkpoint. Start at **first not-validly-closed lesson**; historic `READY_FOR_STAGE_D` alone is not sufficient.
2. Open current Edulytics lesson and only the 2021 source segments/evidence not already verified. Batch local reads; do not rescan whole books or repeat verified reads.
3. Compare objectives, original tasks and subparts, examples, math, activity order, source visuals and licensing, Student Facing, teacher-led exercises, practice and cool-down, and current Rich V2/Practice.
4. **Reuse** correct content and existing generator/grader; **Extend** an existing engine for specific missing families; create **New Engine** only for a genuinely different mechanism. Repair proven errors only; add missing elements; remove confirmed out-of-scope additions without damaging other lesson contracts.
5. Create an Edulytics-authored, teacher-reviewed **alternative cool-down** when the publisher 2021 original is login-gated. Mark the original source's literal equivalence `SOURCE_UNAVAILABLE`; do not assert source-exact parity without evidence. Do not copy restricted publisher artwork.
6. Write/adjust **targeted tests as WRITTEN_NOT_RUN**, do **not** run local tests, builds, .NET, GitHub Actions, CI, Stage D, authenticated E2E, merges or deployments in this A–B–C phase. GitHub is the implementation record; Our-CS is read-only for source reference, never the test executor. If a future separately authorized test is needed, use GitHub Actions rather than running it on Our-CS.
7. One clear lesson-closure commit (where feasible) with source itemization, first-edition locator, evidence, lesson checkpoint, and correct ledger; no redundant commits or reports. Capture actual start/end times and elapsed duration, never invent timings. About 3 minutes per straightforward reused lesson is an efficiency **target**, not a waiver of accuracy.
8. **Only close `READY_FOR_STAGE_D` when every A–B–C handoff requirement passes**, with no unresolved academic-source item; no simultaneous `A_C_EXPLICIT_GAP` and READY. If source or rights are missing, record explicit blocker and requirement. A publisher-gated Cool-down may use the **owner-approved first-party functional replacement only**, with exact original text openly marked SOURCE_UNAVAILABLE and teacher review required; no original-source or human academic approval may be fabricated. Preserve prior work and continue independent lessons. Do not claim grade completion with blockers.
9. Proceed in `U01:L01 → U01:L02 → … → U08` order, without asking for permission after each lesson. Aggregate brief progress at the end of each full unit; continue next unit. Prepare one comprehensive final A–B–C report for user review; **Stage D remains out of scope**.

## Revalidation state (2026-11-02)
- Candidate before revalidation: `421d3e2ad56fa2657c151cc3ec355b3bdb91a072` on `curriculum/us-ccss-g5-full-grade-20261028`.
- `U01:L01` revalidated and A–B–C closed with **owner-authorized independent teacher-reviewed Cool-down alternative**; original publisher text is still source-unavailable and must not be claimed literal parity. The next lesson cursor is `U01:L02`. The open publisher source issue is retained as a source-verbatim limitation, not a defect in the independent Edulytics Cool-down.
- Existing original Edulytics cool-down and Grade 5 Rich V2/Practice are **retained**. First-edition teacher lesson SHA-256: `88920fb053a82ff0ecd76289ea096548530d2fb5dcfccc8edc63498b6597f7cb`; preparation SHA-256: `0caa7f39bd3e8fe06ed6bb7f0d9f7a8236c947f0458d0a51cc4c5e3757f65195`.
- No CI or tests are claimed from this revalidation. Do not promote the lesson or all 148 by altering flags alone.
