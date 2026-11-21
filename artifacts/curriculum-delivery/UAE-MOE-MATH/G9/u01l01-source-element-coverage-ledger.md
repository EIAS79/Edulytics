# UAE Grade 9 General — U01L01 source-to-Edulytics element ledger (Stage A–B–C)

Source: uploaded original **01.pdf** (UAE General 2025–2026), printed pages **5–9**, PDF pages **16–20**. SHA-256: `341c104a9d05bca7f02462ff87ea7a6787143bb1ef2a06f19e690e08f6ec6ade`. No Advanced/Applied source used.

**Lesson:** `1-1 المتغيرات والتعابير`.

## Scope and evidence

| Printed page | Actual source material seen in uploaded book | Original learner preparation | Treatment |
|---|---|---|---|
| 5 | تعريف التعبير الجبري، المتغير، الحد، العوامل، ناتج الضرب، القوة، الأساس والأس؛ وصف رموز جبرية لفظيًا؛ مثال 1 وتمرين موجه | عربية أصلية في `Explanation`, `KeyConceptsAndRules`, `WorkedExamples`; coefficient/term recognition spec | Numeric component questions; open verbal explanations teacher assessed |
| 6 | جدول مفردات الجمع والطرح والضرب والقسمة؛ أمثلة تحويل الكلمات إلى تعابير؛ تكلفة مبيعات مع رسوم متكررة | العربية مع حفظ ترتيب الطرح والأقواس، ونمذجة رسوم ثابتة/متغيرة؛ Cool-down | Symbolic equivalence candidate + human review for explanations |
| 7 | التحقق من الفهم 1–10، التدريب وحل المسائل 11–34، أوصاف رمزية، تحويل لفظي، حجم أسطوانة، مواقف تكلفة/راتب | أمثلة جبرية وهندسية وسياقية + ستة test-case specifications | اختبارات الترجمة والتفسير والتمثيل؛ لا تحوّل كل سؤال مفتوح إلى إجابة عددية تلقائية |
| 8 | التدريب وحل المسائل 35–37، تفكير عليا/استنتاج 38–42؛ سياقات فرق رياضية/احتمال، قوى ذات أسس، تفسير فروق صيغ جبرية، مكعب مصور | مقارنة الأقواس والأسس، نماذج حجم مكعب، مواصفات reasoning وteacher review | مراجعة بشرية للتبريرات الحرة، تمثيل رقمي للتعابير المغلقة |
| 9 | تدريب اختبار معياري 43–46؛ مراجعة شاملة 47–55؛ مراجعة مهارات سابقة 56–61 | سؤال هندسي أصلي عن المحيط ورسم SVG أصلي؛ إعداد حالات الاختبار | أسئلة المراجعة السابقة تُربط بمهاراتها الصحيحة ولا تنسب تلقائيًا إلى هدف الدرس 1-1 |

## Prepared learning objectives and question specs

| Objective | Source proof | Current Edulytics artifact | Assessment intent |
|---|---|---|---|
| O1 مفهوم التعبير وحدوده ومعاملاته وقواه | p.5, p.7 examples | Arabic lesson pack + contract JSON | `ITEM_COEFFICIENT`; verbal interpretation teacher-reviewed |
| O2 ترجمة الكلمات إلى رموز مع ترتيب الطرح والأقواس | p.6–7 | Arabic lesson pack + contract JSON | `ITEM_VERBAL_SYMBOLIC`, `ITEM_READ_EXPRESSION` |
| O3 بناء نماذج واقعية وهندسية وتفسير المتغيرات | p.6–9 | lesson pack, `u01l01-rectangle-original.svg`, contract JSON | `ITEM_FIXED_FEE`, `ITEM_RECTANGLE`; cylinder/cube teacher context |
| O4 التفسير والاستدلال ومقارنة الصيغ | p.8 and p.7 reasoning questions | lesson pack and rubric specs | `ITEM_COMPARE_EXPRESSIONS` human-reviewed justification |

**Known limitations, recorded rather than concealed:** The current Stage C artifact defines question **specifications**, not proof that an existing production generator emits the correct Arabic questions or grades symbolic equivalents correctly. `LessonPracticeContractRegistry` must not receive `READY_VERIFIED` solely from an already working US Common Core family. The draft lesson-content JSON has not been established as active in the deployment loader; do not claim it is live. A per-exercise 1:1 check is not yet certified from this categorised matrix; exercises 1–61 are inventoried by section, with review material distinguished from direct targets. The school-year-wide source edition conflict remains unresolved.

## Explicit checkpoint

`A_CONTENT_ALIGNMENT=PREPARED`; `B_ORIGINAL_ARABIC_CONTENT=COMMITTED`; `B_DIAGRAM=COMMITTED`; `C_QUESTION_SPECIFICATIONS=COMMITTED`; `C_RUNTIME_BINDING=OPEN`; `REAL_GENERATED_QUESTIONS=NOT_RUN`; `SEMANTIC_GATE=NOT_RUN`; `LESSON_CLOSED=false`; `READY_FOR_STAGE_D=false`.

The only correct next unit of work is **U01L01**, specifically a runtime-supported family/grader mapping and verified source-level denominator in Stage A–B–C, without executing any tests until Stage D. Do not jump to U01L02 yet.
