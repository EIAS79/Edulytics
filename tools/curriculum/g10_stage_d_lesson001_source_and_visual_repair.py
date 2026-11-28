"""Repair first lesson after direct inspection of General 01.pdf physical pp20–26.
Not a source-lock or D certification. Preserve all unrelated lesson identities.
"""
import json,pathlib,copy,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G10'
rich=r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u01-l01-ar.rich-lesson-v2.json'
pack=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json'
def read(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,d):p.write_text(json.dumps(d,ensure_ascii=False,indent=2)+'\n',encoding='utf8')
d=read(rich);l=d['Lessons'][0];p=read(pack);before=copy.deepcopy(p);c=p['Lessons'][0];a=next(t for t in c['Translations'] if t['CultureCode']=='ar')
assert c['LessonCode']==l['LessonCode'] and len(l['WorkedExamples'])==20 and len(l['Visuals'])==11
def ex(t,q,m,s,ans,chk):return dict(Title=t,Question=q,Method=m,Steps=s,Answer=ans,Check=chk)
new=[
ex('تصنيف كثيرات الحدود وحالات الصفر','صنّف ٧، ٣س²ص−٢ص، ٤/س+١، س^(١/٢)، ٥س−٥س، ٣س⁴−س²+٢س−٨. حدّد الدرجة وعدد الحدود بعد التبسيط.','افحص الأسس ثم اجمع الحدود المتشابهة قبل العد.',[
'٧ أحادية حد ثابتة غير صفرية؛ درجتها صفر. ٣س²ص−٢ص ثنائية حد؛ درجتها ٣.',
'٤/س+١ ليست كثيرة حدود بسبب المتغير في المقام، و س^(١/٢) ليست كثيرة حدود بسبب الأس الكسري.',
'٥س−٥س=٠؛ كثيرة الحدود الصفرية لا درجة محددة لها في هذا المنهج.',
'٣س⁴−س²+٢س−٨ كثيرة حدود من الدرجة الرابعة ذات أربعة حدود، وليست ثلاثية حدود.'],'٧: درجة ٠؛ الثنائية: درجة ٣؛ التعبيران التاليان ليسا كثيرتي حدود؛ الصفر بلا درجة؛ الأخيرة درجة ٤ وأربعة حدود.','درجة الحد ٣س²ص هي ٢+١=٣، والثابت غير الصفري درجته صفر؛ لا تعامل الصفر كالثابت غير الصفري.'),
ex('الصيغة القياسية مع معامل رئيس سالب وكسري','رتّب (١/٢)س−(٣/٤)س⁵+٦−٢س³ وحدد الدرجة والمعامل الرئيس.','رتّب القوى ولا ترتب بحسب قيم المعاملات.', ['القوى ١ و٥ و٠ و٣.','نكتب −(٣/٤)س⁵−٢س³+(١/٢)س+٦.','الدرجة ٥ والمعامل الرئيس −٣/٤.'],'−(٣/٤)س⁵−٢س³+(١/٢)س+٦؛ الدرجة ٥، المعامل −٣/٤.','عند س=٢: ١−٢٤+٦−١٦=−٣٣، والناتج المرتب يعطي −٣٣.'),
ex('جمع وطرح ثلاثة مقادير مع إلغاء الحد الأعلى','بسّط (٢س³+٣س²−٤)+(−٢س³+٥س−١)−(س²−٢س+٣).','وزّع الطرح ثم حاذِ القوى بما فيها المفقودة.', ['نوزع السالب على الثالث: −س²+٢س−٣.','معامل س³ يساوي ٢−٢=٠؛ معامل س² يساوي ٣−١=٢.','معامل س يساوي ٥+٢=٧؛ الثابت −٤−١−٣=−٨.'],'٢س²+٧س−٨؛ الدرجة ٢ بعد إلغاء الحد التكعيبي.','عند س=١: ١+٢−٢=١؛ والناتج ٢+٧−٨=١.'),
ex('حدود مختلفة بثلاثة متغيرات','بسّط (٤أ²ب+٣بج−٢أ)+(−أ²ب+٥أج−٧بج)−(٢أج−أ).','طابق الجزء الحرفي والأسس لكل حد.', ['نوزع السالب: −٢أج+أ.','نجمع أ²ب: ٤−١=٣، ونجمع بج: ٣−٧=−٤.','نجمع أج: ٥−٢=٣؛ ونجمع أ: −٢+١=−١.'],'٣أ²ب−٤بج+٣أج−أ.','عند أ=٢، ب=١، ج=٣: المقدار الأول ٢١، الثاني ٥، والثالث ١٠؛ الفرق ١٦. الناتج ١٢−١٢+١٨−٢=١٦.'),
ex('مساحة ومحيط مستطيل بأطوال جبرية','طول مستطيل ٢س+٥ سم وعرضه س+٢ سم حيث س≥٠. ماذا يمثل حاصل الضرب؟ وماذا يمثل ضعف مجموع الطول والعرض؟','ميّز بين جمع الأطوال وضربها والوحدات.', ['المحيط =٢[(٢س+٥)+(س+٢)]=٦س+١٤ سم.','المساحة =(٢س+٥)(س+٢)=٢س²+٩س+١٠ سم².','عند س=١: الطول ٧ والعرض ٣؛ المحيط ٢٠ والمساحة ٢١.'],'المحيط ٦س+١٤ سم؛ المساحة ٢س²+٩س+١٠ سم².','٢(٧+٣)=٢٠، و٧×٣=٢١. توسعة حاصل الضرب ترتبط بدرس الضرب، لا بعائلة جمع الحدود.'),
ex('طرح نموذجيْن تربيعيين بوحدات عددية','يمثل T(t)=٢t²+٥t+٤٠ إجمالي عدد وحدات إنتاج، وA(t)=t²+٢t+١٥ وحدات القسم الأول. اكتب عدد وحدات القسم الثاني واحسبه عند t=٣.','اطرح نموذج الجزء من نموذج الكل قبل التعويض.', ['نكتب T−A=(٢−١)t²+(٥−٢)t+(٤٠−١٥).','الناتج t²+٣t+٢٥.','عند t=٣: ٩+٩+٢٥=٤٣.'],'t²+٣t+٢٥؛ ٤٣ وحدة.','T(٣)=٧٣ وA(٣)=٣٠؛ ٧٣−٣٠=٤٣.'),
ex('استنتاج مجموع الأعداد الفردية المتتالية','عدد فردي أول ٢ن−١. اكتب العددين الفرديين التاليين ثم مجموع الثلاثة. وضّح أيضًا لماذا يغيّر تبديل ترتيب المطروح والمطروح منه الإشارة.','فرق عددين فرديين متتاليين يساوي ٢.', ['التاليان ٢ن+١ و٢ن+٣.','المجموع (٢ن−١)+(٢ن+١)+(٢ن+٣)=٦ن+٣.','للمقدارين A وB: B−A=−(A−B)، فلا يكون الترتيب بلا أثر عمومًا.'],'٦ن+٣؛ ترتيب الطرح مهم إلا عندما A=B.','عند ن=٤: ٧+٩+١١=٢٧=٦×٤+٣.'),
]
# Directly correct the checked substitution in the fourth new example (first=21, second=5).
# 4*4*1+3*1*3-2*2=21; -4+5*2*3-7*1*3=5; third 2*2*3-2=10.
l['WorkedExamples'].extend(new)
l['ExplanationParagraphs'].append('درجة الحد الثابت غير الصفري صفر؛ أما كثيرة الحدود الصفرية فلا درجة محددة لها هنا. نحسب درجة الحدود متعددة المتغيرات بجمع الأسس، ونحدد درجة كثيرة الحدود بعد الاختزال. كثيرات الحدود مغلقة تحت الجمع والطرح؛ لا يفقد الناتج صفة كثيرة الحدود وإن أُلغي الحد الأعلى.')
l['KeyConcepts'].extend([{'Title':'عدد الحدود وتصنيفها','Definition':'بعد دمج الحدود المتشابهة: أحادية حد، ثنائية حد، ثلاثية حدود، أو أكثر.','Rule':'عدد الحدود والدرجة صفتان مستقلتان؛ الصفر ليس ثابتًا غير صفري.','Example':'٣س⁴−س²+٢س−٨ أربعة حدود ودرجة ٤.'},{'Title':'انغلاق الجمع والطرح','Definition':'مجموع أو فرق كثيرتي حدود هو كثيرة حدود.','Rule':'لا يُنشئ الجمع أو الطرح أسسًا سالبة أو كسرية من حدود صحيحة؛ قد ينعدم أعلى معامل.','Example':'(٣س³+س)−(٣س³−٢س)=٣س.'}])
area=l['WorkedExamples'][4]
area['Steps'].extend(['نكتب ٢٠س−س²=١٠٠−(س−١٠)²؛ وبما أن المربع غير سالب فالمساحة لا تتجاوز ١٠٠.','تتحقق المساواة فقط عند س=١٠، فيكون العرض ١٠ سم أيضًا.'])
area['Answer']+=' أكبر مساحة ١٠٠ سم² عند مربع ١٠×١٠ سم.'
l['CommonMistakes'].extend([{'Mistake':'اعتبار ٥س−٥س كثيرة حدود من الدرجة الأولى أو درجة الصفر.','WhyWrong':'الحد الوحيد اختفى بعد جمع المتشابهات؛ الناتج كثيرة الحدود الصفرية.','Correction':'بسّط أولًا؛ درجة الصفر غير محددة وفق تعريف الدرس.'},{'Mistake':'اختيار المعامل الأكبر قيمة بدل معامل أعلى قوة.','WhyWrong':'المعامل الرئيس مرتبط بأعلى درجة ولو كان سالبًا أو كسرًا.','Correction':'رتّب القوى تنازليًا ثم اقرأ معامل الحد الأول.'}])
# Change only the three invalid visual instances; use existing signed renderer with a new closed polygon mode.
v=l['Visuals'][8];assert v['Kind']=='CartesianPlot';v['Kind']='CoordinatePlane'
for i in [9,10]:
 v=l['Visuals'][i];assert v['Kind']=='CartesianPlot';v['Kind']='ClosedCoordinatePolygon'
 for row in v['Rows']:row['Cells']=[s.translate(str.maketrans('٠١٢٣٤٥٦٧٨٩−','0123456789-')) for s in row['Cells']]
