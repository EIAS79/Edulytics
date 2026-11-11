import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2]
g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json')
assert pr['A_B_C_Ready']==22 and pr['FirstUnclosedLessonOrdinal']==23
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u03-l08-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:03:08'
l=next(x for x in C['Lessons'] if x['LessonCode']==code);b=next(x for x in B['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][2]['ls'][7]=='استقصاء حل المسائل: الإجابات المنطقية'
assert R['Lessons'][0]['CultureCode']=='ar' and len(R['Lessons'][0]['WorkedExamples'])>=3
skills=['استخدام خطوات الفهم والتخطيط والحل والتحقق',
'تقدير مجموع كميات ثم تقييم معقوليته',
'استعمال العمليات الحسابية الدقيقة للتحقق',
'تقييم إجابة قائمة على جدول بيانات',
'تقييم قدرة الادخار على بلوغ هدف',
'تقييم مسافة رحلة أو زمن معطى',
'تصحيح افتراض حسابي غير معقول',
'شرح إجابة منطقية بدليل عددي']
ledger={'Grade':4,'Unit':3,'LessonOrdinal':23,'LessonCode':code,'Title':b['Title'],
'StudentPdf':'04-01.pdf','SourceSha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
'PrintedPages':[179,180,181,182,183,184],'PdfPages':[202,203,204,205,206,207],
'Skills':skills,'MappedCount':8,'TargetCount':8,'OriginalArabicRich':True,
'ExactFamilies':3,'Visual':'EquationSet',
'CoolDown':{'prompt':'هل تقدير ٣٠٠ للمجموع ٢٨٥ + ٣٠٨ معقول؟ اكتب تقديرًا أنسب واشرح.',
'expected':'لا؛ نحو ٦٠٠','teacherReview':'التفسير'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},
'StageDRun':False,'Status':'READY_FOR_STAGE_D'}
save(g/'lesson-023-atomic-source-coverage.json',ledger)
save(g/'lesson-023-stage-a-c-checkpoint.json',{'LessonOrdinal':23,'LessonCode':code,'Unit':3,
'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'StudentSource':'04-01.pdf printed 179-184','AcademicMapped':'8/8',
'ArabicRich':True,'ExactFamilies':3,'Tests':'3 passed, 0 failed, 0 skipped',
'StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT',
'NextLesson':'PED:UAE-MOE-MATH:L4:COMMON:03:09'})
pr.update({'A_B_C_Ready':23,'LastClosedLessonOrdinal':23,'FirstUnclosedLessonOrdinal':24,
'Note':'23/132 student-book A-B-C lessons closed; U03 L09 next, Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L23 closure; next U03 L09')
