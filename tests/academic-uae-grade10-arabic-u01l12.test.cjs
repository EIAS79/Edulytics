'use strict';
// A–B–C authoring guardrails; STAGE D owns test execution.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U01:L12:C012';
const source=read(base+'lesson-012-source-elements.json');
const contract=read(base+'lesson-012-stage-c-exact-contract.json');
const bp=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u01-l12-ar.rich-lesson-v2.json');
const mapping=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const registry=read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');

test('Canonical approved 148-index and source Arabic lesson12 retain unique identity',()=>{
 assert.equal(bp.Units.length,18);assert.equal(bp.Lessons.length,148);
 const lesson=bp.Lessons.find(x=>x.LessonCode===code);
 assert.ok(lesson);assert.equal(lesson.Title,'تحليل التمثيلات البيانية للدوال كثيرة الحدود');
 assert.equal(lesson.UnitTitle,'كثيرات الحدود والتحليل');assert.equal(lesson.LessonNumber,12);
 const body=pack.Lessons.find(x=>x.LessonCode===code);
 assert.ok(body);assert.equal(body.SourceSha256,'0a554589f4cc72537007f3229098c1ab46775d7bf2301bae59f38fe6861a9815');
 assert.match(body.SourceLocator,/221–230.*204–213/);
 assert.match(body.AdaptationStatus,/READY_FOR_STAGE_D/);
 const ar=body.Translations.find(t=>t.CultureCode==='ar');
 assert.ok(ar);assert.equal(ar.Title,lesson.Title);
 for(const key of ['Explanation','KeyConceptsAndRules','WorkedExamples','StepByStepSolutions','CommonMistakes','QuickSummary'])
   assert.ok(ar[key].length>150,key);
});
test('Source PDF 221..230 covers 34 mapped original goal/assessment types including graphing lab',()=>{
 assert.deepEqual(source.sourcePdfPages,[221,222,223,224,225,226,227,228,229,230]);
 assert.equal(source.sourceElementCount,34);
 assert.equal(source.elementDenominatorForThisBook,34);
 assert.equal(source.sourceElements.length,34);
 assert.ok(source.sourceElements.every(e=>e.appLocation&&e.goal&&e.sourcePrintedPage===e.sourcePhysicalPage-17));
 assert.equal(new Set(source.sourceElements.map(x=>x.elementId)).size,34);
 assert.ok(source.sourceElements.some(e=>e.category==='technology_lab'));
 assert.equal(source.testsRun,'WRITTEN_NOT_RUN');
});
test('First-party graph visuals use mathematically correct original coordinate sets',()=>{
 assert.equal(rich.Lessons.length,1);
 const l=rich.Lessons[0];
 assert.equal(l.WorkedExamples.length,10);
 assert.equal(l.Visuals.length,5);
 const plots=l.Visuals.filter(v=>v.Kind==='CartesianPlot');
 assert.equal(plots.length,3);
 assert.ok(plots.every(v=>v.Rows.length>=5));
 const f=x=>x**3-4*x,g=x=>x**4-5*x*x+4,q=x=>-(x-1)**2+4;
 for(const [i,fun] of [[0,f],[1,q],[2,g]]) {
   for(const pt of plots[i].Rows){
     const x=Number(pt.Cells[0]),y=Number(pt.Cells[1]);
     assert.equal(y,fun(x),plots[i].Title+' x='+x);
   }
 }
 for(const x of [-2,-1,0,1,2,3]) {
   assert.equal((x+2)*(x-1)*(x-3),x**3-2*x*x-5*x+6);
 }
 assert.equal(2*4*4+3*4+1,45);
});
test('One source-exact primary skill and three fail-closed graph answer families',()=>{
 const m=mapping.mappings.find(x=>x.lessonCode===code);assert.ok(m);
 assert.deepEqual(m.primarySkills,['algebra.polynomial.graph_analysis']);
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(contract.runtimeGate.productionRouting,false);
 assert.equal(contract.rendererMatrix.length,3);
 for(const row of contract.rendererMatrix)assert.equal(row.renderer,'StructuredEntry');
 for(const family of contract.allowedQuestionFamilies){
   const f=registry.families.find(x=>x.id===family);
   assert.ok(f,family);assert.equal(f.skillId,m.primarySkills[0]);
   assert.equal(f.lessonPracticeRouting,true);
   assert.equal(f.productionRouting,false);
 }
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 const verifier=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10GraphAnalysis.cs'),'utf8');
 assert.match(engine,/BuildG10GraphAnalysis/);
 assert.match(verifier,/UaeG10GraphAnalysisResponse.Verify/);
});
