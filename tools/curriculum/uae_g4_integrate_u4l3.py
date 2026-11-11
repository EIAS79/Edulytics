import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:04:03';title='نشاط عملي: استخدام القيمة المكانية للضرب'
unit='الضرب في الأعداد المكونة من رقم واحد'
l=next(x for x in C['Lessons'] if x['LessonCode']==code);b=next(x for x in B['Lessons'] if x['LessonCode']==code)
t={'CultureCode':'ar','Title':title,
'Explanation':'أستخدم مكعبات القيمة المكانية لتمثيل حاصل ضرب رقم واحد بعدد من رقمين. أفصل العدد إلى عشرات وآحاد، ثم أنشئ مجموعات متساوية، وأحسب كل جزء ثم أعيد تجميع الناتج.',
'KeyConceptsAndRules':'أ × (١٠ب + ج) = (أ × ١٠ب) + (أ × ج). تمثل القضبان العشرات والمكعبات الصغيرة الآحاد؛ وعندما يصل عدد الآحاد إلى ١٠ أعيد تجميعها في عشرة واحدة.',
'WorkedExamples':'١. ٢ × ٣٢ = ٢ × ٣٠ + ٢ × ٢ = ٦٠ + ٤ = ٦٤. ٢. ٤ × ٢٢ = ٤ × ٢٠ + ٤ × ٢ = ٨٠ + ٨ = ٨٨. ٣. ٣ × ٢٣ = ٦٠ + ٩ = ٦٩. ٤. ٤ × ١٢ = ٤٠ + ٨ = ٤٨.',
'StepByStepSolutions':'١. أقرأ العدد ذي الرقمين إلى عشرات وآحاد. ٢. أرسم عدد المجموعات المطلوبة وأضع في كل مجموعة عشراتها وآحادها. ٣. أوجد ناتج ضرب العشرات والآحاد على حدة. ٤. أجمع الناتجين وأتحقق من صحة النموذج.',
'CommonMistakes':'جمع عدد العشرات مع عدد الآحاد دون مراعاة قيمة العشرة؛ تجاهل عدد المجموعات؛ نسيان جمع الناتجين الجزئيين؛ رسم مصفوفة غير متساوية.',
'QuickSummary':'أنمذج العشرات والآحاد في مجموعات ثم أحسب نواتج الضرب الجزئية وأجمعها.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l['SourceTitle']='UAE Grade4 Arabic General Student Book';l['SourceLocator']='04-01.pdf U04 L03 printed pp209-214 PDF 232-237'
l['TitleSourceReference']=l['SourceLocator'];l['SourceEdition']='Student book authority; year nonblocking'
l['SourceSha256']='859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3'
l['CanonicalBodySha256']=hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest()
l['AdaptationStatus']='Original Arabic equivalent; Stage D pending'
b['Title']=title;b['UnitTitle']=unit;B['Units'][3]['Title']=unit;M['Units'][3]['title']=unit;M['Units'][3]['ls'][2]=title
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs';s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit4Lesson02EstimateProductsQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1;s=s.replace(a,a+'\n          UaeGrade4Unit4Lesson03PlaceValueModelsQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit4Lesson02EstimateProductsQuestions.Supports(family))\n            return UaeGrade4Unit4Lesson02EstimateProductsQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit4Lesson03PlaceValueModelsQuestions.Supports(family))\n            return UaeGrade4Unit4Lesson03PlaceValueModelsQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit4Lesson02EstimateProductsQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit4Lesson02EstimateProductsQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit4Lesson03PlaceValueModelsQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit4Lesson03PlaceValueModelsQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit4Lesson02EstimateProductsQuestions.Supports(problem.Family))\n            return UaeGrade4Unit4Lesson02EstimateProductsQuestions.Solve(problem.Family, p);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit4Lesson03PlaceValueModelsQuestions.Supports(problem.Family))\n            return UaeGrade4Unit4Lesson03PlaceValueModelsQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs';s=p.read_text(encoding='utf-8-sig')
anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u04.l03.model_two_digit_product','uae.g4.u04.l03.model_tens_product','uae.g4.u04.l03.model_ones_product']
c='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:04:03"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:04:03", "number.multiply_two_digit_place_value_models",
            "BASE_TEN_BLOCKS_PARTIAL_PRODUCTS",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP209_214_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,c+anchor),encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json';j=load(p)
template=next(x for x in j['families'] if x['id']=='uae.g4.u04.l02.estimate_rounded_product')
for x in f:
    assert not any(e['id']==x for e in j['families'])
    z=dict(template);z.update({'id':x,'skillId':'number.multiply_two_digit_place_value_models','answerType':'exact_integer','verificationPolicy':'g4-u04-l03-student-source-stage-d-not-run','representations':['base_ten_blocks','equation'],'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(z)
save(p,j)
print('PASS U04 L03 content, registry, contracts')
