"""Initialize Stage D ledger from actual A-B-C evidence, not invented test successes."""
from pathlib import Path
import json,datetime,hashlib,subprocess
ROOT=Path(__file__).resolve().parents[2]
BASE=Path('artifacts/curriculum-delivery/US-CCSS-MATH/G11')
load=lambda p:json.loads((ROOT/p).read_text(encoding='utf8'))
ledger=load(BASE/'abc-independent-course-ledger.json')
inv=load(BASE/'local-archive-source-inventory-20261106.json')
sha=subprocess.check_output(['git','rev-parse','HEAD'],cwd=ROOT,text=True).strip()
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
snapshot='g11-600-local-html-sha256:'+hashlib.sha256(''.join(row['documents'][k]['sha256'] for row in inv['lessons'] for k in sorted(row['documents'])).encode()).hexdigest()
entries=[]
for entry in ledger['LessonEntries']:
 ev=load(Path(entry['ElementEvidence']))
 elems=ev.get('Elements',ev.get('SourceElements',[]))
 declared=ev.get('Count',ev.get('SourceElementCount',ev.get('ElementCount')))
 assert len(elems)==declared
 verified=sum(x.get('Status') in ('MATCHED_DIGITAL','MATCHED_TEACHER_LED') for x in elems)
 signed=len(elems)>0 and verified==len(elems) and ev.get('SourceElementDenominatorVerified') is True and ev.get('PublisherImagesPdfReviewed') is True
 entries.append({'ordinal':entry['Ordinal'],'lessonCode':entry['LessonCode'],'unit':entry['Unit'],'lesson':entry['Lesson'],
  'aCStatus':entry['Status'],'sourceEvidencePath':entry['ElementEvidence'],'assessmentPath':entry['AssessmentPath'],
  'sourceElementCountAuthored':declared,'sourceElementCountIndependentVerified':verified if signed else None,
  'sourceParity':'VERIFIED' if signed else 'UNKNOWN','sourceDenominatorVerified':bool(ev.get('SourceElementDenominatorVerified')),
  'baselineACSha':sha,'technicalStatus':'NOT_TESTED',
  'academicStatus':'OWNER_AUTHORIZATION_RECORDED_NOT_RIGHTS_CERTIFICATION','teacherReviewStatus':'PENDING_REVIEW',
  'actionsRunIds':[],'unitTests':'NOT_RUN','integrationTests':'NOT_RUN','authenticatedE2E':'NOT_RUN',
  'visualAccessibility':'NOT_RUN','practiceFamilyCoverage':'UNKNOWN','dStatus':'D_BLOCKED',
  'blockerReasons':['SOURCE_PARITY_DENOMINATOR_UNKNOWN','ASSETS_LICENSE_REVIEW_PENDING','RUNTIME_GRADE11_UNVERIFIED','GRADE11_HTTPS_DB_AUTH_NOT_RUN'],
  'updatedAtUtc':now})
out={'Grade':11,'UnitCounts':[11,26,19,18,11,19,16],'SourceSnapshotId':snapshot,
 'Branch':'curriculum/us-ccss-g11-algebra2-abc-20261106','BaselineACSha':sha,
 'CandidateSha':'PENDING_INITIAL_STAGE_D_COMMIT_AND_ACTIONS_RUN','FirstIncompleteLessonOrdinal':1,
 'VerifiedLessons':0,'BlockedLessons':list(range(1,121)),'TechnicalDecision':'NO_GO_TECHNICAL',
 'CI':'NOT_RUN','StageDStartedUtc':now,'LessonEntries':entries}
(ROOT/BASE/'grade11-stage-d-execution-ledger.json').write_text(json.dumps(out,ensure_ascii=False,indent=2)+'\n',encoding='utf8')
report=[
 '# Grade 11 Stage D — evidence index','',
 'Stage D initiated. No tests or CI success credited without a GitHub Actions run on actual candidate SHA.','',
 '- Scope: 120 A-B-C-authored lessons, 7 units. A-B-C baseline: '+sha,
 '- Source snapshot: '+snapshot,
 '- Source element counts recorded by A-B-C do not certify a complete denominator or independent rights and semantic parity.',
 '- Three Node shards: D1 1–40, D2 41–80, D3 81–120.',
 '- Workflow: .github/workflows/us-ccss-g11-stage-d.yml.',
 '- Current decision: NO_GO_TECHNICAL until real runtime tests and CI finish.','',
 '## Pending evidence','1. Authenticated isolated HTTPS student and teacher Grade 11 with real DB grading persistence.',
 '2. Independent math verification of supported real question families and wrong/invalid responses.',
 '3. Per-source visual/rights/teacher-reviewed task clearance.',
 '4. Visual-accessibility and shared-grade regression.'
]
(ROOT/BASE/'grade11-stage-d-evidence-index.md').write_text('\n'.join(report)+'\n',encoding='utf8')
(ROOT/BASE/'grade11-final-ci-summary.md').write_text('# Grade 11 Stage D / Final CI\n\nNO_GO_TECHNICAL — candidate test runs pending.\n\nD_VERIFIED 0/120. No run IDs or successful E2E tests established yet.\n',encoding='utf8')
print('INITIAL_STAGE_D_LEDGER_CREATED',len(entries),'LESSONS ZERO_D_VERIFIED',snapshot)
