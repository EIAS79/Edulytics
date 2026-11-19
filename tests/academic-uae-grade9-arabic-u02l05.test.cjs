'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const load = rel => JSON.parse(fs.readFileSync(path.join(root, rel), 'utf8'));
const code = 'PED:UAE-MOE-MATH:L9:GENERAL:02:05:ABSOLUTE-VALUE-EQUATIONS';
const lessonPack = 'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json';
const indexPath = 'docs/curriculum/UAE_G9_GENERAL_2025_STUDENT_INDEX.json';
const evidencePath = 'artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u02l05-a-c-source-review-in-progress.json';

test('G9 General U02L05 Arabic content is source-scoped and remains unaudited', () => {
  const pack = load(lessonPack);
  const lesson = pack.Lessons.find(x => x.LessonCode === code);
  assert.ok(lesson, 'Grade 9 absolute-value-equations lesson must exist');
  const translation = lesson.Translations.find(x => x.CultureCode === 'ar');
  assert.equal(translation.Title, 'حل معادلات تتضمن قيمة مطلقة');
  assert.equal(pack.AcademicLanguage, 'ar');
  assert.equal(pack.Status, 'Draft');
  for (const field of ['Explanation', 'KeyConceptsAndRules', 'WorkedExamples', 'StepByStepSolutions', 'CommonMistakes', 'QuickSummary']) {
    assert.ok(translation[field]?.length > 70, field);
  }
  assert.match(translation.Explanation, /سالبة/);
  assert.match(translation.WorkedExamples, /لا حل/);
  assert.match(translation.WorkedExamples, /خط الأعداد|منتصفهما/);
  assert.match(lesson.AdaptationStatus, /NOT COMPLETE/);
});

test('G9 General annual scope is 86 target lessons, not 43 falsely certified', () => {
  const index = load(indexPath);
  assert.equal(index.annualExecutionScope.targetUnitCount, 14);
  assert.equal(index.annualExecutionScope.targetLessonCount, 86);
  assert.equal(index.annualExecutionScope.verifiedLessonCount, 43);
  assert.equal(index.sourceLock.verifiedSchoolYearLessonCount, null);
  assert.equal(index.abcClosedLessonCount, 12);
  assert.equal(index.nextUnclosedLesson, '2-5');
  const unit2 = index.units.find(x => x.unit === 2);
  assert.equal(unit2.lessons.find(x => x.code === '2-5').abcStatus, 'CONTENT_PREPARED_PRACTICE_ENGINE_GAP');
});

test('Absolute value equation independently checks source exercises and edge cases', () => {
  const evidence = load(evidencePath);
  assert.deepEqual(evidence.source.lessonPages, [103,104,105,106,107,108]);
  assert.equal(evidence.execution.practiceMappingModified, true);
  assert.equal(evidence.execution.testsRun, 'NOT_RUN');
  const cases = [
    [1, 5, 17, [-22,12]], [1,-1,-3,[]], [1,2,4,[-6,2]],
    [3,-4,-1,[]], [1,7,5,[-12,-2]], [4,-1,-6,[]],
    [2,6,6,[-6,0]], [2,-6,0,[3]], [1,-15,4,[11,19]],
  ];
  const solve = (a,b,c) => c < 0 ? [] : [...new Set([(-c-b)/a, (c-b)/a])].sort((x,y)=>x-y);
  assert.equal(evidence.sourceVerifiedExercises.length, cases.length);
  for (const [index, [a,b,c,answer]] of cases.entries()) {
    assert.deepEqual(solve(a,b,c), answer, `case ${index + 1}`);
    assert.deepEqual(evidence.sourceVerifiedExercises[index].answers, answer);
  }
});

test('Grade 9 absolute-value equations are routed to dedicated exact solver families', () => {
  const mapping = load('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
  const entry = mapping.mappings.find(x => x.lessonCode === code);
  assert.ok(entry, 'U02L05 mapping exists');
  assert.equal(entry.allowedQuestionFamilies.length, 5);
  assert.ok(entry.allowedQuestionFamilies.every(x => x.startsWith('uae.g9.general.u02.l05.')));
  const contracts = fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
  const engine = fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
  assert.ok(contracts.includes('byLesson["' + code + '"]'));
  assert.ok(engine.includes('UaeG9AbsoluteValueEquationQuestions.Verify('));
  assert.ok(engine.includes('UaeG9AbsoluteValueEquationQuestions.Build('));
  assert.ok(engine.includes('UaeG9AbsoluteValueEquationQuestions.Solve('));
  assert.ok(engine.includes('UaeG9AbsoluteValueEquationQuestions.Supports('));
});
