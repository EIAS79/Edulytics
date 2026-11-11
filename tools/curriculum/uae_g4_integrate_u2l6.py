import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:02:06';title='طرح الأعداد الكلية'
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
t={'CultureCode':'ar','Title':title,
'Explanation':'نطرح الأعداد الكلية ذات المنازل المتعددة بكتابة المطروح تحت المطروح منه في أعمدة متطابقة. نبدأ من الآحاد، وإذا كان رقم المطروح منه أصغر من رقم المطروح نعيد تجميع عشرة من المنزلة الأكبر إلى المنزلة الحالية.',
'KeyConceptsAndRules':'المطروح منه − المطروح = الفرق. لإعادة التجميع نستبدل عشرًا واحدة من المنزلة الأكبر بعشر وحدات في المنزلة الحالية. عند التحقق نجمع الفرق والمطروح؛ يجب أن نحصل على المطروح منه.',
'WorkedExamples':'١. ٣٬٢١٤ − ٩٥٧ = ٢٬٢٥٧؛ إعادة التجميع في الآحاد والعشرات والمئات. ٢. ٤٬٢٧٥ − ١٬٣٤٥ = ٢٬٩٣٠؛ التحقق ٢٬٩٣٠ + ١٬٣٤٥ = ٤٬٢٧٥. ٣. ٦٬٣٢٥ − ٢٬٨٤١ = ٣٬٤٨٤؛ تقدير ٦٬٠٠٠ − ٣٬٠٠٠ ≈ ٣٬٠٠٠.',
'StepByStepSolutions':'١. أحاذي المنازل ذات القيمة نفسها. ٢. أطرح أرقام كل منزلة من اليمين. ٣. إذا لم يكف الرقم أعيد التجميع من المنزلة الأكبر. ٤. أراجع الحل بالتقدير وجمع الفرق مع المطروح.',
'CommonMistakes':'تبديل المطروح والمطروح منه؛ نسيان إنقاص المنزلة التي أخذنا منها عشرة؛ محاذاة الأرقام على اليسار بدل اليمين؛ اعتبار التقدير جوابًا دقيقًا.',
'QuickSummary':'أطرح الأعداد الكلية بإعادة تجميع المنازل عند الحاجة وأتحقق باستعمال الجمع.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l['SourceTitle']='UAE Grade 4 Arabic student mathematics, General'
l['SourceLocator']='04-01.pdf Unit 2 Lesson 6 printed 93-98 PDF 116-121'
l['TitleSourceReference']=l['SourceLocator']
l['SourceEdition']='Student source year not blocker'
l['SourceSha256']='859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3'
l['CanonicalBodySha256']=hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest()
l['AdaptationStatus']='Original Arabic math equivalent; Stage D pending'
b['Title']=title;b['UnitTitle']='جمع الأعداد الكلية وطرحها'
B['Units'][1]['Title']='جمع الأعداد الكلية وطرحها'
M['Units'][1]['title']='جمع الأعداد الكلية وطرحها';M['Units'][1]['ls'][5]=title
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'
s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit2Lesson05AdditionQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1
s=s.replace(a,a+'\n          UaeGrade4Unit2Lesson06SubtractionQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit2Lesson05AdditionQuestions.Supports(family))\n            return UaeGrade4Unit2Lesson05AdditionQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit2Lesson06SubtractionQuestions.Supports(family))\n            return UaeGrade4Unit2Lesson06SubtractionQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit2Lesson05AdditionQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit2Lesson05AdditionQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit2Lesson06SubtractionQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit2Lesson06SubtractionQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit2Lesson05AdditionQuestions.Supports(problem.Family))\n            return UaeGrade4Unit2Lesson05AdditionQuestions.Solve(problem.Family, p);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit2Lesson06SubtractionQuestions.Supports(problem.Family))\n            return UaeGrade4Unit2Lesson06SubtractionQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig')
anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u02.l06.subtract_four_digits','uae.g4.u02.l06.subtract_six_digits','uae.g4.u02.l06.missing_subtrahend']
contract='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:02:06"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:02:06",
            "number.whole_subtraction_with_regrouping",
            "PLACE_VALUE_COLUMN_SUBTRACTION_BORROW_CHECK",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP93_98_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,contract+anchor),encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
j=load(p)
template=next(x for x in j['families'] if x['id']=='uae.g4.u02.l05.add_four_digits')
for x in f:
    assert not any(e['id']==x for e in j['families'])
    z=dict(template)
    z.update({'id':x,'skillId':'number.whole_subtraction_with_regrouping','answerType':'exact_integer',
       'verificationPolicy':'g4-u02-l06-student-source-stage-d-not-run',
       'representations':['place_value_chart','symbolic'],'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(z)
save(p,j)
print('PASS integrated grade4 U02 L06 3 subtraction families')
