# Edulytics — خطة تطوير Assessment: Exam/Test + Homework + Worksheet

**حالة الوثيقة:** خطة تحليل وتنفيذ فقط — لا تغيّر أي سلوك Production  
**Repository:** `EIAS79/Edulytics`  
**Baseline branch:** `main`  
**Baseline SHA:** `d7f42477e76a59f5624cf53cb54f755ec2013c04`  
**تاريخ الخطة:** 2026-09-30  
**اللغة:** العربية  

## الهدف

توسيع نظام Assessment الحالي في Edulytics ليخدم ثلاثة أنواع واضحة:

```text
1. Exam / Test
2. Homework
3. Worksheet
```

مع إعادة استخدام نفس طريقة إنشاء الأسئلة الموجودة في Assessment Builder الحالي:

```text
Manual creation
OR
Edulytics generation / AI generation
→ Review
→ Edit
→ Approve
→ Publish / Assign
```

مع تثبيت القواعد التالية كقرارات Product أساسية:

```text
Exam/Test:
- له درجات
- يدخل Evaluation
- Online أو Offline
- يمكن أن يملك Start Time / Auto Timer

Homework:
- بلا درجات
- لا يدخل Evaluation
- Online فقط
- له Due / Return time
- بلا Exam Timer

Worksheet:
- بلا درجات
- لا يدخل Evaluation
- Online أو Offline
- بلا Due / Return time
- الطالب يستطيع حله في أي وقت أثناء توفره
```

رفع PDF **ليس طريقة لإنشاء الأسئلة أو المهمة** في هذا النطاق.

---

# 1. Scope Lock

هذه المرحلة تخطيط فقط.

لا يتم الآن:

- تعديل Entity؛
- إنشاء Migration؛
- تغيير Assessment Builder؛
- تغيير Evaluation؛
- تغيير Student Portal؛
- تغيير Production behavior.

الغرض هو تثبيت المتطلبات قبل التنفيذ.

---

# 2. Baseline الحقيقي من الكود الحالي

## 2.1 آخر حالة main

الـbaseline المستخدم:

```text
d7f42477e76a59f5624cf53cb54f755ec2013c04

Merge PR #323:
complete Adaptive V2 learner flow and difficulty progression
```

أحدث العمل قبل هذه الخطة كان متعلقًا بـAdaptive V2، بما في ذلك:

- review-state للطالب؛
- voice ordering؛
- historical Question Log metadata؛
- difficulty progression؛
- accessibility semantics؛
- Place Value / Rounding variant integrity.

## 2.2 Review finding قائم

وقت إعداد الخطة يوجد P2 حديث متعلق بـ:

```text
PracticeAssessmentTaxonomy
```

حيث توجد بعض Challenge slots whose stored `questionForm` metadata لا تطابق الشكل الفعلي للسؤال.

هذا ليس جزءًا من Homework/Worksheet feature، لكنه يجب ألا يُنسى إذا تم استخدام Question Form metadata لاحقًا في analytics أو coverage.

---

# 3. Assessment الحالي في Edulytics

الـAssessment الحالي يعرف:

```text
AssessmentStatus
- Draft
- Open
- Closed
```

ويعرف:

```text
AssessmentDeliveryMode
- Offline
- Online
```

ويعرف:

```text
AssessmentTargetType
- Class
- Student
```

ويعرف:

```text
AssessmentDifficultyBand
```

لكنه **لا يعرف حاليًا نوع المهمة**:

```text
Exam
Homework
Worksheet
```

والـAssessment entity الحالي يحتوي على:

- Title؛
- AssessmentDate؛
- MaxScore؛
- Status؛
- Class؛
- Subject؛
- Term؛
- Target؛
- DeliveryMode؛
- Difficulty.

ولا يحتوي حاليًا على domain semantics واضحة لـ:

- AssessmentType؛
- scheduled start؛
- Homework due time؛
- Worksheet timeless availability.

