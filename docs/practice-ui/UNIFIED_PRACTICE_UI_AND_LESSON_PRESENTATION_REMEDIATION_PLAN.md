# Unified Practice UI and Lesson Presentation Remediation Plan

**Created:** 2026-09-20  
**Status:** IN PROGRESS  
**Branch:** `fix/unified-practice-ui-remediation`

## Scope

This plan closes the learner-facing defects confirmed during authenticated Production smoke after the unified Practice remediation.

Confirmed defects:

1. Practice workspace width/scale changes visibly between renderers.
2. Lesson title and unit/topic subtitle have insufficient vertical separation.
3. Practice runtimes still render legacy Eddy assets instead of the currently approved transparent Edulytics character.
4. Practice shell details are inconsistent between server-verified, exact-skill and specialized renderers, including progress wording and internal labels.
5. Internal interaction/runtime names can leak into learner-facing helper copy.
6. Lesson presentation parsing can remove mathematical less-than expressions because plain-text `< ... >` ranges are treated as residual HTML.
7. The Practice remediation must not add a calculator; no calculator is part of this scope.

## Design authority

The currently approved learner-facing character asset is:

`/images/public/edulaytiks-character.png`

Legacy runtime assets such as `/images/game/v9/eddy-guide.webp` and `eddy-hint.webp` must not be rendered by the unified Practice experience.

## P1 — Runtime and presentation inventory

- Inventory active Practice launch paths and renderer families.
- Identify width, shell, progress and mascot divergence.
- Identify the exact lesson-presentation corruption mechanism.

**Exit:** every confirmed defect maps to a concrete source file or runtime path.

## P2 — Unified Practice shell

- Add one shared Practice UI override loaded after legacy workspace CSS.
- Use one responsive outer width and one minimum/maximum Practice stage geometry.
- Normalize page header spacing, title/subtitle hierarchy, HUD and progress presentation.
- Preserve renderer-specific mechanics.

**Exit:** server-verified and client-rendered Practice use the same responsive shell dimensions.

## P3 — Current character migration and learner-facing cleanup

- Replace active Practice references to legacy Eddy guide/hint assets with the approved transparent Edulytics character.
- Remove learner-facing raw interaction enum/runtime terminology.
- Normalize progress wording to a single visual contract.

**Exit:** active Practice paths contain no legacy Eddy image reference and no raw interaction enum is shown to learners.

## P4 — Renderer consistency

- Verify exact-skill, server-verified, lesson-grounded and specialized workspace renderers remain inside the shared shell.
- Keep specialized controls and mathematical visuals intact.
- Prevent renderer-local dimensions from shrinking or expanding the whole workspace.

**Exit:** renderer-specific mechanics remain functional without shell divergence.

## P5 — Mathematical text presentation repair

- Fix lesson plain-text sanitization so mathematical comparison operators such as `<` and `>` survive rendering.
- Preserve removal of actual HTML/script/style markup.
- Add regression coverage using the Stage 6 fraction comparison worked example and quick summary.

**Exit:** `3/5 < 5/8`, `3/8 > 1/2` and `<, > or =` survive parsing exactly as learner-facing text.

## P6 — Automated regression certification

Add/extend acceptance tests to certify:

- shared Practice UI override is loaded by both Practice entry views;
- approved character asset is used and legacy Eddy assets are absent from active Practice runtimes;
- consistent shell/progress contract markers exist;
- title/subtitle spacing contract exists;
- mathematical comparison symbols survive `LessonPresentationParser`;
- no calculator is introduced.

**Exit:** targeted tests and full repository CI are green.

## P7 — Merge, deploy and Production verification

- Open PR and wait for required CI.
- Merge only after required checks pass.
- Verify main CI.
- Verify Render auto-deploy reaches `live`.
- Verify public liveness/readiness.
- Verify the Production Practice pages retain lesson identity and no generic fallback is introduced.
- Record final commit/deploy evidence here and set **Status: COMPLETED** only after successful deployment verification.

## Completion evidence

Pending.
