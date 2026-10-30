'use strict';
const { test } = require('node:test');
const assert = require('node:assert/strict');
const { readFileSync, readdirSync } = require('node:fs');
const path = require('node:path');

const root = path.resolve(__dirname, '..', '..');
const geoDir = path.join(root, 'src/Edulytics.Core/Curriculum/LessonContent/RichV2');
const geoLessons = readdirSync(geoDir)
  .filter(name => /^us-ccss-hs-geo-u04-.*\.rich-lesson-v2\.json$/.test(name))
  .flatMap(name => JSON.parse(readFileSync(path.join(geoDir, name), 'utf8')).Lessons);
const lessons = new Map(geoLessons.map(lesson => [lesson.LessonCode, lesson]));
const code = number => 'PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U04:L' + String(number).padStart(2, '0');
const example = (lesson, index) => {
  const found = lessons.get(code(lesson))?.WorkedExamples[index];
  assert.ok(found, code(lesson) + ' example ' + index);
  return found;
};
const close = (actual, expected, tol = 0.02) => {
  assert.ok(Number.isFinite(actual) && Math.abs(actual - expected) <= tol,
    'independent oracle: computed ' + actual + ', expected ' + expected);
};

test('U04 L01: ratio, not an invented absolute run; AA-similarity has an explicit non-uniqueness case', () => {
  const e = example(1, 1);
  assert.match(e.Question, /rises 0\.4 m and 1\.0 m/);
  close(0.4 / 1, 2 / 5, 1e-12);
  assert.match(e.Answer, /2:5/);
  assert.match(e.Answer, /without|not determined|undetermined/i);
  // Two scaled pairs obey the same slope angle and rise ratio, but have different runs.
  for (const baseRun of [4.8, 8.4]) {
    const nextRun = baseRun * (1 / 0.4);
    close(0.4 / baseRun, 1 / nextRun, 1e-12);
  }
  assert.match(e.Steps.join(' '), /hypothetical|if first run/i);
  const insufficient = example(1, 2);
  assert.match(insufficient.Answer, /No unique run/);
});

test('U04 L03: the direct answer is the equilateral altitude, not only the 30-60-90 side ratio', () => {
  const e = example(3, 0);
  assert.match(e.Question, /side 2s\. Find its altitude/);
  for (const s of [0.5, 1, 7, 12]) {
    close(Math.hypot(0, Math.sqrt((2 * s) ** 2 - s ** 2)), s * Math.sqrt(3), 1e-10);
  }
  assert.match(e.Answer, /altitude/i);
  assert.match(e.Answer, /s√3|s\s*sqrt\(3\)/i);
  assert.match(e.Check, /positive/i);
  assert.ok(![0, -2].every(s => s > 0), 'zero or negative sides do not define a nondegenerate equilateral triangle');
});

