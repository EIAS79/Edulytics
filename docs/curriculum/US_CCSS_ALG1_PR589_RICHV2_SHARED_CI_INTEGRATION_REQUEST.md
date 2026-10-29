# ALG1 / PR #589 — Shared Rich V2 CI integration request

**Owner requested:** American Supervisor 2 must assign one writer for the shared Rich V2 test contracts. This request is not authority for Coder 5 to alter any other course or cross-curriculum test.

## Verified failing baseline

- Baseline `main`: `39c757630d2bd67217a1468fce099653ed0f3f53`.
- PR #589 initial head: `343a25f856818b80eb09ea6359ebf7611f3ceac8` (three additional source-specific ALG1 Rich V2 sidecars).
- The initial PR CI has **five failed workflows**. The full logs show the same new course inclusion mismatch in two previously hardcoded catalogue tests:
  - `RichLessonContentV2PilotTests.PilotRegistry_PreservesOriginal26And143Grade3And149Grade4LessonsWithoutDuplicates`: expected `26+143+149=318`, actual **321** (Math Foundation job `123124776779`, Grade 4 job `123129540458`).
  - `RichLessonContentV2RolloutTests.R8_ClassifiesTheWholeCatalogue_AndCompilesEveryEligibleEnglishAndPolishLesson`: expected 318 curated, actual **321** (Math Foundation and full product regression jobs).
- Other affected jobs are downstream executions of the same full tests. We did not see mathematical assertion failures in the new Algebra 1-specific seven tests on the first candidate head.
- Grade 4 and original Cambridge/UAE/Polish course material must remain unchanged.

## Exact shared paths and required patch design

1. `tests/Edulytics.Tests/MathematicsIntelligence/RichLessonContentV2PilotTests.cs` method `PilotRegistry_PreservesOriginal26And143Grade3And149Grade4LessonsWithoutDuplicates` (near lines 323–335).
   - Current erroneous assumption: `AllDocuments.SelectMany(Lessons).Length == 26+143+149`, which treats historic Grade 3/4 and 26 pilot lessons as the entire eternal Rich V2 catalogue.
   - **Preferred patch:** retain the original 318 baseline cohort as a filter by its exact historical lesson codes or pack+lesson identities, assert it remains exactly 318 and unique, then separately assert **global uniqueness** across all registered Rich V2 `LessonCode`/`CultureCode` combinations. Do not simply replace 318 with a moving total or disable the test.
2. `tests/Edulytics.Tests/MathematicsIntelligence/RichLessonContentV2RolloutTests.cs` method `R8_ClassifiesTheWholeCatalogue_AndCompilesEveryEligibleEnglishAndPolishLesson` (around lines 116–126).
   - Current `readyCurated == 318` becomes false when new valid directly curated ALG1 sidecars are introduced.
   - **Preferred patch:** derive the expected curated count from the actual validated direct Rich V2 lesson registry crosswalk against the fixed canonical 5,110 lesson catalog; assert total partition conservation, no duplicate identities, and exact course-specific scopes. Preserve the expected 5,110 canonical lessons and the blocked-status invariants; never weaken academic acceptance criteria.
3. Course-local new verification is in `tests/Edulytics.Tests/MathematicsIntelligence/UsCcssAlgebra1Unit1RemainingRichV2Tests.cs`, asserting all authored ALG1 sidecars have unique course IDs; course-specific per-lesson source/math checks are run separately.

## Safe integration and revalidation

**Designated single writer required:** coordinate ownership before editing shared test files; inspect all open PRs, especially other active American Coder 4/6 changes, and check British/Cambridge, UAE and Polish regressions. Use a separate integration PR or a reviewed shared integration commit, under Supervisor 2's authority, without overwriting course-owned PR #589. On the exact candidate SHA, rerun both failing tests, the complete required Math Foundation/Evaluation/V2/Grade 4/Offline Inventory suites, and all cross-curriculum security/seed checks. No CI weakening, production data mutation, or release claim is permitted.

**Course state:** PR #589 remains draft. New ALG1 source-authoring files are not production proof; exact Practice, school/pathway adoption and authenticated tenant-persistence gates remain blocked independently by issue #453.
