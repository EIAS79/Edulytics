'use strict';

const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');

const root = path.resolve(__dirname, '..');
const pack = JSON.parse(fs.readFileSync(path.join(
  root, 'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json'
), 'utf8'));
const registry = fs.readFileSync(path.join(
  root, 'src/Edulytics.Core/Curriculum/MathematicsCurriculumPackRegistry.cs'
), 'utf8');

test('Grade 10 Arabic U01L01 is an enhancement of the original lesson identity', () => {
  assert.equal(pack.PackCode, 'UAE-MOE-MATH');
  assert.equal(pack.Lessons.length, 48, 'do not create a duplicate lesson identity');
  const lesson = pack.Lessons.find(x => x.LessonCode.startsWith('PED:UAE-MOE-MATH:L10:GENERAL:01:01:'));
  assert.ok(lesson);
  const ar = lesson.Translations.find(x => x.CultureCode === 'ar');
  assert.ok(ar);
  assert.equal(ar.Title, 'جمع كثيرات الحدود وطرحها');
  for (const key of ['Explanation', 'KeyConceptsAndRules', 'WorkedExamples', 'StepByStepSolutions', 'CommonMistakes', 'QuickSummary']) {
    assert.ok(ar[key].length > 100, key);
    assert.match(ar[key], /[\u0600-\u06ff]/u, key);
  }
  assert.match(ar.Explanation, /الحدود المتشابهة/);
  assert.match(ar.CommonMistakes, /إشارة/);
  assert.match(ar.StepByStepSolutions, /التحقق|تحقق/);
  assert.match(lesson.AdaptationStatus, /NOT A\/B\/C CLOSED/);
  assert.ok(lesson.Translations.some(x => x.CultureCode === 'en'), 'retain existing English content until full Arabic cutover');
});

test('Source-bound textbook content is not declared all-Arabic prematurely', () => {
  assert.equal(pack.AcademicLanguage, 'en', 'only 1 of 48 lessons has been authored in Arabic; final switch is a separate gate');
  assert.ok(pack.Lessons.some(x => !x.Translations.some(t => t.CultureCode === 'ar')));
});

test('Original independent arithmetic oracle catches incorrect polynomial examples', () => {
  const P = x => 3*x*x - 4*x + 7;
  const Q = x => -x*x + 6*x - 2;
  const S = x => 2*x*x + 2*x + 5;
  const D = x => 4*x*x - 10*x + 9;
  const R = x => (2*x*x - 3*x + 1) - (5*x*x + x - 4);
  const RS = x => -3*x*x - 4*x + 5;
  for (const x of [-7,-2,-1,0,1,2,9]) {
    assert.equal(P(x)+Q(x),S(x));
    assert.equal(P(x)-Q(x),D(x));
    assert.equal(R(x),RS(x));
  }
});

test('Only UAE Grade 10 Advanced selection is removed; other levels preserved', () => {
  assert.doesNotMatch(registry, /new\(10,"Grade 10","Secondary","Advanced",true\)/);
  assert.match(registry, /new\(10,"Grade 10","Secondary","General",true\)/);
  assert.match(registry, /new\(11,"Grade 11","Secondary","Advanced",true\)/);
  assert.match(registry, /new\(12,"Grade 12","Secondary","Advanced",true\)/);
  assert.match(registry, /uae\.Levels\.Count != 17/);
});
