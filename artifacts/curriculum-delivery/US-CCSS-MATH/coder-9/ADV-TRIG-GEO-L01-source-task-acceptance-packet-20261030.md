# Coder 9 — L01 exact original task, representation and assessment decision packet
**Date:** 2026-10-30. **Lesson:** `PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L01`. **PR:** #586. **Source:** MVP Secondary Math III task **7.5 Maintaining Your Identity**, original local `sec3mod7tn718.pdf` pp. 50–53, with teacher purpose on p. 53. Existing independently recorded SHA-256 `8213e7498696cc71320ae254d21606083b0d24fbe73acc8a27e1588f0bc1d790` matches the authorized Our-CS source; no edition change or new download. **Status: SUBMITTED_FOR_REVIEW; NOT_AN_ACADEMIC_APPROVAL.**

## Exact question-to-content comparison (not publisher reproduction)

| Original task (source PDF) | L01 verified learner provision | Gap/action and evidence |
| --- | --- | --- |
| #1 acute-triangle cofunction relation in degrees | Exact `π/6` and `π/3` triangle, `sin x=cos(π/2−x)`, triangle interpretation | **Covered by original first-party example**; original publisher picture not copied |
| #2 unit-circle extension of cofunction relation beyond acute angles | Generic `(cos x,sin x)`, reflection across `y=x`, explicit proof and signed checks | **Covered as an accessible coordinate construction**; independent academic reviewer must decide whether this replaces the source visual affordance adequately |
| #3(a–d) parity, Pythagorean relation and sine/cosine ratio identity, with diagram reasoning | `sin(−x)=−sin x`, `cos(−x)=cos x`, `sin²x+cos²x=1`, `tan x=sin x/cos x`, domain `cos x≠0`; diagram-free proof requirement | **Math covered**; teacher rubric needs general reflection and domain, not only a value |
| #4 conjecture six `π−x`, `π+x`, `2π−x` identities; use graphs for supporting evidence | General circle sign and rotation identities were already correct; added first-party **graph/table investigation** covering all six, with accessible angle-value table and a clear graph-is-not-proof distinction | **Specific new gap closed at authored-content level**, not yet student-renderer or signed source parity |
| #5 prove `tan(−x)` odd and `tan(π+x)` periodic through algebra | Quotient identities and poles were already stated; added **both symbolic derivations** under `cos x≠0` | **Specific new gap closed at authored-content level** |
| Independent understanding assessment | Previously text-based identities and numeric checks; now explicit four-part **Check for understanding** with exact radical/sign answers, generic identity proof, undefined tangent and reflection explanation | **Authored first-party assessment added**; teacher-scored proof remains distinct from runtime Practice |

## Mathematical checks / expected outcomes
- Exact answers: `cos(π−π/3)=−1/2`, `sin(π+π/3)=−sqrt(3)/2`. No tolerance or signs inferred from approximate calculator results.
- `sin(π−x)=sin x`, `cos(π−x)=−cos x`; `sin(π+x)=−sin x`, `cos(π+x)=−cos x`; `sin(2π−x)=−sin x`, `cos(2π−x)=cos x` for real x.
- `tan(−x)=−tan x`, `tan(π+x)=tan x` **only** when `cos x≠0`; `tan(π/2)` excluded. The new focused source-task Node test checks signed and non-special angles, exact values, and this exclusion.
- Accessible diagram substitute: original first-party general-coordinate reflections plus nonvisual table. Graph comparisons offer **support**, not mathematical proof; no proprietary publisher graph or diagram embedded.
- Human rubric: existing `ADV-TRIG-GEO-human-scoring-rubrics.v1.json`, L01, three criteria ×2 points =6. Numeric generators must not award these proof points automatically.

## Practice contract / Shared Integration request
- Lesson-exact `PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L01`; candidates `l01-reflection-cos`, `l01-rotation-sin`, `l01-fullturn-cos`, `l01-tangent-pole` in `ADV-TRIG-GEO-parameterized-practice-acceptance.v1.json`. Existing candidate kernel `trig.unit_circle_source_exact_angles` on PR #600 is still **IntegrationPending**, not student-routable.
- Correct `-sqrt(3)/2`, equivalent `-0.8660254037844386` only if approximate answers are explicitly permitted, and exact algebraic equivalents; wrong sign, malformed syntax, nonfinite response, tangent pole, angle-unit mismatch must be rejected with deterministic feedback. Do not silently treat proof as numeric score.
- Shared owner (#587 / #453; PR #600): select and certify exact generator/normalizer/verifier, rubric workflow, feedback persistence, predecessor version/materialization, adopted `Fourth Courses + ADV-TRIG-GEO`, authenticated two-school negative isolation. Shared engine files are **not** changed in this branch.

## Explicit decision needed (authorized academic and version owner)
**Academic reviewer:** Accept/change/reject (with reviewer identity, dated verdict and exact candidate content SHA) whether this source-derived but independently written coordinate/table representation is sufficiently task-equivalent for original items #1–5, especially the source's pictured/right-triangle reasoning and graph-investigation affordance; check accessible learner rendering and assessment. Source's additional `F.TF.8` teacher focus is not silently added to locked formal targets.

**Authorized version/predecessor owner:** Determine and approve new append-only content version / predecessor link for this edited L01 body; original `sourceSha256`, `canonicalBodySha256`, `contentVersion`, LessonCode and formal targets are intentionally unchanged and cannot certify this transition. The pack's `Published` label is not evidence of real effective content.

**Verdict:** `SOURCE_TASK_MATHEMATICS_ALIGNED_CANDIDATE`; `A_CLOSED=false` (academic parity sign-off pending); `B_CLOSED=false` (approved learner version/academic sign-off pending); `C_PREPARED=true`, `C_CLOSED=false` (selected runtime Practice pending); `ABC_READY_FOR_D=false`. Cursor remains L01; Stage D prohibited.
