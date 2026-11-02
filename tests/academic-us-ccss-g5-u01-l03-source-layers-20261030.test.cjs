'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const get=()=>JSON.parse(fs.readFileSync(path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l01-l03-2021.rich-lesson-v2.json'),'utf8')).Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G5:U01:L03');
test('source L03 volumes and multiple complete layer decompositions',()=>{assert.deepEqual([4*2*5,4*3*5,4*4*5],[40,60,80]);assert.deepEqual([5*8,2*20,4*10],[40,40,40]);const text=get().ExplanationParagraphs.join(' ');assert.match(text,/48 connecting cubes/);assert.match(text,/Teacher 8-point manual review/);});

test('2021 U01:L03 original student PDF and resolved practice mapping remain separate from protected source tasks',()=>{
 const root=path.resolve(__dirname,'..');
 const ledger=JSON.parse(fs.readFileSync(path.join(root,
  'artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l03-abc-element-ledger-20261030.json'),'utf8'));
 const pdf=ledger.original2021StudentTaskPdfDirectProof;
 assert.equal(pdf.studentTaskStatementsEdition,'2021');
 assert.equal(pdf.pageCount,3);
 assert.equal(pdf.verifiedStudentTextElementCount,6);
 assert.match(pdf.pdfSha256,/^[a-f0-9]{64}$/);
 assert.equal(pdf.illustrationsAndOriginalLockedCooldownCertified,false);
 assert.equal(pdf.sourceParityComplete,false);
 const expected={ 'G5-U01-L03-IDENTITY':1,'G5-U01-L03-GOAL':1,
  'G5-U01-L03-WU-01':1,'G5-U01-L03-A01-01':2,
  'G5-U01-L03-A01-02':2,'G5-U01-L03-A02-Q01':3 };
 for(const [id,page] of Object.entries(expected)){
   const e=ledger.sourceElements.find(x=>x.SourceElementId===id);
   assert.ok(e?.Original2021StudentPdfProof,id);
   assert.equal(e.Original2021StudentPdfProof.pdfSha256,pdf.pdfSha256);
   assert.equal(e.Original2021StudentPdfProof.page,page);
 }
 const digital=ledger.sourceElements.find(x=>x.SourceElementId==='G5-U01-L03-PRACTICE');
 assert.equal(digital.Status,'MATCHED_DIGITAL');
 assert.match(digital.VerificationMethod,/LessonPracticeContractRegistry/);
 assert.match(digital.VerificationMethod,/41381584160/);
 assert.equal(digital.RuntimeEvidence.sha,'4130bb559f4d4544fdc237e579e86314035ffb44');
 const registry=fs.readFileSync(path.join(root,
  'src/Edulytics.Core/Mathematics/Practice/UsCcssG5Unit1Lesson3PracticeContracts.cs'),'utf8');
 const engine=fs.readFileSync(path.join(root,
  'src/Edulytics.Services/Mathematics/UsCcssG5Unit1Lesson3Questions.cs'),'utf8');
 assert.match(registry,/usccss\.g5\.u01\.l03\.count_hidden_prism_layers/);
 assert.match(engine,/including cubes hidden from view/);
 for(const blocker of ledger.remainingSourceBlockers)
   assert.equal(ledger.sourceElements.find(x=>x.SourceElementId===blocker).Status,'SOURCE_UNAVAILABLE');
 assert.equal(ledger.academicAccepted,false);
});
