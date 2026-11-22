# EDULYTICS — UAE MOE MATHEMATICS — GRADE 10 — STAGE D + FINAL CI
## خطة مستقلة للاختبار والإغلاق والنشر: منهج عربي واحد، إنهاء المسار المتقدم، وحماية كل المستخدمين والمحركات

**الإعداد:** 22 نوفمبر 2026  
**المستودع:** `EIAS79/Edulytics`  
**المنهج:** `UAE-MOE-MATH`، **الصف:** `Grade 10`، **الواجهة:** منهج واحد بلا قائمة مسارات، **توافق تخزيني مرحلي:** `General` فقط، **اللغة:** `ar/RTL`، **الهدف:** العام الدراسي 2026–2027.  
**المدخل:** Handoff حقيقي من `EDULYTICS_UAE_GRADE_10_STAGE_A_B_C_100_PERCENT_EXECUTION_PLAN.md`.  
**طبيعة الوثيقة:** خطة Gate وتشغيل، **لا تدعي CI ناجحًا أو حذف بيانات أو نشرًا قبل تحقق الأدلة**.

> **مبدأ المالك:** نُبقي كل الرياضيات الصحيحة والمحركات والطلاب والمدارس والمناهج الأخرى؛ نحذف فقط هوية Grade 10 Advanced وتبعياتها الحصرية بعد دمج المحتوى الدراسي الفريد. لا يُسمح بتكرار درس، سقوط مهارة أو حذف اشتراك/نتيجة مشتركة.

## 1. خط الأساس الفعلي قبل Stage D

- GitHub `main` قبل تنفيذ الصف العاشر: Manifest عام **7 وحدات/48 درسًا** وManifest متقدم **9 وحدات/52 درسًا**، كل من Content Packs مضبوط على `AcademicLanguage=en` مع `Status=Published` رغم طلب العربية.
- المصدر العربي المرفق للعام 2025–2026 يظهر **7 وحدات/48 درسًا**؛ فهرس المتقدم المرفق 2025–2026 يظهر **12 وحدة/86 عنوانًا أوليًا**. لا تُحوّل هذه الأرقام تلقائيًا إلى عدد نهائي للمنهج الموحد؛ يجب أن يحدد Source-Lock وCrosswalk التكرار والتغطية الفعلية.
- سبعة ملفات مرفقة: `01.pdf` (537 ص)، `01متقدم_.pdf` (353)، `رياضيات كتاب الطالب تطبيقي 10 8.pdf` (225)، `03.pdf` (264)، `02-1.pdf` (76)، `02-2.pdf` (171)، `03متقدم_.pdf` (267). بعض الملفات مقتطفات وطبعات 2018/2019 وأخرى 2025/2026؛ افصل الفهرس السنوي عن المحتوى الفعلي للجزء.
- Neon الإنتاجية قبيل التغيير: `General` **48** pedagogical lessons / **49** pack nodes؛ `Advanced` **52** lessons / **53** nodes؛ **2** advanced school adoptions، **2** class groups، **50** student-enrollment records، **2** advanced school topics، **0** Advanced Grade10 personal subscriptions. `SchoolSubscriptions` مدرسية، وليست محددة بمسار في مخطط الجدول؛ لا تحذف اشتراك مدرسة كاملة.
- التعديلات التحضيرية على فرع `feat/uae-g10-unified-arabic-source-rebuild`: إخراج Grade 10 Advanced من سجل المستوى الإماراتي، والإبقاء على Grade10 General، وتعديل اختبار `Phase29CurriculumExperienceRuntimeTests` لتثبيت **17** مستوى UAE بدل **18**. هذه التعديلات وحدها لا تعني نجاح CI ولا إلغاء بيانات Neon.

**مصادر الشفرة الأساسية:**
`src/Edulytics.Core/Curriculum/MathematicsCurriculumPackRegistry.cs`,
`src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json`,
`src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json`,
ومكافئاتهما المتقدمة المستهدفة بالتقاعد **بعد نقل المحتوى الفريد**.

