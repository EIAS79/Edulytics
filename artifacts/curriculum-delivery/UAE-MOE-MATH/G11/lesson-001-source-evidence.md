# G11-001 — التعابير والصيغ — source review (partial)

Historical General identity: `PED:UAE-MOE-MATH:L11:GENERAL:06:01:EXPRESSIONS-AND-FORMULAS` at 5f249a9b3b27e8f2991ede798b56e7245d195a44; actual historical content has only en and generic example placeholder. PR #656 merged into 1c3db842b210ac33ad3a5e309f4779d0c9734c6d removed G11 active catalogues; no current student route. Retain the historical code as a migration candidate, not an already registered route. Do not revive the 123 retired foreign-language entries wholesale.

Read source: T2 physical PDF 18–23 (printed 217–222), G1 physical PDF 27–32 (printed 5–10). T2 newer relevant edition read first. Two common objectives: expression evaluation using order of operations and using formulas. No publisher prose or image reproduced. Original examples in lesson-001-content-draft.json.

| Physical source (T2/G1) | Observed category / skill | Draft coverage |
|---|---|---|
| 18/27 | terminology, precedence, substitution, signed bases | E01,E02, priority table |
| 19/28 | multi-operation decimal and rational substitution, cone volume, prism guided practice | E03,E04,E05; prism example pending |
| 20/29 | exercises 1–9, 11–16,18–23: evaluate multi-variable expressions | E02–E04, candidate reuse only for bounded subset |
| 20/29 | 10 percentage formula; 17 fuel economy; 24 temperature; 25 triangle area; 26 finance | E06–E09, finance separate example pending |
| 21/30 | 27 circumference, period; 28 pyramid volume; 29–34 rational/exponent evaluation; 35 cone formula; 36 ranking formula; 37 temperature from rate; 38 multi-input sports formula; 39 polynomial model comparison | E03–E05,E11; more formula variants pending |
| 22/31 | 40 Heron; 41 radical formula; 42 compare cylinders via drawing, table, words, algebra; 43 error analysis; 44 custom operator; 45 equivalent models; 46 fraction midpoint; 47 author expression; 48–49 mathematical writing | E10,E11,E12; additional source-specific analogs pending |
| 23/32 | 50–53 standardized formula/model questions and 54–67 review | pending numbered subpart review and separate prerequisite classification |

Status: source body observed, categorized coverage partial, atomic denominator UNKNOWN. No READY_FOR_STAGE_D assertion. No source-specific full exercise-equivalence claim. The original teacher Cool-down is not accessible; original substitute authored with rubric. No tests or CI intentionally run. No production modification.

## Continuation: native content and bounded exact engine

The four missing local supplements now have verified SHA-256 and physical page counts; all 13 distinct source files and three aliases are identified. This does not certify their body content or edition equivalence.

Added original E13–E30: prism, profit per share, circumference/period, pyramid, symbolic cone, ranking and temperature models, multi-input rate, radical formula, cylinder comparison, defined operation, equivalent quadratic models, midpoint of fractions, area scaling, table extrapolation, angle-bisector review, linear height and authoring a multistep expression. Native RichV2 file: `src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g11-unified-u01-l01-ar.rich-lesson-v2.json` (not embedded or routed). The first-party substitute Cool-down is retained. The final T2 page contains standardized questions 50–53 (area scaling, table extrapolation, angle bisectors, linear height) and review 54–67 (right-triangle classification, hypotenuse, scale, factoring, expansion, number modeling, roots); the review is explicitly prerequisite, not silently counted as new lesson objectives.

Three bounded substitution families extend the existing ExactSkillContractQuestionEngine; no new engine or active G11 route. Independent test specifications written, not executed. Source denominator and READY_FOR_STAGE_D remain open pending atomic mapping and real catalogue integration.
