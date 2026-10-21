# US-CCSS Grade 1 — source edition, licensing, and release boundary

**Candidate PR:** #567. **Status:** PRE-RELEASE REVIEW; **academic acceptance:** NOT GRANTED; **production:** NO_GO.

## Canonical course scope

- The canonical Grade 1 implementation inventory contains 146 lessons. Historical ordinals 1–50 remain unchanged.
- Candidate ordinals 51–146 have 96 exact teacher/student source files independently checked against the stored SHA-256 fingerprints from the read-only Our-CS source cache on 2026-10-19.
- The cache-integrity check proves source identity, not equivalence of student tasks, approved pedagogical content, valid graphics usage, deployed interaction, student grading persistence, independent academic review, or owner UAT. See `artifacts/curriculum-delivery/US-CCSS-MATH/G1/readonly-source-cache-integrity-20261019.json`.

## Separate source editions

| Source edition | Source platform | Curriculum license | Commercial-use condition |
| --- | --- | --- | --- |
| IM K–5 Math 1st Edition, © 2019–2021 | im.kendallhunt.com | CC BY 4.0 | Permits commercial use **with attribution and the applicable license conditions**. Does not grant trademark/logo rights. |
| IM K–12 Math v.360, © 2024+ | accessim.org | CC BY-NC 4.0 | **No commercial adaptation or redistribution** without separate appropriate authorization. Publisher names/logos remain controlled. |

Primary publisher sources: https://illustrativemathematics.org/site-ip-content/ and https://illustrativemathematics.org/terms-of-use/.

**Operating rule:** Use the fingerprint-matched first-edition 2021 teacher/student pages as the canonical curricular reference. Do not silently substitute newer v.360 content, exercises, diagrams, or screenshots into a commercial Edulytics release. Independently author Edulytics explanations, tasks, interactives and assets; separately verify licensed/third-party figures. If first-edition source material is adapted, supply legally required attribution in the existing centralized Curriculum Sources & Licenses view rather than adding publisher labels to every student lesson. Obtain legal review for edge cases; this engineering document is not legal authorization.

## Variant lesson inventory

A separately downloaded current `accessim_teacher`/`accessim_student` mirror contains **148** teacher/student lesson pages, including these **optional v.360 pages outside the present 146-lesson canonical inventory**:

- U02 L23: *Story Problem Information (Optional)*.
- U07 L18: *Halves and Fourths in Our World (Optional)*.

Their presence does **not** prove two missing 2021 lesson requirements. The two editions must not be conflated. A syllabus/version owner must explicitly decide whether these v.360-only optional lessons are relevant to a future **separately versioned** course. Do not renumber historical lesson IDs or add non-commercial-source adaptations under the commercial 2021 scope.

## Academic approval blockers

All 96 new lessons remain `IN_PROGRESS`, not `INTERNALLY_ACCEPTED`. Release requires actual per-lesson comparison of the original **student activity**, **teacher goal**, displayed explanations and examples, allowed/forbidden Practice families, mathematical answer validity, visual representations, and required nonnumeric classroom evidence. Teacher-authored Grade 1 student work at ordinals 65 and 146 needs authentic artefacts and the existing human-review rubric. Authenticated learner rendering, practice scoring/persistence, a reviewer approval, license/asset checks, protected CI results for the exact final SHA, Render live verification and owner UAT must be independently recorded.

**Do not change the approval ledger to GREEN or deploy this PR solely because a source-cache hash, generator test, or general-purpose CI job passes.**
