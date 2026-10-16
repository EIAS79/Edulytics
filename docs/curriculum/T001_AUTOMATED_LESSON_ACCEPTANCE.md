# T001 lesson acceptance — tests only (no mandatory human academic reviewer)

**Scope:** US-CCSS Grade 1 Unit 1, lessons L01–L10, in the isolated T001 release candidate. This supersedes *only* the mandatory human academic-review/sign-off step for this T001 delivery workflow. It does **not** change historical B01 records, other curricula, teacher approval of teacher-authored assessments, or the original publisher's educational objectives.

## Operational acceptance rule

A lesson is **TEST-ACCEPTED FOR SOFTWARE RELEASE** when the lesson's actual code, content/source mapping, question generator, independent verifier, student interaction (where required), authenticated answer submission, grading, persistent storage and ownership/regression/security checks have all passed on **the exact same candidate Git SHA**.

**No outside specialist, teacher, or separate academic review signature is required** to grant this *technical* acceptance. Failures create engineering fixes and retests, not a human-review queue.

### Required reproducible proof (T001)

1. `t001-compile-and-source-math`: source-locked L01–L10 math/question families, seeded independent correctness and malformed-answer tests, client JavaScript syntax, integrity guard. Fail when a family cannot generate, grade or meet meaningful item diversity.
2. `independent-full-product-regression`: broad independent system regression, school/account isolation, bootstrap/seed/runtime compatibility.
3. `isolated-us-school-https`: genuine authenticated student Practice and grading through HTTPS, desktop and mobile Chromium, persisted PostgreSQL answer/score proof, approved lesson-family scope, cross-school isolation and content rendering.

The executable `tools/us-ccss-t001-test-acceptance.cjs` reads **current GitHub check runs**, not hand-entered PASS labels or a human reviewer field. It fails closed until all three checks on the requested full commit SHA have concluded `success`; it reports `BATCH_TESTS_ACCEPTED` only then. Run it **after** workflows finish:

```bash
node tools/us-ccss-t001-test-acceptance.cjs --sha=<full-commit-sha>
```

For a private repository, supply a read-only `GITHUB_TOKEN` with Checks access.

## What the acceptance means—and does not mean

- Means: the implemented Edulytics curriculum/Practice behavior for these lessons passed the specified technical and source-coverage tests on a known revision.
- Does **not** mean: a real child demonstrated mastery; a physical teacher-observed explanation was measured; L09 actually polled independent human participants; an external publisher or authority accredited Edulytics.
- The lessons retain their original learning goals. The L09 backend verifies that the anonymous votes and pupil-built chart are internally consistent. It does **not** prove the origin of the responses.
- No previously failed, incomplete, absent or SHA-stale technical test can be re-labeled PASS.

## Separation from historical academic-certification records

The legacy `artifacts/academic-us-ccss/04-lesson-certification-records.json` and its six-gate academic decision are preserved as **historical academic evidence**, not silently forged or promoted. `BATCH_NO-GO` in that ledger does not negate a separate tests-only technical verdict, and a tests-only verdict does **not** become `BATCH_CERTIFIED`. This policy removes a mandatory person in the **software acceptance path**, not provenance, release SHA, privacy, correctness or test safeguards.

**Workflow status:** PR #494 T001 base; PR #495 graph authoring; PR #496 additional authored activities. Merge and deployment are separate release operations and remain out of scope for this policy update.
