import pathlib,json,copy
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
prev='PED:UAE-MOE-MATH:L4:COMMON:06:08';code='PED:UAE-MOE-MATH:L4:COMMON:06:09'
assert B['Units'][5]['LessonCount']==8 and not any(x['LessonCode']==code for x in B['Lessons'])
title='قسمة الأعداد الكبيرة'
c=copy.deepcopy(next(x for x in C['Lessons'] if x['LessonCode']==prev))
c.update({'LessonCode':code,'OfficialReferenceCode':'UAE:REF:TEXTBOOK:G4:COMMON:T1:U06:L09','SourceLocator':'04-01.pdf U06 L09 printed pp379-384 PDF402-407',
'TitleSourceReference':'04-01.pdf U06 L09 printed pp379-384 PDF402-407','AdaptationStatus':'PENDING_VERIFICATION'})
i=next(i for i,x in enumerate(C['Lessons']) if x['LessonCode']==prev);C['Lessons'].insert(i+1,c)
b=copy.deepcopy(next(x for x in B['Lessons'] if x['LessonCode']==prev))
b.update({'LessonCode':code,'SourceLessonCode':'EDU:UAE-MOE-MATH:L4:COMMON:U06:L09','LessonNumber':9,'Title':title,'SortOrder':49,
'OfficialReferenceCode':'UAE:REF:TEXTBOOK:G4:COMMON:T1:U06:L09'})
for x in b.get('Alignments',[]):x['ReferenceCode']=b['OfficialReferenceCode']
for x in B['Lessons']:
    if x['SortOrder']>=49:x['SortOrder']+=1
i=next(i for i,x in enumerate(B['Lessons']) if x['LessonCode']==prev);B['Lessons'].insert(i+1,b)
B['Units'][5]['LessonCount']=9;M['Units'][5]['ls'].append(title)
M['LessonCount']=sum(len(x['ls']) for x in M['Units'])
save(cp,C);save(bp,B);save(mp,M)
print('PASS U06 L09 added to both packs and shifted subsequent sort orders, lesson count',M['LessonCount'])
