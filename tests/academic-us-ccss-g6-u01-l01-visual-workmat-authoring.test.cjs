'use strict';
// SOURCE-SCOPED AUTHORING TESTS ONLY. Intentionally AUTHORED_NOT_RUN until
// Grade 6 course-wide Stage D. NOT production Practice, authenticated UI,
// renderer, persistence or teacher-scoring E2E certification.
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const read = p => fs.readFileSync(path.join(root, p), 'utf8');
const labRoot = 'src/Edulytics.Web/wwwroot/lesson-labs/us-ccss-g6/';
const html = read(labRoot + 'u01-l01-tiling-lab.html');
const css = read(labRoot + 'u01-l01-tiling-lab.css');
const js = read(labRoot + 'u01-l01-tiling-lab.js');
const svg = read('src/Edulytics.Web/wwwroot/images/public/us-ccss-g6-u01-l01-first-party-tilings.svg');
const rich = JSON.parse(read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g6-u01-l01-l03-area-20261028.rich-lesson-v2.json'));
const lesson = rich.Lessons.find(l => l.LessonCode === 'PED:US-CCSS-MATH:G6:U01:L01');

test('U01:L01 content points to its own original workmat without marking Practice as integrated', () => {
  assert.ok(lesson);
  assert.ok(lesson.ExplanationParagraphs.some(x => x.includes('/lesson-labs/us-ccss-g6/u01-l01-tiling-lab.html')));
  assert.ok(lesson.ExplanationParagraphs.some(x => /NOT the selected or persisted Practice assessment/.test(x)));
  assert.equal(rich.Lessons.filter(l => l.ExplanationParagraphs.some(x => x.includes('/lesson-labs/us-ccss-g6/'))).length, 1);
  assert.match(svg, /href="\/lesson-labs\/us-ccss-g6\/u01-l01-tiling-lab.html"/);
});

test('U01:L01 workmat has accessible controls, original illustration, and nonautomatic free-response prompts', () => {
  assert.match(html, /<html lang="en">/);
  assert.match(html, /alt="Edulytiks-original patterns:/);
  assert.match(html, /data-preset="a"/);
  assert.match(html, /data-preset="b"/);
  assert.match(html, /data-preset="v"/);
  assert.match(html, /data-preset="h"/);
  assert.match(html, /data-preset="gap"/);
  assert.match(html, /data-preset="trick"/);
  assert.match(html, /id="class-reason"/);
  assert.match(html, /id="construction-reason"/);
  assert.match(html, /id="comparison-reason"/);
  assert.match(html, /aria-live="polite"/);
  assert.match(js, /label.htmlFor = id/);
  assert.match(css, /focus-visible/);
  assert.match(css, /@media print/);
});

test('U01:L01 teaching UI separates count, piece-area sum, region coverage, gaps and overlaps', () => {
  for (const metric of ['square-count', 'triangle-count', 'area-sum', 'covered-area', 'gap-area', 'overlap-area']) {
    assert.match(html, new RegExp('id="' + metric + '"'));
    assert.match(js, new RegExp("'"+metric+"'"));
  }
  assert.match(js, /trick: \['square', 'pair', 'half', 'overlap'\]/);
  assert.match(js, /const gaps = 4 - covered/);
  assert.match(js, /const overlaps = pieceArea - covered/);
  assert.match(js, /a piece-area sum alone cannot prove coverage/);
  assert.match(html, /Most pieces:<\/strong> green \(56\)/);
  assert.match(html, /Greatest area:<\/strong> red \(72 triangular units\)/);
  assert.match(html, /8 × 3 = <strong>24<\/strong>/);
  assert.match(html, /<strong>72<\/strong>/);
  assert.match(html, /<strong>64<\/strong>/);
  assert.match(html, /<strong>56<\/strong>/);
});

test('Teacher rubric is human-entered with four criteria, not automatically graded or saved', () => {
  for (const criterion of ['rubric-coverage', 'rubric-units', 'rubric-distinct', 'rubric-reasoning']) {
    assert.match(html, new RegExp('id="' + criterion + '"'));
  }
  assert.match(html, /not automatic scoring/i);
  assert.match(html, /No answer, rubric or feedback is transmitted/);
  assert.match(html, /id="teacher-comments"/);
  assert.match(js, /Manually assigned, not submitted or persisted/);
  assert.doesNotMatch(js, /\bfetch\s*\(/);
  assert.doesNotMatch(js, /\blocalStorage\b|\bsessionStorage\b|\bXMLHttpRequest\b/);
  assert.doesNotMatch(html, /<form\b/i);
});
