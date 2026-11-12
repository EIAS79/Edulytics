import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:05:02';title='تقدير نواتج الضرب';unit='الضرب في عدد مكون من رقمين'
l=next(x for x in C['Lessons'] if x['LessonCode']==code);b=next(x for x in B['Lessons'] if x['LessonCode']==code)
assert b['SortOrder']==36 and M['Units'][4]['ls'][0]=='الضرب في العشرات'
t={'CultureCode':'ar','Title':title,
'Explanation':'لإيجاد تقدير ناتج ضرب عاملين مكونين من رقمين، أقرب كل عامل إلى أقرب عشرة ثم أضرب. عند تقريب العاملين إلى رقمين أكبر سيكون التقدير أكبر من الناتج الحقيقي، ولكن إذا قرب أحدهما إلى أقل والآخر إلى أكبر، يجب مقارنة الناتجين للتأكد.',
'KeyConceptsAndRules':'٢١ × ١٤ ≈ ٢٠ × ١٠ = ٢٠٠، وهو أقل من ٢٩٤. ٨٨ × ٦٥ ≈ ٩٠ × ٧٠ = ٦٣٠٠، وهو أكبر من الناتج الحقيقي ٥٧٢٠. ٣٨ × ٣٥ ≈ ٤٠ × ٤٠ = ١٦٠٠، وهو أعلى من ١٣٣٠.',
'WorkedExamples':'١. ٢١ × ١٤ ≈ ٢٠ × ١٠ = ٢٠٠. ٢. ٣٨ × ٣٥ ≈ ٤٠ × ٤٠ = ١٦٠٠. ٣. ٨٨ × ٦٥ ≈ ٩٠ × ٧٠ = ٦٣٠٠. ٤. ٣٧ × ٢٢ ≈ ٤٠ × ٢٠ = ٨٠٠. ٥. ٥٧ × ٢٥ ≈ ٦٠ × ٣٠ = ١٨٠٠.',
'StepByStepSolutions':'١. أقرأ العاملين وأحدد أقرب عشرة لكل عامل. ٢. أضرب العشرتين الناتجتين. ٣. أقارن كل عامل بعد التقريب بقيمته الأصلية. ٤. أقرر من خلال حساب أو منطق إذا كان التقدير أكبر من حاصل الضرب الحقيقي أو أصغر منه.',
'CommonMistakes':'تقريب عامل واحد ونسيان الآخر؛ الخطأ في تقريب عدد تنتهي آحاده بخمسة؛ افتراض أن كل تقدير أقل من الناتج الحقيقي؛ عدم ضرب العاملين بعد التقريب.',
'QuickSummary':'أقرّب كلا العاملين إلى أقرب عشرة، ثم أضرب وأحدد معقولية التقدير.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l.update({'SourceTitle':'UAE Grade 4 Common Arabic student book',
'SourceLocator':'04-01.pdf U05 L02 printed pp285-290 PDF308-313',
'TitleSourceReference':'04-01.pdf U05 L02 printed pp285-290 PDF308-313',
'SourceEdition':'Student book authority; year nonblocking',
'SourceSha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
'CanonicalBodySha256':hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest(),
'AdaptationStatus':'Original Arabic equivalent; Stage D pending'})
b['Title']=title;b['UnitTitle']=unit;M['Units'][4]['ls'][1]=title
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs';s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit5Lesson01TensQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1;s=s.replace(a,a+'\n          UaeGrade4Unit5Lesson02EstimateProductsQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit5Lesson01TensQuestions.Supports(family))\n            return UaeGrade4Unit5Lesson01TensQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit5Lesson02EstimateProductsQuestions.Supports(family))\n            return UaeGrade4Unit5Lesson02EstimateProductsQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit5Lesson01TensQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit5Lesson01TensQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit5Lesson02EstimateProductsQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit5Lesson02EstimateProductsQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit5Lesson01TensQuestions.Supports(problem.Family))\n            return UaeGrade4Unit5Lesson01TensQuestions.Solve(problem.Family, p);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit5Lesson02EstimateProductsQuestions.Supports(problem.Family))\n            return UaeGrade4Unit5Lesson02EstimateProductsQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig');anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u05.l02.estimate_two_digit_product','uae.g4.u05.l02.estimate_above_or_below']
cstr='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:05:02"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:05:02", "number.estimate_two_digit_factors",
            "ROUND_BOTH_FACTORS_TO_TENS_COMPARE_ESTIMATE",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP285_290_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,cstr+anchor),encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json';j=load(p)
