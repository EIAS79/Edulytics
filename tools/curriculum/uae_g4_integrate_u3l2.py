import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:03:02'
title='علاقة القسمة بالطرح'
unit='فهم الضرب والقسمة'
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
t={'CultureCode':'ar','Title':title,
'Explanation':'القسمة يمكن فهمها بالطرح المتكرر للمقدار نفسه من عدد كلي. عندما نبدأ بالمقسوم ونطرح المقسوم عليه مرة بعد أخرى حتى نصل إلى الصفر، فإن عدد مرات الطرح هو خارج القسمة ما دامت القسمة بلا باقٍ.',
'KeyConceptsAndRules':'المقسوم ÷ المقسوم عليه = خارج القسمة. إذا كان ١٢ ÷ ٤ = ٣ فإن ١٢ − ٤ − ٤ − ٤ = ٠؛ عدد مرات الطرح ثلاث. يمكن تمثيل الطرح المتكرر على خط أعداد بقفزات متساوية.',
'WorkedExamples':'١. ١٢ ÷ ٤ = ٣ لأن ١٢ − ٤ = ٨، ٨ − ٤ = ٤، ٤ − ٤ = ٠. ٢. ١٠ ÷ ٢ = ٥: نطرح اثنين خمس مرات حتى الصفر. ٣. ٢٧ ÷ ٩ = ٣ عبر ثلاث قفزات متساوية من ٢٧ إلى الصفر.',
'StepByStepSolutions':'١. أبدأ بالمقسوم عند الطرف الأيمن لخط الأعداد. ٢. أقفز يسارًا بقيمة المقسوم عليه في كل خطوة. ٣. أستمر حتى الصفر. ٤. أعدّ القفزات لتحديد خارج القسمة وأتحقق بالضرب.',
'CommonMistakes':'اعتبار الرقم الذي يصل إليه الطرح هو خارج القسمة بدل عدد مرات الطرح؛ عدم طرح المقدار نفسه في كل خطوة؛ التوقف قبل الوصول إلى صفر في حالة القسمة التامة؛ الخلط بين ١٢ ÷ ٤ و١٢ − ٤.',
'QuickSummary':'يمكنني تمثيل القسمة التامة بالطرح المتكرر على خط الأعداد؛ وخارج القسمة هو عدد مرات الطرح المتساوي.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l['SourceTitle']='UAE Grade 4 Common Arabic student book'
l['SourceLocator']='04-01.pdf U03 L02 printed pp141-146 PDF 164-169'
l['TitleSourceReference']=l['SourceLocator']
l['SourceEdition']='Student-book authority, year nonblocking'
l['SourceSha256']='859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3'
l['CanonicalBodySha256']=hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest()
l['AdaptationStatus']='Original Arabic mathematical equivalent; Stage D not run'
b['Title']=title;b['UnitTitle']=unit
B['Units'][2]['Title']=unit
M['Units'][2]['title']=unit;M['Units'][2]['ls'][1]=title
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'
s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit3Lesson01FactFamiliesQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1
s=s.replace(a,a+'\n          UaeGrade4Unit3Lesson02RepeatedSubtractionQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit3Lesson01FactFamiliesQuestions.Supports(family))\n            return UaeGrade4Unit3Lesson01FactFamiliesQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit3Lesson02RepeatedSubtractionQuestions.Supports(family))\n            return UaeGrade4Unit3Lesson02RepeatedSubtractionQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit3Lesson01FactFamiliesQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit3Lesson01FactFamiliesQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit3Lesson02RepeatedSubtractionQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit3Lesson02RepeatedSubtractionQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit3Lesson01FactFamiliesQuestions.Supports(problem.Family))\n            return UaeGrade4Unit3Lesson01FactFamiliesQuestions.Solve(problem.Family, p);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit3Lesson02RepeatedSubtractionQuestions.Supports(problem.Family))\n            return UaeGrade4Unit3Lesson02RepeatedSubtractionQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig')
anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u03.l02.count_subtraction_jumps','uae.g4.u03.l02.equal_groups_quotient','uae.g4.u03.l02.reconstruct_dividend']
contract='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:03:02"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:03:02",
            "number.division_as_repeated_subtraction",
            "EQUAL_DECREMENT_NUMBERLINE_DIVISION",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP141_146_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,contract+anchor),encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
j=load(p)
template=next(x for x in j['families'] if x['id']=='uae.g4.u03.l01.array_quotient')
for x in f:
    assert not any(e['id']==x for e in j['families'])
    z=dict(template)
    z.update({'id':x,'skillId':'number.division_as_repeated_subtraction','answerType':'exact_integer',
       'verificationPolicy':'g4-u03-l02-local-student-stage-d-not-run',
       'representations':['number_line','symbolic'],'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(z)
save(p,j)
print('PASS Grade4 U03 L02 three exact repeated-subtraction families integrated')
