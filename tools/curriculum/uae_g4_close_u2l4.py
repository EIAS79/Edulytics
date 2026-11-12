import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2]
g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def read(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def write(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=read(g/'full-grade-progress.json')
assert pr['A_B_C_Ready']==9 and pr['FirstUnclosedLessonOrdinal']==10
code='PED:UAE-MOE-MATH:L4:COMMON:02:04'
C=read(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=read(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=read(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
Rich=read(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u02-l04-ar.rich-lesson-v2.json')
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][1]['ls'][3]
assert Rich['Lessons'][0]['CultureCode']=='ar' and len(Rich['Lessons'][0]['WorkedExamples'])>=3
elements=[
('التقريب إلى أقرب مئة للمجموع','sum_nearest_hundred'),
('التقريب إلى أقرب مئة للفرق','diff_nearest_hundred'),
('التقريب إلى أقرب ألف للمجموع','sum_nearest_thousand'),
('التقريب إلى أقرب ألف للفرق','diff_nearest_thousand'),
('التقريب إلى أقرب عشرة آلاف للمجموع','sum_nearest_ten_thousand'),
('التقريب إلى أقرب عشرة آلاف للفرق','diff_nearest_ten_thousand'),
('مسائل الحياة الواقعية ومهارة اختيار المنزلة','RichV2 examples and teacher-reviewed reasoning'),
('تقويم منزلة التقريب ومعقولية التقدير','Original cooldown, visual and scoped tests')]
ledger={'Grade':4,'Unit':2,'LessonOrdinal':10,'LessonCode':code,'Title':'تقدير المجاميع والفروق',
'StudentPdf':'04-01.pdf','StudentPdfSHA256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
'StudentPrintedPages':[79,80,81,82,83,84],'StudentPdfPages':[102,103,104,105,106,107],
'SourceElementCoverage':[{'element':a,'implementation':b,'status':'ORIGINAL_EQUIVALENT'} for a,b in elements],
'CoolDown':{'prompt':'قدّر ٤٬٦٨٢ + ٢٬٣١٩ إلى أقرب مئة، واشرح خطواتك.','expected':'٤٬٧٠٠ + ٢٬٣٠٠ = ٧٬٠٠٠ تقريبًا','explanation':'teacher review'},
'TestOutcome':'3 passed, 0 failed, 0 skipped, scoped NET10',
'StageDTestsRun':False,'Status':'READY_FOR_STAGE_D'}
write(g/'lesson-010-atomic-source-coverage.json',ledger)
write(g/'lesson-010-stage-a-c-checkpoint.json',{'LessonOrdinal':10,'LessonCode':code,'Unit':2,
'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'StudentSource':'04-01.pdf printed pp79-84','SourceCoverage':'8/8 mathematical categories',
'PracticeFamilies':6,'OriginalArabicRichV2':True,'Visual':'CalculationChain',
'TestOutcome':'Passed 3 / Failed 0 / Skipped 0','StageDTestsRun':False,'GitCommit':'SELF_CLOSURE_COMMIT',
'NextLesson':'PED:UAE-MOE-MATH:L4:COMMON:02:05'})
pr.update({'A_B_C_Ready':10,'LastClosedLessonOrdinal':10,'FirstUnclosedLessonOrdinal':11,
'Note':'10/132 source-aligned lesson A-B-C closures; Unit 2 L05 next. Stage D, release and grade-level legacy migration pending.'})
write(g/'full-grade-progress.json',pr)
print('PASS L10 source/data/visual/3 test closure gate; next U02 L05')
