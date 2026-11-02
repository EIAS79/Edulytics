'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const os=require('node:os');
const path=require('node:path');
const {execFileSync}=require('node:child_process');
const {build}=require('../tools/curriculum/us-ccss-b001-source-remediation-500.cjs');

test('all 500 original 2021 teaching goals, canonical source hashes, outcomes and raw packs are reconciled',()=>{
  const data=build();
  assert.equal(data.scope,500);
  assert.equal(data.rows.length,500);
  assert.equal(new Set(data.rows.map(r=>r.lessonCode)).size,500);
  assert.deepEqual(data.rows.map(x=>x.ordinal),Array.from({length:500},(_,i)=>i+1));
  assert.deepEqual([
    data.grades['Grade 1'].total,
    data.grades['Grade 2'].total,
    data.grades['Grade 3'].total,
    data.grades['Grade 4'].total
  ],[146,146,143,65]);
  assert.equal(data.rows.filter(r=>r.historicalFirst15PreviouslyPublished).length,15);
  assert.equal(data.rows[499].lessonCode,'PED:US-CCSS-MATH:G4:U04:L20');
  assert.equal(data.rows[179].lessonCode,'PED:US-CCSS-MATH:G2:U02:L16');
  assert.equal(data.rows[179].originalStudentGoalExcerptPresent,false);
  assert.equal(data.rows[179].originalGoalAbsenceVerifiedOptional,true);
  assert.deepEqual(data.rows[179].reviewFlags.filter(x=>x.includes('GOAL')),
    ['OPTIONAL_LESSON_NO_SEPARATE_STUDENT_GOAL_IN_SOURCE']);
  assert.ok(data.rows[179].nextAction.includes('market inventory'));
  assert.equal(data.grades['Grade 2'].missingStudentGoal,1);
  assert.equal(data.grades['Grade 2'].verifiedOptionalWithoutGoal,1);
  assert.equal(data.priorityCounts.P0_SOURCE_GOAL??0,0);
  assert.ok(data.grades['Grade 1'].rawGenericExamples>30);
  assert.ok(data.grades['Grade 2'].repeatedRawStepTemplates>20);
  assert.ok(data.grades['Grade 3'].repeatedRawStepTemplates>20);
  // The Grade 4 first-65 canonical bodies have since been individually authored;
  // the old >10 repeated-template expectation was a stale baseline, not an
  // invariant. Preserve the exact zero measurement instead of fabricating risk.
  assert.equal(data.grades['Grade 4'].repeatedRawStepTemplates,0);
  assert.ok(data.rows.filter(row=>row.grade==='Grade 4').every(row=>
    row.rawEmbeddedNormalizedStepTemplateUsesInGrade===1));
  assert.equal(Object.values(data.priorityCounts).reduce((a,n)=>a+n,0),500);
  assert.equal(data.independentAcademicAcceptedInThisAudit,0);
  assert.equal(data.fullBatchPublishAuthorization,false);
  assert.equal(data.releaseDecision,'NO_GO');
  assert.ok(data.rows.every(row=>
    /^[0-9a-f]{64}$/.test(row.rawContentSha256) &&
    /^[0-9a-f]{64}$/.test(row.sourceSemanticSha256) &&
    row.sourceUrl.startsWith('https://') &&
    row.originalTeacherPurpose.length>0 &&
    row.internalAcceptance==='NOT_GRANTED_BY_SOURCE_RISK_AUDIT' &&
    row.currentRenderedSourceParity==='NOT_VERIFIED_BY_SOURCE_RISK_AUDIT' &&
    row.lessonIndependentMathProof==='NOT_VERIFIED_BY_SOURCE_RISK_AUDIT' &&
    row.lessonAuthenticatedProductionUat==='NOT_VERIFIED_BY_SOURCE_RISK_AUDIT' &&
    row.nextAction.length>0));
});

test('the 500-source risk artifact never fabricates a lesson PASS or academic acceptance',()=>{
  const out=fs.mkdtempSync(path.join(os.tmpdir(),'b001-500-risk-'));
  try{
    execFileSync(process.execPath,[
      path.resolve(__dirname,'../tools/curriculum/us-ccss-b001-source-remediation-500.cjs'),
      '--out='+out
    ],{encoding:'utf8'});
    const report=JSON.parse(fs.readFileSync(path.join(out,
      'source-aligned-500-lesson-remediation-ledger.json'),'utf8'));
    assert.equal(report.rows.length,500);
    assert.equal(report.releaseDecision,'NO_GO');
    assert.equal(report.independentAcademicAcceptedInThisAudit,0);
    assert.ok(report.rows.every(row=>row.reviewPriority.startsWith('P')&&
      row.internalAcceptance!=='INTERNALLY_ACCEPTED'));
  }finally{
    fs.rmSync(out,{force:true,recursive:true});
  }
});
