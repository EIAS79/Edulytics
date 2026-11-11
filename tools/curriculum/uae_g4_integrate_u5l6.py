import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:05:06'
title='استقصاء حل المسائل: رسم جدول';unit='الضرب في عدد مكون من رقمين'
l=next(x for x in C['Lessons'] if x['LessonCode']==code);b=next(x for x in B['Lessons'] if x['LessonCode']==code)
assert b['SortOrder']==40 and B['Units'][4]['LessonCount']==6
t={'CultureCode':'ar','Title':title,
'Explanation':'استراتيجية رسم جدول تساعدني على تنظيم الأعداد في مسائل مجموعات متساوية تتكرر بمرور الأيام أو الأسابيع أو الصفحات. أكتب قيمة الوحدة ثم أكرر الضرب إلى أن أصل إلى عدد الوحدات المطلوب. أتحقق بضرب قيمة الوحدة في عددها.',
'KeyConceptsAndRules':'إذا قدم فريق ٤٨ وجبة إفطار في كل صباح لمدة ٥ أيام، أكتب صف الأيام ١،٢،٣،٤،٥، وصف الوجبات ٤٨،٩٦،١٤٤،١٩٢،٢٤٠. ويمكن حساب المجموع ٤٨×٥=٢٤٠. إذا قرأ طالب ١٦ صفحة كل أسبوع لمدة ٥ أسابيع فالمجموع ٨٠ صفحة.',
'WorkedExamples':'١. ٤٨ وجبة في ٥ أيام: ٤٨،٩٦،١٤٤،١٩٢،٢٤٠؛ الناتج ٢٤٠. ٢. ١٦ صفحة أسبوعيًا لخمسة أسابيع: ١٦،٣٢،٤٨،٦٤،٨٠. ٣. ٢٠ بطاقة، تُكتب بطاقتان يوميًا: يلزم ١٠ أيام. ٤. ٦ فئران تأكل ٦ صراصير لكل فأر يوميًا لمدة ١٣ أسبوعًا: أعيد تفسير وحدات الوقت قبل الحل.',
'StepByStepSolutions':'١. أحدد الكمية المتكررة وعدد المرات. ٢. أكتب عمودًا للعدد المتتابع للأيام أو الأسابيع أو المجموعات. ٣. أكتب القيم التراكمية في العمود المقابل. ٤. أتحقق بضرب الكمية لكل مرة في عدد المرات.',
'CommonMistakes':'كتابة الكمية اليومية نفسها في عمود المجموع بدل المجموع التراكمي؛ خطأ في عدد الفترات؛ نسيان تحويل الساعات أو الدقائق؛ جمع قيم الجدول التراكمية معًا بدل أخذ القيمة النهائية.',
'QuickSummary':'أرسم جدولًا للعدد المتكرر والقيمة التراكمية، ثم أتحقق بالضرب.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l.update({'SourceTitle':'UAE Grade 4 Common Arabic student book','SourceLocator':'04-01.pdf U05 L06 printed pp311-316 PDF334-339',
'TitleSourceReference':'04-01.pdf U05 L06 printed pp311-316 PDF334-339','SourceEdition':'Student book authority; year nonblocking',
'SourceSha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
'CanonicalBodySha256':hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest(),'AdaptationStatus':'Original Arabic equivalent; Stage D pending'})
b['Title']=title;b['UnitTitle']=unit;M['Units'][4]['ls'][5]=title
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Services/Mathematics/UaeGrade4Unit5Lesson05MultiStepQuestions.cs';s=p.read_text(encoding='utf-8-sig')
assert 'internal const string Missing=' in s and 'f is Net or Missing;' in s
s=s.replace('internal const string Missing="uae.g4.u05.l05.missing_after_groups";',
'''internal const string Missing="uae.g4.u05.l05.missing_after_groups";
    internal const string Table="uae.g4.u05.l06.table_total";
    internal const string Days="uae.g4.u05.l06.table_number_of_days";''')
s=s.replace('f is Net or Missing;', 'f is Net or Missing or Table or Days;')
start='''string prompt=f==Net?$"يدخر طالب {a} درهمًا أسبوعيًا وينفق {b} دراهم. كم يدخر بعد {n} أسبوعًا؟":
            $"يوجد {n} صناديق في كل منها {a} قطع، وأضيفت {b} قطع وقطع مجهولة حتى أصبح المجموع {n*a+b+(a+b)}. ما العدد المجهول؟";'''
assert s.count(start)==1
s=s.replace(start,'''string prompt=f switch
        {
            Net=>$"يدخر طالب {a} درهمًا أسبوعيًا وينفق {b} دراهم. كم يدخر بعد {n} أسبوعًا؟",
            Missing=>$"يوجد {n} صناديق في كل منها {a} قطع، وأضيفت {b} قطع وقطع مجهولة حتى أصبح المجموع {n*a+b+(a+b)}. ما العدد المجهول؟",
            Table=>$"ارسم جدولًا لـ {n} أيام، في كل يوم {a} نشاطًا. كم العدد التراكمي للأنشطة في اليوم الأخير؟",
            _=>$"ارسم جدولًا؛ ننجز {a} مهام في اليوم ونحتاج {a*n} مهمة. كم عدد الأيام اللازمة؟"
        };''')
