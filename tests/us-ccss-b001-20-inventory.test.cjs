'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const { buildManifest, outputPath, SIZE, PLAN } =
  require('../tools/curriculum/us-ccss-b001-20-inventory.cjs');

test('New B001 is an immutable 20-ID canonical ordered cohort, never a recast 500 run', () => {
  const manifest = buildManifest();
  assert.equal(manifest.planId, PLAN);
  assert.equal(manifest.total, SIZE);
  assert.equal(manifest.lessons.length, SIZE);
  assert.equal(new Set(manifest.lessons.map(x => x.lessonCode)).size, SIZE);
  assert.deepEqual(manifest.orderedProgrammePositions, [1,20]);
  assert.deepEqual(manifest.lessons.map(x => x.gradeOrdinal), Array.from({length:20},(_,i)=>i+1));
  assert(manifest.lessons.every(x => x.gradeOrCourse === 'Grade 1'));
  assert.equal(manifest.lessons[0].lessonCode, 'PED:US-CCSS-MATH:G1:U01:L01');
  assert.equal(manifest.lessons[19].gradeOrdinal, 20);
  assert.match(manifest.legacyCatalogueSha256, /^[0-9a-f]{64}$/);
  assert.equal(manifest.academicAcceptance, 'NOT_STARTED');
  assert.equal(manifest.acceptedUnderCurrent20Policy, 0);
});

test('The 15 historically shipped positions remain distinct from the five future positions', () => {
  const lessons = buildManifest().lessons;
  assert.equal(lessons.filter(x => x.historicalPreviouslyShipped).length, 15);
  assert.deepEqual(lessons.slice(0,10).map(x=>x.historicalRelease),Array(10).fill('T001'));
  assert.deepEqual(lessons.slice(10,15).map(x=>x.historicalRelease),Array(5).fill('GRADE_1_BATCH_03'));
  assert(lessons.slice(15).every(x => x.historicalRelease === null && !x.historicalPreviouslyShipped));
  assert(lessons.every(x => x.historicalReleaseDriftVerified === false));
  assert(lessons.every(x => x.academicAcceptanceStatus === 'NOT_ACCEPTED_UNDER_V2_20'));
});

test('Committed 20 intake matches actual canonical source and remains separate from 500 historical JSON', () => {
  assert.deepEqual(JSON.parse(fs.readFileSync(outputPath,'utf8')), buildManifest());
});
