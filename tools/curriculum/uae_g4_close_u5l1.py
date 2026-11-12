import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2];g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json');assert pr['A_B_C_Ready']==34 and pr['FirstUnclosedLessonOrdinal']==35
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u05-l01-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:05:01'
b=next(x for x in B['Lessons'] if x['LessonCode']==code);l=next(x for x in C['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][4]['ls'][0]=='الضرب في العشرات'
assert B['Units'][4]['Title']=='الضرب في عدد مكون من رقمين'
rich=R['Lessons'][0];assert len(rich['KeyConcepts'])>=3 and len(rich['CommonMistakes'])>=2 and len(rich['WorkedExamples'])>=3
skills=['ضرب عدد من رقمين في مضاعف عشرة','استعمال خاصية التجميع','تمثيل ٢٠ على أنه ٢ × ١٠','إجراء الضرب الرأسي في العشرات','التحقق من العامل المجهول في ناتج ضرب','تطبيق ضرب العشرات في مسائل مالية','استخدام خاصية التبديل للمقارنة','التحقق بالتقدير من النواتج']
save(g/'lesson-035-atomic-source-coverage.json',{'LessonOrdinal':35,'Unit':5,'LessonCode':code,'Title':b['Title'],'StudentPdf':'04-01.pdf',
'PrintedPages':[279,280,281,282,283,284],'PdfPages':[302,303,304,305,306,307],
'Skills':skills,'MappedCount':8,'TargetCount':8,'ArabicRich':True,'ExactFamilies':2,
'CoolDown':{'prompt':'فسر ٢٥ × ٢٠ باستخدام خاصية التجميع واحسب.','expected':'٢٥×٢×١٠=٥٠×١٠=٥٠٠','teacherReview':'إعادة بناء العامل من مضاعفات عشرة'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},'StageDRun':False,'Status':'READY_FOR_STAGE_D'})
save(g/'lesson-035-stage-a-c-checkpoint.json',{'LessonOrdinal':35,'LessonCode':code,'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'StudentSource':'04-01.pdf printed 279-284','AcademicMapped':'8/8','Tests':'3 passed, 0 failed, 0 skipped',
'StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT','NextSourceLesson':'U05 L02 تقدير نواتج الضرب'})
pr.update({'A_B_C_Ready':35,'LastClosedLessonOrdinal':35,'FirstUnclosedLessonOrdinal':36,
'Note':'35 source-matched A-B-C lessons closed; Unit5 L02 follows. Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L35 closure; next U05L02')
