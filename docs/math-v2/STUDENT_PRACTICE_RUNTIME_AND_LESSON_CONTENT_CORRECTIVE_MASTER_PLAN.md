# Edulytics Student Practice Runtime & Lesson Content Corrective Master Plan

**Repository:** `EIAS79/Edulytics`  
**Baseline branch:** `main`  
**Baseline SHA:** `2740122f7ff1494ff3ff50b7cd201b44869b2d6b`  
**Created:** 2026-09-20  
**Status:** PLANNED — corrective execution not started  
**Execution stages:** 8 (`C1`–`C8`)

---

## 1. Purpose

This plan is the single execution source for correcting the production gaps discovered after the Polish 1,569 Practice remediation and live student-portal verification.

It covers four separate problems:

1. **Practice availability/runtime integration**
   - A lesson may be classified as Practice-ready by catalogue/audit tooling while the student lesson page still has no Practice entry point.
   - Confirmed reproduction:
     - `PED:CAMBRIDGE-INTL-MATH:S6:6F-2:BUILD`
     - **Express fractions in a common denomination: Build the Idea**
     - Production student page has no Practice tab/button.

2. **Divergence between Practice contract authority and game routing**
   - Some lessons receive Practice through `GameLessonRouteResolver` title/content routing rather than through the same generalized Practice capability path.
   - Confirmed working comparison:
     - `PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:BUILD`
     - **Reading scales with 2, 4, 5 or 10 intervals: Build the Idea**
     - Production page shows Practice and launches `/student/practice/game`.

3. **Student-facing lesson content quality/alignment**
   - The live `6NPV-4:BUILD` lesson contains generic place-value material instead of a learner-facing explanation of reading scales.
   - A more precise Reading Scales correction exists in code, but currently targets `6NPV-4:APPLY`, not `6NPV-4:BUILD`.

4. **Login copy**
   - Remove the requested Platform Administrator explanatory copy from the public login page in every supported login locale.

This programme must close the gaps with runtime evidence, not catalogue-level assumptions.

---

## 2. Non-negotiable rules

1. **No guessing.** A root cause is not accepted until a reproducible test or runtime probe identifies the failing condition.
2. **No fake curriculum alignment.** Supporting lessons remain Supporting lessons and must not receive invented official OutcomeCodes.
3. **No weakening verification.** Existing solver/verifier requirements, exact SkillContract routing and fail-closed behavior remain mandatory.
4. **No title-only production authority where an exact reviewed contract exists.**
5. **No generic student content accepted as lesson-complete.** Learner content must teach the exact target.
6. **No “4453/4453” production claim from Python/audit readiness alone.** Closure requires the live C# runtime/UI path.
7. **No single-lesson patch as final closure.** Reproductions identify defect classes; permanent gates must cover the full catalogue.
8. **No silent projection failure.** Projection/registry failures must be observable and test-failing.

---

## 3. Verified baseline facts

### 3.1 Practice visibility

`Views/StudentPortal/Lesson.cshtml` renders Lesson/Practice navigation only when one of these is present:

- `GameAdoptionId`
- `ExactPracticeAdoptionId`
- `LessonPracticePilotAdoptionId`

Current order:

1. game route,
2. exact Practice contract,
3. pilot fallback.

### 3.2 Confirmed working Supporting lesson

`PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:BUILD`

- Supporting lesson.
- Exists in the learner's Stage 6 workspace.
- Production Practice button is visible.
- Launches `/student/practice/game`.
- `GameLessonRouteResolver` has a specific `reading scales` route to `SCALE_READING`.

This proves **Supporting status itself does not hide Practice**.

### 3.3 Confirmed non-working Supporting lesson

`PED:CAMBRIDGE-INTL-MATH:S6:6F-2:BUILD`

- Supporting lesson.
- Exists in the same Stage 6 workspace.
- Production lesson page returns 200 but does not render Practice navigation.
- `supporting-practice-target-rules.v1.json` contains the reviewed exact-title pattern:
  - `^Express fractions in a common denomination$`
