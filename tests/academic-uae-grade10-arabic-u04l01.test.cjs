'use strict';
// Source Grade10 U04L01 A/B/C validation source; execution belongs to Stage D.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const load=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U04:L01:C034';
const blueprint=load('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const pack=load('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u04-l01-ar.rich-lesson-v2.json').Lessons[0];
const ledger=load(base+'lesson-034-source-elements.json');
const contract=load(base+'lesson-034-stage-c-exact-contract.json');
const mappings=load('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const registry=load('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');
test('Canonical lesson34 in 148 Lesson Blueprint matches correct Advanced 2-1 source and isolated technology lab',()=>{
 const b=blueprint.Lessons.find(x=>x.LessonCode===code);assert.ok(b);
 assert.equal(b.SortOrder,34);assert.equal(b.UnitNumber,4);assert.equal(b.LessonNumber,1);
 assert.equal(b.Title,'تمثيل الدوال التربيعية بيانيًا');
 assert.deepEqual(ledger.sourceLessonPdfPages,[112,113,114,115,116,117,118,119]);
 assert.equal(ledger.sourceTechnologyLabPage,121);
 assert.equal(ledger.sourceExcludedPage.pdf,120);
 assert.equal(ledger.sourceElements.length,23);
});
test('Original Arabic student explanation includes real coordinate plots, vertex and range checks',()=>{
 const l=pack.Lessons.find(x=>x.LessonCode===code);assert.ok(l);
 assert.match(l.AdaptationStatus,/READY_FOR_STAGE_D/);
 assert.equal(l.Translations.filter(t=>t.CultureCode==='ar').length,1);
 assert.equal(rich.WorkedExamples.length,12);
 assert.equal(rich.Visuals.length,6);
 assert.equal(rich.Visuals.filter(v=>v.Kind==='CartesianPlot').length,3);
 for(const x of [-2,-1,0,1,2,3,4,5]){
    assert.equal(x*x-4*x+3,(4-x)*(4-x)-4*(4-x)+3);
 }
 assert.equal(-1,2*2-4*2+3);
 assert.equal(5,-2*2*2+8*2-3);
});
test('Stage C bounded graph questions do not claim visual drawing or regression auto grading',()=>{
 const m=mappings.mappings.find(x=>x.lessonCode===code);assert.ok(m);
 assert.deepEqual(m.primarySkills,['algebra.quadratic.graph_features']);
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.equal(contract.runtimeGate.productionRouting,false);
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 for(const id of contract.allowedQuestionFamilies){
   const family=registry.families.find(x=>x.id===id);
   assert.ok(family,id);assert.equal(family.productionRouting,false);
   assert.equal(family.skillId,m.primarySkills[0]);
   assert.equal(family.answerType,'exact_integer');
 }
 const e=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 assert.match(e,/BuildG10QuadraticGraph/);
 const ver=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10QuadraticGraph.cs'),'utf8');
 assert.match(ver,/UaeG10QuadraticGraphResponse.Verify/);
});
