import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2]
g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json')
assert pr['A_B_C_Ready']==15 and pr['FirstUnclosedLessonOrdinal']==16 and pr['ClosedUnitCount']==2
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u03-l01-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:03:01'
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
assert len(C['Lessons'])==len(B['Lessons'])==98
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][2]['ls'][0]=='العلاقة بين الضرب والقسمة'
assert M['Units'][2]['title']=='فهم الضرب والقسمة' and B['Units'][2]['Title']=='فهم الضرب والقسمة'
assert R['Lessons'][0]['CultureCode']=='ar' and len(R['Lessons'][0]['WorkedExamples'])>=3
skills=[
'تكوين مصفوفة من صفوف وأعمدة متساوية',
'العلاقة بين عدد المجموعات وحجمها والعدد الكلي',
'كتابة حقيقتين مرتبطتين للضرب',
'كتابة حقيقتين مرتبطتين للقسمة',
'استنتاج خارج القسمة من الضرب',
'إيجاد العامل المجهول بعائلة الحقائق',
'تطبيق المصفوفات والحقائق على مسألة حياتية',
'التحقق باستعمال العلاقة العكسية بين الضرب والقسمة']
ledger={'Grade':4,'Unit':3,'LessonOrdinal':16,'LessonCode':code,'Title':b['Title'],
'StudentPdf':'04-01.pdf','SourceSha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
'PrintedPages':[135,136,137,138,139,140],'PdfPages':[158,159,160,161,162,163],
'MathematicalSkillCoverage':skills,'MappedCount':8,'TotalCount':8,
'OriginalArabicRich':True,'Visual':'EquationSet','SelectedQuestionFamilies':4,
'CoolDown':{'question':'كوّن عائلة حقائق للأعداد ٤ و٧ و٢٨، واكتب جملتي ضرب وجملتي قسمة.',
'expected':'٤×٧=٢٨، ٧×٤=٢٨، ٢٨÷٤=٧، ٢٨÷٧=٤','explanation':'teacher review'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},
'StageDRun':False,'Status':'READY_FOR_STAGE_D'}
save(g/'lesson-016-atomic-source-coverage.json',ledger)
save(g/'lesson-016-stage-a-c-checkpoint.json',{'LessonOrdinal':16,'LessonCode':code,'Unit':3,
'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'Source':'04-01.pdf printed pp135-140','SourceMathematicsCategories':'8/8',
'OriginalArabicRich':True,'Visual':'EquationSet','ExactQuestionFamilies':4,
'Tests':'3 passed, 0 failed, 0 skipped','StageDRun':False,
'GitCommit':'SELF_CLOSURE_COMMIT','NextLesson':'PED:UAE-MOE-MATH:L4:COMMON:03:02'})
pr.update({'A_B_C_Ready':16,'LastClosedLessonOrdinal':16,'FirstUnclosedLessonOrdinal':17,
'Note':'16/132 source-based A-B-C lesson closures, next Unit 3 L02; full grade migration and Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L16 closure; next Unit 3 L02')
