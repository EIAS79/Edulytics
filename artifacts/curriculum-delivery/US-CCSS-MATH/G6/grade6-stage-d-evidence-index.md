# Grade 6 Stage D — Tested-Candidate Evidence Index

This is a scoped technical test index, not a publisher-parity, academic approval, WCAG certification, release or deployment receipt.

- Candidate **branch HEAD**: [`274eea62cb64fff3b405b72b10eb0fbc13421b81`](https://github.com/EIAS79/Edulytics/commit/274eea62cb64fff3b405b72b10eb0fbc13421b81)
- Actual Actions run: [`#41752209024`](https://github.com/EIAS79/Edulytics/actions/runs/41752209024) — all nine Stage D job names completed successfully at that candidate.
- PR [#591](https://github.com/EIAS79/Edulytics/pull/591) is a **draft** and GitHub reports **mergeable=false / dirty**. No PR merge-ref verification.
- Grade 6 denominator: **147 lessons in nine units**, order 1–147.
- Real TRX per-lesson maths: **5,521/5,521 passed**, shards D1=1,538 (50 lessons), D2=1,851 (50), D3=2,132 (47). Run artifact: `g6-d-147-real-per-lesson-math-evidence-NOT-release`.
- Disposable PostgreSQL 18 + HTTPS + Student/Teacher/School Admin/Subject Supervisor: 147 exact lesson-family generations, 147 correct scored and stored, 147 malformed rejected and stored, 147 well-formed wrong rejected and stored, and 147 cross-school learner page denials. The six first-party authored teaching sections were asserted per lesson; learner pages were read with real role-scoped sessions. Run artifact: `g6-d-isolated-authenticated-147-evidence-not-release`.
- One synthetically authenticated Grade 6 teacher draft assessment was created and persisted, with foreign-school access denied. **Generation/review/approval/assignment/learner submission of that assessment has not been established**.
- Real headless Chromium desktop and mobile: **294/294 per-lesson viewport/DOM/focus/alt/overflow assertions passed**; 18 full-page screenshots from nine units. Run artifact: `g6-d-real-9unit-chromium-mobile-desktop-spotchecks`. **Not** a manual review of 147 complete visuals or a comprehensive WCAG audit.

## Limits and pending gates

1. Stage D suite success does not prove exhaustive academic source-element 1:1 equivalence. Verified numerator and denominator for source elements remain **UNKNOWN**, not 100%.
2. Independent review of third-party images, copyright/attribution, protected teacher resources and teacher-led drawing/rubric tasks remains a separate accountable process. No automated academic signing.
3. Assessment Builder review/approval/publish/delivery/student assessment result and diagnosis/remediation pathways remain unverified in this Grade 6 disposable runtime scope.
4. PR #591 cannot be treated as final CI until merge conflicts are resolved and required checks are executed against the applicable PR merge ref.
5. No merge, Render deployment, production database write or auto-release decision was performed.

`D_TESTS_PASSED` applies to the **specific tested suites** for 147 lessons. `D_VERIFIED=0`; `APPROVED=0`; `releaseDecision=NO_GO_TECHNICAL`.

Sources: actual GitHub Actions run linked above. This index is documentation committed *after* the tested candidate. A documentation commit is not itself a tested code SHA.