## 2. بوابة الدخول `GRADE10_UNIFIED_STAGE_D_HANDOFF_GATE`

لا تُمنح `D_VERIFIED` لأي درس إلا إذا توافرت:
1. `FINAL_UNIT_COUNT`, `FINAL_LESSON_COUNT`, ترتيب رسمي-أساسي + إضافات مالك موثقة، بلا أعداد مفترضة، بعد تفكيك فهرس عام/متقدم/تطبيقي ووحدات/طبعات الكتب السبعة.
2. هوية `CanonicalLessonCode` ثابتة وفريدة، وCrosswalk `General/Advanced/Applied → canonical` لكل عنوان مصدر، مع إزالة التكرار الدلالي.
3. دليل لــ **كل** هدف وتعريف وشرح ومثال ورسم ونشاط وتقييم مؤهل من المصدر المختار: `SourceFileSha256`, `PdfPage`, `SourceElementId`, وartifact أصلي.
4. `ar/RTL` حقيقي في Pack وفي Student Portal وفي المعادلات/الرسوم، دون الترجمة العامة الإنجليزية.
5. `SkillContract` مستقل محدد بالهدف، `QuestionFamily/Renderer/Grader/IndependentVerifier` للمهارات الرقمية، ومعيار مراجعة بشرية للمفتوحة.
6. شهادات إغلاق `A_VERIFIED`, `B_VERIFIED`, `C_WRITTEN` وCheckpoint/Commit تسلسلي لكل درس.
7. لا `SOURCE_PART_MISSING`, `SOURCE_CONFLICT`, `DENOMINATOR_INCOMPLETE`, `RIGHTS_BLOCKED` مستترة. إن بقي أي منها، يسجل `NO_GO_SOURCE_100` ولا يُعد تنبيهًا شكليًا.
8. جرد علاقات Advanced عبر المدارس/الصفوف/التعيينات/الطلاب/الاشتراكات والفواتير/التقييم، وخريطة تحويل تحافظ على جميع الكيانات المشتركة.

**Stage D لا تملأ ثغرة A–B–C بإعادة تسمية Lesson أو ضغط عدد المقام أو إعلان اعتماد شكلي.**

## 3. مؤشرات القبول المستقلة

| المؤشر | قاعدة النجاح |
|---|---|
| `source_inventory_coverage` | توثيق جميع مصادر الدروس/العناصر اللازمة من السبعة والصفحات، بما في ذلك كل تعارض/جزء غير موجود |
| `semantic_pedagogical_coverage` | بدائل Edulytics أصلية مكافئة لجميع العناصر المؤهلة، بدون تكرار أو إسقاط |
| `evidence_completeness` | كل عنصر مصدر له PDF/SHA/page + artifact + reviewer/checkpoint |
| `digital_skill_coverage` | جميع المهارات التي تقبل تصحيحًا آليًا تتولد وتُصحح فعليًا |
| `mathematical_correctness` | اختبار صحيح/خاطئ/غير صالح/حدود/تأويل/وحدة/مكافئات لكل question family |
| `arabic_runtime_integrity` | محتوى عربي وRTL ومعادلات ورموز رياضية ورسوم صحيحة على الهاتف والحاسوب |
| `data_retirement_safety` | لا Advanced Grade10 selectable، لا صفوف/تبنيات معلقة، ولا فقد لأي طفل/مدرسة/نتيجة/دفع أو محرك |
| `cross_curriculum_regression` | لا تراجع Grade 1–12 UAE، US CCSS، Cambridge، Poland في ما يتقاطع |
| `final_ci` | required GitHub workflows كلها green **على SHA المرشح نفسه** |
| `academic_and_rights_approval` | موافقة المالك الأكاديمية منفصلة عن تحقق الحقوق وCI وقرار GO |

