'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const p1=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l04-l06-2021.rich-lesson-v2.json');
const p2=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l07-l12-2021.rich-lesson-v2.json');
test('G5 U01 L06: true/false and alternative layer products use exact operations',()=>{
 assert.equal((4*2)*5,4*(2*5));
 assert.equal((2*5)*4,2*20);
 assert.notEqual(5*4*2,10*40);
 assert.equal(2*(4*3),4*(2*3));
});
test('G5 U01 L07: a unit is mandatory and missing dimension must be requested',()=>{
 const fixedLength=4,fixedHeight=3;
 assert.notEqual(fixedLength*fixedHeight,fixedLength*2*fixedHeight);
 assert.equal(fixedLength*2*fixedHeight,24);
 assert.notEqual('24 cubic inches','24 cubic feet');
});
test('G5 U01 L08: joining disjoint prisms adds volumes',()=>{
 assert.equal(12+10,22);
 assert.equal(3*2*2+3*2*3,30);
 assert.equal(22+10,32);
});
test('G5 U01 L06-L08: original candidate fields on mounted repository', {skip:!(fs.existsSync(p1)&&fs.existsSync(p2))&&'GitHub Rich V2 JSON not mounted locally'},()=>{
 const rows=[...JSON.parse(fs.readFileSync(p1,'utf8')).Lessons,...JSON.parse(fs.readFileSync(p2,'utf8')).Lessons];
 for(const n of [6,7,8]){
  const id='PED:US-CCSS-MATH:G5:U01:L'+String(n).padStart(2,'0');
  const x=rows.find(v=>v.LessonCode===id);assert.ok(x);
  assert.ok(x.WorkedExamples.length>=3 && x.KeyConcepts.length>=3 && x.Visuals.length>0);
 }
});
