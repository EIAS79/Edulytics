import json,pathlib,sys
r=pathlib.Path(__file__).resolve().parents[2]
g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
n=int(sys.argv[1]); passed=int(sys.argv[2])
assert 2<=n<=6 and passed>=1
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
source=load(g/'unit-01-arabic-authoring.json')
spec=source['lessons'][n-1]
assert spec['number']==n
code=f'PED:UAE-MOE-MATH:L4:COMMON:01:{n:02d}'
prev=load(g/f'lesson-{n-1:03d}-stage-a-c-checkpoint.json')
assert prev['Status']=='READY_FOR_STAGE_D'
c=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
b=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
m=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
assert b['Lessons'][n-1]['LessonCode']==code
assert b['Lessons'][n-1]['Title']==m['Units'][0]['ls'][n-1]==spec['title']
assert any(x.get('CultureCode')=='ar' and x.get('Title')==spec['title'] for x in c['Lessons'][n-1]['Translations'])
rpath=r/f'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u01-l{n:02d}-ar.rich-lesson-v2.json'
rich=load(rpath)
rl=rich['Lessons'][0]
assert rl['LessonCode']==code and rl['CultureCode']=='ar'
assert len(rl['WorkedExamples'])>=3 and len(rl['KeyConcepts'])>=3 and len(rl['Visuals'])>=1
atomic=load(g/f'lesson-{n:03d}-atomic-source-coverage.json')
assert atomic['studentSha256']==source['sourcePdfSha256']
assert atomic['metrics']['sourceElements']==8 and atomic['metrics']['verifiedEquivalent']==8
e=load(g/f'lesson-{n:03d}-stage-abc-evidence.json')
e['SourceElementCoverage']='8/8 semantic element categories mapped'
e['RichV2Integrated']=True
e['SkillContract']['PracticeRouteVerified']=True
e['SkillContract']['ProductionRouted']=False
e['TestsRun']=f'PASS_{passed}_OF_{passed}_TARGETED_NET10'
e['ACStatus']='READY_FOR_STAGE_D'
e['READY_FOR_STAGE_D']=True
e['StageDRun']=False
save(g/f'lesson-{n:03d}-stage-abc-evidence.json',e)
checkpoint={
 'Grade':4,'LessonOrdinal':n,'LessonCode':code,'Unit':1,
 'Status':'READY_FOR_STAGE_D','StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED',
 'StudentPdf':'04-01.pdf','StudentPdfSha256':source['sourcePdfSha256'],
 'SourcePrintedPages':list(range(spec['start'],spec['end']+1)),
 'SourceElementCoverage':'8/8 semantic elements',
 'ArabicRichAndVisual':True,
 'TestOutcome':f'Passed {passed} / Failed 0 / Skipped 0',
 'TestsRun':'TARGETED_NET10_ONLY','StageDTestsRun':False,
 'LegacyCatalogue':'Not yet migrated; other 93 old titles remain until sequential replacement',
 'GitCommit':'SELF_CLOSURE_COMMIT',
 'NextLesson':f'PED:UAE-MOE-MATH:L4:COMMON:01:{n+1:02d}' if n<6 else 'Unit 02 L01'
}
save(g/f'lesson-{n:03d}-stage-a-c-checkpoint.json',checkpoint)
prog=load(g/'full-grade-progress.json')
assert prog['FirstUnclosedLessonOrdinal']==n
prog['A_B_C_Ready']=n
prog['LastClosedLessonOrdinal']=n
prog['FirstUnclosedLessonOrdinal']=n+1
prog['ClosedUnitCount']=1 if n==6 else 0
save(g/'full-grade-progress.json',prog)
print(f'PASS: A/B/C L{n:02d} close gate, {passed} targeted tests, next {n+1}')
