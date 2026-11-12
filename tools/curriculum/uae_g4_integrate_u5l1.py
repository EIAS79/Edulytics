import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:05:01';title='الضرب في العشرات';unit='الضرب في عدد مكون من رقمين'
l=next(x for x in C['Lessons'] if x['LessonCode']==code);b=next(x for x in B['Lessons'] if x['LessonCode']==code)
assert B['Units'][4]['LessonCount']==6 and b['SortOrder']==35
t={'CultureCode':'ar','Title':title,
'Explanation':'أضرب عددًا مكونًا من رقمين في مضاعف عشرة، مستخدمًا حقيقة الضرب وقيمة العشرات. أستخدم خاصية التجميع: ٢٥ × ٢٠ = ٢٥ × (٢ × ١٠) = (٢٥ × ٢) × ١٠ = ٥٠٠. أحافظ على الصفر في الآحاد عند الضرب الرأسي، وأضرب في عدد العشرات.',
'KeyConceptsAndRules':'أ × (ب × ١٠) = (أ × ب) × ١٠. عند ضرب ٩٩ × ٣٠، أحسب ٩٩ × ٣ = ٢٩٧، ثم أضرب في ١٠ فأحصل على ٢٩٧٠. لمعرفة العامل المجهول في ٢٢ × □ = ٤٤٠ يكون العامل ٢٠.',
'WorkedExamples':'١. ٢٥ × ٢٠ = ٥٠٠. ٢. ٩٩ × ٣٠ = ٢٩٧٠. ٣. ٦٣ × ٢٠ = ١٢٦٠. ٤. ٥٣ × ٦٠ = ٣١٨٠. ٥. ٢٧ × ٣٠ = ٨١٠.',
'StepByStepSolutions':'١. أحدد العدد ذا الرقمين والعامل من مضاعفات العشرة. ٢. أضرب العدد في عدد العشرات. ٣. أضرب الناتج في ١٠ أو أحتفظ بصفر في الآحاد. ٤. أتحقق من حاصل الضرب بخاصية التجميع أو التقدير.',
'CommonMistakes':'إهمال صفر الآحاد في العامل؛ ضرب العدد مباشرة في رقم العشرات دون ضرب الناتج في عشرة؛ الخلط بين ٢٠ و٢؛ نسيان حمل العشرات في حاصل الضرب الأول.',
'QuickSummary':'أضرب العدد في رقم العشرات ثم في ١٠، وأتحقق بالتجميع.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l.update({'SourceTitle':'UAE Grade 4 Common Arabic student book',
'SourceLocator':'04-01.pdf U05 L01 printed pp279-284 PDF 302-307',
'TitleSourceReference':'04-01.pdf U05 L01 printed pp279-284 PDF 302-307',
'SourceEdition':'Owner selected student book, year nonblocking',
'SourceSha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
'CanonicalBodySha256':hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest(),
'AdaptationStatus':'Original Arabic equivalent; Stage D pending'})
b['Title']=title;b['UnitTitle']=unit;B['Units'][4]['Title']=unit
M['Units'][4]['title']=unit;M['Units'][4]['ls'][0]=title
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs';s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit4Lesson10PrecisionChoiceQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1;s=s.replace(a,a+'\n          UaeGrade4Unit5Lesson01TensQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit4Lesson10PrecisionChoiceQuestions.Supports(family))\n            return UaeGrade4Unit4Lesson10PrecisionChoiceQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit5Lesson01TensQuestions.Supports(family))\n            return UaeGrade4Unit5Lesson01TensQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit4Lesson10PrecisionChoiceQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit4Lesson10PrecisionChoiceQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit5Lesson01TensQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit5Lesson01TensQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit4Lesson10PrecisionChoiceQuestions.Supports(problem.Family))\n            return UaeGrade4Unit4Lesson10PrecisionChoiceQuestions.Solve(problem.Family, p);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit5Lesson01TensQuestions.Supports(problem.Family))\n            return UaeGrade4Unit5Lesson01TensQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig');anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u05.l01.tens_product','uae.g4.u05.l01.unknown_tens_factor']
cstr='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:05:01"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:05:01", "number.multiply_two_digit_by_tens",
            "MULTIPLY_2_DIGIT_BY_MULTIPLES_OF_TEN",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP279_284_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,cstr+anchor),encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json';j=load(p)
