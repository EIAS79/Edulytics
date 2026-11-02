'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const catalogue=JSON.parse(fs.readFileSync(path.join(root,'docs/curriculum/batches/US-CCSS-B001-CATALOGUE.json'),'utf8'));
const sources=JSON.parse(fs.readFileSync(path.join(root,'docs/curriculum/batches/US-CCSS-B001-IM2021-STUDENT-TASKS-PDF-SOURCES-500.json'),'utf8'));

test('Exactly 500 edition-linked original student task sheet PDF URLs in canonical B001 order',()=>{
  assert.equal(sources.schemaVersion,1);
  assert.equal(sources.batchId,'US-CCSS-B001');
  assert.equal(sources.count,500);
  assert.equal(sources.lessons.length,500);
  assert.equal(catalogue.lessons.length,500);
  assert.equal(sources.allUrlsExtracted,true);
  assert.equal(sources.duplicateUrls,0);
  assert.equal(new Set(sources.lessons.map(x=>x.originalStudentTaskStatementPdfUrl)).size,500);
  assert.equal(new Set(sources.lessons.map(x=>x.lessonCode)).size,500);
  sources.lessons.forEach((lesson,i)=>{
    assert.equal(lesson.ordinal,i+1);
    assert.equal(lesson.lessonCode,catalogue.lessons[i].lessonCode);
    assert.equal(lesson.sourceTeacherPreparationUrl,catalogue.lessons[i].sourceUrl);
    assert.equal(lesson.sourceEdition,catalogue.lessons[i].sourceEdition);
    assert.match(lesson.originalStudentTaskStatementPdfUrl,
      /^https:\/\/cms-assets\.illustrativemathematics\.org\/[a-zA-Z0-9]+$/);
    assert.equal(lesson.pdfUrlPresentInArchivedTeacherPreparation,true);
    assert.equal(lesson.verifiedPdfHttpStatus,null);
    assert.equal(lesson.studentTaskContentReviewed,false);
    assert.equal(lesson.academicAccepted,false);
  });
});

test('No link inventory or a 16-PDF sample is mistaken for full-content academic approval',()=>{
  assert.equal(sources.pdfHttpVerifiedSampleSize,16);
  assert.equal(sources.pdfHttpVerifiedSampleSuccess,16);
  assert.match(sources.note,/NOT confirmation.*Student Book/i);
  assert.ok(sources.lessons.every(x=>!x.academicAccepted));
  assert.ok(sources.lessons.every(x=>!x.studentTaskContentReviewed));
});
