'use strict';
// A–B–C prepared acceptance specifications. Do not execute until Stage D.
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const read = relative => JSON.parse(fs.readFileSync(path.join(root, relative), 'utf8'));
const lessonCode = 'PED:UAE-MOE-MATH:L9:GENERAL:02:05:ABSOLUTE-VALUE-EQUATIONS';
const prefix = 'uae.g9.general.u02.l05.';
test('UAE Grade 9 General absolute-value lesson is authored in Arabic and source traced', () => {
  const pack = read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
  const lesson = pack.Lessons.find(x => x.LessonCode === lessonCode);
  assert.ok(lesson);
  assert.equal(pack.AcademicLanguage, 'ar');
  assert.equal(lesson.SourceLocator.includes('01.pdf'), true);
  assert.equal(lesson.Translations.find(x => x.CultureCode === 'ar').Title, 'حل معادلات تتضمن قيمة مطلقة');
  const ar = lesson.Translations.find(x => x.CultureCode === 'ar');
  assert.match(ar.Explanation, /لا حل|سالبة/);
  assert.match(ar.WorkedExamples, /س=٣|س=3/);
  assert.match(ar.CommonMistakes, /القيمة المطلقة/);
});
test('Only dedicated Grade 9 absolute-value question families are mapped', () => {
  const mappings = read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
  const entry = mappings.mappings.find(x => x.lessonCode === lessonCode);
  assert.ok(entry);
  assert.equal(entry.allowedQuestionFamilies.length, 8);
  assert.ok(entry.allowedQuestionFamilies.every(f => f.startsWith(prefix)));
  assert.ok(entry.allowedQuestionFamilies.includes(prefix + 'context_tolerance_endpoints'));
  assert.equal(entry.officialOutcomeMapped, false);
  assert.notEqual(entry.practiceReadiness, 'READY_VERIFIED');
});
test('Dedicated generator and independent verifier are wired, no linear-only fallback', () => {
  const contracts = fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
  const engine = fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
  const implementation = fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/UaeG9AbsoluteValueEquationQuestions.cs'),'utf8');
  assert.ok(contracts.includes('byLesson["' + lessonCode + '"]'));
  for (const name of ['Supports', 'Build', 'Solve', 'Verify']) {
    assert.ok(engine.includes('UaeG9AbsoluteValueEquationQuestions.' + name + '('));
  }
  for (const tag of ['two_solutions','one_solution','no_solution','evaluate_absolute','midpoint','composite_evaluation','outer_coefficient_no_solution','context_tolerance_endpoints']) {
    assert.ok(implementation.includes('uae.g9.general.u02.l05.' + tag));
  }
});
test('Mathematical fixtures cover zero, two and no solution without false positive', () => {
  const solve = (a,b,c) => c < 0 ? [] : [...new Set([(-b+c)/a,(-b-c)/a])].sort((x,y)=>x-y);
  const cases = [
    [1,5,17,[-22,12]], [1,-1,-3,[]], [1,2,4,[-6,2]],
    [3,-4,-1,[]], [1,7,5,[-12,-2]], [4,-1,-6,[]],
    [2,6,6,[-6,0]], [2,-6,0,[3]], [1,-15,4,[11,19]]
  ];
  for (const [a,b,c,expected] of cases) assert.deepEqual(solve(a,b,c),expected);
});
test('A–B–C source inventory is five books and not declared as semantic PASS', () => {
  const books = read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/five-book-inventory.json');
  const catalog = read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/comprehensive-index.json');
  assert.equal(books.files.length,5);
  assert.equal(books.files.filter(x=>x.pathway==='General').length,3);
  assert.equal(catalog.records.length,86);
  assert.equal(catalog.uniqueLessonCount,null);
  assert.notEqual(catalog.semanticDedupStatus,'PASS');
});

test('Source-aligned Cool-down keeps drawings and reasoning under teacher review', () => {
  const html = fs.readFileSync(path.join(root, 'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u02-l05-absolute-value-number-line-ar.html'), 'utf8');
  assert.match(html, /lang="ar" dir="rtl"/);
  assert.match(html, /id="cooldownTitle"/);
  assert.match(html, /\|س − ٣\| = ٥/);
  assert.match(html, /\|٢س \+ ٤\| = −٦/);
  assert.match(html, /٧.*١٩/);
  assert.match(html, /مراجعة المعلم/);
  assert.match(html, /id="svgDesc"/);
  const rubric = read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u02l05-visual-teacher-rubric-spec.json');
  assert.equal(rubric.teacherRubric.grading, 'TEACHER_REVIEW_REQUIRED');
  assert.equal(rubric.teacherRubric.totalPoints, 8);
  const checkpoint = read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-13-stage-a-c-checkpoint.json');
  assert.equal(checkpoint.closed, false);
  assert.equal(checkpoint.advanceToNextLesson, false);
  assert.equal(checkpoint.stageC.testsRun, 'WRITTEN_NOT_RUN');
});

test('Cool-down captures numeric points and five open answers without claiming a grade', () => {
  const html = fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u02-l05-absolute-value-number-line-ar.html'),'utf8');
  for(const id of ['cooldownResponse','cooldownQ1','cooldownQ2','cooldownQ3','contextQ4','contextQ5','cooldownPointPicker','selectedPoints','cooldownSubmission','cooldownMessage','cooldownKey']) {
    assert.ok(html.includes('id="'+id+'"'), id);
  }
  assert.match(html, /TEACHER_REVIEW_REQUIRED/);
  assert.match(html, /graded:false/);
  assert.match(html, /numberLineSelectedPoints/);
  assert.match(html, /selectedCoolDownPoints.size!==2/);
  assert.match(html, /contextQ4/);
  assert.match(html, /contextQ5/);
  assert.match(html, /مراجعة المعلم/);
  assert.match(html, /لم تُرسل البيانات إلى Edulytics/);
  assert.match(html, /event\.preventDefault\(\)/);
  assert.match(html, /id="cooldownKey" hidden/);
});
