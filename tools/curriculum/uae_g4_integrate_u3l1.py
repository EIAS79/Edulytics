import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:03:01'
title='العلاقة بين الضرب والقسمة'
unit='فهم الضرب والقسمة'
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
t={'CultureCode':'ar','Title':title,
'Explanation':'الضرب والقسمة عمليتان مترابطتان وعكسيتان. تمثل المصفوفة صفوفًا وأعمدة متساوية وتبين حاصل ضرب عدد الصفوف في عدد العناصر بكل صف. عندما نعرف المجموع وعدد الصفوف نقسم لمعرفة العناصر في الصف، أو العكس.',
'KeyConceptsAndRules':'من ثلاثة أعداد مترابطة أ وب وأ×ب، نصنع عائلة حقائق تتكون من جملتي ضرب وجملتي قسمة. مثال: ٣ × ٤ = ١٢، ٤ × ٣ = ١٢، ١٢ ÷ ٣ = ٤، ١٢ ÷ ٤ = ٣. نستعمل حقيقة الضرب للتحقق من ناتج القسمة.',
'WorkedExamples':'١. مصفوفة من ٣ صفوف و٤ أعمدة تضم ١٢ عنصرًا. ٢. إذا وزعنا ٣٦ كتابًا بالتساوي على أربعة رفوف، فإن ٣٦ ÷ ٤ = ٩؛ ويمكن التحقق بـ٤ × ٩ = ٣٦. ٣. من ٦ و٩ و٥٤: ٦ × ٩ = ٥٤، ٩ × ٦ = ٥٤، ٥٤ ÷ ٦ = ٩، ٥٤ ÷ ٩ = ٦.',
'StepByStepSolutions':'١. أحدد العدد الكلي وعدد المجموعات المتساوية. ٢. أرسم مصفوفة أو مجموعات إذا لزم. ٣. أكتب جملة الضرب التي تعبر عن الصفوف والأعمدة. ٤. أستعمل جملة قسمة عكسية لإيجاد عدد مجهول. ٥. أتحقق بالعملية الأخرى.',
'CommonMistakes':'خلط عدد الصفوف بعدد العناصر الكلية؛ نسيان أن ناتج الضرب هو المقسوم في عائلة الحقائق؛ تقسيم عدد إلى مجموعات غير متساوية؛ القسمة على الصفر غير معرفة.',
'QuickSummary':'أمثل مجموعات متساوية بمصفوفة، وأكتب عائلة الحقائق من جملتي ضرب وجملتي قسمة، وأتحقق بالعكس.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l['SourceTitle']='UAE Grade 4 Arabic Student Book General'
l['SourceLocator']='04-01.pdf U03 L01 printed pp135-140 PDF 158-163'
l['TitleSourceReference']=l['SourceLocator']
l['SourceEdition']='Owner selected student text, year nonblocking'
l['SourceSha256']='859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3'
l['CanonicalBodySha256']=hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest()
l['AdaptationStatus']='Original Arabic equivalent, Stage D pending'
b['Title']=title;b['UnitTitle']=unit
B['Units'][2]['Title']=unit
M['Units'][2]['title']=unit;M['Units'][2]['ls'][0]=title
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'
s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit2Lesson09MultiStepQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1
s=s.replace(a,a+'\n          UaeGrade4Unit3Lesson01FactFamiliesQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit2Lesson09MultiStepQuestions.Supports(family))\n            return UaeGrade4Unit2Lesson09MultiStepQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit3Lesson01FactFamiliesQuestions.Supports(family))\n            return UaeGrade4Unit3Lesson01FactFamiliesQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit2Lesson09MultiStepQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit2Lesson09MultiStepQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit3Lesson01FactFamiliesQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit3Lesson01FactFamiliesQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit2Lesson09MultiStepQuestions.Supports(problem.Family))\n            return UaeGrade4Unit2Lesson09MultiStepQuestions.Solve(problem.Family, p);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit3Lesson01FactFamiliesQuestions.Supports(problem.Family))\n            return UaeGrade4Unit3Lesson01FactFamiliesQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig')
anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u03.l01.array_product','uae.g4.u03.l01.array_quotient','uae.g4.u03.l01.unknown_factor','uae.g4.u03.l01.inverse_fact']
contract='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:03:01"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:03:01",
            "number.multiplication_division_fact_families",
            "MULTIPLICATION_DIVISION_INVERSE_ARRAY_FACTS",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP135_140_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,contract+anchor),encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
j=load(p)
template=next(x for x in j['families'] if x['id']=='uae.g4.u02.l09.add_then_subtract')
for x in f:
    assert not any(e['id']==x for e in j['families'])
    z=dict(template)
    z.update({'id':x,'skillId':'number.multiplication_division_fact_families','answerType':'exact_integer',
       'verificationPolicy':'g4-u03-l01-local-student-source-stage-d-not-run',
       'representations':['array','equation'],'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(z)
save(p,j)
print('PASS integrated U03 L01 four fact-family question types')
