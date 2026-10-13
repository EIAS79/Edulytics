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
  let certified = 0;
  const outstanding = [];
  for (const entry of manifest.lessons) {
    const record = all.get(entry.lessonCode);
    assert(record, 'Missing canonical ledger row: ' + entry.lessonCode);
    const passed = record.status === 'INTERNALLY_CERTIFIED' &&
      record.academicDecision === 'INTERNALLY_CERTIFIED' &&
      record.unresolvedDefects.length === 0;
    if (passed) certified++;
    else outstanding.push({ ordinal: entry.ordinal,
      lessonCode: entry.lessonCode, state: record.status });
  }
  return {
    batchId: 'B01', total: manifest.lessons.length, certified,
    remaining: manifest.lessons.length - certified,
    state: certified === manifest.lessons.length ?
      'BATCH_CERTIFIED_PENDING_CI_AND_DEPLOY' : 'BATCH_NO-GO',
    nextExactLesson: outstanding[0]?.lessonCode ?? null,
    outstandingPreview: outstanding.slice(0, 12),
    note: 'Release requires separate green CI and verified Render Live/health.'
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
  process.stdout.write(JSON.stringify(result, null, 2) + '\n');
  if (releaseCheck && result.certified !== result.total) {
    throw Error('RELEASE BLOCKED: B01 has ' + result.remaining +
      ' uncertified lessons; do not merge academic content into main.');
  }
  return result;
}

if (require.main === module) {
  try { main(); }
  catch (err) {
    console.error('[US CCSS B01 release gate] ' + err.message);
    process.exitCode = 1;
  }
}
module.exports = { inspectBatch, main };
