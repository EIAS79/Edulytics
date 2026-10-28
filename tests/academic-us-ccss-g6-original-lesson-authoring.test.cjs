'use strict';

// US-CCSS-MATH Grade 6 candidate guards only.
// These tests verify repository JSON structure and source-specific authoring
// invariants. They do NOT certify exact Practice, runtime rendering, .NET,
// authenticated routes, teacher assessment, or production state.

const assert = require('node:assert/strict');
const test = require('node:test');
const fs = require('node:fs');
const path = require('node:path');

const root = path.resolve(__dirname, '..');
const blueprint = JSON.parse(fs.readFileSync(path.join(root,
  'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-grade6-our-2017.lesson-blueprint.json'), 'utf8'));
const pack = JSON.parse(fs.readFileSync(path.join(root,
  'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g6-phase29-v1.lesson-content-pack.json'), 'utf8'));
const byCode = new Map(pack.lessons.map(x => [x.lessonCode, x]));
const lcode = (u, l) => `PED:US-CCSS-MATH:G6:U${String(u).padStart(2, '0')}:L${String(l).padStart(2, '0')}`;
const target = [
  [1,1, 'compare how much area tiling patterns cover'],
  [1,2, 'find area by splitting figures'],
  [1,3, 'find and justify polygon areas'],
  [2,1, 'describe two quantities with precise ratio language'],
  [3,1, 'use a rate per one unit'],
  [4,1, 'estimate how changing a positive divisor'],
  [5,1, 'estimate and calculate with decimals'],
  [6,1, 'draw and interpret tape diagrams'],
  [7,1, 'represent positive and negative quantities'],
  [8,1, 'collect and distinguish categorical and numerical data'],
  [9,1, 'make defensible estimates of real-world quantities']
];

test('Grade 6 canonical identity and nine-unit inventory are exact', () => {
  assert.equal(blueprint.BlueprintCode, 'US-CCSS-MATH:G6:OUR-IM-2017');
  assert.equal(blueprint.NativeLevel, 'Grade 6');
  assert.equal(blueprint.LogicalLevel, 7);
  assert.equal(pack.packCode, 'US-CCSS-MATH');
  assert.equal(pack.versionCode, 'CCSSM-2010');
  assert.equal(blueprint.Units.length, 9);
  assert.equal(blueprint.Lessons.length, 147);
  assert.equal(pack.lessons.length, 147);
  assert.equal(byCode.size, 147);
  assert.deepEqual(
    [...byCode.keys()].sort(),
    blueprint.Lessons.map(x => x.LessonCode).sort()
  );
  const expected = [19,17,17,17,15,19,19,18,6];
  expected.forEach((count, i) => {
    const inUnit = blueprint.Lessons.filter(l => l.UnitNumber === i + 1);
    assert.equal(inUnit.length, count, `U${i+1} count`);
    assert.deepEqual(inUnit.map(l => l.LessonNumber), Array.from({length: count}, (_, n) => n + 1));
  });
});

test('Every English lesson has a mathematical action goal and preserved source provenance', () => {
  assert.match(pack.contentVersion, /^us-ccss-g6-learner-goal-repair-candidate-/);
  for (const canonical of blueprint.Lessons) {
    const lesson = byCode.get(canonical.LessonCode);
    const en = lesson.translations.find(t => t.cultureCode === 'en');
    assert.ok(en, canonical.LessonCode);
    const first = en.explanation.split('\n', 1)[0];
    assert.match(first, /^Learning goal: You will (?:[a-z][^\n]{15,})\.$/, canonical.LessonCode);
    assert.doesNotMatch(first, /You will (?:the |a |an |you |are |of |what |which |\w+ing[.])/i, canonical.LessonCode);
    assert.equal(lesson.sourceUrl, canonical.SourceUrl);
    assert.equal(lesson.sourceRights, 'CC BY 4.0');
    assert.ok(lesson.sourceSha256 && lesson.canonicalBodySha256, canonical.LessonCode);
    assert.equal(lesson.translations.length, 1);
  }
});

test('Eleven original, lesson-targeted examples and strategies replace template teaching', () => {
  for (const [u, l, phrase] of target) {
    const lesson = byCode.get(lcode(u, l));
    assert.ok(lesson);
    const en = lesson.translations[0];
    assert.ok(en.explanation.startsWith('Learning goal: You will ' + phrase), lesson.lessonCode);
    assert.ok(en.workedExamples.includes('Example 1'), lesson.lessonCode);
    assert.ok(en.workedExamples.includes('Example 2'), lesson.lessonCode);
    assert.ok(en.stepByStepSolutions.includes('Step 5:'), lesson.lessonCode);
    assert.doesNotMatch(en.stepByStepSolutions, /Step 1: Identify what the task asks you to find or show\./);
    assert.doesNotMatch(en.commonMistakes, /Common mistake: jumping to an answer before checking/);
    assert.ok(en.quickSummary.length >= 70);
  }
});

test('Area source lessons explicitly teach area, units, and conservation, not only classification', () => {
  for (let l = 1; l <= 3; l++) {
    const en = byCode.get(lcode(1, l)).translations[0];
    assert.match(en.explanation, /area/i);
    assert.match(en.workedExamples, /square units/i);
    assert.match(en.keyConceptsAndRules, /non-overlapping|overlap/i);
    assert.doesNotMatch(en.stepByStepSolutions, /classify, construct, or reason about the figure/);
  }
});

test('Fermi modeling is explicitly marked unsuitable for single-value automatic assessment', () => {
  const en = byCode.get(lcode(9, 1)).translations[0];
  assert.match(en.explanation, /single machine-graded number/i);
  assert.match(en.workedExamples, /Teacher-review prompt:/);
  assert.match(en.commonMistakes, /solely because it differs from one reference number/i);
});

test('Explicit Rich V2 U01 L01-L03 area lessons take precedence over generated shape-properties examples', () => {
  const rich = JSON.parse(fs.readFileSync(path.join(root,
    'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g6-u01-l01-l03-area-20261028.rich-lesson-v2.json'), 'utf8'));
  assert.equal(rich.SchemaVersion, 1);
  assert.equal(rich.PackCode, 'US-CCSS-MATH');
  assert.equal(rich.Lessons.length, 3);
  assert.deepEqual(rich.Lessons.map(l => l.LessonCode), [lcode(1, 1), lcode(1, 2), lcode(1, 3)]);
  rich.Lessons.forEach(lesson => {
    assert.equal(lesson.CultureCode, 'en');
    assert.ok(lesson.ExplanationParagraphs.length >= 2);
    assert.ok(lesson.KeyConcepts.length >= 3);
    assert.ok(lesson.WorkedExamples.length >= 3);
    assert.ok(lesson.CommonMistakes.length >= 2);
    assert.ok(lesson.SummaryPoints.length >= 3);
    assert.ok(lesson.Visuals.length >= 1);
    lesson.WorkedExamples.forEach(ex => {
      assert.ok(ex.Title && ex.Question && ex.Method && ex.Answer);
      assert.ok(ex.Steps.length >= 3);
    });
    assert.match(lesson.ExplanationParagraphs.join(' '), /area/i);
    assert.ok(lesson.Visuals.every(v => v.Description && v.Description.length > 40));
  });
});
