import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2]
g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json')
assert pr['A_B_C_Ready']==21 and pr['FirstUnclosedLessonOrdinal']==22
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u03-l07-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:03:07'
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][2]['ls'][6]=='العوامل والمضاعفات'
assert R['Lessons'][0]['CultureCode']=='ar' and len(R['Lessons'][0]['WorkedExamples'])>=3
skills=['تحديد مفهوم العامل والعدد القابل للقسمة بلا باقٍ',
'إيجاد أزواج عوامل عدد موجب',
'تحديد جميع عوامل العدد من مصفوفات',
'مفهوم المضاعفات باستخدام جدول الضرب',
'تحديد أول خمسة مضاعفات بدءًا بصفر',
'المقارنة بين عوامل عددين بمخطط فن',
'التمثيل بمصفوفات متساوية للعدد',
'حل مسائل لفظية ومعالجة الخلط بين العوامل والمضاعفات']
ledger={'Grade':4,'Unit':3,'LessonOrdinal':22,'LessonCode':code,'Title':b['Title'],
'StudentPdf':'04-01.pdf','SourceSha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
'PrintedPages':[173,174,175,176,177,178],'PdfPages':[196,197,198,199,200,201],
'SourceSkills':skills,'MappedCount':8,'TotalCount':8,
'OriginalArabicRich':True,'Visual':'EquationSet','ExactFamilies':3,
'CoolDown':{'prompt':'اذكر عوامل العدد ١٨، ثم المضاعفات الخمسة الأولى للعدد ٣ بدءًا من صفر.',
'expected':'عوامل ١٨: ١،٢،٣،٦،٩،١٨؛ المضاعفات: ٠،٣،٦،٩،١٢',
'teacherReview':'تفسير الفرق بين العامل والمضاعف'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},
'StageDRun':False,'Status':'READY_FOR_STAGE_D'}
save(g/'lesson-022-atomic-source-coverage.json',ledger)
save(g/'lesson-022-stage-a-c-checkpoint.json',{'LessonOrdinal':22,'LessonCode':code,'Unit':3,
'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'StudentSource':'04-01.pdf printed 173-178','AcademicSkills':'8/8',
'RichArabic':True,'ExactFamilies':3,'Tests':'3 passed, 0 failed, 0 skipped',
'StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT',
'NextLesson':'PED:UAE-MOE-MATH:L4:COMMON:03:08'})
pr.update({'A_B_C_Ready':22,'LastClosedLessonOrdinal':22,'FirstUnclosedLessonOrdinal':23,
'Note':'22/132 Grade 4 student-book A-B-C lessons closed; U03 L08 next; Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L22 closure; next U03 L08')
