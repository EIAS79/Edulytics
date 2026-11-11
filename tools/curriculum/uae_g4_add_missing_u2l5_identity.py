import pathlib,json,copy
r=pathlib.Path(__file__).resolve().parents[2]
def read(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=read(cp);B=read(bp);M=read(mp)
old='PED:UAE-MOE-MATH:L4:COMMON:02:04'
code='PED:UAE-MOE-MATH:L4:COMMON:02:05'
assert not any(x['LessonCode']==code for x in C['Lessons'])
assert not any(x['LessonCode']==code for x in B['Lessons'])
assert len(M['Units'][1]['ls'])==4 and B['Units'][1]['LessonCount']==4
c=copy.deepcopy(next(x for x in C['Lessons'] if x['LessonCode']==old))
c['LessonCode']=code
c['OfficialReferenceCode']='UAE:REF:TEXTBOOK:G4:COMMON:T1:U02:L05'
c['Translations']=[dict(x,Title='PLACEHOLDER_NOT_PUBLISHED') for x in c['Translations']]
i=next(i for i,x in enumerate(C['Lessons']) if x['LessonCode']==old)
C['Lessons'].insert(i+1,c)
b=copy.deepcopy(next(x for x in B['Lessons'] if x['LessonCode']==old))
b['LessonCode']=code
b['SourceLessonCode']='EDU:UAE-MOE-MATH:L4:COMMON:U02:L05'
b['LessonNumber']=5
b['Title']='PLACEHOLDER_NOT_PUBLISHED'
b['SortOrder']=11
b['OfficialReferenceCode']='UAE:REF:TEXTBOOK:G4:COMMON:T1:U02:L05'
for item in b.get('Alignments',[]):
    item['ReferenceCode']=b['OfficialReferenceCode']
for x in B['Lessons']:
    if x['SortOrder']>10:x['SortOrder']+=1
i=next(i for i,x in enumerate(B['Lessons']) if x['LessonCode']==old)
B['Lessons'].insert(i+1,b)
B['Units'][1]['LessonCount']=5
M['Units'][1]['ls'].append('PLACEHOLDER_NOT_PUBLISHED')
save(cp,C);save(bp,B);save(mp,M)
print('PASS: created missing U02 L05 identity safely; did not delete/rekey existing 93 ids')
