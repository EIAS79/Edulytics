import pathlib,json,copy,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
prev='PED:UAE-MOE-MATH:L4:COMMON:04:08';code='PED:UAE-MOE-MATH:L4:COMMON:04:09'
assert not any(x['LessonCode']==code for x in C['Lessons']) and not any(x['LessonCode']==code for x in B['Lessons'])
assert B['Units'][3]['LessonCount']==8
title='الضرب في عدد متعدد الأرقام'
t={'CultureCode':'ar','Title':title,
'Explanation':'أستخدم نواتج الضرب الجزئية والطريقة الرأسية لضرب عدد من ثلاثة أو أربعة أرقام في عدد من رقم واحد. أضرب الآحاد ثم العشرات فالمئات فالألوف، وأعيد تجميع كل منزلة عند الضرورة. أتحقق من صحة الناتج بالتقدير وبجمع نواتج الضرب الجزئية.',
'KeyConceptsAndRules':'٩ × ٣٦٥ = ٩ × (٣٠٠ + ٦٠ + ٥) = ٢٧٠٠ + ٥٤٠ + ٤٥ = ٣٢٨٥. وأيضًا ٣ × ٢٧٦٣ = ٣ × (٢٠٠٠ + ٧٠٠ + ٦٠ + ٣) = ٦٠٠٠ + ٢١٠٠ + ١٨٠ + ٩ = ٨٢٨٩.',
'WorkedExamples':'١. ٩ × ٣٦٥ = ٣٢٨٥. ٢. ٣ × ١١٧٥ = ٣٥٢٥. ٣. ٣ × ٢٧٦٣ = ٨٢٨٩. ٤. ٦ × ٥٣٢ = ٣١٩٢. ٥. ٨ × ٦٣٢٨ = ٥٠٦٢٤.',
'StepByStepSolutions':'١. أقدر الناتج بتقريب العدد الكبير. ٢. أضرب الآحاد وأعيد تجميع العشرات. ٣. أضرب العشرات والمئات والألوف مع إضافة المحمول لكل منزلة. ٤. أتحقق بإيجاد الضرب الجزئي لكل منزلة ثم جمع النتائج.',
'CommonMistakes':'نسيان المحمول أثناء الانتقال من المئات إلى الألوف؛ إهمال منزلة الصفر في العدد؛ عدم جمع النواتج الجزئية في قيمتها المكانية؛ مقارنة الناتج بالتقدير دون مراعاة اتجاه التقريب.',
'QuickSummary':'أضرب من اليمين إلى اليسار مع إعادة التجميع، وأتحقق بالتوزيع والتقدير.'}
c=copy.deepcopy(next(x for x in C['Lessons'] if x['LessonCode']==prev))
c.update({'LessonCode':code,'OfficialReferenceCode':'UAE:REF:TEXTBOOK:G4:COMMON:T1:U04:L09',
'SourceLocator':'04-01.pdf U04 L09 printed pp247-252 PDF 270-275',
'TitleSourceReference':'04-01.pdf U04 L09 printed pp247-252 PDF 270-275',
'CanonicalBodySha256':hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest(),
'AdaptationStatus':'Original Arabic equivalent; Stage D pending'})
c['Translations']=[x for x in c['Translations'] if x['CultureCode']!='ar']+[t]
i=next(i for i,x in enumerate(C['Lessons']) if x['LessonCode']==prev);C['Lessons'].insert(i+1,c)
b=copy.deepcopy(next(x for x in B['Lessons'] if x['LessonCode']==prev))
b.update({'LessonCode':code,'SourceLessonCode':'EDU:UAE-MOE-MATH:L4:COMMON:U04:L09','LessonNumber':9,'Title':title,'SortOrder':32,
'OfficialReferenceCode':'UAE:REF:TEXTBOOK:G4:COMMON:T1:U04:L09'})
for x in b.get('Alignments',[]):x['ReferenceCode']=b['OfficialReferenceCode']
for x in B['Lessons']:
    if x['SortOrder']>=32:x['SortOrder']+=1
i=next(i for i,x in enumerate(B['Lessons']) if x['LessonCode']==prev);B['Lessons'].insert(i+1,b)
B['Units'][3]['LessonCount']=9;M['Units'][3]['ls'].append(title);M['LessonCount']=sum(len(u['ls']) for u in M['Units'])
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs';s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit4Lesson02EstimateProductsQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1;s=s.replace(a,a+'\n          UaeGrade4Unit4Lesson09MultiDigitQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit4Lesson02EstimateProductsQuestions.Supports(family))\n            return UaeGrade4Unit4Lesson02EstimateProductsQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit4Lesson09MultiDigitQuestions.Supports(family))\n            return UaeGrade4Unit4Lesson09MultiDigitQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit4Lesson02EstimateProductsQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit4Lesson02EstimateProductsQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit4Lesson09MultiDigitQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit4Lesson09MultiDigitQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit4Lesson02EstimateProductsQuestions.Supports(problem.Family))\n            return UaeGrade4Unit4Lesson02EstimateProductsQuestions.Solve(problem.Family, p);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit4Lesson09MultiDigitQuestions.Supports(problem.Family))\n            return UaeGrade4Unit4Lesson09MultiDigitQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig');anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u04.l09.multidigit_product','uae.g4.u04.l09.partial_hundreds','uae.g4.u04.l09.round_estimate']
cstr='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:04:09"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:04:09", "number.multiply_multidigit_by_one",
            "MULTIDIGIT_COLUMN_MULTIPLICATION_AND_PARTIAL_PRODUCTS",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP247_252_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,cstr+anchor),encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json';j=load(p)
