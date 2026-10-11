#!/usr/bin/env node
'use strict';

// Evidence-only, fail-closed internal US Common Core G1-G12 certification gate.
// Does not edit canonical curriculum, grade student work, modify databases or issue
// independent academic approval. The status is NO-GO until every source, academic,
// operational and human-signoff condition has machine-verifiable evidence.
const fs = require('node:fs');
const path = require('node:path');
const crypto = require('node:crypto');
const cp = require('node:child_process');

const root = path.resolve(__dirname, '..');
const artifactDir = path.join(root, 'artifacts', 'academic-us-ccss');
// Phase 11 is the PRE-DEPLOY approval gate. Phase 12 independently verifies
// the actual deployed SHA/digest. Requiring post-deploy proof before release
// authorization would create an impossible circular dependency.
// Dynamic human approvals must be supplied out-of-band after the candidate
// commit exists; a file committed inside its own SHA cannot reference that SHA.
const stageFlag = process.argv.find(a => a.startsWith('--stage='));
const stage = stageFlag ? stageFlag.slice('--stage='.length) : 'final';
if (!['predeploy', 'final'].includes(stage))
  throw Error('Allowed gate stages: predeploy or final');
const externalFlag = process.argv.find(a => a.startsWith('--approval-dir='));
const externalDir = externalFlag ? path.resolve(externalFlag.slice('--approval-dir='.length)) : null;
const externalNames = new Set(['stagingEvidence', 'academicReviews', 'releaseDecision', 'productionEvidence']);
const files = Object.freeze({
  structural: 'docs/curriculum/us-ccss-g1-g12-structural-audit.json',
  ledger: 'docs/curriculum/us-ccss-academic-evidence-ledger.json',
  clauses: 'docs/curriculum/us-ccss-121-clause-evidence-corrected.json',
  semantic: 'docs/curriculum/us-ccss-practice-semantic-blockers.json',
  textDiff: 'docs/curriculum/us-ccss-official-text-diff.json',
  lessonAudit: 'artifacts/academic-us-ccss/04-lesson-academic-audit.json',
  parentTrace: 'artifacts/academic-us-ccss/05-parent-standard-traceability.json',
  childTrace: 'artifacts/academic-us-ccss/05-child-clause-traceability.json',
  familyEvidence: 'artifacts/academic-us-ccss/06-question-family-acceptance.json',
  gradingEvidence: 'artifacts/academic-us-ccss/07-math-verification-evidence.json',
  routingEvidence: 'artifacts/academic-us-ccss/08-surface-routing-evidence.json',
  stagingEvidence: 'artifacts/academic-us-ccss/09-staging-acceptance.json',
  academicReviews: 'artifacts/academic-us-ccss/10-academic-review-decisions.json',
  releaseDecision: 'artifacts/academic-us-ccss/11-release-decision.json',
  productionEvidence: 'artifacts/academic-us-ccss/12-production-verification.json'
});
const source = {};
const problems = [];
const facts = {};
function check(ok, code, explanation) {
  if (!ok) problems.push({code, explanation});
  return Boolean(ok);
}
function load(name, required = true) {
  const file = externalDir && externalNames.has(name)
    ? path.join(externalDir, path.basename(files[name]))
    : path.join(root, files[name]);
  if (!fs.existsSync(file)) {
    if (required) check(false, 'MISSING_' + name.toUpperCase(), files[name]);
    return null;
  }
  try {
    const buffer = fs.readFileSync(file);
    source[name] = {path: externalDir && externalNames.has(name)
      ? '[external approvals]/' + path.basename(files[name]) : files[name],
      sha256: crypto.createHash('sha256').update(buffer).digest('hex')};
    return JSON.parse(buffer.toString('utf8'));
  } catch (error) {
    check(false, 'INVALID_' + name.toUpperCase(), String(error.message));
    return null;
  }
}
function rows(document) {
  if (Array.isArray(document)) return document;
  return document?.records ?? document?.rows ?? document?.clauses ??
    document?.lessons ?? document?.standards ?? document?.items ?? [];
}
function uniqueCount(data, field) {
  return new Set(rows(data).map(row => row[field]).filter(Boolean)).size;
}
function readyEvidence(name, expectedCount, id, approvedState) {
  const evidence = load(name, false);
  if (!check(Boolean(evidence), 'MISSING_' + name.toUpperCase(),
    'No independently reviewed ' + name + ' evidence')) return false;
  const list = rows(evidence);
  const reviewed = list.filter(row => row.status === approvedState &&
    row.sourceEvidenceId && row.reviewerId && row.reviewEvidenceId);
  const enough = list.length === expectedCount &&
    uniqueCount(evidence, id) === expectedCount && reviewed.length === expectedCount;
  return check(enough, 'INCOMPLETE_' + name.toUpperCase(),
    name + ': ' + reviewed.length + ' / ' + expectedCount + ' reviewed records');
}
function explicitPass(name, requiredFields) {
  const evidence = load(name, false);
  if (!check(Boolean(evidence), 'MISSING_' + name.toUpperCase(), files[name])) return false;
  const ok = evidence.status === 'PASS' && requiredFields.every(field =>
    typeof evidence[field] === 'string' && evidence[field].trim().length > 0);
  return check(ok, 'INCOMPLETE_' + name.toUpperCase(),
    name + ' needs PASS and explicit, non-empty evidence fields: ' + requiredFields.join(', '));
}
const sha = (() => {
  try { return cp.execFileSync('git', ['rev-parse', 'HEAD'], {cwd: root, encoding: 'utf8'}).trim(); }
  catch { return null; }
})();
check(Boolean(sha), 'NO_SOURCE_SHA', 'Git checkout SHA is mandatory');
const structural = load('structural');
const ledger = load('ledger');
const clauses = load('clauses');
const semantic = load('semantic');
const diff = load('textDiff');
const totalLessons = structural?.lessonInventory?.lessons;
const parentCount = structural?.official?.codesInEngine;
const childCount = clauses?.summary?.allClauses;
const reported = ledger?.summary;
facts.scope = 'US-CCSS-MATH/CCSSM-2010/Grades-1-12';
facts.kindergartenExcluded = true;
facts.sourceCommitSha = sha;
facts.blueprintLessons = totalLessons ?? null;
facts.officialParentStandards = parentCount ?? null;
facts.subordinateClauses = childCount ?? null;
facts.childPublisherDirectRefs = clauses?.summary?.withPublisherExactDirectCode ?? null;
facts.childParentOnlyRefs = clauses?.summary?.withoutExactDirectCode ?? null;
facts.certifiedChildClauses = reported?.academicallyCertified ?? null;
facts.textDiffsPendingDisposition = reported?.textDifferencesPendingIndependentDisposition ?? null;
facts.textCorrectionsUnverifiedLive = reported?.correctionsPendingLiveNeonVerification ?? null;
facts.authenticatedLiveStaging = semantic?.authenticatedLiveStagingFlow ?? null;

