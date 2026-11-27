'use strict';
// Stage A-B-C authored; DO NOT EXECUTE before Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:02:04:SCIENTIFIC-NOTATION';
const bp=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u02-l04-ar.rich-lesson-v2.json');
const source=read(base+'lesson-020-source-elements.json'),contract=read(base+'lesson-020-stage-c-exact-contract.json');
const maps=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const family=read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');
test('Grade10 ordinal20 retained in canonical approved 148/18 and authored Arabic content',()=>{
 assert.equal(bp.Lessons.length,148);assert.equal(bp.Units.length,18);
 const b=bp.Lessons.find(x=>x.LessonCode===code);assert.ok(b);
 assert.equal(b.SortOrder,20);assert.equal(b.UnitNumber,2);assert.equal(b.LessonNumber,4);
 assert.equal(b.Title,'الترميز العلمي');
 const c=pack.Lessons.find(x=>x.LessonCode===code);assert.ok(c);
 assert.match(c.AdaptationStatus,/READY_FOR_STAGE_D/);
 assert.match(c.SourceLocator,/121–127.*108–114/);
 assert.equal(c.SourceSha256,'d7e492a960675c30ac7eebb0144ed2dc51d1f4e9c91c9834ea05638f614a3c72');
 assert.ok(c.Translations.find(x=>x.CultureCode==='ar')?.Explanation.length>300);
});
test('Arabic scientific notation examples and diagrams are original and correct',()=>{
 const l=rich.Lessons[0];assert.equal(l.LessonCode,code);
 assert.equal(l.WorkedExamples.length,13);assert.equal(l.Visuals.length,5);
 assert.equal(7.25e6,7250000);
 assert.ok(Math.abs(8.3e-5-0.000083)<1e-15);
 assert.equal(8e-2*5e3,400);
 assert.ok(Math.abs(2.4e3/6e5-0.004)<1e-12);
 assert.ok(l.WorkedExamples.some(x=>x.Answer.includes('٧٫٢٥×١٠⁶')));
});
test('Source lesson has 24 categorized goals across 7 scanned pages',()=>{
 assert.deepEqual(source.sourcePdfPages,[121,122,123,124,125,126,127]);
 assert.equal(source.sourceElements.length,24);
 assert.equal(new Set(source.sourceElements.map(x=>x.elementId)).size,24);
 assert.ok(source.sourceElements.every(x=>x.appLocation&&x.sourcePrintedPage===x.sourcePhysicalPage-13));
 assert.equal(source.testsRun,'WRITTEN_NOT_RUN');
});
test('Bounded independent scientific exponent contracts are source-matched and fail closed',()=>{
 const m=maps.mappings.find(x=>x.lessonCode===code);assert.ok(m);
 assert.deepEqual(m.primarySkills,['supporting.indices.roots_standard_form']);
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 for(const id of m.allowedQuestionFamilies){const r=family.families.find(x=>x.id===id);assert.ok(r,id);assert.equal(r.skillId,m.primarySkills[0]);assert.equal(r.answerType,'exact_integer');assert.equal(r.productionRouting,false);}
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(contract.runtimeGate.productionRouting,false);
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10ScientificNotation.cs'),'utf8');
 assert.match(engine,/class\s+UaeG10ScientificExponentResponse\b/);
 assert.match(engine,/static\s+bool\s+Verify\s*\(/);
 assert.match(fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8'),/return\s+UaeG10ScientificExponentResponse\.Verify\(family,\s*parameters,\s*answer\)/);
});

test('Stage D advanced cross-target original astronomy context has exact units and bounded teacher route',()=>{const l=rich.Lessons[0],e=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G10/lesson-001-stage-d-advanced-subparts-and-routes.json'),p=pack.Lessons.find(x=>x.LessonCode===code),a=p.Translations.find(t=>t.CultureCode==='ar');assert.ok(l.WorkedExamples.some(x=>x.Title==='تطبيق فلكي: زمن انتقال الضوء بين مسافتين'));assert.ok(a.WorkedExamples.includes('تطبيق فلكي: زمن انتقال الضوء بين مسافتين'));assert.equal(4.5e16/3e8,1.5e8);assert.equal(7.2e11/3e8,2400);assert.equal(2400/60,40);assert.ok(Math.abs(1.5e8/(365*24*3600)-4.7564687976)<1e-8);assert.equal(e.rows.length,9);assert.equal(e.numberedExerciseSlotsCountingExplicitSubparts,74);assert.equal(e.semanticParity,'UNKNOWN');assert.equal(e.productionRouting,false);assert.equal(contract.runtimeGate.productionRouting,false);});
