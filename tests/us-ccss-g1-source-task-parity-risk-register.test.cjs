'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const dir=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(dir,p),'utf8'));
const root='artifacts/curriculum-delivery/US-CCSS-MATH/G1/';
const ledger=read(root+'G1-SOURCE-ORIGINAL-TASK-PARITY-REMAINING-RISKS.json');
const originals=read(root+'full-grade-original-student-facing-task-inventory.json').rows;
const digital=read(root+'full-grade-practice-candidate-map.json').records;
test('all manually triaged original Student Facing task-risk rows use exact original SHA and new grade scope',()=>{
 assert.equal(ledger.planId,'EDULYTICS-GRADE-FIRST-FULL-GRADE-V1',
   'Archived source triage retains its original historical policy identity; active execution uses V2');
 assert.equal(ledger.rows.length,96,'All NEW Grade1 lessons require individual initial source-task triage');
 assert.equal(ledger.countActuallyTriaged,ledger.rows.length);
 assert.equal(ledger.total96AcademicAcceptedOnThisEvidence,0);
 assert.equal(ledger.rows.filter(x=>x.initialSourceReviewCohort==='INITIAL_SOURCE_EXCERPT_TRIAGE').length,69,
   'The original 69 excerpt-intake rows are retained even as some are later reviewed directly');
 assert.equal(ledger.rows.filter(x=>x.initialSourceReviewCohort==='PRIOR_DETAILED_TARGETED_REVIEW').length,27,
   'The 27 prior targeted-review rows must remain identifiable');
 assert.ok(ledger.rows.some(x=>x.ordinal===102&&x.evidenceStage.startsWith('DIRECT_READONLY_ORIGINAL')),
   'Direct original student source review must not be downgraded to an excerpt after deeper auditing');
 assert.equal(ledger.rows.filter(x=>x.internalAcademicApproval==='NOT_CLAIMED').length,96);
 assert.deepEqual(ledger.rows.map(x=>x.ordinal),Array.from({length:96},(_,i)=>i+51));
 const seen=new Set();
 for(const r of ledger.rows){
  assert.ok(Number.isInteger(r.ordinal)&&r.ordinal>=51&&r.ordinal<=146);
  assert.ok(!seen.has(r.ordinal),'Duplicate lesson task review '+r.ordinal);
  seen.add(r.ordinal);
  const src=originals.find(x=>x.ordinal===r.ordinal);
  const map=digital.find(x=>x.ordinal===r.ordinal);
  assert.ok(src&&map);
  assert.equal(r.lessonCode,src.lessonCode);
  assert.equal(r.original2021FullTeacherLessonHtmlSha256,src.original2021FullTeacherLessonHtmlSha256);
  assert.equal(r.original2021TeacherPreparationSha256,src.original2021TeacherPreparationSha256);
  assert.equal(r.original2021StudentFacingHtmlSha256,src.original2021StudentFacingHtmlSha256);
  assert.equal(r.candidateDigitalFamily,map.candidateFamily);
  assert.equal(r.assessingMode,map.assessmentMode);
  assert.ok(r.sourceRequiredTaskInterpretation.length>35);
  assert.ok(r.currentDigitalOrTeacherContentCandidate.length>30);
  assert.ok(r.remainingAcademicTaskParityWork.length>30);
  assert.equal(r.sourcePublisherArtworkFullyReviewed,false);
  assert.equal(r.internalAcademicApproval,'NOT_CLAIMED');
  assert.equal(r.actualAuthenticatedTaskRenderingAtExactHeadConfirmed,false);
 }
});
test('source task parity register tracks original authorship and geometry/measurement risks without certifying pupils',()=>{
 const byOrdinal=o=>ledger.rows.find(x=>x.ordinal===o);
 for(const o of [65,119,142,144,146])assert.match(byOrdinal(o).remainingAcademicTaskParityWork,/authentic|actual|learner|child|group|teacher|partner/i);
 for(const o of [53,74,85,103,104,105,114,116,120,121,123,124,125,126,128,130,132,133,134,135,136,143,145])
   assert.ok(byOrdinal(o)?.remainingAcademicTaskParityWork,'Missing source task decision for '+o);
});

