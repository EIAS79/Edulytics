import json,pathlib,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def read(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,o):p.write_text(json.dumps(o,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=read(cp);B=read(bp);M=read(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:02:04'
title='تقدير المجاميع والفروق'
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
t={'CultureCode':'ar','Title':title,
'Explanation':'عندما نحتاج جوابًا تقريبيًا للجمع أو الطرح نقرّب كل عدد إلى المنزلة المطلوبة ثم نجمع أو نطرح القيم المقرّبة. تتيح لنا هذه الخطوة التحقق سريعًا من معقولية الناتج الدقيق وتقدير الكميات في مواقف حياتية.',
'KeyConceptsAndRules':'للتقدير إلى أقرب مئة أو ألف أو عشرة آلاف نقرب كل حد بصورة مستقلة ثم نجمعهما أو نطرحهما. إذا كان رقم المنزلة التالية ٥ أو أكثر نزيد المنزلة واحدًا، وإلا نبقيها. التقدير ليس دائمًا النتيجة الدقيقة، بل مقدارًا قريبًا مناسبًا لسؤال المسألة.',
'WorkedExamples':'١. ٥٬٤٨١ + ٢٬٣٢٦ إلى أقرب مئة: ٥٬٥٠٠ + ٢٬٣٠٠ = ٧٬٨٠٠. ٢. ٧٬٥٤٢ − ٣٬٢٢٥ إلى أقرب مئة: ٧٬٥٠٠ − ٣٬٢٠٠ = ٤٬٣٠٠. ٣. ٥٥٬٥٩٩ − ٢٢٬٧٨٢ إلى أقرب ألف: ٥٦٬٠٠٠ − ٢٣٬٠٠٠ = ٣٣٬٠٠٠.',
'StepByStepSolutions':'١. أحدد العملية والمنزلة التي ينبغي التقريب إليها. ٢. أقرب كل عدد مستقلًا. ٣. أحسب مجموع أو فرق الأعداد المقرّبة. ٤. أراجع أن الناتج معقول بالاعتماد على حجم الأعداد الأصلية.',
'CommonMistakes':'تقريب المجموع أو الفرق النهائي بدل تقريب كل عدد عند مطالبة المسألة بذلك؛ نسيان الاحتفاظ بأصفار المنازل؛ خلط التقريب إلى أقرب مئة بالتقريب إلى أقرب ألف؛ تقديم الناتج التقريبي كأنه دقيق.',
'QuickSummary':'أقدّر المجاميع والفروق بتقريب الحدود إلى المنزلة المحددة ثم إجراء العملية، وأشرح معقولية التقدير.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l['SourceTitle']='UAE Grade 4 owner-selected student mathematics, General'
l['SourceLocator']='04-01.pdf, Unit 2 Lesson 4, printed pp79-84, PDF pp102-107'
l['TitleSourceReference']=l['SourceLocator']
l['SourceEdition']='Student book edition nonblocking'
l['SourceSha256']='859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3'
l['CanonicalBodySha256']=hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest()
l['AdaptationStatus']='First-party Arabic math equivalent; Stage D not run'
b['Title']=title;b['UnitTitle']='جمع الأعداد الكلية وطرحها'
B['Units'][1]['Title']='جمع الأعداد الكلية وطرحها'
M['Units'][1]['title']='جمع الأعداد الكلية وطرحها'
M['Units'][1]['ls'][3]=title
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'
s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit2Lesson03MentalQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1
s=s.replace(a,a+'\n          UaeGrade4Unit2Lesson04EstimateQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit2Lesson03MentalQuestions.Supports(family))\n            return UaeGrade4Unit2Lesson03MentalQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit2Lesson04EstimateQuestions.Supports(family))\n            return UaeGrade4Unit2Lesson04EstimateQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit2Lesson03MentalQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit2Lesson03MentalQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit2Lesson04EstimateQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit2Lesson04EstimateQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit2Lesson03MentalQuestions.Supports(problem.Family))\n            return UaeGrade4Unit2Lesson03MentalQuestions.Solve(problem.Family, p);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit2Lesson04EstimateQuestions.Supports(problem.Family))\n            return UaeGrade4Unit2Lesson04EstimateQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig')
marker='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(marker)==1
families=['uae.g4.u02.l04.'+side+'_nearest_'+place for side in ('sum','diff') for place in ('hundred','thousand','ten_thousand')]
contract='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:02:04"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:02:04",
            "number.estimate_add_subtract",
            "ESTIMATE_SUM_DIFF_ROUND_BOTH_TERMS",
            ['''+', '.join('"'+f+'"' for f in families)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP79_84_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(marker,contract+marker),encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
j=read(p)
template=next(x for x in j['families'] if x['id']=='uae.g4.u02.l03.make_friendly_ten')
for fid in families:
    assert not any(x['id']==fid for x in j['families'])
    e=dict(template)
    e.update({'id':fid,'skillId':'number.estimate_add_subtract','answerType':'exact_integer',
       'verificationPolicy':'g4-u02-l04-local-student-stage-d-not-run',
       'representations':['place_value_chart','symbolic'],'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(e)
save(p,j)
print('PASS integrated grade4 U02 L04 6 estimation families')
