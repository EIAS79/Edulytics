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

test('G5 U01 L07 authored visuals do not falsely claim the original 3-foot edge was an invented extra measurement',()=>{
 const fs=require('node:fs'),path=require('node:path');
 const r=path.resolve(__dirname,'..');
 const evidence=JSON.parse(fs.readFileSync(path.join(r,'artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l07-abc-element-ledger-20261030.json'),'utf8'));
 const doc=JSON.parse(fs.readFileSync(path.join(r,'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l07-l12-2021.rich-lesson-v2.json'),'utf8'));
 const lesson=doc.Lessons.find(x=>x.LessonCode===evidence.lessonCode);
 const visual=lesson.Visuals.find(x=>x.Title==='Edulytiks-original cubic-unit context and information gap');
 assert.match(evidence.original.images.find(x=>x.id==='148052').alt,/4 by blank by 3 feet/);
 assert.match(visual.Description,/source figure shows edges 4 feet and 3 feet/);
 assert.doesNotMatch(visual.Description,/3-foot height is an Edulytiks-original extra condition/);
 assert.equal(visual.Rows[2].Cells[1],'Problem figure: 4-foot and 3-foot edges');
 assert.equal(visual.Rows[3].Label,'Edulytiks-authored worked representation');
 assert.equal(evidence.academicAccepted,false);
});
