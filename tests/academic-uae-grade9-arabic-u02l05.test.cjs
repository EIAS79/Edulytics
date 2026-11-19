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
  for(const id of ['cooldownResponse','cooldownQ1','cooldownQ2','cooldownQ3','contextQ4','contextQ5','cooldownPointPicker','cooldownStudentNumberLine','selectedPoints','cooldownSubmission','cooldownMessage','cooldownKey']) {
    assert.ok(html.includes('id="'+id+'"'), id);
  }
  assert.match(html, /TEACHER_REVIEW_REQUIRED/);
  assert.match(html, /graded:false/);
  assert.match(html, /numberLineSelectedPoints/);
  assert.match(html, /function renderStudentLine\(/);
  assert.match(html, /renderStudentLine\(\);/);
  assert.match(html, /viewBox="0 0 760 145"/);
  assert.match(html, /selectedCoolDownPoints.size!==2/);
  assert.match(html, /contextQ4/);
  assert.match(html, /contextQ5/);
  assert.match(html, /مراجعة المعلم/);
  assert.match(html, /لم تُرسل البيانات إلى Edulytics/);
  assert.match(html, /event\.preventDefault\(\)/);
  assert.match(html, /id="cooldownKey" hidden/);
});

test('Proposed 13x82 crosswalk retains historical quadratic unit 7 and all four recent geometry source refs', () => {
  const proposed = read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/proposed-13x82-canonical-crosswalk.json');
  assert.equal(proposed.status, 'STRUCTURAL_13x82_PROPOSAL_NOT_SEMANTIC_LOCK');
  assert.equal(proposed.canClaimCanonicalSourceLock, false);
  assert.equal(proposed.records.length, 82);
  assert.equal(new Set(proposed.records.map(x => x.unit)).size, 13);
  assert.equal(proposed.records.filter(x => x.unit === 7).length, 9);
  assert.ok(proposed.records.some(x => x.primarySource === '02-1.pdf:7-9'));
  const originalRefs = new Set([
    ...proposed.records.map(x => x.primarySource),
    ...proposed.records.flatMap(x => x.supplementarySources)
  ]);
  assert.equal(originalRefs.size, 86);
  for(const num of [1,2,3,4])assert.ok(originalRefs.has('01.pdf:7-' + num));
  assert.equal(proposed.reviewPriority.filter(x => x.status.startsWith('CONFIRMED_')).length, 1);
});

test('Every source exercise has guarded numeric or teacher-reviewed routing without false graph-equation scoring', () => {
  const a = read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u02l05-atomic-coverage.json');
  const bank = read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u02l05-teacher-reviewed-extension-bank.json');
  assert.equal(a.sourceExerciseInventory.items.length, 64);
  assert.equal(new Set(a.sourceExerciseInventory.items.map(x => x.itemId)).size, 64);
  assert.equal(a.sourceExerciseInventory.items.filter(x => !x.digitalFamily).length, 33);
  assert.equal(a.sourceExerciseInventory.items.filter(x => x.taskCategory === 'solve_and_graph' && x.teacherVisualReview).length, 6);
  assert.equal(bank.sourceExerciseCount, 33);
  const linked = new Set(bank.sourceExerciseIdsClassified);
  for (const item of a.sourceExerciseInventory.items) {
    if (!item.digitalFamily) assert.ok(linked.has(item.itemId), item.itemId);
    if (['equation_from_number_line','number_line_to_equation'].includes(item.taskCategory)) {
      assert.equal(item.digitalFamily, null);
      assert.ok(item.numericAssistanceFamily.endsWith('.midpoint'));
    }
  }
  for (const num of [1,2,3]) {
    const item = a.sourceExerciseInventory.items.find(x => x.itemId.endsWith('EX'+String(num).padStart(2, '0')));
    assert.ok(item.digitalFamily.endsWith('.composite_evaluation'));
  }
  for (const num of [6,9]) {
    const item = a.sourceExerciseInventory.items.find(x => x.itemId.endsWith('EX'+String(num).padStart(2, '0')));
    assert.ok(item.digitalFamily.endsWith('.no_solution'));
    assert.equal(item.teacherVisualReview, true);
  }
  for (const num of [24,27,28]) {
    const item = a.sourceExerciseInventory.items.find(x => x.itemId.endsWith('EX'+num));
    assert.ok(item.digitalFamily.endsWith('.no_solution'));
  }
  for (const num of [38,39]) {
    const item = a.sourceExerciseInventory.items.find(x => x.itemId.endsWith('EX'+num));
    assert.ok(item.digitalFamily.endsWith('.two_solutions'));
  }
  assert.ok(a.sourceExerciseInventory.items.find(x => x.itemId.endsWith('EX41')).digitalFamily.endsWith('.outer_coefficient_no_solution'));
});


