'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
test('G5 U01 L12 optional 2021 mass/volume and five unique container models',()=>{
 const arrays=[[1,6,10],[2,5,6],[3,4,5],[2,3,10],[1,3,20]];
 arrays.forEach(a=>assert.equal(a.reduce((x,y)=>x*y,1),60));assert.equal(new Set(arrays.map(a=>a.join('×'))).size,5);
 const l=JSON.parse(fs.readFileSync(path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l07-l12-2021.rich-lesson-v2.json'),'utf8')).Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G5:U01:L12');
 assert.match(l.ExplanationParagraphs.join(' '),/Teacher 8-point optional-source modeling rubric/);
 const ledger=JSON.parse(fs.readFileSync(path.resolve(__dirname,'../artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l12-abc-element-ledger-20261030.json'),'utf8'));
 assert.equal(ledger.optional,true);assert.equal(ledger.acceptance.originalCoolDown,'NOT_IN_SOURCE');
});
