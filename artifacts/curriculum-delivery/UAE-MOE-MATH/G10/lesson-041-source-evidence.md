# Grade10 General — canonical G10-U04-L08 source-to-app Stage A–B–C evidence

**Owner-approved Grade10 course:** 18 units / 148 unique canonical lessons, General only. **Current lesson:** «معدل تغير الدالة التربيعية» — a one-page mathematical exploration in the **Applied** Grade10 source, not a missing second theory unit.

**Primary source:** `رياضيات كتاب الطالب تطبيقي 10 8.pdf` (UAE Applied Arabic 2022–2023 Volume2), SHA256 `96df18c4fb47e45fc7a98f876d80e00e3cbdc080e5acfbdc9674a25c85f8cb9e`, physical PDF page **31** / printed **214**. It is a one-page algebra lab associated with Applied Unit4 lesson 4-1, and PDF32 begins the next 4-2 lesson. **Secondary corroboration:** `03.pdf` annual contents page9 identifies the same algebra lab on printed p214, but supplies *only* a matching contents line, not a second body or a duplicate lesson. Publisher photo/text not copied.

## A — 16 source teaching, exercise and assessment categories

| Element | Source printed page | Original mathematical objective | Edulytics location and assessment boundary |
|---|---:|---|---|
| `G10-APPLIED-U04L08-E01` | 214 | رسم علاقة ارتفاع صاروخ اختبار مع الزمن بدالة تربيعية −١٦ز²+١٤٤ز | RichV2 original rocket trajectory and table — `CONTENT_AUTHORED` |
| `G10-APPLIED-U04L08-E02` | 214 | تمييز القدم لارتفاع الصاروخ والقدم/ثانية لمعدل تغير الارتفاع | RichV2 explanation, WorkedExamples and teacher rubric — `TEACHER_REVIEW_REQUIRED` |
| `G10-APPLIED-U04L08-E03` | 214 | إنشاء جدول من ز=٠،٠٫٥،١،١٫٥ حتى ٩ وإيجاد قيمة الدالة عند كل زمن | RichV2 original rocket sample table and visual — `CONTENT_AUTHORED` |
| `G10-APPLIED-U04L08-E04` | 214 | تمثيل نقاط الصاروخ على محوري الزمن والارتفاع ورسم قطع مكافئ | RichV2 original CartesianPlot plus teacher plotting rubric — `TEACHER_REVIEW_REQUIRED` |
| `G10-APPLIED-U04L08-E05` | 214 | تحديد فترات التزايد والتناقص من منحنى الارتفاع | RichV2 vertex and sign of average-rate example — `CONTENT_AUTHORED` |
| `G10-APPLIED-U04L08-E06` | 214 | معدل التغير المتوسط هو فرق المخرجات مقسومًا على فرق المدخلات | RichV2 rate formula and original examples — `CONTENT_AUTHORED` |
| `G10-APPLIED-U04L08-E07` | 214 | حساب معدل التغير على فترات متتالية طولها نصف ثانية | new G10 U04L08 exact average_rate family, tests written — `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-APPLIED-U04L08-E08` | 214 | تمييز المعدلات الموجبة والسالبة والصفرية بعد حسابها | new G10 U04L08 exact rate_is_positive family, tests written — `AUTHORED_TEST_DIGITAL_SCORING_PENDING` |
| `G10-APPLIED-U04L08-E09` | 214 | تفسير لماذا يتباطأ ارتفاع الصاروخ قبل الرأس ثم يبدأ الانخفاض | original worked examples and teacher explanation rubric — `TEACHER_REVIEW_REQUIRED` |
| `G10-APPLIED-U04L08-E10` | 214 | إنشاء جدول للدالة ص=س² عند س من −٤ إلى ٤ | original x² table/CartesianPlot in RichV2 — `CONTENT_AUTHORED` |
| `G10-APPLIED-U04L08-E11` | 214 | تمثيل دالة ص=س² على شبكة وتحديد رأْسها | original CartesianPlot, manually assessed scaled graph — `TEACHER_REVIEW_REQUIRED` |
| `G10-APPLIED-U04L08-E12` | 214 | التمييز بين تناقص ص=س² عندما س<٠ وتزايدها عندما س>٠ | RichV2 x²-rate original demonstration — `CONTENT_AUTHORED` |
| `G10-APPLIED-U04L08-E13` | 214 | حساب ومقارنة متوسط معدل التغير للفترات الصحيحة حول الرأس | RichV2 calculations; exact G10 rate family supports bounded even coefficient; odd fractional proof teacher rubric — `TEACHER_REVIEW_REQUIRED` |
| `G10-APPLIED-U04L08-E14` | 214 | نموذج سقوط جسم من ارتفاع ١٠٠ قدم بالدالة −١٦ز²+١٠٠ | original freefall table and answer verification — `CONTENT_AUTHORED` |
| `G10-APPLIED-U04L08-E15` | 214 | التحقق من تناقص الارتفاع وزيادة مقدار السرعة السالبة بمرور الزمن | RichV2 original freefall rate comparison and teacher rubric — `TEACHER_REVIEW_REQUIRED` |
| `G10-APPLIED-U04L08-E16` | 214 | حساب لحظة ملامسة الأرض ز=٢٫٥ مع قيد الزمن والارتفاع غير السالب | RichV2 freefall context and rubric for physical domain — `TEACHER_REVIEW_REQUIRED` |

