'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const os=require('node:os');
const path=require('node:path');
const {execFileSync}=require('node:child_process');
const {build}=require('../tools/curriculum/us-ccss-b001-build-500-reviewer-worklist.cjs');
test('all 500 original academic reviewer packets are source-PDF/teacher-goal cross-checked and unsigned',()=>{
  const result=build();
  assert.equal(result.total,500);
  assert.equal(result.rows.length,500);
  assert.equal(new Set(result.rows.map(x=>x.lessonCode)).size,500);
  assert.equal(result.observedOriginalStudentPdfTextInspectionCount,7);
  assert.equal(result.qualifiedTeacherApprovals,0);
  assert.equal(result.independentGraphicsApprovals,0);
  assert.equal(result.independentMathApprovals,0);
  assert.equal(result.productionLearnerUatApprovals,0);
  assert.equal(result.academicReleaseDecision,'NO_GO');
  assert.equal(result.rows.filter(x=>x.pdfTextInspection==='NOT_INDIVIDUALLY_INSPECTED').length,493);
  assert.ok(result.rows.every(x=>x.releaseDecision==='NO_GO'&&
    x.teacherSignoff==='UNSIGNED'&&
    x.independentMathOracleApproval==='PENDING_HUMAN_REVIEW'&&
    x.graphicsVisualReview==='PENDING'&&
    x.realClassroomActivityUat==='PENDING'&&
    x.teacherSourceUrl.startsWith('https://')&&x.studentTaskPdfUrl.startsWith('https://')&&
    x.specificReviewGaps.length>0));
  assert.equal(result.rows[18].lessonCode,'PED:US-CCSS-MATH:G1:U02:L04');
  assert.equal(result.rows[24].lessonCode,'PED:US-CCSS-MATH:G1:U02:L10');
  assert.ok(result.rows[21].specificReviewGaps.join(' ').includes('partitions'));
});
test('emit a reviewable 500-row UTF-8 CSV and linked JSON artifact, not a certificate',()=>{
  const out=fs.mkdtempSync(path.join(os.tmpdir(),'b001-review-'));
  try{
    execFileSync(process.execPath,[
      path.resolve(__dirname,'../tools/curriculum/us-ccss-b001-build-500-reviewer-worklist.cjs'),
      '--out='+out
    ],{encoding:'utf8'});
    const csv=fs.readFileSync(path.join(out,'b001-500-source-pdf-math-human-review-worklist.csv'),'utf8');
    const json=JSON.parse(fs.readFileSync(path.join(out,'b001-500-source-pdf-math-human-review-worklist.json'),'utf8'));
    assert.equal(csv.trim().split('\n').length,501);
    assert.equal(json.rows.length,500);
    assert.equal(json.academicReleaseDecision,'NO_GO');
    assert.ok(!csv.includes('ACADEMICALLY_APPROVED'));
  }finally{
    fs.rmSync(out,{recursive:true,force:true});
  }
});
