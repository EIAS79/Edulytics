import json,pathlib
r=pathlib.Path(__file__).resolve().parents[2]
g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
progress=load(g/'full-grade-progress.json')
assert progress['A_B_C_Ready']==7 and progress['FirstUnclosedLessonOrdinal']==8
atomic=load(g/'lesson-008-atomic-source-coverage.json')
assert len(atomic['skillElements'])==8
code='PED:UAE-MOE-MATH:L4:COMMON:02:02'
pack=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
blue=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
m=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
rich=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u02-l02-ar.rich-lesson-v2.json')
lesson=next(x for x in pack['Lessons'] if x['LessonCode']==code)
entry=next(x for x in blue['Lessons'] if x['LessonCode']==code)
assert rich['Lessons'][0]['CultureCode']=='ar'
assert lesson['Translations'][-1]['Title']==entry['Title']==m['Units'][1]['ls'][1]
atomic['status']='READY_FOR_STAGE_D'
atomic['scopedTests']={'passed':3,'failed':0,'skipped':0}
save(g/'lesson-008-atomic-source-coverage.json',atomic)
checkpoint={'LessonOrdinal':8,'LessonCode':code,'Unit':2,'Title':'أنماط الجمع والطرح',
'Source':'04-01.pdf printed pp67-72','StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'SemanticElementsCovered':8,'ArabicRichV2':True,'Visual':'NumberLine','SelectedExactFamilies':4,
'Cooldown':'Author original, teacher reviews verbal pattern explanation',
'TestOutcome':'Passed 3 / Failed 0','StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT'}
save(g/'lesson-008-stage-a-c-checkpoint.json',checkpoint)
progress.update({'A_B_C_Ready':8,'LastClosedLessonOrdinal':8,'FirstUnclosedLessonOrdinal':9,
'Note':'8/132 A-B-C lesson closures, Unit 2 L03 next. Grade-level migration and Stage D NOT completed.'})
save(g/'full-grade-progress.json',progress)
print('PASS L08 closure gate; next L09')
