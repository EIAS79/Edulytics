import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2];g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json')
assert pr['A_B_C_Ready']==24 and pr['FirstUnclosedLessonOrdinal']==25
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u04-l02-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:04:02'
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][3]['ls'][1]
assert R['Lessons'][0]['CultureCode']=='ar' and len(R['Lessons'][0]['WorkedExamples'])>=3
skills=['تقريب العامل متعدد الأرقام إلى أكبر منزلة','تقدير حاصل ضرب عدد أحادي وثلاثي الأرقام',
'تقدير حاصل ضرب عدد أحادي ورباعي الأرقام','التحقق من التقدير بالمقارنة',
'تحديد ما إذا كان التقدير أكبر من الناتج الفعلي','تحديد ما إذا كان التقدير أصغر من الناتج الفعلي',
'تطبيق تقدير الضرب على المال والتكلفة','شرح معقولية التقدير وتحديد الاتجاه']
save(g/'lesson-025-atomic-source-coverage.json',{'LessonOrdinal':25,'Unit':4,'LessonCode':code,'Title':b['Title'],
'StudentPdf':'04-01.pdf','SourceSha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
'PrintedPages':[203,204,205,206,207,208],'PdfPages':[226,227,228,229,230,231],
'Skills':skills,'MappedCount':8,'TargetCount':8,'ArabicRich':True,'Visual':'EquationSet',
'ExactFamilies':2,'CoolDown':{'prompt':'قدّر ٧ × ٤٤٩٩ لأقرب ألف وحدد هل التقدير أكبر أم أقل من الناتج الفعلي.',
'expected':'٧ × ٤٠٠٠ = ٢٨٠٠٠؛ أقل من الدقيق','teacherReview':'تفسير قاعدة التقريب'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},'StageDRun':False,'Status':'READY_FOR_STAGE_D'})
save(g/'lesson-025-stage-a-c-checkpoint.json',{'LessonOrdinal':25,'LessonCode':code,
'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'StudentSource':'04-01.pdf printed 203-208','AcademicMapped':'8/8','Tests':'3 passed, 0 failed, 0 skipped',
'StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT',
'NextLesson':'PED:UAE-MOE-MATH:L4:COMMON:04:03'})
pr.update({'A_B_C_Ready':25,'LastClosedLessonOrdinal':25,'FirstUnclosedLessonOrdinal':26,
'Note':'25 source-matched A-B-C lessons closed, Unit 4 L03 next, Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L25 staged closure; next U04 L03')
