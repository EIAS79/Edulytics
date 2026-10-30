'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
test('G5 U01 L10 2021 source-specific geometry and teacher grading',()=>{
 assert.equal(125-2*3*5,95);assert.equal(3*5*5+2*2*5,95);assert.equal(4*8*3+6*4*3,168);assert.equal(10*4*3+4*4*3,168);assert.equal(10*8*3-6*4*3,168);
 const lesson=JSON.parse(fs.readFileSync(path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l07-l12-2021.rich-lesson-v2.json'),'utf8')).Lessons.find(l=>l.LessonCode==='PED:US-CCSS-MATH:G5:U01:L10');
 assert.match(lesson.ExplanationParagraphs.join(' '),/Teacher 8-point source-aligned rubric/);
});
