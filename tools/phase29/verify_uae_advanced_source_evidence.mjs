#!/usr/bin/env node
// Validate user-approved UAE Reveal Math textbook TOC provenance without
// claiming official curriculum equivalence or completed lesson content.
import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';
import { fileURLToPath } from 'node:url';
import { dirname, resolve } from 'node:path';

const root = resolve(dirname(fileURLToPath(import.meta.url)), 'evidence');
const inventory = JSON.parse(readFileSync(resolve(root, 'uae_advanced_student_sources_2025_2026.json'), 'utf8'));
const key = (g, v) => inventory.books.find(b => b.grade === g && b.pathway === 'Advanced' && b.volume === v);

assert.equal(inventory.targetAcademicYear, '2025-2026');
assert.equal(inventory.scopePolicy, 'operator-approved-source-not-ministry-current-year-certification');
assert.equal(inventory.publicationPolicy.assertOfficial2026_2027Approval, false);
assert.equal(inventory.publicationPolicy.copyPublisherProse, false);
assert.equal(inventory.publicationPolicy.publishOnlyAfterCurriculumReviewAndStrictAudit, true);

for (const [grade, volume, year, pages] of [[5, 1, '2025-2026', 273], [5, 2, '2023-2024', 289], [6, 1, '2025-2026', 304]]) {
  const book = key(grade, volume);
  assert.ok(book, `Missing source evidence: G${grade} V${volume}`);
  assert.equal(book.sourceAcademicYear, year);
  assert.equal(book.pageCount, pages);
  assert.equal(book.coverVerified, true);
  assert.equal(book.printedUnits[0].number, volume === 2 ? 8 : 1);
  let previous = 0;
  for (const unit of book.printedUnits) {
    assert.ok(Number.isInteger(unit.number) && unit.number > previous, 'Unit numbers must ascend');
    assert.ok(unit.title && unit.title.trim(), 'Missing source-backed unit title');
    previous = unit.number;
    if (unit.verifiedNumberedLessonTitles) {
      assert.equal(unit.verifiedNumberedLessonTitles.length, unit.numberedLessons);
      assert.equal(new Set(unit.verifiedNumberedLessonTitles).size, unit.numberedLessons);
      for (const lesson of unit.verifiedNumberedLessonTitles) assert.ok(lesson.trim().length > 3);
    }
  }
}
assert.equal(key(5, 2).operatorApproved, true);
const count = (g, v, maxUnit = 999) => key(g, v).printedUnits
  .filter(u => u.number <= maxUnit)
  .reduce((sum, u) => sum + (u.numberedLessons || 0), 0);
assert.equal(count(5, 1), 44, 'G5 Advanced V1 printed TOC lessons');
assert.equal(count(5, 2), 48, 'G5 Advanced V2 printed TOC lessons');
assert.equal(count(6, 1, 5), 33, 'G6 Advanced V1 first five modules');
assert.equal(count(6, 1), 59, 'G6 Advanced 2025-2026 V1 all 10 modules');
console.log('UAE_ADVANCED_SOURCE_EVIDENCE_PASS: G5 V1=44, G5 V2=48, G6 V1=59 (first 5 modules=33)');
console.log('This does NOT assert full 64/64 closure or approve any production deployment.');
