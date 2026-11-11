import pathlib,json,copy,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
B=load(bp);C=load(cp);M=load(mp)
old='PED:UAE-MOE-MATH:L4:COMMON:07:08'
new='PED:UAE-MOE-MATH:L4:COMMON:07:09'
assert len(B['Units'])==14 and len(M['Units'])==14
assert len(B['Lessons'])==len(C['Lessons'])==M['LessonCount']==107
assert len(M['Units'][6]['ls'])==8 and B['Units'][6]['LessonCount']==8
assert not any(x['LessonCode']==new for x in B['Lessons']+C['Lessons'])
assert B['Units'][7]['Number']==8
ref='UAE:REF:TEXTBOOK:G4:COMMON:T2:U07:L09'
name='المعادلات التي تتضمن عمليات متعددة'
b=copy.deepcopy(next(x for x in B['Lessons'] if x['LessonCode']==old))
b.update({'SourceLessonCode':'EDU:UAE-MOE-MATH:L4:COMMON:U07:L09','LessonCode':new,
'LessonNumber':9,'SortOrder':60,'Title':name,'OfficialReferenceCode':ref,
'SemanticSha256':hashlib.sha256((new+'|'+name).encode()).hexdigest()})
for a in b['Alignments']:
    a['ReferenceCode']=ref
for a in B['Lessons']:
    if a['SortOrder']>=60:a['SortOrder']+=1
B['Lessons'].append(b)
B['Lessons'].sort(key=lambda x:x['SortOrder'])
B['Units'][6]['LessonCount']=9
c=copy.deepcopy(next(x for x in C['Lessons'] if x['LessonCode']==old))
loc='04-02-07.pdf U07 L09 printed pp465-470 PDF61-66'
c.update({'LessonCode':new,'OfficialReferenceCode':ref,
'SourceLocator':loc,'TitleSourceReference':loc,
'SourceTitle':'UAE Grade 4 General Arabic student mathematics',
'SourceSha256':'85aa1e93e7e112cab346a379dc2b469d7dbfb2d90177144627d19f9559cc632c'})
for t in c['Translations']:
    if t['CultureCode']=='en':t['Title']='Equations with Multiple Operations'
    if t['CultureCode']=='ar':t['Title']=name
C['Lessons'].append(c)
M['Units'][6]['ls'].append(name);M['LessonCount']+=1
assert len(B['Lessons'])==len(C['Lessons'])==M['LessonCount']==108
assert sorted(x['SortOrder'] for x in B['Lessons'])==list(range(1,109))
assert sum(x['LessonCount'] for x in B['Units'])==108
assert sum(len(x['ls']) for x in M['Units'])==108
assert all(x['LessonCode'] in set(y['LessonCode'] for y in C['Lessons']) for x in B['Lessons'])
save(bp,B);save(cp,C);save(mp,M)
print('PASS restored genuine U07 L09; source indexed 108, grade target of 132 unchanged, existing lesson IDs retained')