---

# 4. Assessment Builder الحالي يجب إعادة استخدامه

الـworkflow الحالي:

```text
Select
→ Generate
→ Review
→ Approve
→ Publish
```

ويدعم بالفعل:

- Manual question creation؛
- generated questions؛
- regenerate؛
- edit؛
- approve؛
- approve all؛
- delete؛
- lesson/outcome scoping؛
- difficulty؛
- Online/Offline Assessment؛
- Offline PDF output؛
- student Online Assessment delivery.

القرار:

> لا نبني Homework Builder وWorksheet Builder كمحركي أسئلة مستقلين.

بل نستخدم نفس Core الخاص بالـAssessment Builder، مع اختلاف الـshell والقواعد حسب النوع.

---

# 5. الأنواع الثلاثة

## 5.1 Exam / Test

`Test` ليس نوعًا رابعًا.

```text
Canonical Type = Exam
UI label = Exam / Test
```

الـExam/Test هو الامتداد الطبيعي للـAssessment الرسمي الحالي.

### خصائصه

- scored؛
- له MaxScore؛
- لكل سؤال marks؛
- يدخل Evaluation؛
- يمكن أن يكون Online؛
- يمكن أن يكون Offline؛
- يمكن أن يملك Start Time؛
- يمكن أن يملك end window أو duration؛
- يمكن أن يستخدم result-release policy.

---

## 5.2 Homework

Homework يشبه الـOnline Assessment الحالي في طريقة إنشاء الأسئلة وتجربة الحل، لكن **ليس اختبارًا مقيمًا بالدرجات**.

### القواعد الثابتة

```text
Delivery = Online only
Marks = None
Evaluation = Excluded
DueAt = Required
Exam timer = None
```

### تجربة المدرس

المدرس:

1. يختار Homework.
2. يحدد الفصل/الطالب والمادة والنطاق.
3. ينشئ الأسئلة:
   - يدويًا؛ أو
   - Generate من Edulytics.
4. يراجع ويعدل الأسئلة.
5. يعتمدها.
6. يحدد Due / Return date and time.
7. ينشر Homework.

### تجربة الطالب

الطالب:

1. يرى Homework بعد نشرها.
2. يفتحها داخل Edulytics.
3. يجيب Online.
4. يمكنه العودة إليها وفق save/progress policy.
5. يضغط Submit قبل DueAt.
6. يرى حالة Submitted.

### غير موجود

لا يوجد:

- MaxScore؛
- marks؛
- percentage؛
- grade؛
- official result؛
- Evaluation evidence؛
- Exam countdown timer؛
- Offline mode toggle.

---

## 5.3 Worksheet

Worksheet تستخدم نفس أسلوب بناء الأسئلة، لكنها نشاط مفتوح بدون موعد تسليم.

### القواعد الثابتة

```text
Delivery = Online OR Offline
Marks = None
Evaluation = Excluded
DueAt = None
Exam timer = None
```

### Online Worksheet

- تُحل داخل Edulytics؛
- لا يوجد deadline؛
- لا يوجد marks؛
- يمكن تتبع completion فقط؛
- تظل متاحة ما دام المدرس لم يسحبها أو يؤرشفها.

### Offline Worksheet

الأسئلة:

```text
Created inside Edulytics
→ Reviewed
→ Approved
→ Rendered as printable worksheet/PDF
```

أي أن PDF هنا **Output** من Edulytics، وليس Input لإنشاء Worksheet.

لا يوجد:

- result import؛
- score؛
- percentage؛
- Evaluation.

---

# 6. قاعدة إنشاء الأسئلة

بالنسبة للأنواع الثلاثة:

```text
Teacher selects:
Class
Subject
Lesson / Outcome / Scope

Then:

Manual Question Creation
OR
Edulytics Generate / AI-assisted generation

Then:

Review
Edit
Regenerate if needed
Approve
Publish / Assign
```

