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
