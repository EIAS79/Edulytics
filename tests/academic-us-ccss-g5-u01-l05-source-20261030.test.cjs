'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
test('G5 U01 L05 2021 source mathematical oracle and teacher rubric',()=>{
 assert.deepEqual([1,2,3,10,25].map(h=>40*h),[40,80,120,400,1000]);assert.equal(3*4*5,60);
 const p=JSON.parse(fs.readFileSync(path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l04-l06-2021.rich-lesson-v2.json'),'utf8')).Lessons.find(l=>l.LessonCode==='PED:US-CCSS-MATH:G5:U01:L05');
 assert.match(p.ExplanationParagraphs.join(' '),/Teacher 8-point source-aligned review/);
});
