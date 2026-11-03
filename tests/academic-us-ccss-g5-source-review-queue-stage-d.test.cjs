'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const {buildReviewQueue} = require('../tools/curriculum/g5-source-review-queue.cjs');

test('all 148 lessons link hashed original-2021 Student Facing source tasks to a candidate Rich V2 lesson without declaring source parity', () => {
  const q = buildReviewQueue();
  assert.deepEqual(q.errors, []);
  assert.equal(q.summary.lessons, 148);
  assert.equal(q.summary.originalStudentFacingSections, 483);
  assert.equal(q.summary.originalFigureReferences, 527);
  assert.equal(q.summary.distinctFigureRecordIds, 475);
  assert.equal(q.summary.linkedRichV2Lessons, 148);
  assert.equal(q.lessons.length, 148);
  assert.equal(q.summary.mappedAndIndependentlyVerifiedSections, 0);
  assert.equal(q.fullSourceElementDenominator, null);
  assert.equal(q.sourceParityPercent, null);
  assert.equal(q.independentlyAcademicallyApproved, 0);
});
test('source hash and offsets never become an automatic academic review or copyright clearance', () => {
  const q = buildReviewQueue();
  assert.equal(q.lessons.flatMap(x => x.original2021StudentFacingSections).length, 483);
  for (const lesson of q.lessons) {
    assert.equal(lesson.dVerified, false);
    assert.equal(lesson.reviewerRequired, true);
    assert.equal(lesson.sourceRightsReviewRequired, true);
    assert.equal(lesson.academicApproval, 'PENDING_INDEPENDENT_SOURCE_REVIEW');
    assert.ok(lesson.richV2?.path?.startsWith('src/Edulytics.Core/Curriculum/LessonContent/RichV2/'));
    for (const s of lesson.original2021StudentFacingSections) {
      assert.match(s.sourceSectionSha256, /^[0-9a-f]{64}$/);
      assert.equal(s.mappingStatus, 'SOURCE_LOCATED_SEMANTIC_MAPPING_NOT_VERIFIED');
      assert.equal(s.humanSourceApproval, null);
      assert.equal(s.originalStudentPdfStatus, 'NOT_VERIFIED');
    }
  }
});
