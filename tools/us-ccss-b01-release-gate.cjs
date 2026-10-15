#!/usr/bin/env node
'use strict';

// B01 is an exactly-500-lesson, source-ordered certification and release unit.
// This gate never creates/replaces certification evidence or changes student data.
const fs = require('node:fs');
const path = require('node:path');
const {
  loadSource, validateLedger, policyId, gateNames, dimensionNames
} = require('./us-ccss-lesson-certification-records.cjs');

const root = path.resolve(__dirname, '..');
const manifestFile = path.join(root,
  'artifacts/academic-us-ccss/batches/B01-lesson-manifest.json');
const ledgerFile = path.join(root,
  'artifacts/academic-us-ccss/04-lesson-certification-records.json');
const isSha256 = x => typeof x === 'string' && /^[0-9a-f]{64}$/i.test(x);
const assert = (v, msg) => { if (!v) throw Error(msg); };

// The release subject is the frozen product/code commit. The evidence ledger
// is committed AFTER that subject and therefore cannot contain the HEAD SHA:
// doing so would recursively change HEAD. Only canonical certification evidence
// may change between the subject and final HEAD. Everything else invalidates the
// candidate and requires renewed, content-bound L1-L6 review.
const { execFileSync } = require('node:child_process');
const evidenceOnlyPaths = [
  /^artifacts[/]academic-us-ccss[/]04-lesson-certification-records[.]json$/,
  /^artifacts[/]academic-us-ccss[/]certification-evidence[/][A-Za-z0-9_.-]+[.]json$/
];
function actualCandidateProvenance(candidateSha, currentSha) {
  const git = args => execFileSync('git', args, {
    cwd: root, encoding: 'utf8', timeout: 15000, stdio: ['ignore','pipe','pipe']
  }).trim();
  assert(git(['rev-parse','HEAD']) === currentSha,
    'Final release GITHUB_SHA must equal the checked-out Git HEAD');
  let isAncestor = false;
  try {
    git(['merge-base','--is-ancestor',candidateSha,currentSha]);
    isAncestor = true;
  } catch {
    // A missing or unrelated baseline must fail closed; never silently assume
    // a shallow checkout is proof of ancestry.
  }
  assert(isAncestor,
    'STALE B01 candidate: frozen releaseCandidateSha is not an ancestor of HEAD');
  const names = git(['diff','--name-only','--no-renames',candidateSha,currentSha]);
  return { headSha: currentSha, ancestorVerified: true,
    changedPaths: names ? names.split('\n') : [] };
}
function requireCurrentCandidateSha(ledger, currentSha, verifiedGitProvenance) {
  assert(typeof currentSha === 'string' && /^[a-f0-9]{40}$/i.test(currentSha),
    'Final B01 release requires immutable current HEAD SHA (GITHUB_SHA or --candidate-sha=40hex)');
  const candidateSha = ledger?.releaseCandidateSha;
  assert(typeof candidateSha === 'string' && /^[a-f0-9]{40}$/i.test(candidateSha),
    'Final B01 release requires a frozen 40-character releaseCandidateSha');
  const proof = verifiedGitProvenance ??
    actualCandidateProvenance(candidateSha, currentSha);
  assert(proof?.headSha === currentSha && proof.ancestorVerified === true &&
    Array.isArray(proof.changedPaths),
    'Release requires verified Git ancestry and exact HEAD provenance');
  const disallowed = proof.changedPaths.filter(file =>
    typeof file !== 'string' || !evidenceOnlyPaths.some(pattern => pattern.test(file)) ||
    file.includes('..') || file.includes(String.fromCharCode(92)));
  assert(disallowed.length === 0,
    'STALE B01 candidate: product, source, validator or workflow changed since frozen SHA: ' +
    disallowed.slice(0, 10).join(', '));
  return candidateSha;
}

