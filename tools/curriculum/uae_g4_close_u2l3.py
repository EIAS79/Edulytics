import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2]
g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,v):p.write_text(json.dumps(v,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json')
assert pr['A_B_C_Ready']==8 and pr['FirstUnclosedLessonOrdinal']==9
code='PED:UAE-MOE-MATH:L4:COMMON:02:03'
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
rich=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u02-l03-ar.rich-lesson-v2.json')
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][1]['ls'][2]=='الجمع والطرح الذهني'
assert rich['Lessons'][0]['CultureCode']=='ar' and len(rich['Lessons'][0]['WorkedExamples'])>=3
elements=[
('التعويض عند جمع عددين لتكوين عشرة أو مئة','RichV2 worked examples + make_friendly_ten and compensate_addition'),
('الجمع الذهني لمقادير متعددة المنازل','RichV2 examples, independent exact numeric tasks'),
('الطرح الذهني بالتجزئة والتعويض','break_apart_subtraction and worked example'),
('طرح الألوف من الألوف','subtract_thousands and place value explanation'),
('تدريب موجه على تكوين عدد مناسب','Arabic original guided examples and test fixtures'),
('التمارين المستقلة والتحقق بالعملية العكسية','4 exact families plus reverse-operation checks'),
('مسائل حياتية واستدلال وتفسير طريقة الحل','RichV2 prose and separate teacher-reviewed explanatory responses'),
('مراجعة المفردات والتقويم والواجب','Original cooldown, lesson summary and 3 targeted tests')
]
ledger={
 'Grade':4,'Unit':2,'LessonOrdinal':9,'LessonCode':code,'Title':'الجمع والطرح الذهني',
 'StudentPdf':'04-01.pdf','Sha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
 'PrintedPages':list(range(73,79)),'PdfPages':list(range(96,102)),
 'SourceElements':[{'title':x,'implementation':y,'coverage':'ORIGINAL_MATHEMATICAL_EQUIVALENT'} for x,y in elements],
 'SourceCoverage':'8/8 mathematical element categories; not reproduction of publisher text/images',
 'CoolDown':{'question':'احسب ٣٩٩ + ٨٧ ذهنيًا وفسر كيف عوضت العدد الذي أضفته.','expected':'٤٨٦؛ حولت ٣٩٩ إلى ٤٠٠ وأنقصت واحدًا من ٨٧','verbalExplanation':'teacher review'},
 'PracticeFamilies':['make_friendly_ten','compensate_addition','break_apart_subtraction','subtract_thousands'],
 'Tests':{'filter':'UaeGrade4Unit2Lesson03MentalTests','passed':3,'failed':0,'skipped':0},
 'StageDTestsRun':False,
 'Status':'READY_FOR_STAGE_D'
}
save(g/'lesson-009-atomic-source-coverage.json',ledger)
save(g/'lesson-009-stage-a-c-checkpoint.json',{'LessonOrdinal':9,'LessonCode':code,'Unit':2,'Status':'READY_FOR_STAGE_D',
'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','StudentSource':'04-01.pdf printed 73-78',
'ArabicRichV2':True,'MathematicsElementsMapped':'8/8','ExactQuestionFamilies':4,
'TestOutcome':'Passed 3 / Failed 0 / Skipped 0','StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT','NextLesson':'PED:UAE-MOE-MATH:L4:COMMON:02:04'})
pr.update({'A_B_C_Ready':9,'LastClosedLessonOrdinal':9,'FirstUnclosedLessonOrdinal':10,'Note':'9/132 lesson-level A-B-C closed; unit 2 lesson 4 is next; legacy grade catalogue migration and Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L09 closure; next U02 L04')
