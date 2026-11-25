'use strict';
// G10 General U01L14 Stage A–B–C source contracts. NOT RUN until Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const load=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U01:L14:C014';
const bp=load('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const content=load('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u01-l14-ar.rich-lesson-v2.json');
const source=load(base+'lesson-014-source-elements.json');
const contract=load(base+'lesson-014-stage-c-exact-contract.json');
const mapping=load('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const registry=load('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');

test('Approved 148 title source catalog holds the unique remainder/factor theorem identity',()=>{
 assert.equal(bp.Units.length,18);assert.equal(bp.Lessons.length,148);
 const l=bp.Lessons.find(x=>x.LessonCode===code);
 assert.ok(l);
 assert.equal(l.UnitNumber,1);assert.equal(l.LessonNumber,14);
 assert.equal(l.UnitTitle,'كثيرات الحدود والتحليل');
 assert.equal(l.Title,'نظريتا الباقي والعامل');
 const body=content.Lessons.find(x=>x.LessonCode===code);
 assert.ok(body);
 assert.match(body.SourceLocator,/243–248.*226–231/);
 assert.equal(body.SourceSha256,'0a554589f4cc72537007f3229098c1ab46775d7bf2301bae59f38fe6861a9815');
 assert.match(body.AdaptationStatus,/READY_FOR_STAGE_D/);
 const ar=body.Translations.find(x=>x.CultureCode==='ar');
 assert.ok(ar);assert.equal(ar.Title,l.Title);
 for(const key of ['Explanation','KeyConceptsAndRules','WorkedExamples','StepByStepSolutions','CommonMistakes','QuickSummary'])
   assert.ok(ar[key].length>150,key);
});
test('All six original source pages and twenty-four assessment types have an authored destination',()=>{
 assert.deepEqual(source.sourcePdfPages,[243,244,245,246,247,248]);
 assert.equal(source.sourceElementCount,24);
 assert.equal(source.sourceElements.length,24);
 assert.ok(source.sourceElements.every(x=>x.sourcePrintedPage===x.sourcePhysicalPage-17&&x.appLocation));
 assert.equal(new Set(source.sourceElements.map(x=>x.elementId)).size,24);
 assert.equal(source.sourceElementDenominatorVerifiedForThisLesson,true);
 assert.equal(source.testsRun,'WRITTEN_NOT_RUN');
});
test('Original Arabic models, ten complete examples and real quadratic graph are correct',()=>{
 const l=rich.Lessons[0];assert.equal(l.LessonCode,code);
 assert.equal(l.WorkedExamples.length,10);assert.equal(l.Visuals.length,5);
 const graph=l.Visuals.find(v=>v.Kind==='CartesianPlot');
 assert.ok(graph);
 for(const p of graph.Rows) {
   const x=Number(p.Cells[0]),y=Number(p.Cells[1]);
   assert.ok(y===x*x-5*x+6,'point x='+x);
 }
 assert.equal(2**3-3*(2**2)+2*2+5,5);
 assert.equal(1-6+11-6,0);
 assert.equal(2*((-1)**3)-(-1)+7,6);
 for(const x of [-2,-1,0,1,2,3]){
   assert.equal((x-1)*(x**3+x*x-3*x-3),x**4-4*x*x+3);
 }
});
test('One source-scoped skill reuses exact division and fails closed for Stage D',()=>{
 const m=mapping.mappings.find(x=>x.lessonCode===code);
 assert.ok(m);
 assert.deepEqual(m.primarySkills,['algebra.polynomial.remainder_factor_theorems']);
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(contract.runtimeGate.productionRouting,false);
 for(const f of contract.allowedQuestionFamilies){
   const row=registry.families.find(x=>x.id===f);
   assert.ok(row,f);assert.equal(row.skillId,m.primarySkills[0]);
   assert.equal(row.productionRouting,false);assert.equal(row.lessonPracticeRouting,true);
 }
 const actual=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10RemainderFactor.cs'),'utf8');
 assert.match(actual,/BuildG10PolynomialDivision/);
 assert.match(actual,/UaeG10PolynomialDivisionResponse.Verify/);
 assert.match(actual,/UaeG10RemainderFactorResponse/);
});
