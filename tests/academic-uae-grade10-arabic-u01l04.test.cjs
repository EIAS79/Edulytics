'use strict';
// Grade10 general U01L04 A–B–C source contracts. Written now; Stage D executes.
const test=require('node:test'),assert=require('node:assert/strict');
const path=require('node:path'),fs=require('node:fs');
const root=path.resolve(__dirname,'..');
const get=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:01:04:SPECIAL-PRODUCTS';
const blueprint=get('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const content=get('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=get('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u01-l04-ar.rich-lesson-v2.json');
const source=get(base+'lesson-004-source-elements.json');
const contract=get(base+'lesson-004-stage-c-exact-contract.json');
const mappings=get('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const registry=get('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');

test('Owner-approved 148/18 index retains exact lesson 4 identity and book evidence',()=>{
 assert.equal(blueprint.Units.length,18);
 assert.equal(blueprint.Lessons.length,148);
 const l=blueprint.Lessons.find(x=>x.LessonCode===code);
 assert.ok(l);assert.equal(l.UnitNumber,1);assert.equal(l.LessonNumber,4);
 assert.equal(l.Title,'نواتج الضرب الخاصة');assert.equal(l.UnitTitle,'كثيرات الحدود والتحليل');
 const c=content.Lessons.find(x=>x.LessonCode===code);
 assert.ok(c);assert.match(c.SourceLocator,/PDF pages 41–46.*printed pages 28–33/);
 assert.equal(c.SourceSha256,'d7e492a960675c30ac7eebb0144ed2dc51d1f4e9c91c9834ea05638f614a3c72');
 assert.match(c.AdaptationStatus,/READY_FOR_STAGE_D/);
 const ar=c.Translations.find(x=>x.CultureCode==='ar');
 assert.ok(ar);
 for(const f of ['Explanation','KeyConceptsAndRules','WorkedExamples','StepByStepSolutions','CommonMistakes','QuickSummary'])
   assert.ok(ar[f].length>100,f);
});
test('Special-product RichV2 covers squared sum/difference, conjugates and original visual area',()=>{
 assert.equal(rich.Lessons.length,1);
 const l=rich.Lessons[0];
 assert.equal(l.LessonCode,code);
 assert.equal(l.WorkedExamples.length,9);
 assert.equal(l.Visuals.length,5);
 assert.ok(l.Visuals.some(x=>x.Kind==='ShapeDecomposition'));
 assert.ok(l.Visuals.some(x=>x.Kind==='ExpressionStripBars'));
 for(const x of [-3,-1,0,1,2,4]){
  assert.equal((x+4)**2,x*x+8*x+16);
  assert.equal((2*x-3)**2,4*x*x-12*x+9);
  assert.equal((3*x+2)*(3*x-2),9*x*x-4);
 }
 assert.ok(l.WorkedExamples.some(x=>x.Answer.includes('٤س²−١٢س+٩')));
});
test('Source ledger has 20 individual page-grounded assessment categories',()=>{
 assert.deepEqual(source.sourcePdfPages,[41,42,43,44,45,46]);
 assert.equal(source.sourceElements.length,20);
 assert.equal(new Set(source.sourceElements.map(x=>x.elementId)).size,20);
 assert.equal(source.sourceElementDenominatorVerifiedForThisLesson,true);
 assert.ok(source.sourceElements.every(x=>x.sourcePhysicalPage-13===x.sourcePrintedPage&&x.appLocation));
 assert.equal(source.testsRun,'WRITTEN_NOT_RUN');
});
test('Exact special products reuse correct Grade9 coefficient families and a bounded full response',()=>{
 const mapping=mappings.mappings.find(x=>x.lessonCode===code);
 assert.ok(mapping);assert.deepEqual(mapping.primarySkills,['algebra.polynomial.special_products']);
 assert.deepEqual(mapping.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(contract.runtimeGate.productionRouting,false);
 for(const id of contract.allowedQuestionFamilies){
  const row=registry.families.find(x=>x.id===id);
  assert.ok(row,id);
  assert.equal(row.skillId,mapping.primarySkills[0]);
  assert.equal(row.productionRouting,false);
 }
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 const specific=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10SpecialProducts.cs'),'utf8');
 assert.match(engine,/BuildG10SpecialProduct/);
 assert.match(specific,/UaeG10PolynomialProductResponse.Verify/);
});
