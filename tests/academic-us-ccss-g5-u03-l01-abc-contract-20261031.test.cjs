'use strict';
// Grade 5 U03:L01 source-aligned independent fixtures.
// AUTHORED_NOT_RUN: do not execute during A-B-C; Stage D only.
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');

const root = path.resolve(__dirname, '..');
const load = p => JSON.parse(fs.readFileSync(path.join(root, p), 'utf8'));
const target = 'PED:US-CCSS-MATH:G5:U03:L01';
const contentPath = 'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-fractions-coordinates-source-lessons-2021.rich-lesson-v2.json';
const rubricPath = 'artifacts/curriculum-delivery/US-CCSS-MATH/G5/u03-l01-original-cooldown-teacher-rubric-v14.json';
const evidencePath = 'artifacts/curriculum-delivery/US-CCSS-MATH/G5/u03-l01-abc-current-lesson-blocker-20261031.json';

const gcd = (a, b) => b ? gcd(b, a % b) : a;
function unitFractionOfUnitFraction(firstDenominator, secondDenominator) {
  if (!Number.isSafeInteger(firstDenominator) ||
      !Number.isSafeInteger(secondDenominator) ||
      firstDenominator < 1 || secondDenominator < 1 ||
      !Number.isSafeInteger(firstDenominator * secondDenominator)) return null;
  const denominator = firstDenominator * secondDenominator;
  const d = gcd(1, denominator);
  return `${1 / d}/${denominator / d}`;
}
function areaModel(halves, divisionsPerSelectedHalf, selectedCell, orientation) {
  if (![halves, divisionsPerSelectedHalf, selectedCell].every(Number.isSafeInteger) ||
      halves < 1 || divisionsPerSelectedHalf < 1 ||
      selectedCell < 0 || selectedCell >= divisionsPerSelectedHalf ||
      !['horizontal', 'vertical'].includes(orientation)) return null;
  return {
    chosenCells: 1,
    cellsInOriginalWhole: halves * divisionsPerSelectedHalf,
    originalWholeFraction: unitFractionOfUnitFraction(halves, divisionsPerSelectedHalf)
  };
}

test('source A1: third of a remaining half is sixth of the original whole', () => {
  assert.equal(unitFractionOfUnitFraction(2, 3), '1/6');
  assert.deepEqual(areaModel(2, 3, 0, 'vertical'), {
    chosenCells: 1, cellsInOriginalWhole: 6, originalWholeFraction: '1/6'
  });
});

test('source A2: two equivalent quarter-of-half diagrams show one eighth', () => {
  assert.equal(unitFractionOfUnitFraction(2, 4), '1/8');
  const a = areaModel(2, 4, 2, 'vertical');
  const b = areaModel(2, 4, 2, 'horizontal');
  assert.deepEqual(a, b);
  assert.equal(a.cellsInOriginalWhole, 8);
});

test('wrong-whole, wrong product and numeric-only diagram claims are not accepted', () => {
  assert.notEqual(unitFractionOfUnitFraction(2, 3), '1/3');
  assert.notEqual(unitFractionOfUnitFraction(2, 4), '1/4');
  assert.notEqual(unitFractionOfUnitFraction(2, 4), '1/6');
  const rubric = load(rubricPath);
  assert.equal(rubric.teacherScored, true);
  assert.equal(rubric.maxPoints, 4);
  assert.match(rubric.acceptance, /numerical answer without a justified diagram cannot earn full credit/i);
});

test('malformed, unequal or degenerate area-model inputs are rejected', () => {
  for (const entry of [[0, 4], [2, 0], [-2, 3], [2.5, 4], ['2', 4], [2, NaN]])
    assert.equal(unitFractionOfUnitFraction(...entry), null);
  for (const entry of [[2, 4, -1, 'horizontal'], [2, 4, 4, 'vertical'],
    [2, 4, 0, 'diagonal'], [2, 0, 0, 'vertical'], [2, 4.5, 0, 'vertical']])
    assert.equal(areaModel(...entry), null);
});

test('source comparison, first-party diagrams, teacher rubric and honest gate status exist', () => {
  const lesson = load(contentPath).Lessons.find(x => x.LessonCode === target);
  const evidence = load(evidencePath);
  const rubric = load(rubricPath);
  assert.ok(lesson);
  assert.equal(evidence.lessonCode, target);
  assert.equal(evidence.status, 'BLOCKED_CURRENT_LESSON');
  assert.equal(evidence.READY_FOR_STAGE_D, false);
  assert.equal(evidence.testExecutionStatus, 'AUTHORED_NOT_RUN');
  assert.equal(rubric.source, 'FIRST_PARTY_EDULYTICS_NOT_ORIGINAL_GATED_2021_COOLDOWN');
  assert.match(lesson.ExplanationParagraphs.join(' '), /between one third and one half/);
  assert.match(lesson.ExplanationParagraphs.join(' '), /DRAW their own diagram/);
  assert.match(lesson.ExplanationParagraphs.join(' '), /Teacher review is needed/);
  assert.ok(lesson.Visuals.length >= 2);
});

test.todo('PENDING sole engine writer: verify actual U03:L01 lesson-selected family, Build/Solve/Verify dispatch, positive/negative/equivalent/malformed cases and human-review boundary');