test('whole-unaccepted Stage A selection covers exact 90 current unaccepted lessons without granting academic acceptance',()=>{
 const progress=read(root+'full-grade-progress.json');
 const cohort=ledger.stageAWholeUnacceptedCohort20261020;
 assert.ok(cohort,'Missing current whole-cohort Stage A intake');
 assert.equal(cohort.selectedCount,90);
 assert.equal(cohort.sourceIndexCount,90);
 assert.equal(cohort.originalTeacherActivityHeadingIndexRows,90);
 assert.equal(cohort.originalStudentFacingExcerptIndexRows,90);
 assert.equal(cohort.originalSourceSHAIndexedRows,90);
 assert.equal(cohort.sourceGoalInterpretationIndexedRows,90);
 assert.equal(cohort.practiceFamilyAndAssessmentMapIndexedRows,90);
 assert.deepEqual(cohort.alreadyIndividuallyApprovedAndNotReopened,[51,52,53,54,55,56]);
 assert.equal(cohort.manualGitHubCITriggered,false);
 assert.equal(cohort.finalRelease,'NO_GO');
 assert.equal(cohort.noFalseApprovals,true);
 const pending=progress.newLessons.filter(x=>x.status!=='INTERNALLY_ACCEPTED');
 assert.equal(pending.length,90);
 assert.deepEqual(pending.map(x=>x.ordinal),Array.from({length:90},(_,i)=>i+57));
 assert.deepEqual(cohort.assessmentModes,{
  DIGITAL_PRACTICE:80,DIGITAL_AND_SOURCE_AUTHORED_TASK:9,SOURCE_AUTHORED_TASK:1});
 assert.match(cohort.notYetProven,/not evidence that all 90 full original/i);
 assert.ok(ledger.rows.filter(x=>x.ordinal>=57).every(x=>
  x.sourceRequiredTaskInterpretation&&x.originalTeacherActivityHeadings?.length&&
  x.originalStudentFacingTaskExcerpts?.length&&x.internalAcademicApproval==='NOT_CLAIMED'));
});

test('Stage A source reinspection reuses 29 original full teacher files without claiming 29 completed academic lessons',()=>{
 const cohort=ledger.stageAWholeUnacceptedCohort20261020
   .fullTeacherTextReinspectionForPreviouslyUncheckpointedRows;
 assert.ok(cohort);
 assert.equal(cohort.inspectedCount,29);
 assert.equal(cohort.expectedShaMatchCount,29);
 assert.equal(cohort.acceptedByInspectionCount,0);
 assert.equal(cohort.originalTeacherActivityHeadingsUsed,true);
 assert.equal(cohort.originalStudentFacingTextSamplesUsed,true);
 const audited=ledger.rows.filter(x=>x.fullTeacherTextReinspection20261020);
 assert.deepEqual(audited.map(x=>x.ordinal),cohort.ordinalList);
 assert.equal(audited.length,29);
 for(const row of audited){
   const actual=row.fullTeacherTextReinspection20261020;
   const match=row.lessonCode.match(/:U(\d+):L(\d+)/);
   assert.ok(match,'Unexpected source lesson code: '+row.lessonCode);
   assert.equal(actual.originalFullTeacherRelativePath,
     'unit-'+Number(match[1])+'/lesson-'+Number(match[2])+'/lesson.html');
   assert.equal(actual.sha256Expected,row.original2021FullTeacherLessonHtmlSha256);
   assert.equal(actual.sha256Observed,actual.sha256Expected);
   assert.equal(actual.hashComparison,'MATCHED_READ_ONLY_LOCAL_FILE_20261020');
   assert.ok(actual.fullTeacherStudentFacingSegmentsObserved>=1);
   assert.equal(actual.originalSourceTextInspectedWithoutLocalModification,true);
   assert.equal(actual.sourceOnlyEvidenceNotAcademicApproval,true);
   assert.equal(actual.legalPublisherVisualsIndependentlyCleared,false);
   assert.equal(actual.authenticatedStudentPracticeGradingAndPersistenceAtCurrentHead,false);
   assert.equal(row.internalAcademicApproval,'NOT_CLAIMED');
 }
});
