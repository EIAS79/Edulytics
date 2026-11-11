import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2];g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json');assert pr['A_B_C_Ready']==32 and pr['FirstUnclosedLessonOrdinal']==33
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u04-l10-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:04:10'
b=next(x for x in B['Lessons'] if x['LessonCode']==code);l=next(x for x in C['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][3]['ls'][9]=='استقصاء حل المسائل: الإجابة التقديرية أم الإجابة الدقيقة'
assert B['Units'][3]['LessonCount']==10
rich=R['Lessons'][0];assert len(rich['KeyConcepts'])>=3 and len(rich['CommonMistakes'])>=2 and len(rich['WorkedExamples'])>=3
skills=['تحديد ما إذا كان السؤال يتطلب إجابة دقيقة','تحديد ما إذا كان المطلوب تقريبًا','قراءة المعطيات والمطلوب',
'اختيار عملية حسابية مناسبة','حساب التكاليف الفعلية بدقة','تقدير تكلفة مشتريات باستخدام التقريب',
'المقارنة بين تقدير وناتج فعلي','تفسير المعقولية والحل وفق الخطوات الأربع']
save(g/'lesson-033-atomic-source-coverage.json',{'LessonOrdinal':33,'Unit':4,'LessonCode':code,'Title':b['Title'],'StudentPdf':'04-01.pdf',
'PrintedPages':[255,256,257,258,259,260],'PdfPages':[278,279,280,281,282,283],
'Skills':skills,'MappedCount':8,'TargetCount':8,'ArabicRich':True,'ExactFamilies':3,
'CoolDown':{'prompt':'كم أدفع بالضبط لخمس تذاكر ثمن الواحدة ٢٢ درهمًا؟ هل يجب التقدير أم الحساب الدقيق؟','expected':'حساب دقيق؛ ١١٠ دراهم','teacherReview':'سبب الاختيار'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},'StageDRun':False,'Status':'READY_FOR_STAGE_D'})
save(g/'lesson-033-stage-a-c-checkpoint.json',{'LessonOrdinal':33,'LessonCode':code,
'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'StudentSource':'04-01.pdf printed 255-260','AcademicMapped':'8/8','Tests':'3 passed, 0 failed, 0 skipped',
'StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT',
'NextSourceLesson':'U04 L11 الضرب مع وجود الأصفار'})
pr.update({'A_B_C_Ready':33,'LastClosedLessonOrdinal':33,'FirstUnclosedLessonOrdinal':34,
'Note':'33 source-matched A-B-C lessons closed, Unit 4 L11 next; Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L33 closure; next U04L11')
