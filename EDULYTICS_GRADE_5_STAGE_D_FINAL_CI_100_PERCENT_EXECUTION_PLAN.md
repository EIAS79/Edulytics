# EDULYTICS — GRADE 5 — STAGE D + FINAL CI
## خطة التحقق الفني والتعليمي، الاختبارات الحقيقية، إغلاق الدروس، والاعتماد المشروط بأدلة مطابقة 100% — وثيقة تشغيل مستقلة

**تاريخ إعداد الخطة:** 30 أكتوبر 2026  
**المستودع المعتمد:** `EIAS79/Edulytics`  
**نطاق المنهج:** `US-CCSS-MATH / Grade 5`  
**المصدر التعليمي المثبت مبدئيًا:** **Illustrative Mathematics — IM K–5 Math Grade 5 — First Edition (2021)**  
**النطاق المرجعي:** **148 درسًا، 8 وحدات**؛ يجب إعادة إحصائه من GitHub/مصدر المنهج المُقفل عند بدء التنفيذ.  
**نوع الوثيقة:** أمر تنفيذ مستقل **يستخدم فقط بعد إكمال A–B–C بدليل**. لا يشغّل إنشاء هذه الوثيقة أي اختبار، ولا يقرر صلاحية النشر.

> **المقصود بـ«100%» هنا: اجتياز كل شروط القبول الإلزامية لكل عنصر ودرس وعائلة أسئلة ومسار تشغيل داخل النطاق المحدد، بالأدلة وعلى النسخة المرشحة الصحيحة.** ليست نسبة افتراضية تحققت بالفعل، ولا ضمانًا رياضيًا بأن تطبيقًا سيخلو من جميع العيوب الممكنة. لا تستخدم «100% مطابق» إذا تعذر استعراض عنصر مصدر، أو بقي مانع مصدر/حقوق/اعتماد بشري، أو نجح CI جزئي، أو لم تُختبر النسخة المنشورة فعلًا في بيئة التحقق.

---

## 1. المهمة الأساسية — لا إعادة لـ A–B–C

عند إصدار أمر تشغيل هذه الخطة:

**ابدأ من الحالة الحقيقية على GitHub بعد تسليم A–B–C. تحقق من كل درس Grade 5 فعليًا، شغّل الاختبارات اللازمة، أصلح عيوب الكود، راجع أدلة المطابقة 1:1، سجّل حالات النجاح والقيود، وأنهِ Final CI على نسخة مرشحة ثابتة.**

- لا تعِد تأليف 148 درسًا أو كتابة خرائط المصدر إذا كانت مكتملة بأدلة صالحة.
- لا تمنح `D_VERIFIED` لمجرد وجود اختبارات مكتوبة أو خضراء على SHA مختلف.
- لا تستبدل التحقق الشامل بملف JSON يعلن نجاحًا، أو test يصادق على أن النص `APPROVED` موجود.
- لا تقفز عشوائيًا بين الدروس عند تسجيل الإغلاق؛ التقدم الإداري/الأكاديمي **1 → 2 → … → 148**. يُسمح بالتوازي التقني للاختبارات بشرط ألا يطمس حالة أي درس.
- إذا كشف D خطأً حقيقيًا في A/B/C، أصلح **السبب المحدد**، وحدّث الأدلة والاختبارات المتأثرة وحالاتها، دون إعادة كتابة نطاق صحيح بلا داعٍ.
- **ممنوع** الدمج التلقائي أو النشر على Render أو تعديلات إنتاجية أو تفعيل `GO` أو منح اعتماد أكاديمي ذاتي نيابة عن شخص مخوّل.

---

## 2. خط أساس Grade 5 الفعلي

المستودع `EIAS79/Edulytics`، فرع `curriculum/us-ccss-g5-full-grade-20261028` وDraft PR #596. المصدر IM K–5 Math Grade 5 First Edition (2021). العدد المبدئي من أحدث Blueprint للصف: **148 درسًا في 8 وحدات**؛ أعد التحقق عند دخول Stage D.

- Blueprint: `src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-grade5-im-k5-2021.lesson-blueprint.json`
- Existing content: `src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g5-phase29-v1.lesson-content-pack.json` وملفات Grade 5 Rich V2.
- Evidence/handoff: `artifacts/curriculum-delivery/US-CCSS-MATH/G5/`
- Source books (read-only): `C:\Users\khali\Downloads\New folder (2)\Common Core\05_Grade_5_IM_K5` على Our-CS. **لا تشغيل اختبارات/أوامر على الجهاز**؛ اعتمد GitHub Actions/بيئة اختبار معزولة.

| الوحدة | العنوان في Grade 5 Blueprint | الدروس | الترتيب |
|---|---|---:|---|
| U01 | Finding Volume | 12 | 1–12 |
| U02 | Fractions as Quotients and Fraction Multiplication | 17 | 13–29 |
| U03 | Multiplying and Dividing Fractions | 20 | 30–49 |
| U04 | Wrapping Up Multiplication and Division with Multi-Digit Numbers | 21 | 50–70 |
| U05 | Place Value Patterns and Decimal Operations | 26 | 71–96 |
| U06 | More Decimal and Fraction Operations | 21 | 97–117 |
| U07 | Shapes on the Coordinate Plane | 13 | 118–130 |
| U08 | Putting It All Together | 18 | 131–148 |
| **الإجمالي** | | **148** | **1–148** |

