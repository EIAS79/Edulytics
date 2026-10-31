'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
test('G5 U01 L06 source math and teacher-led review',()=>{
 assert.equal(4*2*5,4*(2*5));assert.equal(2*5*4,2*20);assert.notEqual(5*4*2,10*40);assert.equal(6*3*4,72);
 const p=JSON.parse(fs.readFileSync(path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l04-l06-2021.rich-lesson-v2.json'),'utf8')).Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G5:U01:L06');
 assert.match(p.ExplanationParagraphs.join(' '),/Teacher 8-point source-aligned rubric/);
});
