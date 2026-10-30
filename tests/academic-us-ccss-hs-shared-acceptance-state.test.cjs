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

test('Coder 8 M07 M08 M10 use bounded inverse eligibility, with reasoning teacher-scored',()=>{
  const data=JSON.parse(fs.readFileSync(p,'utf8'));
  const cases=new Map(data.cases.map(row=>[row.caseId,row]));
  for(const id of ['M07','M08','M10']){
    const row=cases.get(id);
    assert.equal(row.lessonCode,'PED:US-CCSS-MATH:HS:FOURTH:ADV-ALG-FUNC:U01:L01');
    assert.equal(row.kernelCandidate,'usccss.hs.matrix.inverse_eligibility');
    assert.match(row.technicalCheck,/EXECUTED_COMPONENT_TEST/);
    assert.equal(row.independentStatus, id==='M10'
      ? 'HUMAN_OR_INVALID_INPUT_REVIEW_REQUIRED'
      : 'KERNEL_EXISTS_FIXTURE_EQUIVALENCE_UNCERTIFIED');
    assert.equal(row.selectedPracticeStatus,'NOT_VERIFIED_STUDENT_ROUTE_DENIED');
  }
  for(const id of ['M05','M11']){
    const row=cases.get(id);
    assert.match(row.humanRubricPolicy,/TEACHER_REVIEW/);
    assert.match(row.humanRubricPolicy,/NOT_AUTOGRADED/);
    assert.equal(row.academicSignoff,'PENDING');
  }
  const matrixRegistry=JSON.parse(fs.readFileSync(require('node:path').resolve(
    __dirname,'../src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'),'utf8'));
  const family=matrixRegistry.families.find(row=>row.id==='usccss.hs.matrix.inverse_eligibility');
  assert.ok(family);
  assert.equal(family.status,'IntegrationPending');
  assert.equal(family.productionRouting,false);
  assert.equal(data.counts.independentExactFixtureCertified,0);
  assert.equal(data.counts.selectedStudentPracticeCertified,0);
});
