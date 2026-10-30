'use strict';
// Grade-owned, source-informed independent fixture suite. AUTHOR ONLY in A-C.
// NOT a live Practice resolver, CI pass, authentication or Stage D claim.
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const file = p => fs.readFileSync(path.join(root, p), 'utf8');
const base='artifacts/curriculum-delivery/US-CCSS-MATH/G7/';
const cd=JSON.parse(file(base+'grade7-u01-l01-edulytiks-cooldown.json'));
const cursor=JSON.parse(file(base+'grade7-u01-l01-abc-cursor.json'));
const partial=file('src/Edulytics.Web/Views/Shared/_USCCSSG7ScaleConstructionFigures.cshtml');
const client=file('src/Edulytics.Web/wwwroot/curriculum/us-ccss-g7/geometry/u01-l01-activities.js');
const dist=(p,q)=>Math.hypot(p[0]-q[0],p[1]-q[1]);
const isFinitePoint=p=>Array.isArray(p)&&p.length===2&&p.every(Number.isFinite);
function sameScaledPolygon(a,b) {
  if(!Array.isArray(a)||!Array.isArray(b)||a.length!==b.length||a.length<3||
     !a.every(isFinitePoint)||!b.every(isFinitePoint))return false;
  const baseline=dist(a[0],a[1]),candidate=dist(b[0],b[1]);
  if(baseline<=0||candidate<=0)return false;
  const k=candidate/baseline;
  for(let i=0;i<a.length;i++)for(let j=i+1;j<a.length;j++){
    if(Math.abs(dist(b[i],b[j])-k*dist(a[i],a[j]))>1e-9)return false;
  }
  return true;
}
test('L01 3-4-5 triangle is geometrically defined; A scales, B does not',()=>{
  assert.equal(cd.lessonCode,'PED:US-CCSS-MATH:G7:U01:L01');
  assert.deepEqual(cd.sourceTriangle.legs,[3,4]);
  assert.equal(cd.sourceTriangle.hypotenuse,5);
  assert.equal(Math.hypot(3,4),5);
  assert.deepEqual([6/3,8/4,10/5],[2,2,2]);
  assert.deepEqual([6/3,8/4,9/5],[2,2,1.8]);
  assert.notEqual(9/5,2);
  assert.deepEqual(cd.sourceTriangle.halfScaleVertices,[[0,0],[1.5,0],[0,2]]);
  assert.equal(Math.hypot(1.5,2),2.5);
});
test('Independent coordinate scaling, rotation, translation and malformed oracle',()=>{
  const p=[[0,0],[3,0],[0,4]];
  assert.ok(sameScaledPolygon(p,[[0,0],[6,0],[0,8]]));
  assert.ok(sameScaledPolygon(p,[[0,0],[1.5,0],[0,2]]));
  assert.ok(sameScaledPolygon(p,[[10,20],[10,26],[2,20]]));
  assert.equal(sameScaledPolygon(p,[[0,0],[6,0],[0,4]]),false);
  assert.equal(sameScaledPolygon(p,[[0,0],[6,0],[0,8],[3,4]]),false);
  assert.equal(sameScaledPolygon(p,[[0,0],[6,0],['bad',8]]),false);
  assert.equal(sameScaledPolygon(p,[[0,0],[6,0],[NaN,8]]),false);
});
test('Ten original polygon cards are five real scaled-copy pairs',()=>{
  const match=client.match(/const cards = (\[[\s\S]*?\]);\s*const byId/);
  assert.ok(match,'Could not locate bounded card data');
  const cards=JSON.parse(match[1].replace(/'/g,'"'));
  assert.equal(cards.length,10);
  assert.equal(new Set(cards.map(x=>x[0])).size,10);
  const lookup=new Map(cards.map(x=>[x[0],x]));
  for(const [name,points,mate] of cards){
    const opposite=lookup.get(mate);
    assert.ok(opposite,'Missing card '+mate);
    assert.equal(opposite[2],name,'Pair must be reciprocal');
    assert.ok(sameScaledPolygon(points,opposite[1]),name+' not a genuine scaled copy');
  }
});
test('Student-facing original activity and distinct cooldown authored in G7 view only',()=>{
  assert.match(partial,/data-g7-u01-l01/);
  assert.match(partial,/data-g7-cards/);
  assert.match(partial,/data-g7-portrait-scale/);
  assert.match(partial,/data-g7-cooldown-check/);
  assert.match(partial,/original Edulytiks assessment, not publisher Cool-down/i);
  assert.match(partial,/teacher must assess/i);
  assert.match(partial,/u01-l01-activities\.js/);
  assert.equal(cd.publisherCooldown,'NOT_VERIFIED_GATED');
  assert.match(cd.deliveryStatus,/INVOCATION_UNVERIFIED/);
  assert.equal(cd.maxScore,12);
  assert.equal(cd.scoringRubric.reduce((s,c)=>s+c.points,0),12);
});
test('G7 original visual and construction prompts retain unique switch cases',()=>{
  for(const section of ['var altText =','var constructionPrompt =']){
    const start=partial.indexOf(section),end=partial.indexOf('};',start);
    assert.ok(start>=0&&end>start);
    const arms=[...partial.slice(start,end).matchAll(/"PED:US-CCSS-MATH:G7:U01:L\d\d"\s*=>/g)].map(m=>m[0]);
    assert.equal(arms.length,new Set(arms).size);
  }
});
test('A-C closure remains truthful: shared Practice still blocked',()=>{
  assert.equal(cursor.READY_FOR_STAGE_D,false);
  assert.match(cursor.C_STATUS,/BLOCKED/);
  assert.equal(cursor.testExecutionStatus,'NOT_RUN');
  assert.equal(cd.testExecutionStatus,'TEST_AUTHORED_NOT_RUN');
});
