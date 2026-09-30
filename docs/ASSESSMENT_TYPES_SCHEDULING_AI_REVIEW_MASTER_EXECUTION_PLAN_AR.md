# Edulytics — خطة تطوير Assessment: الأنواع، الجدولة التلقائية، التسليم الورقي/PDF، المراجعة بالذكاء الاصطناعي، ونشر النتائج

**حالة الوثيقة:** خطة تحليل وتنفيذ فقط — لا تغيّر أي سلوك Production  
**Repository:** `EIAS79/Edulytics`  
**Baseline branch:** `main`  
**Baseline SHA:** `d7f42477e76a59f5624cf53cb54f755ec2013c04`  
**تاريخ الخطة:** 2026-09-30  
**اللغة:** العربية  
**الهدف:** تحويل Assessment من كيان عام واحد إلى منظومة واضحة تدعم Exam/Test وWorksheet وHomework، مع جدولة تلقائية، قنوات تسليم متعددة، رفع PDF/صور، AI-assisted marking مع مراجعة بشرية إلزامية قبل اعتماد الدرجات، ونشر نتائج منفصل صراحة عن إغلاق المهمة.

---

# 1. قرار النطاق

هذه الوثيقة **لا تنفذ الميزة** ولا تغيّر قاعدة البيانات أو الخدمات أو الواجهات الآن.

المطلوب في هذه المرحلة:

1. تثبيت فهم المتطلبات.
2. توثيق الوضع الحالي الحقيقي من الكود الحالي.
3. تحديد القرارات المعمارية.
4. تحديد المخاطر.
5. وضع خطة مراحل قابلة للتنفيذ والاختبار والدمج تدريجيًا.
6. منع أي تغيير يكسر Assessment Builder أو Analytics أو Student Evaluation الحالي.

---

# 2. ملخص الوضع الحالي في المستودع

## 2.1 آخر حالة `main`

الـbaseline المستخدم هنا هو:

```text
d7f42477e76a59f5624cf53cb54f755ec2013c04
Merge PR #323: complete Adaptive V2 learner flow and difficulty progression
```

أحدث سلسلة عمل كانت مرتبطة بـ Adaptive V2، ومنها:

- الحفاظ على اختيار الطالب أثناء شاشة المراجعة؛
- إصلاح semantics الخاصة بالـradio controls في read-only review؛
- إصلاح أسماء الدروس التاريخية في Question Log؛
- ضبط difficulty/form variant alignment لبعض مولدات Place Value وRounding؛
- تحسين ترتيب voice guidance؛
- تثبيت progression/recovery behavior.

## 2.2 ملاحظة review حالية يجب ألا تُنسى

بعد دمج PR #323 ظهر review finding من نوع P2 ما زال غير outdated وغير resolved وقت إعداد هذه الخطة:

- بعض Challenge slots في `PracticeAssessmentTaxonomy` معلّمة كأنها `ErrorAnalysis`;
- بينما بعض الأسئلة الفعلية الناتجة هي digit-replacement / inverse-place / interval-intersection؛
- النتيجة: metadata يمكن أن تدّعي Question Form لم يحصل عليه الطالب بالفعل.

هذه ليست جزءًا من ميزة Assessment الجديدة، لكن يجب حلها في corrective PR منفصل **قبل الاعتماد على Question Form metadata كدليل تقييم أو coverage**.

---

# 3. ما هو موجود بالفعل في Assessment اليوم

## 3.1 الحالة الحالية

الـAssessment الحالي يملك:

```text
AssessmentStatus:
- Draft
- Open
- Closed
```

ويملك:

```text
AssessmentDeliveryMode:
- Offline
- Online
```

ويملك targeting:

```text
AssessmentTargetType:
- Class
- Student
```

ويملك difficulty:

```text
AssessmentDifficultyBand
```

لكن لا يوجد حاليًا مفهوم رسمي يفرق بين:

```text
Exam/Test
Worksheet
Homework
```

ولا يوجد في `Assessment` الحالي:

```text
AvailableFromUtc
DueAtUtc
AttemptTimeLimitMinutes
AutoOpen
AutoClose
ResultReleaseStatus
ResultsPublishedAtUtc
AssessmentType
SubmissionChannels
EvidenceRole
```

## 3.2 Assessment Builder الحالي

الـworkflow الحالي المهم يجب الحفاظ عليه:

```text
Select
→ Generate
→ Review
→ Approve
→ Publish
```

والـBuilder الحالي يدعم:

- إنشاء أسئلة يدويًا؛
- generation؛
- regenerate؛
- edit؛
- approve question؛
- approve all؛
- delete؛
- publish؛
- Online/Offline settings؛
- Class أو Student targeting؛
- Difficulty؛
- PDF للطالب في Offline؛
- PDF Answer Key للمدرس في Offline.

## 3.3 Online Assessment الحالي

في الوضع الحالي:

- الطالب لا يستطيع فتح Assessment إلا إذا:
  - Status = Open؛
  - DeliveryMode = Online؛
  - الطالب enrolled في الفصل؛
  - والـtarget يشمله.
- Submit يحسب score آليًا في الأسئلة التي يمكن تقييمها باستخدام `MathematicsAnswerEquivalence`.
- النتيجة تُحفظ فورًا.
- لكن الطالب لا يرى نتيجة الـOnline Assessment إلا بعد أن يصبح Assessment = Closed.

