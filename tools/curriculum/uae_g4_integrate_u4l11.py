import pathlib,json,copy,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
prev='PED:UAE-MOE-MATH:L4:COMMON:04:10';code='PED:UAE-MOE-MATH:L4:COMMON:04:11'
assert not any(x['LessonCode']==code for x in C['Lessons']) and not any(x['LessonCode']==code for x in B['Lessons'])
assert B['Units'][3]['LessonCount']==10
title='الضرب مع وجود الأصفار'
t={'CultureCode':'ar','Title':title,'Explanation':'عند ضرب عدد متعدد الأرقام يضم أصفارًا في عامل مكون من رقم واحد أحافظ على المنزلة التي يظهر فيها الصفر. يمكن استخدام خاصية التوزيع أو الضرب الرأسي، ثم أتحقق بالتقدير. الصفر في منزلة المئات لا يعني حذف المئات من العدد أو تغيير مواقع الأرقام الأخرى.',
'KeyConceptsAndRules':'٦ × ١٠٨ = ٦ × (١٠٠ + ٠ + ٨) = ٦٠٠ + ٠ + ٤٨ = ٦٤٨. ٣ × ٢٠٢٥ = ٦٠٠٠ + ٠ + ٦٠ + ١٥ = ٦٠٧٥. ٩ × ٩٠٠٢ = ٨١٠١٨. الصفر مضروبًا في أي عامل يساوي صفرًا، ويجب مراعاة حمل المنازل السابقة.',
'WorkedExamples':'١. ٦ × ١٠٨ = ٦٤٨. ٢. ٣ × ٢٠٢٥ = ٦٠٧٥. ٣. ٢ × ٢٠١ = ٤٠٢. ٤. ٥ × ٧٠٩ = ٣٥٤٥. ٥. ٩ × ٩٠٠٢ = ٨١٠١٨.',
'StepByStepSolutions':'١. أكتب العدد حسب قيمه المكانية بما فيها الأصفار. ٢. أضرب الآحاد مع إعادة التجميع. ٣. أضرب العشرات والمئات والألوف مع اعتبار الصفر في منزلته وإضافة المحمول عند وجوده. ٤. أتحقق بخاصية التوزيع والتقدير.',
'CommonMistakes':'حذف صفر داخلي من العدد أثناء الضرب؛ نسيان إضافة المحمول إلى منزلة صفر؛ تحريك الأرقام بين المنازل؛ افتراض أن ناتج الضرب سيكون صفرًا بمجرد وجود صفر في عامل متعدد الأرقام.',
'QuickSummary':'أحافظ على مواضع الأصفار وأجمع المحمول إلى المنزلة الصحيحة ثم أتحقق بالتقدير.'}
c=copy.deepcopy(next(x for x in C['Lessons'] if x['LessonCode']==prev))
c.update({'LessonCode':code,'OfficialReferenceCode':'UAE:REF:TEXTBOOK:G4:COMMON:T1:U04:L11',
'SourceLocator':'04-01.pdf U04 L11 printed pp261-264 PDF 284-287',
'TitleSourceReference':'04-01.pdf U04 L11 printed pp261-264 PDF 284-287',
'CanonicalBodySha256':hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest(),
'AdaptationStatus':'Original Arabic equivalent; Stage D pending'})
c['Translations']=[x for x in c['Translations'] if x['CultureCode']!='ar']+[t]
i=next(i for i,x in enumerate(C['Lessons']) if x['LessonCode']==prev);C['Lessons'].insert(i+1,c)
b=copy.deepcopy(next(x for x in B['Lessons'] if x['LessonCode']==prev))
b.update({'LessonCode':code,'SourceLessonCode':'EDU:UAE-MOE-MATH:L4:COMMON:U04:L11','LessonNumber':11,'Title':title,'SortOrder':34,
'OfficialReferenceCode':'UAE:REF:TEXTBOOK:G4:COMMON:T1:U04:L11'})
for x in b.get('Alignments',[]):x['ReferenceCode']=b['OfficialReferenceCode']
for x in B['Lessons']:
    if x['SortOrder']>=34:x['SortOrder']+=1
