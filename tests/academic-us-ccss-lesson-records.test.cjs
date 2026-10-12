'use strict';

const assert = require('node:assert/strict');
const test = require('node:test');
const fs = require('node:fs');
const path = require('node:path');
const {
  loadSource, createInitialLedger, validateLedger, EXPECTED_LESSONS,
  gateNames, dimensionNames, policyId
} = require('../tools/us-ccss-lesson-certification-records.cjs');

const file = path.join(__dirname, '../artifacts/academic-us-ccss/04-lesson-certification-records.json');
const proofSha = 'a'.repeat(40);
const contentSha = 'b'.repeat(64);
const time = '2026-10-12T21:01:00Z';
const base = () => createInitialLedger(loadSource());

function addSyntheticEvidence(row) {
  row.contentVersion = 'synthetic-only-unit-test';
  row.contentSha256 = contentSha;
  row.reviewDimensions = Object.fromEntries(dimensionNames.map(k => [k, {
    status: 'PASS', evidenceIds: ['unit-test:'+k], issueIds: [],
    verifiedAtUtc: time, contentSha256: contentSha, notApplicableReason: null
  }]));
  for (const name of gateNames)
    row.gates[name] = {
      status: 'PASS', evidenceIds: ['unit-test:'+name],
      verifiedAtUtc: time, sourceSha: proofSha
    };
}

test('Source inventory is exactly 1560; persisted ledger contains only L1-L6', () => {
  const source = loadSource();
  const current = JSON.parse(fs.readFileSync(file, 'utf8'));
  assert.equal(validateLedger(source, current).total, EXPECTED_LESSONS);
  assert.equal(current.schemaVersion, 2);
  assert.equal(current.policyId, policyId);
  assert.equal(validateLedger(source, current).certified, 0);
  assert.equal(new Set(current.records.map(x => x.lessonCode)).size, EXPECTED_LESSONS);
  assert(current.records.every(x => x.status === 'QUEUED' &&
    x.academicDecision === 'NOT_CERTIFIED' &&
    x.reviewDimensions === null &&
    Object.keys(x.gates).length === 6 && !Object.hasOwn(x.gates, 'L7') &&
    !Object.hasOwn(x, 'independentReviewerId') &&
    !Object.hasOwn(x, 'signoffEvidenceId') &&
    gateNames.every(g => x.gates[g].status === 'NOT_STARTED')));
});

test('Initialization is deterministic, evidence is NOT fabricated', () => {
  const one = base(), two = base();
  assert.equal(JSON.stringify(one), JSON.stringify(two));
  assert.deepEqual(validateLedger(loadSource(), one), validateLedger(loadSource(), two));
  assert.equal(validateLedger(loadSource(), one).certified, 0);
});

test('Duplicate, missing and unknown identities fail closed', () => {
  const a = base(); a.records.pop();
  assert.throws(() => validateLedger(loadSource(), a), /six-gate internal ledger|count mismatch|policy\/schema\/count/i);
  const b = base(); b.records[3].lessonCode = b.records[0].lessonCode;
  assert.throws(() => validateLedger(loadSource(), b), /duplicate/);
  const c = base(); c.records[0].lessonCode = 'PED:US-CCSS-MATH:G0:WRONG';
  assert.throws(() => validateLedger(loadSource(), c), /Unknown/);
});

test('Authoritative source identity, policy ID, and six-gate schema cannot drift', () => {
  const a = base(); a.records[0].sourceSemanticSha256 = '0'.repeat(64);
  assert.throws(() => validateLedger(loadSource(), a), /Publisher source drift/);
  const b = base(); b.records[0].gates.L7 = {status:'PASS'};
  assert.throws(() => validateLedger(loadSource(), b), /exactly six/);
  const c = base(); c.policyId = 'OTHER';
  assert.throws(() => validateLedger(loadSource(), c), /policy\/schema/);
});

test('No PASS without evidence, valid UTC timestamp and exact source/content SHA', () => {
  const a = base(), gate = a.records[0].gates.L1;
  gate.status = 'PASS';
  assert.throws(() => validateLedger(loadSource(), a), /PASS without evidence/);
  a.records[0].contentSha256 = contentSha;
  gate.evidenceIds = ['test-evidence']; gate.sourceSha = proofSha;
  gate.verifiedAtUtc = '2026-02-30T10:00:00Z';
  assert.throws(() => validateLedger(loadSource(), a), /PASS without evidence/);
  gate.verifiedAtUtc = time; gate.sourceSha = 'not-sha';
  assert.throws(() => validateLedger(loadSource(), a), /PASS without evidence/);
  gate.sourceSha = proofSha;
  assert.equal(validateLedger(loadSource(), a).certified, 0);
});

test('No false reviewed/completed/certified states without all six gates and eight dimensions', () => {
  for (const status of ['CONTENT_REVIEWED','EVIDENCE_READY','INTERNALLY_CERTIFIED']) {
    const a = base();
    a.records[0].status = status;
    assert.throws(() => validateLedger(loadSource(), a), /Missing review dimensions|Internal certification|L1-L4/i);
  }
  const b = base(); b.records[0].academicDecision = 'INTERNALLY_CERTIFIED';
  assert.throws(() => validateLedger(loadSource(), b), /Internal certification/);
});

test('Synthetic test-only complete record proves six-gate certification without human signature', () => {
  const a = base(); const r = a.records[0];
  addSyntheticEvidence(r);
  r.status = 'INTERNALLY_CERTIFIED'; r.academicDecision = 'INTERNALLY_CERTIFIED';
  assert.throws(() => validateLedger(loadSource(), a), /candidate SHA/);
  a.releaseCandidateSha = proofSha;
  assert.equal(validateLedger(loadSource(), a).certified, 1);
  r.unresolvedDefects.push('MATH-DEFECT-TEST');
  assert.throws(() => validateLedger(loadSource(), a), /Internal certification/);
  r.unresolvedDefects = [];
  r.gates.L6.sourceSha = 'c'.repeat(40);
  assert.throws(() => validateLedger(loadSource(), a), /Internal certification/);
  r.gates.L6.sourceSha = proofSha;
  r.reviewDimensions.questionsAndGrading.status = 'BLOCKED';
  assert.throws(() => validateLedger(loadSource(), a), /Internal certification/);
});

test('Unreviewed or missing evidence may not be called internally certified', () => {
  const a = base(); addSyntheticEvidence(a.records[0]);
  a.releaseCandidateSha = proofSha;
  a.records[0].reviewDimensions.visualRepresentations.status = 'NOT_APPLICABLE';
  a.records[0].reviewDimensions.visualRepresentations.notApplicableReason = null;
  assert.throws(() => validateLedger(loadSource(), a), /NOT_APPLICABLE/);
  a.records[0].reviewDimensions.visualRepresentations.notApplicableReason = 'No visual needed';
  a.records[0].status = 'INTERNALLY_CERTIFIED';
  a.records[0].academicDecision = 'INTERNALLY_CERTIFIED';
  assert.equal(validateLedger(loadSource(), a).certified, 1);
  a.records[0].reviewDimensions.questionsAndGrading.issueIds.push('OPEN-ISSUE');
  assert.throws(() => validateLedger(loadSource(), a), /Reviewed dimension missing/);
});
