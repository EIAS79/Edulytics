# US Common Core G1–G12 — academic release gate (2026-10-10)

**Scope:** Edulytiks `US-CCSS-MATH` (`CCSSM-2010`), Grade 1–8 + traditional high-school courses and supplemental lessons. Kindergarten excluded from this release audit.

**Source baseline:** `EIAS79/Edulytics` main `1620cd3ad5de33c9f745606b3ea792043089bca1` (2026-10-10).
**Method:** independently fetch official Common Core pages, compare numbered standards with the committed Practice map and 17 curriculum blueprints, inspect official-text differences, and execute lesson-family Practice runtime tests.

## Gate 1 — Official code inventory: PASS (numbered parent standards only)

- 60 official web pages processed without HTTP errors.
- 363 official numbered content standards found (207 G1–8; 156 high-school).
- 363 matching standard identities in `official-outcome-practice-map.v1.json`.
- Zero missing/extra numbered standard codes after normalizing the `HS*-...` notation used by the source pack.
- All 1,560 blueprint lessons accounted for across 17 files.
- Zero missing practice TargetRule references or addressing/outcome structural conflicts.
- The official HTML includes **121 additional subordinate a/b/c/... standard clauses**, represented within parent-standard descriptions. Explicit pedagogical verification of these subordinate requirements is still required before claiming full content mastery.

Evidence: `docs/curriculum/reference/us-ccss-official-codes.snapshot.json` and `docs/curriculum/us-ccss-g1-g12-structural-audit.json`.

## Gate 2 — Official text + semantic lesson mapping: BLOCKED

- The normalized HTML-to-pack comparison found 256 exact parent-text matches, 107 differences, including 21 low token-overlap cases (threshold < 0.90). Some differences involve source-document footnotes, mathematical typography or the integration of child clauses; **do not assume all differences are content errors**.
- Examples requiring source-by-source review: `1.G.A.3` (extraneous footnote), `5.G.A.1` and `6.EE.A.2` (apparent truncation), and parent standards with subordinate requirements.
- 35 lessons are linked to Mathematical Practices only. They have a valid official MP mapping, but do not yet have verified independent content-standard alignment. Review original publisher material before assigning any content standard; do not manufacture links.
- **Neon production row-by-row comparison: PASS.** Read-only export from Edulytiks production project `tiny-lab-44877119`, branch `br-frosty-block-b52tnjky`, confirmed all 1,560 US lessons and all 2,882 distinct lesson→outcome links match the 17 accepted source blueprints exactly, with **zero** missing/extra lessons or mismatched outcome identities. This is a structural identity pass, **not** a semantic endorsement of the 2,882 links. No production row was modified. Reproducible diff script/report: `tools/us-ccss-neon-blueprint-diff.cjs` and `docs/curriculum/us-ccss-neon-blueprint-diff.json`.

Evidence: `docs/curriculum/us-ccss-official-text-diff.json`, `docs/curriculum/us-ccss-neon-blueprint-diff.json` and earlier production audit dated 2026-10-10.

## Gate 3 — Skill/family Practice runtime: PASS for tested deterministic cases; full academic certification PENDING

- Existing targeted curriculum / Practice suite: 17 passing tests, zero failures.
- Added `UsCommonCoreFullLessonPracticeAuditTests`: confirms all 1,560 American lessons have a runtime-resolvable SkillContract, then generates and independently verifies one question **for every allowed Question Family** at each of Standard, Stretch and Challenge difficulties. Passed locally on .NET 10.0.300 (temporary local SDK override; committed `global.json` remains unchanged).
- This proves deterministic per-family runtime coverage of the sampled seeds. It does **not** prove semantic accuracy of each question against each official subordinate clause, every possible seed, answer equivalence, assessment approval or live database routing.

## Gate 4 — Merge and production deployment: BLOCKED

**Do not merge this as a claim of 100% officially certified US curriculum or trigger a production deployment until:**