function inspectBatch({ manifest, source, ledger }) {
  assert(manifest.schemaVersion === 1 &&
    manifest.kind === 'US_CCSS_ACADEMIC_500_LESSON_BATCH_MANIFEST' &&
    manifest.policyId === policyId &&
    manifest.executionMode === 'FIXED_500_LESSON_BATCHES' &&
    manifest.batchId === 'B01' && manifest.ordinalStart === 1 &&
    manifest.ordinalEnd === 500 && manifest.expectedLessonCount === 500 &&
    manifest.totalCurriculumLessons === 1560,
    'B01 policy, count or ordinal boundaries mismatch');

  assert(Array.isArray(manifest.lessons) && manifest.lessons.length === 500,
    'B01 must contain exactly 500 entries');
  assert(Array.isArray(source.records) && source.records.length === 1560,
    'Authoritative source inventory must contain 1,560 entries');

  const seen = new Set();
  for (const [index, entry] of manifest.lessons.entries()) {
    const expected = source.records[index];
    assert(entry.ordinal === index + 1, 'Invalid ordinal: ' + (index + 1));
    assert(typeof entry.lessonCode === 'string' &&
      !seen.has(entry.lessonCode), 'Missing or duplicate B01 LessonCode');
    seen.add(entry.lessonCode);
    assert(expected && entry.lessonCode === expected.lessonCode,
      'B01 source ordering mismatch at ordinal ' + (index + 1));
    assert(entry.title === expected.title &&
      entry.gradeOrCourse === expected.gradeOrCourse &&
      entry.sourceUrl === expected.sourceUrl &&
      entry.sourceSemanticSha256 === expected.sourceSemanticSha256 &&
      isSha256(entry.sourceSemanticSha256) &&
      JSON.stringify(entry.officialOutcomeCodes) ===
        JSON.stringify(expected.officialOutcomeCodes),
      'B01 provenance drift: ' + entry.lessonCode);
  }
  assert(manifest.lessons[0].lessonCode ===
    'PED:US-CCSS-MATH:G1:U01:L01' &&
    manifest.lessons[499].lessonCode ===
    'PED:US-CCSS-MATH:G4:U04:L20',
    'B01 endpoints changed');
  assert(manifest.existingCertifiedPilot?.lessonCode ===
    manifest.lessons[0].lessonCode,
    'Historic pilot must be the first B01 lesson');

  // Full policy check: structural evidence validity, all six gates,
  // 8 review dimensions, content SHA, source candidate, and zero defects.
  validateLedger(source, ledger);
  const all = new Map(ledger.records.map(r => [r.lessonCode, r]));
  const isCommitSha = x => typeof x === 'string' && /^[a-f0-9]{40}$/i.test(x);
  let evidenceReady = 0;
  let historicallyCertified = 0;
  let openDefects = 0;
  const outstanding = [];
  const byGrade = {};
  const missingGateProofTotals = Object.fromEntries(gateNames.map(g => [g,0]));
  const missingDimensionProofTotals = Object.fromEntries(dimensionNames.map(d => [d,0]));
  for (const entry of manifest.lessons) {
    const record = all.get(entry.lessonCode);
    assert(record, 'Missing canonical ledger coverage row: ' + entry.lessonCode);
    const grade = entry.gradeOrCourse;
    byGrade[grade] ??= { total: 0, evidenceReady: 0, outstanding: 0 };
    byGrade[grade].total++;
    openDefects += record.unresolvedDefects.length;
    // This is coverage evidence, NOT an individual new certification.
    const previouslyCertified = record.status === 'INTERNALLY_CERTIFIED' &&
      record.academicDecision === 'INTERNALLY_CERTIFIED';
    const allEvidenceReady = record.status === 'EVIDENCE_READY' &&
      record.academicDecision === 'NOT_CERTIFIED';
    const acceptedState = previouslyCertified || allEvidenceReady;
    const missingGates=gateNames.filter(g =>
      record.gates[g]?.status !== 'PASS' ||
      record.gates[g]?.sourceSha !== ledger.releaseCandidateSha);
    const missingDimensions=dimensionNames.filter(d => {
      const field=record.reviewDimensions?.[d];
      return !field || !['PASS','NOT_APPLICABLE'].includes(field.status) ||
        field.contentSha256 !== record.contentSha256;
    });
    for(const gate of missingGates)missingGateProofTotals[gate]++;
    for(const dimension of missingDimensions)missingDimensionProofTotals[dimension]++;
    const sixGates = missingGates.length === 0;
    const eightDimensions = missingDimensions.length === 0 &&
      record.reviewDimensions && Object.values(record.reviewDimensions).length === 8;
    const ready = acceptedState && sixGates && eightDimensions &&
      record.unresolvedDefects.length === 0 &&
      isSha256(record.contentSha256) && isCommitSha(ledger.releaseCandidateSha) &&
      typeof record.contentVersion === 'string' && record.contentVersion.length > 0;
    if (ready) {
      evidenceReady++;
      byGrade[grade].evidenceReady++;
      if (previouslyCertified) historicallyCertified++;
    } else {
      byGrade[grade].outstanding++;
      outstanding.push({ ordinal: entry.ordinal, lessonCode: entry.lessonCode,
        evidenceState: record.status, academicDecision: record.academicDecision,
        missingGates, missingDimensions,
        missingContentSha: !isSha256(record.contentSha256),
        missingContentVersion: !record.contentVersion,
        missingSixGateOrDimensions: !sixGates || !eightDimensions,
        unresolvedDefects: record.unresolvedDefects.length,
        noRecordedDefectsDoesNotMeanMathReviewed: true });
    }
  }
  return {
    batchId: 'B01', total: manifest.lessons.length,
    evidenceReady, historicalCertificationsReused: historicallyCertified,
    remainingEvidence: manifest.lessons.length - evidenceReady,
    openDefects, byGrade, missingGateProofTotals, missingDimensionProofTotals,
    releaseCandidateSha: ledger.releaseCandidateSha,
    evidenceGapRows: outstanding,
    noteNoDefectsRecordedDoesNotProveReviewed: true,
    state: evidenceReady === manifest.lessons.length && openDefects === 0 ?
      'BATCH_EVIDENCE_READY' : 'BATCH_NO-GO',
    gapsPreview: outstanding.slice(0, 15),
    decisionScope: 'ONE_COLLECTIVE_BATCH_DECISION_ONLY',
    note: 'Coverage does not imply L1-L6 correctness; 500 SHA-bound actual evidence records and full CI are mandatory. BATCH_CERTIFIED is decided collectively only by release-check.'
  };
}