test('Source exercise routing ledgers remain synchronized for all 64 exercises', () => {
  const source = read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u02l05-atomic-coverage.json');
  const ledger = read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u02l05-source-exercise-treatment-ledger.json');
  const items = source.sourceExerciseInventory.items;
  assert.equal(items.length, 64);
  assert.equal(ledger.entries.length, items.length);
  const index = new Map(items.map(item => [item.itemId, item]));
  assert.equal(index.size, items.length);
  for (const entry of ledger.entries) {
    const item = index.get(entry.sourceElementId);
    assert.ok(item, entry.sourceElementId + ' is missing in source inventory');
    assert.equal(entry.AllowedDigitalFamily ?? null, item.digitalFamily ?? null,
      entry.sourceElementId + ': mismatched digital family');
    if (item.teacherReviewRequired === true || item.teacherVisualReview === true) {
      assert.match(entry.AssessmentRoute, /TEACHER/,
        entry.sourceElementId + ': teacher-reviewed work must not be treated as fully autograded');
    }
    assert.notEqual(entry.ExactEquivalent, true,
      entry.sourceElementId + ': source equivalence requires independent audit');
  }
  const visual = source.objectives.find(o => o.objectiveId === 'O3');
  assert.ok(visual);
  assert.equal(visual.humanReviewRequired, true,
    'Number-line drawing and explanation must retain teacher review');
  assert.equal(visual.testsRun, 'WRITTEN_NOT_RUN');
});


test('All 64 source mathematical outcomes are recorded without claiming Stage D validation', () => {
  const evidence = read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u02l05-atomic-coverage.json');
  assert.equal(evidence.sourceExerciseInventory.items.length, 64);
  assert.equal(evidence.summary.sourceMathematicsSpotAudit.verifiedCount, 64);
  assert.equal(evidence.summary.sourceMathematicsSpotAudit.pendingCount, 0);
  for (const item of evidence.sourceExerciseInventory.items) {
    assert.ok(item.sourceMathematicsSpotAudit, item.itemId);
    assert.ok(item.sourceMathematicsSpotAudit.expectedMathAnswer, item.itemId);
    assert.equal(item.sourceMathematicsSpotAudit.verificationScope.includes('RUNTIME'), true);
  }
  assert.equal(evidence.summary.sourceMathematicsSpotAudit.testsRun, 'WRITTEN_NOT_RUN');
});

test('Original teacher-reviewed activities cover each source timing case and conditional reasoning', () => {
  const bank = read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u02l05-teacher-reviewed-extension-bank.json');
  const html = fs.readFileSync(path.join(root,bank.originalAuthoredBank),'utf8');
  const timed = bank.groups.find(x=>x.taskId==='extensionQ3');
  assert.deepEqual(timed.ids,[32,43]);
  assert.match(html,/٥٠٫٥ ثانية/);
  assert.match(html,/٤ دقائق مع فرق ٥ ثوانٍ/);
  const reasoning = bank.groups.find(x=>x.taskId==='extensionQ7');
  assert.ok(reasoning.ids.includes(59));
  assert.match(html,/المتباينة \|س\|\+ج<٠ بلا حلول/);
  assert.match(html,/أحيانًا أو أبدًا/);
  assert.equal(bank.assessedAutomatically,false);
});
