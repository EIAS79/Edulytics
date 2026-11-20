'use strict';
// Strict academic-source completeness gate for UAE MOE Grade 9 General.
// Prepared during Stage A-B-C; execution is reserved for Stage D.
// This test is INTENTIONALLY RED until every source element has evidence.
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const read = rel => JSON.parse(fs.readFileSync(path.join(root, rel), 'utf8'));
const base = 'artifacts/curriculum-delivery/UAE-MOE-MATH/G9/';

test('Full academic index lock requires 86 independently audited raw source lessons, all 82 target lessons and resolved supplements', () => {
  const index = read(base + 'grade9-82-target-index.json');
  const audit = read(base + 'semantic-audit-ledger.json');
  const checkpoint = read(base + 'full-grade-progress.json');

  assert.equal(index.expectedLessons, 82);
  assert.equal(index.expectedUnits, 13);
  assert.equal(index.records.length, 82);
  assert.equal(audit.rows.length, 86);
  assert.equal(index.sourceRows, 86);

  const rawUnverified = audit.rows.filter(row =>
    row.fullLessonAuditPassed !== true ||
    !Number.isInteger(row.elementCountDenominator) ||
    row.elementCountDenominator <= 0 ||
    row.objectiveCoverageVerification !== 'FULL_SOURCE_ATOMIC_LEDGER_VERIFIED'
  );
  assert.deepEqual(rawUnverified.map(row => row.rawOrdinal), [],
    'Unverified source lesson ordinals must be fully audited before index lock');

  const targetUnverified = index.records.filter(row =>
    row.ElementCoverage !== 'FULL_SOURCE_ELEMENT_1_TO_1_VERIFIED');
  assert.deepEqual(targetUnverified.map(row => row.ordinal), [],
    'Canonical lessons require explicit source-element parity evidence');

  const supplementary = ['01.pdf:7-1', '01.pdf:7-2', '01.pdf:7-3', '01.pdf:7-4'];
  for (const source of supplementary) {
    const receiving = index.records.filter(row => row.SourceRefs.includes(source));
    assert.equal(receiving.length, 1, 'Supplement mapping must be unambiguous: ' + source);
    assert.equal(receiving[0].ElementCoverage, 'FULL_SOURCE_ELEMENT_1_TO_1_VERIFIED',
      'Supplement-specific exercises must be accounted for: ' + source);
  }
  assert.equal(index.SOURCE_INDEX_COMPLETE, true,
    'Academic source index cannot be locked by authoring-only checkpoint counts');
  assert.equal(checkpoint.targetIndexComplete, true);
  assert.equal(checkpoint.uniqueLessonsVerified, 82);
});
