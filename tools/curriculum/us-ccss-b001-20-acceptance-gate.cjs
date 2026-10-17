#!/usr/bin/env node
'use strict';

/**
 * Fail-closed B001 20-lesson V2 P5 EVIDENCE READINESS gate.
 * This only reads genuine P4 academic records and independent GitHub checks.
 * It does not run tests, confer external certification or claim any deployment.
 * The legacy 500-cohort acceptance gate remains unchanged for historical audit.
 */
const fs = require('node:fs');
const path = require('node:path');
const assert = require('node:assert/strict');
const {fetchChecks} = require('../us-ccss-t001-test-acceptance.cjs');
const {buildManifest, outputPath, SIZE, PLAN} = require('./us-ccss-b001-20-inventory.cjs');

const fullSha = /^[0-9a-f]{40}$/i;
const sha256 = /^[0-9a-f]{64}$/i;
const evidenceKinds = Object.freeze([
  'source_provenance_and_licence',
  'effective_learner_visible_content',
  'lesson_specific_mathematics_oracle',
  'question_family_generation',
  'wrong_and_malformed_answer_rejection',
  'authenticated_practice_persistence',
  'student_and_school_isolation'
]);
const requiredChecks = Object.freeze([
  'b001-source-catalogue',
  'b001-math-capability-triage',
  'b001-500-engine-sampling',
  'b001-effective-learner-content-review',
  'b001-independent-source-content-and-math',
  'b001-authenticated-practice-persistence',
  'b001-20-inventory-and-fail-closed-gate'
]);
const isNonempty = x => typeof x === 'string' && x.trim() !== '';
const validRunUrl = x => typeof x === 'string' &&
  /^https:\/\/github\.com\/EIAS79\/Edulytics\/actions\/runs\/[1-9]\d*(?:\/(?:job|jobs)\/[1-9]\d*)?\/?$/.test(x);

