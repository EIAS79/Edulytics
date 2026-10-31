# Algebra 1 U01:L01 ? student P3 answer-key exposure repair (2026-10-31)

**Scope:** Original PR #589, single current lesson `U01:L01 ? Getting to Know You`. Source A and prior Rich V2 teaching, the existing first-party paper worksheet, teacher-scored 8-point survey and separate 6-point Edulytiks Cool-down are preserved, not reauthored.

**Defect:** Previous student-facing Rich V2 `ExplanationParagraphs` printed the full `{A,C,E}` correct response to the P3 assessment and described which other choices were wrong. That compromised the exact-choice evidence even though the separate student printable sheet did not reveal the key.

**Fix:** Version `alg1-u01-l01-local-source-aligned-20261031-v4` removes the answer key and differentiating distractor explanation from the student lesson. The exact six-choice answer set is preserved **only** in the lesson-local assessment contract, not removed from the teacher's oracle. The learner sheet remains intact.

**Fixture precision:** The intended future browser UI will send a typed array of checkbox choice IDs A?F. The independent authoring oracle also preserves the previously valid comma-transcribed `"c, E, a"` response, with a strict grammar of exactly three nonempty A?F tokens. Both forms normalize case, trim whitespace and ignore ordering while rejecting duplicates, missing/extra/unknown IDs and malformed delimiters. This authoring allowance is NOT evidence that a machine Practice route accepts either format.

**Test updates:** Existing `UsCcssAlgebra1CourseUnit1RichV2Tests` assertions updated for v4 and to prohibit answer disclosure; existing lesson-local Node fixture now checks printed six choices, answer-key isolation and both positive/negative/malformed contract cases. **AUTHORING_ONLY, TESTS_NOT_RUN**. No Stage D, CI, .NET build, authenticated grading, teacher data save or new selected Practice route claimed.

**Current gate:** `ACTIVE_LESSON=U01:L01`, `C=BLOCKED_C_SHARED_SELECTED_PRACTICE`, `READY_FOR_STAGE_D=false`, `LAST_CONFIRMED_CLOSED_LESSON=NONE`. Shared single-writer request remains in [Issue #605](https://github.com/EIAS79/Edulytics/issues/605); Coder 5 does not own shared Practice files. No L02 implementation.
