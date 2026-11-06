'use strict';
const { test } = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const resource = path.resolve(__dirname, '../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l01-l03-2021.rich-lesson-v2.json');

test('G5 U01:L01 source Activity 1 identity, timing and MLR2 teaching moves', () => {
  const pack = JSON.parse(fs.readFileSync(resource, 'utf8'));
  const lesson = pack.Lessons.find(x => x.LessonCode === 'PED:US-CCSS-MATH:G5:U01:L01');
  assert.ok(lesson);
  const explanation = lesson.ExplanationParagraphs.join(' ');
  assert.match(explanation, /Activity 1 — Build Objects with Cubes \(15 minutes\)/);
  assert.match(explanation, /5 minutes for individual work and 5 minutes for partner work/);
  assert.match(explanation, /MLR2 Collect and Display/);
  assert.match(explanation, /two pairs/);
  assert.match(explanation, /eight identical cubes/);
  assert.match(explanation, /Edulytiks-original comparison pairs/);
  assert.match(explanation, /gated 2021 publisher Cool-down/);
});