إذن يوجد بالفعل جزء من فكرة:
```text
Teacher-controlled result release
```
لكنها مربوطة اليوم بـ `Closed` وليس بزر مستقل اسمه `Post Results`.

## 3.4 Offline Assessment الحالي

يوجد بالفعل:

- Student Paper PDF؛
- Teacher Answer Key PDF؛
- Offline Assessment Results XLSX workflow؛
- Bulk import للدرجات؛
- Preview قبل confirm؛
- Student-centric result presentation.

لكن غير موجود:

```text
Student uploads solved PDF
→ OCR / handwriting extraction
→ map answer regions to questions
→ AI proposes grading
→ Teacher reviews/overrides
→ Teacher finalizes
→ Teacher posts results
```

إذن الـPDF المقصود في المتطلب الجديد **ليس تكرارًا** للـPDF الموجود حاليًا.

---

# 4. الفهم النهائي للأنواع الثلاثة

سنعتمد **3 أنواع Canonical فقط**:

```text
1. Exam
2. Worksheet
3. Homework
```

## 4.1 Exam / Test

`Test` ليس نوعًا رابعًا في قاعدة البيانات.

سيكون:

```text
Canonical type: Exam
UI label: Exam / Test
```

ويمكن لاحقًا إضافة display subtype إذا احتجنا:

```text
ExamLabel:
- Exam
- Test
- Quiz
```

لكن بدون خلق مسارات grading مختلفة بلا داعٍ.

## 4.2 Worksheet

قد تكون:

- Online؛
- Paper؛
- File upload؛
- أو أكثر من قناة تسليم إذا سمح المدرس.

## 4.3 Homework

قد تكون:

- Online form؛
- Paper solved then uploaded؛
- PDF/Photo submission؛
- أو mixed submission channels.

---

# 5. قرار معماري أساسي: لا نخلط Type مع Delivery مع Evaluation

هذه ثلاثة أبعاد مستقلة.

## البعد الأول — Assessment Type

```text
Exam
Worksheet
Homework
```

يجيب عن:

> ما طبيعة النشاط؟

## البعد الثاني — Submission / Delivery

يجيب عن:

> كيف سيستلم الطالب المهمة وكيف سيرسل الحل؟

يجب أن يسمح مستقبلًا بقنوات مثل:

```text
OnlineForm
StudentFileUpload
TeacherRecordedPaper
```

ويمكن السماح بأكثر من قناة لنفس Homework/Worksheet.

## البعد الثالث — Evidence Role

يجيب عن:

> هل هذه النتيجة يجب أن تغيّر Mastery/Evaluation الرسمي؟

مثل:

```text
Summative
Formative
PracticeOnly
```

عدم فصل هذه الأبعاد سيؤدي إلى أخطاء مثل:

- كل Homework يصبح تلقائيًا امتحانًا رسميًا؛
- كل Worksheet يغيّر Mastery؛
- Offline = غير رسمي؛
- Online = رسمي؛

وكل هذه استنتاجات غير صحيحة.

---

# 6. التصميم المقترح للـAssessment Type

إضافة enum جديدة:

```text
AssessmentType
- Exam
- Worksheet
- Homework
```

Defaults المقترحة:

| النوع | Evidence Role الافتراضي | Result Release الافتراضي |
|---|---|---|
| Exam/Test | Summative | Manual |
| Worksheet | Formative أو PracticeOnly حسب سياق المدرسة | Manual |
| Homework | PracticeOnly | Manual |

مهم: هذه defaults وليست قواعد صلبة تمنع المدرسة من تحديد policy مختلفة.

---

# 7. Auto Timer / Scheduling — التصميم الصحيح

فكرة Auto Timer يجب ألا تكون مجرد background task يقول:

```text
if time == 17:00:
    set Open
```

هذا غير كافٍ، لأن worker قد يتأخر أو يتوقف.

القاعدة الصحيحة:

> **وقت الخادم + access guard هو مصدر الحقيقة.**

## 7.1 الحقول الأساسية المقترحة

```text
AvailableFromUtc?
DueAtUtc?
AttemptTimeLimitMinutes?
AutoOpenEnabled
AutoCloseEnabled
SchoolTimeZoneId
```

## 7.2 Exam/Test Online

مثال:

```text
Created: 14:00
AvailableFrom: 17:00
DueAt: 18:00
AttemptTimeLimit: 60 minutes
```

قبل 17:00:

- الطالب لا يراه في قائمة available assessments؛
- لا يمكنه فتح URL مباشرة؛
- لا يمكنه إنشاء attempt؛
- لا يمكنه submit؛
- المدرس يستطيع التعديل طالما لم تبدأ نافذة الاختبار ولم يبدأ أي attempt.

عند 17:00:

- يصبح متاحًا تلقائيًا؛
- يظهر للطلاب المستهدفين؛
- يمكن إرسال notification؛
- يبدأ global availability window.

إذا استُخدم `AttemptTimeLimitMinutes`:

- وقت الطالب الشخصي يبدأ عندما يبدأ attempt؛
- لكن لا يجوز له تجاوز global hard close إذا المدرسة اختارت hard end.

## 7.3 لا نعتمد على scheduler وحده

يجب أن يكون هناك:

```text
CanAccessAssessment(nowUtc, assessment, student)
```

ويتحقق من:

```text
target
enrollment
status
delivery/submission channel
AvailableFromUtc
DueAtUtc
attempt state
time limit
cancellation state
```

حتى لو background worker لم يعمل، لا يستطيع الطالب الدخول مبكرًا.

## 7.4 وظيفة background scheduler

تكون وظيفته:

- إرسال notification عند الفتح؛
- تحديث presentation status إذا احتجنا؛
- auto-close للمهام المنتهية؛
- إنشاء audit events؛
- تشغيل outbox بشكل idempotent.

لكن **ليس** هو security boundary.

---

# 8. التعديل قبل بداية Exam

المتطلب:

> المدرس يستطيع تعديل/تغيير/تمديد/حذف الاختبار قبل أن يبدأ.

سيتم تثبيته هكذا:

## قبل AvailableFromUtc

يسمح بـ:

- تعديل title؛
- questions؛
- marks؛
- schedule؛
- class/target ضمن القيود؛
- duration؛
- delivery؛
- حذف كامل إذا لا توجد submissions/attempts.

## بعد الفتح وقبل أي Attempt

يمكن السماح ببعض التعديلات الإدارية، لكن تغيير questions بعد الإتاحة مخاطرة.

القرار المقترح:

- content يصبح locked عند أول إتاحة فعلية؛
- أو عند أول attempt — أيهما أسبق حسب policy.

## بعد وجود Attempt أو Submission

لا يسمح بـ:

- hard delete؛
- تغيير question identity؛
- تغيير answer key بطريقة صامتة؛
- تقليل deadline بما يضر attempt قائم.

يسمح بـ:

- extend deadline؛
- extend time window؛
- cancel assessment مع reason؛
- emergency correction من خلال versioned change + audit، وليس تعديل صامت.

---

# 9. Homework وWorksheet — Start Date + Due Date

كلاهما يدعم:

```text
AvailableFromUtc
DueAtUtc
```

مثال:

```text
Homework created now
Available: tomorrow 08:00
Due: after 2 days at 18:00
```

أو:

```text
Available: now
Due: after 1 hour
```

أو:

```text
Available: now
Due: after 5 minutes
```

الواجهة يجب أن تسمح بطريقتين:

1. اختيار date/time صريح.
2. اختيار relative duration ثم تحويله إلى timestamp واضح قبل الحفظ.

لا نخزن فقط عبارة:
```text
"after 2 days"
```
بل نحسب ونخزن timestamp نهائيًا واضحًا.

---

# 10. Late Submission Policy

مطلوب من البداية لأن Due Date بدون policy ناقص.

القيم المقترحة:

```text
BlockAfterDue
AcceptAndMarkLate
TeacherApprovalRequired
```

Defaults:

- Exam: `BlockAfterDue`
- Worksheet: حسب المدرسة
- Homework: `AcceptAndMarkLate` أو school policy

كل late submission يجب أن يسجل:

```text
SubmittedAtUtc
WasLate
DueAtUtcAtSubmission
LateBySeconds
```

حتى لو تم تمديد الموعد لاحقًا نحتفظ بالسياق التاريخي.

---

# 11. قنوات التسليم الجديدة

بدل تحويل `AssessmentDeliveryMode` الحالي مباشرة إلى enum ضخمة، نبدأ بإضافة مفهوم مستقل متعدد القنوات.

مقترح:

```text
AssessmentSubmissionChannel
- OnlineForm
- StudentFileUpload
- TeacherRecordedPaper
```

وقد يملك Assessment أكثر من channel.

أمثلة:

## Exam Online

```text
OnlineForm
```

## Exam Paper

```text
TeacherRecordedPaper
```

## Homework Online

```text
OnlineForm
```

## Homework Offline solved on paper ثم upload

```text
StudentFileUpload
```

## Homework يسمح Online أو Scan

```text
OnlineForm + StudentFileUpload
```

هذا يحقق معنى أن Homework/Worksheet قد يدعمان online وoffline/mixed بدون كسر معنى `AssessmentDeliveryMode` الحالي فورًا.

---

# 12. Student PDF / Scan Upload Flow

## 12.1 السيناريو

```text
Teacher creates Homework/Worksheet
→ Paper/PDF is distributed
→ Student solves on paper
→ Student scans or photographs pages
→ Student uploads PDF/images
→ Edulytics validates file
→ extraction/OCR
→ answer-to-question mapping
→ proposed grading
→ Teacher review
→ Teacher finalizes
→ results remain hidden
→ Teacher clicks Post Results
→ student sees final result
```

## 12.2 أنواع الملفات

المرحلة الأولى المقترحة:

```text
PDF
JPEG
PNG
```

ثم normalization إلى document pages.

## 12.3 قيود أمنية

يجب وجود:

- MIME validation؛
- extension/content mismatch detection؛
- file size limit؛
- page count limit؛
- image dimension limit؛
- malware scan؛
- safe PDF parsing؛
- no active JavaScript/forms execution؛
- private object storage؛
- signed short-lived download URLs؛
- school/tenant isolation؛
- retention policy؛
- audit trail.

---

# 13. Paper Identity — كيف نعرف أن الورقة تخص أي Assessment وأي Student؟

لا ينبغي الاعتماد فقط على اسم الملف.

المقترح للـPDF الذي يولده Edulytics:

- Assessment ID machine-readable token؛
- paper version؛
- optional Student-specific token؛
- QR code أو barcode آمن/موقّع؛
- question anchors/page coordinates إذا أمكن.

هذا يجعل:

```text
scan
→ identify assessment/version
→ identify question regions
```

أكثر موثوقية بكثير.

إذا رفع الطالب scan لورقة خارجية:

- يسمح upload؛
- لكن mapping يصبح manual/assisted وقد يحتاج teacher confirmation.

---

# 14. OCR / Handwriting / AI Extraction

## قاعدة مهمة

الذكاء الاصطناعي **ليس المصحح النهائي**.

المسار:

```text
Uploaded Document
→ Document Normalization
→ OCR / Handwriting Extraction
→ Page Segmentation
→ Question Region Detection
→ Extracted Student Answer
→ Confidence Score
→ Mathematical / rubric evaluation
→ Proposed Score
→ Teacher Review
```

## 14.1 مستويات confidence

مقترح:

```text
High
Medium
Low
Unrecognized
```

كل answer يعرض للمدرس:

- الصورة الأصلية للمنطقة؛
- النص/المعادلة المستخرجة؛
- confidence؛
- correct answer/rubric؛
- proposed score؛
- explanation لماذا اقترح النظام الدرجة.

## 14.2 Mathematics

إذا answer يمكن تمثيله رياضيًا:

- نستخدم Mathematics Answer Evaluator / Math Kernel عندما يكون capability verified؛
- لا نعتمد على LLM كحكم رياضي وحيد.

## 14.3 Free-response

في الإجابات المقالية/الشرح:

- AI قد يقترح rubric match؛
- لكن الدرجة الرسمية تبقى pending حتى Teacher review.

---

# 15. Teacher Review Queue

إنشاء workflow واضح:

```text
Uploaded
→ Processing
→ ExtractionReady
→ NeedsTeacherReview
→ TeacherApproved
→ Finalized
→ ResultsPublished
```

Teacher UI المقترحة:

يسار:
- scan page / answer crop

يمين:
- question؛
- expected answer/rubric؛
- extracted answer؛
- AI proposed score؛
- confidence؛
- teacher final score؛
- teacher note؛
- override reason عند اختلاف كبير.

Actions:

```text
Approve
Edit extracted answer
Change score
Flag unreadable
Request resubmission
Finalize student
Finalize all
```

---

# 16. فصل Finalize عن Post Results

هذا قرار أساسي.

## Finalize

يعني:

> المدرس أنهى المراجعة وأصبح score نهائيًا داخل النظام.

## Post Results

يعني:

> يسمح للطالب برؤية الدرجة والتعليقات.

يجب ألا يكون:

```text
Close Assessment == Publish Results
```

دائمًا.

إضافة مفهوم:

```text
ResultReleaseStatus
- Withheld
- Ready
- Published
```

و:

```text
ResultsPublishedAtUtc?
ResultsPublishedByUserId?
```

Default لكل الأنواع في v1:

```text
Manual result release
```

حتى لا تظهر نتيجة OCR/AI أو auto-grading قبل أن يعتمدها المدرس.

---

# 17. ماذا يرى الطالب قبل Post Results؟

بعد submission:

```text
Submitted
Awaiting teacher review
```

ولا يرى:

- provisional AI score؛
- OCR confidence؛
- internal extraction؛
- answer key؛
- teacher draft comments.

بعد Post Results فقط يرى:

- final score؛
- percentage؛
- teacher feedback؛
- per-question breakdown إذا policy تسمح؛
- correct solution إذا policy تسمح.

---

# 18. القرار بشأن Homework/Worksheet داخل Evaluation

## 18.1 المشكلة الحالية

Evaluation الحالي يعرف أساسًا:

```text
Assessment
Practice
```

وأي Assessment غير Draft يمكن أن يدخل في official evidence.

إذا أضفنا Homework وWorksheet كـAssessment Types بدون policy إضافية:

> Homework وWorksheet سيؤثران تلقائيًا على Official Mastery.

هذا غير مقبول.

## 18.2 القرار المقترح

إضافة:

```text
AssessmentEvidenceRole
- Summative
- Formative
- PracticeOnly
```

## 18.3 Exam/Test

Default:

```text
Summative
```

ويؤثر في:

- official assessment mastery؛
- current mastery؛
- confidence؛
- trends؛
- student evaluation؛
- staff analytics.

لكن فقط بعد final grading وفق الـpolicy المعتمدة.

## 18.4 Homework

Default:

```text
PracticeOnly
```

السبب:

Homework قد يتم:

- بكتاب مفتوح؛
- بمساعدة ولي أمر؛
- بمساعدة مدرس خاص؛
- باستخدام AI؛
- بعد retries متعددة؛
- بدون إشراف.

لذلك لا يصلح افتراضيًا كدليل مستقل قوي على mastery.

لكن لا نهمله.

يظهر في Student Self Evaluation داخل قسم منفصل مثل:

```text
Homework / Formative Progress
```

بدون خلطه تلقائيًا بالـofficial mastery.

يمكن للمدرسة لاحقًا السماح لبعض Homework بأن تكون `Formative` إذا كانت policy واضحة.

## 18.5 Worksheet

Default يعتمد على السياق:

### In-class supervised worksheet

يمكن أن تكون:

```text
Formative
```

### Take-home worksheet

الأفضل:

```text
PracticeOnly
```

## 18.6 لا نستخدم type وحده لتحديد القوة التقييمية

الأصح:

```text
AssessmentType = Worksheet
EvidenceRole = Formative
```

أو:

```text
AssessmentType = Worksheet
EvidenceRole = PracticeOnly
```

بناءً على سياقها الحقيقي.

---

# 19. weighting المقترح للتقييم

لا أنصح ببدء النظام بوزن حر يكتبه كل مدرس مثل 0.17 أو 0.63.

ابدأ presets governed:

```text
Summative   = full official evidence
Formative   = reduced/capped official evidence
PracticeOnly = no official mastery effect
```

للتجربة الأولى يمكن استخدام factor تقريبي مثل:

```text
Summative = 1.00
Formative = 0.35
PracticeOnly = 0.00
```

لكن هذه الأرقام **ليست final policy** ويجب معايرتها على بيانات حقيقية قبل تثبيتها.

Formula المستقبلية يمكن أن تصبح:

```text
effectiveWeight =
    mappingWeight
  × difficultyWeight
  × recencyWeight
  × evidenceRoleWeight
```

---

# 20. قاعدة مهمة: AI provisional score لا يدخل Evaluation

لا يدخل أي AI/OCR proposed score إلى:

- Official Mastery؛
- Student Evaluation؛
- Teacher Analytics؛
- intervention decisions؛
- reports؛

حتى يصبح:

```text
TeacherApproved / Finalized
```

ولمنع تسريب نتيجة قبل Post Results إلى الطالب من خلال Self Evaluation، يجب أن يكون مسار student-facing official evidence متوافقًا مع Result Release policy.

الخيار الأبسط والأكثر أمانًا في v1:

> Student-facing official evaluation يستهلك فقط النتائج النهائية المنشورة.

---

# 21. State Model المقترح

مع الحفاظ على `AssessmentStatus` القديم للـbackward compatibility، نضيف runtime state مشتقة بدل كسر كل المستهلكين دفعة واحدة.

## Content lifecycle

```text
Draft
→ Questions Reviewed
→ Approved
→ Assigned/Scheduled
```

## Availability lifecycle

```text
NotScheduled
Scheduled
Available
ClosedForSubmission
Cancelled
```

## Grading lifecycle

```text
NoSubmission
Submitted
AutoProcessed
NeedsReview
TeacherApproved
Finalized
```

## Result lifecycle

```text
Withheld
Ready
Published
```

هذه المحاور لا يجب ضغطها كلها داخل enum واحدة.

---

# 22. Permissions

## Teacher / authorized assessment manager

قبل الإتاحة:

- full edit؛
- scheduling؛
- channels؛
- evidence role ضمن school policy؛
- delete.

بعد الإتاحة:

- extend deadline؛
- close؛
- cancel؛
- review submissions؛
- finalize؛
- publish results.

بعد وجود submission:

- لا hard delete؛
- لا silent question mutation؛
- كل تغيير حساس audit event.

## Student

- لا يرى Exam قبل start إذا policy = hidden؛
- لا يمكنه bypass بالرابط المباشر؛
- يرسل فقط في النافذة المسموحة؛
- يرى submission receipt؛
- لا يرى score قبل result release.

---

# 23. Notifications

استخدام Outbox/Event pattern الحالي.

Events المقترحة:

```text
AssessmentScheduled
AssessmentOpened
AssessmentDueSoon
AssessmentClosed
SubmissionReceived
SubmissionNeedsReview
ResultsReady
ResultsPublished
```

يجب أن تكون idempotent لتجنب duplicate notifications.

---

# 24. Time Zone

كل timestamps تُخزن UTC.

الواجهة تعرضها حسب:

```text
SchoolTimeZoneId
```

لا نستخدم local server time.

عند scheduling:

```text
teacher local date/time
→ school timezone
→ UTC
```

ثم عند العرض:

```text
UTC
→ school/user timezone
```

اختبارات DST مطلوبة.

---

# 25. Data Model المقترح

## 25.1 Assessment additions

```text
AssessmentType
AvailableFromUtc?
DueAtUtc?
AttemptTimeLimitMinutes?
AutoOpenEnabled
AutoCloseEnabled
EvidenceRole
ResultReleaseStatus
ResultsPublishedAtUtc?
ResultsPublishedByUserId?
CancelledAtUtc?
CancellationReason?
```

## 25.2 Submission channels

كيان أو mapping:

```text
AssessmentSubmissionChannel
AssessmentId
ChannelType
IsEnabled
```

## 25.3 Student submission

```text
AssessmentSubmission
Id
SchoolId
AssessmentId
StudentProfileId
AttemptNumber
StartedAtUtc?
SubmittedAtUtc?
WasLate
Status
SelectedChannel
CreatedAtUtc
UpdatedAtUtc
RowVersion
```

## 25.4 Submission file

```text
AssessmentSubmissionFile
Id
SubmissionId
StorageKey
OriginalFileName
MimeType
ByteSize
PageCount?
Sha256
UploadStatus
MalwareScanStatus
CreatedAtUtc
```

## 25.5 Extraction

```text
SubmissionExtraction
Id
SubmissionFileId
Provider
ProviderVersion
Status
Confidence
StructuredPayload
CreatedAtUtc
```

## 25.6 Extracted answer