**موانع A–C قبل D:** U01:L01 يحتاج عقود بناء المكعبات المتصلة/teacher assessment بدل scalar-prism-only Practice. ثمة اختلاف 210/215 formal mappings و26/27 standards بين حقول Blueprint. ملفات الطالب PDF المنفصلة ورسومات المصدر والـCool-down المحمي وحقوق الوسائط لا تعتبر مفحوصة دون دليل. وجود 148 sidecars ليس handoff مغلقًا؛ سجل Grade5 الحالي **0/148 A+B+C**. لا تبدأ Stage D على أساس كتابة هذه الخطة.

---

## 3. شرط دخول Stage D — A–B–C handoff gate

**لا تبدأ إعلان قبول D لأي درس قبل التحقق من ملف A–B–C**؛ يمكن بدء اختبارات البنية المشتركة بغرض اكتشاف الأخطاء، لكن لا يعني ذلك صلاحية الدرس للاعتماد.

لكل درس يجب وجود:

1. `LessonCode` ثابت، ترتيب رسمي، وحدة صحيحة، نسخة/رابط مصدر، وإثبات تأكيد الهوية وعدم وجود تعارض عناوين/معايير مؤثر.
2. خريطة source-element 1:1 **تضم كافة عناصر المعلم والطالب ومصادر الرسوم والـPDF** بما فيها Warm-up، كل Activity/جزء فرعي، Cool-down، Practice وموارد المراكز، والمهام الفردية والجماعية/اليدوية والأنشطة الشفوية المفتوحة.
3. حالات العناصر: `MATCHED_DIGITAL`، `MATCHED_TEACHER_LED`، أو مانع واضح. لا تعتبر `SOURCE_UNAVAILABLE`, `RIGHTS_RESTRICTED`, `SOURCE_CONFLICT`, `MISMATCH_BLOCKED` متحققًا.
4. شرح Edulytics Rich V2 المصحح، أمثلة/حلول صحيحة، representations، وصف المهارات وشروط Question Family الصحيحة، دقة grading، ونسخة محتوى قابلة للتعيين في runtime.
5. اختبارات Stage C **مكتوبة مع حالة `WRITTEN_NOT_RUN` أو أدلة لاحقة فعلية**؛ ملفات الاختبارات وحدها ليست قبولًا.
6. سجل `READY_FOR_STAGE_D` مع source snapshot/branch/commit والأدلة. إن وجد اختلاف ledger مع checkpoint أو pack أو GitHub، يُسوّى من الأدلة قبل الترقية.
7. موارد/تقييمات الوحدة المشتركة التي لا تنتمي إلى درس منفرد لها ownership واضح وevidence مستقل، حتى لا تسقط من المقام.

**قاعدة فشل مغلق:** درس لا يحقق handoff يظل `D_BLOCKED` أو `D_PENDING_A_C_EVIDENCE`. لا يجوز تحويل عنصر source محرر دون فحص إلى `MATCHED` أو حذف عنصر غير مغطى من denominator.

---

## 4. تعريف التغطية الكاملة وقياسها في D

ممنوع دمج الأبعاد المختلفة في «نسبة نجاح» مضللة. سجّل المؤشرات منفصلة:

| البعد | 100% المقبولة | لا يُقبل دليلًا |
|---|---|---|
| التعداد المصدرّي | جميع عناصر المصدر المُعرّفة، دون مقام مجهول، مع source snapshot ثابت | عدد الدروس أو أسماء الوحدات وحده |
| التكافؤ التعليمي | كل عنصر مغطى بمكافئ صحيح ومعنى/وحدات/تمثيل وشروط مطابقة، أو مانع يمنع الاعتماد | تطابق معيار CCSS أو title/hash فقط |
| التغطية الرقمية | كل مهارة قابلة للقياس الرقمي لها family/renderer/grader واختبارات محددة مجتازة | سؤال رياضي عام يحمل اسم الموضوع |
| صحة الرياضيات | كل عائلة/نوع إجابة داخل العقد ينجح في golden/property/regression والـverifier المستقل بحسب القدرة | validator يعيد نفس عملية المولد الخاطئة |
| تكامل التطبيق | الاختبارات الفعلية لكل المسارات المطلوبة (Student/Teacher/Assessments/DB/Auth) على environment مُثبت | وجود Service أو API بلا اختبار |
| واجهة العرض/الوصول | نتائج assertions وevidence لكل درس تتضمن الرسوم/الأبعاد/ARIA/keyboard بقدر ما ينطبق | نجاح Build أو E2E HTTP 200 فقط |
| CI المرشح النهائي | كل required check نجح على commit/merge ref الصحيح وحالة dependencies مثبتة | نجاح قديم أو run لنسخة أخرى |
| الحقوق والاعتماد البشري | كل ترخيص/attribution/عنصر يلزم معلمًا أو مراجعًا تمت تسويته بالشكل الصحيح | زر «Approve» آلي أو مرور اختبار عددي |

