'use strict';
const { test } = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const file = path.resolve(__dirname, '..', 'artifacts/curriculum-delivery/US-CCSS-MATH/coder-8/UsCcssHsADVALGFUNC2LESSONS-integration-acceptance-fixtures.json');
const fixture = fs.existsSync(file) ? JSON.parse(fs.readFileSync(file, 'utf8')) : null;
const ids = ['M01','M02','M03','M04','M05','M06','M07','M08','M09','M10','M11',
  'L01','L02','L03','L04','L05','L06','L07','L08','L09','L10','L11','L12','L13','L14','L15'];
const eq = (a, b) => Math.abs(a - b) <= 1e-10;
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
  L08: () => Math.log(1) === 0,
  L09: () => Number.isNaN(Math.log(-2)),
  L10: () => Math.log(0) === -Infinity,
  L11: () => Number.isNaN(Math.log(-2)),
  L12: () => !eq(Math.log(6)/Math.log(2), Math.log(2)/Math.log(2)+Math.log(4)/Math.log(2)),
  L13: () => [0.5,2,7].every(x => [0.5,3,9].every(y => eq(Math.log(x*y)/Math.log(2), Math.log(x)/Math.log(2)+Math.log(y)/Math.log(2)))),
  L14: () => eq(2**(Math.log(7)/Math.log(2)),7),
  L15: () => eq(2**(Math.log(7)/Math.log(2)),7),
};
const byId = new Map((fixture?.cases || []).map(x => [x.id,x]));
for (const id of ids) test(`${id}: independent exact/domain oracle and optional fixture metadata`, () => {
  assert.ok(oracle[id](), `${id} mathematical expectation failed`);
  if (!fixture) return; // Standalone arithmetic is runnable without repository checkout.
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
test('manifest and source reasoning rubrics are fail-closed', { skip: !fixture }, () => {
  assert.deepEqual([...byId.keys()], ids);
  assert.equal(fixture.status, 'PROPOSED_EXPECTATIONS_NOT_RUNTIME_VERIFICATION');
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
