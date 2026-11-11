import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:02:05';title='جمع الأعداد الكلية'
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
t={'CultureCode':'ar','Title':title,
'Explanation':'لجمع الأعداد الكلية متعددة المنازل نضع الأرقام تحت بعضها في جدول المنازل، بحيث تقابل الآحاد الآحاد والعشرات العشرات، ونبدأ من اليمين. عندما يزيد مجموع أرقام منزلة عن تسعة نعيد التجميع بنقل عشرة من تلك المنزلة إلى المنزلة الأعلى.',
'KeyConceptsAndRules':'نحاذي الآحاد والعشرات والمئات والألوف وعشرات الألوف ومئات الألوف. نجمع كل منزلة ونكتب رقم آحاد الناتج ونحمل الباقي إلى المنزلة الأعلى. التحقق بالتقدير أو بالطرح العكسي يكشف أخطاء إعادة التجميع.',
'WorkedExamples':'٦٬٨٢٤ + ٣٤٩ = ٧٬١٧٣: الآحاد ٤+٩=١٣، نحمل ١؛ العشرات ٢+٤+١=٧؛ المئات ٨+٣=١١ ونحمل ١ إلى الألوف. ٥٨٬٧١٣ + ٤٣٬٨٢٧ = ١٠٢٬٥٤٠، مقارنة بالتقدير ٦٠٬٠٠٠ + ٤٠٬٠٠٠ ≈ ١٠٠٬٠٠٠.',
'StepByStepSolutions':'١. أضع الأعداد في أعمدة المنازل المتطابقة. ٢. أجمع الآحاد، ثم العشرات، ثم المئات وهكذا. ٣. أعيد التجميع إذا كان الناتج من منزلتين أو أكثر. ٤. أراجع الناتج بالتقدير والطرح العكسي.',
'CommonMistakes':'محاذاة ٣٤٩ أسفل آلاف العدد الكبير بدل مئاته؛ نسيان حمل العشرة إلى المنزلة التالية؛ إغفال أصفار العدد أو الظن أن التقدير جواب دقيق؛ كتابة عدد من منزلة واحدة فوق تسعة في خانة واحدة.',
'QuickSummary':'أجمع الأعداد الكلية حتى مئات الألوف بطريقة المنازل، وأعيد التجميع عندما يلزم، وأتحقق بالتقدير.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l['SourceTitle']='UAE Grade 4 student General/Common'
l['SourceLocator']='04-01.pdf, Unit 2 Lesson 5, printed pp87-92, PDF pp110-115'
l['TitleSourceReference']=l['SourceLocator']
l['SourceEdition']='Student content authority; edition year does not block'
l['SourceSha256']='859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3'
l['CanonicalBodySha256']=hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest()
l['AdaptationStatus']='Original Arabic math equivalent; Stage D not run'
b['Title']=title;b['UnitTitle']='جمع الأعداد الكلية وطرحها'
B['Units'][1]['Title']='جمع الأعداد الكلية وطرحها'
M['Units'][1]['title']='جمع الأعداد الكلية وطرحها';M['Units'][1]['ls'][4]=title
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'
s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit2Lesson04EstimateQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1
s=s.replace(a,a+'\n          UaeGrade4Unit2Lesson05AdditionQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit2Lesson04EstimateQuestions.Supports(family))\n            return UaeGrade4Unit2Lesson04EstimateQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit2Lesson05AdditionQuestions.Supports(family))\n            return UaeGrade4Unit2Lesson05AdditionQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit2Lesson04EstimateQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit2Lesson04EstimateQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit2Lesson05AdditionQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit2Lesson05AdditionQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit2Lesson04EstimateQuestions.Supports(problem.Family))\n            return UaeGrade4Unit2Lesson04EstimateQuestions.Solve(problem.Family, p);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit2Lesson05AdditionQuestions.Supports(problem.Family))\n            return UaeGrade4Unit2Lesson05AdditionQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig')
marker='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(marker)==1
families=['uae.g4.u02.l05.add_four_digits','uae.g4.u02.l05.add_six_digits','uae.g4.u02.l05.missing_addend']
contract='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:02:05"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:02:05",
            "number.whole_addition_with_regrouping",
            "PLACE_VALUE_COLUMN_ADDITION_CARRY_CHECK",
            ['''+', '.join('"'+f+'"' for f in families)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP87_92_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(marker,contract+marker),encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
j=load(p)
template=next(x for x in j['families'] if x['id']=='uae.g4.u02.l04.sum_nearest_hundred')
for fid in families:
    assert not any(x['id']==fid for x in j['families'])
    e=dict(template)
    e.update({'id':fid,'skillId':'number.whole_addition_with_regrouping','answerType':'exact_integer',
      'verificationPolicy':'g4-u02-l05-student-arabic-stage-d-not-run',
      'representations':['place_value_chart','symbolic'],'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(e)
save(p,j)
print('PASS integrated grade4 U02 L05 3 exact addition families')
