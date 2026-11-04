'use strict';
// G7 U01:L08 independent route/distance/time oracles. AUTHORED_NOT_RUN.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const view=fs.readFileSync(path.join(__dirname,'..','src/Edulytics.Web/Views/Shared/_USCCSSG7ScaleConstructionFigures.cshtml'),'utf8');
const dist=(a,b)=>Math.hypot(a[0]-b[0],a[1]-b[1]);
test('source train versus car rates compare like units',()=>{
  assert.equal(243/4,60.75);
  assert.equal(65*4,260);
  assert.ok(65>60.75);
});
test('first-party road path versus direct path: source-aligned speed',()=>{
  const A=[0,0],P=[12,0],B=[12,5];
  const pathGrid=dist(A,P)+dist(P,B);
  const directGrid=dist(A,B);
  assert.equal(pathGrid,17);
  assert.equal(directGrid,13);
  const roadMiles=pathGrid*.5,airMiles=directGrid*.5;
  assert.equal(roadMiles,8.5);
  assert.equal(airMiles,6.5);
  assert.equal(roadMiles/(8/60),63.75);
  assert.equal(airMiles/(8/60),48.75);
  assert.ok(63.75>55);
});
test('cyclic map key and original Cool-down conversion',()=>{
  assert.equal(12.5*4,50);
  assert.equal((50/15)*60,200);
  assert.equal(3*4,12);
  assert.equal(7*4,28);
  assert.equal(5*4,20);
  assert.equal(28/(24/60),70);
  assert.equal(dist([0,0],[3,4]),5);
});
test('L08 activity and original assessment are grade-scoped',()=>{
  assert.match(view,/data-g7-u01-l08/);
  assert.match(view,/road is 17 grid units/);
  assert.match(view,/original Edulytiks map assessment/);
  assert.match(view,/does not record work/);
});

test('L08 selected exact mile-route Practice family authored NOT_RUN',()=>{
 const root=path.join(__dirname,'..');
 const contracts=fs.readFileSync(path.join(root,
  'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 const cursor=JSON.parse(fs.readFileSync(path.join(root,
  'artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u01-l08-abc-cursor.json'),'utf8'));
 assert.match(contracts,/usccss\.g7\.u01\.l08\.map_route_rate_time/);
 assert.equal(cursor.READY_FOR_STAGE_D,true);
 assert.equal(cursor.testExecution,'TEST_AUTHORED_NOT_RUN');
});
