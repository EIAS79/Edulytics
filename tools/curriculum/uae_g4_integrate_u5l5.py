import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:05:05'
title='حل المسائل الكلامية المكونة من عدة خطوات'; unit='الضرب في عدد مكون من رقمين'
l=next(x for x in C['Lessons'] if x['LessonCode']==code);b=next(x for x in B['Lessons'] if x['LessonCode']==code)
assert b['SortOrder']==39
t={'CultureCode':'ar','Title':title,
'Explanation':'أقرأ المسألة الكلامية متعددة الخطوات وأحدد المطلوب والمعطيات. أكتب معادلة تتضمن ضرب الأعداد من رقمين ثم جمعًا أو طرحًا. أنفذ العمليات بترتيبها الصحيح وأتحقق من الإجابة ومعقوليتها.',
'KeyConceptsAndRules':'الخطوات الأربع: أفهم، أخطط، أحل، أتحقق. إذا كان الادخار الأسبوعي ١٢ درهمًا والإنفاق ٣ دراهم لمدة ١٤ أسبوعًا فالصافي (١٢−٣)×١٤=١٢٦ درهمًا. إذا أضفت قطعًا إلى مجموع مجموعات متساوية، أحسب الضرب ثم أجمع الإضافات.',
'WorkedExamples':'١. (١٢−٣)×١٤=١٢٦ درهمًا. ٢. ١٨ مجموعة في كل منها ٢٤ قطعة وأضيفت ١٠ قطع: ١٨×٢٤+١٠=٤٤٢. ٣. ٢٧ طالبًا يدفع كل منهم ٣٥ درهمًا مع خصم ٤٥ درهمًا: ٢٧×٣٥−٤٥=٩٠٠. ٤. إذا اشترينا ١٥ عبوة تضم كل منها ١٨ قطعة ثم أضفنا ٢٥، فالمجموع ٢٩٥.',
'StepByStepSolutions':'١. أقرأ وأحدد نوع كل كمية. ٢. أقرر هل يلزم ضرب ثم جمع أو طرح. ٣. أكتب معادلة ثم أنفذ الضرب الصحيح. ٤. أجري الجمع أو الطرح وأتحقق من أن النتيجة تجيب عن المطلوب.',
'CommonMistakes':'جمع المعطيات جميعها دون تحديد العلاقات؛ إجراء الجمع قبل حساب عدد المجموعات؛ إهمال وحدة القياس؛ التوقف عند حاصل الضرب وترك الخطوة الإضافية.',
'QuickSummary':'أفهم المسألة، أحدد ترتيب العمليات، أضرب ثم أجمع أو أطرح وأتحقق.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l.update({'SourceTitle':'UAE Grade 4 Common Arabic student book','SourceLocator':'04-01.pdf U05 L05 printed pp305-310 PDF328-333','TitleSourceReference':'04-01.pdf U05 L05 printed pp305-310 PDF328-333','SourceEdition':'Student book authority; year nonblocking','SourceSha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3','CanonicalBodySha256':hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest(),'AdaptationStatus':'Original Arabic equivalent; Stage D pending'})
b['Title']=title;b['UnitTitle']=unit;M['Units'][4]['ls'][4]=title
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs';s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit5Lesson02EstimateProductsQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1;s=s.replace(a,a+'\n          UaeGrade4Unit5Lesson05MultiStepQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit5Lesson02EstimateProductsQuestions.Supports(family))\n            return UaeGrade4Unit5Lesson02EstimateProductsQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit5Lesson05MultiStepQuestions.Supports(family))\n            return UaeGrade4Unit5Lesson05MultiStepQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit5Lesson02EstimateProductsQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit5Lesson02EstimateProductsQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit5Lesson05MultiStepQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit5Lesson05MultiStepQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit5Lesson02EstimateProductsQuestions.Supports(problem.Family))\n            return UaeGrade4Unit5Lesson02EstimateProductsQuestions.Solve(problem.Family, p);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit5Lesson05MultiStepQuestions.Supports(problem.Family))\n            return UaeGrade4Unit5Lesson05MultiStepQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig');anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u05.l05.net_weekly_savings','uae.g4.u05.l05.missing_after_groups']
cstr='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:05:05"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:05:05", "number.multistep_word_problems",
            "MULTIPLICATION_TWO_STEP_WORD_PROBLEMS",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP305_310_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,cstr+anchor),encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json';j=load(p)