`source_parity_percent = matched_verified_elements / all_identified_source_elements * 100`

`digital_coverage_percent = verified_eligible_digital_targets / all_eligible_digital_targets * 100`

احفظ العددين والبسط/المقام والـscope والـsnapshot، لا النسب وحدها. **إذا كان denominator مجهولًا فالنتيجة `UNKNOWN` لا 100%.** لا تعد عناصر `TEACHER_REVIEW_REQUIRED` من الاختبارات الرقمية الناجحة؛ يمكن إثبات وجود مسار Teacher-led وظيفي منفصلًا ثم توثيق الاعتماد البشري على النشاط نفسه إن كان مطلوبًا. إذا كان أي عنصر بلا ترخيص واضح أو مصدر غير متاح، قد تنجح البوابة الفنية، لكن لا تُمنح `ACADEMIC_100_PERCENT_APPROVED`.

الاختبارات الحتمية/المُعيّنة بذرة يمكنها إثبات التغطية طبقًا لعقود ومجموعة حالات متفق عليها، وليس **جميع** توليفات الأرقام الممكنة بلا حد؛ سجل حدود الاختبار وعمق العينات ومخاطر المجال غير المختبر.

---

## 5. Stage D — طريقة بدء التنفيذ

1. اتصل بـGitHub وحدد **آخر فرع عمل Grade 5 فعلي مرتبط بالـPR/السلسلة**، وقارن `main` وعمليات النقل القديمة والجديدة؛ لا تُعد اعتماد Grade 3 اعتمادًا لـGrade 5.
2. اقرأ HEAD وcommit tree/metadata، `source-inventory`, `full-grade-progress`, جميع checkpoints، أحدث A–B–C handoff، وعناوين اختبارات Grade 5 وملفات workflow الموجودة.
3. صالِح اختلاف السجلات: حالات `IN_PROGRESS` القديمة أو `READY` المكتوبة يدويًا لا تُعد قبولًا دون evidence map؛ وتحقق من تعديل branch متزامن.
4. ثبت `CandidateSha` و`ContentVersion` و`Schema/MigrationVersion` و`SourceSnapshotId` و`Branch` ووقت الالتقاط، وحدد ما إذا كان CI يستخدم PR merge commit أو branch HEAD.
5. اكتشف إن كان أي درس اجتاز D بالفعل على نفس code/data/contract؛ لا تعِد الاختبار دون داعٍ، ولكن لا تنقل نجاحًا قديمًا بعد تغيير محرك/محتوى يمس النتائج.
6. افحص `.github/workflows/` واستخرج بوابات Grade 5 الفعلية، بما فيها full-grade/authenticated/academic workflows ذات الصلة؛ أسماء Workflows السابقة ليست ضمانًا لوجودها أو تغطيتها Grade 5.
7. أنشئ/استكمل `Grade 5 Stage D Execution Ledger`، مع حالة درس–درس ورابط كل Run إلى Commit ثابت.

### تدقيق الارتباط بمصدر 2021

لا تعِد A–C، لكن **أكد أن كل source-element في handoff له نتيجة مستقرة**. عند العثور على فجوة أصلية/حقوقية/semantic mismatch، افتح issue وأعد فقط العنصر المتأثر إلى A–C، وحدد نتيجة D `D_BLOCKED` لذلك الدرس، حتى تُصلح وتُراجع.

---

## 6. تشغيل الاختبارات على ثلاث دفعات

استخدم استراتيجية: **One Shared Build → Parallel Isolated Tests → Sequential Lesson Acceptance → Final Green CI**.

| الدفعة | مدى الدروس | العدد في baseline | ملاحظات |
|---|---|---:|---|
| `G5-D1` | 1–50 | 50 | يشمل U01/U02/U03 وبداية U04 |
| `G5-D2` | 51–100 | 50 | يشمل بقية U04 وU05 وبداية U06 |
| `G5-D3` | 101–148 | 48 | يشمل بقية U06 وU07/U08/U09 |
| **المجموع** | **1–148** | **148** | إعادة الحساب إذا تغير عدد الدروس المعتمد |

- **القبول الأكاديمي/الـledger تسلسلي**: 1→148.
- **الاختبار تقنيًا قد يتوازى** على shards آمنة مع بيانات/جلسات/قواعد معزولة.
- CI لا يُكرر Restore/Build 148 مرة. خط بناء مشترك وكاش آمن ومصفوفة jobs يحفظ كل درس/عائلة/نسخة/نتيجة منفصلة.
- لا تضع الدروس المحجوبة تحت اختبار خفي ناجح يتجاوزها. أظهر حالتها في التقرير.
- راقب `path filters` وrequired checks وقيود ساعات GitHub Actions حتى لا يُشغّل CI بشكل متكرر دون قيمة.
- أصلاح محرك مشترك يعيد اختبار مجموعة التأثير، لا درسًا عشوائيًا واحدًا.

