# Edulytics — 500-Lesson Curriculum Execution Plan V2

**Plan ID:** `EDULYTICS-US-CCSS-500-V2`  
**Effective:** 2026-10-16  
**Status:** Authoritative operational batch policy, replacing the five-lesson delivery policy for the US Common Core programme  
**Repository:** `EIAS79/Edulytics`  
**Programme:** `US-CCSS-MATH`  
**Recorded scope:** 1,560 unique lessons, excluding Kindergarten, from `docs/curriculum/us-ccss-g1-g12-structural-audit.json` (2026-10-11 snapshot).  
**Requested work unit:** **500 lessons**, except the final 60.  
**Important:** This document changes the *planned size* of curriculum batches. It does **not** certify, implement, or deploy 500 lessons; the extractor, academic checks, and optimized CI still require implementation and verified execution.

## 1. Governing decision

1. Replace the previous **five-lesson chunks** with **500-lesson operational batches**: `B001`, `B002`, `B003`, and `B004` (60 lessons). No further five-lesson release units are scheduled.
2. Preserve **curriculum / grade / unit / lesson ordering**. A batch may cross a grade/course boundary; it must never reorder, duplicate, omit, or invent a lesson. Treat grade/course boundaries as explicit subsegments for accurate source reconciliation, capability testing, reporting and navigation.
3. Keep a stable canonical `LessonCode` for each entry. Reconcile actual runtime content and database data to the authoritative inventory *before* claiming the batch's exact 500 IDs. Counts from the dated snapshot are planning estimates until this reconciliation passes.
4. **Reuse first:** inspect real student-visible lesson content, outcomes, current Practice routing, shared mathematics capabilities and assessments. Repair documented defects; do not regenerate passing lessons or build 500 separate generators.
5. Batch scope is **500** for extraction, workload tracking and aggregate acceptance. Internal engineering can use **grade-scoped checkpoints, reviewable PRs, test shards and resumable jobs** for operational safety. Those subdivisions are *not* a return to the five-lesson completion policy.
6. **All 500 must have genuine lesson-level evidence** before `BATCH_INTERNAL_ACADEMIC_ACCEPTED` can be recorded. One blocked or unverified lesson means the batch is not accepted. A CI green build by itself is insufficient.
7. A 500-lesson **production release** is permitted only once the entire batch has met acceptance and existing GitHub + Render safety gates have passed. Incremental commits on a work branch do not mean the public site is live; do not skip protection rules or deploy unaccepted content. Owner confirms the live deployment and samples end-to-end workflows.
8. Keep **15 previously shipped Grade 1 lessons** in historical baseline: T001 (Unit 1 L01–L10) plus Grade 1 Batch 03 (L11–L15, prior release commit `54833b1669fe33f39c0043ac229f7440e1c8da9a`). Preserve their exact SHAs and prior acceptance evidence. These positions must be rechecked for drift, not reported as 15 newly implemented or automatically recertified lessons.
9. Never fabricate source approval, external accreditation, publisher licence, independent verification, deployment IDs, test PASS labels or production evidence.

## 2. Exact sequence by planning snapshot

