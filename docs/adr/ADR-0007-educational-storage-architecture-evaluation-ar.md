# ADR-0007: اختيار التخزين الهجين المحسّن للإصدار التجريبي

**الحالة:** ACCEPTED FOR STAGING / PRODUCTION NOT APPROVED
**التاريخ:** 2026-10-09
**المستودع:** EIAS79/Edulytics، فرع `feat/neon-clean-bootstrap-json-content-20261008`
**الخطة المرجعية:** [خطة V2](../operations/EDULYTICS_V2_ARCHITECTURE_FIRST_12_STAGE_PLAN_AR.md)

## سؤال القرار
كيف نحقق أقل egress وعدد استعلامات إلى Neon من دون إضعاف FK والهوية الرسمية والاعتماديات وPractice والتقييمات والصلاحيات؟

## الخيارات
- **A — PG-centric:** الهوية والتفاصيل والأمثلة في PostgreSQL.
- **B — JSON-only reference:** الهويات الرسمية والوحدات والمعايير والمخرجات والروابط في JSON وفهرس الذاكرة؛ التشغيل الشخصي فقط في PostgreSQL، بعد إعادة تصميم الروابط إلى مفاتيح أكواد ثابتة. **غير منفذ**.
- **C — minimal FK + JSON:** المحتوى العام المعتمد محفوظ بنسخته الأصلية في JSON، والنصوص التعليمية تُقرأ من index مضمّن، بينما هوية الدرس وإصدار المنهج وما يلزم لفحوص FK والتبنّي المدرسي في PostgreSQL. **التصميم المختار لتنفيذ V2 وتجاربها المعزولة؛ غير معتمد للإنتاج بعد**.

## حقائق ثابتة في الكود
1. `SchoolCurriculumAdoptionConfiguration` يعرّف FK على `CurriculumFrameworkVersion`. إزالة النسخ المرجعية دون تغيير هذا الربط تعطل تبنّي المدارس للمنهج.
2. `CurriculumPedagogicalLessonOutcomeConfiguration` يربط `CurriculumPedagogicalLesson` بـ `CurriculumPackContentNode` عبر FK.
3. `CurriculumLessonContentConfiguration` يربط المحتوى بكل من إصدار المنهج وهوية الدرس التعليمية.
4. `LessonContentService` يثبت صلاحيات الدور والمدرسة/الطالب قبل إعطاء التفاصيل؛ لا يجوز اختصار هذا المسار إلى رابط JSON مكشوف.
5. المصدر التعليمي العام في ملفات المنهج ومحتوى الدرس ومخططات الدروس، لكن البيانات التشغيلية الجديدة في Neon.