إذا كانت بنية المستودع لا تسمح بتجزئة آمنة، فضّل استخدام workflow الحالي أو مجموعات أقل بالتوازي ولا تخفف الشروط. إذا تغير المصدر/عدد الدروس، أعِد احتساب تقسيم متوازن يغطي **كل** الدروس مرة واحدة.

---

## 7. D.1 — Restore, Build, unit, static/source contracts

نفّذ على GitHub Actions وبيئة اختبار موثقة:

- .NET Restore/Build، dependency locks، compilation of affected projects، source/content schema validation.
- تحقق من **وجود وعرض 148 LessonCodes وحيدًا وفريدًا** في pack/seed/runtime مع unit/order/title/outcome/sourceVersion، بلا إسقاط أو duplicate أو placeholders.
- Coverage linter يخفق عند Missing source element، coverage count mismatch، `denominator UNKNOWN`، duplicate source IDs، or missing source-linked artifact.
- Content shape/semantic tests: لا broken JSON references، لا null content fields واجبة، لا mismatched lesson code/grade/unit/sections، مع تحقق من أن content pack المستخدم في CI هو الذي سيصل staging.
- Math unit tests لـSkillContracts وquestion-family routing والقيود/الأرقام والكسور والأجزاء المتكافئة والقياسات والأشكال ومسائل الكلمات واستقلال verifier.
- اختبارات Correct/Wrong/Invalid/Multiple Valid/Boundary/Zero/Negative (إذا سمح المصدر)/rounding/unit / locale-format متى ينطبق الدرس.
- Tests منع تسرب الحل في السؤال/HTML/JS/HTTP، ونموذج إعادة إنشاء question seeds/parameters/anti-repetition fingerprints.
- اختبارات المولدات shared across lessons، backwards compatibility مع الدروس الأخرى وlegacy question version، وعدم انتشار family عامة ممنوعة.
- Security lint وvalidation ضد مدخلات غير صالحة ونطاقات كبيرة تهدد الموارد، دون تغيير عقود المشروع التاريخية.

**لا تستخدم نجاح Building بوصفه إثباتًا أكاديميًا أو بديلاً عن review لمهمة صورة أو رسم حر.**

### معيار ثبات الاختبارات

لكل suite: اسم test، scope، المعايير/skills المتصلة، commit SHA، run ID، status، الزمن، notes؛ سجل seed corpus وعينات property tests ونتائج regression، وحدود التشغيل. الـunit test الذي لا يفحص محتوى الدرس لا يسد فجوة محتوى.

---

## 8. D.2 — فحوص الرياضيات الخاصة بالصف الرابع

استخرج من source/SkillContracts المهارات الدقيقة، لا تعتمد على أسماء الوحدات فقط. اختبر على الأقل، **حيث تنطبق بحسب الدروس المصدرية الفعلية**:

| المجال | أمثلة فحوص لازمة، بعد ربطها بالعناصر المصدرية |
|---|---|
| العوامل والمضاعفات | factor/multiple، مضاعفات متتالية، divisibility، التحقق من تفسيرات النشاط ومجالات الأعداد |
| الكسور والتكافؤ والمقارنة | equivalence، same whole، number-line/area-model، تمثيلات متعددة، ترتيب ومقارنة الكسور وفق الدرس |
| عمليات الكسور | تركيب/تحليل الكسور وعملياتها المسموح بها، النماذج المرئية، الوحدات، الأجوبة والتمثيلات المكافئة |
| القيمة المكانية والأعداد الكبيرة | قراءة/كتابة/مقارنة/تقريب/تمثيل أعداد بحسب الحدود الرسمية، علاقات القيمة المكانية |
| المقارنة الضربية والقياس | معنى multiplicative comparison، الوحدات والتحويلات وشروط المسألة، غير قابل للتحويل إلى جمع متكرر تلقائيًا |
| الضرب والقسمة متعددة الأرقام | place-value strategies، الخوارزميات، البواقي، التقدير والمراجعة العكسية، exact answer checks |
| الزوايا | القراءة والمنقلة إن انطبقت، اتجاهات/درجات/مرجع الزاوية، عدم اعتماد صورة مُقاسة بأبعاد خاطئة |
| الأشكال ثنائية الأبعاد | الخصائص والعلاقات والتصنيف والرسم/التفسير، لا اختزال الاستدلال إلى سؤال عددي |
| وحدة المراجعة | التكامل بين المهارات وانتقال التمثيلات وتعدد الخطوات حسب المصدر، مع منع drift إلى مهارة خارج النطاق |

لكل family يجب وجود corpus فعلي لمشاكل صحيح حلها وخاطئ حلها وشروط الأطراف، وبيانات مصدر تدعم اختيار family ومجالها. على D التحقق من **الإجابة الرقمية والهدف المعرفي والتمثيل**؛ إذا نجح الحل الرياضي لكن السؤال يقيس Skill آخر، فالنتيجة `ALIGNMENT_FAIL`.

