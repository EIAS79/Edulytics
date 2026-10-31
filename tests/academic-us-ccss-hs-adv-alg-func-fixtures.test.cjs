'use strict';
const { test } = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const file = path.resolve(__dirname, '..', 'artifacts/curriculum-delivery/US-CCSS-MATH/coder-8/UsCcssHsADVALGFUNC2LESSONS-integration-acceptance-fixtures.json');
const fixture = JSON.parse(fs.readFileSync(file, 'utf8')); // Fail closed if actual repository fixture is missing.
const ids = ['M01','M02','M03','M04','M05','M06','M07','M08','M09','M10','M11',
  'L01','L02','L03','L04','L05','L06','L07','L08','L09','L10','L11','L12','L13','L14','L15'];
const eq = (a, b) => Math.abs(a - b) <= 1e-10;
const validRealLog = (base, argument) => Number.isFinite(base) && Number.isFinite(argument)
  && base > 0 && base !== 1 && argument > 0;
const mul = (a, b) => a.map(r => b[0].map((_, j) => r.reduce((s, n, i) => s + n * b[i][j], 0)));
const A = [[1,1,1],[2,-1,1],[1,2,-1]];
const N = [[-1,3,2],[3,-2,1],[5,-1,-3]];
const I = (a, n, k) => mul(a, n).every((r, i) => r.every((v, j) => v === (i === j ? k : 0)));
const oracle = {
  M01: () => mul(A, [[1],[2],[3]]).flat().join(',') === '6,3,2',
  M02: () => I([[2,1],[1,1]], [[1,-1],[-1,2]], 1) && mul([[1,-1],[-1,2]], [[5],[3]]).flat().join(',') === '2,1',
  M03: () => I(A, N, 7) && mul(N, [[6],[3],[2]]).flat().join(',') === '7,14,21',
  M04: () => mul(A, [[1],[2],[3]]).flat().every((n,i) => n === [6,3,2][i]),
  M05: () => [-3,0,5].every(t => (2-t)+t === 2 && 2*(2-t)+2*t === 4),
  M06: () => (5 - 2*2) === 1 && (2-2*1) === 0,
  M07: () => 1*4-2*2 === 0,
  M08: () => [[2,1],[1,1]][0].length !== [5,3,1].length,
  M09: () => [2,-1,1].reduce((s,n,i) => s + n*[1,2,3][i],0) === 3,
  M10: () => [[1,2,3],[4,5,6]].length !== 3,
  M11: () => [2,-1,1].map((n,i) => n-2*[1,1,1][i]).map((n,i) => n+2*[1,1,1][i]).join(',') === '2,-1,1',
  L01: () => 2**5 === 32,
  L02: () => eq(3**(-2), 1/9),
  L03: () => (1/2)**(-2) === 4,
  L04: () => Math.log(3)/Math.log(2) < Math.log(5)/Math.log(2),
  L05: () => Math.log(3)/Math.log(1/2) > Math.log(5)/Math.log(1/2),
  L06: () => Math.log(3)/Math.log(4) < Math.log(3)/Math.log(2)
    && eq(Math.log(1)/Math.log(2), Math.log(1)/Math.log(3))
    && Math.log(1/2)/Math.log(2) < Math.log(1/2)/Math.log(3)
    && Math.log(9)/Math.log(2) > Math.log(9)/Math.log(3),
  L07: () => 5>3 && eq(Math.log(5-3)/Math.log(2),1),
  L08: () => !validRealLog(1, 5) && validRealLog(2, 5),
  L09: () => !validRealLog(2, -2) && validRealLog(2, 1/9),
  L10: () => !validRealLog(2, 0) && validRealLog(2, 1),
  L11: () => !validRealLog(-2, 8) && !validRealLog(0, 8) && validRealLog(2, 8),
  L12: () => !eq(Math.log(6)/Math.log(2), Math.log(2)/Math.log(2)+Math.log(4)/Math.log(2)),
  L13: () => [0.5,2,7].every(x => [0.5,3,9].every(y => eq(Math.log(x*y)/Math.log(2), Math.log(x)/Math.log(2)+Math.log(y)/Math.log(2)))),
  L14: () => eq(2**(Math.log(7)/Math.log(2)),7),
  L15: () => eq(2**(Math.log(7)/Math.log(2)),7),
};
const byId = new Map((fixture?.cases || []).map(x => [x.id,x]));
for (const id of ids) test(`${id}: independent exact/domain oracle and optional fixture metadata`, () => {
  assert.ok(oracle[id](), `${id} mathematical expectation failed`);
  const row = byId.get(id);
  assert.ok(row, `${id} missing from 26-case manifest`);
  for (const field of ['expectedCanonical','acceptedEquivalent','rejectWrongOrMalformed','rejectMalformed','boundaryOracle'])
    assert.ok(typeof row[field] === 'string' && row[field].trim(), `${id}: ${field} missing`);
  assert.notEqual(row.expectedCanonical, row.rejectWrongOrMalformed);
  assert.notEqual(row.expectedCanonical, row.rejectMalformed);
  assert.notEqual(row.acceptedEquivalent, row.rejectWrongOrMalformed);
  assert.notEqual(row.acceptedEquivalent, row.rejectMalformed);
  assert.notEqual(row.rejectWrongOrMalformed, row.rejectMalformed);
  assert.equal(row.lessonCode.endsWith(id[0] === 'M' ? 'L01' : 'L02'), true);
});
test('manifest and source reasoning rubrics are fail-closed', () => {
  assert.deepEqual([...byId.keys()], ids);
  assert.equal(fixture.status, 'PROPOSED_EXPECTATIONS_NOT_RUNTIME_VERIFICATION');
  const l06 = byId.get('L06');
  assert.equal(l06.gradingPolicy, 'ORDER_RELATION_AND_RUBRIC');
  for (const phrase of ['log_4(3)<log_2(3)', 'false', 'x=1', '0<x<1', 'x>1'])
    assert.ok(l06.expectedCanonical.includes(phrase), 'L06 missing: ' + phrase);
  const rubricFile = path.resolve(__dirname, '..', 'artifacts/curriculum-delivery/US-CCSS-MATH/coder-8/UsCcssHsADVALGFUNC2LESSONS-human-scoring-rubrics.v1.json');
  const rubrics = JSON.parse(fs.readFileSync(rubricFile, 'utf8'));
  assert.deepEqual(rubrics.rubrics.map(x=>x.caseId), ['L04','L05','L06','L13','M05','M11']);
  for (const r of rubrics.rubrics) {
    assert.equal(r.gradingPolicy, byId.get(r.caseId).gradingPolicy);
    assert.equal(r.requiresHumanReview,true);
    assert.equal(r.machineAutoGrade,false);
    assert.equal(r.criteria.reduce((s,x)=>s+x.points,0),4);
  }
  for(const flag of Object.values(rubrics.releaseGates)) assert.equal(flag,false);
});
test('original painting task: exact inverse, receipt checks and negative-price boundary', () => {
  const paint = [[2,0,1],[1,4,0],[0,2,1]];
  const numerator = [[4,2,-4],[-1,2,1],[2,-4,8]];
  assert.ok(I(paint, numerator, 10));
  assert.deepEqual(mul(paint, [[12],[2],[6]]).flat(), [30,20,10]);
  assert.deepEqual(mul(paint, [[-4],[6],[38]]).flat(), [30,20,50]);
  assert.notEqual(2*12+2,30); // Wrong first-receipt item.
  if (fixture) {
    const scenario = fixture.sourceBoundScenarios.find(x=>x.scenarioId==='MVP-A2-10.6H-PAINT-SYSTEM');
    assert.ok(scenario);
    assert.deepEqual(scenario.representation.matrix, paint);
    assert.deepEqual(scenario.expected.prices, [12,2,6]);
    assert.equal(scenario.expected.determinant,10);
  }
});
test('original log number-line task: exact powers and changing cross-base order', () => {
  const args=[1/9,1/3,1,3,9], powers=[-2,-1,0,1,2];
  args.forEach((v,i)=>assert.ok(eq(3**powers[i],v)));
  assert.ok(Math.log(5)/Math.log(3)>1 && Math.log(5)/Math.log(3)<2);
  assert.ok(Math.log(9)/Math.log(2)>Math.log(9)/Math.log(3));
  assert.ok(Math.log(1/9)/Math.log(2)<Math.log(1/9)/Math.log(3));
  assert.ok(eq(Math.log(1)/Math.log(2),Math.log(1)/Math.log(3)));
  if (fixture) {
    const scenario = fixture.sourceBoundScenarios.find(x=>x.scenarioId==='MVP-M3-2.1-LOG-NUMBER-LINE');
    assert.ok(scenario);
    assert.deepEqual(scenario.representation.numberLineExponents,powers);
    assert.equal(scenario.expected.crossBaseUniversalClaim,false);
  }
});

