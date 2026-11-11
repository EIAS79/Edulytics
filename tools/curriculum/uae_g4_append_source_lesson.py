import sys,pathlib,json,copy
r=pathlib.Path(__file__).resolve().parents[2]
u=int(sys.argv[1]);n=int(sys.argv[2]);title=sys.argv[3];order=int(sys.argv[4])
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(cp);B=load(bp);M=load(mp)
prev=f'PED:UAE-MOE-MATH:L4:COMMON:{u:02}:{n-1:02}'
code=f'PED:UAE-MOE-MATH:L4:COMMON:{u:02}:{n:02}'
assert not any(x['LessonCode']==code for x in B['Lessons'])
assert B['Units'][u-1]['LessonCount']==n-1
c=copy.deepcopy(next(x for x in C['Lessons'] if x['LessonCode']==prev))
c.update({'LessonCode':code,'OfficialReferenceCode':f'UAE:REF:TEXTBOOK:G4:COMMON:T1:U{u:02}:L{n:02}','AdaptationStatus':'PENDING_SOURCE_VERIFICATION'})
i=next(i for i,x in enumerate(C['Lessons']) if x['LessonCode']==prev);C['Lessons'].insert(i+1,c)
b=copy.deepcopy(next(x for x in B['Lessons'] if x['LessonCode']==prev))
b.update({'LessonCode':code,'SourceLessonCode':f'EDU:UAE-MOE-MATH:L4:COMMON:U{u:02}:L{n:02}','LessonNumber':n,'Title':title,'SortOrder':order,'OfficialReferenceCode':c['OfficialReferenceCode']})
for x in b.get('Alignments',[]):x['ReferenceCode']=b['OfficialReferenceCode']
for x in B['Lessons']:
    if x['SortOrder']>=order:x['SortOrder']+=1
i=next(i for i,x in enumerate(B['Lessons']) if x['LessonCode']==prev);B['Lessons'].insert(i+1,b)
B['Units'][u-1]['LessonCount']=n;M['Units'][u-1]['ls'].append(title);M['LessonCount']=sum(len(x['ls']) for x in M['Units'])
save(cp,C);save(bp,B);save(mp,M)
print('PASS ADDED',code,'order',order,'manifest',M['LessonCount'])
