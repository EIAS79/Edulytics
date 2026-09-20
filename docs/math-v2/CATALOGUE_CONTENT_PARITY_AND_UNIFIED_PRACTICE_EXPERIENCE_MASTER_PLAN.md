# Edulytics Catalogue Content Parity & Unified Practice Experience Master Plan

**Status:** PLANNED  
**Created:** 2026-09-20  
**Execution stages:** 8 (`U1`–`U8`)  
**Scope:** whole published mathematics catalogue, all learner lesson pages, all lesson-launched Practice routes, Production persistence, CI/runtime/deployment verification.

---

# 1. Why this plan exists

The previous corrective programme closed several real runtime defects, but live Production evidence exposed two remaining system-level gaps that must be corrected across the entire product rather than patched lesson-by-lesson.

## 1.1 Confirmed content-persistence divergence

For the Supporting lesson:

`PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-1:BUILD`  
**Quantify additive and multiplicative relationships: Build the Idea**

Production currently contains:

`phase29-cambridge-primary-stage6-dfe-ogl-v1`

with generic learner text such as:

- “Always state what each number, unit, operation or geometric property represents before calculating.”
- “Explain each step and name the mathematical relationship being used.”
- generic five-step problem-solving guidance.

However, the reviewed Supporting Practice target registry contains a target-specific rule for:

`Quantify additive and multiplicative relationships`

and the in-memory audit path applies `SupportingLessonPracticeContentCorrections` before semantic inspection.

Therefore the audit can inspect corrected content while the persisted Production lesson remains old.

This is a verified architecture mismatch:

`audited corrected content != persisted Production content`

## 1.2 Confirmed Practice presentation divergence

The Supporting lesson:

`PED:CAMBRIDGE-INTL-MATH:S6:6F-3:BUILD`  
**Compare fractions with different denominators: Build the Idea**

opens the specialized lesson Practice route:

`/student/practice/game`

and renders the existing server-verified Practice experience.

The Supporting lesson:

`PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-1:BUILD`

currently launches through:

`/student/practice/attempt/{attemptId}?mode=lesson-game`

which renders the generic `Attempt.cshtml` experience.

The `Attempt.cshtml` file itself was not changed by the previous corrective work; the visible experience changed because routing now sends some lesson Practice sessions through that generic page.

This is a verified architecture mismatch:

`lesson Practice capability != lesson Practice presentation route`

## 1.3 Confirmed Practice-quality risk

The live `6AS-MD-1:BUILD` Practice attempt generated ten questions from:

- Skill: `supporting.reasoning.core`
- Family: `supporting.reasoning.multistep`

with repeated algebraic forms similar to:

`x × a + b = total; find x`

The questions are technically solvable and verifier-compatible, but technical validity alone is not sufficient evidence of lesson-target coverage, question variety, Build/Apply distinction, or appropriate interaction design.

---

# 2. Required end state

A published lesson is not considered complete until this chain is true:

`Canonical lesson target`
→ `Reviewed learner content`
→ `Persisted Production learner content`
→ `Exact Practice capability`
→ `Exact Question Family set`
→ `Appropriate interaction renderer`
→ `Unified Practice shell`
→ `Production verification`

The same authoritative lesson state must be used by:

- audits,
- CI,
- seeding/persistence,
- student lesson pages,
- lesson Practice launch,
- Production verification.

No audit-only correction layer may claim a lesson is fixed unless the same effective content is persisted or deterministically materialized for Production.

---

# 3. Non-negotiable principles