test('log-domain negative controls use the actual invalid base, not an invalid argument surrogate', () => {
  assert.equal(validRealLog(1, 5), false);
  assert.equal(validRealLog(0, 8), false);
  assert.equal(validRealLog(-2, 8), false);
  assert.equal(validRealLog(2, -2), false);
  assert.equal(validRealLog(2, 0), false);
  assert.equal(validRealLog(0.5, 4), true);
  assert.equal(validRealLog(2, 8), true);
});

test('fixture binding needs two source scenarios and all six human review dispositions', () => {
  assert.deepEqual(fixture.sourceBoundScenarios.map(x=>x.scenarioId),
    ['MVP-A2-10.6H-PAINT-SYSTEM','MVP-M3-2.1-LOG-NUMBER-LINE']);
  const studentLog = fixture.sourceBoundScenarios[1];
  assert.equal(studentLog.sourceInspection.includes('DIRECTLY_VIEWED'), true);
  assert.equal(studentLog.expected.crossBaseUniversalClaim, false);
  const rubricFile = path.resolve(__dirname, '..', 'artifacts/curriculum-delivery/US-CCSS-MATH/coder-8/UsCcssHsADVALGFUNC2LESSONS-human-scoring-rubrics.v1.json');
  const rubrics = JSON.parse(fs.readFileSync(rubricFile, 'utf8'));
  for (const id of ['M05','M11','L04','L05','L06','L13']) {
    const rubric = rubrics.rubrics.find(x=>x.caseId===id);
    const caseRow = byId.get(id);
    assert.ok(rubric && caseRow, 'Missing linked rubric/case: '+id);
    assert.equal(rubric.gradingPolicy, caseRow.gradingPolicy);
    assert.equal(rubric.requiresHumanReview,true);
    assert.equal(rubric.machineAutoGrade,false);
  }
});

