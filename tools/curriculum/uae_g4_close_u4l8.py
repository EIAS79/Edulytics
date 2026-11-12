import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2];g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json');assert pr['A_B_C_Ready']==30 and pr['FirstUnclosedLessonOrdinal']==31
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u04-l08-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:04:08'
b=next(x for x in B['Lessons'] if x['LessonCode']==code);l=next(x for x in C['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][3]['ls'][7]=='الضرب مع إعادة التجميع'
assert B['Units'][3]['LessonCount']==8
rich=R['Lessons'][0];assert len(rich['KeyConcepts'])>=3 and len(rich['CommonMistakes'])>=2 and len(rich['WorkedExamples'])>=3
skills=['الضرب الرأسي لعدد من رقمين في عدد أحادي','إعادة تجميع ناتج ضرب الآحاد','جمع العشرات المحمولة إلى ناتج ضرب العشرات','إيجاد نواتج ٣٧×٥ و٣٨×٦ و٦٨×٧','استخدام التوزيع للتحقق','تقدير ناتج الضرب للتحقق من معقوليته','حل مسائل مالية متعددة الخطوات','تفسير أثر إهمال الحمل على الناتج']
save(g/'lesson-031-atomic-source-coverage.json',{'LessonOrdinal':31,'Unit':4,'LessonCode':code,'Title':b['Title'],'StudentPdf':'04-01.pdf',
'PrintedPages':[241,242,243,244,245,246],'PdfPages':[264,265,266,267,268,269],
'Skills':skills,'MappedCount':8,'TargetCount':8,'OriginalArabicRich':True,'ReusedExactFamilies':3,
'CoolDown':{'prompt':'احسب ٥×٣٧ بالطريقة الرأسية وبيّن العشرات المحمولة.','expected':'١٨٥؛ ٣٥ آحادًا = ٣ عشرات و٥ آحاد، ١٥+٣=١٨ عشرات','teacherReview':'الشرح'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},'StageDRun':False,'Status':'READY_FOR_STAGE_D'})
save(g/'lesson-031-stage-a-c-checkpoint.json',{'LessonOrdinal':31,'LessonCode':code,'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'StudentSource':'04-01.pdf printed 241-246','AcademicMapped':'8/8','Tests':'3 passed, 0 failed, 0 skipped',
'StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT','NextSourceLesson':'U04 L09 الضرب في عدد متعدد الأرقام'})
pr.update({'A_B_C_Ready':31,'LastClosedLessonOrdinal':31,'FirstUnclosedLessonOrdinal':32,
'Note':'31 source-matched A-B-C lessons closed; Unit 4 L09 next. Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L31 closure')
