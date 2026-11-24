'use strict';
// Original Arabic UAE Grade 10 General nonmonic lesson, A/B/C test source.
// Intentionally written but NOT RUN before Stage D.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path'),root=path.resolve(__dirname,'..');
const load=x=>JSON.parse(fs.readFileSync(path.join(root,x),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:01:07:SOLVING-AX-BX-C-0';
const blueprint=load('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const pack=load('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u01-l07-ar.rich-lesson-v2.json');
const ledger=load(base+'lesson-007-source-elements.json');
const contract=load(base+'lesson-007-stage-c-exact-contract.json');
const cross=load(base+'lesson-006-crossbook-semantic-crosswalk.json');
const mappings=load('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const skills=load('src/Edulytics.Core/Mathematics/Skills/skill-registry.v1.json');
const families=load('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');

test('Canonical approved 148-title General U01L07 preserves original PED lesson and verified source pages',()=>{
 assert.equal(blueprint.Units.length,18);
 assert.equal(blueprint.Lessons.length,148);
 const bp=blueprint.Lessons.find(x=>x.LessonCode===code);
 assert.ok(bp);assert.equal(bp.SortOrder,7);
 assert.equal(bp.UnitNumber,1);assert.equal(bp.LessonNumber,7);
 assert.equal(bp.Title,'حل ax² + bx + c = 0');assert.equal(bp.UnitTitle,'كثيرات الحدود والتحليل');
 const l=pack.Lessons.find(x=>x.LessonCode===code);
 assert.ok(l);assert.match(l.SourceLocator,/65–69.*52–56/);
 assert.equal(l.SourceSha256,'d7e492a960675c30ac7eebb0144ed2dc51d1f4e9c91c9834ea05638f614a3c72');
 assert.match(l.AdaptationStatus,/READY_FOR_STAGE_D/);
 const ar=l.Translations.find(x=>x.CultureCode==='ar');
 assert.ok(ar);assert.equal(ar.Title,'حل ax² + bx + c = 0');
 for(const key of ['Explanation','KeyConceptsAndRules','WorkedExamples','StepByStepSolutions','CommonMistakes','QuickSummary'])
  assert.ok(ar[key].length>220,key);
});

test('Original Arabic nonmonic worked examples and signed/rational/geometry source math stay correct',()=>{
 assert.equal(rich.Lessons.length,1);
 const l=rich.Lessons[0];assert.equal(l.LessonCode,code);
 assert.equal(l.WorkedExamples.length,12);
 assert.equal(l.Visuals.length,7);
 assert.ok(l.Visuals.some(x=>x.Kind==='ShapeDecomposition'));
 assert.ok(l.Visuals.some(x=>x.Kind==='ExpressionStripBars'));
 for(const x of [-3,-1,0,1,2,6]){
  assert.equal((3*x+1)*(2*x+3),6*x*x+11*x+3);
  assert.equal((3*x+1)*(x-2),3*x*x-5*x-2);
  assert.equal((3*x-2)*(2*x+1),6*x*x-x-2);
  assert.equal(-(3*x+1)*(2*x-3),-6*x*x+7*x+3);
  assert.equal((2*x+15)*(x-2),2*x*x+11*x-30);
 }
 assert.equal((2*2+3)*(2+4),42);
 assert.equal(-10*4**2+30*4+40,0);
});

test('General nonmonic U01L07 27 category ledger does not duplicate monic/special-products lessons',()=>{
 assert.deepEqual(ledger.sourcePdfPages,[65,66,67,68,69]);
 assert.equal(ledger.sourceElements.length,27);
 assert.ok(ledger.sourceElements.every(e=>e.sourcePrintedPage===e.sourcePhysicalPage-13&&e.appLocation));
 assert.equal(new Set(ledger.sourceElements.map(e=>e.elementId)).size,27);
 assert.equal(ledger.sourceElementDenominatorVerifiedForThisLesson,true);
 assert.equal(ledger.reviewBoundary.cumulativeReviewPdfPage,70);
 assert.equal(ledger.reviewBoundary.nextLessonStartPdfPage,71);
 assert.equal(ledger.crossbookEvidence.relatedGoals.length,5);
 for(const id of ledger.crossbookEvidence.relatedGoals)
  assert.equal(cross.goalCrosswalk.find(x=>x.id===id)?.canonicalDestination,'G10-U01-L07');
 assert.equal(ledger.testsRun,'WRITTEN_NOT_RUN');
});

test('Exact full factor/rational roots/prime contract uses registered nonmonic skill with routing blocked',()=>{
 const m=mappings.mappings.find(x=>x.lessonCode===code);
 assert.ok(m);assert.deepEqual(m.primarySkills,['algebra.quadratic.factor_nonmonic']);
 assert.ok(skills.skills.some(x=>x.id==='algebra.quadratic.factor_nonmonic'));
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.equal(contract.runtimeGate.productionRouting,false);
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 for(const id of contract.allowedQuestionFamilies){
  const row=families.families.find(x=>x.id===id);
  assert.ok(row,id);
  assert.equal(row.skillId,m.primarySkills[0]);
  assert.equal(row.productionRouting,false);
  assert.equal(row.lessonPracticeRouting,true);
 }
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 const helper=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10NonmonicQuadratic.cs'),'utf8');
 assert.match(engine,/BuildG10NonmonicQuadratic/);
 assert.match(engine,/UaeG10NonmonicQuadraticResponse.Verify/);
 assert.match(helper,/FactorableOverIntegers/);
 assert.match(helper,/ParseFraction/);
});
