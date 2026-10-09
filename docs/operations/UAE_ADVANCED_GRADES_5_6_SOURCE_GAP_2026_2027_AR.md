# Edulytics — فجوة مصادر الإمارات للصفين 5 و6 Advanced (2026–2027)

**التاريخ:** 2026-10-09
**الحالة:** OPEN — المسار المدرسي موجود رسميًا؛ مصدر منهج الرياضيات المستقل/المشترك لم يُحسم

## قرار نطاق المصدر: 2025–2026 (قرار المستخدم 9 أكتوبر 2026)

**قرار قبول المحتوى:** يُسمح باستخدام **العام الدراسي 2025–2026** بوصفه **سنة المرجع المستهدفة** لمساري رياضيات الصفين الخامس والسادس Advanced في خطة V2. لا يجب وصف هذا المحتوى بأنه منهج 2026–2027، ولا يُدَّعى أنه مطابق للعام الأحدث من دون أدلة منفصلة. هذا تعديل لنطاق المنتج، **وليس إثباتًا آليًا لإغلاق البوابتين**.

### الملفات التي فُحصت من المستخدم

- `رياضيات كتاب الطالب 5 22.pdf`: غلاف وزارة التربية والتعليم، **Grade 5 General، 2025–2026**، 424 صفحة. ليس مصدرًا مثبتًا للمسار Advanced.
- `رياضيات-كتاب-الطالب-6-22.pdf` و`كتاب رياضيات ريفيل.pdf`: **Grade 6 General، 2025–2026**، نسختان متطابقتان (SHA-256 واحد). لا تصلحان لإثبات Advanced.
- `Reveal Math G5 Volume 2.pdf`: **Grade 5 Advanced، 2023–2024**، مجلد 2، 289 صفحة؛ مرجع مقارنة أقدم.
- `رياضيات كتاب الطالب ريفيل متقدم 6 29.pdf`: **Grade 6 Advanced، 2024–2025**، مجلد 1، 304 صفحات؛ مرجع مقارنة أقدم.

### معيار قبول 2025–2026 دون اختلاق محتوى

1. العثور على نسخ **Grade 5 Advanced وGrade 6 Advanced لعام 2025–2026** أو مصدر حكومي/ناشر موثوق يثبت اشتراك رياضيات Advanced مع General في هذا العام تحديدًا.
2. مطابقة كل الوحدات والمجلدات المطلوبة ومخرجات التعلم، وتوثيق إصدار المصدر وحدود الحقوق والمحتوى الأصلي المؤلف للمنصة.
3. فحص توافق أسماء السنوات عبر SourceCatalog والتقارير وعناوين الواجهة؛ لا تعيد تسمية مواد 2023–2025 بأنها 2025–2026.
4. إعادة تشغيل تدقيق التغطية الصارم؛ احتفظ بمخرج **62/64** وحالة **NO-GO** حتى نجاح المراجعة والمطابقة الفعلية. لا تعطل الفحص ولا تخفض عدد النطاقات.
5. عند النشر، يجب أن يُظهر المنتج بوضوح أن نسخة المحتوى المُعتمدة هي **2025–2026**، وأن إصدار 2026–2027 ليس ضمن ادعاء المطابقة.

---

## تصحيح مهم بعد مراجعة دليل الوزارة الرسمي 2026–2027

**هناك مسار تسجيل متقدم للصفين الخامس والسادس في الحلقة الثانية.** دليل وزارة التربية والتعليم الرسمي لهذا العام يعرض صراحة:
- الانتقال من الصف الرابع إلى **الخامس المتقدم**.
- الانتقال من الصف الخامس إلى **السادس المتقدم**.

