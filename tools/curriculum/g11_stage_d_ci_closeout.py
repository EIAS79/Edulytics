#!/usr/bin/env python3
"""Close Grade 11 Stage D ONLY against a completed exact-SHA GitHub Actions run.
This records technical verification and owner's academic decision separately
from unperformed independent publisher source parity or human WCAG inspection.
"""
import json,sys
from pathlib import Path
assert len(sys.argv)==3, "usage: closeout.py github-run.json github-jobs.json"
run=json.loads(Path(sys.argv[1]).read_text(encoding='utf-8-sig'))
jobs_json=json.loads(Path(sys.argv[2]).read_text(encoding='utf-8-sig'))
jobs=jobs_json.get('jobs',[])
assert run['name']=='Grade 11 Algebra 2 Stage D - evidence and runtime gates'
assert run['conclusion']=='success' and run['status']=='completed'
sha=run['head_sha']
assert len(sha)==40 and all(c in '0123456789abcdef' for c in sha)
assert len(jobs)>=9 and all(j['status']=='completed' for j in jobs)
required=[
 'structural-inventory',
 'owner-academic-acceptance',
 'owner-accepted-source-handoff-and-license',
 'dotnet-build-and-targeted-regression',
 'authenticated-g11-120-https',
 'verified-practice-route-coverage',
 'named-lesson-contract-shards (D1, 1, 40)',
 'named-lesson-contract-shards (D2, 41, 80)',
 'named-lesson-contract-shards (D3, 81, 120)',
]
by_name={j['name']:j for j in jobs}
assert all(by_name[n]['conclusion']=='success' for n in required)
root=Path(__file__).resolve().parents[2]
base=root/'artifacts/curriculum-delivery/US-CCSS-MATH/G11'
owner=json.loads((base/'grade11-owner-academic-publication-authorization-20261106.json').read_text(encoding='utf-8-sig'))
assert owner['OwnerDecision']=='ACADEMICALLY_APPROVED_BY_OWNER'
assert owner['StageDTechnicalAcceptance']=='NOT_IMPLIED'
ledger_path=base/'grade11-stage-d-execution-ledger.json'
ledger=json.loads(ledger_path.read_text(encoding='utf-8-sig'))
assert len(ledger['LessonEntries'])==120
assert all(e['aCStatus']=='READY_FOR_STAGE_D' for e in ledger['LessonEntries'])
ledger['CandidateSha']=sha
ledger['VerifiedLessons']=120
ledger['FirstIncompleteLessonOrdinal']=None
ledger['BlockedLessons']=[]
ledger['TechnicalDecision']='GO_G11_STAGE_D_TECHNICAL_VERIFIED_NOT_DEPLOYED'
ledger['CI']={'actionsRunId':run['id'],'exactTestedSha':sha,'jobCountVerified':len(required),
 'allNineGrade11JobsPassed':True,'workflowUrl':run['html_url'],
 'scope':'REAL_120_LESSON_HTTPS_AND_120_SYNTHETIC_TEACHER_GRADED_ROWS'}
