'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const sidecar=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l07-l12-2021.rich-lesson-v2.json');
const audit=path.resolve(__dirname,'../artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l09-source-geometry-v10.json');
function cubes(parts){
  if(!Array.isArray(parts)||!parts.length)throw Error('missing parts');
  const keys=new Set();
  for(const p of parts){
    if(![p.x,p.y,p.w,p.h,p.d].every(Number.isSafeInteger))throw Error('malformed');
    if(p.x<0||p.y<0||p.w<=0||p.h<=0||p.d<=0)throw Error('invalid');
    if(p.unit!==parts[0].unit)throw Error('mixed units');
    for(let x=p.x;x<p.x+p.w;x++)for(let y=p.y;y<p.y+p.h;y++)for(let z=0;z<p.d;z++){
      const key=[x,y,z].join(':');
      if(keys.has(key))throw Error('overlap');
      keys.add(key);
    }
  }
  return [...keys].sort();
}
const part=(x,y,w,h,d,unit)=>({x,y,w,h,d,unit});
test('G5 U01:L09 first 2021 measured solid: equal 78 ft3 cuts and missing lengths',()=>{
  assert.equal(7-5,2);
  const a=[part(0,0,2,7,2,'ft'),part(2,0,5,5,2,'ft')];
  const b=[part(0,0,7,5,2,'ft'),part(0,5,2,2,2,'ft')];
  assert.deepEqual(cubes(a),cubes(b));assert.equal(cubes(a).length,78);
  assert.equal(2*7*2+5*5*2,78);assert.equal(7*5*2+2*2*2,78);
});
test('G5 U01:L09 second measured solid: equal 259 in3 cuts and overhang',()=>{
  assert.equal(9-5,4);assert.equal(5-2,3);
  const a=[part(0,0,5,5,7,'in'),part(5,2,4,3,7,'in')];
  const b=[part(0,0,5,2,7,'in'),part(0,2,9,3,7,'in')];
  assert.deepEqual(cubes(a),cubes(b));assert.equal(cubes(a).length,259);
  assert.equal(7*9*3+7*5*2,259);assert.equal(7*5*5+7*4*3,259);
});
test('G5 U01:L09 420 cubic-inch expressions represent identical positions',()=>{
  const a=[part(0,0,4,9,5,'in'),part(4,0,8,6,5,'in')];
  const b=[part(0,0,12,6,5,'in'),part(0,6,4,3,5,'in')];
  assert.deepEqual(cubes(a),cubes(b));assert.equal(cubes(a).length,420);
  assert.equal(5*8*6+5*4*9,420);assert.equal(5*4*3+5*12*6,420);
});
test('G5 U01:L09 grouped source expressions and powers-of-ten warmup',()=>{
  assert.equal((2*3)*4+(3*3)*2,42);assert.equal(5*6+3*4,42);
  assert.deepEqual([6*2,6*2*10,6*20*10,60*20*10],[12,120,1200,12000]);
});
test('G5 U01:L09 reject wrong volume, overlap, nonpositive, malformed and mixed units',()=>{
  assert.notEqual(2*7*2+5*5*2,75);
  assert.throws(()=>cubes([part(0,0,0,2,2,'ft')]),/invalid/);
  assert.throws(()=>cubes([part(0,0,2,2,'2','ft')]),/malformed/);
  const a=part(0,0,2,7,2,'ft');
  assert.throws(()=>cubes([a,a]),/overlap/);
  assert.throws(()=>cubes([a,part(2,0,1,1,1,'in')]),/mixed units/);
});
test('G5 U01:L09 source audit must remain unapproved', {skip:!fs.existsSync(audit)&&'exact GitHub audit file not mounted'},()=>{
  const x=JSON.parse(fs.readFileSync(audit,'utf8'));
  assert.equal(x.lessonCode,'PED:US-CCSS-MATH:G5:U01:L09');
  assert.equal(x.reviewClassification,'GRADE5_DEFECT_FIXED');
  assert.equal(x.studentTaskPdfChecked,false);
  assert.equal(x.gatedCooldownChecked,false);
  assert.equal(x.stageA,'NOT_COMPLETE');
});
test('G5 U01:L09 grade-only Rich V2 covers measured tasks', {skip:!fs.existsSync(sidecar)&&'GitHub branch sidecar not mounted'},()=>{
  const doc=JSON.parse(fs.readFileSync(sidecar,'utf8'));
  assert.equal(doc.SchemaVersion,1);assert.equal(doc.Lessons.length,6);
  const x=doc.Lessons.find(y=>y.LessonCode==='PED:US-CCSS-MATH:G5:U01:L09');
  assert.ok(x);
  assert.match(x.ExplanationParagraphs.join(' '),/259 cubic inches/);
  assert.match(x.ExplanationParagraphs.join(' '),/420 cubic inches/);
  assert.ok(x.Visuals.length>=3);assert.ok(x.WorkedExamples.length>=4);
  assert.ok(x.CommonMistakes.some(m=>/feet|inches/.test(m.Correction)));
});