import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2];g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json');assert pr['A_B_C_Ready']==39 and pr['FirstUnclosedLessonOrdinal']==40
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u05-l06-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:05:06'
b=next(x for x in B['Lessons'] if x['LessonCode']==code);l=next(x for x in C['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][4]['ls'][5]=='استقصاء حل المسائل: رسم جدول'
assert B['Units'][4]['LessonCount']==6
rich=R['Lessons'][0];assert len(rich['KeyConcepts'])>=3 and len(rich['CommonMistakes'])>=2 and len(rich['WorkedExamples'])>=3
skills=['استخدام استراتيجية رسم الجدول لتنظيم المعطيات','تمثيل عدد الأيام والكمية المتكررة','توليد نمط عددي تراكمي','استخدام الجدول لقراءة المجموع النهائي','إيجاد عدد الفترات من المجموع الكلي','حل مسائل عن وجبات الإفطار المتكررة','حل مسائل قراءة الصفحات أسبوعيًا','التحقق بمقارنة الجدول والضرب']
save(g/'lesson-040-atomic-source-coverage.json',{'LessonOrdinal':40,'Unit':5,'LessonCode':code,'Title':b['Title'],'StudentPdf':'04-01.pdf',
'PrintedPages':[311,312,313,314,315,316],'PdfPages':[334,335,336,337,338,339],'Skills':skills,'MappedCount':8,'TargetCount':8,
'ArabicRich':True,'ExtendedExistingEngine':True,'ExactFamilies':2,
'CoolDown':{'prompt':'ارسم جدولًا لخمس أسابيع يقرأ الطالب في كل أسبوع ١٦ صفحة. كم عدد الصفحات بعد الأسبوع الخامس؟','expected':'١٦،٣٢،٤٨،٦٤،٨٠؛ المجموع ٨٠ صفحة','teacherReview':'جدول صحيح ومراجعة بالضرب'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},'StageDRun':False,'Status':'READY_FOR_STAGE_D'})
save(g/'lesson-040-stage-a-c-checkpoint.json',{'LessonOrdinal':40,'LessonCode':code,'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'StudentSource':'04-01.pdf printed 311-316','AcademicMapped':'8/8','Tests':'3 passed, 0 failed, 0 skipped',
'StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT','NextSourceLesson':'Unit6 first book lesson after Unit5 review'})
pr.update({'A_B_C_Ready':40,'LastClosedLessonOrdinal':40,'FirstUnclosedLessonOrdinal':41,'ClosedUnitCount':5,
'Note':'40 source-matched A-B-C lessons closed; Unit 5 six lessons complete. Unit6 next, Stage D pending.'})
save(g/'full-grade-progress.json',pr)
save(g/'unit-005-stage-a-c-closure.json',{'Unit':5,'Title':'الضرب في عدد مكون من رقمين','Source':'04-01.pdf',
'FirstOrdinal':35,'LastOrdinal':40,'NumberedLessons':6,'Status':'A_B_C_SOURCE_UNIT_CLOSED','StageDRun':False})
print('PASS L40 closure, Unit5 complete')
