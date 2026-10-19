'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const dir=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(dir,p),'utf8'));
const root='artifacts/curriculum-delivery/US-CCSS-MATH/G1/';
const ledger=read(root+'G1-SOURCE-ORIGINAL-TASK-PARITY-REMAINING-RISKS.json');
const originals=read(root+'full-grade-original-student-facing-task-inventory.json').rows;
const digital=read(root+'full-grade-practice-candidate-map.json').records;
test('all manually triaged original Student Facing task-risk rows use exact original SHA and new grade scope',()=>{
 assert.equal(ledger.planId,'EDULYTICS-GRADE-FIRST-FULL-GRADE-V1');
 assert.equal(ledger.rows.length,96,'All NEW Grade1 lessons require individual initial source-task triage');
 assert.equal(ledger.countActuallyTriaged,ledger.rows.length);
 assert.equal(ledger.total96AcademicAcceptedOnThisEvidence,0);
 assert.equal(ledger.rows.filter(x=>x.evidenceStage==='2021_STUDENT_FACING_EXCERPT_AND_TEACHER_ACTIVITY_TRIAGE_ONLY_NOT_FULL_SOURCE_ACTIVITY_APPROVAL').length,69,
   'Source excerpt-only triage is never confused with full original-activity review');
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
 for(const o of [65,119,146])assert.match(byOrdinal(o).remainingAcademicTaskParityWork,/authentic|actual|learner|child|group|teacher|partner/i);
 for(const o of [53,74,85,103,104,105,114,116,120,121,123,124,125,126,128,130,132,133,134,135,136,143,145])
   assert.ok(byOrdinal(o)?.remainingAcademicTaskParityWork,'Missing source task decision for '+o);
});
