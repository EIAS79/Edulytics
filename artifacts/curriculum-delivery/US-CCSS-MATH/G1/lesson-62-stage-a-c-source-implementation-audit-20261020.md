# G1 ordinal 62 — U03 L25 “How Do You Want to Subtract?” — original-source / first-party A–C review

**Authoring:** `READY_FOR_STAGE_D` based on source-goal aligned Edulytics assessment approved by the owner. **Not** final academic acceptance, CI, or publisher-protected 2021 Cool-down verification.

## A. 2021 original source, independently inspected

Authorized Our-CS **read-only** original local teacher HTML: `C:\Users\khali\Downloads\New folder (2)\Common Core\01_Grade_1_IM_K5\unit-3\lesson-25\lesson.html`. The registered historical source HTML SHA-256 is `b60f6a4e850c7ad881747a5405313de63a57ab235c11b5042deda174f76ae4d6` (registry hash reused, not recalculated here).

**2021 student/teacher requirements:**
- 10-minute choral count **forward by 1 from 50 through 100**.
- Activity 1 **Choose Your Own Subtraction Method** (20 minutes). Learners choose an efficient method for five exact expressions: `20−15=5`, `19−3=16`, `13−5=8`, `18−9=9`, and `17−15=2`. Each asks pupils to explain with drawings/numbers/words. Source favors taking away three for `19−3`, counting on for `20−15`; there is no mandated identical method for all.
- Activity 2 **Solve Story Problems** (15 minutes). Three different meanings of a missing value: (1) 12 pencils, teacher removes 7, `12−7=5` remain (result unknown); (2) Clare has 8 glue sticks and later 15, `8+□=15`, **□=7** (change unknown); (3) Kiran had 17 crayons and now has 9 after giving some away, `17−□=9`, **□=8** (subtrahend unknown). The original narrative requires **the correct unknown position**, and also asks pupil-drawn or spoken explanations.
- Original end-of-lesson synthesis prompts `14−9` and compares subtraction and related addition.
- The original five-minute assessment is titled **Cool-down: Subtraction Methods** but its protected 2021 exact student question is behind publisher authentication. This review does **not** verify or reproduce that protected task. The earlier illustration from a later Imagine Learning edition is also NOT treated as original Kendall Hunt 2021 evidence.

## B. Content and independent Edulytics exit check

Inspected existing `CommonCoreG1FullGradeStartingContentCorrections.cs` rich learner material; preserved the accurate 2021 mathematical activities, exact five expressions, all three stories and warm-up. Added explicit **Edulytics-authored formative exit check** `16−7=9`, confirmed by `9+7=16`; this is a **distinct** example from the source `14−9` lesson synthesis and not represented as the protected publisher Cool-down. Source-independent choice of method, drawings and mathematical explanation remain **teacher-reviewed**, never inferred from a correct answer alone.

Persisted content version changes **only U03 L25** from `us-ccss-g1-u03-l25-original-activities-v6` to `...-v7`, with a source-identity-guarded v6→v7 migration. The source-specific teaching unit assertions are authored in `UsCcssG1FullGradeStartingTeachingTests.cs`.

## C. Source-exact Practice generator, verifier and prepared tests

Repaired existing `UsCcssG1FullGradeStrategyAndTensQuestions.FlexibleSubtract`, family `usccss.g1.u03.l25.strategy.flexible_subtract`. The old family wrongly restricted **b≤9**, suppressing source `20−15` and `17−15`. Flexible subtractors now may be two digits where `1≤b<a≤20`; `CountOnOrAway` U03 L24 remains independently scoped.

Distinct exact guarded Practice modes:
- `mode=2`: preserved exact original pencil story `12−7=5`.
- `mode=4`: generates only the original five Activity 1 operand pairs, correctly measuring differences plus a related-addition check.
- `mode=5`: Clare's **unknown added** quantity, `UNKNOWN=7;EQUATION=8+7=15`.
- `mode=6`: Kiran's **unknown subtracted** quantity, `UNKNOWN=8;EQUATION=17-8=9`.
- `mode=7`: explicitly labelled **EDULYTICS ORIGINAL EXIT CHECK**, independent `16−7=9`, answer `DIFFERENCE=9;CHECK=9+7=16`.
- Other generic source-goal mode operands remain valid within 20. No purported automatic recognition of drawings, choice of efficient method, peer discussion or physical counter interactions.

Actual numeric verifier compares the exact question-family parameter contract, expected field keys/values and equation (including correct unknown positions). New `Lesson62Exact2021FlexibleSubtractionUnknownStoriesAndOwnedExitCheck` test covers all five exact source problems, both story unknowns, first-party check, invalid mode spoofing, wrong/malformed answers and seeded generator coverage. No tests have been executed.

## A–C decision and boundaries

Edulytics is **not a publisher copy**. The owner expressly approved independently designed formative assessments once the mathematical goal and available source teaching requirements are understood; thus the protected 2021 Cool-down is **not a source-access gate to authoring closure**, and its exact text is honestly marked **unverified**. Accessible original student/teacher activities are reviewed and represented by Edulytics own lesson and numerically gradeable Practice. This decision does not certify classroom explanations or exact publisher assessment parity.

**A–C authoring CLOSED / READY_FOR_STAGE_D.** The canonical academic ledger remains **IN_PROGRESS**. Original Stage D .NET tests and exact-head GitHub CI, child-session rendering, persisted grading, visual rights/accessibility, full-grade gate, owner UAT and release are deferred until whole-grade freeze. No CI, protected branch, merge or deployment touched. Next ordinal: **63 (U03 L26)**.
