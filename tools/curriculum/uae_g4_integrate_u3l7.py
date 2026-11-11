import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:03:07';title='العوامل والمضاعفات';unit='فهم الضرب والقسمة'
l=next(x for x in C['Lessons'] if x['LessonCode']==code);b=next(x for x in B['Lessons'] if x['LessonCode']==code)
t={'CultureCode':'ar','Title':title,
'Explanation':'العامل هو عدد يقسم عددًا كليًا آخر قسمة تامة دون باقٍ، ويمكن العثور على عوامل العدد بتكوين مصفوفات لها صفوف وأعمدة متساوية. والمضاعف هو حاصل ضرب العدد في أحد الأعداد الكلية مثل ٠ و١ و٢ و٣.',
'KeyConceptsAndRules':'إذا كان أ × ب = ج فإن أ وب عاملان للعدد ج، و ج مضاعف لكل من أ وب. العدد ١ عامل لكل عدد كلي موجب، والعدد نفسه أحد عوامله. للمضاعفات أول خمسة حدود تبدأ بـ٠ × العدد ثم ١ × العدد وهكذا كما في كتاب الطالب.',
'WorkedExamples':'١. عوامل ١٢ هي ١، ٢، ٣، ٤، ٦، ١٢ عبر الأزواج ١×١٢، ٢×٦، ٣×٤. ٢. المضاعفات الخمسة الأولى للعدد ٤ بدءًا بصفر هي ٠، ٤، ٨، ١٢، ١٦. ٣. عوامل ٢٨: ١، ٢، ٤، ٧، ١٤، ٢٨.',
'StepByStepSolutions':'١. أفحص العدد من ١ حتى العدد المطلوب. ٢. أختبر إن كان يقسم العدد دون باقٍ. ٣. أكتب زوج العوامل لكل قسمة تامة. ٤. أضرب العدد في ٠ ثم ١، ٢، ٣، ٤ لتحديد المضاعفات الخمسة الأولى.',
'CommonMistakes':'إضافة صفر إلى قائمة عوامل عدد موجب؛ نسيان الزوج المقابل لكل عامل؛ الخلط بين العامل والمضاعف؛ بدء قائمة المضاعفات من العدد نفسه رغم أن مرجع الكتاب يبدأ من صفر.',
'QuickSummary':'العوامل أعداد تقسّم عددًا دون باقٍ، والمضاعفات نواتج ضربه في أعداد كلية؛ أستخدم أزواج العوامل والمصفوفات للتحقق.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l['SourceTitle']='Owner-selected UAE Grade4 Arabic general student book'
l['SourceLocator']='04-01.pdf U03 L07 printed pp173-178 PDF 196-201'
l['TitleSourceReference']=l['SourceLocator'];l['SourceEdition']='Student source, edition nonblocking'
l['SourceSha256']='859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3'
l['CanonicalBodySha256']=hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest()
l['AdaptationStatus']='Original Arabic equivalent; Stage D pending'
b['Title']=title;b['UnitTitle']=unit;B['Units'][2]['Title']=unit
M['Units'][2]['title']=unit;M['Units'][2]['ls'][6]=title
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'
s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit3Lesson06AssociativeQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1;s=s.replace(a,a+'\n          UaeGrade4Unit3Lesson07FactorsMultiplesQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit3Lesson06AssociativeQuestions.Supports(family))\n            return UaeGrade4Unit3Lesson06AssociativeQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit3Lesson07FactorsMultiplesQuestions.Supports(family))\n            return UaeGrade4Unit3Lesson07FactorsMultiplesQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit3Lesson06AssociativeQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit3Lesson06AssociativeQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit3Lesson07FactorsMultiplesQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit3Lesson07FactorsMultiplesQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit3Lesson06AssociativeQuestions.Supports(problem.Family))\n            return UaeGrade4Unit3Lesson06AssociativeQuestions.Solve(problem.Family, p);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit3Lesson07FactorsMultiplesQuestions.Supports(problem.Family))\n            return UaeGrade4Unit3Lesson07FactorsMultiplesQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig');anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u03.l07.is_factor','uae.g4.u03.l07.nth_multiple','uae.g4.u03.l07.factor_pair']
contract='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:03:07"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:03:07", "number.factors_and_multiples",
            "FACTOR_PAIRS_AND_WHOLE_NUMBER_MULTIPLES",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP173_178_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,contract+anchor),encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
j=load(p);template=next(x for x in j['families'] if x['id']=='uae.g4.u03.l06.triple_product')
for x in f:
    assert not any(e['id']==x for e in j['families'])
    z=dict(template);z.update({'id':x,'skillId':'number.factors_and_multiples','answerType':'exact_integer','verificationPolicy':'g4-u03-l07-student-source-stage-d-not-run','representations':['array','symbolic'],'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(z)
save(p,j)
print('PASS U03 L07 three factors/multiples families')
