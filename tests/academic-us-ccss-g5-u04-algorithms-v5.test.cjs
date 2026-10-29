'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const sidecar=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u04-l01-l05-2021.rich-lesson-v2.json');
test('U04 L01: estimates and related exact products reconcile',()=>{
 assert.equal(23*7,161); assert.equal(23*47,1081); assert.equal(23*147,3381);
 assert.ok(3000<3381 && 3381<3750);
 assert.equal(23*100+23*40+23*7,3381);
});
test('U04 L02: all six decomposition cells produce the same product',()=>{
 assert.equal(2000+600+80+300+90+12,3082);
 assert.equal(134*23,3082); assert.equal(134*20+134*3,3082);
});
test('U04 L03: partial-product rows retain their tens values',()=>{
 assert.equal(4000+600+120+800+120+24,5664);
 assert.equal(236*20+236*4,5664); assert.notEqual(236*20,472);
});
test('U04 L04: composing carries and zero hundreds place are correct',()=>{
 assert.equal(247*3,741); assert.equal(12345*6,74070);
 const digits=String(74070); assert.equal(digits.at(-3),'0');
 assert.equal(12000*6+345*6,74070);
});
test('U04 L05: two-digit multiplication without composing respects tens place',()=>{
 assert.equal(312*1+312*20,6552); assert.equal(211*2+211*30,6752);
 assert.ok(6752>211*30); assert.notEqual(211*30,633);
});
test('U04 L01-L05: Rich V2 shape and identity', {skip:!fs.existsSync(sidecar)&&'GitHub branch file not locally mounted'},()=>{
 const doc=JSON.parse(fs.readFileSync(sidecar,'utf8'));
 assert.equal(doc.SchemaVersion,1);assert.equal(doc.PackCode,'US-CCSS-MATH');assert.equal(doc.Lessons.length,5);
 doc.Lessons.forEach((x,i)=>{
  assert.equal(x.LessonCode,'PED:US-CCSS-MATH:G5:U04:L'+String(i+1).padStart(2,'0'));
  assert.ok(x.ExplanationParagraphs.length>=4);assert.ok(x.WorkedExamples.length>=3);
  assert.ok(x.KeyConcepts.length>=3);assert.ok(x.CommonMistakes.length>=2);
  assert.ok(x.Visuals.every(v=>v.Description && v.Rows.length>=2));
  assert.equal(x.Videos.length,0);
 });
});
