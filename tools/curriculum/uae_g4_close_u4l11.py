import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2];g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json');assert pr['A_B_C_Ready']==33 and pr['FirstUnclosedLessonOrdinal']==34
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u04-l11-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:04:11'
b=next(x for x in B['Lessons'] if x['LessonCode']==code);l=next(x for x in C['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][3]['ls'][10]=='الضرب مع وجود الأصفار'
assert B['Units'][3]['LessonCount']==11
rich=R['Lessons'][0];assert len(rich['KeyConcepts'])>=3 and len(rich['CommonMistakes'])>=2 and len(rich['WorkedExamples'])>=3
skills=['تفسير وجود الأصفار بين الأرقام في العدد','ضرب ثلاثة أرقام في عامل واحد عند وجود صفر','ضرب أربعة أرقام في عامل واحد عند وجود صفر','ضرب الصفر وإضافة المحمول','التحقق من القيمة المكانية للنتائج','التحقق بخاصية التوزيع','تقدير ناتج الضرب للتأكد من معقوليته','حل مسائل حياتية عن مبالغ وأعداد مع أصفار']
save(g/'lesson-034-atomic-source-coverage.json',{'LessonOrdinal':34,'Unit':4,'LessonCode':code,'Title':b['Title'],'StudentPdf':'04-01.pdf',
'PrintedPages':[261,262,263,264,265,266],'PdfPages':[284,285,286,287,288,289],'Skills':skills,'MappedCount':8,'TargetCount':8,
'OriginalArabicRich':True,'ReusedExactFamilies':3,
'CoolDown':{'prompt':'أوجد ٣×٢٠٢٥ مع بيان دور الصفر في منزلة المئات.','expected':'٦٠٧٥؛ نحتفظ بمنزلة المئات ولا نحذف الصفر','teacherReview':'تفسير القيمة المكانية'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},'StageDRun':False,'Status':'READY_FOR_STAGE_D'})
save(g/'lesson-034-stage-a-c-checkpoint.json',{'LessonOrdinal':34,'LessonCode':code,'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'StudentSource':'04-01.pdf printed 261-266','AcademicMapped':'8/8','Tests':'3 passed, 0 failed, 0 skipped',
'StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT','NextSourceLesson':'Unit 5 first numbered lesson after unit 4 review pp267-270'})
pr.update({'A_B_C_Ready':34,'LastClosedLessonOrdinal':34,'ClosedUnitCount':4,'FirstUnclosedLessonOrdinal':35,'Note':'34 source-matched A-B-C lessons closed; Unit 4 has 11 numbered lessons; next Unit 5 after review. Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L34; Unit 4 numbered lessons finished')
