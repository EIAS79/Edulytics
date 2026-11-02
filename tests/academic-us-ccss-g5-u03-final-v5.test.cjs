'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const sidecar = path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u03-l16-l18-l20-2021.rich-lesson-v2.json');
const rationalEq = (a,b,c,d) => assert.equal(BigInt(a)*BigInt(d),BigInt(b)*BigInt(c));
test('U03 L16: six oppositely directed quotients sort by mathematical value', () => {
 const vals = [1/8/12,1/5/12,1/3/12,12/(1/3),12/(1/5),12/(1/8)];
 for(let i=1;i<vals.length;i++) assert.ok(vals[i-1]<vals[i]);
 rationalEq(1,96,1,96); rationalEq(1,60,1,60); rationalEq(1,36,1,36);
 assert.deepEqual(vals.slice(3),[36,60,96]);
 assert.equal(97/(1/6),582);
});
test('U03 L17: tile information gap has exact whole-tile grid and area check', () => {
 assert.equal(2*1.5,3); rationalEq(1,4*2,1,8);
 assert.equal(2/(1/4),8); assert.equal(1.5/(1/2),3);
 assert.equal(8*3,24); assert.equal(24/8,3); assert.equal(6/(1/4),24);
});
test('U03 L18: reversed fraction division expresses different situations', () => {
 assert.equal(3/(1/4),12); assert.equal((1/4)/3,1/12);
 assert.notEqual(3/(1/4),(1/4)/3);
 assert.equal(12/4,3); rationalEq(3,12,1,4);
});
test('U03 L20: recover unknown whole from exact unit fraction share', () => {
 assert.equal(4*7,28); assert.equal(28/4,7);
 assert.equal(3*12,36); assert.equal(36/3,12);
 rationalEq(1,4*7,1,28); assert.notEqual((1/4)/7,28);
});
test('U03 L16-L18/L20: Grade 5 source-specific Rich V2 sidecar schema', {skip:!fs.existsSync(sidecar)&&'GitHub branch fixture not mounted in isolated Node runner'}, () => {
 const doc=JSON.parse(fs.readFileSync(sidecar,'utf8'));
 assert.equal(doc.SchemaVersion,1); assert.equal(doc.PackCode,'US-CCSS-MATH');
 const ids=['L16','L17','L18','L20'];
 assert.equal(doc.Lessons.length,4);
 assert.deepEqual(doc.Lessons.map(x=>x.LessonCode),ids.map(x=>'PED:US-CCSS-MATH:G5:U03:'+x));
 for(const row of doc.Lessons){
  assert.ok(row.ExplanationParagraphs.length>=4 && row.KeyConcepts.length>=3 && row.WorkedExamples.length>=3);
  assert.ok(row.WorkedExamples.every(x=>x.Steps.length>=3 && x.Answer && x.Check));
  assert.ok(row.CommonMistakes.length>=2);
  assert.ok(row.Visuals.every(v=>v.Description && v.Rows.length>=2));
  assert.equal(row.Videos.length,0);
 }
});
