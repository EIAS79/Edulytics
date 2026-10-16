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
