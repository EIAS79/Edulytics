# UAE Grade 1 Stage D — Live evidence index

- Scope: 102 existing Grade 1/Common Arabic Blueprint lessons, 11 technical units; 10 numbered printed units.
- Stage A–B–C base SHA: 78bded7ad1427e08c345455d44d3110fdc27efb1
- Handoff source snapshot: sha256:571ca1f4d8687100dfec0c239a4c9718f0104a5b65d81731398de6d21e1f1718
- ContentVersion: p29-uae-g1-common-t1-ogl-v1-v1
- Stage D branch: curriculum/uae-g1-stage-d-20261108
- Stage D workflow: .github/workflows/uae-g1-stage-d.yml
- Technical tests: **NOT_RUN** at ledger creation.
- Authenticated four-role E2E, isolated DB and Arabic RTL/visual/accessibility: **NOT_RUN**.
- Semantic source numerator/denominator: **UNKNOWN for 102/102 lessons**.
- Newer Arabic student edition parity for lessons 43–102: **UNVERIFIED**; 2019–2020 source editions retained and disclosed.
- Human and rights review: **NOT_VERIFIED**. Existing owner academic approval does not waive rights and source-version gate.
- Initial candidateSHA: **UNSET**; GitHub Actions run(s) on a fresh exact ref will be recorded later, not fabricated.
- Stage D initial state: **D_PENDING_A_C_EVIDENCE** for 102/102; this is independent of recorded ABC READY_FOR_STAGE_D.
- Release: **NO_GO_TECHNICAL** until actual compiler, grader tests, isolated multi-role E2E, RTL, CI, semantic/edition and rights gates.

The Stage D gate is fail-closed. The structural job may pass while the separate academic-source handoff and final release gates correctly fail. A green isolated .NET test job alone does not constitute full-grade D verification. Preserve last stable A–B–C branch and do not merge or deploy.

## First actual GitHub-hosted Stage D attempt
- Run ID 42508673780 on SHA b806fa6e37196a9cd7e2b6d911a6e83c7af4ee6b.
- Structural Arabic/source identity checks PASS; source-element parity gate FAIL CLOSED (unknown denominator/newer student volumes).
- Restore PASS; full solution Build FAILED due CS0103 / CS0136 / CS0165 in UAE Grade 1 verifier, so subsequent targeted tests SKIPPED.
- Repair: change eight UAE Unit4 verifier branches to use actual parameters dictionary, and rename shadowed parser variable from value to parsedUaeValue. No test is declared passed on the new repair SHA.
- Link: https://github.com/EIAS79/Edulytics/actions/runs/42508673780
- Final gate remains NO_GO; authenticated multi-role E2E not run.
