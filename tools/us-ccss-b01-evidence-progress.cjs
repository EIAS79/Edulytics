#!/usr/bin/env node
'use strict';

// Read-only, SHA-aware B01 evidence progress. Neither screening nor this report
// can promote a lesson or write PASS into the canonical certification ledger.
const fs = require('node:fs');
const path = require('node:path');
const { loadSource, validateLedger, gateNames, dimensionNames } =
  require('./us-ccss-lesson-certification-records.cjs');
const root = path.resolve(__dirname, '..');
const read = file => JSON.parse(fs.readFileSync(path.join(root, file), 'utf8'));
const assert = (ok, msg) => { if (!ok) throw Error(msg); };
const isSha = x => typeof x === 'string' && /^[a-f0-9]{64}$/i.test(x);
const isCommit = x => typeof x === 'string' && /^[a-f0-9]{40}$/i.test(x);
const GATE_SIGNALS = Object.freeze({
  L1: ['publisher', 'staticScreen'],
  L2: ['staticScreen', 'effectiveRuntime'],
  L3: ['publisher', 'originalTeacherActivities', 'richV2'],
  L4: ['originalTeacherActivities', 'richV2', 'sourceVisualMatch'],
  L5: ['originalTeacherActivities', 'effectiveRuntime', 'sourceQuestionMatch'],
  L6: ['richV2', 'effectiveRuntime']
});
// Artifact pointers are TRIAGE INPUTS, not proof that the full gate passed.
function evidenceProgress({ manifest, source, ledger, report }) {
  validateLedger(source, ledger);
  assert(manifest.batchId === 'B01' && manifest.lessons?.length === 500,
    'Expected exact B01 500-lesson manifest');
  assert(report.batchId === 'B01' && report.lessons?.length === 500 &&
    report.coverage?.joined === 500 && report.coverage?.unique === 500,
    'Expected exactly 500 joined eight-dimension report rows');
  const records = new Map(ledger.records.map(r => [r.lessonCode, r]));
  const seen = new Set();
  const gates = Object.fromEntries(gateNames.map(g => [g,
    { evidencePass: 0, missing: 0, staleOrDefective: 0 }]));
  const dimensions = Object.fromEntries(dimensionNames.map(d => [d,
    { evidencePass: 0, missing: 0, staleOrDefective: 0 }]));
  const clusters = new Map();
  const lessons = [];
  let ready = 0, historical = 0, changedBody = 0;
  for (let i = 0; i < 500; i++) {
    const item = manifest.lessons[i];
    const row = report.lessons[i];
    const canonical = source.records[i];
    assert(item && row && canonical &&
      item.ordinal === i + 1 && row.ordinal === i + 1 &&
      item.lessonCode === canonical.lessonCode &&
      row.lessonCode === item.lessonCode &&
      !seen.has(item.lessonCode), 'Missing, duplicate or re-ordered lesson at ' + (i + 1));
    seen.add(item.lessonCode);
    assert(item.sourceSemanticSha256 === canonical.sourceSemanticSha256 &&
      item.sourceUrl === canonical.sourceUrl &&
      item.title === canonical.title &&
      row.sourceSemanticSha256 === item.sourceSemanticSha256 &&
      row.sourceUrl === item.sourceUrl, 'Source fingerprint or identity drift: ' + item.lessonCode);
    const ledgerRow = records.get(item.lessonCode);
    assert(ledgerRow, 'Missing canonical ledger row: ' + item.lessonCode);
    const effectiveSha = row.effectiveContentSha256;
    const bodyMatches = isSha(effectiveSha) &&
      effectiveSha === ledgerRow.contentSha256;
    if (ledgerRow.contentSha256 && !bodyMatches) changedBody++;
    const hasDetectedDefect = row.verdict === 'NEEDS_REPAIR' ||
      (row.confirmedDefects || []).length > 0 ||
      ledgerRow.unresolvedDefects.length > 0;
    const gateStates = {};
    for (const gate of gateNames) {
      const record = ledgerRow.gates[gate];
      const recordPass = record.status === 'PASS';
      const shaPass = isCommit(ledger.releaseCandidateSha) &&
        record.sourceSha === ledger.releaseCandidateSha;
      const current = recordPass && shaPass && bodyMatches && !hasDetectedDefect;
      const status = current ? 'LEDGER_PASS' :
        recordPass && (!shaPass || !bodyMatches || hasDetectedDefect) ?
          'STALE_OR_DEFECTIVE' : 'MISSING_VERIFIED_EVIDENCE';
      if (current) gates[gate].evidencePass++;
      else if (status === 'STALE_OR_DEFECTIVE') gates[gate].staleOrDefective++;
      else gates[gate].missing++;
      gateStates[gate] = {
        status, ledgerStatus: record.status,
        evidenceIds: recordPass ? record.evidenceIds : [],
        // Source names merely help find existing checks; they do NOT promote PASS.
        candidateArtifactSources: (GATE_SIGNALS[gate] || [])
          .map(k => row.evidenceSources?.[k]).filter(Boolean)
      };
    }
    const dimensionStates = {};
    for (const dimension of dimensionNames) {
      const rec = ledgerRow.reviewDimensions?.[dimension];
      const recordPass = rec && ['PASS','NOT_APPLICABLE'].includes(rec.status);
      const current = recordPass && bodyMatches &&
        rec.contentSha256 === effectiveSha && !hasDetectedDefect;
      const status = current ? 'LEDGER_PASS' :
        recordPass ? 'STALE_OR_DEFECTIVE' : 'MISSING_VERIFIED_EVIDENCE';
      if (current) dimensions[dimension].evidencePass++;
      else if (status === 'STALE_OR_DEFECTIVE') dimensions[dimension].staleOrDefective++;
      else dimensions[dimension].missing++;
      dimensionStates[dimension] = status;
    }
    const sixReady = gateNames.every(g => gateStates[g].status === 'LEDGER_PASS');
    const eightReady = dimensionNames.every(d =>
      dimensionStates[d] === 'LEDGER_PASS');
    const acceptedState = ledgerRow.status === 'EVIDENCE_READY' &&
      ledgerRow.academicDecision === 'NOT_CERTIFIED' ||
      ledgerRow.status === 'INTERNALLY_CERTIFIED' &&
      ledgerRow.academicDecision === 'INTERNALLY_CERTIFIED';
    const lessonReady = acceptedState && sixReady && eightReady &&
      !hasDetectedDefect && bodyMatches && row.verdict !== 'BLOCKED_MISSING_EVIDENCE';
    if (lessonReady) {
      ready++;
      if (ledgerRow.status === 'INTERNALLY_CERTIFIED') historical++;
    }
    for (const fault of row.confirmedDefects || []) {
      const k = fault.code || 'UNKNOWN';
      if (!clusters.has(k)) clusters.set(k, { code: k, count: 0, lessons: [] });
      const bucket = clusters.get(k);
      bucket.count++;
      bucket.lessons.push(item.lessonCode);
    }
    lessons.push({
      ordinal: i + 1, lessonCode: item.lessonCode,
      sourceSemanticSha256: item.sourceSemanticSha256,
      effectiveContentSha256: isSha(effectiveSha) ? effectiveSha : null,
      ledgerContentSha256: ledgerRow.contentSha256,
      ledgerState: ledgerRow.status, screeningVerdict: row.verdict,
      contentFingerprintMatchesLedger: bodyMatches,
      releaseCandidateSha: ledger.releaseCandidateSha,
      sixGateEvidence: gateStates, eightDimensionEvidence: dimensionStates,
      confirmedDefects: row.confirmedDefects || [],
      unresolvedLedgerDefects: ledgerRow.unresolvedDefects,
      evidenceReady: Boolean(lessonReady),
      reasonIfNotReady: lessonReady ? null :
        hasDetectedDefect ? 'UNRESOLVED_DEFECT_OR_REPAIR' :
        !bodyMatches ? 'MISSING_OR_STALE_CONTENT_FINGERPRINT' :
        !sixReady ? 'INCOMPLETE_L1_L6_GATE_PROOFS' :
        !eightReady ? 'INCOMPLETE_EIGHT_DIMENSION_PROOFS' :
        'NOT_COLLECTIVELY_EVIDENCE_READY'
    });
  }
  const defectClusters = [...clusters.values()].sort((a,b) =>
    b.count - a.count || a.code.localeCompare(b.code));
  return {
    schemaVersion: 1, kind: 'B01_COLLECTIVE_EVIDENCE_PROGRESS',
    policy: 'READ_ONLY_NO_SYNTHETIC_PASS',
    batchId: 'B01', expected: 500, observed: lessons.length,
    releaseCandidateSha: ledger.releaseCandidateSha,
    evidenceReady: ready, historicalCertifiedReused: historical,
    remainingEvidence: 500 - ready,
    contentFingerprintMismatches: changedBody,
    gateProgress: gates, dimensionProgress: dimensions,
    defectClusters,
    state: ready === 500 && defectClusters.length === 0 ?
      'BATCH_EVIDENCE_READY' : 'BATCH_NO-GO',
    note: 'Audit artifact names and screening passes are pointers, not gate proof. Only SHA-valid canonical ledger PASS and zero defects count. This tool NEVER writes the ledger or approves release.',
    lessons
  };
}
function main() {
  const progress = evidenceProgress({
    manifest: read('artifacts/academic-us-ccss/batches/B01-lesson-manifest.json'),
    source: loadSource(),
    ledger: read('artifacts/academic-us-ccss/04-lesson-certification-records.json'),
    report: read('b01-unified-eight-dimension-report-500.json')
  });
  fs.writeFileSync('b01-collective-evidence-progress-500.json',
    JSON.stringify(progress, null, 2) + '\n');
  const summary = {
    kind: progress.kind, expected: progress.expected, evidenceReady: progress.evidenceReady,
    remainingEvidence: progress.remainingEvidence,
    contentFingerprintMismatches: progress.contentFingerprintMismatches,
    gateProgress: progress.gateProgress, dimensionProgress: progress.dimensionProgress,
    defectClusters: progress.defectClusters.slice(0, 20), state: progress.state
  };
  fs.writeFileSync('b01-collective-evidence-progress-summary.json',
    JSON.stringify(summary, null, 2) + '\n');
  process.stdout.write(JSON.stringify(summary, null, 2) + '\n');
}
if (require.main === module) {
  try { main(); } catch (e) {
    console.error('[B01 evidence progress] ' + e.message);
    process.exitCode = 1;
  }
}
module.exports = { evidenceProgress, GATE_SIGNALS };
