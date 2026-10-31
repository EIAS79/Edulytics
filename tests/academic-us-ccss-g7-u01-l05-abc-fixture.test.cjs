'use strict';
// G7 U01:L05 independent mathematical checkpoints, tests authored not run.
const test=require('node:test');const assert=require('node:assert/strict');
const fs=require('node:fs');const path=require('node:path');
const view=fs.readFileSync(path.join(__dirname,'..','src/Edulytics.Web/Views/Shared/_USCCSSG7ScaleConstructionFigures.cshtml'),'utf8');
const cards=[
  ['A',[2,3],[4,6],2],['B',[4,6],[2,3],.5],
  ['C',[4,2],[4,2],1],['D',[4,2],[6,3],1.5],
  ['E',[6,3],[4,2],2/3],['F',[2,3],[6,9],3],
  ['G',[6,9],[2,3],1/3],['H',[4,2],[4,2],1]
];
test('L05 eight first-party cards have valid uniform factors',()=>{
  for(const [id,p,q,k] of cards){
    assert.ok(Math.abs(q[0]/p[0]-k)<1e-12,id);
    assert.ok(Math.abs(q[1]/p[1]-k)<1e-12,id);
    assert.ok(k>0,id);
  }
});
test('L05 reciprocal pairs and all size classes',()=>{
  assert.ok(Math.abs(cards[0][3]*cards[1][3]-1)<1e-12);
  assert.ok(Math.abs(cards[3][3]*cards[4][3]-1)<1e-12);
  assert.ok(Math.abs(cards[5][3]*cards[6][3]-1)<1e-12);
  assert.equal(cards[2][3],1);
  assert.equal(cards[7][3],1);
  assert.equal(Math.pow(.5,4),1/16);
  assert.equal(.75*4/3,1);
});
test('L05 source number talk and original puzzle scale',()=>{
  assert.deepEqual([176/16,8/16,1/16,1/(1/5),1/(2/5)],[11,.5,.0625,5,2.5]);
  assert.equal((6*.5)*(4*.5),6);
  assert.equal(6*4,24);
  assert.equal(6/24,1/4);
  assert.deepEqual([8*.75,4*.75],[6,3]);
});
test('L05 learner task visible in grade-only view but not falsely persisted',()=>{
  assert.match(view,/data-g7-u01-l05/);
  assert.match(view,/5.2 Sort and defend original Edulytiks scale cards/);
  assert.match(view,/Cool-down — Edulytiks-original/);
  assert.match(view,/No student work\s+is automatically scored, submitted or persisted/);
});