s=s.replace('result=f==Net?(a-b)*n:a+b;', 'result=f switch {Net=>(a-b)*n,Missing=>a+b,Table=>a*n,_=>n};')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig');anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u05.l06.table_total','uae.g4.u05.l06.table_number_of_days']
cstr='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:05:06"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:05:06", "number.multiplicative_table_strategy",
            "DRAW_TABLE_REPEATED_MULTIPLICATION",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP311_316_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,cstr+anchor),encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json';j=load(p)
template=next(x for x in j['families'] if x['id']=='uae.g4.u05.l05.net_weekly_savings')
for x in f:
    assert not any(e['id']==x for e in j['families'])
    z=dict(template);z.update({'id':x,'skillId':'number.multiplicative_table_strategy','answerType':'exact_integer',
    'verificationPolicy':'g4-u05-l06-student-source-stage-d-not-run','representations':['table','equation'],'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(z)
save(p,j)
rich={'SchemaVersion':1,'ContentVersion':'uae-g4-u05-l06-ar-20261111','PackCode':'UAE-MOE-MATH','Lessons':[{'LessonCode':code,'CultureCode':'ar','Title':title,
'ExplanationParagraphs':['أرسم جدولًا عندما تتكرر كمية ثابتة خلال أيام أو أسابيع أو مجموعات.','أكتب قيم المرات في صف، والمجموع التراكمي المقابل في صف ثان.','العدد الأخير في جدول المجموع يساوي الكمية لكل مرة مضروبة في عدد المرات.'],
'KeyConcepts':[{'Title':'رسم جدول','Definition':'تنظيم رقم المرة والعدد التراكمي في صفوف وأعمدة.','Rule':'المجموع عند المرة ن = ن × الكمية لكل مرة.','Example':'٤٨ وجبة يوميًا لخمس أيام.'},{'Title':'النمط التراكمي','Definition':'زيادة ثابتة في كل مرة.','Rule':'أضيف نفس الكمية إلى القيمة السابقة.','Example':'٤٨،٩٦،١٤٤،١٩٢،٢٤٠.'},{'Title':'التحقق بالضرب','Definition':'اختبار القيمة النهائية للجدول.','Rule':'أضرب مقدار كل فترة في عدد الفترات.','Example':'٥×٤٨=٢٤٠.'}],
'WorkedExamples':[{'Title':'وجبات الإفطار','Question':'تقدم جهة ٤٨ وجبة في الصباح لمدة ٥ أيام. كم وجبة؟','Method':'جدول الأيام والوجبات.','Steps':['الأيام: ١،٢،٣،٤،٥.','الوجبات: ٤٨،٩٦،١٤٤،١٩٢،٢٤٠.','المجموع ٢٤٠ وجبة.'],'Answer':'٢٤٠ وجبة','Check':'٤٨×٥=٢٤٠.'},{'Title':'قراءة الصفحات','Question':'يقرأ طالب ١٦ صفحة كل أسبوع لمدة ٥ أسابيع. كم صفحة؟','Method':'جدول الأسابيع.','Steps':['أكتب الأسابيع من ١ إلى ٥.','الصفحات: ١٦،٣٢،٤٨،٦٤،٨٠.','المجموع ٨٠ صفحة.'],'Answer':'٨٠ صفحة','Check':'١٦×٥=٨٠.'},{'Title':'البطاقات اليومية','Question':'لدى طالب ٢٠ بطاقة ويكتب بطاقتين كل يوم. كم يومًا؟','Method':'جدول الأيام حتى بلوغ ٢٠ بطاقة.','Steps':['أكتب ٢ ثم ٤ ثم ٦ ثم ٨.','أواصل الزيادة بمقدار ٢.','أصل إلى ٢٠ بطاقة في اليوم العاشر.'],'Answer':'١٠ أيام','Check':'١٠×٢=٢٠.'}],
'CommonMistakes':[{'Mistake':'كتابة نفس كمية اليوم في جميع خلايا المجموع.','WhyWrong':'يجب جمع الكميات المتتالية.','Correction':'أضع مجموعًا تراكميًا يزداد كل فترة.'},{'Mistake':'جمع جميع الأعداد التراكمية بدل أخذ المجموع النهائي.','WhyWrong':'يكرر حساب الوحدات.','Correction':'أستعمل قيمة الصف الأخير.'}],
'SummaryPoints':['أنظم المرات والمجاميع في جدول.','أتبع الزيادة الثابتة.','أتحقق بضرب عدد المرات في المقدار المتكرر.'],
'Visuals':[{'Kind':'EquationSet','Title':'جدول إجمالي وجبات الإفطار','Description':'تمثيل حسابي أصلي لجدول الأيام والقيم التراكمية.','Items':['اليوم: ١،٢،٣،٤،٥','الوجبات: ٤٨،٩٦،١٤٤،١٩٢،٢٤٠','٤٨ × ٥ = ٢٤٠']}],'Videos':[]}]}
save(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u05-l06-ar.rich-lesson-v2.json',rich)
print('PASS U05 L06 reused extended generator and student table strategy')
