# B001 500 original 2021 student task PDFs: inspect real contents without false approvals

Scope: exactly the same 500 lesson IDs as the single B001 acceptance/release cohort. The existing 2021 source crosswalk (#524) verified that 500 original student PDF links were mapped, but NOT that any of their contents had been independently read. The teacher purpose/full-lesson HTML packet audit (#527) is separate.

New executable audit:
- Opens the actual original 2021 student task PDF for **every** canonical lesson, with up to four requests in parallel, two bounded attempts, a 25-second request limit and a 100-MB file ceiling.
- Uses Poppler to inspect real PDF pages and extract machine-readable text; records source link, SHA-256, byte count, page count, word and character counts, and per-lesson inspection failure/visual-only warnings.
- Saves only **metadata** in the workflow artifact (retained 14 days). No original publisher PDF, copyrighted page text or artwork is committed or uploaded.
- Is read-only; does not modify existing application content or live student records.
- Fails the *technical* PDF inspection if fewer than 500 PDFs can be opened and parsed; failures remain individual per-lesson evidence for follow-up.

Workflow: .github/workflows/us-ccss-b001-original-student-pdf-inspection.yml

**Assurance boundary:** successful PDF text extraction is not a quality check of diagrams, mathematical correctness, educational objectives or source-to-learner activity fidelity. In particular, lower grades often teach via visuals and manipulatives; low text density must trigger visual human review, not an automatic rejection of the content.

The same 500 lessons still need source-by-source instructional remediation, verified Practice question families and grading, accessible visual representation, source/licensing rights, authenticated student/teacher UAT, and 500 independent academic acceptance records on a single release SHA. None of these approvals are set by this audit. **B001 remains NO_GO; no publication is authorised by these results.**

Observed upstream exceptional case: B001 ordinal 227 (G2 U05 L14) points to a valid HTTP 200 application/pdf original resource with Content-Length 77,871,987 bytes. The first run's 35-MB cap was insufficient; the cap was increased to 100 MB without dropping streaming or timeouts. The very large resource remains subject to PDF parsing and visual review, not automatic approval.


## Additional source-activity versus PDF lexical crosscheck (all 500)

The audit now loads both original 2021 teacher/full-lesson activity dossiers from PR #527 and checks their exact canonical ordered lesson identities against the original student task PDF index. On each PDF's extracted text it compares **distinctive word tokens** against each available original student-facing task excerpt, reporting only a numeric best-recall signal and a non-certifying classification.

- `LEXICAL_SUPPORT_ONLY`: at least one sufficiently specific activity excerpt shares at least 65% of its distinctive tokens with the PDF. This is source correspondence evidence, **not** approval of the math or lesson.
- `LOW_LEXICAL_SUPPORT_REVIEW`: less than 25% best distinctive-token recall. Requires investigation, **not** a finding that the PDF is wrong; some student tasks appear only in teacher-side activities or rely on images.
- `INCONCLUSIVE_LEXICAL_SUPPORT`: limited middle-range lexical correspondence.
- `NO_SOURCE_STUDENT_PROMPT_EXCERPT`: no student-facing task excerpt extracted in PR #527. This needs independent original-source inspection, not a fabricated match.
- `SOURCE_EXCERPT_TOO_GENERIC`: excerpt lacks six informative tokens; no confident lexical comparison is possible.
- `PDF_TEXT_TOO_SPARSE`: PDF has too few distinct machine-readable terms. Visual inspection is needed.

The artifact includes no PDF text or original excerpts; it retains only source identities, structural PDF metadata and numeric lexical metrics. This lexical check is not a mathematical reasoner, an image/diagram review, an independent teacher reviewer or a student attempt. No historical approval fields are altered and the 500-lesson B001 release remains `NO_GO` until its existing 500 individual evidence and owner signoff gates pass.
