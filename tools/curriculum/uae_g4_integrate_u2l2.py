import json,pathlib,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cpath=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bpath=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mpath=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
c=load(cpath);b=load(bpath);m=load(mpath)
code='PED:UAE-MOE-MATH:L4:COMMON:02:02'
title='أنماط الجمع والطرح'
unit='جمع الأعداد الكلية وطرحها'
lesson=next(x for x in c['Lessons'] if x['LessonCode']==code)
blue=next(x for x in b['Lessons'] if x['LessonCode']==code)
source='04-01.pdf; unit 2 lesson 2; printed pages 67-72; PDF pages 90-95'
translation={
 'CultureCode':'ar','Title':title,
 'Explanation':'نمط الجمع والطرح تسلسل يزيد أو ينقص بقيمة ثابتة. نستعمل القيمة المكانية لفحص الفرق بين كل حدين متتاليين. ويمكن أن تكون الخطوة واحدة أو عشرة أو مئة أو ألفًا أو عشرة آلاف أو مئة ألف. في جدول الأعداد تؤدي الحركة إلى اليمين إلى زيادة معينة وإلى الأسفل زيادة أخرى بحسب التعليمات.',
 'KeyConceptsAndRules':'لإكمال نمط عددي نحدد الفرق بين عددين متجاورين، ثم نختبر أنه متكرر، ونضيف الفرق نفسه أو نطرحه من الحد الأخير. يمكن حساب الحد المجهول من معادلة الجمع أو الطرح باستخدام العملية العكسية.',
 'WorkedExamples':'١. ٧٨٬٢٤٥ ثم ٧٩٬٢٤٥: الفرق +١٬٠٠٠ فالحد التالي ٨٠٬٢٤٥. ٢. ٥٦٬٧٨٩، ٥٦٬٦٨٩، ٥٦٬٥٨٩: الفرق −١٠٠، والحد التالي ٥٦٬٤٨٩. ٣. ٣٦٬٥٢٥ + □ = ٣٦٬٦٢٥؛ الحد المجهول ١٠٠.',
 'StepByStepSolutions':'١. افحص عددين متجاورين. ٢. احسب الفرق وحدد اتجاه الزيادة أو النقصان. ٣. طبّق الخطوة ذاتها لإكمال الجدول أو التسلسل. ٤. تحقق من جميع الفروق لتكتشف الخطأ إن وجد.',
 'CommonMistakes':'الخلط بين الزيادة بمئة والألف، أو إهمال حمل المنزلة عند انتقال ٩ إلى ٠؛ الانتباه إلى الفرق في القيمة لا إلى الشكل الظاهري للأرقام؛ عدم التحقق من كل حد.',
 'QuickSummary':'أحدد خطوة النمط وأكمله وأشرح الفرق الثابت، وأستخدم الجمع والطرح لإيجاد الحد المجهول.'
}
lesson['Translations']=[x for x in lesson['Translations'] if x['CultureCode']!='ar']+[translation]
lesson['TitleSourceReference']=source
lesson['SourceLocator']=source
lesson['SourceTitle']='UAE Grade 4 Arabic student mathematics, Common'
lesson['SourceEdition']='Owner selected book regardless edition year'
lesson['SourceSha256']='859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3'
lesson['CanonicalBodySha256']=hashlib.sha256(json.dumps(translation,sort_keys=True,ensure_ascii=False,separators=(',',':')).encode()).hexdigest()
lesson['AdaptationStatus']='Original Arabic source-aligned mathematical lesson; Stage D pending'
blue['Title']=title
blue['UnitTitle']=unit
b['Units'][1]['Title']=unit
m['Units'][1]['title']=unit
m['Units'][1]['ls'][1]=title
save(cpath,c);save(bpath,b);save(mpath,m)
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'
s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit2Lesson01PropertiesQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1
s=s.replace(a,a+'\n          UaeGrade4Unit2Lesson02PatternsQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit2Lesson01PropertiesQuestions.Supports(family))\n            return UaeGrade4Unit2Lesson01PropertiesQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit2Lesson02PatternsQuestions.Supports(family))\n            return UaeGrade4Unit2Lesson02PatternsQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit2Lesson01PropertiesQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit2Lesson01PropertiesQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit2Lesson02PatternsQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit2Lesson02PatternsQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit2Lesson01PropertiesQuestions.Supports(problem.Family))\n            return UaeGrade4Unit2Lesson01PropertiesQuestions.Solve(problem.Family, p);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit2Lesson02PatternsQuestions.Supports(problem.Family))\n            return UaeGrade4Unit2Lesson02PatternsQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
path=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=path.read_text(encoding='utf-8-sig')
anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
families=['uae.g4.u02.l02.add_place_step','uae.g4.u02.l02.subtract_place_step','uae.g4.u02.l02.find_missing_step','uae.g4.u02.l02.continue_pattern']
assert s.count(anchor)==1
codeBlock='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:02:02"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:02:02",
            "number.patterns.add_subtract_place_value",
            "ADD_SUB_STEP_PATTERNS_UP_TO_HUNDRED_THOUSANDS",
            ['''+', '.join('"'+x+'"' for x in families)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP67_72_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
s=s.replace(anchor,codeBlock+anchor)
path.write_text(s,encoding='utf-8')
path=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
j=load(path)
template=next(x for x in j['families'] if x['id']=='uae.g4.u02.l01.commutative_add')
for fid in families:
    assert not any(x['id']==fid for x in j['families'])
    e=dict(template)
    e.update({'id':fid,'skillId':'number.patterns.add_subtract_place_value','answerType':'exact_integer',
      'verificationPolicy':'g4-u02-l02-source-student-stage-d-not-run','representations':['place_value_chart','symbolic','contextual'],
      'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(e)
save(path,j)
print('PASS: Grade4 U02 L02 Arabic canonical, four exact families routed and selected')