- The rule maps to:
  - Skill: `fractions.compare.unlike_denominators`
  - Family: `fractions.compare.unlike.common_denominator`
- The current game-content resolver has no specific route for this title.

The **final root cause of the missing exact runtime contract is not yet proven**. C1 must prove it before behavior is changed.

### 3.4 Practice audit/runtime distinction

Current catalogue/audit tooling reports the published mathematics catalogue as Practice-ready. That is not enough to prove student availability because the student page uses C# runtime routing/registries.

The new closure criteria therefore require runtime resolution and UI availability.

### 3.5 Confirmed lesson-content defect

For `PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:BUILD`, the Production database currently contains generic Phase 29 material such as:

- `A digit's value depends on its position.`
- place-value composition/decomposition,
- powers of ten,
- worked example based on `6,203,405`.

That is not an adequate learner-facing explanation of **Reading scales with 2, 4, 5 or 10 intervals**.

A targeted Reading Scales correction exists in:

`src/Edulytics.Data/Seeding/CambridgePrimaryStage6LessonContentCorrections.cs`

but currently targets:

`PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:APPLY`

not the `BUILD` lesson.

### 3.6 Login copy baseline

The requested Platform Administrator statements are currently embedded directly in:

`src/Edulytics.Web/Views/Account/Login.cshtml`

The current login implementation contains English and Polish inline variants.

---

# 4. Corrective execution stages

## C1 — Runtime truth baseline and reproducible probes

### Objective

Prove the exact live/runtime failure before fixing anything.

### Work

1. Add a direct C# test for:
   - `LessonPracticeContractRegistry.TryResolve("PED:CAMBRIDGE-INTL-MATH:S6:6F-2:BUILD", ...)`
   - returned contract,
   - readiness,
   - skill,
   - allowed families.

2. Add a paired control test for:
   - `PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:BUILD`.

3. Exercise `StudentPortalController.FindLessonPracticeAvailabilityAsync` or an extracted pure resolver for both lessons.

4. Record exactly which field becomes null:
   - game route,
   - exact Practice,
   - pilot.

5. If `TryResolve` is false for `6F-2:BUILD`, trace the supporting projection chain:
   - embedded content pack loading,
   - translation selection,
   - title normalization,
   - reviewed rule match,
   - SkillId validation,
   - question-family routing validation,
   - duplicate grouping,
   - final registry insertion.

6. If `TryResolve` is true, trace downstream until the exact condition that clears `ExactPracticeAdoptionId` is found.

### Exit criteria

- Exact failing statement/condition for `6F-2:BUILD` is proven by a red test.
- No routing fix before that red test exists.
- Working `6NPV-4:BUILD` remains green.

---

## C2 — Repair Practice contract/runtime availability

### Objective

Fix the proven runtime defect without weakening fail-closed behavior.

### Work

1. Correct only the verified failure in the projection/registry/controller path.
2. Remove any silent failure mode that can discard a large contract projection without a failing test.
3. Preserve:
   - reviewed-rule authority,
   - valid SkillIds,
   - allowed question families,
   - verification policies,
   - solver/verifier requirements.
4. Do not create an official OutcomeCode for Supporting lessons.
5. Do not hard-code only `6F-2:BUILD` unless evidence proves a genuinely lesson-specific exception.

### Exit criteria

For `6F-2:BUILD`:

- runtime contract resolves,
- readiness is `READY_VERIFIED`,
- exact question generation succeeds,
- persisted-item verification succeeds,
- student availability returns Practice,
- Lesson/Practice navigation is rendered.

---

## C3 — Establish one Practice availability authority

### Objective

Eliminate disagreement between catalogue readiness, generalized Practice contracts and game presentation routing.

### Target architecture

`LessonPracticeContractRegistry` (or a replacement single capability service) becomes the authority for:

