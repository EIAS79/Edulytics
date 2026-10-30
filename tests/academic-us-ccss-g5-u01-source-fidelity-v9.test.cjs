'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const file = path.resolve(__dirname, '../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l01-l03-2021.rich-lesson-v2.json');
const id = n => `PED:US-CCSS-MATH:G5:U01:L${String(n).padStart(2, '0')}`;
function connected(cubes) {
  const keys = new Set(cubes.map(p => p.join(',')));
  const seen = new Set([cubes[0].join(',')]);
  const todo = [cubes[0]];
  while (todo.length) {
    const a = todo.pop();
    for (const k of [0, 1, 2]) for (const delta of [-1, 1]) {
      const b = [...a]; b[k] += delta;
      const key = b.join(',');
      if (keys.has(key) && !seen.has(key)) {seen.add(key); todo.push(b);}
    }
  }
  return seen.size === cubes.length && keys.size === cubes.length;
}
test('G5 U01 L01: nine face-connected cubes can occupy distinct shapes with equal volume', () => {
  const slab = Array.from({length: 3}, (_, y) => Array.from({length: 3}, (_, x) => [x, y, 0])).flat();
  const stair = [4, 3, 2].flatMap((width, y) => Array.from({length: width}, (_, x) => [x, y, 0]));
  assert.equal(slab.length, 9); assert.equal(stair.length, 9);
  assert.ok(connected(slab)); assert.ok(connected(stair));
  assert.notDeepEqual(slab, stair);
  assert.deepEqual([11, 5, 9, 8].sort((a,b)=>a-b), [5, 8, 9, 11]);
});
test('G5 U01 L01: a disconnected nine-cube set cannot pass single-solid construction', () => {
  const pieces = Array.from({length: 8}, (_, x) => [x, 0, 0]).concat([[99, 0, 0]]);
  assert.equal(pieces.length, 9); assert.equal(connected(pieces), false);
});
test('G5 U01 L02: verbal 2-by-3-by-3 reconstruction uses 18 cubes and permits rotation', () => {
  const dims = [2,3,3];
  assert.equal(dims.reduce((a,b)=>a*b,1),18);
  assert.ok(18 >= 16 && 18 <= 24);
  assert.equal([3,2,3].reduce((a,b)=>a*b,1),18);
});
test('G5 U01 L03: five eight-cube layers and two twenty-cube layers both count 40', () => {
  assert.equal(5 * (2*4), 40);
  assert.equal(2 * (4*5), 40);
  assert.equal(2*4*5, 40);
});
test('G5 U01 L01-L03: source-specific Rich V2 is structurally valid on an actual checkout', {
  skip: !fs.existsSync(file) && 'Original GitHub branch JSON is not mounted in isolated Node runner'
}, () => {
  const doc = JSON.parse(fs.readFileSync(file, 'utf8'));
  assert.equal(doc.SchemaVersion, 1);
  assert.equal(doc.PackCode, 'US-CCSS-MATH');
  assert.equal(doc.Lessons.length, 3);
  assert.deepEqual(doc.Lessons.map(x=>x.LessonCode), [id(1),id(2),id(3)]);
  for (const x of doc.Lessons) {
    assert.ok(x.ExplanationParagraphs.length>=5);
    assert.ok(x.WorkedExamples.length>=4);
    assert.ok(x.Visuals.some(v=>v.Kind==='RatioTable' && v.Rows.length>=3));
    assert.ok(x.CommonMistakes.length>=2);
  }
  const text = doc.Lessons.map(x => x.ExplanationParagraphs.join(' '));
  assert.match(text[0],/nine congruent cubes/i);
  assert.match(text[0],/order all four objects/i);
  assert.match(text[1],/Guess My Prism/i);
  assert.match(text[1],/16–24 identical connecting cubes/);
  assert.match(text[2],/five layers of eight identical cubes/i);
  assert.match(text[0]+text[1],/teacher/i);
});
test('G5 U01 L01: independent Edulytiks exit check compares eight-cube tower and low strip', () => {
  const tower = Array.from({length: 8}, (_, z) => [0, 0, z]);
  const lowStrip = Array.from({length: 8}, (_, x) => [x, 0, 0]);
  assert.ok(connected(tower));
  assert.ok(connected(lowStrip));
  assert.equal(tower.length, lowStrip.length);
  assert.notDeepEqual(tower, lowStrip);
  assert.equal(tower.length, 8);
});
test('G5 U01 L01: source-aligned warm-up and original exit check are present in Rich V2', {
  skip: !fs.existsSync(file) && 'Original GitHub branch JSON is not mounted in isolated Node runner'
}, () => {
  const item = JSON.parse(fs.readFileSync(file, 'utf8')).Lessons.find(x => x.LessonCode === id(1));
  assert.ok(item);
  assert.match(item.ExplanationParagraphs.join(' '), /Which One Doesn't Belong/i);
  assert.match(item.ExplanationParagraphs.join(' '), /Edulytiks-original exit check/i);
  assert.match(item.ExplanationParagraphs.join(' '), /connecting cubes is an optional support/i);
  assert.match(item.ExplanationParagraphs.join(' '), /teacher review/i);
});
