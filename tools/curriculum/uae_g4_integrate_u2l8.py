import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:02:08'
title='استقصاء حل المسائل: تصميم مخطط شريطي'
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
t={'CultureCode':'ar','Title':title,
'Explanation':'نستخدم المخطط الشريطي لتمثيل العلاقات بين الأعداد في المسائل اللفظية. يمثل الشريط الكامل الكل، وتمثل أجزاء الشريط الكميات التي يتكون منها الكل. إذا عرفنا الجزأين نجمعهما، وإذا عرفنا الكل وأحد الجزأين نطرح لإيجاد الجزء المجهول. ويمكن استعمال شريطين للمقارنة بين كميتين وإيجاد الفرق.',
'KeyConceptsAndRules':'أقرأ المسألة وأحدد المعطيات والمطلوب، وأختار بين نموذج الجزء والكل أو نموذج المقارنة. في نموذج الجزء والكل: الجزء الأول + الجزء الثاني = الكل. في نموذج المقارنة: الفرق = الكمية الأكبر − الكمية الأصغر. أكتب جملة عددية ثم أتحقق.',
'WorkedExamples':'١. ثمن أداة ٢٬٤٩٢ درهمًا وثمن مواد ١٢٬٦٠٧ دراهم: المجموع ١٥٬٠٩٩ درهمًا. ٢. يوجد في مجموعتين ٥٢٬٦٧٢ و٣٤٬٠٨٩ طالبًا؛ الفرق = ١٨٬٥٨٣. ٣. شارك ٢٬٧٣١ يومًا و٤٬٣٢٧ في يوم آخر؛ الإجمالي ٧٬٠٥٨.',
'StepByStepSolutions':'١. أقرأ القصة وأسجل الأعداد ووحداتها. ٢. أحدد إذا كنت أبحث عن الكل أو أحد الجزأين أو الفرق. ٣. أرسم شريطًا مقسمًا إلى أجزاء معلومة ومجهولة، أو شريطين للمقارنة. ٤. أختار الجمع أو الطرح وأوجد الناتج. ٥. أراجع معقولية الإجابة وأشرح لماذا يمثلها الرسم.',
'CommonMistakes':'جمع عددين رغم أن المطلوب الفرق، أو طرحهما رغم أن المطلوب الكل؛ رسم شريط مجموع رغم أن المسألة مقارنة؛ إهمال الوحدات أو عدم وضع المجهول في الجزء الصحيح.',
'QuickSummary':'أصمم مخططًا شريطيًا مناسبًا للعلاقة بين الكل والأجزاء أو المقارنة، وأختار العملية وأتحقق من الإجابة.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l['SourceTitle']='Owner-selected UAE Grade 4 general student mathematics'
l['SourceLocator']='04-01.pdf U02 L08, printed 107-112, PDF 130-135'
l['TitleSourceReference']=l['SourceLocator']
l['SourceEdition']='Student source authority; edition nonblocking'
l['SourceSha256']='859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3'
l['CanonicalBodySha256']=hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest()
l['AdaptationStatus']='Original Arabic mathematical equivalent; Stage D pending'
b['Title']=title;b['UnitTitle']='جمع الأعداد الكلية وطرحها'
B['Units'][1]['Title']='جمع الأعداد الكلية وطرحها'
M['Units'][1]['title']='جمع الأعداد الكلية وطرحها';M['Units'][1]['ls'][7]=title
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'
s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit2Lesson07ZeroSubtractionQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1
s=s.replace(a,a+'\n          UaeGrade4Unit2Lesson08BarModelQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit2Lesson07ZeroSubtractionQuestions.Supports(family))\n            return UaeGrade4Unit2Lesson07ZeroSubtractionQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit2Lesson08BarModelQuestions.Supports(family))\n            return UaeGrade4Unit2Lesson08BarModelQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit2Lesson07ZeroSubtractionQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit2Lesson07ZeroSubtractionQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit2Lesson08BarModelQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit2Lesson08BarModelQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit2Lesson07ZeroSubtractionQuestions.Supports(problem.Family))\n            return UaeGrade4Unit2Lesson07ZeroSubtractionQuestions.Solve(problem.Family, p);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit2Lesson08BarModelQuestions.Supports(problem.Family))\n            return UaeGrade4Unit2Lesson08BarModelQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig')
anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u02.l08.bar_part_whole','uae.g4.u02.l08.bar_compare','uae.g4.u02.l08.bar_missing_part']
contract='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:02:08"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:02:08",
            "number.bar_model_word_problems",
            "PART_WHOLE_AND_COMPARE_BAR_REPRESENTATION",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP107_112_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,contract+anchor),encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
j=load(p)
template=next(x for x in j['families'] if x['id']=='uae.g4.u02.l07.subtract_through_zero_four')
for x in f:
    assert not any(e['id']==x for e in j['families'])
    z=dict(template)
    z.update({'id':x,'skillId':'number.bar_model_word_problems','answerType':'exact_integer',
      'verificationPolicy':'g4-u02-l08-local-student-source-stage-d-not-run',
      'representations':['bar_model','symbolic','contextual'],'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(z)
save(p,j)
print('PASS integrated UAE Grade4 U02 L08 three bar-model exact question families')
