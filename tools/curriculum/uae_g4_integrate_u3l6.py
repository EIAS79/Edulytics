import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:03:06';title='خاصية التجميع في الضرب';unit='فهم الضرب والقسمة'
l=next(x for x in C['Lessons'] if x['LessonCode']==code);b=next(x for x in B['Lessons'] if x['LessonCode']==code)
t={'CultureCode':'ar','Title':title,
'Explanation':'عندما نضرب ثلاثة عوامل أو أكثر يمكن تغيير طريقة تجميع عاملين دون تغيير الناتج. تسمح خاصية التجميع في الضرب باختيار الزوج الذي ينتج عددًا يسهل ضربه ذهنيًا.',
'KeyConceptsAndRules':'(أ × ب) × ج = أ × (ب × ج). تتغير الأقواس فقط وليس ترتيب العوامل في خاصية التجميع. مثال: (٩ × ٢) × ٤ = ٩ × (٢ × ٤) = ٧٢.',
'WorkedExamples':'١. ٩ × ٢ × ٤ = ٩ × (٢ × ٤) = ٩ × ٨ = ٧٢. ٢. ٣ × ٤ × ٢ = (٣ × ٤) × ٢ = ١٢ × ٢ = ٢٤. ٣. ٥ × ٣ × ٣ = ٥ × ٩ = ٤٥.',
'StepByStepSolutions':'١. أحدد العوامل الثلاثة. ٢. أختار زوجًا يسهل ضربه أولًا وأضعه بين قوسين. ٣. أحسب حاصل ضرب هذا الزوج. ٤. أضرب الناتج في العامل الأخير وأتحقق بطريقة تجميع أخرى.',
'CommonMistakes':'تغيير عامل بدل الأقواس؛ الخلط بين تبديل العاملين وتجميعهما؛ نسيان ضرب العامل الثالث بعد حساب الزوج؛ افتراض جواز تطبيق التجميع ذاته على الطرح أو القسمة.',
'QuickSummary':'خاصية التجميع تسمح بتغيير الأقواس في ضرب ثلاثة عوامل دون تغيير الناتج، مما يسهل الضرب الذهني.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l['SourceTitle']='Owner-selected UAE Grade 4 general Arabic student book'
l['SourceLocator']='04-01.pdf U03 L06 printed 167-172 PDF 190-195'
l['TitleSourceReference']=l['SourceLocator'];l['SourceEdition']='Student source; edition nonblocking'
l['SourceSha256']='859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3'
l['CanonicalBodySha256']=hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest()
l['AdaptationStatus']='Original Arabic math equivalent; Stage D pending'
b['Title']=title;b['UnitTitle']=unit;B['Units'][2]['Title']=unit
M['Units'][2]['title']=unit;M['Units'][2]['ls'][5]=title
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'
s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit3Lesson05PropertiesQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1;s=s.replace(a,a+'\n          UaeGrade4Unit3Lesson06AssociativeQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit3Lesson05PropertiesQuestions.Supports(family))\n            return UaeGrade4Unit3Lesson05PropertiesQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit3Lesson06AssociativeQuestions.Supports(family))\n            return UaeGrade4Unit3Lesson06AssociativeQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit3Lesson05PropertiesQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit3Lesson05PropertiesQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit3Lesson06AssociativeQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit3Lesson06AssociativeQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit3Lesson05PropertiesQuestions.Supports(problem.Family))\n            return UaeGrade4Unit3Lesson05PropertiesQuestions.Solve(problem.Family, p);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit3Lesson06AssociativeQuestions.Supports(problem.Family))\n            return UaeGrade4Unit3Lesson06AssociativeQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig');anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u03.l06.triple_product','uae.g4.u03.l06.missing_factor','uae.g4.u03.l06.grouped_product']
contract='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:03:06"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:03:06", "number.associative_multiplication",
            "THREE_FACTOR_ASSOCIATIVE_PROPERTY",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP167_172_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,contract+anchor),encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
j=load(p);template=next(x for x in j['families'] if x['id']=='uae.g4.u03.l05.identity')
for x in f:
    assert not any(e['id']==x for e in j['families'])
    z=dict(template);z.update({'id':x,'skillId':'number.associative_multiplication','answerType':'exact_integer','verificationPolicy':'g4-u03-l06-student-source-stage-d-not-run','representations':['equation'],'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(z)
save(p,j)
print('PASS U03 L06 three associativity question families integrated')
