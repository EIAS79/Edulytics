import pathlib,json,copy,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
prev='PED:UAE-MOE-MATH:L4:COMMON:04:09';code='PED:UAE-MOE-MATH:L4:COMMON:04:10'
assert not any(x['LessonCode']==code for x in C['Lessons']) and not any(x['LessonCode']==code for x in B['Lessons'])
assert B['Units'][3]['LessonCount']==9
title='استقصاء حل المسائل: الإجابة التقديرية أم الإجابة الدقيقة'
t={'CultureCode':'ar','Title':title,
'Explanation':'أحدد ما إذا كانت المسألة تحتاج إجابة دقيقة أم إجابة تقديرية بحسب المطلوب. إذا كنا سنحاسب على تكلفة فعلية أو نوزع أشياء محددة نحتاج الدقة، أما إذا كان المطلوب حوالي أو تقريبًا أو تقدير الميزانية فنستخدم التقريب. نختار العملية ثم نقارن النتائج ونفسر.',
'KeyConceptsAndRules':'خطوات حل المسألة: الفهم، التخطيط، الحل، التحقق. أشير إلى الكلمات التي تدل على الدقة أو التقريب، ثم أضرب. ٥ تذاكر بسعر ٢٢ درهمًا للواحدة تتطلب ٥×٢٢=١١٠ دراهم بالضبط، بينما نحو ٧ دراجات سعر الواحدة ٧٥ درهمًا يمكن تقدير التكلفة ٧×٨٠≈٥٦٠.',
'WorkedExamples':'١. خمسة أشخاص وسعر التذكرة ٢٢ درهمًا: ٥×٢٢=١١٠، الإجابة الدقيقة. ٢. ٧ دراجات سعر الواحدة ٧٥ درهمًا: التقدير ٧×٨٠=٥٦٠ درهمًا. ٣. ٦ طابعات سعر الواحدة ٣٨٤ درهمًا وحوالي ٨٨ درهمًا للأوراق، هل يكفي ٢٤٠٠ درهم؟ التكلفة الدقيقة ٦×٣٨٤+٨٨=٢٣٩٢، فيكفي. ٤. ٣ أفلام مدة الواحد نحو ساعتين، التقدير ٦ ساعات.',
'StepByStepSolutions':'١. أحدد المعطيات والمطلوب. ٢. أقرر إذا كان المطلوب عددًا دقيقًا أم تقريبيًا وأذكر السبب. ٣. أنفذ الضرب الدقيق أو أقرّب العامل ثم أضرب. ٤. أراجع المعقولية وأكتب وحدة القياس المناسبة.',
'CommonMistakes':'تقديم تقدير عند دفع مبلغ محدد؛ إجراء عملية دقيقة مع سؤال يطلب تقريبًا دون التقريب؛ إهمال وحدة القياس؛ تجاهل عبارة هل يكفي التي تتطلب مقارنة دقيقة أو حدًا آمنًا.',
'QuickSummary':'أختار بين التقدير والحساب الدقيق بحسب معنى المسألة، ثم أتحقق من معقولية الناتج.'}
c=copy.deepcopy(next(x for x in C['Lessons'] if x['LessonCode']==prev))
c.update({'LessonCode':code,'OfficialReferenceCode':'UAE:REF:TEXTBOOK:G4:COMMON:T1:U04:L10',
'SourceLocator':'04-01.pdf U04 L10 printed pp255-260 PDF 278-283',
'TitleSourceReference':'04-01.pdf U04 L10 printed pp255-260 PDF 278-283',
'CanonicalBodySha256':hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest(),
'AdaptationStatus':'Original Arabic equivalent; Stage D pending'})
c['Translations']=[x for x in c['Translations'] if x['CultureCode']!='ar']+[t]
i=next(i for i,x in enumerate(C['Lessons']) if x['LessonCode']==prev);C['Lessons'].insert(i+1,c)
b=copy.deepcopy(next(x for x in B['Lessons'] if x['LessonCode']==prev))
b.update({'LessonCode':code,'SourceLessonCode':'EDU:UAE-MOE-MATH:L4:COMMON:U04:L10','LessonNumber':10,'Title':title,'SortOrder':33,
'OfficialReferenceCode':'UAE:REF:TEXTBOOK:G4:COMMON:T1:U04:L10'})
for x in b.get('Alignments',[]):x['ReferenceCode']=b['OfficialReferenceCode']
for x in B['Lessons']:
    if x['SortOrder']>=33:x['SortOrder']+=1
i=next(i for i,x in enumerate(B['Lessons']) if x['LessonCode']==prev);B['Lessons'].insert(i+1,b)
B['Units'][3]['LessonCount']=10;M['Units'][3]['ls'].append(title);M['LessonCount']=sum(len(u['ls']) for u in M['Units'])
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs';s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit4Lesson09MultiDigitQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1;s=s.replace(a,a+'\n          UaeGrade4Unit4Lesson10PrecisionChoiceQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit4Lesson09MultiDigitQuestions.Supports(family))\n            return UaeGrade4Unit4Lesson09MultiDigitQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit4Lesson10PrecisionChoiceQuestions.Supports(family))\n            return UaeGrade4Unit4Lesson10PrecisionChoiceQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit4Lesson09MultiDigitQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit4Lesson09MultiDigitQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit4Lesson10PrecisionChoiceQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit4Lesson10PrecisionChoiceQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit4Lesson09MultiDigitQuestions.Supports(problem.Family))\n            return UaeGrade4Unit4Lesson09MultiDigitQuestions.Solve(problem.Family, p);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit4Lesson10PrecisionChoiceQuestions.Supports(problem.Family))\n            return UaeGrade4Unit4Lesson10PrecisionChoiceQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig');anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u04.l10.choose_answer_precision','uae.g4.u04.l09.multidigit_product','uae.g4.u04.l09.round_estimate']
cstr='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:04:10"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:04:10", "number.exact_vs_estimated_answers",
            "CHOOSE_ESTIMATION_OR_EXACT_ANSWER",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP255_260_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,cstr+anchor),encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json';j=load(p)
