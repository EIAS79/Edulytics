# Edulytics — Clean Neon Rebuild & JSON Lesson Delivery

**Status:** Phase 1 implementation started — no Neon or Render changes.
**Date:** 2026-10-08
**GitHub:** EIAS79/Edulytics
**Baseline main:** 18cd29f6b97583f90f10e416cd1d2c6c7f051e6c
**Feature branch:** feat/neon-clean-bootstrap-json-content-20261008
**Render workspace observed:** My Workspace, tea-dakq3dh594qs7395pgt0, khalidkhalifa79@gmail.com. Exact target service not selected.
**Neon target:** Independent NEW account/project. Old quota-blocked Neon project excluded entirely.

## Constraints

- Zero migration from old Neon: no schools, identities, students, teachers, supervisors, admins, attempts, assessments, analytics, subscription/billing or demo records.
- Keep the schema and functionality necessary to create future new users and schools; definitions for the five Identity roles are allowed, but no precreated user.
- Build accepted *platform* curricula and lesson mappings from versioned JSON in GitHub. Do not equate JSON entries with importable/verified lesson count.
- Keep immutable approved lesson text in JSON after safe, tested content-provider refactor, not by deleting current DB content prematurely.
- No expensive production data maintenance on ordinary web startups; explicit idempotent seed process.
- Do not manipulate old database or deploy to Render before migration acceptance. Credentials stay in secrets.

## Source input snapshot

| Input | Count |
|---|---:|
| Curriculum JSON packs | 4 |
| Pedagogical blueprint JSON packs | 50 |
| Canonical content JSON packs | 69 |
| RichV2 JSON packs | 2 |
| Raw lesson entries across canonical content packs | 5,110 |
| Cambridge entries | 566 |
| Polish entries | 1,569 |
| UAE entries | 1,415 |
| US Common Core entries | 1,560 |

These are source entries, not a validated distinct/import-ready database count. Existing Phase 29 readiness requirements remain in force.

## 12 execution stages and gates

1. **Architecture & baseline:** freeze SHA, inventory EF tables, migrations, tenant ownership, bootstrap, Practice and lesson dependencies. **Gate:** complete dependency matrix.
2. **Database usage fixes:** review open PR #418 worker idle backoff rather than duplicate; optimize lesson catalogue overfetch, health probes, analytics, practice context, startup maintenance and query budgets. **Gate:** CI and measured isolated-database regressions green.
3. **Offline canonical content audit:** parse packs; validate unique IDs, versions, locales, outcome codes, source rights, published statuses, content math correctness; compare against Phase 29 strict closure. **Gate:** deterministic accepted/blocked inventory.
4. **JSON content reader:** versioned manifest keyed by curriculum/version/lesson/culture, validated hashes, bounded caching and authorized server-side retrieval; DB still handles adoption/identity/outcome links and mutable tenant content. **Gate:** identical student/staff/Practice behavior and no public content leak.
5. **Clean EF schema:** replay all migrations from zero on isolated PostgreSQL 18; verify indexes/FKs/history. **Gate:** repeatable clean initialization.
6. **Reference seeding:** framework packs -> pedagogical lessons + outcomes -> approved publication/manifest data; refactor canonical text seeder to new design. **Gate:** IDs/hashes match, no tenant rows.
7. **Zero-demo/bootstrap safety:** disable demo seeders, meeting repairs and unattended SuperAdmin account in clean mode; retain only role definitions. **Gate:** zero rows for old/unsolicited users, schools, personal state and financial events.
8. **Testing:** CI, SAST, schema, security, end-to-end lessons/locales, synthetic disposable school account, adaptive practice, assessments, analytics and performance. **Gate:** green and test data isolated.
9. **New Neon Free project:** operator selects correct account, PostgreSQL 18, secure pooled runtime/direct migrations, migrations/seed, clean rows, backups. **Gate:** independent verified new DB.
10. **Render change:** operator confirms exact service/workspace, deploy reviewed build, point to new Neon, verify health and behaviors; never delete old DB. **Gate:** correct live instance and new connections.
11. **Quota monitoring:** track egress, compute, idle polls, connections, query volume/latency and alerts; keep recovery runbook. **Gate:** observed safe consumption.
12. **Signoff:** deliver accepted lesson counts, CI, source versions, schema/seed commands, zero-tenant proof, deployment and cost evidence. **Gate:** full acceptance.

## Implementation notes (2026-10-08)

- Connected GitHub login: EIAS79. Baseline branch: main. Working branch created separately.
- PR #418 remains open; worker-polling code lives there and is not duplicated.
- Offline inventory added at tools/phase29/clean_neon_content_inventory.py with unit regression tests and a CI workflow. This script never connects to Neon or reads personal information.
- **Not done:** final JSON readiness validation, production code refactor, database creation, Render deployment.
- No Render service has been selected or changed.

## Audit commands

    python3 tools/phase29/clean_neon_content_inventory.py --strict --write-report
    python3 -m unittest discover -s tests/python -p 'test_clean_neon_content_inventory.py' -v
    python3 tools/phase29/full_curriculum_closure_audit.py --strict --write-report

The clean Neon plan does not supersede the separate curriculum-intelligence and advanced mathematics engine roadmaps.

## Verification 2026-10-08

- [GitHub Actions run #37845473560](https://github.com/EIAS79/Edulytics/actions/runs/37845473560): **success** on the latest implementation commit.
- Python inventory regression suite: **5 passed**.
- Actual source corpus: **5,110 lesson entries, 5,110 unique codes, 0 structural errors, 0 missing-translation warnings**; this still does not certify the database import or educational QA.
- .NET solution build: **succeeded**.
- Focused bootstrap and curriculum contract tests: **11 passed, 0 failed**.
- New Neon project, Render deployment, full end-to-end migration: **not yet performed**.
