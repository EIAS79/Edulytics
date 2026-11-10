# EDULYTICS — UAE GRADE 4 — STAGE A–B–C — SOURCE-LOCKED EXECUTION POLICY

**Project:** `EIAS79/Edulytics` | `UAE-MOE-MATH` | Grade 4 | Common | Arabic RTL | target 2026–2027.

**Owner instruction:** Use local UAE mathematics student books first, teacher guide for auxiliary pedagogy, ministry examination structure as scope cross-check, and existing Edulytics lessons only to identify what can be reused. This file is a task policy, NOT a completion certificate.

## Source paths and version precedence
- Root: `C:\Users\khali\Downloads\New folder (2)\emirates\Grade_04`.
- Preferred 2026–2027 student text: `01_Student_Books\01.pdf` (434 image-only PDF pages), SHA-256 `54760d6eb2483624e94417f63bcf5e3590b4cc46f288b9df13cc7cc0923e5a69`.
- Student PDFs `02.pdf`, `03.pdf`, `e1.pdf`, `e2.pdf` must be edition-verified individually. Do NOT treat all volumes as the same edition automatically.
- Teacher sources: `02_Teacher_Guides`. Auxiliary sources `03_Supporting_Materials`. Do NOT allow an older guide to overwrite more recent student-book objectives.
- Source-index evidence: latest book TOC, physical PDF pages 8–23, 16 units and 132 numbered lessons. The physical 2026–2027 `01.pdf` contains student lesson bodies for Units 1–6 (51 lessons); Unit 7 onward is **not source-edition verified** in this document. From Unit 10 onward TOC page locators are literally `xxx` and must not be invented.
- `_verified_2026-2027_exam_structure_bridge_t1.pdf` is a Grade 4 Bridge-labelled *term-1 examination structure*. It is not proof that the selected book is Reveal or that every page follows a Bridge series.

## Grade-4 catalogue migration before public switch
- Existing published `main` Grade 4 has 14 units/93 English lessons based on previous Reveal annual plan. New intended book: 16 units/132 Arabic lessons.
- **Do not rename an existing lesson code to mean a different topic**. Record old ID, topic, references, progress/attempt links, target ID, and decision. Preserve pre-existing student and school data. Test any migration against a disposable database before production.
- Proposed 132-lesson TOC index: `artifacts/curriculum-delivery/UAE-MOE-MATH/G4/grade4-2026-2027-toc-132.json`.
- Version-change migration map: `artifacts/curriculum-delivery/UAE-MOE-MATH/G4/grade4-edition-migration-map.json`.
- Until a coherent, tested 132-unit catalogue cutover, do not break the existing 93-lesson production registry by publishing a partial blueprint/content pack.

## Exact lesson workflow — no random movement
1. Begin with the **first unclosed** Grade 4 target lesson in ascending ordinal. Source and version must match the TOC record before any reuse.
2. **Stage A (source):** compare title, page, learning objectives, skill, vocabulary, worked examples, visual manipulative, guided/independent practice and activities. Record PDF SHA, PDF page, printed page, element, correct existing content, gaps and evidence.
3. **Stage B (fix):** reuse the existing mathematical engine if suitable; extend with exact question family only when missing; add a new engine only for genuinely different mechanics. Repair correct components, never rebuild them without reason. Author original Arabic learning text, examples, tasks and diagrams; do not copy source publisher artwork or prose.
4. If teacher-only Cool-down cannot be viewed, author a short **Edulytics Cool-down** that checks the same objective and explicitly identify it as newly authored, not verbatim publisher content.
5. **Stage C (targeted):** JSON/schema, objective math and answer verifier (correct/wrong/malformed), RTL/mobile, registration, references and impacted tests. Log `TESTED` or `WRITTEN_NOT_RUN` honestly. No global CI/Stage D per lesson.
6. Only mark `READY_FOR_STAGE_D` after actual lesson is usable in target app, source evidence exists, and Stage A/B/C are satisfied. Commit a distinct evidence checkpoint for each closed lesson **before moving to the next lesson**.
7. Batch prepared PDF reads and batch remote/API operations. Avoid repeated downloads, full restore/build, serial GitHub latency and reports per lesson. Target ~3 minutes for a mostly-correct lesson, not a deadline for untested approval.
8. Continue straight to the next lesson after recording the previous closure; review units in order 01→16, reporting only once per unit and at the end. Do not stop to request permission after each step.
9. Use `A_C_EXPLICIT_GAP` only if a documented source gap really exists; never label such a record `READY_FOR_STAGE_D`. A TOC row alone is not a completed lesson.
10. Stage D and release remain separate. No production merge/deploy or `CI_PASS` assertion from A–B–C-only checks.

## Status and evidence
- Target: **16/16 units, 132/132 lessons**; current TOC evidence 132/132; verified latest-edition lesson-body scope **51/132** with **81 edition gaps** pending verification against additional originals.
- Keep the distinction: `TOC_INDEXED` ≠ `LESSON_A_VERIFIED` ≠ `B_FIXED` ≠ `C_TESTED` ≠ `READY_FOR_STAGE_D` ≠ `PUBLISHED`.
- The owner's academic publishing authority covers Edulytics-authored materials. Source names, edition and license provenance remain accurately documented *internally*; public learner display is first-party Edulytics, never an implied publisher endorsement or blanket permission to copy third-party assets.
- Official 2026–2027 item-level parity is 100% only when every selected source element, objective, exercise-type and pedagogical feature has matched evidence. Missing source editions cannot be claimed as verified.
