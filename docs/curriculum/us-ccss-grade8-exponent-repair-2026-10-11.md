# US CCSS 8.EE.A.1 — verified exponent notation correction

## Academic evidence
User's official *Common Core State Standards for Mathematics* PDF (93 pages), PDF page 54, Grade 8 Expressions and Equations, standard **8.EE.A.1**. The accepted source pack's original OfficialText incorrectly flattened exponent notation from the mathematically correct worked example **3² × 3⁻⁵ = 3⁻³ = 1/3³ = 1/27** to `32 × 3–5 = 3–3 = 1/33 = 1/27`. This changes how a learner could interpret the mathematics, not merely typography.

## Release-scoped correction
- Preserve the original immutable accepted `us-ccss-math.curriculum-pack.json` and the published **five-issue** authority repair manifest/digest entirely unchanged.
- Add a second SHA-256-verifiable authority addendum for *one* official standard: `CCSS:8.EE.A.1` with original node hash, original text hash, corrected text/hash, verified predecessor five-fix digest and next digest.
- Seeder accepts exactly three legitimate CCSS states: baseline -> five repairs -> exponent correction; previously deployed five repairs -> exponent correction; fully corrected -> read-only validation. Unexpected state, altered rows, links or digests fail closed; all within the existing transaction on Npgsql.
- Neither lessons, learning outcomes, Practice grading, access control, school adoptions, nor student results are edited. Guarded database migration is forward-only and must be verified on **disposable PostgreSQL** before live rollout. Do not run the clean-seed tool on production or change the source digests to conceal drift.
- New test validates source identity, five-fix predecessor, exact exponent notation, and complete hash chain. It does not certify the Grade 8 standard's lesson-level skill/rubric or resolve the other baseline text differences.

## Outstanding
Proof is needed that the actual production/staging row advances to the new digest (startup maintenance may be disabled), with live/readiness checks. Full 121-clause academic signoff remains NO-GO.