اختبر الفرق بين `MATCHED_TEACHER_LED` وبين `MATCHED_DIGITAL`: لا تمنح automated marking للرسومات والشرح الحر والعمل الجماعي بلا grader صالح ومراجعة بشرية عند اللزوم.

---

## 9. D.3 — Integration, database, Student/Teacher authenticated E2E

**بيئة اختبارات معزولة** ببيانات صف/مدرسة/طالب مصطنعة وHTTPS وصلاحيات محدودة، دون قاعدة بيانات الإنتاج أو بيانات طلبة حقيقيين.

1. **الكتالوج:** 148 درسًا صحيحًا؛ ظهور Grade 5 في الصف والوحدة والتسلسل والمدرسة الصحيحة وارتباط المعايير والمصادر.
2. **Rich V2:** فتح الدرس كطالب، التأكد من أن **المحتوى الناتج فعلًا** هو المحتوى الذي خضع للفحص، مع نصوصه ورسومه وجداوله ومقاساته وإرشاداته وأمثلة الحل؛ لا مجرد HTTP 200.
3. **Practice:** اختيار درس/Skill مستهدف، توليد عائلة مسموحة، إظهار prompt بدون expected answer/verification secrets، إرسال إجابات صحيحة وخاطئة وغير صالحة ومكافئة عند السماح، وتحقق من المعالجة.
4. **Assessment Builder:** اختيار outcomes/lessons، توليد، مراجعة المدرس، الاعتماد والتعيين للطلاب، تسليم وتخزين النتائج، ومنع تخطي approval workflow.
5. **Diagnosis/Remediation/Reassessment:** التوافق مع مهارات الدرس، احتساب mastery والضعف والتجديد بشكل صحيح، مع عدم الخلط بين private Practice وevidence الرسمي.
6. **Game / interactive practice:** geometry/number-line/fractions/angle interactions إذا كانت مدعومة؛ اختبر أن منطق الإجابة authoritative في النظام الفعلي، وأن JS غير قادر على تزوير قبول إجابة.
7. **Database integrity:** حفظ المحاولة، نوع السؤال، seed، الاختيار، النتيجة، المعلم/المدرسة/الطالب، version، الاسترجاع والتكرار الآمن؛ لا تسرب لسجلات مدرسة أخرى.
8. **Auth/RBAC:** Student، Teacher، Subject Supervisor، School Admin وحالات الرفض/الوصول بالحدود الفعلية، مع فصل نطاق المدارس والفصول والطلاب.
9. **Accessibility:** keyboard، focus، labels، alt text ذو معنى/بدائل مكافئة، قارئ الشاشة للنص الرياضي حيث تدعمه المنصة، تكبير/صغر الشاشة وresponsive layout، وعدم الاعتماد فقط على اللون لشرح الرياضيات.
10. **Visual integrity:** لقطات/فحوص رسمية أو snapshot tests لأشكال الزوايا، مسارات الكسور، number line، الرسوم البيانية، ومساحات الإجابة. تحقق من التطابق الرياضي بعد العرض.
11. **Internationalization:** رموز وأرقام وكسور وفواصل محلية ووحدات الرياضيات ومناطق الوقت المرتبطة بالتخزين عند انطباقها، من دون قبول كاذب لإجابة ملتبسة.
12. **Performance/Security:** قياس baseline سلامة زمن الرد وحدود المدخلات والمولد، وعدم وجود حقن عبر المحتوى/المدخلات/المصادر الخارجية.
13. **Compatibility:** دقّق تأثير تغييرات Grade 5 على المسارات المشتركة وأكواد الدروس القديمة وتقييمات Grades 1–3/5–6 بالاختبارات المناسبة.

فشل اختبار يثبت عيبًا، لكن نجاح اختبار واحد لا يُثبت أن كل عناصر lesson 1:1 ظهرت أو كانت مفهومة؛ اربط لكل درس runtime assertions إلى elements موجودة في handoff.

---

## 10. D.4 — المسارات البشرية وحقوق المواد

**ثلاث نتائج مختلفة يجب إبقاؤها منفصلة:**

1. **Technical runtime verified:** تحقق تقني رقمي على CandidateSha ثابت.
2. **Source/semantic evidence complete:** مطابقة 1:1 من المصدر الأصلي مع إثبات الإتاحة والترخيص، دون فجوات.
3. **Academic approval complete:** مراجعة/اعتماد بشري لكل ما تنص سياسة المشروع أنه يحتاج معلمًا أو مشرفًا، بما فيه الأنشطة المفتوحة/الرسم/التفسير/الأدوات.

إذا كانت مهام رسم زاوية أو تصنيف مفتوح أو شرح فكرته أو مركز عمل ثنائي تعتمد على حكم معلم، فسجّل `TEACHER_REVIEW_REQUIRED`. تَحقُّق أن interface يعرض إرشادات المعلم ليس هو نفسه تَحقُّق ناتج الطالب تحت الإشراف. ينبغي حفظ rubric/protocol/required evidence واسم/صلاحية reviewer عندما تتطلب سياسة الاعتماد هذه المراجعة، مع عدم اختلاق توقيع.

