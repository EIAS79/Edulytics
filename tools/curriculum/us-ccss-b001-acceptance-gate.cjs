#!/usr/bin/env node
'use strict';

/**
 * B001 fail-closed RELEASE READINESS gate, not independent proof of any claim
 * recorded in untrusted JSON. This enforces *completeness* and authentic
 * GitHub check runs for the exact candidate SHA, but cannot replace actual
 * lesson-level teaching, Practice, grading and production tests.
 */
const fs = require('node:fs');
const path = require('node:path');
const { fetchChecks } = require('../us-ccss-t001-test-acceptance.cjs');

const catalogueFile = path.resolve(__dirname, '../../docs/curriculum/batches/US-CCSS-B001-CATALOGUE.json');
const fullSha = /^[a-f0-9]{40}$/i;
const sha256 = /^[a-f0-9]{64}$/i;
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
  'b001-independent-source-content-and-math',
  'b001-authenticated-practice-persistence'
]);
const isNonempty = value => typeof value === 'string' && value.trim() !== '';
const isValidGitHubActionsUrl = value =>
  typeof value === 'string' &&
  /^https:\/\/github\.com\/EIAS79\/Edulytics\/actions\/runs\/[1-9]\d*(?:\/jobs\/[1-9]\d*)?(?:\/)?$/.test(value);

function loadCatalogue(filename = catalogueFile) {
  const catalogue = JSON.parse(fs.readFileSync(filename, 'utf8'));
  if (catalogue.batchId !== 'US-CCSS-B001' || catalogue.total !== 500 ||
      !Array.isArray(catalogue.lessons) || catalogue.lessons.length !== 500 ||
      new Set(catalogue.lessons.map(x => x.lessonCode)).size !== 500)
    throw Error('B001 canonical manifest must contain 500 distinct lesson codes');
  return catalogue;
}

function evaluateEvidence(catalogue, evidence, candidateSha) {
  if (!fullSha.test(candidateSha)) throw Error('B001 requires full 40-character candidate SHA');
  const details = [];
  if (!evidence || evidence.batchId !== 'US-CCSS-B001' ||
      evidence.schemaVersion !== 1 ||
      evidence.candidateSha !== candidateSha ||
      !Array.isArray(evidence.lessons) || evidence.lessons.length !== 500) {
    return { state: 'NO_GO', acceptedCount: 0, missingOrInvalidCount: 500,
      reasons: ['B001 exact candidate SHA and 500 lesson evidence records are required'] };
  }

  const byCode = new Map();
  for (const row of evidence.lessons) {
    if (!row || !isNonempty(row.lessonCode) || byCode.has(row.lessonCode))
      return { state: 'NO_GO', acceptedCount: 0, missingOrInvalidCount: 500,
        reasons: ['Duplicate or malformed lesson evidence records'] };
    byCode.set(row.lessonCode, row);
  }

  const canonical = new Set(catalogue.lessons.map(x => x.lessonCode));
  if ([...byCode.keys()].some(k => !canonical.has(k)))
    return { state: 'NO_GO', acceptedCount: 0, missingOrInvalidCount: 500,
      reasons: ['Evidence contains an unrelated curriculum/lesson'] };

  for (const source of catalogue.lessons) {
    const row = byCode.get(source.lessonCode);
    if (!row) { details.push({lessonCode: source.lessonCode, why: 'MISSING_EVIDENCE'}); continue; }
    const problems = [];
    if (row.sourceSemanticSha256 !== source.semanticSha256)
      problems.push('SOURCE_SHA_MISMATCH');
    if (row.sourceEdition !== source.sourceEdition)
      problems.push('SOURCE_EDITION_MISMATCH');
    if (row.catalogueContentVersion !== source.contentVersion)
      problems.push('CATALOGUE_CONTENT_VERSION_MISMATCH');
    if (!sha256.test(row.effectiveBodySha256)) problems.push('EFFECTIVE_BODY_SHA_MISSING');
    if (row.decision !== 'INTERNALLY_ACCEPTED') problems.push('NOT_ACCEPTED');
    if (!fullSha.test(row.candidateSha) || row.candidateSha !== candidateSha)
      problems.push('LESSON_SHA_MISMATCH');
    if (!row.evidence || typeof row.evidence !== 'object' || Array.isArray(row.evidence)) {
      problems.push('REQUIRED_EVIDENCE_MISSING');
    } else {
      for (const kind of evidenceKinds) {
        const v = row.evidence[kind];
        if (!v || v.result !== 'pass' || v.candidateSha !== candidateSha ||
            !isValidGitHubActionsUrl(v.runUrl) || !isNonempty(v.testId))
          problems.push('MISSING_OR_STALE_' + kind.toUpperCase());
      }
    }
    if (source.historicalT001 && row.historicalT001DriftVerified !== true)
      problems.push('HISTORICAL_T001_DRIFT_NOT_VERIFIED');
    if (problems.length) details.push({ lessonCode: source.lessonCode, why: problems });
  }
  const acceptedCount = 500 - details.length;
  return { state: details.length ? 'NO_GO' : 'EVIDENCE_COMPLETENESS_READY',
    acceptedCount, missingOrInvalidCount: details.length,
    reasons: details.slice(0, 20) };
}

