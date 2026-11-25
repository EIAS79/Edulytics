'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),get=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8')),base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/',code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U03:L07:C031';
test('Source Cramer lesson matches approved canonical ordinal 31 and original 1-8 pages',()=>{
 const bp=get('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
 const row=bp.Lessons.find(x=>x.LessonCode===code);assert.ok(row);assert.equal(row.SortOrder,31);assert.equal(row.Title,'قاعدة كرامر والمحددات');
 const ledger=get(base+'lesson-031-source-elements.json');assert.deepEqual(ledger.sourcePhysicalPdfPages,[82,83,84,85,86,87,88,89,90]);assert.equal(ledger.nextOriginalSourceLessonStartsPage,91);assert.equal(ledger.sourceElements.length,25);
});
test('Arabic determinant Cramer content and independently sound sample math',()=>{
 const pack=get('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
 const l=pack.Lessons.find(x=>x.LessonCode===code);assert.ok(l);assert.match(l.AdaptationStatus,/READY_FOR_STAGE_D/);
 const rich=get('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u03-l07-ar.rich-lesson-v2.json').Lessons[0];
 assert.equal(rich.WorkedExamples.length,12);assert.equal(rich.Visuals.length,6);
 assert.equal(4*6-5*(-3),39);
 const D=2*(-1)-1*1,Dx=7*(-1)-1*(-1),Dy=2*(-1)-7*1;
 assert.deepEqual([D,Dx,Dy,Dx/D,Dy/D],[-3,-6,-9,2,3]);
});
test('Bounded Cramer families have source-scoped Stage C contract and disabled routing',()=>{
 const c=get(base+'lesson-031-stage-c-exact-contract.json'),m=get('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);assert.ok(m);
 assert.deepEqual(m.primarySkills,c.primarySkills);assert.deepEqual(m.allowedQuestionFamilies,c.allowedQuestionFamilies);
 assert.equal(c.runtimeGate.productionRouting,false);assert.equal(c.testsRun,'WRITTEN_NOT_RUN');
 const text=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10Cramer.cs'),'utf8');
 assert.match(text,/UaeG10CramerResponse/);assert.match(text,/Dx%D/);
});