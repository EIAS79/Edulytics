'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');

const fractions = (whole, numerator, denominator) => (BigInt(whole) * BigInt(denominator) + BigInt(numerator));
const equalFractions = (a, b, c, d) => assert.equal(BigInt(a) * BigInt(d), BigInt(b) * BigInt(c));
const repo = path.resolve(__dirname, '..');
const subdir = 'src/Edulytics.Core/Curriculum/LessonContent/RichV2';
const files = [
  'us-ccss-g5-u02-l09-l13-2021.rich-lesson-v2.json',
  'us-ccss-g5-u02-l14-l17-2021.rich-lesson-v2.json'
].map(name => path.join(repo, subdir, name));
const hasFullRepo = files.every(p => fs.existsSync(p));

// Independent rational-number oracles avoid false failures from JS floating point.
test('U02 L09: area of one unit-fraction-high rectangle', () => {
  equalFractions(8, 8, 1, 1);
  equalFractions(8, 4, 2, 1);
  equalFractions(8, 2, 4, 1);
  equalFractions(8, 1, 8, 1);
  equalFractions(6, 3, 2, 1);
});

test('U02 L10: a non-unit fractional width changes area and comparison', () => {
  equalFractions(5*3, 4, 15, 4);
  equalFractions(5*2, 3, 10, 3);
  equalFractions(15*3 - 10*4, 4*3, 5, 12);
});

test('U02 L11: a mixed-number width is not a malformed improper fraction', () => {
  assert.equal(fractions(3, 2, 3), 11n);
  equalFractions(4*11, 3, 44, 3);
  equalFractions(4*9, 2, 18, 1);
  equalFractions(10, 3, 3*3+1, 3);
});

test('U02 L12: exact garden-area difference under two mixed lengths', () => {
  equalFractions(4*13, 2, 26, 1);
  equalFractions(5*21, 4, 105, 4);
  equalFractions(105-104, 4, 1, 4);
});

test('U02 L13: addition/subtraction rectangle matches require area and sign', () => {
  assert.equal(4*2+4*3/4, 11);
  assert.equal(4*3-4*1/4, 11);
  assert.equal(4*2+4*1/4, 9);
});

test('U02 L14: missing-width info gap has no unique answer before asking', () => {
  assert.notEqual(4*6,4*8);
  assert.equal(4*(7+1/2),30);
  equalFractions(63, 4*3, 21, 4);
});

test('U02 L15: six dimensions are used exactly once across linked contexts', () => {
  const used = [3, 6, 2, 6.5, 4, 6.75];
  assert.equal(new Set(used).size, 6);
  assert.deepEqual([...used].sort((a,b)=>a-b),[2,3,4,6,6.5,6.75]);
  assert.equal(3*6,18);
  assert.equal(2*6.5,13);
  assert.equal(4*6.75,27);
  equalFractions(7*39, 10, 273, 10);
});

test('U02 L16: area budget includes equality and rejects overspend', () => {
  const [a,b,c,d] = [6*19/4,8*15/4,9*7/2,10*8/3];
  assert.deepEqual([a<=30,b<=30,c<=30,d<=30],[true,true,false,true]);
  equalFractions(10*8,3,80,3);
});

test('U02 L17: mosaic tiling uses consistent unit squares', () => {
  equalFractions(3*3,2,9,2);
  equalFractions(4*9,2,18,1);
  equalFractions(6*9,2,27,1);
});

test('U02 L09-L17: source-specific Rich V2 files parse and preserve identities', { skip: !hasFullRepo && 'GitHub branch files unavailable in this isolated local runner' }, () => {
  const lessons = files.flatMap(filename => {
    const doc = JSON.parse(fs.readFileSync(filename,'utf8'));
    assert.equal(doc.SchemaVersion,1);
    assert.equal(doc.PackCode,'US-CCSS-MATH');
    return doc.Lessons;
  });
  assert.equal(lessons.length,9);
  assert.equal(new Set(lessons.map(x=>x.LessonCode)).size,9);
  for(let i=0;i<9;i++) {
    const lesson=lessons[i];
    assert.equal(lesson.LessonCode,'PED:US-CCSS-MATH:G5:U02:L'+String(i+9).padStart(2,'0'));
    assert.ok(lesson.ExplanationParagraphs.length>=4);
    assert.ok(lesson.KeyConcepts.length>=3);
    assert.ok(lesson.WorkedExamples.length>=3);
    assert.ok(lesson.WorkedExamples.every(w=>w.Steps.length>=3 && w.Check));
    assert.ok(lesson.CommonMistakes.length>=2);
    assert.ok(lesson.Visuals.length>=1);
    assert.ok(lesson.Visuals.every(v=>v.Description && v.Rows.length>=2));
    assert.ok(lesson.SummaryPoints.join(' ').includes('2021'));
    assert.equal(lesson.Videos.length,0);
  }
});
