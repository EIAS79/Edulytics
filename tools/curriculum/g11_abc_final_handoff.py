"""Generate Stage A-B-C final bookkeeping after the last strictly sequential lesson.
No runtime tests or CI.
"""
from pathlib import Path
import datetime,json
ROOT=Path(__file__).resolve().parents[2]
BASE=Path('artifacts/curriculum-delivery/US-CCSS-MATH/G11')
def load(p):return json.loads((ROOT/p).read_text(encoding='utf8'))
ledger=load(BASE/'abc-independent-course-ledger.json')
progress=load(BASE/'full-grade-progress.json')
inventory=load(BASE/'local-archive-source-inventory-20261106.json')
counts=[11,26,19,18,11,19,16]
entries=ledger['LessonEntries']
assert len(entries)==120
assert len(inventory['lessons'])==120
assert [row['Ordinal'] for row in entries]==list(range(1,121))
assert len({row['LessonCode'] for row in entries})==120
assert [sum(row['Unit']==u for row in entries) for u in range(1,8)]==counts
assert [x['lessonCode'] for x in inventory['lessons']]==[x['LessonCode'] for x in entries]
assert all(x['ABCReadyForD'] and x['Status']=='READY_FOR_STAGE_D' and x['TestsExecuted']=='NOT_RUN' and x['StageD']=='NOT_RUN' for x in entries)
assert progress['LessonsReadyForStageD']==120 and progress['LessonsRemaining']==0 and progress['FirstOpenLesson'] is None
assert progress['StageD']=='NOT_RUN' and progress['CI']=='NOT_RUN' and progress['Publication']=='NOT_PERFORMED'
evidence={}
for u,count in enumerate(counts,1):
 total=0
 for n in range(1,count+1):
  doc=load(BASE/f'u{u:02d}-l{n:02d}-element-coverage.json')
  total+=doc.get('Count',doc.get('SourceElementCount',doc.get('ElementCount',0)))
 evidence[f'U{u:02d}']=total
snap={
 'Grade':11,'Course':'IM Algebra 2 Kendall Hunt local archive','TotalUnits':7,'TotalLessons':120,
 'SourcePagesIndexed':600,'LessonsReadyForStageD':120,'LessonsRemaining':0,
 'LastLesson':entries[-1]['LessonCode'],
 'UnitLessonCounts':{f'U{u+1:02d}':count for u,count in enumerate(counts)},
 'SourceEvidenceItemCounts':evidence,
 'SourcePolicy':'Local teacher/student source archive; no AccessIM v360',
 'AuthoredTests':'WRITTEN_NOT_RUN','TestsExecuted':'NOT_RUN','CI':'NOT_RUN','StageD':'NOT_RUN',
 'Merged':'NO','Deployed':'NO','GeneratedUtc':datetime.datetime.now(datetime.timezone.utc).isoformat()}
(ROOT/BASE/'full-grade-abc-final-evidence.json').write_text(json.dumps(snap,ensure_ascii=False,indent=2)+'\n',encoding='utf8')
report=[
 '# Grade 11 Algebra 2 — A–B–C authoring handoff','',
 '**Status: 120/120 lessons registered READY_FOR_STAGE_D.**','',
 'Unit counts: '+', '.join(f'U{u+1:02d}={count}' for u,count in enumerate(counts)), '',
 '## Evidence and source of truth',
 'Local Kendall Hunt teacher/student archive was inventoried: 600 original HTML pages for 120 lessons with SHA-256 hashes. AccessIM and v360 were excluded.',
 'Per-lesson Rich V2 original explanation, mathematical examples, teacher evidence tasks, original Edulytics cooldowns, source-element mapping, checkpoints, and authored (unrun) test contracts were recorded in sequence. Original blueprint identity was preserved.','',
 'Source element evidence is a traceability map, not independent certification of full exact publisher parity, interactive assets or operation. Authored tests have NOT been executed. Teacher-reviewed diagrams, graphing, experiments and written arguments are not automatically graded.',
 'Publisher cooldown content requiring teacher authentication was not claimed retrieved; independently authored equivalent assessments were added.',
 'Prior owner academic authorisation is acknowledged; it does not prove third-party asset clearance or runtime correctness.','',
 '## Next stage',
 'Stage D must test actual rendering, math engines, feedback and grading, media and interactive actions, CI and release criteria. Stage D should not repeat A–B–C local source comparison.',
 'No Stage D, tests, CI, merge or deployment were performed in this authoring stage.'
]
(ROOT/BASE/'grade11-abc-handoff.md').write_text('\n'.join(report)+'\n',encoding='utf8')
print('G11_FINAL_ABC_METADATA_VERIFIED',len(entries),'LESSONS',evidence)
