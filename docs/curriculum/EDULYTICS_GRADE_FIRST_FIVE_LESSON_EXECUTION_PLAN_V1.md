# SUPERSEDED — Five-Lesson Curriculum Delivery Plan

**Status:** Superseded as the operating plan for US-CCSS mathematics on 2026-10-16 at the project owner's request.

**Current authoritative plan:** [Edulytics 500-Lesson Batch Execution Plan V2](EDULYTICS_500_LESSON_BATCH_EXECUTION_PLAN_V2.md).

The old **five-lesson batch size**, five-lesson grade-only release chunks and G1-003 (L11–L15) next-release recommendation are **no longer active policy**.

The requested US-CCSS programme now uses **500 + 500 + 500 + 60** lesson work/acceptance batches across grade boundaries, preserving source order and per-lesson evidence. The first 500 includes the ten historical T001 lessons (already previously shipped) and 490 outstanding positions, subject to source/DB reconciliation.

The previous detailed document remains available in Git history (including PR #497). Its useful principles remain: reuse existing content and shared Math/Practice capabilities, inspect student-visible work, verify each lesson independently, select tests by change impact, protect licences/student data, preserve historical T001 evidence, and keep branch/CI/Render production protections in force.

**This redirect does not assert that 500 lessons have been completed, accepted or deployed.**

## Historical V1 addendum — owner-approved execution commitments (2026-10-17)

**Applicability and precedence:** These same 19 owner-approved commitments are retained here at the owner's request, for the historical V1 reference and for all continuing Edulytics implementation. **V1 remains superseded**: its five-lesson batch/release policy is not reinstated. For US-CCSS, the authoritative V2 document controls P0–P7 and the B001 500-lesson release unit. This is a documentation and execution-accountability update only, not a new acceptance gate or permission to alter source content.

1. **عدم استخدام TinyFish؛ استخدام Desktop Commander.** ممنوع استخدام TinyFish نهائيًا في تنفيذ Edulytics، بما في ذلك كبديل عند التعطل. يُستخدم Remote Desktop Commander لفحص الملفات وتنفيذ الأوامر والاختبارات على الأجهزة **المصرح بالاتصال بها فقط**. يمكن أيضًا استخدام أدوات GitHub وRender وNeon المتصلة مباشرة عند الحاجة. إذا كان Desktop Commander غير متصل، يجب توضيح العائق وعدم الادعاء بالوصول أو التحول إلى TinyFish.

2. **كتب الرياضيات الخارجية مراجع فقط.** تُستخدم كتب Illustrative Mathematics وغيرها، بما فيها Teacher Guide وStudent Book، لمراجعة أهداف كل درس ومهاراته وصحة الرياضيات والشرح والأنشطة المطلوبة للاعتماد وفق V2؛ لا تُعامل هذه المراجع كمحتوى مطلوب نقله إلى Edulytics.

3. **استقلال محتوى Edulytics.** الشرح والأمثلة والأسئلة والتمارين والأنشطة والتفاعلات والرسومات داخل المنتج تُؤلَّف وتُطوَّر بصورة مستقلة، ولا تُنسخ نصوص الكتب أو صورها أو رسوماتها أو أنشطتها حرفيًا. المطلوب تحقيق الهدف التعليمي بدقة بمحتوى أصلي، لا استنساخ صفحة الناشر.

4. **عدم فرض اسم الناشر داخل البرنامج.** لا يُضاف اسم Illustrative Mathematics أو شعاره أو نسبة دروس Edulytics إليه في واجهة المستخدم لمجرد الاستعانة بكتبه في المراجعة، ولا يُفترض وجوب نسب المحتوى المستقل إليه. إن أُريد إدخال أصل خارجي بعينه فعلًا، تُراجع حقوقه وشروطه أولًا ولا يُدرج تلقائيًا. استخدام الكتاب كمرجع لا يُعدّ نسخًا لمحتواه ولا نتيجة اعتماد.

5. **الحفاظ على خطة V2.** لا تُضاف مراحل أو بوابات أو شروط اعتماد أو توقيعات جديدة من دون قرار صريح من صاحب المشروع. هذه الإضافة توثّق تعليمات التنفيذ ولا تُعدّل الآلية المعتمدة لـP0–P7.

6. **العمل على الدروس الـ500 كلها.** B001 تشمل 146 درسًا من الصف الأول، و146 من الثاني، و143 من الثالث، و65 من الرابع. إصلاح 12 أو 15 درسًا أو فتح PR جزئي لا يعني اكتمال أو اعتماد الدفعة. الدروس الـ15 المنشورة سابقًا تُحفظ أدلتها وتُراجع لأي تغيّر وفق V2.

7. **التحقق التعليمي لكل درس.** تُراجع الأهداف والشرح والأمثلة والخطوات والأنشطة والرسوم والإجابات ومطابقة Practice للمهارة، بالاستعانة بالمراجع وبفحص ما يراه الطالب فعليًا؛ لا يُكتفى بالعنوان أو نجاح تشغيل الصفحة.

8. **إصلاح الأخطاء الحقيقية فقط.** لا يُعاد إنشاء أو تغيير درس صحيح بسبب التشابه أو تكرار النص الخام وحده. يجب إثبات الخلل الفعلي ومجاله قبل التعديل، مع الاحتفاظ بالأجزاء الصحيحة.

9. **عدم تكرار الاختبارات الناجحة بلا سبب تقني.** تُحفظ نتائج P4 وأدلتها ومعرّفات الاختبارات وروابط تشغيلها وSHA الصحيح، ولا تُعاد إلا عند تغيير مؤثر أو فساد دليل أو حاجة فنية لاختبار الانحدار. لا تُخلط نتائج نسخ مختلفة.

10. **الفصل بين بوابة الاختبار والاعتماد.** P4 تنفّذ الاختبارات الفعلية وتسجّل PASS أو FAIL بأدلتها؛ P5 تقرأ الأدلة المسجّلة وتطبّق الشروط الموجودة في V2، دون إعادة تنفيذ الاختبارات داخلها أو اختلاق نتائج.

11. **لا توقيع معلم ولا موافقة ناشر كشرط اعتماد داخلي إضافي.** يتم الاعتماد الداخلي وفق اختبارات Edulytics وأدلة V2 القائمة؛ لا تُفرض توقيعات أو موافقات خارجية غير متفق عليها. هذا لا يعفي أي أصل خارجي يُستخدم فعلًا من حقوقه الواجبة.

12. **عدم اختلاق نتائج نجاح.** لا يُسجّل PASS أو TESTED أو INTERNALLY_ACCEPTED إلا بدليل حقيقي صالح للدرس ونسخة الكود المعنية. نجاح CI العام أو قراءة PDF أو نجاح التوليد التقني وحده لا يثبت جودة التدريس.

13. **الحفاظ على الكود والأعمال السابقة.** تُراجع ملفات المستودع والفروع وPRs والاختبارات الناجحة والقرارات قبل أي تعديل؛ لا تُحذف أو تُصفّر أو تُستبدل تغييرات قائمة بغير مبرر موثّق وإذن مناسب.

14. **إكمال المسار حتى النشر وفق V2.** إعداد/إصلاح المحتوى المستقل وPractice ← اختبارات P4 وأدلتها ← اعتماد 500/500 في P5 ← دمج GitHub بالشروط القائمة ← نشر Render ← التحقق من SHA الفعلي وعمل الطالب وتخزين إجاباته وإجراء owner UAT. لا تجاوز لمراحل الخطة.

15. **عدم إعلان نشر الـ500 قبل اكتماله.** نشر ملفات متابعة أو تحديث CI أو بعض الدروس لا يُسمّى إطلاق B001. إعلان الإصدار يتطلب قبول الدفعة كلها ونشرها واختبارها فعليًا.

16. **تقارير دقيقة بالأرقام والروابط.** يجب التفريق بين المحصور والمنفّذ والمراجع والمختبَر والمعتمد والمدمج والمنشور والمتحقّق منه، مع GitHub PR/run وروابط الأدلة وcommit SHA وRender deployment وأسباب التعطل الدقيقة. لا تُعرض الأعداد المخططة كمنجزة.

17. **عدم تكرار القرارات المحسومة.** يُستكمل العمل من آخر حالة صحيحة دون إعادة البحث أو الجدل في الترخيص أو استقلال المحتوى أو توقيع المعلم ما لم تظهر مسألة جديدة مؤثرة فعلًا.

18. **عدم نسبة اتفاق إلى صاحب المشروع لم يصدر منه.** يُفرَّق بين قراره الصريح والمعلومة العامة والاستنتاج والاقتراح. يُقدّم أحدث قرار صريح منه عند أي تعارض.

19. **عدم الادعاء بالعمل في الخلفية.** لا يُقال إن العمل سيستمر ذاتيًا بعد انتهاء المحادثة العادية. تُذكر فقط الأعمال المنفّذة بالفعل والتحققات الحقيقية والمهام الجارية في أدوات مرتبطة والعوائق المعروفة؛ ولا يُدّعى تشغيل مؤجل إلا إذا وُجد تفويض وتشغيل فعلي.

### معيار المحاسبة

**النجاح النهائي ليس بعدد الـPRs أو نجاح CI وحده**، بل استيفاء شروط V2 الأصلية لكل الدروس الـ500، واعتمادها داخليًا بأدلة صادقة، ثم الدمج والنشر والتحقق من التشغيل.

- **تم تنفيذه:** محتوى/كود درس مستقل ومطابق أو وظيفة موجودة فعلًا وجرى التحقق منها، وليس مجرد إدراج الدرس في القائمة.
- **تم اختباره:** دليل درس فعلي من P4 مرتبط بمعرّف الاختبار ونسخة المصدر وcommit SHA.
- **تم اعتماده:** قرار P5 الصحيح على نطاق الدفعة كاملة وفق شروط V2 دون إضافة بوابات.
- **تم نشره والتحقق منه:** SHA حيّ فعلي على Render مع تحقق الطالب وPractice وحفظ الإجابات وowner UAT.

**أي حالة غير مؤكدة تُسجّل "غير متحقق منها" ولا تُعرض كنجاح.** هذه الالتزامات تعليمات تشغيل ومحاسبة، وليست بوابة نشر إضافية.

