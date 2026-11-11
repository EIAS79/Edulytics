import pathlib,json,copy,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def read(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
rp=r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u04-l06-ar.rich-lesson-v2.json'
C=read(cp);B=read(bp);M=read(mp)
prev='PED:UAE-MOE-MATH:L4:COMMON:04:05';code='PED:UAE-MOE-MATH:L4:COMMON:04:06'
assert not any(x['LessonCode']==code for x in C['Lessons']) and not any(x['LessonCode']==code for x in B['Lessons'])
assert B['Units'][3]['LessonCount']==5
title='نشاط عملي: تمثيل إعادة التجميع'
t={'CultureCode':'ar','Title':title,'Explanation':'أمثل ضرب عدد من رقمين في رقم واحد باستخدام مكعبات القيمة المكانية. إذا نتج من الآحاد عشرة أو أكثر، أجمع كل عشرة آحاد وأستبدلها بعشرة واحدة. ثم أضيف العشرات الناتجة إلى العشرات الأصلية وأكتب حاصل الضرب.','KeyConceptsAndRules':'٣ × ٢٦: آحاد = ٣ × ٦ = ١٨ آحادًا = عشرة واحدة و٨ آحاد. العشرات = ٣ × ٢٠ = ٦٠، ومع العشرة الجديدة تصبح ٧٠، والناتج ٧٨. وفي ٤ × ٣١: ٤ × ٣٠ = ١٢٠ = مئة وعشرتان، ثم +٤ آحاد =١٢٤.','WorkedExamples':'١. ٣ × ٢٦ = ٦٠ + ١٨ = ٧٨. ٢. ٤ × ٣١ = ١٢٠ + ٤ = ١٢٤. ٣. ٤ × ١٤ = ٤٠ + ١٦ = ٥٦. ٤. ٢ × ٤٦ = ٨٠ + ١٢ = ٩٢.','StepByStepSolutions':'أرسم مجموعات متساوية من العشرات والآحاد. أحسب آحاد الناتج. أعيد تجميع كل ١٠ آحاد في عشرة. أحسب العشرات مع العشرات الجديدة، وأعيد تجميع كل ١٠ عشرات في مئة. أكتب القيمة النهائية وأتحقق بالجمع أو الضرب.','CommonMistakes':'ترك ١٨ في منزلة الآحاد دون إعادة تجميع؛ نسيان العشرة المحمولة؛ عدم تحويل ١٢ عشرة إلى مئة وعشرتين؛ تمثيل العدد بطريقة تغير عدد المجموعة.','QuickSummary':'كل عشر آحاد تعادل عشرة؛ وكل عشر عشرات تعادل مئة. أستعمل هذا التكافؤ في تمثيل ناتج الضرب.'}
c=copy.deepcopy(next(x for x in C['Lessons'] if x['LessonCode']==prev))
c['LessonCode']=code;c['OfficialReferenceCode']='UAE:REF:TEXTBOOK:G4:COMMON:T1:U04:L06'
c['Translations']=[x for x in c['Translations'] if x['CultureCode']!='ar']+[t]
c['SourceLocator']='04-01.pdf U04 L06 printed pp229-234 PDF 252-257'
c['TitleSourceReference']=c['SourceLocator'];c['CanonicalBodySha256']=hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest()
c['AdaptationStatus']='Original Arabic equivalent, Stage D pending'
i=next(i for i,x in enumerate(C['Lessons']) if x['LessonCode']==prev);C['Lessons'].insert(i+1,c)
b=copy.deepcopy(next(x for x in B['Lessons'] if x['LessonCode']==prev))
b.update({'LessonCode':code,'SourceLessonCode':'EDU:UAE-MOE-MATH:L4:COMMON:U04:L06','LessonNumber':6,'Title':title,'SortOrder':29,'OfficialReferenceCode':'UAE:REF:TEXTBOOK:G4:COMMON:T1:U04:L06'})
for x in b.get('Alignments',[]):x['ReferenceCode']=b['OfficialReferenceCode']
for x in B['Lessons']:
    if x['SortOrder']>=29:x['SortOrder']+=1
i=next(i for i,x in enumerate(B['Lessons']) if x['LessonCode']==prev);B['Lessons'].insert(i+1,b)
B['Units'][3]['LessonCount']=6;M['Units'][3]['ls'].append(title);M['LessonCount']=sum(len(u['ls']) for u in M['Units'])
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig')
anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
s=s.replace(anchor,'''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:04:06"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:04:06", "number.regroup_base_ten_models",
            "BASE_TEN_BLOCKS_REGROUPING",
            ["uae.g4.u04.l03.model_two_digit_product",
             "uae.g4.u04.l03.model_tens_product",
             "uae.g4.u04.l03.model_ones_product"],
            "UAE_G4_STUDENT_04_01_PRINTED_PP229_234_REUSE_EXACT_FAMILIES_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''+anchor)
p.write_text(s,encoding='utf-8')
rich={'SchemaVersion':1,'ContentVersion':'uae-g4-u04-l06-ar-20261111','PackCode':'UAE-MOE-MATH','Lessons':[{'LessonCode':code,'CultureCode':'ar','Title':title,
'ExplanationParagraphs':['إعادة التجميع تعني استبدال عشر آحاد بعشرة واحدة أو عشر عشرات بمئة واحدة دون تغيير قيمة العدد.','أستعمل مكعبات القيمة المكانية لتمثيل مجموعات الضرب قبل وبعد إعادة التجميع.','أتأكد أن الناتج النهائي يتفق مع نواتج ضرب العشرات والآحاد.'],
'KeyConcepts':[{'Title':'إعادة تجميع الآحاد','Definition':'تحويل كل عشر آحاد إلى عشرة واحدة.','Rule':'١٠ آحاد = عشرة واحدة.','Example':'١٨ آحادًا = عشرة واحدة و٨ آحاد.'},{'Title':'إعادة تجميع العشرات','Definition':'تحويل عشر عشرات إلى مئة واحدة.','Rule':'١٠ عشرات = ١٠٠.','Example':'١٢ عشرة = ١٠٠ + ٢٠.'}],
'WorkedExamples':[{'Title':'ثلاث مجموعات من ٢٦','Question':'مثّل ٣ × ٢٦ بالمكعبات.','Method':'العشرات والآحاد ثم إعادة التجميع.','Steps':['٣ × ٢٠ = ٦٠.','٣ × ٦ = ١٨ = ١٠ + ٨.','٦٠ + ١٠ + ٨ = ٧٨.'],'Answer':'٧٨','Check':'٢٦ + ٢٦ + ٢٦ = ٧٨.'},{'Title':'أربع مجموعات من ٣١','Question':'احسب ٤ × ٣١ بعد إعادة تجميع العشرات.','Method':'تكوين مئة واحدة من ١٠ عشرات.','Steps':['٤ × ٣٠ = ١٢٠.','٤ × ١ = ٤.','١٢٠ + ٤ = ١٢٤.'],'Answer':'١٢٤','Check':'١٢ عشرات = ١٠٠ + ٢٠.'},{'Title':'أربع مجموعات من ١٤','Question':'احسب ٤ × ١٤.','Method':'تحويل ١٦ آحادًا إلى عشرة و٦ آحاد.','Steps':['٤ × ١٠ = ٤٠.','٤ × ٤ = ١٦.','٤٠ + ١٦ = ٥٦.'],'Answer':'٥٦','Check':'١٤ أربع مرات = ٥٦.'}],
'CommonMistakes':[{'Mistake':'نسيان العشرة الجديدة','WhyWrong':'يقل الناتج عشر وحدات.','Correction':'أنقل كل عشر آحاد إلى منزلة العشرات.'}],
'SummaryPoints':['أمثل المجموعات بمكعبات العشرات والآحاد.','أحوّل عشر آحاد إلى عشرة.','أجمع العشرات والآحاد بعد إعادة التجميع.'],
'Visuals':[{'Kind':'EquationSet','Title':'إعادة التجميع في الضرب','Description':'تمثيل حسابي مستقل لناتج ٣ × ٢٦.','Items':['٣ × ٦ = ١٨ = ١٠ + ٨','٣ × ٢٠ = ٦٠','٦٠ + ١٠ + ٨ = ٧٨']}],'Videos':[]}]}
save(rp,rich)
print('PASS U04 L06 source-index addition + original Arabic content')
