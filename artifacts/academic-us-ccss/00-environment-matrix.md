# PHASE 00 — verified source / deployment / database baseline

**Scope:** `US-CCSS-MATH` • `CCSSM-2010` • Grades 1–12 only; Kindergarten excluded.

**Code/input baseline:** `fd72849823ae31b92e58fb420a373218fc25b328` on `EIAS79/Edulytics/main` at start of this bounded audit. This SHA identifies the *inputs* to the audit, not the eventual academic release candidate or an internal certification decision.

| System | Observed evidence | Meaning |
|---|---|---|
| GitHub | Main `fd72849823ae31b92e58fb420a373218fc25b328`; #461, #462, #463 merged | Technical change baseline, not signoff |
| Render production | Service `srv-dakq5n2fngtc73a62i10`; verified live deploy `dep-db5v77gu01pc73f3b390` at exactly `fd728498...` | Technical application deployment |
| HTTP | `/health/live`, `/health/ready`, `/account/login?culture=en`, `/` each returned HTTP 200 after prior deployment | Unauthenticated smoke only, **not** staging student/admin/teacher acceptance |
| Neon production | Project `tiny-lab-44877119`, branch `br-frosty-block-b52tnjky`; canonical US import state content digest `b9e10bdceab9cb587ba9ea25797336d85b9ef76442b8783dff0ef770696c747f` | Live data is at six-source-corrections predecessor, not the nine-correction state |
| Official standards | 363 parent and 121 child codes; source snapshot 60 pages | Reproducible source inventory, not semantic review |
| Source lessons | 17 blueprints and 1,560 lessons; source URL for every lesson (V2 HS metadata lives in nested `Sources[]`) | Source fields known, independent license and teaching quality pending |
| Live school routing | Eight `Course/pathway mapping` HS adoptions, zero confirmed new explicit HS course keys at preceding read-only check | No automatic course assignment; authorized individual school choice required |
| Official HS repairs | Three persisted old-`ContentHash` values and import digest matched the protected before-state of the committed manifest | Exactly three **eligible for guarded review**, not permission to mutate production |
| Backup/snapshot | 2026-10-11 attempt to create a fresh snapshot was rejected by Neon: `snapshots limit exceeded` | **Production canonical migration blocked** until a verifiable backup/rollback alternative and operations signoff exist |
| Academic reviewers | No independent reviewers/signoff identities supplied | Full internal certification **NO-GO** |

All database reads in this closure batch were read-only. No SQL UPDATE or DELETE was executed against production. Do not use the unverified historical snapshot as an automatically equivalent fresh backup.

## Reproducing the baseline

```bash
node tools/us-ccss-standards-audit.cjs --strict
node tools/us-ccss-academic-evidence-ledger.cjs
node tools/us-ccss-practice-semantic-gate.cjs --check
node tools/us-ccss-source-provenance-audit.cjs
node tools/us-ccss-official-adjudication-queue.cjs
node tools/us-ccss-content-integrity-audit.cjs
node tools/us-ccss-clause-gaps.cjs
node tools/us-ccss-certification-gate.cjs --stage=predeploy --strict
```

**Gate 00 still PARTIAL**, because an independent academic reviewer and the ultimate approved immutable target SHA have not been established. This document must be refreshed against that release candidate before final decision.
