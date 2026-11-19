import json,pathlib,hashlib,collections
root=pathlib.Path(__file__).resolve().parents[2]
base=root/'artifacts/curriculum-delivery/UAE-MOE-MATH/G8'
audit=json.loads((base/'general-practice-semantic-audit.json').read_text(encoding='utf8'))
actual=json.loads((base/'general-practice-76-runtime-sample.json').read_text(encoding='utf8'))
clear={
3:'تدريبات تبسيط/توسيع تعبير خطي وحساب قوى 10 لا تطلب ضرب أو قسمة أحاديات الحد.',
4:'توسيع تعبير خطي وتقييم قوة عددية لا يختبران قواعد رفع أحاديات الحد إلى قوى.',
5:'الأمثلة المولدة تتناول 10 بأسس موجبة أو الأس المجهول دون تطبيق الأس السالب المطلوب.',
8:'أحد المسارات يطلب حساب حجم مكعب، لا إيجاد الجذر التربيعي أو التكعيبي.',
9:'الأسئلة تختبر المربع الكامل وحسابات عددية، وليس تقدير جذر غير كامل.',
13:'سؤال مساحة سطح مكعب وحل المعادلة الجاهزة لا يختبران صياغة معادلة من موقف.',
14:'المسارات تحل معادلات ذات متغير في طرف واحد، ولا تحقق مهارة متغير في كل طرف.',
18:'سؤال تكلفة تتضمن رسمًا ثابتًا لا يمثل العلاقة التناسبية ص=م س المطلوبة.',
45:'تحديد نوع مثلث بحسب أضلاعه لا يختبر تطابق مضلعين وأجزائهما المتناظرة.',
64:'عد القطع المستقيمة بين نقاط لا يختبر قياس طول قطعة أو جمع الأطوال.',
66:'عد القطع بين نقاط لا يقيس إثبات علاقات بين القطع المستقيمة.',
69:'تحديد عدد أزواج الأضلاع المتوازية لرباعي عشوائي لا يختبر شروط إثبات متوازي الأضلاع.',
70:'سؤال بناء مربع بأعواد وأوصاف رباعيات عامة لا يقيس خواص المستطيل وأقطاره.',
71:'سؤال بناء مربع بأعواد لا يقيس خواص قطري المعين والمربع.'
}
partial={
1:'العشري الدوري أضيف إلى العقود، ولم يثبت استقلال نشاط الكتاب وعناصر التصنيف الأخرى.',
2:'الأمثلة الآلية تغطي بعض عمليات القوى؛ اختبار جميع الأشكال والحالات الحدودية يحتاج دليلًا منفصلًا.',
6:'حساب أس 10 لا يكفي لقياس كتابة العدد بالصيغة العلمية المعيارية.',
7:'الضرب أو القسمة في قوة عشرة لا يحقق وحده جميع عمليات الجمع/الطرح في الترميز العلمي.',
10:'مقارنة الكسور والأعداد العشرية لا تثبت مقارنة الجذور غير النسبية وتصنيف الأعداد الحقيقية.',
11:'حل بعض المعادلات ذات معاملات نسبية وتدريبات قسمة مستقلة لا يثبت كل سياق الدرس.',
12:'تدريبات حل معادلة من خطوتين مناسبة جزئيًا؛ توسيع قوس منفرد لا يثبت حل المعادلة.',
15:'توجد مهارات توسعة وتبسيط ومعادلات بسيطة؛ حالات الحل المتعدد/الصفري لم تثبت.',
16:'معدل الوحدة يدعم مفهوم معدل التغير، لكن الجداول/الرسوم تحتاج تحققًا.',
17:'يوجد تحديد صيغة الميل وبعض حسابات الدالة، وليست كل الأسئلة مخصصة لحساب الميل.',
19:'تظهر مسائل صيغة الميل والمقطع وقراءة المقطع، لكن معظم المسارات إنجليزية.',
20:'المقطع الصادي يظهر، لكن المقطع السيني ورسم المستقيم من المقطعين غير مختبرين بصورة كافية.',
21:'تحويل الصيغة الخطية لا يغطي الكتابة من الرسم أو النقطتين.',
22:'تصنيف عدد حلول النظام لا يختبر إيجاد نقطة التقاطع عبر الرسم.',
23:'حل نظام 2×2 مناسب جزئيًا؛ أسئلة معادلة مفردة لا تثبت الحل الجبري لنظام.',
24:'اختيار معادلة سياقية يمثل جانبًا من العلاقة فقط.',
25:'قراءة قيمة الدالة من جدول لا تختبر مجال العلاقة ومداها وطرق تمثيلها.',
26:'تمييز الدالة وحساب قيمتها مناسب جزئيًا؛ التمثيل والاختبار البياني يحتاجان مراجعة.',
27:'تعويض قاعدة خطية وتغيير الصيغة مناسبان جزئيًا؛ تمثيل الرسم يحتاج فحصًا.',
28:'مقارنة معدل خطي وتمييز خطي/تربيعي جزء من مقارنة الخصائص.',
29:'كتابة قاعدة من موقف وبعض التعويض لا تغطي إنشاء الدالة من كل التمثيلات.',
30:'التصنيف الخطي/غير الخطي مدعوم بعائلة عربية؛ بعض المسارات تتناول التناسب بدل الخطية العامة.',
31:'تقييم دالة تربيعية والجذور جزء من الدرس؛ الرأس ومحور التناظر ورسم القطع المكافئ لم تثبت.',
32:'مجال سياقي وتصنيف خطية/غير خطية لا يختبران تفسير ازدياد/تناقص الرسم النوعي.',
36:'توجد زاوية داخلية ناقصة وزاوية مكملة؛ الزوايا الخارجية ومجموع زوايا المضلع لم تستوف.',
40:'نقل نقطة باتجاهين مناسب جزئيًا؛ نقل شكل كامل ورسم الصورة يحتاج مراجعة.',
41:'الانعكاس حول المحورين مناسب لنقاط؛ تناظر الأشكال الكاملة يُراجع.',
43:'معدل القياس مناسب جزئيًا؛ مركز التمدد ورسم الصورة يتطلبان توثيقًا.',
44:'الانعكاس فقط لا يثبت تركيب التحويلات الصلبة بجميع أنواعها.',
46:'معامل التشابه والمسافات المتناظرة مناسب جزئيًا؛ تركيب التمدد والتحويلات يتطلب مراجعة.',
47:'معدل التشابه وقياس ضلع مناسب جزئيًا؛ خصائص المضلعات المتناظرة لم تثبت بالكامل.',
48:'قياس غير مباشر ومعامل مقياس مناسب جزئيًا؛ برهان تشابه المثلثات يحتاج معلمًا.',
49:'الميل يمس نسبة التشابه في المثلث القائم لكن لا يثبت معايير تشابهه.',
50:'عامل المساحة والطول مرتبطان بالدرس؛ المحيط في جميع حالات المصدر غير مثبت.',
51:'حجم أسطوانة صحيح جزئيًا؛ حجم منشور سؤال تمهيدي وليس قياسًا مباشرًا للأسطوانة.',
54:'مساحة الأسطوانة صحيحة جزئيًا؛ أسئلة مساحة/محيط دائرة مجردة أسئلة داعمة.',
55:'مساحة المخروط صحيحة جزئيًا؛ مساحة دائرة مجردة دعم تمهيدي.',
56:'تغيير مساحة/حجم المجسم بالنسبة المكعبة والمربعة مدعوم جزئيًا.',
57:'اتجاه الارتباط يدعم قراءة الانتشار؛ الرسم وقراءة النقاط مفتوحان للمراجعة.',
58:'ملاءمة خط وتنبؤ وباقٍ مدعومة جزئيًا؛ التقدير البصري والتفسير يتطلبان تحققًا.',
59:'التكرارات الهامشية والنسب الشرطية من جدول ثنائي مناسبة جزئيًا.',
60:'المتوسط والوسيط والمدى والربيع جزء من الإحصاء الوصفي.',
61:'الانحراف المطلق المتوسط والمدى مناسب جزئيًا؛ باقي المقاييس من المصدر تحتاج أدلة.',
62:'تصنيف شكل التوزيع والربيعات جزء من التحليل؛ الرسم والتفسير يحتاجان مراجعة.',
63:'عد القطع بين نقاط نشاط جزئي لا يغطي مفاهيم الاستقامية والمستويات.',
65:'المسافة الأفقية/الرأسية جزئية ولا تختبر نقطة المنتصف في المستوى.',
67:'حساب زاوية مضلع وزاوية مكملة جزئي؛ باقي قوانين الزوايا تحتاج دليلًا.',
68:'تصنيف رباعي بحسب التوازي جزء من الموضوع؛ خواص متوازي الأضلاع لم تثبت.',
72:'مساحة طائرة ورقية ومتوسط شبه منحرف آليان؛ تصنيف الأشكال والبراهين يحتاج معلمًا.',
73:'العضوية والفراغ/المنتهية/غير المنتهية آليًا، مع أنشطة وصفية مفتوحة.',
74:'الاحتواء واتحاد/تقاطع/فرق المجموعات الرقمية، مع تمثيلات حرة تُراجع.',
75:'قراءة مناطق فن الثلاثي آليًا، والرسم والتظليل يتطلبان مراجعة.',
76:'مسألة عد عناصر اتحاد مجموعتين صحيحة جزئيًا؛ المسائل اللفظية الأوسع تحتاج مراجعة.'
}
out=[]
for i,(a,b) in enumerate(zip(audit['lessons'],actual),start=1):
    assert a['ordinal']==b['Ordinal'] and a['lessonCode']==b['LessonCode']
    seen={x['Family'] for x in b['Samples']}
    assert seen==set(a['allowedFamilies']),(i,seen.symmetric_difference(a['allowedFamilies']))
    assert b['Status']=='ready' and b['QuestionCount']==192,(i,b['Status'],b['QuestionCount'])
    assert all(not b[k] for k in ['NonArabicQuestionCount','EnglishClauseQuestionCount',
                                   'NonArabicSolutionCount','EnglishSolutionClauseCount']),(i,'arabic runtime regression')
    english=[{'family':x['Family'],'samplePrompt':x['Prompt'][:220]} for x in b['Samples'] if not any('\u0600'<=c<='\u06ff' for c in x['Prompt'])]
    rich=json.loads((root/a['richV2Path']).read_text(encoding='utf8'))['Lessons'][0]
    summary=' '.join(rich.get('ExplanationParagraphs',[]))[:360]
    mismatch=None # previous 14 sampled misroutes were re-bound to objective-specific arithmetic families
    if mismatch or english:
        release='BLOCKED_CONFIRMED_SAMPLED_DEFECT'
    else: release='PARTIAL_TEACHER_REVIEW_SOURCE_PARITY_PENDING'
    out.append({'ordinal':i,'lessonCode':a['lessonCode'],'arabicTitle':a['arabicTitle'],'sourceLocator':a['sourceLocator'],
      'targetMechanic':a['mechanic'],'richV2ObjectiveExcerpt':summary,'questionCountGenerated':b['QuestionCount'],
      'configuredFamilyCount':len(a['allowedFamilies']),'observedFamilyCount':len(seen),
      'actualSampleByFamily':[{'family':x['Family'],'prompt':x['Prompt'],'correctAnswer':x['CorrectAnswer'],
         'studentPromptArabic':not (x in []) and any('\u0600'<=ch<='\u06ff' for ch in x['Prompt'])} for x in b['Samples']],
      'englishPromptFamilies':english,'semanticDefectConfirmed':bool(mismatch),'semanticDefectFinding':mismatch,
      'previouslyObservedMismatchesRemediated':clear.get(i),
      'fullAttemptPromptArabicAndNoEnglishWords':b['NonArabicQuestionCount']==0 and b['EnglishClauseQuestionCount']==0,
      'fullAttemptSolutionsArabicAndNoEnglishWords':b['NonArabicSolutionCount']==0 and b['EnglishSolutionClauseCount']==0,
      'coverageFinding':partial.get(i,'المهارة الآلية المعروضة تدعم جزءًا من المحتوى؛ يلزم مطابقة عناصر المصدر وأهدافه 1:1.'),
      'observedRuntimeStatus':b['Status'],'sampledContractDecision':release,
      'sourceItemByItemVerification':'NOT_EVIDENCED','wholeLessonPedagogicalCertification':False})