```text
ExtractedSubmissionAnswer
SubmissionId
AssessmentQuestionId
ExtractedText
NormalizedAnswer?
Confidence
PageNumber
BoundingBox?
ProposedScore?
ProposedReason?
```

## 25.7 Review/finalization

```text
SubmissionReview
SubmissionId
AssessmentQuestionId
TeacherUserId
FinalScore
FinalResponseText?
TeacherComment?
ReviewedAtUtc
OverrideReason?
```

---

# 26. Phase Plan

# Phase 0 — Baseline lock + corrective dependency check

## الهدف

منع بناء الميزة فوق افتراضات قديمة.

## المهام

- تثبيت main SHA.
- توثيق current Assessment routes/entities/statuses.
- تثبيت current Online/Offline behavior.
- تثبيت current Student result release behavior.
- تثبيت current Evaluation evidence behavior.
- إنشاء regression snapshot.
- تسجيل P2 الحالي في PracticeAssessmentTaxonomy كـknown external dependency.

## لا تغييرات behavior.

## Exit Gate

نعرف بالاختبارات ماذا يفعل النظام قبل التعديل.

---

# Phase 1 — Domain contracts فقط

## الهدف

إضافة vocabulary بدون تغيير behavior.

## إضافة

```text
AssessmentType
AssessmentEvidenceRole
AssessmentResultReleaseStatus
AssessmentSubmissionChannelType
AssessmentAvailabilityPolicy
LateSubmissionPolicy
```

## Migration

- existing assessments backfill إلى:
  - Type = Exam أو LegacyAssessment مؤقتًا داخليًا إذا احتجنا migration-safe path؛
  - ResultReleaseStatus مبني على current status؛
  - preserve current DeliveryMode.
- لا تغيير في student visibility.

## Exit Gate

كل assessment تاريخي يُقرأ كما كان.

---

# Phase 2 — Assessment Creation UX v2

## الهدف

عند Create Assessment يختار المدرس:

```text
Type
Class/Student target
Subject
Term
Title
Max Score
Submission channels
Evidence role
```

مع defaults واضحة.

## UX

Cards:

```text
Exam / Test
Worksheet
Homework
```

بعد اختيار النوع تظهر الإعدادات المناسبة فقط.

## Exit Gate

إنشاء الأنواع الثلاثة بدون scheduling behavior بعد.

---

# Phase 3 — Scheduling foundation

## الهدف

إضافة:

```text
AvailableFrom
DueAt
Time zone
Auto-open
Auto-close
```

## Server Rules

- timestamps valid؛
- Due > Available؛
- no invalid school timezone؛
- direct URL blocked before availability؛
- no submission after hard due؛
- concurrency safe schedule edits.

## Exit Gate

student access guard يفرض الوقت حتى لو scheduler متوقف.

---

# Phase 4 — Exam/Test Auto Timer

## الهدف

تنفيذ سيناريو 17:00 الحقيقي.

## Features

- scheduled exam hidden before start؛
- auto availability at start؛
- notification؛
- optional global end؛
- optional per-attempt timer؛
- teacher can extend؛
- questions locked عند safe boundary؛
- no hard delete after attempt starts.

## Tests

- exactly before start؛
- exactly at start؛
- after start؛
- due boundary؛
- extended deadline؛
- worker delayed؛
- direct URL attack؛
- two concurrent start requests.

## Exit Gate

Auto Timer آمن ولا يعتمد على scheduler وحده.

---

# Phase 5 — Homework/Worksheet windows + late policy

## الهدف

Start/Due date مثل Teams مع semantics أوضح.

## Features

- AvailableFrom؛
- DueAt؛
- relative date helper؛
- late policy؛
- resubmission policy؛
- teacher extension؛
- due-soon notification.

## Exit Gate

Homework وWorksheet يعملان independently من Exam timer.

---

# Phase 6 — Submission Channels + Hybrid delivery

## الهدف

السماح بـ:

```text
OnlineForm
StudentFileUpload
TeacherRecordedPaper
```

مع multi-channel support.

## Backward compatibility

`AssessmentDeliveryMode` لا يزال يُقرأ للمحتوى القديم.

## Exit Gate

يمكن لـHomework أن يسمح OnlineForm + FileUpload بدون duplication للAssessment.

---

# Phase 7 — Secure PDF/Image upload

## الهدف

إضافة file submission بدون AI أولًا.

## Features

- upload؛
- receipt؛
- versioning؛
- replace before due حسب policy؛
- file validation؛
- malware scan؛
- storage isolation؛
- download authorization؛
- teacher preview؛
- student submission history.

## Exit Gate

paper-to-digital workflow يعمل يدويًا بالكامل قبل إدخال OCR.

---

# Phase 8 — OCR/AI extraction shadow mode

## الهدف

AI يساعد فقط ولا يغيّر grades.

## Pipeline

```text
file
→ normalize
→ OCR
→ question mapping
→ answer extraction
→ confidence
→ proposed grading
```

## Shadow Mode

- teacher يرى proposal؛
- النظام لا يحفظه كـfinal score؛
- نقيس agreement مع teacher.

## Metrics

- extraction success؛
- unreadable rate؛
- question mapping accuracy؛
- teacher override rate؛
- score delta؛
- latency؛
- provider failure.

## Exit Gate

نعرف أين AI موثوق وأين لا.

---

# Phase 9 — Teacher Moderation + Final Grades

