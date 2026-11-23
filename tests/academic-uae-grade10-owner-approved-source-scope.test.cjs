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
test('General volume1 lessons do not stand for complete annual General syllabus',()=>{
  assert.equal(s.authoritativeGeneralBook.tocUnitCount,7);
  assert.deepEqual(s.authoritativeGeneralBook.tocLessonCounts,[9,6,6,6,9,6,6]);
  assert.equal(s.authoritativeGeneralBook.tocLessonCount,48);
  assert.equal(s.authoritativeGeneralBook.pdfPages,537);
  assert.equal(s.authoritativeGeneralBook.isCompleteAnnualGeneral,false);
  assert.equal(s.authoritativeGeneralBook.annualLessonCount,null);
  assert.equal(cov.generalAnnualSourceComplete,false);
  assert.equal(s.authoritativeGeneralBook.all48AppLessonsVerified,false);
  assert.equal(s.sourceScopeSemanticClosure,'PENDING');
  assert.equal(s.canonicalLessonCount,null);
  assert.equal(s.testsExecutionPolicy,'WRITTEN_NOT_RUN_FOR_STAGE_A_B_C');
});

test('No assumption that General 48 are annual total; older and partial volumes remain distinct',()=>{
  const vols=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G10/volume-and-term-coverage-correction.json');
  assert.equal(vols.generalAnnualLessonCount,null);
  assert.equal(vols.bookFiles.find(x=>x.name==='01.pdf').sourceLessonHeadingCountInAvailablePart,48);
  assert.equal(vols.bookFiles.find(x=>x.name==='01.pdf').annualPathwayComplete,false);
  assert.ok(vols.bookFiles.some(x=>x.name==='02-2.pdf' && x.edition==='2018–2019'));
  assert.equal(vols.bookFiles.filter(x=>x.pathway==='Applied').length,2);
});
