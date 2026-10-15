'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const { inspectBatch, requireCurrentCandidateSha } = require('../tools/us-ccss-b01-release-gate.cjs');
const { createInitialLedger, gateNames, dimensionNames, policyId } =
  require('../tools/us-ccss-lesson-certification-records.cjs');

const candidate = '1'.repeat(40);
const semantic = 'a'.repeat(64);
const body = 'b'.repeat(64);
const when = '2026-10-13T01:00:00Z';

function fixture() {
  const records = Array.from({ length: 1560 }, (_, i) => ({
    lessonCode: i === 0 ? 'PED:US-CCSS-MATH:G1:U01:L01' :
      i === 499 ? 'PED:US-CCSS-MATH:G4:U04:L20' :
      'PED:US-CCSS-MATH:FIXTURE:' + String(i).padStart(4, '0'),
    title: 'Lesson ' + (i + 1),
    gradeOrCourse: i < 435 ? 'Grade 1' : 'Grade 4',
    sourceUrl: 'https://example.org/lessons/' + i,
    sourceSemanticSha256: semantic,
    officialOutcomeCodes: ['CCSS:1.OA.C.5'],
    formalTargetCodes: []
  }));
  const source = {
    scope: 'US-CCSS-MATH/CCSSM-2010/G1-G12', lessonCount: 1560,
    records
  };
  const ledger = createInitialLedger(source);
  ledger.releaseCandidateSha = candidate;
  const manifest = {
    schemaVersion: 1, kind: 'US_CCSS_ACADEMIC_500_LESSON_BATCH_MANIFEST',
    policyId, executionMode: 'FIXED_500_LESSON_BATCHES',
    batchId: 'B01', ordinalStart: 1, ordinalEnd: 500,
    expectedLessonCount: 500, totalCurriculumLessons: 1560,
    existingCertifiedPilot: { lessonCode: records[0].lessonCode },
    lessons: records.slice(0, 500).map((r, i) => ({
      ordinal: i + 1, lessonCode: r.lessonCode, title: r.title,
      gradeOrCourse: r.gradeOrCourse, sourceUrl: r.sourceUrl,
      sourceSemanticSha256: r.sourceSemanticSha256,
      officialOutcomeCodes: [...r.officialOutcomeCodes]
    }))
  };
  return { manifest, source, ledger };
}
function certify(row) {
  row.status = 'INTERNALLY_CERTIFIED';
  row.academicDecision = 'INTERNALLY_CERTIFIED';
  row.contentVersion = 'test-verified-content';
  row.contentSha256 = body;
  row.gates = Object.fromEntries(gateNames.map(g => [g, {
    status: 'PASS', evidenceIds: ['fixture-' + g],
    verifiedAtUtc: when, sourceSha: candidate
  }]));
  row.reviewDimensions = Object.fromEntries(dimensionNames.map(d => [d, {
    status: 'PASS', evidenceIds: ['fixture-' + d],
    issueIds: [], verifiedAtUtc: when,
    contentSha256: body, notApplicableReason: null
  }]));
}

test('B01 stays NO-GO with just one properly certified lesson', () => {
  const x = fixture();
  certify(x.ledger.records[0]);
  const state = inspectBatch(x);
  assert.equal(state.evidenceReady, 1);
  assert.equal(state.remainingEvidence, 499);
  assert.equal(state.evidenceGapRows.length, 499);
  assert.equal(state.evidenceGapRows[0].ordinal, 2);
  assert.deepEqual(state.evidenceGapRows[0].missingGates,
    ['L1','L2','L3','L4','L5','L6']);
  assert.equal(state.evidenceGapRows[0].missingDimensions.length, 8);
  assert.equal(state.missingGateProofTotals.L6, 499);
  assert.equal(state.missingDimensionProofTotals.mathematicalCorrectness, 499);
  assert.equal(state.noteNoDefectsRecordedDoesNotProveReviewed,true);
  assert.equal(state.state, 'BATCH_NO-GO');
  assert.equal(state.gapsPreview[0].lessonCode, x.manifest.lessons[1].lessonCode);
});

test('B01 never accepts a missing or duplicated lesson identity', () => {
  const x = fixture();
  x.manifest.lessons[8].lessonCode = x.manifest.lessons[7].lessonCode;
  assert.throws(() => inspectBatch(x), /Missing or duplicate/);
});