- whether a lesson is Practice-capable,
- exact skill,
- allowed families,
- readiness,
- mechanic/capability metadata.

The game router becomes a **renderer/presentation selection layer**, not an independent mathematical eligibility authority.

### Work

1. Introduce one lesson Practice capability resolver.
2. Feed student lesson availability from this resolver.
3. Make game rendering consume the resolved capability where applicable.
4. Preserve specialized game experiences such as `SCALE_READING`, but they must agree with the authoritative contract.
5. Fail closed when:
   - game says playable but no verified capability exists,
   - contract says READY_VERIFIED but UI cannot expose Practice.
6. Preserve intentional legacy/pilot routes only where explicitly required.

### Exit criteria

For every published lesson:

- one authoritative server-side decision answers whether Practice is available,
- page, generator and game layer cannot disagree,
- title heuristics alone cannot authorize unrelated mathematics.

---

## C4 — Whole-catalogue runtime Practice closure gate

### Objective

Replace the distinction between “audit READY” and “actually usable from the student lesson”.

### Scope

All published learner-facing mathematics lessons in the canonical catalogue.

The baseline currently reports **4,453 lessons**, but the test must derive the count from source data rather than permanently hard-code it.

### Required per-lesson assertions

For every eligible published lesson:

1. lesson exists and is unique,
2. Practice capability resolves in C# runtime,
3. readiness is `READY_VERIFIED`,
4. SkillContract is valid,
5. all question families exist,
6. families permit lesson Practice routing,
7. verification policy exists,
8. deterministic generation produces items,
9. independent verifier accepts valid items,
10. broad fallback is not used,
11. student lesson availability exposes Practice,
12. any selected game route is compatible with the exact capability.

### Failure output

The gate must print:

- LessonCode,
- title,
- curriculum,
- grade,
- source type,
- exact failure reason.

### Exit criteria

- Runtime-available count equals published eligible lesson count.
- Zero catalogue-ready but UI-unavailable lessons.
- Zero UI-playable lessons without verified Practice capability.
- Gate is permanent in CI.

Only after C4 may the project state that all published mathematics lessons have student-accessible Practice.

---

## C5 — Student-facing lesson content forensic audit

### Objective

Find every lesson whose body is generic, off-target, teacher-facing, authoring-oriented, duplicated from an adjacent skill, or unsuitable for direct student learning.

### Compare for every published lesson

- lesson title,
- official Outcome evidence when outcome-backed,
- reviewed pedagogical/source target when Supporting,
- Explanation,
- Key Concepts and Rules,
- Worked Examples,
- Step-by-Step Solutions,
- Common Mistakes,
- Quick Summary,
- concept visual/diagram specification where applicable.

### Defect classes

1. **OFF_TARGET** — teaches a different skill.
2. **GENERIC_TEMPLATE** — generic unit template substitutes for exact lesson content.
3. **TEACHER_AUTHORING_LANGUAGE** — instructions to teacher/content author rather than learner.
4. **NO_CONCRETE_EXAMPLE** — no worked example for the exact target.
5. **EXAMPLE_TARGET_MISMATCH** — example belongs to an adjacent skill.
6. **BUILD_APPLY_DUPLICATION** — Build and Apply do not perform distinct pedagogical jobs.
7. **VISUAL_MISMATCH** — visual/model does not support the target.
8. **AGE_REGISTER_MISMATCH** — wording is unsuitable for the learner level.
9. **SOURCE_ALIGNMENT_RISK** — content broadens or changes the reviewed source target.
10. **EMPTY_OR_TRUNCATED** — missing or unfinished learner body.

### Student-facing quality standard

A learner should be able to answer:

- What am I learning?
- What does the idea mean?
- How do I do it?
- Can I see a correct worked example?
- What mistake should I avoid?
- What should I remember?

without teacher-only interpretation.

### Exit criteria

