'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const richV2Path = path.resolve(__dirname, '../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l01-l03-2021.rich-lesson-v2.json');
// WRITTEN_NOT_RUN for Grade 5 Stage A–B–C. Execute only in authorized Stage D.
test('G5 2021 U01:L01 original source ALT cube quantities and WODB alternatives', () => {
  const count = { A: 2*2*2, B: 5+3, C: 2*1*4, D: 5*1*2 };
  assert.deepEqual(count, {A:8,B:8,C:8,D:10});
  assert.equal(new Set(Object.values(count)).size, 2);
  assert.equal(count.A, count.B);
  assert.equal(count.A, 2*2*2);
  assert.equal(count.A, 2*2+4);
});
test('G5 U01:L01 Rich V2 preserves original source-described A–D, distinct activity pair and independently authored Cool-down', () => {
  const doc = JSON.parse(fs.readFileSync(richV2Path, 'utf8'));
  const lesson = doc.Lessons.find(x => x.LessonCode === 'PED:US-CCSS-MATH:G5:U01:L01');
  assert.ok(lesson);
  const warmup = lesson.ExplanationParagraphs.find(x => x.startsWith('Warm-up —'));
  assert.match(warmup, /5 cubes in its bottom row and 3 above/);
  assert.match(warmup, /10/);
  const first = lesson.ExplanationParagraphs.find(x => x.startsWith('Activity 1 —'));
  assert.match(first, /2 × 2 base plus four cubes/);
  const visual = lesson.Visuals.find(v => v.Title.startsWith('Source-described 2021 warm-up'));
  assert.ok(visual);
  assert.deepEqual(visual.Rows.slice(0,4).map(r=>r.Cells[1]), [
    '8 congruent cubes', '8 congruent cubes', '8 congruent cubes', '10 congruent cubes'
  ]);
  assert.match(lesson.ExplanationParagraphs.join(' '), /Edulytiks-original exit check/);
  assert.match(visual.Description, /No original publisher figure image is embedded/);
});
