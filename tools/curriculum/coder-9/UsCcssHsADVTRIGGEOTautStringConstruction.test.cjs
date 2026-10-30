'use strict';
// Source-specific, independent L05 pencil-string geometry and text invariants.
// A first-party text construction, not a claim of deployed/observed drawing UI.
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const pack=JSON.parse(fs.readFileSync(path.resolve(__dirname,'../../../src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-adv-trig-geo-phase29-v1.lesson-content-pack.json'),'utf8'));
const en=pack.lessons.find(x=>x.lessonCode.endsWith(':U01:L05')).translations.find(x=>x.cultureCode==='en');
const f1=[-3,0],f2=[3,0];
const dist=(a,b)=>Math.hypot(a[0]-b[0],a[1]-b[1]);
const focalSum=point=>dist(point,f1)+dist(point,f2);
const span=dist(f1,f2);
const closedLoop=point=>focalSum(point)+span;
test('L05 two-ended length 10 and closed-loop length 16 encode SAME ellipse for its 6-unit focal separation',()=>{
 assert.equal(span,6);
 const contour=[[5,0],[-5,0],[0,4],[0,-4],[3,3.2]];
 for(const p of contour){
  assert.ok(Math.abs(focalSum(p)-10)<1e-10,JSON.stringify(p)+' must be on ellipse');
  assert.ok(Math.abs(closedLoop(p)-16)<1e-10,'closed loop must include fixed return segment');
 }
 assert.match(en.stepByStepSolutions,/10-unit string/);
 assert.match(en.stepByStepSolutions,/CLOSED LOOP/);
 assert.match(en.stepByStepSolutions,/16 units/);
 assert.match(en.stepByStepSolutions,/distance sum 4/);
 assert.match(en.workedExamples,/physical length 16/);
});
test('L05 wrong physical loop length is impossible by the triangle inequality, not another accepted ellipse',()=>{
 const invalidLoopLength=10;
 const impliedFocalSum=invalidLoopLength-span;
 assert.equal(impliedFocalSum,4);
 assert.ok(impliedFocalSum<span);
 for(const p of [[0,0],[0,4],[5,0],[0,5],[3,3.2]]){
  assert.ok(focalSum(p)>=span,'focal-distance sum bounded by focus separation');
  assert.ok(closedLoop(p)>=2*span,'any taut loop around both foci and pencil is at least twice focal separation');
  assert.ok(Math.abs(closedLoop(p)-invalidLoopLength)>1e-9);
 }
 assert.match(en.commonMistakes,/16 units long, not 10/);
});
test('L05 correct focal definition retained; original ellipse vs first-party hyperbola remains distinguished',()=>{
 const p=[0,5];const d=focalSum(p);
 assert.ok(Math.abs(d-2*Math.sqrt(34))<1e-10);
 assert.ok(d>10);
 assert.match(en.stepByStepSolutions,/independently authored extension/);
 assert.match(en.stepByStepSolutions,/constant-DIFFERENCE hyperbola/);
 assert.equal(pack.lessons.find(x=>x.lessonCode.endsWith(':U01:L05')).sourceSha256,
 '4aca916e175d944d966894469d103b6b34b32ae9f032ae0e1b4e534bb189fd56');
});
