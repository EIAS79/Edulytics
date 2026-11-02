'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const os=require('node:os');
const path=require('node:path');
const {execFileSync}=require('node:child_process');
const {build}=require('../tools/curriculum/us-ccss-b001-build-500-reviewer-worklist.cjs');
test('all 500 original source dossiers have reproducible review states without required teacher signatures',()=>{
  const result=build();
  assert.equal(result.total,500);
  assert.equal(result.rows.length,500);
  assert.equal(new Set(result.rows.map(x=>x.lessonCode)).size,500);
  assert.equal(result.observedOriginalStudentPdfTextInspectionCount,7);
  assert.equal(result.missingOriginalStudentFacingGoalExcerptCount,1);
  assert.equal(result.rows[179].lessonCode,'PED:US-CCSS-MATH:G2:U02:L16');
  assert.ok(result.rows[179].specificReviewGaps.join(' ').includes('student-facing learning-goal excerpt was absent'));

  assert.equal(result.internalAcademicAcceptedCount,0);
  assert.equal(result.sourceVisualParityVerifiedCount,0);
  assert.equal(result.mathOracleVerifiedCount,0);
  assert.equal(result.productionLearnerUatPassedCount,0);
  assert.equal(result.academicReleaseDecision,'NO_GO');
  assert.equal(result.rows.filter(x=>x.pdfTextInspection==='NOT_INDIVIDUALLY_INSPECTED').length,493);
  assert.ok(result.rows.every(x=>x.releaseDecision==='NO_GO'&&
    !Object.hasOwn(x,'teacherSignoff')&&
    x.mathOracleVerification==='PENDING_INDEPENDENT_ORACLE_EVIDENCE'&&
    x.sourceToLearnerParityVerification==='PENDING_SOURCE_ALIGNED_EVIDENCE'&&
    x.sourceVisualParityVerification==='PENDING_GRAPHICS_PARITY_EVIDENCE'&&
    x.studentInteractionVerification==='PENDING_LEARNER_PATH_UAT'&&
    x.teacherSourceUrl.startsWith('https://')&&x.studentTaskPdfUrl.startsWith('https://')&&
    x.specificReviewGaps.length>0));
  assert.equal(result.rows[18].lessonCode,'PED:US-CCSS-MATH:G1:U02:L04');
  assert.equal(result.rows[24].lessonCode,'PED:US-CCSS-MATH:G1:U02:L10');
  assert.ok(result.rows[21].specificReviewGaps.join(' ').includes('partitions'));
});
test('emit a reviewable 500-row UTF-8 CSV and linked JSON artifact, not a certificate or external human signature',()=>{
  const out=fs.mkdtempSync(path.join(os.tmpdir(),'b001-review-'));
  try{
    execFileSync(process.execPath,[
      path.resolve(__dirname,'../tools/curriculum/us-ccss-b001-build-500-reviewer-worklist.cjs'),
      '--out='+out
    ],{encoding:'utf8'});
    const csv=fs.readFileSync(path.join(out,'b001-500-source-pdf-math-internal-verification-worklist.csv'),'utf8');
    const json=JSON.parse(fs.readFileSync(path.join(out,'b001-500-source-pdf-math-internal-verification-worklist.json'),'utf8'));
    assert.equal(csv.trim().split('\n').length,501);
    assert.equal(json.rows.length,500);
    assert.equal(json.academicReleaseDecision,'NO_GO');
    assert.ok(!csv.includes('ACADEMICALLY_APPROVED'));
  }finally{
    fs.rmSync(out,{recursive:true,force:true});
  }
});
