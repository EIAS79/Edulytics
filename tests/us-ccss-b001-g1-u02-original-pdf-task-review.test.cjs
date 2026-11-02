'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const base = path.resolve(__dirname,'../docs/curriculum/batches');
const json = name => JSON.parse(fs.readFileSync(path.join(base,name),'utf8'));
const review = json('US-CCSS-B001-G1-U02-L04-L10-ORIGINAL-STUDENT-PDF-TEXT-AND-REVIEW-BLOCKERS.json');
const catalogue = json('US-CCSS-B001-CATALOGUE.json');
const sourcePdfs = json('US-CCSS-B001-IM2021-STUDENT-TASKS-PDF-SOURCES-500.json');
const pedagogy = json('US-CCSS-B001-2021-SOURCE-PEDAGOGY-PACKETS-1-OF-2.json');

test('seven inspected primary-source PDFs correspond exactly to original B001 canonical identities',()=>{
  assert.equal(catalogue.total,500);
  assert.equal(sourcePdfs.count,500);
  assert.equal(review.batchId,'US-CCSS-B001');
  assert.equal(review.schemaVersion,1);
  assert.equal(review.lessons.length,7);
  const ordinals = review.lessons.map(x=>x.ordinal);
  assert.deepEqual(ordinals,[19,20,21,22,23,24,25]);
  assert.equal(new Set(review.lessons.map(x=>x.lessonCode)).size,7);
  for(const row of review.lessons){
    const c=catalogue.lessons[row.ordinal-1];
    const pdf=sourcePdfs.lessons[row.ordinal-1];
    const packet=pedagogy.lessons[row.ordinal-1];
    assert.equal(row.lessonCode,c.lessonCode);
    assert.equal(row.lessonCode,pdf.lessonCode);
    assert.equal(row.lessonCode,packet.code);
    assert.equal(row.sourceStudentPdfUrl,pdf.originalStudentTaskStatementPdfUrl);
    assert.equal(pdf.sourceTeacherPreparationUrl,c.sourceUrl);
    assert.match(row.sourceStudentPdfUrl,/^https:\/\/cms-assets\.illustrativemathematics\.org\/[a-z0-9]+$/);
    assert.equal(row.sourceTextInspection,'TEXT_EXTRACTED_AND_COMPARED_TO_CANDIDATE');
    assert.ok(row.observedPdfPages>=2);
    assert.ok(row.observedActivityPages.length>0);
    assert.ok(row.observedActivityPages.every(page=>Number.isSafeInteger(page)&&page>0&&page<=row.observedPdfPages));
    assert.ok(row.observedTaskDescription.length>60);
    assert.ok(row.candidatePracticeFamily.startsWith('usccss.g1.u02.'));
    assert.ok(packet.teacherPurposeExcerpt.length>50);
    assert.ok(row.blockingFindings.length>=1);
    assert.ok(row.blockingFindings.every(x=>typeof x==='string'&&x.length>=45));
  }
});
test('source text inspection may never forge human graphics, classroom or certification status',()=>{
  assert.equal(review.sourceTaskPdfTextObserved,7);
  assert.equal(review.pdfGraphicsIndependentlyApproved,0);
  assert.equal(review.studentActivityUatPassed,0);
  assert.equal(review.academicSignedCount,0);
  assert.equal(review.b001ReleaseDecision,'NO_GO');
  assert.match(review.nonCertificationNotice,/does NOT establish/);
  assert.ok(review.lessons.every(x=>x.publisherGraphicsInspection==='PENDING'));
  assert.ok(review.lessons.every(x=>x.effectiveLearnerInteractiveUat==='PENDING'));
  assert.ok(review.lessons.every(x=>x.independentTeacherDecision==='UNSIGNED'));
  assert.ok(review.lessons.every(x=>x.academicDecision==='NO_GO'));
});
test('all source family mode-specific authentic learning gaps remain visible for reviewers',()=>{
  const byOrdinal=new Map(review.lessons.map(x=>[x.ordinal,x]));
  assert.ok(byOrdinal.get(20).blockingFindings.join(' ').includes('Center'));
  assert.ok(byOrdinal.get(22).blockingFindings.join(' ').includes('partitions'));
  assert.ok(byOrdinal.get(24).blockingFindings.join(' ').includes('gallery'));
  assert.ok(byOrdinal.get(25).blockingFindings.join(' ').includes('gameplay'));
});
