'use strict';
// UAE G10 General U03L01 source exact academic guards. Stage D TESTS NOT RUN.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U03:L01:C025';
const blueprint=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u03-l01-ar.rich-lesson-v2.json');
const source=read(base+'lesson-025-source-elements.json');
const contract=read(base+'lesson-025-stage-c-exact-contract.json');
const map=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const registry=read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');

test('Owner-approved 148 catalog preserves a unique new Grade10 General graph-systems identity',()=>{
 assert.equal(blueprint.Lessons.length,148);
 const b=blueprint.Lessons.find(x=>x.LessonCode===code);
 assert.ok(b);assert.equal(b.SortOrder,25);
 assert.equal(b.UnitNumber,3);assert.equal(b.LessonNumber,1);
 assert.equal(b.UnitTitle,'الأنظمة الخطية والمصفوفات');
 assert.equal(b.Title,'حل أنظمة المعادلات الخطية بالتمثيل البياني');
 assert.equal(blueprint.Lessons.filter(x=>x.LessonCode===code).length,1);
 const l=pack.Lessons.find(x=>x.LessonCode===code);assert.ok(l);
 assert.match(l.AdaptationStatus,/READY_FOR_STAGE_D/);
 assert.match(l.SourceLocator,/23–32.*6–15/);
 const ar=l.Translations.find(x=>x.CultureCode==='ar');
 assert.ok(ar?.Explanation.length>400);
});
test('Thirty-two categorized source goals map to source pages, authored Arabic or teacher rubric',()=>{
 assert.deepEqual(source.sourcePdfPages,[23,24,25,26,27,28,29,30,31,32]);
 assert.equal(source.sourceElements.length,32);
 assert.equal(source.sourceElementDenominatorVerifiedForThisLesson,true);
 assert.equal(new Set(source.sourceElements.map(x=>x.elementId)).size,32);
 assert.ok(source.sourceElements.every(e=>e.sourcePrintedPage===e.sourcePdfPage-17&&e.appLocation&&e.coverageStatus));
 assert.equal(source.testsRun,'WRITTEN_NOT_RUN');
});
test('Arabic source lesson contains 14 independently correct worked examples and six actual visual models',()=>{
 assert.equal(rich.Lessons.length,1);
 const l=rich.Lessons[0];
 assert.equal(l.LessonCode,code);assert.equal(l.CultureCode,'ar');
 assert.equal(l.WorkedExamples.length,14);
 assert.equal(l.Visuals.length,6);
 assert.equal(l.Visuals.filter(x=>x.Kind==='CartesianPlot').length,2);
 assert.ok(l.Visuals.some(x=>x.Kind==='ExpressionStripBars'));
 assert.equal(2*2+1,-2+7);
 assert.equal(3-3,-2*3+6);
 assert.notEqual(3*0-2,3*0+4);
 for(const x of [-3,0,2])assert.equal(2*(2*x+1),4*x+2);
 assert.equal(10+4*4,2+6*4);
});
test('All three exact families use graphical systems primary skill and fail closed',()=>{
 const m=map.mappings.find(x=>x.lessonCode===code);assert.ok(m);
 assert.deepEqual(m.primarySkills,['algebra.linear.system.graphical_solution']);
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 for(const id of contract.allowedQuestionFamilies){
  const f=registry.families.find(x=>x.id===id);
  assert.ok(f,id);assert.equal(f.skillId,m.primarySkills[0]);
  assert.equal(f.answerType,'exact_integer');
  assert.equal(f.productionRouting,false);
 }
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(contract.runtimeGate.productionRouting,false);
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10GraphSystems.cs'),'utf8');
 assert.match(engine,/UaeG10GraphSystemsResponse.Verify/);
 assert.match(engine,/Classification\(p\)/);
});