| Batch | Sequence in canonical curriculum order | Planned total | Previously delivered / work remaining |
|---|---|---:|---|
| **B001** | Grade 1: 146; Grade 2: 146; Grade 3: 143; Grade 4: first 65 | **500** | 15 prior G1 releases (T001 10 + G1 B03 5); **485 need processing/status reconciliation** |
| **B002** | Grade 4: remaining 84; Grade 5: 148; Grade 6: 147; Grade 7: first 121 | **500** | Status unverified |
| **B003** | Grade 7: remaining 24; Grade 8: 131; Algebra 1: 132; Geometry: 124; Algebra 2: first 89 | **500** | Status unverified |
| **B004** | Algebra 2: remaining 31; named advanced supplementary courses: 29 (retain each course's own identity) | **60** | Status unverified |
| **Total** | US CCSS Grade 1–8, named high-school courses/supplements | **1,560** | 15 prior G1 lessons documented as published (T001 10 + B03 5) |

This partition is **count-based** until exact ordered `LessonCode` arrays are built and cross-checked with actual database records, blueprint sources and officially licensed/authorized edition references. Do not group unrelated advanced supplements under a fabricated grade label.

For B001, grade 4 positions 1–65 are **grade-ordinal positions**, not assumed to equal one particular unit's L01–L65. Preserve real unit identifiers.

The British/Cambridge, Polish and UAE curricula are outside the 1,560 US-CCSS scope. Onboarding any of them requires its own recorded count, edition, source mapping and explicit programme selection; do not silently mix curricula.

## 3. B001 intake and fixed checkpoint

**Next operational target:** `US-CCSS-B001`, positions 1–500 of the US CCSS stable cross-grade order. The first 15 Grade 1 lessons were previously released in T001 (first ten) and G1 Batch 03 (next five); next not previously shipped position is **Grade 1 #16**. Prior production teaching and attempt drift verification remains required.

**Intake outputs required before lesson editing:**

- Ordered, immutable inventory of **500 actual lesson codes** with grade, unit, title, source edition, attribution/licence state and hash of current student-visible content.
- Mapping to exact outcomes, objectives, prerequisite skills, allowed/forbidden question families, representation/answer types, solver+verifier capability and current Practice entrypoint.
- Compare source catalogues to live database and canonical content bundles; reject duplicate IDs, source-missing entries, ordinal discrepancies and unresolved licence gaps.
- A per-lesson status including `NOT_REVIEWED`, `READY_REUSE`, `CONTENT_REPAIR`, `PRACTICE_REPAIR`, `NEW_SHARED_CAPABILITY`, `BLOCKED`, `TESTED`, `INTERNALLY_ACCEPTED`. Never mark these 500 as accepted by default.
- A grade boundary-aware resume cursor and batch dashboard that survives interruptions without repeating completed grade extraction.

**Initial checkpoint record:** `docs/curriculum/batches/US-CCSS-B001-INTAKE.json`; it is **planning metadata**, not evidence that any of the 485 outstanding lessons were completed.

## 4. Track A + Track B for each lesson

**Track A: curriculum fidelity**
1. Check exact `LessonCode`, edition, original pedagogical aim, standards/outcome association and source licence.
2. Inspect **rendered** learner content including introduction, explanation, worked steps, visual representations, accessibility descriptions, common mistakes and summary.
3. Choose exact primary and secondary `SkillContract` mappings with evidence; never map from broad keywords alone.
4. Identify missing, incorrect, over-difficult or unrelated teaching and patch **only** those deficiencies.

**Track B: mathematical correctness and Practice**
1. Reuse existing generator, solver, verifier, answer evaluator, graph/game or interaction when capable.
2. If capabilities are missing, add **shared reusable** families and/or Mathematics Intelligence Kernel primitives; do not write a one-off engine per lesson.
3. Enforce answer types and input bounds, allowed and forbidden families, difficulty profiles and curriculum alignment.
4. Independently verify correct answers and reject incorrect/malformed/forbidden responses; preserve server-side authority, persisted attempts and student isolation.
5. Block unsafe/unverified lessons from auto-generation; never silently substitute a broad unrelated math question.

## 5. High-volume processing without fake speed

1. **Extract source indexes once per grade** and version/hash the results. Re-intake only changed grades or entries.
2. Partition the 500 tasks into **grade-aware execution shards** for parallelizable extraction, deterministic content checks and math property tests. Shards are internal execution machinery; the acceptance unit remains 500.
3. Group repeated defects and skills. One shared solver/generator/renderer fix should benefit every affected lesson; detect regressions on all consumers of changed shared components.
4. Save immutable checkpoints after each verified work shard: exact completed lesson IDs, source/input version, changed files, test-run IDs, failing items and next cursor.
5. Stage failed lessons for repair; retain passing records without reporting an entire batch as passed.
6. Expose accurate metrics: 500 expected, inventoried, reviewed, reused, modified, tested, internally accepted, blocked, deployed; include proof links and source hashes.
7. Do not promise a fixed duration for 500 lessons. New mathematical families, licensing gaps, solver deficiencies, CI environment limits and live deployments may dominate the schedule.

## 6. Verification gates

For **each** of the 500, require:

- Stable canonical code, grade/unit position, title, source/edition/licence and honest outcome mapping.
- Actual rendered learner content aligned with the intended goal, correct examples and expected representation.
- Exact `SkillContract` and acceptable family routing; absence of unrelated contextual fallback.
- Seeded question generation covering permitted constraints and grade difficulty, with independent math solution verification and correct/wrong/malformed answer tests.
- Relevant route and scoring/persistence tests; UI, keyboard/mobile/accessibility tests when interactive components change.
- No missing data, fabricated teacher/school approval, tenant isolation bypass, historic grade mutation or unlicensed asset.

**Risk-based CI:** test unchanged text differently from a new shared math engine. Always run existing required repository checks until separate, measured CI optimization is implemented. Schema/auth/security/core grading changes require wider regression, security and migration verification.

**Batch-level gate**: `500/500` genuinely accepted for B001–B003; `60/60` for B004. Compute from machine-readable per-lesson evidence tied to source edition and exact code SHAs. Anything below the target is `BATCH_BLOCKED`, not `BATCH_ACCEPTED`. Historical evidence for the 15 prior Grade 1 lessons may count only after full content-version, student-visible behavior and SHA drift checks; all 500 still require B001-level acceptance.

**Deployment**: use normal branch protections, checks, merge and Render triggers. Verify deployment ID, live SHA, HTTP health, real course navigation and authenticated Practice persisted grading. Owner runs post-release production UAT. A merged Markdown policy is **not** a released 500-lesson curriculum batch.

## 7. Status and artifacts

Proposed lifecycle (requires implementation; identifiers below are not asserted to exist in current code):

```text
BATCH_PLANNED
  -> INVENTORY_RECONCILED (all actual LessonCodes + source metadata)
  -> REVIEW_IN_PROGRESS (per-lesson Track A/Track B work)
  -> LESSON_TESTED / LESSON_BLOCKED
  -> 500/500 INTERNALLY_ACCEPTED (60/60 for final)
  -> BATCH_READY
  -> MERGED
  -> VERIFIED_LIVE
  -> OWNER_UAT
```

Recommended artifacts:

```text
docs/curriculum/batches/US-CCSS-B001-INTAKE.json  # current planning checkpoint
artifacts/curriculum-delivery/US-CCSS-MATH/
  source-index.json
  B001/ordered-lesson-codes.json
  B001/lesson-evidence.jsonl
  B001/gap-dashboard.json
  B001/checkpoint.json
  B001/batch-acceptance.json
```

Store screenshots, browser traces and any identifying student data securely in permitted CI/private locations, not public Git source.

## 8. Rollout sequence

| Phase | Deliverable | Completion proof |
|---|---|---|
| P0 | Adopt authoritative 500-lesson policy; retire five-lesson policy | Main README and policy link point here |
| P1 | B001 extractor + reconcile the first 500 real LessonCodes, grades and database/source metadata | Exact inventory, zero unhandled duplicates/missing codes |
| P2 | Classify all 500 (source/goal/content/Practice/maths) | Per-lesson gap matrix without invented PASS |
| P3 | Reuse/repair content and add only reusable missing skills/Practice capability | Source-aligned student-visible output |
| P4 | Scoped and shared-impact test suites + per-lesson evidence | Each tested lesson independently verified |
| P5 | 500-lesson acceptance gate + security/deployment checks | Genuine `500/500` with source/Git hashes |
| P6 | Merge, verify Render live release, owner production UAT | Actual live SHA, deployment ID and test evidence |
| P7 | Repeat B002, B003, B004 | Completed 1,560/1,560, independently evidenced |

**No modification to production maths code, database, CI, branch rules or Render is authorized by this Markdown policy alone.** Implement and test these follow-on changes separately. Preserve T001 historical IDs, SHAs and approved source content.

## 9. Reporting contract

After any work session or checkpoint, report the **actual**:

```text
Programme: US-CCSS-MATH
Batch: B001 of 4  |  planned scope: 500
Actual IDs inventoried: X/500
Previously released baseline: 15 (T001 10 + G1 B03 5; drift checks required)
New lessons source/content reviewed: X/485
Practice/Math aligned and independently tested: X/500
Per-lesson internal acceptance: X/500
Blocked: count + exact LessonCodes/reasons
Git PR/commit and required checks: actual state
Render live SHA/deployment: actual state
Next cursor: curriculum/grade/unit/lesson
```

A count of *planned* lessons must never be presented as a count of *implemented* lessons.

## 10. Precedence and historical compatibility

This supersedes `EDULYTICS_GRADE_FIRST_FIVE_LESSON_EXECUTION_PLAN_V1.md` **for future US-CCSS batching**, but preserves its useful reuse-first, grade intake and test-by-impact engineering principles. The earlier `US_CCSS_GRADE_BY_GRADE_LESSON_ACADEMIC_CERTIFICATION_PLAN.md` remains historical; its old 500-lesson gate is **not** automatically restored as executable software. T001 ten-lesson certification/acceptance and older six-gate ledgers remain historically intact.

This programme remains curriculum-specific; other curricula require separate intake decisions. Existing production security protections and mathematically honest generation requirements always take precedence over schedule optimization.

## 11. Owner-approved execution commitments (2026-10-17)

**Authority:** These are the owner's explicit operating and accountability instructions for future Edulytics V2 execution. They do **not** introduce, remove, relax or reorder V2 phases P0–P7, acceptance checks, the 500-lesson release unit, or deployment protections. The owner requested this addition to V2; it must not be interpreted as permission to redesign V2.

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

### 11.1. معيار المحاسبة

**النجاح النهائي ليس بعدد الـPRs أو نجاح CI وحده**، بل استيفاء شروط V2 الأصلية لكل الدروس الـ500، واعتمادها داخليًا بأدلة صادقة، ثم الدمج والنشر والتحقق من التشغيل.

- **تم تنفيذه:** محتوى/كود درس مستقل ومطابق أو وظيفة موجودة فعلًا وجرى التحقق منها، وليس مجرد إدراج الدرس في القائمة.
- **تم اختباره:** دليل درس فعلي من P4 مرتبط بمعرّف الاختبار ونسخة المصدر وcommit SHA.
- **تم اعتماده:** قرار P5 الصحيح على نطاق الدفعة كاملة وفق شروط V2 دون إضافة بوابات.
- **تم نشره والتحقق منه:** SHA حيّ فعلي على Render مع تحقق الطالب وPractice وحفظ الإجابات وowner UAT.

**أي حالة غير مؤكدة تُسجّل "غير متحقق منها" ولا تُعرض كنجاح.** هذه الالتزامات تعليمات تشغيل ومحاسبة، وليست بوابة نشر إضافية.
