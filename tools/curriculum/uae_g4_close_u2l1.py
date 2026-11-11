import json,pathlib
r=pathlib.Path(__file__).resolve().parents[2]
g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
p=g/'full-grade-progress.json'
j=load(p)
assert j['A_B_C_Ready']==6 and j['FirstUnclosedLessonOrdinal']==7
c=load(g/'lesson-007-atomic-source-coverage.json')
assert len(c['coverage'])==8 and c['tests']['passed']==3
pack=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
blue=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
manifest=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
rich=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u02-l01-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:02:01'
l=next(x for x in pack['Lessons'] if x['LessonCode']==code)
b=next(x for x in blue['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==manifest['Units'][1]['ls'][0]
assert rich['Lessons'][0]['Title']==b['Title']
assert rich['Lessons'][0]['CultureCode']=='ar'
e={'LessonOrdinal':7,'LessonCode':code,'Unit':2,'Title':b['Title'],
'StudentSource':'04-01.pdf, printed pp61-66, SHA256 859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'SourceSkillsCovered':8,'SourceSkillsTotal':8,'Scope':'Skill and outcome equivalence, not reproduction of source text',
'PracticeFamilyCount':5,'RichArabic':True,'OriginalVisual':'CalculationChain',
'Cooldown':'Author-written from lesson goal; teacher reviews reasoning',
'TestOutcome':'3/3 targeted NET10 passed','StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT'}
save(g/'lesson-007-stage-a-c-checkpoint.json',e)
j.update({'A_B_C_Ready':7,'LastClosedLessonOrdinal':7,'FirstUnclosedLessonOrdinal':8,
'ClosedUnitCount':1,'Note':'7/132 lesson-scoped A-B-C closure records; Unit 2 next L02; legacy 93/14 not fully migrated or deployed.'})
save(p,j)
print('PASS: L07 closure 8 source skill categories, tests 3/3, next L08')
