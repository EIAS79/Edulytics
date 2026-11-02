#!/usr/bin/env node
'use strict';

// Edulytics US CCSS six-gate INTERNAL lesson evidence ledger.
// Structural validation only: the checker NEVER fabricates reviews,
// signs anything, or treats general CI success as subject-level approval.
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const auditPath = path.join(root, 'artifacts/academic-us-ccss/04-lesson-academic-audit.json');
const ledgerPath = path.join(root, 'artifacts/academic-us-ccss/04-lesson-certification-records.json');
const policyId = 'EDULYTICS-INTERNAL-SOURCE-VERIFIED-CERTIFICATION-V2';
const gateNames = ['L1', 'L2', 'L3', 'L4', 'L5', 'L6'];
const dimensionNames = [
  'titleAlignment', 'standardProvenance', 'mathematicalCorrectness',
  'instructionalProgression', 'workedExamples', 'visualRepresentations',
  'commonMisconceptions', 'questionsAndGrading'
];
const EXPECTED_LESSONS = 1560;
const SOURCE_SCOPE = 'US-CCSS-MATH/CCSSM-2010/G1-G12';
const assert = (ok, message) => { if (!ok) throw Error(message); };
const nonempty = s => typeof s === 'string' && s.trim().length > 0;
const isSha = s => typeof s === 'string' && /^[a-f0-9]{64}$/i.test(s);
const isCommitSha = s => typeof s === 'string' && /^[a-f0-9]{40}$/i.test(s);
const utc = s => typeof s === 'string' &&
  /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(?:\.\d{1,3})?Z$/.test(s) &&
  Number.isFinite(Date.parse(s)) &&
  new Date(s).toISOString().replace(/\.000Z$/, 'Z') === s.replace(/\.000Z$/, 'Z');

