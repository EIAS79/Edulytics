# US Common Core — source-specific student lesson, answer and grading HTTPS acceptance (2026-10-11)

## Re-audited gap

PR #451 introduced isolated US primary and secondary synthetic school authentication, private Practice generation, and two-tenant security regression. Its Practice submission used `WholeCurriculum`, so it did not independently confirm the learner's adopted framework, an exact publisher-lesson Practice contract, the permitted question family, or correct-answer grading and persisted completion.

## Evidence added in this bounded follow-up

For both American CI student accounts (primary and secondary), the new test:

1. Reads the real authenticated Practice form over HTTPS and requires the selected adopted framework identity to be exactly `US-CCSS-MATH | CCSSM-2010`.
2. Matches lesson IDs actually offered by the UI to the database lesson identities, then selects only a reviewed `READY_VERIFIED` US Common Core lesson mapping from the checked-in skill-contract source.
3. Generates a one-question `Lesson`-scoped private Practice attempt, not a mixed Whole Curriculum question; independently verifies the persisted exact `GenerationFamily` belongs to the original lesson's allowed families.
4. Uses the real rendered antiforgery and attempt-item fields to submit the exact correct answer over HTTPS, then checks that the attempt was submitted and one correct practice response/score was persisted.
5. Preserves prior cross-school denial checks for assessments and private Practice attempts.

The existing Cambridge rehearsal remains `WholeCurriculum` and its original behavior is not changed. These accounts and scores live only in disposable GitHub Actions PostgreSQL `edulytics_clean_ci`, not in Neon or public Render staging.

## Academic and deployment boundaries

This tests accurate **runtime routing of specific source-reviewed CCSS lesson question families**, not formal certification of all child standards, learner-drawn graph grading, essay grading, publisher child-reference attribution, or live staging authentication. Keep `productionReady:false` and all 121 clauses `NOT_FULLY_CERTIFIED`. Merge only after both US and Cambridge isolated acceptance checks pass. No production model, migrations, app source, framework content, school records, credential or release service change.


## High-school course-selection blocker found by the first strict run

The initial US secondary account had no learner-visible lessons. The reason is structural: the Common Core high-school `CurriculumLevelIdentityRegistry` contains a **placeholder** `Course/pathway mapping` for Grade 9–12, but accepted high-school source blueprints have concrete `Pathway` values such as `Traditional` and `Fourth Courses`. The private Practice repository matches these values exactly; a placeholder cannot match a course. Passing generic Whole Curriculum tests had previously concealed this.

The isolated acceptance job now explicitly binds the single **disposable secondary test adoption** for logical level 11 to the existing, source-recognized `Traditional` course before starting the HTTPS server. An assertion requires exactly one update. This validates the supported explicit course flow; it does **not** silently choose an American course for any real school or alter the curriculum level registry. Production-ready school setup still needs a formal pathway/course selector with identity-safe persistence and routing, and must never automatically combine unrelated high-school course sequences.

The 121 subordinate-clause academic release remains NO-GO, and live high-school adoption without an explicit course is also not cleared by this CI fixture.
