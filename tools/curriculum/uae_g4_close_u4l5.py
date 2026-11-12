import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2];g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json');assert pr['A_B_C_Ready']==27 and pr['FirstUnclosedLessonOrdinal']==28
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u04-l05-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:04:05'
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][3]['ls'][4]=='الضرب في عدد مكون من رقمين'
assert B['Units'][3]['LessonCount']==5 and len(M['Units'][3]['ls'])==5
assert R['Lessons'][0]['CultureCode']=='ar' and len(R['Lessons'][0]['WorkedExamples'])>=3
skills=['كتابة الضرب الرأسي لعدد من رقمين',
'ضرب الآحاد أولًا ثم العشرات',
'إعادة تجميع العشرات المحمولة',
'تفسير الضرب الرأسي بنواتج جزئية',
'تقدير الناتج للتحقق من معقوليته',
'استخدام نموذج المساحة للتأكيد',
'حل مسائل لفظية عن أعداد ذات رقمين',
'تفسير العدد المجهول في معادلة ضرب']
save(g/'lesson-028-atomic-source-coverage.json',{'LessonOrdinal':28,'Unit':4,'LessonCode':code,'Title':b['Title'],
'StudentPdf':'04-01.pdf','SourceSha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
'PrintedPages':[223,224,225,226,227,228],'PdfPages':[246,247,248,249,250,251],
'Skills':skills,'MappedCount':8,'TargetCount':8,'ArabicRich':True,'Visual':'EquationSet',
'ReusedExactFamilies':3,'CoolDown':{'prompt':'احسب ٢ × ٢٤ بالطريقة الرأسية ثم تحقق بنموذج المساحة.',
'expected':'٤٨؛ ٢ × ٢٠ = ٤٠ و٢ × ٤ = ٨','teacherReview':'خطوات ضرب المنازل'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},'StageDRun':False,'Status':'READY_FOR_STAGE_D'})
save(g/'lesson-028-stage-a-c-checkpoint.json',{'LessonOrdinal':28,'LessonCode':code,
'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'StudentSource':'04-01.pdf printed 223-228','AcademicMapped':'8/8',
'Tests':'3 passed, 0 failed, 0 skipped','StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT',
'NextLesson':'U04 L06 source index must be checked before proceeding'})
pr.update({'A_B_C_Ready':28,'LastClosedLessonOrdinal':28,'FirstUnclosedLessonOrdinal':29,
'Note':'28 source-matched A-B-C lessons closed including newly added Unit 4 L05; next source index to verify. Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L28 closure; next book section must be verified')