function loadSource(file = auditPath) {
  const source = JSON.parse(fs.readFileSync(file, 'utf8'));
  assert(source.scope === SOURCE_SCOPE && source.lessonCount === EXPECTED_LESSONS &&
    Array.isArray(source.records) && source.records.length === EXPECTED_LESSONS,
    'Source audit scope/count mismatch');
  const ids = source.records.map(x => x.lessonCode);
  assert(ids.every(nonempty) && new Set(ids).size === EXPECTED_LESSONS,
    'Source audit duplicate or missing lesson codes');
  for (const x of source.records)
    assert(nonempty(x.title) && nonempty(x.gradeOrCourse) && nonempty(x.sourceUrl) &&
      isSha(x.sourceSemanticSha256) && Array.isArray(x.officialOutcomeCodes) &&
      Array.isArray(x.formalTargetCodes), 'Incomplete authoritative source: ' + x.lessonCode);
  return source;
}
const emptyGate = () => ({ status: 'NOT_STARTED', evidenceIds: [], verifiedAtUtc: null, sourceSha: null });
const emptyDimension = () => ({
  status: 'NOT_STARTED', evidenceIds: [], issueIds: [],
  verifiedAtUtc: null, contentSha256: null, notApplicableReason: null
});
function createInitialLedger(source) {
  // Source metadata lives in the immutable authoritative audit file; do not
  // duplicate it in 1,560 pending records. Per-dimension evidence is allocated
  // only when work on a lesson actually starts (null means not reviewed).
  const records = source.records.map(x => ({
    lessonCode: x.lessonCode,
    contentVersion: null, contentSha256: null,
    gates: Object.fromEntries(gateNames.map(g => [g, emptyGate()])),
    reviewDimensions: null, unresolvedDefects: [],
    status: 'QUEUED', academicDecision: 'NOT_CERTIFIED'
  }));
  return {
    schemaVersion: 2, kind: 'LESSON_LEVEL_INTERNAL_CERTIFICATION_LEDGER', policyId,
    scope: SOURCE_SCOPE,
    authoritativeInput: 'artifacts/academic-us-ccss/04-lesson-academic-audit.json',
    releaseCandidateSha: null, intendedLessonCount: EXPECTED_LESSONS, records
  };
}
function validateLedger(source, ledger) {
  assert(ledger.schemaVersion === 2 &&
    ledger.kind === 'LESSON_LEVEL_INTERNAL_CERTIFICATION_LEDGER' &&
    ledger.policyId === policyId && ledger.scope === SOURCE_SCOPE &&
    ledger.intendedLessonCount === EXPECTED_LESSONS &&
    Array.isArray(ledger.records) && ledger.records.length === EXPECTED_LESSONS,
    'Six-gate internal ledger policy/schema/count mismatch');
  assert(ledger.releaseCandidateSha === null || isCommitSha(ledger.releaseCandidateSha),
    'Release candidate must be null or a full git SHA');
  const canonical = new Map(source.records.map(x => [x.lessonCode, x]));
  const seen = new Set();
  const states = new Set([
    'QUEUED', 'AUTOMATED_SCREENED', 'CONTENT_REVIEWED',
    'EVIDENCE_READY', 'INTERNALLY_CERTIFIED', 'BLOCKED', 'REWORK_REQUIRED'
  ]);
  const answer = { total: EXPECTED_LESSONS, certified: 0, evidenceReady: 0, blocked: 0 };
  for (const row of ledger.records) {
    const origin = canonical.get(row.lessonCode);
    assert(origin && !seen.has(row.lessonCode), 'Unknown or duplicate lesson: ' + row.lessonCode);
    seen.add(row.lessonCode);
    // Source title, publisher SHA, outcomes, grade and course identity are
    // rehydrated from the authoritative audit; never copied into this ledger.
    if (Object.hasOwn(row, 'sourceSemanticSha256'))
      assert(row.sourceSemanticSha256 === origin.sourceSemanticSha256,
        'Publisher source drift: ' + row.lessonCode);

    assert(row.gates && Object.keys(row.gates).length === 6 &&
      gateNames.every(g => Object.hasOwn(row.gates, g)),
      'Expected exactly six L1-L6 gates: ' + row.lessonCode);
    if (row.reviewDimensions !== null)
      assert(row.reviewDimensions && Object.keys(row.reviewDimensions).length === 8 &&
        dimensionNames.every(g => Object.hasOwn(row.reviewDimensions, g)),
        'Expected eight lesson dimensions when review starts: ' + row.lessonCode);
    assert(row.contentSha256 === null || isSha(row.contentSha256),
      'Invalid effective content SHA: ' + row.lessonCode);
    assert(states.has(row.status) && Array.isArray(row.unresolvedDefects),
      'Invalid status/defects: ' + row.lessonCode);

    for (const g of gateNames) {
      const item = row.gates[g];
      assert(item && ['NOT_STARTED','IN_PROGRESS','PASS','FAIL','BLOCKED'].includes(item.status) &&
        Array.isArray(item.evidenceIds) && item.evidenceIds.every(nonempty),
        'Invalid gate: ' + row.lessonCode + '/' + g);
      if (item.status === 'PASS')
        assert(item.evidenceIds.length && utc(item.verifiedAtUtc) &&
          isCommitSha(item.sourceSha) && isSha(row.contentSha256),
          'PASS without evidence/time/SHA: ' + row.lessonCode + '/' + g);
    }
    for (const d of row.reviewDimensions === null ? [] : dimensionNames) {
      const item = row.reviewDimensions[d];
      assert(item && ['NOT_STARTED','IN_PROGRESS','PASS','FAIL','BLOCKED','NOT_APPLICABLE'].includes(item.status) &&
        Array.isArray(item.evidenceIds) && item.evidenceIds.every(nonempty) &&
        Array.isArray(item.issueIds) && item.issueIds.every(nonempty),
        'Invalid dimension: ' + row.lessonCode + '/' + d);
      if (['PASS','NOT_APPLICABLE'].includes(item.status)) {
        assert(item.evidenceIds.length && item.issueIds.length === 0 &&
          utc(item.verifiedAtUtc) && item.contentSha256 === row.contentSha256 &&
          isSha(item.contentSha256),
          'Reviewed dimension missing SHA-bound evidence: ' + row.lessonCode + '/' + d);
        if (item.status === 'NOT_APPLICABLE')
          assert(d === 'visualRepresentations' && nonempty(item.notApplicableReason),
            'Only unneeded visuals may be NOT_APPLICABLE with reason');
      }
    }

    const l1to4 = gateNames.slice(0, 4).every(x => row.gates[x].status === 'PASS');
    const l1to6 = gateNames.every(x => row.gates[x].status === 'PASS');
    const eight = row.reviewDimensions !== null && dimensionNames.every(d =>
      ['PASS','NOT_APPLICABLE'].includes(row.reviewDimensions[d].status));
    if (row.reviewDimensions === null)
      assert(row.status === 'QUEUED' || row.status === 'AUTOMATED_SCREENED' ||
        row.status === 'REWORK_REQUIRED' || row.status === 'BLOCKED',
        'Missing review dimensions for reviewed lesson: ' + row.lessonCode);
    if (row.status === 'CONTENT_REVIEWED')
      assert(l1to4 && eight, 'CONTENT_REVIEWED lacks L1-L4 or eight dimensions: ' + row.lessonCode);
    if (row.status === 'EVIDENCE_READY') {
      assert(l1to6 && eight, 'EVIDENCE_READY lacks L1-L6 or eight dimensions: ' + row.lessonCode);
      answer.evidenceReady++;
    }
    if (row.academicDecision === 'INTERNALLY_CERTIFIED' ||
      row.status === 'INTERNALLY_CERTIFIED') {
      assert(row.academicDecision === 'INTERNALLY_CERTIFIED' &&
        row.status === 'INTERNALLY_CERTIFIED' &&
        l1to6 && eight && row.unresolvedDefects.length === 0 &&
        nonempty(row.contentVersion) && isSha(row.contentSha256) &&
        isCommitSha(ledger.releaseCandidateSha) &&
        gateNames.every(g => row.gates[g].sourceSha === ledger.releaseCandidateSha),
        'Internal certification requires all six proven gates, eight dimensions, exact candidate SHA, no defects: ' +
        row.lessonCode);
      answer.certified++;
    } else {
      assert(row.academicDecision === 'NOT_CERTIFIED',
        'Uncertified lesson has incorrect academic decision: ' + row.lessonCode);
    }
    if (row.status === 'BLOCKED' || row.status === 'REWORK_REQUIRED') answer.blocked++;
  }
  assert(seen.size === EXPECTED_LESSONS, 'Missing unique lesson records');
  return answer;
}
function run(argv = process.argv.slice(2)) {
  const init = argv.includes('--init'), check = argv.includes('--check');
  assert(init !== check, 'Choose exactly --init or --check');
  const source = loadSource();
  if (init) {
    assert(!fs.existsSync(ledgerPath), 'Ledger already exists; never overwrite audit work');
    const ledger = createInitialLedger(source);
    fs.writeFileSync(ledgerPath, JSON.stringify(ledger, null, 2) + '\n', { flag: 'wx' });
    process.stdout.write(JSON.stringify({ operation: 'init', ...validateLedger(source, ledger) }) + '\n');
  } else {
    assert(fs.existsSync(ledgerPath), 'Ledger missing');
    const ledger = JSON.parse(fs.readFileSync(ledgerPath, 'utf8'));
    process.stdout.write(JSON.stringify({ operation: 'check', ...validateLedger(source, ledger) }) + '\n');
  }
}
if (require.main === module) {
  try { run(); }
  catch (e) { console.error('[US CCSS six-gate internal ledger] ' + e.message); process.exitCode = 1; }
}
module.exports = { loadSource, createInitialLedger, validateLedger, run,
  EXPECTED_LESSONS, gateNames, dimensionNames, policyId };