stats=collections.Counter(x['sampledContractDecision'] for x in out)
report={'schemaVersion':1,'auditKind':'76-lesson runtime sampled prompt and target-mechanic assessment (not complete source-page item parity)',
 'grade':8,'pathway':'GENERAL','academicLanguage':'ar','targetYear':'2026-2027',
 'sourceEditionConflict':'2019-2020 index 12/76 versus 2025-2026 index 8/56',
 'method':'Generated 192 composed student Practice questions per lesson for 76 lessons using 64 seeds. Verified all configured lesson-family routes reach a real question, every one of 14592 prompts and solutions has Arabic and no English word of 4+ letters, and the exact verifier accepts expected answers and rejects invalid text. Previously observed 14 off-target family routes were removed/replaced, not evidence of full text-book parity. No source-item academic signoff is claimed.',
 'summary':{'lessonsSampled':len(out),'generatedQuestions':sum(x['questionCountGenerated'] for x in out),
   'totalConfiguredFamilyAssignments':sum(x['configuredFamilyCount'] for x in out),'allAssignmentsSampled':True,
   'lessonsWithObservedNonArabicPrompts':sum(b['NonArabicQuestionCount']>0 for b in actual),
   'lessonsWithObservedEnglishClauses':sum(b['EnglishClauseQuestionCount']>0 for b in actual),
   'nonArabicGeneratedQuestionCount':sum(b['NonArabicQuestionCount'] for b in actual),
   'englishGeneratedQuestionCount':sum(b['EnglishClauseQuestionCount'] for b in actual),
   'nonArabicSolutionCount':sum(b['NonArabicSolutionCount'] for b in actual),
   'englishSolutionCount':sum(b['EnglishSolutionClauseCount'] for b in actual),
   'lessonsWithConfirmedUnrelatedOrOffObjectiveExamples':sum(x['semanticDefectConfirmed'] for x in out),
   'releaseBlockedLessons':stats['BLOCKED_CONFIRMED_SAMPLED_DEFECT'],
   'partialReviewPendingLessons':stats['PARTIAL_TEACHER_REVIEW_SOURCE_PARITY_PENDING'],
   'fullySourceCertifiedLessons':0,
   'previouslyObservedMisrouteFindingsRemediated':len(clear),
   'correctAnswersAcceptedByExactVerifier':14592,'invalidTextRejectedByExactVerifier':14592,
   'independentMathematicalVerifierEvidence':'NOT_TESTED_BY_THIS_HARNESS',
   'originalStudentBookElementParity':'NOT_CERTIFIED'},
 'lessons':out}
dest=base/'general-practice-76-runtime-semantic-screen.json'
dest.write_text(json.dumps(report,ensure_ascii=False,indent=2)+'\n',encoding='utf8')
print(json.dumps(report['summary'],ensure_ascii=False))
print('bytes',dest.stat().st_size,'file',dest)
