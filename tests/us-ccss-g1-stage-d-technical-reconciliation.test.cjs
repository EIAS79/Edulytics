'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const {evaluateTechnical}=require('../tools/curriculum/us-ccss-g1-stage-d-technical-reconciliation.cjs');
const h=n=>n.toString(16).padStart(64,'0');
function fixture(){
 const manifest={lessons:Array.from({length:146},(_,i)=>{
    const ordinal=i+1;return {gradeOrdinal:ordinal,lessonCode:'L'+ordinal,
      sourceTeacherUrl:'https://example.org/L'+ordinal,
      teacherPreparationSha256:h(ordinal+2000)};})};
 const map={records:[],},source={rows:[]},progress={release:'NO_GO',newLessons:[]},
   artifact={scope:'G1_51_TO_146_ONLY',testedCheckoutSha:'a'.repeat(40),rows:[]},
   checkpoints={};
 for(let n=51;n<=146;n++){
   const mode=n===65?'SOURCE_AUTHORED_TASK': 'DIGITAL_PRACTICE';
   map.records.push({ordinal:n,lessonCode:'L'+n,
     assessmentMode:mode,candidateFamily:n===65?null:'family'+n,
     sourceTeacherUrl:'https://example.org/L'+n});
   source.rows.push({ordinal:n,lessonCode:'L'+n,
     original2021TeacherPreparationSha256:h(n+2000),
     original2021FullTeacherLessonHtmlSha256:h(n+3000),
     original2021StudentFacingHtmlSha256:h(n+4000),
     teacherActivityHeadings:['Activity'],studentFacingTaskExcerpts:['Task']});
   progress.newLessons.push({ordinal:n,lessonCode:'L'+n,
     status:n<=56?'INTERNALLY_ACCEPTED':'IN_PROGRESS'});
   const r={ordinal:n,lessonCode:'L'+n,assessmentMode:mode,
     unescapedAuthenticatedLearnerHtmlSha256:h(n),authenticStudentAuthorshipNotClaimed:true};
   if(n>=57)r.originalSourceBodyFragmentsVerified=n===57?4:n<=65?3:2;
   if(n===65)r.learnerTeachingRendered=true;
   else Object.assign(r,{actualFamily:'family'+n,
     correctAnswerSelfConsistentAndPersisted:true,
     malformedAnswerRejectedAndPersisted:true,
     wellFormedIncorrectAnswerRejectedAndPersisted:true,
     l51AuthenticatedTwoCardSourceBoardVerified:n===51});
   artifact.rows.push(r);
   if(n>=57)checkpoints[n]={ordinal:n,lessonCode:'L'+n,
     aToCClosure:{outcome:'READY_FOR_STAGE_D',openACDefects:[]}};
 }
 // Exactly 90 source-body records and 190 sampled original-task phrases.
 return {artifact,manifest,map,source,checkpoints,progress,
   checkoutSha:'a'.repeat(40),runId:1,artifactDigest:'b'.repeat(64)};
}
test('96 authenticated source-scoped rows are reconciled without teacher signoff',()=>{
 const x=evaluateTechnical(fixture());
 assert.equal(x.authenticatedHtmlVerified,96);
 assert.equal(x.digitalGradingPersisted,95);
 assert.equal(x.sourceOriginalSelectedTextVerified,90);
 assert.equal(x.selectedSourceTextFragmentsVerified,190);
 assert.equal(x.gradeRelease,'NO_GO');
 assert.equal(x.requiresHumanTeacherOrAcademicSignoff,false);
 assert.equal(x.rows.find(r=>r.ordinal===65).actualPracticeFamily,null);
});
test('source-only story cannot masquerade as auto-graded',()=>{
 const p=fixture();
 p.artifact.rows[14].correctAnswerSelfConsistentAndPersisted=true;
 assert.throws(()=>evaluateTechnical(p),/OPEN_AUTHORING_FALSE_AUTO_GRADING/);
});
test('missing source-only lesson 65 evidence must fail closed',()=>{
 const p=fixture();delete p.artifact.rows[14].originalSourceBodyFragmentsVerified;
 assert.throws(()=>evaluateTechnical(p),/AUTHENTICATED_SOURCE_TASK_TEXT_NOT_RECORDED/);
});
test('incorrect persisted grade, wrong family, invalid source and misleading class claim fail',()=>{
 for(const kind of ['grade','family','source','classroom']){
  const p=fixture();
  if(kind==='grade')p.artifact.rows[44].malformedAnswerRejectedAndPersisted=false;
  if(kind==='family')p.artifact.rows[44].actualFamily='unrelated';
  if(kind==='source')p.source.rows[44].original2021TeacherPreparationSha256='0'.repeat(64);
  if(kind==='classroom')p.artifact.rows[44].authenticStudentAuthorshipNotClaimed=false;
  assert.throws(()=>evaluateTechnical(p));
 }
});
test('wrong checkout SHA, missing lesson, and false release fail closed',()=>{
 for(const kind of ['checkout','omit','release']){
  const p=fixture();
  if(kind==='checkout')p.checkoutSha='c'.repeat(40);
  if(kind==='omit')p.artifact.rows.pop();
  if(kind==='release')p.progress.release='GO';
  assert.throws(()=>evaluateTechnical(p));
 }
});
