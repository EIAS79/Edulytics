'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const { evidenceProgress, GATE_SIGNALS } =
  require('../tools/us-ccss-b01-evidence-progress.cjs');
const { createInitialLedger, gateNames, dimensionNames } =
  require('../tools/us-ccss-lesson-certification-records.cjs');
const candidate = '1'.repeat(40);
const sourceHash = 'a'.repeat(64), contentHash = 'b'.repeat(64);
function fixture() {
  const records = Array.from({ length: 1560 }, (_, i) => ({
    lessonCode: i === 0 ? 'PED:US-CCSS-MATH:G1:U01:L01' :
      'PED:US-CCSS-MATH:FIXTURE:' + String(i).padStart(4, '0'),
    title: 'Lesson ' + (i+1), gradeOrCourse: 'Grade 1',
    sourceUrl: 'https://example.org/' + i,
    sourceSemanticSha256: sourceHash,
    officialOutcomeCodes: ['CCSS:1.OA.C.5'], formalTargetCodes: []
  }));
  const source = {scope:'US-CCSS-MATH/CCSSM-2010/G1-G12',
    lessonCount:1560, records};
  const ledger = createInitialLedger(source);
  ledger.releaseCandidateSha = candidate;
  const manifest = {batchId:'B01',
    lessons:records.slice(0,500).map((r,i)=>({...r,ordinal:i+1}))};
  const report = {batchId:'B01', coverage:{joined:500,unique:500},
    lessons:manifest.lessons.map(x=>({
      ordinal:x.ordinal,lessonCode:x.lessonCode,sourceUrl:x.sourceUrl,
      sourceSemanticSha256:x.sourceSemanticSha256,
      effectiveContentSha256:contentHash,
      verdict:'BLOCKED_MISSING_EVIDENCE',
      confirmedDefects:[],
      evidenceSources:{
        publisher:'b01-publisher-goal-alignment-500',
        staticScreen:'b01-bulk-content-screen',
        effectiveRuntime:'b01-effective-content-and-practice',
        richV2:'b01-rich-v2-composition-500',
        sourceVisualMatch:'b01-rich-v2-publisher-visual-gaps',
        sourceQuestionMatch:'b01-500-lesson-practice-source-conflicts'
      }
    }))};
  return {manifest,source,ledger,report};
}
function certifyPilot(x) {
  const first=x.ledger.records[0];
  first.status='INTERNALLY_CERTIFIED';
  first.academicDecision='INTERNALLY_CERTIFIED';
  first.contentVersion='test';
  first.contentSha256=contentHash;
  first.gates=Object.fromEntries(gateNames.map(k=>[k,{
    status:'PASS',evidenceIds:['verified-'+k],
    verifiedAtUtc:'2026-10-13T01:00:00Z',sourceSha:candidate
  }]));
  first.reviewDimensions=Object.fromEntries(dimensionNames.map(k=>[k,{
    status:'PASS',evidenceIds:['verified-'+k],issueIds:[],
    verifiedAtUtc:'2026-10-13T01:00:00Z',
    contentSha256:contentHash,notApplicableReason:null
  }]));
  x.report.lessons[0].verdict='CORRECT';
}
test('a screened 500-row report NEVER promotes academic evidence',()=>{
  const x=fixture(), r=evidenceProgress(x);
  assert.equal(r.expected,500);
  assert.equal(r.evidenceReady,0);
  assert.equal(r.remainingEvidence,500);
  assert.equal(r.gateProgress.L2.evidencePass,0);
  assert.equal(r.gateProgress.L5.missing,500);
  assert.equal(r.lessons[42].sixGateEvidence.L5.status,'MISSING_VERIFIED_EVIDENCE');
  assert.ok(r.lessons[42].sixGateEvidence.L5.candidateArtifactSources.length>0);
  assert.equal(r.state,'BATCH_NO-GO');
});
test('uses existing certified pilot once, never treats 499 screens as proof',()=>{
  const x=fixture();certifyPilot(x);
  const r=evidenceProgress(x);
  assert.equal(r.evidenceReady,1);
  assert.equal(r.historicalCertifiedReused,1);
  assert.equal(r.remainingEvidence,499);
  for(const k of gateNames)assert.equal(r.gateProgress[k].evidencePass,1);
  for(const k of dimensionNames)assert.equal(r.dimensionProgress[k].evidencePass,1);
  assert.equal(r.lessons[0].evidenceReady,true);
  assert.equal(r.lessons[1].evidenceReady,false);
});
test('a defect on certified pilot invalidates progress without rewriting the ledger',()=>{
  const x=fixture();certifyPilot(x);
  x.report.lessons[0].confirmedDefects=[{code:'MATH_ERROR',dimension:'solutions'}];
  x.report.lessons[0].verdict='NEEDS_REPAIR';
  const r=evidenceProgress(x);
  assert.equal(r.evidenceReady,0);
  assert.equal(r.gateProgress.L2.staleOrDefective,1);
  assert.equal(r.defectClusters[0].code,'MATH_ERROR');
  assert.equal(x.ledger.records[0].gates.L2.status,'PASS');
});
test('changed learner-body fingerprint invalidates all previously proven gates',()=>{
  const x=fixture();certifyPilot(x);
  x.report.lessons[0].effectiveContentSha256='c'.repeat(64);
  const r=evidenceProgress(x);
  assert.equal(r.evidenceReady,0);
  assert.equal(r.contentFingerprintMismatches,1);
  assert.equal(r.gateProgress.L1.staleOrDefective,1);
  assert.equal(r.dimensionProgress.visualRepresentations.staleOrDefective,1);
});
test('missing gate proof cannot be fabricated by a PASS flag',()=>{
  const x=fixture();certifyPilot(x);
  x.ledger.records[0].gates.L6.evidenceIds=[];
  assert.throws(()=>evidenceProgress(x),/PASS without evidence/);
});
test('rejects broken 500-way joins and source changes',()=>{
  let x=fixture();x.report.lessons[1].lessonCode=x.report.lessons[0].lessonCode;
  assert.throws(()=>evidenceProgress(x),/re-ordered lesson/);
  x=fixture();x.manifest.lessons[7].sourceSemanticSha256='e'.repeat(64);
  assert.throws(()=>evidenceProgress(x),/Source fingerprint/);
  x=fixture();x.report.lessons.pop();
  assert.throws(()=>evidenceProgress(x),/exactly 500 joined/);
});
test('gate evidence candidates remain pointer-only metadata',()=>{
  const x=fixture(),r=evidenceProgress(x);
  assert.deepEqual(GATE_SIGNALS.L6,['richV2','effectiveRuntime']);
  assert.equal(r.lessons[4].sixGateEvidence.L6.ledgerStatus,'NOT_STARTED');
  assert.equal(r.lessons[4].sixGateEvidence.L6.status,'MISSING_VERIFIED_EVIDENCE');
});
