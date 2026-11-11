import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:06:01'
title='قسمة مضاعفات الأعداد ١٠ و١٠٠ و١٠٠٠';unit='القسمة على عدد مكون من رقم واحد'
l=next(x for x in C['Lessons'] if x['LessonCode']==code);b=next(x for x in B['Lessons'] if x['LessonCode']==code)
assert b['SortOrder']==41
t={'CultureCode':'ar','Title':title,'Explanation':'أستخدم حقائق الضرب والقسمة الأساسية وأنماط مضاعفات العشرة والمئة والألف لإيجاد نواتج القسمة دون عد متكرر. أحافظ على القيمة المكانية: إذا كان ١٥ ÷ ٥ = ٣، فإن ١٥٠ ÷ ٥ = ٣٠، و١٥٠٠ ÷ ٥ = ٣٠٠. أتحقق بالضرب.',
'KeyConceptsAndRules':'إذا كان أ÷ب=ج، فإن (أ×١٠)÷ب=ج×١٠، وكذلك المئات والألوف. مثال ٢٤٠٠÷٤=٦٠٠، لأن ٢٤÷٤=٦ ثم أضرب الناتج في ١٠٠. ٣٦٠٠÷٩=٤٠٠.',
'WorkedExamples':'١. ١٥٠٠÷٥=٣٠٠. ٢. ٢٤٠٠÷٤=٦٠٠. ٣. ١٢٠٠÷٤=٣٠٠. ٤. ٣٦٠٠÷٩=٤٠٠. ٥. ٤٢٠٠÷٧=٦٠٠.',
'StepByStepSolutions':'١. أحدد حقيقة القسمة الأساسية للأرقام غير الصفرية. ٢. أحدد ما إذا كان المقسوم يمثل عشرات أو مئات أو آلاف. ٣. أوجد حاصل قسمة الحقائق الأساسية ثم أطبق نفس القيمة المكانية على الناتج. ٤. أتحقق بضرب ناتج القسمة في المقسوم عليه.',
'CommonMistakes':'حذف جميع الأصفار دون إدراك قيمة المنزلة؛ الخلط بين قسمة ١٥٠ و١٥٠٠ على ٥؛ عدم التحقق بالضرب؛ تجاهل عدد المجموعات المتساوية في المسألة.',
'QuickSummary':'أستعمل العلاقة بين القسمة والضرب وأنماط القيمة المكانية، وأتحقق بالعملية العكسية.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l.update({'SourceTitle':'UAE Grade 4 Arabic student mathematics book','SourceLocator':'04-01.pdf U06 L01 printed pp329-334 PDF352-357','TitleSourceReference':'04-01.pdf U06 L01 printed pp329-334 PDF352-357','SourceEdition':'Student-book authority; date nonblocking','SourceSha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3','CanonicalBodySha256':hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest(),'AdaptationStatus':'Original Arabic equivalent; Stage D pending'})
b['Title']=title;b['UnitTitle']=unit;B['Units'][5]['Title']=unit
M['Units'][5]['title']=unit;M['Units'][5]['ls'][0]=title
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs';s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit5Lesson01TensQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1;s=s.replace(a,a+'\n          UaeGrade4Unit6Lesson01DivisionPowersQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit5Lesson01TensQuestions.Supports(family))\n            return UaeGrade4Unit5Lesson01TensQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit6Lesson01DivisionPowersQuestions.Supports(family))\n            return UaeGrade4Unit6Lesson01DivisionPowersQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit5Lesson01TensQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit5Lesson01TensQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit6Lesson01DivisionPowersQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit6Lesson01DivisionPowersQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit5Lesson01TensQuestions.Supports(problem.Family))\n            return UaeGrade4Unit5Lesson01TensQuestions.Solve(problem.Family, p);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit6Lesson01DivisionPowersQuestions.Supports(problem.Family))\n            return UaeGrade4Unit6Lesson01DivisionPowersQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig');anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u06.l01.divide_powers_10_100_1000','uae.g4.u06.l01.missing_quotient_place_value']
cstr='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:06:01"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:06:01", "number.divide_multiples_by_one_digit",
            "DIVIDE_MULTIPLES_OF_10_100_1000",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP329_334_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,cstr+anchor),encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json';j=load(p)
