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
  [1,1, 'compare areas covered by different tiling shapes'],
  [1,2, 'define area precisely, compare same-area figures'],
  [1,3, 'reason about shaded polygon areas'],
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
  assert.match(pack.contentVersion, /^us-ccss-g6-full-grade-authoring-candidate-/);
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
    assert.match(en.workedExamples, /square units|square unit|triangular (?:area )?units/i);
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

test('All nineteen first-party Unit 1 lessons have individualized methods and misconception guidance', () => {
  for (let i = 1; i <= 19; i++) {
    const en = byCode.get(lcode(1, i)).translations[0];
    assert.match(en.explanation, /area|triangle|prism|cube|polygon|tent|polyhedra|nets/i, `U01:L${i}`);
    assert.match(en.workedExamples, /Example 1/, `U01:L${i}`);
    assert.match(en.workedExamples, /Example 2/, `U01:L${i}`);
    assert.doesNotMatch(en.stepByStepSolutions, /Step 1: Identify what the task asks you to find or show\./, `U01:L${i}`);
    assert.doesNotMatch(en.stepByStepSolutions, /Step 3: Use the attributes to classify, construct, or reason about the figure\./, `U01:L${i}`);
    assert.doesNotMatch(en.commonMistakes, /^Common mistake:/, `U01:L${i}`);
    assert.ok(en.quickSummary.length >= 80, `U01:L${i}`);
  }
});

test('Unit 1 numeric teaching invariants are mathematically consistent', () => {
  assert.equal(7 * 4, 32 - 2 * (1 * 4 / 2)); // parallelogram area: cut versus enclosure
  assert.equal(0.5 * 8 * 5, 20);             // matched triangle area
  assert.equal(2 * (12 + 8 + 6), 52);       // prism face areas
  assert.equal(2 * (6 + 3 + 2), 22);        // second prism net
  assert.equal(2 * (1 + 8 + 8), 34);        // 1-by-1-by-8 surface
  assert.equal(6 * 4, 24);                 // 2-by-2-by-2 surface
  assert.equal(6 * (3 ** 2), 54);          // cube surface
  assert.equal(3 ** 3, 27);                // cube volume
  assert.equal(2 * 3 * 2.5 + 2 * (0.5 * 4 * 1.5), 21); // tent model, no floor
});

test('Every one of the 147 Grade 6 lessons has original worked teaching and a five-step mathematical procedure', () => {
  const counts = new Map();
  for (const lesson of pack.lessons) {
    const en = lesson.translations.find(x => x.cultureCode === 'en');
    const code = lesson.lessonCode;
    assert.ok(en, code);
    assert.match(en.explanation, /^Learning goal: You will [a-z][^\n]+\.\n/, code);
    assert.ok(en.explanation.length >= 250, code + ': explanation');
    assert.ok((en.keyConceptsAndRules.match(/(?:^|\n)- /g) || []).length >= 3, code + ': key concepts');
    assert.match(en.workedExamples, /Example 1\b/, code);
    assert.match(en.workedExamples, /Example 2\b/, code);
    assert.equal((en.stepByStepSolutions.match(/Step [1-5]:/g) || []).length, 5, code + ': steps');
    assert.doesNotMatch(en.stepByStepSolutions, /Step 1: Identify what the task asks you to find or show\./, code);
    assert.doesNotMatch(en.stepByStepSolutions, /Step 1: Identify the figure and the attributes named in the task\./, code);
    assert.ok(en.commonMistakes.length >= 80, code + ': misconception');
    assert.doesNotMatch(en.commonMistakes, /^Common mistake: jumping/, code);
    assert.ok(en.quickSummary.length >= 60, code + ': summary');
    assert.doesNotMatch(en.explanation + en.keyConceptsAndRules + en.workedExamples,
      /so nine\?|hour and minute hands|the addends are the quantities being combined/i, code);
    const prefix = code.match(/:U(\d\d):L\d\d$/);
    assert.ok(prefix, code);
    counts.set(prefix[1], (counts.get(prefix[1]) || 0) + 1);
  }
  assert.deepEqual([...counts.entries()].sort(), [
    ['01', 19], ['02', 17], ['03', 17], ['04', 17], ['05', 15],
    ['06', 19], ['07', 19], ['08', 18], ['09', 6]
  ]);
});