## ممنوع في هذا scope

لا يكون workflow الأساسي:

```text
Upload PDF
→ Edulytics reads PDF
→ creates Homework/Worksheet
```

هذا ليس المطلوب.

---

# 7. Domain Model المقترح

إضافة:

```csharp
public enum AssessmentType
{
    Exam = 1,
    Homework = 2,
    Worksheet = 3
}
```

## قواعد الـdomain

### Exam

```text
AllowsMarks = true
CountsTowardEvaluation = true
AllowedDelivery = Online | Offline
AllowsDueWindow = true
AllowsAttemptTimer = true
```

### Homework

```text
AllowsMarks = false
CountsTowardEvaluation = false
AllowedDelivery = Online only
RequiresDueAt = true
AllowsAttemptTimer = false
```

### Worksheet

```text
AllowsMarks = false
CountsTowardEvaluation = false
AllowedDelivery = Online | Offline
RequiresDueAt = false
AllowsAttemptTimer = false
```

هذه القواعد يجب أن تكون enforced في Service/Domain layer وليس CSS أو UI فقط.

---

# 8. Marks model

## Exam/Test

يستمر current model:

```text
Assessment.MaxScore
AssessmentQuestion.MaxScore
AssessmentResult.Score
AssessmentResult.Percentage
StudentAnswer.Score
```

## Homework وWorksheet

يجب ألا يعاملا كـzero-score Exam.

أي لا يكون الحل:

```text
MaxScore = 0
```

ثم نترك بقية scoring pipeline تعمل.

الأصح أن contracts الخاصة بـHomework/Worksheet لا تعرض score semantics من الأصل.

لو بقيت legacy columns في جدول Assessment أثناء migration، تكون compatibility detail داخل persistence فقط.

الـbusiness logic لا يسمح بmarks للنوعين.

---

# 9. Evaluation policy — قرار نهائي

هذه النقطة ليست configurable في النسخة الحالية.

```text
Exam/Test
→ Included in Evaluation

Homework
→ Never included in Evaluation

Worksheet
→ Never included in Evaluation
```

## Homework وWorksheet لا يظهران في Evaluate

لا يدخلان:

- Official Mastery؛
- Assessment Mastery؛
- Current Mastery؛
- confidence؛
- Evaluation trend؛
- intervention calculation؛
- Student Self Evaluation؛
- Teacher Evaluation؛
- Supervisor Evaluation؛
- evaluation PDFs/reports.

ولا يظهران حتى كـ"Formative section" داخل Evaluate.

إذا أردنا مستقبلًا عرض Homework/Worksheet activity، يكون في:

```text
Activity / Assignments / Completion analytics
```

وليس داخل Evaluation.

---

# 10. التغيير المطلوب في Evaluation Engine

حاليًا Assessment evidence يمكن أن يدخل عندما Assessment ليس Draft.

بعد إضافة `AssessmentType` يجب أن تكون القاعدة صريحة:

```text
Only AssessmentType.Exam
can generate official Assessment evaluation evidence.
```

أي أن:

```csharp
if (assessment.AssessmentType != AssessmentType.Exam)
{
    continue;
}
```

يجب أن يكون جزءًا من normalization/business policy.

ولا نعتمد على:

- MaxScore = 0؛
- lack of results؛
- UI hiding؛

لمنع Homework/Worksheet من التقييم.

---

# 11. Exam Auto Timer

الـAuto Timer يخص **Online Exam/Test**.

مثال:

```text
Teacher creates Exam at 14:00
Start = 17:00
End = 18:00
```

قبل 17:00:

- الطالب لا يستطيع الدخول؛
- direct URL يجب أن يُرفض؛
- لا Attempt؛
- لا Submit؛
- المدرس يستطيع التعديل قبل safe lock boundary.

عند 17:00:

- Exam تصبح available تلقائيًا؛
- تظهر للطلاب؛
- يمكن إرسال notification.

## Security rule

لا نعتمد على background scheduler فقط.

يجب وجود server-side access guard:

```text
CanStudentAccessExam(
    nowUtc,
    exam,
    student,
    attempt
)
```

حتى لو worker متأخر، لا يمكن bypass للوقت.

---

# 12. Exam scheduling fields

للـExam/Test:

```text
AvailableFromUtc?
DueAtUtc?
AttemptTimeLimitMinutes?
AutoOpenEnabled
AutoCloseEnabled
```

يمكن دعم:

### Global window

```text
17:00 → 18:00
```

### Per-attempt duration

مثال:

```text
Student starts 17:10
Duration = 45 min
```

مع hard close policy عند الحاجة.

---

# 13. Homework Due / Return Time

Homework لا تملك Exam timer.

لها:

```text
PublishedAtUtc
DueAtUtc
```

مثال:

```text
Published: 1 October 08:00
Due: 3 October 18:00
```

الطالب يستطيع العمل عليها خلال الفترة.

## v1 policy

النسخة الأولى:

```text
now <= DueAtUtc
→ Submit allowed

now > DueAtUtc
→ Submit blocked
```

Late submission يمكن إضافته مستقبلًا كسياسة منفصلة إذا تقرر ذلك.

## Teacher

قبل DueAt يستطيع:

- تعديل DueAt؛
- تمديد DueAt؛
- سحب Homework إذا لم تعد مطلوبة.

تغييرات مهمة بعد submissions يجب أن تكون audited.

---

# 14. Worksheet بلا Return Time

Worksheet لا تملك:

```text
DueAtUtc
ReturnDate
LateSubmission
Countdown
```

بعد نشرها:

```text
Published
→ Available
→ Student solves whenever
```

وتظل كذلك حتى يقوم المدرس بـ:

- Withdraw؛ أو
- Archive.

هذا قرار إداري وليس deadline.

---

# 15. Delivery rules

## Exam

```text
Online
Offline
```

نستمر باستخدام current `AssessmentDeliveryMode`.

## Homework

```text
Online only
```

لا يظهر للمدرس Online/Offline selector.

## Worksheet

```text
Online
Offline
```

Online = interactive inside Edulytics.

Offline = printable worksheet generated من الأسئلة التي بُنيت في Edulytics.

---

# 16. Student state model

## Exam

```text
Unavailable
Available
InProgress
Submitted
Closed
ResultWithheld
ResultPublished
```

## Homework

```text
NotStarted
InProgress
Submitted
MissedDueDate
```

لا Grade state.

## Worksheet Online

```text
NotStarted
InProgress
Completed
```

## Worksheet Offline

يمكن أن يكون:

```text
AvailableForDownload
```

ولا يلزم وجود completion record في v1 إلا إذا قررنا أن الطالب يضغط Mark as completed.

---

# 17. Result Release

## Exam/Test

يمكن تنفيذ الفصل بين:

```text
Close Exam
!=
Post Results
```

حتى يعتمد المدرس النتائج قبل أن يراها الطلاب.

## Homework

لا يوجد Post Results رقمي لأن لا توجد marks.

يمكن مستقبلًا إضافة:

- teacher comment؛
- reviewed/not reviewed؛

لكن بدون grade.

## Worksheet

لا يوجد Result Release.

---

# 18. PDF policy

## PDF كـInput لإنشاء المهمة

غير مطلوب.

```text
NO:
Upload teacher PDF
→ parse it
→ create Homework/Worksheet questions
```

## PDF كـOutput

مطلوب للـOffline Worksheet:

```text
Edulytics-authored questions
→ Printable Worksheet PDF
```

وCurrent Offline Exam PDF workflow يبقى كما هو.

## Student solved-PDF upload / OCR

ليس جزءًا من Homework/Worksheet core scope في النسخة المعدلة من الخطة.

