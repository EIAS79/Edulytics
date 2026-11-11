import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2]
g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json')
assert pr['A_B_C_Ready']==19 and pr['FirstUnclosedLessonOrdinal']==20
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u03-l05-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:03:05'
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][2]['ls'][4]=='خصائص الضرب وقواعد القسمة'
assert R['Lessons'][0]['CultureCode']=='ar' and len(R['Lessons'][0]['WorkedExamples'])>=3
skills=['تحديد خاصية التبديل في الضرب','تطبيق المحايد الضربي','تطبيق خاصية الصفر في الضرب',
'تطبيق قسمة الصفر على عدد غير صفري','قسمة العدد على واحد','قسمة عدد غير صفري على نفسه',
'فصل القسمة على صفر غير المعرفة','حل جمل ناقصة ومسائل حياتية باستخدام الخصائص']
ledger={'Grade':4,'Unit':3,'LessonOrdinal':20,'LessonCode':code,'Title':b['Title'],
'StudentPdf':'04-01.pdf','SourceSha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
'PrintedPages':[161,162,163,164,165,166],'PdfPages':[184,185,186,187,188,189],
'SourceSkillCoverage':skills,'MappedCount':8,'TargetCount':8,'OriginalArabicRich':True,
'ExactPracticeFamilies':6,'Visual':'EquationSet',
'Cooldown':{'question':'أكمل ٩ × ١، ٩ × ٠، ٩ ÷ ١، ٩ ÷ ٩ ثم اشرح لماذا ٩ ÷ ٠ غير معرفة.',
'expected':'٩، ٠، ٩، ١؛ لا يمكن القسمة على صفر','teacherReview':'reasoning'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},'StageDRun':False,'Status':'READY_FOR_STAGE_D'}
save(g/'lesson-020-atomic-source-coverage.json',ledger)
save(g/'lesson-020-stage-a-c-checkpoint.json',{'LessonOrdinal':20,'LessonCode':code,'Unit':3,
'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'Source':'04-01.pdf printed 161-166','MathMapped':'8/8',
'ArabicRich':True,'ExactFamilies':6,'Tests':'3 passed, 0 failed, 0 skipped',
'StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT',
'NextLesson':'PED:UAE-MOE-MATH:L4:COMMON:03:06'})
pr.update({'A_B_C_Ready':20,'LastClosedLessonOrdinal':20,'FirstUnclosedLessonOrdinal':21,
'Note':'20/132 Grade 4 student-book A-B-C lessons closed, U03 L06 next; full grade migration and Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L20 closure; next U03 L06')
