# Grade 6 Stage D — Current CI Status (not release sign-off)

**Tested code SHA:** [`274eea62cb64fff3b405b72b10eb0fbc13421b81`](https://github.com/EIAS79/Edulytics/commit/274eea62cb64fff3b405b72b10eb0fbc13421b81)
**Tests:** [GitHub Actions run 41752209024](https://github.com/EIAS79/Edulytics/actions/runs/41752209024)
**Reference:** `branch-head` (NOT a tested PR merge ref)
**Scope:** OUR/IM 6–8 Grade 6 — 147 lessons in 9 units.

| Check | Actual result |
|---|---|
| Inventory/build/Node | PASS on tested SHA |
| Grade 6 selected math TRX | PASS 5,521/5,521; 147/147 lessons |
| HTTPS authenticated first-party six sections | PASS 147/147 lessons |
| Lesson-specific generated family | PASS 147/147 generated cases (not all possible families) |
| Correct/invalid/well-formed wrong Practice grades stored | PASS 147/147 each |
| Cross-tenant lesson access denial | PASS 147/147 |
| Student/Teacher/Supervisor/Admin authentication | PASS synthetic accounts |
| Synthetic teacher assessment draft creation | PASS; full Builder flow untested |
| Chromium 2 viewports and baseline accessibility DOM | PASS 294/294; not full WCAG |
| Source-element 1:1 and rights | UNKNOWN / pending independent record |
| Human teacher-led review | Pending; not auto-approved |
| PR mergeability | BLOCKED: #591 draft with dirty merge conflicts |

**Per-lesson state:** `147 D_TESTS_PASSED` for the named tests, `0 D_VERIFIED`, `0 APPPROVED` from runtime.
**Decision:** `NO_GO_TECHNICAL` — outstanding technical workflows and PR merge ref. No merge or deployment done.

Refer to [the evidence index](grade6-stage-d-evidence-index.md) and [the per-lesson ledger](grade6-stage-d-execution-ledger.json).
