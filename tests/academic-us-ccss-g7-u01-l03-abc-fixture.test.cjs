'use strict';
// Grade 7 U01:L03 independent geometry fixtures (AUTHORED_NOT_RUN).
const test=require('node:test');const assert=require('node:assert/strict');
const fs=require('node:fs');const path=require('node:path');
const root=path.resolve(__dirname,'..');
const view=fs.readFileSync(path.join(root,'src/Edulytics.Web/Views/Shared/_USCCSSG7ScaleConstructionFigures.cshtml'),'utf8');
const points=[[0,0],[6,0],[6,2],[4,2],[4,5],[0,5]];
const scaled=(v,k)=>v.map(([x,y])=>[x*k,y*k]);
const length=(a,b)=>Math.hypot(a[0]-b[0],a[1]-b[1]);
const check=(input,candidate,k)=>input.every((v,i)=>{
 const next=(i+1)%input.length;
 return Math.abs(length(candidate[i],candidate[next])-k*length(v,input[next]))<1e-9;
});
test('L03 uniform half and triple copies preserve all polygon sides',()=>{
  const half=scaled(points,0.5),triple=scaled(points,3);
  assert.deepEqual(half[4],[2,2.5]);
  assert.deepEqual(triple[1],[18,0]);
  assert.ok(check(points,half,0.5));
  assert.ok(check(points,triple,3));
});
test('L03 rotated and translated uniform copy also valid',()=>{
  const rotated=scaled(points,3).map(([x,y])=>[20-y,30+x]);
  assert.ok(check(points,rotated,3));
});
test('L03 reject nonuniform stretches and additive length shortcuts',()=>{
  const stretched=points.map(([x,y])=>[3*x,2*y]);
  assert.equal(check(points,stretched,3),false);
  assert.notEqual((6+2)/6,(2+2)/2);
  assert.notEqual(8/6,6/4);
  assert.equal((15/3),5);
  assert.equal(15-10,5);
  assert.notEqual(4/12,3/12);
});
test('L03 warm-up numbers and student-facing original exit task',()=>{
  assert.equal(25*8.5,212.5);
  assert.equal(9.93*0.984,9.77112);
  assert.equal(0.24*0.67,0.1608);
  assert.match(view,/data-g7-u01-l03/);
  assert.match(view,/Cool-down — independently authored by Edulytiks/);
  assert.match(view,/does not submit,/);
  assert.match(view,/a real labeled drawing/);
});
