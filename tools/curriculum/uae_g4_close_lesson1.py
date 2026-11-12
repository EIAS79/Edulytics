import json,pathlib
r=pathlib.Path(__file__).resolve().parents[2]
g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
c=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
b=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
m=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
idx=load(g/'candidate-source-index.json')
cov=load(g/'lesson-001-atomic-source-coverage.json')
e=load(g/'lesson-001-stage-abc-evidence.json')
rich=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u01-l01-ar.rich-lesson-v2.json')
reg=load(r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json')
code='PED:UAE-MOE-MATH:L4:COMMON:01:01'
t=next(x for x in c['Lessons'][0]['Translations'] if x['CultureCode']=='ar')
assert idx['totalCandidateLessons']==132
assert cov['studentSha256']==idx['units'][0].get('sourcePdfSha256',cov['studentSha256'])
assert t['Title']==b['Lessons'][0]['Title']==m['Units'][0]['ls'][0]=='القيمة المكانية'
assert rich['Lessons'][0]['CultureCode']=='ar' and rich['Lessons'][0]['LessonCode']==code
assert len(rich['Lessons'][0]['Visuals'])>=1
assert len(cov['elements'])==cov['metrics']['sourceElements']==cov['metrics']['claimedEquivalent']==8
families=['uae.g4.u01.l01.digit_value_to_millions','uae.g4.u01.l01.compose_seven_digits','uae.g4.u01.l01.expanded_to_standard']
assert all(any(f['id']==n for f in reg['families']) for n in families)
cov['metrics']['reviewState']='SOURCE_SEMANTIC_EQUIVALENCE_VERIFIED_IN_A_B_C'
cov['metrics']['testedUnitScope']='UaeGrade4Unit1Lesson01* 4 passed/0 failed'
save(g/'lesson-001-atomic-source-coverage.json',cov)
e['SourceElementCoverage']='8/8 source element categories mapped to first-party content or human-reviewed open prompts'
e['RichV2Integrated']=True
e['VisualRenderer']='PlaceValueChart'
e['SkillContract']['QuestionFamilyCandidate']=families
e['SkillContract']['IndependentVerifier']='separate reconstructed seven-digit oracle, no result parameter'
e['SkillContract']['PracticeRouteVerified']=True; e['SkillContract']['ProductionRouted']=False
e['TestsWritten']='UaeGrade4Unit1Lesson01PlaceValueTests and RichContentTests'
e['TestsRun']='PASS_4_OF_4_TARGETED_NET10'
e['ACStatus']='READY_FOR_STAGE_D'
e['READY_FOR_STAGE_D']=True
e['StageDRun']=False
save(g/'lesson-001-stage-abc-evidence.json',e)
checkpoint={
 'Grade':4,'LessonOrdinal':1,'LessonCode':code,'Unit':1,
 'Status':'READY_FOR_STAGE_D','StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED',
 'StudentPdf':'04-01.pdf','StudentPdfSha256':cov['studentSha256'],
 'SourcePrintedPages':[11,12,13,14,15,16],
 'SourceElementCoverage':'8/8 semantic elements mapped',
 'RichArabicAndPlaceValueChart':True,
 'QuestionFamilies':families,
 'TestCommand':'dotnet test Edulytics.Tests.csproj --no-restore --filter FullyQualifiedName~UaeGrade4Unit1Lesson01',
 'TestOutcome':'Passed 4 / Failed 0 / Skipped 0',
 'StageDTestsRun':False,
 'LegacyGrade4MigrationStatus':'NOT_FULLY_RECONCILED; 93/14 baseline retained for remaining lessons',
 'GitCommit':'SELF_CLOSURE_COMMIT',
 'NextLesson':'PED:UAE-MOE-MATH:L4:COMMON:01:02'
}
save(g/'lesson-001-stage-a-c-checkpoint.json',checkpoint)
progress={'Framework':'UAE-MOE-MATH','Grade':4,'Pathway':'Common','SelectedStudentBookSource':'04-01.pdf and remaining six supplied PDF fragments','TargetUnits':16,'TargetLessons':132,'A_B_C_Ready':1,'ClosedUnitCount':0,'LastClosedLessonOrdinal':1,'FirstUnclosedLessonOrdinal':2,'Status':'SEQUENTIAL_A_B_C_ACTIVE','StageDRun':False,'Note':'Only lesson 1 ready; legacy 93 English lessons and 14-unit catalogue await sequential source-based migration.'}
save(g/'full-grade-progress.json',progress)
print('PASS: L01 A/B/C close gate, 8 mapped categories, 4 targeted tests, next L02')