`source` غير المكتمل = `UNKNOWN`، وليس صفرًا أو 100%. `CI green` لا يساوي دقة الكتاب؛ `Published` القديم لا يثبت الدقة.

## 4. مرشح الإصدار وحماية الفروع

1. اقرأ آخر `main` وأحدث فرع/PR خاص بالصف العاشر. لا تعكس تغييرات صادرة من زملاء/فروع نشطة.
2. ثبّت `CandidateSha`, `SourceSnapshotId`, `SourceEditionSet`, `FinalLessonCount`, `SchemaVersion`, `MigrationId`, `TestedRefKind`, `DeploymentId` متى وجدت.
3. لا تُرجع `Grade10 Advanced` للواجهة عن طريق fallback من قاعدة البيانات بعد اختفائه من Registry؛ اختبر كل طرق API/Enrollment/School Admin.
4. لا تقم بتعديل حزم Grade 11/12 Advanced أو Grade8/9 المُلغاة أو محركات رياضية مشتركة لتبسيط تنظيف Grade10.
5. مفتاح `General` مرحلي لاستقرار روابط/بيانات Grade10 في التطبيق؛ واجهة الطالب لا تعرض اختيار مسار إطلاقًا. إذا استبدل داخليًا إلى `Common` لاحقًا، يلزم ترحيل محدد باختبارات، لا إعادة تسمية نصية.
6. لا يجوز اعتماد مصادر 2018 أو 2025 باعتبارها مثبتة الطبعة 2026–2027؛ يحتفظ Metadata بالسنة الحقيقية.

## 5. كفاءة الاختبارات

- `One Shared Restore/Build` لمرشح ثابت بعد تجميع إصلاحات جميع الدروس؛ لا بناء شامل لكل درس.
- تنفيذ الاختبارات المستقلة في CI shards، PostgreSQL معزولة للترحيل، حسابات طالب ومعلم ومدير مدرسة وهمية.
- تجارب الرياضيات يمكن أن تتوازى، لكن إغلاق Ledger وتسليم كل درس يبقى حسب `LessonOrdinal` فقط.
- الإصلاح للسبب الجذري دون حذف اختبارات فاشلة أو استبدالها بنتيجة وهمية.
- صفر Desktop Commander مبدئيًا؛ الملفات موجودة محليًا؛ لا TinyFish ولا بحث ويب للكتب.

## 6. D.1 — تدقيق البنية والهويات والحقوق

1. Build Release، validation لــ JSON schema والـPack loader والـembedded resources والـDI؛ تحقق أن Pack الموحد مصدره `ar` الحقيقي.
2. `UniqueLessonCode`, `UniqueUnitOrder`, `OneCanonicalSkillTarget`, `NoOrphanChild`, `SourcePageEvidence`, `NoPlaceholderExplanation`.
3. التأكد من عدم وجود `UAE L10 ADVANCED` ضمن selectable levels بعد إغلاق ترحيل البيانات، مع بقاء `L10 GENERAL` داخليًا ودون ظهور خيار في الواجهة.
4. لا ضياع لعقد `Outcome` المشتركة أو `QuestionFamilies`/engines عبر حذف علاقة Advanced.
5. `SourceSnapshotId` لا يشير إلى أجزاء غير متوافرة؛ `SourceEdition` يحدد 2018–2019/2025–2026 كما في المصدر.
6. الحقوق: لكل رسم/مثال أصل منشأ داخل Edulytics أو ترخيص صريح؛ مرجعية كتاب وزارة التعليم لا تمنح تلقائيًا حق نسخ صور/صفحات الناشر.
7. أي `MissingSourceElement` أو `UnsupportedAnswerType` أو خطأ رسمي في رقم صفحة/كتاب يجعل Stage D `BLOCKED`.

## 7. D.2 — اختبارات رياضيات الصف العاشر