i=next(i for i,x in enumerate(B['Lessons']) if x['LessonCode']==prev);B['Lessons'].insert(i+1,b)
B['Units'][3]['LessonCount']=11;M['Units'][3]['ls'].append(title);M['LessonCount']=sum(len(u['ls']) for u in M['Units'])
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig');anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
f=['uae.g4.u04.l09.multidigit_product','uae.g4.u04.l09.partial_hundreds','uae.g4.u04.l09.round_estimate']
cstr='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:04:11"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:04:11", "number.multidigit_multiply_with_zeros",
            "MULTIDIGIT_ZERO_PLACEHOLDER_MULTIPLICATION",
            ['''+', '.join('"'+x+'"' for x in f)+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP261_264_REUSED_MULTIDIGIT_FAMILIES_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
p.write_text(s.replace(anchor,cstr+anchor),encoding='utf-8')
rich={'SchemaVersion':1,'ContentVersion':'uae-g4-u04-l11-ar-20261111','PackCode':'UAE-MOE-MATH','Lessons':[{'LessonCode':code,'CultureCode':'ar','Title':title,
'ExplanationParagraphs':['عند ضرب عدد يحتوي أصفارًا أترك الصفر في منزلته ولا أحذف أي قيمة مكانية.','أبدأ من الآحاد، ثم أتابع إلى العشرات والمئات والألوف وأضيف المحمول حتى في المنزلة التي تحتوي صفرًا.','أتحقق بحساب نواتج الضرب الجزئية والتقدير.'],
'KeyConcepts':[{'Title':'الصفر في العدد','Definition':'منزلة لا تحتوي وحدات من قيمتها لكنها تحافظ على ترتيب العدد.','Rule':'أ × ٠ = ٠ مع إضافة المحمول عند وجوده.','Example':'٦ × ١٠٨ = ٦٤٨.'},{'Title':'قيمة المنزلـة','Definition':'قيمة الرقم حسب موقعه من الآحاد إلى الألوف.','Rule':'لا أحذف صفرًا داخليًا أثناء إجراء الضرب.','Example':'٢٠٢٥ ليس ٢٢٥.'},{'Title':'التحقق بالنواتج الجزئية','Definition':'تفكيك العدد إلى قيمه المكانية وجمع الضرب الجزئي.','Rule':'أ × (ألوف + مئات + عشرات + آحاد).','Example':'٣ × ٢٠٢٥ = ٦٠٠٠ + ٦٠ + ١٥ = ٦٠٧٥.'}],
'WorkedExamples':[{'Title':'الضرب مع صفر في العشرات','Question':'احسب ٦ × ١٠٨.','Method':'نموذج مساحة أو توزيع.','Steps':['١٠٨ = ١٠٠ + ٠ + ٨.','٦ × ١٠٠ = ٦٠٠ و٦ × ٨ = ٤٨.','الناتج ٦٤٨.'],'Answer':'٦٤٨','Check':'٦ × ١٠٠ ≈ ٦٠٠.'},{'Title':'عدد به صفر في المئات','Question':'احسب ٣ × ٢٠٢٥.','Method':'الضرب الرأسي.','Steps':['٣ × ٥ = ١٥، أكتب ٥ وأنقل ١.','٣ × ٢ عشرات +١ =٧ عشرات و٣ × صفر مئات =٠.','٣ × ٢ ألوف=٦ ألوف، الناتج ٦٠٧٥.'],'Answer':'٦٠٧٥','Check':'٦٠٠٠ + ٦٠ + ١٥ = ٦٠٧٥.'},{'Title':'صفر في العشرات','Question':'احسب ٥ × ٧٠٩.','Method':'توزيع على القيم المكانية.','Steps':['٥ × ٧٠٠ = ٣٥٠٠.','٥ × صفر عشرات =٠.','٥ × ٩ = ٤٥، والمجموع ٣٥٤٥.'],'Answer':'٣٥٤٥','Check':'٥ × ٧٠٠ ≈ ٣٥٠٠.'}],
'CommonMistakes':[{'Mistake':'حذف صفر داخلي عند كتابة العدد.','WhyWrong':'يتغير العدد وقيم منازله.','Correction':'أبقي الصفر في مكانه أثناء الضرب.'},{'Mistake':'إهمال المحمول حين تكون المنزلة الأصلية صفرًا.','WhyWrong':'يُهمل قيمة حمل صحيحة.','Correction':'أجمع المحمول حتى عند ضرب الصفر.'}],
'SummaryPoints':['لا أحذف الأصفار الداخلية من العدد.','أضرب كل منزلة وأضيف المحمول.','أراجع الناتج بالتوزيع والتقدير.'],
'Visuals':[{'Kind':'EquationSet','Title':'الضرب مع الأصفار','Description':'نموذج للقيمة المكانية عند وجود صفر.','Items':['٣ × ٢٠٢٥ = ٣ × (٢٠٠٠ + ٠ + ٢٠ + ٥)','٦٠٠٠ + ٠ + ٦٠ + ١٥','= ٦٠٧٥']}],'Videos':[]}]}
save(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u04-l11-ar.rich-lesson-v2.json',rich)
print('PASS U04 L11 student book title and existing multidigit engine')
