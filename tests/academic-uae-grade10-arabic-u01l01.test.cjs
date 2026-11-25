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
  assert.equal(pack.Lessons.length, 148);
  assert.equal(new Set(pack.Lessons.map(x=>x.LessonCode)).size,148,'do not create a duplicate lesson identity');
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
  assert.match(lesson.AdaptationStatus, /READY_FOR_STAGE_D/);
  assert.ok(lesson.Translations.some(x => x.CultureCode === 'en'), 'retain existing English content until full Arabic cutover');
});

test('Arabic cutover requires one Arabic translation for every approved lesson', () => {
  assert.equal(pack.AcademicLanguage, 'ar');
  assert.ok(pack.Lessons.every(x => x.Translations.filter(t => t.CultureCode === 'ar').length === 1));
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
  assert.match(lesson.AdaptationStatus, /READY_FOR_STAGE_D/);
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
    'uae.g9.general.u07.l01.subtract_coefficient',
    'uae.g10.general.u01.l01.full_polynomial_add_subtract_expression'
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
test('Grade10 U01L01 25-element source audit is Stage C ready and Stage D tests remain unrun', () => {
  const ledger=JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/UAE-MOE-MATH/G10/lesson-001-source-elements.json'),'utf8'));
  assert.equal(ledger.sourceElements.length,25);
  assert.equal(ledger.elementDenominatorForThisBook,25);
  assert.equal(ledger.sourceIndexLocked,true);
  assert.equal(ledger.sourceElementDenominatorVerifiedForThisLesson,true);
  assert.equal(ledger.sourceElementCoverageCount,25);
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
  assert.equal(pack.Lessons.length,148,'owner-approved unified catalog, not an independently verified annual textbook count');
});

test('Grade10 General blueprint registers 148 approved titles with a complete 148-entry student content pack',()=>{
 const blueprint=JSON.parse(fs.readFileSync(path.join(root,
   'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json'),'utf8'));
 const manifest=JSON.parse(fs.readFileSync(path.join(root,
   'docs/curriculum/UAE_G10_GENERAL_OFFICIAL_MANIFEST.json'),'utf8'));
 assert.equal(blueprint.Units[0].Title,'كثيرات الحدود والتحليل');
 assert.match(blueprint.SourceEdition,/2025–2026, Volume 1/);
 assert.match(blueprint.PedagogicalSourceType,/OfficialUaeStudentBook/);
 assert.ok(manifest.SourceFiles.includes('01.pdf'));
 assert.equal(manifest.Units[0].ls[0],'جمع كثيرات الحدود وطرحها');
 assert.equal(manifest.LessonCount,148);
 assert.equal(manifest.UnitCount,18);
 assert.equal(blueprint.Lessons.length,148);
 assert.ok(!blueprint.SourceTitle.includes('GCSE'));
});
test('Grade10 U01L01 contract covers exact whole polynomial while teacher graphics remain separate',()=>{
 const con=JSON.parse(fs.readFileSync(path.join(root,
  'artifacts/curriculum-delivery/UAE-MOE-MATH/G10/lesson-001-stage-c-exact-contract.json'),'utf8'));
 assert.deepEqual(con.primarySkills,['algebra.polynomial.add_subtract']);
 assert.ok(con.allowedQuestionFamilies.includes('uae.g10.general.u01.l01.full_polynomial_add_subtract_expression'));
 assert.ok(con.rendererMatrix.some(x=>x.kind==='StructuredEntry'));
 assert.ok(con.teacherReviewRequired.some(x=>/diagram|algebra-tile|graphic/.test(x)));
 assert.match(con.runtimeGate.currentlyApprovedRuntime,/^NO:/);
});

test('Grade10 U01L01 uses an actual Arabic RichV2 lesson including visual calculation and source-correct mathematical checks',()=>{
 const rich=JSON.parse(fs.readFileSync(path.join(root,
  'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u01-l01-ar.rich-lesson-v2.json'),'utf8'));
 assert.equal(rich.SchemaVersion,1);
 assert.equal(rich.PackCode,'UAE-MOE-MATH');
 assert.equal(rich.Lessons.length,1);
 const x=rich.Lessons[0];
 assert.equal(x.LessonCode,'PED:UAE-MOE-MATH:L10:GENERAL:01:01:ADDING-AND-SUBTRACTING-POLYNOMIALS');
 assert.equal(x.CultureCode,'ar');
 assert.equal(x.Title,'جمع كثيرات الحدود وطرحها');
 assert.ok(x.WorkedExamples.length>=5);
 assert.ok(x.Visuals.length>=3);
 assert.ok(x.Visuals.every(v=>v.Items.length>=3));
 assert.ok(x.Visuals.some(v=>v.Kind==='ExpressionStripBars'&&v.Rows.length===3));
 assert.ok(x.Visuals.some(v=>v.Kind==='ShapeDecomposition'&&v.Rows.length===3));
 assert.ok(x.KeyConcepts.some(v=>v.Title==='المعامل الرئيس'));
 assert.ok(x.WorkedExamples.some(v=>v.Answer.includes('٣س³−٤س²+٧س−١٣')));
 assert.ok(x.WorkedExamples.some(v=>v.Answer.includes('٢٠س−س²')));
});
test('U01L01 rental application includes per-day charge, per-distance rate and correct multi-day arithmetic',()=>{
 const rich=JSON.parse(fs.readFileSync(path.join(root,
 'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u01-l01-ar.rich-lesson-v2.json'),'utf8'));
 const lesson=rich.Lessons[0];
 const ex=lesson.WorkedExamples.find(x=>x.Title==='نموذج تأجير متعدد الأيام وجمع كثيرتي تكلفة');
 assert.ok(ex);
 assert.ok(ex.Question.includes('١٨ي+٠٫٢ك'));
 assert.ok(ex.Answer.includes('١٦٢ و٤٢'));
 assert.equal(18*3+0.2*240,102);
 assert.equal(12*3+0.1*240,60);
 assert.equal(102+60,162);
 assert.equal(102-60,42);
 assert.ok(lesson.Visuals.some(x=>x.Title.includes('تكلفة ثابتة لكل يوم')));
});
test('U01L01 RichV2 closes original geometry, sign-distribution, and multivariate teaching elements',()=>{
 const rich=JSON.parse(fs.readFileSync(path.join(root,
 'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u01-l01-ar.rich-lesson-v2.json'),'utf8'));
 const item=rich.Lessons[0];
 assert.ok(item.WorkedExamples.some(e=>e.Answer.includes('٥س+٧')));
 assert.ok(item.WorkedExamples.some(e=>e.Answer.includes('−س²−٤س+٢')));
 assert.ok(item.WorkedExamples.some(e=>e.Answer.includes('٤س²ص+٣سص+٣')));
 assert.equal((12*1+18)-(4*1+6)-(3*1+5),12);
 assert.equal((2-1)-(3+3-2),-3);
 assert.equal((3-2+4)+(1+5-1),10);
});

test('All 48 existing General Volume1 lesson titles occur once in the adopted 148-lesson unified curriculum',()=>{
 const manifest=JSON.parse(fs.readFileSync(path.join(root,'docs/curriculum/UAE_G10_GENERAL_OFFICIAL_MANIFEST.json'),'utf8'));
 const blueprint=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json'),'utf8'));
 assert.equal(manifest.LessonCount,148);assert.equal(manifest.UnitCount,18);
 assert.equal(manifest.CanonicalUnifiedIndexLocked,true);
 const all=manifest.Units.flatMap(u=>u.ls);
 assert.equal(all.length,148);
 assert.equal(new Set(all).size,148);
 assert.equal(blueprint.Lessons.length,148);
 for(const lesson of blueprint.Lessons) {
   assert.ok(all.includes(lesson.Title),'baseline source lesson title missing in canonical 148: '+lesson.Title);
 }
 assert.equal(manifest.FullLessonLevelSemanticAuditComplete,false);
});
test('Grade10 U01L01 is A-C ready with Stage D and source-period caveats retained',()=>{
  const selection=JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/UAE-MOE-MATH/G10/source-selection.json'),'utf8'));
  const checkpoint=JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/UAE-MOE-MATH/G10/lesson-001-stage-a-c-checkpoint.json'),'utf8'));
  assert.equal(selection.pdfs.find(x=>x.name==='01.pdf').classification,'PRIMARY_GENERAL_VOLUME1_TERM1');
  assert.equal(selection.generalAnnualSourceComplete,false);
  assert.equal(selection.finalCanonicalLessonCount,148);
  assert.equal(checkpoint.status,'READY_FOR_STAGE_D');
  assert.equal(checkpoint.firstIncompleteLessonOrdinal,2);
  assert.equal(checkpoint.testsRun,'WRITTEN_NOT_RUN');
  assert.match(pack.SourceCurriculumPeriod,/2025[-–]2026.*Volume\s*1/);
  assert.match(pack.TargetCurriculumPeriod,/2026-2027/);
  assert.equal(pack.AcademicLanguage,'ar');
  assert.ok(pack.Lessons.every(x=>x.Translations.filter(t=>t.CultureCode==='ar').length===1));
});
