#!/usr/bin/env node
'use strict';

// First 10-lesson certification/release decision, replacing the 500-lesson
// release unit without changing historical B01 audit evidence or its manifest.
// No PASS is ever created by this tool. T002+ require separate candidate-SHA
// migration and previous-batch live proof before their release can be enabled.
const fs = require('node:fs');
const path = require('node:path');
const {
  loadSource, validateLedger, policyId, gateNames, dimensionNames
} = require('./us-ccss-lesson-certification-records.cjs');
// No dependency on the retired broad 500-lesson B01 release gate.
const root = path.resolve(__dirname, '..');
const manifestFile = path.join(root,
  'artifacts/academic-us-ccss/batches/T001-lesson-manifest.json');
const ledgerFile = path.join(root,
  'artifacts/academic-us-ccss/04-lesson-certification-records.json');
const assert = (c, m) => { if (!c) throw Error(m); };
const isSha = v => typeof v === 'string' && /^[0-9a-f]{64}$/i.test(v);
const isCommit = v => typeof v === 'string' && /^[0-9a-f]{40}$/.test(v);
const requireCurrentCandidateSha = (ledger, headSha) => {
  assert(isCommit(headSha) && ledger.releaseCandidateSha === headSha,
    'T001 release candidate must be bound to exact reviewed HEAD SHA');
};

function inspectTen({manifest, source, ledger}) {
  assert(manifest.schemaVersion === 1 &&
    manifest.kind === 'US_CCSS_ACADEMIC_10_LESSON_BATCH_MANIFEST' &&
    manifest.policyId === policyId &&
    manifest.executionMode === 'FIXED_10_LESSON_BATCHES' &&
    manifest.batchId === 'T001' && manifest.batchNumber === 1 &&
    manifest.batchSize === 10 && manifest.totalBatches === 156 &&
    manifest.totalCurriculumLessons === 1560 &&
    manifest.ordinalStart === 1 && manifest.ordinalEnd === 10 &&
    manifest.expectedLessonCount === 10, 'T001 fixed-ten batch specification mismatch');
  assert(Array.isArray(manifest.lessons) && manifest.lessons.length === 10,
    'T001 must contain exactly ten lessons');
  assert(manifest.lessons[0]?.lessonCode === 'PED:US-CCSS-MATH:G1:U01:L01' &&
    manifest.lessons[9]?.lessonCode === 'PED:US-CCSS-MATH:G1:U01:L10',
    'T001 source endpoints changed');
  assert(source.records?.length === 1560, 'Authoritative 1560 source lessons required');
  validateLedger(source, ledger);
  const ledgerMap = new Map(ledger.records.map(x => [x.lessonCode, x]));
  const seen = new Set();
  const missing = [],missingGateProofTotals = Object.fromEntries(gateNames.map(g => [g,0]));
  const missingDimensionProofTotals = Object.fromEntries(dimensionNames.map(d => [d,0]));
  let evidenceReady = 0, openDefects = 0, historicalCertified = 0;
  for (let i = 0; i < 10; i++) {
    const m = manifest.lessons[i], sourceRow = source.records[i];
    assert(m.ordinal === i + 1 && m.lessonCode === sourceRow.lessonCode &&
      !seen.has(m.lessonCode) &&
      m.title === sourceRow.title &&
      m.gradeOrCourse === sourceRow.gradeOrCourse &&
      m.sourceUrl === sourceRow.sourceUrl &&
      m.sourceSemanticSha256 === sourceRow.sourceSemanticSha256 &&
      isSha(m.sourceSemanticSha256) &&
      JSON.stringify(m.officialOutcomeCodes) ===
        JSON.stringify(sourceRow.officialOutcomeCodes),
      'T001 missing, duplicate or source-drifted lesson ' + (i+1));
    seen.add(m.lessonCode);
    const l = ledgerMap.get(m.lessonCode);
    assert(l, 'T001 canonical ledger missing ' + m.lessonCode);
    openDefects += l.unresolvedDefects.length;
    const missingGates = gateNames.filter(g =>
      l.gates[g]?.status !== 'PASS' ||
      l.gates[g]?.sourceSha !== ledger.releaseCandidateSha);
    const missingDimensions = dimensionNames.filter(d => {
      const item = l.reviewDimensions?.[d];
      return !item || !['PASS','NOT_APPLICABLE'].includes(item.status) ||
        item.contentSha256 !== l.contentSha256;
    });
    for (const g of missingGates) missingGateProofTotals[g]++;
    for (const d of missingDimensions) missingDimensionProofTotals[d]++;
    const historical = l.status === 'INTERNALLY_CERTIFIED' &&
      l.academicDecision === 'INTERNALLY_CERTIFIED';
    const prepared = l.status === 'EVIDENCE_READY' &&
      l.academicDecision === 'NOT_CERTIFIED';
    const ready = (historical || prepared) && missingGates.length === 0 &&
      missingDimensions.length === 0 && l.unresolvedDefects.length === 0 &&
      isSha(l.contentSha256) && isCommit(ledger.releaseCandidateSha) &&
      typeof l.contentVersion === 'string' && l.contentVersion.length > 0;
    if (ready) {
      evidenceReady++;
      if (historical) historicalCertified++;
    } else {
      missing.push({
        ordinal: i + 1, lessonCode: m.lessonCode, status: l.status,
        missingGates, missingDimensions, unresolvedDefects: l.unresolvedDefects.length
      });
    }
  }
  return {
    batchId: 'T001', batchSize: 10, totalBatches: 156,
    total: 10, sourceLocked: seen.size,
    evidenceReady, remainingEvidence: 10 - evidenceReady, openDefects,
    historicalCertified, missingGateProofTotals, missingDimensionProofTotals,
    releaseCandidateSha: ledger.releaseCandidateSha,
    state: evidenceReady === 10 && openDefects === 0 ?
      'BATCH_EVIDENCE_READY' : 'BATCH_NO-GO',
    lessonGaps: missing,
    noAutomaticAcademicPass: true,
    nextBatchBlockedUntil: 'BATCH_PASS_AND_LIVE',
    note: 'No certification from screening alone; only ten actual SHA-bound L1-L6 and eight-dimension evidence rows qualify.'
  };
}
function main(args = process.argv.slice(2)) {
  const status = args.includes('--status'), releaseCheck = args.includes('--release-check');
  assert(status !== releaseCheck, 'Choose exactly one --status or --release-check');
  const source = loadSource();
  const ledger = JSON.parse(fs.readFileSync(ledgerFile, 'utf8'));
  const manifest = JSON.parse(fs.readFileSync(manifestFile, 'utf8'));
  const result = inspectTen({source,ledger,manifest});
  if (releaseCheck) {
    const headArg = args.find(x => x.startsWith('--head-sha='));
    const headSha = headArg?.slice('--head-sha='.length) ?? process.env.GITHUB_SHA;
    requireCurrentCandidateSha(ledger,headSha);
    assert(result.state === 'BATCH_EVIDENCE_READY',
      'T001 release blocked: ' + result.remainingEvidence +
        ' lessons require L1-L6 evidence or have unresolved defects');
    result.state = 'BATCH_CERTIFIED';
  }
  process.stdout.write(JSON.stringify(result,null,2) + '\n');
  return result;
}
if (require.main === module) {
  try { main(); }
  catch(e){ console.error('[US CCSS T001 release gate] ' + e.message); process.exitCode=1; }
}
module.exports = { inspectTen, main };
