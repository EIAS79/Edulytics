#!/usr/bin/env node
'use strict';

// Phase 04 evidence ledger bootstrap and fail-closed structural checker.
// This tool does NOT review or certify lessons; it preserves later human decisions.
const fs = require('node:fs');
const path = require('node:path');

const root = path.resolve(__dirname, '..');
const auditPath = path.join(root, 'artifacts/academic-us-ccss/04-lesson-academic-audit.json');
const recordsPath = path.join(root, 'artifacts/academic-us-ccss/04-lesson-certification-records.json');
const gateNames = ['L1','L2','L3','L4','L5','L6','L7'];
const EXPECTED_LESSONS = 1560;
const SOURCE_SCOPE = 'US-CCSS-MATH/CCSSM-2010/G1-G12';

const assert = (condition, message) => {
  if (!condition) throw new Error(message);
};
const nonempty = value => typeof value === 'string' && value.trim().length > 0;
const isSha = value => typeof value === 'string' && /^[a-f0-9]{64}$/i.test(value);

function loadSource(file = auditPath) {
  const input = JSON.parse(fs.readFileSync(file, 'utf8'));
  assert(input.scope === SOURCE_SCOPE, 'Source inventory scope mismatch');
  assert(input.lessonCount === EXPECTED_LESSONS, 'Source audit lesson count drift');
  assert(Array.isArray(input.records) && input.records.length === EXPECTED_LESSONS,
    'Missing source lesson records');
  const codes = input.records.map(x => x.lessonCode);
  assert(codes.every(nonempty) && new Set(codes).size === EXPECTED_LESSONS,
    'Source audit duplicate or missing lesson codes');
  for (const row of input.records) {
    assert(nonempty(row.title) && nonempty(row.gradeOrCourse) &&
      nonempty(row.sourceUrl) && isSha(row.sourceSemanticSha256),
      'Incomplete publisher/source identity: ' + row.lessonCode);
    assert(Array.isArray(row.officialOutcomeCodes) && Array.isArray(row.formalTargetCodes),
      'Invalid official outcome arrays: ' + row.lessonCode);
  }
  return input;
}

const emptyGate = () => ({ status: 'NOT_STARTED', evidenceIds: [], verifiedAtUtc: null });
function createInitialLedger(source) {
  const records = source.records.map(row => ({
    lessonCode: row.lessonCode,
    title: row.title,
    gradeOrCourse: row.gradeOrCourse,
    blueprintCode: row.blueprintCode,
    pathway: row.pathway ?? null,
    courseCode: row.courseCode ?? null,
    publisherSourceUrl: row.sourceUrl,
    sourceSemanticSha256: row.sourceSemanticSha256,
    contentVersion: null,
    officialOutcomeCodes: row.officialOutcomeCodes,
    formalTargetCodes: row.formalTargetCodes,
    lessonSkillProfileId: null,
    gates: Object.fromEntries(gateNames.map(name => [name, emptyGate()])),
    unresolvedDefects: [],
    status: 'QUEUED',
    academicDecision: 'NOT_CERTIFIED',
    independentReviewerId: null,
    signedAtUtc: null,
    signoffEvidenceId: null
  }));
  return {
    schemaVersion: 1,
    kind: 'LESSON_LEVEL_ACADEMIC_CERTIFICATION_LEDGER',
    scope: SOURCE_SCOPE,
    authoritativeInput: 'artifacts/academic-us-ccss/04-lesson-academic-audit.json',
    // A mutable release SHA is deliberately not inferred from a source inventory:
    releaseCandidateSha: null,
    intendedLessonCount: EXPECTED_LESSONS,
    records
  };
}