Each goal category is accounted for, either in first-party Arabic content, written but not run exact tests, or open graph/real-context teacher review. **No guarantee of 1:1 reproduction of each textbook numerically repeated exercise is claimed.** This lab's source denominator is 16 categorized items from one PDF page, not the total year or other canonical lessons.

## B — Original Arabic lesson and quantified representations

- Reuses canonical Grade10 `G10-U04-L08` identity and new `PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U04:L08:C041`; it is not registered as an independent Applied pathway.
- `src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u04-l08-ar.rich-lesson-v2.json` contains **14 original worked examples**, **6 visual representations**, **three Cartesian point-array diagrams** (rocket quadratic, symmetric `y=x²`, body fall), an original interval-rate comparison and independent algebraic difference-quotient proof.
- Original source skill: for a rocket height `h(t)=−16t²+144t` feet, `h(0)=0`, `h(0.5)=68`, `h(1)=128`, `h(4)=320`, `h(4.5)=324`, `h(5)=320`, `h(9)=0`. Calculated average rates: **136 ft/s** on [0,0.5], **120 ft/s** on [0.5,1], **8 ft/s** on [4,4.5], and **−8 ft/s** on [4.5,5]. Increase, peak, decrease and physical-time domain are explicit.
- For `y=x²`, the same book asks for values on −4..4 and comparison of signed average rates; the authored example distinguishes negative intervals before the vertex from positive intervals after it.
- The source free-fall function `h(t)=−16t²+100` feet reaches zero at **2.5 seconds**, within the physical domain, with signed rates −8 and −24 ft/s over the first two half seconds.
- Physical source sketches/measurements and open interpretations are documented in `artifacts/curriculum-delivery/UAE-MOE-MATH/G10/lesson-041-teacher-rubric.md`, rather than fictitiously auto-graded.

## C — Exact bounded rate contract and tests deferred to Stage D

- Primary skill `algebra.quadratic.rate_of_change`, two first-party families `uae.g10.general.u04.l08.average_rate` and `uae.g10.general.u04.l08.rate_is_positive`; both use exact integer response input and independent parameter-based verification, routed in the existing `ExactSkillContractQuestionEngine`.
- A half-second interval is represented by integer `start2/2` and `step2/2`, so the validator never relies on floating-point equality for correctness. Restricting `a` to bounded nonzero even integers ensures the average slope `a(u+v)+b` is integral; real coefficients and freehand graph proof stay teacher-scored.
- File `src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10QuadraticRates.cs` computes answers from `a,b,c,start2,step2` and normalizes Arabic-Indic digits, with strict parameter bounds.
- Exact contract `artifacts/curriculum-delivery/UAE-MOE-MATH/G10/lesson-041-stage-c-exact-contract.json`; families are `productionRouting=false`, `practiceReadiness=PARTIAL_REUSE_ONLY_NOT_READY_FOR_STAGE_D`.
- Tests written, **NOT RUN** in A–B–C: `tests/academic-uae-grade10-arabic-u04l08.test.cjs` and `tests/Edulytics.Tests/MathematicsIntelligence/UaeG10QuadraticRatesTests.cs`.

**Exit boundary:** READY_FOR_STAGE_D certifies authoring and source/skill/teacher evidence for lesson **41 only**. It is not Stage D pass, actual student UI verification, final full-course parity or production publication.
