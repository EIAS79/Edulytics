# UAE-MOE-MATH / Grade 9 General — A–B–C execution contract

Scope is **Grade 9 / General (Common)**, Arabic `ar`, RTL, target 2026–2027. NEVER modify Advanced/Applied, other UAE grades, Cambridge, US CCSS, Polish or live Neon production.

## Source and lesson order

1. Source of truth is the uploaded/local General **student-book PDF**; teacher guide is supplementary. Prefer the newest authentic edition for each available term. Source edition year is not a blocker; record the actual source year and avoid claiming it is 2026–2027.
2. Term 1: `01.pdf` (2025–2026 General), 7 units, 43 numbered lessons. Cover clearly identifies **term 1**. Older General `02-1.pdf` and `03-1.pdf` are term 2 and 3 editions (2019–2020) and the old term-2 unit 7 **quadratic expressions** conflicts with term-1 2025 unit 7 **geometry tools**. Record source identity per lesson and resolve this before full-grade source lock. `02.pdf` and `03.pdf` are Applied and must never be imported.
3. Resume from the first lesson not closed in the GitHub checkpoint, currently **U01L01**. Do not skip ahead, invent a completion, reuse legacy English blueprint, or re-seed retired Grade 9 General/Advanced content from PR #649. Existing source-correct content is retained.
4. Workflow: open existing lesson + original book → compare objectives/definitions/examples/activities/visuals/assessment by source element → fix only the missing or incorrect → prepare question contract and regression cases → record evidence/checkpoint → one lesson-closure Commit → next lesson.
5. Read page ranges for a whole unit in batches from already mounted user PDFs. **Avoid Remote Desktop Commander** when PDF bytes are already available. Do not use TinyFish for local books. Avoid repeated reading, repository searches, builds and multiple Commits for one lesson. Target <= 3 minutes per existing lesson as an *efficiency goal*, never as a reason to waive evidence.
6. Review progress unit by unit, continuing without needing a new authorization between lessons in the same active request. No automations or scheduled continuation.

## Stage segregation

- Stage A: source and Edulytics mapping; B: authored Arabic / RTL rich content, original diagrams and cool-down (derive cool-down when teacher-only original inaccessible); C: SkillContract `Reuse → Extend → New Engine` and prepared tests, **no running tests**.
- **All running of tests, CI, E2E, build, semantic gates, real generated question reviews, and final academic/technical acceptance happen exclusively in Stage D.**
- Shared family name from US / UK / Poland is **not proof of error** nor proof of suitability. Record actual objective, question, expected/invalid answers, grader, independent verifier, language.
- In A–B–C, `TestsWritten=...`, `TestsRun=NOT_RUN`; never mark `READY_VERIFIED` from a mapping file alone.
- Close a lesson as `READY_FOR_STAGE_D` only if A/B/C evidence, source-element denominator, runtime-ready lesson mapping and prepared fixtures are actually complete. Otherwise keep an explicit gap and the same first-unclosed lesson pointer. Stage D must independently block on any incomplete semantic audit.

## Current working tree checkpoint

- Branch: `curriculum/uae-grade9-general-ar-abcs`
- `docs/curriculum/UAE_G9_GENERAL_2025_STUDENT_INDEX.json`: 43 **term-1** numbered lessons, NOT a full school-year count.
- `artifacts/curriculum-delivery/UAE-MOE-MATH/G9/source-selection.json`: edition/term conflict facts.
- U01L01 source pages 5–9 are reviewed and documented in `u01l01-source-element-coverage-ledger.md`; Arabic lesson content, practice specifications, candidate skill mapping and original diagrams exist.
- U01L01 is **NOT CLOSED**: exact grader/renderer readiness and year-wide source lock are not proven. No Stage D gate has run; no lessons have been deployed.
