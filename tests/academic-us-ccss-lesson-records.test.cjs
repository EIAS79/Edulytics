'use strict';

const assert = require('node:assert/strict');
const test = require('node:test');
const fs = require('node:fs');
const path = require('node:path');
const { loadSource, createInitialLedger, validateLedger, EXPECTED_LESSONS } =
  require('../tools/us-ccss-lesson-certification-records.cjs');

const root = path.resolve(__dirname,'..');
const file = path.join(root,'artifacts/academic-us-ccss/04-lesson-certification-records.json');

test('All 1,560 lessons are present once with separate seven-gate pending records', () => {
  const source = loadSource();
  const current = JSON.parse(fs.readFileSync(file, 'utf8'));
  const result = validateLedger(source,current);
  assert.equal(result.total, EXPECTED_LESSONS);
  assert.equal(result.certified,0);
  assert.equal(result.evidenceReady,0);
  assert.equal(result.awaitingSignoff,0);
  assert.deepEqual([...new Set(current.records.map(x => x.lessonCode))].length, EXPECTED_LESSONS);
  assert(current.records.every(x => x.status === 'QUEUED' &&
    x.academicDecision === 'NOT_CERTIFIED' &&
    Object.values(x.gates).every(g => g.status === 'NOT_STARTED')));
});

test('Initialization is repeatable and never invents approved evidence', () => {
  const source = loadSource();
  const a = createInitialLedger(source);
  const b = createInitialLedger(source);
  assert.equal(JSON.stringify(a), JSON.stringify(b));
  assert.deepEqual(validateLedger(source,a),validateLedger(source,b));
  assert(a.records.every(x => x.independentReviewerId === null &&
    x.signedAtUtc === null && x.signoffEvidenceId === null));
});

test('Duplicate, omitted or unknown lesson identity fails closed', () => {
  const source = loadSource();
  const base = createInitialLedger(source);
  const missing = structuredClone(base); missing.records.pop();
  assert.throws(() => validateLedger(source,missing), /exactly 1,560/);
  const duplicate = structuredClone(base); duplicate.records[3] = structuredClone(duplicate.records[0]);
  assert.throws(() => validateLedger(source,duplicate), /duplicate/);
});

test('Publisher source hash, official outcome and course mapping are immutable', () => {
  const source = loadSource(), base = createInitialLedger(source);
  const tampered = structuredClone(base);
  tampered.records[0].sourceSemanticSha256 = '0'.repeat(64);
  assert.throws(() => validateLedger(source,tampered), /Publisher\/official outcome drift/);
  const wrongOutcome = structuredClone(base);
  wrongOutcome.records[0].officialOutcomeCodes = ['CCSS:HSA-REI.C.6'];
  assert.throws(() => validateLedger(source,wrongOutcome), /Publisher\/official outcome drift/);
});

test('PASS without evidence and reviewer metadata cannot certify', () => {
  const source = loadSource(), base = createInitialLedger(source);
  const spoofedGate = structuredClone(base);
  spoofedGate.records[0].gates.L2.status = 'PASS';
  assert.throws(() => validateLedger(source,spoofedGate), /PASS without evidence/);
  const spoofedSignoff = structuredClone(base);
  spoofedSignoff.records[0].academicDecision = 'ACADEMICALLY_CERTIFIED';
  assert.throws(() => validateLedger(source,spoofedSignoff), /human signoff/);
});

test('Uncertified lesson cannot contain a superficially passed human gate', () => {
  const source = loadSource(), base = createInitialLedger(source);
  const row = base.records[1];
  row.gates.L7 = { status: 'PASS', evidenceIds: ['invented-id'], verifiedAtUtc: '2026-10-12T01:00:00Z' };
  assert.throws(() => validateLedger(source,base), /cannot PASS an uncertified lesson/);
});

test('Release candidate SHA is only null or exact git SHA', () => {
  const source = loadSource(), base = createInitialLedger(source);
  base.releaseCandidateSha = 'not-a-sha';
  assert.throws(() => validateLedger(source,base), /full git SHA/);
});