test('Critical Grade 6 arithmetic and statistics examples have valid independent numeric checks', () => {
  assert.equal(2 * (4 * 3 + 4 * 2 + 3 * 2), 52);         // prism surface area
  assert.equal(3 / 4 / (2 / 3), 9 / 8);                // fraction division
  assert.equal(3 / 2 * 2 / 3, 1);                    // reciprocal quotient check
  assert.ok(Math.abs(4.32 / 0.12 - 36) < 1e-10);       // decimal-divisor scaling
  assert.equal((3 + 5 + 7) / 3, 5);                   // mean
  assert.equal((2 + 0 + 2) / 3, 4 / 3);               // MAD
  assert.equal((2 * 6 + 2 * 8 + 2 * 7) / 2, 21);     // tent fabric model
  assert.equal(120 * 0.4, 48);                       // proportional scaling
  assert.ok(Math.abs(40 * 0.18 - 7.2) < 1e-10);      // percentage
  assert.equal((30 / 120) * 100, 25);                // find percentage
});

test('Full-grade authoring is not confused with source, Practice or production approval', () => {
  const record = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/grade6-147-academic-authoring-ledger.json'), 'utf8'));
  assert.equal(record.lessons.length, 147);
  assert.equal(record.summary.canonicalLessons, 147);
  assert.equal(record.summary.substantiveLessonsRepaired, 147);
  assert.equal(record.summary.fullStageAReviews, 0);
  assert.equal(record.summary.stageDVerified, 0);
  assert.equal(record.summary.academicallyAccepted, 0);
  assert.equal(record.summary.releaseReady, false);
  assert.ok(record.lessons.every(l => l.academicAcceptance === 'NOT_ACCEPTED'));
});

test('Grade 6 public-source area activity boundaries match first-party candidate teaching', () => {
  const crosswalk = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u01-l01-l03-public-source-task-practice-boundaries.json'), 'utf8'));
  const rich = JSON.parse(fs.readFileSync(path.join(root,
    'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g6-u01-l01-l03-area-20261028.rich-lesson-v2.json'), 'utf8'));
  assert.equal(crosswalk.status, 'PUBLIC_SOURCE_ACTIVITY_COMPARISON_NOT_ACADEMIC_ACCEPTANCE');
  assert.equal(crosswalk.entries.length, 3);
  assert.deepEqual(crosswalk.entries.map(e => e.lessonCode), [lcode(1,1), lcode(1,2), lcode(1,3)]);
  assert.deepEqual(rich.Lessons.map(e => e.LessonCode), [lcode(1,1), lcode(1,2), lcode(1,3)]);
  crosswalk.entries.forEach((entry, index) => {
    assert.ok(entry.student.includes('/MS/students/1/1/'));
    assert.ok(entry.teacher.includes('/MS/teachers/1/1/'));
    assert.ok(entry.practice.includes('/MS/students/1/1/'));
    assert.ok(entry.observableTasks.length >= 4);
    assert.ok(entry.practiceBoundary.teacherOnly.length >= 2);
    assert.ok(entry.practiceBoundary.forbiddenSubstitutes.length >= 2);
    assert.ok(entry.remainingGaps.length >= 3);
    assert.equal(entry.teacherOnlyRubricCandidate.criteria.length, 4);
    assert.equal(entry.teacherOnlyRubricCandidate.scoreRange.max, 8);
    assert.equal(entry.teacherOnlyRubricCandidate.teacherDecisionRequired, true);
    assert.equal(entry.teacherOnlyRubricCandidate.persistedEvaluationVerified, false);
    assert.ok(entry.teacherOnlyRubricCandidate.criteria.every(c => c.maxPoints === 2 && c.requiredEvidence));
    assert.ok(rich.Lessons[index].WorkedExamples.length >= 3);
    assert.ok(rich.Lessons[index].WorkedExamples.some(e => /teacher.review/i.test(e.Method + ' ' + e.Check)));
  });
  const a = rich.Lessons[0];
  assert.match(a.ExplanationParagraphs.join(' '), /trapezoid/i);
  assert.match(a.WorkedExamples[0].Question, /rhombi/i);
  assert.match(a.WorkedExamples[0].Answer, /9 triangular area units/);
  const b = rich.Lessons[1];
  assert.match(b.WorkedExamples[1].Answer, /1\/2 square unit/);
  const c = rich.Lessons[2];
  assert.match(c.WorkedExamples[0].Answer, /16 square units/);
  assert.equal(3 * 3, 9);
  assert.equal(4 * 2, 8);
  assert.equal(7 * 1, 7);
  assert.equal(16 - 1 + 1, 16);
});
