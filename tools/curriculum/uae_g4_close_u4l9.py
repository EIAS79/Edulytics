import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2];g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json');assert pr['A_B_C_Ready']==31 and pr['FirstUnclosedLessonOrdinal']==32
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u04-l09-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:04:09'
b=next(x for x in B['Lessons'] if x['LessonCode']==code);l=next(x for x in C['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][3]['ls'][8]=='الضرب في عدد متعدد الأرقام'
assert B['Units'][3]['LessonCount']==9
rich=R['Lessons'][0];assert len(rich['KeyConcepts'])>=3 and len(rich['CommonMistakes'])>=2 and len(rich['WorkedExamples'])>=3
skills=['ضرب رقم في عدد من ثلاثة أرقام','ضرب رقم في عدد من أربعة أرقام',
'تحديد نواتج الضرب الجزئية للآحاد والعشرات والمئات والألوف',
'الضرب الرأسي مع إعادة التجميع المتتالية',
'تفسير ناتج ٩×٣٦٥ و٣×٢٧٦٣',
'تقدير ناتج الضرب للتحقق',
'تطبيق النواتج الجزئية على مسائل حياتية ومالية',
'التحقق بخاصية التوزيع']
save(g/'lesson-032-atomic-source-coverage.json',{'LessonOrdinal':32,'Unit':4,'LessonCode':code,'Title':b['Title'],'StudentPdf':'04-01.pdf',
'PrintedPages':[247,248,249,250,251,252],'PdfPages':[270,271,272,273,274,275],'Skills':skills,'MappedCount':8,'TargetCount':8,
'OriginalArabicRich':True,'ExactFamilies':3,'CoolDown':{'prompt':'احسب ٣×٢٧٦٣ باستخدام نواتج الضرب الجزئية.','expected':'٦٠٠٠+٢١٠٠+١٨٠+٩=٨٢٨٩','teacherReview':'تفسير القيم المكانية'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},'StageDRun':False,'Status':'READY_FOR_STAGE_D'})
save(g/'lesson-032-stage-a-c-checkpoint.json',{'LessonOrdinal':32,'LessonCode':code,'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'StudentSource':'04-01.pdf printed 247-252','AcademicMapped':'8/8','Tests':'3 passed, 0 failed, 0 skipped',
'StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT','NextSourceSection':'U04 L10 problem-solving exact-versus-estimated answer'})
pr.update({'A_B_C_Ready':32,'LastClosedLessonOrdinal':32,'FirstUnclosedLessonOrdinal':33,
'ClosedUnitCount':3,
'Note':'32 source-matched A-B-C lessons closed; next student-book U04 L10; Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS U04L09 closure; next U04L10')