لأن:

- Homework Online only؛
- Worksheet غير graded؛
- ولا نحتاج AI grading لهما.

إذا تقرر مستقبلًا OCR لـOffline Exam، يكون Phase منفصلة تخص Exam فقط.

---

# 19. AI role

في هذا scope، AI دوره في Homework/Worksheet هو **إنشاء الأسئلة والمساعدة في authoring**، وليس إعطاء درجات.

يسمح:

```text
Generate questions
Regenerate
Suggest variants
Use lesson/outcome scope
```

ثم teacher review/approval.

لا يوجد:

```text
Homework AI score
Worksheet AI score
Homework grading AI
Worksheet grading AI
```

---

# 20. Teacher creation UX

صفحة Create Assessment الجديدة تعرض 3 cards:

```text
Exam / Test
Homework
Worksheet
```

## Exam selected

تظهر:

- class/target؛
- subject/term؛
- title؛
- marks؛
- online/offline؛
- difficulty؛
- optional schedule؛
- timer/end settings.

## Homework selected

تظهر:

- class/target؛
- subject/term؛
- title؛
- Due date/time.

ولا تظهر:

- MaxScore؛
- marks؛
- Online/Offline؛
- Evaluation setting؛
- attempt timer.

## Worksheet selected

تظهر:

- class/target؛
- subject/term؛
- title؛
- Online/Offline.

ولا تظهر:

- MaxScore؛
- marks؛
- Due date؛
- Evaluation setting؛
- timer.

---

# 21. Shared Builder UX

بعد إنشاء shell الأساسي، تدخل الأنواع الثلاثة إلى Builder المشترك.

المكونات المشتركة:

- learning scope؛
- lesson selection؛
- unit/outcome selection؛
- manual question؛
- generated questions؛
- regenerate؛
- edit؛
- approve؛
- approve all؛
- ordering؛
- preview.

## Type-specific adaptation

### Exam

يعرض:

- question marks؛
- total marks؛
- difficulty/evaluation semantics.

### Homework

يعرض question content فقط.

### Worksheet

يعرض question content فقط.

---

# 22. Data Model المقترح

## Assessment

إضافة:

```text
AssessmentType
```

### Exam fields

```text
MaxScore
AvailableFromUtc?
DueAtUtc?
AttemptTimeLimitMinutes?
AutoOpenEnabled
AutoCloseEnabled
```

### Homework fields

```text
DueAtUtc
```

### Worksheet fields

```text
DeliveryMode
```

## Activity records

Homework/Worksheet لا يجب أن تستخدم `AssessmentResult` كأنها درجات.

نحتاج activity/submission record مثل:

```text
LearningTaskAttempt
- Id
- SchoolId
- AssessmentId
- StudentProfileId
- StartedAtUtc?
- SubmittedAtUtc?   // Homework
- CompletedAtUtc?   // Worksheet Online
- Status
- UpdatedAtUtc
```

هذه البيانات operational وليست Evaluation evidence.

---

# 23. Migration strategy

الـAssessments التاريخية الحالية تمثل النظام الرسمي scored Assessment.

لذلك migration المقترحة:

```text
Existing Assessment rows
→ AssessmentType = Exam
```

وبذلك:

- current results remain valid؛
- current mastery remains reconstructable؛
- current Student Portal behavior لا يفقد semantics؛
- Offline/Online assessments القديمة تظل Exams.

لا نحاول infer Homework/Worksheet من البيانات التاريخية لأنه لا يوجد أساس موثوق لذلك.

---

# 24. Permissions

## Teacher

مشترك:

- create؛
- generate؛
- edit؛
- approve؛
- publish؛
- withdraw/archive وفق policy.

### Exam

- scores؛
- delivery؛
- schedule؛
- close؛
- results؛
- post results.

### Homework

- set/extend DueAt؛
- view student submission status؛
- no grade controls.

