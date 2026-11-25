'use strict';
// Grade 10 A–B–C sequential handoff guard. These are Stage D test sources;
// creating this file does not certify that the tests have run.
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');

const root = path.resolve(__dirname, '..');
const read = relative => JSON.parse(fs.readFileSync(path.join(root, relative), 'utf8'));
const evidence = 'artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const folder = path.join(root, evidence);
const blueprint = read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const pack = read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const checkpoints = fs.readdirSync(folder)
  .filter(name => /^lesson-\d{3}-stage-a-c-checkpoint\.json$/.test(name))
  .map(name => ({ file: name, checkpoint: read(evidence + name) }));
const byOrdinal = new Map();

test('Grade 10 General checkpoints have unique, correctly numbered canonical identities', () => {
  assert.equal(blueprint.Units.length, 18);
  assert.equal(blueprint.Lessons.length, 148);
  for (const { file, checkpoint } of checkpoints) {
    const ordinal = Number(file.match(/^lesson-(\d{3})-/)[1]);
    assert.equal(checkpoint.canonicalOrdinal, ordinal, file);
    assert.equal(checkpoint.grade, 10, file);
    assert.equal(checkpoint.pathway, 'General', file);
    assert.ok(ordinal >= 1 && ordinal <= 148, file);
    assert.ok(!byOrdinal.has(ordinal), 'duplicate checkpoint for ordinal ' + ordinal);
    byOrdinal.set(ordinal, checkpoint);
  }
});

test('READY_FOR_STAGE_D is a contiguous prefix; missing-source lessons block all successors', () => {
  // Build this map independently: tests do not rely on test execution order.
  const records = new Map(checkpoints.map(({ checkpoint }) => [checkpoint.canonicalOrdinal, checkpoint]));
  let expectedOrdinal = 1;
  for (let ordinal = 1; ordinal <= 148; ordinal++) {
    const checkpoint = records.get(ordinal);
    if (checkpoint?.status === 'READY_FOR_STAGE_D') {
      assert.equal(ordinal, expectedOrdinal, 'cannot close lesson ' + ordinal + ' before lesson ' + expectedOrdinal);
      assert.equal(checkpoint.productionRouting, false, 'A–B–C closure must not enable production routing');
      expectedOrdinal++;
    }
    if (checkpoint?.sourceBodyVerified === false) {
      assert.notEqual(checkpoint.status, 'READY_FOR_STAGE_D', 'cannot close lesson ' + ordinal + ' with unverified source body');
    }
  }
  const firstIncomplete = records.get(expectedOrdinal);
  if (firstIncomplete) {
    assert.notEqual(firstIncomplete.status, 'READY_FOR_STAGE_D');
    assert.equal(firstIncomplete.firstIncompleteGlobalOrdinal, expectedOrdinal);
  }
});

test('Lesson 43 stays an explicit source gap until original pages are verified and authored', () => {
  const lesson = read(evidence + 'lesson-043-stage-a-c-checkpoint.json');
  const expectedCode = 'PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U05:L02:C043';
  const identity = blueprint.Lessons.find(x => x.LessonCode === expectedCode);
  assert.ok(identity, 'approved lesson identity must be preserved');
  assert.equal(identity.Title, 'حل المعادلات والمتباينات الأسية');
  assert.equal(identity.SortOrder, 43);

  if (lesson.status === 'A_C_EXPLICIT_GAP') {
    assert.equal(lesson.sourceIndexTitleVerified, true);
    assert.equal(lesson.sourceBodyVerified, false);
    assert.equal(lesson.sourceElementDenominatorForThisLesson, null);
    assert.equal(lesson.sourceElementsMapped, 0);
    assert.equal(lesson.productionRouting, false);
    assert.equal(lesson.firstIncompleteGlobalOrdinal, 43);
    assert.equal(pack.Lessons.some(x => x.LessonCode === expectedCode), false,
      'a TOC-only identity cannot be counted as a completed Arabic student lesson');
    const unit = read(evidence + 'unit-05-stage-a-c-progress.json');
    assert.equal(unit.gradeClosedCount, 42);
    assert.equal(unit.firstIncompleteGlobalOrdinal, 43);
  } else {
    assert.equal(lesson.status, 'READY_FOR_STAGE_D',
      'gap can only be resolved by a subsequently certified A–B–C close');
    assert.equal(lesson.sourceBodyVerified, true);
    assert.ok(lesson.sourceElementDenominatorForThisLesson > 0);
    assert.ok(pack.Lessons.some(x => x.LessonCode === expectedCode));
  }
});