تحقق من مصادر الصور/PDFs والبيانات المرخّصة، ونسخة الترخيص والنسب والروابط؛ تسجيل `CC BY 4.0` للمنهج في الـBlueprint **لا يكفي لإباحة كل مواد طرف ثالث**. لا تُعلن `SOURCE_MATERIALS_100_PERCENT` إذا بقي أصل مصور/مطبوع أو حق إعادة الاستخدام غير محسوم.

يمكن تسجيل `D_TECHNICALLY_VERIFIED` مع `ACADEMIC_APPROVAL_PENDING`، لكن لا تساوِ ذلك بـ`APPROVED` أو `RELEASE_GO`.

---

## 11. قالب نتائج الدرس في Stage D

كل درس يحتاج مصدرًا ونتيجة مستقلة، مثال للحقول (هيكل توجيهي لا يُكتب `PASS` إلا إذا وجدت الأدلة):

```yaml
lessonCode: PED:US-CCSS-MATH:G5:U01:L01
lessonOrdinal: 1
unit: 1
sourceEdition: IM K-5 First Edition 2021
sourceSnapshotId: "<from accepted A-C handoff>"
sourceElementCount: "<actual count>"
verifiedEquivalentElementCount: "<actual count>"
sourceParity: "<computed percentage or UNKNOWN>"
rightsStatus: "<verified/blocked>"
aCStatus: "<READY_FOR_STAGE_D or explicit blocker>"
contentVersion: "<tested version>"
candidateSha: "<actual tested code SHA>"
ciRefKind: "<branch-head or pr-merge-ref>"
unitTests: "<pass/fail/not-run>"
integrationTests: "<pass/fail/not-run>"
authenticatedE2E: "<pass/fail/not-run>"
visualAccessibility: "<pass/fail/not-run/manual>"
practiceFamilyCoverage: "<numerator/denominator>"
teacherReviewStatus: "<pending/completed with evidence>"
openDefects: []
actionsRunIds: []
evidencePaths: []
dStatus: "<D_PENDING|D_RUNNING|D_BLOCKED|D_TESTS_PASSED|D_VERIFIED|APPROVAL_PENDING|APPROVED>"
updatedAtUtc: "<timestamp>"
```

**لا تخترع hashes أو Run IDs أو قبولًا تاريخيًا**. اجعل كل حالة قابلة للتفسير، وكل artifact له مرجع GitHub Actions/commit/document مع صلاحية النسخة.

### الحالات وإمتى تستخدم

- `D_PENDING`: لم يحن التحقق أو لم يبدأ.
- `D_RUNNING`: اختبارات/تحقيق جارٍ ولم يُجمع الدليل الكامل.
- `D_BLOCKED`: فشل اختبار أو نقص A–C أو مصدر/حقوق/اعتماد يمنع الخروج، مع كود السبب.
- `D_TESTS_PASSED`: suites المحددة ناجحة على SHA معلوم، لكن عناصر قبول أخرى لم تستكمل.
- `D_VERIFIED`: الأدلة التقنية المطلوبة لهذا الدرس ناجحة على المرشح الصحيح، مع فصل اعتماد البشر/الحقوق.
- `APPROVAL_PENDING`: نجحت البوابات الفنية، ويوجد اعتماد أكاديمي/حقوقي/إداري لم يحدث بعد.
- `APPROVED`: يُستخدم **فقط** عندما تثبت جميع الموافقات التي تتطلبها سياسة Edulytics بصلاحية وتوقيع/أثر مراجعة بشري صحيح؛ لا استنتاج تلقائي من CI.

إذا نجحت البوابة الفنية مع `D_BLOCKED` أكاديميًا، احفظ حقلي `technicalStatus` و`academicStatus` منفصلين كي لا تحجب نتيجة أحدهما حقيقة الآخر.

---

## 12. Ledger دائم وقاعدة استئناف آمنة

اقرأ/أنشئ على GitHub (إذا لم يوجد تنظيم قائم أسبق):

```text
artifacts/curriculum-delivery/US-CCSS-MATH/G5/
  full-grade-progress.json
  grade5-stage-d-execution-ledger.json
  grade5-stage-d-evidence-index.md
  lesson-{N}-v2-stage-a-d-checkpoint.json
  grade5-final-ci-summary.md
```

لكل درس: source snapshot، CandidateSha، أي Commits أحدث أثرت عليه، آخر اختبارات/Run IDs، build/Unit/Integration/E2E/visual status، source parity، QA reviewer، open defects، blocker reason، sample/seed، حالة الاعتماد، أول خطوة تالية. أدرِج `firstIncompleteLessonOrdinal` محسوبًا من حالات 1–148 **و`blockedLessons` منفصلًا**؛ لا تُظهر درسًا محجوبًا كأنه تم إغلاقه.

**عند استئناف محادثة جديدة:**

