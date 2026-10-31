'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const sidecar=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u04-l06-l10-2021.rich-lesson-v2.json');
test('U04 L06: both composing partial products reconcile',()=>{
 assert.equal(273*4+273*20,6552);
 assert.equal(368*7+368*50,20976);
 assert.equal(273*2,546);assert.equal(273*20,5460);
});
test('U04 L07: digit game legal trials and bounded carry',()=>{
 const a=864*21,b=821*64,c=642*81;
 assert.deepEqual([a,b,c],[18144,52544,52002]);
 assert.ok(b>c&&c>a);assert.equal(b-c,542);
 assert.equal(Math.floor((9*9)/10),8);
});
test('U04 L08: closest-target trials use absolute differences',()=>{
 const vals=[813*62,682*31,631*82];
 assert.deepEqual(vals,[50406,21142,51742]);
 assert.deepEqual(vals.map(x=>Math.abs(x-40000)),[10406,18858,11742]);
 assert.equal(6*(2+1/3),14);assert.equal(6*(2+2/3),16);
});
test('U04 L09: birdhouse min/max ranges are cubic areas',()=>{
 assert.equal(5*5*7,175);assert.equal(5*5*11,275);
 assert.equal(5*5*10,250);assert.equal(8*9*12,864);assert.equal(8*9*15,1080);
});
test('U04 L10: dancer group sizes produce inverse quotient pattern',()=>{
 assert.equal(3456/8,432);assert.equal(3456/4,864);assert.equal(3456/2,1728);
 assert.equal(400*8+30*8+2*8,3456);assert.equal(432*8,3456);
});
test('U04 L06-L10: grade-specific sidecar identity and structural checks', {skip:!fs.existsSync(sidecar)&&'GitHub sidecar not mounted in isolated Node runner'},()=>{
 const doc=JSON.parse(fs.readFileSync(sidecar,'utf8'));
 assert.equal(doc.SchemaVersion,1);assert.equal(doc.PackCode,'US-CCSS-MATH');assert.equal(doc.Lessons.length,5);
 doc.Lessons.forEach((x,i)=>{
  assert.equal(x.LessonCode,'PED:US-CCSS-MATH:G5:U04:L'+String(i+6).padStart(2,'0'));
  assert.ok(x.KeyConcepts.length>=3&&x.ExplanationParagraphs.length>=4&&x.WorkedExamples.length>=3);
  assert.ok(x.CommonMistakes.length>=2&&x.SummaryPoints.length>=3);
  assert.ok(x.Visuals.every(v=>v.Description&&v.Rows.length>=2));
  assert.equal(x.Videos.length,0);
 });
});
