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
 assert.equal(lesson.Visuals[0].Kind,"Grade5Unit1CubeComparison");
 assert.match(lesson.Visuals[0].Description,/two-by-two-by-two cube \(eight cubes\)/i);
 assert.match(lesson.Visuals[0].Description,/five cubes in a bottom row and three above/i);
 assert.match(lesson.Visuals[0].Description,/five-by-one-by-two solid \(ten cubes\)/i);
 const renderer=fs.readFileSync(path.join(root,
   'src/Edulytics.Web/Views/Shared/_RichLessonVisual.cshtml'),'utf8');
 assert.match(renderer,/RichLessonVisualKind.Grade5Unit1CubeComparison/);
 for(const contract of ['Count = 8','Count = 10','5 bottom + 3 above = 8','2 × 1 × 4 = 8']){
   assert.ok(renderer.includes(contract),"Source figure contract missing: "+contract);
 }
 const kind=fs.readFileSync(path.join(root,
   'src/Edulytics.Core/Curriculum/RichLessonContentV2.cs'),'utf8');
 assert.match(kind,/Grade5Unit1CubeComparison = 11/);
});

test('U01:L01 independent author exit measures the greater eight-versus-nine cube volume, not height',()=>{
 const fs=require('node:fs');const path=require('node:path');
 const root=path.resolve(__dirname,'..');
 const j=JSON.parse(fs.readFileSync(path.join(root,
  'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l01-l03-2021.rich-lesson-v2.json'),'utf8'));
 const lesson=j.Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G5:U01:L01');
 const exit=lesson.ExplanationParagraphs.find(x=>x.startsWith('Edulytiks-original five-minute exit check'));
 assert.ok(exit,'First-party source-goal exit instruction missing');
 assert.match(exit,/eight identical unit cubes/);
 assert.match(exit,/nine identical unit cubes/);
 assert.match(exit,/one more unit-cube space/);
 assert.match(exit,/teacher evaluates a valid construction/i);
 assert.equal(9-8,1);
 const evidence=JSON.parse(fs.readFileSync(path.join(root,
  'artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l01-abc-element-ledger-20261030.json'),'utf8'));
 const original=evidence.lessonElements.find(x=>x.SourceElementId==='G5-U01-L01-CD-SOURCE-Q01');
 const author=evidence.lessonElements.find(x=>x.SourceElementId==='G5-U01-L01-CD-AUTHOR-Q01');
 assert.equal(original.Status,'SOURCE_UNAVAILABLE','2021 protected exact task is not invented');
 assert.equal(author.Status,'MATCHED_TEACHER_LED');
 assert.match(author.ExpectedMathematics,/8-versus-9/);
 assert.equal(evidence.sourceCoverage100Percent,false);
});


test('U01:L02 2021 original Student Task Statements source and accessible four-model cube figures',()=>{
 const fs=require('node:fs'),path=require('node:path');
 const root=path.resolve(__dirname,'..');
 const rich=JSON.parse(fs.readFileSync(path.join(root,
  'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l01-l03-2021.rich-lesson-v2.json'),'utf8'));
 const lesson=rich.Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G5:U01:L02');
 const drawing=lesson.Visuals.filter(x=>x.Kind==='Grade5Unit1CubeModelWodb');
 assert.equal(drawing.length,1);
 assert.match(drawing[0].Description,/two separate 1-by-2-by-3 solids/i);
 assert.match(drawing[0].Description,/missing one top cube/i);
 assert.match(drawing[0].Description,/four-, four-, three-, and one-cube layers/i);
 assert.match(drawing[0].Description,/3-by-2-by-2 solid/i);
 const ledger=JSON.parse(fs.readFileSync(path.join(root,
  'artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l02-abc-element-ledger-20261030.json'),'utf8'));
 const source=ledger.original2021StudentTaskPdfDirectProof;
 assert.equal(source.pages,3);
 assert.equal(source.verifiedTextItems,8);
 assert.match(source.pdfSha256,/^[a-f0-9]{64}$/);
 assert.equal(source.originalImagePixelParityCertified,false);
 assert.equal(source.originalLockedCoolDownIncluded,false);
 assert.equal(source.academicSourceParityComplete,false);
 assert.deepEqual(ledger.newFirstPartySourceModelVisual20261102.fourModelCubeCounts,[12,11,12,12]);
 assert.equal(ledger.newFirstPartySourceModelVisual20261102.firstModelIsTwoSeparateSixCubePrisms,true);
 for(const e of ledger.lessonElements.filter(e=>e.Original2021StudentPdfProof)){
  assert.equal(e.Original2021StudentPdfProof.sha256,source.pdfSha256);
  assert.ok(e.Original2021StudentPdfProof.page>=1 && e.Original2021StudentPdfProof.page<=3);
 }
 assert.equal(ledger.sourceCoverage100Percent,false);
 const renderer=fs.readFileSync(path.join(root,
  'src/Edulytics.Web/Views/Shared/_RichLessonVisual.cshtml'),'utf8');
 const kind=fs.readFileSync(path.join(root,
  'src/Edulytics.Core/Curriculum/RichLessonContentV2.cs'),'utf8');
 assert.match(kind,/Grade5Unit1CubeModelWodb = 12/);
 assert.match(renderer,/RichLessonVisualKind.Grade5Unit1CubeModelWodb/);
 assert.match(renderer,/two separate prisms|2 separate prisms/i);
 assert.match(renderer,/1 missing top cube/);
 assert.match(renderer,/4 \+ 4 \+ 3 \+ 1 = 12/);
 assert.match(renderer,/3 by 2 by 2; 12/);
 assert.match(renderer,/aria-label="@Model.Description"/);
});
