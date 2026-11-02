'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const base = 'artifacts/curriculum-delivery/US-CCSS-MATH/G5';
const read = f=>JSON.parse(fs.readFileSync(path.join(root,f),'utf8'));
const pack = read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l04-l06-2021.rich-lesson-v2.json');
const refs = new Map([
 [4,{pages:6,url:'ghvun67la8rtwkccygj2f5x1lwcx',needle:['5 × 24','6 × 20','120']}],
 [5,{pages:4,url:'h9q5z2d85oii6wgm4ygew2kqhbwu',needle:['4 by 10','25','1,000']}],
 [6,{pages:5,url:'3zh9uh4oe9esdpjjy2jgn5b69ggy',needle:['A Tale of Two Tables','Two Truths and a Lie','10×40']}]
]);
test('official original IM2021 Student Task PDFs for U01 L04-L06 are individually located, visually inspected, and compared without claiming gated Cool-down or academic approval',()=>{
 for(const [lessonNum,expected] of refs){
  const id=String(lessonNum).padStart(2,'0');
  const source=read(base+'/u01-l'+id+'-abc-element-ledger-20261030.json');
  const proof=source.stageDOriginalStudentPdfSourceReview20261102;
  assert.equal(source.lessonCode,'PED:US-CCSS-MATH:G5:U01:L'+id);
  assert.equal(source.status,'A_C_EXPLICIT_GAP');
  assert.equal(source.academicAccepted,false);
  assert.equal(source.sourceCoverage100Percent,false);
  assert.equal(proof.pdfPageCount,expected.pages);
  assert.ok(proof.officialStudentTaskPdfUrl.startsWith('https://cms-assets.illustrativemathematics.org/'));
  assert.ok(proof.officialStudentTaskPdfUrl.endsWith(expected.url));
  assert.deepEqual(proof.pdfPagesVisuallyInspected,
   Array.from({length:expected.pages},(_,i)=>i+1));
  assert.equal(proof.studentTaskPdfSourceScopeReviewed,true);
  assert.equal(proof.originalPublisherCoolDownStillGated,true);
  assert.equal(proof.independentAcademicSignoffRecorded,false);
  assert.equal(proof.completeTeacherStudentSourceParityApproved,false);
  assert.equal(proof.stageDVerified,false);
  assert.equal(proof.pdfCryptographicHashVerifiedInThisReview,false);
  assert.ok(proof.sourceTasksComparedToRichV2.length>=3);
  const lesson=pack.Lessons.find(x=>x.LessonCode===source.lessonCode);
  assert.ok(lesson,'must use existing Grade5 Rich V2 '+id);
  const body=lesson.ExplanationParagraphs.join('\n');
  for(const text of expected.needle)assert.ok(body.includes(text),'missing original PDF student task concept '+text+' in '+id);
 }
});