على ضوء الفهرس النهائي، تغطي الاختبارات كل ما ثبت في الكتب:
- **المقادير الجبرية:** حدود وكثيرات حدود، عمليات وقوانين الأسس والجذور، التحليل، المعادلات والمتباينات التربيعية، حلول مكررة/مركبة متى وردت، مجالات وصحة تحويلات رمزية.
- **المستقيمات والتحويلات:** الميل/التوازي/التعامد، انعكاس/انسحاب/دوران/تمدد/تناظر، تحول مركب، التطابق والإثبات بالإحداثيات، أخطاء ترتيب الرؤوس.
- **المثلثات والأشكال:** SSS/SAS/ASA/AAS، علاقات ومراكز المثلث، شروط التباين، الرباعيات والمحيط/المساحة، براهين وشروط هندسية.
- **امتدادات المتقدم:** الأنظمة والمصفوفات، الدوال كثيرة الحدود والجذرية والعكسية/النسبية/الأسية/اللوغاريتمية، الدوائر، الاحتمالات والإحصاء، المثلثيات والمتطابقات، المتتابعات والمتسلسلات **وفق وقوعها في Crosswalk فقط**.
- **التطبيقي:** الاستخدامات العملية والتناسب والتشابه وما يثبت من الأهداف الإضافية في كتب التطبيقي، مع اعتماد صريح لما هو مكرر أو فريد.

**حالات إجبارية لكل سؤال رقمي قابل للفحص:** `Correct`, `Wrong`, `Invalid`, `Boundary`, `Equivalent`, `FixedSeedReplay`, `NoAnswerLeak`, `Arabic/WesternDigits`, `ScaleAndUnits`, `MathDomain`, `QuestionSkillAlignment`, `VisualMeaning`.

رفض الحلول التي تخرق المجال (مثل مقام صفر أو لوغاريتم غير موجب حقيقي)، وعدم إقرار تحويل غير مكافئ؛ السماح بإجابات مكافئة رياضيًا؛ رفض السؤال الذي يحمل عنوان درس لكنه يختبر عائلة مهارات أخرى.

## 8. D.3 — اختبار الطالب والمدرسة والاشتراكات E2E

بحسابات تجريبية مصادق عليها ومعزولة:
1. **Catalog:** اختيار «المنهج الإماراتي — الصف العاشر» مرة واحدة؛ **لا** General/Advanced switch؛ جميع الدروس العربية بالترتيب المصدر النهائي.
2. **Student Lesson:** كل درس يعرض عنوانه وشرحه وأمثلة مستقلة ورسوماته وتدريبه، واتجاه RTL صحيح، والمعادلة غير معكوسة.
3. **Practice:** على الأقل محاولة صحيحة وأخرى خاطئة محفوظتان لكل درس ذي مهارة رقمية، وتأكيد grader وskill mapping وDB result دون تسريب الحل.
4. **Open tasks:** teacher-review rubric لا تُعامل كتصحيح آلي.
5. **Teacher/Supervisor:** إنشاء تعيين وتقييم ومشاهدة نتائج المجموعات المنقولة دون ازدواج الطلاب.
6. **School Admin:** تبني منهج Grade10 واحد، والتقارير والصفوف لا تعرض مسارًا محذوفًا؛ كل مدرسة ما زالت تملك مناهجها الأخرى.
7. **Billing:** لا تتغير اشتراكات المدارس/الفواتير والمدفوعات بالصدفة؛ اختبارات حالات المسار القديم والتجديد.
8. **Mobile/RTL/Accessibility:** مقتطفات نص عربية، مواضع (x,y) والكسور، التركيز ولوحة المفاتيح، ألوان الرسوم وبدائلها.
9. **Security:** عزل المدارس والأدوار والطلاب، وعدم الوصول إلى بيانات Advanced القديمة من مسار عام غير مفوّض.
10. **Regression:** عينات قوية من جميع المناهج والمسارات الباقية، واختبارات متعمقة لأي Engine/Renderer مشترك جرى تمديده.

## 9. D.4 — إصلاح محدد وإعادة اختبار الأثر

