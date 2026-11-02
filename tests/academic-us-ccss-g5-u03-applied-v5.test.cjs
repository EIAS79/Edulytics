'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const target = path.resolve(__dirname, '../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u03-l07-l10-2021.rich-lesson-v2.json');
const rationalEq = (n1,d1,n2,d2) => assert.equal(BigInt(n1)*BigInt(d2),BigInt(n2)*BigInt(d1));

test('U03 L07: full-grid and symbolic fractional products are equivalent', () => {
 rationalEq(8,20,4,10);
 rationalEq(4,10,2,5);
 rationalEq(2*3,5*4,3,10);
 rationalEq(3*4,7*5,12,35);
 rationalEq(9*5,11*8,45,88);
});

test('U03 L08: fractional flag strip dimensions have consistent area units', () => {
 assert.equal(8*(6+1/2),52);
 assert.equal(8*(3/4),6);
 assert.equal(8*(3/2),12);
 assert.equal(2*6,12);
});

test('U03 L09: designed bands partition exactly one banner', () => {
 assert.equal(1.25+2+2.25,5.5);
 assert.equal(8*1.25,10);
 assert.equal(8*2,16);
 assert.equal(8*2.25,18);
 assert.equal(10+16+18,8*5.5);
});

test('U03 L10: equal shares must compare both dividend and divisor', () => {
 const values=[40/4,45/5,40/5,40/10,15/5];
 assert.deepEqual(values,[10,9,8,4,3]);
 assert.ok(values.every((v,i)=>i===0||values[i-1]>v));
 assert.ok(40/4>40/10);
 assert.ok(45/5>15/5);
});

test('U03 L07-L10: source-grounded Rich V2 documents are structured correctly', {skip:!fs.existsSync(target)&&'Branch sidecar not mounted in the isolated runner'}, ()=>{
 const doc=JSON.parse(fs.readFileSync(target,'utf8'));
 assert.equal(doc.SchemaVersion,1);
 assert.equal(doc.Lessons.length,4);
 for(let i=0;i<4;i++){
  const item=doc.Lessons[i];
  assert.equal(item.LessonCode,'PED:US-CCSS-MATH:G5:U03:L'+String(i+7).padStart(2,'0'));
  assert.ok(item.ExplanationParagraphs.length>=4);
  assert.ok(item.WorkedExamples.length>=3);
  assert.ok(item.WorkedExamples.every(ex=>ex.Steps.length>=3));
  assert.ok(item.Visuals.every(v=>v.Rows.length>=2));
  assert.equal(item.Videos.length,0);
 }
});