1. افتح آخر GitHub HEAD وقارن CandidateSha المحفوظ.
2. اقرأ الـhandoff وخريطة المصدر والـledger وCheckpoints.
3. اقرأ آخر GitHub Actions وأسماء الاختبارات والأدلة لكل درس.
4. حدِّد ما إذا كانت نتائج Run بقيت صالحة أو أصبحت stale بعد تغيير source/pack/family/renderer/grader/schema.
5. استأنف من أول درس لم تُجمع له أدلة صالحة، ولا تعِد كتابة المحتوى المكتمل.
6. احتفظ بالعودة إلى جميع `D_BLOCKED` قبل إعلان انتهاء الصف.

**ثبات النتائج:** `CI-PASS on SHA-A` لا يساوي `CI-PASS on SHA-B`. إذا تغير الاختبار أو الـworkflow أو الـsource pack أو أحد المحركات المشتركة، حدّد impact graph وأعد تشغيل suites المتأثرة. لا تجعل Commit إضافة تقرير جديد يعني أن تقريره قد اجتاز CI تلقائيًا؛ سجّل tested code SHA منفصلًا عن evidence-documentation SHA.

---

## 13. التعامل مع الأخطاء والإصلاح دون إعادة غير لازمة

عند الفشل:

1. حدد `CONTENT_SOURCE_MISMATCH`, `GENERATOR`, `ANSWER_GRADER`, `MATH_VERIFIER`, `RENDERER`, `AUTH`, `DB`, `TEST_BUG`, `CI_INFRA` أو `RIGHTS/ACADEMIC`.
2. احتفظ بـRun ID، testcase، seed، evidence/screenshot، Commit/ContentVersion وscope الدروس المتأثرة.
3. أصلح السبب الجذري فقط مع المحافظة على عقود الدروس القديمة؛ لا تُحذف checks لتصير خضراء.
4. اختبر الفشل الأصلي واختبارات regression والمسارات المشتركة.
5. إذا كشف source drift، حدّث عنصر A–B–C اللازم والخريطة وحالة handoff، ثم اختبره من جديد.
6. احفظ commit/PR والـledger، وتحقق من عدم وجود تغييرات متزامنة؛ لا force push.
7. لا توقف اختبارات دروس مستقلة بسبب عيب خاص بدرس آخر، لكن **لا تعلن كل الصف مكتملًا** بينما blocker مفتوح.

إذا فشل Test بسبب بنية غير متاحة، ضع `D_BLOCKED_INFRA` لا `PASS_WITH_WARNING`، مع إجراء الإصلاح وإعادة التحقق على البيئة المناسبة. إذا فشلت مراجعة حقوق/مصدر، لا تستبدلها بإعادة تشغيل CI؛ هذا ليس عيبًا تقنيًا يعالجه Build.

---

## 14. CI النهائية، Full Grade Gate، والاعتماد

راجع `.github/workflows/` وأسماء Required Checks الفعلية. من أمثلة المسارات التي **قد** توجد في المستودع، وليست ضمانًا للتغطية Grade 5 دون فحص:

- `us-ccss-full-grade-gate.yml`
- `us-ccss-authenticated-acceptance.yml`
- `v2-phase11-12-acceptance.yml`
- `us-ccss-academic-certification-evidence.yml`

### شروط Final Green CI

- [ ] أنجزت اختبارات Grade 5 لجميع 148 درسًا (أو آخر العدد الرسمي الموثق)، لا subset/B001 فقط.
- [ ] خريطة source elements لكل وحدة/درس سليمة ومكتملة، وليس النص `100%` مصطنعًا في تقرير.
- [ ] Build/Unit/Integration/Authenticated E2E/Visual-Accessibility/Regression اللازمة ناجحة على **CandidateSha/PR merge ref الصحيح**.
- [ ] نجاح جميع Required Checks ذات الصلة، مع فحص `skipped`, `neutral`, `cancelled` والـmatrix jobs الفائتة؛ لا يحسب job مخفي غير مشغّل نجاحًا.
- [ ] لا اختبارات ذات نتائج stale بعد آخر إصلاح؛ وثّقت نسخة المحتوى وmigration/data seed.
- [ ] تعارض العنوان/المعايير في الـBlueprint حُسم أو ظل blocker يمنع «full parity».
- [ ] لا توجد عائلات أسئلة خارج المنهج أو تقييمات متناقضة أو تسرّب إجابة أو اختلاط بين المدارس.
- [ ] لا ملفات PDF/رسوم أصلية ضرورية غير مستعرضة أو قيود حقوق مجهولة ثم إعلان source-materials parity.
- [ ] مسارات Teacher-led موثقة، وحدود الاعتماد الأكاديمي واضحة؛ لا «auto-approve» لأن machine CI خضراء.
- [ ] Release candidate محدد، وبيانات تقرير نهائي قابلة للتكرار وروابط runs/commits محفوظة.
- [ ] تم الحفاظ على branch protection وطلب قرار منفصل للدمج/النشر.

### فشل مغلق للبوابة

