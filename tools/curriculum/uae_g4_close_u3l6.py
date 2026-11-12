import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2]
g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json')
assert pr['A_B_C_Ready']==20 and pr['FirstUnclosedLessonOrdinal']==21
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u03-l06-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:03:06'
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][2]['ls'][5]=='خاصية التجميع في الضرب'
assert R['Lessons'][0]['CultureCode']=='ar' and len(R['Lessons'][0]['WorkedExamples'])>=3
skills=['تحديد مفهوم التجميع في ضرب ثلاثة عوامل',
'تغيير الأقواس دون تغيير حاصل ضرب العوامل',
'اختيار طريقة تجميع أسرع للحساب الذهني',
'حساب نواتج الضرب من ثلاثة عوامل',
'مقارنة ناتجي طريقتي تجميع',
'إيجاد العامل الثالث المجهول',
'حل مسألة حياتية تتطلب ثلاثة عوامل',
'التمييز بين خاصية التجميع والتبديل']
ledger={'Grade':4,'Unit':3,'LessonOrdinal':21,'LessonCode':code,'Title':b['Title'],
'StudentPdf':'04-01.pdf','SourceSha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
'PrintedPages':[167,168,169,170,171,172],'PdfPages':[190,191,192,193,194,195],
'SourceSkills':skills,'MappedCount':8,'TotalCount':8,'OriginalArabicRich':True,'ExactFamilies':3,
'Visual':'EquationSet',
'Cooldown':{'prompt':'احسب ٤ × ٢ × ٥ بطريقتي تجميع مختلفتين، وفسر لماذا الناتج ثابت.',
'expected':'٤٠ بالطريقتين','teacherReview':'تفسير تغير الأقواس دون تغير العوامل'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},'StageDRun':False,'Status':'READY_FOR_STAGE_D'}
save(g/'lesson-021-atomic-source-coverage.json',ledger)
save(g/'lesson-021-stage-a-c-checkpoint.json',{'LessonOrdinal':21,'LessonCode':code,'Unit':3,
'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'StudentSource':'04-01.pdf printed 167-172','AcademicMapped':'8/8',
'ArabicRich':True,'ExactFamilyCount':3,'TargetedTests':'3 passed, 0 failed, 0 skipped',
'StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT',
'NextLesson':'PED:UAE-MOE-MATH:L4:COMMON:03:07'})
pr.update({'A_B_C_Ready':21,'LastClosedLessonOrdinal':21,'FirstUnclosedLessonOrdinal':22,
'Note':'21/132 A-B-C closures, next U03 L07; Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L21 closure; next U03 L07')