- Full catalogue audit report produced.
- Every failed lesson has a defect code and remediation target.
- No broad “looks reasonable” pass.

---

## C6 — Reconstruct failed learner lesson bodies

### Objective

Repair every lesson identified by C5 using the correct source authority.

### Outcome-backed lessons

Use:

- exact official OutcomeCode,
- pinned official evidence,
- reviewed mapping,
- curriculum/grade context.

Do not broaden beyond the official target.

### Supporting lessons

Use:

- preserved source teaching sequence,
- reviewed supporting target,
- source locator/reference,
- no invented official mapping.

### Required learner structure

1. **Explanation**
   - direct learner-facing explanation of the exact idea,
   - concise vocabulary,
   - concrete quantities/representations.

2. **Key Concepts and Rules**
   - only rules needed for this target,
   - no generic authoring instructions.

3. **Worked Examples**
   - at least one fully worked example specific to the target.

4. **Step-by-Step Solutions**
   - executable learner steps aligned to the example and Practice mechanic.

5. **Common Mistakes**
   - mistakes specific to this concept.

6. **Quick Summary**
   - short recall aid, not generic QA prose.

7. **Visual**
   - when the concept depends on representation, the visual must model the same mathematics.

### Build vs Apply

- `Build the Idea`: introduce and construct understanding.
- `Reason and Apply`: use the idea in varied/contextual reasoning.

They must not be duplicate templates with only a suffix changed.

### Known first correction

`PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:BUILD`

must be rebuilt around equal scale intervals, not place-value decomposition.

The existing `ApplyScaleReadingCorrection` can inform the mathematics, but Build content must be pedagogically appropriate for concept introduction rather than copied blindly.

### Exit criteria

- Every C5 failure remediated.
- Semantic-content audit green.
- Learner-facing acceptance tests green.
- Seeder upgrades existing published content idempotently under a new content version.
- No unrelated curriculum/outcome mapping changes.

---

## C7 — Login copy cleanup across supported locales

### Objective

Remove the requested Platform Administrator explanatory copy without changing authentication behavior.

### Remove

English:

- `The platform administrator can sign in directly.`
- `The platform administrator can continue without a selection.`
- `The platform administrator does not need to select a public account type.`

Also remove corresponding localized text from every supported login locale.

At the current baseline the login view contains English and Polish inline variants. Execution must also search views, scripts and resources so equivalent copy cannot survive elsewhere.

### Preserve

- school-role account selection,
- authentication logic,
- Platform Administrator authentication behavior,
- role validation,
- accessibility,
- Change Language control.

### Tests

1. English login HTML contains none of the removed phrases.
2. Polish login HTML contains none of their translations.
3. No removed phrase remains in visible copy, data attributes, JavaScript fallback copy or resource files.
4. Role selection behavior remains unchanged.

### Exit criteria

- Copy removed in all supported login languages.
- Authentication/role tests green.

---

## C8 — Full regression, CI protection, deployment and live verification

### Objective

Close the corrective programme only after end-to-end production evidence.

### Required CI

1. Mathematics Intelligence suite.
2. New whole-catalogue C# Practice runtime closure gate.
3. Lesson-content semantic/student-facing quality gate.
4. Existing solver/verifier tests.
5. Full regression with coverage.
6. PostgreSQL/migrations.
7. architecture/security/tenant/IDOR checks.
8. SAST/CodeQL.
9. container build and vulnerability scan.

### Production deployment sequence

`fix → tests → PR checks → merge → main checks → Render deploy → live verification`

Do not trigger a duplicate deploy if AutoDeploy already picked up the tested merge commit.

### Mandatory live checks

1. **Previously missing Practice**
   - `PED:CAMBRIDGE-INTL-MATH:S6:6F-2:BUILD`
   - Practice navigation visible.
   - Practice launch works.
   - questions match common-denominator fraction work.

2. **Previously working control**
   - `PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:BUILD`
   - Practice remains available.
   - game/practice remains mathematically aligned.