صنّف الفشل إلى `SOURCE`, `CONTENT`, `ARABIC`, `DUPLICATE`, `QUESTION_FAMILY`, `GRADER`, `ENGINE`, `MIGRATION`, `AUTH`, `BILLING`, `RIGHTS`, `CI_INFRA`.  
لكل خلل وثّق `CommitSha/CaseId/LessonCode/SourcePage/Expected/Actual/RootCause/Fix/Retest`. لا تعتبر إصلاح CI بديلاً عن تصحيح الدرس. لا تنقل الدرس التالي إلى `D_VERIFIED` إذا كان الحالي بلا أدلة.

## 10. Ledger/checkpoint وحذف الإنتاج المنضبط

**بصمة إغلاق الدرس:**
```yaml
curriculum: UAE-MOE-MATH
grade: 10
displayPathways: none
internalCompatibilityPathway: General
language: ar
direction: rtl
canonicalLessonCode: "<verified>"
unitOrdinal: "<verified>"
lessonOrdinal: "<1..FINAL_LESSON_COUNT>"
sourceSnapshotId: "<verified>"
sourcePdfSha256: "<verified>"
sourcePdfPage: "<verified>"
crosswalkGeneralCodes: []
crosswalkAdvancedCodes: []
crosswalkAppliedReferences: []
sourceElementDenominator: "<measured>"
verifiedEquivalentNumerator: "<measured>"
skillContracts: []
stageA: "<verified>"
stageB: "<verified>"
stageC: "<written>"
stageD: "<pending/pass/blocked>"
candidateSha: "<fixed>"
commitSha: "<actual>"
```

**خطة حذف Advanced في Neon، بدون حذف المدرسة أو الطالب:**
1. `READ_ONLY_INVENTORY`: جرد دقيق للمفاتيح الأجنبية والنتائج والاشتراكات وفروق الهوية؛ baseline 52 درسًا و53 node و2 تبني و2 class و50 enrollment، لا يؤخذ كعدد نهائي بعد وقت الجرد.
2. `MIGRATION_PREVIEW`: أنشئ Crosswalk للطلاب/الأفواج/التقييمات والتبنيات إلى Grade10 الموحد؛ تأكد من أن المدرستين لديهما الوصول المناسب، وأن المجموعتين يمكن نقلهما أو إعادة تعيينهما مع الاحتفاظ برقم الصف والطلبة.
3. `DRY_RUN_POSTGRES`: اختبار على قاعدة معزولة؛ كل خطوة `WHERE framework/version/grade/pathway` دقيقة، transaction واحدة، و`assert row counts + FK`، و`idempotency`.
4. `PRESERVE_SHARED`: لا `DELETE Schools`, `StudentProfiles`, `Users`, `SchoolSubscriptions`, `Payment` أو `LearningOutcome` مشتركة؛ لا حذف نتائج منسوبة لغيره.
5. `TRANSFER`: إعادة ربط الفصول والطلاب والمحتوى والدرجات إلى الكيانات الموحدة إن أمكن دلاليًا وقانونيًا، أو تعليق الحذف في `DATA_MIGRATION_BLOCKED` حتى تُصحّح الإحالة بدلاً من مسح الطالب.
6. `PURGE_SCOPED`: حذف السجلات التعليمية وعقد/حزم Advanced Grade10 **الحصرية** فقط بعد إثبات نقل المحتوى الفريد، وإزالة العلاقات اليتيمة المرتبطة بالمسار دون لمس أي Grade 11/12.
7. `POSTCHECK`: 0 Advanced Grade10 adopters/active cohorts/selectable/canonical rows؛ 0 orphan enrollments؛ `General` canonical N lessons؛ عدد المدارس/الطلاب الفريدين وملخص الفواتير لا يتراجع؛ محركات shared hash unchanged.
8. في حال أي عدم تطابق، `ROLLBACK` داخل نفس transaction؛ لا تنفيذ SQL تدميري «للتجربة» على الإنتاج.

