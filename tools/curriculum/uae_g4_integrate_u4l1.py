import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:04:01'
title='ضرب مضاعفات الأعداد ١٠ و١٠٠ و١٠٠٠'
unit='الضرب في الأعداد المكونة من رقم واحد'
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
t={'CultureCode':'ar','Title':title,
'Explanation':'أستخدم الحقائق الأساسية في الضرب وأنماط القيمة المكانية لأضرب الأعداد المكونة من رقم واحد في مضاعفات ١٠ و١٠٠ و١٠٠٠. عندما أضرب ٦ × ٨ = ٤٨، يكون ٦ × ٨٠ = ٤٨٠، و٦ × ٨٠٠ = ٤٨٠٠، و٦ × ٨٠٠٠ = ٤٨٠٠٠.',
'KeyConceptsAndRules':'إذا كان أ × ب = ج، فإن أ × (ب × ١٠) = ج × ١٠، وأ × (ب × ١٠٠) = ج × ١٠٠، وأ × (ب × ١٠٠٠) = ج × ١٠٠٠. أستعمل القيمة المكانية ولا أتعامل مع الأصفار وحدها كقاعدة منفصلة.',
'WorkedExamples':'١. ٥ × ٦٠٠ = ٣٠٠٠ لأن ٥ × ٦ = ٣٠ ثم ٣٠ مئة. ٢. ٨ × ٢٠٠٠ = ١٦٠٠٠ لأن ٨ × ٢ = ١٦ ثم ١٦ ألفًا. ٣. ٧ × ٥٠٠٠ = ٣٥٠٠٠. ٤. ٦ × □ = ٣٦٠٠؛ المجهول ٦٠٠.',
'StepByStepSolutions':'١. أتعرف الرقم الأساسي وقيمة منزلة الرقم. ٢. أضرب الأرقام الأساسية أولًا. ٣. أضرب الناتج في ١٠ أو ١٠٠ أو ١٠٠٠ بحسب القيمة المكانية. ٤. أتحقق بمقارنة النتيجة بنمط مضاعفات العشرة أو بالقسمة العكسية.',
'CommonMistakes':'تجاهل الأصفار التي تحدد القيمة المكانية؛ الخلط بين ٦ × ٨٠ و٦ × ٨٠٠؛ التعامل مع عدد الأصفار دون فهم قيمتها؛ إغفال الصفر في نتيجة مثل ٨ × ٥٠٠ = ٤٠٠٠.',
'QuickSummary':'أستعمل حقيقة الضرب الأساسية والقيمة المكانية لحساب مضاعفات ١٠ و١٠٠ و١٠٠٠ وأتحقق بالأنماط.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l['SourceTitle']='UAE Grade 4 General Arabic Student Mathematics';l['SourceLocator']='04-01.pdf U04 L01 printed pp197-202 PDF 220-225'
l['TitleSourceReference']=l['SourceLocator'];l['SourceEdition']='Student text authority; year nonblocking'
l['SourceSha256']='859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3'
l['CanonicalBodySha256']=hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest()
l['AdaptationStatus']='Original Arabic mathematical equivalents; Stage D pending'
b['Title']=title;b['UnitTitle']=unit;B['Units'][3]['Title']=unit
M['Units'][3]['title']=unit;M['Units'][3]['ls'][0]=title
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'
s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit3Lesson08ReasonablenessQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1;s=s.replace(a,a+'\n          UaeGrade4Unit4Lesson01MultiplesOfPowersOfTenQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit3Lesson08ReasonablenessQuestions.Supports(family))\n            return UaeGrade4Unit3Lesson08ReasonablenessQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit4Lesson01MultiplesOfPowersOfTenQuestions.Supports(family))\n            return UaeGrade4Unit4Lesson01MultiplesOfPowersOfTenQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit3Lesson08ReasonablenessQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit3Lesson08ReasonablenessQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit4Lesson01MultiplesOfPowersOfTenQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit4Lesson01MultiplesOfPowersOfTenQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit3Lesson08ReasonablenessQuestions.Supports(problem.Family))\n            return UaeGrade4Unit3Lesson08ReasonablenessQuestions.Solve(problem.Family, p);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit4Lesson01MultiplesOfPowersOfTenQuestions.Supports(problem.Family))\n            return UaeGrade4Unit4Lesson01MultiplesOfPowersOfTenQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig')
anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u04.l01.multiply_power_of_ten','uae.g4.u04.l01.unknown_factor_power_ten','uae.g4.u04.l01.compare_powers_ten']
c='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:04:01"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:04:01", "number.multiply_multiples_ten",
            "MULTIPLY_SINGLE_DIGIT_BY_10_100_1000_MULTIPLES",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP197_202_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,c+anchor),encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
j=load(p);template=next(x for x in j['families'] if x['id']=='uae.g4.u03.l08.sum_estimate')
for x in f:
    assert not any(e['id']==x for e in j['families'])
    z=dict(template);z.update({'id':x,'skillId':'number.multiply_multiples_ten','answerType':'exact_integer','verificationPolicy':'g4-u04-l01-student-source-stage-d-not-run','representations':['equation','place_value'],'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(z)
save(p,j)
print('PASS U04 L01 title, source, three exact families')
