# Grade 7 A–B–C — active execution rule

## Scope and immutable source selection

- Repository: `EIAS79/Edulytics`; working PR: #590; 145 lessons in nine units.
- Curriculum identity: `US-CCSS-MATH:G7:OUR-IM-2017` (Open Up Resources / IM 6–8 Grade 7, 2017–2019 lineage).
- **Only original local teacher source:** `C:\Users\khali\Downloads\New folder (2)\Common Core\07_Grade_7_OUR_IM`
- **Only original local student source:** `C:\Users\khali\Downloads\New folder (2)\Common Core\07_Grade_7_OUR_IM_Student`
- **Exclude v360 and AccessIM:** Do not read `Illustrative_Mathematics_v360` or `AccessIM_Grade7_*` as source-of-truth. Do not silently treat an `Exact_Sources` derivative or a publisher mirror as the approved edition. Cross-check identity before using.
- Read-only original sources. All changed code, evidence, checkpoints and commits are on the existing Grade 7 GitHub branch; do not edit original books.

## Mandatory cursor and workflow

1. Re-read **current PR HEAD and the checkpoint** for the last A–B–C-closed lesson. Current lesson is the **first unclosed** ordinal; sequential order `1 → 145`.
2. Prefetch/parse several local source pages in a grouped read for speed, but **do not edit/close a later lesson ahead of the current one**. Read teacher preparation, full lesson, student page and practice wherever they exist.
3. Compare original purpose, goals, all warm-up/activity/optional-task subparts, learning sequence, examples, questions, diagrams, practice, teacher boundary and Cool-down with effective Edulytiks Rich V2/practice contracts. Reuse already verified evidence; do not re-read identical snapshots without a concrete reason.
4. **Reuse → Extend → New Engine.** Preserve correct implementations; repair only identified gaps. Reuse tested question families where semantically valid; extend an existing family before creating a separate engine. Never substitute a generic mathematical question that measures a different skill.
5. Prepare and document a **first-party Edulytiks Cool-down** for the same objective where the original publisher Cool-down is gated. Do not present it as a quotation or verified reproduction of the inaccessible original. Human-drawn, oral and written products remain teacher-reviewed.
6. In A–B–C: author scoped acceptance fixtures with `AUTHORED_NOT_RUN`; **do not intentionally run .NET tests, full builds, Stage D, E2E or CI.** The user has authorized Grade 7 academic publication, but runtime, checks and deployment remain distinct release processes.
7. Update the specific lesson's element evidence/checkpoint and authoring ledger; make **one atomic lesson commit** where possible. Mark `READY_FOR_STAGE_D` only if the scoped teaching and Practice authoring gates are met; never claim runtime verification/CI PASS. No `REVIEW complete 2021` on Grade 7.
8. Then move to the next lesson automatically, no stop-for-permission and no long per-lesson audit. Report concisely after completing the **whole unit**, then continue with the next unit while work is possible. Record real start/closure times only if measured; never promise three minutes as a hard accuracy bound.

## Source rights and product attribution

Edulytiks-owned mathematical explanations, new examples and independently designed figures are not publisher artwork. Similar mathematical ideas alone do not establish copyright infringement or transfer of ownership. However, the project must preserve internal provenance and comply with **CC BY 4.0 attribution** wherever it uses/adapts licensed expression or third-party figures. Do not delete a legally required credit merely because the author approves publication. Keep publisher branding/trademarks out of the learner-facing instructional body unless legally required. Separate owner academic consent from CI, code integration, actual deployment and source-material certification.

## Current progression

- U01:L01: A–B–C authoring closed on recorded previous commit; Stage D NOT_RUN.
- U01:L02: first missing selected Practice route; completed exact correspondence/scale question authoring in this commit, scoped static test cases written, Stage D NOT_RUN.
- Next sequential lesson **U01:L03**, after the L02 GitHub closure is verified.