### Worksheet

- choose Online/Offline؛
- publish/archive؛
- printable action للOffline؛
- no due controls؛
- no grade controls.

## Student

### Exam

- time-gated access؛
- submit؛
- result visibility حسب policy.

### Homework

- Online solve؛
- submit before DueAt؛
- no score.

### Worksheet

- solve Online whenever available؛
- أو download/print Offline؛
- no due؛
- no score.

---

# 25. Notifications

استخدام Outbox/Event pattern الحالي.

## Exam

```text
ExamScheduled
ExamOpened
ExamClosed
ExamResultsPublished
```

## Homework

```text
HomeworkPublished
HomeworkDueSoon
HomeworkSubmitted
HomeworkMissedDue
```

## Worksheet

```text
WorksheetPublished
```

لا يوجد WorksheetDueSoon.

---

# 26. Phase Plan

## Phase 0 — Baseline Lock

الهدف:

- تثبيت current Assessment behavior؛
- تثبيت Builder contracts؛
- تثبيت Evaluation behavior؛
- إضافة regression coverage قبل التغيير.

لا behavior change.

---

## Phase 1 — AssessmentType Contract

إضافة:

```text
AssessmentType:
Exam
Homework
Worksheet
```

Migration:

```text
Existing rows → Exam
```

Acceptance:

- جميع الـAssessment الحالية تعمل كما قبل.

---

## Phase 2 — Type-aware Create UX

إضافة cards:

```text
Exam / Test
Homework
Worksheet
```

والـform يصبح type-specific.

Acceptance:

- Homework لا يعرض marks/delivery toggle؛
- Worksheet لا يعرض marks/due؛
- Exam يحتفظ بالحقول الحالية.

---

## Phase 3 — Shared Builder Refactor

الهدف:

إعادة استخدام نفس question-authoring/generation system للثلاثة.

```text
Manual
Generate
Regenerate
Edit
Review
Approve
```

Acceptance:

- لا يوجد duplicate generation engine؛
- Homework/Worksheet لا ينشئان scored questions.

---

## Phase 4 — Exam Scheduling / Auto Timer

إضافة:

- AvailableFrom؛
- End/Due window؛
- Attempt duration عند الحاجة؛
- server-side access guard؛
- scheduler/outbox notifications؛
- teacher extension؛
- concurrency tests.

Acceptance:

- لا يمكن للطالب الدخول قبل start حتى بالرابط المباشر.

---

## Phase 5 — Homework Online Workflow

إضافة:

- Online-only rendering؛
- DueAt required؛
- draft/progress save؛
- Submit؛
- server-side deadline check؛
- teacher submission status view؛
- student status view.

لا marks.

لا Evaluation.

Acceptance:

```text
Homework cannot create AssessmentResult
Homework cannot reach EvaluationEvidenceNormalizer
```

---

## Phase 6 — Worksheet Workflow

### Online

- interactive question rendering؛
- no due؛
- no score؛
- progress/completion state.

### Offline

- printable PDF generated من system-authored questions؛
- no score؛
- no result import؛
- no deadline.

Acceptance:

- Worksheet can never save a numeric grade.

---

## Phase 7 — Evaluation Hard Exclusion

تعديل:

- `EvaluationEvidenceNormalizer`؛
- `MasteryEvidenceEngine`؛
- Student Self Evaluation source selection؛
- staff Evaluation source selection.

القاعدة:

```text
Only AssessmentType.Exam
is official assessment evidence.
```

Acceptance:

- Homework/Worksheet = zero Evaluation evidence by design.

---

## Phase 8 — Student Portal Separation

واجهة الطالب تفصل بوضوح:

```text
Exams / Tests
Homework
Worksheets
```

Exam card:
- date/time؛
- status؛
- result status.

Homework card:
- due time؛
- submission status.

Worksheet card:
- online/offline؛
- availability/completion.

