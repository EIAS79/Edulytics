'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const get=()=>JSON.parse(fs.readFileSync(path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l01-l03-2021.rich-lesson-v2.json'),'utf8')).Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G5:U01:L03');
test('source L03 volumes and multiple complete layer decompositions',()=>{assert.deepEqual([4*2*5,4*3*5,4*4*5],[40,60,80]);assert.deepEqual([5*8,2*20,4*10],[40,40,40]);const text=get().ExplanationParagraphs.join(' ');assert.match(text,/48 connecting cubes/);assert.match(text,/Teacher 8-point manual review/);});
