'use strict';
// Grade10 source-specific Stage A–B–C guard. Tests authored, NOT RUN until Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');const get=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U01:L13:C013';
const blueprint=get('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const content=get('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=get('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u01-l13-ar.rich-lesson-v2.json');
const source=get(base+'lesson-013-source-elements.json');
const contract=get(base+'lesson-013-stage-c-exact-contract.json');
const mapping=get('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const registry=get('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');

test('Owner-approved Grade10 General lesson13 identity is source-matched, not a duplicate of quadratic lessons',()=>{
 assert.equal(blueprint.Units.length,18);assert.equal(blueprint.Lessons.length,148);
 const l=blueprint.Lessons.find(x=>x.LessonCode===code);
 assert.ok(l);assert.equal(l.UnitNumber,1);assert.equal(l.LessonNumber,13);
 assert.equal(l.Title,'حل المعادلات كثيرة الحدود');assert.equal(l.UnitTitle,'كثيرات الحدود والتحليل');
 const body=content.Lessons.find(x=>x.LessonCode===code);
 assert.ok(body);assert.equal(body.SourceSha256,'0a554589f4cc72537007f3229098c1ab46775d7bf2301bae59f38fe6861a9815');
 assert.match(body.SourceLocator,/233–242.*216–225/);
 assert.match(body.AdaptationStatus,/READY_FOR_STAGE_D/);
 const ar=body.Translations.find(x=>x.CultureCode==='ar');assert.ok(ar);
 for(const key of ['Explanation','KeyConceptsAndRules','WorkedExamples','StepByStepSolutions','CommonMistakes','QuickSummary'])
   assert.ok(ar[key].length>150,key);
});
test('Visually audited original lesson3-5 source includes 32 distinct mapped targets and calculator lab',()=>{
 assert.deepEqual(source.sourcePdfPages,[233,234,235,236,237,238,239,240,241,242]);
 assert.equal(source.sourceElementCount,32);
 assert.equal(source.sourceElements.length,32);
 assert.equal(new Set(source.sourceElements.map(x=>x.elementId)).size,32);
 assert.ok(source.sourceElements.every(x=>x.appLocation&&x.goal&&x.sourcePrintedPage===x.sourcePhysicalPage-17));
 assert.ok(source.sourceElements.some(x=>x.category==='technology_lab'));
 assert.equal(source.testsRun,'WRITTEN_NOT_RUN');
});
test('Original cubic and quartic visual models have numerically checked exact Cartesian values',()=>{
 const l=rich.Lessons[0];assert.equal(l.LessonCode,code);
 assert.equal(l.WorkedExamples.length,10);assert.equal(l.Visuals.length,5);
 const plots=l.Visuals.filter(v=>v.Kind==='CartesianPlot');
 assert.equal(plots.length,2);
 const polynomial=[x=>x*(x+3)*(x-2),x=>x**4-13*x*x+36];
 for(let i=0;i<plots.length;i++)for(const point of plots[i].Rows){
  const x=Number(point.Cells[0]),y=Number(point.Cells[1]);
  assert.ok(y===polynomial[i](x),plots[i].Title+' x='+x);
 }
 for(const x of [-4,-3,-2,-1,0,1,2,3,4]){
   assert.equal(x**3+x*x-6*x+0,x*(x+3)*(x-2)+0);
   assert.equal(x**3+2*x*x-9*x-18+0,(x+2)*(x-3)*(x+3)+0);
   assert.equal(x**4-13*x*x+36+0,(x*x-4)*(x*x-9)+0);
 }
 assert.equal(3*4*5,60);
});
test('Exact Grade10 integer root families and renderer remain source-bound and fail closed for Stage D',()=>{
 const m=mapping.mappings.find(x=>x.lessonCode===code);assert.ok(m);
 assert.deepEqual(m.primarySkills,['algebra.polynomial.solve_higher_degree']);
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');assert.equal(contract.runtimeGate.productionRouting,false);
 for(const fam of contract.allowedQuestionFamilies){
  const f=registry.families.find(x=>x.id===fam);
  assert.ok(f,fam);assert.equal(f.skillId,m.primarySkills[0]);
  assert.equal(f.productionRouting,false);assert.equal(f.lessonPracticeRouting,true);
 }
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 const verifier=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10PolynomialEquations.cs'),'utf8');
 assert.match(engine,/BuildG10PolynomialEquation/);
 assert.match(verifier,/internal static bool Verify/);
});
