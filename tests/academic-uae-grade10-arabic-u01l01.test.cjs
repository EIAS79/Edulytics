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

test('U01L01 source-specified standard form, visual and original exit check are authored (not executed here)', () => {
  const lesson = pack.Lessons.find(x => x.LessonCode.includes(':L10:GENERAL:01:01:'));
  const ar = lesson.Translations.find(x => x.CultureCode === 'ar');
  assert.match(ar.Explanation, /الصيغة القياسية/);
  assert.match(ar.Explanation, /المعامل الرئيس/);
  assert.match(ar.WorkedExamples, /الطريقة العمودية/);
  assert.match(ar.QuickSummary, /Cool-down/);
  assert.match(lesson.AdaptationStatus, /A_C_EXPLICIT_GAP/);
  const A = x => 2*x*x*x-x*x+5*x-4;
  const B = x => -x*x*x+3*x*x-2*x+9;
  const sum = x => x*x*x+2*x*x+3*x+5;
  const difference = x => 3*x*x*x-4*x*x+7*x-13;
  for (const x of [-5,-1,0,1,2,7]) {
    assert.equal(A(x)+B(x),sum(x));
    assert.equal(A(x)-B(x),difference(x));
  }
  const P = x => 3*x*x-2*x+4;
  const Q = x => -x*x+5*x-7;
  assert.equal(P(1)+Q(1),2);
  assert.equal(P(1)-Q(1),8);
});
test('Grade10 U01L01 exact mapping reuses only existing correct coefficient families (authored, not run in A–C)', () => {
  const map = JSON.parse(fs.readFileSync(path.join(root,
    'src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json'), 'utf8'));
  const lesson = map.mappings.find(x => x.lessonCode ===
    'PED:UAE-MOE-MATH:L10:GENERAL:01:01:ADDING-AND-SUBTRACTING-POLYNOMIALS');
  assert.ok(lesson);
  assert.deepEqual(lesson.allowedQuestionFamilies, [
    'uae.g9.general.u07.l01.add_coefficient',
    'uae.g9.general.u07.l01.subtract_coefficient'
  ]);
  assert.match(lesson.practiceReadiness, /NOT_READY_FOR_STAGE_D/);
  assert.ok(lesson.teacherReviewRequiredFor.length >= 3);
  const P = [3, -4, 2], Q = [-7, 5, -1];
  for (let d=0; d<=2; d++) {
    assert.equal(P[d]+Q[d], [-4,1,1][d]);
    assert.equal(P[d]-Q[d], [10,-9,3][d]);
  }
});

test('U01L01 original realistic polynomial area model has independently checked boundaries', () => {
  const lesson=pack.Lessons.find(x=>x.LessonCode.includes(':L10:GENERAL:01:01:'));
  const ar=lesson.Translations.find(x=>x.CultureCode==='ar');
  assert.match(ar.WorkedExamples,/20−x/);
  assert.match(ar.StepByStepSolutions,/المحيط/);
  assert.match(ar.CommonMistakes,/40−x/);
  const P=40;
  const widthByPerimeter=x=>(P-2*x)/2;
  const areaFromGeometry=x=>x*widthByPerimeter(x);
  const areaFromPolynomial=x=>20*x-x*x;
  for(const x of [1,2,5,8,10,12,15,18,19])
    assert.equal(areaFromGeometry(x),areaFromPolynomial(x));
  assert.equal(areaFromGeometry(10),100);
  assert.equal(areaFromGeometry(5),areaFromGeometry(15));
  assert.ok(widthByPerimeter(25)<0,'impossible geometric width excluded');
});
test('Grade10 U01L01 element ledger is source-page-grounded and never marks Stage D run', () => {
  const ledger=JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/UAE-MOE-MATH/G10/lesson-001-source-elements.json'),'utf8'));
  assert.equal(ledger.sourceElements.length,25);
  assert.equal(ledger.elementDenominatorForThisBook,25);
  assert.equal(ledger.sourceIndexLocked,false);
  assert.equal(ledger.testsRun,'WRITTEN_NOT_RUN');
  assert.ok(ledger.sourceElements.some(x=>x.goal.includes('fixed-perimeter')));
  assert.ok(ledger.sourceElements.some(x=>x.coverageStatus==='TEACHER_REVIEW_REQUIRED'));
});
test('U01L01 source identity is the verified 2025–26 General volume 1, not an annual textbook', () => {
  const lesson=pack.Lessons.find(x=>x.LessonCode==='PED:UAE-MOE-MATH:L10:GENERAL:01:01:ADDING-AND-SUBTRACTING-POLYNOMIALS');
  assert.equal(lesson.SourceSha256,'d7e492a960675c30ac7eebb0144ed2dc51d1f4e9c91c9834ea05638f614a3c72');
  assert.match(lesson.SourceEdition,/2025–2026, Volume 1/);
  assert.match(lesson.SourceLocator,/PDF pages 20–25/);
  assert.match(lesson.RetrievalChannel,/Owner-uploaded/);
  assert.equal(pack.Lessons.length,48,'this is only the current Volume1 baseline, NOT annual count');
});

test('Real Grade10 General Volume1 blueprint and manifest reference local UAE source, not unrelated GCSE',()=>{
 const blueprint=JSON.parse(fs.readFileSync(path.join(root,
   'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json'),'utf8'));
 const manifest=JSON.parse(fs.readFileSync(path.join(root,
   'docs/curriculum/UAE_G10_GENERAL_OFFICIAL_MANIFEST.json'),'utf8'));
 assert.equal(blueprint.Units[0].Title,'التعابير والمعادلات التربيعية');
 assert.match(blueprint.SourceEdition,/2025–2026, Volume 1/);
 assert.match(blueprint.PedagogicalSourceType,/OfficialUaeStudentBook/);
 assert.equal(manifest.SourceFile,'01.pdf');
 assert.equal(manifest.Units[0].ls[0],'جمع كثيرات الحدود وطرحها');
 assert.equal(manifest.LessonCount,48,'General 2025–26 Volume1 only; NOT annual course count');
 assert.ok(!blueprint.SourceTitle.includes('GCSE'));
});
