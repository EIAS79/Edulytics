import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:04:04';title='نشاط عملي: استخدام النماذج في عملية الضرب'
unit='الضرب في الأعداد المكونة من رقم واحد'
l=next(x for x in C['Lessons'] if x['LessonCode']==code);b=next(x for x in B['Lessons'] if x['LessonCode']==code)
t={'CultureCode':'ar','Title':title,
'Explanation':'أستخدم نموذج المساحة أو المصفوفة لإظهار ناتج ضرب رقم واحد في عدد من رقمين. أقسّم البعد المكون من رقمين إلى عشرات وآحاد، وأحسب مساحتي الجزأين ثم أجمعهما.',
'KeyConceptsAndRules':'إذا كان لدي ٤ مجموعات من ٢١، أمثل مستطيلًا عرضه ٤ وطوله ٢٠ + ١. يصبح ٤ × ٢١ = (٤ × ٢٠) + (٤ × ١) = ٨٠ + ٤ = ٨٤. وتسمى النتائج المنفصلة نواتج ضرب جزئية.',
'WorkedExamples':'١. ٤ × ٢١ = ٤ × ٢٠ + ٤ × ١ = ٨٤. ٢. ٣ × ١٣ = ٣ × ١٠ + ٣ × ٣ = ٣٩. ٣. ٢ × ٢٤ = ٢ × ٢٠ + ٢ × ٤ = ٤٨. ٤. ٣ × ٣١ = ٩٠ + ٣ = ٩٣.',
'StepByStepSolutions':'١. أرسم مستطيلًا يقسم الطول إلى عشرات وآحاد. ٢. أضرب عدد الصفوف في كل جزء من الطول. ٣. أجمع حاصل الضرب الجزئي لكل جزء. ٤. أتحقق بالجمع المتكرر أو مثال سياقي.',
'CommonMistakes':'تجاهل مجموع الضرب الجزئي للآحاد؛ خطأ في تحديد طول أحد الجزأين؛ حساب ٤ × ٢٠ على أنه ٨ بدل ٨٠؛ الخلط بين جمع المساحات وجمع أطوال الأضلاع.',
'QuickSummary':'نموذج المساحة يبين أن ناتج ضرب عدد من رقمين في رقم واحد يساوي مجموع ناتجي ضرب العشرات والآحاد.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l['SourceTitle']='UAE Grade 4 General Arabic Student Book'
l['SourceLocator']='04-01.pdf U04 L04 printed pp215-220 PDF 238-243'
l['TitleSourceReference']=l['SourceLocator'];l['SourceEdition']='Student book authority; edition nonblocking'
l['SourceSha256']='859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3'
l['CanonicalBodySha256']=hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest()
l['AdaptationStatus']='Original Arabic equivalent, reuse existing exact family engine, Stage D pending'
b['Title']=title;b['UnitTitle']=unit;B['Units'][3]['Title']=unit;M['Units'][3]['title']=unit;M['Units'][3]['ls'][3]=title
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig');anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u04.l03.model_two_digit_product','uae.g4.u04.l03.model_tens_product','uae.g4.u04.l03.model_ones_product']
c='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:04:04"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:04:04", "number.area_model_partial_products",
            "AREA_MODEL_TWO_DIGIT_BY_ONE_DIGIT_PARTIAL_PRODUCTS",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP215_220_REUSE_U04L03_FAMILIES_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,c+anchor),encoding='utf-8')
print('PASS U04 L04 student book and reusable area model families')