## الهدف

إنشاء review workspace.

## Features

- scan vs extracted answer side-by-side؛
- approve/change score؛
- override reason؛
- unreadable flag؛
- request resubmission؛
- finalize one student؛
- finalize class؛
- audit log.

## Exit Gate

لا توجد final score من AI فقط.

---

# Phase 10 — Explicit Post Results

## الهدف

فصل grading عن student visibility.

## Features

```text
Finalize
!=
Post Results
```

- results withheld by default؛
- Post Results button؛
- publish timestamp/user؛
- student result appears only after release؛
- optional bulk publish؛
- no answer-key leakage before release.

## Exit Gate

لا يوجد أي route أو Self Evaluation leak يكشف النتيجة قبل النشر.

---

# Phase 11 — Evaluation / Mastery integration

## الهدف

إدخال الأنواع الجديدة بدون تشويه Student Evaluation.

## Policy

### Exam/Test
```text
Summative default
```

### Homework
```text
PracticeOnly default
```

### Worksheet
```text
Formative when supervised
PracticeOnly when take-home by default
```

## Engine Changes

- EvidenceNormalizer يقرأ EvidenceRole؛
- provisional results excluded؛
- AI-only proposed scores excluded؛
- student-facing evaluation respects result release؛
- Formative evidence has governed weight factor؛
- PracticeOnly displayed separately؛
- reports label source type truthfully.

## Exit Gate

Homework لا يغيّر official mastery لمجرد أنه Assessment entity.

---

# Phase 12 — Analytics, audit, observability

## Metrics

```text
scheduled_open_success
scheduled_open_lag
assessment_access_denied_before_start
late_submission_count
file_upload_failure
ocr_failure
ocr_low_confidence
ai_teacher_agreement
teacher_override_rate
result_publish_delay
result_release_leak_attempt
evaluation_evidence_by_role
```

## Audit events

كل تغيير في:

- schedule؛
- duration؛
- due date؛
- grading؛
- override؛
- finalization؛
- result publication؛
- cancellation.

---

# Phase 13 — Hardening + rollout

## Feature flags

```text
AssessmentTypesV2
AssessmentSchedulingV1
AssessmentFileSubmissionV1
AssessmentAiReviewShadow
AssessmentAiReviewTeacherAssist
AssessmentExplicitResultRelease
AssessmentEvidenceRoles
```

## Rollout

```text
internal test school
→ selected teacher
→ selected class
→ selected assessment type
→ broader school rollout
```

لا global cutover دفعة واحدة.

---

# 27. PR sequence المقترح

```text
PR 1  — contracts + enums + migrations, no behavior change
PR 2  — assessment creation type UX
PR 3  — scheduling fields + access guard
PR 4  — exam auto timer + scheduler/outbox
PR 5  — homework/worksheet due + late policy
PR 6  — submission channel model
PR 7  — secure file upload
PR 8  — teacher file preview/manual review
PR 9  — OCR extraction shadow pipeline
PR 10 — AI grading proposal shadow mode
PR 11 — teacher moderation/finalization
PR 12 — explicit Post Results
PR 13 — evaluation evidence-role integration
PR 14 — analytics/observability/security hardening
PR 15 — production rollout gates
```

كل PR يجب أن يكون independently mergeable.

---

# 28. Compatibility مع Assessment Builder الحالي

لا نكسر:

```text
Select
Generate
Review
Approve
Publish
```

لكن نوضح أن كلمة `Publish` الحالية يجب ألا تختلط مستقبلًا مع:

```text
Post Results
```

التسمية المقترحة في UX:

```text
Assign / Schedule Assessment
Post Results
```

بدل استخدام كلمة Publish لنفس شيئين مختلفين.

---

# 29. Compatibility مع Generated Exam Engine

لا نحول ExamGenerationEngine إلى scheduling engine.

يبقى مسؤولا عن:

- generated batch validation؛
- materialization؛
- question identity؛
- review/approval lifecycle.

Scheduling يكون subsystem منفصل فوق Assessment delivery.

---

# 30. Acceptance Matrix

| Scenario | Before start | During window | After due | Result before Post | Result after Post |
|---|---|---|---|---|---|
| Online Exam/Test | Hidden/blocked | Take exam | Block | Hidden | Visible |
| Online Homework | Hidden أو scheduled حسب policy | Submit | Late/block حسب policy | Hidden | Visible |
| Online Worksheet | Hidden أو scheduled | Submit | Late/block | Hidden | Visible |
| Paper Homework + Upload | No upload | Upload scan | late/block | Hidden | Visible |
| Paper Worksheet + Upload | No upload | Upload scan | late/block | Hidden | Visible |
| Offline Exam + teacher record | Existing paper workflow | teacher records/imports | closed by teacher | Hidden إذا manual release | Visible |

---

# 31. الاختبارات الإلزامية

## Domain

- enum round-trip؛
- migration backfill؛
- legacy assessment compatibility.

## Scheduling

- UTC boundaries؛
- school timezone؛
- DST؛
- worker outage؛
- clock boundary؛
- concurrent open/submit؛
- deadline extension.

## Authorization

- cross-school file access forbidden؛
- student cannot access other student submission؛
- crafted assessment ID blocked؛
- teacher scope enforced.

## Files

