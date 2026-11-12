import pathlib,json,copy,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
prev='PED:UAE-MOE-MATH:L4:COMMON:04:07';code='PED:UAE-MOE-MATH:L4:COMMON:04:08'
assert not any(x['LessonCode']==code for x in C['Lessons']) and not any(x['LessonCode']==code for x in B['Lessons'])
assert B['Units'][3]['LessonCount']==7
title='الضرب مع إعادة التجميع'
t={'CultureCode':'ar','Title':title,'Explanation':'أستخدم الضرب الرأسي لعدد من رقمين في رقم واحد، وأعيد تجميع نواتج الآحاد أو العشرات عندما تبلغ عشرًا أو أكثر. أضرب الآحاد وأكتب آحاد الناتج ثم أحمل عشراته، وأضرب العشرات مع إضافة المحمول. أتحقق بالتقدير وبخاصية التوزيع.',
'KeyConceptsAndRules':'في ٥ × ٣٧: ٥ × ٧ = ٣٥، فأكتب ٥ آحاد وأعيد تجميع ٣ عشرات. ثم ٥ × ٣ عشرات = ١٥ عشرة، وأضيف ٣ عشرات لأحصل على ١٨ عشرة = ١٨٠، والناتج ١٨٥. ٦ × ٣٨ = ٢٢٨، و٧ × ٦٨ = ٤٧٦.',
'WorkedExamples':'١. ٥ × ٣٧ = ٣٥ + ١٥٠ = ١٨٥؛ أو بالطريقة الرأسية ٣٥ آحادًا و١٨ عشرات. ٢. ٦ × ٣٨ = ٤٨ + ١٨٠ = ٢٢٨. ٣. ٧ × ٦٨ = ٥٦ + ٤٢٠ = ٤٧٦. ٤. ٤ × ٢٤ = ٩٦.',
'StepByStepSolutions':'١. أقدر حاصل الضرب لأقرب عشرة. ٢. أضرب الآحاد وأكتب الباقي في منزلة الآحاد وأحمل كل العشرات. ٣. أضرب العشرات وأضيف المحمول. ٤. أراجع الناتج بخاصية التوزيع.',
'CommonMistakes':'نسيان جمع المحمول بعد ضرب العشرات؛ كتابة ٣٥ آحادًا في منزلة الآحاد دون تجميع؛ الخطأ في معنى الصفر أو المنزلة؛ عدم مقارنة الناتج بالتقدير.',
'QuickSummary':'أضرب الآحاد والعشرات بالترتيب، وأعيد التجميع ثم أتحقق بالتقدير والتوزيع.'}
c=copy.deepcopy(next(x for x in C['Lessons'] if x['LessonCode']==prev))
c.update({'LessonCode':code,'OfficialReferenceCode':'UAE:REF:TEXTBOOK:G4:COMMON:T1:U04:L08',
'SourceLocator':'04-01.pdf U04 L08 printed pp241-246 PDF 264-269',
'TitleSourceReference':'04-01.pdf U04 L08 printed pp241-246 PDF 264-269',
'CanonicalBodySha256':hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest(),
'AdaptationStatus':'Original Arabic equivalent; Stage D pending'})
c['Translations']=[x for x in c['Translations'] if x['CultureCode']!='ar']+[t]
i=next(i for i,x in enumerate(C['Lessons']) if x['LessonCode']==prev);C['Lessons'].insert(i+1,c)
b=copy.deepcopy(next(x for x in B['Lessons'] if x['LessonCode']==prev))
b.update({'LessonCode':code,'SourceLessonCode':'EDU:UAE-MOE-MATH:L4:COMMON:U04:L08','LessonNumber':8,'Title':title,'SortOrder':31,
'OfficialReferenceCode':'UAE:REF:TEXTBOOK:G4:COMMON:T1:U04:L08'})
for x in b.get('Alignments',[]):x['ReferenceCode']=b['OfficialReferenceCode']
for x in B['Lessons']:
    if x['SortOrder']>=31:x['SortOrder']+=1
