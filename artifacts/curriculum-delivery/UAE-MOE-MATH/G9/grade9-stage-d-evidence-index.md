# UAE Grade 9 General — Stage D initial evidence index

Candidate: PR #650 (`fix/g9-general-full-year-scope-86`).
Source snapshot: `SHA256SOURCESET:5e9c781829dd99b6fcd4449b5052930d15a28756abd78303466b5a5ec3fe3adc`.
Target: 13 units / 82 General Arabic RTL lessons, school year target 2026–2027 (not official 2026–2027 edition certification).

## Entry inspection
- At start of Stage D, branch candidate SHA: `f95fd5f76bc992d340b8a8e3aa65c804c7d78cbe`; subsequently changed by Stage D diagnostics files, so this original SHA must **not** be treated as the tested final release candidate.
- Required path `grade9-a-c-handoff.md` is not present under `artifacts/curriculum-delivery/UAE-MOE-MATH/G9/` at initial inspection. Existing `full-grade-progress.json` and individual lesson checkpoints document **82 authoring closures only** and do not substitute full source-element evidence.
- `source-inventory.json` includes 3 General source books (86 raw lessons), 2 Applied reference books; editions 2025–2026 and 2019–2020. `edition-diff.json` leaves full semantic dedup pending.
- `grade9-82-target-index.json`: 82 target rows, `SOURCE_INDEX_COMPLETE=false`.
- `semantic-audit-ledger.json`: 86 raw entries, zero rows `fullLessonAuditPassed=true`; denominator unverified for all 86. `full-grade-progress.json` target index unverified.
- Stage D uploaded plan has stray `Grade: 8` in section 2(1) and checkpoint template, inconsistent with its explicit Grade 9 scope; all executable metadata is fixed to Grade 9. The uploaded plan itself was not silently edited.

## Diagnostics authored
- `.github/workflows/uae-grade9-general-stage-d.yml`: dedicated Node tests, strict academic source gate, scoped .NET 10 tests; commits `8fbc6f71e4c2012115fd6b2a9e12d2fe19cb0630`, `88ee91968942a99508c53a069e0f82f55ddff2da`.
- `grade9-stage-d-execution-ledger.json`: initial fail-closed ledger, commit `f2794b38994ebc7e098c9848eca86961048cffbe`.
- No actual G9 workflow run ID or PASS result has been confirmed as of this entry. Do not count other Grade/US workflows as G9 Stage D completion.
- The strict source index test is intentionally expected to fail until source objective/exercise/visual parity evidence is complete; failure is a **release blocker**, not permission to change the numeric gate.

## Stage D gate
`D_PENDING_A_C_EVIDENCE` / `NO_GO_SOURCE_100` until the required 86-source / 82-target semantic element coverage is complete, and independently executed CI, authenticated E2E, RTL, Math and DB checks pass on a fixed SHA. No merge, production database changes, or deployment made by these diagnostic commits.

## Stage D source-entry recheck - 2026-11-21T02:51:32Z
- Handoff present: D_PENDING_A_C_EVIDENCE, source 0/86 audited and target 0/82 1:1 verified.
- 5/5 real local PDFs matched SHA and page count only. Semantic full-page inspection NOT certified.
- Source-selection 14/86 differs from latest handoff 13/82. Unit 7 remains unresolved.
- Run 44857599478 queued on inspected candidate 97e399bee1b898dc112d5b8a5c3942cc47fce598, not a passing hosted CI run.
- Checkpoint: lesson-1-stage-d-checkpoint.json. Release: grade9-release-validation.json. CI status: grade9-final-ci-summary.md.

- Lesson 1 D.4 Q48: corrected source-relevant mm:ss race-time representation using 14 independent values and 5-value student SVG; actual local Edge pass, 4 new math/validation tests. Full source parity, authenticated student E2E and hosted CI remain unverified.

- A-C owner-versus-raw scope metadata corrected: source-selection.json ownerTarget=13/82, source raw=14/86, unique source lesson counts remain unknown. Stage D source gate still D_PENDING_A_C_EVIDENCE.

- U01L01 preliminary source-element IDs: 61 numbered exercise containers + 21 known instructional elements by original printed pages 5-9, with additional 7 structural groups not double-counted. All atomic subparts/total denominator/teacher parity remain unverified; no D_VERIFIED status.

- U01L01 printed p8/PDF p19 Q35 reading corrected: describe direct 3/4 fraction, never halve it. Repaired independent 3/5 of 50 = 30 analog; source example remains 3/4 of 28 = 21. Documented 11 lettered child items under Q34-Q37 numbered parents; not additive to exhaustive atomic count. Lesson D_PENDING, NO_GO.

- Revised 21 Nov Grade9 Stage D Arabic pathway gate: shared Grade9 student/staff API selection now rejects unreadied and legacy English G9 General. Local .NET G9/G7 policy regression 11/11 PASS and Node prepared 372/372 PASS. Approved-G9 missing-Arabic HTTP fixture, DOM, 82 Arabic titles and tenant E2E NOT_RUN. Gate remains partial and release NO_GO. Evidence grade9-arabic-pathway-guard-evidence-NO_GO.json.

- Stage D guard 82-identity negative policy verification (same locked scope): 82 distinct checked-in canonical draft LessonCodes were loaded by a new C# xUnit MemberData theory; combined scoped G9/G7 policy tests 93/93 PASS, zero failed/skipped, local TRX g9-test-results/g9-arabic-pathway-all82.trx. No real authenticated G9 API/DOM, student pages, teacher signatures, or academic source closure claimed. Evidence appended to grade9-arabic-pathway-guard-evidence-NO_GO.json. Stage D entry and source release remain NO_GO.
