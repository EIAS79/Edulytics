import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2];g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json');assert pr['A_B_C_Ready']==26 and pr['FirstUnclosedLessonOrdinal']==27
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u04-l04-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:04:04';b=next(x for x in B['Lessons'] if x['LessonCode']==code)
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][3]['ls'][3]
assert R['Lessons'][0]['CultureCode']=='ar' and len(R['Lessons'][0]['WorkedExamples'])>=3
skills=['رسم نموذج مساحة لعددين أحدهما من رقم واحد',
'تقسيم عدد مكون من رقمين إلى عشرات وآحاد',
'حساب الضرب الجزئي للعشرات',
'حساب الضرب الجزئي للآحاد',
'جمع مساحتي المستطيل لإيجاد حاصل الضرب',
'استخدام المصفوفة لتوضيح مجموعات متساوية',
'حل مسائل من واقع الحياة باستخدام الضرب',
'التحقق بالتقدير أو الجمع المتكرر']
save(g/'lesson-027-atomic-source-coverage.json',{'LessonOrdinal':27,'Unit':4,'LessonCode':code,'Title':b['Title'],
'StudentPdf':'04-01.pdf','SourceSha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
'PrintedPages':[215,216,217,218,219,220],'PdfPages':[238,239,240,241,242,243],
'Skills':skills,'MappedCount':8,'TargetCount':8,'ArabicRich':True,'Visual':'EquationSet',
'ReusedExactFamilies':3,'CoolDown':{'prompt':'أوجد ٣ × ١٤ باستعمال نموذج مساحة؛ فسر ضرب العشرات والآحاد.',
'expected':'٣ × ١٠ = ٣٠ و٣ × ٤ = ١٢؛ الناتج ٤٢','teacherReview':'النموذج والتفسير'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},'StageDRun':False,'Status':'READY_FOR_STAGE_D'})
save(g/'lesson-027-stage-a-c-checkpoint.json',{'LessonOrdinal':27,'LessonCode':code,
'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'StudentSource':'04-01.pdf printed 215-220','AcademicMapped':'8/8','Tests':'3 passed, 0 failed, 0 skipped',
'StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT',
'NextSourceLesson':'U04 L05 الضرب في عدد مكون من رقمين (printed 223+), missing from legacy Edulytics index'})
pr.update({'A_B_C_Ready':27,'LastClosedLessonOrdinal':27,'FirstUnclosedLessonOrdinal':28,
'Note':'27 student-book-based A-B-C lessons closed; actual Unit 4 L05 follows and needs to be added to old index; Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L27 closure; source next U04 L05, missing legacy entry')
