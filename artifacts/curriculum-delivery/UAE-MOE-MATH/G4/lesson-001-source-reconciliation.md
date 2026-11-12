# UAE Grade 4 — source reconciliation / lesson 001 (work in progress)

Repository: EIAS79/Edulytics
Source root (Our-CS): C:\Users\khali\Downloads\New folder (2)\emirates\Grade_04
Student books: 01_Student_Books
Teacher guides: 02_Teacher_Guides

## Verified inputs
- Seven actual student PDFs: 04-01.pdf, 04-02-07.pdf, 04-02-08.pdf, 04-02-09.pdf, 04-02-10.pdf, 04-02-11.pdf, 04-03.pdf.
- Teacher materials: رياضيات دليل المعلم 4 22.pdf (427 pages); رياضيات دليل المعلم 4 2_compress.pdf (188 pages). The 427-page teacher guide is scanned and did not expose textual content through the direct PDF reader; no lesson-level teacher guide claim is made.
- Local exam-structure PDF: _verified_2026-2027_exam_structure_bridge_t1.pdf, 3 pages. The exam structure explicitly states Grade 4 Mathematics, academic year 2026–2027, Bridge, Term 1, 25 questions. It is an exam coverage document, not a full three-term lesson inventory.
- 04-01.pdf shows 2025–2026 on the cover and lists 16 units in its table of contents; use as a candidate, not automatic proof of complete 2026–2027 equivalence.
- Existing GitHub main official manifest has 14 units/93 English-named Reveal lessons, which conflicts with this Arabic candidate book. This is a source/content conflict, not evidence that any learner lesson is already verified.

## First lesson, candidate textbook order
- Unit 01: القيمة المكانية
- Lesson 01: القيمة المكانية
- Candidate source: 04-01.pdf, PDF page 34 (printed page 11), continuing PDF pages 35–39 (printed pages 12–16).
- Essential skill: identify a digit's position and value in multi-digit numbers, including millions; read a place-value chart by periods and explain why moving one place left multiplies value by ten.
- Verified examples at PDF page 34: 184,800 and the shaded 8; at PDF page 35: 2,419,200 and the changing value of digit 2. The required learner visuals include place-value chart and labeled periods.
- Questions: identify a marked digit's place and actual value; compare positions after shifting the same digit. Create new examples rather than copying source questions verbatim.
- Current code `PED:UAE-MOE-MATH:L4:COMMON:01:01` is labeled `Math Is Mine`; do not silently call this matched or merge into production. A mapping/migration impact review is required before changing public lesson identities.
- 2026–2027 Bridge exam structure page 2 explicitly covers digit place value and reading numbers to millions, supporting the skill's T1 inclusion but not proving whole-year 16/130.

## Required before A–B–C closure
1. Verify student and teacher source versions/lesson range, identify all eligible elements and SHA-256.
2. Reconcile full 16-unit/130-lesson candidate inventory with 2026–2027 Bridge/Reveal scope and current app IDs; preserve student assignment/result history.
3. Author/integrate correct Arabic learner content, charts, source-specific practice contracts, independent grader tests and teacher-reviewed open responses.
4. Verify JSON structure and lesson-scoped calculations, capture evidence/checkpoint and commit before `READY_FOR_STAGE_D`.

Status: SOURCE_RECONCILIATION_IN_PROGRESS
ClosedLessons: 0
StageDTestsRun: false
Do not interpret this file as lesson A–B–C completion or 100% academic parity.