- يجب أن يفشل Full Grade Gate إن كان يُثبت أقل من نطاق Grade 5 النهائي؛ لا تُخفّض denominator إلى عدد lesson tests الموجودة.
- يجب أن يفشل عند statuses يدّعي صاحبها `APPROVED` دون مصدر/Reviewer/Candidate SHA صحيح.
- لا تجعل Negative Control ناجحًا (اكتشاف محتوى فاسد) دليلًا على أن محتوى الإنتاج مطابق؛ يجب أن يكون **اختبارًا منفصلًا** يثبت فقط سلامة رفض الحالة الفاسدة.
- لا تُحذف العبارات `blocked`, `manual`, `unsupported` من التقرير للوصول إلى `GO`.

### مستويات النتيجة النهائية

| الحالة | ما تعنيه |
|---|---|
| `NO_GO_TECHNICAL` | اختبارات إلزامية فاشلة/ناقصة أو evidence غير صالح أو SHA غير مطابق |
| `TECHNICALLY_GREEN_ACADEMIC_PENDING` | CI الأخضر مثبت لكن الاعتماد الأكاديمي/الحقوقي/البشري غير مكتمل؛ **ليس 100% اعتمادًا شاملًا** |
| `ACADEMICALLY_APPROVED_RELEASE_PENDING` | معايير المصدر والحقوق والاعتماد المطلوبة مثبتة، لكن الدمج/النشر ينتظر إذنًا مستقلًا |
| `READY_FOR_RELEASE_DECISION` | مرشح/بوابات سليمة، وتحتاج قرار المسؤول صراحة؛ لا نشر تلقائي |

`RELEASE_GO` ليس نتيجة تلقائية لـStage D، حتى في آخر حالة؛ تحكمه صلاحيات وإجراءات الإصدار الفعلية في المشروع.

---

## 15. التسليم النهائي والتواصل أثناء التنفيذ

**تقرير نهائي واحد** بعد إكمال نطاق العمل أو بلوغ مانع خارجي لا يمكن رفعه؛ يشمل:

1. آخر نطاق رسمي/عدد وحدات/دروس وتفسير أي اختلاف عن baseline 148/9.
2. العدد الحقيقي للدروس الجاهزة A–B–C، وعدد الدروس التي اختُبرت، والتي أصبحت `D_VERIFIED`، والتي حصلت على اعتماد بشري/أكاديمي **فعلي**.
3. كل درس غير مكتمل/محجوب ورقم الوحدة/الدرس وسبب واضح ومرجع الدليل/الـissue.
4. نتائج source-element coverage بالبسط والمقام على مستوى الدروس والوحدات والصف، digital coverage منفصلًا، وحالة source rights/manual review.
5. tested `CandidateSha` النهائي وbranch/PR merge-ref وأي docs-only commits لاحقة.
6. روابط GitHub Actions Run IDs الناجحة والفاشلة، ونتائج Build/Unit/Integration/E2E/Visual/Accessibility/Regression.
7. ملفات source artifacts والـCheckpoints والـledger والـPR والـCommits والتعديلات في المحركات المشتركة.
8. الحالة النهائية الفعلية للإصدار، هل Ready for release decision أم `NO_GO` ولماذا، وأول خطوة لازمة.
9. تصريح صريح **بعدم الدمج/النشر/تغيير الإنتاج** ما لم يصدر إذن مستقل وتُنفذ تلك الإجراءات بالفعل.

لا تقارير Audit بدل الإصلاح، ولا تقارير مطولة بعد كل درس، ولا أسئلة متكررة «أكمل؟». رسائل تقدم قصيرة فقط عند حدود مهمة أو اكتشاف مانع كبير أو تغير للـCandidateSha.

---

## 16. أمر التشغيل لاحقًا — انسخه كما هو عند تفعيل الخطة

> استأنف **Edulytics Grade 5 Stage D + Final CI** وفق هذه الوثيقة على `EIAS79/Edulytics`. اقرأ أحدث GitHub HEAD والفرع الصحيح وA–B–C handoff وجميع checkpoints والـledger، وثبّت مصدر **IM K–5 Grade 5, First Edition 2021** والنطاق الرسمي **148 درسًا في 8 وحدات مبدئيًا**. لا تُعد A–C إلا لإصلاح نقص/عيب موثق. شغّل الاختبارات الفعلية على GitHub Actions ببيئة معزولة، اجمع نتائج Build/Unit/Integration/Authenticated E2E والرسوم/الوصول، واستخدم 3 دفعات 1–50، 51–100، 101–148 مع التحقق التسلسلي لكل درس. أصلح الإخفاقات، أثبت النتائج على CandidateSha الصحيح، وحدّث السجلات وCommits ثم شغّل Full Grade Gate وFinal Green CI اللازمة. لا تعلن 100% إذا بقي عنصر مصدر/حقوق/مراجعة بشرية/CI/درس غير مكتمل. لا تدمج أو تنشر ولا تستخدم إنتاجًا. قدّم تقريرًا نهائيًا موثقًا واحدًا عندما تنتهي المهمة أو تُحدد العوائق الحقيقية.

**هذه خطة Stage D منفصلة لا تُشغّل قبل handoff أكاديمي A–B–C موثق للصف الخامس؛ ليست نتيجة اختبار أو اعتماد.**