**المصدر الأصلي:** [دليل التسجيل الرسمي للعام 2026–2027، الصفحات 16–17](https://www.moe.gov.ae/en/guides/Documents/Registration-2026-2027/Registration-guide-2026-2027-Ar.pdf).

**التمييز الحاسم:** إثبات وجود *مسار التحاق بالمدرسة* لا يثبت في حد ذاته وجود *منهج رياضيات متقدم مستقل مغاير لمنهج الرياضيات العام* للصف نفسه. **لا تُستبعد هاتان الحالتان من فحص التغطية بافتراض عدم وجود المسار**، ولا نختلق لهما منهجًا خاصًا.

**إجراء الإغلاق الصحيح:** إما (أ) الحصول على مصدر رسمي لمنهج رياضيات متقدم مستقل واستكماله، وإما (ب) إثبات رسمي أن طلاب المسار المتقدم في هذين الصفين يدرسون نفس محتوى الرياضيات العام، وتسجيل خريطة إعادة استخدام المحتوى المشترك صراحة بالنسخة والهوية والحقوق المناسبة. لا يصح إعلان نجاح Phase29 قبل تحقق أحد المسارين واختبارات الربط.

---
## ما تحقق من كود المشروع
- فحص Phase29 الصارم: `62/64` نطاقًا، والناقص فقط `UAE-MOE-MATH:L05:Advanced` و`UAE-MOE-MATH:L06:Advanced`.
- أداة `tools/phase29/complete_remaining_curricula.py` ترفض تلقائيًا أي نطاق بلا `SourceCatalog` نشط.
- أظهرت [GitHub Actions](https://github.com/EIAS79/Edulytics/actions/runs/37854840813) الخطأ `FAIL: UAE source catalog missing for G5/Advanced`.
- **لم تتم إضافة دروس مفترضة أو خفض فحص 64/64 أو وسم محتوى غير مُراجَع بأنه مُعتمد.**

## ما وجدناه في المصادر الخارجية
- [دليل وزارة التربية والتعليم للمسارات 2025–2026](https://www.moe.gov.ae/en/guides/Pages/Parents-%26-Students-Guide-to-the-Educational-Streams-in-Cycle-3-2025-2026.aspx) يتناول المسارات العامة والمتقدمة في الحلقة الثالثة (الصفوف 9–12)، **وليس دليلًا كافيًا لبيانات 2026–2027 للحلقة الثانية**.
- توجد [نسخة منشورة خارج الموقع الحكومي من الخطة الدراسية 2026–2027](https://ru.scribd.com/document/1069909861/Academic-Plan-2026-2027)، ويظهر في وصفها جدول «Grades 5–8 (General and Advanced Streams)» ورياضيات 7 حصص لكلا المسارين. **رابط النشر هذا لا يثبت أصالة النسخة ولا عناوين دروسها**.
- [وزارة التربية والتعليم أعلنت تغييرات في منهج 2026–2027](https://www.moe.gov.ae/en/guides/Pages/Student-Assessment-Policy-Guide-2026-2027.aspx) لكن لا يوجد حتى هذه المراجعة ربط موثّق في المستودع بين دليل الرياضيات الرسمي ومحتوى G5/G6 Advanced.

## مطابقة التقرير القديم مع الملفات الفعلية (9 أكتوبر 2026)

**المصادر هنا من مستودع GitHub نفسه فقط**، دون استنتاج أن منهج الرياضيات المتقدم مستقل أو مشترك:

| النطاق | ما يسرده تقرير `docs/PHASE_29_REMAINING_CURRICULA_ROLLOUT_AUDIT.json` | ما يوجد فعليًا على `main` |
|---|---|---|
| الصف الخامس، General | ملف Blueprint وملف Content | الاثنان موجودان |
| الصف الخامس، Advanced | 28 درسًا وملفان متوقعان | لا يوجد ملف `uae-g5-advanced-t1-ogl-v1.lesson-blueprint.json` ولا ملف `uae-g5-advanced-t1-ogl-v1.lesson-content-pack.json` |
| الصف السادس، General | ملف Blueprint وملف Content | الاثنان موجودان |
| الصف السادس، Advanced | 33 درسًا وملفان متوقعان | لا يوجد ملف `uae-g6-advanced-t1-ogl-v1.lesson-blueprint.json` ولا ملف `uae-g6-advanced-t1-ogl-v1.lesson-content-pack.json` |

**النتيجة المحصورة:** تقرير سبتمبر يتضمن مراجع ملفات **غير موجودة حاليًا** في المستودع، ولا يجوز معاملته كدليل اكتمال أو كبرهان على اختلاف محتوى رياضيات General وAdvanced. يبقى المساران بحالة `SOURCE_NOT_VERIFIED` حتى التحقق من مادة الرياضيات المقابلة لهما، ولا نولّد دروسًا تخمينية.


## Evidence from locally inspected UAE Reveal Math student editions (2026-10-09)

The operator supplied PDF student editions for **review only**. Their first-page covers and printed tables of contents were inspected; no PDF binaries or copyrighted textbook paragraphs are committed to this repository.

| Cover identity | Published edition | Volume | Pages | Evidence / limitation |
|---|---|---|---:|---|
| Reveal Math UAE Edition, **Grade 5 Advanced** | **2023–2024** | **2** | 289 | Printed contents of Volume 2 list modules 8–14: Divide Decimals; Add/Subtract Fractions; Multiply Fractions; Divide Fractions; Measurement and Data; Geometry; Algebraic Thinking. A summary also lists Volume 1 modules 1–7, but **the Volume 1 PDF was not provided**. |
| Reveal Math UAE Edition, **Grade 6 Advanced** | **2024–2025** | **1** | 304 | Printed contents summarize 10 modules: ratios/rates, fractions/decimals/percents, arithmetic, integers/rationals/coordinate plane, algebraic expressions, equations/inequalities, relationships, area, volume/surface area, statistics. |
| Reveal Math UAE Edition, **Grade 6 General** | **2025–2026** | **1** | 304 | Clearly marked **General** and therefore must NOT be cataloged as Advanced. Two operator-provided copies were byte-identical by SHA-256 (`E00533D7514548BD176B09858C2DD59AD3C3567E9C2A687804918CD891FA4431`). |

**What this proves:** UAE-specific Advanced editions existed in the respective prior years, and their older high-level mathematical scope can be compared against the canonical curriculum. The copies **do not establish** the 2026–2027 approved content, sequence, learning outcomes, all school-year volumes, a redistribution license, or a shared-syllabus policy. In particular, do not convert an older book's table of contents into `SourceCatalog` with falsely asserted 2026–2027 provenance.

**Required follow-up:** obtain a ministry/publisher-licensed **2026–2027 Grade 5 Advanced Volume 1 + other prescribed volumes** and **2026–2027 Grade 6 Advanced prescribed volumes**, or an authoritative current-year curriculum/outcome map, then record issuer, school-year, grade, stream, volume, source location, review date and rights. Only after independent mathematical review can the two strict audit scopes be closed.

## إجراءات الإغلاق الملزمة
1. الحصول على مرجع رسمي أو معتمد من الوزارة لكل من G5 Advanced وG6 Advanced للعام الدراسي 2026–2027، مع معرف المصدر/المحتوى والترخيص وتاريخ الاسترجاع.
2. تحديد هل المقصود **مسار مستقل ذو محتوى مختلف** أم جدول حصص للمسار مع منهج مشترك؛ لا نفترض تساوي الحالتين.
3. إدخال `SourceCatalog` وفق معايير `uae-moe-math.curriculum-pack.json` وبأدلة المصدر.
4. إعداد مخططات الدروس ومحتوى JSON المطابق لعناوين ومخرجات تعلم مثبتة؛ عدم نسخ نصوص محمية بلا ترخيص.
5. مراجعة رياضية ومراجعة مواءمة مع المصدر؛ تشغيل أدوات سلامة الأكواد والبصمات وحالة النشر.
6. تشغيل `full_curriculum_closure_audit.py --strict`، ثم إعادة بناء PostgreSQL 18 واختبارات API وPractice وCI الكامل.

## مبدأ عدم تجاوز الفحص
حتى العثور على مصدر معتمد: يُسمح باختبار مخطط المنصة والمناهج الحالية على قاعدة منفصلة، **ولا يُسمح بالتصريح بأن كامل نطاق 64/64 مكتمل أو أن نشر المحتوى الجديد جاهز**.

**المرجع التقني:** [V2 12 stages](./EDULYTICS_V2_ARCHITECTURE_FIRST_12_STAGE_PLAN_AR.md)
