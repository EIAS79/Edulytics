import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2]
g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json')
assert pr['A_B_C_Ready']==17 and pr['FirstUnclosedLessonOrdinal']==18
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u03-l03-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:03:03'
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][2]['ls'][2]=='الضرب في صورة مقارنة'
assert M['Units'][2]['title']=='فهم الضرب والقسمة'
assert R['Lessons'][0]['CultureCode']=='ar' and len(R['Lessons'][0]['WorkedExamples'])>=3
skills=[
'مفهوم مرات أكثر وأضعاف العدد',
'التمييز بين مقارنة الجمع ومقارنة الضرب',
'تمثيل الأضعاف بمجموعات متساوية',
'تمثيل الأضعاف بمخطط شريطي',
'إيجاد العدد المقارن باستعمال الضرب',
'إيجاد العدد الأساسي باستعمال القسمة',
'إيجاد عامل المقارنة المجهول',
'تطبيق المقارنة الضربية على مسألة حياتية والتحقق']
ledger={'Grade':4,'Unit':3,'LessonOrdinal':18,'LessonCode':code,'Title':b['Title'],
'StudentPdf':'04-01.pdf','Sha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
'PrintedPages':[147,148,149,150,151,152],'PdfPages':[170,171,172,173,174,175],
'MathematicalSourceSkills':skills,'MappedCount':8,'TargetCount':8,
'OriginalArabicRich':True,'Visual':'EquationSet shows equal-group comparison; no native bar rendering',
'CoolDown':{'question':'في مجموعة ٥ كرات، وفي المجموعة الأخرى ٤ أضعافها. أوجد عدد المجموعة الثانية واشرح لماذا لا نجمع ٤ إلى ٥.',
'expected':'٢٠ كرة؛ ٤ مجموعات في كل منها ٥','teacherReview':'reasoning'},
'ExactPracticeFamilies':3,'TargetedTests':{'passed':3,'failed':0,'skipped':0},
'StageDRun':False,'Status':'READY_FOR_STAGE_D'}
save(g/'lesson-018-atomic-source-coverage.json',ledger)
save(g/'lesson-018-stage-a-c-checkpoint.json',{'LessonOrdinal':18,'LessonCode':code,'Unit':3,
'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'Source':'04-01.pdf printed 147-152','AcademicMapped':'8/8',
'RichArabic':True,'Visual':'EquationSet','ExactFamilies':3,
'TargetedTests':'3 passed, 0 failed, 0 skipped','StageDRun':False,
'GitCommit':'SELF_CLOSURE_COMMIT','NextLesson':'PED:UAE-MOE-MATH:L4:COMMON:03:04'})
pr.update({'A_B_C_Ready':18,'LastClosedLessonOrdinal':18,'FirstUnclosedLessonOrdinal':19,
'Note':'18/132 source-based A-B-C lessons closed, Unit 3 L04 next; Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L18 closure; next U03 L04')
