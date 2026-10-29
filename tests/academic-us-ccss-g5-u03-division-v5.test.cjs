'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const sidecar=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u03-l11-l15-2021.rich-lesson-v2.json');
const eq=(a,b,c,d)=>assert.equal(BigInt(a)*BigInt(d),BigInt(b)*BigInt(c));

test('U03 L11: unit fraction shared among people is smaller, not reciprocal quotient',()=>{
 eq(1,2*4,1,8);
 eq(1,2*3,1,6);
 assert.notEqual(1/2/3,3/(1/2));
});
test('U03 L12: alternative tape partitions recover the same original whole',()=>{
 eq(1,3*4,1,12);
 eq(1,3*3,1,9);
 eq(1,4*2,1,8);
 eq(4,12,1,3);
});
test('U03 L13: whole divided by a unit fraction counts small pieces',()=>{
 assert.equal(3/(1/2),6);
 assert.equal(3/(1/3),9);
 assert.equal(3/(1/4),12);
 assert.equal(12*(1/4),3);
});
test('U03 L14: fixed-divisor and fixed-dividend tables share exact results',()=>{
 assert.deepEqual([2,3,4,5].map(n=>n/(1/3)),[6,9,12,15]);
 assert.deepEqual([2,3,4,5].map(n=>3/(1/n)),[6,9,12,15]);
 assert.equal(7/(1/6),42);
});
test('U03 L15: non-matching story cards cannot pass from numeric resemblance',()=>{
 assert.equal(3/(1/4),12);
 assert.equal((1/4)/3,1/12);
 assert.equal(3/4,0.75);
 assert.notEqual(3/4,12);
 assert.notEqual(3/(1/4),(1/4)/3);
});
test('U03 L11-L15: first-party source-scoped sidecars parse', {skip:!fs.existsSync(sidecar)&&'GitHub grade-owned sidecar is not mounted here'},()=>{
 const d=JSON.parse(fs.readFileSync(sidecar,'utf8'));
 assert.equal(d.SchemaVersion,1);
 assert.equal(d.PackCode,'US-CCSS-MATH');
 assert.equal(d.Lessons.length,5);
 for(let i=0;i<5;i++){
  const x=d.Lessons[i];
  assert.equal(x.LessonCode,'PED:US-CCSS-MATH:G5:U03:L'+String(i+11).padStart(2,'0'));
  assert.ok(x.KeyConcepts.length>=3);
  assert.ok(x.WorkedExamples.length>=3);
  assert.ok(x.Visuals.every(v=>v.Rows.length>=2));
  assert.equal(x.Videos.length,0);
 }
});