1. **No lesson-specific emergency patches as the primary architecture.** Known lessons may be regression fixtures, but the repair must cover the whole catalogue.
2. **Audit truth must equal Production truth.** In-memory corrected content and persisted learner content cannot diverge silently.
3. **Lesson Practice and Personal/AI Practice are separate product modes.** A lesson-launched Practice session must not silently fall into a generic personal-practice page merely because an exact contract exists.
4. **One Practice shell, multiple mathematical interactions.** The overall student experience remains consistent while the inner control/renderer changes by skill.
5. **Do not force one interaction mechanic onto every subject.** Fractions, scales, geometry, graphs, drag/drop, numeric entry and other skills keep specialized controls.
6. **Practice availability is not pedagogical certification.** Runtime existence, mathematical verification, target alignment, diversity and learner experience remain separate gates.
7. **No silent fallback.** If a required renderer or content projection is unavailable, CI must identify the exact lesson/family rather than masking the gap.
8. **No invented official outcomes.** Supporting lessons remain Supporting and do not acquire fabricated curriculum outcome mappings.
9. **Production verification must inspect persisted/live state, not only embedded source packs.**
10. **Existing specialized Practice experience is preserved unless an intentional redesign is explicitly approved.**

---

# 4. Execution plan

## U1 — Whole-catalogue divergence inventory

### Objective

Build one authoritative inventory of every published lesson where any of the following differ:

- embedded canonical lesson body,
- corrected/materialized learner body,
- persisted Production learner body,
- Practice capability,
- Practice Question Families,
- selected Practice route,
- selected renderer.

### Required work

For every published mathematics lesson:

1. resolve its canonical lesson record,
2. apply all reviewed content corrections using the same production materialization code,
3. compute a deterministic content fingerprint,
4. read the persisted Production content fingerprint,
5. resolve `LessonPracticeCapabilityResolver`,
6. resolve the lesson Practice presentation route,
7. record the Question Families and renderer,
8. classify divergence.

### Required classifications

- `CONTENT_PARITY_OK`
- `CONTENT_PRODUCTION_STALE`
- `CONTENT_RULE_MISSING`
- `PRACTICE_CAPABILITY_MISSING`
- `PRACTICE_ROUTE_MISMATCH`
- `PRACTICE_RENDERER_MISSING`
- `PRACTICE_GENERIC_FALLBACK`
- `QUESTION_FAMILY_ALIGNMENT_REVIEW`

### Mandatory regression fixtures

At minimum include:

- `S6:6AS-MD-1:BUILD`
- `S6:6F-3:BUILD`
- `S6:6NPV-4:BUILD`
- `S6:6F-2:BUILD`

### Exit criteria

- Inventory covers the entire published catalogue.
- Every mismatch is listed by exact LessonCode.
- No “READY” aggregate may hide route/content divergence.

---

## U2 — One effective learner-content authority

### Objective

Eliminate the split between “content seen by audit” and “content persisted for students”.

### Architecture

Introduce one deterministic content materialization service/function:

`CanonicalLessonContentMaterializer`

It must own the ordered application of:

- canonical embedded lesson body,
- Stage-specific reviewed corrections,
- Supporting Practice content corrections,
- Official Practice content corrections,
- Polish content corrections,
- any future reviewed correction layer.

Both audit and persistence must call this same authority.

### Required guarantees

For a given:

`LessonCode + source pack version + correction registry version`

the materializer must return the same effective learner body everywhere.

Create a stable fingerprint over:

- Explanation
- KeyConceptsAndRules
- WorkedExamples
- StepByStepSolutions
- CommonMistakes
- QuickSummary
- culture
- effective content version

### Exit criteria

- No independent audit-only correction implementation remains.
- Audit and seeder use the same materializer.
- Equivalent source input produces identical content fingerprints.

---

## U3 — Production content parity repair

### Objective

Persist all approved effective learner content safely for every lesson that requires it.

### Required work

1. Compare effective content fingerprints with Production DB.
2. Produce an exact update set.
3. Preserve:
   - lesson identity,
   - framework identity,
   - official OutcomeCodes,
   - Supporting status,
   - culture boundaries.
4. Upgrade only when the incoming effective version is reviewed and permitted.
5. Reject unrelated drift rather than silently overwriting it.
6. Persist an auditable effective-content version/fingerprint.