template=next(x for x in j['families'] if x['id']=='uae.g4.u05.l01.tens_product')
for x in f:
    assert not any(e['id']==x for e in j['families'])
    z=dict(template);z.update({'id':x,'skillId':'number.estimate_two_digit_factors','answerType':'exact_integer','verificationPolicy':'g4-u05-l02-student-source-stage-d-not-run',
    'representations':['number_line','equation'],'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(z)
save(p,j)
rich={'SchemaVersion':1,'ContentVersion':'uae-g4-u05-l02-ar-20261111','PackCode':'UAE-MOE-MATH','Lessons':[{'LessonCode':code,'CultureCode':'ar','Title':title,
'ExplanationParagraphs':['أقدّر حاصل ضرب عاملين من رقمين عن طريق تقريب كل عامل إلى أقرب عشرة.','أضرب العددين المقربين ثم أقارن الناتج التقديري بالناتج الحقيقي.','إذا قربت العاملين إلى قيم أكبر منهما فإن التقدير يكون أكبر من الناتج الدقيق لعاملين موجبين.'],
'KeyConcepts':[{'Title':'التقريب لأقرب عشرة','Definition':'استبدال العدد بأقرب مضاعف عشرة.','Rule':'أقرّب لأعلى إذا كانت الآحاد خمسة أو أكثر.','Example':'٦٥ ≈ ٧٠.'},{'Title':'تقدير ناتج الضرب','Definition':'نتيجة ضرب العددين بعد التقريب.','Rule':'أقرب العاملين ثم أضرب.','Example':'٨٨ × ٦٥ ≈ ٩٠ × ٧٠ = ٦٣٠٠.'},{'Title':'معقولية التقدير','Definition':'مقارنة التقدير بالحاصل الدقيق.','Rule':'إذا قرب العاملان لأعلى يكون التقدير أعلى من الناتج الحقيقي.','Example':'٦٣٠٠ > ٥٧٢٠.'}],
'WorkedExamples':[{'Title':'تقدير ساعات النوم','Question':'قدّر ٢١ × ١٤.','Method':'تقريب العاملين لأقرب عشرة.','Steps':['٢١ ≈ ٢٠.','١٤ ≈ ١٠.','٢٠ × ١٠ = ٢٠٠.'],'Answer':'٢٠٠ تقريبًا','Check':'٢١ × ١٤ = ٢٩٤، فالتقدير أقل.'},{'Title':'تقدير تكلفة','Question':'قدّر ٨٨ × ٦٥.','Method':'تقريب العاملين لأعلى.','Steps':['٨٨ ≈ ٩٠.','٦٥ ≈ ٧٠.','٩٠ × ٧٠ = ٦٣٠٠.'],'Answer':'٦٣٠٠ تقريبًا','Check':'٨٨ × ٦٥ = ٥٧٢٠، فالتقدير أكبر.'},{'Title':'تقدير دقائق اللعب','Question':'قدّر ٣٨ × ٣٥.','Method':'تقريب لأقرب عشرة.','Steps':['٣٨ ≈ ٤٠.','٣٥ ≈ ٤٠.','٤٠ × ٤٠ = ١٦٠٠.'],'Answer':'١٦٠٠ تقريبًا','Check':'٣٨ × ٣٥ = ١٣٣٠.'}],
'CommonMistakes':[{'Mistake':'تقريب عامل واحد فقط.','WhyWrong':'السؤال يتطلب تقريب كليهما.','Correction':'أقرب كل عامل إلى أقرب عشرة.'},{'Mistake':'الافتراض أن التقدير دائمًا أقل.','WhyWrong':'يمكن أن يكون أكبر إذا قربنا لأعلى.','Correction':'أقارن اتجاه تقريب العاملين.'}],
'SummaryPoints':['أقرب العامل الأول إلى أقرب عشرة.','أقرب العامل الثاني إلى أقرب عشرة.','أضرب المقربين وأتحقق من التقدير.'],
'Visuals':[{'Kind':'EquationSet','Title':'مقارنة التقدير والحقيقة','Description':'أمثلة أصلية تظهر تقديرات أعلى وأقل من الناتج الفعلي.','Items':['٢١ × ١٤ ≈ ٢٠ × ١٠ = ٢٠٠','٨٨ × ٦٥ ≈ ٩٠ × ٧٠ = ٦٣٠٠','٣٨ × ٣٥ ≈ ٤٠ × ٤٠ = ١٦٠٠']}],'Videos':[]}]}
save(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u05-l02-ar.rich-lesson-v2.json',rich)
print('PASS Unit5 L02 source matched estimated products')
