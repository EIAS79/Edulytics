'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const p=path.resolve(__dirname,'../artifacts/curriculum-delivery/US-CCSS-MATH/shared-hs-integration/pr600-fixture-acceptance-state.v1.json');

test('all 54 Coder 8 and 9 proposed source cases have separate independent and selected runtime states',()=>{
  const data=JSON.parse(fs.readFileSync(p,'utf8'));
  assert.equal(data.decision,'NO_GO');
  assert.equal(data.cases.length,54);
  assert.equal(data.counts.fixtureRows,54);
  assert.equal(data.cases.filter(x=>x.course==='ADV-ALG-FUNC').length,26);
  assert.equal(data.cases.filter(x=>x.course==='ADV-TRIG-GEO').length,28);
  assert.equal(new Set(data.cases.map(x=>x.course+':'+x.caseId)).size,54);
  assert.equal(data.fixtures.length,2);
  for(const source of data.fixtures){
    assert.match(source.sourcePrHead,/^[a-f0-9]{40}$/);
    assert.match(source.sourceBlobSha,/^[a-f0-9]{40}$/);
  }
  for(const row of data.cases){
    assert.match(row.lessonCode,/^PED:US-CCSS-MATH:HS:FOURTH:/);
    assert.equal(row.selectedPracticeStatus,'NOT_VERIFIED_STUDENT_ROUTE_DENIED');
    assert.equal(row.gradingFeedbackPersistence,'NOT_VERIFIED');
    assert.equal(row.academicSignoff,'PENDING');
    assert.ok(row.independentStatus==='KERNEL_EXISTS_FIXTURE_EQUIVALENCE_UNCERTIFIED'||
      row.independentStatus==='HUMAN_OR_INVALID_INPUT_REVIEW_REQUIRED');
  }
  assert.equal(data.counts.independentExactFixtureCertified,0);
  assert.equal(data.counts.selectedStudentPracticeCertified,0);
  assert.equal(data.counts.persistedTeacherFeedbackCertified,0);
  assert.equal(data.counts.academicLessonApprovals,0);
});

test('all seven pending source lesson identities are present and nobody is promoted',()=>{
  const data=JSON.parse(fs.readFileSync(p,'utf8'));
  const ids=new Set(data.cases.map(x=>x.lessonCode));
  const expected=[
    'PED:US-CCSS-MATH:HS:FOURTH:ADV-ALG-FUNC:U01:L01',
    'PED:US-CCSS-MATH:HS:FOURTH:ADV-ALG-FUNC:U01:L02',
    ...[1,2,3,4,5].map(n=>'PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L'+String(n).padStart(2,'0'))
  ];
  assert.deepEqual([...ids].sort(),expected.sort());
  assert.equal(data.cases.filter(x=>x.course==='ADV-ALG-FUNC').length,26);
  assert.equal(data.cases.filter(x=>x.course==='ADV-TRIG-GEO').length,28);
});
