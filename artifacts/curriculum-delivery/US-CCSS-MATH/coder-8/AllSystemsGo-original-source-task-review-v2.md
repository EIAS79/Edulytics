# All Systems Go — course-owned source-task comparison (P0 V2)

**Status:** source-excerpt-verified, mathematical extension implemented on Draft PR #585; full original-PDF page/figure-by-page review and publisher-binary SHA-256 verification NOT RUN. This is not independent academic approval or Stage C/D UAT.

## Directly evidenced source excerpts

- Primary existing publisher/teacher URL: https://www.mathematicsvisionproject.org/uploads/1/1/6/3/11636986/a2mod10tnh718.pdf
- Student task URL discovered separately: https://www.mathematicsvisionproject.org/uploads/1/1/6/3/11636986/a2mod10seh718.pdf
- Source lesson: MVP Algebra II, Module 10H, Matrices Revisited, **10.6H All Systems Go!** (Practice Understanding Task).
- Student-task publisher text indexed from the original PDF: Carlos and Clarita determine per-item costs from three purchases. Week 1: two gallons of paint plus one tape roll, $30. Week 2: one gallon plus four brushes, $20. Week 3: two brushes plus one tape roll, $10. The student exercise also multiplies a 3×3 identity matrix by a 3×3 coefficient matrix and extends inverse solving beyond two variables.
- Teacher-note publisher text indexed from the original PDF: the purpose is to express larger systems as matrix equations and to use inverse matrices found with technology for 3×3 and higher systems; CCSS A.REI.8/A.REI.9.
- **Evidence limitation:** original PDF URL content was available through indexed text search, but web PDF opening exceeded a 25–27 MB response limit; original PDF bytes were not downloaded, rendered, or independently rehashed. No claim of full-page source fidelity is made.

## Concrete defect compared to prior Coder 8 candidate

The pre-V2 learner explanation gave a correct abstract 3×3 row-reduction/inversion example with RHS [6,3,2], but did **not** model the actual painting-supplies source task with its unit prices, zero coefficients, three receipt totals, or price interpretation. This was a source-task contextual gap, not an error in the abstract example. The preexisting Log Logic body is unchanged.

## Implemented first-party teaching for U01:L01

Define p=paint price per gallon, q=brush price, r=tape price per roll, ordered as x=[p,q,r]ᵀ.
- Receipts: 2p+r=30, p+4q=20, 2q+r=10.
- Matrix: A=[[2,0,1],[1,4,0],[0,2,1]], c=[30,20,10]ᵀ.
- det(A)=10. A⁻¹=(1/10)[[4,2,−4],[−1,2,1],[2,−4,8]].
- A⁻¹c=[12,2,6]ᵀ, so paint $12, brush $2, tape $6.
- Independent substitution: 2·12+6=30; 12+4·2=20; 2·2+6=10. A·(10A⁻¹)=10I₃.
- Negative test: wrongly placing a brush instead of tape in week 1 produces 2·12+2=26, not $30. Maintain coefficient/variable order.
- **Clearly hypothetical boundary, not original publisher data:** changing ONLY the third total to $50 results in [-4,6,38], which satisfies all altered equations and keeps det(A)=10 yet makes paint price negative. This is a contextual-model-invalid result, *not* an inconsistent matrix system.

## Proposed lineage and acceptance

- Historical Phase04 revision canonical SHA (immutable): `fc9f1a49bf236c330112fcf1c02e108893344eab58c2275c8eee80552ec5560c`.
- Earlier unapproved Coder 8 teaching-v1 body SHA: `6abd1e7b8c3d2472c4d8ec349e4d816f8607f1cdb1f5e3455d1284159a4c3e50`.
- New unapproved teaching-v2 body SHA: `9c30b68f87f48b668c32875483260a44b6600ee69ca46b9ec5b8a0706ed6b6f7`.
- Immutable recorded publisher source SHA: `87f0872674e13e3f53e4ccc81004cb150eb7a64539879efa9f04f8cd58349e55`; **not rehashed from PDF bytes**.
- Candidate pack version: `us-ccss-hs-adv-alg-func-2lesson-teaching-v2`. The v1 candidate was never approved/installed; do not fabricate a v1 persisted predecessor.
- Separate shared integration owner must append authorized field/body predecessor evidence before any materializer transition; old Phase04 manifest and approved rows cannot be rewritten.
- Both lessons remain NOT academically accepted. 26 fixture cases and teacher rubrics M05/M11/L13 remain only proposed; actual generated lesson Practice, role-authenticated teacher scores, school adoption/tenant isolation, persisted attempts, CI parity and owner UAT remain external release gates.

