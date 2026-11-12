import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:04:02'
title='التقريب لتقدير ناتج الضرب'
unit='الضرب في الأعداد المكونة من رقم واحد'
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
t={'CultureCode':'ar','Title':title,
'Explanation':'يمكن تقدير حاصل ضرب عدد أحادي الرقم بعدد مكون من ثلاثة أو أربعة أرقام بتقريب العدد الأكبر إلى أكبر قيمة مكانية. أضرب العدد المقرب في العامل الآخر وأتحقق من كون التقدير أعلى أو أدنى من الناتج الدقيق.',
'KeyConceptsAndRules':'عند تقريب ٢٤٩٦ إلى أقرب ألف نحصل على ٢٠٠٠، ولذلك ٨ × ٢٤٩٦ ≈ ٨ × ٢٠٠٠ = ١٦٠٠٠. وعند تقريب ٨٥٦٢ لأقرب ألف نحصل على ٩٠٠٠، و٣ × ٨٥٦٢ ≈ ٢٧٠٠٠. التقدير الأعلى من العدد الأصلي يعطي ناتجًا أعلى عند الضرب في عدد موجب.',
'WorkedExamples':'١. ٨ × ٢٤٩٦ ≈ ٨ × ٢٠٠٠ = ١٦٠٠٠، وهذا أقل من الناتج الحقيقي. ٢. ٣ × ٨٥٦٢ ≈ ٣ × ٩٠٠٠ = ٢٧٠٠٠، وهذا أكبر من الناتج الدقيق. ٣. ٦ × ٥٦٢ ≈ ٦ × ٦٠٠ = ٣٦٠٠. ٤. ٤ × ٧٥٦ ≈ ٤ × ٨٠٠ = ٣٢٠٠.',
'StepByStepSolutions':'١. أحدد العامل الذي يتكون من عدة أرقام. ٢. أقرّبه إلى أقرب أكبر قيمة مكانية له. ٣. أضرب العامل الآخر في العدد المقرب. ٤. أحدد إذا كان التقدير أكبر من حاصل الضرب الفعلي أم أصغر منه وأفسّر.',
'CommonMistakes':'التقريب إلى عشرات عند المطلوب أكبر قيمة مكانية؛ تجاهل قاعدة التقريب عند الرقم خمسة؛ مقارنة التقدير بالعدد الأصلي لا بحاصل الضرب الدقيق؛ افتراض أن كل تقدير أكبر من الفعلي.',
'QuickSummary':'أقدّر حاصل الضرب بتقريب العامل الأكبر إلى أكبر قيمة مكانية، ثم أضرب وأقارن التقدير بالحقيقة.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l['SourceTitle']='UAE Grade 4 Common Arabic student book';l['SourceLocator']='04-01.pdf U04 L02 printed pp203-208 PDF 226-231'
l['TitleSourceReference']=l['SourceLocator'];l['SourceEdition']='Student source; edition nonblocking'
l['SourceSha256']='859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3'
l['CanonicalBodySha256']=hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest()
l['AdaptationStatus']='First party Arabic equivalent; Stage D pending'
b['Title']=title;b['UnitTitle']=unit;B['Units'][3]['Title']=unit
M['Units'][3]['title']=unit;M['Units'][3]['ls'][1]=title
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'
s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit4Lesson01MultiplesOfPowersOfTenQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1;s=s.replace(a,a+'\n          UaeGrade4Unit4Lesson02EstimateProductsQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit4Lesson01MultiplesOfPowersOfTenQuestions.Supports(family))\n            return UaeGrade4Unit4Lesson01MultiplesOfPowersOfTenQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit4Lesson02EstimateProductsQuestions.Supports(family))\n            return UaeGrade4Unit4Lesson02EstimateProductsQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit4Lesson01MultiplesOfPowersOfTenQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit4Lesson01MultiplesOfPowersOfTenQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit4Lesson02EstimateProductsQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit4Lesson02EstimateProductsQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit4Lesson01MultiplesOfPowersOfTenQuestions.Supports(problem.Family))\n            return UaeGrade4Unit4Lesson01MultiplesOfPowersOfTenQuestions.Solve(problem.Family, p);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit4Lesson02EstimateProductsQuestions.Supports(problem.Family))\n            return UaeGrade4Unit4Lesson02EstimateProductsQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig')
anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u04.l02.estimate_rounded_product','uae.g4.u04.l02.estimate_above_below']
c='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:04:02"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:04:02", "number.rounding_to_estimate_products",
            "ROUND_GREATEST_PLACE_COMPARE_ESTIMATE_TO_ACTUAL",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP203_208_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,c+anchor),encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
j=load(p);template=next(x for x in j['families'] if x['id']=='uae.g4.u04.l01.multiply_power_of_ten')
for x in f:
    assert not any(e['id']==x for e in j['families'])
    z=dict(template);z.update({'id':x,'skillId':'number.rounding_to_estimate_products','answerType':'exact_integer','verificationPolicy':'g4-u04-l02-student-source-stage-d-not-run','representations':['contextual','equation'],'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(z)
save(p,j)
print('PASS U04 L02 student title and two estimate family routes')
