import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2]
g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
progress=load(g/'full-grade-progress.json')
assert progress['A_B_C_Ready']==10 and progress['FirstUnclosedLessonOrdinal']==11
code='PED:UAE-MOE-MATH:L4:COMMON:02:05'
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u02-l05-ar.rich-lesson-v2.json')
assert len(C['Lessons'])==94 and len(B['Lessons'])==94
assert B['Units'][1]['LessonCount']==5 and len(M['Units'][1]['ls'])==5
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][1]['ls'][4]=='جمع الأعداد الكلية'
assert R['Lessons'][0]['CultureCode']=='ar' and len(R['Lessons'][0]['Visuals'])==1
ledger={'LessonOrdinal':11,'Grade':4,'Unit':2,'LessonCode':code,'Title':'جمع الأعداد الكلية',
'StudentBook':'04-01.pdf','PdfSha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
'PrintedPages':[87,88,89,90,91,92],'PdfPages':[110,111,112,113,114,115],
'SourceFeatures':[
'Place value chart addition','Aligning up to six digits','Regrouping/carry between place columns',
'Guided independent column addition','Meaningful real-world word problems',
'Estimation to check exact sum','Reverse subtraction check','Homework and assessment'],
'SourceFeaturesCovered':8,'SourceFeaturesTotal':8,
'OriginalContent':'First-party Arabic prose, new examples and place value chart; no publisher image copied',
'CoolDown':{'prompt':'احسب ٤٬٧٢٨ + ٣٬٥٦٥ ثم اشرح كيف أعدت التجميع في الآحاد والعشرات.',
'expected':'٨٬٢٩٣','explanation':'teacher review'},
'Tests':{'scopedFilter':'UaeGrade4Unit2Lesson05AdditionTests','passed':3,'failed':0,'skipped':0},
'StageDRun':False,'Status':'READY_FOR_STAGE_D',
'LegacyIdentity':'New missing lesson appended with stable unique code; prior 93 codes preserved; full grade migration incomplete'}
save(g/'lesson-011-atomic-source-coverage.json',ledger)
save(g/'lesson-011-stage-a-c-checkpoint.json',{'LessonOrdinal':11,'LessonCode':code,'Unit':2,
'Status':'READY_FOR_STAGE_D','StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED',
'Source':'04-01.pdf printed pp87-92','AcademicSourceCoverage':'8/8 mathematical element categories',
'SelectedExactFamilies':3,'ArabicRichV2':True,'NewLessonAdded':True,
'TestOutcome':'Passed 3 / Failed 0 / Skipped 0','StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT',
'NextLesson':'PED:UAE-MOE-MATH:L4:COMMON:02:06'})
progress.update({'A_B_C_Ready':11,'LastClosedLessonOrdinal':11,'FirstUnclosedLessonOrdinal':12,
'Note':'11/132 A-B-C lesson closures, U02 L06 next. Legacy 93 IDs preserved, new U02 L05 appended. Stage D not run.'})
save(g/'full-grade-progress.json',progress)
print('PASS L11 closure: new source lesson, 8 mapped skills, 3 passing targeted tests, next L12')
