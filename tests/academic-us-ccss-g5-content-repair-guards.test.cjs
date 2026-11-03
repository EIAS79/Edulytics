'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');

const root = path.resolve(__dirname, '..');
const read = (relativePath) => JSON.parse(
  fs.readFileSync(path.join(root, relativePath), 'utf8')
);
const blueprint = read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-grade5-im-k5-2021.lesson-blueprint.json');
const pack = read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g5-phase29-v1.lesson-content-pack.json');
const lessonByCode = new Map(pack.lessons.map(row => [row.lessonCode, row]));
const translation = (code) => {
  const row = lessonByCode.get(code);
  assert.ok(row, 'missing lesson: ' + code);
  const english = row.translations.find(item => item.cultureCode === 'en');
  assert.ok(english, 'missing English content: ' + code);
  return english;
};

test('Grade 5: exactly 148 source-bound unique canonical lessons in eight units', () => {
  assert.equal(blueprint.BlueprintCode, 'US-CCSS-MATH:G5:IM-K5-2021');
  assert.equal(blueprint.Lessons.length, 148);
  assert.equal(pack.lessons.length, 148);
  assert.equal(lessonByCode.size, 148);
  assert.equal(new Set(blueprint.Lessons.map(x => x.LessonCode)).size, 148);
  for (const lesson of blueprint.Lessons) {
    assert.ok(lessonByCode.has(lesson.LessonCode), lesson.LessonCode);
    assert.equal(lessonByCode.get(lesson.LessonCode).sourceUrl, lesson.SourceUrl);
    assert.equal(lessonByCode.get(lesson.LessonCode).sourceEdition, '1st Edition (2021)');
    assert.ok(/^PED:US-CCSS-MATH:G5:U\d{2}:L\d{2}$/.test(lesson.LessonCode));
    assert.ok(lesson.Alignments.length > 0, lesson.LessonCode);
  }
  assert.equal(blueprint.Units.length, 8);
  assert.deepEqual(blueprint.Units.map(u => u.LessonCount), [12, 17, 20, 21, 26, 21, 13, 18]);
});

test('Grade 5: the original unit-volume learner guidance is three-dimensional', () => {
  for (let lessonNumber = 1; lessonNumber <= 10; lessonNumber++) {
    const code = 'PED:US-CCSS-MATH:G5:U01:L' + String(lessonNumber).padStart(2,'0');
    const body = translation(code);
    assert.match(body.stepByStepSolutions, /cubes|layers|prisms|volume|base area/i, code);
    assert.doesNotMatch(body.stepByStepSolutions, /compare sides, angles, corners/i, code);
    assert.doesNotMatch(body.keyConceptsAndRules, /side[s]?, angles, parallelism, or length—not size/i, code);
    assert.match(body.commonMistakes, /volume|cubes|base area|prisms|cubic units/i, code);
  }
});

test('Grade 5: selected fraction, coordinate and decimal-division lessons avoid unrelated templates', () => {
  const cases = [
    ['U02:L01', /sharing|equal|whole|people/i, /greater, less, or equal/i],
    ['U03:L01', /fraction|partition|whole/i, /starting point|endpoint, scale/i],
    ['U05:L23', /divisor|divide|quotient/i, /greater, less, or equal/i],
    ['U05:L24', /divisor|divide|quotient/i, /greater, less, or equal/i],
    ['U05:L25', /divisor|divide|quotient/i, /greater, less, or equal/i],
    ['U07:L03', /coordinate|axis|point/i, /categories, and scale/i],
    ['U08:L01', /product|factors|estimate/i, /what needs to be measured/i]
  ];
  for (const [shortCode, expected, rejected] of cases) {
    const code = 'PED:US-CCSS-MATH:G5:' + shortCode;
    const steps = translation(code).stepByStepSolutions;
    assert.match(steps, expected, code);
    assert.doesNotMatch(steps, rejected, code);
  }
});

test('Grade 5: preserve existing fraction-area lesson-specific solution', () => {
  const body = translation('PED:US-CCSS-MATH:G5:U02:L14');
  assert.match(body.stepByStepSolutions, /Step 3: Multiply the whole number and fraction/);
  assert.match(body.stepByStepSolutions, /square units/);
});

test('Grade 5: baseline lesson content does not itself confer runtime or academic acceptance', () => {
  // These checks establish only static source-content consistency.
  // They deliberately do not assert final Practice routing, deployed SHA,
  // database materialization, teacher review, owner UAT or academic approval.
  assert.equal(pack.status, 'Published');
  assert.equal(pack.packCode, 'US-CCSS-MATH');
});


test('Grade 5: remove false clock-hand and measurement rules from fraction/decimal lessons', () => {
  const cases = [
    ['U02:L16', /fraction.*greater than 1/, /hour and minute hands/],
    ['U06:L21', /line plot/, /hour and minute hands/],
    ['U08:L12', /decimal game/i, /Measurement units must be equal/],
    ['U08:L10', /same denominator/, /Subtraction can represent/],
    ['U04:L06', /tens-row partial product/, /Measurement units must be equal/]
  ];
  for (const [shortCode, expected, rejected] of cases) {
    const body = translation('PED:US-CCSS-MATH:G5:' + shortCode);
    assert.match(body.keyConceptsAndRules, expected, shortCode);
    assert.doesNotMatch(body.keyConceptsAndRules, rejected, shortCode);
  }
});

test('Grade 5: human-reviewed investigations remain written tasks', () => {
  for (const code of ['U03:L19', 'U06:L21', 'U08:L13']) {
    const body = translation('PED:US-CCSS-MATH:G5:' + code);
    assert.match(body.stepByStepSolutions, /teacher|peer|partner/i, code);
  }
});
