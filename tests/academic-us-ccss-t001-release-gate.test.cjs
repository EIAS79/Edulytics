'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const {inspectTen}=require('../tools/us-ccss-t001-release-gate.cjs');
const {createInitialLedger,gateNames,dimensionNames,policyId}=
  require('../tools/us-ccss-lesson-certification-records.cjs');
const sourceSha='a'.repeat(64), bodySha='b'.repeat(64), commit='1'.repeat(40);
function fixture(){
  const records=Array.from({length:1560},(_,i)=>({
    lessonCode:i<10?'PED:US-CCSS-MATH:G1:U01:L'+String(i+1).padStart(2,'0'):
      'PED:US-CCSS-MATH:OTHER:'+String(i+1).padStart(4,'0'),
    title:'Lesson '+(i+1),gradeOrCourse:'Grade 1',
    sourceUrl:'https://im.kendallhunt.com/k5/teachers/example/'+i,
    sourceSemanticSha256:sourceSha,officialOutcomeCodes:['CCSS:1.OA.C.5'],
    formalTargetCodes:[]
  }));
  const source={scope:'US-CCSS-MATH/CCSSM-2010/G1-G12',lessonCount:1560,records};
  const ledger=createInitialLedger(source);
  ledger.releaseCandidateSha=commit;
  const manifest={
    schemaVersion:1,kind:'US_CCSS_ACADEMIC_10_LESSON_BATCH_MANIFEST',
    policyId,executionMode:'FIXED_10_LESSON_BATCHES',batchId:'T001',
    batchNumber:1,batchSize:10,totalBatches:156,totalCurriculumLessons:1560,
    ordinalStart:1,ordinalEnd:10,expectedLessonCount:10,
    lessons:records.slice(0,10).map((x,i)=>({
      ordinal:i+1,lessonCode:x.lessonCode,title:x.title,
      gradeOrCourse:x.gradeOrCourse,sourceUrl:x.sourceUrl,
      sourceSemanticSha256:x.sourceSemanticSha256,
      officialOutcomeCodes:x.officialOutcomeCodes
    }))
  };
  return {source,ledger,manifest};
}
function pass(row,certified=false){
  row.status=certified?'INTERNALLY_CERTIFIED':'EVIDENCE_READY';
  row.academicDecision=certified?'INTERNALLY_CERTIFIED':'NOT_CERTIFIED';
  row.contentVersion='source-matched-verified-content';
  row.contentSha256=bodySha;
  row.gates=Object.fromEntries(gateNames.map(g=>[g,{
    status:'PASS',evidenceIds:['realistic-test-fixture-'+g],
    sourceSha:commit,verifiedAtUtc:'2026-10-15T16:00:00Z'
  }]));
  row.reviewDimensions=Object.fromEntries(dimensionNames.map(d=>[d,{
    status:'PASS',evidenceIds:['realistic-test-fixture-'+d],
    issueIds:[],verifiedAtUtc:'2026-10-15T16:00:00Z',
    contentSha256:bodySha,notApplicableReason:null
  }]));
}
test('T001 is ten lessons, never the old 500 release unit',()=>{
 const x=fixture();pass(x.ledger.records[0],true);
 const r=inspectTen(x);
 assert.equal(r.total,10);assert.equal(r.evidenceReady,1);
 assert.equal(r.remainingEvidence,9);assert.equal(r.lessonGaps.length,9);
 assert.equal(r.lessonGaps[0].lessonCode,x.manifest.lessons[1].lessonCode);
 assert.equal(r.state,'BATCH_NO-GO');assert.equal(r.noAutomaticAcademicPass,true);
 assert.equal(r.missingGateProofTotals.L6,9);
 assert.equal(r.missingDimensionProofTotals.mathematicalCorrectness,9);
});
test('T001 requires exactly ten complete SHA-bound L1-L6 and eight-dimension evidence rows',()=>{
 const x=fixture();
 x.ledger.records.slice(0,10).forEach((r,i)=>pass(r,i===0));
 let r=inspectTen(x);
 assert.equal(r.evidenceReady,10);assert.equal(r.remainingEvidence,0);
 assert.equal(r.state,'BATCH_EVIDENCE_READY');
 x.ledger.records[9].gates.L5.sourceSha='2'.repeat(40);
 r=inspectTen(x);
 assert.equal(r.evidenceReady,9);assert.equal(r.state,'BATCH_NO-GO');
 assert.deepEqual(r.lessonGaps[0].missingGates,['L5']);
});
test('T001 rejects wrong ten-lesson source order, identity and source drift',()=>{
 const a=fixture();a.manifest.lessons[9].lessonCode=a.manifest.lessons[8].lessonCode;
 assert.throws(()=>inspectTen(a),/endpoints changed|missing, duplicate/);
 const b=fixture();b.manifest.lessons[1].sourceSemanticSha256='c'.repeat(64);
 assert.throws(()=>inspectTen(b),/source-drifted/);
 const c=fixture();c.manifest.ordinalEnd=500;
 assert.throws(()=>inspectTen(c),/fixed-ten/);
 const d=fixture();d.manifest.lessons.push(d.manifest.lessons[9]);
 assert.throws(()=>inspectTen(d),/exactly ten/);
});
test('T001 blocks an open defect without blocking unrelated 1550 lessons',()=>{
 const x=fixture();
 x.ledger.records.slice(0,10).forEach((r,i)=>pass(r,i===0));
 x.ledger.records[8].unresolvedDefects.push('PROVEN-SOURCE-DRIFT');
 const state=inspectTen(x);
 assert.equal(state.evidenceReady,9);
 assert.equal(state.openDefects,1);
 assert.equal(state.state,'BATCH_NO-GO');
 assert.equal(x.ledger.records[20].status,'QUEUED');
});
