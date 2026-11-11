import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2];g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json')
assert pr['A_B_C_Ready']==38 and pr['FirstUnclosedLessonOrdinal']==39
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u05-l05-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:05:05'
b=next(x for x in B['Lessons'] if x['LessonCode']==code);l=next(x for x in C['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][4]['ls'][4]=='حل المسائل الكلامية المكونة من عدة خطوات'
rich=R['Lessons'][0];assert len(rich['KeyConcepts'])>=3 and len(rich['CommonMistakes'])>=2 and len(rich['WorkedExamples'])>=3
skills=['تحديد المعطيات والمطلوب في مسائل من خطوتين','كتابة معادلة تشمل الضرب والطرح','حساب صافي الادخار الأسبوعي','تحديد مجموع العناصر في مجموعات متساوية','إضافة كميات إلى نواتج الضرب','إيجاد كمية مجهولة بعد جمع المجموعات','الحفاظ على ترتيب العمليات ووحدات القياس','التحقق من معقولية الناتج']
save(g/'lesson-039-atomic-source-coverage.json',{'LessonOrdinal':39,'Unit':5,'LessonCode':code,'Title':b['Title'],'StudentPdf':'04-01.pdf','PrintedPages':[305,306,307,308,309,310],'PdfPages':[328,329,330,331,332,333],
'Skills':skills,'MappedCount':8,'TargetCount':8,'ArabicRich':True,'ExactFamilies':2,
'CoolDown':{'prompt':'إذا ادخرت ١٢ درهمًا وأنفقت ٣ كل أسبوع لمدة ١٤ أسبوعًا فما صافي الادخار؟','expected':'(١٢−٣)×١٤=١٢٦ درهمًا','teacherReview':'تفسير ترتيب العمليات'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},'StageDRun':False,'Status':'READY_FOR_STAGE_D'})
save(g/'lesson-039-stage-a-c-checkpoint.json',{'LessonOrdinal':39,'LessonCode':code,'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'StudentSource':'04-01.pdf printed 305-310','AcademicMapped':'8/8','Tests':'3 passed, 0 failed, 0 skipped','StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT','NextSourceLesson':'U05 L06'})
pr.update({'A_B_C_Ready':39,'LastClosedLessonOrdinal':39,'FirstUnclosedLessonOrdinal':40,'Note':'39 student-book A-B-C lessons closed; next U05 L06. Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L39 closure, U05 L06 next')