3. **Corrected lesson body**
   - `6NPV-4:BUILD` displays learner-facing Reading Scales content, not generic place-value content.

4. **Login**
   - the three Platform Administrator statements are absent in every supported login language.

5. **Logs**
   - no new error/critical startup or request failures attributable to the deployment.

### Final closure statement allowed only when

- whole-catalogue runtime Practice gate is green,
- student-facing content audit has zero unresolved failures,
- login cleanup is verified,
- Production is live on the exact tested commit,
- live smoke tests pass.

---

# 5. Primary files expected to be involved

## Practice/runtime

- `src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs`
- `src/Edulytics.Core/Mathematics/Practice/SupportingLessonPracticeRuleProjection.cs`
- `src/Edulytics.Core/Mathematics/Practice/SupportingPracticeTargetRuleRegistry.cs`
- `src/Edulytics.Core/Mathematics/Curriculum/supporting-practice-target-rules.v1.json`
- `src/Edulytics.Web/Controllers/StudentPortalController.cs`
- `src/Edulytics.Web/Controllers/StudentPracticeController.cs`
- `src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml`
- `src/Edulytics.Web/GameRouting/GameLessonRouteResolver.cs`
- `src/Edulytics.Web/GameRouting/GameLessonRouter.cs`
- `src/Edulytics.Services/Practice/StudentPrivatePracticeService.cs`
- `src/Edulytics.Data/Repositories/StudentPrivatePracticeRepository.cs`

## Lesson content

- `src/Edulytics.Core/Curriculum/LessonContent/Packs/*.lesson-content-pack.json`
- `src/Edulytics.Data/Seeding/CambridgePrimaryStage6LessonContentCorrections.cs`
- `src/Edulytics.Data/Seeding/SupportingLessonPracticeContentCorrections.cs`
- other curriculum-specific correction/seed paths discovered by C5.

## Login

- `src/Edulytics.Web/Views/Account/Login.cshtml`
- `src/Edulytics.Web/wwwroot/js/login-role-selector-v29.js`
- `src/Edulytics.Web/Resources/AccountResource*.resx`
- additional supported-locale login resources discovered during execution.

## Tests/audits

- `tests/Edulytics.Tests/MathematicsIntelligence/*`
- `tests/Edulytics.Tests/Acceptance/*`
- `tools/math_intelligence/*`
- `.github/workflows/math-intelligence-foundation.yml`
- Phase16 CI configuration where permanent gates belong.

---

# 6. Required execution order

`C1 → C2 → C3 → C4 → C5 → C6 → C7 → C8`

C5 may collect evidence while C1–C4 are implemented, but C8 cannot begin until all previous stages meet their exit criteria.

---

# 7. Completion scoreboard

| Stage | Description | Status |
|---|---|---|
| C1 | Prove exact Practice runtime failure | NOT STARTED |
| C2 | Repair runtime availability defect | NOT STARTED |
| C3 | Establish one Practice availability authority | NOT STARTED |
| C4 | Whole-catalogue runtime Practice gate | NOT STARTED |
| C5 | Student-facing content forensic audit | NOT STARTED |
| C6 | Reconstruct failed lesson bodies | NOT STARTED |
| C7 | Login Platform Administrator copy cleanup | NOT STARTED |
| C8 | Full CI, deploy and live verification | NOT STARTED |

---

# 8. Current execution checkpoint

The next action is **C1**.

Do not modify Practice routing first. Reproduce and prove the exact failure for:

`PED:CAMBRIDGE-INTL-MATH:S6:6F-2:BUILD`

against the known working control:

`PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:BUILD`

Only after the exact failing runtime condition is proven should C2 begin.

For lesson content, the first known C5/C6 defect is already recorded:

`PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:BUILD`

whose live body is generic place-value material and must be replaced with learner-facing Reading Scales instruction.

This file remains the authoritative checkpoint until all eight stages are closed.
