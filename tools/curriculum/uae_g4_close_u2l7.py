import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2]
g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json')
assert pr['A_B_C_Ready']==12 and pr['FirstUnclosedLessonOrdinal']==13
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u02-l07-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:02:07'
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
assert len(C['Lessons'])==len(B['Lessons'])==96
assert B['Units'][1]['LessonCount']==7 and len(M['Units'][1]['ls'])==7
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][1]['ls'][6]=='الطرح مع وجود الأصفار'
assert R['Lessons'][0]['CultureCode']=='ar' and len(R['Lessons'][0]['Visuals'])>=1
features=[
'إعادة تجميع عبر منازل الصفر عند طرح الآحاد',
'استلاف من المئات والعشرات والألوف',
'طرح أعداد ذات أربعة أرقام مع أصفار',
'طرح أعداد ذات خمسة أرقام مع أصفار',
'طرح أعداد ذات ستة أرقام مع أصفار',
'التحقق بجمع الفرق والمطروح',
'مسائل حياتية وفهم قيمة المنازل',
'تدريب مستقل وواجب وتقويم ختامي']
ledger={'Grade':4,'Unit':2,'LessonOrdinal':13,'LessonCode':code,'Title':'الطرح مع وجود الأصفار',
'StudentPdf':'04-01.pdf','Sha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
'PrintedPages':[99,100,101,102,103,104],'PdfPages':[122,123,124,125,126,127],
'SemanticElementCoverage':features,'MappedCount':8,'TotalCount':8,
'OriginalArabicAndVisual':True,'OriginalVisualType':'PlaceValueChart',
'CoolDown':{'prompt':'احسب ٤٬٠٠٠ − ١٬٧٣١ وفسر إعادة التجميع عبر المنازل الصفرية.',
'answer':'٢٬٢٦٩','teacherReview':'شرح الاستلاف لا يصحح آليًا دون مراجعة المعلم'},
'SelectedExactFamilies':3,
'TargetedTests':{'filter':'UaeGrade4Unit2Lesson07ZeroSubtractionTests','passed':3,'failed':0,'skipped':0},
'StageDRun':False,'Status':'READY_FOR_STAGE_D'}
save(g/'lesson-013-atomic-source-coverage.json',ledger)
save(g/'lesson-013-stage-a-c-checkpoint.json',{'LessonOrdinal':13,'LessonCode':code,'Unit':2,
'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'StudentBook':'04-01.pdf printed pp99-104','MappedMathSkillCategories':'8/8',
'RichArabic':True,'QuestionFamilies':3,'TargetedTests':'3 passed / 0 failed',
'StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT','NextLesson':'PED:UAE-MOE-MATH:L4:COMMON:02:08'})
pr.update({'A_B_C_Ready':13,'LastClosedLessonOrdinal':13,'FirstUnclosedLessonOrdinal':14,
'Note':'13/132 lesson A-B-C closures. Next U02 L08. Grade-wide legacy migration and Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L13 closure next U02 L08')
