'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const r=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(r,p),'utf8'));
const ledger=read('artifacts/curriculum-delivery/US-CCSS-MATH/G1/full-grade-progress.json');
const rubric=read('artifacts/curriculum-delivery/US-CCSS-MATH/G1/lesson-65-learner-authored-story-review-rubric.json');
test('exact 20 new lessons have active work states but zero counterfeit approvals',()=>{
  const inprogress=ledger.newLessons.filter(x=>x.status==='IN_PROGRESS');
  assert.deepEqual(inprogress.map(x=>x.ordinal),Array.from({length:20},(_,i)=>i+51));
  assert.equal(ledger.newLessons.filter(x=>x.status==='INTERNALLY_ACCEPTED').length,0);
  assert.equal(ledger.release,'NO_GO');
  assert.equal(ledger.nextOrdinal,51);
});
test('Grade 1 ordinal 65 authored-story rubric remains teacher review required',()=>{
  assert.equal(rubric.lessonOrdinal,65);
  assert.equal(rubric.lessonCode,'PED:US-CCSS-MATH:G1:U03:L28');
  assert.equal(rubric.status,'TEACHER_REVIEW_REQUIRED_NOT_CERTIFIED');
  assert.equal(rubric.independentTeacherAcceptance,false);
  assert.equal(rubric.authenticatedStudentWorkVerified,false);
  assert.equal(rubric.submissionEvidence,null);
  assert.ok(rubric.teacherRubric.some(x=>x.criterion==='Authorship'));
  assert.equal(ledger.newLessons[65-51].status,'IN_PROGRESS');
  assert.equal(ledger.newLessons[65-51].evidence,null);
});
