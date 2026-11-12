import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:03:08';title='استقصاء حل المسائل: الإجابات المنطقية';unit='فهم الضرب والقسمة'
l=next(x for x in C['Lessons'] if x['LessonCode']==code);b=next(x for x in B['Lessons'] if x['LessonCode']==code)
t={'CultureCode':'ar','Title':title,
'Explanation':'نفحص معقولية الإجابات قبل الاعتماد عليها. نحدد أولًا المعطيات والمطلوب، ثم نستخدم التقدير أو الضرب أو الجمع لنعرف هل العبارة منطقية. في مسائل النقود نقارن المدخرات أو الإيرادات بالتكاليف، وفي المسافة أو الزمن نتحقق من العلاقة بين الكميات.',
'KeyConceptsAndRules':'خطوات حل المسألة: أفهم، أخطط، أحل، أتحقق. أقدر الأعداد لتقريب النتيجة أو أستخدم العملية الدقيقة عندما تتطلب المسألة ذلك. لا تكفي عبارة نعم أو لا بل نحتاج إلى تفسير مدعوم بالأعداد.',
'WorkedExamples':'١. مجموع ٢٨٣ و٣٠٢ يساوي ٥٨٥، لذلك تقدير العدد ٤٠٠ غير معقول. ٢. في أربعة أسابيع، ٤ × ٢٤٠ = ٩٦٠ درهمًا وهو أكبر من ٤٠٠. ٣. ٤٨ و٥٢ و٤٧ و٥٣ مجموعها ٢٠٠ لا نحو ١٠٠.',
'StepByStepSolutions':'١. أقرأ المسألة وأسجل الأعداد ومعنى المطلوب. ٢. أضع تقديرًا أو معادلة مناسبة. ٣. أقارن الناتج بالتقدير أو الحد الوارد. ٤. أختار إجابة منطقية وأشرح السبب.',
'CommonMistakes':'اعتماد تقدير بعيد دون مراجعة؛ الخلط بين الإجمالي ونصيب الفرد؛ مقارنة عدد الأسبوع الواحد بحد إجمالي عدة أسابيع؛ إهمال المضاعفة أو عدد الأشخاص في حساب التكلفة.',
'QuickSummary':'أفحص الإجابات من خلال التقدير والعملية الصحيحة والمقارنة، وأشرح لماذا هي منطقية أو غير منطقية.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l['SourceTitle']='Owner-selected UAE Grade 4 Arabic general student book'
l['SourceLocator']='04-01.pdf U03 L08 printed pp179-184 PDF 202-207'
l['TitleSourceReference']=l['SourceLocator'];l['SourceEdition']='Student source authority; edition nonblocking'
l['SourceSha256']='859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3'
l['CanonicalBodySha256']=hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest()
l['AdaptationStatus']='Original Arabic mathematical equivalent, Stage D pending'
b['Title']=title;b['UnitTitle']=unit;B['Units'][2]['Title']=unit
M['Units'][2]['title']=unit;M['Units'][2]['ls'][7]=title
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'
s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit3Lesson07FactorsMultiplesQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1;s=s.replace(a,a+'\n          UaeGrade4Unit3Lesson08ReasonablenessQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit3Lesson07FactorsMultiplesQuestions.Supports(family))\n            return UaeGrade4Unit3Lesson07FactorsMultiplesQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit3Lesson08ReasonablenessQuestions.Supports(family))\n            return UaeGrade4Unit3Lesson08ReasonablenessQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit3Lesson07FactorsMultiplesQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit3Lesson07FactorsMultiplesQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit3Lesson08ReasonablenessQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit3Lesson08ReasonablenessQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit3Lesson07FactorsMultiplesQuestions.Supports(problem.Family))\n            return UaeGrade4Unit3Lesson07FactorsMultiplesQuestions.Solve(problem.Family, p);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit3Lesson08ReasonablenessQuestions.Supports(problem.Family))\n            return UaeGrade4Unit3Lesson08ReasonablenessQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig');anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u03.l08.sum_estimate','uae.g4.u03.l08.savings_feasible','uae.g4.u03.l08.travel_feasible']
contract='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:03:08"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:03:08", "number.reasonableness_estimation",
            "EVALUATE_ESTIMATES_AND_FEASIBILITY",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP179_184_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,contract+anchor),encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
j=load(p);template=next(x for x in j['families'] if x['id']=='uae.g4.u03.l07.is_factor')
for x in f:
    assert not any(e['id']==x for e in j['families'])
    z=dict(template);z.update({'id':x,'skillId':'number.reasonableness_estimation','answerType':'exact_integer','verificationPolicy':'g4-u03-l08-student-source-stage-d-not-run','representations':['contextual','equation'],'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(z)
save(p,j)
print('PASS U03 L08 three reasonableness family routes')
