'use strict';

const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const { buildCatalogue } = require('../tools/curriculum/us-ccss-b001-inventory.cjs');
const root = path.resolve(__dirname, '..');
const manifestPath = path.join(root, 'docs/curriculum/batches/US-CCSS-B001-CATALOGUE.json');

test('B001 is reproducible and matches committed source+stored-content manifest', () => {
  const expected = buildCatalogue();
  const committed = JSON.parse(fs.readFileSync(manifestPath, 'utf8'));
  assert.deepEqual(committed, expected, 'The B001 manifest has drifted from canonical source/content; run extractor --write');
  assert.equal(expected.total, 500);
  assert.equal(expected.summary.canonicalSourceCodesIndexed, 500);
  assert.equal(expected.summary.storedContentRowsCrossReferenced, 500);
  assert.equal(new Set(expected.lessons.map(r => r.lessonCode)).size, 500);
  assert.deepEqual(expected.segments.map(x => x.includedLessonCount), [146, 146, 143, 65]);
});

test('B001 exact source order and grade boundaries do not re-label any lessons', () => {
  const lessons = buildCatalogue().lessons;
  assert.equal(lessons[0].lessonCode, 'PED:US-CCSS-MATH:G1:U01:L01');
  assert.equal(lessons[10].lessonCode, 'PED:US-CCSS-MATH:G1:U01:L11');
  assert.equal(lessons[145].gradeOrCourse, 'Grade 1');
  assert.equal(lessons[146].gradeOrCourse, 'Grade 2');
  assert.equal(lessons[291].gradeOrCourse, 'Grade 2');
  assert.equal(lessons[292].gradeOrCourse, 'Grade 3');
  assert.equal(lessons[434].gradeOrCourse, 'Grade 3');
  assert.equal(lessons[435].gradeOrCourse, 'Grade 4');
  assert.equal(lessons[499].lessonCode, 'PED:US-CCSS-MATH:G4:U04:L20');
  for (const [index, lesson] of lessons.entries()) {
    assert.equal(lesson.batchOrdinal, index + 1);
    assert.match(lesson.lessonCode, /^PED:US-CCSS-MATH:G[1-4]:U\d+:L\d+$/);
    assert.ok(lesson.sourceUrl.startsWith('https://'));
    assert.match(lesson.semanticSha256, /^[a-f0-9]{64}$/i);
    assert.match(lesson.storedContentSha256, /^[a-f0-9]{64}$/i);
    assert.ok(lesson.outcomeCodes.length > 0);
  }
});

test('An indexed lesson must not be misrepresented as academically accepted or released', () => {
  const manifest = buildCatalogue();
  assert.equal(manifest.productionDbReconciliation, 'NOT_RUN');
  assert.equal(manifest.mathematicalGenerationVerification, 'NOT_RUN');
  assert.equal(manifest.lessonAcademicAcceptance, 'NOT_RUN');
  assert.equal(manifest.authoritativeRuntimeInventoryVerified, false);
  assert.equal(manifest.gradeSourcesCheckedAgainstActualPublishers, false);
  assert.equal(manifest.summary.previouslyPublishedT001LessonPositions, 10);
  assert.equal(manifest.summary.newLessonPositionsToReview, 490);
  assert.equal(manifest.summary.independentlyAcademicallyAcceptedInThisRun, 0);
  assert.equal(manifest.summary.newLessonsReleasedInThisRun, 0);
  assert.equal(manifest.lessons.filter(x => x.historicalT001).length, 10);
  assert.equal(manifest.lessons.filter(x => x.academicAcceptanceStatus === 'NOT_REVIEWED').length, 490);
  assert.equal(manifest.lessons.filter(x => x.academicAcceptanceStatus === 'HISTORICAL_T001_DRIFT_CHECK_REQUIRED').length, 10);
  for (const lesson of manifest.lessons) {
    assert.equal(lesson.practiceVerification, 'NOT_RUN');
    assert.equal(lesson.studentVisibleRendering, 'NOT_TESTED');
    assert.equal(lesson.productionDbReconciliation, 'NOT_RUN');
  }
});
