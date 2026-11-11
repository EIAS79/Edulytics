import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:05:03';title='نشاط عملي: استخدام خاصية التوزيع في الضرب';unit='الضرب في عدد مكون من رقمين'
l=next(x for x in C['Lessons'] if x['LessonCode']==code);b=next(x for x in B['Lessons'] if x['LessonCode']==code)
assert b['SortOrder']==37
t={'CultureCode':'ar','Title':title,
'Explanation':'أستخدم نموذج المساحة لتوضيح خاصية التوزيع عند ضرب عدد مكون من رقمين في عدد مكون من رقمين. أفكك عاملًا إلى عشرات وآحاد، وأحسب كل جزء من مساحة المستطيل، ثم أجمع المساحتين لإيجاد ناتج الضرب.',
'KeyConceptsAndRules':'أ × (ب + ج) = أ×ب + أ×ج. ١٢ × ١٥ = ١٢ × (١٠ + ٥) = ١٢٠ + ٦٠ = ١٨٠. ٣٦ × ٢٤ = (٣٦ × ٢٠) + (٣٦ × ٤) = ٧٢٠ + ١٤٤ = ٨٦٤.',
'WorkedExamples':'١. ١٢ × ١٥ = ١٨٠. ٢. ٣٦ × ٢٤ = ٨٦٤. ٣. ٢٦ × ٢٥ = ٦٥٠. ٤. ٧٣ × ٣٤ = ٢٤٨٢. ٥. ١٨ × ٣٩ = ٧٠٢.',
'StepByStepSolutions':'١. أمثل العاملين بطولي بعدي المستطيل. ٢. أفكك عاملًا إلى عشرات وآحاد. ٣. أضرب العامل الآخر في العشرات ثم في الآحاد للحصول على مساحتي الجزأين. ٤. أجمع المساحتين وأتحقق من الناتج.',
'CommonMistakes':'نسيان أن العشرات تمثل عشرة أمثال الرقم؛ جمع العددين بدل جمع نواتج ضربهما؛ نسيان أحد جزأي النموذج؛ الخطأ في اختيار أبعاد المستطيل.',
'QuickSummary':'أفكك عاملًا إلى عشرات وآحاد وأوزع الضرب ثم أجمع النواتج الجزئية.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l.update({'SourceTitle':'UAE Grade 4 Common Arabic student book','SourceLocator':'04-01.pdf U05 L03 printed pp293-298 PDF316-321',
'TitleSourceReference':'04-01.pdf U05 L03 printed pp293-298 PDF316-321','SourceEdition':'Student book authority; year nonblocking',
'SourceSha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
'CanonicalBodySha256':hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest(),
'AdaptationStatus':'Original Arabic equivalent; Stage D pending'})
b['Title']=title;b['UnitTitle']=unit;M['Units'][4]['ls'][2]=title
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig');anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u05.l03.area_two_digit_product','uae.g4.u05.l03.area_tens_partial','uae.g4.u05.l03.area_ones_partial']
cstr='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:05:03"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:05:03", "number.area_distributive_two_digit",
            "AREA_MODEL_DISTRIBUTIVE_TWO_DIGIT",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP293_298_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,cstr+anchor),encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json';j=load(p)
template=next(x for x in j['families'] if x['id']=='uae.g4.u05.l02.estimate_two_digit_product')
for x in f:
    assert not any(e['id']==x for e in j['families'])
    z=dict(template);z.update({'id':x,'skillId':'number.area_distributive_two_digit','answerType':'exact_integer','verificationPolicy':'g4-u05-l03-student-source-stage-d-not-run',
    'representations':['area_model','equation'],'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(z)
save(p,j)
rich={'SchemaVersion':1,'ContentVersion':'uae-g4-u05-l03-ar-20261111','PackCode':'UAE-MOE-MATH','Lessons':[{'LessonCode':code,'CultureCode':'ar','Title':title,
'ExplanationParagraphs':['نموذج المساحة يبين تقسيم أحد العاملين إلى عشرات وآحاد.','مساحة كل جزء تمثل ناتج ضرب العامل الآخر في قسم من العدد.','نجمع الناتجين الجزئيين للحصول على المساحة الكلية أو حاصل الضرب.'],
'KeyConcepts':[{'Title':'خاصية التوزيع','Definition':'ضرب العدد في مجموع عددين يساوي جمع ناتجي ضربه بكل عدد.','Rule':'أ×(ب+ج)=أب+أج.','Example':'١٢×(١٠+٥)=١٢٠+٦٠.'},{'Title':'نموذج المساحة','Definition':'مستطيل مقسم إلى منطقتين تمثلان ناتجي الضرب الجزئيين.','Rule':'المساحة الكلية = مجموع المساحتين.','Example':'٣٦×٢٤=٧٢٠+١٤٤.'},{'Title':'النواتج الجزئية','Definition':'نتيجة ضرب كل قسم من العامل بعد التفكيك.','Rule':'أجمع نواتج العشرات والآحاد.','Example':'٢٦×٢٥=٥٢٠+١٣٠=٦٥٠.'}],
'WorkedExamples':[{'Title':'نموذج ١٢ في ١٥','Question':'أوجد ١٢×١٥ بنموذج مساحة.','Method':'تقسيم ١٥ إلى ١٠ و٥.','Steps':['١٥ = ١٠ + ٥.','١٢×١٠=١٢٠، و١٢×٥=٦٠.','١٢٠+٦٠=١٨٠.'],'Answer':'١٨٠','Check':'١٥×١٢=١٨٠.'},{'Title':'المساحة ٣٦ في ٢٤','Question':'أوجد ٣٦×٢٤.','Method':'تفكيك العامل ٢٤.','Steps':['٢٤=٢٠+٤.','٣٦×٢٠=٧٢٠ و٣٦×٤=١٤٤.','٧٢٠+١٤٤=٨٦٤.'],'Answer':'٨٦٤','Check':'مجموع مساحتي جزأي المستطيل.'},{'Title':'نماذج لمجموعات الحيوانات','Question':'احسب ٢٦×٢٥ بخاصية التوزيع.','Method':'٢٥ = ٢٠ + ٥.','Steps':['٢٦×٢٠=٥٢٠.','٢٦×٥=١٣٠.','٥٢٠+١٣٠=٦٥٠.'],'Answer':'٦٥٠','Check':'قريب من ٣٠×٢٠=٦٠٠.'}],
'CommonMistakes':[{'Mistake':'الضرب في رقم العشرات دون قيمته.','WhyWrong':'٢ ليست مساوية إلى ٢٠.','Correction':'أضرب في ٢٠ لا ٢.'},{'Mistake':'تجاهل مساحة أحد جزأي المستطيل.','WhyWrong':'يحذف ناتجًا جزئيًا.','Correction':'أجمع منطقتي النموذج.'}],
'SummaryPoints':['أرسم نموذج مساحة للعاملين.','أقسم عاملًا إلى عشرات وآحاد.','أحسب ثم أجمع نواتج الضرب الجزئية.'],
'Visuals':[{'Kind':'EquationSet','Title':'نموذج المساحة الجزئي','Description':'تفكيك أحد العاملين لتوضيح مساحة مستطيل.','Items':['٣٦ × ٢٤ = ٣٦ × (٢٠ + ٤)','٣٦ × ٢٠ = ٧٢٠','٣٦ × ٤ = ١٤٤','٧٢٠ + ١٤٤ = ٨٦٤']}],'Videos':[]}]}
save(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u05-l03-ar.rich-lesson-v2.json',rich)
print('PASS U05L03 area model distributive academic integration')
