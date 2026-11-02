'use strict';
// Stage D handoff integrity only: this test NEVER certifies original media rights,
// source-element semantic parity, teacher reviews, or authenticated runtime behavior.
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');

const root = path.resolve(__dirname, '..');
const dir = 'artifacts/curriculum-delivery/US-CCSS-MATH/G5';
const load = (relative) => JSON.parse(fs.readFileSync(path.join(root, relative), 'utf8'));
const sourceLedger = load(path.join(dir, 'g5-abc-lesson-ledger-v1.json'));
const reviewLedger = load(path.join(dir, 'g5-2021-second-pass-review-ledger-v1.json'));
const counts = [12, 17, 20, 21, 26, 21, 13, 18];
const lessonCode = (u, l) =>
  'PED:US-CCSS-MATH:G5:U' + String(u).padStart(2, '0') +
  ':L' + String(l).padStart(2, '0');

test('2021 second-pass handoff retains all 148 ordered Grade 5 closures with independent evidence', () => {
  assert.equal(sourceLedger.lessons?.length, 148);
  assert.equal(reviewLedger.lessons?.length, 148);
  assert.equal(reviewLedger.reviewComplete2021Count, 148);
  assert.equal(reviewLedger.existingStageDReadyCount, 148);
  assert.equal(reviewLedger.unitsCompleted, 8);
  assert.equal(reviewLedger.nextSecondPassLesson, null);
  assert.equal(reviewLedger.reviewStatus, 'REVIEW_CLOSED_WITH_SOURCE_LIMITATIONS');
  assert.equal(reviewLedger.lastSecondPassClosed, lessonCode(8, 18));
  assert.match(reviewLedger.sourceEdition, /First Edition \(2021\)/);
  const seen = new Set();
  let ordinal = 0;
  for (let u = 1; u <= counts.length; u++) {
    for (let l = 1; l <= counts[u - 1]; l++) {
      const previous = sourceLedger.lessons[ordinal];
      const row = reviewLedger.lessons[ordinal];
      const expected = lessonCode(u, l);
      ordinal++;
      assert.equal(previous.LessonCode, expected, 'baseline order #' + ordinal);
      assert.equal(row.ordinal, ordinal, 'review order #' + ordinal);
      assert.equal(row.lessonCode, expected);
      assert.ok(!seen.has(expected), 'duplicate ' + expected);
      seen.add(expected);
      assert.equal(previous.READY_FOR_STAGE_D === true ||
        previous.READY_FOR_STAGE_D === 'READY_FOR_STAGE_D', true,
        'historical READY must remain #' + ordinal);
      assert.equal(row.readyForStageDPreviousPass, true, expected);
      assert.equal(row.REVIEW_COMPLETE_2021, true, expected);
      assert.equal(row.reviewState, 'CLOSED', expected);
      assert.equal(row.sourceEditionVerified, 'IM K-5 Grade 5 First Edition 2021', expected);
      assert.match(row.reviewCommitSha, /^[0-9a-f]{40}$/, expected);
      const started = Date.parse(row.startedAtUtc);
      const closed = Date.parse(row.closedAtUtc);
      assert.ok(Number.isFinite(started) && Number.isFinite(closed) && closed >= started,
        'invalid closure timestamps ' + expected);
      assert.equal(typeof row.reviewEvidencePath, 'string', expected);
      const target = path.resolve(root, row.reviewEvidencePath);
      assert.ok(target.startsWith(root + path.sep), 'escaped repository path ' + expected);
      assert.ok(fs.statSync(target).isFile(), 'missing closure evidence ' + expected);
      const evidence = load(row.reviewEvidencePath);
      assert.equal(evidence.lessonCode, expected, 'wrong evidence lesson ' + expected);
      assert.equal(evidence.REVIEW_COMPLETE_2021, true, expected);
      assert.equal(evidence.oldReadyForStageD, 'PRESERVED', expected);
      assert.match(evidence.sourceEdition, /2021/, expected);
    }
  }
  assert.equal(ordinal, 148);
  assert.equal(seen.size, 148);
});

test('second-pass completion does not silently promote the Stage D or academic ledger', () => {
  const d = load(path.join(dir, 'grade5-stage-d-execution-ledger.json'));
  assert.equal(d.scope.lessons, 148);
  assert.equal(d.lessons.length, 148);
  assert.equal(d.technicalVerifiedCount, 0);
  assert.equal(d.academicallyApprovedCount, 0);
  for (let ordinal = 0; ordinal < 148; ordinal++) {
    assert.equal(d.lessons[ordinal].lessonCode, reviewLedger.lessons[ordinal].lessonCode);
    assert.notEqual(d.lessons[ordinal].dStatus, 'APPROVED');
  }
});