### Production parity gate

Deployment closure must prove:

`EffectiveContentFingerprint == ProductionContentFingerprint`

for every published lesson.

### Exit criteria

- Zero `CONTENT_PRODUCTION_STALE`.
- Known `6AS-MD-1:BUILD` contains target-specific learner content in Production.
- Production DB and audit report the same effective content version/fingerprint for every lesson.

---

## U4 — Whole-catalogue learner-content certification

### Objective

Raise the content gate beyond the small known phrase list and certify actual lesson usefulness.

### Checks

For every published learner body:

- target/title semantic alignment,
- grade/age-appropriate explanation,
- worked example demonstrates the exact target,
- solution steps are target-specific,
- common mistakes are concept-specific,
- no teacher/authoring instructions,
- no generic all-purpose problem-solving template,
- no unrelated skill drift,
- Build vs Apply distinction where both exist,
- no malformed mathematical symbols or truncated comparisons,
- visual references only when a corresponding visual/model exists,
- supporting lesson wording remains pedagogical and does not imply official mapping.

### Exit criteria

- Zero unresolved content blockers.
- Zero unclassified learner bodies without explicit reviewed disposition.
- CI fails with exact LessonCode and field when a body regresses.

---

## U5 — Unified lesson Practice shell

### Objective

Make lesson-launched Practice visually and behaviorally consistent without flattening mathematical interaction types.

### Product rule

All lesson-launched Practice must enter one learner shell with consistent:

- lesson title,
- domain/unit,
- Practice identity,
- progress indicator,
- score/reward state,
- Eddy/feedback area,
- navigation,
- completion state,
- responsive layout,
- accessibility behavior.

The shell must not unexpectedly switch to a generic “Personal practice test” presentation for a lesson Practice session.

### Routing rule

`Lesson page → Lesson Practice Orchestrator → Unified Practice Shell → Interaction Renderer`

Do not route lesson Practice directly to a separate generic personal-practice page based only on contract availability.

### Personal/AI Practice

Personal/adaptive Practice may retain its own orchestration semantics, but lesson-launched Practice must be visually identified as the selected lesson and remain inside the unified lesson Practice experience.

### Exit criteria

- Every lesson-launched Practice session uses the unified shell.
- No lesson Practice URL/presentation silently falls back to generic Personal Practice.
- Existing specialized lesson game appearance is preserved unless explicitly improved.

---

## U6 — Specialized interaction renderer registry

### Objective

Keep one Practice experience while selecting the correct interaction for each mathematical family.

### Required renderer categories

The registry must explicitly support, where required by current families:

- numeric entry / keypad,
- comparison operators,
- fractions,
- number line,
- reading scales,
- multiple choice,
- drag/drop,
- ordering,
- graph/plot interaction,
- geometry/shape interaction,
- coordinate interaction,
- algebraic entry,
- measurement/unit input,
- other currently published exact mechanics.

### Contract

Each Practice Question Family must declare or resolve:

- interaction type,
- renderer key,
- answer contract,
- validation contract,
- accessibility fallback.

No Question Family may reach Production lesson Practice without a compatible renderer.

### Exit criteria

- Zero `PRACTICE_RENDERER_MISSING`.
- Zero uncontrolled generic fallback.
- Whole-catalogue family → renderer compatibility gate is green.

---

## U7 — Practice target coverage, diversity and pedagogical certification

### Objective

Prevent technically correct but repetitive or under-scoped Practice.

### Per lesson / family checks

Across deterministic seeds and difficulty bands:

- exact lesson-target alignment,
- sufficient conceptual coverage,
- question-form diversity,
- parameter diversity,
- no near-duplicate sequence dominating a session,
- correct Build/Apply cognitive distinction,
- plausible distractors where applicable,
- appropriate interaction type,
- mathematically verified answer,
- independent verifier agreement,
- feedback/explanation quality,
- grade-appropriate wording.