template=next(x for x in j['families'] if x['id']=='uae.g4.u05.l01.tens_product')
for x in f:
    assert not any(e['id']==x for e in j['families'])
    z=dict(template);z.update({'id':x,'skillId':'number.divide_multiples_by_one_digit','answerType':'exact_integer','verificationPolicy':'g4-u06-l01-student-source-stage-d-not-run','representations':['equation','place_value'],'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(z)
save(p,j)
rich={'SchemaVersion':1,'ContentVersion':'uae-g4-u06-l01-ar-20261111','PackCode':'UAE-MOE-MATH','Lessons':[{'LessonCode':code,'CultureCode':'ar','Title':title,
'ExplanationParagraphs':['أستخدم حقائق القسمة الأساسية ونمط مضاعفات العشرات والمئات والألوف.','إن قسمة ١٥٠٠ على ٥ تشبه ١٥÷٥، لكن الناتج يجب أن يكون ١٠٠ مرة أكبر.','أراجع نتيجة القسمة بالضرب في المقسوم عليه.'],
'KeyConcepts':[{'Title':'علاقة الضرب بالقسمة','Definition':'القسمة تعكس الضرب.','Rule':'إذا ب×ج=أ، فإن أ÷ب=ج.','Example':'٤×٦٠٠=٢٤٠٠.'},{'Title':'القيمة المكانية','Definition':'حقيقة القسمة تتكرر مع زيادة عدد الأصفار والقيمة المكانية.','Rule':'(أ×١٠٠)÷ب = (أ÷ب)×١٠٠.','Example':'٢٤٠٠÷٤=٦٠٠.'},{'Title':'التحقق','Definition':'أستخدم العملية العكسية لمراجعة الناتج.','Rule':'ناتج القسمة × المقسوم عليه = المقسوم.','Example':'٦٠٠×٤=٢٤٠٠.'}],
'WorkedExamples':[{'Title':'رحلة القطار','Question':'١٥٠٠ شخص في خمسة قطارات متساوية، كم شخصًا في كل قطار؟','Method':'حقائق الضرب والقيمة المكانية.','Steps':['١٥÷٥=٣.','١٥٠٠=١٥×١٠٠.','٣×١٠٠=٣٠٠.'],'Answer':'٣٠٠ شخص','Check':'٥×٣٠٠=١٥٠٠.'},{'Title':'تقسيم ٢٤٠٠','Question':'أوجد ٢٤٠٠÷٤.','Method':'النمط من ٢٤ إلى ٢٤٠٠.','Steps':['٢٤÷٤=٦.','٢٤٠٠ هو ١٠٠×٢٤.','٦×١٠٠=٦٠٠.'],'Answer':'٦٠٠','Check':'٦٠٠×٤=٢٤٠٠.'},{'Title':'تقسيم ٣٦٠٠','Question':'أوجد ٣٦٠٠÷٩.','Method':'حقيقة ٣٦÷٩.','Steps':['٣٦÷٩=٤.','٣٦٠٠=٣٦×١٠٠.','الناتج ٤٠٠.'],'Answer':'٤٠٠','Check':'٤٠٠×٩=٣٦٠٠.'}],
'CommonMistakes':[{'Mistake':'إهمال المئات أو الألوف في المقسوم.','WhyWrong':'يقل الناتج بعامل عشرة أو مئة.','Correction':'أحدد القيمة المكانية قبل حل المسألة.'},{'Mistake':'عدم التحقق بضرب الناتج.','WhyWrong':'لا يكتشف الخطأ في قيمة الأصفار.','Correction':'أضرب الناتج في المقسوم عليه.'}],
'SummaryPoints':['أحدد حقيقة القسمة الأساسية.','أستخدم القيمة المكانية لمضاعفات عشرة ومئة وألف.','أتحقق بالضرب.'],
'Visuals':[{'Kind':'EquationSet','Title':'أنماط قسمة مضاعفات المئات','Description':'تدرج حسابي من حقيقة أساسية إلى مضاعف ألف.','Items':['١٥ ÷ ٥ = ٣','١٥٠ ÷ ٥ = ٣٠','١٥٠٠ ÷ ٥ = ٣٠٠']}],'Videos':[]}]}
save(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u06-l01-ar.rich-lesson-v2.json',rich)
print('PASS U06L01 division multiples integrated')
