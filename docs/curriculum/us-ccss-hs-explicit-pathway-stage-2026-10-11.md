# American Common Core: explicit high-school pathway selection — partial issue #453 remediation

New US HS school adoptions previously offered only `Course/pathway mapping` (a placeholder) in the normal level-picker. Because lesson blueprints are actually authored under `Traditional` and `Fourth Courses`, this produced valid-looking student assignments with zero exact lesson-scoped Practice choices.

This bounded change retains the **legacy placeholder identity readable for old school records** but removes it from new American HS selection; each Grade 9–12 selector instead explicitly presents `Traditional` and `Fourth Courses`, which exactly match accepted blueprint `Pathway` values. No automatically selected pathway is invented and service-side validation refuses a forged or stale placeholder key.

Acceptance requirements: stable unique key for each logical Grade 9–12 × pathway, no placeholder among new school picker options, unchanged legacy key resolution, and unchanged non-American identity registries. No database migration is applied and current school adoptions are not silently rewritten.

**Issue #453 remains open:** `Traditional` itself spans **Algebra 1, Geometry and Algebra 2** (`CourseCode` in the accepted High School LessonBlueprints); choosing a pathway alone does not prove that Grade 11 receives exactly Algebra 2. The administration UI still needs **separate source-backed course selection and safe migration/update of existing placeholder adoptions**, then end-to-end live learner/teacher verification. Until that is completed, do NOT mark high school programme release-ready or full US academic CCSS certification.
