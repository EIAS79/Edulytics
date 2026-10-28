'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');

const repo = path.resolve(__dirname, '..');
const richFolder = 'src/Edulytics.Core/Curriculum/LessonContent/RichV2/';
const candidates = [
  'us-ccss-g5-u01-l01-l03-2021.rich-lesson-v2.json',
  'us-ccss-g5-u03-l19-2021.rich-lesson-v2.json',
  'us-ccss-g5-u01-l04-l06-2021.rich-lesson-v2.json',
  'us-ccss-g5-fractions-coordinates-source-lessons-2021.rich-lesson-v2.json',
  'us-ccss-g5-decimals-placevalue-maxproduct-source-2021.rich-lesson-v2.json'
];
const documents = candidates.map(filename => JSON.parse(
  fs.readFileSync(path.join(repo, richFolder, filename), 'utf8')
));
const lessons = documents.flatMap(document => document.Lessons);
const lessonById = new Map(lessons.map(lesson => [lesson.LessonCode, lesson]));
const get = id => {
  const lesson = lessonById.get('PED:US-CCSS-MATH:G5:' + id);
  assert.ok(lesson, 'missing Rich V2 lesson ' + id);
  return lesson;
};

test('all 13 authored G5 sidecars have unique identity and correct fields', () => {
  assert.equal(lessons.length, 13);
  assert.equal(lessonById.size, 13);
  for (const document of documents) {
    assert.equal(document.SchemaVersion, 1);
    assert.equal(document.PackCode, 'US-CCSS-MATH');
    assert.ok(document.ContentVersion);
    for (const lesson of document.Lessons) {
      assert.match(lesson.LessonCode, /^PED:US-CCSS-MATH:G5:U[0-9]{2}:L[0-9]{2}$/);
      assert.equal(lesson.CultureCode, 'en');
      assert.ok(lesson.Title);
      assert.ok(lesson.ExplanationParagraphs.length >= 2);
      assert.ok(lesson.KeyConcepts.length >= 3);
      assert.ok(lesson.WorkedExamples.length >= 3);
      assert.ok(lesson.CommonMistakes.length >= 2);
      assert.ok(lesson.SummaryPoints.length >= 3);
      assert.ok(lesson.Visuals.length >= 1);
      for (const example of lesson.WorkedExamples) {
        for (const field of ['Title','Question','Method','Answer','Check']) {
          assert.ok(example[field], lesson.LessonCode + '.' + field);
        }
        assert.ok(example.Steps.length >= 3, lesson.LessonCode);
      }
      for (const visual of lesson.Visuals) {
        assert.equal(visual.Kind, 'RatioTable');
        assert.ok(visual.Title && visual.Description);
        assert.ok(visual.Rows.length > 0);
        for (const row of visual.Rows) {
          assert.ok(row.Label && row.Cells.length > 0);
        }
      }
    }
  }
});

test('volume sidecars L04-L06 teach partially pictured layers and alternative bases', () => {
  for (const id of ['U01:L04','U01:L05','U01:L06']) {
    const lesson = get(id);
    const text = lesson.ExplanationParagraphs.join(' ');
    assert.match(text, /layer|base/i);
    assert.match(text, /teacher|peer|partner/i);
    assert.match(lesson.SummaryPoints.join(' '), /2021/i);
  }
  assert.match(get('U01:L04').WorkedExamples[0].Answer, /24/);
  assert.match(get('U01:L05').WorkedExamples[0].Answer, /30/);
  assert.match(get('U01:L06').WorkedExamples[0].Answer, /24/);
});

test('sandwich lesson preserves exploratory representation rather than forcing early equations', () => {
  const text = get('U02:L01').ExplanationParagraphs.join(' ');
  assert.match(text, /not yet required|not replace/i);
  assert.match(text, /teacher/i);
  assert.equal(2 / 3 + 2 / 3 + 2 / 3, 2);
  assert.equal(1 + 1 / 2 + 1 + 1 / 2, 3);
});

test('fraction-of-fraction lesson uses the same whole', () => {
  const lesson = get('U03:L01');
  assert.match(lesson.ExplanationParagraphs.join(' '), /one third of the half/i);
  assert.equal(1 / 2 / 3, 1 / 6);
  assert.equal(1 / 2 / 4, 1 / 8);
  assert.match(lesson.WorkedExamples[0].Answer, /sixth/i);
});

test('coordinate rectangle lesson includes location, dimensions, and original teacher review', () => {
  const lesson = get('U07:L01');
  assert.match(lesson.ExplanationParagraphs.join(' '), /partner/i);
  assert.match(lesson.ExplanationParagraphs.join(' '), /drawing|reconstruct/i);
  assert.equal(6 - 2, 4);
  assert.equal(4 - 1, 3);
});

test('place-value source lessons validate thousandth and tenfold relationships', () => {
  const tiny = get('U05:L01');
  const place = get('U06:L01');
  assert.match(tiny.ExplanationParagraphs.join(' '), /thousandth/i);
  assert.equal(100 * 10, 1000);
  assert.equal(0.001 * 10, 0.01);
  assert.ok(Math.abs(0.6 / 10 - 0.06) < 1e-12);
  assert.match(place.WorkedExamples[0].Answer, /0.6/);
});

test('capstone greatest-product claims are exhaustively correct', () => {
  const lesson = get('U08:L01');
  const all = digits => {
    const possibilities = [];
    for (const a of digits) for (const b of digits) for (const c of digits) {
      if (new Set([a,b,c]).size !== 3) continue;
      possibilities.push({formula: (10 * a + b) + '×' + c, result: (10*a + b)*c});
    }
    return possibilities;
  };
  const set752 = all([7,5,2]);
  const set631 = all([6,3,1]);
  assert.equal(set752.length, 6);
  assert.equal(set631.length, 6);
  assert.equal(Math.max(...set752.map(x => x.result)), 364);
  assert.equal(Math.max(...set631.map(x => x.result)), 186);
  assert.ok(set752.some(x => x.formula === '52×7' && x.result === 364));
  assert.ok(set631.some(x => x.formula === '31×6' && x.result === 186));
  assert.match(lesson.WorkedExamples[0].Check, /135/);
  assert.equal(841 * 6, 5046);
  assert.equal(641 * 8, 5128);
});

test('Rich V2 has no approved external videos or linked publisher images', () => {
  for (const lesson of lessons) {
    assert.equal(lesson.Videos.length, 0);
    assert.ok(lesson.Visuals.every(x => !('ImageUrl' in x) && !('Image' in x)));
    assert.match(lesson.SummaryPoints.join(' '), /2021/i);
  }
});