## الدليل التجريبي الحالي
- اختبار إعادة بناء PostgreSQL 18: [CI](https://github.com/EIAS79/Edulytics/actions/runs/37853249607) **PASS**. حمّل 4 مناهج و5,110 هوية درس مع `users=0 schools=0 postgresProseRows=0`.
- المسار **C** بعد استعلام SQL مضغوط: `CLEAN_JSON_READ_QUERY_BUDGET_PASS sqlQueries=1 proseSqlQueries=0` على PostgreSQL 18، مع شرح يُسترجع من JSON.
- مسار **A** مقابل **C**: [مقارنة PostgreSQL 18 ناجحة](https://github.com/EIAS79/Edulytics/actions/runs/37853446113): `legacySqlQueries=2 jsonHybridSqlQueries=1` لنفس هوية الدرس في قاعدة اختبار مؤقتة. **هذه مقارنة لأوامر SQL، لا لتكافؤ متن الدرس** لأن جدول النصوص في الوضع الجديد فارغ.
- المسار **B**: لم تُثبت بعد مماثلة النتائج أو قابلية حذف FKs على النظام الحالي. لا يجوز وصف استهلاكه الحقيقي بأنه 0 للاستعلامات، لأن صلاحيات الطالب/التبنّي/السجلات تتطلب بيانات تشغيل.
- اختبار دفعة 25 درسًا على نفس PostgreSQL 18: `lessons=25 jsonHybridSqlQueries=1 proseSqlQueries=0`، [السجل](https://github.com/EIAS79/Edulytics/actions/runs/37853446113).
- **لم تُقَس** أحجام نقل الصفوف، p50/p95، استهلاك Neon في الإنتاج، ولا سلوك الذاكرة تحت الحمل.
- فحص Phase29 الصارم يقف عند `62/64`: `UAE-MOE-MATH:L05:Advanced` و`UAE-MOE-MATH:L06:Advanced` غير مكتملين.

## كيف نختار؟
1. نجاح التوافق المرجعي والمناهج وPractice والاختبارات والأمان شرط حاسم. أي خيار لا يحققه يُرفض.
2. إجراء اختبارات متطابقة على صفحات المحتوى العام وقائمة الدروس والدرس التفصيلي والطالب المدرسي وPractice والاختبارات.
3. تسجيل SQL round trips، حجم نتائج DB، الذاكرة، زمن p95 (بارد/دافئ)، ثم Neon egress/CU-hours على بيئة جديدة.
4. تقييم تعقيد هجرة الهوية والتحديثات وتغيير الإصدارات والرجوع للخلف.
5. اعتُمد **C** كمسار تنفيذ منخفض المخاطر على قاعدة نظيفة، مع مراجعة صلاحيته للإنتاج بعد اكتمال القياسات. أي انتقال لاحق إلى **B** يتطلب ADR وMigration وتقرير أداء منفصلين.

## سبب اختيار C الآن
- البيانات العامة وشروحات الدروس مصدرها GitHub JSON، ويستطيع التطبيق قراءة 25 درسًا بعملية SQL مرجعية واحدة دون جلب نصوص الشرح.
- الاحتفاظ بهويات المناهج والدروس ومخرجات التعلم الضرورية يسمح باستمرار قواعد FK والتبني المدرسي وPractice والاختبارات دون إعادة كتابة واسعة محفوفة بالمخاطر.
- الخيار A يبقي نصوص المحتوى في قاعدة البيانات دون ميزة مثبتة تبرر نقلها، والخيار B يحتاج فك روابط مرجعية مترابطة قبل ثبوت أي وفر إضافي يستحق ذلك.
- الاختيار **للتنفيذ والاختبارات فقط**: لا ادعاء بخفض egress بنسبة محددة أو جاهزية للنشر قبل قياس Neon الجديد واكتمال Phase29 64/64.

## توجيه التنفيذ حتى اكتمال بوابات النشر
- إبقاء خيار C **معطلًا افتراضيًا** حتى يُفعّل صراحة في قاعدة جديدة بعد التهيئة.
- تحسين استعلامات القراءة وقياسها دون إزالة FK أو كسر API.
- بقاء JSON المصدر المعتمد للشروحات؛ عدم نسخ بيانات مدرسة أو شخص من Neon القديم.
- عدم توجيه Render للإصدار الجديد قبل اجتياز بوابات المصدر والبيانات والتشغيل وقياسات الأداء.

**القرار التنفيذي هو أقل مرجع PostgreSQL لازم لسلامة العلاقات + JSON للمحتوى الثابت؛ ولا يُفسَّر كترخيص لحذف بيانات أو نشر منتج غير مُختبَر.**


## Read-only PostgreSQL staging measurements — 2026-10-09

These measurements add a **limited** runtime data point to Stage 2 of the latest cutover plan. Recorded against the verified isolated Neon PostgreSQL 18 branch `br-ancient-smoke-b5694k1f` of project `tiny-lab-44877119` using `EXPLAIN (ANALYZE, BUFFERS, FORMAT JSON)` on 25 Grade 5 General metadata rows. Each query was executed five times; no inserts, updates or schema changes were made.

| Query shape | Five server-side execution times, ms (ascending) | Median, ms |
|---|---|---:|
| Metadata-only (lesson code/title/unit) | 0.243; 0.247; 0.264; 0.340; 19.810 | **0.264** |
| Metadata + content-version left join | 1.112; 1.156; 1.219; 1.240; 12.094 | **1.219** |

This is *not* a valid A/B/C end-to-end comparison: metadata-only and join queries do not return identical fields; the direct database baseline excludes client network latency, application-side JSON lookup, authorization, cold cache, server load, and transferred bytes. The samples are too small for a reliable p95. Do not infer Neon egress savings or production readiness from these figures. The separate CI test establishing `legacySqlQueries=2` vs `jsonHybridSqlQueries=1` remains the comparable round-trip-count gate.

At this checkpoint, a read-only Neon SQL count returned `pedagogicalLessons=5110`, `lessonOutcomeLinks=5749`, `users=0`, `schools=0`, `postgresProseTranslations=0` on isolated staging. Project usage counter read-back: `data_transfer_bytes=8941338`, `compute_time_seconds=2360`; these are cumulative counters, **not** attributed to this microbenchmark. Full representative-workday Neon egress, production-authenticated E2E, and identical-payload A/B/C comparisons remain mandatory before cutover.


## تحديث القرار بعد النشر — مراحل 2 و3 و6 و11 و12 (9 أكتوبر 2026)

**حالة التنفيذ:** التصميم C مستخدم بالفعل في Render الإنتاجي المرتبط بفرع Neon `production`؛ أما **الاعتماد النهائي بأنه الأمثل من A/B/C فلم يكتمل**. النص السابق «للتجربة فقط» يصف قرار ما قبل النشر، ولا يمثل حالة تشغيل الموقع الحالية.

- **ما نعرفه بثقة:** CI أثبت 5,110 هوية درس + 5,749 ارتباطًا بالمخرجات، و5,110 سجل إصدار/نشر و0 شروحات PostgreSQL، ونجاح 2,201 اختبار تطبيق. الهويات والعلاقات المرجعية باقية لسلامة المفاتيح والعزل؛ قراءة الشرح من ملفات JSON.
- **مقارنة محدودة متاحة:** في الاختبار المعزول، قراءة الهوية القديمة تستخدم استعلامين مقابل واحد في التصميم C لدرس، وواحد لدفعة 25 درسًا. المقارنة لا تتضمن متنًا متماثلًا؛ لذا ليست دليلًا على نسبة خفض الاستهلاك.
- **فحص أداء إضافي:** أُضيفت 20 قراءة متكررة لدفعة 25 درسًا في أداة التهيئة الآمنة لقياس p50 وp95 للذاكرة الساخنة + PostgreSQL 18؛ المخرجات تعلن صراحة أنها ليست p95 باردًا أو بيانات egress الإنتاج.
- **المتبقي الحاسم:** تنفيذ B الفعلي غير موجود وليس مناسبًا افتراض صفر استعلامات له؛ المقارنة المتماثلة A/B/C غير مثبتة، ولا توجد بعد قياسات تحميل مصادق عليه تشمل مدرستين، صفحات التقييم وPractice وWorksheets أو egress من Neon مرتبط بنوافذ تشغيل محددة. لذلك يبقى قرار C *معتمدًا للتشغيل الحالي مع مراقبة* وليس معتمدًا كأفضل تصميم بالأرقام.
- **القياس الحالي من الموصلات:** Neon أعاد مقياس نقل تراكميًا للمشروع مقداره 8,941,338 بايت وcompute تراكمي 2,360 ثانية؛ لا يمثل الاستهلاك الفعلي للدروس على الإنتاج أو معدلًا لفترة زمنية بعينها. Render يعيد عينات CPU/ذاكرة ونقل، لكن سلسلة HTTP latency وعدد الطلبات كانت فارغة وقت الاستعلام. لا يجوز اشتقاق توفير مزعوم من هذه الأرقام.

**قرار تقليل جداول المرحلة 6:** لا نحذف أي سجل مرجعي له FK مستخدم بالمدارس أو الاختبارات أو Practice دون إعادة تصميم وعزل واختبارات. إزالة نصوص الشروحات من Neon حققت صفر صفوف Prose؛ أما تقليص هوية الدرس وروابط المخرجات فمؤجل إلى حين بيانات قياس مماثلة.
