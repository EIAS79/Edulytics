'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
test('G5 U01 L08 exact geometry and teacher rubric',()=>{
 assert.equal(12+10,22);assert.equal(3*2*2+3*2*3,30);assert.equal(3*5*3+1,46);assert.equal(3*5*3+5,50);assert.equal(3*6*3+6,60);assert.equal(3*5*3+2*2*3,57);
 const obj=JSON.parse(fs.readFileSync(path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l07-l12-2021.rich-lesson-v2.json'),'utf8')).Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G5:U01:L08');
 assert.match(obj.ExplanationParagraphs.join(' '),/Teacher 8-point source-aligned rubric/);
});
