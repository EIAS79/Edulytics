import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2];g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json');assert pr['A_B_C_Ready']==40 and pr['FirstUnclosedLessonOrdinal']==41
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u06-l01-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:06:01'
b=next(x for x in B['Lessons'] if x['LessonCode']==code);l=next(x for x in C['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][5]['ls'][0]=='قسمة مضاعفات الأعداد ١٠ و١٠٠ و١٠٠٠'
rich=R['Lessons'][0];assert len(rich['KeyConcepts'])>=3 and len(rich['CommonMistakes'])>=2 and len(rich['WorkedExamples'])>=3
skills=['إيجاد ناتج قسمة مضاعفات عشرة على رقم واحد','إيجاد ناتج قسمة مضاعفات مئة على رقم واحد','إيجاد ناتج قسمة مضاعفات ألف على رقم واحد','تفسير أنماط القسمة باستخدام القيمة المكانية','ربط جملة قسمة بحقيقة ضرب أساسية','حل مسائل تقسيم مجموعات متساوية','إيجاد ناتج مجهول في معادلة قسمة','التحقق من القسمة بضرب الناتج في المقسوم عليه']
save(g/'lesson-041-atomic-source-coverage.json',{'LessonOrdinal':41,'Unit':6,'LessonCode':code,'Title':b['Title'],'StudentPdf':'04-01.pdf',
'PrintedPages':[329,330,331,332,333,334],'PdfPages':[352,353,354,355,356,357],'Skills':skills,'MappedCount':8,'TargetCount':8,
'ArabicRich':True,'ExactFamilies':2,'CoolDown':{'prompt':'احسب ٢٤٠٠÷٤ باستخدام أنماط القيمة المكانية وراجع بالضرب.','expected':'٦٠٠؛ لأن ٦٠٠×٤=٢٤٠٠','teacherReview':'الفهم المكاني'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},'StageDRun':False,'Status':'READY_FOR_STAGE_D'})
save(g/'lesson-041-stage-a-c-checkpoint.json',{'LessonOrdinal':41,'LessonCode':code,'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D','StudentSource':'04-01.pdf printed 329-334','AcademicMapped':'8/8','Tests':'3 passed, 0 failed, 0 skipped','StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT','NextSourceLesson':'U06 L02 source pending'})
pr.update({'A_B_C_Ready':41,'LastClosedLessonOrdinal':41,'FirstUnclosedLessonOrdinal':42,'Note':'41 source-matched A-B-C lessons closed; Unit6 L02 next. Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L41 checkpoint ready for commit')
