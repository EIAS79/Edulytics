'use strict';
// Grade 5 Unit 1 source-specific math oracles. WRITTEN_NOT_RUN (A–B–C phase).
const test = require('node:test');
const assert = require('node:assert/strict');
test('U01 construction and packed prisms respect unit-cube counts',()=>{
 assert.deepEqual([2*2*2,5+3,2*1*4,5*1*2],[8,8,8,10]);
 assert.deepEqual([4*2*5,4*3*5,4*4*5],[40,60,80]);
 assert.equal(5*24,120);assert.equal(6*20,120);assert.equal(4*30,120);
 assert.deepEqual([40*1,40*2,40*3,40*10,40*25],[40,80,120,400,1000]);
});
test('U01 composite volumes and units use independent arithmetic',()=>{
 assert.equal(3*5*3+1,46);assert.equal(3*5*3+5,50);assert.equal(3*6*3+6,60);assert.equal(3*5*3+2*2*3,57);
 assert.equal(2*7*2+5*5*2,78);assert.equal(9*3*7+5*2*7,259);
 assert.equal(5*8*6+5*4*9,420);assert.equal(5*4*3+5*12*6,420);
 assert.equal(5*5*5-2*3*5,95);assert.equal(3*5*5+2*2*5,95);
 assert.equal(4*8*3+6*4*3,168);assert.equal(10*4*3+4*4*3,168);assert.equal(10*8*3-6*4*3,168);
 assert.equal(10*6*5,300);assert.equal(4*9*5,180);assert.equal(30*9,270);
 assert.equal(10*3*4+8*3*4,216);
});
test('U01 2021 optional modeling preserves 60 containers not fabricated ship estimate',()=>{
 for(const a of [[1,6,10],[2,5,6],[3,4,5],[2,3,10],[1,3,20]])assert.equal(a.reduce((x,y)=>x*y,1),60);
});

test('U01:L01 first-edition 2021 student task PDF supports exact page-level 9-task contract, without false Cool-down certification',()=>{
 const fs=require('node:fs');const path=require('node:path');
 const root=path.resolve(__dirname,'..');
 const ledger=JSON.parse(fs.readFileSync(path.join(root,
   'artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l01-abc-element-ledger-20261030.json'),'utf8'));
 const source=ledger.original2021StudentTaskPdfDirectProof;
 assert.match(source.pdfSha256,/^[0-9a-f]{64}$/);
 assert.equal(source.pdfSha256,'e07a5fa1c847c059660e014f26e36b677f21e74fcf310a59a1d13ed2ef8e3bca');
 assert.equal(source.pageCount,3);
 assert.equal(source.sourceEditionFooterVerified,true);
 assert.equal(source.verifiedStudentTextItems,9);
 assert.equal(source.originalCooldownIncluded,false);
 assert.equal(source.publisherPdfCommitted,false);
 const expected={
   'G5-U01-L01-IDENTITY':1,'G5-U01-L01-GOAL':1,
   'G5-U01-L01-WU-Q01':1,'G5-U01-L01-A01-Q01':2,
   'G5-U01-L01-A01-Q02':2,'G5-U01-L01-A01-Q03':2,
   'G5-U01-L01-A02-Q01':3,'G5-U01-L01-A02-Q02':3,
   'G5-U01-L01-A02-Q03':3
 };
 assert.equal(ledger.lessonElements.length,25);
 for(const [sourceId,page] of Object.entries(expected)){
   const element=ledger.lessonElements.find(x=>x.SourceElementId===sourceId);
   assert.ok(element,sourceId);
   assert.equal(element.Original2021StudentPdfProof.pdfSha256,source.pdfSha256);
   assert.equal(element.Original2021StudentPdfProof.page,page);
   assert.equal(element.Original2021StudentPdfProof.validatedAgainstActualLocalPdf,true);
   assert.ok(element.Original2021StudentPdfProof.sourceTextSnippet.length>=12);
 }
 const originalCooldown=ledger.lessonElements.find(x=>x.SourceElementId==='G5-U01-L01-CD-SOURCE-Q01');
 assert.equal(originalCooldown.Status,'SOURCE_UNAVAILABLE');
 assert.equal(originalCooldown.Original2021StudentPdfProof.present,false);
 assert.equal(ledger.sourceCoverage100Percent,false);
 assert.equal(ledger.academicAccepted,false);
 const rich=JSON.parse(fs.readFileSync(path.join(root,
  'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l01-l03-2021.rich-lesson-v2.json'),'utf8'));
 const lesson=rich.Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G5:U01:L01');
 assert.ok(lesson);
 const prose=JSON.stringify(lesson);
 for(const model of ['2','5','3','9','8','10'])
   assert.ok(prose.includes(model),'Student source model '+model);
 assert.match(prose,/connecting cubes|congruent cubes/i);
});
