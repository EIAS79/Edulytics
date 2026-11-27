'use strict';
// Stage A–B–C prepared source regression tests; Stage D will run them.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const j=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U04:L08:C041';
const blueprint=j('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const pack=j('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=j('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u04-l08-ar.rich-lesson-v2.json');
const source=j(base+'lesson-041-source-elements.json');
const contract=j(base+'lesson-041-stage-c-exact-contract.json');
const mappings=j('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const registry=j('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');
test('Owner approved final unit4 lesson is one page Applied lab, not duplicate theoretical chapter',()=>{
 assert.equal(blueprint.Lessons.length,148);
 const l=blueprint.Lessons.find(x=>x.LessonCode===code);
 assert.ok(l);assert.equal(l.UnitNumber,4);assert.equal(l.LessonNumber,8);
 assert.equal(l.Title,'معدل تغير الدالة التربيعية');
 const lesson=pack.Lessons.find(x=>x.LessonCode===code);
 assert.ok(lesson);
 assert.equal(lesson.SourceSha256,'96df18c4fb47e45fc7a98f876d80e00e3cbdc080e5acfbdc9674a25c85f8cb9e');
 assert.match(lesson.SourceLocator,/PDF page31.*printed page214/);
 assert.ok(lesson.Translations.some(x=>x.CultureCode==='ar'));
 assert.match(lesson.AdaptationStatus,/READY_FOR_STAGE_D/);
});
test('Original source exercise categories are individually linked and not falsely doubled',()=>{
 assert.deepEqual(source.sourceLessonPdfPages,[31]);
 assert.deepEqual(source.sourceLessonPrintedPages,[214]);
 assert.equal(source.sourceElements.length,16);
 assert.equal(new Set(source.sourceElements.map(x=>x.elementId)).size,16);
 assert.ok(source.sourceElements.every(x=>x.sourcePhysicalPage===31&&x.sourcePrintedPage===214&&x.appLocation));
 assert.equal(source.secondaryTocEvidence.role,'ANNUAL_TOC_ONLY_NOT_SEPARATE_BODY');
 assert.equal(source.testsRun,'WRITTEN_NOT_RUN');
});
test('Rocket, x-squared and freefall numeric graph data are mathematically independent and valid',()=>{
 const l=rich.Lessons[0];
 assert.equal(l.LessonCode,code);
 assert.equal(l.WorkedExamples.length,15);
 assert.equal(l.Visuals.length,6);
 assert.equal(l.Visuals.filter(x=>x.Kind==='CartesianPlot').length,3);
 const rocket=t=>-16*t*t+144*t;
 assert.deepEqual([0,.5,1,4,4.5,5,9].map(rocket),[0,68,128,320,324,320,0]);
 assert.deepEqual([.5,1,1.5].map(t=>(rocket(t)-rocket(t-.5))/.5),[136,120,104]);
 assert.equal((rocket(4.5)-rocket(4))/.5,8);
 assert.equal((rocket(5)-rocket(4.5))/.5,-8);
 const fall=t=>-16*t*t+100;
 assert.equal(fall(2.5),0);
 assert.equal((fall(.5)-fall(0))/.5,-8);
 assert.equal((fall(1)-fall(.5))/.5,-24);
});
test('Exact half-step rate question families are correctly scoped and fail closed',()=>{
 const m=mappings.mappings.find(x=>x.lessonCode===code);
 assert.ok(m);
 assert.deepEqual(m.primarySkills,['algebra.quadratic.rate_of_change']);
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(contract.runtimeGate.productionRouting,false);
 for(const f of m.allowedQuestionFamilies){
  const row=registry.families.find(x=>x.id===f);
  assert.ok(row,f);assert.equal(row.skillId,m.primarySkills[0]);
  assert.equal(row.productionRouting,false);
  assert.equal(row.lessonPracticeRouting,true);
 }
 const body=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10QuadraticRates.cs'),'utf8');
 assert.match(body,/UaeG10QuadraticRatesResponse/);
 assert.match(body,/a\/2/);
});
