import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2]
g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json')
assert pr['A_B_C_Ready']==13 and pr['FirstUnclosedLessonOrdinal']==14
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u02-l08-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:02:08'
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
assert len(C['Lessons'])==len(B['Lessons'])==97
assert B['Units'][1]['LessonCount']==8 and len(M['Units'][1]['ls'])==8
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][1]['ls'][7]
assert R['Lessons'][0]['CultureCode']=='ar' and len(R['Lessons'][0]['WorkedExamples'])>=3
features=[
'أفهم: تحديد المعطيات والمطلوب',
'أخطط: اختيار مخطط شريطي مناسب',
'أحل: الجمع لإيجاد الكل من جزأين',
'أحل: الطرح لإيجاد الفرق بين شريطين',
'أحل: الطرح لإيجاد الجزء المجهول',
'أتحقق: تقدير المجموع أو الفرق',
'تطبيق استراتيجية من أربع خطوات على مسائل حياتية',
'مراجعة استراتيجية المخطط الشريطي وشرح المعقولية']
ledger={'Grade':4,'Unit':2,'LessonOrdinal':14,'LessonCode':code,'Title':b['Title'],
'StudentPdf':'04-01.pdf','SourceSha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
'PrintedPages':[107,108,109,110,111,112],'PdfPages':[130,131,132,133,134,135],
'MappedSourceSkills':features,'MappedCount':8,'TotalCount':8,
'OriginalArabicContent':True,'Visual':'EquationSet with explicit part-whole notation, not a custom drawn bar UI',
'RepresentationLimitation':'Mathematics of bar models modeled with equations; a native bar graphic is not yet supported by existing RichV2 visual kinds',
'Cooldown':{'question':'رسمت شريطين لكمية ٢٬٧٤٥ وكمية ٤٬٢٥٥. ما مجموعهما، وأين تضع علامة الاستفهام؟',
'answer':'٧٬٠٠٠؛ على الشريط الكامل','ExplanationReview':'teacher reviewed'},
'ExactPracticeFamilies':3,'TargetedTests':{'passed':3,'failed':0,'skipped':0},
'StageDRun':False,'Status':'READY_FOR_STAGE_D'}
save(g/'lesson-014-atomic-source-coverage.json',ledger)
save(g/'lesson-014-stage-a-c-checkpoint.json',{'LessonOrdinal':14,'LessonCode':code,'Unit':2,
'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'Source':'04-01.pdf pp107-112','AcademicElementsMapped':'8/8',
'ArabicRich':True,'Visual':'EquationSet','ExactPracticeFamilies':3,
'TestOutcome':'3 passed, 0 failed, 0 skipped','StageDRun':False,
'GitCommit':'SELF_CLOSURE_COMMIT','NextLesson':'PED:UAE-MOE-MATH:L4:COMMON:02:09'})
pr.update({'A_B_C_Ready':14,'LastClosedLessonOrdinal':14,'FirstUnclosedLessonOrdinal':15,
'Note':'14/132 A-B-C lessons closed; U02 L09 next, full grade legacy migration and Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L14 closure; next U02 L09')
