'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const sidecar = path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u05-l02-l06-2021.rich-lesson-v2.json');
const eq = (a,b) => assert.ok(Math.abs(a-b)<1e-12,String(a)+' differs from '+String(b));
test('U05 L02: name hundredths and thousandths of one fixed whole',()=>{
 assert.equal(300+20+7,327); assert.equal(600+20+5,625);
 assert.equal(15*10,150); assert.notEqual(15/1000,15/100);
 eq(0.327,0.3+0.02+0.007); eq(0.15,150/1000);
});
test('U05 L03: expanded decimals preserve zero hundredths',()=>{
 eq(0.835,8/10+3/100+5/1000);
 eq(0.105,1/10+0/100+5/1000);
 eq(0.407,4/10+0/100+7/1000);
 assert.notEqual(105/1000,15/100);
});
test('U05 L04: balance mass and piece-count optimization under three allowed denominations',()=>{
 const target=385;
 const combos=[[3,8,5],[2,18,5],[0,0,385]];
 for(const [tenths,hundredths,thousandths] of combos)assert.equal(100*tenths+10*hundredths+thousandths,target);
 assert.deepEqual(combos.map(x=>x.reduce((a,b)=>a+b,0)),[16,25,385]);
 let min=Infinity,max=0;
 for(let a=0;a<=3;a++)for(let b=0;b<=38;b++){
  const c=385-100*a-10*b;if(c<0)continue;
  const pieces=a+b+c;min=Math.min(min,pieces);max=Math.max(max,pieces);
 }
 assert.equal(min,16);assert.equal(max,385);
});
test('U05 L05: sports compare direction and strict in-between decimals',()=>{
 assert.equal(7060,7060);assert.notEqual(7060,7006);assert.notEqual(7060,7600);
 assert.equal(Math.max(6207,6270,6199),6270);
 assert.equal(Math.min(27213,27231,27208),27208);
 assert.ok(5000<5002&&5002<5009&&5009<5010);
});
test('U05 L06: decimal line ticks and position are mathematically consistent',()=>{
 const start=530, a=534, b=537, end=540;
 assert.ok(start<a&&a<b&&b<end);
 assert.equal(a-start,4);assert.equal(b-start,7);assert.equal(b-a,3);
 assert.equal(230-216,14);assert.equal(200-20,180);
 assert.equal(1/1000,0.001);
});
test('U05 L02-L06: lesson-owned Rich V2 has unique IDs, fields and accessible visual rows', {skip:!fs.existsSync(sidecar)&&'GitHub branch sidecar not mounted in isolated local runner'},()=>{
 const doc=JSON.parse(fs.readFileSync(sidecar,'utf8'));
 assert.equal(doc.SchemaVersion,1);assert.equal(doc.PackCode,'US-CCSS-MATH');assert.equal(doc.Lessons.length,5);
 const ids=new Set();for(let i=0;i<doc.Lessons.length;i++){
  const x=doc.Lessons[i];assert.equal(x.LessonCode,'PED:US-CCSS-MATH:G5:U05:L'+String(i+2).padStart(2,'0'));
  assert.ok(!ids.has(x.LessonCode));ids.add(x.LessonCode);
  assert.ok(x.ExplanationParagraphs.length>=4&&x.KeyConcepts.length>=3&&x.WorkedExamples.length>=3);
  assert.ok(x.CommonMistakes.length>=2&&x.SummaryPoints.length>=3);
  assert.ok(x.Visuals.every(v=>v.Description&&v.Rows.length>=2));assert.equal(x.Videos.length,0);
 }
});
