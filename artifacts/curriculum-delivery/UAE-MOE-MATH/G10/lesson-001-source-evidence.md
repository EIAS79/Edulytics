# Grade 10 / General — U01L01 source-to-app audit (partial, NOT CLOSED)

**LessonCode:** `PED:UAE-MOE-MATH:L10:GENERAL:01:01:ADDING-AND-SUBTRACTING-POLYNOMIALS`
**Student book:** `01.pdf`, printed pages 7–12, physical PDF pages 20–25. **Verified file SHA256:** `d7e492a960675c30ac7eebb0144ed2dc51d1f4e9c91c9834ea05638f614a3c72`. 
**Source snapshot:** `G10-SEVEN-BOOKS-SHA-2026-11-23`; **status:** `A_C_EXPLICIT_GAP`; **tests:** `WRITTEN_NOT_RUN`.

## Actual visual comparison with student pages
- PDF 20–21: two learning goals are **standard form of polynomials** and **adding/subtracting polynomials**. Elements: polynomial/monomial terminology, monomial degree, polynomial degree, standard descending form, leading coefficient, classification, vertical/horizontal addition. The prior Arabic student content lacked explicit standard-form/leading-coefficient instruction and full classification.
- PDF 22: subtraction through the additive inverse, complete sign inversion, column alignment; a real-world sales model and guided problems. Prior Arabic content already explained additive inverse; original non-copying vertical and contextual examples were added.
- PDF 23: independent checks and graded tasks address polynomial identification, degree/leading coefficient, simplified addition/subtraction, and a word problem. Prior content was mathematical but lacked exercise-type coverage.
- PDF 24: multi-term symbolic exercises, real-world modeling, geometric perimeter/shape reasoning and open explanation. Added conceptual modeling guidance; manual sketch and explanation flagged for teacher rubric.
- PDF 25: context calculations, measurement/area data collection, error diagnosis, mathematical writing. A new short original Cool-down and rubric were added; open-ended physical tasks remain teacher-reviewed.

## Retained versus repaired
- **Retained correct:** Arabic definitions of like terms, distributive negative sign, original polynomial addition/subtraction examples, numerical substitution verification, core mistakes.
- **Added only missing:** standard form and degree/leading coefficient, non-polynomial counterexample, vertical grouping with two original trinomials, independent contextual example, non-digital task rule, original three-part Cool-down.
- **Originality:** no copied book diagram, exercise text or protected image embedded in the pack.
- **Practice reuse candidate:** existing Grade 9 `algebra.polynomial.add_subtract` uses `uae.g9.general.u07.l01.add_coefficient` and `.subtract_coefficient` for numeric coefficient checks, but **it does not yet constitute an exact Grade 10 runtime SkillContract**. Do not silently route Grade10 to the wrong family. Full-expression/diagram grading needs review or suitable extension.

## Explicit gaps / no fake closure
1. Canonical index and semantic overlap of all **seven** books remain `SOURCE_LOCK_PENDING`; 207 source entries / 156 candidate headings remain preliminary, not the final denominator.
2. Exact Grade 10 `SkillContract → question family → renderer → grader → independent verifier` runtime mapping is not confirmed. Separate addition/subtraction coefficient checks already exist for G9, but scope extension is not verified for G10.
3. All drawings, student physical activity and open mathematical explanations require original teacher rubrics or separately proven digital support.
4. The teacher-login Cool-down source was not available; the displayed Cool-down is `EDULYTICS_AUTHORED_SUBSTITUTE`, not claimed textbook-parity.
5. Tests added in this commit are **authored and NOT RUN**. No CI, .NET, E2E or Stage D results are claimed.

**Continuation cursor:** ordinal **1** until gaps above are resolved and `Evidence + Checkpoint + Commit` are sufficient for `READY_FOR_STAGE_D`. Do not advance to lesson 2 on this checkpoint.

## Cross-book index discovery — Advanced 2025–2026

