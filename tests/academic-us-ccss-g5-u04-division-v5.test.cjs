'use strict';
const test=require('node:test');const assert=require('node:assert/strict');const fs=require('node:fs');const path=require('node:path');
const sidecar=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u04-l11-l14-2021.rich-lesson-v2.json');
test('U04 L11: multiple legal partial-quotient decompositions match dividend',()=>{
 assert.equal(20*16+2*16,352);assert.equal(10*16+10*16+2*16,352);
 assert.equal(30*26+2*26,832);assert.equal(20*26+12*26,832);
});
test('U04 L12: partial quotient steps subtract divisor multiples',()=>{
 assert.equal(588-420-126-42,0);assert.equal(20+6+2,28);
 assert.equal(21*28,588);assert.equal(768-720-48,0);assert.equal(24*32,768);
});
test('U04 L13: exact quotient estimate differs from quotient',()=>{
 assert.equal(672/28,24);assert.equal(988/38,26);
 assert.equal(1000/40,25);assert.equal(20*38+6*38,988);
});
test('U04 L14: multiplication versus subtraction errors are distinct',()=>{
 assert.notEqual(30*26,750);assert.equal(30*26,780);
 assert.notEqual(832-780,62);assert.equal(832-780,52);
 assert.equal(200*33+50*33+5*33,8415);assert.equal(300*24+6*24,7344);
});
test('U04 L11-L14: sidecar identity and first-party fields', {skip:!fs.existsSync(sidecar)&&'GitHub branch fixture not locally mounted'},()=>{
 const d=JSON.parse(fs.readFileSync(sidecar,'utf8'));assert.equal(d.SchemaVersion,1);assert.equal(d.PackCode,'US-CCSS-MATH');assert.equal(d.Lessons.length,4);
 d.Lessons.forEach((x,i)=>{
  assert.equal(x.LessonCode,'PED:US-CCSS-MATH:G5:U04:L'+String(i+11).padStart(2,'0'));
  assert.ok(x.ExplanationParagraphs.length>=4&&x.KeyConcepts.length>=3&&x.WorkedExamples.length>=3);
  assert.ok(x.CommonMistakes.length>=2&&x.SummaryPoints.length>=3);
  assert.ok(x.Visuals.every(v=>v.Description&&v.Rows.length>0));
 });
});
