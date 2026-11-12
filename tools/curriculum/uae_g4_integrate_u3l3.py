import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:03:03'
title='الضرب في صورة مقارنة';unit='فهم الضرب والقسمة'
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
t={'CultureCode':'ar','Title':title,
'Explanation':'المقارنة بالضرب تستخدم عبارة عدد مرات أو أضعاف لتبيّن أن كمية تساوي كمية أخرى مضروبة في عامل. إذا كان لدى أحد الأطفال عدد من الأدوات ولدى طفل آخر ثلاثة أمثال العدد، فإن الكمية الثانية تساوي ثلاثة في الكمية الأولى.',
'KeyConceptsAndRules':'العدد المقارن = الكمية الأساسية × عامل المقارنة. إذا عرفنا العدد المقارن وعامل المقارنة نقسم لإيجاد الكمية الأساسية. يمكن تمثيل الموقف بشريط مقسم إلى أجزاء متساوية بعدد مرات المضاعفة.',
'WorkedExamples':'١. حضر أحد الطلاب ٧ أيام، وحضر زميله ثلاثة أمثال ذلك، فيكون ٣ × ٧ = ٢١ يومًا. ٢. استخدمت إيمان ١٥ خرزة وهو ثلاثة أمثال ما استخدمته منال: ١٥ ÷ ٣ = ٥ خرزات. ٣. ٤ أضعاف ٦ = ٤ × ٦ = ٢٤.',
'StepByStepSolutions':'١. أحدد الكمية الأصلية وعامل المقارنة. ٢. أرسم مجموعات متساوية أو شريطًا مقسمًا بحسب عدد الأضعاف. ٣. أكتب معادلة الضرب أو القسمة. ٤. أتحقق من أن الناتج يطابق معنى أكثر بعدة مرات وليس أكثر بمقدار ثابت.',
'CommonMistakes':'الخلط بين أكثر بثلاثة وأكبر ثلاثة أضعاف؛ إضافة العدد بدل ضربه؛ قسمة الكمية الأصلية عندما المطلوب العدد المقارن؛ إهمال تفسير عدد الأجزاء في الشريط.',
'QuickSummary':'المقارنة الضربية تصف عدد الأضعاف؛ أستخدم الضرب لإيجاد الكمية الأكبر والقسمة لإيجاد الكمية الأساسية أو عامل المقارنة.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l['SourceTitle']='Owner-selected UAE Grade 4 Arabic general student book'
l['SourceLocator']='04-01.pdf U03 L03 printed 147-152 PDF 170-175'
l['TitleSourceReference']=l['SourceLocator']
l['SourceEdition']='Student book authority, edition nonblocking'
l['SourceSha256']='859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3'
l['CanonicalBodySha256']=hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest()
l['AdaptationStatus']='Original Arabic math equivalent; Stage D pending'
b['Title']=title;b['UnitTitle']=unit
B['Units'][2]['Title']=unit
M['Units'][2]['title']=unit;M['Units'][2]['ls'][2]=title
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'
s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit3Lesson02RepeatedSubtractionQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1
s=s.replace(a,a+'\n          UaeGrade4Unit3Lesson03MultiplicativeComparisonQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit3Lesson02RepeatedSubtractionQuestions.Supports(family))\n            return UaeGrade4Unit3Lesson02RepeatedSubtractionQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit3Lesson03MultiplicativeComparisonQuestions.Supports(family))\n            return UaeGrade4Unit3Lesson03MultiplicativeComparisonQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit3Lesson02RepeatedSubtractionQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit3Lesson02RepeatedSubtractionQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit3Lesson03MultiplicativeComparisonQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit3Lesson03MultiplicativeComparisonQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit3Lesson02RepeatedSubtractionQuestions.Supports(problem.Family))\n            return UaeGrade4Unit3Lesson02RepeatedSubtractionQuestions.Solve(problem.Family, p);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit3Lesson03MultiplicativeComparisonQuestions.Supports(problem.Family))\n            return UaeGrade4Unit3Lesson03MultiplicativeComparisonQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig')
anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u03.l03.times_as_many','uae.g4.u03.l03.unknown_comparison_base','uae.g4.u03.l03.unknown_multiplier']
contract='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:03:03"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:03:03",
            "number.multiplicative_comparison",
            "TIMES_AS_MANY_AND_UNKNOWN_FACTOR_COMPARISON",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP147_152_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,contract+anchor),encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
j=load(p)
template=next(x for x in j['families'] if x['id']=='uae.g4.u03.l02.equal_groups_quotient')
for x in f:
    assert not any(e['id']==x for e in j['families'])
    z=dict(template)
    z.update({'id':x,'skillId':'number.multiplicative_comparison','answerType':'exact_integer',
      'verificationPolicy':'g4-u03-l03-student-source-stage-d-not-run',
      'representations':['bar_model','symbolic','contextual'],'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(z)
save(p,j)
print('PASS UAE G4 U03 L03 3 multiplication-comparison exact families')