check(totalLessons === 1560 && structural?.lessonInventory?.blueprints === 17,
  'LESSON_BASELINE_MISMATCH', '1560 lessons in 17 publisher blueprint sources expected; inventory must be independently regenerated');
check(parentCount === 363 && structural?.official?.missingInEngine?.length === 0,
  'OFFICIAL_STANDARDS_INCOMPLETE', '363 numbered parent standards and zero missing source codes required');
check(childCount === 121 && rows(clauses).length === 121 &&
  (clauses?.summary?.withPublisherExactDirectCode ?? 0) +
  (clauses?.summary?.withoutExactDirectCode ?? 0) === 121,
  'CHILD_CLAUSE_INVENTORY_INCOMPLETE', '121 clauses with traceable publisher evidence categories required');
check(diff?.sourceNodes === 363 && diff?.fetchErrors?.length === 0,
  'OFFICIAL_TEXT_SOURCE_UNVERIFIED', 'Independent official source diff requires 363 standards and zero fetch failures');
check(reported?.textDifferencesPendingIndependentDisposition === 0 &&
  (reported?.correctionsPendingLiveNeonVerification ?? -1) === 0,
  'OFFICIAL_TEXT_NOT_CLOSED', 'Every source difference must have an adjudication, and repairs must be verified live');