test('MVP 10.6H source task six: 3x3 inverse, identity and original system', () => {
  const A = [[1,2,2],[2,1,3],[1,2,1]], N=[[-5,2,4],[1,-1,1],[3,0,-3]];
  const b=[[16],[17],[13]], x=[[2],[4],[3]];
  const identity=[[1,0,0],[0,1,0],[0,0,1]];
  assert.ok(I(A,N,3), 'A times inverse numerator must equal 3I');
  assert.deepEqual(mul(identity,A), A, 'I3 A=A');
  assert.deepEqual(mul(A,x), b, 'all three source equations must hold');
  assert.deepEqual(mul(N,b).flat(), [6,12,9], 'numerator inverse must return 3x');
  assert.notDeepEqual(mul(A,[[2],[3],[4]]),b, 'swapped variables must fail');
  const source=fixture.sourceBoundScenarios.find(s=>s.scenarioId==='MVP-A2-10.6H-PAINT-SYSTEM');
  assert.ok(source,'source-bound 10.6H case must be present');
  assert.equal(source.publisherPdfBinaries.teacher.sha256,
    '87f0872674e13e3f53e4ccc81004cb150eb7a64539879efa9f04f8cd58349e55');
  assert.deepEqual(source.publisherPdfBinaries.student.taskPages,[34,35,36]);
  assert.deepEqual(source.publisherPdfBinaries.teacher.notesPages,[52,53]);
  assert.equal(source.sourceTaskSequence.length,4);
  assert.deepEqual(source.sourceTaskSix.matrix,A);
  assert.deepEqual(source.sourceTaskSix.solution,[2,4,3]);
  assert.deepEqual(source.sourceTaskSix.rightHandSide,b.flat());
  assert.equal(source.sourceTaskSix.determinant,3);
  assert.deepEqual(source.sourceTaskSix.inverseNumerator,N);
});