function validateLedger(source, ledger) {
  assert(ledger.schemaVersion === 1 &&
    ledger.kind === 'LESSON_LEVEL_ACADEMIC_CERTIFICATION_LEDGER' &&
    ledger.scope === SOURCE_SCOPE && ledger.intendedLessonCount === EXPECTED_LESSONS,
    'Ledger scope/schema/size mismatch');
  assert(Array.isArray(ledger.records) && ledger.records.length === EXPECTED_LESSONS,
    'Certification ledger must contain exactly 1,560 lessons');
  const byCode = new Map(source.records.map(row => [row.lessonCode,row]));
  const seen = new Set();
  const summary = { total: EXPECTED_LESSONS, certified: 0, awaitingSignoff: 0, blocked: 0, evidenceReady: 0 };
  for (const row of ledger.records) {
    const original = byCode.get(row.lessonCode);
    assert(original && !seen.has(row.lessonCode),
      'Unknown or duplicate lesson identity: ' + String(row.lessonCode));
    seen.add(row.lessonCode);
    for (const key of ['title','gradeOrCourse','blueprintCode']) {
      assert(row[key] === original[key], 'Immutable source identity drift ' + key + ': ' + row.lessonCode);
    }
    assert(row.pathway === (original.pathway ?? null) &&
      row.courseCode === (original.courseCode ?? null) &&
      row.publisherSourceUrl === original.sourceUrl &&
      row.sourceSemanticSha256 === original.sourceSemanticSha256 &&
      JSON.stringify(row.officialOutcomeCodes) === JSON.stringify(original.officialOutcomeCodes) &&
      JSON.stringify(row.formalTargetCodes) === JSON.stringify(original.formalTargetCodes),
      'Publisher/official outcome drift: ' + row.lessonCode);
    assert(row.gates && Object.keys(row.gates).length === 7 &&
      gateNames.every(g => Object.hasOwn(row.gates,g)),
      'Incomplete seven-gate coverage: ' + row.lessonCode);
    for (const gateName of gateNames) {
      const gate = row.gates[gateName];
      assert(gate && ['NOT_STARTED','IN_PROGRESS','PASS','FAIL','BLOCKED'].includes(gate.status) &&
        Array.isArray(gate.evidenceIds) && gate.evidenceIds.every(nonempty),
        'Invalid gate state: ' + row.lessonCode + '/' + gateName);
      if (gate.status === 'PASS') {
        assert(gate.evidenceIds.length > 0 && nonempty(gate.verifiedAtUtc),
          'PASS without evidence/time: ' + row.lessonCode + '/' + gateName);
      }
    }
    assert(Array.isArray(row.unresolvedDefects),
      'Missing defect list: ' + row.lessonCode);
    const allPassed = gateNames.every(g => row.gates[g].status === 'PASS');
    if (row.academicDecision === 'ACADEMICALLY_CERTIFIED') {
      assert(allPassed && nonempty(row.independentReviewerId) &&
        nonempty(row.signedAtUtc) && nonempty(row.signoffEvidenceId) &&
        row.unresolvedDefects.length === 0,
        'Academic certification without all gates and human signoff: ' + row.lessonCode);
      summary.certified++;
    } else {
      assert(row.academicDecision === 'NOT_CERTIFIED',
        'Unrecognized academic decision: ' + row.lessonCode);
      assert(row.gates.L7.status !== 'PASS',
        'L7 human signature cannot PASS an uncertified lesson: ' + row.lessonCode);
    }
    if (row.status === 'AWAITING_ACADEMIC_SIGNOFF') summary.awaitingSignoff++;
    if (row.status === 'EVIDENCE_READY') summary.evidenceReady++;
    if (row.status === 'BLOCKED' || row.status === 'REWORK_REQUIRED') summary.blocked++;
  }
  assert(seen.size === EXPECTED_LESSONS,'Certification ledger is incomplete');
  if (ledger.releaseCandidateSha !== null) {
    assert(/^[0-9a-f]{40}$/i.test(ledger.releaseCandidateSha),
      'Release candidate must be null or a full git SHA');
  }
  return summary;
}

function run(argv = process.argv.slice(2)) {
  const initialize = argv.includes('--init');
  const check = argv.includes('--check');
  assert(initialize !== check, 'Choose exactly --init or --check');
  const source = loadSource();
  if (initialize) {
    assert(!fs.existsSync(recordsPath),
      'Ledger exists: refusing to overwrite edits or human approvals');
    const ledger = createInitialLedger(source);
    const summary = validateLedger(source, ledger);
    const text = JSON.stringify(ledger, null, 2) + '\n';
    fs.writeFileSync(recordsPath, text, { flag: 'wx' });
    process.stdout.write(JSON.stringify({ operation: 'init', ...summary }) + '\n');
  } else {
    assert(fs.existsSync(recordsPath), 'Ledger missing; initialize it first');
    const ledger = JSON.parse(fs.readFileSync(recordsPath,'utf8'));
    const summary = validateLedger(source,ledger);
    process.stdout.write(JSON.stringify({ operation: 'check', ...summary }) + '\n');
  }
}

if (require.main === module) {
  try { run(); } catch (error) {
    process.stderr.write('[CCSS lesson certification] ' + error.message + '\n');
    process.exitCode = 1;
  }
}
module.exports = { loadSource, createInitialLedger, validateLedger, run, EXPECTED_LESSONS };