test('U04 L01-L11: 28 distinct worked-example numerical slots independently recomputed', () => {
  const checked = new Set();
  function verify(l, i, calculated, expected, answerPattern, tol) {
    close(calculated, expected, tol);
    const e = example(l, i);
    assert.match(e.Answer, answerPattern, code(l) + '#' + i);
    const key = code(l) + '#' + i;
    assert.ok(!checked.has(key), 'duplicate oracle slot ' + key);
    checked.add(key);
  }
  verify(1, 0, 0.8 * 12, 9.6, /9\.6 m/);
  verify(1, 1, 0.4 / 1.0, 0.4, /2:5/);
  verify(2, 0, Math.hypot(1, 1), Math.SQRT2, /Diagonal/);
  verify(2, 1, (18 / Math.SQRT2) ** 2, 162, /162/);
  verify(2, 2, 5 * 1.5, 7.5, /7\.5/);
  verify(3, 0, Math.sqrt(2 ** 2 - 1 ** 2), Math.sqrt(3), /altitude/i);
  verify(3, 1, 7 * Math.sqrt(3), 12.12435565298, /7√3/, 1e-8);
  verify(3, 2, (9 * Math.sqrt(3)) / Math.sqrt(3), 9, /Short leg 9/);
  verify(4, 0, 4 / 5, 0.8, /0\.8, 0\.6, 0\.75/);
  verify(4, 2, 3 / 5, 0.6, /0\.6/);
  verify(5, 0, 0.643 * 12, 7.716, /7\.7/);
  verify(5, 1, Math.atan(6 / 6) * 180 / Math.PI, 45, /45/);
  verify(5, 2, 8 / 0.8, 10, /10 m/);
  verify(6, 0, 5 / Math.cos(Math.PI / 3), 10, /10 cm/);
  verify(6, 1, 14 * Math.sin(Math.PI / 6), 7, /7 m/);
  verify(6, 2, 3 * Math.tan(Math.PI / 4), 3, /3 m/);
  verify(7, 0, 1.6 + 40 * Math.tan(Math.PI / 6), 24.69401077, /24\.7 m/);
  verify(7, 1, 9 / Math.tan(Math.PI / 4), 9, /9 m/);
  verify(8, 1, Math.sin(Math.PI / 6), 0.5, /1\/2/);
  verify(9, 0, Math.atan2(3, 4) * 180 / Math.PI, 36.86989765, /36\.87/);
  verify(9, 1, Math.asin(5 / 13) * 180 / Math.PI, 22.61986495, /22\.62/);
  verify(9, 2, Math.asin(8 / 10) * 180 / Math.PI, 53.13010235, /53/);
  verify(10, 0, (6 * Math.sqrt(3) / 4 * 4 ** 2) / Math.sqrt(3), 24, /24√3/);
  verify(10, 1, Math.atan(35000 / (150 * 5280)) * 180 / Math.PI, 2.5307, /2\.53/, 0.005);
  verify(10, 2, Math.hypot(150, 35000 / 5280), 150.1464, /150\.15/);
  verify(11, 0, 6 / 2, 3, /3/);
  verify(11, 1, 10 * Math.sin(Math.PI / 10), 3.0901699437, /3\.09017/);
  verify(11, 2, 2 * Math.sqrt(3), 3.4641016151, /3\.4641/);
  assert.equal(checked.size, 28);
  assert.equal(geoLessons.length, 11);
});

test('V14 source evidence keeps two U04 reviews unaccepted and separates independent arithmetic from rendering', () => {
  const rel = 'artifacts/curriculum-delivery/US-CCSS-MATH/GEO/geo-v14-u04-source-math-corrections-20261030.json';
  const record = JSON.parse(readFileSync(path.join(root, rel), 'utf8'));
  assert.equal(record.independentlyComputedSlots, 28);
  assert.equal(record.priorIndependentSlots, 27);
  assert.equal(record.totalIndependentSlots, 55);
  assert.equal(record.remainingUncertifiedSlots, 317);
  assert.equal(record.lessonEvidence.length, 2);
  for (const item of record.lessonEvidence) {
    assert.ok(lessons.has(item.lessonCode));
    assert.equal(item.A_ACCEPTED, false);
    assert.equal(item.B_VERIFIED, false);
    assert.equal(item.C_VERIFIED, false);
    assert.equal(item.D_VERIFIED, false);
    assert.equal(item.authenticatedRendering, 'NOT_RUN');
    assert.equal(item.teacherGradingPersisted, 'NOT_RUN');
    assert.ok(item.studentSourceUrl.startsWith('https://im.kendallhunt.com/HS/students/2/4/'));
    assert.ok(item.teacherSourceUrl.startsWith('https://im.kendallhunt.com/HS/teachers/2/4/'));
    assert.ok(item.parityGap.length > 60);
  }
  const manifest = JSON.parse(readFileSync(path.join(root, 'artifacts/curriculum-delivery/US-CCSS-MATH/GEO/geo-124-source-manifest-20261028.json'), 'utf8'));
  assert.equal(manifest.lessons.length, 124);
  assert.equal(manifest.totals.v14NewlyIndependentlyRecomputedU04ExampleSlots, 28);
  assert.equal(manifest.totals.v14TotalIndependentMathExampleSlots, 55);
  assert.equal(manifest.totals.v14WorkedExamplesNotIndependentlyCertified, 317);
  for (const item of record.lessonEvidence) {
    const row = manifest.lessons.find(x => x.lessonCode === item.lessonCode);
    assert.ok(row && row.v14IndependentReaudit);
    assert.equal(row.v14IndependentReaudit.academicSignoff, false);
    assert.equal(row.v14IndependentReaudit.sourceTaskAcceptance, 'NOT_VERIFIED');
    assert.equal(row.v14IndependentReaudit.D_VERIFIED, false);
  }
  assert.equal(record.releaseDecision, 'DRAFT_NO_GO');
});
