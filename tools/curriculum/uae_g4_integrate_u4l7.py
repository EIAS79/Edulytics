import pathlib,json,copy,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
prev='PED:UAE-MOE-MATH:L4:COMMON:04:06';code='PED:UAE-MOE-MATH:L4:COMMON:04:07'
assert not any(x['LessonCode']==code for x in C['Lessons']) and not any(x['LessonCode']==code for x in B['Lessons'])
assert B['Units'][3]['LessonCount']==6
title='خاصية التوزيع'
t={'CultureCode':'ar','Title':title,
'Explanation':'خاصية التوزيع تعني أنه يمكن ضرب العدد في مجموع عددين، فنضرب كل حد من حدود الجمع في العدد نفسه، ثم نجمع ناتجي الضرب. تفيد في الحساب الذهني وفي نماذج المساحة، وبخاصة عند تفكيك عدد من رقمين إلى عشرات وآحاد.',
'KeyConceptsAndRules':'أ × (ب + ج) = (أ × ب) + (أ × ج). على سبيل المثال ٦ × ١٢ = ٦ × (١٠ + ٢) = (٦ × ١٠) + (٦ × ٢) = ٦٠ + ١٢ = ٧٢.',
'WorkedExamples':'١. ٦ × ١٢ = ٦٠ + ١٢ = ٧٢. ٢. ٥ × ٢٧ = (٥ × ٢٠) + (٥ × ٧) = ١٠٠ + ٣٥ = ١٣٥. ٣. ٨ × ١٦ = ٨٠ + ٤٨ = ١٢٨. ٤. ٩ × ١٢ = ٩٠ + ١٨ = ١٠٨.',
'StepByStepSolutions':'١. أفكك العدد ذي الرقمين إلى مجموع العشرات والآحاد. ٢. أضرب العامل الآخر في جزء العشرات. ٣. أضربه في الآحاد. ٤. أجمع الناتجين الجزئيين وأتحقق بضرب رأسي أو نموذج مساحة.',
'CommonMistakes':'ضرب العامل في حد واحد فقط من المجموع؛ جمع العشرات والآحاد دون ضربهما؛ إغفال قيمة العشرات؛ كتابة ناتج الضرب الجزئي في منزلة خاطئة.',
'QuickSummary':'أوزع ضرب العدد على كل حد في المجموع، ثم أجمع النتائج.'}
c=copy.deepcopy(next(x for x in C['Lessons'] if x['LessonCode']==prev))
c.update({'LessonCode':code,'OfficialReferenceCode':'UAE:REF:TEXTBOOK:G4:COMMON:T1:U04:L07',
'SourceLocator':'04-01.pdf U04 L07 printed pp235-240 PDF 258-263',
'TitleSourceReference':'04-01.pdf U04 L07 printed pp235-240 PDF 258-263',
'CanonicalBodySha256':hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest(),
'AdaptationStatus':'Original Arabic equivalent; Stage D pending'})
c['Translations']=[x for x in c['Translations'] if x['CultureCode']!='ar']+[t]
i=next(i for i,x in enumerate(C['Lessons']) if x['LessonCode']==prev);C['Lessons'].insert(i+1,c)
b=copy.deepcopy(next(x for x in B['Lessons'] if x['LessonCode']==prev))
b.update({'LessonCode':code,'SourceLessonCode':'EDU:UAE-MOE-MATH:L4:COMMON:U04:L07','LessonNumber':7,'Title':title,'SortOrder':30,
'OfficialReferenceCode':'UAE:REF:TEXTBOOK:G4:COMMON:T1:U04:L07'})
for x in b.get('Alignments',[]):x['ReferenceCode']=b['OfficialReferenceCode']
for x in B['Lessons']:
    if x['SortOrder']>=30:x['SortOrder']+=1
