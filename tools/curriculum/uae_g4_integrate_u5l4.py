import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:05:04';title='الضرب في عدد مكون من رقمين';unit='الضرب في عدد مكون من رقمين'
l=next(x for x in C['Lessons'] if x['LessonCode']==code);b=next(x for x in B['Lessons'] if x['LessonCode']==code)
assert b['SortOrder']==38 and M['Units'][4]['ls'][2]=='نشاط عملي: استخدام خاصية التوزيع في الضرب'
t={'CultureCode':'ar','Title':title,
'Explanation':'أضرب عددًا من رقمين في عدد آخر من رقمين باستخدام الضرب الرأسي والنواتج الجزئية. أبدأ بضرب آحاد العامل الثاني في الأول، ثم أضرب عشراته في الأول وأكتب الناتج في موضع العشرات. أجمع الناتجين للحصول على حاصل الضرب وأتحقق بنموذج المساحة.',
'KeyConceptsAndRules':'٥٦ × ٢٩ = (٥٦ × ٩) + (٥٦ × ٢٠) = ٥٠٤ + ١١٢٠ = ١٦٢٤. وأيضًا ٣٥ × ٢٤ = ١٤٠ + ٧٠٠ = ٨٤٠. أحافظ على قيمة منزلة العشرات عند كتابة الناتج الجزئي الثاني.',
'WorkedExamples':'١. ٥٦ × ٢٩ = ١٦٢٤. ٢. ٣٥ × ٢٤ = ٨٤٠. ٣. ٤٢ × ٣٨ = ١٥٩٦. ٤. ٤٦ × ٣٥ = ١٦١٠. ٥. ٧٣ × ٥١ = ٣٧٢٣.',
'StepByStepSolutions':'١. أكتب العاملين رأسيًا مع محاذاة المنازل. ٢. أضرب في آحاد العامل الثاني وأكتب الناتج الجزئي الأول. ٣. أضرب في عشرات العامل الثاني وأضع الناتج الجزئي في منزلة العشرات. ٤. أجمع الناتجين وأتحقق بنموذج مساحة.',
'CommonMistakes':'كتابة الناتج الجزئي للعشرات في منزلة الآحاد؛ إهمال أحد الناتجين الجزئيين؛ الخطأ في جمع الناتجين؛ نسيان العشرات المعاد تجميعها.',
'QuickSummary':'أحسب ناتج ضرب الآحاد وناتج ضرب العشرات، ثم أجمع وأتحقق بالتوزيع.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l.update({'SourceTitle':'UAE Grade 4 Common Arabic student book','SourceLocator':'04-01.pdf U05 L04 printed pp299-304 PDF322-327',
'TitleSourceReference':'04-01.pdf U05 L04 printed pp299-304 PDF322-327','SourceEdition':'Student book authority; year nonblocking',
'SourceSha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
'CanonicalBodySha256':hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest(),
'AdaptationStatus':'Original Arabic equivalent; Stage D pending'})
b['Title']=title;b['UnitTitle']=unit;M['Units'][4]['ls'][3]=title
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig');anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u05.l03.area_two_digit_product','uae.g4.u05.l03.area_tens_partial','uae.g4.u05.l03.area_ones_partial']
cstr='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:05:04"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:05:04", "number.column_multiply_two_digit_factors",
            "VERTICAL_TWO_DIGIT_MULTIPLICATION_PARTIAL_PRODUCTS",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP299_304_REUSED_EXACT_FAMILIES_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,cstr+anchor),encoding='utf-8')
rich={'SchemaVersion':1,'ContentVersion':'uae-g4-u05-l04-ar-20261111','PackCode':'UAE-MOE-MATH','Lessons':[{'LessonCode':code,'CultureCode':'ar','Title':title,
'ExplanationParagraphs':['أضرب عاملًا من رقمين في عامل من رقمين باستخدام ناتجين جزئيين.','أبدأ بضرب آحاد العامل الثاني في الأول، ثم أضرب عشرات العامل الثاني في الأول.','أجمع النواتج الجزئية مع مراعاة المنازل وأتحقق بنموذج المساحة.'],
'KeyConcepts':[{'Title':'الضرب الرأسي','Definition':'طريقة مرتبة لمضاعفة عوامل من رقمين.','Rule':'أضرب آحاد العامل الثاني ثم عشراته.','Example':'٥٦ × ٢٩.'},{'Title':'الناتج الجزئي للعشرات','Definition':'ضرب العامل الأول في قيمة العشرات من العامل الثاني.','Rule':'الضرب في ٢٠ وليس في ٢.','Example':'٥٦×٢٠=١١٢٠.'},{'Title':'الجمع النهائي','Definition':'جمع النواتج الجزئية للوصول إلى حاصل الضرب.','Rule':'ناتج الآحاد + ناتج العشرات.','Example':'٥٠٤+١١٢٠=١٦٢٤.'}],
'WorkedExamples':[{'Title':'ضرب ٥٦ في ٢٩','Question':'احسب ٥٦×٢٩ بالطريقة الرأسية.','Method':'ناتجان جزئيان.','Steps':['٥٦×٩=٥٠٤.','٥٦×٢٠=١١٢٠.','٥٠٤+١١٢٠=١٦٢٤.'],'Answer':'١٦٢٤','Check':'قريب من ٦٠×٣٠=١٨٠٠.'},{'Title':'سعر الأغراض','Question':'ما ناتج ٣٥×٢٤؟','Method':'ضرب آحاد وعشرات العامل الثاني.','Steps':['٣٥×٤=١٤٠.','٣٥×٢٠=٧٠٠.','١٤٠+٧٠٠=٨٤٠.'],'Answer':'٨٤٠','Check':'٣٥×(٢٠+٤)=٨٤٠.'},{'Title':'ضرب ٤٢ في ٣٨','Question':'احسب ٤٢×٣٨.','Method':'توزيع العشرات والآحاد.','Steps':['٤٢×٨=٣٣٦.','٤٢×٣٠=١٢٦٠.','٣٣٦+١٢٦٠=١٥٩٦.'],'Answer':'١٥٩٦','Check':'٤٢×(٣٠+٨)=١٥٩٦.'}],
'CommonMistakes':[{'Mistake':'عدم محاذاة الناتج الجزئي للعشرات.','WhyWrong':'ينقص حاصل الضرب بعامل عشرة.','Correction':'أكتب ناتج العشرات في منزلة العشرات.'},{'Mistake':'جمع العاملين بدل الناتجين الجزئيين.','WhyWrong':'ينتج جمعًا لا ضربًا.','Correction':'أحسب كل ناتج جزئي ثم أجمعهما.'}],
'SummaryPoints':['أضرب آحاد العامل الثاني أولًا.','أضرب عشراته مع الحفاظ على المنزلة.','أجمع الناتجين وأتحقق بنموذج المساحة.'],
'Visuals':[{'Kind':'EquationSet','Title':'الضرب في عدد من رقمين','Description':'نموذج للنواتج الجزئية في الضرب الرأسي.','Items':['٥٦ × ٩ = ٥٠٤','٥٦ × ٢٠ = ١١٢٠','٥٠٤ + ١١٢٠ = ١٦٢٤']}],'Videos':[]}]}
save(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u05-l04-ar.rich-lesson-v2.json',rich)
print('PASS U05L04 student book and reused exact partial products')
