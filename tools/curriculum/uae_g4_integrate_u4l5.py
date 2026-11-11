import json,pathlib,copy,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:04:05';prev='PED:UAE-MOE-MATH:L4:COMMON:04:04'
assert not any(x['LessonCode']==code for x in C['Lessons']) and not any(x['LessonCode']==code for x in B['Lessons'])
assert len(M['Units'][3]['ls'])==B['Units'][3]['LessonCount']==4
title='الضرب في عدد مكون من رقمين';unit='الضرب في الأعداد المكونة من رقم واحد'
c=copy.deepcopy(next(x for x in C['Lessons'] if x['LessonCode']==prev))
c['LessonCode']=code;c['OfficialReferenceCode']='UAE:REF:TEXTBOOK:G4:COMMON:T1:U04:L05'
c['TitleSourceReference']='04-01.pdf U04 L05 printed 223-228 PDF246-251'
c['SourceLocator']=c['TitleSourceReference']
c['SourceTitle']='Owner selected UAE Grade4 Arabic student book'
c['SourceEdition']='Student authority, date nonblocking'
c['SourceSha256']='859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3'
t={'CultureCode':'ar','Title':title,
'Explanation':'أضرب عددًا من رقمين في رقم واحد باستخدام الطريقة الرأسية المبنية على القيمة المكانية. أبدأ بضرب الآحاد، ثم العشرات، وأكتب الناتج في المنزلة الصحيحة، وأتحقق بالتقدير ونموذج المساحة.',
'KeyConceptsAndRules':'عند ضرب ٢٤ × ٢ أبدأ ٢ × ٤ = ٨ آحاد، ثم ٢ × ٢ عشرات = ٤ عشرات، فيكون الناتج ٤٨. عندما يكون ناتج ضرب الآحاد ١٠ أو أكثر، أنقل العشرات الناتجة إلى منزلة العشرات.',
'WorkedExamples':'١. ٢٤ × ٢ = ٤٨. ٢. ٣١ × ٢ = ٦٢. ٣. ٤٢ × ٢ = ٨٤. ٤. ٢١ × ٤ = ٨٤. ٥. ١٣ × ٢ = ٢٦.',
'StepByStepSolutions':'١. أكتب الرقمين في عمودين aligned حسب الآحاد والعشرات. ٢. أضرب الآحاد في العامل. ٣. أضرب العشرات وأضيف أي عشرات أعيد تجميعها. ٤. أراجع الناتج بتقدير أو نموذج مساحة.',
'CommonMistakes':'بدء الضرب من العشرات ونسيان الآحاد؛ عدم محاذاة المنازل؛ نسيان إعادة تجميع عشرة آحاد؛ عدم التحقق بتقدير قريب.',
'QuickSummary':'أضرب الآحاد ثم العشرات بالطريقة الرأسية وأتحقق بالمساحة والتقدير.'}
c['Translations']=[x for x in c['Translations'] if x['CultureCode']!='ar']+[t]
c['CanonicalBodySha256']=hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest()
c['AdaptationStatus']='Original Arabic equivalent, Stage D pending'
i=next(i for i,x in enumerate(C['Lessons']) if x['LessonCode']==prev);C['Lessons'].insert(i+1,c)
b=copy.deepcopy(next(x for x in B['Lessons'] if x['LessonCode']==prev))
b.update({'LessonCode':code,'SourceLessonCode':'EDU:UAE-MOE-MATH:L4:COMMON:U04:L05','LessonNumber':5,'Title':title,'UnitTitle':unit,'SortOrder':28,
'OfficialReferenceCode':'UAE:REF:TEXTBOOK:G4:COMMON:T1:U04:L05'})
for x in b.get('Alignments',[]):x['ReferenceCode']=b['OfficialReferenceCode']
for x in B['Lessons']:
    if x['SortOrder']>=28:x['SortOrder']+=1
i=next(i for i,x in enumerate(B['Lessons']) if x['LessonCode']==prev);B['Lessons'].insert(i+1,b)
B['Units'][3]['LessonCount']=5;M['Units'][3]['ls'].append(title);M['LessonCount']=sum(len(u['ls']) for u in M['Units'])
save(cp,C);save(bp,B);save(mp,M)
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig');anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
s=s.replace(anchor,'''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:04:05"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:04:05", "number.column_multiply_two_digit_by_one",
            "COLUMN_MULTIPLICATION_TWO_DIGIT_BY_ONE",
            ["uae.g4.u04.l03.model_two_digit_product",
             "uae.g4.u04.l03.model_tens_product",
             "uae.g4.u04.l03.model_ones_product"],
            "UAE_G4_STUDENT_04_01_PRINTED_PP223_228_REUSE_EXISTING_EXACT_FAMILIES_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''+anchor)
p.write_text(s,encoding='utf-8')
print('PASS appended U04L05 after source-confirmed U04L04 and reused existing families')
