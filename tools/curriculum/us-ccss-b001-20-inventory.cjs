#!/usr/bin/env node
'use strict';

// New V2 20-lesson release candidate; the 500-lesson file is immutable legacy evidence.
// --write is explicit; --check is strictly read-only, and never awards academic acceptance.
const fs = require('node:fs');
const path = require('node:path');
const crypto = require('node:crypto');
const assert = require('node:assert/strict');
const { buildCatalogue } = require('./us-ccss-b001-inventory.cjs');

const root = path.resolve(__dirname, '../..');
const legacyPath = path.join(root, 'docs/curriculum/batches/US-CCSS-B001-CATALOGUE.json');
const outputPath = path.join(root, 'docs/curriculum/batches/US-CCSS-B001-20-INTAKE.json');
const PLAN = 'EDULYTICS-US-CCSS-20-V2';
const BATCH = 'US-CCSS-B001';
const SIZE = 20;
const sha = value => crypto.createHash('sha256').update(value).digest('hex');

function buildManifest() {
  const legacyText = fs.readFileSync(legacyPath, 'utf8');
  const committed = JSON.parse(legacyText);
  const actual = buildCatalogue();
  assert.equal(committed.batchId, BATCH);
  assert.equal(committed.total, 500, 'Legacy 500 catalogue changed unexpectedly');
  assert.equal(committed.lessons.length, 500);
  assert.deepEqual(committed, actual, 'Legacy catalogue and actual source/content differ');
  const source = actual.lessons.slice(0, SIZE);
  assert.equal(source.length, SIZE);
  assert.equal(new Set(source.map(x => x.lessonCode)).size, SIZE);
  assert(source.every((x, i) => x.gradeOrCourse === 'Grade 1' && x.gradeOrdinal === i + 1),
    'B001 first 20 not consecutive Grade 1 ordinals');
  assert(source.slice(0, 10).every(x => x.historicalT001));
  assert(source.slice(10).every(x => !x.historicalT001));
  assert.equal(source[0].lessonCode, 'PED:US-CCSS-MATH:G1:U01:L01');
  return {
    schemaVersion: 1,
    kind: 'US_CCSS_V2_20_LESSON_BATCH_INTAKE_NOT_ACADEMIC_ACCEPTANCE',
    planId: PLAN, batchId: BATCH, programme: 'US-CCSS-MATH',
    orderedProgrammePositions: [1, SIZE], total: SIZE,
    sourceScope: 'FIRST_20_FROM_RECONCILED_HISTORICAL_500_CATALOGUE',
    legacyCataloguePath: 'docs/curriculum/batches/US-CCSS-B001-CATALOGUE.json',
    legacyCatalogueSha256: sha(legacyText),
    authoritativeRuntimeInventoryVerified: false,
    academicAcceptance: 'NOT_STARTED',
    productionDeployment: 'NOT_RELEASED_BY_20_POLICY',
    lessonEvidenceRequired: SIZE,
    historicalPreviouslyShipped: 15,
    newlyTargetedPositions: 5,
    acceptedUnderCurrent20Policy: 0,
    lessons: source.map((x, index) => ({
      ...x,
      batchOrdinal: index + 1,
      programmeOrdinal: index + 1,
      historicalPreviouslyShipped: index < 15,
      historicalRelease: index < 10 ? 'T001' : index < 15 ? 'GRADE_1_BATCH_03' : null,
      historicalReleaseDriftVerified: false,
      academicAcceptanceStatus: 'NOT_ACCEPTED_UNDER_V2_20',
      evidenceStatus: 'NOT_YET_INDEPENDENTLY_VERIFIED'
    }))
  };
}
function main(argv = process.argv.slice(2)) {
  const manifest = buildManifest();
  const serialized = JSON.stringify(manifest, null, 2) + '\n';
  if (argv.includes('--write')) {
    fs.writeFileSync(outputPath, serialized);
  } else if (argv.includes('--check')) {
    assert.equal(fs.readFileSync(outputPath, 'utf8'), serialized,
      '20-lesson intake differs from live canonical sources; review regeneration before committing');
  } else if (!argv.includes('--summary')) {
    throw Error('Use --write, --check or --summary');
  }
  console.log(JSON.stringify({ batchId: manifest.batchId, planId: manifest.planId,
    total: manifest.total, historical: 15, notPreviouslyShipped: 5,
    academicAccepted: 0, lastCode: manifest.lessons.at(-1).lessonCode }));
}
if (require.main === module) main();
module.exports = { buildManifest, main, outputPath, SIZE, PLAN };