check(reported?.academicallyCertified === 121 &&
  rows(ledger?.clauseEvidence).length === 121 &&
  ledger.clauseEvidence.every(x => x.certificationStatus === 'ACADEMICALLY_CERTIFIED'),
  'SUBCLAUSE_NOT_CERTIFIED', '121 independent teaching, assessment and grading signoffs required');
check(semantic?.productionReady === true && semantic?.independentlyCertifiedChildClauses === 121,
  'SEMANTIC_RELEASE_BLOCKED', 'Cannot certify while practice semantic gate is NO-GO');
readyEvidence('lessonAudit', 1560, 'lessonCode', 'ACADEMICALLY_CERTIFIED');
readyEvidence('parentTrace', 363, 'standardCode', 'ACADEMICALLY_CERTIFIED');
readyEvidence('childTrace', 121, 'clauseCode', 'ACADEMICALLY_CERTIFIED');
for (const name of ['familyEvidence', 'gradingEvidence', 'routingEvidence']) {
  explicitPass(name, ['testEvidenceSha', 'academicScopeEvidenceId']);
}
explicitPass('stagingEvidence', ['commitSha', 'studentEvidenceId', 'teacherEvidenceId',
  'schoolAdminEvidenceId', 'crossSchoolIsolationEvidenceId']);
const reviews = load('academicReviews', false);
check(Boolean(reviews) && reviews.status === 'APPROVED' &&
  Array.isArray(reviews.independentReviewerSignoffs) &&
  reviews.independentReviewerSignoffs.length > 0 &&
  reviews.independentReviewerSignoffs.every(x => x.reviewerId && x.signedAtUtc && x.signedScope &&
    x.evidenceId) && reviews.openObjections === 0,
  'INDEPENDENT_ACADEMIC_APPROVAL_MISSING',
  'Human mathematics reviewers must independently sign the fixed scope, evidence and version');
const release = load('releaseDecision', false);
check(Boolean(release) && release.status === 'GO' && release.targetSha === sha &&
  release.academicApprovalEvidenceId && release.securityApprovalEvidenceId &&
  release.releaseOwnerApprovalEvidenceId, 'RELEASE_AUTHORIZATION_MISSING',
  'Release must be approved for the exact current SHA by academic/security/release owners');
if (stage === 'final') {
  const production = load('productionEvidence', false);
  check(Boolean(production) && production.status === 'PASS' && production.deployedSha === sha &&
    production.contentDigestEvidenceId && production.userJourneyEvidenceId,
    'PRODUCTION_VALIDATION_MISSING',
    'Deployed SHA, canonical digest and real user smoke proofs required');
}

const output = {
  schemaVersion: 1,
  certificationType: 'INTERNAL_EVIDENCE_BASED',
  gateStage: stage,
  scope: facts.scope,
  sourceCommitSha: sha,
  decision: problems.length === 0
    ? (stage === 'predeploy' ? 'RELEASE_CANDIDATE_APPROVED' : 'ACADEMICALLY_CERTIFIED_FULL')
    : 'NO-GO',
  facts, inputFiles: source, blockers: problems
};
if (process.argv.includes('--write-baseline')) {
  fs.mkdirSync(artifactDir, {recursive: true});
  fs.writeFileSync(path.join(artifactDir, '00-baseline.json'), JSON.stringify(output, null, 2) + '\n');
}
console.log(JSON.stringify(output, null, 2));
if (process.argv.includes('--strict') && output.decision === 'NO-GO') {
  process.exitCode = 2;
}
