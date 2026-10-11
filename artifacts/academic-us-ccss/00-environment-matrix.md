# Phase 00 — Connected environment verification matrix

**Observed:** 2026-10-11. **Scope:** `US-CCSS-MATH / CCSSM-2010`, Grades 1–12 (Kindergarten excluded).  
**Status:** **PARTIAL / NO-GO**. All live data inspection was **read-only**.

| Boundary | Verified finding | Release meaning |
|---|---|---|
| GitHub | `EIAS79/Edulytics`, branch `docs/us-ccss-academic-certification-20261011` contains exact 13-phase plan and preliminary academic evidence tooling | Not merged to `main`; not a deployed certification |
| Existing HS PR | [#461](https://github.com/EIAS79/Edulytics/pull/461) was open; four associated CI workflows passed for the previous reviewed head | Adds pathway choices but **not** per-course (`CourseCode`) adoption |
| HS production defect | [#453](https://github.com/EIAS79/Edulytics/issues/453) open | Lesson-scoped Practice still needs safe exact course routing |
| Render account / workspace | **Khalifa → My Workspace** (`tea-dakq3dh594qs7395pgt0`) | Explicit production target, not an assumption about another workspace |
| Render production web service | `Edulytics` (`srv-dakq5n2fngtc73a62i10`), https://edulytics-4346.onrender.com | Latest checked deploy ID `dep-db5t2mh5efls73b0rb30` at SHA `4dfbfcac04cc900076c8f7af744b3576131d78f2`, status `live`; **not** proof of full user journey |
| Render staging (last service inventory) | `edulytics-v2-neon-staging` (`srv-db42982d0e5s73fhvpd0`) was suspended with manual deployment | Real staging acceptance has **not** been signed off; not substituted by disposable CI |
| Neon production | Project `tiny-lab-44877119`, branch `br-frosty-block-b52tnjky`, account `KHAlid` | Read-only SQL: **1,560** `PED:US-CCSS-MATH:%` lessons |
| Neon HS school adoptions | **8** production adoptions still have `Course/pathway mapping` | Do not assign ALG1/GEO/ALG2 automatically; require authorized SchoolAdmin course choice |
| Neon standard prose | Previous read-only check observed three guarded high-school exponent/notation corrections not yet applied | Must perform guarded, rehearsed migration; no ad hoc SQL against live data |
| Current academic register | 363 parent standards, 121 child clauses; 0/121 full independent certification, 98 historical unreviewed source-text mismatches | Academic certification remains **NO-GO** |
| User data | No production write, migration, school/adoption record update, sign-in with real account or deploy invoked in this evidence batch | User data remains untouched by this execution step |

## Release controls

1. Treat GitHub `main`, Render live SHA, and Neon canonical content digests as **three separately verified states**.
2. `live` only establishes Render deployment status, not curriculum completeness.
3. Resolve the eight ambiguous HS adoption rows by explicit verified course assignment, preserving school scope/history; do not silently mutate.
4. Before any candidate production deployment: immutable candidate SHA, successful applicable CI, disposable migration rehearsal, authenticated real staging, independent academic review and sign-off, and authorized GO decision.
5. No current evidence supports `ACADEMICALLY_CERTIFIED_FULL`. The strict gate must continue to exit nonzero until independently supported.
