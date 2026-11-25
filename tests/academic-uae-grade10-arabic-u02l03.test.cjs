'use strict';
// Grade10 U02L03 Stage A-B-C authored guards. Execute only in Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),load=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:02:03:RATIONAL-EXPONENTS';
const bp=load('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const content=load('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u02-l03-ar.rich-lesson-v2.json');
const source=load(base+'lesson-019-source-elements.json'),contract=load(base+'lesson-019-stage-c-exact-contract.json');
const mapping=load('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const registry=load('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');
test('Rational exponents retain Grade10 owner-approved canonical identity',()=>{
 const b=bp.Lessons.find(x=>x.LessonCode===code);
 assert.equal(bp.Lessons.length,148);assert.equal(bp.Units.length,18);
 assert.ok(b);assert.equal(b.SortOrder,19);assert.equal(b.UnitNumber,2);assert.equal(b.LessonNumber,3);
 assert.equal(b.Title,'الأسس النسبية');assert.equal(b.UnitTitle,'الأسس والتعابير الجذرية');
 const c=content.Lessons.find(x=>x.LessonCode===code);assert.ok(c);
 assert.match(c.SourceLocator,/PDF113–119.*PDF311–316/);
 assert.match(c.AdaptationStatus,/READY_FOR_STAGE_D/);
 assert.equal(c.SourceSha256,'d7e492a960675c30ac7eebb0144ed2dc51d1f4e9c91c9834ea05638f614a3c72');
 const ar=c.Translations.find(x=>x.CultureCode==='ar');assert.ok(ar);
 for(const key of ['Explanation','KeyConceptsAndRules','WorkedExamples','StepByStepSolutions','CommonMistakes','QuickSummary'])assert.ok(ar[key].length>100,key);
});
test('Original Arabic rational powers include source-specific real domains and checks',()=>{
 const l=rich.Lessons[0];assert.equal(l.LessonCode,code);
 assert.equal(l.WorkedExamples.length,12);assert.equal(l.Visuals.length,5);
 assert.ok(l.WorkedExamples.some(x=>x.Answer==='١/٢٥'));
 assert.ok(l.ExplanationParagraphs.some(x=>x.includes('س²')));
 const nums=[[27,2,3,9],[16,3,4,8],[125,-2,3,1/25],[32,2,5,4],[9,3,2,27]];
 for(const [a,m,n,expect] of nums)assert.ok(Math.abs(a**(m/n)-expect)<1e-9);
 assert.equal(Math.cbrt(-8),-2);
});
test('Source spans and category-level denominator identify 7 general and 6 advanced pages without double-count',()=>{
 assert.deepEqual(source.sourcePdfPages,[113,114,115,116,117,118,119]);
 assert.deepEqual(source.advancedSource.pdfPages,[311,312,313,314,315,316]);
 assert.equal(source.sourceElements.length,24);
 assert.equal(new Set(source.sourceElements.map(x=>x.elementId)).size,24);
 assert.ok(source.sourceElements.every(x=>x.sourcePhysicalPage-13===x.sourcePrintedPage&&x.appLocation));
 assert.equal(source.testsRun,'WRITTEN_NOT_RUN');
});
test('Exact sources reuse existing primary skill and do not enable production routing',()=>{
 const m=mapping.mappings.find(x=>x.lessonCode===code);assert.ok(m);
 assert.deepEqual(m.primarySkills,['algebra.exponent.rational_power']);
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(contract.runtimeGate.productionRouting,false);
 for(const id of m.allowedQuestionFamilies){const r=registry.families.find(x=>x.id===id);assert.ok(r,id);assert.equal(r.skillId,m.primarySkills[0]);assert.equal(r.productionRouting,false);}
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10RationalExponents.cs'),'utf8');
 assert.match(engine,/class\s+UaeG10RationalExponentsResponse\b/);
 assert.match(engine,/static\s+bool\s+Verify\s*\(/);
 assert.match(fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8'),/return\s+UaeG10RationalExponentsResponse\.Verify\(family,\s*parameters,\s*answer\)/);
});
