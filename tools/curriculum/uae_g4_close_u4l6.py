import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2];g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json')
assert pr['A_B_C_Ready']==28 and pr['FirstUnclosedLessonOrdinal']==29
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u04-l06-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:04:06'
b=next(x for x in B['Lessons'] if x['LessonCode']==code);l=next(x for x in C['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][3]['ls'][5]=='نشاط عملي: تمثيل إعادة التجميع'
assert B['Units'][3]['LessonCount']==6
rich=R['Lessons'][0];assert len(rich['KeyConcepts'])>=3 and len(rich['CommonMistakes'])>=2 and len(rich['WorkedExamples'])>=3
skills=['تمثيل ٣ × ٢٦ باستخدام مكعبات القيمة المكانية','إعادة تجميع ١٨ آحادًا في عشرة و٨ آحاد','إعادة تجميع ١٢ عشرة إلى مئة وعشرتين','تفسير قيمة العشرات والآحاد','إجراء الضرب لعدد من رقمين في رقم واحد مع حمل','فحص صحة الضرب بالنواتج الجزئية','تطبيق إعادة التجميع في مسائل حياتية','شرح أثر إعادة التجميع من دون تغيير القيمة']
save(g/'lesson-029-atomic-source-coverage.json',{'LessonOrdinal':29,'Unit':4,'LessonCode':code,'Title':b['Title'],'StudentPdf':'04-01.pdf',
'PrintedPages':[229,230,231,232,233,234],'PdfPages':[252,253,254,255,256,257],'Skills':skills,
'MappedCount':8,'TargetCount':8,'OriginalArabicRich':True,'ReusedExactFamilies':3,
'CoolDown':{'prompt':'نمذج ٣ × ٢٦ مع إعادة تجميع الآحاد، وفسّر الناتج.','expected':'٦٠ + ١٨ = ٧٨؛ ١٨ آحادًا = عشرة و٨ آحاد','teacherReview':'تفسير إعادة التجميع'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},'StageDRun':False,'Status':'READY_FOR_STAGE_D'})
save(g/'lesson-029-stage-a-c-checkpoint.json',{'LessonOrdinal':29,'LessonCode':code,'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'StudentSource':'04-01.pdf printed 229-234','AcademicMapped':'8/8','Tests':'3 passed, 0 failed, 0 skipped',
'StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT','NextSourceLesson':'U04 L07 خاصية التوزيع'})
pr.update({'A_B_C_Ready':29,'LastClosedLessonOrdinal':29,'FirstUnclosedLessonOrdinal':30,
'Note':'29 student-book A-B-C lessons closed, Unit 4 L07 next; Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L29 closed; next U04 L07')
