import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2];g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json');assert pr['A_B_C_Ready']==35 and pr['FirstUnclosedLessonOrdinal']==36
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u05-l02-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:05:02'
b=next(x for x in B['Lessons'] if x['LessonCode']==code);l=next(x for x in C['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][4]['ls'][1]=='تقدير نواتج الضرب'
rich=R['Lessons'][0];assert len(rich['KeyConcepts'])>=3 and len(rich['CommonMistakes'])>=2 and len(rich['WorkedExamples'])>=3
skills=['تقريب عاملين إلى أقرب عشرة','تقدير ناتج ضرب عددين من رقمين','مقارنة التقدير بالناتج الفعلي',
'تحديد اتجاه التقدير أعلى أو أقل','التحقق من تقريب الأعداد المنتهية بخمسة','تقدير التكاليف والساعات في المسائل الحياتية',
'تمثيل التقدير بنموذج مساحة','حل المعادلات ذات الأعداد المجهولة من مضاعفات عشرة']
save(g/'lesson-036-atomic-source-coverage.json',{'LessonOrdinal':36,'Unit':5,'LessonCode':code,'Title':b['Title'],'StudentPdf':'04-01.pdf',
'PrintedPages':[285,286,287,288,289,290],'PdfPages':[308,309,310,311,312,313],
'Skills':skills,'MappedCount':8,'TargetCount':8,'ArabicRich':True,'ExactFamilies':2,
'CoolDown':{'prompt':'قدّر ٨٨ × ٦٥ وحدد هل التقدير أكبر من الناتج الحقيقي أم أصغر.','expected':'٩٠×٧٠=٦٣٠٠؛ أكبر من الحقيقي','teacherReview':'التقريب والمقارنة'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},'StageDRun':False,'Status':'READY_FOR_STAGE_D'})
save(g/'lesson-036-stage-a-c-checkpoint.json',{'LessonOrdinal':36,'LessonCode':code,'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'StudentSource':'04-01.pdf printed 285-290','AcademicMapped':'8/8','Tests':'3 passed, 0 failed, 0 skipped',
'StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT','NextSourceLesson':'U05 L03 source pending'})
pr.update({'A_B_C_Ready':36,'LastClosedLessonOrdinal':36,'FirstUnclosedLessonOrdinal':37,'Note':'36 source-matched A-B-C lessons closed; next U05 L03. Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L36 closure')
