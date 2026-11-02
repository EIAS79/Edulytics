'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const {
  loadManifest,evaluateEvidence,evaluateChecks,evidenceKinds,requiredChecks
}=require('../tools/curriculum/us-ccss-b001-20-acceptance-gate.cjs');
const sha='a'.repeat(40), manifest=loadManifest();
const makeProof=()=>({result:'pass',candidateSha:sha,testId:'SYNTHETIC_SHAPE_TEST_ONLY',
  runUrl:'https://github.com/EIAS79/Edulytics/actions/runs/12345'});
function fixture(){
  return {schemaVersion:1,planId:manifest.planId,batchId:manifest.batchId,
    total:20,candidateSha:sha,lessons:manifest.lessons.map(x=>({
      lessonCode:x.lessonCode,sourceSemanticSha256:x.semanticSha256,
      sourceEdition:x.sourceEdition,catalogueContentVersion:x.contentVersion,
      effectiveBodySha256:'b'.repeat(64),candidateSha:sha,decision:'INTERNALLY_ACCEPTED',
      historicalReleaseDriftVerified:x.historicalPreviouslyShipped,
      evidence:Object.fromEntries(evidenceKinds.map(k=>[k,makeProof()]))
    }))};
}
test('Reject no academic evidence, incomplete cohort, legacy 500 evidence and duplicate/foreign codes',()=>{
  assert.equal(evaluateEvidence(manifest,null,sha).state,'NO_GO');
  const badPlan=fixture();badPlan.planId='EDULYTICS-US-CCSS-500-V2';
  assert.equal(evaluateEvidence(manifest,badPlan,sha).state,'NO_GO');
  const short=fixture();short.lessons.pop();
  assert.equal(evaluateEvidence(manifest,short,sha).state,'NO_GO');
  const duplicate=fixture();duplicate.lessons[1]=duplicate.lessons[0];
  assert.equal(evaluateEvidence(manifest,duplicate,sha).state,'NO_GO');
  const foreign=fixture();foreign.lessons[19].lessonCode='PED:US-CCSS-MATH:G1:U02:L06';
  assert.equal(evaluateEvidence(manifest,foreign,sha).state,'NO_GO');
  assert.throws(()=>evaluateEvidence({...manifest,total:500},fixture(),sha),/Invalid exact 20/);
});
test('Each of 20 independently checks source/edition/content/test-SHA, proof and historical drift',()=>{
  const valid=fixture();assert.equal(evaluateEvidence(manifest,valid,sha).acceptedCount,20);
  const source=fixture();source.lessons[18].sourceSemanticSha256='x'.repeat(64);
  assert.equal(evaluateEvidence(manifest,source,sha).missingOrInvalidCount,1);
  const missing=fixture();delete missing.lessons[19].evidence.lesson_specific_mathematics_oracle;
  assert.equal(evaluateEvidence(manifest,missing,sha).missingOrInvalidCount,1);
  const edition=fixture();edition.lessons[17].sourceEdition='unknown';
  assert.equal(evaluateEvidence(manifest,edition,sha).missingOrInvalidCount,1);
  const fake=fixture();fake.lessons[18].evidence.authenticated_practice_persistence.runUrl='https://evil.example/1';
  assert.equal(evaluateEvidence(manifest,fake,sha).missingOrInvalidCount,1);
  const wrongSha=fixture();wrongSha.lessons[1].evidence.source_provenance_and_licence.candidateSha='c'.repeat(40);
  assert.equal(evaluateEvidence(manifest,wrongSha,sha).missingOrInvalidCount,1);
  const historical=fixture();historical.lessons[12].historicalReleaseDriftVerified=false;
  assert.equal(evaluateEvidence(manifest,historical,sha).missingOrInvalidCount,1);
  const noApproval=fixture();noApproval.lessons[0].decision='TESTED_PENDING_ACADEMIC_VERIFICATION';
  assert.equal(evaluateEvidence(manifest,noApproval,sha).missingOrInvalidCount,1);
});
test('Synthetic schema completeness is not GitHub evidence or deployment permission',()=>{
  // Synthetic evidence here only tests the validation boundary, never real PASS.
  assert.equal(evaluateEvidence(manifest,fixture(),sha).state,'EVIDENCE_COMPLETENESS_READY');
  assert.equal(evaluateChecks([],sha).passed,false);
  const checks=requiredChecks.map((name,i)=>({
    name,head_sha:sha,status:'completed',conclusion:'success',id:i+1,
    app:{slug:'github-actions'},
    html_url:'https://github.com/EIAS79/Edulytics/actions/runs/12345'
  }));
  assert.equal(evaluateChecks(checks,sha).passed,true);
  checks[2].conclusion='failure';assert.equal(evaluateChecks(checks,sha).passed,false);
  checks[2].conclusion='success';checks[3].head_sha='0'.repeat(40);
  assert.equal(evaluateChecks(checks,sha).passed,false);
  checks[3].head_sha=sha;checks[3].app.slug='untrusted-app';
  assert.equal(evaluateChecks(checks,sha).passed,false);
});
