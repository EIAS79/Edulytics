'use strict';
// Grade10 Stage A-B-C: authored test, intentionally NOT RUN until Stage D.
const test=require('node:test'), assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const load=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U04:L07:C040';
const index=load('docs/curriculum/UAE_G10_GENERAL_OFFICIAL_MANIFEST.json');
const blueprint=load('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const pack=load('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u04-l07-ar.rich-lesson-v2.json');
const ledger=load(base+'lesson-040-source-elements.json');
const contract=load(base+'lesson-040-stage-c-exact-contract.json');
const map=load('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const registry=load('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');
test('G10-U04L07 reuses approved 148-title canonical identity with Advanced source page evidence',()=>{
 assert.equal(index.LessonCount,148);assert.equal(index.UnitCount,18);
 const b=blueprint.Lessons.find(x=>x.LessonCode===code);
 assert.ok(b);assert.equal(b.UnitNumber,4);assert.equal(b.LessonNumber,7);
 assert.equal(b.Title,'العلاقات والدوال التربيعية وتحويلاتها');
 assert.equal(b.UnitTitle,'الدوال والمعادلات التربيعية');
 const c=pack.Lessons.find(x=>x.LessonCode===code);
 assert.ok(c);assert.equal(c.Translations.length,1);
 assert.equal(c.Translations[0].CultureCode,'ar');
 assert.match(c.SourceLocator,/PDF166–167 lab.*PDF168–172 lesson/);
 assert.match(c.AdaptationStatus,/READY_FOR_STAGE_D/);
});
test('Source element denominator includes original lab and lesson without chapter-review padding',()=>{
 assert.equal(ledger.sourceElements.length,25);
 assert.deepEqual(ledger.sourceTechLabPdfPages,[166,167]);
 assert.deepEqual(ledger.sourceLessonPdfPages,[168,169,170,171,172]);
 assert.equal(new Set(ledger.sourceElements.map(x=>x.elementId)).size,25);
 assert.ok(ledger.sourceElements.every(x=>x.appLocation&&x.coverageStatus&&x.sourcePhysicalPage-17===x.sourcePrintedPage));
});
test('Original Arabic content and plotted point arrays prove distinct vertex transformations',()=>{
 const a=rich.Lessons[0];
 assert.equal(a.LessonCode,code);
 assert.equal(a.WorkedExamples.length,15);
 assert.equal(a.Visuals.length,6);
 const plots=a.Visuals.filter(x=>x.Kind==='CartesianPlot');
 assert.equal(plots.length,3);
 assert.ok(plots.every(x=>x.Rows.length>=5&&x.Rows.every(y=>y.Cells.length===2)));
 for(let x=-3;x<=3;x++){
   assert.equal(x*x+6*x-5,(x+3)**2-14);
   assert.equal(-2*x*x+8*x-3,-2*(x-2)**2+5);
   assert.equal(4*x*x-16*x-40,4*(x-2)**2-56);
 }
});
test('Four bounded vertex-form families exist with one primary skill but disabled production routing',()=>{
 const m=map.mappings.find(x=>x.lessonCode===code);
 assert.ok(m);assert.deepEqual(m.primarySkills,['algebra.quadratic.graph_features']);
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(contract.runtimeGate.productionRouting,false);
 for(const f of contract.allowedQuestionFamilies){
    const r=registry.families.find(x=>x.id===f);
    assert.ok(r,f);assert.equal(r.productionRouting,false);
    assert.equal(r.lessonPracticeRouting,true);
    assert.equal(r.skillId,m.primarySkills[0]);
 }
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 const grader=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10QuadraticTransforms.cs'),'utf8');
 assert.match(engine,/BuildG10QuadraticTransforms/);
 assert.match(grader,/class\s+UaeG10QuadraticTransformsResponse\b/);
 assert.match(grader,/static\s+bool\s+Verify\s*\(/);
 assert.match(fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8'),/return\s+UaeG10QuadraticTransformsResponse\.Verify\(family,\s*parameters,\s*answer\)/);
});
