# B001 — all 500 lessons original-source teaching review ledger (2026-10-16)

**Whole-batch scope: 500/500**, not an arbitrary pilot or recurring 5-lesson cap. Original first-edition (2021) source inputs: teacher preparation, full lesson (including student-facing activities), associated student task-statement PDF reference (see existing `US-CCSS-B001-IM2021-STUDENT-TASKS-PDF-SOURCES-500.json`), original B001 lesson codes, and existing student lesson bodies in the core pack.

Two machine-readable source dossiers provide an ordered, *per-lesson* evidence record for all 500 canonical IDs:

- `US-CCSS-B001-2021-SOURCE-PEDAGOGY-PACKETS-1-OF-2.json` (1–250)
- `US-CCSS-B001-2021-SOURCE-PEDAGOGY-PACKETS-2-OF-2.json` (251–500)

These records contain verbatim short lesson-goal/source-activity excerpts from 2021 publisher HTML with attribution under CC BY 4.0, and flags comparing the original learning purpose to the **uncorrected embedded Edulytics instructional body**. They do not commit images, entire teacher lessons, books or student PDFs. Original in-application effective V2 content may differ from the embedded pack when a source-guarded correction is applied.

## Full B001 source-pass results

| Stage or indicator | Found |
|---|---:|
| Canonical B001 lesson-source packet mappings | **500/500** |
| Original 2021 lesson purpose extracted | **500/500** |
| Original 2021 teacher learning-goal extraction | **499/500** |
| Original student-facing lesson-goal extraction | **499/500** |
| Original activity blocks parsed from full lesson HTML | **498/500** |
| Original activity embedded student prompt found | **492/500** |
| Raw learner worked examples use generic `The source task` language | **97/500** |
| Raw learner worked examples shorter than 220 characters | **39/500** |
| Raw learner step instructions shorter than 220 characters | **47/500** |
| Direct source-vs-learner no-regrouping contradiction detected | **1** (G2 U02 L01; already source-grounded corrected in the merged 7-lesson correction work) |
| Individually confirmed 2021 **student PDF contents** reviewed as part of this bulk pass | **0/500** |
| Source-activity fidelity/Practice/independent teaching approval | **0/500** |

The 500 source packets are **not** 500 authored or academically validated lessons. Regex detection can miss mathematical or pedagogical mistakes and can flag benign differences, while word matches cannot establish conceptual learning quality. Some candidate fixes have already been separately authored/merged; this report intentionally records original uncorrected pack conditions, not falsely treating them as current production defects.

## One-release implementation and acceptance model

B001 remains the **single** named 500-lesson release. Individual lessons still require their own source-aligned explanations, representations, classroom strategies, activities, practice families, independently checked numerical answer oracles, learner test sessions, and rights verification. Run source and test automation over all 500 at once; record lesson-specific corrections and approvals as entries within the same release candidate, not as smaller *replacement release scopes*. No automatic approval from a source link, keyword match, PDF availability or successful code build.

Do **not** call the batch finished, approve it, or publish it as 500/500 until the existing fail-closed release gate verifies all 500 lesson-level evidence records on the exact same commit, current GitHub checks, authenticated full end-user Practice and production UAT, followed by human/owner educational signoff.

Source study attribution: Illustrative Mathematics, IM K–5 Mathematics, 1st edition 2021, CC BY 4.0 for licensed text; graphics/trademarks separately restricted. Original source URL for each of the 500 lessons is preserved in `US-CCSS-B001-CATALOGUE.json`.

**Release decision at this stage: B001 NO_GO.**
