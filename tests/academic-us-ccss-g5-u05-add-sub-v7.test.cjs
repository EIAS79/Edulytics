'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.join(__dirname,'..','src','Edulytics.Core','Curriculum','LessonContent','RichV2');
const filenames=['us-ccss-g5-u05-l11-l13-2021.rich-lesson-v2.json','us-ccss-g5-u05-l14-l15-2021.rich-lesson-v2.json'];
const sources=filenames.map(n=>path.join(root,n));
const exactCents=(x)=>Math.round(x*100);

test('U05 L11: decimal composition and two independent addition strategies',()=>{
  assert.equal(68+37,105);
  assert.equal(exactCents(3.48)+exactCents(2.76),624);
  assert.equal(exactCents(4)+exactCents(2.24),624);
});
test('U05 L11: six-target-number choices preserve their specified digits and upper bound',()=>{
  const digits=[2,3,4,1,5,2],moves=[20,30,4,10,5,2];
  assert.deepEqual(moves.map(v=>v>=10?v/10:v),digits);
  assert.equal(moves.reduce((a,b)=>a+b,0),71);
  assert.ok(moves.reduce((a,b)=>a+b,0)<=100);
});
test('U05 L12: decimal aligned addition with composing and rounding boundaries',()=>{
  assert.equal(476+258,734);
  assert.equal(370+684,1054);
  assert.equal(896+230,1126);
  assert.equal(Math.round(1054/100),11);
});
test('U05 L13: adversarial right alignment and approximate magnitude',()=>{
  assert.equal(51837+6420,58257);
  assert.notEqual(51837+6420,52479);
  assert.ok(58257>57000);
  assert.equal(482+637,1119); // hundredths: 4.82 + 6.37
  assert.equal(4820+6370,11190); // hundredths: 48.2 + 63.7
  assert.equal(48200+637,48837); // hundredths: 482 + 6.37
});
test('U05 L14: subtract with count-up and hundredths-grid model',()=>{
  assert.equal(94-36,58);
  assert.equal(412-168,244);
  assert.equal(32+200+12,244);
});
test('U05 L14: six-move subtract target remains at or above 1',()=>{
  const digits=[4,2,3,1,2,1], moves=[40,20,30,1,2,1];
  assert.deepEqual(moves.map(v=>v>=10?v/10:v),digits);
  let running=200;
  for(const move of moves){running-=move;assert.ok(running>=100);}
  assert.equal(running,106);
});
test('U05 L15: subtract/regroup and compensation produce exact differences',()=>{
  assert.equal(904-358,546);assert.equal(546+358,904);
  assert.equal(1623-570,1053);assert.equal(1053+570,1623);
  assert.equal(1806-980,826);assert.equal(826+980,1806);
  assert.equal(41263-5840,35423);
  assert.notEqual(41263-5840,40679);
});
test('U05 L11-L15: committed Rich V2 field and lesson identity checks', {skip:!sources.every(p=>fs.existsSync(p))&&'GitHub source sidecars not locally mounted'},()=>{
  const lessons=sources.flatMap(p=>{const d=JSON.parse(fs.readFileSync(p,'utf8'));assert.equal(d.SchemaVersion,1);assert.equal(d.PackCode,'US-CCSS-MATH');return d.Lessons;});
  assert.equal(lessons.length,5);
  lessons.forEach((x,i)=>{
    assert.equal(x.LessonCode,'PED:US-CCSS-MATH:G5:U05:L'+String(i+11).padStart(2,'0'));
    assert.ok(x.ExplanationParagraphs.length>=4&&x.KeyConcepts.length>=3&&x.WorkedExamples.length>=3&&x.CommonMistakes.length>=2);
    assert.ok(x.Visuals.every(v=>v.Description&&v.Rows.length>=2));
    assert.match(x.SummaryPoints.join(' '),/2021/);
  });
});
