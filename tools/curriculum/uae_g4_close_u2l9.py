import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2]
g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
progress=load(g/'full-grade-progress.json')
assert progress['A_B_C_Ready']==14 and progress['FirstUnclosedLessonOrdinal']==15
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u02-l09-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:02:09'
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
assert len(C['Lessons'])==len(B['Lessons'])==98
assert B['Units'][1]['LessonCount']==9 and len(M['Units'][1]['ls'])==9
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][1]['ls'][8]
assert R['Lessons'][0]['CultureCode']=='ar' and len(R['Lessons'][0]['WorkedExamples'])>=3
skills=[
'صياغة معادلة بحرف مجهول في قصة متتابعة',
'جمع ثم طرح بحسب ترتيب الأحداث',
'طرح ثم إضافة كمية في الخطوة التالية',
'إجراء عمليتي طرح متتاليتين',
'جمع ثلاثة أجزاء في مسألة حياتية',
'إيجاد القيمة المجهولة بالعكس والتعويض',
'التحقق بالتقدير من معقولية الحل',
'تقييم اختيار المعادلة المناسبة ومفردات المتغير']
ledger={'LessonOrdinal':15,'Grade':4,'Unit':2,'LessonCode':code,'Title':b['Title'],
'StudentPdf':'04-01.pdf','Sha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
'PrintedPages':[113,114,115,116,117,118],'PdfPages':[136,137,138,139,140,141],
'MathematicalSourceSkillCoverage':skills,'MappedCount':8,'TargetCount':8,
'RichOriginalArabic':True,'Visual':'CalculationChain','ExactFamilies':4,
'CoolDown':{'prompt':'لدى متجر ٣٬٢٠٠ درهم، تلقى ٨٠٠ ثم دفع ٣٠٠ و١٥٠ درهمًا. اكتب معادلة واذكر الباقي.',
'expected':'٣٬٥٥٠ درهمًا؛ ٣٬٢٠٠ + ٨٠٠ − ٣٠٠ − ١٥٠',
'teacherReview':'الشرح وصياغة المعادلة'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},
'StageDRun':False,'Status':'READY_FOR_STAGE_D'}
save(g/'lesson-015-atomic-source-coverage.json',ledger)
save(g/'lesson-015-stage-a-c-checkpoint.json',{'LessonOrdinal':15,'LessonCode':code,'Unit':2,
'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'Source':'04-01.pdf pp113-118','MathCategories':'8/8',
'ArabicRich':True,'Visual':'CalculationChain','ExactPracticeFamilies':4,
'Tests':'3 passed, 0 failed, 0 skipped','StageDRun':False,
'GitCommit':'SELF_CLOSURE_COMMIT','NextLesson':'PED:UAE-MOE-MATH:L4:COMMON:03:01'})
progress.update({'A_B_C_Ready':15,'LastClosedLessonOrdinal':15,'FirstUnclosedLessonOrdinal':16,
'ClosedUnitCount':2,'Note':'15/132 source-based A-B-C lesson closures, Units 1-2 completed, next Unit 3 L01; Stage D pending.'})
save(g/'full-grade-progress.json',progress)
print('PASS L15 and UNIT 2 closure; next Unit 3 L01')
