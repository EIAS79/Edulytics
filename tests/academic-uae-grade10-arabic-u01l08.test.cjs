'use strict';
// Source-scoped academic A–B–C assertions. Do not execute before Stage D.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:01:08:DIFFERENCE-OF-TWO-SQUARES';
const blueprint=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u01-l08-ar.rich-lesson-v2.json');
const source=read(base+'lesson-008-source-elements.json');
const contract=read(base+'lesson-008-stage-c-exact-contract.json');
const mappings=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const registry=read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');

test('Canonical U01L08 identity preserves original PED and evidence PDF71–76',()=>{
 assert.equal(blueprint.Units.length,18);
 assert.equal(blueprint.Lessons.length,148);
 const b=blueprint.Lessons.find(x=>x.LessonCode===code);
 assert.ok(b);assert.equal(b.UnitNumber,1);assert.equal(b.LessonNumber,8);
 assert.equal(b.Title,'الفرق بين مربعين');assert.equal(b.UnitTitle,'كثيرات الحدود والتحليل');
 const lesson=pack.Lessons.find(x=>x.LessonCode===code);
 assert.ok(lesson);assert.match(lesson.SourceLocator,/PDF pages 71–76.*printed pages 58–63/);
 assert.equal(lesson.SourceSha256,'d7e492a960675c30ac7eebb0144ed2dc51d1f4e9c91c9834ea05638f614a3c72');
 assert.match(lesson.AdaptationStatus,/READY_FOR_STAGE_D/);
 const ar=lesson.Translations.find(x=>x.CultureCode==='ar');
 assert.ok(ar);assert.match(ar.Explanation,/الفرق بين مربعين/);
 assert.ok(ar.WorkedExamples.length>2000);
});

test('Original Arabic teaching contains inverse factoring, GCF, repeat factoring and frame area',()=>{
 assert.equal(rich.Lessons.length,1);
 const l=rich.Lessons[0];
 assert.equal(l.WorkedExamples.length,11);
 assert.equal(l.Visuals.length,6);
 assert.ok(l.Visuals.some(x=>x.Kind==='ExpressionStripBars'));
 assert.ok(l.WorkedExamples.some(x=>x.Answer.includes('٣س(٢س−١)(٢س+١)')));
 assert.ok(l.WorkedExamples.some(x=>x.Answer.includes('٢(س+٣)(س−١)(س+١)')));
 for(const x of [-5,-2,-1,0,1,2,5]){
  assert.equal(9*x*x-16,(3*x-4)*(3*x+4));
  assert.equal(x**4-81,(x-3)*(x+3)*(x*x+9));
  assert.equal(2*x**3+6*x*x-2*x-6,2*(x+3)*(x-1)*(x+1));
 }
});
test('All 26 book-source element types preserve original page provenance and boundaries',()=>{
 assert.deepEqual(source.sourcePdfPages,[71,72,73,74,75,76]);
 assert.equal(source.sourceElements.length,26);
 assert.equal(source.sourceElementDenominatorVerifiedForThisLesson,true);
 assert.equal(new Set(source.sourceElements.map(x=>x.elementId)).size,26);
 assert.ok(source.sourceElements.every(x=>x.appLocation&&x.sourcePrintedPage===x.sourcePhysicalPage-13));
 assert.equal(source.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(source.crossbookOverlap.goalId,'C11');
});
test('Exact inverses are not duplicated forward special-products practice',()=>{
 const m=mappings.mappings.find(x=>x.lessonCode===code);
 assert.ok(m);assert.deepEqual(m.primarySkills,['algebra.factor.difference_of_squares']);
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(contract.runtimeGate.productionRouting,false);
 assert.ok(m.forbiddenQuestionFamilies.includes('uae.g10.general.u01.l04.full_special_product'));
 for(const id of m.allowedQuestionFamilies){
  const f=registry.families.find(x=>x.id===id);assert.ok(f,id);
  assert.equal(f.skillId,'algebra.factor.difference_of_squares');
  assert.equal(f.productionRouting,false);assert.equal(f.lessonPracticeRouting,true);
 }
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10DifferenceSquares.cs'),'utf8');
 assert.match(engine,/internal static bool Verify\(string family/);
 assert.match(engine,/FactorFamily/);
 const dispatcher=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 assert.match(dispatcher,/UaeG10DifferenceSquaresResponse\.Verify\(family,parameters,answer\)/);
 assert.match(dispatcher,/BuildG10DifferenceSquares\(family,random,difficulty\)/);
});
