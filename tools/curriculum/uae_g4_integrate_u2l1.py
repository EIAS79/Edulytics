import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cpath=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bpath=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mpath=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cpath);B=load(bpath);M=load(mpath)
code='PED:UAE-MOE-MATH:L4:COMMON:02:01'
title='خواص الجمع وقواعد الطرح'
unit='جمع الأعداد الكلية وطرحها'
lesson=next(x for x in C['Lessons'] if x['LessonCode']==code)
blue=next(x for x in B['Lessons'] if x['LessonCode']==code)
source='04-01.pdf, Unit 2 Lesson 1, printed pages 61-66, PDF pages 84-89'
text={
 'CultureCode':'ar','Title':title,
 'Explanation':'نستعمل خواص الجمع الثلاث: التبديل والتجميع والمحايد الجمعي لتسهيل الحساب. التبديل يغير ترتيب الحدين دون تغيير المجموع، والتجميع يغير طريقة جمع ثلاثة حدود دون تغيير المجموع، والصفر لا يغير مجموع العدد. في الطرح، طرح صفر لا يغيّر العدد وطرح العدد من نفسه يساوي صفرًا.',
 'KeyConceptsAndRules':'خاصية التبديل: أ + ب = ب + أ. خاصية التجميع: (أ + ب) + ج = أ + (ب + ج). خاصية المحايد الجمعي: أ + ٠ = أ. قاعدتا الطرح: أ − ٠ = أ؛ أ − أ = ٠. الطرح ليس تبديليًا على العموم.',
 'WorkedExamples':'الأول: ١٨ + ٢٢ + ٤٤ = (١٨ + ٢٢) + ٤٤ = ٨٤. الثاني: ٤٢ + ٢٥ = ٢٥ + ٤٢ = ٦٧. الثالث: ٣٧ − ٠ = ٣٧ بينما ٣٧ − ٣٧ = ٠.',
 'StepByStepSolutions':'١. اقرأ المعادلة وحدد المطلوب. ٢. اختر الخاصية المناسبة للجمع أو قاعدة الطرح. ٣. أعد كتابة العملية دون تغيير قيمتها. ٤. احسب وتحقق من طرفي المعادلة.',
 'CommonMistakes':'عكس ترتيب الطرح ٣٧ − ١٩ و١٩ − ٣٧ باعتبار الطرح تبديليًا؛ الخلط بين المحايد الجمعي والجمع مع الواحد؛ سوء استعمال الأقواس بما يغيّر ترتيب العمليات في الطرح.',
 'QuickSummary':'أستخدم التبديل والتجميع والمحايد الجمعي للصيغ المتكافئة، وأتحقق من قاعدتي الطرح من الصفر ومن العدد نفسه.'
}
lesson['Translations']=[x for x in lesson['Translations'] if x['CultureCode']!='ar']+[text]
lesson['TitleSourceReference']=source
lesson['SourceLocator']=source
lesson['SourceTitle']='UAE Grade 4 Arabic student mathematics, Common'
lesson['SourceSha256']='859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3'
lesson['CanonicalBodySha256']=hashlib.sha256(json.dumps(text,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest()
lesson['SourceEdition']='Owner selected student PDF; edition age is not a blocker'
lesson['AdaptationStatus']='Original Arabic mathematical equivalent to reviewed student pp61-66; local Common pathway, Stage D pending'
blue['Title']=title
blue['UnitTitle']=unit
B['Units'][1]['Title']=unit
M['Units'][1]['title']=unit
M['Units'][1]['ls'][0]=title
save(cpath,C);save(bpath,B);save(mpath,M)
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'
s=p.read_text(encoding='utf-8-sig')
old='UaeGrade4Unit1FourStepQuestions.Supports(family.Trim()) ||'
assert s.count(old)==1
s=s.replace(old,old+'\n          UaeGrade4Unit2Lesson01PropertiesQuestions.Supports(family.Trim()) ||')
old='if (UaeGrade4Unit1FourStepQuestions.Supports(family))\n            return UaeGrade4Unit1FourStepQuestions.Verify(family, parameters, answer);'
assert s.count(old)==1
s=s.replace(old,old+'\n        if (UaeGrade4Unit2Lesson01PropertiesQuestions.Supports(family))\n            return UaeGrade4Unit2Lesson01PropertiesQuestions.Verify(family, parameters, answer);')
old='if (UaeGrade4Unit1FourStepQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit1FourStepQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(old)==1
s=s.replace(old,old+'\n        if (UaeGrade4Unit2Lesson01PropertiesQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit2Lesson01PropertiesQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
old='if (UaeGrade4Unit1FourStepQuestions.Supports(problem.Family))\n            return UaeGrade4Unit1FourStepQuestions.Solve(problem.Family, p);'
assert s.count(old)==1
s=s.replace(old,old+'\n        if (UaeGrade4Unit2Lesson01PropertiesQuestions.Supports(problem.Family))\n            return UaeGrade4Unit2Lesson01PropertiesQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
contract=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=contract.read_text(encoding='utf-8-sig')
marker='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(marker)==1
families=['uae.g4.u02.l01.commutative_add','uae.g4.u02.l01.associative_add','uae.g4.u02.l01.additive_identity','uae.g4.u02.l01.subtract_zero','uae.g4.u02.l01.subtract_self']
new='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:02:01"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:02:01",
            "number.properties.addition_and_subtraction",
            "COMMUTATIVE_ASSOCIATIVE_IDENTITY_SUBTRACTION_RULES",
            ['''+', '.join('"'+x+'"' for x in families)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP61_66_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
assert 'COMMUTATIVE_ASSOCIATIVE_IDENTITY_SUBTRACTION_RULES' not in s
contract.write_text(s.replace(marker,new+marker),encoding='utf-8')
registry=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
reg=load(registry)
template=next(x for x in reg['families'] if x['id']=='uae.g4.u01.l06.find_total')
for fid in families:
    assert not any(x['id']==fid for x in reg['families'])
    e=dict(template)
    e.update({'id':fid,'skillId':'number.properties.addition_and_subtraction',
       'answerType':'exact_integer','verificationPolicy':'g4-u02-l01-local-arabic-student-source-stage-d-not-run',
       'representations':['symbolic','contextual'],'status':'ShadowVerified','productionRouting':False,'lessonPracticeRouting':True})
    reg['families'].append(e)
save(registry,reg)
assert C['Lessons'][6]['LessonCode']==code
print('PASS: Grade 4 U02 L01 canonical, lesson title, engine, five families and source evidence')
