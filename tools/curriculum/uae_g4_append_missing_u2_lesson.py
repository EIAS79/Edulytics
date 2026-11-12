import json,pathlib,copy,sys
r=pathlib.Path(__file__).resolve().parents[2]
n=int(sys.argv[1]); assert 6<=n<=9
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
old=f'PED:UAE-MOE-MATH:L4:COMMON:02:{n-1:02d}'
code=f'PED:UAE-MOE-MATH:L4:COMMON:02:{n:02d}'
assert not any(x['LessonCode']==code for x in C['Lessons'])
assert not any(x['LessonCode']==code for x in B['Lessons'])
assert B['Units'][1]['LessonCount']==n-1 and len(M['Units'][1]['ls'])==n-1
c=copy.deepcopy(next(x for x in C['Lessons'] if x['LessonCode']==old))
c['LessonCode']=code
c['OfficialReferenceCode']=f'UAE:REF:TEXTBOOK:G4:COMMON:T1:U02:L{n:02d}'
c['Translations']=[dict(x,Title='PLACEHOLDER_NOT_PUBLISHED') for x in c['Translations']]
i=next(i for i,x in enumerate(C['Lessons']) if x['LessonCode']==old)
C['Lessons'].insert(i+1,c)
b=copy.deepcopy(next(x for x in B['Lessons'] if x['LessonCode']==old))
b['LessonCode']=code
b['SourceLessonCode']=f'EDU:UAE-MOE-MATH:L4:COMMON:U02:L{n:02d}'
b['LessonNumber']=n
b['Title']='PLACEHOLDER_NOT_PUBLISHED'
ordinal=6+n
b['SortOrder']=ordinal
b['OfficialReferenceCode']=f'UAE:REF:TEXTBOOK:G4:COMMON:T1:U02:L{n:02d}'
for x in b.get('Alignments',[]):x['ReferenceCode']=b['OfficialReferenceCode']
for x in B['Lessons']:
    if x['SortOrder']>=ordinal:x['SortOrder']+=1
i=next(i for i,x in enumerate(B['Lessons']) if x['LessonCode']==old)
B['Lessons'].insert(i+1,b)
B['Units'][1]['LessonCount']=n
M['Units'][1]['ls'].append('PLACEHOLDER_NOT_PUBLISHED')
save(cp,C);save(bp,B);save(mp,M)
print(f'PASS created missing student-based Grade 4 U02 L{n:02d} without deleting/rekeying prior lesson codes')