template=next(x for x in j['families'] if x['id']=='uae.g4.u05.l02.estimate_two_digit_product')
for x in f:
    assert not any(e['id']==x for e in j['families'])
    z=dict(template);z.update({'id':x,'skillId':'number.multistep_word_problems','answerType':'exact_integer','verificationPolicy':'g4-u05-l05-student-source-stage-d-not-run','representations':['word_problem','equation'],'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(z)
save(p,j)
rich={'SchemaVersion':1,'ContentVersion':'uae-g4-u05-l05-ar-20261111','PackCode':'UAE-MOE-MATH','Lessons':[{'LessonCode':code,'CultureCode':'ar','Title':title,'ExplanationParagraphs':['أحدد المعطيات والمطلوب ثم أحول السؤال إلى معادلة من خطوتين على الأقل.','أستعمل الضرب لمعرفة المجموعات المتساوية، ثم الجمع أو الطرح لحساب الكمية النهائية.','أراجع ترتيب العمليات ووحدات القياس لأتأكد أن النتيجة تجيب عن السؤال.'],'KeyConcepts':[{'Title':'خطة حل المسألة','Definition':'الفهم والتخطيط والحل والتحقق.','Rule':'أحدد العمليات المطلوبة قبل الحساب.','Example':'١٢ درهمًا ادخارًا و٣ إنفاقًا في ١٤ أسبوعًا.'},{'Title':'ترتيب العمليات','Definition':'أبدأ بضرب المجموعات ثم الجمع أو الطرح بحسب العلاقات.','Rule':'الأقواس أولًا ثم الضرب ثم الجمع.','Example':'(١٢−٣)×١٤=١٢٦.'},{'Title':'التحقق من المعقولية','Definition':'مقارنة الناتج بسياق السؤال والتقدير.','Rule':'أراجع وحدة القياس وأحسب بطريقة أخرى.','Example':'١٨×٢٤+١٠=٤٤٢.'}],'WorkedExamples':[{'Title':'ادخار أسبوعي','Question':'يدخر طالب ١٢ درهمًا وينفق ٣ دراهم في كل أسبوع لمدة ١٤ أسبوعًا. كم صافي الادخار؟','Method':'أطرح ثم أضرب.','Steps':['١٢−٣=٩ دراهم صافية في الأسبوع.','٩×١٤=١٢٦.','الناتج ١٢٦ درهمًا.'],'Answer':'١٢٦ درهمًا','Check':'١٢×١٤−٣×١٤=١٢٦.'},{'Title':'صناديق قطع','Question':'في ١٨ صندوقًا ٢٤ قطعة لكل صندوق، وأضيفت ١٠ قطع. كم قطعة إجمالًا؟','Method':'أضرب ثم أجمع.','Steps':['١٨×٢٤=٤٣٢.','٤٣٢+١٠=٤٤٢.','أكتب العدد مع الوحدة.'],'Answer':'٤٤٢ قطعة','Check':'حوالي ٢٠×٢٠+١٠.'},{'Title':'رحلة الطلاب','Question':'يدفع ٢٧ طالبًا ٣٥ درهمًا لكل منهم وخصمت المدرسة ٤٥ درهمًا. كم تدفع المدرسة؟','Method':'ضرب ثم طرح.','Steps':['٢٧×٣٥=٩٤٥.','٩٤٥−٤٥=٩٠٠.','المبلغ النهائي ٩٠٠ درهم.'],'Answer':'٩٠٠ درهم','Check':'أقل من ٩٤٥.'}],'CommonMistakes':[{'Mistake':'البدء بعملية لا تعبر عن العلاقة في السؤال.','WhyWrong':'يغير المعنى الحسابي.','Correction':'أكتب معادلة قبل التنفيذ.'},{'Mistake':'إهمال الخطوة الأخيرة بعد الضرب.','WhyWrong':'يكون الناتج وسيطًا لا الجواب.','Correction':'أجيب عن المطلوب النهائي.'}],'SummaryPoints':['أفهم المعطيات والعلاقات.','أحدد ترتيب الخطوات ثم أنفذها.','أراجع الناتج ووحداته.'],'Visuals':[{'Kind':'EquationSet','Title':'مسألة من خطوتين','Description':'أمثلة حسابية توضح العلاقة بين الضرب والجمع أو الطرح.','Items':['(١٢−٣)×١٤ = ١٢٦','١٨×٢٤+١٠ = ٤٤٢','٢٧×٣٥−٤٥ = ٩٠٠']}],'Videos':[]}]}
save(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u05-l05-ar.rich-lesson-v2.json',rich)
print('PASS U05L05 integrated')