The `01متقدم_.pdf` local file (SHA256 `0a554589f4cc72537007f3229098c1ab46775d7bf2301bae59f38fe6861a9815`), physical PDF page **7**, identifies Advanced Unit 3 lesson **3-1** as «العمليات على كثيرات الحدود», printed opening page **179**. Physical PDF page **196** (printed 179) confirms goals including simplifying expressions and adding/subtracting/multiplying/dividing polynomials. This creates a **candidate semantic overlap** with General U01L01 and adjacent lessons, not a proven duplicate. Further lesson-body visual review is pending. The preliminary 156-row source appendix incorrectly mapped Unit 3 lesson numbers/titles; rows 66–73 are corrected at the indexed-heading level, **not yet semantic closure**.

## 2026-11-23 — Advanced 3-1 semantic overlap and partial Stage C
Visual review of `01متقدم_.pdf` physical PDF pages 196–201 / printed 179–184 confirmed that Advanced 3-1 combines distinct skills rather than being a whole-lesson duplicate. Standard form/degree and add/sub overlap General U01L01; monomial/polynomial product objectives map toward General U01L02/U01L03; exponents toward General U02. Crosswalk: `artifacts/curriculum-delivery/UAE-MOE-MATH/G10/advanced-u03l01-semantic-crosswalk.json`.

Added an original multivariate polynomial-degree example to existing General U01L01 Arabic content. Inspected existing Grade9 exact numeric coefficient generation + Solve + Verify; registered a **Grade10-exact lesson mapping reusing these two numeric families**, with strict teacher-review boundaries for full expressions, visuals and explanations. This is **mapping authored only**, not selected Grade10 Practice runtime proof, and the whole unit/grade index is not locked. Teacher rubric: `artifacts/curriculum-delivery/UAE-MOE-MATH/G10/lesson-001-teacher-rubric.md`. Tests are `WRITTEN_NOT_RUN`. Do not close lesson or move to U01L02 yet.

## Direct owner-uploaded PDFs — 2026-11-23, no Desktop Commander calls

Reopened original owner-uploaded `01.pdf` (correct original SHA-256), visually reviewed physical pages **20–25** / printed **7–12** directly. New element-level ledger: `artifacts/curriculum-delivery/UAE-MOE-MATH/G10/lesson-001-source-elements.json` with **25 categorized elements**, source page, original first-party equivalent and non-digital assessment boundary. Important source requirement not previously explicit: original printed page 12 includes **tabulated/graphical optimization of area under fixed rectangle perimeter**, not simply symbolic addition; added first-party 40 cm perimeter example, (A(x)=20x-x^2), realistic domain and teacher scoring. Also authored a fixed + variable vehicle cost model (without reproducing publisher task). This is preparation of honest original equivalent content, not a claim of source artwork reproduction or authenticated visualization.

Verified all seven uploaded PDF byte hashes against prior source-inventory ledger and classified distinct editions/overlapping variants, including ambiguous `02-1.pdf` which **must not be assigned an unsupported pathway**. Source selection: `artifacts/curriculum-delivery/UAE-MOE-MATH/G10/source-selection.json`. **Only 25 source elements of one General lesson** are indexed here; no 7-book SOURCE_LOCK or Stage-D verification is claimed.

## 2026-11-23 — Precise selected Practice runtime status
Read `LessonPracticeCapabilityResolver.cs`, `LessonPracticeContractProjection.cs` and `LessonPracticeContracts.cs` directly. **Important:** simply creating a Grade10 entry in `lesson-skill-mappings.v1.json` does **not** enable learner Practice. Projection only emits an exact `READY_VERIFIED` mapping with **exactly one primary skill** and a recognized, routed, verified family. The current Grade10 U01L01 mapping is intentionally `PARTIAL_REUSE_ONLY_NOT_READY_FOR_STAGE_D` and lists four primary teaching skills, so **it cannot become a permitted selected Practice contract**. This is a real fail-closed safety boundary, not a passing test. Source-led Stage C contract with exact engine signatures and planned renderer is documented at `artifacts/curriculum-delivery/UAE-MOE-MATH/G10/lesson-001-stage-c-exact-contract.json`. Wrote C# exact generation, Arabic prompt and verifier regression checks at `tests/Edulytics.Tests/MathematicsIntelligence/UaeG10GeneralPolynomialAddSubtractReuseTests.cs`; **tests not executed** and **no runtime routing/passing score claimed**.