function evaluateChecks(checks, candidateSha) {
  if (!Array.isArray(checks)) throw Error('GitHub check runs missing');
  const results = {};
  for (const name of requiredChecks) {
    const matching = checks.filter(x => x.name === name &&
      x.head_sha === candidateSha && x.app?.slug === 'github-actions' &&
      isValidGitHubActionsUrl(x.html_url) && x.status === 'completed');
    const current = matching.sort((a,b) => (b.id||0) - (a.id||0))[0];
    results[name] = current
      ? { conclusion: current.conclusion, runUrl: current.html_url }
      : { conclusion: 'missing', runUrl: null };
  }
  return {
    passed: Object.values(results).every(x => x.conclusion === 'success'),
    checks: results
  };
}

async function main(args = process.argv.slice(2)) {
  const sha = args.find(x => x.startsWith('--sha='))?.slice(6);
  const file = args.find(x => x.startsWith('--evidence='))?.slice(11);
  const catalogue = loadCatalogue();
  if (!sha || !fullSha.test(sha))
    throw Error('Provide --sha=<exact full candidate commit SHA>');
  let evidence = null;
  if (file) evidence = JSON.parse(fs.readFileSync(path.resolve(file), 'utf8'));
  const result = evaluateEvidence(catalogue, evidence, sha);
  let checks = {passed:false, checks:{}};
  if (result.state === 'EVIDENCE_COMPLETENESS_READY')
    checks = evaluateChecks(await fetchChecks('EIAS79/Edulytics',
      sha, process.env.GITHUB_TOKEN), sha);
  const ready = result.state === 'EVIDENCE_COMPLETENESS_READY' && checks.passed;
  const decision = {
    batchId: 'US-CCSS-B001', candidateSha: sha,
    indexedLessons: 500,
    evidenceAcceptedCount: result.acceptedCount,
    missingOrInvalidCount: result.missingOrInvalidCount,
    evidenceState: result.state,
    requiredChecks: checks.checks,
    outcome: ready ? 'ELIGIBLE_FOR_DEPLOYMENT_VERIFICATION' : 'NO_GO',
    deployed: false,
    productionUatComplete: false,
    academicDecision: ready ? 'INTERNAL_TEST_EVIDENCE_COMPLETE_PENDING_LIVE_UAT' : 'NOT_ACCEPTED',
    reasons: result.reasons,
    note: 'Never declare a 500-lesson production release from this gate alone: verify Render live SHA, real authenticated learner experience and owner UAT separately.'
  };
  process.stdout.write(JSON.stringify(decision, null, 2) + '\n');
  if (!ready) process.exitCode = 1;
  return decision;
}
if (require.main === module) main().catch(error => {
  console.error('[B001 FAIL-CLOSED GATE] '+error.message);
  process.exitCode = 1;
});
module.exports = {loadCatalogue, evaluateEvidence, evaluateChecks, evidenceKinds, requiredChecks, main};
