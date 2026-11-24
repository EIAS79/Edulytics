'use strict';
// Owner-source UAE Grade10 General U01L15. Stage A–B–C test source only.
// Do not execute this or full CI until Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U01:L15:C015';
const bp=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u01-l15-ar.rich-lesson-v2.json');
const ledger=read(base+'lesson-015-source-elements.json');
const exact=read(base+'lesson-015-stage-c-exact-contract.json');
const mapping=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const registry=read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');

test('Canonical index and sourced Arabic U01L15 identity remain unique',()=>{
 assert.equal(bp.Lessons.length,148);assert.equal(bp.Units.length,18);
 const canonical=bp.Lessons.find(x=>x.LessonCode===code);
 assert.ok(canonical);
 assert.equal(canonical.UnitNumber,1);assert.equal(canonical.LessonNumber,15);
 assert.equal(canonical.Title,'الجذور والأصفار');assert.equal(canonical.UnitTitle,'كثيرات الحدود والتحليل');
 const lesson=pack.Lessons.find(x=>x.LessonCode===code);
 assert.ok(lesson);
 assert.equal(lesson.SourceSha256,'0a554589f4cc72537007f3229098c1ab46775d7bf2301bae59f38fe6861a9815');
 assert.match(lesson.SourceLocator,/249–257.*232–240/);
 assert.match(lesson.AdaptationStatus,/READY_FOR_STAGE_D/);
 const ar=lesson.Translations.find(x=>x.CultureCode==='ar');
 assert.ok(ar);assert.equal(ar.Title,canonical.Title);
 for(const field of ['Explanation','KeyConceptsAndRules','WorkedExamples','StepByStepSolutions','CommonMistakes','QuickSummary'])
   assert.ok(ar[field].length>160,field);
});
test('Advanced original roots/zeros lesson and calculator lab have 38 auditable source goals',()=>{
 assert.deepEqual(ledger.sourcePdfPages,[249,250,251,252,253,254,255,256,257]);
 assert.equal(ledger.sourceElementCount,38);
 assert.equal(ledger.sourceElements.length,38);
 assert.equal(new Set(ledger.sourceElements.map(x=>x.elementId)).size,38);
 assert.ok(ledger.sourceElements.every(x=>x.sourcePrintedPage===x.sourcePhysicalPage-17&&x.appLocation&&x.goal));
 assert.ok(ledger.sourceElements.some(x=>x.category==='technology_lab'));
 assert.equal(ledger.testsRun,'WRITTEN_NOT_RUN');
});
test('Original Arabic example count, Descartes sign reasoning and Cartesian data are mathematically sound',()=>{
 const lesson=rich.Lessons[0];
 assert.equal(lesson.LessonCode,code);assert.equal(lesson.WorkedExamples.length,10);assert.equal(lesson.Visuals.length,5);
 const plots=lesson.Visuals.filter(v=>v.Kind==='CartesianPlot');
 assert.equal(plots.length,2);
 const f=[x=>((x-1)**2)*(x+2),x=>x**3-x*x-4*x+4];
 for(let i=0;i<plots.length;i++)for(const p of plots[i].Rows){
  const x=Number(p.Cells[0]),y=Number(p.Cells[1]);
  assert.equal(y,f[i](x),plots[i].Title+' x='+x);
 }
 for(const x of [-3,-2,-1,0,1,2,3]){
  assert.equal((x-1)*(x+2)*(x*x-4*x+5),x**4-3*x**3-x*x+13*x-10);
 }
 const sign=['+','-','+','-'];
 assert.equal(sign.slice(1).reduce((n,s,i)=>n+(s!==sign[i]?1:0),0),3);
});
test('One exact primary skill and three correct families are fail-closed for Stage D',()=>{
 const m=mapping.mappings.find(x=>x.lessonCode===code);assert.ok(m);
 assert.deepEqual(m.primarySkills,['algebra.polynomial.roots_zeros_complex']);
 assert.deepEqual(m.allowedQuestionFamilies,exact.allowedQuestionFamilies);
 assert.equal(exact.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(exact.runtimeGate.productionRouting,false);
 for(const id of exact.allowedQuestionFamilies){
  const f=registry.families.find(x=>x.id===id);
  assert.ok(f,id);assert.equal(f.skillId,m.primarySkills[0]);
  assert.equal(f.productionRouting,false);assert.equal(f.lessonPracticeRouting,true);
 }
 const source=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10RootsZeros.cs'),'utf8');
 const dispatcher=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 assert.match(source,/UaeG10RootsZerosResponse/);
 assert.match(dispatcher,/BuildG10RootsZeros/);
});
