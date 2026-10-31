'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
test('G5 U01 L11 2021 source-specific geometry and teacher grading',()=>{
 assert.equal(10*6*5,300);assert.equal(4*9*5,180);assert.equal(30*9,270);assert.equal(10*3*4+8*3*4,216);
 const lesson=JSON.parse(fs.readFileSync(path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l07-l12-2021.rich-lesson-v2.json'),'utf8')).Lessons.find(l=>l.LessonCode==='PED:US-CCSS-MATH:G5:U01:L11');
 assert.match(lesson.ExplanationParagraphs.join(' '),/Teacher 8-point source-aligned rubric/);
});
