'use strict';
// Written under UAE Grade 10 A–B–C, not executed until Stage D.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:01:03:MULTIPLYING-POLYNOMIALS';
const blueprint=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const content=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u01-l03-ar.rich-lesson-v2.json');
const ledger=read(base+'lesson-003-source-elements.json');
const contract=read(base+'lesson-003-stage-c-exact-contract.json');
const mappings=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const family=read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');

test('Grade10 148-title Blueprint reuses original U01L03 and correctly records primary source pages',()=>{
 assert.equal(blueprint.Lessons.length,148);
 const b=blueprint.Lessons.find(x=>x.LessonCode===code);
 assert.ok(b);assert.equal(b.UnitNumber,1);assert.equal(b.LessonNumber,3);
 assert.equal(b.Title,'ضرب كثيرات الحدود');assert.equal(b.UnitTitle,'كثيرات الحدود والتحليل');
 const c=content.Lessons.find(x=>x.LessonCode===code);
 assert.ok(c);
 assert.equal(c.SourceSha256,'d7e492a960675c30ac7eebb0144ed2dc51d1f4e9c91c9834ea05638f614a3c72');
 assert.match(c.SourceLocator,/PDF pages 33–40.*printed pages 20–27/);
 assert.match(c.AdaptationStatus,/READY_FOR_STAGE_D/);
 const ar=c.Translations.find(x=>x.CultureCode==='ar');
 assert.ok(ar);
 for(const key of ['Explanation','KeyConceptsAndRules','WorkedExamples','StepByStepSolutions','CommonMistakes','QuickSummary'])assert.ok(ar[key].length>160,key);
});

test('Original Arabic RichV2 retains general polynomial and area grids with independently valid answers',()=>{
 assert.equal(rich.Lessons.length,1);
 const l=rich.Lessons[0];
 assert.equal(l.LessonCode,code);
 assert.equal(l.WorkedExamples.length,11);
 assert.equal(l.Visuals.length,5);
 assert.ok(l.Visuals.some(x=>x.Kind==='ExpressionStripBars'));
 assert.ok(l.Visuals.some(x=>x.Kind==='ShapeDecomposition'));
 for(const x of [-3,-1,0,1,2,5]){
  assert.equal((x+2)*(x+5),x*x+7*x+10);
  assert.equal((x*x+2*x-1)*(x-3),x*x*x-x*x-7*x+3);
  assert.equal((2*x*x-x+3)*(x*x+2),2*x**4-x**3+7*x*x-2*x+6);
 }
 assert.ok(l.WorkedExamples.some(x=>x.Answer.includes('٢س⁴−س³+٧س²−٢س+٦')));
});

test('Source page scopes and 24 curriculum element categories have accountable mappings',()=>{
 assert.deepEqual(ledger.sourcePdfPages,[33,34,35,36,37,38,39,40]);
 assert.equal(ledger.sourceElements.length,24);
 assert.equal(ledger.sourceElementDenominatorVerifiedForThisLesson,true);
 assert.equal(new Set(ledger.sourceElements.map(e=>e.elementId)).size,24);
 assert.ok(ledger.sourceElements.every(e=>e.sourcePrintedPage===e.sourcePhysicalPage-13&&e.appLocation));
 assert.equal(ledger.testsRun,'WRITTEN_NOT_RUN');
});

test('Exact family routes fit polynomial×polynomial and remain disabled until Stage D',()=>{
 const m=mappings.mappings.find(x=>x.lessonCode===code);
 assert.ok(m);
 assert.deepEqual(m.primarySkills,['algebra.polynomial.multiply_distribute']);
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.ok(m.forbiddenQuestionFamilies.includes('uae.g10.general.u01.l02.full_monomial_polynomial_product'));
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(contract.runtimeGate.productionRouting,false);
 for(const id of contract.allowedQuestionFamilies){
  const f=family.families.find(x=>x.id===id);
  assert.ok(f,id);
  assert.equal(f.skillId,m.primarySkills[0]);
  assert.equal(f.productionRouting,false);
 }
 const source=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10PolynomialProduct.cs'),'utf8');
 assert.match(source,/UaeG10PolynomialProductResponse/);
 assert.match(source,/TryParse/);
});
