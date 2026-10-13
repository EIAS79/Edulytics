#!/usr/bin/env node
'use strict';

// B01 is an exactly-500-lesson, source-ordered certification and release unit.
// This gate never creates/replaces certification evidence or changes student data.
const fs = require('node:fs');
const path = require('node:path');
const {
  loadSource, validateLedger, policyId
} = require('./us-ccss-lesson-certification-records.cjs');

const root = path.resolve(__dirname, '..');
const manifestFile = path.join(root,
  'artifacts/academic-us-ccss/batches/B01-lesson-manifest.json');
const ledgerFile = path.join(root,
  'artifacts/academic-us-ccss/04-lesson-certification-records.json');
const isSha256 = x => typeof x === 'string' && /^[0-9a-f]{64}$/i.test(x);
const assert = (v, msg) => { if (!v) throw Error(msg); };

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
    const sixGates = ['L1','L2','L3','L4','L5','L6'].every(g =>
      record.gates[g]?.status === 'PASS' &&
      record.gates[g]?.sourceSha === ledger.releaseCandidateSha);
    const eightDimensions = record.reviewDimensions &&
      Object.values(record.reviewDimensions).length === 8 &&
      Object.values(record.reviewDimensions).every(d =>
        ['PASS','NOT_APPLICABLE'].includes(d.status) &&
        d.contentSha256 === record.contentSha256);
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
        evidenceState: record.status, missingSixGateOrDimensions: !sixGates ||
          !eightDimensions, unresolvedDefects: record.unresolvedDefects.length });
    }
  }
  return {
    batchId: 'B01', total: manifest.lessons.length,
    evidenceReady, historicalCertificationsReused: historicallyCertified,
    remainingEvidence: manifest.lessons.length - evidenceReady,
    openDefects, byGrade,
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
module.exports = { inspectBatch, main };