---

## Phase 9 — Teacher Management UX

إضافة:

- filters by type؛
- badges؛
- type-specific actions؛
- Homework due management؛
- Worksheet print/download؛
- removal of irrelevant score actions.

---

## Phase 10 — Exam Result Release

للـExam/Test فقط:

```text
Close / Finalize
→ Post Results
```

Homework/Worksheet لا تدخل هذا المسار.

---

## Phase 11 — Audit / Observability

Metrics:

```text
exam_scheduled_open_success
exam_access_denied_before_start
homework_submitted_before_due
homework_missed_due
worksheet_online_completed
worksheet_pdf_generated
non_exam_evaluation_evidence_blocked
```

Audit:

- Exam schedule changes؛
- Homework DueAt changes؛
- publish/withdraw/archive actions.

---

## Phase 12 — Hardening + Rollout

Feature flags:

```text
AssessmentTypesV2
ExamSchedulingV1
HomeworkV1
WorksheetV1
ExamExplicitResultRelease
```

Rollout:

```text
internal
→ selected teacher
→ selected class
→ selected school
→ wider rollout
```

---

# 27. PR Sequence

```text
PR 1  — AssessmentType contracts + migration
PR 2  — type-aware Create UX
PR 3  — shared Builder refactor
PR 4  — Exam Auto Timer / scheduling
PR 5  — Homework online unscored workflow + DueAt
PR 6  — Worksheet online/offline unscored workflow
PR 7  — Worksheet printable PDF output
PR 8  — Evaluation hard exclusion for non-Exam types
PR 9  — Student Portal separation
PR 10 — Teacher management UX
PR 11 — Exam-only explicit result release
PR 12 — audit, regression, hardening, rollout
```

كل PR يجب أن يكون independently mergeable.

---

# 28. Acceptance Matrix

| النوع | إنشاء الأسئلة | Delivery | Marks | Due/Return | Timer | Evaluation |
|---|---|---|---|---|---|---|
| Exam/Test | Manual أو Generate داخل Edulytics | Online / Offline | نعم | Exam window حسب الإعداد | نعم عند الحاجة | نعم |
| Homework | Manual أو Generate داخل Edulytics | Online فقط | لا | نعم، إلزامي | لا | لا |
| Worksheet | Manual أو Generate داخل Edulytics | Online / Offline | لا | لا | لا | لا |

---

# 29. Mandatory Tests

## Domain

- Homework rejects marks.
- Worksheet rejects marks.
- Homework requires DueAt.
- Homework rejects Offline.
- Worksheet accepts Online/Offline.
- Worksheet rejects DueAt semantics.
- Exam preserves current scoring.

## Builder

- all types can use manual question creation.
- all types can use Edulytics generation.
- review/approve flow works.
- no PDF input is needed for authoring.

## Exam Scheduling

- before-start blocked.
- exact-start allowed.
- after-close blocked.
- direct URL blocked.
- worker delay does not bypass access guard.
- teacher extension safe.

## Homework

- before due submit allowed.
- after due submit blocked in v1.
- no result/score created.
- no evaluation evidence created.

## Worksheet

- no due validation path.
- Online works without deadline.
- Offline PDF renders.
- no result/score created.
- no evaluation evidence created.

## Evaluation

- historical/current Exam evidence still works.
- Homework excluded.
- Worksheet excluded.
- Student Self Evaluation excludes both.
- staff Evaluation excludes both.
- reports/PDFs exclude both as evaluation evidence.

---

# 30. Main Risks

## Risk 1 — Reusing Assessment entity accidentally sends Homework/Worksheet into Evaluation

**Solution:** explicit `AssessmentType.Exam` filtering in normalization.

## Risk 2 — UI hides marks but backend accepts them

**Solution:** domain/service validation.

## Risk 3 — Worksheet accidentally inherits DueAt from Homework

**Solution:** type-specific contracts/view models and validation.

