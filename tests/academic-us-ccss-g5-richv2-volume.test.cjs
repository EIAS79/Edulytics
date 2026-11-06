'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const repo = path.resolve(__dirname, '..');
const rich = JSON.parse(fs.readFileSync(
  path.join(repo, 'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l01-l03-2021.rich-lesson-v2.json'), 'utf8'
));
const suffix = id => 'PED:US-CCSS-MATH:G5:U01:L' + String(id).padStart(2,'0');
const lesson = n => rich.Lessons.find(x => x.LessonCode === suffix(n));

test('three unique Rich V2 lessons have required first-party content fields', () => {
  assert.equal(rich.SchemaVersion, 1);
  assert.equal(rich.PackCode, 'US-CCSS-MATH');
  assert.equal(rich.Lessons.length, 3);
  assert.equal(new Set(rich.Lessons.map(x => x.LessonCode)).size, 3);
  for (const [index, item] of rich.Lessons.entries()) {
    assert.equal(item.LessonCode, suffix(index+1));
    assert.equal(item.CultureCode, 'en');
    assert.ok(item.ExplanationParagraphs.length >= 2);
    assert.ok(item.KeyConcepts.length >= 3);
    assert.ok(item.WorkedExamples.length >= 3);
    assert.ok(item.CommonMistakes.length >= 2);
    assert.ok(item.SummaryPoints.length >= 3);
    assert.ok(item.Visuals.length >= 1);
    for (const example of item.WorkedExamples) {
      for (const field of ['Title', 'Question', 'Method', 'Answer']) {
        assert.ok(example[field].length > 0, item.LessonCode + ':' + field);
      }
      assert.ok(example.Steps.length >= 3, example.Title);
    }
  }
});

test('L01 is conceptual, with open-ended models and teacher review', () => {
  const item = lesson(1);
  assert.match(item.ExplanationParagraphs.join(' '), /different.*eight.*cubes|eight.*different.*cubes/i);
  assert.match(item.ExplanationParagraphs.join(' '), /teacher-assessed|human review|teacher-reviewed/i);
  assert.match(item.WorkedExamples[0].Answer, /eight/i);
  assert.doesNotMatch(item.ExplanationParagraphs[0], /length.{0,20}width.{0,20}height/i);
});

test('L02 and L03 explicitly count hidden cubes and equal layers', () => {
  for (const n of [2,3]) {
    const item = lesson(n);
    assert.match(item.ExplanationParagraphs.join(' '), /layers|cube/i);
    assert.match(item.WorkedExamples.map(e => e.Method).join(' '), /layers|cube|hidden|count/i);
    assert.match(item.SummaryPoints.join(' '), /2021/);
  }
  assert.match(lesson(3).WorkedExamples[0].Answer, /24 cubic units/);
});

test('visuals are textually accessible without publisher artwork', () => {
  for (const item of rich.Lessons) {
    assert.ok(item.Visuals.every(v => v.Description && v.Rows.length > 0));
    assert.ok(item.Visuals.every(v => v.Rows.every(row => row.Label && row.Cells.length > 0)));
    assert.equal(item.Videos.length, 0);
  }
});
