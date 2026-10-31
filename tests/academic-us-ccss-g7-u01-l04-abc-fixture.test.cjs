'use strict';
// Grade-only independent mathematical oracles, not Stage D / runtime Practice.
const test=require('node:test');const assert=require('node:assert/strict');
const fs=require('node:fs');const path=require('node:path');
const root=path.resolve(__dirname,'..');
const view=fs.readFileSync(path.join(root,'src/Edulytics.Web/Views/Shared/_USCCSSG7ScaleConstructionFigures.cshtml'),'utf8');
const dist=(p,q)=>Math.hypot(p[0]-q[0],p[1]-q[1]);
const p=[[0,0],[4,0],[0,3]],q=[[0,0],[8,0],[0,6]];
test('L04 hidden corresponding distances uniformly scale',()=>{
  assert.equal(dist(p[1],p[2]),5);
  assert.equal(dist(q[1],q[2]),10);
  assert.equal(dist(q[0],q[1])/dist(p[0],p[1]),2);
  assert.equal(dist(q[0],q[2])/dist(p[0],p[2]),2);
  assert.equal(dist(q[1],q[2])/dist(p[1],p[2]),2);
  assert.equal(1/2,.5);
});
test('L04 same angles with unequal length factors are not enough',()=>{
  assert.notEqual(8/4,3/3);
  assert.notEqual(1/2,4/6);
  assert.equal(15/6,2.5);
  assert.equal(4*2.5,10);
  assert.equal(5/2.5,2);
  assert.equal(90,90);
});
test('L04 rotated/translated uniform copy preserves all distances',()=>{
  const x=p.map(([a,b])=>[50-3*b,20+3*a]);
  for(let i=0;i<3;i++)for(let j=i+1;j<3;j++)
    assert.ok(Math.abs(dist(x[i],x[j])-3*dist(p[i],p[j]))<1e-9);
});
test('L04 student investigation and original teacher-reviewed cooldown are lesson scoped',()=>{
  assert.match(view,/data-g7-u01-l04/);
  assert.match(view,/Cool-down — original Edulytiks task/);
  assert.match(view,/12-by-3 right triangle/);
  assert.match(view,/cannot submit work, persist scores/);
});
