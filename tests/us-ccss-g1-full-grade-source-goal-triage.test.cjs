'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const get=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const audit=get('artifacts/curriculum-delivery/US-CCSS-MATH/G1/G1-L51-L146-SOURCE-GOAL-TRIAGE.json');
const manifest=get('artifacts/curriculum-delivery/US-CCSS-MATH/G1/grade-manifest.json');
test('96 exact new ordinals screened, historic first 50 excluded',()=>{
  assert.equal(audit.historical50Excluded,true);
  assert.equal(audit.records.length,96);
  assert.deepEqual(audit.records.map(x=>x.lessonCode),manifest.lessons.slice(50).map(x=>x.lessonCode));
  assert.equal(audit.records[0].ordinal,51);
  assert.equal(audit.records[95].ordinal,146);
});
test('triage never asserts new course/Practice academic acceptance',()=>{
  for(const r of audit.records){
    assert.equal(r.internalAcademicAcceptance,'NOT_CLAIMED');
    assert.equal(r.sourceSpecificPracticeReview,'PENDING_INDEPENDENT_PRACTICE_AND_GRADING_EVIDENCE');
    assert.ok(r.teacherPreparationSource.startsWith('https://im.kendallhunt.com/k5/teachers/grade-1/'));
    assert.ok(r.originalTeacherGoalExcerpt && r.originalTeacherGoalExcerpt.length>8);
  }
});
