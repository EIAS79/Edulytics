'use strict';
// Source-first Grade10 U04L02 A/B/C guards; execute only under Stage D.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),load=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U04:L02:C035';
test('Canonical35 source boundaries are original Advanced lesson2-2 plus separate technology lab',()=>{
 const bp=load('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
 const l=bp.Lessons.find(x=>x.LessonCode===code);assert.ok(l);
 assert.equal(l.SortOrder,35);assert.equal(l.UnitNumber,4);assert.equal(l.LessonNumber,2);
 assert.equal(l.Title,'حل المعادلات التربيعية بالتمثيل البياني');
 const src=load(base+'lesson-035-source-elements.json');
 assert.deepEqual(src.sourceLessonPdfPages,[122,123,124,125,126,127,128,129]);
 assert.equal(src.sourceTechnologyLabPdfPage,130);
 assert.equal(src.nextOfficialLessonStartsPdfPage,131);
 assert.equal(src.sourceElements.length,25);
 assert.ok(src.sourceElements.every(x=>x.sourcePrintedPage===x.sourcePhysicalPage-17));
});
test('Arabic source content includes distinct graph root cases, four CartesianPlots and real domain',()=>{
 const c=load('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
 const l=c.Lessons.find(x=>x.LessonCode===code);assert.ok(l);
 assert.match(l.AdaptationStatus,/READY_FOR_STAGE_D/);
 const rich=load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u04-l02-ar.rich-lesson-v2.json').Lessons[0];
 assert.equal(rich.WorkedExamples.length,13);
 assert.equal(rich.Visuals.filter(x=>x.Kind==='CartesianPlot').length,4);
 assert.ok(rich.WorkedExamples.some(x=>x.Answer.includes('س=٣ أو س=٥')));
 for(const x of [-3,-2,0,1,3,4])assert.ok(x*x-x-6===(x+2)*(x-3));
 assert.equal(-16*5*5+64*5+80,0);
 assert.equal(-16*(-1)**2+64*(-1)+80,0);
});
test('Exact Stage C chooses root counts instead of reusing unrelated vertex-only generator',()=>{
 const c=load(base+'lesson-035-stage-c-exact-contract.json');
 const m=load('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);
 assert.ok(m);
 assert.deepEqual(m.primarySkills,['algebra.quadratic.solve_by_graphing']);
 assert.deepEqual(m.allowedQuestionFamilies,c.allowedQuestionFamilies);
 assert.equal(c.testsRun,'WRITTEN_NOT_RUN');
 const registry=load('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');
 for(const id of c.allowedQuestionFamilies) {
  const r=registry.families.find(x=>x.id===id);assert.ok(r,id);
  assert.equal(r.skillId,m.primarySkills[0]);assert.equal(r.productionRouting,false);
 }
 const core=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 assert.match(core,/BuildG10QuadraticGraphRoots/);
});
