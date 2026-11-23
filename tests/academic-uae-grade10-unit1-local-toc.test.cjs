'use strict';

// Authored against the local seven-book source ledger. Stage A–B–C must NOT run this
// test or claim PASS; it belongs to later Stage D. The 156 candidate count remains
// preliminary and MUST NOT be treated as final canonical lesson count.
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const load = (filename) => JSON.parse(fs.readFileSync(path.join(root, filename), 'utf8'));

const index = load('artifacts/curriculum-delivery/UAE-MOE-MATH/G10/source-index-preliminary.json');
const pack = load('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const inventory = load('artifacts/curriculum-delivery/UAE-MOE-MATH/G10/source-inventory.json');

const fromBook = [
  ['جمع كثيرات الحدود وطرحها', 7],
  ['ضرب كثيرة حدود في أحادية الحد', 14],
  ['ضرب كثيرات الحدود', 22],
  ['نواتج الضرب الخاصة', 28],
  ['استخدام خاصية التوزيع', 36],
  ['حل المعادلات التربيعية على الصورة x² + bx + c = 0', 45],
  ['حل المعادلات التربيعية على الصورة ax² + bx + c = 0', 52],
  ['الفرق بين مربعين', 58],
  ['المربعات الكاملة', 64],
];

test('Grade10 General Unit1 9 candidate titles remain in verified local book order', () => {
  assert.equal(fromBook.length, 9);
  assert.equal(new Set(fromBook.map(x => x[0])).size, 9, 'unit lesson titles must be distinct');
  for (let i = 0; i < fromBook.length; i++) {
    const row = index.rows[i];
    assert.equal(row.candidateOrdinal, i + 1);
    assert.equal(row.sourceLessonNumbersPreliminary, `1-${i+1}`);
    assert.equal(row.titleArabicCandidate, fromBook[i][0]);
    assert.equal(row.bookPrintedOpeningPage, fromBook[i][1]);
    assert.equal(row.bookTocPhysicalPdfPage, 5);
    assert.equal(row.bookSha256,
      'd7e492a960675c30ac7eebb0144ed2dc51d1f4e9c91c9834ea05638f614a3c72');
    assert.match(row.currentPositionLinkStatus, /GOAL_VERIFICATION_PENDING/);
    assert.ok(pack.Lessons.some(lesson =>
      lesson.LessonCode === row.currentGeneralPositionCandidate), 'existing lesson must be reused');
  }
});

test('seven-book source hashes do not masquerade as semantic validation', () => {
  assert.equal(inventory.fileCount, 7);
  assert.equal(inventory.allSevenFileHashesMatched, true);
  assert.equal(index.sourceLock, false, 'do not silently mark global source index approved');
  assert.equal(inventory.sourceLock, false, 'book hashes are not proof of semantic coverage');
});
