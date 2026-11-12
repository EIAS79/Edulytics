import json,pathlib,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p): return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,x): p.write_text(json.dumps(x,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cpath=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bpath=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mpath=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
code='PED:UAE-MOE-MATH:L4:COMMON:02:03'
title='الجمع والطرح الذهني'
C=load(cpath);B=load(bpath);M=load(mpath)
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
bl=next(x for x in B['Lessons'] if x['LessonCode']==code)
assert l['LessonCode']==code and bl['LessonCode']==code
source='04-01.pdf, Unit 2 Lesson 3, printed pp73-78, PDF pp96-101'
t={'CultureCode':'ar','Title':title,
'Explanation':'نستطيع الجمع والطرح ذهنيًا باستعمال قيم المنازل وتكوين العشرات والمئات والتعويض. عندما نزيد عددًا قليلًا لتحويله إلى عدد سهل، يجب أن نطرح الزيادة من النتيجة أو نعوّضها بصورة مكافئة. وعند الطرح يمكن تجزئة المطروح إلى أجزاء أو تكوين أقرب عشرة أو مئة ثم إيجاد الفرق.',
'KeyConceptsAndRules':'كوّن عشرة أو مئة عند جمع عددين. في التعويض: أ + ب = (أ + ١) + (ب − ١). لتسهيل طرح عدد قريب من عشرة: أ − (ب − ١) = أ − ب + ١. اطرح الألوف من الألوف في الأعداد ذات الأصفار المشتركة.',
'WorkedExamples':'١. ٥٧ + ٥٨ = ٥٥ + ٦٠ = ١١٥ (نقل ٢). ٢. ٤٩٩ + ٧٧ = ٥٠٠ + ٧٦ = ٥٧٦. ٣. ١٨٤ − ٧٦ = (١٨٤ − ٨٠) + ٤ = ١٠٨. ٤. ٨٢٬٠٠٠ − ٧٦٬٠٠٠ = (٨٢ − ٧٦) ألفًا = ٦٬٠٠٠.',
'StepByStepSolutions':'١. اقرأ العددين وحدد القيمة المكانية المشتركة. ٢. اختر جمعًا مريحًا، أو أعد تجزئة المطروح، أو انتقل لأقرب مئة. ٣. احسب ذهنيًا مع التعويض عن أي زيادة أو نقصان. ٤. تحقق بالتقدير أو العملية العكسية.',
'CommonMistakes':'عند تحويل ٤٩٩ إلى ٥٠٠ لا ننسى إنقاص الحد الآخر بمقدار ١؛ في ١٨٤ − ٧٦ إذا طرحنا ٨٠ نعيد إضافة ٤؛ ٨٢ ألفًا ناقص ٧٦ ألفًا يساوي ٦ آلاف وليس ٦.',
'QuickSummary':'أستخدم الحساب الذهني باستراتيجيات تكوين العشرة أو المئة والتعويض وتجزيء العدد، وأفسر سبب تساوي النتيجتين.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l['TitleSourceReference']=source
l['SourceLocator']=source
l['SourceTitle']='UAE Grade 4 Arabic student mathematics General/Common'
l['SourceEdition']='Owner selected student text, year not blocking'
l['SourceSha256']='859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3'
l['CanonicalBodySha256']=hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest()
l['AdaptationStatus']='Original Arabic equivalents for owner selected student book; Stage D pending'
bl['Title']=title; bl['UnitTitle']='جمع الأعداد الكلية وطرحها';B['Units'][1]['Title']='جمع الأعداد الكلية وطرحها'
M['Units'][1]['title']='جمع الأعداد الكلية وطرحها';M['Units'][1]['ls'][2]=title
save(cpath,C);save(bpath,B);save(mpath,M)
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'
s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit2Lesson02PatternsQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1
s=s.replace(a,a+'\n          UaeGrade4Unit2Lesson03MentalQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit2Lesson02PatternsQuestions.Supports(family))\n            return UaeGrade4Unit2Lesson02PatternsQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit2Lesson03MentalQuestions.Supports(family))\n            return UaeGrade4Unit2Lesson03MentalQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit2Lesson02PatternsQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit2Lesson02PatternsQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit2Lesson03MentalQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit2Lesson03MentalQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit2Lesson02PatternsQuestions.Supports(problem.Family))\n            return UaeGrade4Unit2Lesson02PatternsQuestions.Solve(problem.Family, p);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit2Lesson03MentalQuestions.Supports(problem.Family))\n            return UaeGrade4Unit2Lesson03MentalQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig')
marker='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(marker)==1
families=['uae.g4.u02.l03.make_friendly_ten','uae.g4.u02.l03.compensate_addition','uae.g4.u02.l03.break_apart_subtraction','uae.g4.u02.l03.subtract_thousands']
new='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:02:03"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:02:03",
            "number.mental_addition_subtraction",
            "MENTAL_FRIENDLY_NUMBER_COMPENSATION_PLACE_VALUE",
            ['''+', '.join('"'+f+'"' for f in families)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP73_78_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
s=s.replace(marker,new+marker)
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
j=load(p)
template=next(x for x in j['families'] if x['id']=='uae.g4.u02.l02.add_place_step')
for fid in families:
    assert not any(x['id']==fid for x in j['families'])
    e=dict(template)
    e.update({'id':fid,'skillId':'number.mental_addition_subtraction','answerType':'exact_integer',
      'verificationPolicy':'g4-u02-l03-student-source-stage-d-not-run','representations':['symbolic','place_value_chart'],
      'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(e)
save(p,j)
print('PASS: G4 U02 L03 Arabic lesson and four exact practice families integrated')
