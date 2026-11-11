import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:02:07';title='الطرح مع وجود الأصفار'
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
t={'CultureCode':'ar','Title':title,
'Explanation':'عند طرح عدد من آخر يحتوي على أصفار في المنازل الوسطى، قد نحتاج إلى إعادة تجميع أكثر من منزلة. نبدأ بالمنزلة التي نريد الطرح منها ونبحث إلى اليسار عن أول منزلة يمكننا الاستلاف منها، ثم نوزع الوحدات العشرية عبر المنازل الصفرية.',
'KeyConceptsAndRules':'إذا كانت الآحاد صفرًا والمطروح في الآحاد أكبر، نستبدل عشرة من منزلة العشرات. وإذا كانت العشرات صفرًا أيضًا ننتقل إلى المئات أو الآلاف، ثم نعيد توزيع عشر وحدات في كل خطوة. نبقي ترتيب المطروح منه والمطروح كما هو. التحقق: الفرق + المطروح = المطروح منه.',
'WorkedExamples':'١. ٥٬١٠٠ − ٣٬٥٢٠ = ١٬٥٨٠: نعيد تجميع الألوف والمئات والعشرات لنطرح الآحاد والعشرات. ٢. ٣٠٬٠٩٠ − ٢٢٬٩٧٧ = ٧٬١١٣: تظهر إعادة تجميع عبر منزلة الصفر. ٣. ١٠٬٢٠٠ − ٤٬٧٩٥ = ٥٬٤٠٥.',
'StepByStepSolutions':'١. أحاذي الأعمدة وأبدأ بالآحاد. ٢. إذا لم يمكن الطرح أبحث عن أول منزلة أكبر من صفر إلى اليسار. ٣. أعيد التجميع عبر منازل الصفر الواحدة تلو الأخرى. ٤. أطرح كل منزلة ثم أتحقق بجمع الفرق والمطروح.',
'CommonMistakes':'الاستلاف من منزلة صفر دون الرجوع إلى أول منزلة غير صفرية؛ نسيان إنقاص المنزلة المستلفة؛ إهمال ترتيب الأصفار في جدول المنازل؛ عدم التحقق من ناتج الطرح بجمع الفرق والمطروح.',
'QuickSummary':'أطرح الأعداد متعددة المنازل مع وجود أصفار بإعادة التجميع عبر المنازل الصفرية وأتحقق بالجمع.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l['SourceTitle']='UAE Grade 4 Arabic student math, General'
l['SourceLocator']='04-01.pdf Unit 2 Lesson 7 printed 99-104 PDF 122-127'
l['TitleSourceReference']=l['SourceLocator']
l['SourceEdition']='Owner-selected student source'
l['SourceSha256']='859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3'
l['CanonicalBodySha256']=hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest()
l['AdaptationStatus']='First-party Arabic skill equivalent, Stage D pending'
b['Title']=title;b['UnitTitle']='جمع الأعداد الكلية وطرحها'
B['Units'][1]['Title']='جمع الأعداد الكلية وطرحها'
M['Units'][1]['title']='جمع الأعداد الكلية وطرحها';M['Units'][1]['ls'][6]=title
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'
s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit2Lesson06SubtractionQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1
s=s.replace(a,a+'\n          UaeGrade4Unit2Lesson07ZeroSubtractionQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit2Lesson06SubtractionQuestions.Supports(family))\n            return UaeGrade4Unit2Lesson06SubtractionQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit2Lesson07ZeroSubtractionQuestions.Supports(family))\n            return UaeGrade4Unit2Lesson07ZeroSubtractionQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit2Lesson06SubtractionQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit2Lesson06SubtractionQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit2Lesson07ZeroSubtractionQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit2Lesson07ZeroSubtractionQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit2Lesson06SubtractionQuestions.Supports(problem.Family))\n            return UaeGrade4Unit2Lesson06SubtractionQuestions.Solve(problem.Family, p);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit2Lesson07ZeroSubtractionQuestions.Supports(problem.Family))\n            return UaeGrade4Unit2Lesson07ZeroSubtractionQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig')
anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u02.l07.subtract_through_zero_four','uae.g4.u02.l07.subtract_through_zero_five','uae.g4.u02.l07.subtract_through_zero_six']
contract='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:02:07"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:02:07",
            "number.subtraction_across_zeros",
            "REGROUP_ACROSS_ZERO_DIGITS_AND_INVERSE_CHECK",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP99_104_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,contract+anchor),encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
j=load(p)
template=next(x for x in j['families'] if x['id']=='uae.g4.u02.l06.subtract_four_digits')
for x in f:
    assert not any(e['id']==x for e in j['families'])
    z=dict(template)
    z.update({'id':x,'skillId':'number.subtraction_across_zeros','answerType':'exact_integer',
      'verificationPolicy':'g4-u02-l07-student-book-source-stage-d-not-run',
      'representations':['place_value_chart','symbolic'],'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(z)
save(p,j)
print('PASS integrated Grade 4 U02 L07 3 zero-regroup subtraction families')
