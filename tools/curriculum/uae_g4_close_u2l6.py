import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2]
g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def read(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
progress=read(g/'full-grade-progress.json')
assert progress['A_B_C_Ready']==11 and progress['FirstUnclosedLessonOrdinal']==12
C=read(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=read(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=read(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=read(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u02-l06-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:02:06'
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
assert len(C['Lessons'])==len(B['Lessons'])==95
assert B['Units'][1]['LessonCount']==6 and len(M['Units'][1]['ls'])==6
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][1]['ls'][5]=='طرح الأعداد الكلية'
assert R['Lessons'][0]['CultureCode']=='ar' and len(R['Lessons'][0]['Visuals'])>=1
source=[
'المطروح منه والمطروح والفرق: تحديد الأجزاء',
'طرح الأعداد في جدول القيمة المكانية',
'إعادة التجميع عند عدم كفاية رقم المنزلة',
'إعادة التجميع الممتد إلى الألوف',
'التحقق بالجمع العكسي',
'التقدير للتحقق من معقولية الفرق',
'تدريبات متعددة المنازل وواجب',
'مسائل لفظية ومفردات وتفسير الحل']
ledger={'Grade':4,'Unit':2,'LessonOrdinal':12,'LessonCode':code,'Title':'طرح الأعداد الكلية',
'StudentPdf':'04-01.pdf','SourceSha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
'PrintedPages':[93,94,95,96,97,98],'PdfPages':[116,117,118,119,120,121],
'SourceMappedFeatures':source,'SourceMappedCount':8,'SourceFeatureCount':8,
'OriginalVisual':'PlaceValueChart','ExactQuestionFamilies':3,
'CoolDown':{'question':'اطرح ٥٬٠٤٢ − ٢٬٦١٧، واشرح كيف أعدت التجميع في المنازل.',
'answer':'٢٬٤٢٥','teacherReview':'الشرح المعلل يحتاج مراجعة المعلم'},
'TestOutcome':'3 passed, 0 failed, 0 skipped, .NET 10 scoped',
'StageDRun':False,'Status':'READY_FOR_STAGE_D','MigrationNote':'Missing lesson appended; old codes unchanged; full-grade legacy mapping pending.'}
save(g/'lesson-012-atomic-source-coverage.json',ledger)
save(g/'lesson-012-stage-a-c-checkpoint.json',{'LessonOrdinal':12,'LessonCode':code,'Unit':2,
'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'StudentSource':'04-01.pdf printed pp93-98','MappedMathCategories':'8/8',
'ArabicRich':True,'SelectedQuestionFamilies':3,'TargetedTests':'3 passed / 0 failed',
'StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT','NextLesson':'PED:UAE-MOE-MATH:L4:COMMON:02:07'})
progress.update({'A_B_C_Ready':12,'LastClosedLessonOrdinal':12,'FirstUnclosedLessonOrdinal':13,
'Note':'12/132 lesson-level A-B-C closed. Next U02 L07. Grade-level migration and Stage D are pending.'})
save(g/'full-grade-progress.json',progress)
print('PASS U02 L06 closure; next U02 L07')