i=next(i for i,x in enumerate(B['Lessons']) if x['LessonCode']==prev);B['Lessons'].insert(i+1,b)
B['Units'][3]['LessonCount']=8;M['Units'][3]['ls'].append(title);M['LessonCount']=sum(len(u['ls']) for u in M['Units'])
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig');anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
s=s.replace(anchor,'''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:04:08"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:04:08", "number.column_multiplication_regroup",
            "COLUMN_MULTIPLICATION_WITH_REGROUPING",
            ["uae.g4.u04.l03.model_two_digit_product",
             "uae.g4.u04.l03.model_tens_product",
             "uae.g4.u04.l03.model_ones_product"],
            "UAE_G4_STUDENT_04_01_PRINTED_PP241_246_REUSED_EXTENDED_EXACT_FAMILIES_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''+anchor)
p.write_text(s,encoding='utf-8')
rich={'SchemaVersion':1,'ContentVersion':'uae-g4-u04-l08-ar-20261111','PackCode':'UAE-MOE-MATH','Lessons':[{'LessonCode':code,'CultureCode':'ar','Title':title,
'ExplanationParagraphs':['في الضرب الرأسي أبدأ بضرب الآحاد ثم أنتقل إلى العشرات.','إذا كانت نتيجة الآحاد أكبر من ٩، أكتب آحادها وأنقل العشرات إلى المنزلة التالية.','أتحقق من الإجابة بمقارنتها بالتقدير وناتج خاصية التوزيع.'],
'KeyConcepts':[{'Title':'المحمول','Definition':'عشرات نحصل عليها بعد ضرب الآحاد أو مئات بعد ضرب العشرات.','Rule':'أضيف المحمول عند الحساب في المنزلة التالية.','Example':'٥×٧=٣٥؛ نكتب ٥ ونحمل ٣.'},{'Title':'الضرب الرأسي','Definition':'ترتيب عمليات الضرب بحسب المنازل.','Rule':'أضرب الآحاد ثم العشرات.','Example':'٣٧×٥=١٨٥.'},{'Title':'التحقق بالتوزيع','Definition':'حساب الناتج مستقلًا بجمع نتائج ضرب العشرات والآحاد.','Rule':'أ×(ب+ج)=أب+أج.','Example':'٥×٣٧=٥×٣٠+٥×٧=١٥٠+٣٥.'}],
'WorkedExamples':[{'Title':'ضرب ٣٧ في ٥','Question':'أوجد ٥×٣٧.','Method':'أستخدم الحمل والتحقق بالتوزيع.','Steps':['٥×٧=٣٥، أكتب ٥ وأحمل ٣.','٥×٣ عشرات=١٥ عشرات ثم أضيف ٣ عشرات.','١٨ عشرة و٥ آحاد =١٨٥.'],'Answer':'١٨٥','Check':'٥×(٣٠+٧)=١٨٥.'},{'Title':'مضاعفة ٣٨ ست مرات','Question':'أوجد ٦×٣٨.','Method':'إعادة تجميع آحاد الضرب.','Steps':['٦×٨=٤٨؛ أكتب ٨ وأحمل ٤.','٦×٣=١٨ وأضيف ٤ فيصبح ٢٢.','الناتج ٢٢٨.'],'Answer':'٢٢٨','Check':'٦×٣٠+٦×٨=٢٢٨.'},{'Title':'ضرب ٦٨ في ٧','Question':'أوجد ٧×٦٨.','Method':'أضرب الآحاد وأحمل ٥.','Steps':['٧×٨=٥٦؛ أكتب ٦ وأحمل ٥.','٧×٦=٤٢ وأضيف ٥ =٤٧.','الناتج ٤٧٦.'],'Answer':'٤٧٦','Check':'٧×(٦٠+٨)=٤٧٦.'}],
'CommonMistakes':[{'Mistake':'إهمال المحمول في ضرب العشرات.','WhyWrong':'يصبح الناتج أقل من الصحيح.','Correction':'أضيف العشرات التي جُمعت من الآحاد.'},{'Mistake':'كتابة ٣٥ في خانة الآحاد.','WhyWrong':'٣٥ تعني ثلاث عشرات وخمسة آحاد.','Correction':'أكتب ٥ وأحمل ٣.'}],
'SummaryPoints':['أضرب الآحاد ثم العشرات.','أعيد تجميع عشرات ناتج الآحاد.','أتحقق بالتقدير والتوزيع.'],
'Visuals':[{'Kind':'EquationSet','Title':'خطة الضرب مع إعادة التجميع','Description':'توضيح مراحل الضرب في عدد ذي رقمين.','Items':['٥ × ٧ = ٣٥','٥ × ٣٠ = ١٥٠','٣٥ + ١٥٠ = ١٨٥']}],'Videos':[]}]}
save(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u04-l08-ar.rich-lesson-v2.json',rich)
print('PASS U04 L08 source title and original lesson')
