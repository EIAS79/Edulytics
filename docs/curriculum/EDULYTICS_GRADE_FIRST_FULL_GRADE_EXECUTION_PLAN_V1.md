# Edulytics — Grade-First Full-Grade Curriculum Delivery Plan

**Plan ID:** `EDULYTICS-GRADE-FIRST-FULL-GRADE-V1`
**Adopted:** 2026-10-18, by owner instruction (implementation must be tested/merged before operational release).
**Programme:** `US-CCSS-MATH`.
**Predecessor:** `EDULYTICS-GRADE-FIRST-5-V1`, retained unmodified to preserve historical evidence.
**Execution/acceptance/release unit:** one **whole grade**, or one named high-school course. No fixed five-lesson chunk.
**Resume:** Grade 1, ordinal **51 of 146**. Ordinals 1–50 were already released; do **not** redo their curriculum authoring or per-lesson academic acceptance. Complete the remaining **96** lessons, then move to Grade 2.

## 1. Binding owner decisions

This is **the same V1 stages A–F, source standards, first-party lessons and Practice, grading, academic correctness, security, licensing and required CI/Render protections**, with one changed delivery granularity: full grade rather than five lessons.

- Previous Grade 1 ordinals 1–50 are historical **already released** work, not pending new lesson tasks. Keep all original batch manifests, evidence and releases untouched, including [final batch PR #566](https://github.com/EIAS79/Edulytics/pull/566), ending at main commit `2c7c43a8f3af7151d422acc6e8e9e8fa5348833c`. Do not recertify or rerun their individual academic acceptance.
- Existing general repository-wide regression CI still runs where currently mandatory; protecting shared code is distinct from redoing earlier per-lesson academic reviews.
- Only new ordinals 51–146 are individually source reviewed, implemented, tested, and newly accepted. Grade coverage reports `50 historical + 96 new`; it must never label previous 50 as newly certified.
- Release decision belongs to the **whole grade**. Do not publish an incomplete new Grade 1 release. A documentation/quality-gate PR may be merged independently without declaring the grade complete.
- No timeslots or minimum/maximum minutes for any lesson or stage; V1's 25–75-minute five-lesson estimate is an obsolete planning illustration, not an execution rule.
- Existing V2 20- or 500-lesson scheduling/NO_GO gates remain historical/inactive, never carried forward as positive acceptance. A successful negative-control test only demonstrates that invalid evidence was rejected.

## Continuous full-grade executor requirement — 2026-10-18 correction

**The executor is not permitted to stop at, select, approve or publish five-lesson waves.** A–D must be applied to every outstanding lesson in the **one Grade 1 work unit 51–146**. Checkpoints and Git commits can occur after any individual lesson or technical change, without treating those saves as release boundaries. Do not artificially wait for a certain number of lessons. A difficult/blocked lesson remains BLOCKED and must be resolved; do not skip to publish a misleading grade success. Content fixes, Practice, source-student-task review, authenticated scoring and lesson-specific tests are performed for each new lesson as required, and the final E/F gate remains ONE grade.

The machine-readable grade scope and resume command is `node tools/curriculum/us-ccss-full-grade-worklist.cjs` (all 96 ordinals) or `--remaining` (unfinished items, without a cap). Its output is a **scope worklist only**, not an autonomous executor, test evidence, acceptance, or production release. Real lesson implementation must continue against that grade-wide worklist on GitHub-authorized development workflows, without five-lesson stopping points. CI verifies that it contains **all 96** and that the historical **50 are referenced only**.

## 2. Grade manifest, checkpoint and ordering

`artifacts/curriculum-delivery/US-CCSS-MATH/G1/grade-manifest.json` inventories **146 ordered unique lessons** across eight units; sources are IM/Kendall Hunt 2021. Original source identity and lawful provenance must be preserved. The old manifest's `nextDeliveryChunk: US-CCSS-G1-004` is stale and **not** a new-work cursor. Use the **new ledger** `artifacts/curriculum-delivery/US-CCSS-MATH/G1/full-grade-progress.json`.

| Grade 1 portion | Ordinals | Treatment |
|---|---|---|
| Previously released | 1–50 | Preserve old evidence and release; 0 new curriculum approvals |
| New work | 51–146 | Exactly 96 real lesson reviews/repairs/tests |
| Grade closure | 1–146 | 50 historical + 96 accepted, then required release checks |

Start at `PED:US-CCSS-MATH:G1:U03:L14` (ordinal 51). Finish at ordinal 146 `PED:US-CCSS-MATH:G1:U08:L10`. Subsequent grade/course sizes and first unfinished ordinals derive from their **own** reconciled inventory, not G1 arithmetic. Never skip, duplicate or change a canonical lesson identity.

## 3. Unchanged V1 production stages A–F

**A — Select and compare:** Choose the next truly unaccepted lesson. Compare original teacher objective, teacher/student activity and response form with real composed student-facing Rich V2 and current actual Practice skill contract. Preserve source URL, edition, fingerprint, official mapped outcomes, required representations and forbidden unrelated families. Classify `READY_REUSE`, `CONTENT_REPAIR`, `PRACTICE_REPAIR`, `NEW_SHARED_CAPABILITY` or `BLOCKED_SOURCE`. Missing original/source/rights evidence blocks only the affected new lesson, without invented proof.

**B — Accurate content:** Reuse correct teaching, age-appropriate examples, step-by-step solutions, visual explanations, misconception repairs and summaries. Change only source-grounded defects; author first-party paraphrases/assets rather than reproduce unlicensed artwork. A synthetic survey or independent quiz cannot certify real classroom/partner discussion.

**C — Exact Practice:** Reuse registered `SkillContract`, approved generator families, mathematical solver and independent verification. Check question family/answer format/operand and grade bounds, diverse valid instances and correct and plausible wrong submissions. Create new **shared** capability only when genuinely needed. Preserve server-side grading, durable student attempts, consent and tenant boundaries.

**D — Verify affected new lesson:** Record exact lesson/outcome/source identity, fully rendered student content, examples, diagrams, accessibility and only permitted Practice families. Independently verify correct/wrong/malformed answers and applicable persistence and authenticated interaction. Run new capability consumers' regression and broader security/schema/tenant/UI tests where affected, plus **every still-required GitHub CI gate**. A missing check or source cannot be marked PASS.

**E — Whole-grade decision:** Persist source/evidence/lesson status after each newly completed lesson; no five-lesson batch completion. Grade 1 requires genuine **96/96 new** lesson acceptances, plus unmodified historical release evidence for 1–50. Fail closed on any new unreviewed/blocked/failed lesson. The programmatic grade gate must be linked to the exact tested commit and must not be mistaken for external publisher accreditation, teacher observation, owner UAT, or deployment.

**F — Release and owner UAT:** Final grade content is merged only after full-grade academic evidence and all current protected GitHub checks. Confirm the exact deployed `main` SHA, Render deployment LIVE and health readiness, authenticated student lesson/Practice and persisted scoring where affected, then record owner production UAT accurately as `PENDING`, `PASS` or `ISSUES_FOUND`. A grade is not production complete on PR merge alone. Grade 2 only follows actual Grade 1 closure.

## 4. Safeguards and evidence

For each new lesson save real versioned source goal/student task review, effective content, permitted and prohibited question-family mapping, independent correct and wrong grading proof, authenticated persistence evidence, relevant license/asset review, and affected test run references. The evidence must match the exact content and candidate code fingerprint. Do not fabricate PASS data, external approval, physical classroom participation or original publisher diagram pixel parity.

- No changes to historic V1 plans, batches G1-001–G1-010, student histories, tenant membership, school adoptions, official standards, production databases or Render infrastructure as part of this policy change.
- No `main` merge of incomplete learner-facing grade content merely because a lightweight control test succeeds.
- A dedicated full-grade **release** gate must fail until all new rows have genuine evidence. A separate **contract/negative-control** workflow may pass while correctly proving release is blocked.
- Use clean isolated branches on **GitHub or another authorized development environment**, **never create a software-development branch/worktree on Our-CS**. Our-CS is read-only for inspecting authorized curriculum source books/teacher materials.
- Do not promise background autonomous execution without an actual scheduled/authorized runner.

## 5. Delivery and exit criteria

1. Add this plan, a full-grade SOP, a 146-inventory/96-new progress checkpoint, plus tested fail-closed grade acceptance logic and GitHub CI without weakening existing checks.
2. Resume lesson 51 and proceed through lesson 146, saving genuine source review and Practice test evidence per new lesson; do not redo old lessons.
3. Gate new Grade 1 release on 96/96 real new acceptances plus historical 50 references, all required CI/security, merge-to-live SHA, production HTTPS and owner UAT.
4. Archive the final grade acceptance/deployment record; only then start Grade 2 as a complete grade.

**Current initial decision:** `NO_GO` for newly completing/releasing Grade 1; **this does not undo previous deployment of lessons 1–50**.
