import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2]
g=r/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def read(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
pr=read(g/'full-grade-progress.json')
assert pr['A_B_C_Ready']==23 and pr['FirstUnclosedLessonOrdinal']==24
cp=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
bp=r/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
mp=r/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=read(cp);B=read(bp);M=read(mp)
code='PED:UAE-MOE-MATH:L4:COMMON:03:09'
old_l=next(x for x in C['Lessons'] if x['LessonCode']==code)
old_b=next(x for x in B['Lessons'] if x['LessonCode']==code)
assert len(M['Units'][2]['ls'])==9
assert B['Units'][2]['LessonCount']==9 and old_b['SortOrder']==24
assert M['Units'][2]['ls'][8]=='Solve Multi-Step Problems Involving Addition and Subtraction'
archive={'Reason':'U03 has only 8 numbered lessons in selected student book. Following U03 L08 comes student Unit 3 review, not U03 L09.',
'StudentBook':'04-01.pdf','PrintedEndOfUnit':184,'ReviewBeginsPrinted':185,
'ObsoleteActiveCode':code,'LegacyBlueprintRecord':old_b,'LegacyContentRecord':old_l,
'MigrationPolicy':'Retain former identity in archive and separate crosswalk; never delete persisted learner records until validated crosswalk migration.'}
save(g/'legacy-u03-l09-reconciliation.json',archive)
C['Lessons']=[x for x in C['Lessons'] if x['LessonCode']!=code]
B['Lessons']=[x for x in B['Lessons'] if x['LessonCode']!=code]
for x in B['Lessons']:
    if x['SortOrder']>24:x['SortOrder']-=1
B['Units'][2]['LessonCount']=8
M['Units'][2]['ls']=M['Units'][2]['ls'][:8]
M['LessonCount']=sum(len(u['ls']) for u in M['Units'])
save(cp,C);save(bp,B);save(mp,M)
assert len(C['Lessons'])==len(B['Lessons'])==97
assert B['Units'][2]['LessonCount']==len(M['Units'][2]['ls'])==8
assert next(x for x in B['Lessons'] if x['LessonCode'].endswith(':04:01'))['SortOrder']==24
pr.update({'ClosedUnitCount':3,
'FirstUnclosedLessonOrdinal':24,
'Note':'23/132 student-book lessons closed; Unit 3 source ends after 8 lessons, obsolete legacy U03 L09 archived; next Unit 4 Lesson 1. Stage D pending.'})
save(g/'full-grade-progress.json',pr)
save(g/'unit-003-stage-a-c-closure.json',{
'Unit':3,'Title':'فهم الضرب والقسمة','StudentBook':'04-01.pdf',
'FirstOrdinal':16,'LastOrdinal':23,'StudentNumberedLessons':8,
'SourceFinalLesson':'استقصاء حل المسائل: الإجابات المنطقية',
'Statuses':['READY_FOR_STAGE_D']*8,
'LegacyReconciledCode':code,
'Status':'A_B_C_SOURCE_UNIT_CLOSED',
'StageDRun':False,'NextLesson':'PED:UAE-MOE-MATH:L4:COMMON:04:01'})
print('PASS Unit3 eight real student lessons; archive old phantom L09; next U04 L01 ordinal24')