ledger['AcademicAcceptance']='OWNER_ACADEMIC_APPROVED'
ledger['IndependentPublisherSourceSemanticCertification']='NOT_CLAIMED'
ledger['ExternalRichV2VisualAssets']='NONE_IN_120_RESOURCES'
ledger['PublicationStatus']='NOT_DEPLOYED_AT_STAGE_D_CLOSEOUT'
for row in ledger['LessonEntries']:
    row['technicalStatus']='PASS_ON_GITHUB_ACTIONS_CANDIDATE_SHA'
    row['academicStatus']='OWNER_ACADEMIC_APPROVED_NOT_PUBLISHER_CERTIFICATION'
    row['teacherReviewStatus']='SYNTHETIC_TEACHER_REVIEW_SCORES_PERSISTED_120_OF_120_NOT_REAL_LEARNER_GRADES'
    row['actionsRunIds']=[run['id']]
    row['unitTests']='G11_D1_D2_D3_NAMED_SHARDS_PASS_AND_DOTNET_TARGETED_PASS'
    row['integrationTests']='PASS_ISOLATED_DATABASE_MIGRATIONS_TEACHER_REVIEW_PERSISTENCE'
    row['authenticatedE2E']='PASS_120_STUDENT_HTTPS_120_TEACHER_SUBMISSIONS_AND_120_SCORE_READBACKS'
    row['visualAccessibility']='RICHV2_HTML_RENDER_TEST_PASS_WCAG_HUMAN_AUDIT_NOT_CLAIMED'
    row['practiceFamilyCoverage']='120_TEACHER_REVIEW_ROUTES_VERIFIED_AUTOGRADED_PRACTICE_NOT_CLAIMED'
    row['dStatus']='D_VERIFIED'
    row['blockerReasons']=[]
    row['verifiedCandidateSha']=sha
    assert row['sourceParity']=='UNKNOWN'
    assert row['sourceDenominatorVerified'] is False
ledger_path.write_text(json.dumps(ledger,ensure_ascii=False,indent=2)+'\n',encoding='utf8')
report=f"""# Grade 11 Algebra 2 Stage D â€” Final CI evidence

- Academic handoff: 7 units, 120 lessons, Aâ€“Bâ€“C owner-approved.
- Technical Stage D: **120/120 D_VERIFIED** on the exact tested candidate SHA `{sha}`.
- [Grade 11 GitHub Actions run]({run['html_url']}): all nine required jobs succeeded. Named D1/D2/D3 shards, .NET regression, isolated PostgreSQL migration, authenticated HTTPS for all 120 lessons, 120 synthetic student response submissions, 120 authorized teacher score writes and student readbacks, negative duplicate submission, cross-school denial.
- **Automation boundary:** teacher review is manual. CI uses synthetic responses and rubric marks; it does not certify real learners' mathematics or claim 120 numeric auto-grading routes.
- **Academic decision:** owner has approved original Edulytics curriculum and authorized publication subject to technical and applicable legal controls. Original archive identifiers and SHA inventories are preserved.
- **Rights boundary:** this release's 120 RichV2 lessons contain zero external image/video assets. The central /content-sources-licenses.html preserves any required adaptation attribution. Similar mathematical objectives do not independently license external artwork.
- **Source matching boundary:** 5,114 source elements were recorded in Aâ€“Bâ€“C across 600 locally archived source pages. Exhaustive independent 1:1 semantic source comparison remains **UNKNOWN** and is not described as certified. Owner's academic acceptance is distinct.
- **Visual boundary:** HTTPS rendering is tested, but independent human WCAG/visual inspection is not claimed.
- Deployment at this closeout: **NOT YET DEPLOYED**. PR merge and post-deploy monitoring must be recorded separately.
"""
(base/'grade11-final-ci-summary.md').write_text(report,encoding='utf8')
(base/'grade11-stage-d-evidence-index.md').write_text(
  '# Grade 11 Stage D â€” evidence index\n\n'
  +f"Exact tested candidate: `{sha}`. [All nine technical jobs]({run['html_url']}) passed.\n\n"
  +'Evidence: grade11-stage-d-execution-ledger.json (120/120 technical D_VERIFIED), '
  +'grade11-owner-academic-publication-authorization-20261106.json, '
  +'abc-independent-course-ledger.json, source-of-truth.json, '
  +'local-archive-source-inventory-20261106.json, final-ci-summary.\n\n'
  +'Owner academic approval is recorded; independent publisher element-level semantic equality is not certified. '
  +'No publisher-owned image/video assets are embedded in these RichV2 lesson files. '
  +'Auto-grading of all 120 lessons is not asserted; authenticated manual teacher-review routes were exercised.\n',encoding='utf8')
print('G11_STAGE_D_TECHNICAL_CLOSEOUT_PASS',sha,run['id'],'D_VERIFIED',ledger['VerifiedLessons'])
