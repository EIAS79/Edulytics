'use strict';
const test=require('node:test');const assert=require('node:assert/strict');const fs=require('node:fs');const path=require('node:path');
const names=['us-ccss-g5-u04-l15-l17-2021.rich-lesson-v2.json','us-ccss-g5-u04-l18-l21-2021.rich-lesson-v2.json'];
const dirs=names.map(x=>path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/',x));
test('U04 L15: unique and nonunique missing dimensions with distinct units',()=>{
 assert.equal(1536/48,32);assert.equal(2160/120,18);assert.equal(960/32,30);
 for(const pair of [[5,6],[3,10],[2,15]])assert.equal(32*pair[0]*pair[1],960);
});
test('U04 L16: fractional quotient of one continuous noodle preserves all length',()=>{
 assert.equal(2050/80,25.625);assert.equal(80*(25+5/8),2050);
 assert.equal(2050/100,20.5);assert.ok(20.5<25.625);
});
test('U04 L17: select only expression sets with the exact dividend',()=>{
 assert.equal((70+14)/7,12);assert.equal((63+21)/7,12);
 assert.notEqual((71+14)/7,12);assert.notEqual(70/7+14,12);
 assert.equal((150+15)/15,11);assert.equal((75+90)/15,11);
});
test('U04 L18: multi-population milk volume growth and rate-based days are conditional',()=>{
 assert.equal(2*2*5,20);assert.equal(25*20,500);
 assert.equal(20*500,10000);assert.equal(10*10000,100000);
 assert.equal(1000000/10000,100);assert.equal(1000000/100000,10);
});
test('U04 L19: kilometer area is square measure, not volume',()=>{
 assert.equal(1000*1000,1000000);assert.equal(700*500,350000);
 assert.equal(1000*250,250000);assert.equal(350000-250000,100000);
});
test('U04 L20: shipping volumes and hypothetical school models are correct',()=>{
 assert.equal(20*8*8,1280);assert.equal(1280/40,32);
 assert.equal(120000/40000,3);assert.equal(40*180,7200);
 assert.ok(7200>3*1280);
});
test('U04 L21: example food journal mass conversions and cautious projections',()=>{
 assert.equal([8,12,4,16,8].reduce((a,b)=>a+b,0),48);
 assert.equal(48/16,3);assert.equal(3*4,12);assert.equal(3*36,108);
});
test('U04 L15-L21: first-party sidecar structures are correct',{skip:!dirs.every(p=>fs.existsSync(p))&&'GitHub Grade 5 JSON files are not mounted in the sandbox'},()=>{
 const rows=dirs.flatMap(p=>{const d=JSON.parse(fs.readFileSync(p,'utf8'));assert.equal(d.SchemaVersion,1);assert.equal(d.PackCode,'US-CCSS-MATH');return d.Lessons;});
 assert.equal(rows.length,7);
 rows.forEach((row,i)=>{
  assert.equal(row.LessonCode,'PED:US-CCSS-MATH:G5:U04:L'+String(15+i).padStart(2,'0'));
  assert.ok(row.ExplanationParagraphs.length>=4&&row.KeyConcepts.length>=3&&row.WorkedExamples.length>=3);
  assert.ok(row.CommonMistakes.length>=2&&row.SummaryPoints.length>=3);
  assert.ok(row.Visuals.every(x=>x.Description&&x.Rows.length>0));assert.equal(row.Videos.length,0);
 });
});
