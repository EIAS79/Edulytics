import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:03:05';title='خصائص الضرب وقواعد القسمة';unit='فهم الضرب والقسمة'
l=next(x for x in C['Lessons'] if x['LessonCode']==code);b=next(x for x in B['Lessons'] if x['LessonCode']==code)
t={'CultureCode':'ar','Title':title,
'Explanation':'تساعدنا خصائص الضرب على إيجاد النواتج والتحقق منها. لا يغير تبديل ترتيب العاملين ناتج الضرب. ضرب عدد في ١ يعيد العدد نفسه، وضرب أي عدد في صفر يساوي صفرًا. في القسمة، قسمة صفر على عدد غير صفري تساوي صفرًا، وقسمة عدد على ١ تعيد العدد نفسه، وقسمة عدد غير صفري على نفسه تساوي ١.',
'KeyConceptsAndRules':'التبديل: أ × ب = ب × أ. المحايد: أ × ١ = أ. الصفر: أ × ٠ = ٠. قواعد القسمة: ٠ ÷ أ = ٠ (أ ≠ ٠)، أ ÷ ١ = أ، أ ÷ أ = ١ (أ ≠ ٠). القسمة على ٠ غير معرفة.',
'WorkedExamples':'١. ٤ × ٢ = ٢ × ٤ = ٨. ٢. ٥ × ١ = ٥، ٣ × ٠ = ٠. ٣. ٠ ÷ ٥ = ٠، ٧ ÷ ١ = ٧، ٤ ÷ ٤ = ١. ٤. لا نستطيع قسمة ١٢ على صفر.',
'StepByStepSolutions':'١. أقرأ المعادلة وأحدد أهي ضرب أم قسمة. ٢. أفحص العامل ٠ أو ١ أو تساوي المقسوم والمقسوم عليه. ٣. إن كان العوامل تبدلت أماكنها أطبق خاصية التبديل. ٤. أتحقق بحساب الناتج أو باستخدام حقيقة عكسية.',
'CommonMistakes':'ادعاء أن قسمة أي عدد على صفر تساوي صفرًا؛ الخلط بين أ × ٠ و أ ÷ ٠؛ اعتبار ٠ ÷ ٠ يساوي ١؛ الاعتقاد أن تبديل المقسوم والمقسوم عليه لا يغير خارج القسمة.',
'QuickSummary':'أتعرف خصائص التبديل والمحايد والصفر في الضرب، وقواعد القسمة على ١ وعلى العدد نفسه وقسمة الصفر، وأرفض القسمة على صفر.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l['SourceTitle']='Owner-selected UAE Grade 4 Arabic student book'
l['SourceLocator']='04-01.pdf U03 L05 printed 161-166 PDF 184-189'
l['TitleSourceReference']=l['SourceLocator'];l['SourceEdition']='Student source edition not blocking'
l['SourceSha256']='859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3'
l['CanonicalBodySha256']=hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest()
l['AdaptationStatus']='Original Arabic math equivalent; Stage D pending'
b['Title']=title;b['UnitTitle']=unit;B['Units'][2]['Title']=unit
M['Units'][2]['title']=unit;M['Units'][2]['ls'][4]=title
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'
s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit3Lesson03MultiplicativeComparisonQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1;s=s.replace(a,a+'\n          UaeGrade4Unit3Lesson05PropertiesQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit3Lesson03MultiplicativeComparisonQuestions.Supports(family))\n            return UaeGrade4Unit3Lesson03MultiplicativeComparisonQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit3Lesson05PropertiesQuestions.Supports(family))\n            return UaeGrade4Unit3Lesson05PropertiesQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit3Lesson03MultiplicativeComparisonQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit3Lesson03MultiplicativeComparisonQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit3Lesson05PropertiesQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit3Lesson05PropertiesQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit3Lesson03MultiplicativeComparisonQuestions.Supports(problem.Family))\n            return UaeGrade4Unit3Lesson03MultiplicativeComparisonQuestions.Solve(problem.Family, p);'
assert s.count(a)==1;s=s.replace(a,a+'\n        if (UaeGrade4Unit3Lesson05PropertiesQuestions.Supports(problem.Family))\n            return UaeGrade4Unit3Lesson05PropertiesQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig');anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u03.l05.commutative_missing_factor','uae.g4.u03.l05.identity','uae.g4.u03.l05.zero','uae.g4.u03.l05.divide_by_one','uae.g4.u03.l05.divide_by_self','uae.g4.u03.l05.zero_dividend']
contract='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:03:05"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:03:05", "number.mult_div_properties",
            "COMMUTATIVE_IDENTITY_ZERO_DIVISION_BY_ONE_AND_SELF",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP161_166_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,contract+anchor),encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
j=load(p);template=next(x for x in j['families'] if x['id']=='uae.g4.u03.l03.times_as_many')
for x in f:
    assert not any(e['id']==x for e in j['families'])
    z=dict(template);z.update({'id':x,'skillId':'number.mult_div_properties','answerType':'exact_integer','verificationPolicy':'g4-u03-l05-student-source-stage-d-not-run','representations':['equation'],'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(z)
save(p,j)
print('PASS U03 L05 six property families integrated')
