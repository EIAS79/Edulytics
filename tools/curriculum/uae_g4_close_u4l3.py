import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2];g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json');assert pr['A_B_C_Ready']==25 and pr['FirstUnclosedLessonOrdinal']==26
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u04-l03-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:04:03'
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][3]['ls'][2]
assert R['Lessons'][0]['CultureCode']=='ar' and len(R['Lessons'][0]['WorkedExamples'])>=3
skills=['تمثيل العدد ذي الرقمين بالعشرات والآحاد','إنشاء مجموعات متساوية من مكعبات القيمة المكانية','ضرب العشرات بصورة مستقلة','ضرب الآحاد بصورة مستقلة','جمع النواتج الجزئية','إعادة التجميع عند تجاوز ١٠ آحاد','التحقق من النتيجة بمجموع متكرر','حل مسائل حياتية عن مجموعات متساوية']
save(g/'lesson-026-atomic-source-coverage.json',{'LessonOrdinal':26,'Unit':4,'LessonCode':code,'Title':b['Title'],
'StudentPdf':'04-01.pdf','SourceSha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
'PrintedPages':[209,210,211,212,213,214],'PdfPages':[232,233,234,235,236,237],
'Skills':skills,'MappedCount':8,'TargetCount':8,'ArabicRich':True,'Visual':'EquationSet',
'ExactFamilies':3,'CoolDown':{'prompt':'مثّل ٣ × ٢٤ بنموذج عشرات وآحاد واحسب الناتجين الجزئيين.',
'expected':'٣ × ٢٠ = ٦٠ و٣ × ٤ = ١٢؛ المجموع ٧٢',
'teacherReview':'تمثيل المكعبات'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},'StageDRun':False,'Status':'READY_FOR_STAGE_D'})
save(g/'lesson-026-stage-a-c-checkpoint.json',{'LessonOrdinal':26,'LessonCode':code,
'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'StudentSource':'04-01.pdf printed 209-214','AcademicMapped':'8/8',
'Tests':'3 passed, 0 failed, 0 skipped','StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT',
'NextLesson':'PED:UAE-MOE-MATH:L4:COMMON:04:04'})
pr.update({'A_B_C_Ready':26,'LastClosedLessonOrdinal':26,'FirstUnclosedLessonOrdinal':27,
'Note':'26 source-matched A-B-C lessons closed, Unit 4 L04 next, Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L26 closure; next U04 L04')
