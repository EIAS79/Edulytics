# U05:L10 — independent academic successor approval request (NOT APPROVED)
2026-10-30 | Coder 7 / American Supervisor 3 | PR #588, course US-CCSS-MATH:HS:TRAD:ALG2:IM-1E

**Decision owner:** designated independent academic reviewer/owner assigned by Supervisor 3. Shared Integration Coder is a technical owner, not the academic approval authority.

## Original First Edition A/source comparison
- Teacher preparation: https://im.kendallhunt.com/HS/teachers/3/5/10/preparation.html ; student 10.1, 10.2 and 10.3, optional activity and summary: https://im.kendallhunt.com/HS/students/3/5/10/index.html .
- Required: create new functions through arithmetic; 2010–2016 books/population table, books/person 2010 and 2016, ratio graph; graph sums E+L, E+M, E+N, E+P, E+Q; optional two-line product; lemonade profit and average profit with contextual domain.
- Existing unchanged predecessor previously explained only 2010 rate and generic arithmetic. Its separate Edulytiks composition extension was correctly labeled original extension and was preserved.

## B: candidate changes to learner-facing teaching
- Added all seven source quotients, including `2700/323.41 = 8.3485359` books/person for 2016 versus `2530/309.35 = 8.1784387` for 2010. 2016 minus 2010 ≈ 0.1700973 books/person. Nonmonotone annual trend and units are explained.
- Added text-accessible source graph-summing instructions. E(0)=1, M(0)=-1, so (E+M)(0)=0; source E+L/N/P/Q have (0,1) at x=0; students add outputs at the **same x**. Actual graphical renderer/accessibility tests have not been executed.
- Original optional line-product example inferred from graph's described slopes/intercepts: U(x)=2-x, V(x)=x-1, W(x)=-x²+3x-2; zeros 1 and 2, vertex (1.5,0.25).
- Source lemonade C(n)=5+0.8n, Rev(n)=2n, P(n)=1.2n-5, A(n)=1.2-5/n for positive whole cups; at n=4 profit -$0.20, n=5 profit $1, n=0 average undefined; at least 5 sold cups for positive profit.
- Preserved original Edulytiks function-composition extension explicitly as non-source extension, not conflated with publisher's required arithmetic graphs.
- New regression `tests/coder-7/UsCcssAlg2U05L10FirstEditionTasks.test.cjs` contains source-specific numerical, wrong/boundary and source-provenance / status assertions. Checked-in full Node and .NET suites still await authorized exact-head execution.

## Immutable predecessor and requested version transition
- Course lesson code: `PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U05:L10`
- Immutable locked source SHA256: `e179a39196685a7055c257ae3eab85a2e6b80b4ab49864fb6b9251b8d349cf77`
- **Historical approved canonical-body SHA256 (do not rewrite):** `6ac21cf73b77c5246ddd1ea26d0734207bcc3c78efc9bb8f02fc2d5c48fe736d`
- **Unapproved candidate authored English body SHA256:** `7a5b882e7a31f37842f68c43126119886085a62e35ea821ef6aae33c14792c60`
- Digest input: newline-joined `title, explanation, keyConceptsAndRules, workedExamples, stepByStepSolutions, commonMistakes, quickSummary` prefixed with each property name and colon, SHA-256 UTF-8.
- Candidate commit: `8f4e63113f49efd722d4cc66ec1c73e7871df781`. New content pack git blob `ef69ef8aaa82c548140cc1b73317843ed372001d`.
- **Independent academic decision:** NOT_VERIFIED. Original predecessor is not a license to overwrite its hash. Authorized reviewer must ACCEPT a separately versioned successor, request REVISE, or REJECT and record identity/time/evidence. Acceptance is not assumed.

## C, D and E release blockers
- Existing explicit Practice mapping: `usccss.hs.function_sum`, `usccss.hs.function_composition`; source 10.2 quotient, 10.3 teacher-scored graph-sketch and actual selected grading/feedback are NOT VERIFIED. Shared integration writer to confirm or implement exact source-aligned Practice and per-school persistence; no shared code changes on Coder 7 branch.
- Authenticated student/teacher lesson display, graph accessibility, wrong/malformed/boundary responses, gradebook persistence, two-school isolation, protected exact-head CI, owner UAT: NOT_RUN / NOT_VERIFIED.
- Source review and authored learner teaching are real work. Final academic **A APPROVED = NO**; B renderer signoff = NO; C Practice accepted = NO; D/E production readiness = **NO-GO**.

**Requested action:** Supervisor 3 assign independent academic reviewer to this packet and U02:L18 companion packet, then record ACCEPT / REVISE / REJECT separately for each changed body. Technical shared owner can implement a later authorized content-version/migration after independent academic acceptance, never self-grant it.
