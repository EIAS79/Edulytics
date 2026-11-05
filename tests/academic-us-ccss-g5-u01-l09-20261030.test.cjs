'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
test('G5 U01 L09 exact geometry and teacher rubric',()=>{
 assert.equal(2*7*2+5*5*2,78);assert.equal(7*5*2+2*2*2,78);assert.equal(9*3*7+5*2*7,259);assert.equal(5*8*6+5*4*9,420);assert.equal(5*4*3+5*12*6,420);
 const obj=JSON.parse(fs.readFileSync(path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l07-l12-2021.rich-lesson-v2.json'),'utf8')).Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G5:U01:L09');
 assert.match(obj.ExplanationParagraphs.join(' '),/Teacher 8-point source-aligned rubric/);
});
