'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const base='artifacts/curriculum-delivery/US-CCSS-MATH/G1/';
const load=p=>JSON.parse(fs.readFileSync(path.resolve(root,base+p),'utf8'));
const checkpoints=()=>Array.from({length:90},(_,i)=>{
  const n=i+57; return load('lesson-'+n+'-v2-stage-a-d-checkpoint.json');
});
const RUN_ID=39600111939,CI_SHA='5fb0d3786d52379e60de01e3773297afc34d3721';
function validate(cp,p,l,map,n){
  const e=[],bad=m=>e.push(m),d=cp?.stageDClosure,mode=map?.assessmentMode;
  if(cp?.ordinal!==n||p?.ordinal!==n||l?.ordinal!==n||map?.ordinal!==n||
     cp?.lessonCode!==p?.lessonCode||cp?.lessonCode!==map?.lessonCode||
     d?.ordinal!==n||d?.lessonCode!==cp?.lessonCode)bad('IDENTITY');
  if(cp?.aToCClosure?.outcome!=='READY_FOR_STAGE_D' ||
     !Array.isArray(cp?.aToCClosure?.openACDefects) ||
     cp.aToCClosure.openACDefects.length)bad('A_C_NOT_CLOSED');
  if(d?.phase!=='D'||d?.status!=='D_VERIFIED'||
     l?.stageDStatus!==(p?.status==='INTERNALLY_ACCEPTED'?'APPROVED':'D_VERIFIED')||
     p?.stageDTechnicalAcceptance?.result!=='VERIFIED_SOURCE_SCOPED_TECHNICAL_CANDIDATE')
    bad('STAGE_D_NOT_VERIFIED');
  if(d?.testedCodeSha!==CI_SHA||d?.authenticatedRunId!==RUN_ID||
     d?.authenticatedLearnerArtifactId!==12345398813||
     d?.technicalReportArtifactId!==12346166910||
     l?.technicalReconciliationProof?.workflowRunId!==RUN_ID||
     l?.technicalReconciliationProof?.testedCodeSha!==CI_SHA||
     d?.authenticatedLearnerPageRendered!==true||
     d?.selectedSourceTaskTextChecked!==true)bad('PROVENANCE');
  if(d?.assessmentMode!==mode ||
     !['DIGITAL_PRACTICE','DIGITAL_AND_SOURCE_AUTHORED_TASK','SOURCE_AUTHORED_TASK'].includes(mode))
    bad('ASSESSMENT_MODE');
  if(d?.correctWrongMalformedPersisted!==(mode!=='SOURCE_AUTHORED_TASK')||
     d?.authoredOnlyInstructionsRendered!==(mode==='SOURCE_AUTHORED_TASK')||
     l?.technicalReconciliationProof?.assessmentMode!==mode)
    bad('FALSE_GRADING_CLAIM');
  if(d?.digitallyCertifiedPupilAuthorship!==false||
     d?.actualClassroomParticipationClaimed!==false||
     d?.approvalFromTeacherAcademicOrExternalBodyRequired!==false||
     d?.sourcePixelParityClaimed!==false)
    bad('FALSE_HUMAN_OR_IMAGE_APPROVAL_CLAIM');
  if(d?.formalInternalV1Acceptance!==p?.status||
     d?.gradeRelease!=='NO_GO'||d?.originalStageDDeferredFieldsAreHistoricalAuthoringSnapshot!==true)
    bad('WRONG_FINAL_ACCEPTANCE_OR_HISTORIC_RECORD');
  if(d?.batch!==(n<=82?'D1':n<=114?'D2':'D3'))bad('BATCH');
  return e;
}
test('Every remaining Grade 1 original A-C checkpoint now carries exact-run Stage D proof, with a contiguous evidence-backed V1 acceptance cursor',()=>{
 const c=checkpoints(),progress=load('full-grade-progress.json'),
   ledger=load('stage-d-bulk-execution-ledger.json'),map=load('full-grade-practice-candidate-map.json');
 assert.equal(c.length,90);
 assert.equal(ledger.lastContiguousDVerifiedOrdinal,146);
 assert.equal(ledger.nextOrdinalRequiringDVerification,147);
 assert.equal(ledger.gradeRelease,'NO_GO');
 assert.equal(progress.release,'NO_GO');
 assert.equal(progress.newLessons.length,96);
 assert.deepEqual(progress.newLessons.filter(x=>x.status==='INTERNALLY_ACCEPTED').map(x=>x.ordinal),
   Array.from({length:progress.nextOrdinal-51},(_,i)=>i+51));
 assert.ok(progress.nextOrdinal>=51 && progress.nextOrdinal<=147,'valid cursor in original G1 scope');
 assert.equal(progress.nextOrdinal,progress.newLessons.find(x=>x.status!=='INTERNALLY_ACCEPTED')?.ordinal??147,'the cursor must be the first unaccepted ordinal');
 for(let i=0;i<90;i++){
   const n=i+57,violations=validate(c[i],progress.newLessons[n-51],
     ledger.lessons[n-51],map.records[n-51],n);
   assert.deepEqual(violations,[],n+': '+violations.join(','));
 }
 assert.equal(c.filter(x=>x.stageDClosure.correctWrongMalformedPersisted).length,89);
 assert.equal(c.filter(x=>x.stageDClosure.authoredOnlyInstructionsRendered).length,1);
 assert.equal(c.find(x=>x.ordinal===65).stageDClosure.correctWrongMalformedPersisted,false);
});
test('Tampered CI IDs, incomplete A-C tasks, invented digital authoring and fake final approval are independently rejected',()=>{
 const progress=load('full-grade-progress.json'),ledger=load('stage-d-bulk-execution-ledger.json'),
 map=load('full-grade-practice-candidate-map.json');
 const p=progress.newLessons[14],l=ledger.lessons[14],m=map.records[14];
 const original=load('lesson-65-v2-stage-a-d-checkpoint.json');
 for(const [field,value,expected] of [
   ['authenticatedRunId',0,'PROVENANCE'],
   ['correctWrongMalformedPersisted',true,'FALSE_GRADING_CLAIM'],
   ['digitallyCertifiedPupilAuthorship',true,'FALSE_HUMAN_OR_IMAGE_APPROVAL_CLAIM'],
   ['approvalFromTeacherAcademicOrExternalBodyRequired',true,'FALSE_HUMAN_OR_IMAGE_APPROVAL_CLAIM'],
   ['formalInternalV1Acceptance',p.status==='INTERNALLY_ACCEPTED'?'IN_PROGRESS':'INTERNALLY_ACCEPTED','WRONG_FINAL_ACCEPTANCE_OR_HISTORIC_RECORD']
 ]){
   const cp=structuredClone(original);cp.stageDClosure[field]=value;
   assert.ok(validate(cp,p,l,m,65).includes(expected),field);
 }
 const cp=structuredClone(original);cp.aToCClosure.openACDefects.push('real defect');
 assert.ok(validate(cp,p,l,m,65).includes('A_C_NOT_CLOSED'));
});
module.exports={validate};
