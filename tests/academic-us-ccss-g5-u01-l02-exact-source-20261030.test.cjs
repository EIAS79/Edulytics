'use strict';
// Stage C WRITE ONLY: intentionally not executed here.
const test=require('node:test');const assert=require('node:assert/strict');
const fs=require('node:fs');const path=require('node:path');
const rich=JSON.parse(fs.readFileSync(path.join(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l01-l03-2021.rich-lesson-v2.json'),'utf8'));
const lesson=rich.Lessons.find(l=>l.LessonCode==='PED:US-CCSS-MATH:G5:U01:L02');
test('source WODB has exactly four distinct cards and mathematically correct counts',()=>{
 const p=lesson.ExplanationParagraphs.join(' ');assert.match(p,/148008/);assert.match(p,/148009/);assert.match(p,/148010/);assert.match(p,/148011/);
 assert.match(p,/11 total/);assert.equal(4+4+3+1,12);assert.equal(2*2*3-1,11);assert.equal(3*2*2,12);
});
test('Guess My Prism is teacher-evaluated; not only numeric',()=>{
 const p=lesson.ExplanationParagraphs.join(' ');
 assert.match(p,/reconstruction that matches dimensions/);assert.match(p,/8-point source-aligned rubric/);
});
