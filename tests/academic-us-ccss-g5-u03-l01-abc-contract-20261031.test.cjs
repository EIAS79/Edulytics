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

// Edulytiks-original equal-area grid fixtures. Independent expected geometry,
// not an assertion that a selected runtime Practice endpoint grades these models.
const areaModelSourcePath =
  'src/Edulytics.Core/Mathematics/Practice/UsCcssG5U03L01AreaModel.cs';

test('A/B original Rich V2 1/6 and paired 1/8 area grids have visible equal cells', () => {
  const rich = load(contentPath);
  const lesson = rich.Lessons.find(x => x.LessonCode === target);
  assert.equal(rich.ContentVersion,
    'g5-fractions-coordinates-im2021-u03-l01-area-grids-v16-20261101');
  const cases = [
    ['one third of the remaining half (3 rows', 3, 2, 6],
    ['Diagram A: quarter of left half (4 rows', 4, 2, 8],
    ['Diagram B: quarter of top half (2 rows', 2, 4, 8]
  ];
  for (const [title, expectedRows, expectedCols, total] of cases) {
    const visual = lesson.Visuals.find(x => x.Title.includes(title));
    assert.ok(visual, `Missing concrete first-party area model: ${title}`);
    assert.match(visual.Description, /ORIGINAL/i);
    assert.match(visual.Description, /original whole|whole pan|whole-pan|full pan/i);
    assert.equal(visual.Rows.length, expectedRows);
    assert.ok(visual.Rows.every(r => r.Cells.length === expectedCols));
    const cells = visual.Rows.flatMap(r => r.Cells);
    assert.equal(cells.length, total);
    assert.equal(cells.filter(c => c.startsWith('■')).length, 1);
    assert.equal(cells.filter(c => c.startsWith('□')).length, total - 1);
  }
  const originalStudent = lesson.ExplanationParagraphs.join(' ');
  assert.match(originalStudent, /Teacher review is needed/);
  assert.match(originalStudent, /a number alone is insufficient/);
});

test('lesson-only C# model supplies original-whole area, orientation and strict grid geometry', () => {
  const source = fs.readFileSync(path.join(root, areaModelSourcePath), 'utf8');
  assert.match(source, /class UsCcssG5U03L01AreaModel/);
  assert.match(source, /enum CutDirection \{ Horizontal, Vertical \}/);
  assert.match(source, /subdivisionsOfHalf is not \(3 or 4\)/);
  assert.match(source, /originalWidth % columns != 0 \|\| originalHeight % rows != 0/);
  assert.match(source, /new Diagram\(originalWidth, originalHeight, subdivisionsOfHalf/);
  assert.match(source, /1, 2 \* subdivisionsOfHalf/);
  assert.match(source, /!seen\.Add\(\(cell\.X, cell\.Y\)\)/);
  assert.match(source, /seen\.Count == expected\.Count && shaded == 1/);
  assert.match(source, /TeacherReviewRequired => true/);
  assert.match(source, /RepresentsSameAreaOfOriginalWhole/);
  assert.doesNotMatch(source, /LessonPracticeContractRegistry|ExactSkillContractQuestionEngine/);
});

test('independent negative area-grid oracle rejects unequal/overlapping/foreign-half cells', () => {
  // Six unit-area cells on a 2×3 whole; first column is the selected half.
  const correct = Array.from({length: 3}, (_, y) =>
    Array.from({length: 2}, (_, x) => ({x, y, area: 1, shade: x === 0 && y === 0}))).flat();
  const valid = (cells, selectedHalf = 'left') =>
    Array.isArray(cells) && cells.length === 6 &&
    cells.every(c => Number.isSafeInteger(c.x) && Number.isSafeInteger(c.y) &&
      c.x >= 0 && c.x < 2 && c.y >= 0 && c.y < 3 && c.area === 1) &&
    new Set(cells.map(c => `${c.x},${c.y}`)).size === 6 &&
    cells.filter(c => c.shade).length === 1 &&
    cells.some(c => c.shade && (selectedHalf === 'left' ? c.x === 0 : c.y === 0));
  assert.equal(valid(correct), true);
  assert.equal(valid(correct.map(c => c.x === 1 && c.y === 2 ? {...c, area: 0.5} : c)), false);
  assert.equal(valid([...correct.slice(0,5), correct[0]]), false);
  assert.equal(valid(correct.map(c => ({...c, shade: c.x === 1 && c.y === 0}))), false);
  assert.equal(valid(correct.slice(0,5)), false);
  assert.equal(valid(correct.map(c => ({...c, shade: false}))), false);
});

test.todo('PENDING sole engine writer: verify actual U03:L01 lesson-selected family, Build/Solve/Verify dispatch, positive/negative/equivalent/malformed cases and human-review boundary');
