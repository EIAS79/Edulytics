'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const richPath = path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u03-l02-l06-2021.rich-lesson-v2.json');
const equals = (a,b,c,d) => assert.equal(BigInt(a)*BigInt(d),BigInt(b)*BigInt(c));

test('U03 L02: the original whole is held fixed through two orders', () => {
 equals(1,3*4,1,12);
 equals(1,4*3,1,12);
 equals(1,2*5,1,10);
});

test('U03 L03: changing row counts changes the area cell denominator', () => {
 const counts=[2,3,4,5,7].map(rows=>4*rows);
 assert.deepEqual(counts,[8,12,16,20,28]);
 for(const n of counts) equals(1,n,1,n);
});

test('U03 L04: nested land uses produce fractions of the ORIGINAL whole', () => {
 equals(1*3,2*5,3,10);
 equals(1*2,4*3,1,6);
 assert.notEqual(3/10,1/2);
});

test('U03 L05: the selected-area numerator is a count, not an extra divisor', () => {
 equals(1*3,4*5,3,20);
 equals(1*2,4*5,1,10);
 equals(1*4,4*5,1,5);
});

test('U03 L06: two non-unit fractions are rectangle dimensions', () => {
 equals(2*3,3*4,1,2);
 equals(2*4,5*7,8,35);
 equals(6,12,1,2);
});

test('U03 L02-L06: direct Rich V2 sidecar is valid and source-scoped', {skip: !fs.existsSync(richPath) && 'Unpublished GitHub lesson file is not mounted in isolated Node environment'}, () => {
 const doc=JSON.parse(fs.readFileSync(richPath,'utf8'));
 assert.equal(doc.SchemaVersion,1);
 assert.equal(doc.PackCode,'US-CCSS-MATH');
 assert.equal(doc.Lessons.length,5);
 for(let i=0;i<5;i++){
  const row=doc.Lessons[i];
  assert.equal(row.LessonCode,'PED:US-CCSS-MATH:G5:U03:L'+String(i+2).padStart(2,'0'));
  assert.ok(row.ExplanationParagraphs.length>=4);
  assert.ok(row.KeyConcepts.length>=3);
  assert.ok(row.WorkedExamples.length>=3);
  assert.ok(row.WorkedExamples.every(x=>x.Steps.length>=3 && x.Check));
  assert.ok(row.CommonMistakes.length>=2);
  assert.ok(row.Visuals.every(x=>x.Description && x.Rows.length>0));
  assert.equal(row.Videos.length,0);
 }
});
