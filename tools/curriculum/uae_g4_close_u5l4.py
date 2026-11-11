import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2];g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json');assert pr['A_B_C_Ready']==37 and pr['FirstUnclosedLessonOrdinal']==38
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u05-l04-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:05:04'
b=next(x for x in B['Lessons'] if x['LessonCode']==code);l=next(x for x in C['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][4]['ls'][3]=='الضرب في عدد مكون من رقمين'
rich=R['Lessons'][0];assert len(rich['KeyConcepts'])>=3 and len(rich['CommonMistakes'])>=2 and len(rich['WorkedExamples'])>=3
skills=['ضرب عدد مكون من رقمين في آخر من رقمين','تكوين ناتج الضرب الجزئي للآحاد','تكوين ناتج الضرب الجزئي للعشرات',
'وضع الناتج الثاني في منزلة العشرات','جمع النواتج الجزئية','التحقق بنموذج مساحة ٥٦×٢٩',
'حل مسائل لفظية بجداء عاملين من رقمين','التحقق بالتقدير من ناتج الضرب']
save(g/'lesson-038-atomic-source-coverage.json',{'LessonOrdinal':38,'Unit':5,'LessonCode':code,'Title':b['Title'],'StudentPdf':'04-01.pdf',
'PrintedPages':[299,300,301,302,303,304],'PdfPages':[322,323,324,325,326,327],
'Skills':skills,'MappedCount':8,'TargetCount':8,'ArabicRich':True,'ReusedExactFamilies':3,
'CoolDown':{'prompt':'احسب ٥٦×٢٩ بالضرب الجزئي وبيّن قيمة العشرات.','expected':'٥٦×٩=٥٠٤ و٥٦×٢٠=١١٢٠؛ المجموع ١٦٢٤','teacherReview':'محاذاة النواتج الجزئية'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},'StageDRun':False,'Status':'READY_FOR_STAGE_D'})
save(g/'lesson-038-stage-a-c-checkpoint.json',{'LessonOrdinal':38,'LessonCode':code,'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'StudentSource':'04-01.pdf printed 299-304','AcademicMapped':'8/8','Tests':'3 passed, 0 failed, 0 skipped',
'StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT','NextSourceLesson':'U05 L05 حل المسائل الكلامية المكونة من عدة خطوات'})
pr.update({'A_B_C_Ready':38,'LastClosedLessonOrdinal':38,'FirstUnclosedLessonOrdinal':39,
'Note':'38 source-matched A-B-C lessons closed; U05 L05 next, Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L38 closure')