template=next(x for x in j['families'] if x['id']=='uae.g4.u04.l02.estimate_rounded_product')
for x in f:
    assert not any(e['id']==x for e in j['families'])
    z=dict(template);z.update({'id':x,'skillId':'number.multiply_multidigit_by_one','answerType':'exact_integer',
'verificationPolicy':'g4-u04-l09-student-source-stage-d-not-run','representations':['place_value','equation'],'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(z)
save(p,j)
rich={'SchemaVersion':1,'ContentVersion':'uae-g4-u04-l09-ar-20261111','PackCode':'UAE-MOE-MATH','Lessons':[{'LessonCode':code,'CultureCode':'ar','Title':title,
'ExplanationParagraphs':['أضرب عددًا من ثلاثة أو أربعة أرقام في رقم واحد من خلال القيم المكانية.','أكتب ناتج كل منزلة في موضعها مع نقل المحمول للمنزلة التالية.','أتحقق بنواتج الضرب الجزئية والتقدير للأقرب.'],
'KeyConcepts':[{'Title':'النواتج الجزئية','Definition':'حواصل ضرب العامل في آحاد العدد وعشراته ومئاته وألوفه.','Rule':'أجمع النتائج الجزئية بقيمتها المكانية.','Example':'٩×٣٦٥=٢٧٠٠+٥٤٠+٤٥.'},{'Title':'الحمل في الضرب','Definition':'إعادة تجميع عشر وحدات من منزلة إلى المنزلة التالية.','Rule':'أنقل كل عشرة إلى المنزلة الأعلى.','Example':'٣×٧ عشرات=٢١ عشرة، أعيد تجميع مئتين وعشرة.'},{'Title':'تقدير النتيجة','Definition':'تقريب العدد الأكبر للتأكد من معقولية ناتج الضرب.','Rule':'أضرب العدد المقرب في العامل.','Example':'٣×٢٧٦٣≈٣×٣٠٠٠=٩٠٠٠.'}],
'WorkedExamples':[{'Title':'ضرب ٣٦٥ في ٩','Question':'كم حاصل ٩×٣٦٥؟','Method':'النواتج الجزئية.','Steps':['٩×٣٠٠=٢٧٠٠.','٩×٦٠=٥٤٠، ٩×٥=٤٥.','٢٧٠٠+٥٤٠+٤٥=٣٢٨٥.'],'Answer':'٣٢٨٥','Check':'قريب من ٩×٤٠٠=٣٦٠٠.'},{'Title':'ضرب أربعة أرقام','Question':'احسب ٣×٢٧٦٣.','Method':'تفكيك الألوف والمئات والعشرات والآحاد.','Steps':['٣×٢٠٠٠=٦٠٠٠.','٣×٧٠٠=٢١٠٠ و٣×٦٠=١٨٠.','٣×٣=٩، والمجموع ٨٢٨٩.'],'Answer':'٨٢٨٩','Check':'أقل من ٩٠٠٠ وهو التقدير.'},{'Title':'شراء أجهزة','Question':'ما تكلفة ٣ أجهزة ثمن الواحد ١١٧٥ درهمًا؟','Method':'الضرب الرأسي من اليمين لليسار.','Steps':['٣×٥=١٥؛ أكتب ٥ وأنقل ١.','٣×٧ عشرات +١=٢٢ عشرات، أنقل ٢.','٣×١ مئة +٢=٥ مئات؛ ٣×١ ألف=٣ آلاف، الناتج ٣٥٢٥.'],'Answer':'٣٥٢٥ درهمًا','Check':'٣×(١٠٠٠+١٠٠+٧٠+٥)=٣٥٢٥.'}],
'CommonMistakes':[{'Mistake':'نسيان إضافة المحمول إلى المنزلة التالية.','WhyWrong':'يغير الناتج بخطأ في قيمة منزلة.','Correction':'أسجل المحمول قبل حساب المنزلة الأعلى.'},{'Mistake':'عدم الاحتفاظ بقيمة الألوف والمئات.','WhyWrong':'تُجمع الأرقام بدل القيم المكانية.','Correction':'أكتب أصفار المنازل في النواتج الجزئية.'}],
'SummaryPoints':['أضرب الآحاد ثم العشرات ثم المئات والألوف.','أعيد تجميع المحمول في كل منزلة.','أتحقق بالتقدير والتوزيع.'],
'Visuals':[{'Kind':'EquationSet','Title':'نواتج الضرب الجزئية','Description':'تفكيك أربعة أرقام إلى قيم مكانية.','Items':['٣ × ٢٧٦٣ = ٣ × (٢٠٠٠ + ٧٠٠ + ٦٠ + ٣)','٦٠٠٠ + ٢١٠٠ + ١٨٠ + ٩','= ٨٢٨٩']}],'Videos':[]}]}
save(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u04-l09-ar.rich-lesson-v2.json',rich)
print('PASS U04L09 exact multidigit generator and source content')
