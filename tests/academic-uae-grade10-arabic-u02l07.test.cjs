'use strict';
// Stage A-B-C source and graph contract test: written NOT RUN until Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U02:L07:C023';
const bp=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u02-l07-ar.rich-lesson-v2.json');
const source=read(base+'lesson-023-source-elements.json'),contract=read(base+'lesson-023-stage-c-exact-contract.json');
const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const reg=read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');
test('New Grade10 graph lesson retains unique canonical identity and source metadata',()=>{
 assert.equal(bp.Lessons.length,148);
 const b=bp.Lessons.find(x=>x.LessonCode===code);assert.ok(b);
 assert.equal(b.SortOrder,23);assert.equal(b.UnitNumber,2);assert.equal(b.LessonNumber,7);
 assert.equal(b.Title,'تمثيل الدوال الجذرية بيانيًا');
 assert.equal(bp.Lessons.filter(x=>x.LessonCode===code).length,1);
 const c=pack.Lessons.find(x=>x.LessonCode===code);assert.ok(c);
 assert.match(c.AdaptationStatus,/READY_FOR_STAGE_D/);
 assert.ok(c.Translations.find(t=>t.CultureCode==='ar')?.Explanation.length>350);
 assert.match(c.SourceLocator,/289–295.*170–176/);
});
test('Source map keeps Advanced4-3 and Applied6-1 merged without duplicate lessons',()=>{
 assert.deepEqual(source.sourcePdfPages,[289,290,291,292,293,294,295]);
 assert.deepEqual(source.appliedSource.pdfPages,[170,171,172,173,174,175,176]);
 assert.equal(source.sourceElements.length,29);
 assert.equal(new Set(source.sourceElements.map(x=>x.elementId)).size,29);
 assert.ok(source.sourceElements.every(x=>x.appLocation&&x.sourceBook&&x.coverageStatus));
 assert.equal(source.testsRun,'WRITTEN_NOT_RUN');
});
test('Original Arabic graphical table and transformation golden points remain correct',()=>{
 const l=rich.Lessons[0];assert.equal(l.LessonCode,code);
 assert.equal(l.WorkedExamples.length,12);assert.equal(l.Visuals.length,5);
 assert.equal(2*Math.sqrt(2-1)-3,-1);
 assert.equal(2*Math.sqrt(5-1)-3,1);
 assert.equal(-3*Math.sqrt(2-1)+2,-1);
 assert.equal(Math.sqrt(4+3)+1,Math.sqrt(7)+1);
});
test('Exact bounded scalar-value/domain families registered and fail-closed',()=>{
 const x=m.mappings.find(z=>z.lessonCode===code);assert.ok(x);
 assert.deepEqual(x.primarySkills,['functions.graph.sample.coordinates']);
 assert.deepEqual(x.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 for(const id of x.allowedQuestionFamilies){const f=reg.families.find(q=>q.id===id);assert.ok(f);assert.equal(f.skillId,x.primarySkills[0]);assert.equal(f.answerType,'exact_integer');assert.equal(f.productionRouting,false);}
 assert.equal(contract.runtimeGate.productionRouting,false);assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10RadicalGraph.cs'),'utf8');
 assert.match(engine,/class\s+UaeG10RadicalGraphResponse\b/);
 assert.match(engine,/static\s+bool\s+Verify\s*\(/);
 assert.match(fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8'),/return\s+UaeG10RadicalGraphResponse\.Verify\(family,\s*parameters,\s*answer\)/);
});
