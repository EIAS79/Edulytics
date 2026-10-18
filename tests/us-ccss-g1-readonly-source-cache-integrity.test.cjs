'use strict';

// Traceability checks for a recorded READ-ONLY Our-CS source-integrity run.
// CI does not have Our-CS access: passing this test is NOT a live checksum
// verification or a sign-off on educational/source-task correctness.
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');

const root = path.resolve(__dirname, '..');
const read = relative => JSON.parse(fs.readFileSync(path.join(root, relative), 'utf8'));
const location = 'artifacts/curriculum-delivery/US-CCSS-MATH/G1/';
const report = read(location + 'readonly-source-cache-integrity-20261019.json');
const manifest = read(location + 'grade-manifest.json');
const ledger = read(location + 'full-grade-progress.json');

test('recorded Grade 1 read-only source fingerprint comparison covers all 96 candidate lessons', () => {
  assert.equal(report.auditId, 'G1-2026-10-19-OUR-CS-READONLY-CACHE-INTEGRITY');
  assert.equal(report.assessedCandidateSha.length, 40);
  assert.equal(report.scope.firstOrdinal, 51);
  assert.equal(report.scope.lastOrdinal, 146);
  assert.equal(report.scope.lessons, 96);
  assert.equal(report.aggregate.lessonsChecked, 96);
  assert.equal(report.aggregate.teacherFilesSha256Matched, 96);
  assert.equal(report.aggregate.studentFilesSha256Matched, 96);
  assert.equal(report.aggregate.missingFiles, 0);
  assert.equal(report.aggregate.hashMismatches, 0);
  assert.equal(report.unitResults.reduce((sum, row) => sum + row.checked, 0), 96);

  const scope = manifest.lessons.filter(row => row.gradeOrdinal >= 51);
  assert.equal(scope.length, 96);
  for (const unit of report.unitResults) {
    const matching = scope.filter(row => row.unitNumber === unit.unit);
    assert.equal(matching.length, unit.checked, 'Unexpected unit scope');
    assert.equal(unit.teacherMatched, matching.length);
    assert.equal(unit.studentMatched, matching.length);
    assert.ok(matching.every(row => /^[0-9a-f]{64}$/.test(row.teacherPreparationSha256)));
    assert.ok(matching.every(row => /^[0-9a-f]{64}$/.test(row.studentSourceHtmlSha256)));
  }
});

test('source hash verification never upgrades incomplete lessons into academic acceptance', () => {
  assert.equal(report.decision, 'INTEGRITY_CHECK_PASSED_ACADEMIC_AND_RELEASE_NO_GO');
  assert.equal(report.acceptanceBoundaries.lessonSourceGoalParityVerified, false);
  assert.equal(report.acceptanceBoundaries.actualStudentTaskParityVerified, false);
  assert.equal(report.acceptanceBoundaries.authenticLearnerRenderingVerified, false);
  assert.equal(report.acceptanceBoundaries.realPracticeDatabasePersistenceVerified, false);
  assert.equal(report.acceptanceBoundaries.academicReviewApproved, false);
  assert.equal(report.acceptanceBoundaries.ownerUatApproved, false);
  assert.equal(ledger.release, 'NO_GO');
  assert.equal(ledger.newLessons.length, 96);
  assert.ok(ledger.newLessons.every(row => row.status !== 'INTERNALLY_ACCEPTED'));

  const omitted = report.publisherLibraryVariantObservation.additionalPages;
  assert.deepEqual(omitted.map(row => row.sourceLessonCode), ['1.2.23', '1.7.18']);
  assert.ok(omitted.every(row => row.canonicalManifestIncluded === false));
  assert.equal(manifest.lessons.length, 146);
});