## Risk 4 — Homework accidentally exposes Offline toggle

**Solution:** Homework delivery is fixed Online in the domain.

## Risk 5 — Three separate Builders diverge

**Solution:** shared Builder core.

## Risk 6 — PDF becomes authoring dependency

**Solution:** PDF is only an output for Offline Worksheet in this scope.

## Risk 7 — Exam scheduler downtime

**Solution:** request-time access guard is authoritative.

---

# 31. Product Decisions Locked by This Revision

1. Canonical types are Exam, Homework, Worksheet.
2. Test is a display label/subtype of Exam.
3. Questions for all types are created inside Edulytics.
4. Creation can be manual or generated by Edulytics/AI.
5. No PDF upload is required to create questions.
6. Exam/Test is scored.
7. Exam/Test enters Evaluation.
8. Exam/Test can be Online or Offline.
9. Online Exam can use scheduled start / timer.
10. Homework is Online only.
11. Homework has no marks.
12. Homework never enters Evaluation.
13. Homework requires Due/Return date and time.
14. Homework has no Exam attempt timer.
15. Worksheet can be Online or Offline.
16. Worksheet has no marks.
17. Worksheet never enters Evaluation.
18. Worksheet has no Due/Return date.
19. Online Worksheet can be solved whenever it remains available.
20. Offline Worksheet is generated/printed from questions authored inside Edulytics.
21. Homework/Worksheet do not appear inside Evaluate, including Student Self Evaluation.
22. No Formative weighting is used for Homework/Worksheet.
23. No AI grading workflow is needed for Homework/Worksheet because they are unscored.
24. Current Assessment Builder generation/review/approval concepts remain the foundation.

---

# 32. Definition of Done

- [ ] Teacher can choose Exam/Test, Homework, or Worksheet.
- [ ] All three reuse the current question-authoring/generation architecture.
- [ ] Exam/Test preserves current scoring and official Evaluation.
- [ ] Exam scheduled access cannot be bypassed.
- [ ] Homework is Online only.
- [ ] Homework has required DueAt.
- [ ] Homework has no score controls in UI or service contracts.
- [ ] Homework cannot produce AssessmentResult/official Evaluation evidence.
- [ ] Worksheet supports Online and Offline.
- [ ] Worksheet has no DueAt.
- [ ] Worksheet has no score controls.
- [ ] Worksheet cannot produce official Evaluation evidence.
- [ ] Online Worksheet can remain available without deadline.
- [ ] Offline Worksheet PDF is generated from Edulytics-authored questions.
- [ ] Student Portal clearly separates Exams, Homework, and Worksheets.
- [ ] Student Evaluate contains no Homework/Worksheet evidence.
- [ ] Teacher/Supervisor Evaluation contains no Homework/Worksheet evidence.
- [ ] Historical Assessment data remains valid.
- [ ] Current Online/Offline Exam behavior remains operational.
- [ ] Full regression suite passes.

---

# 33. Final Behavioral Contract

```text
EXAM / TEST
------------------------------------------------
Question creation: Edulytics Manual / Generate
Delivery: Online or Offline
Marks: YES
Evaluation: YES
Scheduled Start: Optional
Timer: Optional
Result: Numeric / official


HOMEWORK
------------------------------------------------
Question creation: Edulytics Manual / Generate
Delivery: Online only
Marks: NO
Evaluation: NO
Due / Return time: YES
Exam Timer: NO
Result: Submission state only


WORKSHEET
------------------------------------------------
Question creation: Edulytics Manual / Generate
Delivery: Online or Offline
Marks: NO
Evaluation: NO
Due / Return time: NO
Exam Timer: NO
Result: Completion/use state only
```

هذا هو الـcontract الذي يجب أن يحكم الـUI والـServices والـPersistence والـEvaluation، وليس مجرد اختلاف شكلي في صفحة الإنشاء.