1. Review and resolve 107 official-text discrepancies, paying particular attention to the 21 high-risk candidates; update legal provenance, source hashes and data migrations if any official content changes.
2. Review all 121 subordinate CCSS requirements against lesson examples, SkillContracts and covered question families.
3. Adjudicate each of the 35 MP-only lessons with a documented `keep_mp_only` or content-outcome mapping decision with publisher evidence.
4. **DONE — structural only:** 1,560 lessons and 2,882 live Neon links match the source blueprint exactly; review the correctness of those links academically, not by assuming numeric equality.
5. Run full relevant CI (tests, build, static checks, database contract and browser smoke tests) against the exact release SHA, then stage and review. Verify Render/Vercel service, workspace, environment and migration readiness.
6. Approve explicit release evidence; deploy once, then verify live Practice, Assessment, learner mastery isolation, uptime and rollback.

## Commands

```sh
node tools/us-ccss-standards-audit.cjs --strict
node tools/us-ccss-standards-audit.cjs --refresh-official --strict
node tools/us-ccss-official-text-audit.cjs
node tools/us-ccss-neon-blueprint-diff.cjs /path/to/read-only-neon-baseline.json
dotnet test tests/Edulytics.Tests/Edulytics.Tests.csproj --filter FullyQualifiedName~UsCommonCoreFullLessonPracticeAuditTests
```

**Release judgment: NO-GO**. The new comparison and runtime test infrastructure can be reviewed safely without mutating school data or claiming certification. Neither this document nor a green numerical coverage test authorizes production publication.

## Follow-up repairs and semantic release findings (2026-10-10)

- **Four verified content omissions repaired in application startup seeding (candidate, not yet deployed):** `CCSS:5.G.A.1`, `CCSS:5.NF.B.7`, `CCSS:6.EE.A.2`, and `CCSS:7.NS.A.2`. An embedded source-anchored, hash-checked manifest and additive, idempotent seeder update verify the existing accepted baseline before updating *only* official standard prose/hash and the import-state digest. No learner records, outcome links or skills are changed. The exact live Neon baseline matches the accepted pre-repair digest.
- **121 sub-clauses:** the initial text-signature audit flagged 10 apparently missing clauses, but direct reading established 4 were formatting/notation false positives and 6 really absent from parent official text (2 each in 5.NF.B.7, 6.EE.A.2 and 7.NS.A.2). The repair manifest supplies all 6. The fact that clauses now occur in the official text is **not proof that questions test them**.
- **35 MP-only lessons:** retain the original explicit MP links. 30 are directly based on numbered publisher MP alignments in the lesson blueprints; 5 high-school lessons use reviewed `FormalTargets` with source evidence, not publisher-supplied alignments. Do not silently replace MP-only links with previously proposed content-standard candidates.
- **Practice semantic gate fails:** manual spot-check of four content standards found current question families exceeding grade scope or lacking required skill specificity. In particular `CCSS:5.G.A.1` exposes midpoint problems, `CCSS:5.NF.B.7` exposes generic fraction-multiplication/simplification, `CCSS:6.EE.A.2` lacks evidence of identifying expression terms/coefficients, and `CCSS:7.NS.A.2` lacks proven signed-rational/decimal-conversion families.
- **Release decision remains NO-GO:** written-text corrections and arithmetic test passes cannot substitute for subject-specific question/answer and assessment review across all 363 parent standards and 121 clauses. Do not merge or deploy this repair without migration rehearsal on a disposable database branch, relevant full CI, academic scope corrections and staging smoke checks.

Evidence: `src/Edulytics.Core/Curriculum/Packs/us-ccss-math.authority-text-repairs.json`, `tests/Edulytics.Tests/Phase275/CommonCoreVerifiedAuthorityTextRepairTests.cs`, `docs/curriculum/us-ccss-35-mp-publisher-decisions.json`, and `docs/curriculum/us-ccss-practice-semantic-blockers.json`.