- oversized file؛
- fake extension؛
- malformed PDF؛
- active content؛
- malware status؛
- duplicate upload؛
- hash integrity.

## AI

- low confidence forced review؛
- unsupported math never silently graded؛
- AI provider timeout؛
- disagreement with verifier؛
- no provisional score in official evaluation.

## Result Release

- submitted but withheld؛
- finalized but withheld؛
- published؛
- self-evaluation leak regression؛
- API direct access regression.

## Evaluation

- Summative counts؛
- Formative reduced weight؛
- PracticeOnly excluded from official mastery؛
- student still sees formative progress separately؛
- historical evaluation remains reconstructable.

---

# 32. المخاطر الرئيسية

## Risk 1 — استخدام Status واحدة لكل شيء

الحل:
فصل content, availability, grading, result release.

## Risk 2 — scheduler downtime

الحل:
request-time access guard هو authority.

## Risk 3 — Homework يفسد mastery

الحل:
EvidenceRole صريح وdefault = PracticeOnly.

## Risk 4 — AI يخطئ في handwriting

الحل:
confidence + teacher review + no direct final grade.

## Risk 5 — الطالب يرى نتيجة قبل المدرس

الحل:
ResultReleaseStatus مستقل + leak tests.

## Risk 6 — تغيير الأسئلة بعد بدء الطلاب

الحل:
content lock/versioning/audit.

## Risk 7 — PDF يفتح attack surface

الحل:
strict validation + malware scanning + sandboxed processing + private storage.

## Risk 8 — خلط Exam/Test كنوعين مستقلين

الحل:
نوع Canonical واحد، UI alias فقط.

---

# 33. قرارات Product المقترحة لاعتمادها قبل التنفيذ

1. الأنواع canonical = Exam, Worksheet, Homework.
2. Test = label/subtype لـExam وليس entity type رابع.
3. Type مستقل عن submission channel.
4. Type مستقل عن EvidenceRole.
5. Exam online يدعم scheduled start + hard access guard.
6. Homework/Worksheet يدعمان AvailableFrom + DueAt.
7. multiple submission channels مسموحة للHomework/Worksheet.
8. file upload يبنى أولًا بدون AI ثم AI shadow mode.
9. AI لا يعتمد grade رسميًا.
10. teacher finalization إلزامي للـscan/AI flow.
11. Post Results مستقل عن Close/Finalize.
12. Homework default = PracticeOnly.
13. Worksheet default = Formative فقط عندما policy/supervision تسمح؛ وإلا PracticeOnly.
14. Exam/Test default = Summative.
15. Student Self Evaluation يعرض Homework/Worksheet progress حتى عندما لا تدخل official mastery، لكن في section منفصل.
16. current Assessment Builder generation/review/approval workflow محفوظ.
17. current historical assessments/results/mastery remain reconstructable.

---

# 34. Definition of Done

الميزة لا تعتبر مكتملة حتى:

- [ ] Exam/Test schedule لا يمكن تجاوزه بالرابط المباشر.
- [ ] start time يعمل حتى لو background worker متأخر.
- [ ] due/late semantics واضحة.
- [ ] Homework/Worksheet يملكان online/file/paper channels المناسبة.
- [ ] student can upload PDF/image securely.
- [ ] OCR extraction traceable.
- [ ] low-confidence answers require review.
- [ ] AI proposal never becomes official score alone.
- [ ] teacher can finalize.
- [ ] teacher must explicitly Post Results في manual mode.
- [ ] student cannot infer result before release عبر أي صفحة أو API.
- [ ] Homework PracticeOnly لا يؤثر official mastery.
- [ ] Worksheet evidence role is explicit.
- [ ] Exam/Test Summative evidence enters evaluation only under finalized policy.
- [ ] current Assessment Builder remains operational.
- [ ] current Offline XLSX import remains operational.
- [ ] current historical data remains readable.
- [ ] audit trail covers schedule/grade/release changes.
- [ ] security/performance tests pass.
- [ ] production rollout is feature-flagged.

---

# 35. القرار النهائي المقترح حول Evaluation

**لا أوافق على إدخال Homework وWorksheet تلقائيًا في تقييم الطالب الرسمي.**

التصميم الأقوى هو:

```text
Exam/Test
→ Official Summative Evidence

Supervised Worksheet
→ Controlled Formative Evidence

Take-home Worksheet
→ PracticeOnly by default

Homework
→ PracticeOnly by default
```

وفي نفس الوقت لا نخفي Homework/Worksheet من الطالب.

بل نعرضها في Self Evaluation كدليل تعلم منفصل:

```text
Official Mastery
Homework / Formative Progress
Private Practice
Assessment Transfer
```

وبذلك نحصل على أفضل شيء من الاتجاهين:

- لا نلوّث mastery الرسمي بدليل قد لا يكون مستقلًا؛
- ولا نفقد قيمة Homework/Worksheet في قياس التطور والممارسة.

---

# 36. مبدأ التنفيذ

المبدأ النهائي:

```text
Assessment Type
!=
Delivery Method
!=
Submission Method
!=
Evidence Strength
!=
Grading State
!=
Result Visibility
```

كل واحد منها يجب أن يكون قرارًا واضحًا ومستقلًا.

هذا هو الأساس الذي يمنع Assessment من التحول إلى مجموعة استثناءات صعبة الصيانة عندما نضيف Homework وWorksheet وAI-assisted paper grading.
