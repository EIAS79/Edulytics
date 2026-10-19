'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const ledger=read('artifacts/curriculum-delivery/US-CCSS-MATH/G1/full-grade-progress.json');
const rubric=read('artifacts/curriculum-delivery/US-CCSS-MATH/G1/lesson-65-learner-authored-story-review-rubric.json');

test('96 ordered new-work records, old 50 referenced without relabeling',()=>{
  assert.equal(ledger.newLessons.length,96);
  assert.equal(ledger.historical.count,50);
  assert.equal(ledger.historical.status,'HISTORICALLY_RELEASED_NO_RETEST');
  assert.deepEqual(ledger.newLessons.map(x=>x.ordinal),Array.from({length:96},(_,i)=>i+51));
  const recognized=new Set(['NOT_STARTED','IN_PROGRESS','INTERNALLY_ACCEPTED',
    'BLOCKED_SOURCE','READY_REUSE','CONTENT_REPAIR','PRACTICE_REPAIR','TESTED']);
  assert.ok(ledger.newLessons.every(x=>recognized.has(x.status)));
  const next=ledger.newLessons.find(x=>x.status!=='INTERNALLY_ACCEPTED')?.ordinal??147;
  assert.equal(ledger.nextOrdinal,next);
  if(next!==147)assert.equal(ledger.release,'NO_GO');
});

test('ordinal 65 learner-authored story cannot be auto-approved by a numeric generator',()=>{
  assert.equal(rubric.lessonOrdinal,65);
  assert.equal(rubric.lessonCode,'PED:US-CCSS-MATH:G1:U03:L28');
  assert.ok(rubric.teacherRubric.some(x=>x.criterion==='Authorship'));
  assert.ok(rubric.teacherRubric.some(x=>x.criterion==='Math accuracy'));
  const row=ledger.newLessons[65-51];
  // Real observed child work is not a pre-release academic requirement in V1.
  if(row.status==='INTERNALLY_ACCEPTED'){
    assert.ok(row.evidence?.originalAuthorshipTaskReviewed);
    assert.ok(row.evidence?.learnerAuthoredTaskRendered);
    assert.ok(row.evidence?.teacherReviewRubricAvailable);
    assert.ok(row.evidence?.actualTeacherObservationNotClaimed);
  }
  if(!rubric.authenticatedStudentWorkVerified)
    assert.equal(rubric.independentTeacherAcceptance,false);
});
