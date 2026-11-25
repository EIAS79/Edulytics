'use strict';
// UAE G10 General approved 148 curriculum, canonical U01L11.
// Authored Stage A–B–C. Tests are WRITTEN_NOT_RUN until Stage D.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U01:L11:C011';
const blueprint=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const content=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u01-l11-ar.rich-lesson-v2.json');
const ledger=read(base+'lesson-011-source-elements.json');
const exact=read(base+'lesson-011-stage-c-exact-contract.json');
const mapping=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const registry=read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');

test('Canonical approved General row eleven has one newly authored Arabic content record',()=>{
 assert.equal(blueprint.Units.length,18);
 assert.equal(blueprint.Lessons.length,148);
 const bp=blueprint.Lessons.find(x=>x.LessonCode===code);
 assert.ok(bp);
 assert.equal(bp.UnitNumber,1);assert.equal(bp.LessonNumber,11);
 assert.equal(bp.Title,'الدوال كثيرة الحدود');
 assert.equal(bp.UnitTitle,'كثيرات الحدود والتحليل');
 assert.equal(content.Lessons.length,148);
 assert.equal(new Set(content.Lessons.map(x=>x.LessonCode)).size,148);
 const matched=content.Lessons.filter(x=>x.LessonCode===code);
 assert.equal(matched.length,1);
 const lesson=matched[0];
 assert.equal(lesson.OfficialReferenceCode,null);
 assert.equal(lesson.SourceSha256,'0a554589f4cc72537007f3229098c1ab46775d7bf2301bae59f38fe6861a9815');
 assert.match(lesson.SourceLocator,/PDF pages 213–220/);
 assert.match(lesson.AdaptationStatus,/READY_FOR_STAGE_D/);
 assert.equal(lesson.Translations.filter(x=>x.CultureCode==='ar').length,1);
 const ar=lesson.Translations.find(x=>x.CultureCode==='ar');
 for(const f of ['Explanation','KeyConceptsAndRules','WorkedExamples','StepByStepSolutions','CommonMistakes','QuickSummary'])
  assert.ok(ar[f].length>100,f);
});
test('First-party Arabic polynomial functions include actual points not merely decorative graph labels',()=>{
 const l=rich.Lessons[0];
 assert.equal(rich.Lessons.length,1);
 assert.equal(l.LessonCode,code);
 assert.equal(l.WorkedExamples.length,11);
 assert.equal(l.Visuals.length,5);
 const graphs=l.Visuals.filter(x=>x.Kind==='CartesianPlot');
 assert.equal(graphs.length,2);
 assert.equal(graphs[0].Rows.length,5);
 assert.equal(graphs[1].Rows.length,5);
 for(const graph of graphs)
  for(const row of graph.Rows) {
   assert.equal(row.Cells.length,2);
   assert.ok(row.Cells.every(x=>Number.isFinite(Number(x))),'graph must hold numeric coordinates');
  }
 for(const row of graphs[0].Rows) {
  const [x,y]=row.Cells.map(Number);
  assert.equal(y,x*x*x-4*x);
 }
 for(const row of graphs[1].Rows) {
  const [x,y]=row.Cells.map(Number);
  assert.equal(y,x*x*x*x-5*x*x+4);
 }
 const model=l.WorkedExamples.find(x=>x.Question.includes('ح(t)'));
 assert.ok(model,'source-model worked example must exist');
 assert.match(model.Answer,/٠٫٥٢/);
});
test('Lesson 11 Advanced PDF 213–220 maps each source teaching/assessment type',()=>{
 assert.deepEqual(ledger.sourcePdfPages,[213,214,215,216,217,218,219,220]);
 assert.equal(ledger.sourceElementCount,27);
 assert.equal(ledger.sourceElements.length,27);
 assert.equal(new Set(ledger.sourceElements.map(x=>x.elementId)).size,27);
 assert.ok(ledger.sourceElements.every(x=>
  x.sourcePrintedPage===x.sourcePhysicalPage-17&&
  x.coverageStatus&&x.appLocation));
 assert.equal(ledger.testsRun,'WRITTEN_NOT_RUN');
});
test('Three exact function families are registered but fail closed until Stage D',()=>{
 const m=mapping.mappings.find(x=>x.lessonCode===code);
 assert.ok(m);
 assert.deepEqual(m.primarySkills,['algebra.polynomial.functions']);
 assert.deepEqual(m.allowedQuestionFamilies,exact.allowedQuestionFamilies);
 assert.equal(exact.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(exact.runtimeGate.productionRouting,false);
 for(const id of m.allowedQuestionFamilies){
  const family=registry.families.filter(x=>x.id===id);
  assert.equal(family.length,1,id);
  assert.equal(family[0].skillId,m.primarySkills[0]);
  assert.equal(family[0].productionRouting,false);
  assert.equal(family[0].lessonPracticeRouting,true);
 }
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 const source=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10PolynomialFunctions.cs'),'utf8');
 assert.match(engine,/BuildG10PolynomialFunction/);
 assert.match(source,/UaeG10PolynomialFunctionResponse/);
});
