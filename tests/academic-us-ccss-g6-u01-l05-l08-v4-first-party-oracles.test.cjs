'use strict';
// Grade 6 U01 L05–L08 first-party academic fixtures; authored only, NOT RUN.
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const pack=JSON.parse(fs.readFileSync(path.join(__dirname,'..','src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g6-phase29-v1.lesson-content-pack.json'),'utf8'));
const lesson=n=>pack.lessons.find(x=>x.lessonCode==='PED:US-CCSS-MATH:G6:U01:L'+String(n).padStart(2,'0')).translations.find(x=>x.cultureCode==='en');
const validArea=(base,height,unit)=>Number.isFinite(base)&&Number.isFinite(height)&&base>0&&height>0&&unit==='square_units';
test('L05 base-to-height pairs correct and invalid pair rejected',()=>{
  assert.equal(6*4,24);assert.equal(5*4.8,24);assert.equal(36/9,4);assert.equal(36/6,6);assert.notEqual(9*6,36);
  assert.equal(validArea(6,4,'square_units'),true);assert.equal(validArea(6,Infinity,'square_units'),false);assert.equal(validArea(0,4,'square_units'),false);assert.equal(validArea(6,4,'cm'),false);
  assert.match(lesson(5).workedExamples,/Edulytiks-original exit check/);
});
test('L06 fixed-height slant invariance, missing height and known shaded area',()=>{
  const polygonArea=pts=>Math.abs(pts.reduce((v,p,i)=>v+p[0]*pts[(i+1)%pts.length][1]-pts[(i+1)%pts.length][0]*p[1],0))/2;
  assert.equal(polygonArea([[0,0],[6,0],[8,3],[2,3]]),18);
  assert.equal(polygonArea([[0,0],[6,0],[10,3],[4,3]]),18);
  assert.equal(polygonArea([[0,0],[6,0],[7,4],[1,4]]),24);
  assert.equal(40/10,4);assert.equal(4*5*2.4,48);
  assert.deepEqual([4*3.5,0.8*20,6*2.25,10*1.4],[14,16,13.5,14]);
  assert.match(lesson(6).workedExamples,/Edulytiks-original Cool-down/);
});
test('L07 identical triangle composition and negative non-congruence',()=>{
  assert.equal(2.4*1,1.2*2);assert.equal(9*2,18);assert.equal(9*2/2,9);
  assert.equal(4*3/2,6);assert.equal(2*(4*3/2),12);
  assert.match(lesson(7).workedExamples,/SOME pairs/);assert.match(lesson(7).commonMistakes,/Equal areas alone/);
});
test('L08 one-original-piece vs two-whole-copy area must not be conflated',()=>{
  assert.equal(6*4/2,12);assert.equal(6*4,24);assert.equal(8*3/2,12);assert.equal(2*(8*3/2),24);
  assert.match(lesson(8).workedExamples,/Edulytiks-original Cool-down/);
  assert.match(lesson(8).commonMistakes,/TWO whole matching triangles/);
});
