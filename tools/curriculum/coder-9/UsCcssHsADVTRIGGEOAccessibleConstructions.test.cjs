'use strict';
// Course-owned functional mathematical oracles for accessible source-task constructions.
// Does not claim authenticated rendering, actual Practice generation or academic approval.
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const pack=JSON.parse(fs.readFileSync(path.join(__dirname,'../../../src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-adv-trig-geo-phase29-v1.lesson-content-pack.json'),'utf8'));
const text=i=>pack.lessons[i].translations.find(t=>t.cultureCode==='en').stepByStepSolutions;
const near=(a,b)=>assert.ok(Math.abs(a-b)<1e-10, a+' differs from '+b);
const distance=(x,y,u,v)=>Math.hypot(x-u,y-v);

test('L01 explicit coordinate construction preserves the correct quadrant signs, including non-acute points',()=>{
  assert.match(text(0),/Accessible unit-circle construction/);
  const p=[Math.sqrt(3)/2,1/2], q=[-p[0],p[1]],r=[-p[0],-p[1]],t=[p[0],-p[1]];
  for(const [point,angle] of [[p,Math.PI/6],[q,5*Math.PI/6],[r,7*Math.PI/6],[t,11*Math.PI/6]]){
    near(Math.cos(angle),point[0]); near(Math.sin(angle),point[1]);
    near(point[0]**2+point[1]**2,1);
  }
  assert.ok(p[0]>0 && q[0]<0 && r[1]<0 && t[0]>0);
});

test('L02 graph checkpoints and location-race classify unique, ambiguous, and incompatible clues',()=>{
  const s=text(1);
  assert.match(s,/Source-inspired inverse graph and location race/);
  const angles=[-Math.PI/2,0,Math.PI/2];
  for(let i=1;i<angles.length;i++) assert.ok(Math.sin(angles[i])>Math.sin(angles[i-1]));
  const cosAngles=[0,Math.PI/2,Math.PI];
  for(let i=1;i<cosAngles.length;i++) assert.ok(Math.cos(cosAngles[i])<Math.cos(cosAngles[i-1]));
  near(Math.asin(0.5),Math.PI/6);
  const candidates=[Math.PI/6,5*Math.PI/6].filter(a=>Math.abs(Math.sin(a)-0.5)<1e-10);
  assert.equal(candidates.length,2);
  assert.equal(candidates.filter(a=>Math.cos(a)<0).length,1);
  assert.equal(candidates.filter(a=>Math.sin(a)<0).length,0);
});

test('L03 accessible graph intersections include all four two-cycle roots with no false endpoint',()=>{
  assert.match(text(2),/Accessible graph\/technology investigation/);
  const roots=[Math.PI/6,Math.PI/3,7*Math.PI/6,4*Math.PI/3];
  assert.equal(new Set(roots.map(a=>a.toFixed(12))).size,4);
  for(const a of roots){assert.ok(a>=0 && a<2*Math.PI);near(Math.sin(2*a),Math.sqrt(3)/2);}
  assert.ok(Math.abs(Math.sin(0)-Math.sqrt(3)/2)>0.5);
  assert.ok(Math.abs(Math.sin(4*Math.PI)-Math.sqrt(3)/2)>0.5);
  for(const t of [1,5]) near(10+4*Math.sin(Math.PI*t/6),12);
});

test('L04 all-height symbolic cross sections agree and require correct cone orientation',()=>{
  assert.match(text(3),/Every-height cross-section construction/);
  const r=3;
  for(const z of [0,0.1,1,1.5,2.5,3]){
    near(Math.PI*(r*r-z*z),Math.PI*r*r-Math.PI*z*z);
  }
  near(Math.PI*(9-1.5**2),27*Math.PI/4);
  assert.notEqual(Math.PI*(r*r-1**2),Math.PI*(r*r-(r-1)**2));
});

test('L05 accessible focus sums select valid ellipse points and reject invalid string positions',()=>{
  const s=text(4);
  assert.match(s,/Accessible ellipse string\/foci construction/);
  const sum=(x,y)=>distance(x,y,-3,0)+distance(x,y,3,0);
  near(sum(5,0),10);near(sum(0,4),10);near(sum(0,5),2*Math.sqrt(34));
  assert.ok(sum(0,5)>10);
  near(6*6-3*3,27);near(5*5-4*4,9);
  near(distance(0,5,0,-3)+distance(0,5,0,3),10);
  assert.match(s,/independently authored extension/);
});
