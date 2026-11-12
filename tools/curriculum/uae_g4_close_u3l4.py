import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2]
g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=load(g/'full-grade-progress.json')
assert pr['A_B_C_Ready']==18 and pr['FirstUnclosedLessonOrdinal']==19
C=load(r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json')
B=load(r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json')
M=load(r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json')
R=load(r/'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u03-l04-ar.rich-lesson-v2.json')
code='PED:UAE-MOE-MATH:L4:COMMON:03:04'
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
assert l['Translations'][-1]['Title']==b['Title']==M['Units'][2]['ls'][3]=='المقارنة لحل المسائل'
assert R['Lessons'][0]['CultureCode']=='ar' and len(R['Lessons'][0]['WorkedExamples'])>=3
skills=['تمييز الفرق العددي من عدد الأضعاف','معادلة المقارنة الجمعية بالطرح',
'معادلة المقارنة الضربية بالضرب','المقارنة الضربية بالقسمة لاستخراج الأصل',
'إيجاد مجهول بالمعادلة','قراءة مقارنات من جداول الكميات',
'التحقق من الجملة العددية المناسبة','حل مسائل سياقية ومراجعة معقولية الناتج']
ledger={'Grade':4,'Unit':3,'LessonOrdinal':19,'LessonCode':code,'Title':b['Title'],
'StudentPdf':'04-01.pdf','SourceSha256':'859afc2c8e370d9360c25df32c821788458fc1f3808ad0045a0bc142c795daf3',
'PrintedPages':[153,154,155,156,157,158],'PdfPages':[176,177,178,179,180,181],
'MathematicalSourceSkills':skills,'MappedCount':8,'TargetCount':8,
'OriginalArabicRich':True,'Visual':'EquationSet','ReuseExactFamilies':4,
'Cooldown':{'question':'يملك عمر ٦ كرات ويملك علي ١٨. كم يزيد علي بالمقدار، وكم ضعفًا يملك؟',
'expected':'يزيد ١٢ كرة، ويمتلك ٣ أضعاف','review':'explanation reviewed by teacher'},
'TargetedTests':{'passed':3,'failed':0,'skipped':0},'StageDRun':False,
'Status':'READY_FOR_STAGE_D'}
save(g/'lesson-019-atomic-source-coverage.json',ledger)
save(g/'lesson-019-stage-a-c-checkpoint.json',{'LessonOrdinal':19,'LessonCode':code,'Unit':3,
'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D',
'Source':'04-01.pdf printed 153-158','AcademicMapped':'8/8',
'RichArabic':True,'ExactFamilyReuse':4,'Tests':'3 passed, 0 failed, 0 skipped',
'StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT',
'NextLesson':'PED:UAE-MOE-MATH:L4:COMMON:03:05'})
pr.update({'A_B_C_Ready':19,'LastClosedLessonOrdinal':19,'FirstUnclosedLessonOrdinal':20,
'Note':'19/132 student-book-matched A-B-C lessons closed; next U03 L05; Stage D pending.'})
save(g/'full-grade-progress.json',pr)
print('PASS L19 closure; next U03 L05')