test('B01 detects source semantic drift even if IDs are unchanged', () => {
  const x = fixture();
  x.manifest.lessons[3].sourceSemanticSha256 = 'f'.repeat(64);
  assert.throws(() => inspectBatch(x), /provenance drift/);
});

test('A synthetic PASS without six-gate evidence fails closed', () => {
  const x = fixture();
  x.ledger.records[0].status = 'INTERNALLY_CERTIFIED';
  x.ledger.records[0].academicDecision = 'INTERNALLY_CERTIFIED';
  assert.throws(() => inspectBatch(x), /Missing review dimensions|Internal certification requires/);
});

test('B01 is certification-ready only when exactly 500 pass', () => {
  const x = fixture();
  for (const row of x.ledger.records.slice(0, 500)) certify(row);
  const state = inspectBatch(x);
  assert.equal(state.evidenceReady, 500);
  assert.equal(state.remainingEvidence, 0);
  assert.equal(state.state, 'BATCH_EVIDENCE_READY');
});

test('One collective decision accepts 499 evidence-ready rows plus the prior certified pilot', () => {
  const x = fixture();
  certify(x.ledger.records[0]);
  for (const row of x.ledger.records.slice(1, 500)) {
    certify(row);
    row.status = 'EVIDENCE_READY';
    row.academicDecision = 'NOT_CERTIFIED';
  }
  const state = inspectBatch(x);
  assert.equal(state.evidenceReady, 500);
  assert.equal(state.historicalCertificationsReused, 1);
  assert.equal(state.state, 'BATCH_EVIDENCE_READY');
  assert.equal(x.ledger.records[1].academicDecision, 'NOT_CERTIFIED');
});

test('One missing evidence record blocks approval of all 500', () => {
  const x = fixture();
  certify(x.ledger.records[0]);
  for (const row of x.ledger.records.slice(1, 499)) {
    certify(row);
    row.status = 'EVIDENCE_READY';
    row.academicDecision = 'NOT_CERTIFIED';
  }
  const state = inspectBatch(x);
  assert.equal(state.evidenceReady, 499);
  assert.equal(state.remainingEvidence, 1);
  assert.equal(state.state, 'BATCH_NO-GO');
});

test('A foreign candidate SHA in one gate blocks collective acceptance', () => {
  const x = fixture();
  certify(x.ledger.records[0]);
  for (const row of x.ledger.records.slice(1, 500)) {
    certify(row);
    row.status = 'EVIDENCE_READY';
    row.academicDecision = 'NOT_CERTIFIED';
  }
  x.ledger.records[245].gates.L5.sourceSha = '2'.repeat(40);
  const state = inspectBatch(x);
  assert.equal(state.evidenceReady, 499);
  assert.equal(state.state, 'BATCH_NO-GO');
});

test('Any unresolved defect blocks the entire batch', () => {
  const x = fixture();
  certify(x.ledger.records[0]);
  for (const row of x.ledger.records.slice(1, 500)) {
    certify(row);
    row.status = 'EVIDENCE_READY';
    row.academicDecision = 'NOT_CERTIFIED';
  }
  x.ledger.records[267].unresolvedDefects.push('B01-OPEN-ISSUE');
  const state = inspectBatch(x);
  assert.equal(state.evidenceReady, 499);
  assert.equal(state.openDefects, 1);
  assert.equal(state.state, 'BATCH_NO-GO');
});

test('Final 500-lesson release binds every L1-L6 proof to the EXACT checked-out immutable HEAD, never stale source', () => {
 const x=fixture();
 for(const row of x.ledger.records.slice(0,500)) certify(row);
 assert.equal(inspectBatch(x).state,'BATCH_EVIDENCE_READY');
 assert.equal(requireCurrentCandidateSha(x.ledger,candidate),candidate);
 assert.throws(() => requireCurrentCandidateSha(x.ledger,'2'.repeat(40)),
   /STALE B01 candidate/);
 assert.throws(() => requireCurrentCandidateSha(x.ledger,null),
   /requires immutable current candidate SHA/);
 assert.throws(() => requireCurrentCandidateSha(x.ledger,'not-a-commit'),
   /requires immutable current candidate SHA/);
 assert.equal(inspectBatch(x).evidenceReady,500,
   'The read-only status report is not a release authorization and stays usable');
});
