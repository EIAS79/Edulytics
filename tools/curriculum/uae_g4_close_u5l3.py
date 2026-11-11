import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2];g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json');assert pr['A_B_C_Ready']==36 and pr['FirstUnclosedLessonOrdinal']==37
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u05-l03-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:05:03'
b=next(x for x in B['Lessons'] if x['LessonCode']==code);l=next(x for x in C['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][4]['ls'][2]=='نشاط عملي: استخدام خاصية التوزيع في الضرب'
rich=R['Lessons'][0];assert len(rich['KeyConcepts'])>=3 and len(rich['CommonMistakes'])>=2 and len(rich['WorkedExamples'])>=3
skills=['رسم نموذج مساحة بضلعين من رقمين','تفكيك أحد العاملين إلى عشرات وآحاد','تحديد ناتج ضرب الجزء الأول من المساحة',
'تحديد ناتج ضرب جزء الآحاد من المساحة','جمع النواتج الجزئية لحساب المساحة الكلية',
'التطبيق على ١٢×١٥ و٣٦×٢٤','استخدام خاصية التوزيع في مسائل حياتية','التحقق من خطأ نموذج غير مكتمل']
save(g/'lesson-037-atomic-source-coverage.json',{'LessonOrdinal':37,'Unit':5,'LessonCode':code,'Title':b['Title'],'StudentPdf':'04-01.pdf',
'PrintedPages':[293,294,295,296,297,298],'PdfPages':[316,317,318,319,320,321],
'Skills':skills,'MappedCount':8,'TargetCount':8,'ArabicRich':True,'ExtendedExistingEngine':True,'ExactFamilies':3,
'CoolDown':{'prompt':'استخدم نموذج مساحة لشرح ١٢×١٥ وبيّن الجزأين.','expected':'١٢×١٠=١٢٠ و١٢×٥=٦٠؛ المجموع ١٨٠','teacherReview':'تمثيل المساحة'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},'StageDRun':False,'Status':'READY_FOR_STAGE_D'})
save(g/'lesson-037-stage-a-c-checkpoint.json',{'LessonOrdinal':37,'LessonCode':code,'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'StudentSource':'04-01.pdf printed 293-298','AcademicMapped':'8/8','Tests':'3 passed, 0 failed, 0 skipped',
'StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT','NextSourceLesson':'U05 L04 source pending'})
pr.update({'A_B_C_Ready':37,'LastClosedLessonOrdinal':37,'FirstUnclosedLessonOrdinal':38,
'Note':'37 source-matched A-B-C lessons closed; next U05 L04. Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L37 closure')