## 11. Final CI وبوابة اعتماد الإصدار

| Gate | شرط النجاح |
|---|---|
| `G10_SOURCE_ATOMIC_100` | لا فجوة ولا تكرار غير مفصول في عناصر المصادر الحاكمة |
| `G10_CANONICAL_INDEX` | `N` وحدات/دروس نهائية فريدة مع Crosswalk من كل الملفات |
| `G10_ARABIC_RTL` | كل `N` درس عربي بصريًا وDOM، لا fallback English |
| `G10_MATH` | جميع مهارات السؤال المؤهلة مولدة ومصححة مع verifier |
| `G10_STUDENT_E2E` | واجهة فعلية ومحاولات محفوظة وأدوار school/teacher/student |
| `G10_ADVANCED_REMOVAL` | Advanced Grade10 غير قابل للاختيار وغير موجود في بيانات المنتج بعد نقل المحتوى |
| `G10_NO_COLLATERAL_DAMAGE` | لا تغير في خدمات/دروس/محركات Grades 1–9/11–12 أو US/PL/Cambridge |
| `G10_BILLING_INTEGRITY` | 0 فقد/إلغاء غير مقصود لاشتراكات ودفع |
| `G10_GITHUB_CI` | required CI PASS على SHA نفسه، لا run queued/pending |
| `G10_RELEASE` | owner approval + rights review + CI/evidence + deployment observed |

**لا Merge ولا حذف إنتاج ولا نشر إذا Gate حاسم فاشل**، حتى لو اعتمد المالك أكاديميًا التأليف. الاعتماد الأكاديمي موثق كقرار مالك، لا يغيّر حقيقة فشل فني/حقوقي/مصدر ناقص.

## 12. التقارير والاستئناف والأدلة

- ابدأ بأول درس لم يغلقه Handoff، لا تعِد فحص درس مغلق ما لم تثبت ثغرة فيه.
- اجعل الدليل لكل درس `source → gap → correction → test → commit`، وتقرير بعد كل وحدة.
- تقارير عامة: `PublishedLessonCount`, `MissingLessonCount`, `DuplicatedSkillCount`, `AdvancedOnlyUnmigrated`, `ArabicCoverage`, `VerifiedAttempts`, `EngineRegression`, `SchoolEnrollmentIntegrity`.
- كل نسبة تسجل بسطًا ومقامًا وتاريخ SHA، مع الفصل بين `PREPARED`, `LOCAL_TEST_PASS`, `HOSTED_CI_PASS`, `DEPLOYED`.
- عدم وجود جهاز متصل لا يستلزم استخدام Desktop Commander ما دامت المرفقات كافية؛ لا أي جدولة آلية أو متابعة مستقبلية غير مأذون بها.

## 13. أمر Stage D بعد إغلاق A–B–C

1. تحقق من `GRADE10_UNIFIED_STAGE_D_HANDOFF_GATE` على **العدد النهائي من المصدر**.
2. بنِ مرشحًا ثابتًا واحدًا، وشغّل اختبارات math/Arabic/E2E/DB/CI الفعلية.
3. عالج الفشل الجذري في نفس الدرس ثم أعد فحص الأثر؛ لا تتخطَّ الترتيب.
4. شغّل اختبار ترحيل المتقدم في PostgreSQL معزولة، بما فيه نقل 50 قيدًا دراسيًا إن ظلت حاضرة ومرتبطة بنطاق المشروع؛ ثم راجع الحواجز وقاعدة بيانات الإنتاج قبل تطبيقه.
5. إذا اجتازت كل البوابات، دمج GitHub → مراقبة نشر Render/Neon → اختبار مصادق عليه من الإنتاج دون تعديل حسابات حقيقية → تقرير نهائي بالأرقام والأدلة. وإلا `NO_GO` مُعلّل، من دون ادعاء نشر.
