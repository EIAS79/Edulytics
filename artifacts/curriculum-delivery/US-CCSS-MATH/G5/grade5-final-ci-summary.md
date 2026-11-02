# Grade 5 Stage D — evidence on current candidate: NO_GO_TECHNICAL

**Release decision is unchanged.** Draft [PR #596](https://github.com/EIAS79/Edulytics/pull/596), no merge, no deployment, no production data access.

## Current tested evidence (not a claim that Stage D is complete)

- **Grade 5 code HEAD tested:** `56909a344340fa5b98342fd0728b88b6eb77cb5c`; [Stage D run 41366005943](https://github.com/EIAS79/Edulytics/actions/runs/41366005943).
- 148/148 unique lesson mappings: **PASS**. Shared .NET build and selected Grade5 mathematical/unit suite: **1352 passed, 0 failed, 0 skipped**. Per-lesson TRX: **120 direct lessons; 28 linked through Node**, not all individually end-to-end.
- Node D1: **129 passed, 0 failed, 1 TODO**. D2 and D3 source shard jobs successful; their 98 lessons rely on linked C# tests rather than Node suites.
- Original-source audit: **136/148 lessons with unknown original element denominator**; 12 itemized, 11 complete source snapshots. Hashed **483 first-edition Student Facing sections** are provenance, **not** independent full semantic or PDF/rights parity. Source gate correctly **failed closed**.
- [Authenticated acceptance run 41366005732](https://github.com/EIAS79/Edulytics/actions/runs/41366005732): **PASS** on PR synthetic merge SHA `332bd159804f0a8710c6810d6843b3fda19474a0`. Main-to-content technical parity passed; disposable HTTPS authenticated learner pages, selected Practice generation, persisted correct/malformed responses passed for **U01:L01 and U03:L02**. Actual U03:L02 student SVG geometry also passed. **Teacher rubric grading/persistence is not verified, and the other 146 lessons were not authenticated-E2E tested.**
- Cross-grade mathematical foundation [run 41366005827](https://github.com/EIAS79/Edulytics/actions/runs/41366005827) **FAIL**: G6 U01:L01 ten-item generation capacity and registry blockers; full V2 regression [41366005844](https://github.com/EIAS79/Edulytics/actions/runs/41366005844) **FAIL**: same capacity issue and Phase29 selector expected 294 / actual 286. Do not suppress unrelated guards.

## Acceptance and blockers

- **A–B–C authoring handoff flags:** 148/148, not independently certified original-source parity.
- **D_VERIFIED:** 0/148. **Academic approvals independently evidenced:** 0/148. **Full 148-lesson authenticated E2E:** not completed.
- Open: 136 unknown original source-element denominators, U01:L04–L07 explicit original-source gaps, original media/PDF rights, required authorized review, untested teacher rubric persistence, 1 D1 TODO, incomplete full runtime coverage, cross-grade CI failures and final full-grade gates.
- First incomplete acceptance ordinal **1**. Blocked lessons remain blocked until individually verified; no automatic acceptance from green shared jobs.

The **tested code SHA** and the **later documentation-only evidence commit SHA** must remain distinct. No evidence-only update grants a PASS. Reference [Issue #619](https://github.com/EIAS79/Edulytics/issues/619) for source/rights blockers.

**NO_GO_TECHNICAL — NO MERGE / NO DEPLOY.**