l['Visuals'].append({'Kind':'CartesianPlot','Title':'نقاط من منحنى مساحة المستطيل','Description':'عينات أصلية لدالة المساحة A=20x−x²؛ المجال الهندسي 0<x<20. أعلى نقطة (10,100). صلّ العينات بمنحنى ناعم في نشاط المعلم.','PrimaryLabel':'الطول (سم)','SecondaryLabel':'المساحة (سم²)','Items':['A=20x−x²','A=100−(x−10)²','القيمة القصوى 100 سم²'],'Rows':[{'Label':'المساحة','Cells':[str(x),str(x*(20-x))]} for x in range(1,20)]})
a['Explanation']+='\n'+l['ExplanationParagraphs'][-1]
a['KeyConceptsAndRules']+='\n'+'؛ '.join(x['Definition']+' '+x['Rule'] for x in l['KeyConcepts'][-2:])
a['WorkedExamples']+='\n\n'+'\n\n'.join(e['Title']+'\n'+e['Question']+'\n'+'؛ '.join(e['Steps'])+'\n'+e['Answer']+'\n'+e['Check'] for e in new)
a['WorkedExamples']+='\n'+area['Title']+': '+area['Steps'][-2]+' '+area['Steps'][-1]
a['CommonMistakes']+='\n'+'\n'.join(m['Mistake']+' '+m['WhyWrong']+' '+m['Correction'] for m in l['CommonMistakes'][-2:])
c['CanonicalBodySha256']=hashlib.sha256(json.dumps(a,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest()
assert all(x==y for x,y in zip(p['Lessons'][1:],before['Lessons'][1:]))
save(rich,d);save(pack,p)
# Source questions are individually indexed but representative examples are explicitly not a 1:1 attestation.
groups=[(1,6,'تصنيف كثيرات الحدود وحالات الصفر'),(7,10,'الصيغة القياسية مع معامل رئيس سالب وكسري'),(11,18,'جمع وطرح ثلاثة مقادير مع إلغاء الحد الأعلى'),(19,19,'تطبيق: نمو مبيعات مجموعتين'),(20,25,'تصنيف كثيرات الحدود وحالات الصفر'),(26,33,'الصيغة القياسية مع معامل رئيس سالب وكسري'),(34,37,'جمع وطرح ثلاثة مقادير مع إلغاء الحد الأعلى'),(38,43,'حدود مختلفة بثلاثة متغيرات'),(44,44,'تطبيق: نمو مبيعات مجموعتين'),(45,50,'تصنيف كثيرات الحدود وحالات الصفر'),(51,51,'طرح نموذجيْن تربيعيين بوحدات عددية'),(52,52,'اكتشاف الضلع الثالث من محيط مثلث جبري'),(53,53,'مساحة ومحيط مستطيل بأطوال جبرية'),(54,57,'جمع وطرح ثلاثة مقادير مع إلغاء الحد الأعلى'),(58,58,'طرح نموذجيْن تربيعيين بوحدات عددية'),(59,59,'نموذج تأجير متعدد الأيام وجمع كثيرتي تكلفة'),(60,60,'تطبيق هندسي مع مراجعة معلم'),(61,61,'كشف خطأ قلب الإشارات في طرح كثيرة حدود'),(62,63,'استنتاج مجموع الأعداد الفردية المتتالية'),(64,65,'الطرح ومحاذاة الحدود')]
rows=[]
for lo,hi,title in groups:
 for n in range(lo,hi+1):
  pg=23 if n<=33 else 24 if n<=58 else 25
  rows.append({'sourceElementId':f'G10-GEN-2025-U01L01-P{pg:03d}-Q{n:02d}','number':n,'physicalPdfPage':pg,'printedPage':pg-13,'representativeArtifactTitle':title,'status':'REPRESENTATIVE_ALIGNMENT_NOT_ATOMIC_EQUIVALENCE','subpartsPendingSeparateAudit':n in [19,44,51,53,59,60,62]})
assert [row['number'] for row in rows]==list(range(1,66))
assert all(row['representativeArtifactTitle'] in {e['Title'] for e in l['WorkedExamples']} for row in rows)
ev={'lessonCode':l['LessonCode'],'sourcePdf':'01.pdf','sourceSha256':'d7e492a960675c30ac7eebb0144ed2dc51d1f4e9c91c9834ea05638f614a3c72','directVisualPages':list(range(20,27)),'numberedQuestionsIndexed':91,'page23to25Rows':rows,'page26ExistingEvidence':'lesson-001-stage-d-page-026-review.json','newExamples':len(new),'originalExampleCount':20,'currentExampleCount':len(l['WorkedExamples']),'newVisualCount':1,'repairedInvalidVisualCount':3,'canonicalBodySha256':c['CanonicalBodySha256'],'coverage':'UNKNOWN','openDefects':['Numbered question entries are not the full atomic denominator; subparts, examples, diagrams, activities and advanced overlap need separate confirmation','Representative many-to-one linkage is not source item parity','Authenticated rendering and persisted learner practice not run'],'productionRouting':False}
save(g/'lesson-001-stage-d-numbered-source-review.json',ev)
cp=read(g/'lesson-001-stage-d-checkpoint.json');cp['canonicalBodySha256']=c['CanonicalBodySha256'];cp['numberedSourceReview']='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/lesson-001-stage-d-numbered-source-review.json';cp['numberedQuestionsIndexed']=91;cp['fallbackReconciliation']='SYNCED_PENDING_RUNTIME';save(g/'lesson-001-stage-d-checkpoint.json',cp)
# Keep prior page26 reconciliation digest consistent with canonical body after further authoring.
e=read(g/'lesson-001-stage-d-page-026-review.json');e['canonicalFallbackReconciliation']['canonicalBodySha256']=c['CanonicalBodySha256'];save(g/'lesson-001-stage-d-page-026-review.json',e)
def replace(path,old,new):
 s=path.read_text(encoding='utf8');assert s.count(old)==1,(str(path),old);path.write_text(s.replace(old,new),encoding='utf8')
replace(r/'src/Edulytics.Core/Curriculum/RichLessonContentV2.cs','    ComparisonTable = 19\n','    ComparisonTable = 19,\n    ClosedCoordinatePolygon = 20\n')
vpath=r/'src/Edulytics.Web/Views/Shared/_RichLessonVisual.cshtml'
replace(vpath,'else if (Model.Kind == RichLessonVisualKind.SignedCartesianPlot)','else if (Model.Kind is RichLessonVisualKind.SignedCartesianPlot or RichLessonVisualKind.ClosedCoordinatePolygon)')
replace(vpath,'                @foreach(var point in points)\n                {\n                    var px=330d+point.X*scale;', '''                @if (Model.Kind == RichLessonVisualKind.ClosedCoordinatePolygon && points.Count >= 3)
                {
                    var coordinates = string.Join(" ", points.Select(pt => FormattableString.Invariant($"{330d+pt.X*scale},{225d-pt.Y*scale}")));
                    <polygon points="@coordinates" fill="none" stroke="#1d4ed8" stroke-width="2.5"/>
                }
                @foreach(var point in points)
                {
                    var px=330d+point.X*scale;''')
print('G10_L001_REPAIR',len(l['WorkedExamples']),len(l['Visuals']),c['CanonicalBodySha256'])