function main(argv = process.argv.slice(2)) {
  const statusOnly = argv.includes('--status');
  const releaseCheck = argv.includes('--release-check');
  assert(statusOnly !== releaseCheck, 'Specify exactly --status or --release-check');
  const source = loadSource();
  const ledger = JSON.parse(fs.readFileSync(ledgerFile, 'utf8'));
  const manifest = JSON.parse(fs.readFileSync(manifestFile, 'utf8'));
  const result = inspectBatch({ manifest, source, ledger });
  if (releaseCheck) {
    // Never allow a stale 500-row ledger or a missing live release SHA.
    const explicit = argv.find(x => x.startsWith('--head-sha=')) ??
      argv.find(x => x.startsWith('--candidate-sha='));
    // GitHub pull_request GITHUB_SHA is normally a synthetic merge commit.
    // The release subject must instead be the explicitly checked-out PR HEAD.
    const currentSha = explicit ? explicit.slice(explicit.indexOf('=') + 1) :
      process.env.B01_RELEASE_HEAD_SHA || process.env.GITHUB_SHA;
    requireCurrentCandidateSha(ledger, currentSha);
  }
  if (releaseCheck && result.state !== 'BATCH_EVIDENCE_READY') {
    process.stdout.write(JSON.stringify(result, null, 2) + '\n');
    throw Error('COLLECTIVE RELEASE BLOCKED: B01 lacks evidence for ' +
      result.remainingEvidence + ' of 500 lessons or has ' + result.openDefects +
      ' unresolved defects. No partial or individual certifications.');
  }
  // This is ONE approval for the entire 500-lesson batch, not 500 rows
  // silently rewritten to certified.
  const decision = releaseCheck
    ? { ...result, state: 'BATCH_CERTIFIED', decisionScope:
        'ONE_AUTOMATIC_COLLECTIVE_DECISION', candidateSha: ledger.releaseCandidateSha }
    : result;
  process.stdout.write(JSON.stringify(decision, null, 2) + '\n');
  return decision;
}

if (require.main === module) {
  try { main(); }
  catch (err) {
    console.error('[US CCSS B01 release gate] ' + err.message);
    process.exitCode = 1;
  }
}
module.exports = { inspectBatch, requireCurrentCandidateSha, actualCandidateProvenance, main };
