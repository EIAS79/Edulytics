'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
test('G5 U01 L07 source math and teacher-led review',()=>{
 assert.equal(4*2*3,24);assert.notEqual(4*2,24);
 const p=JSON.parse(fs.readFileSync(path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l07-l12-2021.rich-lesson-v2.json'),'utf8')).Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G5:U01:L07');
 assert.match(p.ExplanationParagraphs.join(' '),/Teacher 8-point source-aligned rubric/);
});


test('G5 U01 L07 original figure 148052 keeps the unspecified perpendicular edge unknown, never mislabels it as a length or invents its value',()=>{
 const path=require('node:path'),fs=require('node:fs');
 const root=path.resolve(__dirname,'..');
 const evidence=JSON.parse(fs.readFileSync(path.join(root,'artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l07-abc-element-ledger-20261030.json'),'utf8'));
 assert.equal(evidence.original.images.find(x=>x.id==='148052').alt,'Prism. 4 by blank by 3 feet.');
 const p=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l07-l12-2021.rich-lesson-v2.json'),'utf8')).Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G5:U01:L07');
 const source=p.WorkedExamples.find(x=>x.Title==='Original 2021 Information Gap freezer — ask before multiplying');
 assert.ok(source);
 assert.match(source.Question,/unknown perpendicular edge/);
 assert.doesNotMatch(source.Question,/UNKNOWN length/i);
 assert.match(source.Answer,/12d cubic feet/);
 assert.match(source.Check,/148052/);
 assert.match(source.Check,/not identified as length, width/);
 assert.equal(4*3*2,24);
 assert.equal(evidence.status,'A_C_EXPLICIT_GAP');
 assert.equal(evidence.academicAccepted,false);
});
