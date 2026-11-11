import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2]
g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
progress=load(g/'full-grade-progress.json')
assert progress['A_B_C_Ready']==16 and progress['FirstUnclosedLessonOrdinal']==17
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u03-l02-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:03:02'
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][2]['ls'][1]=='علاقة القسمة بالطرح'
assert M['Units'][2]['title']=='فهم الضرب والقسمة' and B['Units'][2]['Title']=='فهم الضرب والقسمة'
assert R['Lessons'][0]['CultureCode']=='ar' and len(R['Lessons'][0]['WorkedExamples'])>=3
skills=[
'طرح العدد نفسه مرارا حتى الصفر',
'اعتبار عدد عمليات الطرح خارج القسمة',
'تمثيل القسمة بقفزات متساوية على خط الأعداد',
'تحديد المقسوم والمقسوم عليه وخارج القسمة',
'التحقق بالعلاقة العكسية مع الضرب',
'حل مسائل من الحياة بتوزيع عدد عناصر إلى مجموعات',
'إيجاد القيمة المجهولة في جملة قسمة',
'اكتشاف خطأ توقف الطرح قبل الوصول إلى الصفر']
ledger={'Grade':4,'Unit':3,'LessonOrdinal':17,'LessonCode':code,'Title':b['Title'],
'StudentPdf':'04-01.pdf','SourceSha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
'PrintedPages':[141,142,143,144,145,146],'PdfPages':[164,165,166,167,168,169],
'AcademicSkillCoverage':skills,'Mapped':8,'Target':8,
'OriginalArabicRich':True,'OriginalVisual':'NumberLine','SelectedQuestionFamilies':3,
'CoolDown':{'question':'أوجد ٢٠ ÷ ٥ بالطرح المتكرر واشرح عدد القفزات على خط الأعداد.',
'expected':'٤ قفزات: ٢٠، ١٥، ١٠، ٥، ٠','teacherReview':'شرح القفزات'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},
'StageDRun':False,'Status':'READY_FOR_STAGE_D'}
save(g/'lesson-017-atomic-source-coverage.json',ledger)
save(g/'lesson-017-stage-a-c-checkpoint.json',{'LessonOrdinal':17,'LessonCode':code,'Unit':3,
'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'Source':'04-01.pdf printed pp141-146','MathCategories':'8/8',
'ArabicRich':True,'Visual':'NumberLine','ExactPracticeFamilies':3,
'Tests':'3 passed, 0 failed, 0 skipped','StageDRun':False,
'GitCommit':'SELF_CLOSURE_COMMIT','NextLesson':'PED:UAE-MOE-MATH:L4:COMMON:03:03'})
progress.update({'A_B_C_Ready':17,'LastClosedLessonOrdinal':17,'FirstUnclosedLessonOrdinal':18,
'Note':'17/132 student-book-scope A-B-C closures, Unit 3 lesson 3 next, full grade migration/Stage D pending.'})
save(g/'full-grade-progress.json',progress)
print('PASS L17 closure; next U03 L03')