function loadManifest(filename = outputPath) {
  const manifest = JSON.parse(fs.readFileSync(filename, 'utf8'));
  assert.equal(manifest.planId, PLAN, 'Old 500-plan manifest cannot be a 20-cohort');
  assert.equal(manifest.batchId, 'US-CCSS-B001');
  assert.equal(manifest.total, SIZE);
  assert.equal(manifest.lessons?.length, SIZE);
  assert.deepEqual(manifest, buildManifest(), '20 manifest is not exact canonical content');
  return manifest;
}
function noGo(reason, count = SIZE) {
  return {state:'NO_GO', acceptedCount:0, missingOrInvalidCount:count,
    reasons: [reason]};
}
function evaluateEvidence(manifest, evidence, candidateSha) {
  if (!fullSha.test(candidateSha || '')) throw Error('20-cohort gate needs exact full 40-char SHA');
  if (manifest?.planId !== PLAN || manifest?.total !== SIZE ||
      manifest?.lessons?.length !== SIZE || new Set(manifest.lessons.map(x=>x.lessonCode)).size!==SIZE)
    throw Error('Invalid exact 20-lesson canonical manifest');
  if (!evidence || evidence.schemaVersion !== 1 || evidence.batchId !== manifest.batchId ||
      evidence.planId !== PLAN || evidence.total !== SIZE ||
      evidence.candidateSha !== candidateSha || !Array.isArray(evidence.lessons) ||
      evidence.lessons.length !== SIZE)
    return noGo('Exact V2 20-cohort, candidate SHA and 20 per-lesson records required');

  const byCode = new Map();
  for (const row of evidence.lessons) {
    if (!row || !isNonempty(row.lessonCode) || byCode.has(row.lessonCode))
      return noGo('Duplicate or malformed 20-cohort evidence');
    byCode.set(row.lessonCode, row);
  }
  const expected = new Set(manifest.lessons.map(x=>x.lessonCode));
  if ([...byCode.keys()].some(x=>!expected.has(x)))
    return noGo('Evidence includes unrelated, out-of-batch lesson codes');
  const problems = [];
  for (const source of manifest.lessons) {
    const row = byCode.get(source.lessonCode);
    if (!row) {problems.push({lessonCode:source.lessonCode,why:'MISSING_EVIDENCE'});continue;}
    const why=[];
    if (row.sourceSemanticSha256 !== source.semanticSha256)why.push('SOURCE_SHA_MISMATCH');
    if (row.sourceEdition !== source.sourceEdition)why.push('SOURCE_EDITION_MISMATCH');
    if (row.catalogueContentVersion !== source.contentVersion)why.push('CONTENT_VERSION_MISMATCH');
    if (!sha256.test(row.effectiveBodySha256||''))why.push('EFFECTIVE_BODY_SHA_MISSING');
    if (row.decision !== 'INTERNALLY_ACCEPTED')why.push('NO_INDEPENDENT_ACADEMIC_ACCEPTANCE');
    if (row.candidateSha !== candidateSha)why.push('CANDIDATE_SHA_MISMATCH');
    if (source.historicalPreviouslyShipped && row.historicalReleaseDriftVerified !== true)
      why.push('HISTORIC_RELEASE_DRIFT_UNVERIFIED');
    if (!row.evidence || typeof row.evidence !== 'object' || Array.isArray(row.evidence)) {
      why.push('ACADEMIC_EVIDENCE_ABSENT');
    } else {
      for(const kind of evidenceKinds) {
        const p=row.evidence[kind];
        if(!p || p.result!=='pass' || p.candidateSha!==candidateSha ||
           !validRunUrl(p.runUrl) || !isNonempty(p.testId))
          why.push('MISSING_OR_STALE_'+kind.toUpperCase());
      }
    }
    if (why.length) problems.push({lessonCode:source.lessonCode,why});
  }
  return {state:problems.length?'NO_GO':'EVIDENCE_COMPLETENESS_READY',
    acceptedCount:SIZE-problems.length,missingOrInvalidCount:problems.length,
    reasons:problems.slice(0,SIZE)};
}
function evaluateChecks(checks,sha) {
  if (!fullSha.test(sha||'') || !Array.isArray(checks))throw Error('Invalid GitHub check runs');
  const results={};
  for(const name of requiredChecks){
    const verified=checks.filter(x=>x.name===name && x.head_sha===sha &&
      x.app?.slug==='github-actions' && x.status==='completed' && validRunUrl(x.html_url));
    const last=verified.sort((a,b)=>(b.id||0)-(a.id||0))[0];
    results[name]=last?{conclusion:last.conclusion,runUrl:last.html_url}
      :{conclusion:'missing',runUrl:null};
  }
  return {passed:Object.values(results).every(x=>x.conclusion==='success'),checks:results};
}
async function main(argv = process.argv.slice(2)) {
  const sha = argv.find(x=>x.startsWith('--sha='))?.slice(6);
  const filename = argv.find(x=>x.startsWith('--evidence='))?.slice(11);
  if(!fullSha.test(sha||''))throw Error('Provide --sha=<exact 40-character candidate SHA>');
  const manifest=loadManifest();
  const evidence=filename?JSON.parse(fs.readFileSync(path.resolve(filename),'utf8')):null;
  const inspected=evaluateEvidence(manifest,evidence,sha);
  let checked={passed:false,checks:{}};
  if(inspected.state==='EVIDENCE_COMPLETENESS_READY')
    checked=evaluateChecks(await fetchChecks('EIAS79/Edulytics',sha,process.env.GITHUB_TOKEN),sha);
  const eligible=inspected.state==='EVIDENCE_COMPLETENESS_READY'&&checked.passed;
  const result={
    planId:PLAN,batchId:'US-CCSS-B001',candidateSha:sha,indexedLessons:SIZE,
    evidenceAcceptedCount:inspected.acceptedCount,
    missingOrInvalidCount:inspected.missingOrInvalidCount,
    evidenceState:inspected.state,requiredChecks:checked.checks,
    outcome:eligible?'ELIGIBLE_FOR_DEPLOYMENT_VERIFICATION':'NO_GO',
    deployed:false,productionUatComplete:false,
    academicDecision:eligible?'INTERNAL_TEST_EVIDENCE_COMPLETE_PENDING_LIVE_UAT':'NOT_ACCEPTED',
    reasons:inspected.reasons,
    note:'No automatic release. Real 20/20 P5 proof, GitHub/Render deployment SHA, persisted learner Practice and owner UAT remain required.'
  };
  process.stdout.write(JSON.stringify(result,null,2)+'\n');
  if(!eligible)process.exitCode=1;
  return result;
}
if(require.main===module)main().catch(e=>{
  console.error('[B001 V2 20 FAIL-CLOSED] '+e.message);process.exitCode=1;
});
module.exports={loadManifest,evaluateEvidence,evaluateChecks,evidenceKinds,requiredChecks,main};