### Session-level diversity gate

A generated session must be evaluated as a set, not only item-by-item.

Examples of failures:

- ten variants of `ax+b=c; find x` for a broader relationship lesson,
- ten fraction comparisons with no representation or reasoning variation when the lesson requires more,
- repeated identical difficulty structure with only changed numbers.

### Exit criteria

- Every published lesson has a certified Practice session profile.
- Session diversity and target-coverage gates are green.
- Known `6AS-MD-1:BUILD` no longer reduces the lesson to one repeated algebra template unless that exact scope is explicitly justified by the reviewed target.

---

## U8 — End-to-end CI, deployment and live Production closure

### Objective

Prove the entire learner chain on the deployed system.

### Permanent CI gates

CI must validate:

1. whole-catalogue effective-content materialization,
2. audit ↔ persistence fingerprint equality in integration fixtures,
3. learner-content certification,
4. Practice capability closure,
5. Question Family → renderer closure,
6. unified lesson Practice shell routing,
7. session-level Practice diversity,
8. solver/verifier correctness,
9. localization,
10. PostgreSQL/migrations,
11. architecture/security/tenant/IDOR,
12. SAST/CodeQL,
13. container build/vulnerability scan.

### Production verification

After deployment:

1. verify deployed SHA,
2. verify startup/migration logs,
3. verify zero unexpected application errors,
4. query Production DB for full effective-content parity,
5. verify representative Supporting and Official lessons,
6. perform authenticated learner smoke tests across representative renderer classes,
7. verify lesson title and Practice mode remain visible in the unified shell,
8. verify no generic Personal Practice fallback for lesson-launched Practice.

### Minimum authenticated smoke matrix

Include at least:

- fraction comparison,
- Reading Scales,
- additive/multiplicative relationships,
- numeric keypad family,
- multiple choice/operator family,
- one visual/interactive family,
- one geometry/graph family if published.

### Exit criteria

The plan may be marked `COMPLETED` only when:

- effective content parity is 100%,
- learner-content blockers are zero,
- Practice capability coverage is 100% for eligible lessons,
- family → renderer coverage is 100%,
- lesson Practice generic-fallback count is zero,
- Practice session-quality gates are green,
- Production DB matches the effective content authority,
- authenticated Production smoke is completed successfully.

---

# 5. Execution order

`U1 → U2 → U3 → U4 → U5 → U6 → U7 → U8`

Stages may gather evidence in parallel, but closure is sequential: a later green result cannot hide an unresolved earlier-stage mismatch.

---

# 6. Scoreboard

| Stage | Objective | Status |
|---|---|---|
| U1 | Whole-catalogue divergence inventory | NOT STARTED |
| U2 | One effective learner-content authority | NOT STARTED |
| U3 | Production content parity repair | NOT STARTED |
| U4 | Whole-catalogue learner-content certification | NOT STARTED |
| U5 | Unified lesson Practice shell | NOT STARTED |
| U6 | Specialized interaction renderer registry | NOT STARTED |
| U7 | Practice target coverage, diversity & pedagogical certification | NOT STARTED |
| U8 | CI, deployment & live Production closure | NOT STARTED |

---

# 7. Definition of done

The programme is complete only when these statements are simultaneously true:

- The learner content audited in CI is the same effective content persisted or deterministically served in Production.
- No published lesson displays stale generic content when a reviewed target-specific correction exists.
- All lesson-launched Practice sessions use one coherent Practice shell.
- Mathematical controls vary correctly by skill without changing the overall Practice product experience.
- No lesson Practice silently routes into generic Personal Practice.
- Practice sessions are mathematically correct, lesson-aligned, sufficiently varied and pedagogically appropriate.
- Production state is checked directly, not inferred from source files alone.
- Authenticated live learner smoke verification succeeds after the final deployment.

This document is the authoritative execution checkpoint for the catalogue content-parity and unified Practice-experience remediation.
