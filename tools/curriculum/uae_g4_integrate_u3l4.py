import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:03:04'
title='المقارنة لحل المسائل';unit='فهم الضرب والقسمة'
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
t={'CultureCode':'ar','Title':title,
'Explanation':'عند مقارنة كميتين نحدد هل يسأل السؤال عن مقدار الفرق أم عن عدد مرات الزيادة. في مقارنة الجمع نوجد مقدار الزيادة أو النقصان باستخدام الطرح، أما في مقارنة الضرب فنبحث عن عدد الأضعاف باستخدام القسمة أو نوجد الكمية المقارنة بالضرب.',
'KeyConceptsAndRules':'مقارنة الجمع: كم يزيد العدد الأكبر عن الأصغر؟ الفرق = الأكبر − الأصغر. مقارنة الضرب: كم ضعفًا يمثل الأكبر الأصغر؟ العامل = الأكبر ÷ الأصغر. ويمكن أن تكون الكمية المقارنة = الكمية الأساسية × عامل المقارنة. أختار العملية وفق معنى السؤال.',
'WorkedExamples':'١. حضر أحمد ٤ مرات وحضر صديقه ثلاثة أضعافه: ٣ × ٤ = ١٢ مرة. ٢. لدى سندية ١٨ بطاقة وهي ستة أضعاف ما لدى محمد: ١٨ ÷ ٦ = ٣ بطاقات. ٣. يوجد ١٩ سمكة في حوض و٦ في حوض آخر؛ يزيد الأول ١٣ سمكة لأن ١٩ − ٦ = ١٣.',
'StepByStepSolutions':'١. اقرأ كلمات المقارنة: كم أكثر أو كم عدد الأضعاف. ٢. حدّد الكمية الأساسية والمقارنة. ٣. اكتب معادلة مناسبة للجمع أو الطرح أو الضرب أو القسمة. ٤. أحسب الناتج وأتحقق من وحداته وأنه معقول.',
'CommonMistakes':'استعمال الطرح للإجابة عن عدد الأضعاف؛ استعمال الضرب حين السؤال عن مقدار الزيادة؛ الخلط بين ٣ أضعاف و٣ إضافية؛ تجاهل كون القيم المقارنة في الوحدات نفسها.',
'QuickSummary':'أميز مقارنة الجمع من مقارنة الضرب باختيار العملية التي توافق مقدار الزيادة أو عدد الأضعاف.'}
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
l['SourceTitle']='UAE Grade 4 Arabic student mathematics, General'
l['SourceLocator']='04-01.pdf U03 L04 printed 153-158 PDF 176-181'
l['TitleSourceReference']=l['SourceLocator']
l['SourceEdition']='Student book source, year nonblocking'
l['SourceSha256']='859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3'
l['CanonicalBodySha256']=hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest()
l['AdaptationStatus']='Original Arabic math equivalent with reused existing exact families; Stage D pending'
b['Title']=title;b['UnitTitle']=unit
B['Units'][2]['Title']=unit
M['Units'][2]['title']=unit;M['Units'][2]['ls'][3]=title
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig')
anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u02.l08.bar_compare','uae.g4.u03.l03.times_as_many','uae.g4.u03.l03.unknown_comparison_base','uae.g4.u03.l03.unknown_multiplier']
contract='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:03:04"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:03:04",
            "number.additive_vs_multiplicative_comparison",
            "CHOOSE_ADDITIVE_OR_MULTIPLICATIVE_COMPARISON",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP153_158_REUSE_EXISTING_FAMILIES_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,contract+anchor),encoding='utf-8')
print('PASS U03 L04 student-book Arabic titles and reused four existing math families')
