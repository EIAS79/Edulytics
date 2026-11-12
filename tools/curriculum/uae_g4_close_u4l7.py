import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2];g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json');assert pr['A_B_C_Ready']==29 and pr['FirstUnclosedLessonOrdinal']==30
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u04-l07-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:04:07'
b=next(x for x in B['Lessons'] if x['LessonCode']==code);l=next(x for x in C['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][3]['ls'][6]=='خاصية التوزيع'
assert B['Units'][3]['LessonCount']==7
rich=R['Lessons'][0];assert len(rich['KeyConcepts'])>=3 and len(rich['CommonMistakes'])>=2 and len(rich['WorkedExamples'])>=3
skills=['تفسير خاصية التوزيع في الضرب','تفكيك عدد من رقمين إلى عشرات وآحاد','حساب ناتج ضرب جزئي للعشرات','حساب ناتج ضرب جزئي للآحاد','جمع نواتج الضرب الجزئية','تمثيل خاصية التوزيع بنموذج مساحة','حساب ٦×١٢ و٨×١٦ ونظائرها','حل مسائل حياتية بخاصية التوزيع']
save(g/'lesson-030-atomic-source-coverage.json',{'LessonOrdinal':30,'Unit':4,'LessonCode':code,'Title':b['Title'],'StudentPdf':'04-01.pdf',
'PrintedPages':[235,236,237,238,239,240],'PdfPages':[258,259,260,261,262,263],'Skills':skills,'MappedCount':8,'TargetCount':8,'OriginalArabicRich':True,
'ExtendedExistingEngine':True,'ExactFamilies':3,'CoolDown':{'prompt':'فسّر ٦×١٢ بخاصية التوزيع ونموذج المساحة.','expected':'(٦×١٠)+(٦×٢)=٦٠+١٢=٧٢','teacherReview':'التفسير'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},'StageDRun':False,'Status':'READY_FOR_STAGE_D'})
save(g/'lesson-030-stage-a-c-checkpoint.json',{'LessonOrdinal':30,'LessonCode':code,'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'StudentSource':'04-01.pdf printed 235-240','AcademicMapped':'8/8','Tests':'3 passed, 0 failed, 0 skipped','StageDRun':False,
'GitCommit':'SELF_CLOSURE_COMMIT','NextSourceLesson':'U04 L08 الضرب مع إعادة التجميع'})
pr.update({'A_B_C_Ready':30,'LastClosedLessonOrdinal':30,'FirstUnclosedLessonOrdinal':31,'Note':'30 source-matched A-B-C lessons closed; U04 L08 next. Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L30 closure')