template=next(x for x in j['families'] if x['id']=='uae.g4.u04.l09.multidigit_product')
z=dict(template);z.update({'id':'uae.g4.u04.l10.choose_answer_precision','skillId':'number.exact_vs_estimated_answers','answerType':'exact_integer','verificationPolicy':'g4-u04-l10-student-source-stage-d-not-run','representations':['contextual','symbolic'],'productionRouting':False,'lessonPracticeRouting':True})
assert not any(e['id']==z['id'] for e in j['families'])
j['families'].append(z);save(p,j)
rich={'SchemaVersion':1,'ContentVersion':'uae-g4-u04-l10-ar-20261111','PackCode':'UAE-MOE-MATH','Lessons':[{'LessonCode':code,'CultureCode':'ar','Title':title,
'ExplanationParagraphs':['أحدد من سياق السؤال هل المطلوب حساب دقيق أم تقدير قريب.','أستخدم الحساب الدقيق عند الدفع وتحديد الكميات المطلوبة فعلًا، والتقريب عند السؤال عن ميزانية تقريبية أو عدد حوالي قيمة معينة.','أتحقق من النتيجة بخطوات الفهم والتخطيط والحل والمراجعة.'],
'KeyConcepts':[{'Title':'الإجابة الدقيقة','Definition':'نتيجة الحساب دون تقريب.','Rule':'أستخدمها لمعرفة ثمن فعلي أو عدد محدد من الأشياء.','Example':'٥ تذاكر × ٢٢ درهمًا=١١٠ دراهم.'},{'Title':'الإجابة التقديرية','Definition':'نتيجة تقريب المعطيات لتسهيل الحساب.','Rule':'أستخدمها إذا ورد حوالي أو تقريبًا.','Example':'٧ × ٧٥ ≈ ٧ × ٨٠ = ٥٦٠.'},{'Title':'خطوات حل المسألة','Definition':'فهم وتخطيط وحل وتحقق.','Rule':'أختار الطريقة المناسبة قبل الحساب.','Example':'أحدد هل ٢٤٠٠ درهم تكفي ثم أحسب التكلفة.'}],
'WorkedExamples':[{'Title':'ثمن تذاكر','Question':'ما الثمن الدقيق لخمس تذاكر سعر الواحدة ٢٢ درهمًا؟','Method':'الحساب الدقيق.','Steps':['المطلوب ثمن فعلي.','أضرب ٥×٢٢.','النتيجة ١١٠ دراهم.'],'Answer':'١١٠ دراهم','Check':'٥×٢٠+٥×٢=١١٠.'},{'Title':'تقدير ثمن دراجات','Question':'حوالي كم تكلفة سبع دراجات ثمن الواحدة ٧٥ درهمًا؟','Method':'أقرب السعر إلى ثمانين.','Steps':['المطلوب حوالي.','أقرب ٧٥ إلى ٨٠.','٧×٨٠=٥٦٠ درهمًا تقريبًا.'],'Answer':'حوالي ٥٦٠ درهمًا','Check':'التقدير أعلى قليلًا من ٧×٧٥=٥٢٥.'},{'Title':'شراء طابعات','Question':'هل تكفي ٢٤٠٠ درهم لشراء ٦ طابعات بسعر ٣٨٤ درهمًا للواحدة وأوراق بـ٨٨ درهمًا؟','Method':'أقارن المبلغ بالتكلفة الدقيقة.','Steps':['٦×٣٨٤=٢٣٠٤.','٢٣٠٤+٨٨=٢٣٩٢.','٢٣٩٢ أصغر من ٢٤٠٠، فيكفي المبلغ.'],'Answer':'نعم، يتبقى ٨ دراهم','Check':'٢٤٠٠−٢٣٩٢=٨.'}],
'CommonMistakes':[{'Mistake':'التقدير عند الحاجة إلى مبلغ دقيق للدفع.','WhyWrong':'قد لا يكفي المبلغ الفعلي.','Correction':'أحسب الثمن الدقيق قبل اتخاذ قرار شراء.'},{'Mistake':'عدم تمييز كلمة حوالي.','WhyWrong':'قد أجري حسابًا مطولًا دون ضرورة.','Correction':'أحدد نوع الإجابة من السؤال قبل الحل.'}],
'SummaryPoints':['أقرر هل يلزم ناتج دقيق أم تقريبي.','أختار التقدير أو الحساب المناسب.','أتحقق وأفسر اختياري.'],
'Visuals':[{'Kind':'EquationSet','Title':'الدقة والتقدير','Description':'مثال مقارن بين حساب دقيق وآخر تقديري.','Items':['٥ × ٢٢ = ١١٠','٧ × ٧٥ ≈ ٧ × ٨٠ = ٥٦٠','٦ × ٣٨٤ + ٨٨ = ٢٣٩٢']}],'Videos':[]}]}
save(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u04-l10-ar.rich-lesson-v2.json',rich)
print('PASS U04L10 exact-vs-estimate source content and families')
