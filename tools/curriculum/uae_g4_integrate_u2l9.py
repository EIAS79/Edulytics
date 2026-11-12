import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:02:09'
title='حل المسائل الكلامية المكونة من عدة خطوات'
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
t={'CultureCode':'ar','Title':title,
'Explanation':'تحتاج المسألة الكلامية متعددة الخطوات إلى أكثر من عملية حسابية. نقرأ أحداث المسألة بالترتيب ونميز بين المبالغ أو الكميات التي أضيفت وتلك التي أنفقت أو استعملت، ونمثل العدد المجهول بحرف أو مربع، ثم نكتب معادلة تتضمن الجمع والطرح.',
'KeyConceptsAndRules':'المتغير رمز لقيمة مجهولة. لتتبع خطوات متتابعة يمكن كتابة: العدد الابتدائي + الزيادة − النقصان = المجهول. عند وجود زيادات أو نفقات متعددة نلتزم بترتيب أحداث القصة ثم نتحقق بالتقدير من معقولية الناتج.',
'WorkedExamples':'١. ٥٢٠ + ٧٢٥ − ١٢٥ − ٦٥ = ١٬٠٥٥: المال المتبقي من حصيلة المبيعات. ٢. على الحافلة ٢٠ راكبًا، نزل ١٤ وصعد ٥ ونزل ٢، وبقي ٢٤ بعد الصعود الأخير: ٢٠ − ١٤ + ٥ − ٢ + ب = ٢٤ ومنه ب = ١٥. ٣. يوجد ٣٦٧ صندوقًا، بيع ١٢٦ و٩٢ وجاء ٢٠٣ أخرى: الباقي = ٣٦٧ − ١٢٦ − ٩٢ + ٢٠٣ = ٣٥٢.',
'StepByStepSolutions':'١. أفهم القصة وأحدد البداية والتغيرات والمجهول. ٢. أمثل المجهول بحرف وأكتب معادلة مرتبة. ٣. أجري كل جمع أو طرح بالترتيب وفق الأحداث. ٤. أعوض الناتج في المعادلة وأراجع بالتقدير أو عكس العملية.',
'CommonMistakes':'إغفال أحد التغيرات؛ استعمال الجمع بدل الطرح في حدث يتضمن إنفاقًا أو خروجًا؛ إهمال ترتيب الأحداث؛ الخلط بين العدد المجهول والعدد النهائي؛ تقدير خاطئ لأن منزلة التقريب لم تُحدد.',
'QuickSummary':'أترجم مسألة حياتية فيها عدة عمليات إلى معادلة بمجهول، وأحل الخطوات بالتسلسل ثم أتحقق.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l['SourceTitle']='Owner-selected UAE Grade 4 General Arabic student mathematics'
l['SourceLocator']='04-01.pdf U02 L09 printed pp113-118 PDF 136-141'
l['TitleSourceReference']=l['SourceLocator']
l['SourceEdition']='Student source authority, edition nonblocking'
l['SourceSha256']='859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3'
l['CanonicalBodySha256']=hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest()
l['AdaptationStatus']='First-party Arabic equivalents; Stage D pending'
b['Title']=title;b['UnitTitle']='جمع الأعداد الكلية وطرحها'
B['Units'][1]['Title']='جمع الأعداد الكلية وطرحها'
M['Units'][1]['title']='جمع الأعداد الكلية وطرحها';M['Units'][1]['ls'][8]=title
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'
s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit2Lesson08BarModelQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1
s=s.replace(a,a+'\n          UaeGrade4Unit2Lesson09MultiStepQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit2Lesson08BarModelQuestions.Supports(family))\n            return UaeGrade4Unit2Lesson08BarModelQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit2Lesson09MultiStepQuestions.Supports(family))\n            return UaeGrade4Unit2Lesson09MultiStepQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit2Lesson08BarModelQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit2Lesson08BarModelQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit2Lesson09MultiStepQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit2Lesson09MultiStepQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit2Lesson08BarModelQuestions.Supports(problem.Family))\n            return UaeGrade4Unit2Lesson08BarModelQuestions.Solve(problem.Family, p);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit2Lesson09MultiStepQuestions.Supports(problem.Family))\n            return UaeGrade4Unit2Lesson09MultiStepQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig')
anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u02.l09.add_then_subtract','uae.g4.u02.l09.subtract_then_add','uae.g4.u02.l09.add_three_parts','uae.g4.u02.l09.subtract_two_costs']
contract='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:02:09"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:02:09",
            "number.multistep_add_subtract",
            "MULTISTEP_EQUATION_WITH_UNKNOWN",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP113_118_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,contract+anchor),encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
j=load(p)
template=next(x for x in j['families'] if x['id']=='uae.g4.u02.l08.bar_part_whole')
for x in f:
    assert not any(e['id']==x for e in j['families'])
    z=dict(template)
    z.update({'id':x,'skillId':'number.multistep_add_subtract','answerType':'exact_integer',
       'verificationPolicy':'g4-u02-l09-local-student-stage-d-not-run',
       'representations':['equation','contextual'],'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(z)
save(p,j)
print('PASS integrated UAE G4 U02 L09 4 multistep exact families')
