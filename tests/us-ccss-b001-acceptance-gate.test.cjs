'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const {
  loadCatalogue, evaluateEvidence, evaluateChecks, evidenceKinds, requiredChecks
} = require('../tools/curriculum/us-ccss-b001-acceptance-gate.cjs');

const candidateSha = 'a'.repeat(40);
const catalogue = loadCatalogue();
const proof = () => ({result:'pass',candidateSha,testId:'synthetic-test-only',
  runUrl:'https://github.com/EIAS79/Edulytics/actions/runs/1234567'});
function fixture() {
  return {schemaVersion:1,batchId:'US-CCSS-B001',candidateSha,
    lessons:catalogue.lessons.map(s=>({
      lessonCode:s.lessonCode,sourceSemanticSha256:s.semanticSha256,
      sourceEdition:s.sourceEdition,catalogueContentVersion:s.contentVersion,
      effectiveBodySha256:'b'.repeat(64),candidateSha,decision:'INTERNALLY_ACCEPTED',
      historicalT001DriftVerified:s.historicalT001,
      evidence:Object.fromEntries(evidenceKinds.map(k=>[k,proof()]))
    }))};
}
test('No evidence file, missing lessons or duplicate evidence must always be NO_GO',()=>{
  assert.equal(evaluateEvidence(catalogue,null,candidateSha).state,'NO_GO');
  const incomplete=fixture();incomplete.lessons.pop();
  assert.equal(evaluateEvidence(catalogue,incomplete,candidateSha).state,'NO_GO');
  const duplicate=fixture();duplicate.lessons[1]=duplicate.lessons[0];
  assert.equal(evaluateEvidence(catalogue,duplicate,candidateSha).state,'NO_GO');
});
test('Each canonical code, source, version and test SHA is verified independently',()=>{
  const sourceMismatch=fixture();sourceMismatch.lessons[18].sourceSemanticSha256='x'.repeat(64);
  assert.equal(evaluateEvidence(catalogue,sourceMismatch,candidateSha).missingOrInvalidCount,1);
  const missingProof=fixture();delete missingProof.lessons[499].evidence.authenticated_practice_persistence;
  assert.equal(evaluateEvidence(catalogue,missingProof,candidateSha).missingOrInvalidCount,1);
  const stale=fixture();stale.lessons[250].evidence.question_family_generation.candidateSha='c'.repeat(40);
  assert.equal(evaluateEvidence(catalogue,stale,candidateSha).missingOrInvalidCount,1);
  const fakeUrl=fixture();fakeUrl.lessons[250].evidence.question_family_generation.runUrl='https://evil.example/runs/1234';
  assert.equal(evaluateEvidence(catalogue,fakeUrl,candidateSha).missingOrInvalidCount,1);
  const notApproved=fixture();notApproved.lessons[249].decision='QUEUED';
  assert.equal(evaluateEvidence(catalogue,notApproved,candidateSha).missingOrInvalidCount,1);
  const badHistorical=fixture();badHistorical.lessons[0].historicalT001DriftVerified=false;
  assert.equal(evaluateEvidence(catalogue,badHistorical,candidateSha).missingOrInvalidCount,1);
});
test('Synthetic 500-row shape is NOT proof; authentic live checks are separately required',()=>{
  // The passing *shape* here uses entirely fabricated test data and is never
  // committed. It only verifies the validator's boundary logic.
  assert.equal(evaluateEvidence(catalogue,fixture(),candidateSha).state,'EVIDENCE_COMPLETENESS_READY');
  assert.equal(evaluateChecks([],candidateSha).passed,false);
  const checks=requiredChecks.map((name,i)=>({
    name,head_sha:candidateSha,status:'completed',conclusion:'success',
    id:i+1,app:{slug:'github-actions'},
    html_url:'https://github.com/EIAS79/Edulytics/actions/runs/12345'
  }));
  assert.equal(evaluateChecks(checks,candidateSha).passed,true);
  checks[2].conclusion='failure';
  assert.equal(evaluateChecks(checks,candidateSha).passed,false);
  checks[2].conclusion='success';
  checks[3].head_sha='0'.repeat(40);
  assert.equal(evaluateChecks(checks,candidateSha).passed,false);
  checks[3].head_sha=candidateSha;
  checks[3].app.slug='random-app';
  assert.equal(evaluateChecks(checks,candidateSha).passed,false);
});
