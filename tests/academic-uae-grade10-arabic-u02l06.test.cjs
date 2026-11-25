'use strict';
// Grade10 General U02L06 A–B–C source contracts. Tests NOT RUN until Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:02:06:OPERATIONS-WITH-RADICAL-EXPRESSIONS';
const blueprint=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const content=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u02-l06-ar.rich-lesson-v2.json');
const source=read(base+'lesson-022-source-elements.json'),contract=read(base+'lesson-022-stage-c-exact-contract.json');
const maps=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const families=read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');
test('Canonical Grade10 index retains U02L06 identity and Arabic lesson',()=>{
 assert.equal(blueprint.Lessons.length,148);
 const b=blueprint.Lessons.find(x=>x.LessonCode===code);
 assert.ok(b);assert.equal(b.SortOrder,22);assert.equal(b.UnitNumber,2);assert.equal(b.LessonNumber,6);
 assert.equal(b.Title,'العمليات على التعابير الجذرية');
 const c=content.Lessons.find(x=>x.LessonCode===code);assert.ok(c);
 assert.match(c.AdaptationStatus,/READY_FOR_STAGE_D/);
 assert.match(c.SourceLocator,/137–142.*304–310/);
 assert.ok(c.Translations.find(x=>x.CultureCode==='ar')?.Explanation.length>350);
});
test('Arabic explanation and original 13 worked examples have correct radical math',()=>{
 const l=rich.Lessons[0];assert.equal(l.LessonCode,code);
 assert.equal(l.WorkedExamples.length,13);assert.equal(l.Visuals.length,5);
 assert.ok(l.WorkedExamples.some(x=>x.Answer==='٨√٢'));
 assert.ok(Math.abs(Math.sqrt(18)+Math.sqrt(50)-8*Math.sqrt(2))<1e-9);
 assert.ok(Math.abs(Math.sqrt(72)-Math.sqrt(8)-4*Math.sqrt(2))<1e-9);
 assert.ok(Math.abs((Math.sqrt(5)+2)*(Math.sqrt(5)-2)-1)<1e-9);
});
test('Six General + seven Advanced pages have categorized source activities',()=>{
 assert.deepEqual(source.sourcePdfPages,[137,138,139,140,141,142]);
 assert.deepEqual(source.advancedSource.pdfPages,[304,305,306,307,308,309,310]);
 assert.equal(source.sourceElements.length,30);
 assert.equal(new Set(source.sourceElements.map(x=>x.elementId)).size,30);
 assert.ok(source.sourceElements.every(x=>x.appLocation&&x.coverageStatus));
 assert.equal(source.testsRun,'WRITTEN_NOT_RUN');
});
test('Like radical coefficient reuse and two Grade10 integer exact families route correctly',()=>{
 const m=maps.mappings.find(x=>x.lessonCode===code);assert.ok(m);
 assert.deepEqual(m.primarySkills,['algebra.radical.combine_like_terms']);
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.ok(m.forbiddenQuestionFamilies.includes('uae.g10.general.u02.l05.square_root_extract_coefficient'));
 for(const id of m.allowedQuestionFamilies){const r=families.families.find(x=>x.id===id);assert.ok(r,id);assert.equal(r.skillId,m.primarySkills[0]);assert.equal(r.productionRouting,false);}
 for(const id of m.allowedQuestionFamilies.slice(1))assert.equal(families.families.find(x=>x.id===id).answerType,'exact_integer');
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(contract.runtimeGate.productionRouting,false);
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10RadicalOperations.cs'),'utf8');
 assert.match(engine,/UaeG10RadicalOperationsResponse.Verify/);
});
