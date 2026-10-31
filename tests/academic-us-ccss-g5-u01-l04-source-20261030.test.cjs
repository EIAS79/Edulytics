'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
test('G5 U01 L04 2021 source mathematical oracle and teacher rubric',()=>{
 assert.equal(5*24,120);assert.equal(6*20,120);assert.equal(4*30,120);assert.notEqual(2*7,42);
 const p=JSON.parse(fs.readFileSync(path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l04-l06-2021.rich-lesson-v2.json'),'utf8')).Lessons.find(l=>l.LessonCode==='PED:US-CCSS-MATH:G5:U01:L04');
 assert.match(p.ExplanationParagraphs.join(' '),/Teacher 8-point source-aligned review/);
});
