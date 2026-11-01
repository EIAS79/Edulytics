# Grade 5 Stage D — evidence index and execution boundary

Starting baseline `64902e974661d1a7136d988d039cb70841f817cb` on draft PR #596.
Observed CI failure CS1003 in source answer bank; targeted comma repair `eeb2b73734e5e673ef39b0d6f6696413f8c91030`.

## Existing real GitHub Actions results on baseline SHA

- Full Grade G1 negative-control: [run 41337750523](https://github.com/EIAS79/Edulytics/actions/runs/41337750523) — failed (G1-specific; not proof of G5 tests).
- Authenticated US two-school E2E: [run 41337750609](https://github.com/EIAS79/Edulytics/actions/runs/41337750609) — failed before E2E at unapproved G5 teaching parity.
- Original V2 parity: [run 41337750526](https://github.com/EIAS79/Edulytics/actions/runs/41337750526) — failed at Grade5 unapproved lesson body/metadata changes.
- Mathematics Intelligence: [run 41337750588](https://github.com/EIAS79/Edulytics/actions/runs/41337750588) — failed at C# compiler CS1003 prior to source repair.
- B001 20-lesson contract: [run 41337750594](https://github.com/EIAS79/Edulytics/actions/runs/41337750594) — passed but unrelated to full Grade5.

## Stage D implementation

- Workflow `.github/workflows/us-ccss-g5-stage-d.yml` asserts 148 unique source-linked lessons, executes Node-based source contract shards D1=1–50, D2=51–100, D3=101–148 and one shared .NET build/test job on disposable GitHub Actions runners.
- Stage D ledger initially gives no `D_VERIFIED` or fabricated SHA/run results. C# source test paths are handled by .NET test job, Node tests by three shards. A green static preflight is not academic certification.
- Original Grade5 academic source gap evidence U01:L04–L07 remains unresolved; publisher asset rights and teacher signoffs need independent evidence.
- Main curriculum-data parity currently fails when lesson bodies diverge from `main` without approved authoring evidence. Do NOT weaken parity script, mark approval automatic, or promote to release.
- Actual Authenticated Student/Teacher E2E, database, accessibility, production renderer and final green CI are NOT yet proven.

**Operational control:** Draft PR #596. No merge, deploy, production connection, or self-issued reviewer approval. Stage D diagnostic is fail-closed.


## 2026-11-01 execution results

- Tested code SHA: `eeb9e51e80fdc2f5135f3867e1b7cbcd3b471309`.
- Stage D dedicated run: [41338917084](https://github.com/EIAS79/Edulytics/actions/runs/41338917084) — 148-row manifest pass, 126/129 Node D1 (3 TODO), 1319/1319 Grade5 .NET pass; final academic gate blocked.
- Authenticated acceptance [41338917105](https://github.com/EIAS79/Edulytics/actions/runs/41338917105) failed at source-body parity before E2E. Original JSON parity [41338917132](https://github.com/EIAS79/Edulytics/actions/runs/41338917132) failed.
- A–B–C legacy U01 source conflicts and official mapping counts 210/26 vs 215/27 remain unapproved. [Source/academic issue #619](https://github.com/EIAS79/Edulytics/issues/619).
- Final Grade5 D_VERIFIED = 0 and release decision `NO_GO_TECHNICAL`. This documentation does not carry its own tested CI result.
