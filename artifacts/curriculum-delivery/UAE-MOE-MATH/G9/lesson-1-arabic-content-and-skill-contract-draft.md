# UAE-MOE-MATH · Grade 9 General · U01L01 · المتغيرات والتعابير

**Delivery stage:** A–B–C candidate, learner-facing Arabic draft; **not published and not yet closed**.  
**Source:** Local General student book `Grade_09/01_Student_Books/01.pdf`, copyright 2025, printed pages 5–7; source SHA-256 `341c104a9d05bca7f02462ff87ea7a6787143bb1ef2a06f19e690e08f6ec6ade`.  
**Existing prior code identity (retired in PR #649):** `PED:UAE-MOE-MATH:L9:GENERAL:01:01:VARIABLES-AND-EXPRESSIONS`; validate adoption of this identity on re-registration, not a reference to still-existing live content.  
**Academic language:** ar; **direction:** RTL. Display mathematical expressions in isolated LTR math containers.

## Learning objectives derived from the inspected source

1. Identify variables, constants, numerical coefficients, terms and exponents in algebraic expressions.
2. Translate an algebraic expression into an accurate verbal description.
3. Represent a verbal quantitative relationship with an algebraic expression, preserving operation order and grouping.
4. Model a simple contextual total cost or geometric quantity symbolically and explain the meaning of variables.

These are source-aligned design objectives, **not** claims of independent, D-stage semantic verification.

## Lesson explanation (original Edulytics text)

**ما المتغير؟** نستخدم حرفًا مثل `x` أو `n` لتمثيل قيمة قد تختلف من موقف لآخر. العدد الثابت لا يعتمد على قيمة المتغير. في التعبير `4x + 7`، يمثّل `x` المتغير، ويمثّل العدد `4` معاملَه العددي، و`7` حدًا ثابتًا. يتكون التعبير من حدين، ولا يحتوي على علامة مساواة؛ لذلك لا نتعامل معه على أنه معادلة مطلوب حلها.

**قراءة الرموز:** التعبير `3a²` يعني ثلاثة أمثال مربع `a`، أي `3 × a × a`، وليس `(3a)²`. وعند كتابة `5(b + 2)`، فإن الأقواس تعني ضرب `5` في مجموع `b` و`2`.

**من الكلمات إلى الجبر:** عبارة «سبعة أكثر من ضعفي عدد» تكتب `2x + 7`. أما «ضعف مجموع عدد وسبعة» فتكتب `2(x + 7)`. الفرق بين الصيغتين جوهري، والأقواس تحفظ المقصود من العبارة.

**من الجبر إلى الكلمات:** نقرأ `(y - 6)/4` على أنه «ربع الفرق بين عدد وستة»، وليس «العدد ناقص ربع الستة». ونقرأ `9 - t` على أنه «تسعة ناقص عددًا»، مع الانتباه إلى أن الطرح ليس عملية تبديلية.

## Worked examples

**مثال 1 — تحديد مكونات التعبير.** في `6m³ - 5`، المتغير هو `m`، والمعامل العددي للحد المتغير هو `6`، والأس `3`، والحد الثابت هو `-5`. لا نخلط بين الأس والمعامل.

**مثال 2 — تحويل عبارة لفظية.** «أربعة أقل من ثلاثة أمثال عدد» تساوي `3x - 4`. نختبر المعنى باختيار `x = 5` فنجد `15 - 4 = 11`، وهي أربعة أقل من خمسة عشر. التعبير `4 - 3x` لا يعبّر عن العبارة نفسها.

**مثال 3 — نموذج تكلفة.** ثمن التذكرة الواحدة `p` درهم ورسم الحجز الثابت `12` درهمًا. تكلفة شراء `n` تذاكر في حجز واحد هي `np + 12`. يجب تعريف `n` عدد التذاكر و`p` ثمن التذكرة؛ ولا نضرب رسم الحجز الثابت في `n` ما دام يُدفع مرة واحدة.

**مثال 4 — التمثيل الهندسي.** مستطيل طوله `x + 3` وعرضه `w`: محيطه `2(x + 3) + 2w`، ومساحته `(x + 3)w`. الشكل التوضيحي المطلوب في Rich V2: مستطيل أصلي موسوم بالضلعين، لا صورة من الكتاب.

## Practice contracts to prepare in Stage C (do NOT run)

| Objective | Example learner prompt (Arabic) | Expected answer | Answer type | Candidate engine approach |
|---|---|---|---|---|
| Recognize components | ما معامل `7z²`؟ | `7` | integer | Reuse validated algebra component recognition if it exists |
| Verbal → symbolic | اكتب «خمسة أكثر من ضعف العدد `n`» تعبيرًا جبريًا | `2n + 5` | symbolic expression equivalence | Reuse → Extend only if symbolic normalization supports commutative equivalence |
| Grouping | اكتب «ثلاثة أمثال مجموع `x` و`4`» | `3(x + 4)` or `3x + 12` | symbolic expression equivalence | Accept equivalent expressions, preserve original prompt meaning |
| Context modeling | اشتريت `k` دفاتر، ثمن الواحد `8` دراهم، مع رسم توصيل ثابت `6` دراهم. ما التكلفة؟ | `8k + 6` | symbolic expression equivalence | No multiplication of the fixed fee |
| Read symbolic expression | صف `(a - 2)/5` بالكلمات | «خُمس الفرق بين `a` واثنين» | human-reviewed verbal description or constrained selection | No numeric-only grader |

**Forbidden misalignment:** generic arithmetic drills, linear-equation solving or unit-rate questions presented as assessment of verbal/symbolic expression translation.

**Stage D tests to write (not execute in A–B–C):** correct, incorrect, equivalent algebraic form, wrong subtraction order, missing parentheses, malformed input, Arabic/Western digits, fixed-seed generation, independent evaluator, learner-visible ar/RTL, no English fallback and no solution leakage. Reuse shared engines from any curriculum **only if** sampled learner prompts genuinely assess the target objectives.

## Cool-down — independent Edulytics formative activity

1. في التعبير `9t - 2`، ما المتغير وما المعامل وما الحد الثابت؟  
   *Expected:* `t`; `9`; `-2`.
2. اكتب تعبيرًا لعبارة «نصف مجموع العدد `q` وثمانية».  
   *Expected:* `(q + 8)/2`, or a mathematically equivalent expression.
3. يشتري طالب `s` من الأقلام بثمن `3` دراهم للقلم، ويُضاف رسم واحد مقداره `5` دراهم. اكتب التكلفة وفسّر الحد الثابت.  
   *Expected:* `3s + 5`; `5` is the once-only fixed charge.

Use immediate feedback for closed symbolic items and teacher rubric for open verbal explanations. This Cool-down is **Edulytics-authored**, not presented as a reproduction of an inaccessible teacher-only activity.

## Source-to-element evidence — Stage A/B authored, NOT D-certified

| Element | Printed pages | Edulytics evidence | Status |
|---|---|---|---|
| variable and algebraic expression terminology | 5 | explanation, Example 1 | DRAFT_COVERED |
| notation of factors, powers, terms | 5 | explanation, Example 1 | DRAFT_COVERED |
| verbal to symbolic translation | 6 | explanation, Example 2 | DRAFT_COVERED |
| contextual price model | 6 | Example 3, Cool-down 3 | DRAFT_COVERED |
| geometric expressions | 7 | Example 4 and intended original drawing | VISUAL_ARTIFACT_PENDING |
| practice, reasoning and checking | 7 | contract specifications and Cool-down | D_STAGE_GENERATION_PENDING |
| all remaining source page elements and exercises | 5–7 and lesson-end boundary not yet confirmed | itemized comparison required | SOURCE_ELEMENT_LEDGER_INCOMPLETE |

## Checkpoint

- `LessonOrdinal=1`; `LessonCode=PED:UAE-MOE-MATH:L9:GENERAL:01:01:VARIABLES-AND-EXPRESSIONS`
- `A=IN_PROGRESS`; `B=DRAFT`; `C=CONTRACT_SPEC_DRAFT`
- `TestsWritten=SPECIFICATION_ONLY`; `TestsRun=NOT_RUN`
- `A_C_Closed=false`; `READY_FOR_STAGE_D=false`
- Blockers: source element denominator incomplete; original visual not yet committed; this candidate is not wired to active Curriculum Registry / RichV2 / engine.
- **Next:** complete Lesson 1 source element ledger, create/wire Arabic learner asset and SkillContract, commit closure evidence before touching Lesson 2. Never claim A–B–C completion from this staging draft.