template=next(x for x in j['families'] if x['id']=='uae.g4.u04.l09.multidigit_product')
for x in f:
    assert not any(e['id']==x for e in j['families'])
    z=dict(template);z.update({'id':x,'skillId':'number.multiply_two_digit_by_tens','answerType':'exact_integer',
    'verificationPolicy':'g4-u05-l01-student-source-stage-d-not-run','representations':['place_value','equation'],'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(z)
save(p,j)
rich={'SchemaVersion':1,'ContentVersion':'uae-g4-u05-l01-ar-20261111','PackCode':'UAE-MOE-MATH','Lessons':[{'LessonCode':code,'CultureCode':'ar','Title':title,
'ExplanationParagraphs':['أستخدم قيمة العشرات عند ضرب عدد من رقمين في مضاعف عشرة.','أحسب الضرب باستخدام رقم العشرات ثم أضرب في عشرة.','يمكن كتابة حاصل الضرب باستعمال خاصية التجميع أو الضرب الرأسي.'],
'KeyConcepts':[{'Title':'مضاعف العشرة','Definition':'عدد يساوي عشرة مضروبة في عدد صحيح.','Rule':'٢٠ = ٢ × ١٠.','Example':'٢٥ × ٢٠.'},{'Title':'خاصية التجميع','Definition':'إعادة ترتيب التجميع دون تغيير حاصل الضرب.','Rule':'(أ × ب) × ج = أ × (ب × ج).','Example':'٢٥ × (٢ × ١٠) = ٥٠ × ١٠.'},{'Title':'الضرب في العشرات','Definition':'الضرب في مجموعة من عشرات.','Rule':'أضرب في عدد العشرات ثم في عشرة.','Example':'٥٣ × ٦٠ = ٣١٨٠.'}],
'WorkedExamples':[{'Title':'صور في حديقة الحيوان','Question':'طُبعت ٢٠ صورة لكل من ٢٥ طالبًا. كم صورة؟','Method':'التجميع.','Steps':['٢٠ = ٢ × ١٠.','٢٥ × ٢ = ٥٠.','٥٠ × ١٠ = ٥٠٠.'],'Answer':'٥٠٠ صورة','Check':'٢٥ × ٢٠ = ٥٠٠.'},{'Title':'سعر الأجهزة','Question':'ما تكلفة ٣٠ جهازًا ثمن الواحد ٩٩ درهمًا؟','Method':'اضرب في ثلاثة ثم عشرة.','Steps':['٩٩ × ٣ = ٢٩٧.','٢٩٧ × ١٠ = ٢٩٧٠.','أكتب المبلغ ٢٩٧٠ درهمًا.'],'Answer':'٢٩٧٠ درهمًا','Check':'قريب من ٣٠ × ١٠٠ = ٣٠٠٠.'},{'Title':'ناتج مجهول','Question':'أوجد ٦٣ × ٢٠.','Method':'حقيقة الضرب ومضاعف العشرة.','Steps':['٦٣ × ٢ = ١٢٦.','٢٠ = ٢ × ١٠.','١٢٦ × ١٠ = ١٢٦٠.'],'Answer':'١٢٦٠','Check':'قريب من ٦٠ × ٢٠ = ١٢٠٠.'}],
'CommonMistakes':[{'Mistake':'نسيان عامل عشرة.','WhyWrong':'يصبح الناتج أصغر بعشر مرات.','Correction':'أضرب الناتج الأول في عشرة.'},{'Mistake':'التعامل مع ٣٠ باعتباره ٣.','WhyWrong':'يسقط الصفر قيمة عشرية.','Correction':'أكتب ٣٠ = ٣ × ١٠.'}],
'SummaryPoints':['أتعرف إلى مضاعف العشرة.','أحسب العدد × رقم العشرات.','أضرب الناتج في ١٠ وأتحقق.'],
'Visuals':[{'Kind':'EquationSet','Title':'ضرب مضاعفات العشرة','Description':'تمثيل للتجميع والقيمة المكانية.','Items':['٢٥ × ٢٠ = ٢٥ × ٢ × ١٠','٥٠ × ١٠ = ٥٠٠','٩٩ × ٣٠ = ٢٩٧٠']}],'Videos':[]}]}
save(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u05-l01-ar.rich-lesson-v2.json',rich)
print('PASS Unit5 L01 source replaced outdated factors lesson')
