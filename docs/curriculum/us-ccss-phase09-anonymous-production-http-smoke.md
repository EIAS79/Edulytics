# Phase 09 — independent anonymous production HTTP smoke

This is a **technical smoke check, not academic certification**, and does not authenticate any user. It checks the publicly deployed Edulytics URL using a GitHub Actions runner, avoiding an authorized local remote-desktop connector whose monthly quota was exhausted.

- Live `GET /health/live` must return HTTP 200.
- Readiness `GET /health/ready` must return HTTP 200.
- Public login `GET /account/login?culture=en` must return HTTP 200.
- Public landing `GET /` must return HTTP 200.
- Cross-origin redirects, nonallowlisted routes, 4xx/5xx, and connection failures fail closed.
- No passwords, tokens, learner data, cookies, or role sessions are collected.

**Limitations**: The checks do not assert the responding container's exact Git SHA; the Render deployment API is the separate source for deployed SHA. HTTP health alone does not establish a functioning practice/question flow or two-school tenant isolation. Synthetic authenticated CI is also not equivalent to staging acceptance with real authorized roles.

Run: `node --test tests/academic-us-ccss-public-production-http-smoke.test.cjs && node tools/us-ccss-public-production-http-smoke.cjs`.

The GitHub workflow runs on the review PR and on the `main` push of this smoke tool. It can also be manually dispatched. Do not change the overall `NO-GO` academic status based on this check.
