'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const s=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G10/owner-approved-source-scope.json');
const cov=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G10/source-toc-and-body-coverage.json');
test('Owner approved exactly seven available PDFs and no acquisition prerequisite',()=>{
  assert.equal(s.approvedFiles.length,7);
  assert.equal(s.additionalBookAcquisitionRequired,false);
  assert.equal(s.finalPathway,'General');
  assert.equal(s.legacyVersionsAllowed,true);
  assert.equal(cov.additionalExternalBooksRequired,false);
  assert.ok(cov.sourceGaps.every(x=>x.acquisitionRequired===false));
});
test('Complete General source and completed General lessons are separate',()=>{
  assert.equal(s.authoritativeGeneralBook.tocUnitCount,7);
  assert.deepEqual(s.authoritativeGeneralBook.tocLessonCounts,[9,6,6,6,9,6,6]);
  assert.equal(s.authoritativeGeneralBook.tocLessonCount,48);
  assert.equal(s.authoritativeGeneralBook.pdfPages,537);
  assert.equal(s.authoritativeGeneralBook.all48AppLessonsVerified,false);
  assert.equal(s.sourceScopeSemanticClosure,'PENDING');
  assert.equal(s.canonicalLessonCount,null);
  assert.equal(s.testsExecutionPolicy,'WRITTEN_NOT_RUN_FOR_STAGE_A_B_C');
});