i=next(i for i,x in enumerate(B['Lessons']) if x['LessonCode']==prev);B['Lessons'].insert(i+1,b)
B['Units'][3]['LessonCount']=7;M['Units'][3]['ls'].append(title);M['LessonCount']=sum(len(u['ls']) for u in M['Units'])
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Services/Mathematics/UaeGrade4Unit4Lesson03PlaceValueModelsQuestions.cs'
s=p.read_text(encoding='utf-8-sig')
assert 'rng.Next(2,6)' in s and 'a is <2 or >5' in s
s=s.replace('rng.Next(2,6)','rng.Next(2,10)').replace('a is <2 or >5','a is <2 or >9')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig')
anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
s=s.replace(anchor,'''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:04:07"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:04:07", "number.distributive_property",
            "DISTRIBUTIVE_PROPERTY_TWO_DIGIT_MULTIPLICATION",
            ["uae.g4.u04.l03.model_two_digit_product",
             "uae.g4.u04.l03.model_tens_product",
             "uae.g4.u04.l03.model_ones_product"],
            "UAE_G4_STUDENT_04_01_PRINTED_PP235_240_REUSED_EXTENDED_EXACT_FAMILIES_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''+anchor)
p.write_text(s,encoding='utf-8')
rich={'SchemaVersion':1,'ContentVersion':'uae-g4-u04-l07-ar-20261111','PackCode':'UAE-MOE-MATH','Lessons':[{'LessonCode':code,'CultureCode':'ar','Title':title,
'ExplanationParagraphs':['خاصية التوزيع تبين أن ضرب العدد في مجموع عددين يساوي مجموع ضربه في كل عدد منهما.','نستخدمها بتفكيك العشرات والآحاد ثم ضرب كل جزء على حدة.','يمكن تمثيل الخاصية بنموذج مساحة مستطيل مقسم إلى جزأين.'],
'KeyConcepts':[{'Title':'خاصية التوزيع','Definition':'توزيع العامل على جميع حدود الجمع.','Rule':'أ × (ب + ج) = أ×ب + أ×ج.','Example':'٦×١٢ = ٦×(١٠+٢).'},{'Title':'ناتج ضرب جزئي','Definition':'ناتج ضرب عامل بأحد الجزأين قبل جمع الناتجين.','Rule':'أجمع الناتجين للوصول إلى الناتج الكامل.','Example':'٦×١٠=٦٠، و٦×٢=١٢.'},{'Title':'نموذج المساحة','Definition':'مستطيل مقسم إلى جزأين بحسب عشرات العدد وآحاده.','Rule':'المساحة الكلية تساوي مجموع مساحتي الجزأين.','Example':'٨×١٦=٨٠+٤٨.'}],
'WorkedExamples':[{'Title':'ست مجموعات من ١٢','Question':'احسب ٦ × ١٢ بالتوزيع.','Method':'تفكيك إلى ١٠ + ٢.','Steps':['٦×١٠=٦٠.','٦×٢=١٢.','٦٠+١٢=٧٢.'],'Answer':'٧٢','Check':'١٢+١٢+١٢+١٢+١٢+١٢=٧٢.'},{'Title':'تذاكر عرض','Question':'٢٧ تذكرة ثمن الواحدة ٥ دراهم. ما الإجمالي؟','Method':'٥×(٢٠+٧).','Steps':['٥×٢٠=١٠٠.','٥×٧=٣٥.','١٠٠+٣٥=١٣٥.'],'Answer':'١٣٥ درهمًا','Check':'٢٧×٥=١٣٥.'},{'Title':'ثمان مجموعات من ١٦','Question':'أوجد ٨×١٦.','Method':'أفكك إلى ١٠ و٦.','Steps':['٨×١٠=٨٠.','٨×٦=٤٨.','٨٠+٤٨=١٢٨.'],'Answer':'١٢٨','Check':'١٦×٨=١٢٨.'}],
'CommonMistakes':[{'Mistake':'توزيع الضرب على الحد الأول فقط.','WhyWrong':'يُهمل جزءًا من العدد.','Correction':'أضرب العامل في كل حد ثم أجمع.'},{'Mistake':'نسيان أن ٢٠ تعني عشرين وليس ٢.','WhyWrong':'يؤدي إلى نتيجة أصغر.','Correction':'أبقي القيمة المكانية للعشرات.'}],
'SummaryPoints':['أفكك العدد إلى عشرات وآحاد.','أضرب العامل في كل جزء.','أجمع الناتجين الجزئيين.'],
'Visuals':[{'Kind':'EquationSet','Title':'خاصية التوزيع في الضرب','Description':'مخطط حسابي للضرب بتوزيع العامل على مجموع جزأين.','Items':['٦ × (١٠ + ٢)','(٦ × ١٠) + (٦ × ٢)','٦٠ + ١٢ = ٧٢']}],'Videos':[]}]}
save(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u04-l07-ar.rich-lesson-v2.json',rich)
print('PASS U04L07 created, reused extended base-ten model family')
