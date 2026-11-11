import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2];g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def read(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=read(g/'full-grade-progress.json')
assert pr['A_B_C_Ready']==23 and pr['FirstUnclosedLessonOrdinal']==24 and pr['ClosedUnitCount']==3
C=read(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=read(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=read(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=read(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u04-l01-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:04:01'
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][3]['ls'][0]
assert R['Lessons'][0]['CultureCode']=='ar' and len(R['Lessons'][0]['WorkedExamples'])>=3
skills=['ضرب عدد أحادي الرقم في مضاعفات عشرة','ضرب عدد أحادي الرقم في مضاعفات مئة',
'ضرب عدد أحادي الرقم في مضاعفات ألف','العلاقة بين حقيقة الضرب الأساسية ومضاعفات عشرة',
'إيجاد عامل مجهول في جملة الضرب','تفسير القيمة المكانية للعدد المضروب',
'تطبيق نمط الضرب على النقود والوزن','تقدير الناتج والتحقق من صحته']
save(g/'lesson-024-atomic-source-coverage.json',{'LessonOrdinal':24,'Unit':4,'LessonCode':code,'Title':b['Title'],
'StudentPdf':'04-01.pdf','SourceSha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
'PrintedPages':[197,198,199,200,201,202],'PdfPages':[220,221,222,223,224,225],
'Skills':skills,'MappedCount':8,'TargetCount':8,'ArabicRich':True,'Visual':'EquationSet',
'ExactFamilies':3,'CoolDown':{'prompt':'احسب ٧ × ٦٠، و٧ × ٦٠٠، و٧ × ٦٠٠٠، وفسّر النمط.',
'expected':'٤٢٠ و٤٢٠٠ و٤٢٠٠٠','teacherReview':'شرح القيمة المكانية'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},'StageDRun':False,'Status':'READY_FOR_STAGE_D'})
save(g/'lesson-024-stage-a-c-checkpoint.json',{'LessonOrdinal':24,'LessonCode':code,
'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'StudentBook':'04-01.pdf pp197-202','AcademicMapped':'8/8','Tests':'3 passed, 0 failed, 0 skipped',
'StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT',
'NextLesson':'PED:UAE-MOE-MATH:L4:COMMON:04:02'})
pr.update({'A_B_C_Ready':24,'LastClosedLessonOrdinal':24,'FirstUnclosedLessonOrdinal':25,
'Note':'24 source-matched A-B-C lessons closed, Unit 4 L02 next, Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L24 staged closure; next U04 L02')
